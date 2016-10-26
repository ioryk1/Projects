using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceForEFGP.Models.Dao;
using WebServiceForEFGP.Models.VeiwModel;
using System.Xml.Serialization;
using System.IO;
using System.Web.Configuration;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.Util;

namespace WebServiceForEFGP.Models.Services {

    /// <summary>
    /// 訊息資源申請單
    /// </summary>
    public class MSPSvc {

        public MSPSvc() {
            this.mspDao = new MSPDao();
            this.msp_host = WebConfigurationManager.AppSettings["msp_host"];
            
        }

        private MSPDao mspDao = null;

        private string msp_host = "";

        public bool MSPSubmit(string processSerialNumber) {
            bool ret = false;
            MSPViewModel.MSPSubmitResponse res = new MSPViewModel.MSPSubmitResponse();
            string url = this.msp_host;

            MSG001 msg =  this.mspDao.getMSGData(processSerialNumber);

            if (msg == null) return ret;

            List<MSG001_gridMsg> msg_grid_list = this.mspDao.getMSGGridList(msg.formSerialNumber);

            if (msg_grid_list.Count == 0) {                
                return ret;
            } 

            //一筆一筆 post
            List<MSPViewModel.MSPSubmitRequestParameter> request_list = this.getRequestListParamter(msg, msg_grid_list);
            foreach (var r in request_list) {
                string post_str = this.getPostMSPString(r);
                UtilitySvc.writeLog(post_str);
                string response_str = UtilitySvc.formPostRequest(url, post_str, new Dictionary<string, object>());
                UtilitySvc.writeLog(response_str);

            }

            if (request_list.Count > 0) {
                ret = true;
            }

            return ret;
        }


        /// <summary>
        /// 匯出訊息列表(Excel 格式)
        /// </summary>
        /// <returns></returns>
        public IWorkbook exportMessageWrokbook(IWorkbook wb, HttpContext context) {
            IWorkbook ret = null;

            ISheet sheet = wb.GetSheetAt(0);

            //設定下拉選單功能                
            sheet = this.setCellExplicitList(sheet);

            //自行產生的 hidden field
            string export_grid_json_string = context.Request.Form["hidGdExportMsgList"].ToString();               //grid view     hidGdExportMsgList            
            JArray gridArr = JArray.Parse(export_grid_json_string);

            int start_row_index = 3;            
            foreach (JArray arrItem in gridArr) {
                int i = 0;
                string sort = arrItem[i++].ToString(); //序號
                string title = arrItem[i++].ToString(); //訊息主旨
                string content = this.decodeExportStr(arrItem[i++].ToString()); //訊息內文
                string memo = this.decodeExportStr(arrItem[i++].ToString());    //備註
                string type = arrItem[i++].ToString(); //名單類型
                string src = arrItem[i++].ToString(); //名單來源
                string list_count = arrItem[i++].ToString();//名單數量
                string condition = arrItem[i++].ToString();  //名單條件
                string list = arrItem[i++].ToString();//試發門號
                string send_type = arrItem[i++].ToString(); //發送種類
                string date_send = arrItem[i++].ToString(); //發送日期
                string time_send = arrItem[i++].ToString(); //發送時間
                string segment = arrItem[i++].ToString();     //申請部門
                string sales = arrItem[i++].ToString();   //申請業務
                string point_region = arrItem[i++].ToString();   //發送區域
                string point_address = arrItem[i++].ToString();  //店點地址
                string point_id = arrItem[i++].ToString();   //營業代碼
                string point_name = arrItem[i++].ToString();  //營業點名稱              

                IRow current_row = sheet.GetRow(start_row_index++);
                
                //設定表頭資訊
                int currentCellIndex = 0;
                current_row = this.setCellValueFromHeaderInfo(current_row, context, currentCellIndex, sort);
                //設定列表資訊
                currentCellIndex = 10;
                current_row = this.setCellValueFromGridInfo(current_row, context, currentCellIndex,
                     title, content,
                    memo, type, src, list_count, condition, list, send_type, date_send, time_send, segment, sales, point_region, point_address,
                    point_id, point_name);

            }
            sheet.ForceFormulaRecalculation = false;
            ret = wb;
            return ret;
        }

        /// <summary>
        /// 設定匯出下拉選單功能
        /// </summary>        
        private ISheet setCellExplicitList(ISheet sheet) {

            //發送系統
            DVConstraint sendSystemConstraint = DVConstraint.CreateExplicitListConstraint(MSPViewModel.export_ddl_sendSystem_arr);
            CellRangeAddressList sendSystemRegion = new CellRangeAddressList(3, 655365, 5, 5);
            HSSFDataValidation sendSystemDataValidate = new HSSFDataValidation(sendSystemRegion, sendSystemConstraint);
            //發送等級
            DVConstraint sendLevelConstraint = DVConstraint.CreateExplicitListConstraint(MSPViewModel.export_ddl_sendLevel_arr);
            CellRangeAddressList sendLevelRegion = new CellRangeAddressList(3, 655365, 6, 6);
            HSSFDataValidation sendLevelDataValidate = new HSSFDataValidation(sendLevelRegion, sendLevelConstraint);
            //訊息分類
            DVConstraint messageCategoryConstraint = DVConstraint.CreateExplicitListConstraint(MSPViewModel.export_ddl_msgCategory_arr);
            CellRangeAddressList messageCategoryRegion = new CellRangeAddressList(3, 655365, 7, 7);
            HSSFDataValidation messageCategoryDataValidate = new HSSFDataValidation(messageCategoryRegion, messageCategoryConstraint);
            //名單類型
            DVConstraint listTypeConstraint = DVConstraint.CreateExplicitListConstraint(MSPViewModel.export_ddl_messageType_arr);
            CellRangeAddressList listTypeRegion = new CellRangeAddressList(3, 655365, 10, 10);
            HSSFDataValidation listTypeDataValidate = new HSSFDataValidation(listTypeRegion, listTypeConstraint);
            //來源名單
            DVConstraint listSourceConstraint = DVConstraint.CreateExplicitListConstraint(MSPViewModel.export_ddl_listSource_arr);
            CellRangeAddressList listSourceRegion = new CellRangeAddressList(3, 655365, 23, 23);
            HSSFDataValidation listSourceDataValidate = new HSSFDataValidation(listSourceRegion, listSourceConstraint);
            //發送種類
            DVConstraint sendTypeConstraint = DVConstraint.CreateExplicitListConstraint(MSPViewModel.export_ddl_sendType_arr);
            CellRangeAddressList sendTypeRegion = new CellRangeAddressList(3, 655365, 25, 25);
            HSSFDataValidation sendTypeDataValidate = new HSSFDataValidation(sendTypeRegion, sendTypeConstraint);


            sheet.AddValidationData(sendSystemDataValidate);
            sheet.AddValidationData(sendLevelDataValidate);
            sheet.AddValidationData(messageCategoryDataValidate);
            sheet.AddValidationData(listTypeDataValidate);
            sheet.AddValidationData(listSourceDataValidate);
            sheet.AddValidationData(sendTypeDataValidate);


            return sheet;
        }


        /// <summary>
        /// 設定匯出表頭資料
        /// </summary>        
        private IRow setCellValueFromHeaderInfo(IRow current_row , HttpContext context , int currentCellIndex   , string sort ) {

            string sendSystemEnum = context.Request.Form["hidSendSystemEnum"] as string ?? string.Empty;
            string sendLevelEnum = context.Request.Form["hidSendLevelEnum"] as string ?? string.Empty;
            string msgCategory_type = context.Request.Form["hidMsgCategory_type"] as string ?? string.Empty;
            string msgCategory_msidn = context.Request.Form["hidMsgCategory_msidn"] as string ?? string.Empty;

            if (!string.IsNullOrEmpty( msgCategory_type)) {
                msgCategory_type = msgCategory_type.Substring(0, 1).ToUpper();
            }

            string applyName = context.Request.Form["txtApplyName"] as string ?? string.Empty;
            string applyExtNo = context.Request.Form["txtApplyExtNo"] as string  ?? string.Empty ;
            string applyDivsion = context.Request.Form["hidApplyDivsion"] as string ?? string.Empty;
            string applyDepname = context.Request.Form["txtApplyDepName"] as string ?? string.Empty;

            current_row.GetCell(currentCellIndex++).SetCellValue(sort);    //項目
            current_row.CreateCell(currentCellIndex++).SetCellValue(applyName);    //申請人
            current_row.CreateCell(currentCellIndex++).SetCellValue(applyExtNo);    //分機
            current_row.CreateCell(currentCellIndex++).SetCellValue(applyDivsion);    //處
            current_row.CreateCell(currentCellIndex++).SetCellValue(applyDepname);    //部
            current_row.CreateCell(currentCellIndex++).SetCellValue(MSPViewModel.sendSystemDic[sendSystemEnum]);    //發送系統
            current_row.CreateCell(currentCellIndex++).SetCellValue(MSPViewModel.sendLevelDic[sendLevelEnum]);    //發送等級
            current_row.CreateCell(currentCellIndex++).SetCellValue(MSPViewModel.msgCategoryDic[msgCategory_type]);    //訊息分類
            current_row.CreateCell(currentCellIndex++).SetCellValue(msgCategory_msidn);    //發送代表號

            //排除條件(依序加序號)
            string excludeConditionNameList_str = context.Request.Form["hidExcludeConditionNameList"] as string;
            List<string> excludeConditionNameList = JsonConvert.DeserializeObject<List<string>>(excludeConditionNameList_str);
            List<string> excludeConditionNameList_display = new List<string>();
            int list_sort = 1;
            foreach (string item in excludeConditionNameList) {
                excludeConditionNameList_display.Add((list_sort++).ToString() + "." + item);
            }

            current_row.CreateCell(currentCellIndex++).SetCellValue(string.Join("\n", excludeConditionNameList_display.ToArray()));    //排除條件                                 
            current_row.GetCell(--currentCellIndex).CellStyle.WrapText = true;

            return current_row;
        }


        /// <summary>
        /// 設定匯出列表資料
        /// </summary>        
        private IRow setCellValueFromGridInfo(IRow current_row, HttpContext context,  int currentCellIndex ,
            string title , string content, string memo , string type,string src,
             string list_count, string condition, string list , string send_type , string date_send , string time_send,
             string segment , string sales , string point_region , string point_address, string point_id, string point_name) {

            

            //set row
            if (MSPViewModel.listTypeDic.ContainsKey(type)) {
                current_row.CreateCell(currentCellIndex++).SetCellValue(MSPViewModel.listTypeDic[type]);    //名單類型
                current_row.CreateCell(currentCellIndex++).SetCellValue(string.Empty);    //其他名單備註
            } else {
                current_row.CreateCell(currentCellIndex++).SetCellValue(MSPViewModel.listTypeDic["{other}"]);    //名單類型
                current_row.CreateCell(currentCellIndex++).SetCellValue(type);    //其他名單備註
            }

            string yearMonth = string.Empty;
            string sendDateStart = string.Empty;
            string sendTimeStart = string.Empty;
            string sendDateEnd = string.Empty;
            string sendTimeEnd = string.Empty;

            
            if (date_send.IndexOf("~") > -1) {
                //週期性發送
                string[] sendDateArr = date_send.Split('~');
                sendDateStart = sendDateArr[0].Trim();
                sendDateEnd = sendDateArr[1].Trim();              
            } else {
                //一次性發送
                sendDateStart = date_send.Trim();
            }

            //週期性發送
            if (time_send.IndexOf("~") > -1) {
                string[] sendTimeArr = time_send.Split('~');
                sendTimeStart = sendTimeArr[0].Trim();
                sendTimeEnd = sendTimeArr[1].Trim();
            } else {
                //一次性發送
                sendTimeStart = time_send.Trim();
            }

            current_row.CreateCell(currentCellIndex++).SetCellValue(yearMonth);    //年/月
            current_row.CreateCell(currentCellIndex++).SetCellValue(sendDateStart);    //發送日期(起)
            current_row.CreateCell(currentCellIndex++).SetCellValue(sendTimeStart);    //發送時間(起)
            current_row.CreateCell(currentCellIndex++).SetCellValue(sendDateEnd);    //發送日期(迄)
            current_row.CreateCell(currentCellIndex++).SetCellValue(sendTimeEnd);    //發送時間(迄)
            current_row.CreateCell(currentCellIndex++).SetCellValue(title);    //訊息主旨
            current_row.CreateCell(currentCellIndex++).SetCellValue(content);    //訊息內文
            current_row.CreateCell(currentCellIndex++).SetCellFormula(string.Format("ROUNDUP(LEN(S{0})/70,0)", (current_row.RowNum + 1).ToString())); //簡訊則數
            
            current_row.CreateCell(currentCellIndex++).SetCellValue(list_count);    //預估名單數
            current_row.CreateCell(currentCellIndex++).SetCellValue(condition);    //名單條件
            current_row.CreateCell(currentCellIndex++).SetCellValue(memo);    //備註事項
            current_row.CreateCell(currentCellIndex++).SetCellValue(src);    //名單來源
            current_row.CreateCell(currentCellIndex++).SetCellValue(list);    //試發門號
            current_row.CreateCell(currentCellIndex++).SetCellValue(send_type);    //發送種類
            current_row.CreateCell(currentCellIndex++).SetCellValue(string.Empty);    //實際發送名單筆數(A1)
            current_row.CreateCell(currentCellIndex++).SetCellValue(string.Empty);    //名單筆數(濾拒收)
            current_row.CreateCell(currentCellIndex++).SetCellValue(string.Empty);    //拒收扣除率          
            current_row.CreateCell(currentCellIndex++).SetCellValue(segment);    //申請部門
            current_row.CreateCell(currentCellIndex++).SetCellValue(sales);    //申請業務
            current_row.CreateCell(currentCellIndex++).SetCellValue(point_region);    //發送區域
            current_row.CreateCell(currentCellIndex++).SetCellValue(point_address);    //申請地址
            current_row.CreateCell(currentCellIndex++).SetCellValue(point_id);    //營業代碼
            current_row.CreateCell(currentCellIndex++).SetCellValue(point_name);    //營業點名稱
            return current_row;
        }

        private string getPostMSPString(MSPViewModel.MSPSubmitRequestParameter param) {
            string ret = "";
            StringBuilder sb = new StringBuilder();

            MSPViewModel.BlockListRequestParameter t_b = new MSPViewModel.BlockListRequestParameter();

            Type cl = param.GetType();
            Type cl_block = t_b.GetType();
            

            sb.Append("<EFSSubmitReq>");
            foreach (var p in cl.GetProperties()) {

                //陣列例外處理
                if (p.Name == "BlockList") {
                    sb.Append("<" + p.Name + ">");
                    foreach (var bl in param.BlockList) {                        
                        foreach (var b in cl_block.GetProperties()) {
                            sb.Append("<" + b.Name + ">");
                            sb.Append(b.GetValue(bl));
                            sb.Append("</" + b.Name + ">");
                        }
                    }
                    sb.Append("</" + p.Name + ">");
                } else {
                    sb.Append("<" + p.Name + ">");
                    sb.Append(p.GetValue(param));
                    sb.Append("</" + p.Name + ">");
                }                
            }
            sb.Append("</EFSSubmitReq>");

            ret = "xml=" + sb.ToString();
            return ret;
        }

        private List< MSPViewModel.MSPSubmitRequestParameter> getRequestListParamter(MSG001 msg, List<MSG001_gridMsg> msg_gird_list) {
            List<MSPViewModel.MSPSubmitRequestParameter> ret = new List<MSPViewModel.MSPSubmitRequestParameter>();

            string[] ChbExcludeConditionArr = msg.hidChbExcludeCondition.Split(';');

            List<MSPViewModel.BlockListRequestParameter> blockList = new List<MSPViewModel.BlockListRequestParameter>();

            //排除名單

            foreach (var b in ChbExcludeConditionArr) {
                int _b = 0;

                if (!int.TryParse(b, out _b)) continue;

                blockList.Add(new MSPViewModel.BlockListRequestParameter() {

                    Oid = int.Parse(b)
                });
            }

            //測試名單
            string testDistArr_str = "";
            List<string> testDistArr = new List<string>();

            if (!string.IsNullOrEmpty(msg.hidApprovedEmp)) {
                foreach (var s in msg.hidApprovedEmp.Split(';')) {
                    if (s.Length > 0) {
                        testDistArr.Add(s);
                    }
                }
                testDistArr_str = string.Join(",", testDistArr.ToArray());
            }



            int sort = 1;
            foreach (var m in msg_gird_list) {

                string SendStartDate = "";
                string SendEndDate = "";
                string SendStartTime = "";
                string SendEndTime = "";               

                //一次性
                if (m.gd_send_type == "一次性發送") {
                    DateTime sendStartDateTime = DateTime.Parse(m.gd_date_send.Trim() + " " + m.gd_time_send.Trim());
                    SendStartDate = sendStartDateTime.ToString("yyyyMMdd");
                    SendStartTime = sendStartDateTime.ToString("HHmm");
                } else if (m.gd_send_type == "週期性發送") {
                    string[] str_date = m.gd_date_send.Split('~');
                    string gd_start_date =  str_date[0].Trim();
                    string gd_end_date = str_date[1].Trim();

                    string[] str_time = m.gd_time_send.Split('~');
                    string gd_start_time = str_time[0].Trim();
                    string gd_end_time = str_time[1].Trim();

                    DateTime sendStartDateTime = DateTime.Parse(gd_start_date + " " + gd_start_time);
                    DateTime sendEndDateTime = DateTime.Parse(gd_end_date + " " + gd_end_time);

                    SendStartDate = sendStartDateTime.ToString("yyyyMMdd");
                    SendStartTime = sendStartDateTime.ToString("HHmm");
                    SendEndDate = sendEndDateTime.ToString("yyyyMMdd");
                    SendEndTime = sendEndDateTime.ToString("HHmm");
                }

                //換行 handler
                m.gd_content  = m.gd_content.Replace("<br />", Environment.NewLine).Replace("&lt;br /&gt;", Environment.NewLine);
                m.gd_memo = m.gd_memo.Replace("<br />", Environment.NewLine).Replace("&lt;br /&gt;", Environment.NewLine);


                MSPViewModel.MSPSubmitRequestParameter req = new MSPViewModel.MSPSubmitRequestParameter() {
                    FormId = msg.SerialNumber + "-" + sort.ToString("000"), //序號  +  1 + 2 + 3
                    Department = UtilitySvc.base64Encode(msg.txtApplyDepName),
                    Division = UtilitySvc.base64Encode(msg.hidApplyDivsion),
                    Username = UtilitySvc.base64Encode(msg.txtApplyName),
                    Ext = msg.txtApplyExtNo,
                    PushSystem = int.Parse(msg.ddlSendSystem),
                    SystemNote = UtilitySvc.base64Encode(msg.hidITDelegateOption),
                    TaskType = int.Parse(msg.hidRadioSendLevel),
                    CpDelegated = int.Parse(msg.hidCPDelegate),
                    CategoryOid = int.Parse(msg.ddlMsgCategory),
                    EmployeeId = msg.txtApplyId,
                    PhoneNumber = msg.hidSendPhoneNumber,
                    Subject = UtilitySvc.base64Encode(UtilitySvc.parseGridValue(m.gd_title)),
                    Content = UtilitySvc.base64Encode(UtilitySvc.parseGridValue(m.gd_content)),
                    Note = UtilitySvc.base64Encode(UtilitySvc.parseGridValue(m.gd_memo)),
                    DistType = UtilitySvc.base64Encode(m.gd_type),
                    DistFrom = UtilitySvc.base64Encode(m.gd_src),
                    DistSize = int.Parse(m.gd_list_count),
                    DistRule = UtilitySvc.base64Encode(UtilitySvc.parseGridValue(m.gd_condition)),
                    RefDistList = UtilitySvc.base64Encode(m.gd_list),               //測試名單
                    PushType = int.Parse(m.gd_send_type == "一次性發送" ? "0" : "1"),
                    SendStartDate = SendStartDate,
                    SendEndDate = SendEndDate,
                    SendStartTime = SendStartTime,
                    SendEndTime = SendEndTime,
                    OtherDepartment = UtilitySvc.base64Encode(m.gd_segment),
                    ApplySales = UtilitySvc.base64Encode(m.gd_sales),
                    SendArea = UtilitySvc.base64Encode(m.gd_point_region),
                    Code = m.gd_point_id,
                    StoreName = UtilitySvc.base64Encode(m.gd_point_name),
                    TestDist = testDistArr_str,                 //若為 一般件 要將簽核過的人員工編號放進至試跑名單中
                    Address = UtilitySvc.base64Encode(m.gd_point_address),
                    ApplyDate = DateTime.Now.ToString("yyyyMMdd"),
                    BlockList = blockList
                };


                ret.Add(req);
                sort++;
            }

            return ret; 
        }


        private string decodeExportStr(string str) {
            string ret = str.Replace("<br />", Environment.NewLine).Replace("&lt;br /&gt;", Environment.NewLine)
                .Replace("<#leftQuot>", "[").Replace("<#rightQuot>", "]").Replace("<#quot>", "'");


            return ret;
        }

    }
}

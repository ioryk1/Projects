using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceForEFGP.Models.VeiwModel {
    public class MSPViewModel {

        public class MSPSubmitRequestParameter {
            public string FormId { get; set; }
            public string Department { get; set; }
            public string Division { get; set; }
            public string Username { get; set; }
            public string Ext { get; set; }
            public string ApplyDate { get; set; }
            public int PushSystem { get; set; }
            public string SystemNote { get; set; }
            public int TaskType { get; set; }
            public int CpDelegated { get; set; }
            public int CategoryOid { get; set; }
            public List<BlockListRequestParameter> BlockList { get; set; }
            public string EmployeeId { get; set; }
            public string PhoneNumber { get; set; }
            public string Subject { get; set; }
            public string Content { get; set; }
            public string Note { get; set; }
            public string DistType { get; set; }
            public string DistFrom { get; set; }
            public int DistSize { get; set; }
            public string DistRule { get; set; }
            public string RefDistList { get; set; }
            public int PushType { get; set; }
            public string SendStartDate { get; set; }
            public string SendEndDate { get; set; }
            public string SendStartTime { get; set; }
            public string SendEndTime { get; set; }
            public string OtherDepartment { get; set; }
            public string ApplySales { get; set; }
            public string SendArea { get; set; }
            public string Address { get; set; }
            public string Code { get; set; }
            public string StoreName { get; set; }
            public string TestDist { get; set; }




            public MSPSubmitRequestParameter() {
                this.FormId = "";
                this.Department = "";
                this.Division = "";
                this.Username = "";
                this.Ext = "";
                this.ApplyDate = "";
                this.PushSystem = -1;
                this.SystemNote = "";
                this.TaskType = -1;
                this.CpDelegated = -1;
                this.CategoryOid = -1;
                this.EmployeeId = "";
                this.PhoneNumber = "";
                this.Subject = "";
                this.Content = "";
                this.Note = "";
                this.DistType = "";
                this.BlockList = new List<BlockListRequestParameter>();
                this.DistFrom = "";
                this.DistSize = 0;
                this.DistRule = "";
                this.RefDistList = "";
                this.PushType = -1;
                this.SendStartDate = "";
                this.SendStartTime = "";
                this.SendEndDate = "";
                this.SendEndTime = "";
                this.OtherDepartment = "";
                this.ApplySales = "";
                this.SendArea = "";
                this.Address = "";
                this.Code = "";
                this.StoreName = "";
                this.TestDist = "";
            }
        }

        public class BlockListRequestParameter {
            public int Oid { get; set; }

            public BlockListRequestParameter() {
                this.Oid = 0;
            }


        }

        public class MSPSubmitResponse {
            public string FormId { get; set; }
            public string ResultCode { get; set; }
            public string ResultText { get; set; }

            public MSPSubmitResponse() {
                this.FormId = "";
                this.ResultCode = "";
                this.ResultText = "";                    
            }

        }


        /// <summary>
        /// 發送系統
        /// </summary>
        public static string[] export_ddl_sendSystem_arr = new string[] {
            "1.MSP系統-網內簡訊",
            "2.MSP系統-IT代發", 
            "3.三竹系統(網外)",
            "4.外勞預付卡" };
        /// <summary>
        /// 發送等級
        /// </summary>
        public static string[] export_ddl_sendLevel_arr = new string[] {
            "1.一般件" ,
            "2.急件(※注意：此流程為至行銷窗口簽核完成後立即發出)",
            "3.特急件(※注意：此流程為申請者開單後立即發出)" };

        /// <summary>
        /// 訊息分類
        /// </summary>
        public static string[] export_ddl_msgCategory_arr = new string[] {
            "A類:必發權益",
            "B類:推廣權益",
            "C類:促銷訊息" };
        /// <summary>
        /// 發送族群
        /// </summary>
        public static string[] export_ddl_messageType_arr = new string[] { "月租用戶", "預付用戶", "其他，請備註" };
        /// <summary>
        /// 名單來源
        /// </summary>
        public static string[] export_ddl_listSource_arr = new string[] { "BO名單", "自行上傳名單" };
        /// <summary>
        /// 發送種類
        /// </summary>
        public static string[] export_ddl_sendType_arr = new string[] { "一次性發送", "週期性發送" };




        public static Dictionary<string, string> sendSystemDic = new Dictionary<string, string>() {
                { "MSP系統 - 網內簡訊","1.MSP系統-網內簡訊" },
                { "MSP系統 - IT 代發","2.MSP系統-IT代發" },
                { "三竹系統(網外)","3.三竹系統(網外)" },
                { "外勞預付卡","4.外勞預付卡" }
            };

        public static Dictionary<string, string> sendLevelDic = new Dictionary<string, string>() {
                {"一般件", "1.一般件" } ,
                {"急件",  "2.急件(※注意：此流程為至行銷窗口簽核完成後立即發出)" } ,
                {"特急件", "3.特急件(※注意：此流程為申請者開單後立即發出)" }
            };

        public static Dictionary<string, string> msgCategoryDic = new Dictionary<string, string>() {
                {"A","A類:必發權益" } , {"B","B類:推廣權益" } , {"C","C類:促銷訊息" }  , {  "" ,""}
            };

        public static Dictionary<string, string> listTypeDic = new Dictionary<string, string>() {
            {"月租用戶","月租用戶" }  , {"預付用戶","預付用戶" } , {"{other}","其他，請備註" }
        };
    



    }
}

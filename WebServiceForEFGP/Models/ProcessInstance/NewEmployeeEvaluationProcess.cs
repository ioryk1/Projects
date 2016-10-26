using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceForEFGP.Models.VeiwModel;

using WebServiceForEFGP.Models.VeiwModel.NaNaViewModel;
using WebServiceForEFGP.Models.Services.NaNaWebService;
using WebServiceForEFGP.Models.Services;
using WebServiceForEFGP.Models.Dao.ProcessInstanceDao;
using WebServiceForEFGP.Models.VeiwModel.ProcessInstanceViewModel;
using System.Web.Configuration;
using System.Xml;

namespace WebServiceForEFGP.Models.ProcessInstance {
    /// <summary>
    /// 新進員工試用期評鑑表
    /// </summary>
    public class NewEmployeeEvaluationProcess : IProcessAction {

        private NewEmployeeEvaluationViewModel.CreateNewEmployeeEvaluationProcessParameter createParam = null;

        private NaNaFormWebSvc nanaFormSvc = null;
        private NaNaProcessWebSvc nanaProcSvc = null;
        private EmployeeSvc empSvc = null;

        private NewEmployeeEvaluationDao newEmpEvaDao = null;

        private string ProcessID = string.Empty;

        private List<string> issuerEmpInfoList = new List<string>();
        private List<string> evaEmpInfoList = new List<string>();
        private List<string> hcpEvaEmpInfoList = new List<string>();


        public NewEmployeeEvaluationProcess(NewEmployeeEvaluationViewModel.CreateNewEmployeeEvaluationProcessParameter createParam) {
            this.createParam = createParam;
            this.nanaFormSvc = new NaNaFormWebSvc();
            this.nanaProcSvc = new NaNaProcessWebSvc();
            this.empSvc = new EmployeeSvc();

            this.newEmpEvaDao = new NewEmployeeEvaluationDao();

            this.ProcessID = WebConfigurationManager.AppSettings["NewEmployeeEvaluationProcess"];

        }

        /// <summary>
        /// 新增一流程
        /// </summary>
        /// <returns></returns>
        public CommonViewModel.Result create() {
            CommonViewModel.Result ret = new CommonViewModel.Result();

            //validate
            ret = this.validateCreateProcessParam();
            if (!ret.success) {
                return ret;
            }

            //get xml template
            NaNaFormViewModel.FormTemplateSimpleResult formOIDRet = this.nanaFormSvc.findFormOIDsOfProcess(this.ProcessID);
            if (!formOIDRet.success) {
                ret.resultMessage = "form Instance not found";
                ret.resultCode = "301";
                return ret;
            }
            string[] formArr = formOIDRet.formOID.Split(',');

            NaNaFormViewModel.FormTemplateSimpleResult formTemplateRet = this.nanaFormSvc.getFormFieldTemplate(formArr[0]);

            if (!formTemplateRet.success) {
                ret.resultMessage = "form Template not found";
                ret.resultCode = "301";
                return ret;
            }
            
            //parse xml document
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(formTemplateRet.xmlTemplate);


            //set post  data
            string FormID = "HR_NewEmployee_1";

            //申請日期
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/txtDate", DateTime.Now.ToString("yyyy/MM/dd"));

            //其他初始值
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/txtCount", "0");
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/txtAdvice", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/hfCheckExtendMonth", "0");           


            //取得填單人的資料
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/txtUserInfo_Dep1", this.issuerEmpInfoList[1]);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/txtUserInfo_Dep2", this.issuerEmpInfoList[2]);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/txtUserInfo_Id", this.issuerEmpInfoList[3]);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/txtUserInfo_Name", this.issuerEmpInfoList[4]);                        

            //取得受評人的EP組織資料(部門名稱、ID...)
            
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/hfUpUpDepName", this.evaEmpInfoList[0]);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/txtDep1", this.evaEmpInfoList[1]);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/txtDep2", this.evaEmpInfoList[2]);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/txtUserID", this.evaEmpInfoList[3]);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/txtUserName", this.evaEmpInfoList[4]);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/txtDep2Id", this.evaEmpInfoList[5]);

            //取得受評人的HCP資料(到職日、職等...)            
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/txtTitle", this.hcpEvaEmpInfoList[0]);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/txtTrial", this.hcpEvaEmpInfoList[1]);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/hfLevel", this.hcpEvaEmpInfoList[2]);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/hfMaturityDate1", this.hcpEvaEmpInfoList[3]);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/hfMaturityDate2", this.hcpEvaEmpInfoList[4]);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/hfOkayDate", this.hcpEvaEmpInfoList[5]);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/hfActTrialDate", this.hcpEvaEmpInfoList[6]);

            //取得填單人的核決層級
            string issuerDicisionLevel = this.newEmpEvaDao.getDecisionLevelByEmpId(this.createParam.issuerId);
            if (string.IsNullOrEmpty(issuerDicisionLevel)) {
                UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/hfApproveLvl", "6000");
            } else {
                UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/hfApproveLvl", issuerDicisionLevel);
            }

            //取得受評人的核決層級
            string evaEmpDicisionLevel = this.newEmpEvaDao.getDecisionLevelByEmpId(this.createParam.issuerId);
            if (string.IsNullOrEmpty(evaEmpDicisionLevel)) {
                UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/hfApproveLvl1", "6000");
            } else {
                UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/hfApproveLvl1", evaEmpDicisionLevel);
            }


            //取得部門相關資訊
            List<string> evaEmpDetpInfo = this.newEmpEvaDao.getDeptInfoByDeptName(evaEmpInfoList[2]);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/hfManagerName", evaEmpDetpInfo[0]);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/hfManagerId", evaEmpDetpInfo[1]);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/hfManagerNameUp", evaEmpDetpInfo[2]);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/hfManagerIdUp", evaEmpDetpInfo[3]);


            //設定最後發起流程的資訊
            List<string> evaEmpProcessInfo = this.newEmpEvaDao.getLastEvaluationProcessByEmpId(this.createParam.evaluationEmpId);
            UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/rbtAppraisal", "0");
            if (evaEmpProcessInfo.Count > 0) {
                int ddl2rdItem = 0;
                int.TryParse(evaEmpProcessInfo[0], out ddl2rdItem);
                if (ddl2rdItem > 0) {
                    UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/hfMaturityDate1", evaEmpProcessInfo[1]);
                    UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/hfMaturityDate2", evaEmpProcessInfo[2]);
                    UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/hfOkayDate", evaEmpProcessInfo[3]);
                    UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/rbtAppraisal", "1");
                    UtilitySvc.trySetXmlDocInnerText(ref doc, FormID + "/txtTrial", evaEmpProcessInfo[4]);
                }
            }

            UtilitySvc.writeLog("===================== xml template:" +
                DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " ================");

            UtilitySvc.writeLog("form xml:");
            UtilitySvc.writeLog(doc.OuterXml);


            ret = this.nanaProcSvc.invokeProcess(this.ProcessID, this.createParam.issuerId, this.issuerEmpInfoList[5] ,
                formArr[0], doc.OuterXml, string.Empty);

            UtilitySvc.writeLog("===================== create process at:" +
                DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " ================");

                             
            UtilitySvc.writeLog("process serial number:");
            UtilitySvc.writeLog(ret.resultMessage);

            return ret;
        }

        /// <summary>
        /// 尚未實作
        /// </summary>        
        public CommonViewModel.Result agree() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 尚未實作
        /// </summary>        
        public CommonViewModel.Result reject() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 驗證拋入參數
        /// </summary>
        /// <returns></returns>
        private CommonViewModel.Result validateCreateProcessParam() {
            CommonViewModel.Result ret = new CommonViewModel.Result();

            //填單人工號是否存在
            if (string.IsNullOrEmpty(this.createParam.issuerId)) {
                return ret.setResultValue(false, "301", "填單人工號不能為空值", string.Empty);
            }
            if (!string.IsNullOrEmpty(this.createParam.applyerId) &&  this.createParam.issuerId != this.createParam.applyerId) {
                return ret.setResultValue(false, "301", "若申請人有填寫則須與填單人工號相同", string.Empty);
            }
            //評鑑人員是否存在
            if (string.IsNullOrEmpty(this.createParam.evaluationEmpId)) {
                return ret.setResultValue(false, "301", "評鑑人員工號不能為空值", string.Empty);
            }

            //受評人相關
            //是否存在於 HCP
            this.hcpEvaEmpInfoList = this.newEmpEvaDao.getHCPInfoByEmpId(this.createParam.evaluationEmpId);

            if (this.hcpEvaEmpInfoList.Count == 0) {
                return ret.setResultValue(false, "302", "於HCP中查無受評人資料", string.Empty);
            }

            //是否存在於 EP
            this.evaEmpInfoList = this.newEmpEvaDao.getEmpInfoByID(this.createParam.evaluationEmpId);
            if (this.evaEmpInfoList.Count == 0) {
                return ret.setResultValue(false, "303", "於EP中查無受評人資料", string.Empty);
            }


            //起單人相關
            this.issuerEmpInfoList = this.newEmpEvaDao.getEmpInfoByID(this.createParam.issuerId);
            if (this.issuerEmpInfoList.Count == 0) {
                return ret.setResultValue(false, "304", "於EP中查無填單人資料", string.Empty);
            }


            //流程是否正在進行中
            if (this.newEmpEvaDao.isProcessing(this.createParam.evaluationEmpId)) {
                ret.success = false;
                ret.resultCode = "306";
                ret.resultMessage = "此評核人員正在評鑑中";
                return ret;
            }

            //是否是已開過的流程
            if (this.newEmpEvaDao.isProcessComplete(this.createParam.evaluationEmpId)) {
                ret.success = false;
                ret.resultCode = "307";
                ret.resultMessage = "此評核人員已評鑑過";
                return ret;
            }



            ret.success = true;
            ret.resultCode = "200";            
            return ret;
        }
        

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Xml;
using Newtonsoft.Json;
using WebServiceForEFGP.Models.Dao.ProcessInstanceDao;
using WebServiceForEFGP.Models.Services;
using WebServiceForEFGP.Models.Services.NaNaWebService;
using WebServiceForEFGP.Models.VeiwModel;
using WebServiceForEFGP.Models.VeiwModel.NaNaViewModel;
using WebServiceForEFGP.Models.VeiwModel.ProcessInstanceViewModel;



namespace WebServiceForEFGP.Models.ProcessInstance {
    public class HireEmployeeProcess : IProcessAction {

        private string processID = string.Empty;
        private string hireEmployeeHost = string.Empty;

        private NaNaFormWebSvc nanaFormSvc = null;
        private NaNaProcessWebSvc nanaProcSvc = null;
        private EmployeeSvc empSvc = null;

        private HireEmployeeDao hireEmpDao = null;

        private HireEmployeeProcessViewModel.CreateHireEmployeeProcessRequestParameter createParam = null;

        private HireEmployeeProcessViewModel.CreateProcessValidateParameter validateParam = null;

        public HireEmployeeProcess() {

            this.nanaFormSvc = new NaNaFormWebSvc();
            this.nanaProcSvc = new NaNaProcessWebSvc();
            this.empSvc = new EmployeeSvc();
            this.hireEmpDao = new HireEmployeeDao();

        }

        public HireEmployeeProcess(HireEmployeeProcessViewModel.CreateHireEmployeeProcessRequestParameter param) {

            this.processID = WebConfigurationManager.AppSettings["HireEmployeeProcess"];

            this.nanaFormSvc = new NaNaFormWebSvc();
            this.nanaProcSvc = new NaNaProcessWebSvc();
            this.empSvc = new EmployeeSvc();
            this.hireEmpDao = new HireEmployeeDao();

            this.createParam = param;
        }

        public CommonViewModel.Result agree() {
            throw new NotImplementedException();
        }

        public CommonViewModel.Result create() {
            CommonViewModel.Result ret = new CommonViewModel.Result();

            //驗證參數
            ret = this.validateCreateProcessParameter();

            if (!ret.success) {
                return ret;
            }
            //取出版型

            //取得XML樣板
            NaNaFormViewModel.FormTemplateSimpleResult formOIDRet = this.nanaFormSvc.findFormOIDsOfProcess(this.processID);
            if (!formOIDRet.success) {
                ret.resultMessage = "系統查無該流程樣板";
                ret.resultCode = "500";
                ret.success = false;
                return ret;
            }

            NaNaFormViewModel.FormTemplateSimpleResult formTemplateRet = this.nanaFormSvc.getFormFieldTemplate(formOIDRet.formOID);

            if (!formTemplateRet.success) {
                ret.resultMessage = "系統查無表單樣板內容";
                ret.resultCode = "500";
                ret.success = false;
                return ret;
            }

            //parse xml document
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(formTemplateRet.xmlTemplate.Replace(">defaultValue<", "><"));

            //新增流程
            ret = this.setCreateProcessValue(doc, formOIDRet.formOID);

            //加簽測試
            if (ret.success) {
                string processSerialNumber = ret.resultMessage;
                CommonViewModel.Result addActivityResult = this.addActivity(processSerialNumber, this.validateParam.createParam.addActivityEmpId);
                UtilitySvc.writeLog(Newtonsoft.Json.JsonConvert.SerializeObject(addActivityResult));
            }

            return ret;
        }

        public CommonViewModel.Result reject() {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 回傳至e召募系統
        /// </summary>
        /// <returns></returns>
        public CommonViewModel.Result deliverHireEmployeeInfo(string processSerialNumber) {
            CommonViewModel.Result ret = new CommonViewModel.Result();
            //驗證表單

            HireEmployeeForm hireForm = this.hireEmpDao.getHireEmployeeFormBySerialNumber(processSerialNumber);
            List<HireEmployeeForm_gdProjectAchievement> grades = new List<HireEmployeeForm_gdProjectAchievement>();
            if (hireForm == null) {
                return ret.setResultValue(false, "301", "", "");
            } else {
                grades = this.hireEmpDao.getHireEmployeeFormGdProjectAchievementsByFormSerialNumber(hireForm.formSerialNumber);
            }

            ret = this.validateDeliveryHireEmployeeInfoRequestParameter(hireForm, grades);

            if (!ret.success) {
                return ret;
            }

            //取得POST資料
            HireEmployeeProcessViewModel.DeliverHireEmployeeRequestParameter param = this.getDeliveryHireEmployeeRequestParam(hireForm, grades);

            UtilitySvc.writeLog("post parameter:");
            UtilitySvc.writeLog(JsonConvert.SerializeObject(param));

            //POST 至 e召募系統
            string posturl = this.hireEmployeeHost + "Process/Deliver/HireEmployee";

            string retStr = UtilitySvc.postRequest(posturl, JsonConvert.SerializeObject(param), new Dictionary<string, object>());
            CommonViewModel.Result post_ret = JsonConvert.DeserializeObject<CommonViewModel.Result>(retStr);
            ret.success = post_ret.success;
            ret.resultCode = post_ret.resultCode;
            ret.resultException = post_ret.resultException;
            ret.resultMessage = post_ret.resultMessage;

            return ret;
        }


        /// <summary>
        /// 動態加簽關卡(有可能加 0 ~ 2 個關卡)
        /// </summary>        
        public CommonViewModel.Result addActivity(string processSerialNumber) {
            CommonViewModel.Result ret = new CommonViewModel.Result();


            HireEmployeeForm hireForm = this.hireEmpDao.getHireEmployeeFormBySerialNumber(processSerialNumber);
            if (hireForm == null) {
                return ret.setResultValue(false, "301", "form instance not found", string.Empty);
            }

            if (string.IsNullOrWhiteSpace(hireForm.hidaddActivityEmpId)) {
                return ret.setResultValue(true, "200", string.Empty, string.Empty);
            }


            //判斷是否有要加簽
            string interviewer_2 = hireForm.txtInterviewerID2;
            if (!string.IsNullOrWhiteSpace(interviewer_2)) {
                EmployeeViewModel.EmployeeInfoBasicResult interviewRet2 = this.empSvc.getEmployeeInfoBasicByEmpId(interviewer_2);

                if (!interviewRet2.success || interviewRet2.empInfo == null) {
                    return ret.setResultValue(false, "301", string.Format("user {0} not found", interviewer_2), string.Empty);
                }

                NaNaProcessViewModel.AddActivityParameter addActivityParam2 = new NaNaProcessViewModel.AddActivityParameter() {
                    multiUserMode = "FOR_EACH",
                    name = hireForm.txtInterviewerName2 + "面談",
                    participantType = "HUMAN",
                    participantID = interviewer_2,
                    participantOID = interviewRet2.empInfo.userOID
                };

                this.nanaProcSvc.addCustomActivity(processSerialNumber, "ACT12", "ACT1", addActivityParam2);
            }

            string interviewer_1 = hireForm.txtInterviewerID1;
            if (!string.IsNullOrWhiteSpace(interviewer_1)) {
                EmployeeViewModel.EmployeeInfoBasicResult interviewRet1 = this.empSvc.getEmployeeInfoBasicByEmpId(interviewer_1);

                if (!interviewRet1.success || interviewRet1.empInfo == null) {
                    return ret.setResultValue(false, "301", string.Format("user {0} not found", interviewer_1), string.Empty);
                }

                NaNaProcessViewModel.AddActivityParameter addActivityParam1 = new NaNaProcessViewModel.AddActivityParameter() {
                    multiUserMode = "FOR_EACH",
                    name = hireForm.txtInterviewerName1 + "面談",
                    participantType = "HUMAN",
                    participantID = interviewer_1,
                    participantOID = interviewRet1.empInfo.userOID
                };
                this.nanaProcSvc.addCustomActivity(processSerialNumber, "ACT14", "ACT1", addActivityParam1);
            }
            return ret.setResultValue(true, "200", string.Empty, string.Empty);
        }




        /// <summary>
        /// 加簽(有可能加 0 ~ 2 個關卡)
        /// </summary>
        /// <param name="processSerialNumber">流程編號</param>
        /// <param name="empIDArr">加簽人員</param>        
        private CommonViewModel.Result addActivity(string processSerialNumber, List<string> empIDArr) {
            CommonViewModel.Result ret = new CommonViewModel.Result();

            //不用加簽
            if (empIDArr.Count == 0) {
                return ret.setResultValue(true, "200", string.Empty, string.Empty);
            }

            //依序加簽
            for (int i = empIDArr.Count; i > 0; i--) {
                string empID = empIDArr[i - 1];
                EmployeeViewModel.EmployeeInfoBasicResult interviewRet = this.empSvc.getEmployeeInfoBasicByEmpId(empID);
                NaNaProcessViewModel.AddActivityParameter addActivityParam = new NaNaProcessViewModel.AddActivityParameter() {
                    multiUserMode = "FOR_EACH",
                    name = interviewRet.empInfo.name + "面談",
                    participantType = "HUMAN",
                    participantID = interviewRet.empInfo.id,
                    participantOID = interviewRet.empInfo.userOID

                };
                this.nanaProcSvc.addCustomActivity(processSerialNumber, "ACT14", "ACT1", addActivityParam);
                UtilitySvc.writeLog(empID);


            }



            return ret.setResultValue(true, "200", string.Join(";", empIDArr.ToArray()), string.Empty);
        }


        /// <summary>
        /// 驗證參數新增表單參數
        /// </summary>        
        private CommonViewModel.Result validateCreateProcessParameter() {
            CommonViewModel.Result ret = new CommonViewModel.Result();
            this.validateParam = new HireEmployeeProcessViewModel.CreateProcessValidateParameter();

            this.validateParam.createParam = this.createParam;

            EmployeeViewModel.EmployeeInfoLightResult issuerEmp = this.empSvc.getEmployeeInfoLightByEmpId(this.createParam.issuerId);
            EmployeeViewModel.EmployeeInfoLightResult applyerEmp = null;
            if (this.createParam.issuerId == this.createParam.applyerId) {
                applyerEmp = issuerEmp;
            } else {
                applyerEmp = this.empSvc.getEmployeeInfoLightByEmpId(this.createParam.applyerId);
            }

            if (issuerEmp.empInfo == null) {
                return ret.setResultValue(false, "302", string.Format("此員工 {0} 不存在", this.createParam.issuerId), "");
            } else {
                this.validateParam.issuerEmp = issuerEmp.empInfo;
            }

            if (applyerEmp.empInfo == null) {
                return ret.setResultValue(false, "302", string.Format("此員工 {0} 不存在", this.createParam.applyerId), "");
            } else {
                this.validateParam.applyerEmp = applyerEmp.empInfo;
            }

            if (this.createParam.hireInformation == null) {
                return ret.setResultValue(false, "301", "尚未填寫錄用資訊", "");
            }

            List<string> functionCodes = HireEmployeeProcessViewModel.functionCodeDic.Keys.ToList();

            bool notIncludeDefaultFunctionCode = functionCodes.Exists(x => !this.createParam.assessment.functions.Exists(y => y.assessmentId == x));

            if (notIncludeDefaultFunctionCode) {
                return ret.setResultValue(false, "301", "職能評估資料錯誤，至少填寫F01~F12欄位資訊", "");
            }

            if (!HireEmployeeProcessViewModel.vacancyReasonCodeDic.ContainsKey(this.createParam.hireInformation.vacancyReason)) {
                return ret.setResultValue(false, "301", "職缺理由代碼錯誤", "");
            }

            if (this.createParam.hireInformation.vacancyReason == "VR03") {

                List<string> vacancyTypeCodeList = new List<string>() { "VR03-01", "VR03-02" };

                if (!vacancyTypeCodeList.Exists(x => x == this.createParam.hireInformation.vacancyType)) {
                    return ret.setResultValue(false, "301", "若職缺理由為 VR03 則出缺補實原因 請填寫 VR03-01 或 VR03-02", "");
                }

                if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.vacancyEmployeeId)) {
                    return ret.setResultValue(false, "301", "若職缺理由為 VR03 ，出缺補實員工工號不能為空值", "");
                }

                if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.vacancyEmployeeName)) {
                    return ret.setResultValue(false, "301", "若職缺理由為 VR03 ，出缺補實員工姓名不能為空值", "");
                }

                EmployeeViewModel.EmployeeInfoLightResult vacancyEmp = this.empSvc.getEmployeeInfoLightByEmpId(this.createParam.hireInformation.vacancyEmployeeId);

                if (vacancyEmp.empInfo == null) {
                    return ret.setResultValue(false, "302", string.Format("此員工 {0} 不存在", this.createParam.hireInformation.vacancyEmployeeId), "");
                } else {
                    this.validateParam.vacancyEmp = vacancyEmp.empInfo;
                }
            }


            if (this.createParam.hireInformation.vacancyReason == "VR02" || this.createParam.hireInformation.vacancyReason == "VR04") {
                if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.vacancyDescription)) {
                    return ret.setResultValue(false, "302", "若職缺理由為 VR02 或 VR04 則職缺理由額外說明必須填寫", "");
                }
            }

            if (!HireEmployeeProcessViewModel.hireProbabilityCodeDic.ContainsKey(this.createParam.hireInformation.hireProbability)) {
                return ret.setResultValue(false, "301", "招募成功機率預估代碼錯誤", "");
            }

            if (!HireEmployeeProcessViewModel.abilityRankCodeDic.ContainsKey(this.createParam.hireInformation.abilityRank)) {
                return ret.setResultValue(false, "301", "能力潛質同儕排序代碼錯誤", "");
            }


            foreach (string empId in this.createParam.addActivityEmpId) {
                EmployeeViewModel.EmployeeInfoLightResult addActivityEmp = this.empSvc.getEmployeeInfoLightByEmpId(empId);
                if (addActivityEmp.empInfo == null) {
                    return ret.setResultValue(false, "302", string.Format("加簽人員 {0} 不存在", empId), "");
                } else {
                    this.validateParam.addActivityEmpList.Add(addActivityEmp.empInfo);
                }
            }


            if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.departmentName)) {
                return ret.setResultValue(false, "301", "錄用單位不能為空值", "");
            }

            //check department id is exist
            if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.departmentId)) {
                return ret.setResultValue(false, "301", "部門代號不能為空值", "");
            }

            if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.vacancyName)) {
                return ret.setResultValue(false, "301", "職缺不能為空值", "");
            }

            if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.workLocation)) {
                return ret.setResultValue(false, "301", "工作地點不能為空值", "");
            }

            if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.hireEmployeeName)) {
                return ret.setResultValue(false, "301", "錄用人員名稱不能為空值", "");
            }

            if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.suggestedLevel)) {
                return ret.setResultValue(false, "301", "建議職等不能為空值", "");
            }

            if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.workTime)) {
                return ret.setResultValue(false, "301", "班別不能為空值", "");
            }

            if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.qualification)) {
                return ret.setResultValue(false, "301", "最高學歷不能為空值", "");
            }

            if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.schoolName)) {
                return ret.setResultValue(false, "301", "學校不能為空值", "");
            }

            if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.schoolDepartment)) {
                return ret.setResultValue(false, "301", "科系不能為空值", "");
            }

            if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.otherSkill)) {
                return ret.setResultValue(false, "301", "其他專業技能/證照不能為空值", "");
            }

            if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.hireProbability)) {
                return ret.setResultValue(false, "301", "招募成功機率不能為空值", "");
            }

            if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.abilityRank)) {
                return ret.setResultValue(false, "301", "能力潛質同儕排序不能為空值", "");
            }

            if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.totalWorkYears)) {
                return ret.setResultValue(false, "301", "總工作年資不能為空值", "");
            }


            if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.relatedJobWorkYears)) {
                return ret.setResultValue(false, "301", "與職務相關工作年資不能為空值", "");
            }


            if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.manageWorkYears)) {
                return ret.setResultValue(false, "301", "管理年資不能為空值", "");
            }

            if (string.IsNullOrWhiteSpace(this.createParam.hireInformation.referenceLink)) {
                return ret.setResultValue(false, "301", "參考資料連結不能為空值", "");
            }

            if (string.IsNullOrWhiteSpace(this.createParam.assessment.topThreeTask)) {
                return ret.setResultValue(false, "301", "重點工作目標列示不能為空值", "");
            }

            if (string.IsNullOrWhiteSpace(this.createParam.assessment.pastWorkComment)) {
                return ret.setResultValue(false, "301", "過往實績考察不能為空值", "");
            }

            if (string.IsNullOrWhiteSpace(this.createParam.assessment.managerComment.advantage)) {
                return ret.setResultValue(false, "301", "優勢不能為空值", "");
            }

            if (string.IsNullOrWhiteSpace(this.createParam.assessment.managerComment.enhanceAbility)) {
                return ret.setResultValue(false, "301", "可加強之處不能為空值", "");
            }

            ret.success = true;
            ret.resultCode = "200";

            return ret;
        }


        private HireEmployeeProcessViewModel.CreateHireEmployeeProcessResult setCreateProcessValue(XmlDocument doc, string formOID) {
            HireEmployeeProcessViewModel.CreateHireEmployeeProcessResult ret = new HireEmployeeProcessViewModel.CreateHireEmployeeProcessResult();
            string formID = "HireEmployeeForm";

            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtDateofApplication", DateTime.Now.ToString("yyyy/MM/dd"));
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/SerialNumber", string.Empty);

            #region 填單人資訊
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtPreparerID", this.validateParam.issuerEmp.id);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtPreparerName", this.validateParam.issuerEmp.name);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtPreparerDepID", this.validateParam.issuerEmp.departmentId);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtPreparerDepName", this.validateParam.issuerEmp.department);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtPreparerTelephoneExtension",
                string.IsNullOrEmpty(this.validateParam.createParam.issuerContactPhoneNumner) ?
                this.validateParam.issuerEmp.extNo : this.validateParam.createParam.issuerContactPhoneNumner);
            #endregion

            #region 申請人資訊
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtApplicantID", this.validateParam.applyerEmp.id);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtApplicantName", this.validateParam.applyerEmp.name);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtApplicantDepID", this.validateParam.applyerEmp.departmentId);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtApplicantDepName", this.validateParam.applyerEmp.department);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtApplicantExtension",
                string.IsNullOrEmpty(this.validateParam.createParam.applyerContactPhoneNumber) ?
                this.validateParam.applyerEmp.extNo : this.validateParam.createParam.applyerContactPhoneNumber);
            #endregion

            #region 錄用人員資訊
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtDepartment", this.validateParam.createParam.hireInformation.departmentName);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtNewEmployeeName", this.validateParam.createParam.hireInformation.hireEmployeeName);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtVacancies", this.validateParam.createParam.hireInformation.vacancyName);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtHighestEducation", this.createParam.hireInformation.qualification);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtShool", this.validateParam.createParam.hireInformation.schoolName);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtDept", this.validateParam.createParam.hireInformation.schoolDepartment);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtJobPlace", this.validateParam.createParam.hireInformation.workLocation);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtSpecialSkills", this.validateParam.createParam.hireInformation.otherSkill);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/rdoVacanciesReason1", this.validateParam.createParam.hireInformation.vacancyReason);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/rdoVacanciesReason2", this.validateParam.createParam.hireInformation.vacancyReason);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/rdoVacanciesReason3", this.validateParam.createParam.hireInformation.vacancyReason);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/rdoVacanciesReason4", this.validateParam.createParam.hireInformation.vacancyReason);
            if (this.validateParam.createParam.hireInformation.vacancyReason == "VR03") {
                UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtVacanciesReason3_Name", this.validateParam.createParam.hireInformation.vacancyName);
                UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/rdoPersonnelChangeOrResigned", this.validateParam.createParam.hireInformation.vacancyType);
                UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/DateExpectedEffectiveDate",
                    this.validateParam.createParam.hireInformation.vacancyEffectiveDate.ToString("yyyy/MM/dd"));
            }
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtTotalSeniorityForHPD", this.validateParam.createParam.hireInformation.totalWorkYears);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtTotalSeniorityForHPDRemark", this.validateParam.createParam.hireInformation.totalWorkYearsMemo);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtRelatedSeniorityForHPD", this.validateParam.createParam.hireInformation.relatedJobWorkYears);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtRelatedSeniorityForHPDRemark", this.validateParam.createParam.hireInformation.relatedJobWorkYearsMemo);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtManageSeniorityForHPD", this.validateParam.createParam.hireInformation.manageWorkYears);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtManageSeniorityForHPDRemark", this.validateParam.createParam.hireInformation.manageWorkYearsMemo);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/rdoAbilityPotentialPeerSequence", this.validateParam.createParam.hireInformation.abilityRank);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/rdoRecruitSuccessProbabilityEstimated", this.validateParam.createParam.hireInformation.hireProbability);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtRecommendGrade", this.validateParam.createParam.hireInformation.suggestedLevel);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtJobTitle", this.validateParam.createParam.hireInformation.title);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtWorkDayShift", this.validateParam.createParam.hireInformation.workLocation);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/hidReferenceLink", this.validateParam.createParam.hireInformation.referenceLink);

            #endregion

            #region 測驗成績
            XmlNode records_node = doc.SelectSingleNode(formID + "/gdProjectAchievement/records");
            XmlNode record_node = doc.SelectSingleNode(formID + "/gdProjectAchievement/records/record").CloneNode(true);
            records_node.InnerXml = "";
            int grade_sort = 1;
            foreach (HireEmployeeProcessViewModel.Grade g in this.validateParam.createParam.hireInformation.grades) {
                XmlNode n = record_node.Clone();
                n.Attributes["id"].Value = "grade_" + grade_sort.ToString();
                n.SelectSingleNode("item[@id='gd_Item']").InnerText = g.name;
                n.SelectSingleNode("item[@id='gd_Achievement']").InnerText = g.score;
                records_node.AppendChild(n);
                grade_sort++;
            }
            #endregion

            #region 職能項目評估
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txaKeyObjectivesTop3",
                this.validateParam.createParam.assessment.topThreeTask);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txaPastJobsPerformanceVisits",
                this.validateParam.createParam.assessment.pastWorkComment);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtInitiativeAndResponsibilityForImmediateSupervisor",
                this.validateParam.createParam.assessment.functions.Find(x => x.assessmentId == "F01").score.ToString());
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtDevelopmentOfCommonGoalForImmediateSupervisor",
                this.validateParam.createParam.assessment.functions.Find(x => x.assessmentId == "F02").score.ToString());
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtCommunicationAndListeningForImmediateSupervisor",
                this.validateParam.createParam.assessment.functions.Find(x => x.assessmentId == "F03").score.ToString());
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtPartnershipsForImmediateSupervisor",
                this.validateParam.createParam.assessment.functions.Find(x => x.assessmentId == "F04").score.ToString());
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtProblemsAndSolutionsForImmediateSupervisor",
                this.validateParam.createParam.assessment.functions.Find(x => x.assessmentId == "F05").score.ToString());
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtExcellenceForImmediateSupervisor",
                this.validateParam.createParam.assessment.functions.Find(x => x.assessmentId == "F06").score.ToString());
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtCourageToInnovateForImmediateSupervisor",
                this.validateParam.createParam.assessment.functions.Find(x => x.assessmentId == "F07").score.ToString());
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtHonestyForImmediateSupervisor",
                this.validateParam.createParam.assessment.functions.Find(x => x.assessmentId == "F08").score.ToString());
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtGoalManagementCapabilitiesForImmediateSupervisor",
                this.validateParam.createParam.assessment.functions.Find(x => x.assessmentId == "F09").score.ToString());
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtShapingSuccessfulTeamForImmediateSupervisor",
                this.validateParam.createParam.assessment.functions.Find(x => x.assessmentId == "F10").score.ToString());
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtGuidanceAndDevelopmentOfHumanResourcesForImmediateSupervisor",
                this.validateParam.createParam.assessment.functions.Find(x => x.assessmentId == "F11").score.ToString());
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtDailyDecisionMakingCapacityForImmediateSupervisor",
                this.validateParam.createParam.assessment.functions.Find(x => x.assessmentId == "F12").score.ToString());

            List<HireEmployeeProcessViewModel.Function> custFunctions = this.validateParam.createParam.assessment.functions.FindAll(x => x.isCustomItem);
            int custFunctionsLength = custFunctions.Count > 4 ? 4 : custFunctions.Count;
            for (int i = 1; i <= custFunctionsLength; i++) {

                UtilitySvc.trySetXmlDocInnerText(ref doc, formID + string.Format("/txtOtherProfessionalFunctionsForImmediateSupervisor{0}", i.ToString()),
                    custFunctions[i - 1].score.ToString());
                UtilitySvc.trySetXmlDocInnerText(ref doc, formID + string.Format("/txtOtherProfessionalFunctionsForFunction{0}", i.ToString()),
                    custFunctions[i - 1].customItemName);
                UtilitySvc.trySetXmlDocInnerText(ref doc, formID + string.Format("/txtOtherProfessionalFunctionsForDefinition{0}", i.ToString()),
                    custFunctions[i - 1].customItemDescription);
            }
            #endregion

            #region 主管評語
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txaAdvantageforSupervisor",
                this.validateParam.createParam.assessment.managerComment.advantage);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txaEnhanceSectionforSupervisor",
                this.validateParam.createParam.assessment.managerComment.enhanceAbility);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/DateInterviewTimeforSupervisor",
                this.validateParam.createParam.assessment.managerComment.dateMeet.ToString("yyyy/MM/dd"));
            #endregion

            #region 跨部門面試反饋
            int addActivityEmpLength = this.validateParam.addActivityEmpList.Count > 2 ? 2 : this.validateParam.addActivityEmpList.Count;

            for (int i = 1; i <= addActivityEmpLength; i++) {
                EmployeeViewModel.EmployeeInfoLight emp = this.validateParam.addActivityEmpList[i - 1];
                UtilitySvc.trySetXmlDocInnerText(ref doc, string.Format(formID + "/txtInterviewerID{0}", i), emp.id);
                UtilitySvc.trySetXmlDocInnerText(ref doc, string.Format(formID + "/txtInterviewerName{0}", i), emp.name);
            }

            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/hidaddActivityEmpId",
                string.Join(";", this.validateParam.createParam.addActivityEmpId.ToArray()));
            #endregion

            #region 附件區塊
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/hidAttachmentLinks", JsonConvert.SerializeObject(this.validateParam.createParam.attachments));
            #endregion

            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/hidHasDefaultValue", "1");

            #region 開單預設空白欄位
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txaAdvantageforExecutives", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txaEnhanceSectionforExecutives", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txaAdvantageforHumanResources", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txaEnhanceSectionforHumanResources", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtGradeForHR", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtApprovedSalaryForHR", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtSalaryComparativelyForHR", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtRemarkForHR", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/rdoSignoffProcessForHR", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtJobTitleForHR", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/DateInterviewTimeforExecutives", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/DateInterviewTimeforHumanResources", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtTotalSeniorityForHR", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtRelatedSeniorityForHR", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtManageSeniorityForHR", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtTotalSeniorityForHrRemark", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtRelatedSeniorityForHrRemark", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtManageSeniorityForHrRemark", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtInitiativeAndResponsibilityForHumanResourcesDepartment", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtDevelopmentOfCommonGoalForHumanResourcesDepartment", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtCommunicationAndListeningForHumanResourcesDepartment", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtPartnershipsForHumanResourcesDepartmentTextbox10", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtProblemsAndSolutionsForHumanResourcesDepartment", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtHonestyForHumanResourcesDepartment", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtExcellenceForHumanResourcesDepartment", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtShapingSuccessfulTeamForHumanResourcesDepartment", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtGoalManagementCapabilitiesForHumanResourcesDepartment", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtGuidanceAndDevelopmentOfHumanResourcesForHumanResourcesDepartment", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtDailyDecisionMakingCapacityForHumanResourcesDepartment", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtOtherProfessionalFunctionsForHR1", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtOtherProfessionalFunctionsForHR2", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtOtherProfessionalFunctionsForHR3", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtOtherProfessionalFunctionsForHR4", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtInitiativeAndResponsibilityForExecutives", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtDevelopmentOfCommonGoalForExecutives", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtCommunicationAndListeningForExecutives", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtPartnershipsForExecutives", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtProblemsAndSolutionsForExecutives", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtExcellenceForExecutives", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtCourageToInnovateForExecutives", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtHonestyForExecutives", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtGoalManagementCapabilitiesForExecutives", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtShapingSuccessfulTeamForExecutives", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtGuidanceAndDevelopmentOfHumanResourcesForExecutives", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtDailyDecisionMakingCapacityForExecutives", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtOtherProfessionalFunctionsForExecutives1", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtOtherProfessionalFunctionsForExecutives2", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtOtherProfessionalFunctionsForExecutives3", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtOtherProfessionalFunctionsForExecutives4", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtCourageToInnovateForHumanResourcesDepartment", string.Empty);

            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/hidDynamicSignaturePersonnel", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txaAdvantageforInterviewer1", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txaCautionRequired1", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/rdoRecruitSuccessProbabilityEstimatedForInterviewer1", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/rdoIdentityCategory1", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtIdentityCategoryOther1", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txaAdvantageforInterviewer2", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txaCautionRequired2", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/rdoRecruitSuccessProbabilityEstimatedForInterviewer2", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/rdoIdentityCategory2", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtIdentityCategoryOther2", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txaPastJobsPerformanceVisitsForHR", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtVacanciesReason2", string.Empty);
            UtilitySvc.trySetXmlDocInnerText(ref doc, formID + "/txtVacanciesReason4", string.Empty);
            #endregion


            CommonViewModel.Result createProcess = this.nanaProcSvc.invokeProcess(this.processID,
                this.createParam.issuerId,
                this.validateParam.issuerEmp.departmentId,
                formOID, doc.OuterXml,
                string.Empty);

            //debug
            //UtilitySvc.writeLog("form xml:");
            //UtilitySvc.writeLog(doc.OuterXml);

            ret.setResultValue(createProcess.success,
                createProcess.resultCode,
                createProcess.resultMessage,
                createProcess.resultException);

            ret.processSerialNumber = createProcess.resultMessage;


            return ret;
        }


        private CommonViewModel.Result validateDeliveryHireEmployeeInfoRequestParameter(HireEmployeeForm hireEmpForm  , List<HireEmployeeForm_gdProjectAchievement > grades) {
            CommonViewModel.Result ret = new CommonViewModel.Result();

            return ret;
        }

        private HireEmployeeProcessViewModel.DeliverHireEmployeeRequestParameter getDeliveryHireEmployeeRequestParam(HireEmployeeForm hireEmpForm, List<HireEmployeeForm_gdProjectAchievement> grades) {
            HireEmployeeProcessViewModel.DeliverHireEmployeeRequestParameter ret = new HireEmployeeProcessViewModel.DeliverHireEmployeeRequestParameter();

            //流程序號
            ret.processSerialNumber = hireEmpForm.processSerialNumber; 
            ret.hireEmployeeName = hireEmpForm.txtNewEmployeeName;
            ret.JobLevel = hireEmpForm.txtGradeForHR;
            ret.JobTitle = hireEmpForm.txtJobTitleForHR;
            ret.Salary = hireEmpForm.txtApprovedSalaryForHR;
            ret.SalaryCompare = hireEmpForm.txtSalaryComparativelyForHR;
            ret.Note = hireEmpForm.txtRemarkForHR;
            ret.totalWorkYears = hireEmpForm.txtTotalSeniorityForHR;
            ret.totalWorkYearsMemo = hireEmpForm.txtTotalSeniorityForHrRemark;
            ret.relatedJobWorkYears = hireEmpForm.txtRelatedSeniorityForHR;
            ret.relatedJobWorkYearsMemo = hireEmpForm.txtRelatedSeniorityForHrRemark;
            ret.manageWorkYears = hireEmpForm.txtManageSeniorityForHR;
            ret.manageWorkYearsMemo = hireEmpForm.txtManageSeniorityForHrRemark;
            ret.pastWorkComment = hireEmpForm.txaPastJobsPerformanceVisitsForHR;

            //職能評估項目
            ret.functions = this.getDeliverFunctions(hireEmpForm);
            //主管評核
            ret.managerComment = this.getDeliverManagerComment(hireEmpForm);
            //跨部門面試
            ret.activityEmpComment = this.getDeliverActivityEmpComment(hireEmpForm);


            return ret;
        }


        /// <summary>
        /// 職能評估項目
        /// </summary>        
        private List<HireEmployeeProcessViewModel.DeliverFunction> getDeliverFunctions(HireEmployeeForm hireEmpForm) {
            List<HireEmployeeProcessViewModel.DeliverFunction> ret = new List<HireEmployeeProcessViewModel.DeliverFunction>();

            #region 上一階主管

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtInitiativeAndResponsibilityForExecutives),
                assessmentId = "F01",
                isCustomItem = false,
                functionType = "mang"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtDevelopmentOfCommonGoalForExecutives),
                assessmentId = "F02",
                isCustomItem = false,
                functionType = "mang"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtCommunicationAndListeningForExecutives),
                assessmentId = "F03",
                isCustomItem = false,
                functionType = "mang"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtPartnershipsForExecutives),
                assessmentId = "F04",
                isCustomItem = false,
                functionType = "mang"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtProblemsAndSolutionsForExecutives),
                assessmentId = "F05",
                isCustomItem = false,
                functionType = "mang"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtExcellenceForExecutives),
                assessmentId = "F06",
                isCustomItem = false,
                functionType = "mang"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtCourageToInnovateForExecutives),
                assessmentId = "F07",
                isCustomItem = false,
                functionType = "mang"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtHonestyForExecutives),
                assessmentId = "F08",
                isCustomItem = false,
                functionType = "mang"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtGoalManagementCapabilitiesForExecutives),
                assessmentId = "F09",
                isCustomItem = false,
                functionType = "mang"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtShapingSuccessfulTeamForExecutives),
                assessmentId = "F10",
                isCustomItem = false,
                functionType = "mang"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtGuidanceAndDevelopmentOfHumanResourcesForExecutives),
                assessmentId = "F11",
                isCustomItem = false,
                functionType = "mang"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtDailyDecisionMakingCapacityForExecutives),
                assessmentId = "F12",
                isCustomItem = false,
                functionType = "mang"
            });

            #endregion

            #region HR評核

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtInitiativeAndResponsibilityForHumanResourcesDepartment),
                assessmentId = "F01",
                isCustomItem = false,
                functionType = "hr"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtDevelopmentOfCommonGoalForHumanResourcesDepartment),
                assessmentId = "F02",
                isCustomItem = false,
                functionType = "hr"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtCommunicationAndListeningForHumanResourcesDepartment),
                assessmentId = "F03",
                isCustomItem = false,
                functionType = "hr"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtPartnershipsForHumanResourcesDepartment),
                assessmentId = "F04",
                isCustomItem = false,
                functionType = "hr"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtProblemsAndSolutionsForHumanResourcesDepartment),
                assessmentId = "F05",
                isCustomItem = false,
                functionType = "hr"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtExcellenceForHumanResourcesDepartment),
                assessmentId = "F06",
                isCustomItem = false,
                functionType = "hr"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtCourageToInnovateForHumanResourcesDepartment),
                assessmentId = "F07",
                isCustomItem = false,
                functionType = "hr"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtHonestyForHumanResourcesDepartment),
                assessmentId = "F08",
                isCustomItem = false,
                functionType = "hr"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtGoalManagementCapabilitiesForHumanResourcesDepartment),
                assessmentId = "F09",
                isCustomItem = false,
                functionType = "hr"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtShapingSuccessfulTeamForHumanResourcesDepartment),
                assessmentId = "F10",
                isCustomItem = false,
                functionType = "hr"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtGuidanceAndDevelopmentOfHumanResourcesForHumanResourcesDepartment),
                assessmentId = "F11",
                isCustomItem = false,
                functionType = "hr"
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                score = int.Parse(hireEmpForm.txtDailyDecisionMakingCapacityForHumanResourcesDepartment),
                assessmentId = "F12",
                isCustomItem = false,
                functionType = "hr"
            });
            #endregion

            #region 額外評估項目
            
            if (!string.IsNullOrWhiteSpace(hireEmpForm.txtOtherProfessionalFunctionsForFunction1)) {
                ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                    assessmentId = string.Empty,
                    customItemDescription = hireEmpForm.txtOtherProfessionalFunctionsForDefinition1,
                    customItemName = hireEmpForm.txtOtherProfessionalFunctionsForFunction1,
                    functionType = "mang",
                    isCustomItem = true,
                    score = int.Parse(hireEmpForm.txtOtherProfessionalFunctionsForExecutives1)
                });
                ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                    assessmentId = string.Empty,
                    customItemDescription = hireEmpForm.txtOtherProfessionalFunctionsForDefinition1,
                    customItemName = hireEmpForm.txtOtherProfessionalFunctionsForFunction1,
                    functionType = "hr",
                    isCustomItem = true,
                    score = int.Parse(hireEmpForm.txtOtherProfessionalFunctionsForHR1)
                });
            }

            if (!string.IsNullOrWhiteSpace(hireEmpForm.txtOtherProfessionalFunctionsForFunction2)) {
                ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                    assessmentId = string.Empty,
                    customItemDescription = hireEmpForm.txtOtherProfessionalFunctionsForDefinition2,
                    customItemName = hireEmpForm.txtOtherProfessionalFunctionsForFunction2,
                    functionType = "mang",
                    isCustomItem = true,
                    score = int.Parse(hireEmpForm.txtOtherProfessionalFunctionsForExecutives2)
                });
                ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                    assessmentId = string.Empty,
                    customItemDescription = hireEmpForm.txtOtherProfessionalFunctionsForDefinition2,
                    customItemName = hireEmpForm.txtOtherProfessionalFunctionsForFunction2,
                    functionType = "hr",
                    isCustomItem = true,
                    score = int.Parse(hireEmpForm.txtOtherProfessionalFunctionsForHR2)
                });
            }

            if (!string.IsNullOrWhiteSpace(hireEmpForm.txtOtherProfessionalFunctionsForFunction3)) {
                ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                    assessmentId = string.Empty,
                    customItemDescription = hireEmpForm.txtOtherProfessionalFunctionsForDefinition3,
                    customItemName = hireEmpForm.txtOtherProfessionalFunctionsForFunction3,
                    functionType = "mang",
                    isCustomItem = true,
                    score = int.Parse(hireEmpForm.txtOtherProfessionalFunctionsForExecutives3)
                });
                ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                    assessmentId = string.Empty,
                    customItemDescription = hireEmpForm.txtOtherProfessionalFunctionsForDefinition3,
                    customItemName = hireEmpForm.txtOtherProfessionalFunctionsForFunction3,
                    functionType = "hr",
                    isCustomItem = true,
                    score = int.Parse(hireEmpForm.txtOtherProfessionalFunctionsForHR3)
                });
            }

            if (!string.IsNullOrWhiteSpace(hireEmpForm.txtOtherProfessionalFunctionsForFunction4)) {
                ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                    assessmentId = string.Empty,
                    customItemDescription = hireEmpForm.txtOtherProfessionalFunctionsForDefinition4,
                    customItemName = hireEmpForm.txtOtherProfessionalFunctionsForFunction4,
                    functionType = "mang",
                    isCustomItem = true,
                    score = int.Parse(hireEmpForm.txtOtherProfessionalFunctionsForExecutives4)
                });
                ret.Add(new HireEmployeeProcessViewModel.DeliverFunction() {
                    assessmentId = string.Empty,
                    customItemDescription = hireEmpForm.txtOtherProfessionalFunctionsForDefinition4,
                    customItemName = hireEmpForm.txtOtherProfessionalFunctionsForFunction4,
                    functionType = "hr",
                    isCustomItem = true,
                    score = int.Parse(hireEmpForm.txtOtherProfessionalFunctionsForHR4)
                });
            }
            #endregion

            return ret;
        }

        /// <summary>
        /// 主管評語
        /// </summary>        
        private List<HireEmployeeProcessViewModel.DeliverManagerComment> getDeliverManagerComment(HireEmployeeForm hireEmpForm) {
            List<HireEmployeeProcessViewModel.DeliverManagerComment> ret = new List<HireEmployeeProcessViewModel.DeliverManagerComment>();

            ret.Add(new HireEmployeeProcessViewModel.DeliverManagerComment() {
                advantage = hireEmpForm.txaAdvantageforExecutives.Trim(),
                commentType = "mang",
                dateMeet = hireEmpForm.DateInterviewTimeforExecutives.Value,
                enhanceAbility = hireEmpForm.txaEnhanceSectionforExecutives.Trim()
            });

            ret.Add(new HireEmployeeProcessViewModel.DeliverManagerComment() {
                advantage = hireEmpForm.txaAdvantageforHumanResources.Trim(),
                commentType = "hr",
                dateMeet = hireEmpForm.DateInterviewTimeforHumanResources.Value,
                enhanceAbility = hireEmpForm.txaEnhanceSectionforHumanResources.Trim()
            });


            return ret;
        }

        /// <summary>
        /// 跨部門面試反饋
        /// </summary>        
        private List<HireEmployeeProcessViewModel.DeliverActivityEmpComment> getDeliverActivityEmpComment(HireEmployeeForm hireEmpForm) {
            List<HireEmployeeProcessViewModel.DeliverActivityEmpComment> ret = new List<HireEmployeeProcessViewModel.DeliverActivityEmpComment>();

            if (!string.IsNullOrWhiteSpace(hireEmpForm.txtInterviewerID1)) {
                ret.Add(new HireEmployeeProcessViewModel.DeliverActivityEmpComment() {
                    activityEmpId = hireEmpForm.txtInterviewerID1.Trim(),
                    activityEmpName = hireEmpForm.txtInterviewerName1.Trim(),
                    advantage = hireEmpForm.txaAdvantageforInterviewer1,
                    enhanceAbility = hireEmpForm.txaCautionRequired1,
                    hireProbability = hireEmpForm.rdoRecruitSuccessProbabilityEstimatedForInterviewer1,
                    identity = hireEmpForm.rdoIdentityCategory1,
                    identityDescription = hireEmpForm.txtIdentityCategoryOther1
                });
            }

            if (!string.IsNullOrWhiteSpace(hireEmpForm.txtInterviewerID2)) {
                ret.Add(new HireEmployeeProcessViewModel.DeliverActivityEmpComment() {
                    activityEmpId = hireEmpForm.txtInterviewerID2.Trim(),
                    activityEmpName = hireEmpForm.txtInterviewerName2.Trim(),
                    advantage = hireEmpForm.txaAdvantageforInterviewer2,
                    enhanceAbility = hireEmpForm.txaCautionRequired2,
                    hireProbability = hireEmpForm.rdoRecruitSuccessProbabilityEstimatedForInterviewer2,
                    identity = hireEmpForm.rdoIdentityCategory2,
                    identityDescription = hireEmpForm.txtIdentityCategoryOther2
                });
            }

            return ret;
        }


    }
}

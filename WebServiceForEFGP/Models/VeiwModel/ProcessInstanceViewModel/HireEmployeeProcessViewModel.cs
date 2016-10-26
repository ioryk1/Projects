using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceForEFGP.Models.VeiwModel.ProcessInstanceViewModel {
    public class HireEmployeeProcessViewModel {

        public class CreateHireEmployeeProcessResult : CommonViewModel.Result {
            public string processSerialNumber { get; set; }

            public CreateHireEmployeeProcessResult() : base() {
                this.processSerialNumber = string.Empty;
            }

        }

        public class CreateHireEmployeeProcessRequestParameter : ProcessInstanceCommonViewModel.CreateProcessBasicInfo {
            public HireInformation hireInformation { get; set; }
            public Assessment assessment { get; set; }
            public List<string> addActivityEmpId { get; set; }
            public List<AttachmentLink> attachments { get; set; }

            public CreateHireEmployeeProcessRequestParameter() : base() {
                this.hireInformation = new HireInformation();
                this.assessment = new Assessment();
                this.addActivityEmpId = new List<string>();
                this.attachments = new List<AttachmentLink>();
            }
        }

        public class HireInformation {
            public string departmentName { get; set; }
            public string departmentId { get; set; }
            public string vacancyName { get; set; }
            public string workLocation { get; set; }
            public string hireEmployeeName { get; set; }
            public string suggestedLevel { get; set; }
            public string title { get; set; }
            public string workTime { get; set; }
            public string vacancyReason { get; set; }
            public string vacancyType { get; set; }
            public string vacancyEmployeeName { get; set; }
            public string vacancyEmployeeId { get; set; }
            public DateTime vacancyEffectiveDate { get; set; }
            public string vacancyDescription { get; set; }
            public string qualification { get; set; }
            public string schoolName { get; set; }
            public string schoolDepartment { get; set; }
            public string otherSkill { get; set; }
            public string hireProbability { get; set; }
            public string abilityRank { get; set; }
            public string totalWorkYears { get; set; }
            public string totalWorkYearsMemo { get; set; }
            public string relatedJobWorkYears { get; set; }
            public string relatedJobWorkYearsMemo { get; set; }
            public string manageWorkYears { get; set; }
            public string manageWorkYearsMemo { get; set; }
            public string referenceLink { get; set; }
            public List<Grade> grades { get; set; }
            

            public HireInformation() {
                this.departmentName = string.Empty;
                this.departmentId = string.Empty;
                this.vacancyName = string.Empty;
                this.workLocation = string.Empty;
                this.hireEmployeeName = string.Empty;
                this.suggestedLevel = string.Empty;
                this.title = string.Empty;
                this.workTime = string.Empty;
                this.vacancyReason = string.Empty;
                this.vacancyType = string.Empty;
                this.vacancyEmployeeName = string.Empty;
                this.vacancyEmployeeId = string.Empty;
                this.vacancyEffectiveDate = DateTime.MinValue;
                this.vacancyDescription = string.Empty;
                this.qualification = string.Empty;
                this.schoolName = string.Empty;
                this.schoolDepartment = string.Empty;
                this.otherSkill = string.Empty;
                this.hireProbability = string.Empty;
                this.totalWorkYears = string.Empty;
                this.totalWorkYearsMemo = string.Empty;
                this.relatedJobWorkYears = string.Empty;
                this.relatedJobWorkYearsMemo = string.Empty;
                this.manageWorkYears = string.Empty;
                this.manageWorkYearsMemo = string.Empty;
                this.referenceLink = string.Empty;
                this.grades = new List<Grade>();                
            }

        }

        public class Grade {
            public string name { get; set; }
            public string score { get; set; }

            public Grade() {
                this.name = string.Empty;
                this.score = string.Empty;

            }
        }

        public class Assessment {
            public string topThreeTask { get; set; }
            public string pastWorkComment { get; set; }
            public List<Function> functions { get; set; }
            public ManagerComment managerComment { get; set; }
            public Assessment() {
                this.topThreeTask = string.Empty;
                this.pastWorkComment = string.Empty;
                this.functions = new List<Function>();
                this.managerComment = new ManagerComment();
            }
        }

        public class Function {
            public bool isCustomItem { get; set; }
            public string customItemName { get; set; }
            public string customItemDescription { get; set; }
            public string assessmentId { get; set; }
            public int score { get; set; }

            public Function() {
                this.isCustomItem = false;
                this.customItemName = string.Empty;
                this.customItemDescription = string.Empty;
                this.assessmentId = string.Empty;
                this.score = 0;
            }

        }

        public class ManagerComment {
            public string advantage { get; set; }
            public string enhanceAbility { get; set; }
            public DateTime dateMeet { get; set; }
            public ManagerComment() {
                this.advantage = string.Empty;
                this.enhanceAbility = string.Empty;
                this.dateMeet = DateTime.MinValue;
            }
        }

        public class AttachmentLink {
            public string name { get; set; }
            public string url { get; set; }

            public AttachmentLink() {
                this.name = string.Empty;
                this.url = string.Empty;
            }
                
        }
        

        /// <summary>
        /// 
        /// </summary>
        public class CreateProcessValidateParameter {

            public CreateHireEmployeeProcessRequestParameter createParam { get; set; }

            public EmployeeViewModel.EmployeeInfoLight issuerEmp { get; set; }
            public EmployeeViewModel.EmployeeInfoLight applyerEmp { get; set; }
            public EmployeeViewModel.EmployeeInfoLight vacancyEmp { get; set; }
            public List<EmployeeViewModel.EmployeeInfoLight> addActivityEmpList { get; set; }

            public CreateProcessValidateParameter() {
                this.createParam = null;
                this.issuerEmp = null;
                this.applyerEmp = null;
                this.vacancyEmp = null;
                this.addActivityEmpList = new List<EmployeeViewModel.EmployeeInfoLight>();
            }
        }

        public class DeliverHireEmployeeRequestParameter {
            public string processSerialNumber { get; set; }
            public string hireEmployeeName { get; set; }
            public string JobLevel { get; set; }
            public string JobTitle { get; set; }
            public string Salary { get; set; }
            public string SalaryCompare { get; set; }
            public string Note { get; set; }
            public string totalWorkYears { get; set; }
            public string totalWorkYearsMemo { get; set; }
            public string relatedJobWorkYears { get; set; }
            public string relatedJobWorkYearsMemo { get; set; }
            public string manageWorkYears { get; set; }
            public string manageWorkYearsMemo { get; set; }
            public string pastWorkComment { get; set; }            
            public List<DeliverFunction> functions { get; set; }
            public List<DeliverManagerComment> managerComment { get; set; }
            public List<DeliverActivityEmpComment> activityEmpComment { get; set; }

            public DeliverHireEmployeeRequestParameter() {
                this.processSerialNumber = string.Empty;
                this.hireEmployeeName = string.Empty;
                this.JobLevel = string.Empty;
                this.JobTitle = string.Empty;
                this.Salary = string.Empty;
                this.SalaryCompare = string.Empty;
                this.Note = string.Empty;
                this.totalWorkYears = string.Empty;
                this.totalWorkYearsMemo = string.Empty;
                this.relatedJobWorkYears = string.Empty;
                this.relatedJobWorkYearsMemo = string.Empty;
                this.manageWorkYears = string.Empty;
                this.manageWorkYearsMemo = string.Empty;
                this.pastWorkComment = string.Empty;
                this.functions = new List<DeliverFunction>();
                this.managerComment = new List<DeliverManagerComment>();
                this.activityEmpComment = new List<DeliverActivityEmpComment>();
            }

        }

        public class DeliverFunction :Function {
            public string functionType { get; set; }
            public DeliverFunction() : base() {
                this.functionType = string.Empty;
            }
        }

        public class DeliverManagerComment : ManagerComment {
            public string commentType { get; set; }
            public DeliverManagerComment() : base() {
                this.commentType = string.Empty;
            }
        }

        public class DeliverActivityEmpComment {
            public string activityEmpId { get; set; }
            public string activityEmpName { get; set; }
            public string advantage { get; set; }
            public string enhanceAbility { get; set; }
            public string hireProbability { get; set; }
            public string identity { get; set; }
            public string identityDescription { get; set; }
            public DeliverActivityEmpComment() {
                this.activityEmpId = string.Empty;
                this.activityEmpName = string.Empty;
                this.advantage = string.Empty;
                this.enhanceAbility = string.Empty;
                this.hireProbability = string.Empty;
                this.identity = string.Empty;
                this.identityDescription = string.Empty;
            }

        }


        public static Dictionary<string, string> vacancyReasonCodeDic = new Dictionary<string, string>() {
            { "VR01","年度預算計劃內核准新增員額"},
            { "VR02","年度預算計劃外特別增加員額"},
            { "VR03","原有員額出缺補實"},
            { "VR03-01","調動"},
            { "VR03-02","離職"},
            { "VR04","其他，請述明原因"}
        };

        public static Dictionary<string, string> functionCodeDic = new Dictionary<string, string>() {
            { "F01","主動負責" },
            { "F02","團隊合作-發展共同目標" },
            { "F03","團隊合作-溝通與傾聽" },
            { "F04","團隊合作-建立夥伴關係" },
            { "F05","追求卓越-問題分析與解決" },
            { "F06","追求卓越-精益求精" },
            { "F07","勇於創新" },
            { "F08","誠信為本" },
            { "F09","目標管理能力" },
            { "F10","塑造成功團隊" },
            { "F11","指導與發展人才" },
            { "F12","日常決策能力" }
        };

        public static Dictionary<string, string> hireProbabilityCodeDic = new Dictionary<string, string>() {
            {"S01","低於25%" },
            {"S02","26% ~ 50%" },
            {"S03","51% ~ 75%" },
            {"S04","超過75%" },
        };

        public static Dictionary<string, string> abilityRankCodeDic = new Dictionary<string, string>() {
            {"A01","排名前25%" },
            {"A02","排名26% ~ 50%" },
            {"A03","排名51% ~ 75%" },
            {"A04","排名75% 以後" },
        };



    }
}

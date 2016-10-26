using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceForEFGP.Models.VeiwModel {
    public class ProcessViewModel {


        /// <summary>
        /// 簽核歷程
        /// </summary>
        public class ProcessHistory {

            public string serialNumber { get; set; }
            public int currentState { get; set; }
            public string unitID { get; set; }
            public string organizationUnitName { get; set; }

            public string userName { get; set; }
            public string workItemName { get; set; }
            public int WcurrentState { get; set; }
            public string executiveComment { get; set; }
            public DateTime createdTime { get; set; }
            public DateTime completedTime { get; set; }
            public string id { get; set; }
            public string performType { get; set; }
            public string newAssigneeId { get; set; }
            public string newAssigneeName { get; set; }
            public string oldAssigneeId { get; set; }
            public string oldAssigneeName { get; set; }
            public string reassignmentType { get; set; }

            public ProcessHistory() {
                this.serialNumber = "";
                this.currentState = -1;
                this.unitID = "";
                this.organizationUnitName = "";
                this.userName = "";
                this.workItemName = "";
                this.WcurrentState = -1;
                this.executiveComment = "";
                this.createdTime = DateTime.MinValue;
                this.completedTime = DateTime.MinValue;

                this.id = "";
                this.performType = "";
                this.newAssigneeId = "";
                this.newAssigneeName = "";
                this.oldAssigneeId = "";
                this.oldAssigneeName = "";
                this.reassignmentType = "";
            }

        }

        /// <summary>
        /// 簽核歷程回傳結果
        /// </summary>
        public class ProcessHistoryListResult : CommonViewModel.ListResult {
            public List<ProcessHistory> list { get; set; }

            public ProcessHistoryListResult() : base() {
                this.list = new List<ProcessHistory>();

            }

        }

    }
}

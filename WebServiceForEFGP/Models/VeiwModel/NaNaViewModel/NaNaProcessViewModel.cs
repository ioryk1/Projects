using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceForEFGP.Models.VeiwModel.NaNaViewModel {
    public class NaNaProcessViewModel {

        /// <summary>
        /// 加簽所需參數
        /// </summary>
        public class AddActivityParameter {
            /// <summary>
            /// 參與者的識別值(工號,群組的ID)
            /// </summary>
            public string participantID { get; set; }
            /// <summary>
            /// 參與者的OID
            /// </summary>
            public string participantOID { get; set; }
            /// <summary>
            /// 參與者的類別
            /// </summary>
            public string participantType { get; set; }

            /// <summary>
            /// 處理模式
            /// <value>FIRST_GET_FIRST_WIN　多人處理模式</value>
            /// <value>STRATEGY　　分派策略模式</value>
            /// <value>FOR_EACH　　一人一份模式</value>
            /// </summary>
            public string multiUserMode { get; set; }

            /// <summary>
            /// 該關卡的名稱
            /// </summary>
            public string name { get; set; }

            public AddActivityParameter() {
                this.participantID = string.Empty;
                this.participantOID = string.Empty;
                this.participantType = "HUMAN";
                this.name = string.Empty;
                this.multiUserMode = "FOR_EACH";
            }


        }

    }
}
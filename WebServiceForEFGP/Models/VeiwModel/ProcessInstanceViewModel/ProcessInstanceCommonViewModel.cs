using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceForEFGP.Models.VeiwModel.ProcessInstanceViewModel {
    public class ProcessInstanceCommonViewModel {


        /// <summary>
        /// 發起表單基本資料
        /// </summary>
        public class CreateProcessBasicInfo {
            /// <summary>
            /// 填單人
            /// </summary>
            public string issuerId { get; set; }
            /// <summary>
            /// 申請人
            /// </summary>
            public string applyerId { get; set; }

            public string issuerContactPhoneNumner { get; set; }

            public string applyerContactPhoneNumber { get; set; }

            public CreateProcessBasicInfo() {
                this.issuerId = "";
                this.applyerId = "";
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceForEFGP.Models.VeiwModel.ProcessInstanceViewModel;

namespace WebServiceForEFGP.Models.VeiwModel.ProcessInstanceViewModel {
    public class NewEmployeeEvaluationViewModel {

        /// <summary>
        /// 發起新進員工試用期評鑑表參數
        /// </summary>
        public class CreateNewEmployeeEvaluationProcessParameter : ProcessInstanceCommonViewModel.CreateProcessBasicInfo {
            public string evaluationEmpId { get; set; }
            public CreateNewEmployeeEvaluationProcessParameter() : base() {
                this.evaluationEmpId = string.Empty;
            }
        }

        /// <summary>
        /// 發起新進員工試用期評鑑表回傳結果
        /// </summary>
        public class CreateNewEmployeeEvaluationProcessResult : CommonViewModel.Result {

            public CreateNewEmployeeEvaluationProcessResult() : base() {

            }
        }
        
    }
}

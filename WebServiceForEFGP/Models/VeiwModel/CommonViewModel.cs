using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceForEFGP.Models.VeiwModel {
    public class CommonViewModel {

        public class Result {
            public bool success { get; set; }
            public string resultCode { get; set; }
            public string resultMessage { get; set; }
            public string resultException { get; set; }

            public Result() {
                this.success = false;
                this.resultCode = "";
                this.resultMessage = "";
                this.resultException = "";
            }

            
            public Result setResultValue(bool success, string resultCode,string resultMessage,string resultException) {
                this.success = success;
                this.resultCode = resultCode;
                this.resultMessage = resultMessage;
                this.resultException = resultException;
                return this;
            }
        }

        public class ListResult : Result {
            public int pageIndex { get; set; }
            public int pageSize { get; set; } 
            public string orderField { get; set; }
            public bool desc { get; set; }
            public int count { get; set; }

            public ListResult() : base() {
                this.pageIndex = 0;
                this.pageSize = 0;
                this.orderField = "";
                this.desc = true;
                this.count = 0;
            }
        }

        public class ListQueryParameter {
            public string orderField { get; set; }
            public bool desc { get; set; }
            public int pageIndex { get; set; }
            public int pageSize { get; set; }

            public ListQueryParameter() {
                this.orderField = "";
                this.desc = true;
                this.pageIndex = 0;
                this.pageSize = 0;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceForEFGP.Models.VeiwModel {
    public class EmployeeViewModel {

        public class EmployeeInfoLight {
            public string name { get; set; }
            public string id { get; set; }
            public string department { get; set; }
            public string departmentId { get; set; }
            public string extNo { get; set; }
            public string avatar { get; set; }

            public EmployeeInfoLight() {
                this.name = "";
                this.id = "";
                this.department = "";
                this.departmentId = "";
            }
        }


        public class EmployeeInfoBasic : EmployeeInfoLight {
            public string userOID { get; set; }
            public string employeeOID { get; set; }
            public string ldap { get; set; }

            public EmployeeInfoBasic() : base() {
                this.userOID = string.Empty;
                this.employeeOID = string.Empty;
                this.ldap = string.Empty;
            }
        }

        public class EmployeeInfoListQueryParam : CommonViewModel.ListQueryParameter {
            public string keyword { get; set; }
            public string id { get; set; }

            public EmployeeInfoListQueryParam() : base() {
                this.keyword = "";
                this.id = "";
            }
        }

        public class EmployeeInfoListResult : CommonViewModel.ListResult {
            public List<EmployeeInfoLight> list { get; set; }
            public EmployeeInfoListResult() : base() {
                this.list = new List<EmployeeInfoLight>();
            }
        }

        public class EmployeeInfoLightResult : CommonViewModel.Result {
            public EmployeeInfoLight empInfo { get; set; }
            public EmployeeInfoLightResult() : base() {
                this.empInfo = null;
            }
        }

        public class EmployeeInfoBasicResult : CommonViewModel.Result {
            public EmployeeInfoBasic empInfo { get; set; }
            public EmployeeInfoBasicResult() : base() {
                this.empInfo = null;
            }
        }

    }
}

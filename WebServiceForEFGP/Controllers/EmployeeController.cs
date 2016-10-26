using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebServiceForEFGP.Models.Services;
using WebServiceForEFGP.Models.VeiwModel;

namespace WebServiceForEFGP.Controllers
{
    [RoutePrefix("Employee")]
    public class EmployeeController : ApiController {

        private EmployeeSvc empSvc = null;

        public EmployeeController() : base() {
            this.empSvc = new EmployeeSvc();
        }

        [HttpPost]        
        [Route("getEmployeeInfoListResult")]
        public EmployeeViewModel.EmployeeInfoListResult getEmployeeInfoListResult(EmployeeViewModel.EmployeeInfoListQueryParam param) {
            EmployeeViewModel.EmployeeInfoListResult ret = new EmployeeViewModel.EmployeeInfoListResult();

            try {

                ret = this.empSvc.getEmployeeInfoListResult(param);

            } catch (Exception ex) {
                ret.success = false;
                ret.resultCode = "500";
                ret.resultException = ex.ToString();
                    
            }

            return ret; 
        }

    }
}

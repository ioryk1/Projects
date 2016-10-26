using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebServiceForEFGP.Models.Services;
using WebServiceForEFGP.Models.VeiwModel;

namespace WebServiceForEFGP.Controllers {

    [RoutePrefix("AccountItem")]
    public class AccountItemController : ApiController {

        private AccountItemSvc accountItemSvc = null;

        public AccountItemController() : base() {
            this.accountItemSvc = new AccountItemSvc();
        }


        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("getCashApplyTypeList")]
        public AccountItemViewModel.CashApplyTypeListResult getCashApplyTypeList() {
            AccountItemViewModel.CashApplyTypeListResult ret = this.accountItemSvc.getCashApplyTypeList();
            return ret;
        }



        // GET: api/AccountItem
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        // GET: api/AccountItem/5
        public string Get(int id) {
            return "value";
        }

        // POST: api/AccountItem
        public void Post([FromBody]string value) {
        }

        // PUT: api/AccountItem/5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE: api/AccountItem/5
        public void Delete(int id) {
        }
    }
}

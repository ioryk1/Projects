using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebServiceForEFGP.Models;
using WebServiceForEFGP.Models.Services;
using WebServiceForEFGP.Models.VeiwModel;

namespace WebServiceForEFGP.Controllers{

    [RoutePrefix("FormSetting")]
    public class FormSettingController : ApiController {

        private FormOptionSettingSvc formOptionSvc = null;

        public FormSettingController():base() {
            this.formOptionSvc = new FormOptionSettingSvc();            
        }

        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("getFormFieldsListResult/{formID}")]
        public FormOptionsSettingViewModel.FormOptionFieldLightListResult getFormFieldsListResult(string formID) {
            FormOptionsSettingViewModel.FormOptionFieldLightListResult ret = new FormOptionsSettingViewModel.FormOptionFieldLightListResult();
            ret = this.formOptionSvc.getOptionFieldListResultByFormID(formID);            
            return ret;
        }

        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("getNotifyFormFieldListResult/{formID}")]
        public FormOptionsSettingViewModel.FormOptionFieldLightListResult getNotifyFormFieldListResult(string formID) {
            FormOptionsSettingViewModel.FormOptionFieldLightListResult ret = new FormOptionsSettingViewModel.FormOptionFieldLightListResult();
            ret = this.formOptionSvc.getNotifyFieldListResultByFormID(formID);
            return ret;
        }
        
               
        

    }
}

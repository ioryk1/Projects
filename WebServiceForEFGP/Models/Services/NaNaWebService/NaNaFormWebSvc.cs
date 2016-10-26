using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceForEFGP.Models.VeiwModel.NaNaViewModel;

namespace WebServiceForEFGP.Models.Services.NaNaWebService {
    public class NaNaFormWebSvc {

        private NaNa.PLMIntegrationEFGP.PLMIntegrationEFGPService plmSvc = null;

        public NaNaFormWebSvc() {
            this.plmSvc = new NaNa.PLMIntegrationEFGP.PLMIntegrationEFGPService();
        }

        /// <summary>
        /// 取得目前流程掛載的表單OID
        /// </summary>        
        public NaNaFormViewModel.FormTemplateSimpleResult findFormOIDsOfProcess(string processID) {
            NaNaFormViewModel.FormTemplateSimpleResult ret = new NaNaFormViewModel.FormTemplateSimpleResult();

            try {
                ret.formOID = this.plmSvc.findFormOIDsOfProcess(processID);
                ret.success = !string.IsNullOrEmpty(ret.formOID);
                ret.resultCode = "200";
            } catch (Exception ex) {
                ret.success = false;
                ret.resultCode = "500";
                ret.resultException = ex.ToString();
            }

            return ret;
        }

        /// <summary>
        /// 取得表單樣板
        /// </summary>        
        public NaNaFormViewModel.FormTemplateSimpleResult getFormFieldTemplate(string formOID) {
            NaNaFormViewModel.FormTemplateSimpleResult ret = new NaNaFormViewModel.FormTemplateSimpleResult();

            try {                
                ret.resultCode = "200";
                ret.xmlTemplate = this.plmSvc.getFormFieldTemplate(formOID);
                ret.success = !string.IsNullOrEmpty(ret.xmlTemplate);
            } catch (Exception ex) {
                ret.success = false;
                ret.resultCode = "500";
                ret.resultException = ex.ToString();
                                
            }

            return ret;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebServiceForEFGP.Models.VeiwModel.NaNaViewModel {
    public class NaNaFormViewModel {

        public class FormTemplateSimpleResult : CommonViewModel.Result {
            public string xmlTemplate { get; set; }
            public string formOID { get; set; }
            public FormTemplateSimpleResult() : base() {
                this.xmlTemplate = string.Empty;
                this.formOID = string.Empty; 
            }
        }
        
    }
}

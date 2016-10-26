using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using WebServiceForEFGP.Models;


namespace WebServiceForEFGP.WebService {
    /// <summary>
    ///MSPWebSvc 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    [SoapRpcService(Use = System.Web.Services.Description.SoapBindingUse.Literal)]
    public class MSPWebSvc : System.Web.Services.WebService {

        private Models.Services.MSPSvc mspSvc = null; 
        public MSPWebSvc() : base() {
            this.mspSvc = new Models.Services.MSPSvc();
        }

        [WebMethod]
        [SoapRpcMethod(Use = System.Web.Services.Description.SoapBindingUse.Literal)]
        public string HelloWorld() {
            return "Hello World";
        }


        
        [WebMethod]
        [SoapRpcMethod(Use = System.Web.Services.Description.SoapBindingUse.Literal)]
        public bool MSPSubmit(string processSerialNumber) {
            bool ret = false;

            UtilitySvc.writeLog("---------------------------");
            UtilitySvc.writeLog(processSerialNumber);

            try {
                ret = this.mspSvc.MSPSubmit(processSerialNumber);
            } catch (Exception ex) {
                ret = false;
                UtilitySvc.writeLog(ex.ToString());                
            }            

            return ret;
        }       
                
    }
}

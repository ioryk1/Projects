using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using WebServiceForEFGP.Models.VeiwModel;
using WebServiceForEFGP.Models.ProcessInstance;
using WebServiceForEFGP.Models;

namespace WebServiceForEFGP.WebService {
    /// <summary>
    ///HireEmployeeWebSvc 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    [SoapRpcService(Use = System.Web.Services.Description.SoapBindingUse.Literal)]
    public class HireEmployeeWebSvc : System.Web.Services.WebService {

        private HireEmployeeProcess hireEmpSvc = new HireEmployeeProcess();           
             
        /// <summary>
        /// 動態加簽關卡(目前加簽直接寫在開單之後 此Method 停用)
        /// </summary>
        /// <param name="processSerialNumber">流程序號</param>        
        [WebMethod]
        [SoapRpcMethod(Use = System.Web.Services.Description.SoapBindingUse.Literal)]
        public bool DynamaticAddActivity(string processSerialNumber) {
            bool ret = false;
            return ret;
        }



        /// <summary>
        /// 回傳錄用核定表資訊至HR系統
        /// </summary>
        /// <param name="processSerialNumber">流程序號</param>        
        [WebMethod]
        [SoapRpcMethod(Use = System.Web.Services.Description.SoapBindingUse.Literal)]
        public bool postHireEmployeeData(string processSerialNumber) {
            bool ret = false;
            UtilitySvc.writeLog("=========== 回傳錄用核定表資訊至HR系統 Start ===========");
            try {
                CommonViewModel.Result deliveryRet = this.hireEmpSvc.deliverHireEmployeeInfo(processSerialNumber);
                ret = deliveryRet.success;
                UtilitySvc.writeLog(Newtonsoft.Json.JsonConvert.SerializeObject(deliveryRet));
            } catch (Exception ex) {
                ret = false;
                UtilitySvc.writeLog(Newtonsoft.Json.JsonConvert.SerializeObject(ex.ToString()));
            }
            UtilitySvc.writeLog("=========== 回傳錄用核定表資訊至HR系統 End ===========");
            //ret = true;
            return ret;            
        }


    }
}

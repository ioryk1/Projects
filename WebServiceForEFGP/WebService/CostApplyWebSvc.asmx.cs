using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using WebServiceForEFGP.Models;


namespace WebServiceForEFGP.WebService {
    /// <summary>
    ///CostApplyWebSvc 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    [SoapRpcService(Use = System.Web.Services.Description.SoapBindingUse.Literal)]
    public class CostApplyWebSvc : System.Web.Services.WebService {

        private Models.Services.AccountItemSvc actSvc = null;
        public CostApplyWebSvc() : base() {
            this.actSvc = new Models.Services.AccountItemSvc();
        }

        [WebMethod]
        public string HelloWorld() {
            return "Hello World";
        }

        [WebMethod]
        [SoapRpcMethod(Use = System.Web.Services.Description.SoapBindingUse.Literal)]
        public bool CostApply(string processSerialNumber) {
            
            //2016.04.27
            //無論怎麼樣都拋 true 因為到會計待結案關卡會再去捞EP轉入temp table 的 flag 為何
            //而且若是 false 會直接結案

            bool ret = true;

            UtilitySvc.writeLog("-----[零用金,併薪,暫支款沖銷,廠商請款]processSerialNumber:" + processSerialNumber + "---------------------- start:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " --------------------");

            try {
                ret = this.actSvc.CostApply(processSerialNumber);
            } catch (Exception ex) {
                UtilitySvc.writeLog(ex.ToString());
            }
            UtilitySvc.writeLog("-----[零用金,併薪,暫支款沖銷,廠商請款]processSerialNumber:" + processSerialNumber + "---------------------- end:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " --------------------");

            return ret;
        }

        /// <summary>
        /// 暫支款申請
        /// </summary>
        /// <param name="processSerialNumber">流程序號</param>        
        [WebMethod]
        [SoapRpcMethod(Use = System.Web.Services.Description.SoapBindingUse.Literal)]
        public bool TempCostApply(string processSerialNumber) {
            bool ret = false;

            UtilitySvc.writeLog("-----[暫支款申請]processSerialNumber:" + processSerialNumber + "---------------------- start:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " --------------------");
            try {
                ret = this.actSvc.tempCostApply(processSerialNumber);
            } catch (Exception ex) {
                UtilitySvc.writeLog(ex.ToString());
            }

            UtilitySvc.writeLog("-----[暫支款申請]processSerialNumber:" + processSerialNumber + "---------------------- end:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " --------------------");
            return ret;
        }


        /// <summary>
        /// 公出費用報銷單
        /// </summary>
        /// <param name="processSerialNumber">流程序號</param>        
        [WebMethod]
        [SoapRpcMethod(Use = System.Web.Services.Description.SoapBindingUse.Literal)]
        public bool OutsideCostApply(string processSerialNumber) {
            bool ret = false;

            UtilitySvc.writeLog("-----[公出費用報銷單]processSerialNumber:" + processSerialNumber + "---------------------- start:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " --------------------");

            try {
                ret = this.actSvc.OutsideCostApply(processSerialNumber);
            } catch (Exception ex) {                
                UtilitySvc.writeLog(ex.ToString());
            }

            UtilitySvc.writeLog("-----[公出費用報銷單]processSerialNumber:" + processSerialNumber + "---------------------- end:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " --------------------");

            return ret;
        }


    }
}

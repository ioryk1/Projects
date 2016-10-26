using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data.OleDb;
using System.Web.Configuration;

namespace WebServiceForEFGP.WebService {
    /// <summary>
    ///HR_WebService 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    [SoapRpcService(Use = System.Web.Services.Description.SoapBindingUse.Literal)]
    public class HR_WebService : System.Web.Services.WebService {

        [WebMethod]
        public string HelloWorld() {
            return "Hello World";
        }

        [WebMethod]
        [SoapRpcMethod(Use = System.Web.Services.Description.SoapBindingUse.Literal)]
        public string Newemp_Yearabsence(string i_seg_segment_no, string i_ID_NO_sZ, string sn) {
            string StoredProcedureName = "HCP.Xxx_Newemp_Yearabsence";
            try {
                using (OleDbConnection cnn = new OleDbConnection(WebConfigurationManager.ConnectionStrings["HRIS_ConnectionString_VIBO"].ConnectionString)) {
                    cnn.Open();
                    OleDbCommand ocd = new OleDbCommand(StoredProcedureName, cnn);
                    ocd.CommandType = System.Data.CommandType.StoredProcedure;
                    ocd.ExecuteNonQuery();
                    cnn.Close();
                }

                Models.UtilitySvc.writeLog("[Success] Newemp_Yearabsence start");
                Models.UtilitySvc.writeLog("[Success] SN = " + sn);
                Models.UtilitySvc.writeLog("[Success] i_seg_segment_no = " + i_seg_segment_no);
                Models.UtilitySvc.writeLog("[Success] i_ID_NO_sZ = " + i_ID_NO_sZ);
                Models.UtilitySvc.writeLog("[Success] Newemp_Yearabsence End");

                
                return "OK";
            } catch (Exception ee) {

                Models.UtilitySvc.writeLog("[ERROR] Newemp_Yearabsence start");
                Models.UtilitySvc.writeLog("[ERROR] SN = " + sn);
                Models.UtilitySvc.writeLog("[ERROR] i_seg_segment_no = " + i_seg_segment_no);
                Models.UtilitySvc.writeLog("[ERROR] i_ID_NO_sZ = " + i_ID_NO_sZ);
                Models.UtilitySvc.writeLog("[ERROR] Erroe MSG = " + ee.Message.ToString());
                Models.UtilitySvc.writeLog("[ERROR] Newemp_Yearabsence End");

                
                return "Error";
            }
        }
    }


}

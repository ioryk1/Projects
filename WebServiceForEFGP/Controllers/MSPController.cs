using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebServiceForEFGP.Models.Services;
using WebServiceForEFGP.Models;

namespace WebServiceForEFGP.Controllers {

    [RoutePrefix("MSP")]
    public class MSPController : ApiController {

        private MSPSvc mspSvc = null;

        public MSPController() : base() {
            this.mspSvc = new MSPSvc();
        }                


        /// <summary>
        /// MSP 訊息匯出
        /// </summary>
        [HttpPost]        
        [Route("export/messages")]
        public void ExportMessages() {
          HttpContext context = HttpContext.Current;
            string filePath = context.Server.MapPath("~/Template/Export-MSP-Sample.xls");
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                using (MemoryStream st = new MemoryStream()) {

                    try {

                        IWorkbook wb = new HSSFWorkbook(fs);
                        wb = this.mspSvc.exportMessageWrokbook(wb, context);
                        wb.Write(st);
                        context.Response.ClearHeaders();
                        context.Response.Clear();
                        context.Response.AddHeader("content-disposition", String.Format("attachment;filename={0}", "MSP-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"));
                        context.Response.BinaryWrite(st.ToArray());

                    } catch (Exception ex)  {
                        UtilitySvc.writeLog("============ Export/MSP Start =============");
                        UtilitySvc.writeLog("DateTime:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        UtilitySvc.writeLog("Exception:" + ex.ToString());
                        UtilitySvc.writeLog("============ Export/MSP End =============");
                    } finally {
                        st.Dispose();
                        st.Close();
                        fs.Close();
                        context.Response.End();
                    }
                }
            }
        }
    }
}

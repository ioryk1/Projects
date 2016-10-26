using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebServiceForEFGP.Models;
using WebServiceForEFGP.Models.VeiwModel;
using WebServiceForEFGP.Models.VeiwModel.ProcessInstanceViewModel;
using WebServiceForEFGP.Models.Services;
using WebServiceForEFGP.Models.ProcessInstance;
using System.Xml;
using System.IO;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace WebServiceForEFGP.Controllers {
    [RoutePrefix("Process")]
    public class ProcessController : ApiController {

        private ProcessSvc procSvc = null;

        public ProcessController() : base() {
            this.procSvc = new ProcessSvc();

        }

        /// <summary>
        /// 新增新進員工試用期評鑑表
        /// </summary>        
        [HttpPost]
        [Route("Create/NewEmployeeEvaluation")]
        public CommonViewModel.Result CreateNewEmployeeEvaluationProcess
            (NewEmployeeEvaluationViewModel.CreateNewEmployeeEvaluationProcessParameter param) {
            CommonViewModel.Result ret = new CommonViewModel.Result();

            IProcessAction NewEmpEvaAct = new NewEmployeeEvaluationProcess(param);
            this.procSvc.procInstance = NewEmpEvaAct;
            ret = this.procSvc.createProcess();

            return ret;
        }

        /// <summary>
        /// 新增錄用核定表流程
        /// </summary>        
        [HttpPost]
        [Route("Create/HireEmployeeProcess")]
        public CommonViewModel.Result CreatHireEmployeeProcess
            (HireEmployeeProcessViewModel.CreateHireEmployeeProcessRequestParameter param) {
            CommonViewModel.Result ret = new CommonViewModel.Result();
            IProcessAction hireEmpAct = new HireEmployeeProcess(param);
            this.procSvc.procInstance = hireEmpAct;
            ret = this.procSvc.createProcess();            
            return ret;
        }       
        
        
                


        //[HttpPost,HttpGet]
        //[Route("test2")]
        //public void test2() {

        //    //wb = new 
        //    HttpContext context = HttpContext.Current;

        //    if (context.Request.HttpMethod == "POST") {
        //        UtilitySvc.writeLog(string.Join(",", context.Request.Form.AllKeys.ToArray()));
        //    }

            
            
        //    using (FileStream fs = new FileStream(@"D:\wwwroot\WebServiceForEFGP\WebServiceForEFGP\Template\12313.xls",
        //        FileMode.Open, FileAccess.Read)) {
        //        IWorkbook wb = new HSSFWorkbook(fs);
        //        ISheet sheet = wb.GetSheetAt(0);
        //        MemoryStream st = new MemoryStream();

        //        DVConstraint constraint = DVConstraint.CreateExplicitListConstraint(new string[] { "itemA", "itemB", "itemC" });
        //        CellRangeAddressList regions = new CellRangeAddressList(2, 65535, 2, 2);

        //        HSSFDataValidation dataValidate = new HSSFDataValidation(regions, constraint);
        //        sheet.AddValidationData(dataValidate);

        //        //current_row.CreateCell(currentCellIndex).SetCellValue()
        //        //sheet.AddValidationData
        //        //DVConstraint.CreateExplicitListConstraint


        //        wb.Write(st);
        //        context.Response.ClearHeaders();
        //        context.Response.Clear();
        //        context.Response.AddHeader("content-disposition", String.Format("attachment;filename={0}", "MSP-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"));
        //        context.Response.BinaryWrite(st.ToArray());

        //        st.Dispose();
        //        st.Close();                

        //        context.Response.End();

        //    }

        //}



    }
}
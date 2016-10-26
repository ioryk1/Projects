using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Oracle.DataAccess.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebServiceForEFGP.Models;
using WebServiceForEFGP.Models.VeiwModel.ProcessInstanceViewModel;
using WebServiceForEFGP.Models.Services;



namespace WebServiceForEFGP {
    public partial class test : System.Web.UI.Page {        
        
        protected void Page_Load(object sender, EventArgs e) {

            //string json_arr = "[[\"1\",\"2\",\"lklvkmkl\"],[\"4\",\"5\",\"adfasdfasd\"]]";
            //string json_arr = "[['1','2','lklvkmkl'],['4','5','adfasdfasd']]";
            //JArray arr1 = JArray.Parse(json_arr);

            //foreach (JArray arr2 in arr1) {
            //    Response.Write(string.Format("line:{0},{1},{2}", arr2[0].ToString(), arr2[1].ToString(), arr2[2].ToString()));
            //    Response.Write(Environment.NewLine);
            //}


            WebServiceForEFGP.Models.Services.NaNaWebService.NaNaFormWebSvc a = new Models.Services.NaNaWebService.NaNaFormWebSvc();
            WebServiceForEFGP.Models.Services.NaNaWebService.NaNaProcessWebSvc b = new Models.Services.NaNaWebService.NaNaProcessWebSvc();

            //Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(a.findFormOIDsOfProcess("PKG14478293768011_clone")));


            //Response.Write(a.getFormFieldTemplate("4c54f370dcc91004897bde45d6229edf").xmlTemplate);
            //Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(a.getFormFieldTemplate("f37449b0dd2510048997e15475ac88ab").xmlTemplate));

            //Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(a.getFormFieldTemplate(a.findFormOIDsOfProcess("PKG14478293768011_clone").formOID.Split(',')[0])));




            /*
            string a = "4c54f370dcc91004897bde45d6229edf";
            string b = "f37449b0dd2510048997e15475ac88ab";
            */

            //Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(HireEmployeeProcessViewModel.functionCodeDic.Keys.ToList()));

            //var ret = a.findFormOIDsOfProcess("HireEmployeeProcess");
            //Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(ret));

            //Response.Write(Environment.NewLine);
            //Response.Write(Environment.NewLine);
            //Response.Write(Environment.NewLine);
            //Response.Write(Environment.NewLine);
            //Response.Write(Environment.NewLine);
            //string xmlstr = Newtonsoft.Json.JsonConvert.SerializeObject(a.getFormFieldTemplate(ret.formOID).xmlTemplate);
            //Response.Write(xmlstr);
            //UtilitySvc.writeLog(xmlstr);

            //Response.Write(string.Format("/txtInterviewerID{0}", 1));

            //Response.Write(b.getProcessInfo("HireEmployeeProcess00000039")); 


        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebServiceForEFGP.Models.Dao;
using WebServiceForEFGP.Models.VeiwModel;
using WebServiceForEFGP.Models.ProcessInstance;

namespace WebServiceForEFGP.Models.Services {
    public class ProcessSvc {

        public IProcessAction procInstance { get; set; }

        private ProcessDao procDao = null;


        public ProcessSvc() {
            this.procDao = new ProcessDao();
            this.procInstance = null;
        }

        public ProcessViewModel.ProcessHistoryListResult getProcessHistoryByProcessSerialNumber(string serialNumber) {
            ProcessViewModel.ProcessHistoryListResult ret = new ProcessViewModel.ProcessHistoryListResult();

            try {

                var list = this.procDao.getProcessHistoryByProcessSerialNumber(serialNumber);
                list = list.OrderBy(x => x.completedTime).ToList(); //用簽核完成時間正向排序
                ret.list = list;
                ret.success = true;
                ret.resultCode = "200";

            } catch (Exception ex) {
                ret.success = false;
                ret.resultCode = "500";
                ret.resultException = ex.ToString();
            }


            return ret;
        }


        /// <summary>
        /// 發起流程
        /// </summary>        
        public CommonViewModel.Result createProcess() {
            CommonViewModel.Result ret = new CommonViewModel.Result();

            try {
                if (this.procInstance == null) {
                    ret.success = false;
                    ret.resultCode = "301";
                    ret.resultMessage = "not specific process instance";
                    return ret;        
                }

                ret = this.procInstance.create();
                UtilitySvc.writeLog("============ create Process result: ============");
                UtilitySvc.writeLog(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
                UtilitySvc.writeLog(Newtonsoft.Json.JsonConvert.SerializeObject(ret));
            } catch (Exception ex) {
                ret.success = false;
                ret.resultCode = "500";
                ret.resultException = ex.ToString();
                UtilitySvc.writeLog("============ create Process exception: ============");
                UtilitySvc.writeLog(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
                UtilitySvc.writeLog(ex.ToString());
            }

            return ret;
        }

        


        


    }
}
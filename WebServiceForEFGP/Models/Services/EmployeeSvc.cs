using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebServiceForEFGP.Models.Dao;
using WebServiceForEFGP.Models.VeiwModel;

namespace WebServiceForEFGP.Models.Services {
    public class EmployeeSvc {

        private EmployeeDao empDao = null;

        public EmployeeSvc() {
            this.empDao = new EmployeeDao();
        }

        public List<EmployeeViewModel.EmployeeInfoLight> getEmpInfoListByEmpIdArr(List<string> empIdArr) {
            List<EmployeeViewModel.EmployeeInfoLight> ret = new List<EmployeeViewModel.EmployeeInfoLight>();

            ret = this.empDao.getEmpInfoListByEmpIdArr(empIdArr);

            return ret;
        }

        public EmployeeViewModel.EmployeeInfoListResult getEmployeeInfoListResult(EmployeeViewModel.EmployeeInfoListQueryParam param) {
            EmployeeViewModel.EmployeeInfoListResult ret = new EmployeeViewModel.EmployeeInfoListResult();


            Tuple<List<EmployeeViewModel.EmployeeInfoLight>, int> list_ret = this.empDao.getEmpInfoList(param);

            ret.success = true;
            ret.resultCode = "200";
            ret.count = list_ret.Item2;
            ret.list = list_ret.Item1;
            ret.orderField = param.orderField;
            ret.pageIndex = param.pageIndex;
            ret.pageSize = param.pageSize;
            
            
            return ret;
        }


        public CommonViewModel.Result isEmployeeExist(string empId) {
            CommonViewModel.Result ret = new CommonViewModel.Result();

            ret.success = this.empDao.getEmpInfoByEmpId(empId) != null;
            ret.resultCode = "200";

            return ret;
        }

        public EmployeeViewModel.EmployeeInfoLightResult getEmployeeInfoLightByEmpId(string empId) {
            EmployeeViewModel.EmployeeInfoLightResult ret = new EmployeeViewModel.EmployeeInfoLightResult();

            try {                
                ret.success = true;
                ret.resultCode = "200";
                ret.empInfo = this.empDao.getEmpInfoByEmpId(empId);
            } catch (Exception ex) {
                ret.success = false;
                ret.resultCode = "500";
                ret.resultException = ex.ToString();
            }

            return ret;
        }


        public EmployeeViewModel.EmployeeInfoBasicResult getEmployeeInfoBasicByEmpId(string empId) {
            EmployeeViewModel.EmployeeInfoBasicResult ret = new EmployeeViewModel.EmployeeInfoBasicResult();

            try {
                ret.success = true;
                ret.resultCode = "200";
                ret.empInfo = this.empDao.getEmpBasicInfoByEmpId(empId);
            } catch (Exception ex) {
                ret.success = false;
                ret.resultCode = "500";
                ret.resultException = ex.ToString();
            }

            return ret;
        }

    }
}
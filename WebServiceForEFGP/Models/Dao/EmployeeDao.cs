using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceForEFGP.Models.VeiwModel;
using LinqKit;


namespace WebServiceForEFGP.Models.Dao {
    public class EmployeeDao {

        public List<EmployeeViewModel.EmployeeInfoLight> getEmpInfoListByEmpIdArr(List<string> empIdArr) {
            List<EmployeeViewModel.EmployeeInfoLight> ret = new List<EmployeeViewModel.EmployeeInfoLight>();



            using (NaNaEntities db = new NaNaEntities()) {

                var query_list = db.ViewEmployeeLightInfo
                        .Where(x => empIdArr.Contains(x.ID))
                        .ToList();

                foreach (var e in query_list) {
                    ret.Add(new EmployeeViewModel.EmployeeInfoLight() {
                        avatar = "",
                        department = string.IsNullOrEmpty(e.departmentName) ? "" : e.departmentName,
                        departmentId = string.IsNullOrEmpty(e.departmentID) ? "" : e.departmentID,
                        extNo = string.IsNullOrEmpty(e.phoneNumber) ? "" : e.phoneNumber,
                        id = string.IsNullOrEmpty(e.ID) ? "" : e.ID,
                        name = string.IsNullOrEmpty(e.userName) ? "" : e.userName
                    });
                }               

            }
            return ret;
        }

        public Tuple<List<EmployeeViewModel.EmployeeInfoLight>, int> getEmpInfoList(EmployeeViewModel.EmployeeInfoListQueryParam param) {
            Tuple<List<EmployeeViewModel.EmployeeInfoLight>, int> ret = null;

            int count = 0;
            List<EmployeeViewModel.EmployeeInfoLight> list = new List<EmployeeViewModel.EmployeeInfoLight>();

            using (NaNaEntities db = new NaNaEntities()) {
                var predicate = PredicateBuilder.True<ViewEmployeeLightInfo>();

                if (!string.IsNullOrEmpty(param.id)){
                    List<string> idsArr = param.id.Split(',').ToList();
                    predicate = predicate.And(x => idsArr.Contains(x.ID));
                }
                

                if (!string.IsNullOrEmpty(param.keyword)) {

                    predicate = predicate.And(
                        x =>
                        x.ldapid.ToLower().Contains(param.keyword.ToLower()) ||
                        x.phoneNumber.Contains(param.keyword) ||
                        x.userName.Contains(param.keyword) ||
                        x.ID.Contains(param.keyword) ||
                        x.departmentID.Contains(param.keyword) ||
                        x.departmentName.Contains(param.keyword));                    
                }

                if (string.IsNullOrEmpty(param.orderField)) {
                    param.orderField = "ID";
                }

                List<ViewEmployeeLightInfo> queryList = db.ViewEmployeeLightInfo.AsExpandable()
                    .Where(predicate)
                    .OrderBy(param.orderField, param.desc)
                    .Skip((param.pageIndex - 1) * param.pageSize)
                    .Take(param.pageSize).ToList();


                foreach (var e in queryList) {

                    list.Add(new EmployeeViewModel.EmployeeInfoLight() {
                        avatar = "",
                        department = string.IsNullOrEmpty(e.departmentName) ? "" : e.departmentName,
                        departmentId = string.IsNullOrEmpty(e.departmentID) ? "" : e.departmentID,
                        extNo = string.IsNullOrEmpty(e.phoneNumber) ? "" : e.phoneNumber,
                        id = string.IsNullOrEmpty(e.ID) ? "" : e.ID,
                        name = string.IsNullOrEmpty(e.userName) ? "" : e.userName
                    });
                }

                count = db.ViewEmployeeLightInfo.AsExpandable()
                    .Where(predicate)
                    .Count();


                ret = Tuple.Create(list, count);

            }


            return ret;
        }

        public EmployeeViewModel.EmployeeInfoLight getEmpInfoByEmpId(string empID) {
            EmployeeViewModel.EmployeeInfoLight ret = null;

            using (NaNaEntities db = new NaNaEntities()) {
                ViewEmployeeLightInfo qryRet = db.ViewEmployeeLightInfo.FirstOrDefault(x => x.ID == empID);                
                if (qryRet != null) {
                    ret = new EmployeeViewModel.EmployeeInfoLight() {
                        avatar = "",
                        department = qryRet.departmentName ?? "",
                        departmentId = qryRet.departmentID ?? "",
                        extNo = qryRet.phoneNumber ?? "",
                        id = qryRet.ID ?? "",
                        name = qryRet.userName ?? ""
                    };
                }
                
            }           

            return ret;
        }

        public EmployeeViewModel.EmployeeInfoBasic getEmpBasicInfoByEmpId(string empID) {
            EmployeeViewModel.EmployeeInfoBasic ret = null;

            using (NaNaEntities db = new NaNaEntities()) {
                ViewEmployeeLightInfo qryRet = db.ViewEmployeeLightInfo.FirstOrDefault(x => x.ID == empID);
                if (qryRet != null) {
                    ret = new EmployeeViewModel.EmployeeInfoBasic() {
                        avatar = string.Empty,
                        department = qryRet.departmentName ?? string.Empty,
                        departmentId = qryRet.departmentID ?? string.Empty,
                        extNo = qryRet.phoneNumber ?? string.Empty,
                        id = qryRet.ID ?? string.Empty,
                        name = qryRet.userName ?? string.Empty,
                        employeeOID = qryRet.employeeOID ?? string.Empty,
                        ldap = qryRet.ldapid ?? string.Empty,
                        userOID = qryRet.userOID ?? string.Empty
                    };
                }

            }


            return ret;
        }
                

    }
}

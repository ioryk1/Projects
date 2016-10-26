using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceForEFGP.Models.Dao.ProcessInstanceDao {
    public class NewEmployeeEvaluationDao {
        private string NaNaConnectionString = string.Empty;
        private string HCPConnectionString = string.Empty;

        public NewEmployeeEvaluationDao() {
            this.NaNaConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NaNaConnectionString"].ToString();
            this.HCPConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["HCPConnectionString"].ToString();
        }

        public bool isProcessComplete(string empID) {
            bool ret = false;

            using (SqlConnection conn = new SqlConnection(this.NaNaConnectionString)) {
                conn.Open();
                string sql = @"select  count(*)  
                                from ProcessInstance  p  
                                left join Users u on u.OID=p.requesterOID  
                                left join HR_NewEmployee_1 h on h.processSerialNumber = p.serialNumber   
                                where  p.currentState in ('3')  
                                and h.rdbConclusion in ('0', '2')  
                                and h.txtUserID = @empID
                                ";
                using (SqlCommand cmd = new SqlCommand(sql,conn)) {
                    cmd.Parameters.AddWithValue("empID", empID);
                    ret = ((int)cmd.ExecuteScalar()) > 0;
                }
            }

            return ret;
        }

        public bool isProcessing(string empID) {
            bool ret = false;
            
            using (SqlConnection conn = new SqlConnection(this.NaNaConnectionString)) {
                conn.Open();
                string sql = @"select COUNT(*) from ProcessInstance  p  
                                left join Users u on u.OID=p.requesterOID  
                                left join HR_NewEmployee_1 h on h.processSerialNumber = p.serialNumber   
                                where  p.currentState in ('1','2')  
                                and p.processInstanceName like '新進員工試用期評鑑表_1'  
                                and h.txtUserID = @empID";
                using (SqlCommand cmd = new SqlCommand(sql, conn)) {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("empID", empID);
                    ret = ((int)cmd.ExecuteScalar()) > 0;
                }
            }

            return ret;
        }

        //getEmpInfoByID
        public List<string> getEmpInfoByID(string empId) {
            List<string> ret = new List<string>();

            using (SqlConnection conn = new SqlConnection(this.NaNaConnectionString)) {
                conn.Open();
                string sql = @"select  SuperUnitUp.organizationUnitName SuperUnitUpName,  
                            SuperUnit.organizationUnitName SuperUnitName,   
                            Unit.organizationUnitName UnitName, U.id,U.userName, Unit.id   
                            FROM Users U 
                            left join Functions F on F.occupantOID = U.OID 
                            left join OrganizationUnit Unit on Unit.OID = F.organizationUnitOID 
                            left join OrganizationUnit SuperUnit on SuperUnit.OID = Unit.superUnitOID 
                            left join OrganizationUnit SuperUnitUp on SuperUnitUp.OID = SuperUnit.superUnitOID  
                            where F.isMain = 1 and (U.leaveDate = '' or U.leaveDate = null or U.leaveDate is null or U.leaveDate > convert(VARCHAR(100),GETDATE(),111) ) 
                            and U.id = @empId";
                using (SqlCommand cmd = new SqlCommand(sql, conn)) {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("empId", empId);

                    SqlDataReader dr = cmd.ExecuteReader();
                    
                    if (!dr.HasRows) {
                        return ret;
                    }

                    dr.Read();
                    ret.Add(DBNull.Value == dr[0] ? null : dr[0].ToString());
                    ret.Add(DBNull.Value == dr[1] ? null : dr[1].ToString());
                    ret.Add(DBNull.Value == dr[2] ? null : dr[2].ToString());
                    ret.Add(DBNull.Value == dr[3] ? null : dr[3].ToString());
                    ret.Add(DBNull.Value == dr[4] ? null : dr[4].ToString());
                    ret.Add(DBNull.Value == dr[5] ? null : dr[5].ToString());
                }
            }
            return ret;
        }

        //getHCPInfoByID
        public List<string> getHCPInfoByEmpId(string empId) {
            List<string> ret = new List<string>();
            string sql = @"select t.titlename as Title, 
                        TO_CHAR(h.trialdate, 'YYYY/MM/DD') as Trial, 
                        substr(t.suggest_grade,1,1) as hfLevel, 
                        TO_CHAR(h.trialdate - 3, 'YYYY/MM/DD') as MaturityDate1, 
                        TO_CHAR(h.trialdate + 5, 'YYYY/MM/DD') as MaturityDate2,  
                        TO_CHAR(h.trialdate + 1, 'YYYY/MM/DD') as OkayDate, 
                        TO_CHAR( h.INDATE , 'YYYY/MM/DD') as InDate 
                        from hcp.hr_personnel_base h, hcp.hr_title t 
                        where h.id_no_sz = ? and  h.title = t.title";

            using (OleDbConnection cn = new OleDbConnection(this.HCPConnectionString)) {
                using (OleDbCommand cmd = new OleDbCommand(sql, cn)) {
                    cn.Open();
                    cmd.Parameters.AddWithValue(":empId", empId);
                    OleDbDataReader dr = cmd.ExecuteReader();

                    if (!dr.HasRows) {
                        return ret;
                    }                    

                    dr.Read();
                    ret.Add(DBNull.Value == dr[0] ? null : dr[0].ToString());
                    ret.Add(DBNull.Value == dr[1] ? null : dr[1].ToString());
                    ret.Add(DBNull.Value == dr[2] ? null : dr[2].ToString());
                    ret.Add(DBNull.Value == dr[3] ? null : dr[3].ToString());
                    ret.Add(DBNull.Value == dr[4] ? null : dr[4].ToString());
                    ret.Add(DBNull.Value == dr[5] ? null : dr[5].ToString());
                    ret.Add(DBNull.Value == dr[6] ? null : dr[6].ToString());

                }
            }
            return ret;
        }

        //getLastEvaluationProcessByEmpId
        public List<string> getLastEvaluationProcessByEmpId(string empId) {
            List<string> ret = new List<string>();
            using (SqlConnection conn = new SqlConnection(this.NaNaConnectionString)) {
                conn.Open();
                string sql = @"select h.ddl2rdItem ,   
		                         Convert(varchar, dateadd(month, Convert(int, h.ddl2rdItem), h.hfMaturityDate1), 111) as hfMaturityDate1,  
		                         Convert(varchar, dateadd(day, 8, dateadd(month, Convert(int, h.ddl2rdItem), h.hfMaturityDate1)), 111) as hfMaturityDate2,   
		                         Convert(varchar, dateadd(day, 4, dateadd(month, Convert(int, h.ddl2rdItem), h.hfMaturityDate1)), 111) as hfOkayDate,  
		                         Convert(varchar, dateadd(day, 3, dateadd(month, Convert(int, h.ddl2rdItem), h.hfMaturityDate1)), 111) as Trial  
		                         from ProcessInstance  p  
		                         left join Users u on u.OID=p.requesterOID  
		                         left join HR_NewEmployee_1 h on h.processSerialNumber = p.serialNumber   
		                         where  p.currentState in ('3')  
		                         and p.processInstanceName like '新進員工試用期評鑑表_1'  
		                         and h.txtUserID = @empId  
		                         and h.rdbConclusion = 1  
		                         order by h.hfDate desc";

                using (SqlCommand cmd = new SqlCommand(sql, conn)) {
                    cmd.Parameters.AddWithValue("empId", empId);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (!dr.HasRows) {
                        return ret;
                    }
                    dr.Read();
                    ret.Add(DBNull.Value == dr[0] ? null : dr[0].ToString());
                    ret.Add(DBNull.Value == dr[1] ? null : dr[1].ToString());
                    ret.Add(DBNull.Value == dr[2] ? null : dr[2].ToString());
                    ret.Add(DBNull.Value == dr[3] ? null : dr[3].ToString());
                    ret.Add(DBNull.Value == dr[4] ? null : dr[4].ToString());
                }
            }
            return ret;
        }

        //getDecisionLevelByEmpId
        public string getDecisionLevelByEmpId(string empId) {
            string ret = string.Empty;
            using (SqlConnection conn = new SqlConnection(this.NaNaConnectionString)) {
                conn.Open();
                string sql = @"select L.levelValue   
                                from Functions F  
                                inner join Users U on F.occupantOID=U.OID  
                                inner join OrganizationUnit O on F.organizationUnitOID=O.OID  
                                inner join OrganizationUnitLevel L on O.levelOID=L.OID   
                                where U.id = @empId and F.isMain =1";

                using (SqlCommand cmd = new SqlCommand(sql, conn)) {
                    cmd.Parameters.AddWithValue("empId", empId);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (!dr.HasRows) {
                        return ret;
                    }

                    dr.Read();
                    ret = DBNull.Value == dr[0] ? string.Empty : dr[0].ToString();
                }
            }
            return ret;
        }

        public List<string> getDeptInfoByDeptName(string deptName) {
            List<string> ret = new List<string>();
            using (SqlConnection conn = new SqlConnection(this.NaNaConnectionString)) {
                conn.Open();
                string sql = @"select u.userName, u.id, uu.userName, uu.id  
                                from OrganizationUnit o   
                                left join Users u on o.managerOID = u.OID    
                                left join OrganizationUnit oo on oo.OID = o.superUnitOID   
                                left join Users uu on  oo.managerOID = uu.OID  
                                where o.organizationUnitName = @deptName";
                using (SqlCommand cmd = new SqlCommand(sql, conn)) {
                    cmd.Parameters.AddWithValue("deptName", deptName);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (!dr.HasRows) {
                        return ret;
                    }
                    dr.Read();
                    ret.Add(DBNull.Value == dr[0] ? null : dr[0].ToString());
                    ret.Add(DBNull.Value == dr[1] ? null : dr[1].ToString());
                    ret.Add(DBNull.Value == dr[2] ? null : dr[2].ToString());
                    ret.Add(DBNull.Value == dr[3] ? null : dr[3].ToString());                    
                }
            }
            return ret;
        }
    }
}

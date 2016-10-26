//using System.Data.Odbc;
using System.Data.OleDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceForEFGP.Models.VeiwModel;
using LinqKit;

namespace WebServiceForEFGP.Models.Dao {
    public class AccountItemDao {

        private string ERPConnectionString;

        public AccountItemDao() {
            this.ERPConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ERPConnectionString"].ToString();

        }

        private Object thisLock = new Object();


        public bool updateAccountItemTaxRateSetting(long id, Dictionary<string, object> dic) {
            bool ret = false;

            using (NaNaEntities db = new NaNaEntities()) {
                AccountItemTaxRateSetting a = db.AccountItemTaxRateSetting.AsQueryable().First(x => x.id == id);

                if (a != null) {
                    Type cl = a.GetType();

                    foreach (var obj in dic) {

                        if (cl.GetProperty(obj.Key) != null) {
                            cl.GetProperty(obj.Key).SetValue(a, obj.Value);
                        }

                    }

                    db.SaveChanges();
                    ret = true;
                }
            }

            return ret;
        }

        public bool deleteAccountItemTaxRateSetting(long id) {
            bool ret = false;

            using (NaNaEntities db = new NaNaEntities()) {

                AccountItemTaxRateSetting a = db.AccountItemTaxRateSetting.FirstOrDefault(x => x.id == id);

                if (a == null) return false;

                db.AccountItemTaxRateSetting.Remove(a);

                db.SaveChanges();

                ret = true;
            }

            return ret;
        }

        public Tuple<int, List<AccountItemTaxRateSetting>> getAccountItemTaxRateSettingList(AccountItemViewModel.AccountItemTaxRateSettingListQueryParameter param) {
            List<AccountItemTaxRateSetting> list = new List<AccountItemTaxRateSetting>();
            int count = 0;
            Tuple<int, List<AccountItemTaxRateSetting>> ret = null;

            using (NaNaEntities db = new NaNaEntities()) {
                var predicate = PredicateBuilder.True<AccountItemTaxRateSetting>();

                if (!string.IsNullOrEmpty(param.cashApplyTypeKey)) {
                    predicate = predicate.And(x => x.applyTypeKey == param.cashApplyTypeKey);
                }
                if (!param.includeDeleteItem) {
                    predicate = predicate.And(x => !x.deleted);
                }

                //用關鍵字搜尋
                if (!string.IsNullOrEmpty(param.keyword)) {
                    predicate = predicate.And(x =>
                      x.code.Contains(param.keyword) ||
                      x.itemName.Contains(param.keyword) ||
                      x.memo.Contains(param.keyword));
                }

                if (string.IsNullOrEmpty(param.orderField)) {
                    param.orderField = "dataCreated";
                }

                

                list = db.AccountItemTaxRateSetting.AsExpandable()
                    .Where(predicate)
                    .OrderBy(param.orderField, param.desc)
                    .Skip((param.pageIndex - 1) * param.pageSize)
                    .Take(param.pageSize)
                    .ToList();

                count = db.AccountItemTaxRateSetting.AsExpandable()
                    .Where(predicate).Count();

                ret = Tuple.Create(count, list);
            }


            return ret;
        }

        public AccountItemTaxRateSetting addAccountItemTaxRateSetting(AccountItemTaxRateSetting a) {
            AccountItemTaxRateSetting ret = null;

            using (NaNaEntities db = new NaNaEntities()) {
                ret = db.AccountItemTaxRateSetting.Add(a);
                db.SaveChanges();
            }

            return ret;
        }

        public AccountItemTaxRateSetting getAccountItemTaxRateSetting(long id) {
            AccountItemTaxRateSetting ret = null;

            using (NaNaEntities db = new NaNaEntities()) {
                ret = db.AccountItemTaxRateSetting.FirstOrDefault(x => x.id == id);
            }

            return ret;
        }

        public List<AccountItemViewModel.CashApplyType> getCashApplyTypeList() {
            List<AccountItemViewModel.CashApplyType> ret = new List<AccountItemViewModel.CashApplyType>() {
                 new AccountItemViewModel.CashApplyType() { key = "salaryAddon", name = "併薪" },
                 new AccountItemViewModel.CashApplyType() { key = "pettyCash", name = "零用金" },
                 new AccountItemViewModel.CashApplyType() { key = "vendor_apply", name = "廠商請款" },
                 //new AccountItemViewModel.CashApplyType() { key = "tempPayment_apply", name = "暫支款申請" },\

                 //2016.03.16 先隱藏此選項
                 //new AccountItemViewModel.CashApplyType() { key = "tempPayment_writeOff", name = "暫支款沖銷" }   
            };

            return ret;
        }

        /// <summary>
        /// 取得會計科目
        /// </summary>
        /// <returns></returns>
        public List<AccountItemViewModel.XXX_EP_ACC_COMB_V> getAccCombVList(string keyword) {

            List<AccountItemViewModel.XXX_EP_ACC_COMB_V> ret = new List<AccountItemViewModel.XXX_EP_ACC_COMB_V>();

            string selectQueryStr = "select * from  APPS.XXX_EP_ACC_COMB_V v where v.ACC_COMB_2_NAME like ? or v.ACC_COMB_2 like ?";

            using (OleDbConnection cn = new OleDbConnection(this.ERPConnectionString)) {

                using (OleDbCommand cmd = new OleDbCommand(selectQueryStr, cn)) {

                    ///cmd.Parameters.Add("", keyword.Trim());
                    cn.Open();

                    cmd.Parameters.AddWithValue(":keyword1", "%" + keyword.Trim() + "%");
                    cmd.Parameters.AddWithValue(":keyword2", "%" + keyword.Trim() + "%");

                    using (var reader = cmd.ExecuteReader()) {

                        while (reader.Read()) {
                            ret.Add(new AccountItemViewModel.XXX_EP_ACC_COMB_V() {
                                ACC_COMB_2 = reader["ACC_COMB_2"].ToString(),
                                ACC_COMB_2_NAME = reader["ACC_COMB_2_NAME"].ToString()
                            });

                        }
                    }
                };

            }
            return ret;
        }

        // Call Procedure
        public CommonViewModel.Result callXXX_EP_AP_INTERFACE_PKG(string  formSerialNumber) {
            CommonViewModel.Result ret = new CommonViewModel.Result();

            using (OleDbConnection cn = new OleDbConnection(this.ERPConnectionString)) {

                using (OleDbCommand cmd = new OleDbCommand()) {
                    cmd.Connection = cn;
                    cn.Open();
                    cmd.CommandText = "APPS.XXX_EP_AP_INT_PKG.CHECK_DATA";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    OleDbParameter P_INV_NUM = new OleDbParameter("@P_INV_NUM", OleDbType.VarChar, 100);
                    OleDbParameter X_RETCODE = new OleDbParameter("@X_RETCODE", OleDbType.VarChar, 100);
                    OleDbParameter X_ERRBUF = new OleDbParameter("@X_ERRBUF", OleDbType.VarChar, 100);

                    P_INV_NUM.Direction = System.Data.ParameterDirection.Input;
                    X_RETCODE.Direction = System.Data.ParameterDirection.Output;
                    X_ERRBUF.Direction = System.Data.ParameterDirection.Output;

                    P_INV_NUM.Value = formSerialNumber;


                    //X_RETCODE.Direction = System.Data.ParameterDirection.ReturnValue;
                    //X_ERRBUF.Direction = System.Data.ParameterDirection.ReturnValue;

                    cmd.Parameters.Add(P_INV_NUM);
                    cmd.Parameters.Add(X_RETCODE);
                    cmd.Parameters.Add(X_ERRBUF);

                    cmd.ExecuteNonQuery();

                    ret.success = true;
                    ret.resultCode = "200";
                    ret.resultMessage = string.Format("XXX_EP_AP_INTERFACE_PKG return value, X_RETCODE = {0} , X_ERRBUF = {1} "
                        , X_RETCODE, X_ERRBUF);


                    UtilitySvc.writeLog(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    UtilitySvc.writeLog("X_RETCODE=" + X_RETCODE.Value);
                    UtilitySvc.writeLog("X_ERRBUF=" + X_ERRBUF.Value);

                };
            }

            return ret;
        }

        // Insert value to temp table
        public CommonViewModel.Result addEP_AP_INVOICE_INTERFACE(AccountItemViewModel.EP_AP_INVOICE_INTERFACE invoice) {
            CommonViewModel.Result ret = new CommonViewModel.Result();

            

            // build sql statement
            string sql = "INSERT INTO XXX.EP_AP_INVOICE_INTERFACE ({fieldArr}) VALUES ({valueArr})";

            List<string> fieldArr = new List<string>();

            List<string> valueArr = new List<string>();

            List<string> ignoreField = new List<string>() {
                "CHECK_FLAG","ERR_MSG","SUCCESS_DATE" ,"IMPORT_FLAG" , "GL_DATE"
            };
            if (invoice.INVOICE_CURRENCY_CODE == "TWD") {
                ignoreField.AddRange(new List<string>() { "EXCHANGE_RATE", "EXCHANGE_RATE_TYPE", "EXCHANGE_DATE" });
            }
               

            Type cl = invoice.GetType();

            foreach (var field in cl.GetProperties()) {

                if (!ignoreField.Contains(field.Name)) {
                    fieldArr.Add(field.Name);
                    valueArr.Add("?");
                }

            }           
                        

            sql = sql.Replace("{fieldArr}", string.Join(", ", fieldArr.ToArray())).Replace("{valueArr}", string.Join(", ", valueArr.ToArray()));

            using (OleDbConnection cn = new OleDbConnection(this.ERPConnectionString)) {

                using (OleDbCommand cmd = new OleDbCommand(sql, cn)) {
                    cn.Open();                    
                   
                    foreach (var field in cl.GetProperties()) {
                        if (!ignoreField.Contains(field.Name)) {
                            cmd.Parameters.AddWithValue(":" + field.Name, field.GetValue(invoice));
                        }
                    }

                    ret.success = cmd.ExecuteNonQuery() > 0;
                    ret.resultCode = "200";
                };
            }

            return ret;
        }

        // Insert value to temp table
        public CommonViewModel.Result addEP_AP_INVOICE_LINES_INTERFACE(AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE invoiceLine) {
            CommonViewModel.Result ret = new CommonViewModel.Result();

            // build sql statement
            string sql = "INSERT INTO XXX.EP_AP_INVOICE_LINES_INTERFACE ({fieldArr}) VALUES ({valueArr})";

            List<string> fieldArr = new List<string>();

            List<string> valueArr = new List<string>();

            List<string> ignoreField = new List<string>() {
                "CHECK_FLAG","ERR_MSG","SUCCESS_DATE" ,"IMPORT_FLAG","ACCOUNTING_DATE"
            };

            Type cl = invoiceLine.GetType();

            foreach (var field in cl.GetProperties()) {
                if (!ignoreField.Contains(field.Name)) {
                    fieldArr.Add(field.Name);
                    valueArr.Add("?");
                }
            }

            sql = sql.Replace("{fieldArr}", string.Join(", ", fieldArr.ToArray())).Replace("{valueArr}", string.Join(", ", valueArr.ToArray()));

            using (OleDbConnection cn = new OleDbConnection(this.ERPConnectionString)) {

                using (OleDbCommand cmd = new OleDbCommand(sql, cn)) {
                    cn.Open();
                    foreach (var field in cl.GetProperties()) {
                        if (!ignoreField.Contains(field.Name)) {
                            cmd.Parameters.AddWithValue(":" + field.Name, field.GetValue(invoiceLine));
                        }
                    }


                    ret.success = cmd.ExecuteNonQuery() > 0;
                    ret.resultCode = "200";

                };
            }

            return ret;
        }


        //Insert value to temp table
        public CommonViewModel.Result addXXX_EP_INT_INV_GV(AccountItemViewModel.XXX_EP_INT_INV_GVP ep_int_inv) {
            CommonViewModel.Result ret = new CommonViewModel.Result();

            // build sql statement
            string sql = "INSERT INTO XXX.XXX_EP_INT_INV_GV ({fieldArr}) VALUES ({valueArr})";

            List<string> fieldArr = new List<string>();

            List<string> valueArr = new List<string>();

            List<string> ignoreField = new List<string>() {
                "CLAIM_TAXNO", "ERR_MSG"
            };

            //如果INVOICE TYPE 是空白則用資料庫預設的
            if (string.IsNullOrEmpty(ep_int_inv.INVOICE_TYPE)) {
                ignoreField.Add("INVOICE_TYPE");
            }

            //如果 發票號碼-代號 及  發票號碼-編號 都是空值則不用insert value
            if (string.IsNullOrEmpty(ep_int_inv.GUI_WORD) && ep_int_inv.GUI_NO == null) {
                ignoreField.Add("GUI_WORD");
                ignoreField.Add("GUI_NO");
            }
                        
            /*else {
                ignoreField.Add("OTHER_DESC");
            }*/

            //UtilitySvc.writeLog(Newtonsoft.Json.JsonConvert.SerializeObject(ep_int_inv));


            Type cl = ep_int_inv.GetType();

            foreach (var field in cl.GetProperties()) {
                if (!ignoreField.Contains(field.Name)) {
                    fieldArr.Add(field.Name);
                    valueArr.Add("?");
                }
            }

            sql = sql.Replace("{fieldArr}", string.Join(", ", fieldArr.ToArray())).Replace("{valueArr}", string.Join(", ", valueArr.ToArray()));

            using (OleDbConnection cn = new OleDbConnection(this.ERPConnectionString)) {

                using (OleDbCommand cmd = new OleDbCommand(sql, cn)) {
                    cn.Open();
                    foreach (var field in cl.GetProperties()) {
                        if (!ignoreField.Contains(field.Name)) {
                            cmd.Parameters.AddWithValue(":" + field.Name, field.GetValue(ep_int_inv));
                        }
                    }


                    ret.success = cmd.ExecuteNonQuery() > 0;
                    ret.resultCode = "200";

                };
            }



            return ret; 
        }
            

        public int getEP_AP_INVOICE_INTERFACE_count() {
            int ret = 0;

            lock (this.thisLock) {

                string sql = "select COUNT(*) from XXX.EP_AP_INVOICE_INTERFACE";

                using (OleDbConnection cn = new OleDbConnection(this.ERPConnectionString)) {

                    using (OleDbCommand cmd = new OleDbCommand(sql, cn)) {
                        cn.Open();

                        ret = int.Parse(cmd.ExecuteScalar().ToString());
                    };
                }

            }

            

            return ret;
        }

        public int getEP_AP_INVOICE_INTERFACE_LINES_count() {
            int ret = 0;
            string sql = "select COUNT(*) from XXX.EP_AP_INVOICE_LINES_INTERFACE";

            lock (this.thisLock) {
                using (OleDbConnection cn = new OleDbConnection(this.ERPConnectionString)) {

                    using (OleDbCommand cmd = new OleDbCommand(sql, cn)) {
                        cn.Open();

                        ret = int.Parse(cmd.ExecuteScalar().ToString());
                    };
                }
            }
            
            return ret;
        }

        public int getXXX_EP_INT_INV_GV_count() {
            int ret = 0;
            string sql = "select COUNT(*) from XXX.XXX_EP_INT_INV_GV";

            lock (this.thisLock) {
                using (OleDbConnection cn = new OleDbConnection(this.ERPConnectionString)) {

                    using (OleDbCommand cmd = new OleDbCommand(sql, cn)) {
                        cn.Open();

                        ret = int.Parse(cmd.ExecuteScalar().ToString());
                    };
                }
            }

            return ret;
        }

        public CostApplyForm getCostApplyForm(string proccessSerialNumber) {
            CostApplyForm ret = null;

            using (NaNaEntities db = new NaNaEntities()) {
                ret = db.CostApplyForm.FirstOrDefault(x => x.processSerialNumber == proccessSerialNumber);
            }


            return ret;
        }

        public List<CostApplyForm_gridNotWithholdList> getCostApplyFormNotWithholdList(string formSerialNumber) {
            List<CostApplyForm_gridNotWithholdList> ret = null;
            using (NaNaEntities db = new NaNaEntities()) {
                ret = db.CostApplyForm_gridNotWithholdList
                    .Where(x => x.formSerialNumber == formSerialNumber).ToList();

                //排序
                ret = ret.OrderBy(x => int.Parse(x.gd_sort)).ToList();
            }
            return ret;
        }

        public List<CostApplyForm_gridWithholdList> getCostApplyFormWithholdList(string formSerialNumber) {
            List<CostApplyForm_gridWithholdList> ret = null;
            using (NaNaEntities db = new NaNaEntities()) {
                ret = db.CostApplyForm_gridWithholdList
                    .Where(x => x.formSerialNumber == formSerialNumber).ToList();

                //排序
                ret = ret.OrderBy(x => int.Parse(x.gd_sort)).ToList();
            }
            return ret;
        }

        public TempPaymentApplyForm getTempPaymentApplyForm(string proccessSerialNumber) {
            TempPaymentApplyForm ret = null;

            using (NaNaEntities db = new NaNaEntities()) {
                ret = db.TempPaymentApplyForm.FirstOrDefault(x => x.processSerialNumber == proccessSerialNumber);
            }

            return ret; 
        }


        public OutsideCostApplyForm getOutsideCostApplyForm(string proccessSerialNumber) {
            OutsideCostApplyForm ret = null;

            using (NaNaEntities db = new NaNaEntities()) {
                ret = db.OutsideCostApplyForm.FirstOrDefault(x => x.processSerialNumber == proccessSerialNumber);                
            }
                             
            return ret;
        }


        public List<OutsideCostApplyForm_gdPrivateTrans> getOutsideCostApplyForm_gdPrivateTransList(string formSerialNumber) {
            List<OutsideCostApplyForm_gdPrivateTrans> ret = new List<OutsideCostApplyForm_gdPrivateTrans>();
            using (NaNaEntities db = new NaNaEntities()) {

                if (db.OutsideCostApplyForm_gdPrivateTrans.Count(x => x.formSerialNumber == formSerialNumber) > 0) {
                    ret = db.OutsideCostApplyForm_gdPrivateTrans
                    .Where(x => x.formSerialNumber == formSerialNumber).ToList();

                    //排序
                    ret = ret.OrderBy(x => int.Parse(x.gd_no)).ToList();
                }

                
            }

            return ret;
        }


        public List<OutsideCostApplyForm_gdStatement> getOutsideCostApplyForm_gdStatementList(string formSerialNumber) {
            List<OutsideCostApplyForm_gdStatement> ret = new List<OutsideCostApplyForm_gdStatement>();

            using (NaNaEntities db = new NaNaEntities()) {
                ret = db.OutsideCostApplyForm_gdStatement
                    .Where(x => x.formSerialNumber == formSerialNumber).ToList();

                //排序
                ret = ret.OrderBy(x => int.Parse(x.gd_no)).ToList();

            }

            return ret;
        }

        /// <summary>
        /// 取得預設的會計科目節段
        /// </summary>
        public AccountItemViewModel.AccountItemSegment  getDefaultAccountItemSegment(AccountItemViewModel.AccountItemEnum actEnum ) {
            AccountItemViewModel.AccountItemSegment ret = null;
            switch (actEnum) {
                case AccountItemViewModel.AccountItemEnum.salaryAddon:
                    ret = new AccountItemViewModel.AccountItemSegment() {
                        seg1 = "02",
                        seg2 = "214120A0",
                        seg3 = "000000",
                        seg4 = "000",
                        seg5 = "000",
                        seg6 = "000"
                    };               
                    break;
                case AccountItemViewModel.AccountItemEnum.pettyCash:
                    ret = new AccountItemViewModel.AccountItemSegment() {
                        seg1 = "02",
                        seg2 = "214120Z0",
                        seg3 = "000000",
                        seg4 = "000",
                        seg5 = "000",
                        seg6 = "000"
                    };
                    break;
                case AccountItemViewModel.AccountItemEnum.vendor_apply:
                    ret = new AccountItemViewModel.AccountItemSegment() {

                        seg1 = "02",
                        seg2 = "214120Z0",
                        seg3 = "000000",
                        seg4 = "000",
                        seg5 = "000",
                        seg6 = "000"
                    };
                    break;
                case AccountItemViewModel.AccountItemEnum.tempPayment_writeOff:
                    ret = new AccountItemViewModel.AccountItemSegment() {

                        seg1 = "02",
                        seg2 = "214120Z0",
                        seg3 = "000000",
                        seg4 = "000",
                        seg5 = "000",
                        seg6 = "000"
                    };
                    break;
                case AccountItemViewModel.AccountItemEnum.tax:
                    ret = new AccountItemViewModel.AccountItemSegment() {
                        seg1 = "02",
                        seg2 = "12642010",
                        seg3 = "000000",
                        seg4 = "000",
                        seg5 = "000",
                        seg6 = "000"

                    };
                    break;
                case AccountItemViewModel.AccountItemEnum.secondNHI:
                    ret = new AccountItemViewModel.AccountItemSegment() {
                        seg1 = "02",
                        seg2 = "22812022",
                        seg3 = "000000",
                        seg4 = "000",
                        seg5 = "000",
                        seg6 = "000"
                    };
                    break;
                case AccountItemViewModel.AccountItemEnum.incomeTaxRate:
                    ret = new AccountItemViewModel.AccountItemSegment() {
                        seg1 = "02",
                        seg2 = "22812020",
                        seg3 = "000000",
                        seg4 = "000",
                        seg5 = "000",
                        seg6 = "000"
                    };
                    break;
            }
            return ret;
        }

    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebServiceForEFGP.Models.Dao;
using WebServiceForEFGP.Models.VeiwModel;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace WebServiceForEFGP.Models.Services {
    public class AccountItemSvc {

        private AccountItemDao accountItemDao = null;

        public AccountItemSvc() {
            this.accountItemDao = new AccountItemDao();
        }

        public AccountItemViewModel.AccountItemTaxRateSettingListResult getAccountItemTaxRateSettingListResult(AccountItemViewModel.AccountItemTaxRateSettingListQueryParameter param) {
            AccountItemViewModel.AccountItemTaxRateSettingListResult ret = new AccountItemViewModel.AccountItemTaxRateSettingListResult();

            try {
                Tuple<int, List<AccountItemTaxRateSetting>> tuple_ret = this.accountItemDao.getAccountItemTaxRateSettingList(param);
                ret.list = tuple_ret.Item2;
                ret.count = tuple_ret.Item1;
                ret.pageIndex = param.pageIndex;
                ret.pageSize = param.pageSize;
                ret.success = true;
                ret.resultCode = "200";
            } catch (Exception ex) {
                ret.success = false;
                ret.resultCode = "500";
                ret.resultException = ex.ToString();
            }

            return ret;

        }

        

        public AccountItemViewModel.AccountItemTaxRateSettingResult addAccountItemTaxRateSetting(AccountItemTaxRateSetting a) {
            AccountItemViewModel.AccountItemTaxRateSettingResult ret = new AccountItemViewModel.AccountItemTaxRateSettingResult();
            try {
                ret.success = true;
                ret.resultCode = "200";
                ret.acc = this.accountItemDao.addAccountItemTaxRateSetting(a);
            } catch (Exception ex) {
                ret.success = false;
                ret.resultCode = "500";
                ret.resultException = ex.ToString();
            }
            return ret;
        }

        public AccountItemViewModel.AccountItemTaxRateSettingResult updateAccountItemTaxRateSetting(long id, Dictionary<string, object> dic) {
            AccountItemViewModel.AccountItemTaxRateSettingResult ret = new AccountItemViewModel.AccountItemTaxRateSettingResult();
            try {
                ret.success = this.accountItemDao.updateAccountItemTaxRateSetting(id, dic);
                ret.resultCode = "200";
            } catch (Exception ex) {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";

            }
            return ret;
        }

        public AccountItemViewModel.AccountItemTaxRateSettingResult deleteAccountItemTaxRateSetting(long id) {
            AccountItemViewModel.AccountItemTaxRateSettingResult ret = new AccountItemViewModel.AccountItemTaxRateSettingResult();
            try {

                ret.success = this.accountItemDao.deleteAccountItemTaxRateSetting(id);
                ret.resultCode = "200";
            } catch (Exception ex) {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";

            }
            return ret;
        }

        public AccountItemViewModel.AccountItemTaxRateSettingResult getAccountItemTaxRateSetting(long id) {
            AccountItemViewModel.AccountItemTaxRateSettingResult ret = new AccountItemViewModel.AccountItemTaxRateSettingResult();
            try {
                ret.acc = this.accountItemDao.getAccountItemTaxRateSetting(id);
                ret.success = ret.acc != null;
                ret.resultCode = "200";
            } catch (Exception ex) {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }

            return ret;
        }


        public AccountItemViewModel.CashApplyTypeListResult getCashApplyTypeList() {
            AccountItemViewModel.CashApplyTypeListResult ret = new AccountItemViewModel.CashApplyTypeListResult();

            ret.success = true;
            ret.resultCode = "200";
            ret.list = this.accountItemDao.getCashApplyTypeList();

            return ret;

        }

        public List<AccountItemViewModel.XXX_EP_ACC_COMB_V> getAccCombVList() {
            List<AccountItemViewModel.XXX_EP_ACC_COMB_V> ret = new List<AccountItemViewModel.XXX_EP_ACC_COMB_V>();

            ret = this.accountItemDao.getAccCombVList("");

            return ret;
        }

        public AccountItemViewModel.CostApplyFormInstanceResult getCostApplyFormInstanceResult(string processSerialNumber) {
            AccountItemViewModel.CostApplyFormInstanceResult ret = new AccountItemViewModel.CostApplyFormInstanceResult();

            try {
                ret.instance = this.getCostApplyFormInstance(processSerialNumber);
                ret.success = ret.instance != null && ret.instance.c != null;
                ret.resultCode = "200";
            } catch (Exception ex) {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }

            return ret;

        }

        public AccountItemViewModel.OutsideApplyFormInstanceResult getOutsideApplyFormInstanceResult(string processSerialNumber) {
            AccountItemViewModel.OutsideApplyFormInstanceResult ret = new AccountItemViewModel.OutsideApplyFormInstanceResult();

            try {
                ret.instance = this.getOutsideApplyFormInstance(processSerialNumber);
                ret.resultCode = "200";
                ret.success = true;
            } catch (Exception ex) {
                ret.success = false;
                ret.resultCode = "500";
                ret.resultException = ex.ToString();                
            }

            return ret;

        }

        private AccountItemViewModel.OutsideApplyFormInstance getOutsideApplyFormInstance(string processSerialNumber) {
            AccountItemViewModel.OutsideApplyFormInstance ret = new AccountItemViewModel.OutsideApplyFormInstance();

            
            ret.o = this.accountItemDao.getOutsideCostApplyForm(processSerialNumber);

            if (ret.o != null) {
                ret.o_gd_private_list = this.accountItemDao.getOutsideCostApplyForm_gdPrivateTransList(ret.o.formSerialNumber);
                ret.o_gd_statement_list = this.accountItemDao.getOutsideCostApplyForm_gdStatementList(ret.o.formSerialNumber);
            }


            return ret;
        }

        /// <summary>
        /// 費用申請 --transtion
        /// </summary>
        public bool CostApply(string processSerialNumber) {
            bool ret = false;
            
                        

            //1.get form list
            AccountItemViewModel.CostApplyFormInstance costApplyFormInstance = this.getCostApplyFormInstance(processSerialNumber);


            //2.parse to erp data list
            AccountItemViewModel.InvoiceInstance invoiceObj = this.convertFormDataToERP(costApplyFormInstance);


            //3.insert into list                         
            //add EP_AP_INVOICE_INTERFACE
            //generate ID 
            invoiceObj.header.INVOICE_ID = this.accountItemDao.getEP_AP_INVOICE_INTERFACE_count() + 1;
            CommonViewModel.Result addInvoice_ret = this.accountItemDao.addEP_AP_INVOICE_INTERFACE(invoiceObj.header);

            //add EP_AP_INVOICE_LINES_INTERFACE
            int lines_sort = 1;
            foreach (var d in invoiceObj.details) {
                d.INVOICE_ID = invoiceObj.header.INVOICE_ID;

                //generate ID
                d.INVOICE_LINE_ID = this.accountItemDao.getEP_AP_INVOICE_INTERFACE_LINES_count() + 1;
                d.LINE_NUMBER = lines_sort;

                if (costApplyFormInstance.c.txtRetailId != "") {
                    //若有填寫直營門市則要將直營門市資訊填寫至明細中 ex:3301008 - 中壢中山(服務中心) 請款摘要
                    d.DESCRIPTION = costApplyFormInstance.c.txtRetailId + " - " + costApplyFormInstance.c.btnRetailName + " " + d.DESCRIPTION;
                }


                CommonViewModel.Result addInvoice_lines_ret = this.accountItemDao.addEP_AP_INVOICE_LINES_INTERFACE(d);
                lines_sort++;
            }


            //4. insert to XXX_EP_INT_INV_GV
            foreach (var i in invoiceObj.invoice_list) {
                CommonViewModel.Result addXXX_EP_INT_INV_GV_ret = this.accountItemDao.addXXX_EP_INT_INV_GV(i);
            }


            //5.call procedure
            //invoice_num 為 表單編號
            CommonViewModel.Result callProcedure_ret = this.accountItemDao.callXXX_EP_AP_INTERFACE_PKG(invoiceObj.header.INVOICE_NUM);
            ret = callProcedure_ret.success;
            UtilitySvc.writeLog(JsonConvert.SerializeObject(ret));

            return ret;
        }

        private AccountItemViewModel.CostApplyFormInstance getCostApplyFormInstance(string proccessSerialNumber) {
            AccountItemViewModel.CostApplyFormInstance ret = new AccountItemViewModel.CostApplyFormInstance();

            ret.c = this.accountItemDao.getCostApplyForm(proccessSerialNumber);

            if (ret.c.radioPaymentlbCertificate == "國內統一發票") {
                ret.c_notwithhold_list = this.accountItemDao.getCostApplyFormNotWithholdList(ret.c.formSerialNumber);
                ret.withhold = false;
            } else {
                ret.c_withhold_list = this.accountItemDao.getCostApplyFormWithholdList(ret.c.formSerialNumber);
                ret.withhold = true;
            }

            return ret;
        }

        /// <summary>
        /// 將費用申請轉成ERP要ISNERT的資料
        /// </summary>        
        private AccountItemViewModel.InvoiceInstance convertFormDataToERP(AccountItemViewModel.CostApplyFormInstance costApplyFormInstance) {
            AccountItemViewModel.InvoiceInstance ret = new AccountItemViewModel.InvoiceInstance();

            //header 相關
            ret.header = this.getCostApplyHeader(costApplyFormInstance);


            //detail => incvoice lines
            if (costApplyFormInstance.c.radioPaymentlbCertificate == "國內統一發票") {
                ret.details = this.convertNotWithholdFormDataToERP(costApplyFormInstance.c, costApplyFormInstance.c_notwithhold_list);
            } else {
                ret.details = this.convertWithholdFormDataToERP(costApplyFormInstance.c, costApplyFormInstance.c_withhold_list);
            }

            //invoice list 
            ret.invoice_list = this.parseCostApplyInvoiceList(costApplyFormInstance);

            //如果有 發票日期帶最大的那天
            if (ret.invoice_list.Count > 0) {
                ret.header.INVOICE_DATE = ret.invoice_list.Max(x => x.INVOICE_DATE);
            }


            //header 的 Amount 總額
            Double invoice_amount = 0;
            invoice_amount = ret.details.Sum(x => x.AMOUNT);          
            ret.header.INVOICE_AMOUNT = invoice_amount;         
            
                  

            return ret;
        }
        
        /// <summary>
        /// 將表單不扣繳資料轉成ERP的資料
        /// </summary>        
        private List<AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE> convertNotWithholdFormDataToERP(CostApplyForm c , List<CostApplyForm_gridNotWithholdList> gd_list) {
            List<AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE> ret = new List<AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE>();

            Regex depRegex = new Regex(@"\(.+?\)");

            Double tax_amount = 0;

            Dictionary<string, Double> priceCompareDic = new Dictionary<string, Double>();
            
            if (c.hidDdlApplyCategorySelected == "pettyCash") {

                #region 零用金的話要將所有的細項藉由會科分類加總起來

                foreach (CostApplyForm_gridNotWithholdList gd_obj in gd_list) {

                    string deptCode = depRegex.Match(gd_obj.gd_belongDepartment).Value.Replace("(", "").Replace(")", "");

                    string priceCompareKey = "02" + "-" + gd_obj.gd_accountItemNo + "-" + deptCode + "-" + gd_obj.gd_telSalesMemo + "-" + gd_obj.gd_paymentMemo + "-" + "000";


                    AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE item = null;

                    //是否已經存在列表中
                    if (ret.Exists(x => x.SEGMENT1 == "02" && x.SEGMENT2 == gd_obj.gd_accountItemNo && x.SEGMENT3 == deptCode
                                   && x.SEGMENT4 == gd_obj.gd_telSalesMemo && x.SEGMENT5 == gd_obj.gd_paymentMemo && x.SEGMENT6 == "000")) {

                        //用會科取出
                        item = ret.First(x => x.SEGMENT1 == "02" && x.SEGMENT2 == gd_obj.gd_accountItemNo
                     && x.SEGMENT3 == deptCode && x.SEGMENT4 == gd_obj.gd_telSalesMemo && x.SEGMENT5 == gd_obj.gd_paymentMemo && x.SEGMENT6 == "000");

                        // 2016.04.19 更改為請款未稅金額
                        item.AMOUNT += this.parseDollarStringToFloat(gd_obj.gd_statementAmountWithoutTax);       //加總
                        item.UNIT_PRICE += this.parseDollarStringToFloat(gd_obj.gd_statementAmountWithoutTax);   //加總

                        //請款摘要要以最大金額的為主                        
                        
                        if (priceCompareDic[priceCompareKey] < this.parseDollarStringToFloat(gd_obj.gd_statementAmountWithoutTax)) {
                            //更新該會科的最大筆金額
                            priceCompareDic[priceCompareKey] = this.parseDollarStringToFloat(gd_obj.gd_statementAmountWithoutTax);
                            item.DESCRIPTION = gd_obj.gd_Description;
                        }

                    } else {

                        item = new AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE() {
                            LINE_TYPE_LOOKUP_CODE = "ITEM",
                            AMOUNT = this.parseDollarStringToFloat(gd_obj.gd_statementAmountWithoutTax),
                            ACCOUNTING_DATE = DateTime.Now,  //2016.08.05 INSERT 時略過該欄位
                            DESCRIPTION = gd_obj.gd_Description,
                            TAX_CODE = "",        //非稅額則將 code 帶 空字串
                            UNIT_PRICE = this.parseDollarStringToFloat(gd_obj.gd_statementAmountWithoutTax), //  單價
                            ORG_ID = 7,
                            PRINTED = "",
                            SEGMENT1 = "02",
                            SEGMENT2 = gd_obj.gd_accountItemNo,
                            SEGMENT3 = deptCode,
                            SEGMENT4 = gd_obj.gd_telSalesMemo,
                            SEGMENT5 = gd_obj.gd_paymentMemo,
                            SEGMENT6 = "000",
                            BATCH_ID = c.txtBatchNumber,
                            CREATION_DATE = DateTime.Now
                        };

                        priceCompareDic.Add(priceCompareKey, item.AMOUNT);

                        ret.Add(item);
                    }

                    Double current_tax_amount = this.parseDollarStringToFloat(gd_obj.gd_taxAmount);
                    //若稅額為 0 則不用寫入此項目
                    if (current_tax_amount != 0) {
                        //稅額
                        tax_amount += current_tax_amount;
                    }
                }

                #endregion

            } else {

                #region 其他申請類型 細項則是每筆分開
                //2016.04.19 更改為請款未稅金額
                //請款未稅金額
                foreach (CostApplyForm_gridNotWithholdList gd_obj in gd_list) {

                    string deptCode = depRegex.Match(gd_obj.gd_belongDepartment).Value.Replace("(", "").Replace(")", "");

                    AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE item = new AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE() {
                        LINE_TYPE_LOOKUP_CODE = "ITEM",
                        AMOUNT = this.parseDollarStringToFloat(gd_obj.gd_statementAmountWithoutTax),   // =>請款未稅金額
                        ACCOUNTING_DATE = DateTime.Now,  //2016.08.05 INSERT 時略過該欄位
                        DESCRIPTION = gd_obj.gd_Description,
                        TAX_CODE = "",        //非稅額則將 code 帶 空字串
                        UNIT_PRICE = this.parseDollarStringToFloat(gd_obj.gd_statementAmountWithoutTax), //  單價 =>請款未稅金額
                        ORG_ID = 7,
                        PRINTED = "",
                        SEGMENT1 = "02",
                        SEGMENT2 = gd_obj.gd_accountItemNo,
                        SEGMENT3 = deptCode,
                        SEGMENT4 = gd_obj.gd_telSalesMemo,
                        SEGMENT5 = gd_obj.gd_paymentMemo,
                        SEGMENT6 = "000",
                        BATCH_ID = c.txtBatchNumber,
                        CREATION_DATE = DateTime.Now
                    };

                    ret.Add(item);


                    Double current_tax_amount = this.parseDollarStringToFloat(gd_obj.gd_taxAmount);
                    //若稅額為 0 則不用寫入此項目
                    if (current_tax_amount != 0) {
                        //稅額
                        tax_amount += current_tax_amount;
                    }
                }

               
                #endregion
            }


            //如果有稅額的話則要加入細項
            //稅額為該請款單的全部加總
            if (tax_amount != 0) {

                //稅額的會科節段
                AccountItemViewModel.AccountItemSegment tax_actItemSeg = this.accountItemDao.getDefaultAccountItemSegment(AccountItemViewModel.AccountItemEnum.tax);

                AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE tax = new AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE() {
                    LINE_TYPE_LOOKUP_CODE = "TAX",
                    AMOUNT = tax_amount,
                    ACCOUNTING_DATE = DateTime.Now,  //2016.08.05 INSERT 時略過該欄位
                    DESCRIPTION = "稅率 5%",
                    TAX_CODE = "10006",        //直接寫死
                    UNIT_PRICE = tax_amount, //
                    ORG_ID = 7,
                    PRINTED = "",
                    SEGMENT1 = tax_actItemSeg.seg1,
                    SEGMENT2 = tax_actItemSeg.seg2,
                    SEGMENT3 = tax_actItemSeg.seg3,
                    SEGMENT4 = tax_actItemSeg.seg4,
                    SEGMENT5 = tax_actItemSeg.seg5,
                    SEGMENT6 = tax_actItemSeg.seg6,
                    BATCH_ID = c.txtBatchNumber,
                    CREATION_DATE = DateTime.Now
                };
                ret.Add(tax);
            }

            return ret;       
        }

        /// <summary>
        /// 將表單扣繳資料轉成ERP的資料
        /// </summary>        
        private List<AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE> convertWithholdFormDataToERP(CostApplyForm c, List<CostApplyForm_gridWithholdList> gd_list) {
            List<AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE> ret = new List<AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE>();

            Regex depRegex = new Regex(@"\(.+?\)");


            foreach (CostApplyForm_gridWithholdList gd_obj in gd_list) {

                string deptCode = depRegex.Match(gd_obj.gd_belongDepartment).Value.Replace("(", "").Replace(")", "");

                if (string.IsNullOrEmpty(gd_obj.gd_withholdType)) {
                    gd_obj.gd_withholdType = "";
                }

                bool isFirmWithold = gd_obj.gd_withholdType.IndexOf("廠商負擔") > -1;

                AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE item = null;

                item = new AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE() {
                    LINE_TYPE_LOOKUP_CODE = "ITEM",
                    AMOUNT = this.parseDollarStringToFloat(gd_obj.gd_oriAmount),            //原幣入帳金額
                    ACCOUNTING_DATE = DateTime.Now,  //2016.08.05 INSERT 時略過該欄位
                    DESCRIPTION = gd_obj.gd_Description,
                    TAX_CODE = "",        //
                    UNIT_PRICE = this.parseDollarStringToFloat(gd_obj.gd_oriAmount),        //原幣入帳金額
                    ORG_ID = 7,
                    PRINTED = "",
                    SEGMENT1 = "02",
                    SEGMENT2 = gd_obj.gd_accountItemNo,
                    SEGMENT3 = deptCode,
                    SEGMENT4 = gd_obj.gd_telSalesMemo,
                    SEGMENT5 = gd_obj.gd_paymentMemo,
                    SEGMENT6 = "000",
                    BATCH_ID = c.txtBatchNumber,
                    CREATION_DATE = DateTime.Now
                };

                ret.Add(item);




                //所得稅率  大於 0 才寫入
                Double incomeTaxAmount = this.parseDollarStringToFloat(gd_obj.gd_taxAmount);

                if (incomeTaxAmount != 0) {

                    //所得稅率的會科節段
                    AccountItemViewModel.AccountItemSegment incomeTax_actItemSeg = this.accountItemDao.getDefaultAccountItemSegment(AccountItemViewModel.AccountItemEnum.incomeTaxRate);
                    AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE incomeTax = new AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE() {
                        LINE_TYPE_LOOKUP_CODE = "ITEM",
                        AMOUNT = -1 * this.parseDollarStringToFloat(gd_obj.gd_taxOriAmount), //所得稅為負項
                        ACCOUNTING_DATE = DateTime.Now,  //2016.08.05 INSERT 時略過該欄位
                        DESCRIPTION = "所得稅率 " + gd_obj.gd_incomeTaxRate + "%",
                        TAX_CODE = "",        //
                        UNIT_PRICE = -1 * this.parseDollarStringToFloat(gd_obj.gd_taxOriAmount),     //負項 原幣所得稅
                        ORG_ID = 7,
                        PRINTED = "",
                        SEGMENT1 = incomeTax_actItemSeg.seg1,
                        SEGMENT2 = incomeTax_actItemSeg.seg2,
                        SEGMENT3 = incomeTax_actItemSeg.seg3,
                        SEGMENT4 = incomeTax_actItemSeg.seg4,
                        SEGMENT5 = incomeTax_actItemSeg.seg5,
                        SEGMENT6 = incomeTax_actItemSeg.seg6,
                        BATCH_ID = c.txtBatchNumber,
                        CREATION_DATE = DateTime.Now
                    };
                    ret.Add(incomeTax);
                }

                //外國人沒有二代健保
                if (c.radioIsCitizen == "1" && this.parseDollarStringToFloat(gd_obj.gd_secondNHIAmount) > 0) {

                    AccountItemViewModel.AccountItemSegment secondNHI_actItemSeg = this.accountItemDao.getDefaultAccountItemSegment(AccountItemViewModel.AccountItemEnum.secondNHI);
                    AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE secHNITax = new AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE() {
                        LINE_TYPE_LOOKUP_CODE = "ITEM",
                        AMOUNT = -1 * this.parseDollarStringToFloat(gd_obj.gd_secondNHIOriAmount),       //txtWithhold_secondNHIAmount
                        ACCOUNTING_DATE = DateTime.Now,  //2016.08.05 INSERT 時略過該欄位
                        DESCRIPTION = "二代健保費率 " + gd_obj.gd_secondNHITaxRate + "%",
                        TAX_CODE = "",        //
                        UNIT_PRICE = -1 * this.parseDollarStringToFloat(gd_obj.gd_secondNHIOriAmount),   //負項  原幣二代健保
                        ORG_ID = 7,
                        PRINTED = "",
                        SEGMENT1 = secondNHI_actItemSeg.seg1,
                        SEGMENT2 = secondNHI_actItemSeg.seg2,
                        SEGMENT3 = secondNHI_actItemSeg.seg3,
                        SEGMENT4 = secondNHI_actItemSeg.seg4,
                        SEGMENT5 = secondNHI_actItemSeg.seg5,
                        SEGMENT6 = secondNHI_actItemSeg.seg6,
                        BATCH_ID = c.txtBatchNumber,
                        CREATION_DATE = DateTime.Now
                    };

                    ret.Add(secHNITax);
                }
            }

            return ret;
        }    

        /// <summary>
        /// 取得費用申請的ERP Invoice Header Object
        /// invoice_amount
        /// </summary>s        
        private AccountItemViewModel.EP_AP_INVOICE_INTERFACE getCostApplyHeader(AccountItemViewModel.CostApplyFormInstance costApplyFormInstance) {
            AccountItemViewModel.EP_AP_INVOICE_INTERFACE ret = new AccountItemViewModel.EP_AP_INVOICE_INTERFACE();

            CostApplyForm c = costApplyFormInstance.c;

            ret.GROUP_ID = c.txtBatchNumber;    //group id 為 batch number
            ret.INVOICE_NUM = c.formSerialNumber; // 2016.03.18 => 更改為 表單編號
            ret.INVOICE_TYPE_LOOKUP_CODE = "STANDARD";
            ret.INVOICE_DATE = DateTime.Now;       //請款日期寫成開此表單的日期 DateTime.Parse(notWithholdData.gd_datePaymentCertificate);   //c.DateNotWithhold_paymentCertificate.Value;
            ret.VENDOR_NUM = c.txtPayeeId;
            ret.VENDOR_NAME = c.txtPayeeName.ToUpper();
            ret.VENDOR_SITE_CODE = ret.VENDOR_NAME;
            ret.INVOICE_AMOUNT = 0;   // 此總金額再 表單資料轉成ERP資料後再做加總 this.parseDollarStringToFloat(notWithholdData.gd_statementAmount); //c.txtNotWithhold_statementAmount
            //ret.INVOICE_CURRENCY_CODE = c.ddlCurrency;
            ret.INVOICE_CURRENCY_CODE = c.hidDdlCurrencySelected;


            //幣別為台幣直接將匯率寫入固定的值
            if (ret.INVOICE_CURRENCY_CODE == "TWD") {
                ret.EXCHANGE_RATE = 0f;
                ret.EXCHANGE_RATE_TYPE = "";
                ret.EXCHANGE_DATE = DateTime.MinValue;
            } else {
                ret.EXCHANGE_RATE = (float)c.txtCurrencyRate;
                ret.EXCHANGE_RATE_TYPE = "Corporate";
                ret.EXCHANGE_DATE = DateTime.Parse(c.txtApplyDate);
            }

            //ret.PAYMENT_METHOD_LOOKUP_CODE = c.ddlPaymentType.ToUpper();
            ret.PAYMENT_METHOD_LOOKUP_CODE = c.hidDdlPaymentTypeSelected.ToUpper();
            //ret.TERMS_NAME = c.ddlPaymentCondition;
            ret.TERMS_NAME = c.hidDdlPaymentConditionSelected.ToUpper();

            ret.DESCRIPTION = c.txtPaymentTitle;
            ret.SOURCE = "EP";
            //2016.08.05 INSERT 時會忽略此欄位
            ret.GL_DATE = DateTime.Now; 
            ret.ORG_ID = 7;
            ret.GL_CODE = "";
            ret.PRINTED = "";

            AccountItemViewModel.AccountItemSegment headerSegment = null;

            //會依照申請類型的不同 ， 表頭的會科節段也會各不相同
            //switch (c.ddlApplyCategory) {
            switch (c.hidDdlApplyCategorySelected) {
                case "salaryAddon":
                    headerSegment = this.accountItemDao.getDefaultAccountItemSegment(AccountItemViewModel.AccountItemEnum.salaryAddon);
                    break;
                case "pettyCash":
                    headerSegment = this.accountItemDao.getDefaultAccountItemSegment(AccountItemViewModel.AccountItemEnum.pettyCash);
                    break;
                case "vendor_apply":
                    headerSegment = this.accountItemDao.getDefaultAccountItemSegment(AccountItemViewModel.AccountItemEnum.vendor_apply);
                    break;
                case "tempPayment_writeOff":
                    headerSegment = this.accountItemDao.getDefaultAccountItemSegment(AccountItemViewModel.AccountItemEnum.tempPayment_writeOff);
                    break;
            }

            ret.SEGMENT1 = headerSegment.seg1;
            ret.SEGMENT2 = headerSegment.seg2;
            ret.SEGMENT3 = headerSegment.seg3;
            ret.SEGMENT4 = headerSegment.seg4;
            ret.SEGMENT5 = headerSegment.seg5;
            ret.SEGMENT6 = headerSegment.seg6;

            if (ret.PAYMENT_METHOD_LOOKUP_CODE.ToUpper() == "CHECK") {
                ret.ATTRIBUTE6 = c.txtPayeeName;
            } else {
                ret.ATTRIBUTE6 = "";
            }

            ret.ATTRIBUTE7 = "";
            ret.BATCH_ID = "";
            ret.CREATION_DATE = DateTime.Now;
            ret.ATTRIBUTE9 = c.SerialNumber;    //attr 9 為表單編號




            return ret;
        }

        /// <summary>
        /// 將有千分號的字串轉成float
        /// </summary>        
        private Double parseDollarStringToFloat(string str) {
            Double ret = 0;
            try {

                if (string.IsNullOrEmpty(str)) {
                    str = "0";
                }
                                
                str = str.Replace(",", "");

                ret = Double.Parse(str);


            } catch (Exception ex) {
                ret = 0;
                UtilitySvc.writeLog(ex.ToString());
            }
            

            return ret;
        }

        /// <summary>
        /// 暫支款申請
        /// </summary>
        /// <returns></returns>
        public bool tempCostApply(string processSerialNumber) {
            bool ret = false;

            TempPaymentApplyForm t = this.accountItemDao.getTempPaymentApplyForm(processSerialNumber);

            //1.get header object
            AccountItemViewModel.EP_AP_INVOICE_INTERFACE invoice = this.getTempCostApplyFormHeader(t);
            UtilitySvc.writeLog(Newtonsoft.Json.JsonConvert.SerializeObject(invoice));
            //2.get detail object
            AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE invoice_line = this.getTempCostApplyFormDetail(t);


            //3.generate ID
            invoice.INVOICE_ID = this.accountItemDao.getEP_AP_INVOICE_INTERFACE_count() + 1;
            CommonViewModel.Result addInvoice_ret = this.accountItemDao.addEP_AP_INVOICE_INTERFACE(invoice);

            invoice_line.INVOICE_ID = invoice.INVOICE_ID;
            invoice_line.INVOICE_LINE_ID = this.accountItemDao.getEP_AP_INVOICE_INTERFACE_LINES_count() + 1;
            invoice_line.LINE_NUMBER = 1;
            CommonViewModel.Result addInvoice_lines_ret = this.accountItemDao.addEP_AP_INVOICE_LINES_INTERFACE(invoice_line);

            //4.call procedure
            // invoice num 為表單編號
            CommonViewModel.Result callProcedure_ret = this.accountItemDao.callXXX_EP_AP_INTERFACE_PKG(invoice.INVOICE_NUM);
            ret = callProcedure_ret.success;
            UtilitySvc.writeLog(JsonConvert.SerializeObject(ret));

            return ret;
        }

        /// <summary>
        /// 暫支款申請 表頭
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public AccountItemViewModel.EP_AP_INVOICE_INTERFACE getTempCostApplyFormHeader(TempPaymentApplyForm t) {
            AccountItemViewModel.EP_AP_INVOICE_INTERFACE ret = new AccountItemViewModel.EP_AP_INVOICE_INTERFACE();

            ret.GROUP_ID = t.txtBatchNumber;
            ret.INVOICE_NUM = t.formSerialNumber;
            ret.INVOICE_TYPE_LOOKUP_CODE = "STANDARD";
            ret.INVOICE_DATE = t.DateRequire.Value; //todo 
            ret.VENDOR_NUM = t.txtApplyUnitHandlingId;
            ret.VENDOR_NAME = t.txtApplyUnitHandlingName;
            ret.VENDOR_SITE_CODE = ret.VENDOR_NAME;
            ret.INVOICE_AMOUNT = this.parseDollarStringToFloat(t.txtInterimPaymentAmount);      //請款金額                     
            ret.INVOICE_CURRENCY_CODE = "TWD";
            ret.EXCHANGE_RATE = 0f;
            ret.EXCHANGE_RATE_TYPE = "";
            ret.EXCHANGE_DATE = DateTime.MinValue;

            ret.PAYMENT_METHOD_LOOKUP_CODE = t.radioPaymentType.ToUpper();
            ret.TERMS_NAME = t.hidPaymentCondition; //   付款條件 todo
            ret.DESCRIPTION = t.txtDescription;
            ret.SOURCE = "EP";
            //2016.08.05 INSERT 時會忽略此欄位
            ret.GL_DATE = DateTime.Now; 
            ret.ORG_ID = 7;
            ret.GL_CODE = "";
            ret.PRINTED = "";



            ret.SEGMENT1 = "02";
            ret.SEGMENT2 = "214120Z0";
            ret.SEGMENT3 = "000000";
            ret.SEGMENT4 = "000";
            ret.SEGMENT5 = "000";
            ret.SEGMENT6 = "000";

            if (ret.PAYMENT_METHOD_LOOKUP_CODE.ToUpper() == "CHECK") {
                ret.ATTRIBUTE6 = t.txtRecipient;  //受款人名稱
            } else {
                ret.ATTRIBUTE6 = t.txtBankAccountNo;    //電匯匯款帳號
            }

            ret.ATTRIBUTE7 = "";
            ret.BATCH_ID = "";
            ret.CREATION_DATE = DateTime.Now;
            ret.ATTRIBUTE9 = t.SerialNumber;    //attr 9 為表單編號

            return ret;
        }

        /// <summary>
        /// 暫支款申請 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE getTempCostApplyFormDetail(TempPaymentApplyForm t) {
            AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE ret = new AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE() {
                LINE_TYPE_LOOKUP_CODE = "ITEM",
                AMOUNT = this.parseDollarStringToFloat(t.txtInterimPaymentAmount),
                ACCOUNTING_DATE = DateTime.Now,  //2016.08.05 INSERT 時略過該欄位
                DESCRIPTION = t.txtDescription,
                TAX_CODE = "",        //直接寫死
                UNIT_PRICE = this.parseDollarStringToFloat(t.txtInterimPaymentAmount), //
                ORG_ID = 7,
                PRINTED = "",
                SEGMENT1 = "02",
                SEGMENT2 = "12712010",
                SEGMENT3 = "000000",
                SEGMENT4 = "000",
                SEGMENT5 = "000",
                SEGMENT6 = "000",
                BATCH_ID = t.txtBatchNumber,
                CREATION_DATE = DateTime.Now

            };




            return ret;
        }

        /// <summary>
        /// 公出費用報銷申請
        /// </summary>
        /// <param name="processSerialNumber"></param>
        /// <returns></returns>
        public bool OutsideCostApply(string processSerialNumber) {
            bool ret = false;

            //1.get form data
            OutsideCostApplyForm of = this.accountItemDao.getOutsideCostApplyForm(processSerialNumber);
            List<OutsideCostApplyForm_gdStatement> of_statement_list = this.accountItemDao.getOutsideCostApplyForm_gdStatementList(of.formSerialNumber);
            List<OutsideCostApplyForm_gdPrivateTrans> of_privateTrans_list = this.accountItemDao.getOutsideCostApplyForm_gdPrivateTransList(of.formSerialNumber);

            //2.convert data to erp temp data
            AccountItemViewModel.InvoiceInstance invoiceInstance = this.getOutsideApplyInvoiceInstance(of, of_statement_list, of_privateTrans_list);

            //3.generate id 
            //add EP_AP_INVOICE_INTERFACE

            invoiceInstance.header.INVOICE_ID = this.accountItemDao.getEP_AP_INVOICE_INTERFACE_count() + 1;
            CommonViewModel.Result addInvoice_ret = this.accountItemDao.addEP_AP_INVOICE_INTERFACE(invoiceInstance.header);


            //add EP_AP_INVOICE_LINES_INTERFACE
            int sort = 1;
            foreach (var d in invoiceInstance.details) {
                d.INVOICE_ID = invoiceInstance.header.INVOICE_ID;

                //generate ID
                d.INVOICE_LINE_ID = this.accountItemDao.getEP_AP_INVOICE_INTERFACE_LINES_count() + 1;
                d.LINE_NUMBER = sort;

                //若有填寫直營門市則要將直營門市資訊填寫至明細中 ex:3301008 - 中壢中山(服務中心) 請款摘要                    
                if (!string.IsNullOrEmpty(of.txtRetailId.Trim())) {
                    d.DESCRIPTION = of.txtRetailId + " - " + of.txtRetailName + " " + d.DESCRIPTION;
                }                

                CommonViewModel.Result addInvoice_lines_ret = this.accountItemDao.addEP_AP_INVOICE_LINES_INTERFACE(d);
                sort++;
            }


            //4. insert to XXX_EP_INT_INV_GV
            foreach (var i in invoiceInstance.invoice_list) {
                CommonViewModel.Result addXXX_EP_INT_INV_GV_ret = this.accountItemDao.addXXX_EP_INT_INV_GV(i);
            }


            //5.call procedure
            //invoice_num 為 表單編號
            CommonViewModel.Result callProcedure_ret = this.accountItemDao.callXXX_EP_AP_INTERFACE_PKG(invoiceInstance.header.INVOICE_NUM);
            ret = callProcedure_ret.success;
            UtilitySvc.writeLog(JsonConvert.SerializeObject(ret));



            return ret;
        }

        /// <summary>
        /// 將公出零用金申請轉成ERP的資料
        /// </summary>
        /// <param name="of"></param>
        /// <param name="of_statment_list"></param>
        /// <param name="of_privateTrans_list"></param>
        /// <returns></returns>
        private AccountItemViewModel.InvoiceInstance getOutsideApplyInvoiceInstance(OutsideCostApplyForm of,
            List<OutsideCostApplyForm_gdStatement> of_statment_list,
            List<OutsideCostApplyForm_gdPrivateTrans> of_privateTrans_list) {

            AccountItemViewModel.InvoiceInstance ret = new AccountItemViewModel.InvoiceInstance();
            ret.header = this.parseOutsideCostApplyHeader(of);
            ret.details = this.parseOutsideCostApplyDetailList(of, of_statment_list, of_privateTrans_list);
            ret.invoice_list = this.parseOutsideCostApplyInvoiceList(of, of_statment_list, of_privateTrans_list);

            //發票日期帶最大的
            if (ret.invoice_list.Count > 0) {
                ret.header.INVOICE_DATE = ret.invoice_list.Max(x => x.INVOICE_DATE);
            }
            

            //header 的 Amount 總額
            Double invoice_amount = 0;
            invoice_amount = ret.details.Sum(x => x.AMOUNT);
            ret.header.INVOICE_AMOUNT = invoice_amount;

            return ret;
        }

        /// <summary>
        /// 取得公出費用申請表頭
        /// </summary>
        /// <param name="of"></param>        
        private AccountItemViewModel.EP_AP_INVOICE_INTERFACE parseOutsideCostApplyHeader(OutsideCostApplyForm of) {
            AccountItemViewModel.EP_AP_INVOICE_INTERFACE ret = new AccountItemViewModel.EP_AP_INVOICE_INTERFACE();

            ret.GROUP_ID = of.txtBatchNumber;
            ret.INVOICE_NUM = of.formSerialNumber;
            ret.INVOICE_TYPE_LOOKUP_CODE = "STANDARD";
            ret.INVOICE_DATE = DateTime.Now;    //DateTime.Parse("1970/01/01"); //請款金額 
            ret.VENDOR_NUM = of.txtPayeeId;
            ret.VENDOR_NAME = of.txtPayeeName;
            ret.VENDOR_SITE_CODE = ret.VENDOR_NAME;
            ret.INVOICE_AMOUNT = 0;      //todo                        
            ret.INVOICE_CURRENCY_CODE = "TWD";
            ret.EXCHANGE_RATE = 0f;
            ret.EXCHANGE_RATE_TYPE = "";
            ret.EXCHANGE_DATE = DateTime.MinValue;

            ret.PAYMENT_METHOD_LOOKUP_CODE = of.hidPaymentType.ToUpper();
            ret.TERMS_NAME = of.hidPaymentCondition; //   付款條件 
            ret.DESCRIPTION = of.txtPaymentTitle;
            ret.SOURCE = "EP";
            //2016.08.05 INSERT 時會忽略此欄位
            ret.GL_DATE = DateTime.Now;
            ret.ORG_ID = 7;
            ret.GL_CODE = "";
            ret.PRINTED = "";

            AccountItemViewModel.AccountItemSegment headerSegment = null;

            //會依照申請類型的不同 ， 表頭的會科節段也會各不相同
            switch (of.ddlApplyCategory) {
                case "salaryAddon":
                    headerSegment = this.accountItemDao.getDefaultAccountItemSegment(AccountItemViewModel.AccountItemEnum.salaryAddon);
                    break;
                case "pettyCash":
                    headerSegment = this.accountItemDao.getDefaultAccountItemSegment(AccountItemViewModel.AccountItemEnum.pettyCash);
                    break;
                case "vendor_apply":
                    headerSegment = this.accountItemDao.getDefaultAccountItemSegment(AccountItemViewModel.AccountItemEnum.vendor_apply);
                    break;
                case "tempPayment_writeOff":
                    headerSegment = this.accountItemDao.getDefaultAccountItemSegment(AccountItemViewModel.AccountItemEnum.tempPayment_writeOff);
                    break;
            }
            ret.SEGMENT1 = headerSegment.seg1;
            ret.SEGMENT2 = headerSegment.seg2;
            ret.SEGMENT3 = headerSegment.seg3;
            ret.SEGMENT4 = headerSegment.seg4;
            ret.SEGMENT5 = headerSegment.seg5;
            ret.SEGMENT6 = headerSegment.seg6;
                        
            ret.ATTRIBUTE6 = of.txtPayeeName;  //受款人名稱            

            ret.ATTRIBUTE7 = "";
            ret.BATCH_ID = "";
            ret.CREATION_DATE = DateTime.Now;
            ret.ATTRIBUTE9 = of.SerialNumber;    //attr 9 為表單編號

            return ret;
        }


        /// <summary>
        /// 取得公出費用申請細項
        /// </summary>                
        private List<AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE> parseOutsideCostApplyDetailList(OutsideCostApplyForm of,
            List<OutsideCostApplyForm_gdStatement> of_statment_list,
            List<OutsideCostApplyForm_gdPrivateTrans> of_privateTrans_list) {
            List<AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE> ret = new List<AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE>();

            List<OutsideCostApplyForm_gdStatement> public_of_statement_list = of_statment_list.Where(x => x.gd_TransferType == "大眾交通工具").ToList();
            List<OutsideCostApplyForm_gdStatement> private_of_statement_list = of_statment_list.Where(x => x.gd_TransferType == "私車公用").ToList();


            //稅額
            Double tax_amount = 0;

            //大眾交通工具
            foreach (var s in public_of_statement_list) {
                AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE item = null;

                string seg1 = "02";
                string seg2 = s.gd_AccountItemNo;       //會科代碼
                string seg3 = s.gd_BelongDepartmentId; //部門所屬經費
                string seg4 = "000";            //電信業務
                string seg5 = "000";            //費用註記
                string seg6 = "000";            //其他

                //如果大眾交通工具存在相同會科則合併再一起
                if (ret.Exists(x => x.SEGMENT1 == seg1 && x.SEGMENT2 == seg2 && x.SEGMENT3 == seg3 && x.SEGMENT4 == seg4 && x.SEGMENT5 == seg5 && x.SEGMENT6 == seg6)) {
                    item = ret.First(x => x.DESCRIPTION == "大眾交通工具" && x.SEGMENT1 == seg1 && x.SEGMENT2 == seg2 && x.SEGMENT3 == seg3 && x.SEGMENT4 == seg4 && x.SEGMENT5 == seg5 && x.SEGMENT6 == seg6);

                    //2016.03.18 => 更改為未稅請款金額
                    //item.AMOUNT += this.parseDollarStringToFloat(s.gd_PaymentCertificateWithoutTaxPrice);
                    //item.UNIT_PRICE += this.parseDollarStringToFloat(s.gd_PaymentCertificateWithoutTaxPrice);

                    item.AMOUNT += this.parseDollarStringToFloat(s.gd_StatementAmountWithoutTax);
                    item.UNIT_PRICE += this.parseDollarStringToFloat(s.gd_StatementAmountWithoutTax);

                } else {

                    //2016.03.18 => 更改為未稅請款金額                    

                    item = new AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE() {
                        LINE_TYPE_LOOKUP_CODE = "ITEM",
                        AMOUNT = this.parseDollarStringToFloat(s.gd_StatementAmountWithoutTax),
                        ACCOUNTING_DATE = DateTime.Now, //2016.08.05 INSERT 時略過該欄位
                        DESCRIPTION = "大眾交通工具",
                        TAX_CODE = "",        //非稅額則將 code 帶 空字串
                        UNIT_PRICE = this.parseDollarStringToFloat(s.gd_StatementAmountWithoutTax), //  單價
                        ORG_ID = 7,
                        PRINTED = "",
                        SEGMENT1 = seg1,
                        SEGMENT2 = seg2,
                        SEGMENT3 = seg3,
                        SEGMENT4 = seg4,
                        SEGMENT5 = seg5,
                        SEGMENT6 = seg6,
                        BATCH_ID = of.txtBatchNumber,
                        CREATION_DATE = DateTime.Now
                    };
                    ret.Add(item);

                }

                //稅額
                Double current_tax_amount = this.parseDollarStringToFloat(s.gd_TaxPrice);

                if (current_tax_amount != 0) {
                    tax_amount += current_tax_amount;
                }

            }

            //私車公用 =>如果小計區塊私車公用有值的話
            Double private_sumStatementAmount = this.parseDollarStringToFloat(of.txtPrivateTrans_sumStatementAmount);
            Double private_sumEtagAmount = this.parseDollarStringToFloat(of.txtPrivateTrans_sumETagPrice);
            Double private_sumStatementWithoutTax = this.parseDollarStringToFloat(of.txtPrivateTrans_sumStatementWithoutAmount);

            //若有私車公用的請款金額則要加總一筆
            if (private_sumStatementAmount != 0 || private_sumEtagAmount != 0 || private_of_statement_list.Count > 0) {
                OutsideCostApplyForm_gdStatement private_of = private_of_statement_list.First();
                string private_seg1 = "02";
                string private_seg2 = private_of.gd_AccountItemNo;
                string private_seg3 = private_of.gd_BelongDepartmentId;
                string private_seg4 = "000";
                string private_seg5 = "000";
                string private_seg6 = "000";
                                
                AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE  private_item = new AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE() {
                    LINE_TYPE_LOOKUP_CODE = "ITEM",
                    AMOUNT = private_sumStatementWithoutTax + private_sumEtagAmount,     //未稅請款金額小計 + etag小計
                    ACCOUNTING_DATE = DateTime.Now, //2016.08.05 INSERT 時略過該欄位
                    DESCRIPTION = "私車公用",
                    TAX_CODE = "",        //非稅額則將 code 帶 空字串
                    UNIT_PRICE = private_sumStatementWithoutTax + private_sumEtagAmount, //未稅請款金額小計 + etag小計
                    ORG_ID = 7,
                    PRINTED = "",
                    SEGMENT1 = private_seg1,
                    SEGMENT2 = private_seg2,
                    SEGMENT3 = private_seg3,
                    SEGMENT4 = private_seg4,
                    SEGMENT5 = private_seg5,
                    SEGMENT6 = private_seg6,
                    BATCH_ID = of.txtBatchNumber,
                    CREATION_DATE = DateTime.Now
                };

                ret.Add(private_item);

                //計算稅額
                foreach (var s in private_of_statement_list) {

                    //稅額
                    Double current_tax_amount = this.parseDollarStringToFloat(s.gd_TaxPrice);

                    if (current_tax_amount != 0) {
                        tax_amount += current_tax_amount;
                    }
                }


            }

            //若有稅額則要加一筆稅額
            if (tax_amount != 0) {
                //稅額的會科節段
                AccountItemViewModel.AccountItemSegment tax_actItemSeg = this.accountItemDao.getDefaultAccountItemSegment(AccountItemViewModel.AccountItemEnum.tax);

                AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE tax = new AccountItemViewModel.EP_AP_INVOICE_LINES_INTERFACE() {
                    LINE_TYPE_LOOKUP_CODE = "TAX",
                    AMOUNT = tax_amount,
                    ACCOUNTING_DATE = DateTime.Now,  //2016.08.05 INSERT 時略過該欄位
                    DESCRIPTION = "稅率 5%",
                    TAX_CODE = "10006",        //直接寫死
                    UNIT_PRICE = tax_amount, //
                    ORG_ID = 7,
                    PRINTED = "",
                    SEGMENT1 = tax_actItemSeg.seg1,
                    SEGMENT2 = tax_actItemSeg.seg2,
                    SEGMENT3 = tax_actItemSeg.seg3,
                    SEGMENT4 = tax_actItemSeg.seg4,
                    SEGMENT5 = tax_actItemSeg.seg5,
                    SEGMENT6 = tax_actItemSeg.seg6,
                    BATCH_ID = of.txtBatchNumber,
                    CREATION_DATE = DateTime.Now
                };
                ret.Add(tax);
            }

            return ret;
        }

        /// <summary>
        /// 將費用請領的轉成發票明細
        /// </summary>
        /// <returns></returns>
        private List<AccountItemViewModel.XXX_EP_INT_INV_GVP> parseCostApplyInvoiceList(AccountItemViewModel.CostApplyFormInstance costApplyFormInstance) {
            List<AccountItemViewModel.XXX_EP_INT_INV_GVP> ret = new List<AccountItemViewModel.XXX_EP_INT_INV_GVP>();

            //detail => incvoice lines
            if (costApplyFormInstance.c.radioPaymentlbCertificate == "國內統一發票") {
                //ret.details = this.convertNotWithholdFormDataToERP(costApplyFormInstance.c, costApplyFormInstance.c_notwithhold_list);

                foreach (var w in costApplyFormInstance.c_notwithhold_list) {

                    //如果稅額為 0 則不用拋進發票檔
                    if (this.parseDollarStringToFloat(w.gd_taxAmount) == 0d) {
                        continue;
                    }
                    
                    Regex engRegex = new Regex(@"^[A-Z]{2}$");


                    string prefix_str = w.gd_paymentCertificateNo.Substring(0, 2).ToUpper();
                    string GUI_WORD = "";
                    int? GUI_NO = null;
                    string OTHER_DESC = "";


                    if (engRegex.IsMatch(prefix_str)) {
                        int gui_no_int = 0;
                        //有可能出現三碼字母
                        bool parseSucess = int.TryParse(w.gd_paymentCertificateNo.Substring(2), out gui_no_int);

                        if (parseSucess) {
                            GUI_WORD = prefix_str;
                            GUI_NO = gui_no_int; //第二碼後面的數字
                        }

                    }

                    OTHER_DESC = w.gd_paymentCertificateNo.Trim();  //直接塞憑證號碼 10 碼

                    //2016.03.18
                    //同憑證號碼多張金額請以「加總」方式拋入GV
                    AccountItemViewModel.XXX_EP_INT_INV_GVP item = null;


                    if (ret.Exists(x => x.GUI_WORD == GUI_WORD && x.GUI_NO == GUI_NO && x.OTHER_DESC == OTHER_DESC)) {
                        item = ret.First(x => x.GUI_WORD == GUI_WORD && x.GUI_NO == GUI_NO && x.OTHER_DESC == OTHER_DESC);

                        //同憑證號碼多張金額請以「加總」方式拋入GV
                        item.SALES_AMT = this.parseDollarStringToFloat(w.gd_paymentCertificateWithoutTax) + item.SALES_AMT;
                        item.VAT_IO = this.parseDollarStringToFloat(w.gd_taxAmount) + item.VAT_IO;
                                                
                    } else {
                        item = new AccountItemViewModel.XXX_EP_INT_INV_GVP() {
                            INVOICE_TYPE = "I",
                            FORMAT_TYPE = w.gd_formatCode,
                            EP_INV_NUMBER = costApplyFormInstance.c.SerialNumber,
                            ERP_INV_NUMBER = costApplyFormInstance.c.txtBatchNumber,
                            OCCURED_YEAR = DateTime.Now.Year,
                            OCCURED_MONTH = DateTime.Now.Month,
                            OCCURED_DATE = DateTime.Now,
                            GUI_WORD = GUI_WORD,
                            GUI_NO = GUI_NO,
                            OTHER_DESC = OTHER_DESC,
                            INVOICE_DATE = DateTime.Parse(w.gd_datePaymentCertificate),
                            SALES_AMT = this.parseDollarStringToFloat(w.gd_paymentCertificateWithoutTax),
                            TAX_CODE = "1",
                            VAT_IO = this.parseDollarStringToFloat(w.gd_taxAmount),
                            CUT_CODE = "1",
                            SALER_CODE = costApplyFormInstance.c.txtPayeeId,
                            SALER_NO = w.gd_sellerTaxId,
                            BUYER_NO = "",
                            BATCH_ID = "",
                            IMPORT_FLAG = "A",  //新增
                            CREATION_DATE = DateTime.Now,
                            CREATED_BY = "EP",
                            STATUS_FLAG = "N",  //未處理
                            STATUS_DATE = DateTime.Now,
                            STATUS_UPDATED_BY = "EP"
                        };

                        ret.Add(item);
                    }

                    
                }
            } 


            return ret;
        }


        /// <summary>
        /// 將公出費用請領轉成發票明細
        /// </summary>
        /// <returns></returns>
        private List<AccountItemViewModel.XXX_EP_INT_INV_GVP> parseOutsideCostApplyInvoiceList(OutsideCostApplyForm of,
            List<OutsideCostApplyForm_gdStatement> of_statment_list,
            List<OutsideCostApplyForm_gdPrivateTrans> of_privateTrans_list) {
            List<AccountItemViewModel.XXX_EP_INT_INV_GVP> ret = new List<AccountItemViewModel.XXX_EP_INT_INV_GVP>();

            foreach (var s in of_statment_list) {

               
                //稅額是0的不用丟發票檔
                if (this.parseDollarStringToFloat(s.gd_TaxPrice) == 0d) {
                    continue;                    
                }
                                

                if (string.IsNullOrEmpty(s.gd_PaymentCertificateNo.Trim()) || string.IsNullOrEmpty(s.gd_PublicVATNo)) {
                    //公出單=> 大眾交通工具 統編或是憑證號碼是空的則跳過
                    continue;
                }

                //2016.03.18
                //同憑證號碼多張金額請以「加總」方式拋入GV
                AccountItemViewModel.XXX_EP_INT_INV_GVP item = null;             


                Regex engRegex = new Regex(@"^[A-Z]{2}$");

                string prefix_str = s.gd_PaymentCertificateNo.Substring(0, 2).ToUpper();
                string GUI_WORD = "";
                int? GUI_NO = null;
                string OTHER_DESC = "";

                if (engRegex.IsMatch(prefix_str)) {
                    GUI_WORD = prefix_str;
                    GUI_NO = int.Parse(s.gd_PaymentCertificateNo.Substring(2)); //第二碼後面的數字
                } 
                OTHER_DESC = s.gd_PaymentCertificateNo.Trim();  //直接塞憑證號碼 10 碼

                if (ret.Exists(x => x.GUI_WORD == GUI_WORD && x.GUI_NO == GUI_NO && x.OTHER_DESC == OTHER_DESC)) {
                    //此憑證號碼已經存在列表中
                    item = ret.First(x => x.GUI_WORD == GUI_WORD && x.GUI_NO == GUI_NO && x.OTHER_DESC == OTHER_DESC);

                    //同憑證號碼多張金額請以「加總」方式拋入GV
                    item.SALES_AMT = this.parseDollarStringToFloat(s.gd_PaymentCertificateWithoutTaxPrice) + item.SALES_AMT;
                    item.VAT_IO = this.parseDollarStringToFloat(s.gd_TaxPrice) + item.VAT_IO;

                } else {
                    //新增此憑證項目
                    item = new AccountItemViewModel.XXX_EP_INT_INV_GVP() {
                        FORMAT_TYPE = s.gd_formatCode,
                        EP_INV_NUMBER = of.SerialNumber,
                        ERP_INV_NUMBER = of.txtBatchNumber,
                        OCCURED_YEAR = DateTime.Now.Year,
                        OCCURED_MONTH = DateTime.Now.Month,
                        OCCURED_DATE = DateTime.Now,
                        GUI_WORD = GUI_WORD,
                        GUI_NO = GUI_NO,
                        OTHER_DESC = OTHER_DESC,
                        INVOICE_DATE = DateTime.Parse(s.gd_DatePaymentCertificate_txt),
                        SALES_AMT = this.parseDollarStringToFloat(s.gd_PaymentCertificateWithoutTaxPrice),
                        TAX_CODE = "1",
                        VAT_IO = this.parseDollarStringToFloat(s.gd_TaxPrice),
                        CUT_CODE = "1",
                        SALER_CODE = of.txtPayeeId,
                        SALER_NO = s.gd_PublicVATNo,
                        BUYER_NO = "",
                        BATCH_ID = "",
                        IMPORT_FLAG = "A",  //新增
                        CREATION_DATE = DateTime.Now,
                        CREATED_BY = "EP",
                        STATUS_FLAG = "N",  //未處理
                        STATUS_DATE = DateTime.Now,
                        STATUS_UPDATED_BY = "EP"
                    };

                    ret.Add(item);
                }

            }



            return ret;
        }

    }
}
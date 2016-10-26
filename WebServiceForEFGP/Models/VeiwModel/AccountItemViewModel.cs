using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceForEFGP.Models.VeiwModel {
    public class AccountItemViewModel {

        public class XXX_EP_ACC_COMB_V {
            public string ACC_COMB_2 { get; set; }
            public string ACC_COMB_2_NAME { get; set; }

            public XXX_EP_ACC_COMB_V() {
                this.ACC_COMB_2 = "";
                this.ACC_COMB_2_NAME = "";
            }
        }

        public class CashApplyType {
            public string key { get; set; }
            public string name { get; set; }

            public CashApplyType() {
                this.key = "";
                this.name = "";
            }

        }

        public class CashApplyTypeListResult : CommonViewModel.ListResult {
            public List<CashApplyType> list { get; set; }
            public CashApplyTypeListResult() : base() {
                this.list = new List<CashApplyType>();
            }

        }

        public class AccCombVListResult : CommonViewModel.ListResult {
            public List<XXX_EP_ACC_COMB_V> list { get; set; }
            public AccCombVListResult() : base() {
                this.list = new List<XXX_EP_ACC_COMB_V>();
            }
        }

        public class AccountItemTaxRateSettingResult:CommonViewModel.Result {
            public AccountItemTaxRateSetting acc { get; set; }

            public AccountItemTaxRateSettingResult() : base() {
                this.acc = null;
            }


        }

        public class AccountItemTaxRateSettingListResult : CommonViewModel.ListResult {
            public List<AccountItemTaxRateSetting> list { get; set; }

            public AccountItemTaxRateSettingListResult() : base() {
                this.list = new List<AccountItemTaxRateSetting>();
            }
        }

        public class AccountItemTaxRateSettingListQueryParameter:  CommonViewModel.ListQueryParameter {            

            public string cashApplyTypeKey { get; set; }

            public bool includeDeleteItem { get; set; }
            
            public string keyword { get; set; }            

            public AccountItemTaxRateSettingListQueryParameter() : base() {
                this.cashApplyTypeKey = "";
                this.includeDeleteItem = false;
                this.keyword = "";
            }


        }


        public class EP_AP_INVOICE_INTERFACE {

            /// <summary>
            /// not null
            /// </summary>
            public int INVOICE_ID { get; set; }

            public string INVOICE_NUM { get; set; }
            public string INVOICE_TYPE_LOOKUP_CODE { get; set; }
            public DateTime INVOICE_DATE { get; set; }
            public string VENDOR_NUM { get; set; }
            public string VENDOR_NAME { get; set; }
            public string VENDOR_SITE_CODE { get; set; }
            public Double INVOICE_AMOUNT { get; set; }
            public string INVOICE_CURRENCY_CODE { get; set; }
            public Double EXCHANGE_RATE { get; set; }
            public string EXCHANGE_RATE_TYPE { get; set; }
            public DateTime EXCHANGE_DATE { get; set; }
            public string PAYMENT_METHOD_LOOKUP_CODE { get; set; }
            public string TERMS_NAME { get; set; }
            public string DESCRIPTION { get; set; }
            public string SOURCE { get; set; }
            public DateTime GL_DATE { get; set; }
            public int ORG_ID { get; set; }
            public string GROUP_ID { get; set; }
            public string GL_CODE { get; set; }
            public string PRINTED { get; set; }
            public string CHECK_FLAG { get; set; }
            public string ERR_MSG { get; set; }
            public DateTime SUCCESS_DATE { get; set; }
            public string IMPORT_FLAG { get; set; }
            public string SEGMENT1 { get; set; }
            public string SEGMENT2 { get; set; }
            public string SEGMENT3 { get; set; }
            public string SEGMENT4 { get; set; }
            public string SEGMENT5 { get; set; }
            public string SEGMENT6 { get; set; }
            public string ATTRIBUTE6 { get; set; }
            public string ATTRIBUTE7 { get; set; }
            public string ATTRIBUTE9 { get; set; }
            public string BATCH_ID { get; set; }
            public DateTime CREATION_DATE { get; set; }

            public EP_AP_INVOICE_INTERFACE() {
                this.ATTRIBUTE6 = "";
                this.ATTRIBUTE7 = "";
                this.BATCH_ID = "";
                this.CHECK_FLAG = "";
                this.CREATION_DATE = DateTime.Now;
                this.DESCRIPTION = "";
                this.ERR_MSG = "";
                this.EXCHANGE_DATE = DateTime.MinValue;
                this.EXCHANGE_RATE = 0f;
                this.EXCHANGE_RATE_TYPE = "";
                this.GL_CODE = "";
                this.GL_DATE = DateTime.MinValue;
                this.GROUP_ID = "";
                this.IMPORT_FLAG = "";
                this.INVOICE_AMOUNT = 0f;
                this.INVOICE_CURRENCY_CODE = "";
                this.INVOICE_DATE = DateTime.Now;
                this.INVOICE_ID = 0;
                this.INVOICE_NUM ="";
                this.INVOICE_TYPE_LOOKUP_CODE = "";
                this.ORG_ID = 0;
                this.PAYMENT_METHOD_LOOKUP_CODE = "";
                this.PRINTED = "";
                this.SEGMENT1 = "";
                this.SEGMENT2 = "";
                this.SEGMENT3 = "";
                this.SEGMENT4 = "";
                this.SEGMENT5 = "";
                this.SEGMENT6 = "";
                this.SOURCE = "";
                this.SUCCESS_DATE = DateTime.MinValue;
                this.TERMS_NAME = "";
                this.VENDOR_NAME = "";
                this.VENDOR_NUM = "";
                this.VENDOR_SITE_CODE = "";
                this.ATTRIBUTE9 = "";

            }
        }

        public class EP_AP_INVOICE_LINES_INTERFACE {
            public int INVOICE_ID { get; set; }
            public int INVOICE_LINE_ID { get; set; }
            public int LINE_NUMBER { get; set; }
            public string LINE_TYPE_LOOKUP_CODE { get; set; }
            public Double AMOUNT { get; set; }
            public DateTime ACCOUNTING_DATE { get; set; }
            public string DESCRIPTION { get; set; }
            public string TAX_CODE { get; set; }
            public Double UNIT_PRICE { get; set; }
            public int ORG_ID { get; set; }
            public string PRINTED { get; set; }
            public string CHECK_FLAG { get; set; }
            public string ERR_MSG { get; set; }
            public DateTime SUCCESS_DATE { get; set; }
            public string IMPORT_FLAG { get; set; }
            public string SEGMENT1 { get; set; }
            public string SEGMENT2 { get; set; }
            public string SEGMENT3 { get; set; }
            public string SEGMENT4 { get; set; }
            public string SEGMENT5 { get; set; }
            public string SEGMENT6 { get; set; }
            public string BATCH_ID { get; set; }
            public DateTime CREATION_DATE { get; set; }
            public EP_AP_INVOICE_LINES_INTERFACE() {
                this.ACCOUNTING_DATE = DateTime.MinValue;
                this.AMOUNT = 0f;
                this.BATCH_ID = "";
                this.CHECK_FLAG = "";
                this.CREATION_DATE = DateTime.Now;
                this.DESCRIPTION = "";
                this.ERR_MSG = "";
                this.IMPORT_FLAG = "";
                this.INVOICE_ID = 0;
                this.INVOICE_LINE_ID = 0;
                this.LINE_NUMBER = 0;
                this.LINE_TYPE_LOOKUP_CODE = "";
                this.ORG_ID = 0;
                this.PRINTED = "";
                this.SEGMENT1 = "";
                this.SEGMENT2 = "";
                this.SEGMENT3 = "";
                this.SEGMENT4 = "";
                this.SEGMENT5 = "";
                this.SEGMENT6 = "";
                this.SUCCESS_DATE = DateTime.MinValue;
                this.TAX_CODE = "";
                this.UNIT_PRICE = 0f; 

                

            }
        }

        public class XXX_EP_INT_INV_GVP {
            public string EP_INV_NUMBER { get; set; }
            public string ERP_INV_NUMBER { get; set; }
            public string INVOICE_TYPE { get; set; }
            public string FORMAT_TYPE { get; set; }
            public int? CLAIM_TAXNO { get; set; }
            public int OCCURED_YEAR { get; set; }
            public int OCCURED_MONTH { get; set; }
            public DateTime? OCCURED_DATE { get; set; }
            public string GUI_WORD { get; set; }
            public int? GUI_NO { get; set; }
            public string OTHER_DESC { get; set; }
            public DateTime INVOICE_DATE { get; set; }
            public Double SALES_AMT { get; set; }
            public string TAX_CODE { get; set; }
            public Double VAT_IO { get; set; }
            public string CUT_CODE { get; set; }
            public string SALER_CODE { get; set; }
            public string SALER_NO { get; set; }
            public string BUYER_NO { get; set; }
            public string BATCH_ID { get; set; }
            public string IMPORT_FLAG { get; set; }
            public DateTime CREATION_DATE { get; set; }
            public string CREATED_BY { get; set; }
            public string STATUS_FLAG { get; set; }
            public DateTime STATUS_DATE { get; set; }
            public string STATUS_UPDATED_BY { get; set; }
            public string ERR_MSG { get; set; }


            public XXX_EP_INT_INV_GVP() {
                this.EP_INV_NUMBER = "";
                this.ERP_INV_NUMBER = "";
                this.INVOICE_TYPE = "";
                this.FORMAT_TYPE = "";
                this.CLAIM_TAXNO = null;
                this.OCCURED_YEAR = -1;
                this.OCCURED_MONTH = -1;
                this.OCCURED_DATE = null;
                this.GUI_WORD = "";
                this.GUI_NO = -1;
                this.OTHER_DESC = "";
                this.INVOICE_DATE = DateTime.MinValue;
                this.SALES_AMT = 0f;
                this.TAX_CODE = "1";
                this.VAT_IO = 0f;
                this.CUT_CODE = "1";
                this.SALER_CODE = "";
                this.SALER_NO = "";
                this.BUYER_NO = "";
                this.BATCH_ID = "";
                this.IMPORT_FLAG = "A";
                this.CREATION_DATE = DateTime.Now;
                this.CREATED_BY = "EP";
                this.STATUS_FLAG = "N";
                this.STATUS_DATE = DateTime.MinValue;
                this.STATUS_UPDATED_BY = "EP";
                this.ERR_MSG = "";
            }

        }

        public class CostApplyFormInstance {
            public CostApplyForm c { get; set; }
            public List<CostApplyForm_gridNotWithholdList> c_notwithhold_list { get; set; }
            public List<CostApplyForm_gridWithholdList> c_withhold_list { get; set; }
            public bool withhold { get; set; }  //是否要扣繳

            public CostApplyFormInstance() {
                this.c = null;
                this.c_notwithhold_list = new List<CostApplyForm_gridNotWithholdList>();
                this.c_withhold_list = new List<CostApplyForm_gridWithholdList>();
            }
        }

        public class CostApplyFormInstanceResult:CommonViewModel.Result {
            public CostApplyFormInstance instance { get; set; }

            public CostApplyFormInstanceResult():base() {
                this.instance = new CostApplyFormInstance();
            }
        }


        public class OutsideApplyFormInstance {
            public OutsideCostApplyForm o { get; set; }
            public List<OutsideCostApplyForm_gdPrivateTrans> o_gd_private_list { get; set; }
            public List<OutsideCostApplyForm_gdStatement> o_gd_statement_list { get; set; }

            public OutsideApplyFormInstance() {
                this.o = null;
                this.o_gd_private_list = new List<OutsideCostApplyForm_gdPrivateTrans>();
                this.o_gd_statement_list = new List<OutsideCostApplyForm_gdStatement>();

            }

        }

        public class OutsideApplyFormInstanceResult : CommonViewModel.Result {
            public OutsideApplyFormInstance instance { get; set; }
            public OutsideApplyFormInstanceResult() : base() {
                this.instance = new OutsideApplyFormInstance();
            }
        }




        public class InvoiceInstance {
            public EP_AP_INVOICE_INTERFACE header { get; set; }

            public List<EP_AP_INVOICE_LINES_INTERFACE> details { get; set; }

            public List<XXX_EP_INT_INV_GVP> invoice_list { get; set; }

            public InvoiceInstance() {
                this.header = null;
                this.details = new List<EP_AP_INVOICE_LINES_INTERFACE>();
                this.invoice_list = new List<XXX_EP_INT_INV_GVP>();
            }

        }


        /// <summary>
        /// 會計科目節段
        /// </summary>
        public class AccountItemSegment {
            public string seg1 { get; set; }
            public string seg2 { get; set; }
            public string seg3 { get; set; }
            public string seg4 { get; set; }
            public string seg5 { get; set; }
            public string seg6 { get; set; }

            public AccountItemSegment() {
                this.seg1 = "";
                this.seg2 = "";
                this.seg3 = "";
                this.seg4 = "";
                this.seg5 = "";
                this.seg6 = "";
            }                 
        }


        public enum AccountItemEnum {

            /// <summary>
            /// 併薪
            /// </summary>
            salaryAddon = 0,
            /// <summary>
            /// 零用金
            /// </summary>
            pettyCash=  1 ,
            /// <summary>
            /// 廠商請款
            /// </summary>
            vendor_apply = 2,
            /// <summary>
            /// 暫支款沖銷
            /// </summary>
            tempPayment_writeOff = 3,
            /// <summary>
            /// 進項稅
            /// </summary>
            tax = 4,
            /// <summary>
            /// 二代健保
            /// </summary>
            secondNHI = 5,  
            /// <summary>
            /// 所得稅
            /// </summary>
            incomeTaxRate = 6   
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebServiceForEFGP.Models;
using WebServiceForEFGP.Models.Services;
using WebServiceForEFGP.Models.VeiwModel;
using Newtonsoft.Json;

namespace WebServiceForEFGP.Manage {
    public partial class PaymentItemSetting : System.Web.UI.Page {

        private AccountItemSvc accountItemSvc = null;

        private List<ListItem> ddl_cashApply_listItem = null;
        private List<ListItem> ddl_accountNo_listItem = null;

        protected void Page_Load(object sender, EventArgs e) {
            this.accountItemSvc = new AccountItemSvc();

            string mode = Request.QueryString["mode"] as string;

            if (string.IsNullOrEmpty(mode)) {
                mode = "list";
            }

            if (!this.IsPostBack) {
                switch (mode) {
                    case "list":
                        //list init
                        this.init();
                        break;
                    case "edit":
                        this.edit_init();
                        break;
                }                
            }

        }

        protected void edit_init() {
            this.mv_paymentItemSetting.SetActiveView(this.vw_paymentItemSetting_edit);

            string id = Request.QueryString["id"] as string;

            if (string.IsNullOrEmpty(id)) {
                Response.Redirect("PaymentItemSetting.aspx");
                return;
            }

            long id_long = 0;

            if (!long.TryParse(id, out id_long)) {
                Response.Redirect("PaymentItemSetting.aspx");
                return;
            }

            AccountItemViewModel.AccountItemTaxRateSettingResult acc_ret = this.accountItemSvc.getAccountItemTaxRateSetting(id_long);

            if (!acc_ret.success) {
                this.lt_paymentItemSetting_msg.Text = UtilitySvc.alertMsg(acc_ret.resultException.ToString());
                return;
            }


            AccountItemViewModel.CashApplyTypeListResult cashApplyListRet = this.accountItemSvc.getCashApplyTypeList();

            if (!cashApplyListRet.list.Exists(x => x.key == "tempPayment_writeOff")) {
                cashApplyListRet.list.Add(new AccountItemViewModel.CashApplyType() { key = "tempPayment_writeOff", name = "暫支款沖銷" });
            }

            List<ListItem> cashApplyOptions = new List<ListItem>();
            cashApplyOptions.Add(new ListItem() { Text = "- 請選擇 -", Value = "" });
            foreach (var c in cashApplyListRet.list) {
                cashApplyOptions.Add(new ListItem() { Text = c.name, Value = c.key });
            }

            this.ddl_paymentItemSetting_form_applyTypeKey.DataSource = cashApplyOptions;
            this.ddl_paymentItemSetting_form_applyTypeKey.DataBind();

            this.ddl_paymentItemSetting_form_applyTypeKey.SelectedValue = acc_ret.acc.applyTypeKey;

            this.txt_paymentItemSetting_form_itemName.Text = acc_ret.acc.accountName;



            List<ListItem> accCombVListOptions = new List<ListItem>();
            accCombVListOptions.Add(new ListItem() { Text = "- 請輸入會科代碼或名稱 -", Value = "" });

            List<AccountItemViewModel.XXX_EP_ACC_COMB_V> accCombVList = this.accountItemSvc.getAccCombVList();
            foreach (var c in accCombVList) {
                accCombVListOptions.Add(new ListItem() { Text = c.ACC_COMB_2_NAME + " - (" + c.ACC_COMB_2 + ")", Value = c.ACC_COMB_2 });
            }


            this.ddl_paymentItemSetting_form_accountNo.DataSource = accCombVListOptions;
            this.ddl_paymentItemSetting_form_accountNo.DataBind();
            this.ddl_paymentItemSetting_form_accountNo.SelectedValue = acc_ret.acc.accountNo;
            this.txt_paymentItemSetting_form_itemName.Text = acc_ret.acc.itemName;
            this.txt_paymentItemSetting_form_taxRate.Text = acc_ret.acc.taxRate.ToString();
            this.txt_paymentItemSetting_form_foreignIncomeTaxRate.Text = acc_ret.acc.foreignIncomeTaxRate.ToString();
            this.txt_paymentItemSetting_form_localIncomeTaxRate.Text = acc_ret.acc.localIncomeTaxRate.ToString();
            this.txt_paymentItemSetting_form_secondNHITaxRate.Text = acc_ret.acc.secondNHITaxRate.ToString();
            this.txt_paymentItemSetting_form_code.Text = acc_ret.acc.code;
            this.chk_paymentItemSetting_form_needCountersign.Checked = acc_ret.acc.needCountersign;
            this.txt_paymentItemSetting_form_memo.Text = acc_ret.acc.memo;

            return;
            
        }



        protected void init() {
            this.mv_paymentItemSetting.SetActiveView(this.vw_paymentItemSetting_list);

            this.clearForm();

            /* init cash apply type */
            AccountItemViewModel.CashApplyTypeListResult cashApplyListRet = this.accountItemSvc.getCashApplyTypeList();

            if (!cashApplyListRet.list.Exists(x => x.key == "tempPayment_writeOff")) {
                cashApplyListRet.list.Add(new AccountItemViewModel.CashApplyType() { key = "tempPayment_writeOff", name = "暫支款沖銷" });
            }



            if (cashApplyListRet.success) {
                
                List<ListItem> cashApplyOptions = new List<ListItem>();
                cashApplyOptions.Add(new ListItem() { Text = "- 請選擇 -", Value = "" });
                foreach (var c in cashApplyListRet.list) {
                    cashApplyOptions.Add(new ListItem() { Text = c.name, Value = c.key });
                }

                this.ddl_paymentItemSetting_type.DataSource = cashApplyOptions;
                this.ddl_paymentItemSetting_type.DataBind();

                this.ddl_paymentItemSetting_filter_cashApply.DataSource = cashApplyOptions;
                this.ddl_paymentItemSetting_filter_cashApply.DataBind();
            }



            /* account item */

            List<ListItem> accCombVListOptions = new List<ListItem>();
            accCombVListOptions.Add(new ListItem() { Text = "- 請輸入會科代碼或名稱 -", Value = "" });

            List<AccountItemViewModel.XXX_EP_ACC_COMB_V> accCombVList = this.accountItemSvc.getAccCombVList();
            foreach (var c in accCombVList) {
                accCombVListOptions.Add(new ListItem() { Text = c.ACC_COMB_2_NAME + " - (" + c.ACC_COMB_2 + ")", Value = c.ACC_COMB_2 });
            }

            this.ddl_paymentItemSetting_accountItem.DataSource = accCombVListOptions;
            this.ddl_paymentItemSetting_accountItem.DataBind();

            this.reloadAccountList();

        }


        protected void btn_paymentItemSetting_add_Click(object sender, EventArgs e) {
            this.lt_paymentItemSetting_msg.Text = "";

            /* validate form */

            if (string.IsNullOrEmpty(this.ddl_paymentItemSetting_accountItem.Text)) {
                this.lt_paymentItemSetting_msg.Text = UtilitySvc.alertMsg("請選擇會計科目");
                return;
            }

            if (string.IsNullOrEmpty(this.ddl_paymentItemSetting_type.Text)) {
                this.lt_paymentItemSetting_msg.Text = UtilitySvc.alertMsg("請選擇費用類型");
                return;
            }

            if (string.IsNullOrEmpty(this.txt_paymentItemSetting_foreignRate.Text)) {
                this.lt_paymentItemSetting_msg.Text = UtilitySvc.alertMsg("請輸入外國人所得稅率");
                return;
            }

            if (string.IsNullOrEmpty(this.txt_paymentItemSetting_localRate.Text)) {
                this.lt_paymentItemSetting_msg.Text = UtilitySvc.alertMsg("請輸入本國人所得稅率");
                return;
            }


            /*
            if (string.IsNullOrEmpty(this.txt_paymentItemSetting_memo.Text)) {
                this.lt_paymentItemSetting_msg.Text = UtilitySvc.alertMsg("請輸入說明文字");
                return;
            }*/


            if (string.IsNullOrEmpty(this.txt_paymentItemSetting_name.Text)) {
                this.lt_paymentItemSetting_msg.Text = UtilitySvc.alertMsg("品項不能為空值");
                return;
            }


            if (string.IsNullOrEmpty(this.txt_paymentItemSetting_secondNHITaxRate.Text)) {
                this.lt_paymentItemSetting_msg.Text = UtilitySvc.alertMsg("二代健保稅率不能為空值");
                return;
            }

            if (string.IsNullOrEmpty(this.txt_paymentItemSetting_taxRate.Text)) {
                this.lt_paymentItemSetting_msg.Text = UtilitySvc.alertMsg("稅率不能為空值");
                return;
            }

            /* add account item */
            List<AccountItemViewModel.XXX_EP_ACC_COMB_V> accCombVList = this.accountItemSvc.getAccCombVList();
            if (!accCombVList.Exists(x => x.ACC_COMB_2 == this.ddl_paymentItemSetting_accountItem.SelectedValue)) {
                this.lt_paymentItemSetting_msg.Text = UtilitySvc.alertMsg("selected  ACC_COMB_2 value is not exist");
                return;
            }
            AccountItemTaxRateSetting acc = new AccountItemTaxRateSetting();
            acc.accountNo = this.ddl_paymentItemSetting_accountItem.SelectedValue;
            acc.accountName = accCombVList.First(x => x.ACC_COMB_2 == acc.accountNo).ACC_COMB_2_NAME;
            acc.applyTypeKey = this.ddl_paymentItemSetting_type.SelectedValue;
            acc.dateCreated = DateTime.Now;
            acc.deleted = false;
            acc.foreignIncomeTaxRate = Math.Round(float.Parse(this.txt_paymentItemSetting_foreignRate.Text), 2);
            acc.itemName = this.txt_paymentItemSetting_name.Text.Trim();
            acc.localIncomeTaxRate = Math.Round(float.Parse(this.txt_paymentItemSetting_localRate.Text), 2);
            acc.memo = this.txt_paymentItemSetting_memo.Text;
            acc.secondNHITaxRate = Math.Round(float.Parse(this.txt_paymentItemSetting_secondNHITaxRate.Text), 2);
            acc.taxRate = Math.Round(float.Parse(this.txt_paymentItemSetting_taxRate.Text), 2);
            acc.needCountersign = this.chk_paymentItemSetting_needCountersign.Checked;
            acc.code = this.txt_paymentItemSetting_code.Text;

            AccountItemViewModel.AccountItemTaxRateSettingResult accRet = this.accountItemSvc.addAccountItemTaxRateSetting(acc);

            if (accRet.success) {
                this.lt_paymentItemSetting_msg.Text = UtilitySvc.alertMsg("新增成功");
                this.clearForm();
                this.reloadAccountList();
            } else {
                this.lt_paymentItemSetting_msg.Text = accRet.resultException;
            }
        }

        protected void clearForm() {
            this.ddl_paymentItemSetting_accountItem.SelectedValue = "";
            this.ddl_paymentItemSetting_type.SelectedValue = "";
            this.txt_paymentItemSetting_foreignRate.Text = "0";
            this.txt_paymentItemSetting_localRate.Text = "0";
            this.txt_paymentItemSetting_memo.Text = "";
            this.txt_paymentItemSetting_name.Text = "";
            this.txt_paymentItemSetting_secondNHITaxRate.Text = "0";
            this.txt_paymentItemSetting_taxRate.Text = "0";
            return;
        }

        protected void reloadAccountList() {

            string pg_str = Request.QueryString["pg"] as string;
            pg_str = string.IsNullOrEmpty(pg_str) ? "1" : pg_str;
            int pg_int = 1;
            int.TryParse(pg_str, out pg_int);

            int pageSize = 20;

            string keyword = Request.QueryString["keyword"] as string;
            string cashApply = Request.QueryString["cashApply"] as string;

            if (string.IsNullOrWhiteSpace(keyword)) {
                keyword = "";
            } else {
                keyword = HttpUtility.UrlDecode(keyword);
            }

            if (string.IsNullOrWhiteSpace(cashApply)) {
                cashApply = "";
            } else {
                cashApply = HttpUtility.UrlDecode(cashApply);
            }



            AccountItemViewModel.AccountItemTaxRateSettingListQueryParameter param = new AccountItemViewModel.AccountItemTaxRateSettingListQueryParameter();

            param.cashApplyTypeKey = cashApply;
            param.includeDeleteItem = false;
            param.orderField = "dateCreated";
            param.pageSize = pageSize;
            param.pageIndex = pg_int;
            param.desc = true;
            param.keyword = keyword;

            AccountItemViewModel.AccountItemTaxRateSettingListResult listResult = this.accountItemSvc.getAccountItemTaxRateSettingListResult(param);

            if (listResult.success) {
                this.rpt_paymentItemSetting.DataSource = listResult.list;
                this.rpt_paymentItemSetting.DataBind();

                this.component_pager.queryString = "pg";
                this.component_pager.pageSize = pageSize;
                this.component_pager.showPageItem = 5;
                this.component_pager.currentPage = pg_int;
                this.component_pager.count = listResult.count;
                this.component_pager.load();
                this.component_pager.DataBind();


            } else {

            }

        }

        protected void ddl_paymentItemSetting_filter_cashApply_SelectedIndexChanged(object sender, EventArgs e) {
            this.reloadAccountList();
        }

        protected void rpt_paymentItemSetting_ItemDataBound(object sender, RepeaterItemEventArgs e) {
            this.getListItem();


            HiddenField hid_paymentItemSetting_list_cashApply = e.Item.FindControl("hid_paymentItemSetting_list_cashApply") as HiddenField;
            HiddenField hid_paymentItemSetting_list_acocounNo = e.Item.FindControl("hid_paymentItemSetting_list_acocounNo") as HiddenField;


            /* bind list dropdownlist */
            DropDownList ddl_paymentItemSetting_list_cashApply = e.Item.FindControl("ddl_paymentItemSetting_list_cashApply") as DropDownList;
            DropDownList ddl_paymentItemSetting_list_accountNo = e.Item.FindControl("ddl_paymentItemSetting_list_accountNo") as DropDownList;


            ddl_paymentItemSetting_list_accountNo.DataSource = this.ddl_accountNo_listItem;
            ddl_paymentItemSetting_list_cashApply.DataSource = this.ddl_cashApply_listItem;
            ddl_paymentItemSetting_list_accountNo.DataBind();
            ddl_paymentItemSetting_list_cashApply.DataBind();

            ddl_paymentItemSetting_list_accountNo.SelectedValue = hid_paymentItemSetting_list_acocounNo.Value;
            ddl_paymentItemSetting_list_cashApply.SelectedValue = hid_paymentItemSetting_list_cashApply.Value;


        }

        protected void rpt_paymentItemSetting_ItemCommand(object source, RepeaterCommandEventArgs e) {

            switch (e.CommandName) {
                case "update":
                    Dictionary<string, object> dicObj = new Dictionary<string, object>();

                    DropDownList ddl_paymentItemSetting_list_cashApply = e.Item.FindControl("ddl_paymentItemSetting_list_cashApply") as DropDownList;
                    TextBox txt_paymentItemSetting_list_name = e.Item.FindControl("txt_paymentItemSetting_list_name") as TextBox;
                    DropDownList ddl_paymentItemSetting_list_accountNo = e.Item.FindControl("ddl_paymentItemSetting_list_accountNo") as DropDownList;
                    TextBox txt_paymentItemSetting_list_taxRate = e.Item.FindControl("txt_paymentItemSetting_list_taxRate") as TextBox;
                    TextBox txt_paymentItemSetting_list_foreignRate = e.Item.FindControl("txt_paymentItemSetting_list_foreignRate") as TextBox;
                    TextBox txt_paymentItemSetting_list_localRate = e.Item.FindControl("txt_paymentItemSetting_list_localRate") as TextBox;
                    TextBox txt_paymentItemSetting_list_secondNHITaxRate = e.Item.FindControl("txt_paymentItemSetting_list_secondNHITaxRate") as TextBox;
                    //TextBox txt_paymentItemSetting_list_memo = e.Item.FindControl("txt_paymentItemSetting_list_memo") as TextBox;

                    List<AccountItemViewModel.XXX_EP_ACC_COMB_V> accCombVList = this.accountItemSvc.getAccCombVList();
                    if (!accCombVList.Exists(x => x.ACC_COMB_2 == ddl_paymentItemSetting_list_accountNo.SelectedValue)) {
                        this.lt_paymentItemSetting_msg.Text = UtilitySvc.alertMsg("selected  ACC_COMB_2 value is not exist");
                        return;
                    }

                    string accountName = accCombVList.First(x => x.ACC_COMB_2 == ddl_paymentItemSetting_list_accountNo.SelectedValue).ACC_COMB_2_NAME;

                    dicObj.Add("applyTypeKey", ddl_paymentItemSetting_list_cashApply.SelectedValue);
                    dicObj.Add("itemName", txt_paymentItemSetting_list_name.Text);
                    dicObj.Add("accountNo", ddl_paymentItemSetting_list_accountNo.Text);
                    dicObj.Add("accountName", accountName);
                    dicObj.Add("taxRate", Math.Round(float.Parse(txt_paymentItemSetting_list_taxRate.Text), 2));
                    dicObj.Add("foreignIncomeTaxRate", Math.Round(float.Parse(txt_paymentItemSetting_list_foreignRate.Text), 2));
                    dicObj.Add("localIncomeTaxRate", Math.Round(float.Parse(txt_paymentItemSetting_list_localRate.Text), 2));
                    dicObj.Add("secondNHITaxRate", Math.Round(float.Parse(txt_paymentItemSetting_list_secondNHITaxRate.Text), 2));
                    //dicObj.Add("memo", txt_paymentItemSetting_list_memo.Text.Trim());

                    this.accountItemSvc.updateAccountItemTaxRateSetting(long.Parse(e.CommandArgument.ToString()), dicObj);
                                        
                    
                    break;
                case "delete":

                    this.accountItemSvc.deleteAccountItemTaxRateSetting(long.Parse(e.CommandArgument.ToString()));                    

                    break;

                case "edit":
                    Response.Redirect("PaymentItemSetting.aspx?mode=edit&id=" + e.CommandArgument.ToString());
                    break;
            }


            this.reloadAccountList();
        }

        protected void getListItem() {
            if (this.ddl_accountNo_listItem == null) {
                this.ddl_accountNo_listItem = new List<ListItem>();
                this.ddl_accountNo_listItem.Add(new ListItem() { Text = "- 請輸入會科代碼或名稱 -", Value = "" });
                List<AccountItemViewModel.XXX_EP_ACC_COMB_V> accCombVList = this.accountItemSvc.getAccCombVList();
                foreach (var c in accCombVList) {
                    this.ddl_accountNo_listItem.Add(new ListItem() { Text = c.ACC_COMB_2_NAME + " - (" + c.ACC_COMB_2 + ")", Value = c.ACC_COMB_2 });
                }
            }

            if (this.ddl_cashApply_listItem == null) {
                this.ddl_cashApply_listItem = new List<ListItem>();
                this.ddl_cashApply_listItem.Add(new ListItem() { Text = "- 請選擇 -", Value = "" });
                AccountItemViewModel.CashApplyTypeListResult cashApplyListRet = this.accountItemSvc.getCashApplyTypeList();

                if (!cashApplyListRet.list.Exists(x => x.key == "tempPayment_writeOff")) {
                    cashApplyListRet.list.Add(new AccountItemViewModel.CashApplyType() { key = "tempPayment_writeOff", name = "暫支款沖銷" });
                }


                foreach (var c in cashApplyListRet.list) {
                    this.ddl_cashApply_listItem.Add(new ListItem() { Text = c.name, Value = c.key });
                }

            }
        }

        protected void btn_search_Click(object sender, EventArgs e) {            
            int pageSize = 20;

            string keyword = this.txt_paymentItemSetting_filter_keyword.Text.Trim();
            string cashApply = this.ddl_paymentItemSetting_filter_cashApply.SelectedValue.Trim();


            AccountItemViewModel.AccountItemTaxRateSettingListQueryParameter param = new AccountItemViewModel.AccountItemTaxRateSettingListQueryParameter();

            param.cashApplyTypeKey = cashApply;
            param.includeDeleteItem = false;
            param.orderField = "dateCreated";
            param.pageSize = pageSize;
            param.pageIndex = 1;
            param.desc = true;
            param.keyword = keyword;

            AccountItemViewModel.AccountItemTaxRateSettingListResult listResult = this.accountItemSvc.getAccountItemTaxRateSettingListResult(param);

            if (listResult.success) {
                this.rpt_paymentItemSetting.DataSource = listResult.list;
                this.rpt_paymentItemSetting.DataBind();

                this.component_pager.queryString = "pg";
                this.component_pager.pageSize = pageSize;
                this.component_pager.showPageItem = 5;
                this.component_pager.currentPage = 1;
                this.component_pager.count = listResult.count;
                this.component_pager.load();
                this.component_pager.DataBind();


            } else {

            }
        }

        protected void btn_paymentItemSetting_form_confirm_Click(object sender, EventArgs e) {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string id = Request.QueryString["id"] as string;
            long id_long = long.Parse(id);

            List<AccountItemViewModel.XXX_EP_ACC_COMB_V> accCombVList = this.accountItemSvc.getAccCombVList();
            if (!accCombVList.Exists(x => x.ACC_COMB_2 == ddl_paymentItemSetting_form_accountNo.SelectedValue)) {
                this.lt_paymentItemSetting_msg.Text = UtilitySvc.alertMsg("selected  ACC_COMB_2 value is not exist");
                return;
            }

            string accountName = accCombVList.First(x => x.ACC_COMB_2 == ddl_paymentItemSetting_form_accountNo.SelectedValue).ACC_COMB_2_NAME;


            dic.Add("applyTypeKey", this.ddl_paymentItemSetting_form_applyTypeKey.SelectedValue);
            dic.Add("itemName", this.txt_paymentItemSetting_form_itemName.Text);
            dic.Add("accountNo", this.ddl_paymentItemSetting_form_accountNo.SelectedValue);
            dic.Add("accountName", accountName);
            dic.Add("taxRate", Math.Round(float.Parse(this.txt_paymentItemSetting_form_taxRate.Text), 2));
            dic.Add("foreignIncomeTaxRate", Math.Round(float.Parse(this.txt_paymentItemSetting_form_foreignIncomeTaxRate.Text), 2));
            dic.Add("localIncomeTaxRate", Math.Round(float.Parse(this.txt_paymentItemSetting_form_localIncomeTaxRate.Text), 2));
            dic.Add("secondNHITaxRate", Math.Round(float.Parse(this.txt_paymentItemSetting_form_secondNHITaxRate.Text), 2));
            dic.Add("memo", this.txt_paymentItemSetting_form_memo.Text.Trim());
            dic.Add("code", this.txt_paymentItemSetting_form_code.Text.Trim());
            dic.Add("needCountersign", this.chk_paymentItemSetting_form_needCountersign.Checked);

            AccountItemViewModel.AccountItemTaxRateSettingResult update_ret = this.accountItemSvc.updateAccountItemTaxRateSetting(id_long, dic);

            if (update_ret.success) {
                Response.Redirect("PaymentItemSetting.aspx");
            }

        }

        protected void btn_paymentItemSetting_form_cancel_Click(object sender, EventArgs e) {
            Response.Redirect("PaymentItemSetting.aspx");
        }
    }


}
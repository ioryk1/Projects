using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebServiceForEFGP.Models;
using WebServiceForEFGP.Models.VeiwModel;
using WebServiceForEFGP.Models.Services;

namespace WebServiceForEFGP.Manage {
    public partial class FormOptionSetting : System.Web.UI.Page {

        private FormOptionSettingSvc formOptionSettingSvc = null;

        private List<FormOptionsSettingViewModel.FormOptionLight> formList = null;

        private int options_list_index = -1; 

        protected void Page_Load(object sender, EventArgs e) {
            this.formOptionSettingSvc = new FormOptionSettingSvc();
                        
            if (!IsPostBack) {
                this.initController();
            }            
            
        }

        private void initController() {
            this.formList = this.formOptionSettingSvc.getFormOptionList();
            List<ListItem> ddl_source_form_list = new List<ListItem>();

            this.ddl_forms.DataTextField = "Text";
            this.ddl_forms.DataValueField = "Value";           
            


            ddl_source_form_list.Add(new ListItem() { Text = "- 請選擇 -", Value = "" });
            foreach (var f in formList) {
                ddl_source_form_list.Add(new ListItem() { Text = f.formName, Value = f.formNo });
            }
            this.ddl_forms.DataSource = ddl_source_form_list;
            this.ddl_forms.DataBind();

            this.ddl_fields.DataTextField = "Text";
            this.ddl_fields.DataValueField = "Value";
        }

        protected void ddl_forms_SelectedIndexChanged(object sender, EventArgs e) {

            this.ddl_fields.DataTextField = "Text";
            this.ddl_fields.DataValueField = "Value";
            List<ListItem> ddl_source_field_list = new List<ListItem>();
            ddl_source_field_list.Add(new ListItem() { Text = "- 請選擇 -", Value = "" }); 

            if (formList == null) {
                this.formList = this.formOptionSettingSvc.getFormOptionList();
            }

            if (string.IsNullOrEmpty(this.ddl_forms.SelectedValue)) {

                this.ddl_fields.DataSource = ddl_source_field_list;
                this.ddl_fields.DataBind();
                return;
            }

            List<FormOptionsSettingViewModel.FormOptionFieldLight> fields = formList.Where(x => x.formNo == this.ddl_forms.SelectedValue).FirstOrDefault().fields;

            foreach (var f in fields) {
                ddl_source_field_list.Add(new ListItem() { Text = f.fieldName, Value = f.fieldID });
            }

            this.ddl_fields.DataSource = ddl_source_field_list;
            this.ddl_fields.DataBind();
        }

        /*
        private void addField() {
            WebServiceForEFGP.Models.Dao.FormOptionSettingDao formDao = new Models.Dao.FormOptionSettingDao();

            formDao.addFormFieldSetting(new Models.FormFieldsSetting() {
                custOptions = true,
                dateCreated = DateTime.Now,
                deleted = false,
                extData1 = "",
                extData2 = "",
                formID = "BCD001",
                id = Guid.NewGuid(),
                key = "chb_customer_category",
                name = "用戶別",
                type = "checkbox",
                 custNotify = false
            });

            formDao.addFormFieldSetting(new Models.FormFieldsSetting() {
                custOptions = true,
                dateCreated = DateTime.Now,
                deleted = false,
                extData1 = "",
                extData2 = "",
                formID = "BCD001",
                id = Guid.NewGuid(),
                key = "chb_customer_age",
                name = "年齡",
                type = "checkbox",
                custNotify = false
            });

            formDao.addFormFieldSetting(new Models.FormFieldsSetting() {
                custOptions = true,
                dateCreated = DateTime.Now,
                deleted = false,
                extData1 = "",
                extData2 = "",
                formID = "BCD001",
                id = Guid.NewGuid(),
                key = "chb_customer_job",
                name = "職業",
                type = "checkbox",
                custNotify = false
            });

            formDao.addFormFieldSetting(new Models.FormFieldsSetting() {
                custOptions = true,
                dateCreated = DateTime.Now,
                deleted = false,
                extData1 = "",
                extData2 = "",
                formID = "BCD001",
                id = Guid.NewGuid(),
                key = "chb_design_item_retail",
                name = "門市製作物",
                type = "checkbox",
                custNotify = true
            });

            formDao.addFormFieldSetting(new Models.FormFieldsSetting() {
                custOptions = false,
                dateCreated = DateTime.Now,
                deleted = false,
                extData1 = "",
                extData2 = "",
                formID = "BCD001",
                id = Guid.NewGuid(),
                key = "chb_design_item_enterprise",
                name = "企業內部製作物",
                type = "checkbox",
                custNotify = true
            });

            formDao.addFormFieldSetting(new Models.FormFieldsSetting() {
                custOptions = false,
                dateCreated = DateTime.Now,
                deleted = false,
                extData1 = "",
                extData2 = "",
                formID = "BCD001",
                id = Guid.NewGuid(),
                key = "chb_design_item_activity",
                name = "活動製作物",
                type = "checkbox",
                custNotify = true
            });

            formDao.addFormFieldSetting(new Models.FormFieldsSetting() {
                custOptions = true,
                dateCreated = DateTime.Now,
                deleted = false,
                extData1 = "",
                extData2 = "",
                formID = "BCD001",
                id = Guid.NewGuid(),
                key = "chb_cust_control_position",
                name = "自控版位",
                type = "checkbox",
                custNotify = false
            });
        }
        */
        protected void rpt_formOption_list_ItemCommand(object source, RepeaterCommandEventArgs e) {
            Dictionary<string, object> dicObj = new Dictionary<string, object>();

            switch (e.CommandName) {
                case "update":
                    TextBox lb_sort = e.Item.FindControl("txt_list_sort") as TextBox;
                    dicObj.Add("sort", int.Parse(lb_sort.Text));
                    break;
                case "delete":
                    dicObj.Add("deleted", true);

                    break;
            }
            this.formOptionSettingSvc.updateFormOptionSetting(long.Parse(e.CommandArgument.ToString()), dicObj);

            this.reload_options_list();
        }

        protected void btn_add_option_Command(object sender, CommandEventArgs e) {

            var field_select_value = this.ddl_fields.SelectedValue;

            if (string.IsNullOrEmpty(field_select_value)) {

                return;
            }
            FormOptionsSetting f = new FormOptionsSetting() {
                dateCreated = DateTime.Now,
                deleted = false,
                fieldID = Guid.Parse(field_select_value),
                fieldName = this.ddl_fields.SelectedItem.Text,
                formName = this.ddl_forms.SelectedItem.Text,
                formNo = this.ddl_forms.SelectedValue,
                formOID = this.ddl_forms.SelectedValue,
                id = 0,
                needKeyIn = this.chk_add_needKeyIn.Checked,
                sort = int.Parse(this.txt_add_sort.Text),
                text = this.txt_add_text.Text,
                type = this.hid_select_field_type.Value,
                value = this.txt_add_value.Text
            };

            


            this.formOptionSettingSvc.addFormOptionSetting(f);

            this.txt_add_sort.Text = "0";
            this.txt_add_text.Text = "";
            this.txt_add_value.Text = "";
            this.chk_add_needKeyIn.Checked = false;

            this.reload_options_list();

            return;
        }

        protected void btn_delete_select_Command(object sender, CommandEventArgs e) {
            Dictionary<string, object> dicObj = new Dictionary<string, object>();
            dicObj.Add("deleted", true);

            foreach (RepeaterItem i in this.rpt_formOption_list.Items) {

                CheckBox chk = i.FindControl("chb_list_select") as CheckBox;
                HiddenField hid_id = i.FindControl("hid_list_id") as HiddenField;

                long id = long.Parse(hid_id.Value);


                if (chk.Checked) {
                    this.formOptionSettingSvc.updateFormOptionSetting(id, dicObj);
                }
            }

            this.reload_options_list();

        }

        protected void ddl_fields_SelectedIndexChanged(object sender, EventArgs e) {
            this.reload_options_list();                       
        }

        protected void reload_options_list() {
            var select_value = this.ddl_fields.SelectedValue;
            if (string.IsNullOrEmpty(select_value)) {
                this.rpt_formOption_list.DataSource = new List<FormOptionSetting>();
                this.rpt_formOption_list.DataBind();
                this.hid_select_field_type.Value = "";
                return;
            }
            var fields = this.formOptionSettingSvc.getFormOptionList().FirstOrDefault(x => x.formNo == this.ddl_forms.SelectedValue).fields;

            var f = fields.FirstOrDefault(x => x.fieldID == select_value);

            this.hid_select_field_type.Value = f.type;

            FormOptionsSettingViewModel.FormOptionsQueryParameter param = new FormOptionsSettingViewModel.FormOptionsQueryParameter() {
                desc = false,
                fieldID = select_value,
                formNo = "",
                formOID = "",
                orderField = "",
                pageIndex = 0,
                pageSize = 0
            };

            FormOptionsSettingViewModel.FormOptionsListResult ret = this.formOptionSettingSvc.getFormOptionListResult(param);

            if (ret.success) {
                this.options_list_index = 1;
                this.rpt_formOption_list.DataSource = ret.list;
                this.rpt_formOption_list.DataBind();                    
            }
            return;

        }

        protected void rpt_formOption_list_ItemDataBound(object sender, RepeaterItemEventArgs e) {

            HiddenField hid_needKeyIn = e.Item.FindControl("hid_list_needKeyIn") as HiddenField;
            Label lb_needKeyIn = e.Item.FindControl("lb_list_needKeyIn") as Label;
            Label lb_index = e.Item.FindControl("lb_list_index") as Label;

            if (hid_needKeyIn.Value == "True") {
                lb_needKeyIn.Text = "顯示";
            } else {
                lb_needKeyIn.Text = "不顯示";
            }
            lb_index.Text = this.options_list_index.ToString();
            this.options_list_index++;

        }
    }
}
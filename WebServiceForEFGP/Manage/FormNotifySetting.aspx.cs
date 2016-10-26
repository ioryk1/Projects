using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebServiceForEFGP.Models;
using WebServiceForEFGP.Models.VeiwModel;
using WebServiceForEFGP.Models.Services;
using Newtonsoft.Json;


namespace WebServiceForEFGP.Manage {
    public partial class FormNotifySetting : System.Web.UI.Page {

        private EmployeeSvc empSvc = null;
        private FormOptionSettingSvc formSvc = null;
        private List<FormOptionsSettingViewModel.FormOptionLight> formList = null;


        protected void Page_Load(object sender, EventArgs e) {

            this.empSvc = new EmployeeSvc();
            this.formSvc = new FormOptionSettingSvc();

            if (!IsPostBack) {
                this.getSelectedEmpList();
                this.getFormList();
            }

        }

        protected void getSelectedEmpList() {
            this.block_select_container.Visible = !string.IsNullOrEmpty(this.ddl_fields.SelectedValue);

            var formList = this.formSvc.getFormNotifyList();
            List<EmployeeViewModel.EmployeeInfoLight> list = new List<EmployeeViewModel.EmployeeInfoLight>();

            var form = formList.FirstOrDefault(x => x.formNo == this.ddl_forms.SelectedValue);

            if (form == null) {
                this.hid_current_select_emp.Value = JsonConvert.SerializeObject(list);
                return;
            } 

            var field = form.fields.FirstOrDefault(x => x.fieldID == this.ddl_fields.SelectedValue);
            if (field == null) {
                this.hid_current_select_emp.Value = JsonConvert.SerializeObject(list);
                return;
            }            

            list = this.empSvc.getEmpInfoListByEmpIdArr(field.extData1.Split(';').ToList());
            this.hid_current_select_emp.Value = JsonConvert.SerializeObject(list);
            this.hid_current_select_emp_id_arr.Value = field.extData1;

        }
        protected void getFormList() {
            this.formList = this.formSvc.getFormNotifyList();

            List<ListItem> ddl_source_form_list = new List<ListItem>();

            this.ddl_forms.DataTextField = "Text";
            this.ddl_forms.DataValueField = "Value";

            ddl_source_form_list.Add(new ListItem() { Text = "- 請選擇 -", Value = "" });
            foreach (var f in formList) {
                ddl_source_form_list.Add(new ListItem() { Text = f.formName, Value = f.formNo });
            }

            this.ddl_forms.DataSource = ddl_source_form_list;
            this.ddl_forms.DataBind();
        }


        protected void ddl_forms_SelectedIndexChanged(object sender, EventArgs e) {
            this.lt_msg.Text = "";
            this.ddl_fields.DataTextField = "Text";
            this.ddl_fields.DataValueField = "Value";
            List<ListItem> ddl_source_field_list = new List<ListItem>();
            ddl_source_field_list.Add(new ListItem() { Text = "- 請選擇 -", Value = "" });

            if (formList == null) {
                this.formList = this.formSvc.getFormNotifyList();
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

        protected void ddl_fields_SelectedIndexChanged(object sender, EventArgs e) {
            this.lt_msg.Text = "";
            this.getSelectedEmpList();            

        }

        protected void btn_save_Click(object sender, EventArgs e) {


            Dictionary<string, object> dicObj = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(this.ddl_fields.SelectedValue)) {
                this.lt_msg.Text = UtilitySvc.alertMsg("請選擇欄位");
                return;
            }
            dicObj.Add("extData1", this.hid_current_select_emp_id_arr.Value);

            CommonViewModel.Result ret = this.formSvc.updateFormFieldSettion(this.ddl_fields.SelectedValue, dicObj);

            if (ret.success) {
                this.lt_msg.Text = "";
                this.getSelectedEmpList();
            } else {
                this.lt_msg.Text = ret.resultException;
            }
                
        }


       
    }
}
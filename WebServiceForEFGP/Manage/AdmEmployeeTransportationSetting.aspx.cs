using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebServiceForEFGP.Models;
using WebServiceForEFGP.Models.Services;
using WebServiceForEFGP.Models.VeiwModel;



namespace WebServiceForEFGP.Manage {
    public partial class AdmEmployeeTransportationSetting : System.Web.UI.Page {

        private FormOptionSettingSvc formOptSvc = null;

        protected void Page_Load(object sender, EventArgs e) {
            this.formOptSvc = new FormOptionSettingSvc();

            if (!Page.IsPostBack) {
                this.init();
            }

        }

        protected void init() {
            
            this.reload();
        }

        protected void reload() {

            this.lt_akert_msg.Text = "";

            string pg_str = Request.QueryString["pg"] as string;
            pg_str = string.IsNullOrEmpty(pg_str) ? "1" : pg_str;
            int pg_int = 1;
            int.TryParse(pg_str, out pg_int);

            int pageSize = 15;


            FormOptionsSettingViewModel.EmployeeTransportationSettingQueryParameter param = new FormOptionsSettingViewModel.EmployeeTransportationSettingQueryParameter() {
                desc = true,
                keyword = this.txt_admEmployeeTransportationSetting_emp_search_keyword.Text,
                orderField = "EmpNo",
                pageIndex = pg_int,
                pageSize = pageSize,
                transType = ""
            };

            FormOptionsSettingViewModel.ViewEmployeeTransportationSettingListResult list_ret = this.formOptSvc.getViewEmployeeTransportationSettingListResult(param);

            

            List<page_viewEmployeeTransationSetting> view_list = new List<page_viewEmployeeTransationSetting>();

            foreach (var v in list_ret.list) {
                view_list.Add(new page_viewEmployeeTransationSetting(v));
            }

            this.rpt_admEmployeeTransportationSetting_list.DataSource = view_list;
            this.rpt_admEmployeeTransportationSetting_list.DataBind();

            this.component_pager.queryString = "pg";
            this.component_pager.pageSize = pageSize;
            this.component_pager.showPageItem = 5;
            this.component_pager.currentPage = pg_int;
            this.component_pager.count = list_ret.count;
            this.component_pager.load();
            this.component_pager.DataBind();


        }

        protected void rpt_admEmployeeTransportationSetting_list_ItemCommand(object source, RepeaterCommandEventArgs e) {

            this.lt_akert_msg.Text = "";

            Dictionary<string, object> dicObj = new Dictionary<string, object>();

            switch (e.CommandName) {
                case "update":
                    TextBox lb_admEmployeeTransportationSetting_list_trnasId = e.Item.FindControl("lb_admEmployeeTransportationSetting_list_trnasId") as TextBox;
                    dicObj.Add("TransId", lb_admEmployeeTransportationSetting_list_trnasId.Text.Trim());
                    break;
                case "delete":
                    dicObj.Add("Deleted", true);                       
                    break;
            }

            CommonViewModel.Result updateRet = this.formOptSvc.updateEmployeeTransportationSetting(long.Parse(e.CommandArgument.ToString()), dicObj);

            //若不成功 則要顯示錯誤訊息
            if (!updateRet.success) {
                this.lt_akert_msg.Text = updateRet.resultException;
            } else {
                this.reload();
            }
        }

        protected void btn_admEmployeeTransportationSetting_emp_search_Click(object sender, EventArgs e) {
            this.reload();
        }

        protected void btn_admEmployeeTransportationSetting_add_Click(object sender, EventArgs e) {
            this.lt_akert_msg.Text = "";

            //check this empNo is Exist in row


            FormOptionsSettingViewModel.EmployeeTransportationSettingQueryParameter param = new FormOptionsSettingViewModel.EmployeeTransportationSettingQueryParameter() {
                desc = true,
                keyword = "",
                orderField = "EmpNo",
                pageIndex = 1,
                pageSize = int.MaxValue,
                transType = this.ddl_admEmployeeTransportationSetting_trans_type.SelectedValue,
                empNo = this.hid_admEmployeeTransportationSetting_emp.Value
            };


            FormOptionsSettingViewModel.ViewEmployeeTransportationSettingListResult list_ret = this.formOptSvc.getViewEmployeeTransportationSettingListResult(param);

            if (list_ret.count > 0) {
                this.lt_akert_msg.Text = UtilitySvc.alertMsg("此員工已存在");
                return;
            }


            EmployeeTransportationSetting ee = new EmployeeTransportationSetting() {
                DateCreated = DateTime.Now,
                Deleted = false,
                EmpNo = this.hid_admEmployeeTransportationSetting_emp.Value,
                TransId = this.txt_admEmployeeTransportationSetting_transId.Text,
                TransType = this.ddl_admEmployeeTransportationSetting_trans_type.SelectedValue
            };

            FormOptionsSettingViewModel.ViewEmployeeTransportationSettingResult add_ret = this.formOptSvc.addEmployeeTransportationSetting(ee);

            if (add_ret.success) {
                this.lt_akert_msg.Text = UtilitySvc.alertMsg("新增成功");
            } else {
                this.lt_akert_msg.Text = UtilitySvc.alertMsg(add_ret.resultException);
            }

        }

        protected class page_viewEmployeeTransationSetting : ViewEmployeeTransportationSetting {

            public string TransTypeEnum { get; set; }
            public string icon { get; set; }


            public page_viewEmployeeTransationSetting(ViewEmployeeTransportationSetting e) : base() {

                
                this.EmpNo = e.EmpNo;
                this.functionDefinitionName = e.functionDefinitionName;
                this.Id = e.Id;
                this.organizationUnitName = e.organizationUnitName;
                this.TransId = e.TransId;
                this.TransType = e.TransType;
                this.userName = e.userName;
                
                switch (this.TransType) {
                    case "car":
                        this.TransTypeEnum = "汽車";
                        this.icon = "fa-car";
                        break;
                    case  "motorcycle" :
                        this.TransTypeEnum = "機車";
                        this.icon = "fa-motorcycle";
                        break;
                }

            }


        }
    }
}
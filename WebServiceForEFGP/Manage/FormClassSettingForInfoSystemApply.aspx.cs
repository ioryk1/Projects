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
    public partial class FormClassSettingForInfoSystemApply : System.Web.UI.Page {

        private FormOptionSettingSvc formOptSvc = null;
        private List<ListItem> ddl_departmentID_listItem = null;
        string query_id = null;
        string pg_str = null;

        protected void Page_Load(object sender, EventArgs e) {
            this.formOptSvc = new FormOptionSettingSvc();

            pg_str = Request.QueryString["pg"] as string;
            pg_str = string.IsNullOrEmpty(pg_str) ? "1" : pg_str;

            query_id = Request.QueryString["id"] as string;

            //2016/10/26
            //test git

            if (string.IsNullOrEmpty(query_id)) {
                //this.MultiView.SetActiveView(View_list);
                this.MultiView.ActiveViewIndex = 0;
            } else {
                //this.MultiView.SetActiveView(View_advanced);
                this.MultiView.ActiveViewIndex = 1;
            }

            if (!Page.IsPostBack) {
                this.init();
                if (this.MultiView.ActiveViewIndex == 1) {
                    this.AdvancedInit();
                }
            }
        }

        protected void init() {
            /*ddl控制項*/
            //分成兩個主要是不能在Dao中寫死pageSize,非常奇怪
            FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param_forDLL = new FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter {
                desc = true,
                orderField = "Id",
                pageIndex = 1,//當換頁時下拉資料會沒有出現,將param.pageIndex固定為1
                pageSize = int.MaxValue,
            };

            //隱藏處理單位對應處理人員
            FormOptionsSettingViewModel.ViewERAUsersOrganizationUnitDepartmentListResult list_retViewERA = this.formOptSvc.getViewERAUsersOrganizationUnitDepartmentListResult(param_forDLL);

            List<ListItem> view_list_hid_departmentID = new List<ListItem>();
            view_list_hid_departmentID.Add(new ListItem() { Text = "- 請選擇 -", Value = "" });

            foreach (var v in list_retViewERA.list) {
                view_list_hid_departmentID.Add(new ListItem() { Text = v.OUName + " - (" + v.OUID + ")", Value = v.ID });
                //view_list_hid_departmentID.Add(new ListItem() { Text = v.OUName, Value = v.ID });
            }

            this.hid_ddl_departmentID.DataValueField = "Value";
            this.hid_ddl_departmentID.DataTextField = "Text";

            this.hid_ddl_departmentID.DataSource = view_list_hid_departmentID;
            this.hid_ddl_departmentID.DataBind();


            //處理單位控制項
            FormOptionsSettingViewModel.OrganizationUnitListResult list_retOrgUnit = this.formOptSvc.getOrganizationUnitListResult(param_forDLL);

            //List<ListItem> view_list_departmentID = new List<ListItem>();
            //view_list_departmentID.Add(new ListItem() { Text = "- 請選擇 -", Value = "" });

            //foreach (var v in list_retOrgUnit.list){
            //    view_list_departmentID.Add(new ListItem() { Text = v.organizationUnitName, Value = v.id });
            //}

            //this.ddl_departmentID.DataValueField = "Value";
            //this.ddl_departmentID.DataTextField = "Text";

            //this.ddl_departmentID.DataSource = view_list_departmentID;
            //this.ddl_departmentID.DataBind();

            //權限單位控制項
            List<ListItem> view_list_PermissionDepartmentID = new List<ListItem>();
            view_list_PermissionDepartmentID.Add(new ListItem() { Text = "- 請選擇 -", Value = "" });

            foreach (var v in list_retOrgUnit.list) {
                view_list_PermissionDepartmentID.Add(new ListItem() { Text = v.organizationUnitName, Value = v.id });
            }

            this.ddl_PermissionDepartmentID.DataValueField = "Value";
            this.ddl_PermissionDepartmentID.DataTextField = "Text";

            this.ddl_PermissionDepartmentID.DataSource = view_list_PermissionDepartmentID;
            this.ddl_PermissionDepartmentID.DataBind();

            //改作所屬部門方式
            this.ddl_departmentID_listItem = new List<ListItem>();
            this.ddl_departmentID_listItem.Add(new ListItem() { Text = "- 請輸入部門名稱 -", Value = "" });

            List<FormOptionsSettingViewModel.departmentLight> depList = this.formOptSvc.getDepList();
            foreach (var c in depList) {
                this.ddl_departmentID_listItem.Add(new ListItem() { Text = c.organizationUnitName + " - (" + c.id + ")", Value = c.id });
            }

            this.ddl_departmentID.DataSource = this.ddl_departmentID_listItem;
            this.ddl_departmentID.DataBind();

            this.reload();
        }


        /// <summary>
        /// 重載列表
        /// </summary>
        protected void reload() {

            this.lt_akert_msg.Text = "";

            int pg_int = 1;
            int.TryParse(pg_str, out pg_int);
            int pageSize = 15;

            var sKeyword = Request.QueryString["k"] as string;
            sKeyword = string.IsNullOrEmpty(sKeyword) ? "" : HttpUtility.UrlDecode(sKeyword);

            var sclassType = Request.QueryString["c"] as string;
            sclassType = string.IsNullOrEmpty(sclassType) ? "0" : sclassType;

            //判斷是否有點選過查詢類別當條件,有的話將查詢條件設定為預設
            if (!string.IsNullOrEmpty(sKeyword)) {
                this.txt_formClassSettingForInfoSystemApply_className_search_keyword.Text = sKeyword;
            }

            //因為主類別刪除時,會更新DB必須重新Reload.所以移至此處
            FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param_forDLL = new FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter {
                desc = true,
                orderField = "Id",
                pageIndex = 1,
                pageSize = int.MaxValue,
            };
            //主類別
            FormOptionsSettingViewModel.ERACategoryForInfListResult list_retERAC = this.formOptSvc.getERACategoryForInfMainClassListResult(param_forDLL);

            List<ListItem> view_list_parentID = new List<ListItem>();
            view_list_parentID.Add(new ListItem() { Text = "- 請選擇 -", Value = "" });
            view_list_parentID.Add(new ListItem() { Text = "無主類別", Value = "-" });

            foreach (var v in list_retERAC.list) {
                view_list_parentID.Add(new ListItem() { Text = v.name, Value = v.Id.ToString() });
            }

            this.ddl_formClassSettingForInfoSystemApply_parentID.DataValueField = "Value";
            this.ddl_formClassSettingForInfoSystemApply_parentID.DataTextField = "Text";

            this.ddl_formClassSettingForInfoSystemApply_parentID.DataSource = view_list_parentID;
            this.ddl_formClassSettingForInfoSystemApply_parentID.DataBind();


            //主類別(搜尋)
            List<ListItem> view_list_mainClass = new List<ListItem>();
            view_list_mainClass.Add(new ListItem() { Text = "- 請選擇 -", Value = "" });

            foreach (var v in list_retERAC.list) {
                view_list_mainClass.Add(new ListItem() { Text = v.name, Value = v.Id.ToString() });
            }

            this.ddl_formClassSettingForInfoSystemApply_mainClass.DataValueField = "Value";
            this.ddl_formClassSettingForInfoSystemApply_mainClass.DataTextField = "Text";

            this.ddl_formClassSettingForInfoSystemApply_mainClass.DataSource = view_list_mainClass;
            this.ddl_formClassSettingForInfoSystemApply_mainClass.DataBind();
            //主類別搜尋移至此處
            if (sclassType != "0") {
                ddl_formClassSettingForInfoSystemApply_mainClass.SelectedIndex = ddl_formClassSettingForInfoSystemApply_mainClass.Items.IndexOf(ddl_formClassSettingForInfoSystemApply_mainClass.Items.FindByValue(sclassType));
                if (ddl_formClassSettingForInfoSystemApply_mainClass.SelectedIndex == 0) {
                    sclassType = "0";
                }
            }

            //表單資料
            FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param = new FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter {
                desc = true,
                orderField = "Id",
                pageIndex = pg_int,
                pageSize = pageSize,
                keyword = sKeyword,
                classType = int.Parse(sclassType)
            };

            FormOptionsSettingViewModel.ViewERACategoryPermissionForInfListResult list_ret = this.formOptSvc.getViewERACategoryPermissionForInfListResult(param);

            List<page_ERACategoryForInfList> view_list = new List<page_ERACategoryForInfList>();

            foreach (var v in list_ret.list) {
                view_list.Add(new page_ERACategoryForInfList(v));
            }

            this.rpt_formClassSettingForInfoSystemApply_list.DataSource = view_list;
            this.rpt_formClassSettingForInfoSystemApply_list.DataBind();

            this.component_pager.queryString = "pg";
            this.component_pager.pageSize = pageSize;
            this.component_pager.showPageItem = 5;
            this.component_pager.currentPage = pg_int;
            this.component_pager.count = list_ret.count;
            this.component_pager.load();
            this.component_pager.DataBind();

            //清空或預設值
            this.ddl_departmentID.SelectedValue = "";
            this.hid_departmentID.Value = "";
            this.txt_formClassSettingForInfoSystemApply_className.Text = "";
            this.ddl_formClassSettingForInfoSystemApply_parentID.SelectedValue = "";
            this.ddl_formClassSettingForInfoSystemApply_dicisionProcessLevel.SelectedValue = "";
            this.ddl_formClassSettingForInfoSystemApply_isforAll.SelectedValue = "";
            this.ddl_PermissionDepartmentID.SelectedValue = "";
        }

        protected void rpt_formClassSettingForInfoSystemApply_list_ItemDataBound(object sender, RepeaterItemEventArgs e) {
            this.getListItem();

            HiddenField hidd_rptPersonnelID = e.Item.FindControl("hidd_rptPersonnelID") as HiddenField;
            HiddenField hid_formClassSettingForInfoSystemApply_list_departmentID = e.Item.FindControl("hid_formClassSettingForInfoSystemApply_list_departmentID") as HiddenField;
            DropDownList ddl_formClassSettingForInfoSystemApply_list_departmentID = e.Item.FindControl("ddl_formClassSettingForInfoSystemApply_list_departmentID") as DropDownList;


            ddl_formClassSettingForInfoSystemApply_list_departmentID.DataSource = this.ddl_departmentID_listItem;
            ddl_formClassSettingForInfoSystemApply_list_departmentID.DataBind();

            ddl_formClassSettingForInfoSystemApply_list_departmentID.SelectedValue = hid_formClassSettingForInfoSystemApply_list_departmentID.Value;

            //判斷處理人員不為空值時,所屬部門下拉選單則為唯讀
            if (hidd_rptPersonnelID.Value != "") {
                ddl_formClassSettingForInfoSystemApply_list_departmentID.Enabled = false;
            }
        }

        protected void rpt_formClassSettingForInfoSystemApply_list_ItemCommand(object source, RepeaterCommandEventArgs e) {

            this.lt_akert_msg.Text = "";

            Dictionary<string, object> dicObj_ERAC = new Dictionary<string, object>();
            Dictionary<string, object> dicObj_ERAP = new Dictionary<string, object>();

            TextBox txt_formClassSettingForInfoSystemApply_list_name;
            HiddenField hidd_rptPersonnelID;
            HiddenField hidd_rptAPersonnelID;
            HiddenField hidd_drp_rptdepartmentID;
            DropDownList drp_rptdepartmentID;

            switch (e.CommandName) {
                case "update":
                    //類別名稱
                    txt_formClassSettingForInfoSystemApply_list_name = e.Item.FindControl("txt_formClassSettingForInfoSystemApply_list_name") as TextBox;
                    dicObj_ERAC.Add("name", txt_formClassSettingForInfoSystemApply_list_name.Text.Trim());

                    hidd_rptPersonnelID = e.Item.FindControl("hidd_rptPersonnelID") as HiddenField;//處理人員
                    hidd_rptAPersonnelID = e.Item.FindControl("hidd_rptActivityPersonnelID") as HiddenField;//加簽人員
                    hidd_drp_rptdepartmentID = e.Item.FindControl("hid_formClassSettingForInfoSystemApply_list_departmentID") as HiddenField;//部門(隱藏)
                    drp_rptdepartmentID = e.Item.FindControl("ddl_formClassSettingForInfoSystemApply_list_departmentID") as DropDownList;//部門

                    //if (hidd_rptPersonnelID.Value != "") {
                    //    this.hid_ddl_departmentID.SelectedValue = hidd_rptPersonnelID.Value;
                    //    var rptPersonnelID_departmentID = this.ddl_departmentID.Items.FindByText(this.hid_ddl_departmentID.SelectedItem.Text).Value;
                    //    this.hid_ddl_departmentID.SelectedValue = "";//清空
                    //    dicObj_ERAC.Add("personnelID", hidd_rptPersonnelID.Value);
                    //    dicObj_ERAC.Add("departmentID", rptPersonnelID_departmentID);
                    //} else {
                    //    dicObj_ERAC.Add("personnelID", "");
                    //    dicObj_ERAC.Add("departmentID", drp_rptdepartmentID);
                    //}

                    dicObj_ERAC.Add("personnelID", hidd_rptPersonnelID.Value);
                    dicObj_ERAC.Add("departmentID", drp_rptdepartmentID.SelectedValue == "" ? hidd_drp_rptdepartmentID.Value : drp_rptdepartmentID.SelectedValue);
                    dicObj_ERAC.Add("addActivityPersonnelID", hidd_rptAPersonnelID.Value);
                    //if (hidd_rptAPersonnelID.Value != "") {
                    //    dicObj_ERAC.Add("addActivityPersonnelID", hidd_rptAPersonnelID.Value);
                    //}

                    drp_rptdepartmentID = e.Item.FindControl("ddl_formClassSettingForInfoSystemApply_list_departmentID") as DropDownList;
                    break;
                case "delete":
                    txt_formClassSettingForInfoSystemApply_list_name = e.Item.FindControl("txt_formClassSettingForInfoSystemApply_list_name") as TextBox;
                    int i = this.checkSubClassCount(txt_formClassSettingForInfoSystemApply_list_name.Text.Trim());

                    if (i > 0) {
                        this.lt_akert_msg.Text = UtilitySvc.alertMsg("請先刪除主類別底下的子類別");
                        return;
                    } else {
                        dicObj_ERAC.Add("deleted", true);
                        dicObj_ERAP.Add("deleted", true);
                    }
                    break;
                case "advanced":
                    //主類別與關鍵字查詢
                    var k = string.IsNullOrEmpty(Request.QueryString["k"]) ? "" : Request.QueryString["k"].ToString();
                    var c = string.IsNullOrEmpty(Request.QueryString["c"]) ? "" : Request.QueryString["c"].ToString();
                    //處理人員與加簽人員
                    this.hid_ViewAdvanced_PersonnelID = e.Item.FindControl("hidd_rptPersonnelID") as HiddenField;
                    this.hid_ViewAdvanced_ActivityPersonnelID = e.Item.FindControl("hidd_rptActivityPersonnelID") as HiddenField;
                    Response.Redirect("FormClassSettingForInfoSystemApply.aspx?pg=" + pg_str + "&id=" + e.CommandArgument.ToString() + "&k=" + k + "&c=" + c + "&pID=" + HttpUtility.UrlEncode(this.hid_ViewAdvanced_PersonnelID.Value) + "&spID=" + HttpUtility.UrlEncode(this.hid_ViewAdvanced_ActivityPersonnelID.Value));
                    break;
            }

            CommonViewModel.Result updateRet_ERAC = this.formOptSvc.updateERACategoryForInf(long.Parse(e.CommandArgument.ToString()), dicObj_ERAC);     //ERACategoryForInf
            CommonViewModel.Result updateRet_ERAP = this.formOptSvc.updateERAPermissionForInf(long.Parse(e.CommandArgument.ToString()), dicObj_ERAP);   //ERAPermissionForInf

            //若不成功 則要顯示錯誤訊息
            if (!updateRet_ERAC.success || !updateRet_ERAC.success) {
                this.lt_akert_msg.Text = updateRet_ERAC.resultException + updateRet_ERAP.resultException;
            } else {
                this.reload();
            }

        }

        protected void getListItem() {
            if (this.ddl_departmentID_listItem == null) {

                this.ddl_departmentID_listItem = new List<ListItem>();
                this.ddl_departmentID_listItem.Add(new ListItem() { Text = "- 請輸入部門名稱 -", Value = "" });
                List<FormOptionsSettingViewModel.departmentLight> depList = this.formOptSvc.getDepList();
                foreach (var c in depList) {
                    this.ddl_departmentID_listItem.Add(new ListItem() { Text = c.organizationUnitName + " - (" + c.id + ")", Value = c.id });
                }
            }
        }

        //判斷主類別底下是否有子類別
        protected int checkSubClassCount(string sName) {
            FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param = new FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter() {
                desc = true,
                orderField = "Id",
                pageIndex = 1,
                pageSize = int.MaxValue,
                name = sName,
                keyword = "",
                classType = 0,
            };

            FormOptionsSettingViewModel.ViewERACategoryPermissionForInfListResult list_ret = this.formOptSvc.getViewERACategoryPermissionForInfListResult(param);

            if (list_ret.count > 0) {
                return list_ret.count;
            } else {
                return 0;
            }
        }

        protected void btn_formClassSettingForInfoSystemApply_className_search_Click(object sender, EventArgs e) {
            this.reload();
        }

        protected void btn_formClassSettingForInfoSystemApply_add_Click(object sender, EventArgs e) {
            this.lt_akert_msg.Text = "";

            //新增時判斷DB是否有此項資料(目前作法判斷項目名稱,只要名稱重複.一律不能新增(已存在).即使是同名稱不同人不同層級)
            FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param = new FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter() {
                desc = true,
                orderField = "Id",
                pageIndex = 1,
                pageSize = int.MaxValue,
                personnelID = this.hid_formClassSettingForInfoSystemApply_personnelID.Value,
                name = this.txt_formClassSettingForInfoSystemApply_className.Text.ToString().Trim(),
                departmentID = this.hid_departmentID.Value,
                dicisionProcessLevel = this.ddl_formClassSettingForInfoSystemApply_dicisionProcessLevel.SelectedValue
            };

            FormOptionsSettingViewModel.ERACategoryForInfListResult list_ret = this.formOptSvc.getERACategoryForInfListResult(param);

            if (list_ret.count > 0) {
                this.lt_akert_msg.Text = UtilitySvc.alertMsg("此類別已存在");
                return;
            }

            //新增ERACategoryForInf項目作法
            long pID = 0;

            if (this.ddl_formClassSettingForInfoSystemApply_parentID.SelectedValue == "-") {
                pID = -1;//代表父類別
            } else {
                pID = long.Parse(this.ddl_formClassSettingForInfoSystemApply_parentID.SelectedValue);//子類別parentID欄位抓取父類別ID
            }

            ERACategoryForInf ec = new ERACategoryForInf() {
                parentID = pID,
                name = this.txt_formClassSettingForInfoSystemApply_className.Text,
                departmentID = this.hid_departmentID.Value,
                personnelID = this.hid_formClassSettingForInfoSystemApply_personnelID.Value,
                dicisionProcessLevel = this.ddl_formClassSettingForInfoSystemApply_dicisionProcessLevel.SelectedValue,
                deleted = false,
                DateCreated = DateTime.Now,
                addActivityPersonnelID = this.hid_formClassSettingForInfoSystemApply_ActivityPersonnelID.Value
            };

            FormOptionsSettingViewModel.ERACategoryForInfResult add_ret_ERAC = this.formOptSvc.addERACategoryForInf(ec);

            //新增ERAPermissionForInf項目作法
            long iERACategoryID = ec.Id;    //ERAPermissionForInf.ERACategoryID
            bool bIsforAll = true;

            if (this.ddl_formClassSettingForInfoSystemApply_isforAll.SelectedValue == "0") {
                bIsforAll = false;
            }

            ERAPermissionForInf ep = new ERAPermissionForInf() {
                ERACategoryID = iERACategoryID,
                isforAll = bIsforAll,
                enable = true,
                departmentID = this.ddl_PermissionDepartmentID.SelectedValue,
                deleted = false,
                DateCreated = DateTime.Now
            };

            FormOptionsSettingViewModel.ERAPermissionForInfResult add_ret_ERAP = this.formOptSvc.addERAPermissionForInf(ep);

            if (add_ret_ERAC.success || add_ret_ERAP.success) {
                this.lt_akert_msg.Text = UtilitySvc.alertMsg("新增成功");
            } else {
                this.lt_akert_msg.Text = UtilitySvc.alertMsg(add_ret_ERAC.resultException) + UtilitySvc.alertMsg(add_ret_ERAP.resultException);
            }

            this.reload();
        }

        protected void ddl_formClassSettingForInfoSystemApply_Change(object sender, EventArgs e) {
            FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param_forDLL = new FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter {
                desc = true,
                orderField = "Id",
                pageIndex = 1,//當換頁時下拉資料會沒有出現,將param.pageIndex固定為1
                pageSize = int.MaxValue,
            };

            //權限單位控制項
            List<ListItem> view_list_PermissionDepartmentID = new List<ListItem>();
            view_list_PermissionDepartmentID.Add(new ListItem() { Text = "- 請選擇 -", Value = "" });

            FormOptionsSettingViewModel.OrganizationUnitListResult list_retOrgUnit = this.formOptSvc.getOrganizationUnitListResult(param_forDLL);
            FormOptionsSettingViewModel.ClassificationUnitListResult unitRet = this.formOptSvc.getPurviewUnitByParentID(this.ddl_formClassSettingForInfoSystemApply_parentID.SelectedValue);

            if (this.ddl_formClassSettingForInfoSystemApply_parentID.SelectedValue != "" && this.ddl_formClassSettingForInfoSystemApply_parentID.SelectedValue != "-" && unitRet.list[0].isforAll == false) {
                var unit = unitRet.list[0].unit.Split(',');

                if (unit.Length > 0) {
                    for (var i = 0; i < unit.Length; i++) {
                        var list_test = list_retOrgUnit.list.FindAll(x => x.OID == unit[i]);

                        foreach (var v in list_test) {
                            view_list_PermissionDepartmentID.Add(new ListItem() { Text = v.organizationUnitName, Value = v.id });
                        }
                    }
                }
            } else {
                foreach (var v in list_retOrgUnit.list) {
                    view_list_PermissionDepartmentID.Add(new ListItem() { Text = v.organizationUnitName, Value = v.id });
                }
            }

            this.ddl_PermissionDepartmentID.DataValueField = "Value";
            this.ddl_PermissionDepartmentID.DataTextField = "Text";

            this.ddl_PermissionDepartmentID.DataSource = view_list_PermissionDepartmentID;
            this.ddl_PermissionDepartmentID.DataBind();
        }

        protected class page_ERACategoryForInfList : ViewERACategoryPermissionForInf {
            public string parentName { get; set; }
            public page_ERACategoryForInfList(ViewERACategoryPermissionForInf e) : base() {
                this.Id = e.Id;
                this.parentID = e.parentID;
                this.name = e.name;
                this.departmentID = e.departmentID;
                this.department = e.department;
                this.personnelID = e.personnelID;
                this.personnel = e.personnel;
                this.dicisionProcessLevel = e.dicisionProcessLevel;
                this.Permission_departmentID = e.Permission_departmentID;
                this.Permission_department = e.Permission_department;
                this.addActivityPersonnelID = e.addActivityPersonnelID;
                this.addActivityPersonnel = e.addActivityPersonnel;

                switch (this.dicisionProcessLevel) {
                    case "3":
                        this.dicisionProcessLevel = "事業部";
                        break;
                    case "2":
                        this.dicisionProcessLevel = "處級";
                        break;
                    case "1":
                        this.dicisionProcessLevel = "部門";
                        break;
                }

                switch (string.IsNullOrEmpty(e.partent_name)) {
                    case true:
                        this.partent_name = "主類別";
                        break;
                    case false:
                        this.partent_name = e.partent_name;
                        break;
                }

                switch (string.IsNullOrEmpty(e.Permission_department)) {
                    case true:
                        this.Permission_department = "適用全單位";
                        break;
                    case false:
                        this.Permission_department = e.Permission_department;
                        break;
                }
            }
        }

        protected class ddl_ERACategoryForInf_mainClass : ERACategoryForInf {
            public ddl_ERACategoryForInf_mainClass(ERACategoryForInf e) : base() {
                this.Id = e.Id;
                this.parentID = e.parentID;
                this.name = e.name;
            }
        }

        //View_advanced控制項
        protected void AdvancedInit() {

            var pID = Request.QueryString["pID"] as string;
            this.hid_ViewAdvanced_PersonnelID.Value = string.IsNullOrEmpty(pID) ? "" : HttpUtility.UrlDecode(pID);//處理人員

            var spID = Request.QueryString["spID"] as string;
            this.hid_ViewAdvanced_ActivityPersonnelID.Value = string.IsNullOrEmpty(spID) ? "" : HttpUtility.UrlDecode(spID);//加簽人員

            FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter View_advanced_forDLL = new FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter {
                desc = true,
                orderField = "Id",
                pageIndex = 1,
                pageSize = int.MaxValue,
                id = int.Parse(query_id)
            };

            //主類別
            FormOptionsSettingViewModel.ERACategoryForInfListResult View_advanced_retERAC = this.formOptSvc.getERACategoryForInfMainClassListResult(View_advanced_forDLL);

            List<ListItem> view_list_parentID = new List<ListItem>();
            view_list_parentID.Add(new ListItem() { Text = "- 請選擇 -", Value = "" });
            view_list_parentID.Add(new ListItem() { Text = "無主類別", Value = "-" });

            foreach (var v in View_advanced_retERAC.list) {
                view_list_parentID.Add(new ListItem() { Text = v.name, Value = v.Id.ToString() });
            }

            this.ddl_ViewAdvanced_PartentID.DataValueField = "Value";
            this.ddl_ViewAdvanced_PartentID.DataTextField = "Text";

            this.ddl_ViewAdvanced_PartentID.DataSource = view_list_parentID;
            this.ddl_ViewAdvanced_PartentID.DataBind();

            //部門單位
            FormOptionsSettingViewModel.OrganizationUnitListResult View_advanced_retOrgUnit = this.formOptSvc.getOrganizationUnitListResult(View_advanced_forDLL);

            //List<ListItem> view_list_departmentID = new List<ListItem>();
            //view_list_departmentID.Add(new ListItem() { Text = "- 請選擇 -", Value = "" });

            //foreach (var v in View_advanced_retOrgUnit.list) {
            //    view_list_departmentID.Add(new ListItem() { Text = v.organizationUnitName, Value = v.id });
            //}

            //this.ddl_ViewAdvanced_DepartmentID.DataValueField = "Value";
            //this.ddl_ViewAdvanced_DepartmentID.DataTextField = "Text";

            //this.ddl_ViewAdvanced_DepartmentID.DataSource = view_list_departmentID;
            //this.ddl_ViewAdvanced_DepartmentID.DataBind();

            //改作所屬部門方式
            this.ddl_departmentID_listItem = new List<ListItem>();
            this.ddl_departmentID_listItem.Add(new ListItem() { Text = "- 請輸入部門名稱 -", Value = "" });

            List<FormOptionsSettingViewModel.departmentLight> depList = this.formOptSvc.getDepList();
            foreach (var c in depList) {
                this.ddl_departmentID_listItem.Add(new ListItem() { Text = c.organizationUnitName + " - (" + c.id + ")", Value = c.id });
            }

            this.ddl_ViewAdvanced_DepartmentID.DataSource = this.ddl_departmentID_listItem;
            this.ddl_ViewAdvanced_DepartmentID.DataBind();

            //隱藏處理單位對應處理人員
            FormOptionsSettingViewModel.ViewERAUsersOrganizationUnitDepartmentListResult list_retViewERA = this.formOptSvc.getViewERAUsersOrganizationUnitDepartmentListResult(View_advanced_forDLL);

            List<ListItem> view_list_hid_departmentID = new List<ListItem>();
            view_list_hid_departmentID.Add(new ListItem() { Text = "- 請選擇 -", Value = "" });

            foreach (var v in list_retViewERA.list) {
                view_list_hid_departmentID.Add(new ListItem() { Text = v.OUName + " - (" + v.OUID + ")", Value = v.ID });
                //view_list_hid_departmentID.Add(new ListItem() { Text = v.OUName, Value = v.ID });
            }

            this.hid_ddl_ViewAdvanced_DepartmentID.DataValueField = "Value";
            this.hid_ddl_ViewAdvanced_DepartmentID.DataTextField = "Text";

            this.hid_ddl_ViewAdvanced_DepartmentID.DataSource = view_list_hid_departmentID;
            this.hid_ddl_ViewAdvanced_DepartmentID.DataBind();

            //權限單位控制項
            List<ListItem> view_list_PermissionDepartmentID = new List<ListItem>();
            view_list_PermissionDepartmentID.Add(new ListItem() { Text = "- 請選擇 -", Value = "" });

            foreach (var v in View_advanced_retOrgUnit.list) {
                view_list_PermissionDepartmentID.Add(new ListItem() { Text = v.organizationUnitName, Value = v.id });
            }

            this.ddl_ViewAdvanced_PermissionDepartmentID.DataValueField = "Value";
            this.ddl_ViewAdvanced_PermissionDepartmentID.DataTextField = "Text";

            this.ddl_ViewAdvanced_PermissionDepartmentID.DataSource = view_list_PermissionDepartmentID;
            this.ddl_ViewAdvanced_PermissionDepartmentID.DataBind();

            //顯示額外欄位
            FormOptionsSettingViewModel.ERADynamicFieldSettingListResult View_advanced_setDynamicField = this.formOptSvc.getERADynamicFieldSettingListResult(View_advanced_forDLL);

            List<ListItem> view_list_DynamicField_text = new List<ListItem>();
            List<ListItem> view_list_DynamicField_temp = new List<ListItem>();

            foreach (var v in View_advanced_setDynamicField.list) {
                string sname = v.isRequire == true ? "*" + v.name : v.name;

                if (v.type == "text") {
                    view_list_DynamicField_text.Add(new ListItem() { Text = sname, Value = v.Id.ToString() });
                }
            }

            this.ckl_DisplayOtherField.Items.Clear();
            this.ckl_DisplayOtherField.Items.AddRange(view_list_DynamicField_text.ToArray());

            //批次範本下載
            foreach (var v in View_advanced_setDynamicField.list) {
                if (v.type == "fileDownloadBtn") {
                    view_list_DynamicField_temp.Add(new ListItem() { Text = v.name + "(" + v.memo + ")", Value = v.Id.ToString() });
                }
            }

            this.chk_TemplateDownload.Items.Clear();
            this.chk_TemplateDownload.Items.AddRange(view_list_DynamicField_temp.ToArray());

            //本頁所點選資料
            FormOptionsSettingViewModel.ViewERACategoryPermissionDynamicFieldSettingForInfListResult View_advanced_data = this.formOptSvc.getViewERACategoryPermissionDynamicFieldSettingForInfListResult(View_advanced_forDLL);

            this.lblAdvancedID.Text = string.IsNullOrEmpty(View_advanced_data.list[0].Id.ToString()) ? "" : View_advanced_data.list[0].Id.ToString();//ID
            this.txt_ViewAdvanced_ClassName.Text = string.IsNullOrEmpty(View_advanced_data.list[0].name) ? "" : View_advanced_data.list[0].name.ToString();//類別名稱
            this.ddl_ViewAdvanced_PartentID.SelectedValue = View_advanced_data.list[0].parentID == -1 ? "-" : View_advanced_data.list[0].parentID.ToString();//主類別
            this.ddl_ViewAdvanced_DepartmentID.SelectedValue = string.IsNullOrEmpty(View_advanced_data.list[0].departmentID) ? "" : this.ddl_ViewAdvanced_DepartmentID.Items.FindByValue(View_advanced_data.list[0].departmentID.ToString()).Value;//部門名稱
            this.hid_ViewAdvanced_departmentID.Value = string.IsNullOrEmpty(View_advanced_data.list[0].departmentID) ? "" : this.ddl_ViewAdvanced_DepartmentID.Items.FindByValue(View_advanced_data.list[0].departmentID.ToString()).Value;//部門名稱
            this.ddl_ViewAdvanced_DicisionProcessLevel.SelectedValue = string.IsNullOrEmpty(View_advanced_data.list[0].dicisionProcessLevel) ? "" : View_advanced_data.list[0].dicisionProcessLevel.ToString();//核決層級
            this.ddl_ViewAdvanced_IsforAll.SelectedValue = View_advanced_data.list[0].isforAll == true ? "1" : "0";//適用全部門
            this.ddl_ViewAdvanced_PermissionDepartmentID.SelectedValue = string.IsNullOrEmpty(View_advanced_data.list[0].Permission_departmentID) ? "" : this.ddl_ViewAdvanced_PermissionDepartmentID.Items.FindByValue(View_advanced_data.list[0].Permission_departmentID.ToString()).Value;//權限單位
            //額外動態欄位
            if (View_advanced_data.list[0].CDF_ID_T != null) {
                var gCDF_ID_T = View_advanced_data.list[0].CDF_ID_T.Split(',');
                for (var i = 0; i < gCDF_ID_T.Count(); i++) {
                    this.ckl_DisplayOtherField.Items.FindByValue(gCDF_ID_T[i]).Selected = true;
                    this.lbl_cklValues.Text += gCDF_ID_T[i].ToString();
                }
            }
            //範本下載
            if (View_advanced_data.list[0].CDF_ID_B != null) {
                var gCDF_ID_B = View_advanced_data.list[0].CDF_ID_B.Split(',');
                for (var i = 0; i < gCDF_ID_B.Count(); i++) {
                    this.chk_TemplateDownload.Items.FindByValue(gCDF_ID_B[i]).Selected = true;
                    this.lbl_cklValues.Text += gCDF_ID_B[i].ToString();
                }
            }
        }
        protected void AdvancedReload() {

            FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter View_advanced_forDLL = new FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter {
                desc = true,
                orderField = "Id",
                pageIndex = 1,
                pageSize = int.MaxValue,
                id = int.Parse(query_id)
            };

            //本頁所點選資料
            FormOptionsSettingViewModel.ViewERACategoryPermissionDynamicFieldSettingForInfListResult View_advanced_data = this.formOptSvc.getViewERACategoryPermissionDynamicFieldSettingForInfListResult(View_advanced_forDLL);

            this.lblAdvancedID.Text = string.IsNullOrEmpty(View_advanced_data.list[0].Id.ToString()) ? "" : View_advanced_data.list[0].Id.ToString();//ID
            this.hid_ViewAdvanced_PersonnelID.Value = string.IsNullOrEmpty(View_advanced_data.list[0].personnel) ? "" : View_advanced_data.list[0].personnel.ToString();//處理人員
            this.txt_ViewAdvanced_ClassName.Text = string.IsNullOrEmpty(View_advanced_data.list[0].name) ? "" : View_advanced_data.list[0].name.ToString();//類別名稱
            this.ddl_ViewAdvanced_PartentID.SelectedValue = View_advanced_data.list[0].parentID == -1 ? "-" : View_advanced_data.list[0].parentID.ToString();//主類別
            this.ddl_ViewAdvanced_DepartmentID.SelectedValue = string.IsNullOrEmpty(View_advanced_data.list[0].departmentID) ? "" : this.ddl_ViewAdvanced_DepartmentID.Items.FindByValue(View_advanced_data.list[0].departmentID.ToString()).Value;//部門名稱
            //this.hid_ViewAdvanced_departmentID.Value = string.IsNullOrEmpty(View_advanced_data.list[0].departmentID) ? "" : this.ddl_ViewAdvanced_DepartmentID.Items.FindByValue(View_advanced_data.list[0].departmentID.ToString()).Value;//部門名稱
            this.ddl_ViewAdvanced_DicisionProcessLevel.SelectedValue = string.IsNullOrEmpty(View_advanced_data.list[0].dicisionProcessLevel) ? "" : View_advanced_data.list[0].dicisionProcessLevel.ToString();//核決層級
            this.ddl_ViewAdvanced_IsforAll.SelectedValue = View_advanced_data.list[0].isforAll == true ? "1" : "0";//適用全部門
            this.ddl_ViewAdvanced_PermissionDepartmentID.SelectedValue = string.IsNullOrEmpty(View_advanced_data.list[0].Permission_departmentID) ? "" : this.ddl_ViewAdvanced_PermissionDepartmentID.Items.FindByValue(View_advanced_data.list[0].Permission_departmentID.ToString()).Value;//權限單位
            this.hid_ViewAdvanced_ActivityPersonnelID.Value = string.IsNullOrEmpty(View_advanced_data.list[0].addActivityPersonnel) ? "" : View_advanced_data.list[0].addActivityPersonnel.ToString();//加簽人員
            //額外動態欄位
            if (View_advanced_data.list[0].CDF_ID_T != null) {
                var gCDF_ID_T = View_advanced_data.list[0].CDF_ID_T.Split(',');
                for (var i = 0; i < gCDF_ID_T.Count(); i++) {
                    this.ckl_DisplayOtherField.Items.FindByValue(gCDF_ID_T[i]).Selected = true;
                    this.lbl_cklValues.Text += gCDF_ID_T[i].ToString();
                }
            }
            //範本下載
            if (View_advanced_data.list[0].CDF_ID_B != null) {
                var gCDF_ID_B = View_advanced_data.list[0].CDF_ID_B.Split(',');
                for (var i = 0; i < gCDF_ID_B.Count(); i++) {
                    this.chk_TemplateDownload.Items.FindByValue(gCDF_ID_B[i]).Selected = true;
                    this.lbl_cklValues.Text += gCDF_ID_B[i].ToString();
                }
            }
        }
        //儲存
        protected void btn_View_advancedSave_Click(object sender, EventArgs e) {
            string ckl_id_save = "";
            //額外動態欄位
            for (int i = 0; i < this.ckl_DisplayOtherField.Items.Count; i++) {
                if (this.ckl_DisplayOtherField.Items[i].Selected) {
                    ckl_id_save += this.ckl_DisplayOtherField.Items[i].Value;
                }
            }
            //範本下載
            for (int i = 0; i < this.chk_TemplateDownload.Items.Count; i++) {
                if (this.chk_TemplateDownload.Items[i].Selected) {
                    ckl_id_save += this.chk_TemplateDownload.Items[i].Value;
                }
            }

            //如果使用者有更新checkboxlist
            if (this.lbl_cklValues.Text != ckl_id_save) {
                FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter View_advanced_forList = new FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter {
                    desc = true,
                    orderField = "Id",
                    pageIndex = 1,
                    pageSize = int.MaxValue,
                    id = int.Parse(query_id),
                    cateogryId = int.Parse(query_id)
                };

                //判斷該筆資料是否已存在(目前使用cateogryId當條件)
                FormOptionsSettingViewModel.ERACategoryDynamicFieldListResult list_ret = this.formOptSvc.getERACategoryDynamicFieldListResult(View_advanced_forList);
                CommonViewModel.Result updateRet_ERACDF;
                Dictionary<string, object> dicObj_ERACDF = new Dictionary<string, object>();
                //新增ERACategoryDynamicField項目作法
                FormOptionsSettingViewModel.ERACategoryDynamicFieldResult add_ret_ERAF;
                ERACategoryDynamicField ef;

                if (list_ret.count > 0) {
                    dicObj_ERACDF.Add("deleted", true);//更新原有舊資料deleted為1
                    //因為有多筆所以使用迴圈
                    for (int i = 0; i < list_ret.count; i++) {
                        updateRet_ERACDF = this.formOptSvc.updateERACategoryDynamicField(list_ret.list[i].Id, dicObj_ERACDF);//ERACategoryDynamicField
                    }
                }
                //將使用者新勾選資料新增
                for (int i = 0; i < this.ckl_DisplayOtherField.Items.Count; i++) {
                    if (this.ckl_DisplayOtherField.Items[i].Selected) {
                        ef = new ERACategoryDynamicField() {
                            dynamicFieldSettingId = long.Parse(ckl_DisplayOtherField.Items[i].Value),
                            cateogryId = long.Parse(query_id),
                            deleted = false,
                            dateCreated = DateTime.Now
                        };

                        add_ret_ERAF = this.formOptSvc.addERACategoryDynamicField(ef);
                        this.lt_akert_msg.Text += (!add_ret_ERAF.success) ? UtilitySvc.alertMsg(add_ret_ERAF.resultException) : "";
                    }
                }

                for (int i = 0; i < this.chk_TemplateDownload.Items.Count; i++) {
                    if (this.chk_TemplateDownload.Items[i].Selected) {
                        ef = new ERACategoryDynamicField() {
                            dynamicFieldSettingId = long.Parse(chk_TemplateDownload.Items[i].Value),
                            cateogryId = long.Parse(query_id),
                            deleted = false,
                            dateCreated = DateTime.Now
                        };

                        add_ret_ERAF = this.formOptSvc.addERACategoryDynamicField(ef);
                        this.lt_akert_msg.Text += (!add_ret_ERAF.success) ? UtilitySvc.alertMsg(add_ret_ERAF.resultException) : "";
                    }
                }
            }

            this.lbl_cklValues.Text = "";//清空隱藏判斷欄位

            //修改ERACategoryForInf
            Dictionary<string, object> dicObj_ERAC = new Dictionary<string, object>();

            //處理人員
            dicObj_ERAC.Add("personnelID", this.hid_ViewAdvanced_PersonnelID.Value);
            //if (this.hid_ViewAdvanced_PersonnelID.Value != "") {
            //    dicObj_ERAC.Add("personnelID", this.hid_ViewAdvanced_PersonnelID.Value);
            //}
            //加簽人員
            dicObj_ERAC.Add("addActivityPersonnelID", this.hid_ViewAdvanced_ActivityPersonnelID.Value);
            //if (this.hid_ViewAdvanced_ActivityPersonnelID.Value != "") {
            //    dicObj_ERAC.Add("addActivityPersonnelID", this.hid_ViewAdvanced_ActivityPersonnelID.Value);
            //}
            //部門名稱
            dicObj_ERAC.Add("departmentID", this.ddl_ViewAdvanced_DepartmentID.SelectedValue == "" ? this.hid_ViewAdvanced_departmentID.Value : this.ddl_ViewAdvanced_DepartmentID.SelectedValue);
            //dicObj_ERAC.Add("departmentID", this.hid_ViewAdvanced_departmentID.Value);
            //if (this.hid_ViewAdvanced_PersonnelID.Value != "") {
            //    dicObj_ERAC.Add("departmentID", this.hid_ViewAdvanced_departmentID.Value);
            //} else {
            //    dicObj_ERAC.Add("departmentID", "");
            //}
            //if (this.hid_ViewAdvanced_departmentID.Value != "") {
            //    dicObj_ERAC.Add("departmentID", this.hid_ViewAdvanced_departmentID.Value);
            //}
            //主類別
            var vpID = this.ddl_ViewAdvanced_PartentID.SelectedValue == "-" ? "-1" : this.ddl_ViewAdvanced_PartentID.SelectedValue;
            dicObj_ERAC.Add("parentID", long.Parse(vpID));
            dicObj_ERAC.Add("name", this.txt_ViewAdvanced_ClassName.Text.Trim());//類別名稱
            dicObj_ERAC.Add("dicisionProcessLevel", this.ddl_ViewAdvanced_DicisionProcessLevel.SelectedValue);//核決層級

            CommonViewModel.Result updateRet_ERAC = this.formOptSvc.updateERACategoryForInf(long.Parse(query_id), dicObj_ERAC);//ERACategoryForInf

            //修改ERACategoryForInf
            Dictionary<string, object> dicObj_ERAP = new Dictionary<string, object>();

            bool bIsforAll = this.ddl_ViewAdvanced_IsforAll.SelectedValue == "0" ? false : true;

            dicObj_ERAP.Add("isforAll", bIsforAll);//是否適用全單位
            dicObj_ERAP.Add("departmentID", this.ddl_ViewAdvanced_PermissionDepartmentID.SelectedValue);//權限單位

            CommonViewModel.Result updateRet_ERAP = this.formOptSvc.updateERAPermissionForInf(long.Parse(query_id), dicObj_ERAP);//ERAPermissionForInf

            //若不成功 則要顯示錯誤訊息
            if (!updateRet_ERAC.success || !updateRet_ERAP.success) {
                this.lt_akert_msg.Text += updateRet_ERAC.resultException + updateRet_ERAP.resultException;
            } else {
                this.lt_akert_msg.Text = UtilitySvc.alertMsg("儲存成功");
                this.AdvancedReload();
                this.MultiView.ActiveViewIndex = 1;
            }
        }
        //返回
        protected void btn_View_advancedBack_Click(object sender, EventArgs e) {
            this.MultiView.ActiveViewIndex = 0;
            Response.Redirect("FormClassSettingForInfoSystemApply.aspx?pg=" + pg_str + "&k=" + Request.QueryString["k"].ToString() + "&c=" + Request.QueryString["c"].ToString());
        }
    }
}
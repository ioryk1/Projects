using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebServiceForEFGP.Models.Services;
using WebServiceForEFGP.Models.VeiwModel;
using WebServiceForEFGP.Models;

namespace WebServiceForEFGP.Manage {
    public partial class OilTransportationSubsidy : System.Web.UI.Page {

        private List<ListItem> years = null;
        private List<ListItem> months = null;

        private FormOptionSettingSvc formOptionSettingSvc = null;

        protected void Page_Load(object sender, EventArgs e) {

            this.formOptionSettingSvc = new FormOptionSettingSvc();            

            if (!Page.IsPostBack) {
                this.years = this.getYearOptions();
                this.months = this.getMonthOptions();

                this.ddl_oilTransSubsidy_search_year.DataSource = years;
                this.ddl_oilTransSubsidy_search_year.DataBind();

                this.ddl_oilTransSubsidy_year.DataSource = years;
                this.ddl_oilTransSubsidy_year.DataBind();

                this.ddl_oilTransSubsidy_month.DataSource = months;
                this.ddl_oilTransSubsidy_month.DataBind();


                this.reload();
            }


        }

        protected void reload() {
            string search_year = this.ddl_oilTransSubsidy_search_year.SelectedValue;

            if (string.IsNullOrEmpty(search_year)) {
                search_year = DateTime.Now.Year.ToString();
            }


            FormOptionsSettingViewModel.OilTransportationSubsidyParameter param = new FormOptionsSettingViewModel.OilTransportationSubsidyParameter() {
                dateEnd = DateTime.Parse(search_year + "/12/31 23:59:59"),
                dateStart = DateTime.Parse(search_year + "/01/01 00:00:00"),
                desc = true,
                orderField = "dateStart",
                pageIndex = 1,
                pageSize = 100,
                transTypes = new List<string>()
            };

            FormOptionsSettingViewModel.OilTransportationSubsidyListResult list_ret = this.formOptionSettingSvc.getOilTransportationSubsidyListResult(param);
                        
            List<page_oilTransportitationSubsidy> list = new List<page_oilTransportitationSubsidy>();

            foreach (var o in list_ret.list) {
                string currentY = o.dateStart.ToString("yyyy/MM");
                page_oilTransportitationSubsidy p_o = null;
                if (list.Exists(x => x.ym_str == currentY)) {
                    p_o = list.First(x => x.ym_str == currentY);
                } else {
                     p_o = new page_oilTransportitationSubsidy() {
                        ym_str = o.dateStart.ToString("yyyy/MM"),
                        car_price = 0d,
                        motorcycle_price = 0d
                    };
                    list.Add(p_o);
                }
                p_o = this.setOilSetting(o, p_o);
            }

            this.rpt_oilTransportationSubsidy_list.DataSource = list;
            this.rpt_oilTransportationSubsidy_list.DataBind();


        }

        protected page_oilTransportitationSubsidy setOilSetting(WebServiceForEFGP.Models.OilTransportationSubsidy o,
            page_oilTransportitationSubsidy p_o) {
            page_oilTransportitationSubsidy ret = p_o;
            switch (o.type) {
                case "car":
                    p_o.car_price = Math.Round(o.subsidyPrice, 2);
                    p_o.car_id = o.id;
                    break;
                case "motorcycle":
                    p_o.motorcycle_price = Math.Round(o.subsidyPrice, 2);
                    p_o.motorcycle_id = o.id;
                    break;
            }
            return ret;
        }


        /// <summary>
        /// 取得年分
        /// </summary>
        /// <returns></returns>
        protected List<ListItem> getYearOptions() {
            List<ListItem> ret = new List<ListItem>();

            int startYear = 2015;

            int endYear = DateTime.Now.Year + 2;

            ret.Add(new ListItem() { Text = "- 請選擇 -", Value = "" });
            for (var i = startYear; i <= endYear; i++) {
                ret.Add(new ListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return ret;
        }

        /// <summary>
        /// 取得年分
        /// </summary>
        /// <returns></returns>
        protected List<ListItem> getMonthOptions() {
            List<ListItem> ret = new List<ListItem>();

            ret.Add(new ListItem() { Text = "- 請選擇 -", Value = "" });
            for (var i = 1; i <= 12; i++) {
                ret.Add(new ListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return ret;
        }

        protected void btn_oilTransSubsidy_add_Click(object sender, EventArgs e) {
            this.lt_msg.Text = "";
            string year = this.ddl_oilTransSubsidy_year.SelectedValue;
            string month = this.ddl_oilTransSubsidy_month.SelectedValue;

            DateTime dateStart = DateTime.Parse(year + "/" + month + "/01 00:00:00");
            int thisMonthLastDay = dateStart.AddMonths(1).AddDays(-1).Day;
            DateTime dateEnd = DateTime.Parse(year + "/" + month + "/" + thisMonthLastDay + " 23:59:59");


            //check is exist 
            FormOptionsSettingViewModel.OilTransportationSubsidyParameter param = new FormOptionsSettingViewModel.OilTransportationSubsidyParameter() {
                dateEnd = dateEnd,
                dateStart = dateStart,
                desc = true,
                orderField = "dateStart",
                pageIndex = 1,
                pageSize = 100,
                transTypes = new List<string>()
            };

            FormOptionsSettingViewModel.OilTransportationSubsidyListResult list_ret = this.formOptionSettingSvc.getOilTransportationSubsidyListResult(param);            
            
            if (list_ret.count > 0) {
                this.lt_msg.Text = UtilitySvc.alertMsg("選取的時間已存在列表中，請用修改的方式修改補助金額");
                return;
            }
            //add
            WebServiceForEFGP.Models.OilTransportationSubsidy o_car = new Models.OilTransportationSubsidy() {
                dateCreated = DateTime.Now,
                dateEnd = dateEnd,
                dateStart = dateStart,
                subsidyPrice = double.Parse(this.txt_oilTransSubsidy_car_price.Text),
                type = "car"
            };
            WebServiceForEFGP.Models.OilTransportationSubsidy o_motorcycle = new Models.OilTransportationSubsidy() {
                dateCreated = DateTime.Now,
                dateEnd = dateEnd,
                dateStart = dateStart,
                subsidyPrice = double.Parse(this.txt_oilTransSubsidy_motorcycle_price.Text),
                type = "motorcycle"
            };

            this.formOptionSettingSvc.addOilTransportationSubsidy(o_car);
            this.formOptionSettingSvc.addOilTransportationSubsidy(o_motorcycle);

            //clear field
            this.ddl_oilTransSubsidy_month.SelectedValue = "";
            this.ddl_oilTransSubsidy_year.SelectedValue = "";
            this.txt_oilTransSubsidy_car_price.Text = "";
            this.txt_oilTransSubsidy_motorcycle_price.Text = "";


            this.reload();
        }

        /// <summary>
        /// 驗證是否為數字
        /// </summary>        
        protected bool isDouble(string str) {

            bool ret = false;

            try {
                double ret_d = 0;

                bool parseSuccess = double.TryParse(str, out ret_d);

                ret = parseSuccess;
            } catch (Exception ex) {
                ret = false;
            }

            return ret;

        }

        protected class page_oilTransportitationSubsidy {
            public double car_price { get; set; }
            public double motorcycle_price { get; set; }
            public string ym_str { get; set; } 

            public long car_id { get; set; }
            public long motorcycle_id { get; set; }
        }

        protected void rpt_oilTransportationSubsidy_list_ItemCommand(object source, RepeaterCommandEventArgs e) {
            this.lt_msg.Text = "";


            HiddenField hid_car_id = e.Item.FindControl("hid_car_id") as HiddenField;
            HiddenField hid_motorcycle_id = e.Item.FindControl("hid_motorcycle_id") as HiddenField;

            long car_id = long.Parse(hid_car_id.Value);
            long motorcycle_id = long.Parse(hid_motorcycle_id.Value);

            TextBox txt_oilTransportationSubsidy_list_car = e.Item.FindControl("txt_oilTransportationSubsidy_list_car") as TextBox;
            TextBox txt_oilTransportationSubsidy_list_motorcycle = e.Item.FindControl("txt_oilTransportationSubsidy_list_motorcycle") as TextBox;


            switch (e.CommandName) {
                case "update":
                    Dictionary<string, object> dic_car = new Dictionary<string, object>();
                    dic_car.Add("subsidyPrice", double.Parse(txt_oilTransportationSubsidy_list_car.Text));

                    Dictionary<string, object> dic_motorcycle = new Dictionary<string, object>();
                    dic_motorcycle.Add("subsidyPrice", double.Parse(txt_oilTransportationSubsidy_list_motorcycle.Text));

                    this.formOptionSettingSvc.updateOilTransportationSubsidy(car_id, dic_car);
                    this.formOptionSettingSvc.updateOilTransportationSubsidy(motorcycle_id, dic_motorcycle);

                    this.lt_msg.Text = UtilitySvc.alertMsg( "更改成功");

                    break;
                case "delete":

                    this.formOptionSettingSvc.deleteOilTransportationSubsidy(car_id);
                    this.formOptionSettingSvc.deleteOilTransportationSubsidy(motorcycle_id);

                    this.lt_msg.Text = UtilitySvc.alertMsg("刪除成功");
                    break;
            }

            this.reload();

        }

        protected void ddl_oilTransSubsidy_search_year_SelectedIndexChanged(object sender, EventArgs e) {

            this.lt_msg.Text = "";

            this.reload();

        }
    }
}
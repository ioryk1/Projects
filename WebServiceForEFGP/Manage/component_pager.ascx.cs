using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebServiceForEFGP.Manage {
    public partial class component_pager : System.Web.UI.UserControl {

        public component_pager() {
            this.currentPage = -1;
        }

        public int count { get; set; }
        public int pageSize { get; set; }
        public int currentPage { get; set; }
        public int showPageItem { get; set; }
        public string queryString { get; set; }
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                this.load();
            }
        }

        public void load() {

            if (count == 0) {
                this.Visible = false;
                return;
            }

            this.Visible = true;

            /* set */
            string pg = Request.QueryString["pg"] as string;

            if (currentPage == -1 || string.IsNullOrEmpty(pg)) {
                pg = "1";
            } else if (currentPage != -1) {
                pg = currentPage.ToString();
            }


            int current_page_int = 1;
            int.TryParse(pg, out current_page_int);

            List<pgItem> pgItemList = new List<pgItem>();

            string path = this.getUrl();
            int maxPage = (int)Math.Ceiling((double)this.count / this.pageSize);


            if (current_page_int > maxPage || current_page_int < 0) {
                current_page_int = 1;
            }

            int pageLength = (int)Math.Floor((double)this.showPageItem / 2);

            int startPage = (current_page_int - pageLength) < 1 ? 1 : current_page_int - pageLength;
            int endPage = (current_page_int + pageLength) > maxPage ? maxPage : current_page_int + pageLength;


            /* 目前長度 */
            int current_total_pages = endPage - startPage + 1;

            /*要修正的長度*/
            int fix_pages = this.showPageItem - current_total_pages;

            /* 若長度小於指定長度則要嘗試修正 */
            if (current_total_pages < this.showPageItem) {
                startPage = this.getMinPage(fix_pages, startPage);
            }

            current_total_pages = endPage - startPage + 1;
            fix_pages = this.showPageItem - current_total_pages;
            if (current_total_pages < this.showPageItem) {
                endPage = this.getMaxPage(fix_pages, endPage, maxPage);
            }



            /* 產生頁數 */
            for (int i = startPage; i <= endPage; i++) {

                if (i == current_page_int) {
                    pgItemList.Add(new pgItem(current_page_int.ToString(), current_page_int, false, "", true));
                } else {
                    pgItemList.Add(new pgItem(i.ToString(), i, true, this.addQueyString(path, this.queryString, i.ToString())));
                }


            }

            /*  first / end / pre / next  page  handler*/
            pgItemList.Insert(0, new pgItem("第一頁", 1, 1 == current_page_int, (current_page_int == 1) ? "" : this.addQueyString(path, this.queryString, "1")));


            pgItemList.Insert(1, new pgItem("上一頁", current_page_int - 1 <= 0 ? 1 : current_page_int - 1,
                            current_page_int - 1 <= 0,
                            this.addQueyString(path, this.queryString, current_page_int - 1 <= 0 ? "1" : (current_page_int - 1).ToString())));



            pgItemList.Add(new pgItem("下一頁",
                                current_page_int + 1 > maxPage ? maxPage : current_page_int + 1,
                                current_page_int + 1 > maxPage,
                                this.addQueyString(path, this.queryString, current_page_int + 1 > maxPage ? maxPage.ToString() : (current_page_int + 1).ToString())));

            pgItemList.Add(new pgItem("最後頁",
                                    maxPage,
                                    current_page_int == maxPage,
                                    (current_page_int == maxPage) ? "" :
                                                this.addQueyString(path, this.queryString, maxPage.ToString())));



            this.rpt_data_pager.DataSource = pgItemList;
            this.rpt_data_pager.DataBind();
        }

        public int getMinPage(int showPageItems, int currentPage) {
            int minPage = currentPage;

            for (int i = 0; i < showPageItems; i++) {
                if ((minPage - 1) <= 0) {
                    break;
                } else {
                    minPage--;
                }
            }


            return minPage;
        }

        public int getMaxPage(int showPageItems, int currentPage, int totalPages) {
            int maxPage = currentPage;

            for (int i = 0; i < showPageItems; i++) {
                if ((maxPage + 1) > totalPages) {
                    break;
                } else {
                    maxPage++;
                }
            }
            return maxPage;
        }


        protected class pgItem {
            public string text { get; set; }
            public int pg { get; set; }
            public bool disabled { get; set; }

            public string url { get; set; }

            public bool isCurrent { get; set; }


            public pgItem(string text, int pg, bool disabled, string url, bool isCurrent = false) {
                this.text = text;
                this.pg = pg;
                this.disabled = disabled;
                this.url = url;
                this.isCurrent = isCurrent;
            }

        }
        private string getUrl() {
            /* get /xxx/xxx/123.aspx */
            string path = Request.Url.AbsolutePath;

            /* filter out query string */
            bool isFirst = true;
            foreach (string key in Request.QueryString.AllKeys) {
                if (key != this.queryString) {
                    path = path + ((isFirst) ? "?" : "&") + key + "=" + Request.QueryString[key];
                    isFirst = false;
                }
            }
            return path;
        }


        public string addQueyString(string url, string key, string value) {
            if (url.IndexOf("?") > -1) {

                url = url + "&" + key + "=" + value;
            } else {
                url = url + "?" + key + "=" + value;
            }

            return url;
        }

    }
}
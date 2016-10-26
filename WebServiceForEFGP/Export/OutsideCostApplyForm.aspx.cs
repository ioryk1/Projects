using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebServiceForEFGP.Models;
using WebServiceForEFGP.Models.VeiwModel;
using WebServiceForEFGP.Models.Services;

namespace WebServiceForEFGP.Export {
    public partial class OutsideCostApplyForm : System.Web.UI.Page {
        private AccountItemSvc actSvc = null;
        private ProcessSvc procSvc = null;

        private AccountItemViewModel.OutsideApplyFormInstance ins = null;
        private ProcessViewModel.ProcessHistoryListResult procHistoryIns = null;
        private int list_sort = 0;

        protected void Page_Load(object sender, EventArgs e) {

            this.actSvc = new AccountItemSvc();
            this.procSvc = new ProcessSvc();



            string procSerialNumber = Page.Request.QueryString["p"] as string;

            if (string.IsNullOrEmpty(procSerialNumber)) {
                this.lt_alert.Text = UtilitySvc.alertMsg("參數錯誤，查無此表單");
                return;
            }
            AccountItemViewModel.OutsideApplyFormInstanceResult outsideRet = this.actSvc.getOutsideApplyFormInstanceResult(procSerialNumber);
            ProcessViewModel.ProcessHistoryListResult procRet = this.procSvc.getProcessHistoryByProcessSerialNumber(procSerialNumber);

            if (!outsideRet.success) {
                this.lt_alert.Text = UtilitySvc.alertMsg("參數錯誤，查無此表單");
                return;
            }

            this.ins = outsideRet.instance;
            this.procHistoryIns = procRet;


            this.init();
               

        }


        protected void init() {
            WebServiceForEFGP.Models.OutsideCostApplyForm o = this.ins.o;

            this.lb_applyDate.Text = o.txtApplyDate;
            this.lb_formSerialNumber.Text = o.SerialNumber;
            this.lb_issuer_id.Text = o.txtIssuerId;
            this.lb_issuer_name.Text = o.txtIssuerName;
            this.lb_issuer_dept_id.Text = o.txtIssuerDepartmentId;
            this.lb_issuer_dept_name.Text = o.txtIssuerDepartmentName;
            this.lb_issuer_ext_no.Text = o.txtIssuerExtNo;
            this.lb_applyer_id.Text = o.txtApplyerId;
            this.lb_applyer_name.Text = o.txtApplyerName;
            this.lb_applyer_dept_id.Text = o.txtApplyerDepartmentId;
            this.lb_applyer_dept_name.Text = o.txtApplyerDepartmentName;
            this.lb_applyer_ext_no.Text = o.txtApplyerExtNo;

            this.lb_applyCategory.Text = this.lb_applyCategory.Text = this.actSvc.getCashApplyTypeList().list.First(x => x.key == o.ddlApplyCategory).name;
            this.lb_retailId.Text = o.txtRetailId;
            this.lb_retailName.Text = o.txtRetailName;

            this.lb_payeeId.Text = o.txtPayeeId;
            this.lb_payeeName.Text = o.txtPayeeName;
            this.lb_paymentTitle.Text = o.txtPaymentTitle;

            if (this.ins.o_gd_statement_list.Count > 0) {
                this.rpt_statement_list.DataSource = this.ins.o_gd_statement_list;
                this.rpt_statement_list.DataBind();

            }

            if (this.ins.o_gd_private_list.Count > 0) {
                this.block_private_trans_content.Visible = true;
                this.block_private_trans_header.Visible = true;


                this.lb_txtTotalAmount_oilTransportationSubsidyPrice.Text = this.ins.o.txtTotalAmount_oilTransportationSubsidyPrice;
                this.lb_txtTotalAmount_etagPrice.Text = this.ins.o.txtTotalAmount_etagPrice;

                this.rpt_privateTrans_list.DataSource = this.ins.o_gd_private_list;
                this.rpt_privateTrans_list.DataBind();

            } else {
                this.block_private_trans_content.Visible = false;
                this.block_private_trans_header.Visible = false;
            }

            
            if (this.ins.o.radioPrivateTrans_subsidyLimit == "earnMileage") {
                //里程數計算
                this.block_earnMileage_check.Visible = true;
            } else {
                //私車公用補助上限
                this.block_subsidyLimit_check.Visible = true;
            }


            this.lb_txtPrivateTrans_earnMileageSubsidyAmount.Text = this.ins.o.txtPrivateTrans_earnMileageSubsidyAmount;
            this.lb_txtPrivateTrans_subsidyLimitAmount.Text = this.ins.o.txtPrivateTrans_subsidyLimitAmount;

            this.lb_txtPrivateTrans_sumPaymentCertificatePrice.Text = this.ins.o.txtPrivateTrans_sumPaymentCertificatePrice;
            this.lb_txtPrivateTrans_sumStatementAmount.Text = this.ins.o.txtPrivateTrans_sumStatementAmount;
            this.lb_txtPrivateTrans_sumStatementWithoutAmount.Text = this.ins.o.txtPrivateTrans_sumStatementWithoutAmount;
            this.lb_txtPrivateTrans_sumETagPrice.Text = this.ins.o.txtPrivateTrans_sumETagPrice;
            this.lb_txtSumTotalPrice.Text = this.ins.o.txtSumTotalPrice;

            this.lb_txtPublicTrans_sumPaymentCertificateAmount.Text = this.ins.o.txtPublicTrans_sumPaymentCertificateAmount;
            this.lb_txtPublicTrans_sumPaymentCertificateWithoutTaxAmount.Text = this.ins.o.txtPublicTrans_sumPaymentCertificateWithoutTaxAmount;
            this.lb_txtPublicTrans_sumTaxAmount.Text = this.ins.o.txtPublicTrans_sumTaxAmount;
            this.lb_txtPublicTrans_sumStatementAmount.Text = this.ins.o.txtPublicTrans_sumStatementAmount;
            this.lb_txtPublicTrans_sumStatementWithoutTaxAmount.Text = this.ins.o.txtPublicTrans_sumStatementWithoutTaxAmount;
            this.lb_txtPrivateTrans_sumTaxAmount.Text = this.ins.o.txtPrivateTrans_sumTaxPrice;

            //計算稅額總額(將字串轉乘數字)
            int PrivateTrans_sumTaxPrice = 0;
            int PublicTrans_sumTaxPrice = 0;

            if (!string.IsNullOrEmpty(this.ins.o.txtPrivateTrans_sumTaxPrice.Trim())) {
                int.TryParse(this.ins.o.txtPrivateTrans_sumTaxPrice.Trim(), out PrivateTrans_sumTaxPrice);
            }

            if (!string.IsNullOrEmpty(this.ins.o.txtPublicTrans_sumTaxAmount.Trim())) {
                int.TryParse(this.ins.o.txtPublicTrans_sumTaxAmount.Trim(), out PublicTrans_sumTaxPrice);
            }

            this.lb_txtSumTaxAmount.Text = (PrivateTrans_sumTaxPrice + PublicTrans_sumTaxPrice).ToString();
            

            if (procHistoryIns.success) {
                this.rpt_process_list.DataSource = procHistoryIns.list;
                this.rpt_process_list.DataBind();
            }



        }




    }
}
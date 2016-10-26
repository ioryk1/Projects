using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceForEFGP.Models.VeiwModel {
    public class FormOptionsSettingViewModel {

        public class FormOptionsQueryParameter : CommonViewModel.ListQueryParameter {

            public string formOID { get; set; }

            public string formNo { get; set; }

            public string fieldID { get; set; }


            public FormOptionsQueryParameter() : base() {
                this.formOID = "";
                this.formNo = "";
                this.fieldID = "";
            }

        }

        public class FormOptionsListResult : CommonViewModel.ListResult {
            public List<FormOptionsSetting> list { get; set; }

            public FormOptionsListResult() : base() {
                this.list = new List<FormOptionsSetting>();
            }

        }

        public class FormOptionsResult : CommonViewModel.Result {
            public FormOptionsSetting f { get; set; }

            public FormOptionsResult() : base() {
                this.f = null;
            }

        }


        public class FormOptionLight {
            public string formName { get; set; }
            public string formOID { get; set; }
            public string formNo { get; set; }
            public List<FormOptionFieldLight> fields { get; set; }


            public FormOptionLight() {
                this.formName = "";
                this.formOID = "";
                this.formNo = "";

                this.fields = new List<FormOptionFieldLight>();
            }
        }

        public class FormOptionFieldLight {
            public string formOID { get; set; }
            public string formNo { get; set; }
            public string fieldName { get; set; }
            public string fieldID { get; set; }
            public string type { get; set; }
            public string key { get; set; }
            public string extData1 { get; set; }
            public string extData2 { get; set; }
            public List<FormOptionFieldOptionLight> options { get; set; }

            public FormOptionFieldLight() {
                this.formOID = "";
                this.formNo = "";
                this.fieldName = "";
                this.fieldID = "";
                this.type = "";
                this.key = "";
                this.extData1 = "";
                this.extData2 = "";
                this.options = new List<FormOptionFieldOptionLight>();

            }


        }

        public class FormOptionFieldOptionLight {
            public string value { get; set; }
            public string text { get; set; }
            public int sort { get; set; }
            public bool needKeyIn { get; set; }


            public FormOptionFieldOptionLight() {
                this.value = "";
                this.text = "";
                this.sort = 0;
                this.needKeyIn = false;
            }

        }

        public class FormOptionFieldLightListResult : CommonViewModel.ListResult {
            public List<FormOptionFieldLight> fields { get; set; }

            public FormOptionFieldLightListResult() {
                this.fields = new List<FormOptionFieldLight>();
            }
        }

        public class OilTransportationSubsidyParameter : CommonViewModel.ListQueryParameter {
            public Nullable<DateTime> dateStart { get; set; }
            public Nullable<DateTime> dateEnd { get; set; }
            public List<string> transTypes { get; set; }

            public OilTransportationSubsidyParameter() : base() {
                this.dateEnd = null;
                this.dateStart = null;
                this.transTypes = new List<string>();
            }

        }

        public class OilTransportationSubsidyListResult : CommonViewModel.ListResult {
            public List<OilTransportationSubsidy> list { get; set; }

            public OilTransportationSubsidyListResult() : base() {
                this.list = new List<OilTransportationSubsidy>();
            }
        }

        public class OilTransportationSubsidyResult : CommonViewModel.Result {
            public OilTransportationSubsidy o { get; set; }

            public OilTransportationSubsidyResult() : base() {
                this.o = null;
            }

        }

        public class EmployeeTransportationSettingQueryParameter : CommonViewModel.ListQueryParameter {
            public string keyword { get; set; }
            public string transType { get; set; }
            public string empNo { get; set; }

            public EmployeeTransportationSettingQueryParameter() : base() {
                this.keyword = "";
                this.transType = "";
                this.empNo = "";
            }
        }


        public class ViewEmployeeTransportationSettingListResult : CommonViewModel.ListResult {
            public List<ViewEmployeeTransportationSetting> list { get; set; }

            public ViewEmployeeTransportationSettingListResult() : base() {
                this.list = new List<ViewEmployeeTransportationSetting>();
            }
        }

        public class ViewEmployeeTransportationSettingResult : CommonViewModel.Result {
            public ViewEmployeeTransportationSetting setting { get; set; }

            public ViewEmployeeTransportationSettingResult() : base() {
                this.setting = null;
            }
        }

        /*2016/05/09 Stephen Add*/
        public class FormClassSettingForInfoSystemApplyQueryParameter : CommonViewModel.ListQueryParameter {
            public string keyword { get; set; }
            public int classType { get; set; }
            public int id { get; set; }
            public string personnelID { get; set; }
            //public string personnelName { get; set; }
            public string name { get; set; }
            public string departmentID { get; set; }
            public string departmentName { get; set; }
            public string dicisionProcessLevel { get; set; }
            public int dynamicFieldSettingId { get; set; }
            public int cateogryId { get; set; }

            public FormClassSettingForInfoSystemApplyQueryParameter() : base() {
                this.keyword = "";
                this.classType = 0;
                this.id = 0;
                this.personnelID = "";
                this.name = "";
                this.departmentID = "";
                this.departmentName = "";
                this.dicisionProcessLevel = "";
                this.orderField = "ID";
                this.dynamicFieldSettingId = 0;
                this.cateogryId = 0;
            }
        }

        //ERACategoryForInf
        public class ERACategoryForInfListResult : CommonViewModel.ListResult {
            public List<ERACategoryForInf> list { get; set; }

            public ERACategoryForInfListResult() : base() {
                this.list = new List<ERACategoryForInf>();
            }
        }

        public class ERACategoryForInfResult : CommonViewModel.Result {
            public ERACategoryForInf setting { get; set; }

            public ERACategoryForInfResult() : base() {
                this.setting = null;
            }
        }

        //ERAPermissionForInf
        public class ERAPermissionForInfListResult : CommonViewModel.ListResult {
            public List<ERAPermissionForInf> list { get; set; }

            public ERAPermissionForInfListResult() : base() {
                this.list = new List<ERAPermissionForInf>();
            }
        }

        public class ERAPermissionForInfResult : CommonViewModel.Result {
            public ERAPermissionForInf setting { get; set; }

            public ERAPermissionForInfResult() : base() {
                this.setting = null;
            }
        }

        //ViewERACategoryPermissionForInf
        public class ViewERACategoryPermissionForInfListResult : CommonViewModel.ListResult {
            public List<ViewERACategoryPermissionForInf> list { get; set; }

            public ViewERACategoryPermissionForInfListResult() : base() {
                this.list = new List<ViewERACategoryPermissionForInf>();
            }
        }

        public class ViewERACategoryPermissionForInfResult : CommonViewModel.Result {
            public ViewERACategoryPermissionForInf setting { get; set; }

            public ViewERACategoryPermissionForInfResult() : base() {
                this.setting = null;
            }
        }

        //ViewERAUsersOrganizationUnitDepartment
        public class ViewERAUsersOrganizationUnitDepartmentListResult : CommonViewModel.ListResult {
            public List<ViewERAUsersOrganizationUnitDepartment> list { get; set; }

            public ViewERAUsersOrganizationUnitDepartmentListResult() : base() {
                this.list = new List<ViewERAUsersOrganizationUnitDepartment>();
            }
        }

        public class ViewERAUsersOrganizationUnitDepartmentResult : CommonViewModel.Result {
            public ViewERAUsersOrganizationUnitDepartment setting { get; set; }

            public ViewERAUsersOrganizationUnitDepartmentResult() : base() {
                this.setting = null;
            }
        }

        //OrganizationUnit
        public class OrganizationUnitListResult : CommonViewModel.ListResult {
            public List<OrganizationUnit> list { get; set; }

            public OrganizationUnitListResult() : base() {
                this.list = new List<OrganizationUnit>();
            }
        }

        public class OrganizationUnitResult : CommonViewModel.Result {
            public OrganizationUnit setting { get; set; }

            public OrganizationUnitResult() : base() {
                this.setting = null;
            }
        }

        /*2016/08/17 Stephen Add*/
        //ERADynamicFieldSetting
        public class ERADynamicFieldSettingListResult : CommonViewModel.ListResult {
            public List<ERADynamicFieldSetting> list { get; set; }

            public ERADynamicFieldSettingListResult() : base() {
                this.list = new List<ERADynamicFieldSetting>();
            }
        }

        public class ERADynamicFieldSettingResult : CommonViewModel.Result {
            public ERADynamicFieldSetting setting { get; set; }

            public ERADynamicFieldSettingResult() : base() {
                this.setting = null;
            }
        }

        //ERACategoryDynamicField
        public class ERACategoryDynamicFieldListResult : CommonViewModel.ListResult {
            public List<ERACategoryDynamicField> list { get; set; }

            public ERACategoryDynamicFieldListResult() : base() {
                this.list = new List<ERACategoryDynamicField>();
            }
        }

        public class ERACategoryDynamicFieldResult : CommonViewModel.Result {
            public ERACategoryDynamicField setting { get; set; }

            public ERACategoryDynamicFieldResult() : base() {
                this.setting = null;
            }
        }

        //ViewERACategoryDynamicFieldSettingForInf
        public class ViewERACategoryPermissionDynamicFieldSettingForInfListResult : CommonViewModel.ListResult {
            public List<ViewERACategoryPermissionDynamicFieldSettingForInf> list { get; set; }

            public ViewERACategoryPermissionDynamicFieldSettingForInfListResult() : base() {
                this.list = new List<ViewERACategoryPermissionDynamicFieldSettingForInf>();
            }
        }

        public class ViewERACategoryPermissionDynamicFieldSettingForInfResult : CommonViewModel.Result {
            public ViewERACategoryPermissionDynamicFieldSettingForInf setting { get; set; }

            public ViewERACategoryPermissionDynamicFieldSettingForInfResult() : base() {
                this.setting = null;
            }
        }


        //2016/08/19 Stephen Add 主類別整合
        public class ClassificationUnit {
            public bool isforAll { get; set; }
            public bool deleted { get; set; }
            public string unit { get; set; }

            public ClassificationUnit() {
                this.isforAll = false;
                this.deleted = false;
                this.unit = "";
            }
        }

        public class ClassificationUnitListResult : CommonViewModel.ListResult {
            public List<ClassificationUnit> list { get; set; }
            public ClassificationUnitListResult() : base() {
                this.list = new List<ClassificationUnit>();
            }
        }

        //2016/09/06 Stephen Add
        public class departmentLight {
            public string id { get; set; }
            public string organizationUnitName { get; set; }

            public departmentLight() {
                this.id = "";
                this.organizationUnitName = "";
            }
        }
    }
}

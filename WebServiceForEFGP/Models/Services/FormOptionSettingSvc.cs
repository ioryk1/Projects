using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceForEFGP.Models.VeiwModel;
using WebServiceForEFGP.Models.Dao;

namespace WebServiceForEFGP.Models.Services {
    public class FormOptionSettingSvc {

        private FormOptionSettingDao formOptionSettingDao = null;

        public FormOptionSettingSvc() {
            this.formOptionSettingDao = new FormOptionSettingDao();
        }

        /// <summary>
        /// 取得自訂選項 (表單包含欄位) 列表
        /// </summary>
        /// <returns></returns>
        public List<FormOptionsSettingViewModel.FormOptionLight> getFormOptionList() {
            List<FormOptionsSettingViewModel.FormOptionLight> ret = null;
            List<FormOptionsSettingViewModel.FormOptionLight> formList = this.formOptionSettingDao.getFormOptionList();

            foreach (var f in formList) {
                List<FormFieldsSetting> fields = this.formOptionSettingDao.getCustomFormFieldsSettingListByFormID(f.formNo);
                foreach (var _field in fields) {
                    f.fields.Add(new FormOptionsSettingViewModel.FormOptionFieldLight() {
                        fieldID = _field.id.ToString(),
                        fieldName = _field.name,
                        formNo = f.formNo,
                        formOID = f.formOID,
                        type = _field.type
                    });
                }
            }

            ret = formList;


            return ret;
        }


        /// <summary>
        /// 取得自訂通知 (表單包含欄位) 列表
        /// </summary>
        /// <returns></returns>
        public List<FormOptionsSettingViewModel.FormOptionLight> getFormNotifyList() {
            List<FormOptionsSettingViewModel.FormOptionLight> ret = null;
            List<FormOptionsSettingViewModel.FormOptionLight> formList = this.formOptionSettingDao.getFormOptionList();

            foreach (var f in formList) {
                List<FormFieldsSetting> fields = this.formOptionSettingDao.getCustomNotifyFormFieldsSettingListByFormID(f.formNo);
                foreach (var _field in fields) {
                    f.fields.Add(new FormOptionsSettingViewModel.FormOptionFieldLight() {
                        fieldID = _field.id.ToString(),
                        fieldName = _field.name,
                        formNo = f.formNo,
                        formOID = f.formOID,
                        type = _field.type,
                        extData1 = _field.extData1,
                        extData2 = _field.extData2,
                        key = _field.key
                    });
                }
            }

            ret = formList;



            return ret;
        }

        
        



        public FormOptionsSettingViewModel.FormOptionsListResult getFormOptionListResult(FormOptionsSettingViewModel.FormOptionsQueryParameter param) {
            FormOptionsSettingViewModel.FormOptionsListResult ret = new FormOptionsSettingViewModel.FormOptionsListResult();

            try {
                var list_ret = this.formOptionSettingDao.getFormOptionsSettingList(param);

                ret.success = true;
                ret.list = list_ret.Item1;
                ret.count = list_ret.Item2;
                ret.resultCode = "200";

            } catch (Exception ex) {
                ret.success = false;
                ret.resultCode = "500";
                ret.resultException = ex.ToString();
            }

            return ret;
        }

        public FormOptionsSettingViewModel.FormOptionsResult addFormOptionSetting(FormOptionsSetting f) {
            FormOptionsSettingViewModel.FormOptionsResult ret = new FormOptionsSettingViewModel.FormOptionsResult();
            try {

                ret.success = true;
                ret.resultCode = "200";
                ret.f = this.formOptionSettingDao.addFormOptionSetting(f);

            } catch (Exception ex) {
                ret.success = false;
                ret.resultCode = "500";
                ret.resultMessage = ex.ToString();
            }

            return ret;

        }

        public FormOptionsSettingViewModel.FormOptionsResult updateFormOptionSetting(long id, Dictionary<string, object> dicObj) {
            FormOptionsSettingViewModel.FormOptionsResult ret = new FormOptionsSettingViewModel.FormOptionsResult();

            try {
                ret.success = this.formOptionSettingDao.updateFormOptionSetting(id, dicObj);
                ret.resultCode = "200";
            } catch (Exception ex) {
                ret.success = false;
                ret.resultCode = "500";
                ret.resultException = ex.ToString();
            }
            return ret;

        }

        public CommonViewModel.Result updateFormFieldSettion(string id, Dictionary<string, object> dicObj) {
            CommonViewModel.Result ret = new CommonViewModel.Result();
            try {
                ret.success = this.formOptionSettingDao.updateFormFieldSetting(id, dicObj);
                ret.resultCode = "200";                 
            } catch (Exception ex) {
                ret.success = true;
                ret.resultCode = "500";
                ret.resultException = ex.ToString();
            }
            return ret;
        }

        public FormOptionsSettingViewModel.FormOptionFieldLightListResult getOptionFieldListResultByFormID(string formID) {
            FormOptionsSettingViewModel.FormOptionFieldLightListResult ret = new FormOptionsSettingViewModel.FormOptionFieldLightListResult();

            try {
                ret.success = true;
                ret.resultCode = "200";

                List<FormFieldsSetting> fields = this.formOptionSettingDao.getCustomFormFieldsSettingListByFormID(formID);

                foreach (var f in fields) {
                    FormOptionsSettingViewModel.FormOptionFieldLight f_l = new FormOptionsSettingViewModel.FormOptionFieldLight() {
                        fieldID = f.id.ToString(),
                        fieldName = f.name,
                        key = f.key,
                        formNo = f.formID,
                        formOID = f.formID,
                        type = f.type,
                        extData1 = f.extData1,
                        extData2 = f.extData2,
                        options = new List<FormOptionsSettingViewModel.FormOptionFieldOptionLight>()
                    };

                    //get options by fieldsID 
                    List<FormOptionsSetting> options = this.formOptionSettingDao.getFormOptionsSettingListByFieldID(f.id);                    
                    foreach (var o in options) {
                        FormOptionsSettingViewModel.FormOptionFieldOptionLight ol = new FormOptionsSettingViewModel.FormOptionFieldOptionLight() {
                            needKeyIn = o.needKeyIn,
                            sort = (o.sort.HasValue ? o.sort.Value : 0),
                            text = o.text,
                            value = o.value
                        };
                        f_l.options.Add(ol);
                    }
                    ret.fields.Add(f_l);
                }

                ret.count = ret.fields.Count;
                ret.pageIndex = 1;
                ret.pageSize = 1;               

            } catch (Exception ex) {
                ret.success = false;
                ret.resultCode = "500";
                ret.resultException = ex.ToString();                
            }


            return ret;
        }

        public FormOptionsSettingViewModel.FormOptionFieldLightListResult getNotifyFieldListResultByFormID(string formID) {
            FormOptionsSettingViewModel.FormOptionFieldLightListResult ret = new FormOptionsSettingViewModel.FormOptionFieldLightListResult();
            try {

                List<FormFieldsSetting> fields = this.formOptionSettingDao.getCustomNotifyFormFieldsSettingListByFormID(formID);
                foreach(var f in fields){
                    ret.fields.Add(new FormOptionsSettingViewModel.FormOptionFieldLight() {
                        fieldID = f.id.ToString(),
                        fieldName = f.name,
                        key = f.key,
                        formNo = f.formID,
                        formOID = f.formID,
                        type = f.type,
                        extData1 = f.extData1,
                        extData2 = f.extData2,
                        options = new List<FormOptionsSettingViewModel.FormOptionFieldOptionLight>()
                    });
                }
                ret.success = true;
                ret.resultCode = "200";
                //ret.fields = 
               

            } catch (Exception ex) {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";                
            }

            return ret; 
        }


        public FormOptionsSettingViewModel.OilTransportationSubsidyListResult getOilTransportationSubsidyListResult(FormOptionsSettingViewModel.OilTransportationSubsidyParameter param) {
            FormOptionsSettingViewModel.OilTransportationSubsidyListResult ret = new FormOptionsSettingViewModel.OilTransportationSubsidyListResult();

            try {

                ret.success = true;                
                ret.list = this.formOptionSettingDao.getOilTransportationSubsidyList(param);
                ret.resultCode = "200";
                ret.count = ret.list.Count;
            } catch (Exception ex) {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";                
            }

            return ret;
        }

        public FormOptionsSettingViewModel.OilTransportationSubsidyResult addOilTransportationSubsidy(OilTransportationSubsidy o) {
            FormOptionsSettingViewModel.OilTransportationSubsidyResult ret = new FormOptionsSettingViewModel.OilTransportationSubsidyResult();

            try {
                ret.o = this.formOptionSettingDao.addOilTransportationSubsidy(o);
                ret.success = true;
                ret.resultCode = "200";
            } catch (Exception ex) {
                ret.success = false;
                ret.resultCode = "500";
                ret.resultException = ex.ToString();                
            }
            return ret;
        }

        public FormOptionsSettingViewModel.OilTransportationSubsidyResult updateOilTransportationSubsidy(long id, Dictionary<string, object> dic) {
            FormOptionsSettingViewModel.OilTransportationSubsidyResult ret = new FormOptionsSettingViewModel.OilTransportationSubsidyResult();

            try {
                ret.success = this.formOptionSettingDao.updateOilTransportationSubsidy(id, dic);
                ret.resultCode = "200";                
            } catch (Exception ex) {
                ret.success = false;
                ret.resultCode = "500";
                ret.resultException = ex.ToString();                
            }

            return ret;
        }

        public FormOptionsSettingViewModel.OilTransportationSubsidyResult deleteOilTransportationSubsidy(long id) {
            FormOptionsSettingViewModel.OilTransportationSubsidyResult ret = new FormOptionsSettingViewModel.OilTransportationSubsidyResult();

            try {
                ret.success = this.formOptionSettingDao.deleteOilTransportationSubsidy(id);
                ret.resultCode = "200";               

            } catch (Exception ex) {
                ret.success = false;
                ret.resultCode = "500";
                ret.resultException = ex.ToString();                
            }

            return ret;
        }
        /// <summary>
        /// 捯
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public FormOptionsSettingViewModel.ViewEmployeeTransportationSettingListResult getViewEmployeeTransportationSettingListResult(FormOptionsSettingViewModel.EmployeeTransportationSettingQueryParameter param) {
            FormOptionsSettingViewModel.ViewEmployeeTransportationSettingListResult ret = new FormOptionsSettingViewModel.ViewEmployeeTransportationSettingListResult();

            try {
                ret.success = true;
                ret.resultCode = "200";
                Tuple<List<ViewEmployeeTransportationSetting>, int> tuple_list = this.formOptionSettingDao.getViewEmployeeTransportationSettingList(param);
                ret.list = tuple_list.Item1;
                ret.count = tuple_list.Item2;

            } catch (Exception ex) {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }

            return ret;
        }

        /// <summary>
        /// 新增員工私車公用設定檔
        /// </summary>        
        public FormOptionsSettingViewModel.ViewEmployeeTransportationSettingResult addEmployeeTransportationSetting(EmployeeTransportationSetting e) {
            FormOptionsSettingViewModel.ViewEmployeeTransportationSettingResult ret = new FormOptionsSettingViewModel.ViewEmployeeTransportationSettingResult();
            try {
                ret.success = this.formOptionSettingDao.addEmployeeTransportationSetting(e) != null;
                ret.resultCode = "200";                
            } catch (Exception ex) {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }

            return ret;
        }

        /// <summary>
        /// 更新員工私車公用設定檔
        /// </summary>        
        public FormOptionsSettingViewModel.ViewEmployeeTransportationSettingResult updateEmployeeTransportationSetting(long id, Dictionary<string, object> dicObj) {
            FormOptionsSettingViewModel.ViewEmployeeTransportationSettingResult ret = new FormOptionsSettingViewModel.ViewEmployeeTransportationSettingResult();
            try {
                ret.success = this.formOptionSettingDao.updateEmployeeTransportationSetting(id, dicObj);
                ret.resultCode = "200";
            } catch (Exception ex) {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }
            return ret;
        }

        ///2016/05/09 Stephen Add
        //ddl
        public FormOptionsSettingViewModel.ERACategoryForInfListResult getERACategoryForInfMainClassListResult(FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param)
        {
            FormOptionsSettingViewModel.ERACategoryForInfListResult ret = new FormOptionsSettingViewModel.ERACategoryForInfListResult();

            try
            {
                ret.success = true;
                ret.resultCode = "200";
                Tuple<List<ERACategoryForInf>, int> tuple_list = this.formOptionSettingDao.getERACategoryForInfMainClassList(param);
                ret.list = tuple_list.Item1;
                ret.count = tuple_list.Item2;

            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }

            return ret;
        }

        //OrganizationUnit
        public FormOptionsSettingViewModel.OrganizationUnitListResult getOrganizationUnitListResult(FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param)
        {
            FormOptionsSettingViewModel.OrganizationUnitListResult ret = new FormOptionsSettingViewModel.OrganizationUnitListResult();

            try
            {
                ret.success = true;
                ret.resultCode = "200";
                Tuple<List<OrganizationUnit>, int> tuple_list = this.formOptionSettingDao.getOrganizationUnitList(param);
                ret.list = tuple_list.Item1;
                ret.count = tuple_list.Item2;

            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }

            return ret;
        }

        //2016/09/06 Stephen Add
        public List<FormOptionsSettingViewModel.departmentLight> getDepList() {
            List<FormOptionsSettingViewModel.departmentLight> ret = new List<FormOptionsSettingViewModel.departmentLight>();

            ret = this.formOptionSettingDao.getDepList("");

            return ret;
        }

        //ERACategoryForInf
        public FormOptionsSettingViewModel.ERACategoryForInfListResult getERACategoryForInfListResult(FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param)
        {
            FormOptionsSettingViewModel.ERACategoryForInfListResult ret = new FormOptionsSettingViewModel.ERACategoryForInfListResult();

            try
            {
                ret.success = true;
                ret.resultCode = "200";
                Tuple<List<ERACategoryForInf>, int> tuple_list = this.formOptionSettingDao.getERACategoryForInfList(param);
                ret.list = tuple_list.Item1;
                ret.count = tuple_list.Item2;

            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }

            return ret;
        }
     
        public FormOptionsSettingViewModel.ERACategoryForInfResult addERACategoryForInf(ERACategoryForInf e)
        {
            FormOptionsSettingViewModel.ERACategoryForInfResult ret = new FormOptionsSettingViewModel.ERACategoryForInfResult();
            try
            {
                ret.success = this.formOptionSettingDao.addERACategoryForInf(e) != null;
                ret.resultCode = "200";
            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }

            return ret;
        }

        public FormOptionsSettingViewModel.ERACategoryForInfResult updateERACategoryForInf(long id, Dictionary<string, object> dicObj)
        {
            FormOptionsSettingViewModel.ERACategoryForInfResult ret = new FormOptionsSettingViewModel.ERACategoryForInfResult();
            try
            {
                ret.success = this.formOptionSettingDao.updateERACategoryForInf(id, dicObj);
                ret.resultCode = "200";
            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }
            return ret;
        }

        //ERAPermissionForInf
        public FormOptionsSettingViewModel.ERAPermissionForInfListResult getERAPermissionForInfListResult(FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param)
        {
            FormOptionsSettingViewModel.ERAPermissionForInfListResult ret = new FormOptionsSettingViewModel.ERAPermissionForInfListResult();

            try
            {
                //ret.success = true;
                //ret.resultCode = "200";
                //Tuple<List<ERAPermissionForInf>, int> tuple_list = this.formOptionSettingDao.getERACategoryForInfList(param);
                //ret.list = tuple_list.Item1;
                //ret.count = tuple_list.Item2;

            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }

            return ret;
        }
   
        public FormOptionsSettingViewModel.ERAPermissionForInfResult addERAPermissionForInf(ERAPermissionForInf e)
        {
            FormOptionsSettingViewModel.ERAPermissionForInfResult ret = new FormOptionsSettingViewModel.ERAPermissionForInfResult();
            try
            {
                ret.success = this.formOptionSettingDao.addERAPermissionForInf(e) != null;
                ret.resultCode = "200";
            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }

            return ret;
        }

        public FormOptionsSettingViewModel.ERAPermissionForInfResult updateERAPermissionForInf(long id, Dictionary<string, object> dicObj)
        {
            FormOptionsSettingViewModel.ERAPermissionForInfResult ret = new FormOptionsSettingViewModel.ERAPermissionForInfResult();
            try
            {
                ret.success = this.formOptionSettingDao.updateERAPermissionForInf(id, dicObj);
                ret.resultCode = "200";
            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }
            return ret;
        }

        //ViewERACategoryPermissionForInf
        public FormOptionsSettingViewModel.ViewERACategoryPermissionForInfListResult getViewERACategoryPermissionForInfListResult(FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param)
        {
            FormOptionsSettingViewModel.ViewERACategoryPermissionForInfListResult ret = new FormOptionsSettingViewModel.ViewERACategoryPermissionForInfListResult();

            try
            {
                ret.success = true;
                ret.resultCode = "200";
                Tuple<List<ViewERACategoryPermissionForInf>, int> tuple_list = this.formOptionSettingDao.getViewERACategoryPermissionForInfList(param);
                ret.list = tuple_list.Item1;
                ret.count = tuple_list.Item2;

            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }

            return ret;
        }

        //ViewERAUsersOrganizationUnitDepartment
        public FormOptionsSettingViewModel.ViewERAUsersOrganizationUnitDepartmentListResult getViewERAUsersOrganizationUnitDepartmentListResult(FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param) {
            FormOptionsSettingViewModel.ViewERAUsersOrganizationUnitDepartmentListResult ret = new FormOptionsSettingViewModel.ViewERAUsersOrganizationUnitDepartmentListResult();

            try {
                ret.success = true;
                ret.resultCode = "200";
                Tuple<List<ViewERAUsersOrganizationUnitDepartment>, int> tuple_list = this.formOptionSettingDao.getViewERAUsersOrganizationUnitDepartmentList(param);
                ret.list = tuple_list.Item1;
                ret.count = tuple_list.Item2;

            } catch (Exception ex) {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }

            return ret;
        }

        //2016/08/17 Stephen Add
        //ERADynamicFieldSetting
        public FormOptionsSettingViewModel.ERADynamicFieldSettingListResult getERADynamicFieldSettingListResult(FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param) {
            FormOptionsSettingViewModel.ERADynamicFieldSettingListResult ret = new FormOptionsSettingViewModel.ERADynamicFieldSettingListResult();

            try {
                ret.success = true;
                ret.resultCode = "200";
                Tuple<List<ERADynamicFieldSetting>, int> tuple_list = this.formOptionSettingDao.getERADynamicFieldSettingList(param);
                ret.list = tuple_list.Item1;
                ret.count = tuple_list.Item2;

            } catch (Exception ex) {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }

            return ret;
        }
    
        public FormOptionsSettingViewModel.ERADynamicFieldSettingResult addERADynamicFieldSetting(ERADynamicFieldSetting e) {
            FormOptionsSettingViewModel.ERADynamicFieldSettingResult ret = new FormOptionsSettingViewModel.ERADynamicFieldSettingResult();
            try {
                ret.success = this.formOptionSettingDao.addERADynamicFieldSetting(e) != null;
                ret.resultCode = "200";
            } catch (Exception ex) {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }

            return ret;
        }

        public FormOptionsSettingViewModel.ERADynamicFieldSettingResult updateERADynamicFieldSetting(long id, Dictionary<string, object> dicObj) {
            FormOptionsSettingViewModel.ERADynamicFieldSettingResult ret = new FormOptionsSettingViewModel.ERADynamicFieldSettingResult();
            try {
                ret.success = this.formOptionSettingDao.updateERACategoryForInf(id, dicObj);
                ret.resultCode = "200";
            } catch (Exception ex) {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }
            return ret;
        }

        //ERACategoryDynamicField
        public FormOptionsSettingViewModel.ERACategoryDynamicFieldListResult getERACategoryDynamicFieldListResult(FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param) {
            FormOptionsSettingViewModel.ERACategoryDynamicFieldListResult ret = new FormOptionsSettingViewModel.ERACategoryDynamicFieldListResult();

            try {
                ret.success = true;
                ret.resultCode = "200";
                Tuple<List<ERACategoryDynamicField>, int> tuple_list = this.formOptionSettingDao.getERACategoryDynamicFieldList(param);
                ret.list = tuple_list.Item1;
                ret.count = tuple_list.Item2;

            } catch (Exception ex) {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }

            return ret;
        }

        public FormOptionsSettingViewModel.ERACategoryDynamicFieldResult addERACategoryDynamicField(ERACategoryDynamicField e) {
            FormOptionsSettingViewModel.ERACategoryDynamicFieldResult ret = new FormOptionsSettingViewModel.ERACategoryDynamicFieldResult();
            try {
                ret.success = this.formOptionSettingDao.addERACategoryDynamicField(e) != null;
                ret.resultCode = "200";
            } catch (Exception ex) {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }

            return ret;
        }

        public FormOptionsSettingViewModel.ERACategoryDynamicFieldResult updateERACategoryDynamicField(long id, Dictionary<string, object> dicObj) {
            FormOptionsSettingViewModel.ERACategoryDynamicFieldResult ret = new FormOptionsSettingViewModel.ERACategoryDynamicFieldResult();
            try {
                ret.success = this.formOptionSettingDao.updateERACategoryDynamicField(id, dicObj);
                ret.resultCode = "200";
            } catch (Exception ex) {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }
            return ret;
        }

        //ViewERACategoryPermissionDynamicFieldSettingForInf
        public FormOptionsSettingViewModel.ViewERACategoryPermissionDynamicFieldSettingForInfListResult getViewERACategoryPermissionDynamicFieldSettingForInfListResult(FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param) {
            FormOptionsSettingViewModel.ViewERACategoryPermissionDynamicFieldSettingForInfListResult ret = new FormOptionsSettingViewModel.ViewERACategoryPermissionDynamicFieldSettingForInfListResult();

            try {
                ret.success = true;
                ret.resultCode = "200";
                Tuple<List<ViewERACategoryPermissionDynamicFieldSettingForInf>, int> tuple_list = this.formOptionSettingDao.getViewERACategoryPermissionDynamicFieldSettingForInfList(param);
                ret.list = tuple_list.Item1;
                ret.count = tuple_list.Item2;

            } catch (Exception ex) {
                ret.success = false;
                ret.resultException = ex.ToString();
                ret.resultCode = "500";
            }

            return ret;
        }


        //2016/08/19 Stephen Add 主類別整合
        public FormOptionsSettingViewModel.ClassificationUnitListResult getPurviewUnitByParentID(string parentID) {
            FormOptionsSettingViewModel.ClassificationUnitListResult ret = new FormOptionsSettingViewModel.ClassificationUnitListResult();

            try {

                var list = this.formOptionSettingDao.getPurviewUnitByParentID(parentID);
                ret.list = list;
                ret.success = true;
                ret.resultCode = "200";

            } catch (Exception ex) {
                ret.success = false;
                ret.resultCode = "500";
                ret.resultException = ex.ToString();
            }


            return ret;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceForEFGP.Models;
using WebServiceForEFGP.Models.VeiwModel;
using LinqKit;
using System.Data.SqlClient;

namespace WebServiceForEFGP.Models.Dao {
    public class FormOptionSettingDao {

        //2016/08/19 Stephen Add 主類別整合
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NaNaConnectionString"].ToString();


        /// <summary>
        /// 新增一筆選項
        /// </summary>        
        public FormOptionsSetting addFormOptionSetting(FormOptionsSetting f) {
            FormOptionsSetting ret = null;

            using (NaNaEntities db = new NaNaEntities()) {

                ret = db.FormOptionsSetting.Add(f);
                db.SaveChanges();

            }

            return ret;
        }

        /// <summary>
        /// 更新選項
        /// </summary>        
        public bool updateFormOptionSetting(long id, Dictionary<string, object> dic) {
            bool ret = false;

            using (NaNaEntities db = new NaNaEntities()) {

                FormOptionsSetting f = db.FormOptionsSetting.AsQueryable().FirstOrDefault(x => x.id == id);

                if (f == null) {
                    return false;
                }

                Type cl = f.GetType();

                foreach (var obj in dic) {

                    if (cl.GetProperty(obj.Key) != null) {
                        cl.GetProperty(obj.Key).SetValue(f, obj.Value);
                    }

                }
                db.SaveChanges();
                ret = true;

            }

            return ret;

        }


        public bool updateFormFieldSetting(string id, Dictionary<string, object> dicObj) {
            bool ret = false;

            using (NaNaEntities db = new NaNaEntities()) {
                Guid guid_id = Guid.Parse(id);

                FormFieldsSetting f = db.FormFieldsSetting.AsQueryable().FirstOrDefault(x => x.id == guid_id);

                if (f == null) {
                    return false;
                }

                Type cl = f.GetType();

                foreach (var obj in dicObj) {

                    if (cl.GetProperty(obj.Key) != null) {
                        cl.GetProperty(obj.Key).SetValue(f, obj.Value);
                    }

                }
                db.SaveChanges();
                ret = true;

            }

            return ret;

        }

        public FormFieldsSetting addFormFieldSetting(FormFieldsSetting f) {
            FormFieldsSetting ret = null;
            using (NaNaEntities db = new NaNaEntities()) {
                ret = db.FormFieldsSetting.Add(f);
                db.SaveChanges();
            }
            return ret;
        }


        public List<FormOptionsSetting> getFormOptionsSettingListByFieldID(Guid id) {
            List<FormOptionsSetting> ret = new List<FormOptionsSetting>();

            using (NaNaEntities db = new NaNaEntities()) {
                ret = db.FormOptionsSetting
                    .Where(x => x.fieldID == id && !x.deleted)
                    .OrderBy(x => x.sort)
                    .ToList();
            }
            return ret;
        }

        /// <summary>
        /// 取得動態欄位列表(可以設定動態選項)
        /// </summary>        
        public List<FormFieldsSetting> getCustomFormFieldsSettingListByFormID(string formID) {
            List<FormFieldsSetting> ret = new List<FormFieldsSetting>();

            using (NaNaEntities db = new NaNaEntities()) {
                ret = db.FormFieldsSetting.Where(x => !x.deleted
                && x.formID == formID
                && x.custOptions).ToList();
            }
            return ret;
        }

        /// <summary>
        /// 取得動態欄位列表(可以設定動態通知窗口)
        /// </summary>
        public List<FormFieldsSetting> getCustomNotifyFormFieldsSettingListByFormID(string formID) {
            List<FormFieldsSetting> ret = new List<FormFieldsSetting>();

            using (NaNaEntities db = new NaNaEntities()) {
                ret = db.FormFieldsSetting.Where(x => !x.deleted
                && x.formID == formID
                && x.custNotify).ToList();
            }
            return ret;
        }

        /// <summary>
        /// 取得列表/總數
        /// </summary>        
        public Tuple<List<FormOptionsSetting>, int> getFormOptionsSettingList(FormOptionsSettingViewModel.FormOptionsQueryParameter param) {
            Tuple<List<FormOptionsSetting>, int> ret = Tuple.Create(new List<FormOptionsSetting>(), 0);

            using (NaNaEntities db = new NaNaEntities()) {
                Guid guid_fieldID = Guid.Parse(param.fieldID);
                var query_ret = db.FormOptionsSetting.Where(x => x.fieldID == guid_fieldID && !x.deleted).OrderBy(x => x.sort);
                ret = Tuple.Create(query_ret.ToList(), query_ret.Count());
            }

            return ret;
        }


        /// <summary>
        /// 取得表單欄位設定
        /// </summary>
        /// <returns></returns>
        public List<FormOptionsSettingViewModel.FormOptionLight> getFormOptionList() {
            List<FormOptionsSettingViewModel.FormOptionLight> ret = new List<FormOptionsSettingViewModel.FormOptionLight>();

            FormOptionsSettingViewModel.FormOptionLight form_BCA = new FormOptionsSettingViewModel.FormOptionLight() {
                fields = new List<FormOptionsSettingViewModel.FormOptionFieldLight>(),
                formName = "廣告申請單",
                formNo = "ADApplyForm",
                formOID = "ADApplyForm"
            };

            FormOptionsSettingViewModel.FormOptionLight form_BCD = new FormOptionsSettingViewModel.FormOptionLight() {
                fields = new List<FormOptionsSettingViewModel.FormOptionFieldLight>(),
                formName = "設計申請單",
                formNo = "BCD001",
                formOID = "BCD001"
            };


            ret.Add(form_BCD);
            ret.Add(form_BCA);

            return ret;
        }



        /// <summary>
        /// 新增油資補助金額
        /// </summary>        
        public OilTransportationSubsidy addOilTransportationSubsidy(OilTransportationSubsidy o) {
            OilTransportationSubsidy ret = null;

            using (NaNaEntities db = new NaNaEntities()) {

                ret = db.OilTransportationSubsidy.Add(o);
                db.SaveChanges();
            }

            return ret;
        }

        /// <summary>
        /// 修改油資補助金額
        /// </summary>        
        public bool updateOilTransportationSubsidy(long id, Dictionary<string, object> dic) {
            bool ret = false;

            using (NaNaEntities db = new NaNaEntities()) {

                OilTransportationSubsidy o = db.OilTransportationSubsidy.AsQueryable().FirstOrDefault(x => x.id == id);

                //不存在此資料
                if (o == null) {
                    return false;
                }

                Type cl = o.GetType();

                foreach (var obj in dic) {

                    if (cl.GetProperty(obj.Key) != null) {
                        cl.GetProperty(obj.Key).SetValue(o, obj.Value);
                    }

                }
                db.SaveChanges();
                ret = true;


            }

            return ret;
        }


        /// <summary>
        /// 刪除油價設定檔
        /// </summary>
        public bool deleteOilTransportationSubsidy(long id) {
            bool ret = false;

            using (NaNaEntities db = new NaNaEntities()) {
                OilTransportationSubsidy o = db.OilTransportationSubsidy.AsQueryable().FirstOrDefault(x => x.id == id);

                //不存在此資料
                if (o == null) {
                    return false;
                }
                db.OilTransportationSubsidy.Remove(o);
                db.SaveChanges();
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 取得油資補助列表
        /// </summary>        
        public List<OilTransportationSubsidy> getOilTransportationSubsidyList(FormOptionsSettingViewModel.OilTransportationSubsidyParameter param) {
            List<OilTransportationSubsidy> ret = new List<OilTransportationSubsidy>();

            using (NaNaEntities db = new NaNaEntities()) {
                var predicate = PredicateBuilder.True<OilTransportationSubsidy>();

                if (param.transTypes.Count > 0) {
                    predicate = predicate.And(x => param.transTypes.Contains(x.type));
                }

                if (param.dateStart != null) {
                    predicate = predicate.And(x => x.dateStart >= param.dateStart);
                }

                if (param.dateEnd != null) {
                    predicate = predicate.And(x => x.dateEnd <= param.dateEnd);
                }

                ret = db.OilTransportationSubsidy.AsExpandable().Where(predicate)
                        .OrderBy(param.orderField, param.desc).ToList();
            }


            return ret;
        }


        public EmployeeTransportationSetting addEmployeeTransportationSetting(EmployeeTransportationSetting e) {
            EmployeeTransportationSetting ret = null;
            using (NaNaEntities db = new NaNaEntities()) {

                ret = db.EmployeeTransportationSetting.Add(e);
                db.SaveChanges();
            }

            return ret;
        }

        public bool updateEmployeeTransportationSetting(long id, Dictionary<string, object> dicObj) {
            bool ret = false;

            using (NaNaEntities db = new NaNaEntities()) {
                EmployeeTransportationSetting e = db.EmployeeTransportationSetting.AsQueryable().FirstOrDefault(x => x.Id == id);

                //不存在此資料
                if (e == null) {
                    return false;
                }


                Type cl = e.GetType();

                foreach (var obj in dicObj) {

                    if (cl.GetProperty(obj.Key) != null) {
                        cl.GetProperty(obj.Key).SetValue(e, obj.Value);
                    }

                }
                db.SaveChanges();
                ret = true;


            }

            return ret;
        }

        /// <summary>
        /// 取得私車公用員工編號列表
        /// </summary>        
        public Tuple<List<ViewEmployeeTransportationSetting>, int>
        getViewEmployeeTransportationSettingList(FormOptionsSettingViewModel.EmployeeTransportationSettingQueryParameter param) {

            List<ViewEmployeeTransportationSetting> list = new List<ViewEmployeeTransportationSetting>();
            int count = 0;
            Tuple<List<ViewEmployeeTransportationSetting>, int> ret = Tuple.Create(list, count);

            using (NaNaEntities db = new NaNaEntities()) {
                var predicate = PredicateBuilder.True<ViewEmployeeTransportationSetting>();

                if (!string.IsNullOrEmpty(param.keyword)) {
                    predicate = predicate.And(x =>
                      x.userName.Contains(param.keyword) ||
                      x.EmpNo.Contains(param.keyword) ||
                      x.organizationUnitName.Contains(param.keyword));
                }


                if (!string.IsNullOrEmpty(param.transType)) {
                    predicate = predicate.And(x => x.TransType == param.transType);
                }

                if (!string.IsNullOrEmpty(param.empNo)) {
                    predicate = predicate.And(x => x.EmpNo == param.empNo);
                }


                list = db.ViewEmployeeTransportationSetting.AsExpandable()
                  .Where(predicate)
                 .OrderBy(param.orderField, param.desc)
                 .Skip((param.pageIndex - 1) * param.pageSize)
                 .Take(param.pageSize).ToList();

                count = db.ViewEmployeeTransportationSetting.AsExpandable().Where(predicate).OrderBy(param.orderField, param.desc).Count();

                ret = Tuple.Create(list, count);

            }


            return ret;

        }

        ///2016/05/09 Stephen Add
        /// <summary>
        /// 取得類別列表
        /// </summary>        
        public Tuple<List<ERACategoryForInf>, int>
        getERACategoryForInfList(FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param) {

            List<ERACategoryForInf> list = new List<ERACategoryForInf>();
            int count = 0;
            Tuple<List<ERACategoryForInf>, int> ret = Tuple.Create(list, count);

            using (NaNaEntities db = new NaNaEntities()) {
                var predicate = PredicateBuilder.True<ERACategoryForInf>();

                if (!string.IsNullOrEmpty(param.name)) {
                    predicate = predicate.And(x => x.name == param.name);
                }

                predicate = predicate.And(x => !x.deleted); //刪除過的類別,如再次新增.應該要可以新增

                list = db.ERACategoryForInf.AsExpandable()
                 .Where(predicate)
                 .OrderBy(param.orderField, param.desc)
                 .Skip((param.pageIndex - 1) * param.pageSize)
                 .Take(param.pageSize).ToList();

                count = db.ERACategoryForInf.AsExpandable().Where(predicate).OrderBy(param.orderField, param.desc).Count();
                ret = Tuple.Create(list, count);
            }

            return ret;
        }

        public ERACategoryForInf addERACategoryForInf(ERACategoryForInf e) {
            ERACategoryForInf ret = null;
            using (NaNaEntities db = new NaNaEntities()) {
                ret = db.ERACategoryForInf.Add(e);
                db.SaveChanges();
            }

            return ret;
        }

        public bool updateERACategoryForInf(long id, Dictionary<string, object> dicObj) {
            bool ret = false;

            using (NaNaEntities db = new NaNaEntities()) {
                ERACategoryForInf e = db.ERACategoryForInf.AsQueryable().FirstOrDefault(x => x.Id == id);

                //不存在此資料
                if (e == null) {
                    return false;
                }

                Type cl = e.GetType();

                foreach (var obj in dicObj) {
                    if (cl.GetProperty(obj.Key) != null) {
                        cl.GetProperty(obj.Key).SetValue(e, obj.Value);
                    }
                }

                db.SaveChanges();
                ret = true;
            }

            return ret;
        }

        //ERAPermissionForInf
        public ERAPermissionForInf addERAPermissionForInf(ERAPermissionForInf e) {
            ERAPermissionForInf ret = null;
            using (NaNaEntities db = new NaNaEntities()) {
                ret = db.ERAPermissionForInf.Add(e);
                db.SaveChanges();
            }

            return ret;
        }

        public bool updateERAPermissionForInf(long id, Dictionary<string, object> dicObj) {
            bool ret = false;

            using (NaNaEntities db = new NaNaEntities()) {
                ERAPermissionForInf e = db.ERAPermissionForInf.AsQueryable().FirstOrDefault(x => x.Id == id);

                //不存在此資料
                if (e == null) {
                    return false;
                }

                Type cl = e.GetType();

                foreach (var obj in dicObj) {
                    if (cl.GetProperty(obj.Key) != null) {
                        cl.GetProperty(obj.Key).SetValue(e, obj.Value);
                    }
                }

                db.SaveChanges();
                ret = true;
            }

            return ret;
        }

        //ViewERACategoryPermissionForInf
        public Tuple<List<ViewERACategoryPermissionForInf>, int>
        getViewERACategoryPermissionForInfList(FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param) {

            List<ViewERACategoryPermissionForInf> list = new List<ViewERACategoryPermissionForInf>();
            int count = 0;
            Tuple<List<ViewERACategoryPermissionForInf>, int> ret = Tuple.Create(list, count);

            using (NaNaEntities db = new NaNaEntities()) {
                var predicate = PredicateBuilder.True<ViewERACategoryPermissionForInf>();

                if (!string.IsNullOrEmpty(param.keyword)) {
                    predicate = predicate.And(x =>
                        x.name.Contains(param.keyword));
                }

                if (param.classType != 0) {
                    predicate = predicate.And(x => x.Id == param.classType || x.parentID == param.classType);
                }

                if (!string.IsNullOrEmpty(param.name)) {
                    predicate = predicate.And(x => x.partent_name.Contains(param.name));
                }

                if (param.id != 0) {
                    predicate = predicate.And(x => x.Id == param.id);
                }

                predicate = predicate.And(x => !x.deleted); //全部秀出但刪除的不秀

                list = db.ViewERACategoryPermissionForInf.AsExpandable()
                 .Where(predicate)
                 .OrderBy(param.orderField, param.desc)
                 .Skip((param.pageIndex - 1) * param.pageSize)
                 .Take(param.pageSize).ToList();

                count = db.ViewERACategoryPermissionForInf.AsExpandable().Where(predicate).OrderBy(param.orderField, param.desc).Count();

                ret = Tuple.Create(list, count);
            }

            return ret;
        }

        ///ddl
        public Tuple<List<ERACategoryForInf>, int>
        getERACategoryForInfMainClassList(FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param) {

            List<ERACategoryForInf> list = new List<ERACategoryForInf>();
            int count = 0;
            Tuple<List<ERACategoryForInf>, int> ret = Tuple.Create(list, count);

            using (NaNaEntities db = new NaNaEntities()) {
                var predicate = PredicateBuilder.True<ERACategoryForInf>();

                predicate = predicate.And(x => !x.deleted && x.parentID == -1);//-1為父類別,並且未刪除

                list = db.ERACategoryForInf.AsExpandable()
                 .Where(predicate)
                 .OrderBy(param.orderField, false)
                 .Skip((param.pageIndex - 1) * param.pageSize)
                 .Take(param.pageSize).ToList();

                count = db.ERACategoryForInf.AsExpandable().Where(predicate).OrderBy(param.orderField, param.desc).Count();
                ret = Tuple.Create(list, count);
            }

            return ret;
        }

        //ViewERAUsersOrganizationUnitDepartment
        public Tuple<List<ViewERAUsersOrganizationUnitDepartment>, int>
        getViewERAUsersOrganizationUnitDepartmentList(FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param) {

            List<ViewERAUsersOrganizationUnitDepartment> list = new List<ViewERAUsersOrganizationUnitDepartment>();
            int count = 0;
            Tuple<List<ViewERAUsersOrganizationUnitDepartment>, int> ret = Tuple.Create(list, count);

            using (NaNaEntities db = new NaNaEntities()) {
                var predicate = PredicateBuilder.True<ViewERAUsersOrganizationUnitDepartment>();

                if (!string.IsNullOrEmpty(param.personnelID)) {
                    predicate = predicate.And(x => x.ID == param.personnelID);
                }

                list = db.ViewERAUsersOrganizationUnitDepartment.AsExpandable()
                 .Where(predicate)
                 .OrderBy(param.orderField.ToUpper(), param.desc)
                 .Skip((param.pageIndex - 1) * param.pageSize)
                 .Take(param.pageSize).ToList();

                count = db.ViewERAUsersOrganizationUnitDepartment.AsExpandable().Where(predicate).OrderBy(param.orderField.ToUpper(), param.desc).Count();
                ret = Tuple.Create(list, count);
            }

            return ret;
        }

        //OrganizationUnit
        public Tuple<List<OrganizationUnit>, int>
        getOrganizationUnitList(FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param) {

            List<OrganizationUnit> list = new List<OrganizationUnit>();
            int count = 0;
            Tuple<List<OrganizationUnit>, int> ret = Tuple.Create(list, count);

            using (NaNaEntities db = new NaNaEntities()) {
                var predicate = PredicateBuilder.True<OrganizationUnit>();

                if (!string.IsNullOrEmpty(param.departmentName)) {
                    predicate = predicate.And(x => x.organizationUnitName == param.departmentName);
                }

                list = db.OrganizationUnit.AsExpandable()
                 .Where(predicate)
                 .OrderBy(param.orderField.ToLower(), param.desc)
                 .Skip((param.pageIndex - 1) * param.pageSize)
                 .Take(param.pageSize).ToList();

                count = db.OrganizationUnit.AsExpandable().Where(predicate).OrderBy(param.orderField.ToLower(), param.desc).Count();
                ret = Tuple.Create(list, count);
            }

            return ret;
        }

        //2016/09/06 Stephen Add
        public List<FormOptionsSettingViewModel.departmentLight> getDepList(string keyword) {

            List<FormOptionsSettingViewModel.departmentLight> ret = new List<FormOptionsSettingViewModel.departmentLight>();

            string selectQueryStr = "select * from OrganizationUnit o where o.organizationUnitName like @organizationUnitName or o.id like @id order by id desc";
            //string selectQueryStr = "select * from OrganizationUnit o where o.organizationUnitName like ? or o.id like ?";

            using (SqlConnection conn = new SqlConnection(this.connectionString)) {

                using (SqlCommand cmd = new SqlCommand(selectQueryStr, conn)) {

                    ///cmd.Parameters.Add("", keyword.Trim());
                    conn.Open();

                    cmd.Parameters.AddWithValue("id", "%" + keyword.Trim() + "%");
                    cmd.Parameters.AddWithValue("organizationUnitName", "%" + keyword.Trim() + "%");
                    //cmd.Parameters.AddWithValue(":keyword1", "%" + keyword.Trim() + "%");
                    //cmd.Parameters.AddWithValue(":keyword2", "%" + keyword.Trim() + "%");

                    using (var reader = cmd.ExecuteReader()) {

                        while (reader.Read()) {
                            ret.Add(new FormOptionsSettingViewModel.departmentLight() {
                                id = reader["id"].ToString(),
                                organizationUnitName = reader["organizationUnitName"].ToString()
                            });

                        }
                    }
                };

            }
         
            return ret;
        }

        ///2016/08/17 Stephen Add
        ///ERADynamicFieldSetting
        public Tuple<List<ERADynamicFieldSetting>, int>
        getERADynamicFieldSettingList(FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param) {

            List<ERADynamicFieldSetting> list = new List<ERADynamicFieldSetting>();
            int count = 0;
            Tuple<List<ERADynamicFieldSetting>, int> ret = Tuple.Create(list, count);

            using (NaNaEntities db = new NaNaEntities()) {
                var predicate = PredicateBuilder.True<ERADynamicFieldSetting>();

                //if (!string.IsNullOrEmpty(param.name)) {
                //    predicate = predicate.And(x => x.name == param.name);
                //}

                list = db.ERADynamicFieldSetting.AsExpandable()
                 .Where(predicate)
                 .OrderBy(param.orderField, false)
                 .Skip((param.pageIndex - 1) * param.pageSize)
                 .Take(param.pageSize).ToList();

                count = db.ERADynamicFieldSetting.AsExpandable().Where(predicate).OrderBy(param.orderField, param.desc).Count();
                ret = Tuple.Create(list, count);
            }

            return ret;
        }

        public ERADynamicFieldSetting addERADynamicFieldSetting(ERADynamicFieldSetting e) {
            ERADynamicFieldSetting ret = null;
            using (NaNaEntities db = new NaNaEntities()) {
                ret = db.ERADynamicFieldSetting.Add(e);
                db.SaveChanges();
            }
            return ret;
        }

        public bool updateERADynamicFieldSetting(long id, Dictionary<string, object> dicObj) {
            bool ret = false;

            using (NaNaEntities db = new NaNaEntities()) {
                ERADynamicFieldSetting e = db.ERADynamicFieldSetting.AsQueryable().FirstOrDefault(x => x.Id == id);

                //不存在此資料
                if (e == null) {
                    return false;
                }

                Type cl = e.GetType();

                foreach (var obj in dicObj) {
                    if (cl.GetProperty(obj.Key) != null) {
                        cl.GetProperty(obj.Key).SetValue(e, obj.Value);
                    }
                }

                db.SaveChanges();
                ret = true;
            }

            return ret;
        }

        ///ERACategoryDynamicField
        public Tuple<List<ERACategoryDynamicField>, int>
        getERACategoryDynamicFieldList(FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param) {

            List<ERACategoryDynamicField> list = new List<ERACategoryDynamicField>();
            int count = 0;
            Tuple<List<ERACategoryDynamicField>, int> ret = Tuple.Create(list, count);

            using (NaNaEntities db = new NaNaEntities()) {
                var predicate = PredicateBuilder.True<ERACategoryDynamicField>();

                if (param.cateogryId > 0) {
                    predicate = predicate.And(x => x.cateogryId == param.cateogryId);
                }

                list = db.ERACategoryDynamicField.AsExpandable()
                 .Where(predicate)
                 .OrderBy(param.orderField, param.desc)
                 .Skip((param.pageIndex - 1) * param.pageSize)
                 .Take(param.pageSize).ToList();

                count = db.ERACategoryDynamicField.AsExpandable().Where(predicate).OrderBy(param.orderField, param.desc).Count();
                ret = Tuple.Create(list, count);
            }

            return ret;
        }

        public ERACategoryDynamicField addERACategoryDynamicField(ERACategoryDynamicField e) {
            ERACategoryDynamicField ret = null;
            using (NaNaEntities db = new NaNaEntities()) {
                ret = db.ERACategoryDynamicField.Add(e);
                db.SaveChanges();
            }
            return ret;
        }

        public bool updateERACategoryDynamicField(long id, Dictionary<string, object> dicObj) {
            bool ret = false;

            using (NaNaEntities db = new NaNaEntities()) {
                ERACategoryDynamicField e = db.ERACategoryDynamicField.AsQueryable().FirstOrDefault(x => x.Id == id);

                //不存在此資料
                if (e == null) {
                    return false;
                }

                Type cl = e.GetType();

                foreach (var obj in dicObj) {
                    if (cl.GetProperty(obj.Key) != null) {
                        cl.GetProperty(obj.Key).SetValue(e, obj.Value);
                    }
                }

                db.SaveChanges();
                ret = true;
            }

            return ret;
        }

        //ViewERACategoryPermissionDynamicFieldSettingForInf
        public Tuple<List<ViewERACategoryPermissionDynamicFieldSettingForInf>, int>
        getViewERACategoryPermissionDynamicFieldSettingForInfList(FormOptionsSettingViewModel.FormClassSettingForInfoSystemApplyQueryParameter param) {

            List<ViewERACategoryPermissionDynamicFieldSettingForInf> list = new List<ViewERACategoryPermissionDynamicFieldSettingForInf>();
            int count = 0;
            Tuple<List<ViewERACategoryPermissionDynamicFieldSettingForInf>, int> ret = Tuple.Create(list, count);

            using (NaNaEntities db = new NaNaEntities()) {
                var predicate = PredicateBuilder.True<ViewERACategoryPermissionDynamicFieldSettingForInf>();

                if (param.id != 0) {
                    predicate = predicate.And(x => x.Id == param.id);
                }

                list = db.ViewERACategoryPermissionDynamicFieldSettingForInf.AsExpandable()
                 .Where(predicate)
                 .OrderBy(param.orderField, param.desc)
                 .Skip((param.pageIndex - 1) * param.pageSize)
                 .Take(param.pageSize).ToList();

                count = db.ViewERACategoryPermissionDynamicFieldSettingForInf.AsExpandable().Where(predicate).OrderBy(param.orderField, param.desc).Count();

                ret = Tuple.Create(list, count);
            }

            return ret;
        }

        public List<FormOptionsSettingViewModel.ClassificationUnit> getPurviewUnitByParentID(string parentID) {
            List<FormOptionsSettingViewModel.ClassificationUnit> ret = new List<FormOptionsSettingViewModel.ClassificationUnit>();

            using (SqlConnection conn = new SqlConnection(this.connectionString)) {

                conn.Open();

                string sql = @"select
                                e.isforAll,
                                e.deleted,
                                case ol.levelValue
                                when 4000 
                                then
                                (select OID+',' from OrganizationUnit where superUnitOID in (select OID from OrganizationUnit where OID = ou.OID) for xml path('')) + ou.OID
                                when 3000 
                                then
                                (select OID+',' from OrganizationUnit where superUnitOID in (select OID from OrganizationUnit where OID = ou.OID) for xml path('')) +
                                (select OID+',' from OrganizationUnit where superUnitOID in (select OID from OrganizationUnit where superUnitOID in (select OID from OrganizationUnit where OID = ou.OID)) for xml path('')) + ou.OID
                                end as unit
                                from ERAPermissionForInf e
                                left join OrganizationUnit ou on e.departmentID = ou.id
                                left join OrganizationUnitLevel ol on ou.levelOID  = ol.OID
                                where e.Id = @parentID";

                using (SqlCommand cmd = new SqlCommand(sql, conn)) {

                    cmd.Parameters.AddWithValue("parentID", parentID);

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read()) {
                        FormOptionsSettingViewModel.ClassificationUnit item = new FormOptionsSettingViewModel.ClassificationUnit();
                        item.isforAll = (dr["isforAll"] as bool?).Value;
                        item.deleted = (dr["deleted"] as bool?).Value;
                        item.unit = dr["unit"] as string;
                        ret.Add(item);

                    }
                }
            }

            return ret;
        }
    }
}



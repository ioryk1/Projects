using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using WebServiceForEFGP.Models.VeiwModel;


namespace WebServiceForEFGP.Models.Dao {
    public class ProcessDao {

        private string connectionString = "";

        public ProcessDao() {
            this.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NaNaConnectionString"].ToString(); ;
        }

        /// <summary>
        /// 取得簽核歷程
        /// </summary>        
        public List<ProcessViewModel.ProcessHistory> getProcessHistoryByProcessSerialNumber(string ProcessSerialNumber) {
            List<ProcessViewModel.ProcessHistory> ret = new List<ProcessViewModel.ProcessHistory>();

            using (SqlConnection conn = new SqlConnection(this.connectionString)) {

                conn.Open();

                string sql = @"Select ProcessInstance.serialNumber, ProcessInstance.currentState,
                                ou.id as unitID,ou.organizationUnitName,
                                Users.userName,WorkItem.workItemName, WorkItem.currentState AS WcurrentState, 
                                cast (WorkItem.executiveComment as nvarchar(4000)) as executiveComment, dbo.WorkItem.createdTime, WorkItem.completedTime, ActivityDefinition.id, ActivityDefinition.performType,
                                ReassignWorkItemAuditData.newAssigneeId,ReassignWorkItemAuditData.newAssigneeName,ReassignWorkItemAuditData.oldAssigneeId,ReassignWorkItemAuditData.oldAssigneeName,ReassignWorkItemAuditData.reassignmentType
                                from ProcessInstance 
                                inner join WorkItem on WorkItem.contextOID=ProcessInstance.contextOID
                                inner join Users on Users.OID =  WorkItem.performerOID 
                                inner join ProcessContext on ProcessContext.OID= ProcessInstance.contextOID
                                inner join ProcessPackage_ProcessDef on ProcessPackage_ProcessDef.ProcessPackageOID =ProcessContext.processPackageOID 
                                inner join ActivityDefinition on ProcessPackage_ProcessDef.ProcessDefinitionOID = ActivityDefinition.containerOID
                                inner join ParticipantActivityInstance on ParticipantActivityInstance.contextOID=ProcessInstance.contextOID and WorkItem.containerOID=ParticipantActivityInstance.OID and ParticipantActivityInstance.definitionId=ActivityDefinition.id
                                left join ReassignWorkItemAuditData on ReassignWorkItemAuditData.sourceOID = WorkItem.OID
                                inner join Functions f on Users.OID = f.occupantOID
                                inner join OrganizationUnit ou on ou.OID = f.organizationUnitOID
                                Where ProcessInstance.serialNumber = @serialNumber1
                                UNION
                                Select ProcessInstance.serialNumber, ProcessInstance.currentState,
                                ou.id as unitID,ou.organizationUnitName,
                                Users.userName,WorkItem.workItemName, WorkItem.currentState AS WcurrentState, 
                                cast (WorkItem.executiveComment as nvarchar(4000)) as executiveComment, dbo.WorkItem.createdTime, WorkItem.completedTime, ActivityDefinition.id, ActivityDefinition.performType,
                                ReassignWorkItemAuditData.newAssigneeId,ReassignWorkItemAuditData.newAssigneeName,ReassignWorkItemAuditData.oldAssigneeId,ReassignWorkItemAuditData.oldAssigneeName,ReassignWorkItemAuditData.reassignmentType
                                from ProcessInstance 
                                inner join WorkItem on WorkItem.contextOID=ProcessInstance.contextOID
                                inner join Users on Users.OID =  WorkItem.performerOID 
                                inner join ProcessContext on ProcessContext.OID= ProcessInstance.contextOID
                                inner join ProcessPackage_ProcessDef on ProcessPackage_ProcessDef.ProcessPackageOID =ProcessContext.processPackageOID 
                                inner join ActivitySetDefinition on ProcessPackage_ProcessDef.ProcessDefinitionOID = ActivitySetDefinition.containerOID
                                inner join ActivityDefinition on ActivitySetDefinition.OID = ActivityDefinition.containerOID
                                inner join ParticipantActivityInstance on ParticipantActivityInstance.contextOID=ProcessInstance.contextOID and WorkItem.containerOID=ParticipantActivityInstance.OID and ParticipantActivityInstance.definitionId = ActivityDefinition.id
                                left join ReassignWorkItemAuditData on ReassignWorkItemAuditData.sourceOID = WorkItem.OID
                                inner join Functions f on Users.OID = f.occupantOID
                                inner join OrganizationUnit ou on ou.OID = f.organizationUnitOID
                                Where ProcessInstance.serialNumber = @serialNumber2";

                using (SqlCommand cmd = new SqlCommand(sql, conn)) {

                    cmd.Parameters.AddWithValue("serialNumber1", ProcessSerialNumber);
                    cmd.Parameters.AddWithValue("serialNumber2", ProcessSerialNumber);

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read()){
                        ProcessViewModel.ProcessHistory item = new ProcessViewModel.ProcessHistory();
                        item.serialNumber = dr["serialNumber"] as string;
                        item.currentState = (dr["currentState"] as int?).Value;
                        item.unitID = dr["unitID"] as string;
                        item.organizationUnitName = dr["organizationUnitName"] as string;
                        item.userName = dr["userName"] as string;
                        item.workItemName = dr["workItemName"] as string;
                        item.WcurrentState = (dr["WcurrentState"] as int?).Value;
                        item.executiveComment = dr["executiveComment"] as string;
                        item.createdTime = (dr["createdTime"] as DateTime?).Value;  //   DateTime.Parse(dr["createdTime"].ToString());
                        item.completedTime = (dr["completedTime"] as DateTime?).Value;
                        item.id = dr["id"] as string;
                        item.performType = dr["performType"] as string;
                        item.newAssigneeId = dr["newAssigneeId"] as string;
                        item.newAssigneeName = dr["newAssigneeName"] as string;
                        item.oldAssigneeId = dr["oldAssigneeId"] as string;
                        item.oldAssigneeName = dr["oldAssigneeName"] as string;
                        item.reassignmentType = dr["reassignmentType"] as string;
                        ret.Add(item);

                    }
                }
            }

            return ret;
        }


        /// <summary>
        /// 取得工作項目識別值
        /// </summary>        
        public string getWorkItemOIDByProcessSerialNumberActivityID(string processSerialNumber,  string activityID) {
            string ret = null;


            using (SqlConnection conn = new SqlConnection(this.connectionString)) {
                conn.Open();
                string sql = @"select TOP 1 w.OID as [workItemOID] ,w.workItemName , pai.definitionId , p_i.serialNumber
                                from WorkItem w 
                                left join ParticipantActivityInstance pai
                                on w.containerOID  = pai.OID
                                left join ProcessInstance p_i
                                on p_i.OID  = pai.containerOID
                                where p_i.serialNumber = @processSerialNumber
                                and pai.definitionId = @activityID";


                using (SqlCommand cmd = new SqlCommand(sql, conn)) {
                    cmd.Parameters.AddWithValue("processSerialNumber", processSerialNumber);
                    cmd.Parameters.AddWithValue("activityID", activityID);

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read()) {
                        ret = dr["workItemOID"] as string;
                    }
                }
            }

            return ret;
        }

    }
}
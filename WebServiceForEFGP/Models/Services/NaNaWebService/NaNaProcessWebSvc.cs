using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceForEFGP.Models.Dao;
using WebServiceForEFGP.Models.VeiwModel;
using WebServiceForEFGP.Models.VeiwModel.NaNaViewModel;
using System.Xml;



namespace WebServiceForEFGP.Models.Services.NaNaWebService {
    public class NaNaProcessWebSvc {

        private NaNa.PLMIntegrationEFGP.PLMIntegrationEFGPService plmSvc = null;
        private ProcessDao procDao = null;

        public NaNaProcessWebSvc() {
            this.plmSvc = new NaNa.PLMIntegrationEFGP.PLMIntegrationEFGPService();
            this.procDao = new ProcessDao();
        }

        /// <summary>
        /// 發起表單
        /// </summary>        
        public CommonViewModel.Result invokeProcess(string processId, string userId, string orgUnitId, string formOID, string formFieldValue, string subject) {

            CommonViewModel.Result ret = new CommonViewModel.Result();
            
            try {
                string processSerialNumber = this.plmSvc.invokeProcess(processId, userId, orgUnitId, formOID, formFieldValue, subject);
                                
                ret.resultMessage = processSerialNumber;
                ret.success = !string.IsNullOrEmpty(processSerialNumber);
                ret.resultCode = "200";
            } catch (Exception ex) {
                ret.success = false;
                ret.resultCode = "500";
                ret.resultException = ex.ToString();

                UtilitySvc.writeLog("======== invokeProcess Start =========");
                UtilitySvc.writeLog("processId:" + processId);
                UtilitySvc.writeLog("userId:" + userId);
                UtilitySvc.writeLog("orgUnitId:" + orgUnitId);
                UtilitySvc.writeLog("formOID:" + formOID);
                UtilitySvc.writeLog("formFieldValue:" + formFieldValue);
                UtilitySvc.writeLog("subject:" + subject);
                UtilitySvc.writeLog("excption:" + ex.ToString());
                UtilitySvc.writeLog("======== invokeProcess End =========");
            }

            return ret;

        }

        
        /// <summary>
        /// 加簽關卡
        /// </summary>        
        public CommonViewModel.Result addCustomActivity(string processSerialNumber, string activityID,            
            string refActId,            NaNaProcessViewModel.AddActivityParameter param) {
            CommonViewModel.Result ret = new CommonViewModel.Result();

            XmlDocument defXML = this.getAddCustomAcivityXML(param);

            UtilitySvc.writeLog(defXML.OuterXml);

            this.plmSvc.addCustomParallelAndSerialActivity(processSerialNumber, activityID, refActId, defXML.OuterXml);
            
            return ret.setResultValue(true, "200", string.Empty, string.Empty);
        }


        public string getProcessInfo(string processSerialNumber) {
            string ret = this.plmSvc.fetchFullProcInstanceWithSerialNo(processSerialNumber);
            return ret;
            
        }

        /// <summary>
        /// 產生加簽XML參數(只增加一個關卡)
        /// </summary>        
        private XmlDocument getAddCustomAcivityXML(NaNaProcessViewModel.AddActivityParameter param) {
            XmlDocument ret = new XmlDocument();

            

            string xmlStr = @"<list>
                              <list>
                                <com.dsc.nana.data_transfer.ActivityDefinitionForClientListDTO>
                                  <performers>
                                    <com.dsc.nana.data_transfer.ActivityDefPerformerForClientListDTO>
                                      <OID>{participantOID}</OID> 
                                      <participantType>
                                        <value>{participantType}</value> 
                                      </participantType>
                                    </com.dsc.nana.data_transfer.ActivityDefPerformerForClientListDTO>
                                  </performers>
                                  <multiUserMode>
                                    <value>{multiUserMode}</value> 
                                  </multiUserMode>
                                  <name>{name}</name> 
                                </com.dsc.nana.data_transfer.ActivityDefinitionForClientListDTO>    
                              </list>  
                            </list>";

            xmlStr = xmlStr.Replace("{participantOID}", param.participantOID)
                 .Replace("{participantType}", param.participantType)
                 .Replace("{multiUserMode}", param.multiUserMode)
                 .Replace("{name}", param.name);

            ret.LoadXml(xmlStr);





            return ret; 
        }

    }
}

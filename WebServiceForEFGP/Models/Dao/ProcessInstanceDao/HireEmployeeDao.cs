using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceForEFGP.Models.Dao.ProcessInstanceDao {
    public class HireEmployeeDao {

        public HireEmployeeForm getHireEmployeeFormBySerialNumber(string serialNumber) {
            HireEmployeeForm ret = null;

            using (NaNaEntities db = new NaNaEntities()) {
                ret = db.HireEmployeeForm.FirstOrDefault(x => x.processSerialNumber == serialNumber);
            }
            return ret;
        }

        public List<HireEmployeeForm_gdProjectAchievement> getHireEmployeeFormGdProjectAchievementsByFormSerialNumber(string formSerialNumber) {
            List<HireEmployeeForm_gdProjectAchievement> ret = new List<HireEmployeeForm_gdProjectAchievement>();

            using (NaNaEntities db = new NaNaEntities()) {
                ret = db.HireEmployeeForm_gdProjectAchievement
                    .Where(x => x.formSerialNumber == formSerialNumber)
                    .OrderBy(y => y.OID).ToList();
            }

            return ret;
        }

    }
}

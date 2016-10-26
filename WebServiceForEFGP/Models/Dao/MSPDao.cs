using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceForEFGP.Models.Dao {
    public class MSPDao {

        public MSPDao() {

        }

        public MSG001 getMSGData(string proccessSerialNumber) {
            MSG001 ret = null;
            using (NaNaEntities db = new NaNaEntities()) {
                ret = db.MSG001.FirstOrDefault(x => x.processSerialNumber == proccessSerialNumber);
            }
            return ret;
        }       

        public List<MSG001_gridMsg> getMSGGridList(string formSerialNumber) {
            List<MSG001_gridMsg> ret = new List<MSG001_gridMsg>();

            using (NaNaEntities db = new NaNaEntities()) {
                ret = (from m in db.MSG001_gridMsg
                       where m.formSerialNumber == formSerialNumber
                       select m).ToList();
                //排序
                ret = ret.OrderBy(x => int.Parse(x.gd_sort)).ToList();

            }

            return ret;

        }




    }
}

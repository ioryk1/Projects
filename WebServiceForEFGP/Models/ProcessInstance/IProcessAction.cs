using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceForEFGP.Models.VeiwModel;

namespace WebServiceForEFGP.Models.ProcessInstance {
     public interface IProcessAction {

        CommonViewModel.Result create();

        CommonViewModel.Result reject();

        CommonViewModel.Result agree();
      
    }
}

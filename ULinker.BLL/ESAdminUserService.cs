using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULinker.Models.DO;
using ULinker.Models.VO;

namespace ULinker.BLL
{
    public class ESAdminUserService
    {
        private static ESAdminUserService _Instance = null;

        public static ESAdminUserService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ESAdminUserService();
                return _Instance;
            }
        }

        private ESAdminUserService()
        {

        }

        

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULinker.Models.DO;
using ULinker.DAL;

namespace ULinker.BLL
{
    public class AppApiService
    {
        private static AppApiService _Instance = null;

        public static AppApiService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new AppApiService();
                return _Instance;
            }
        }

        private AppApiService()
        {

        }

        public List<AppApi> GetByAppId(int appId)
        {
            return AppApiDao.Instance.GetByAppId(appId);
        }

        public bool Create(AppApi model)
        {
            return AppApiDao.Instance.Create(model);
        }

        public bool PhysicDelete(int appId, string loginAccount)
        {
            return AppApiDao.Instance.PhysicDelete(appId,loginAccount);
        }

    }
}

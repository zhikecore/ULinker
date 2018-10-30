using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULinker.Models.DO;
using ULinker.DAL;

namespace ULinker.BLL
{
    public class AppTypeService
    {
        private static AppTypeService _Instance = null;

        public static AppTypeService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new AppTypeService();
                return _Instance;
            }
        }

        private AppTypeService()
        {

        }

        public AppType GetById(int id)
        {
            return AppTypeDao.Instance.GetById(id);
        }

        public List<AppType> GetBySomeWhere(string keyword, int limit, int pageSize, out int total)
        {
            return AppTypeDao.Instance.GetBySomeWhere(keyword,limit,pageSize,out total);
        }

        public bool Create(Platform model)
        {
            return AppTypeDao.Instance.Create(model);
        }

        public bool PhysicDelete(int id, string loginAccount)
        {
            return AppTypeDao.Instance.PhysicDelete(id,loginAccount);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULinker.Models.DO;
using ULinker.DAL;

namespace ULinker.BLL
{
    public class AppService
    {
        private static AppService _Instance = null;

        public static AppService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new AppService();
                return _Instance;
            }
        }

        private AppService()
        {

        }

        public object GetAppsForCombox()
        {
            List<object> objs = new List<object>();
            List<App> apps = AppDao.Instance.GetAll();
            foreach (App item in apps)
            {
                objs.Add(new { Id = item.Id, Name = item.Name });
            }

            return objs;
        }

        public App GetById(int id)
        {
            return AppDao.Instance.GetById(id);
        }

        public List<App> GetBySomeWhere(string keyword, int limit, int pageSize, out int total)
        {
            return AppDao.Instance.GetBySomeWhere(keyword,limit,pageSize,out total);
        }

        public bool Create(App model)
        {
            return AppDao.Instance.Create(model);
        }

        public bool Update(App model)
        {
            return AppDao.Instance.Update(model);
        }

        public bool PhysicDelete(int id, string loginAccount)
        {
            return AppDao.Instance.PhysicDelete(id,loginAccount);
        }



    }
}

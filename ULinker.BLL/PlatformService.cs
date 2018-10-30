using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULinker.Models.DO;
using ULinker.DAL;

namespace ULinker.BLL
{
    public class PlatformService
    {
        private static PlatformService _Instance = null;

        public static PlatformService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new PlatformService();
                return _Instance;
            }
        }

        private PlatformService()
        {

        }

        public object GetPlatformsForCombox()
        {
            List<object> objs = new List<object>();
            List<Platform> platforms = PlatformDao.Instance.GetAll();
            foreach (Platform item in platforms)
            {
                objs.Add(new { Id = item.Id, Name = item.Name });
            }

            return objs;
        }

        public Platform GetById(int id)
        {
            return PlatformDao.Instance.GetById(id);
        }

        public List<Platform> GetAll()
        {
            return PlatformDao.Instance.GetAll();
        }

        public List<Platform> GetBySomeWhere(string keyword, int limit, int pageSize, out int total)
        {
            return PlatformDao.Instance.GetBySomeWhere(keyword,limit,pageSize,out total);
        }

        public bool Create(Platform model)
        {
            return PlatformDao.Instance.Create(model);
        }

        public bool Update(Platform model)
        {
            return PlatformDao.Instance.Update(model);
        }

        public bool PhysicDelete(int id, string loginAccountId)
        {
            return PlatformDao.Instance.PhysicDelete(id,loginAccountId);
        }

    }
}

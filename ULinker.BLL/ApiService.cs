using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULinker.Models.DO;
using ULinker.DAL;

namespace ULinker.BLL
{
    public class ApiService
    {
        private static ApiService _Instance = null;

        public static ApiService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ApiService();
                return _Instance;
            }
        }

        private ApiService()
        {

        }

        public Api GetById(int id)
        {
            return ApiDao.Instance.GetById(id);
        }

        public List<Api> GetBySomeWhere(string keyword, int limit, int pageSize, out int total)
        {
            return ApiDao.Instance.GetBySomeWhere(keyword,limit,pageSize,out total);
        }

        public bool Create(Api model)
        {
            return ApiDao.Instance.Create(model);
        }

        public bool PhysicDelete(int id, string loginAccount)
        {
            return ApiDao.Instance.PhysicDelete(id,loginAccount);
        }
    }
}

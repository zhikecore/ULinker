using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULinker.Models.DO;
using ULinker.DAL;

namespace ULinker.BLL
{
    public class UserPlatformService
    {
        private static UserPlatformService _Instance = null;

        public static UserPlatformService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new UserPlatformService();
                return _Instance;
            }
        }

        private UserPlatformService()
        {

        }

        public bool IsExist(int platformId, int appId)
        {
            return UserPlatformDao.Instance.IsExist(platformId, appId);
        }

        public bool IsExistAppKey(string appKey)
        {
            return UserPlatformDao.Instance.IsExistAppKey(appKey);
        }

        public UserPlatform GetById(int id)
        {
            return UserPlatformDao.Instance.GetById(id);
        }

        public UserPlatform GetByAppKey(string appkey,string appsecrect)
        {
            return UserPlatformDao.Instance.GetByAppKey(appkey,appsecrect);
        }

        public bool Create(UserPlatform model)
        {
            return UserPlatformDao.Instance.Create(model);
        }

        public bool Update(UserPlatform model)
        {
            return UserPlatformDao.Instance.Update(model);
        }

        public bool UpdateToken(string appkey, string token, DateTime tokenExpireTime)
        {
            return UserPlatformDao.Instance.UpdateToken(appkey,token,tokenExpireTime);
        }

        public List<UserPlatform> GetBySomeWhere(int appId, int platformId, int limit, int pageSize, out int total)
        {
            return UserPlatformDao.Instance.GetBySomeWhere(appId,platformId,limit,pageSize,out total);
        }
    }
}

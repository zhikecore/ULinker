using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULinker.Models.DO;
using ULinker.DAL;

namespace ULinker.BLL
{
    public class AdminUserService
    {
        private static AdminUserService _Instance = null;

        public static AdminUserService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new AdminUserService();
                return _Instance;
            }
        }

        private AdminUserService()
        {

        }

        public object GetUsersForCombox()
        {
            List<object> objs = new List<object>();
            List<AdminUser> users = AdminUserDao.Instance.GetAll();
            objs.Add(new { Id = -1, Name = "全部" });
            foreach (AdminUser user in users)
            {
                objs.Add(new { Id = user.Id, Name = user.RealName });
            }

            return objs;
        }

        public AdminUser GetById(int id)
        {
            return AdminUserDao.Instance.GetById(id);
        }

        public AdminUser GetByAccount(string account)
        {
            return AdminUserDao.Instance.GetByAccount(account);
        }

        public List<AdminUser> GetAll()
        {
            return AdminUserDao.Instance.GetAll();
        }
        
        public List<AdminUser> GetBySomeWhere(string keyword, int limit, int pageSize, out int total)
        {
            return AdminUserDao.Instance.GetBySomeWhere(keyword,limit,pageSize,out total);
        }

        public bool Create(AdminUser model)
        {
            return AdminUserDao.Instance.Create(model);
        }

        public bool CanUse(int id, string loginAccount)
        {
            return AdminUserDao.Instance.CanUse(id, loginAccount);
        }

        public bool PhysicDelete(int id, string loginAccount)
        {
            return AdminUserDao.Instance.PhysicDelete(id,loginAccount);
        }

        public void Init()
        {
            AdminUser u = new AdminUser
            {
                //Id=1,
                Account = "Admin@126.com",
                Phone = "18780287009",
                RealName = "SuperAdmin",
                Email = String.Empty,
                IsUse = true
            };
            var _u = AdminUserService.Instance.GetByAccount(u.Account);

            if (_u == null)
            {
                AdminUserDao.Instance.Create(u);
            }
        }
    }
}

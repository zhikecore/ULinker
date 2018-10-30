using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ULinker.BLL;
using ULinker.Models.DO;
using ULinker.Models.VO;
using ULinker.WebApp.Services;

namespace ULinker.WebApp.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            //CreateOrder();
            Test();
            return View();
        }

        public void Test()
        {
            //PersonService.Instance.Add();
            //List<AdminUser> lstUsers = AdminUserService.Instance.GetAll();
            //foreach (AdminUser user in lstUsers)
            //{
            //    if (user == null)
            //        continue;

            //    Notice notice=PersonService.Instance.Add(user);
            //}

            PersonService.Instance.Search();
        }

        public void CreateOrder()
        {
            for (int i=3;i<100;i++)
            {
                AdminUser user = new AdminUser
                {
                    Id=i,
                    RealName = String.Format("LuckyHu{0}", i),
                    Account = String.Format("LuckyHu{0}", i),
                    Email = @"576810529@qq.colm",
                    Phone="18780287009",
                    IsUse=false,
                    Avatar=String.Empty,
                    Description=String.Empty,
                    CreateTime=DateTime.Now,
                    ModifyTime=DateTime.Now
                };

                AdminUserService.Instance.Create(user);

            }

        }
    }
}
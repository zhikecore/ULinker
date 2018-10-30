using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ULinker.Models.DO;
using ULinker.WebApp.Helpers;

namespace System.Web.Mvc
{
    public static class WebViewPageExtensions
    {
        public static AdminUser CurrentAdminUser(this WebViewPage wvp)
        {
            return wvp.Session[AccountHashKeys.CurrentAdminUser] as AdminUser;
        }
    }
}
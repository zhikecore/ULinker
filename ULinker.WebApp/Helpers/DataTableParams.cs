using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ULinker.WebApp.Helpers
{
    public class DataTableParams
    {
        public DataTableParams(ControllerBase controller)
        {
            Controller = controller;
            DisplayStart = int.Parse(controller.ControllerContext.HttpContext.Request["iDisplayStart"]);
            DisplayLength = int.Parse(controller.ControllerContext.HttpContext.Request["iDisplayLength"]);
            SearchKey = controller.ControllerContext.HttpContext.Request["sSearch"];
            Echo = controller.ControllerContext.HttpContext.Request["sEcho"];
        }

        public ControllerBase Controller { get; set; }

        public int DisplayStart { get; set; }

        public int DisplayLength { get; set; }

        public int TotalCount { get; set; }

        public string SearchKey { get; set; }

        public string Echo { get; set; }
    }
}
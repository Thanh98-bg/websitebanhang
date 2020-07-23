﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnI.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri,QuanLySanPham")]
    public class HomeController : Controller
    {
        //
        // GET: /admin/Home/
        public ActionResult Index()
        {
            return View();
        }
	}
}
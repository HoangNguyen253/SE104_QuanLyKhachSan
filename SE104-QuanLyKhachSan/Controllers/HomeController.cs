﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SE104_QuanLyKhachSan.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SE104_QuanLyKhachSan.Controllers
{
    public class HomeController : Controller
    {
        const string SessionKeyUser = "_User";

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            NhanVien nhanVien_OnBoard = HttpContext.Session.Get<NhanVien>(SessionKeyUser);
            ViewData["nhanVien_OnBoard"] = nhanVien_OnBoard;
            return View();
        }

        public IActionResult SoDoPhong()
        {
            return PartialView();
        }

        public IActionResult ListBill()
        {
            return PartialView();
        }

        public IActionResult ListDetail()
        {
            return PartialView();
        }
        public IActionResult ListStaff()
        {
            return PartialView();
        }
        public IActionResult ListRoom()
        {
            return PartialView();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

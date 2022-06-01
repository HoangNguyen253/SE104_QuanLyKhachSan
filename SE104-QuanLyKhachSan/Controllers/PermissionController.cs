using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SE104_QuanLyKhachSan.Models;

namespace SE104_QuanLyKhachSan.Controllers
{
    public class PermissionController : Controller
    {
        public IActionResult Index()
        {
            Database db = new Database();
            ViewData["ListPhanQuyen"] = db.GetPhanQuyen();
            ViewData["ListQuyen"] = db.GetQuyen();
            ViewData["ListChucVu"] = db.GetChucVu();
            return PartialView();
        }

        public string SubmitPermission(IFormCollection formData)
        {
            string stringPermission = formData["stringPermission"].ToString();
            Database db = new Database();
            if (db.Decentralization(stringPermission)) return "true";
            return "false";
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace SE104_QuanLyKhachSan.Controllers
{
    public class BillController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SE104_QuanLyKhachSan.Models;

namespace SE104_QuanLyKhachSan.Controllers
{
    public class BillController : Controller
    {
        const string SessionKeyUser = "_User";

        private readonly ILogger<BillController> _logger;

        public BillController(ILogger<BillController> logger)
        {
            _logger = logger;
        }
    }
}

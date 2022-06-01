using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SE104_QuanLyKhachSan.Models;

namespace SE104_QuanLyKhachSan.Controllers
{
    public class LoginController : Controller
    {
        const string SessionKeyUser = "_User";
        public IActionResult Login()
        {
            if (HttpContext.Session.Get<NhanVien>(SessionKeyUser) != null)
            {
                return Redirect("/Home/DTtheoLoaiPhong");
            }
            return View();
        }
        public JsonResult CheckLogin (IFormCollection formLogin)
        {
            string userName = formLogin["userName"].ToString();
            string password = formLogin["password"].ToString();
            Database db  = new Database();
            NhanVien nv = db.GetUser(userName, password);
            if (nv == null)
            {
                return Json(false);
            }
            HttpContext.Session.Set<NhanVien>(SessionKeyUser, nv);
            return Json(true);
        }
    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}

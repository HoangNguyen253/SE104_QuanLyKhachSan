using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SE104_QuanLyKhachSan.Models;
using Newtonsoft.Json;
using System.Text;
using System;

namespace SE104_QuanLyKhachSan.Controllers
{
    public class LoginController : Controller
    {
        const string SessionKeyUser = "_User";
        const string SessionKeyPermission = "_Permission";
        public IActionResult Login()
        {
            if (HttpContext.Session.Get<NhanVien>(SessionKeyUser) != null)
            {
                return Redirect("/Home/Index");
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
            string permissionPerNhanVien = db.GetPhanQuyenForSession(nv.MaChucVu);
            HttpContext.Session.SetString(SessionKeyPermission, permissionPerNhanVien);
            ViewData["permissionPerNhanVien"] = permissionPerNhanVien;
            return Json(true);
        }
        private string GenerateOTP()
        {
            int length = 6;
            const string valid = "1234567890";
            //const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public string GetOTP(string email)
        {
            Database db = new Database();
            string otp = GenerateOTP();
            if (db.SetOTPForNhanVien(otp, email))
            {
                Mailer mail = new Mailer();
                string bodyMail = "<h2>Chào bạn</h2><p>Mã OTP của bạn là:" + otp + "</p>";
                if (mail.Send(email, "Mã OTP", bodyMail) == "OK")
                {
                    return "true";
                }
                else
                {
                    return "Lỗi máy chủ. Vui lòng thử lại.";
                }
            } else
            {
                return "Email sai hoặc không tồn tại";
            }
        }

        public string ResetPassword(IFormCollection formData)
        {
            string otp = formData["otp"].ToString();
            string email = formData["email"].ToString();
            Database db = new Database();
            int resultStatus = db.ResetPassword(otp, email);

            if(resultStatus == 1)
            {
                return "Mã OTP hết hạn";
            }
            if (resultStatus == 2)
            {
                return "true";
            }
            if (resultStatus == 3)
            {
                return "Mã OTP không hợp lệ";
            }
            if (resultStatus == 5)
            {
                return "Lỗi máy chủ gửi mật khẩu mới qua email không thành công";
            }
            return "Địa chỉ email không hợp lệ";
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

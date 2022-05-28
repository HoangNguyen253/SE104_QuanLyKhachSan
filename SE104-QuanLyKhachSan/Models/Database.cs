using MySql.Data.MySqlClient;
using SE104_QuanLyKhachSan.Common;
using System;
using System.Collections.Generic;

namespace SE104_QuanLyKhachSan.Models
{
    public class Database
    {
        public string ConnectionString { get; set; }
        public Database()
        {
            this.ConnectionString = "server = 127.0.0.1; user id = root; password =; port = 3307; database = gloriahotel;";
        }
        public Database(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        private MySqlConnection GetConnection()
        {
            return (new MySqlConnection(ConnectionString));
        }

        public NhanVien GetUser(string userName, string password)
        {
            using (MySqlConnection conncheck = this.GetConnection())
            {
                conncheck.Open();
                string str = "SELECT * " +
                    "FROM NhanVien " +
                    "WHERE (@userName IN (MaNhanVien, CCCD, Email, SoDienThoai)) AND MatKhau = @password;";
                MySqlCommand cmd = new MySqlCommand(str, conncheck);
                cmd.Parameters.AddWithValue("userName", userName);
                cmd.Parameters.AddWithValue("password", password);
                NhanVien nv = new NhanVien();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        nv.MaNhanVien = reader["MaNhanVien"].ToString();
                        nv.MatKhau = reader["MatKhau"].ToString();
                        nv.CCCD = reader["CCCD"].ToString();
                        nv.HoTen = reader["HoTen"].ToString();
                        nv.GioiTinh = Convert.ToByte(reader["GioiTinh"]);
                        nv.NgaySinh = Convert.ToDateTime(reader["NgaySinh"]);
                        nv.Email = reader["Email"].ToString();
                        nv.SoDienThoai = reader["SoDienThoai"].ToString();
                        nv.NgayVaoLam = Convert.ToDateTime(reader["NgayVaoLam"]);
                        nv.MaChucVu = Convert.ToUInt16(reader["MaChucVu"]);
                        nv.HinhAnh = reader["HinhAnh"].ToString();
                        nv.Luong = Convert.ToInt32(reader["Luong"]);
                        return nv;
                    }
                    else return null;
                }
            }
        }

        public List<HoaDon> getAllBill()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getAllBill, conn);
                List<HoaDon> bills = new List<HoaDon>();
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            HoaDon bill = new HoaDon();
                            bill.MaHoaDon = Convert.ToInt32(result["MaHoaDon"]);
                            bill.NV.HoTen = result["HoTen"].ToString();
                            bill.ThoiGianXuat = Convert.ToDateTime(result["ThoiGianXuat"]);
                            bill.TongSoTien = Convert.ToInt32(result["TongSoTien"]);
                            bills.Add(bill);
                        }
                        conn.Close();
                        return bills;
                    }
                    else
                    {
                        conn.Close();
                        return null;
                    }
                }
            }
        }

        public List<ChiTietHoaDon> getAllDetailBills()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getAllDetailBills, conn);
                List<ChiTietHoaDon> detailBills = new List<ChiTietHoaDon>();
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            ChiTietHoaDon detail = new ChiTietHoaDon();
                            detail.MaPhong = result["MaPhong"].ToString();
                            detail.LoaiPhong.TenLoaiPhong = result["TenLoaiPhong"].ToString();
                            detail.ThoiGianNhanPhong = Convert.ToDateTime(result["ThoiGianNhanPhong"]);
                            detail.ThoiGianTraPhong = Convert.ToDateTime(result["ThoiGianTraPhong"]);
                            detail.TrangThai = Convert.ToByte(result["TrangThai"]);
                            detailBills.Add(detail);
                        }
                        conn.Close();
                        return detailBills;
                    }
                    else
                    {
                        conn.Close();
                        return null;
                    }
                }
            }
        }
        public List<NhanVien> getAllDetailStaff()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getAllDetailStaff, conn);
                List<NhanVien> listNhanVien = new List<NhanVien>();
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            NhanVien nv = new NhanVien();
                            nv.MaNhanVien = result["MaNhanVien"].ToString();
                            nv.MatKhau = result["MatKhau"].ToString();
                            nv.CCCD = result["CCCD"].ToString();
                            nv.HoTen = result["HoTen"].ToString();
                            nv.GioiTinh = Convert.ToByte(result["GioiTinh"]);
                            nv.NgaySinh = Convert.ToDateTime(result["NgaySinh"]);
                            nv.Email = result["Email"].ToString();
                            nv.SoDienThoai = result["SoDienThoai"].ToString();
                            nv.NgayVaoLam = Convert.ToDateTime(result["NgayVaoLam"]);
                            nv.MaChucVu = Convert.ToUInt16(result["MaChucVu"]);
                            nv.HinhAnh = result["HinhAnh"].ToString();
                            nv.Luong = Convert.ToInt32(result["Luong"]);
                            listNhanVien.Add(nv);
                        }
                        conn.Close();
                        return listNhanVien;
                    }
                    else
                    {
                        conn.Close();
                        return null;
                    }
                }
            }
        }
        public List<Phong> getAllDetailRoom()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getAllDetailRoom, conn);
                List<Phong> listPhong = new List<Phong>();

                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            Phong p = new Phong();
                            p.MaPhong = result["MaPhong"].ToString();
                            p.MaLoaiPhong = Convert.ToInt32(result["MaLoaiPhong"]);
                            p.Tang = Convert.ToByte(result["Tang"]);
                            p.TenLoaiPhong = result["TenLoaiPhong"].ToString();
                            p.TrangThai = Convert.ToByte(result["TrangThai"]);
                            p.GhiChu = result["GhiChu"].ToString();


                            listPhong.Add(p);
                        }
                        conn.Close();
                        return listPhong;
                    }
                    else
                    {
                        conn.Close();
                        return null;
                    }
                }
            }
        }
        public List<LoaiPhong> getAllRoomStyle()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getAllRoomStyle, conn);
                List<LoaiPhong> listLoaiPhong = new List<LoaiPhong>();

                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            LoaiPhong lp = new LoaiPhong();
                      
                            lp.MaLoaiPhong = Convert.ToInt32(result["MaLoaiPhong"]);
                            lp.GiaTienCoBan = Convert.ToInt32(result["GiaTienCoBan"]);
                            lp.TenLoaiPhong = result["TenLoaiPhong"].ToString();
                            lp.DaXoa = Convert.ToByte(result["DaXoa"]);                     
                            listLoaiPhong.Add(lp);
                        }
                        conn.Close();
                        return listLoaiPhong;
                    }
                    else
                    {
                        conn.Close();
                        return null;
                    }
                }
            }
        }

        public object postNewRoom(Phong ph)
        {
            object a = new object();
            string @MaPhong = "";
            int @Tang = 0;
            int @MaLoaiPhong = 0;
            byte @TrangThai = 0;
            string @GhiChu = "";

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                @GhiChu = ph.GhiChu;
                @MaPhong = ph.MaPhong;
                @Tang = ph.Tang;
                @MaLoaiPhong = ph.MaLoaiPhong;
                @TrangThai = ph.TrangThai;
   
                MySqlCommand cmd = new MySqlCommand(SQLQuery.postNewRoom, conn);
            }
            return a;
        }
    }
}

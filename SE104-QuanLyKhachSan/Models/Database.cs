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
                            nv.TenChucVu = result["TenChucVu"].ToString();
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
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();            
                MySqlCommand cmd = new MySqlCommand(SQLQuery.postNewRoom, conn);
                cmd.Parameters.AddWithValue("MaPhong", ph.MaPhong);
                cmd.Parameters.AddWithValue("MaLoaiPhong", ph.MaLoaiPhong);
                cmd.Parameters.AddWithValue("Tang", ph.Tang);
                cmd.Parameters.AddWithValue("TrangThai", ph.TrangThai);
                cmd.Parameters.AddWithValue("GhiChu", ph.GhiChu);
                if(cmd.ExecuteNonQuery() == 1)
                {
                    //Form thành công
                }    
                else
                {
                    //Form báo lỗi: trùng id/ Tầng không quá 255
                }    
                conn.Close();       
            }
            return null;
           
        }
        public object postNewStaff(NhanVien nv)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.postNewStaff, conn);

                cmd.Parameters.AddWithValue("MaNhanVien", nv.MaNhanVien);
                cmd.Parameters.AddWithValue("MatKhau", nv.MatKhau);
                cmd.Parameters.AddWithValue("CCCD", nv.CCCD);
                cmd.Parameters.AddWithValue("HoTen", nv.HoTen);
                cmd.Parameters.AddWithValue("GioiTinh", nv.GioiTinh);
                cmd.Parameters.AddWithValue("NgaySinh", nv.NgaySinh);
                cmd.Parameters.AddWithValue("Email", nv.Email);
                cmd.Parameters.AddWithValue("SoDienThoai", nv.SoDienThoai);
                cmd.Parameters.AddWithValue("NgayVaoLam", nv.NgayVaoLam);
                cmd.Parameters.AddWithValue("MaChucVu", nv.MaChucVu);
                cmd.Parameters.AddWithValue("HinhAnh", nv.HinhAnh);
                cmd.Parameters.AddWithValue("Luong", nv.Luong);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    //Form thành công
                }
                else
                {
                    //Form báo lỗi:
                }
                conn.Close();
            }
            return null;

        }
        public List<ChucVu> getAllDetailRoles()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getAllDetailRoles, conn);
                List<ChucVu> listChucVu = new List<ChucVu>();
            
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            ChucVu cv = new ChucVu();
                            cv.MaChucVu = Convert.ToByte(result["MaChucVu"]);
                            cv.TenChucVu = result["TenChucVu"].ToString();
                            cv.DaXoa = Convert.ToByte(result["DaXoa"]);

                            listChucVu.Add(cv);
                        }
                        conn.Close();
                        return listChucVu;
                    }
                    else
                    {

                        conn.Close();
                        return null;
                    }
                    
                }
            }
        }
        public NhanVien getChosenStaff(string maNhanVien)
         {
             using (MySqlConnection conn = new MySqlConnection(ConnectionString))
             {
                 conn.Open();
                 MySqlCommand cmd = new MySqlCommand(SQLQuery.getChosenStaff, conn);
                 NhanVien nhanvien = new NhanVien();

                 cmd.Parameters.AddWithValue("MaNhanVien", maNhanVien);


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
                             nhanvien = nv;
                         }
                         conn.Close();
                         return nhanvien;
                     }
                     else
                     {

                         conn.Close();
                         return null;
                     }

                 }
             }
         }

        public object deleteRoom(string ph)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.deleteRoom, conn);
                cmd.Parameters.AddWithValue("MaPhong", ph);
       
                if (cmd.ExecuteNonQuery() == 1)
                {
                    //Form thành công
                }
                else
                {
                    //Form báo lỗi: trùng id/ Tầng không quá 255
                }
                conn.Close();
            }
            return null;

        }
    }
}

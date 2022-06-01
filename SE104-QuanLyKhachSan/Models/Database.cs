using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using SE104_QuanLyKhachSan.Common;
using SE104_QuanLyKhachSan.Controllers;
using System;
using System.Collections.Generic;

namespace SE104_QuanLyKhachSan.Models
{
    public class Database
    {
        public string ConnectionString { get; set; }
        public object SessionKeyUser { get; private set; }

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

        struct ThongKeKhach
        {
            public string ngay;
            public byte KhachNoiDia;
            public byte KhachNN;
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

        public NhanVien GetNV(String maNV)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getNV, conn);
                cmd.Parameters.AddWithValue("maNV", maNV);
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

        public List<HoaDon> GetAllBill()
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
                            String maNV = result["MaNhanVien"].ToString();
                            bill.NV = GetNV(maNV);
                            bill.ThoiGianXuat = Convert.ToDateTime(result["ThoiGianXuat"]);
                            bill.DoiTuongThanhToan = result["DoiTuongThanhToan"].ToString();
                            bill.TongSoTien = Convert.ToInt32(result["TongSoTien"]);
                            bill.ChiTiet = GetDetailById(bill.MaHoaDon);
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

        public LoaiPhong GetLPByMP(String maPhong)
        {
            using(MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getLPByMP, conn);
                cmd.Parameters.AddWithValue("MaPhong", maPhong);
                var result = cmd.ExecuteReader();
                if(result != null)
                {
                    result.Read();
                    LoaiPhong lp = new LoaiPhong();
                    lp.MaLoaiPhong = Convert.ToInt32(result["MaLoaiPhong"]);
                    lp.TenLoaiPhong = result["TenLoaiPhong"].ToString();
                    lp.GiaTienCoBan = Convert.ToInt32(result["GiaTienCoBan"]);
                    lp.DaXoa = Convert.ToByte(result["DaXoa"]);
                    conn.Close();
                    return lp;
                } else
                {
                    conn.Close ();
                    return null;
                }
            }
        }
        public List<ChiTietHoaDon> GetAllDetailBills()
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
                            detail.MaCTHD = Convert.ToInt32(result["MaCTHD"]);
                            detail.MaPhong = result["MaPhong"].ToString();
                            detail.LoaiPhong = GetLPByMP(detail.MaPhong);
                            detail.ThoiGianNhanPhong = Convert.ToDateTime(result["ThoiGianNhanPhong"]);
                            detail.ThoiGianTraPhong = Convert.ToDateTime(result["ThoiGianTraPhong"]);
                            detail.TrangThai = Convert.ToByte(result["TrangThai"]);
                            if(result["MaHoaDon"] == DBNull.Value)
                            {
                                detail.MaHoaDon = null;
                            } else
                            {
                                detail.MaHoaDon = Convert.ToInt32(result["MaHoaDon"]);
                            }
                            detail.GiaPhong = Convert.ToInt32(result["GiaPhong"]);
                            detail.TongTienPhong = Convert.ToInt32(result["TongTienPhong"]);
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

        public List<ChiTietHoaDon> GetDetailById(int MaHoaDon)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getDetailById, conn);
                cmd.Parameters.AddWithValue("maHoaDon", MaHoaDon);
                List<ChiTietHoaDon> details = new List<ChiTietHoaDon>();
                using(var result = cmd.ExecuteReader())
                {
                    while(result.Read())
                    {
                        ChiTietHoaDon detail = new ChiTietHoaDon();
                        detail.MaCTHD = Convert.ToInt16(result["MaCTHD"]);
                        detail.MaHoaDon = Convert.ToInt16(result["MaHoaDon"]);
                        detail.MaPhong = result["MaPhong"].ToString();
                        detail.DsSoLuong = GetListSoLuong(detail.MaCTHD);
                        detail.LoaiPhong = GetLPByMP(detail.MaPhong);
                        detail.ThoiGianNhanPhong = Convert.ToDateTime(result["ThoiGianNhanPhong"]);
                        detail.ThoiGianTraPhong = Convert.ToDateTime(result["ThoiGianTraPhong"]);
                        detail.GhiChu = result["GhiChu"].ToString();
                        detail.GiaPhong = Convert.ToInt32(result["GiaPhong"]);
                        detail.PhuThuCICO = Convert.ToInt32(result["PhuThuCICO"]);
                        detail.TongTienPhong = Convert.ToInt32(result["TongTienPhong"]);
                        detail.TrangThai = Convert.ToByte(result["TrangThai"]);
                        details.Add(detail);
                    }
                    conn.Close();
                    return details;
                }
            }
        }

        public HoaDon GetHoaDon(int MaHoaDon)
        {
            using(MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getHoaDon, conn);
                cmd.Parameters.AddWithValue("@maHoaDon", MaHoaDon);
                var result = cmd.ExecuteReader();
                result.Read();
                if(result != null)
                {
                    HoaDon hoaDon = new HoaDon();
                    hoaDon.NV = GetNV(result["MaNhanVien"].ToString());
                    hoaDon.ThoiGianXuat = Convert.ToDateTime(result["ThoiGianXuat"]);
                    hoaDon.TongSoTien = Convert.ToInt32(result["TongSoTien"]);
                    hoaDon.DoiTuongThanhToan = result["DoiTuongThanhToan"].ToString();
                    hoaDon.ChiTiet = GetDetailById(MaHoaDon);
                    return hoaDon;
                } else
                {
                    return null;
                }
            }
        }

        public ChiTietHoaDon GetNullDetailByRoomID(String roomID)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getNullDetailByRoomID, conn);
                cmd.Parameters.AddWithValue("maPhong", roomID);
                var result = cmd.ExecuteReader();
                if(result.Read())
                {
                    ChiTietHoaDon ct = new ChiTietHoaDon();
                    ct.MaCTHD = Convert.ToInt16(result["MaCTHD"]);
                    ct.MaPhong = result["MaPhong"].ToString();
                    LoaiPhong lp = GetLPByMP(ct.MaPhong);
                    ct.LoaiPhong = lp;
                    ct.MaHoaDon = null;
                    ct.ThoiGianNhanPhong = Convert.ToDateTime(result["ThoiGianNhanPhong"]);
                    ct.ThoiGianTraPhong = Convert.ToDateTime(result["ThoiGianTraPhong"]);
                    ct.GiaPhong = Convert.ToInt32(result["GiaPhong"]);
                    ct.GhiChu = result["GhiChu"].ToString();
                    ct.TongTienPhong = Convert.ToInt32(result["TongTienPhong"]);
                    ct.TrangThai = Convert.ToByte(result["TrangThai"]);
                    conn.Close();
                    return ct;
                }
                else
                {
                    conn.Close();
                    return null;
                }
            }
        }

        public bool CheckInsertDetailToBill(String roomID)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                ChiTietHoaDon detail = GetNullDetailByRoomID(roomID);
                if(detail == null)
                {
                    conn.Close();
                    return false;
                }
                else
                {
                    conn.Close();
                    return true;
                }
            }
        }

        private static List<ChiTietHoaDon> pendingList = new List<ChiTietHoaDon>();

        public void PendingDetail(String roomID)
        {
            using(MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                bool checkRoom = CheckInsertDetailToBill(roomID);
                if(checkRoom == true)
                {
                    pendingList.Add(GetNullDetailByRoomID(roomID));
                }
                conn.Close();
            }
        }

        public bool IsPendingListNotNull()
        {
            return pendingList.Count != 0;
        }

        public void CancelDetailBill()
        {
            pendingList = new List<ChiTietHoaDon>();
        }

        public double PhuThuCI(DateTime thoiGianNhanPhong)
        {
            if(thoiGianNhanPhong.Hour < 14)
            {
                if(Math.Abs(thoiGianNhanPhong.Hour - 14) <= 3)
                {
                    return 0.3;
                } else if (thoiGianNhanPhong.Hour - 14 <= 5)
                {
                    return 0.5;
                } else
                {
                    return 1;
                }
            } else
            {
                return 0;
            }
        }

        public double PhuThuCO(DateTime thoiGianTraPhong)
        {
            if(thoiGianTraPhong.Hour > 12)
            {
                if(Math.Abs(thoiGianTraPhong.Hour - 12) < 3)
                {
                    return 0.3;
                } else if(Math.Abs(thoiGianTraPhong.Hour - 12) <= 5)
                {
                    return 0.5;
                } else
                {
                    return 1;
                }
            } else { return 0; }
        }

        public int SaveListDetailBill(String maNV, String doiTuong)
        {
            if (pendingList != null)
            {
                using(MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open ();
                    HoaDon hoaDonMoi = new HoaDon();

                    MySqlCommand pendBill = new MySqlCommand (SQLQuery.insertPendingBill, conn);
                    pendBill.ExecuteNonQuery();

                    MySqlCommand getId = new MySqlCommand(SQLQuery.getPendingBillID, conn);
                    var result = getId.ExecuteReader();
                    result.Read();
                    int id = Convert.ToInt32(result["MaHoaDon"]);
                    result.Close();
                    int total = 0;
                    foreach (ChiTietHoaDon detail in pendingList)
                    {
                        int totalDetail = 0;
                        int mact = detail.MaCTHD;
                        List<SoLuongKhachThue> listSLKT = ConvertToSLKT(mact);
                        foreach(SoLuongKhachThue luongKhach in listSLKT)
                        {
                            totalDetail += luongKhach.ThanhTien;
                        }
                        int phuThuCI = (int)(PhuThuCI(detail.ThoiGianNhanPhong) * detail.GiaPhong);
                        int phuThuCO = (int)(PhuThuCO(detail.ThoiGianTraPhong) * detail.GiaPhong);
                        int tongPhuThu = phuThuCI + phuThuCO;
                        totalDetail += tongPhuThu;
                        detail.PhuThuCICO = tongPhuThu;
                        MySqlCommand updateCT = new MySqlCommand(SQLQuery.updateBillIdForDetail, conn);
                        updateCT.Parameters.AddWithValue("maHoaDon", id);
                        updateCT.Parameters.AddWithValue("maCT", mact);
                        updateCT.Parameters.AddWithValue("tongTien", totalDetail);
                        updateCT.Parameters.AddWithValue("phuThuCICO", detail.PhuThuCICO);
                        updateCT.ExecuteNonQuery();
                        total += totalDetail;
                    }
                    
                    hoaDonMoi.MaHoaDon = id;
                    hoaDonMoi.ThoiGianXuat = DateTime.Now;
                    hoaDonMoi.ChiTiet = GetDetailById(id);
                    hoaDonMoi.NV = GetNV(maNV);
                    hoaDonMoi.DoiTuongThanhToan = doiTuong;
                    hoaDonMoi.TongSoTien = total;

                    MySqlCommand updateBill = new MySqlCommand(SQLQuery.updateBillInfor, conn);
                    updateBill.Parameters.AddWithValue("maNV", hoaDonMoi.NV.MaNhanVien);
                    updateBill.Parameters.AddWithValue("thoiGianXuat", hoaDonMoi.ThoiGianXuat.ToString("yyyy-MM-dd HH:mm:ss"));
                    updateBill.Parameters.AddWithValue("tongTien", hoaDonMoi.TongSoTien);
                    updateBill.Parameters.AddWithValue("dtThanhToan", hoaDonMoi.DoiTuongThanhToan);
                    updateBill.Parameters.AddWithValue("id", hoaDonMoi.MaHoaDon);
                    updateBill.ExecuteNonQuery();

                    pendingList = new List<ChiTietHoaDon>();
                    return id;
                }
            }
            return 0;
        }

        public List<KhachThue> GetListKhachThue(int maCTHD)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getListKhachThue, conn);
                cmd.Parameters.AddWithValue("maCTHD", maCTHD);
                var result = cmd.ExecuteReader();
                List<KhachThue> listKhachThue = new List<KhachThue>();
                if(result.HasRows)
                {
                    while(result.Read())
                    {
                        KhachThue khachThue = new KhachThue();
                        khachThue.MaKhachThue = Convert.ToInt32(result["MaKhachThue"]);
                        khachThue.CCCD = result["CCCD"].ToString();
                        khachThue.ThoiGianCheckin = Convert.ToDateTime(result["ThoiGianCheckIn"]);
                        if(result["ThoiGianCheckOut"] != DBNull.Value)
                        {
                            khachThue.ThoiGianCheckout = Convert.ToDateTime(result["ThoiGianCheckOut"]);
                        } else
                        {
                            khachThue.ThoiGianCheckout = Convert.ToDateTime(null);
                        }
                        khachThue.HoTen = result["HoTen"].ToString();
                        khachThue.MaLoaiKhachHang = Convert.ToInt16(result["MaLoaiKhachHang"]);
                        khachThue.DiaChi = result["DiaChi"].ToString();
                        khachThue.MaCTHD = Convert.ToInt32(result["MaCTHD"]);
                        listKhachThue.Add(khachThue);
                    }
                    conn.Close();
                    return listKhachThue;
                } else
                {
                    conn.Close();
                    return null;
                }
            }
        }

        public List<SoLuongKhachThue> GetListSoLuong(int maCTHD)
        {
            using(MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getListSoLuong, conn);
                cmd.Parameters.AddWithValue("maCTHD", maCTHD);
                var result = cmd.ExecuteReader();
                List<SoLuongKhachThue> kq = new List<SoLuongKhachThue>();
                if(result != null)
                {
                    while(result.Read())
                    {
                        SoLuongKhachThue record = new SoLuongKhachThue();
                        record.MaCTHD = maCTHD;
                        record.SoKhachNN = Convert.ToByte(result["SoKhachNN"]);
                        record.SoKhachThue = Convert.ToByte(result["SoKhachThue"]);
                        record.SoNgayThue = Convert.ToByte(result["SoNgayThue"]);
                        record.GhiChu = result["GhiChu"].ToString();
                        record.DonGia = Convert.ToInt32(result["DonGia"]);
                        record.ThanhTien = Convert.ToInt32(result["ThanhTien"]);
                        record.PhuThu = Convert.ToInt32(result["PhuThu"]);
                        record.HeSoKhach = (float)Convert.ToDouble(result["HeSoKhach"]);
                        kq.Add(record);
                    }
                    return kq;
                } else
                {
                    return null;
                }
            }
        }
        public ChiTietHoaDon GetDetailByDetailID(int maCTHD)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getDetailByDetailId, conn);
                cmd.Parameters.AddWithValue("maCTHD", maCTHD);
                var result = cmd.ExecuteReader();
                if(result.Read())
                {
                    ChiTietHoaDon kq = new ChiTietHoaDon();
                    kq.MaCTHD = maCTHD;
                    kq.MaPhong = result["MaPhong"].ToString();
                    if(result["MaHoaDon"] == DBNull.Value)
                    {
                        kq.MaHoaDon = null;
                    } else
                    {
                        kq.MaHoaDon = Convert.ToInt32(result["MaHoaDon"]);
                    }
                    kq.ThoiGianNhanPhong = Convert.ToDateTime(result["ThoiGianNhanPhong"]);
                    kq.ThoiGianTraPhong = Convert.ToDateTime(result["ThoiGianTraPhong"]);
                    kq.GiaPhong = Convert.ToInt32(result["GiaPhong"]);
                    if(result["PhuThuCICO"] == DBNull.Value)
                    {
                        kq.PhuThuCICO = 0;
                    } else
                    {
                        kq.PhuThuCICO = Convert.ToInt32(result["PhuThuCICO"]);
                    }
                    kq.GhiChu = result["GhiChu"].ToString();
                    kq.TongTienPhong = Convert.ToInt32(result["TongTienPhong"]);
                    kq.TrangThai = Convert.ToByte(result["TrangThai"]);
                    kq.DsKhachThue = GetListKhachThue(maCTHD);
                    kq.DsSoLuong = GetListSoLuong(maCTHD);
                    return kq;
                } else
                {
                    return null;
                }
            }
        }

        public int TinhTien(int donGia, int soNgay, int phuThu, float heSo)
        {
            int result = (int)(((donGia * soNgay) + ((donGia * phuThu) / 100)) * heSo);
            return result;
        }

        public List<SoLuongKhachThue> ConvertToSLKT(int maCTHD)
        {
            
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.updateCheckOutKhach, conn);
                cmd.Parameters.AddWithValue("thoiGianCO", DateTime.Now);
                cmd.Parameters.AddWithValue("maCTHD", maCTHD);
                cmd.ExecuteNonQuery();

                cmd = new MySqlCommand(SQLQuery.getCheckInCheckOutDetail, conn);
                cmd.Parameters.AddWithValue("maCTHD", maCTHD);
                var resultTimeRange = cmd.ExecuteReader();
                resultTimeRange.Read();
                DateTime checkIn = Convert.ToDateTime(resultTimeRange["ThoiGianNhanPhong"]);
                DateTime checkOut = Convert.ToDateTime(resultTimeRange["ThoiGianTraPhong"]);
                resultTimeRange.Close();

                TimeSpan diff = checkOut - checkIn;

                List<DateTime> days = new List<DateTime>();
                if(diff.Hours > 10)
                {
                    for(int i = 0; i<=diff.Days + 1; i++)
                    {
                        days.Add(checkIn.Date.AddDays(i));
                    }
                } else
                {
                    for (int i = 0; i <= diff.Days; i++)
                    {
                        days.Add(checkIn.Date.AddDays(i));
                    }
                }

                List<ThongKeKhach> listThongKe = new List<ThongKeKhach>();

                // Convert timeCI timeCO
                string checkInString = checkIn.ToString("yyyy-MM-dd HH:mm:ss");
                string checkOutString = checkOut.ToString("yyyy-MM-dd HH:mm:ss");

                foreach (DateTime day in days)
                {
                    string ngay = day.Year + "-" + day.Month + "-" + day.Day;
                    cmd = new MySqlCommand(SQLQuery.countKhachNN, conn);
                    cmd.Parameters.AddWithValue("maCTHD", maCTHD);
                    cmd.Parameters.AddWithValue("thoiGianCO", ngay);
                    var result = cmd.ExecuteReader();
                    result.Read();

                    ThongKeKhach newRecord = new ThongKeKhach();
                    newRecord.ngay = ngay;
                    newRecord.KhachNN = Convert.ToByte(result["KhachNN"]);
                    result.Close();

                    cmd = new MySqlCommand(SQLQuery.countKhachNoiDia, conn);
                    cmd.Parameters.AddWithValue("maCTHD", maCTHD);
                    cmd.Parameters.AddWithValue("thoiGianCO", ngay);
                    result = cmd.ExecuteReader();
                    result.Read();
                    
                    newRecord.KhachNoiDia = Convert.ToByte(result["KhachNoiDia"]);
                    result.Close();
                    listThongKe.Add(newRecord);
                }
                // Xong bang thong ke luong khach theo ngay

                Dictionary<string, byte> dictCount = new Dictionary<string, byte>();
                foreach(ThongKeKhach record in listThongKe)
                {
                    // Struct key = KhachNoiDia + KhachNN
                    string key = record.KhachNoiDia.ToString() + record.KhachNN.ToString();
                    
                    if(dictCount.ContainsKey(key))
                    {
                        dictCount[key]++;
                    } else
                    {
                        dictCount.Add(key, 1);
                    }
                }
                // Da co duoc Dict so luong khach theo loai khach va theo so luong ngay
                List<SoLuongKhachThue> kq = new List<SoLuongKhachThue> ();
                foreach(KeyValuePair<string, byte> entry in dictCount)
                {
                    string key = entry.Key;
                    SoLuongKhachThue history = new SoLuongKhachThue();
                    history.SoKhachNN = Convert.ToByte(key[1].ToString());
                    history.SoKhachThue = Convert.ToByte(history.SoKhachNN + Convert.ToByte(key[0].ToString()));
                    history.SoNgayThue = entry.Value;
                    history.MaCTHD = maCTHD;
                    cmd = new MySqlCommand(SQLQuery.getGiaPhongByDetailID, conn);
                    cmd.Parameters.AddWithValue("maCTHD", maCTHD);
                    var res = cmd.ExecuteReader();
                    res.Read();
                    history.DonGia = Convert.ToInt32(res["GiaTienCoBan"]);
                    res.Close();
                    if(history.SoKhachThue > 2)
                    {
                        cmd = new MySqlCommand(SQLQuery.getPhuThuTheoSoLuong, conn);
                        cmd.Parameters.AddWithValue("soLuong", history.SoKhachThue);
                        cmd.Parameters.AddWithValue("thoiGianCI", checkInString);
                        var phuthu = cmd.ExecuteReader();
                        phuthu.Read();
                        history.PhuThu = Convert.ToInt32(phuthu["TiLePhuThu"]);
                        phuthu.Close();
                    }

                    //Tinh he so lkh
                    cmd = new MySqlCommand(SQLQuery.getHeSoLKH, conn);
                    cmd.Parameters.AddWithValue("thoiGianCI", checkIn);
                    cmd.Parameters.AddWithValue("soKhachNN", history.SoKhachNN);
                    cmd.Parameters.AddWithValue("soKhachND", Convert.ToByte(key[0].ToString()));
                    var heso = cmd.ExecuteReader();
                    heso.Read();
                    if(heso != null)
                    { 
                        history.HeSoKhach = (float)Convert.ToDouble(heso["HeSo"]);
                    } else
                    {
                        history.HeSoKhach = 1;
                    }
                    history.ThanhTien = TinhTien(history.DonGia, history.SoNgayThue, history.PhuThu, history.HeSoKhach);
                    history.GhiChu = null;
                    heso.Close();

                    // Insert to DB
                    cmd = new MySqlCommand(SQLQuery.insertSLKT, conn);
                    cmd.Parameters.AddWithValue("soLuongKhach", history.SoKhachThue);
                    cmd.Parameters.AddWithValue("soKhachNN", history.SoKhachNN);
                    cmd.Parameters.AddWithValue("soNgay", history.SoNgayThue);
                    cmd.Parameters.AddWithValue("ghiChu", history.GhiChu);
                    cmd.Parameters.AddWithValue("donGia", history.DonGia);
                    cmd.Parameters.AddWithValue("phuThu", history.PhuThu);
                    cmd.Parameters.AddWithValue("heSo", history.HeSoKhach);
                    cmd.Parameters.AddWithValue("thanhTien", history.ThanhTien);
                    cmd.Parameters.AddWithValue("maCTHD", history.MaCTHD);
                    cmd.ExecuteNonQuery();
                    kq.Add(history);
                }
                return kq;
            }
        }

        public void DeleteDetailById(int maCTHD)
        {
            using(MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand del = new MySqlCommand(SQLQuery.deleteDetailByID, conn);
                del.Parameters.AddWithValue("maCTHD", maCTHD);
                del.ExecuteNonQuery();
            }
        }

        public void UpdateCancelStatusDetail(int maCTHD)
        {
            using(MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand update = new MySqlCommand(SQLQuery.updateCancelStatusDetail, conn);
                update.Parameters.AddWithValue("maCTHD", maCTHD);
                update.ExecuteNonQuery();
            }
        }
    }
}

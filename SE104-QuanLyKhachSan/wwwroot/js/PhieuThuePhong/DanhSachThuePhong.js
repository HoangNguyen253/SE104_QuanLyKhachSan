function formatGiaTien(giaTien) {
    let giaTienAfter = "";
    let len = giaTien.length;
    for (let j = 1; j <= len; j++) {
        giaTienAfter = giaTien[len - j] + giaTienAfter;
        if (j % 3 == 0 && j != len) {
            giaTienAfter = "." + giaTienAfter;
        }
    }
    return giaTienAfter;
}

function getToday() {
    let today = new Date();
    let dd = String(today.getDate()).padStart(2, '0');
    let mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    let yyyy = today.getFullYear();

    today = yyyy + '-' + mm + '-' + dd;
    return today;
}
function Export_ToPDF_From_Content(contentElement) {
    return new Promise(function (myResolve) {
        let file_name = "PhieuThuePhong.pdf";
        html2pdf(
            contentElement,
            {
                margin: 2,
                filename: file_name,
                jsPDF: { unit: 'mm', format: 'a4', orientation: 'landscape' }
            }
        )
        myResolve()
    });
}
function Export_PhieuThuePhong_ToPDF() {
    let contentElement = document.getElementById("content_thongtinchitietthuephong_popup_window_id");
    Export_ToPDF_From_Content(contentElement).then(function (value) {
        toastMessage({ title: "Thông báo", message: "Xuất thành công!", type: "success" });
        Close_Detail_PhieuThuePhong();
    });
}
function Export_HoaDonBoPhong_ToPDF() {
    let contentElement = document.getElementById("content_receipt_export_print_popup_window_id");
    Export_ToPDF_From_Content(contentElement).then(function (value) {
        toastMessage({ title: "Thông báo", message: "Xuất thành công!", type: "success" });
        Close_HoaDonBoPhong_Printed();
    });
}
function rightclick_phieudathanhtoan() {
    let e = window.event;
    let context_menu_dstp = document.getElementById("context_menu_dstp_id");
    context_menu_dstp.querySelector("ul").innerHTML = "<li id='xemchitietptp_function_id'><p>Xem chi tiết</p></li>";
    context_menu_dstp.style.display = "block";
    context_menu_dstp.style.left = e.clientX + "px";
    context_menu_dstp.style.top = e.clientY + "px";
    $('#xemchitietptp_function_id').click(function () {
        Open_Detail_PhieuThuePhong(e.target, "dathanhtoan")
    });
}
function rightclick_phieudangthue() {
    let e = window.event;
    let context_menu_dstp = document.getElementById("context_menu_dstp_id");
    context_menu_dstp.querySelector("ul").innerHTML = "<li id='xemhientrangptp_function_id'><p>Hiện trạng</p></li><li id='huyphieuthueptp_function_id'><p>Hủy phiếu thuê</p></li>";
    context_menu_dstp.style.display = "block";
    context_menu_dstp.style.left = e.clientX + "px";
    context_menu_dstp.style.top = e.clientY + "px";
    $('#xemhientrangptp_function_id').click(function () {
        Open_Detail_PhieuThuePhong(e.target, "dangthue");
    });
    $('#huyphieuthueptp_function_id').click(function () {
        Open_Confirm_HuyPhieuThue_Popup(e.target);
    });
}
function Open_Confirm_HuyPhieuThue_Popup(e) {
    let maCTHD = e.getAttribute("macthd");
    let maPhong = e.getAttribute("maphong");
    if (!$("#notification_confirm_delete_phieuthue_window_container_id").hasClass("show")) {
        document.querySelector(".notification_confirm_delete_phieuthue_window_container .content_notification_confirm_delete_phieuthue_window").innerHTML = 'Bạn chắc chắn muốn hủy phiếu thuê phòng?<br>Mã phòng: ' + maPhong + " (đang thuê)";
        $("#btn_confirm_notification_delete_phieuthue_window").attr("macthd", maCTHD);
        $("#notification_confirm_delete_phieuthue_window_container_id").addClass("show");
    }
}
function Close_Confirm_HuyPhieuThue_Popup() {
    if ($("#notification_confirm_delete_phieuthue_window_container_id").hasClass("show")) {
        document.querySelector(".notification_confirm_delete_phieuthue_window_container .content_notification_confirm_delete_phieuthue_window").innerHTML = ' ';
        $("#btn_confirm_notification_delete_phieuthue_window").attr("macthd", '');
        $("#notification_confirm_delete_phieuthue_window_container_id").removeClass("show");
    }
}
function Xoa_PhieuThuePhongByMaCTHD(maCTHD) {
    return new Promise(function (myResolve) {
        let xhr_delete_ptp = new XMLHttpRequest();
        let url_delete_ptp = "https://localhost:5001/PhieuThuePhong/XoaPhieuThuePhong?maCTHD=" + maCTHD;
        xhr_delete_ptp.open("GET", url_delete_ptp, true);
        xhr_delete_ptp.timeout = 50000;
        xhr_delete_ptp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                myResolve(this.responseText);
            }
        }
        xhr_delete_ptp.send();
    });
}
function XacNhan_HuyPhieuThuePhong(e) {
    let macthd = e.getAttribute("macthd");
    if (macthd != null && macthd != "") {
        Xoa_PhieuThuePhongByMaCTHD(macthd).then(function (value) {
            let result = value;
            if (result == "success") {
                toastMessage({ title: "Thông báo", message: "Đã xóa phiếu thuê thành công!", type: "success" });
                Load_Data_For_DanhSachThuePhong();
            }
            else if (result == "fail") {
                toastMessage({ title: "Thông báo", message: "Xóa phiếu thuê thất bại!", type: "fail" });
            }
            Close_Confirm_HuyPhieuThue_Popup();
        });
    }
}
function rightclick_phieubophong() {
    let e = window.event;
    let context_menu_dstp = document.getElementById("context_menu_dstp_id");
    context_menu_dstp.querySelector("ul").innerHTML = "<li id='thanhtoanptp_function_id'><p>Thanh toán</p></li>";
    context_menu_dstp.style.display = "block";
    context_menu_dstp.style.left = e.clientX + "px";
    context_menu_dstp.style.top = e.clientY + "px";
    $('#thanhtoanptp_function_id').click(function () {
        Open_XacNhanThanhToanBoPhong_Popup(e.target, "bophong")
    });
}
function GetCTHDBoPhong(maCTHD) {
    return new Promise(function (myResolve) {
        let xhr_get_ptp = new XMLHttpRequest();
        let url_get_ptp = "https://localhost:5001/PhieuThuePhong/GetCTHDBoPhong?maCTHD=" + maCTHD;
        xhr_get_ptp.open("GET", url_get_ptp, true);
        xhr_get_ptp.timeout = 50000;
        xhr_get_ptp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                let dataResponse = JSON.parse(this.response);
                myResolve(dataResponse);
            }
        }
        xhr_get_ptp.send();
    });
}
function Open_XacNhanThanhToanBoPhong_Popup(e) {
    if (!$("#xacnhanthanhtoanbophong_popup_container_id").hasClass("show")) {
        let maCTHD = e.getAttribute("macthd");
        GetCTHDBoPhong(maCTHD).then(function (value) {
            let dataCTHD = value;
            if (dataCTHD != null && dataCTHD != "") {
                let infoCTHD = document.querySelectorAll(".xacnhanthanhtoanbophong_popup_container .content_xacnhanthanhtoanbophong_popup input");
                infoCTHD[0].value = dataCTHD["maPhong"];
                infoCTHD[1].value = dataCTHD["thoiGianNhanPhong"].replace("T", " ");
                infoCTHD[2].value = dataCTHD["thoiGianTraPhong"].replace("T", " ");
                $("#btn_confirm_xacnhanthanhtoanbophong_popup").attr("macthd", dataCTHD["maCTHD"]);
                $("#xacnhanthanhtoanbophong_popup_container_id").addClass("show");
            }
            else {
                toastMessage({ title: "Thông báo", message: "Đã xảy ra lỗi!", type: "fail" });
            }
        });
    }
}
function Close_XacNhanThanhToanBoPhong_Popup() {
    if ($("#xacnhanthanhtoanbophong_popup_container_id").hasClass("show")) {
        let infoCTHD = document.querySelectorAll(".xacnhanthanhtoanbophong_popup_container .content_xacnhanthanhtoanbophong_popup input");
        infoCTHD[0].value = "";
        infoCTHD[1].value = "";
        infoCTHD[2].value = "";
        infoCTHD[3].value = "";
        $("#btn_confirm_xacnhanthanhtoanbophong_popup").attr("macthd", "");
        $("#xacnhanthanhtoanbophong_popup_container_id").removeClass("show");
    }
}
function Open_Receipt_PhieuBoPhong(e) {
    if ($("#xacnhanthanhtoanbophong_popup_container_id").hasClass("show")) {
        let macthd = e.getAttribute("macthd");
        let doiTuongThanhToan = $("#xacnhanthanhtoanbophong_popup_container_id .content_xacnhanthanhtoanbophong_popup input").last().val();
        if (macthd != "" && macthd != null && doiTuongThanhToan != null && doiTuongThanhToan != "") {
            ThanhToan_PhieuThueBoPhong(macthd, doiTuongThanhToan).then(function (value) {
                Close_XacNhanThanhToanBoPhong_Popup();
                let detailHD = value;
                $("#receipt_date_receipt_export_print_popup_window_id").html(detailHD["thoiGianXuat"].substr(0, 19).replace("T", " "));
                $("#receipt_customer_receipt_export_print_popup_window_id").html(detailHD["doiTuongThanhToan"]);
                $("#receipt_staff_receipt_export_print_popup_window_id").html(detailHD["tenNhanVien"]);
                $("#number_receipt_receipt_export_print_popup_window_id").html(detailHD["maHoaDon"]);

                let renderTable = "";
                let tongSoTien = 0;

                let _lenSLTK = detailHD["dsChiTiet"]["dsSoLuongKhachThue"].length;
                renderTable = renderTable + '<tr>' +
                    '<td rowspan = "' + _lenSLTK + '"> 1</td>' +
                    '<td rowspan="' + _lenSLTK + '">' + detailHD["dsChiTiet"]["maPhong"] + '</td>' +
                    '<td>' + detailHD["dsChiTiet"]["dsSoLuongKhachThue"][0]["soKhachThue"] + '</td>' +
                    '<td>' + detailHD["dsChiTiet"]["dsSoLuongKhachThue"][0]["soNgayThue"] + '</td>' +
                    '<td>' + formatGiaTien(String(detailHD["dsChiTiet"]["dsSoLuongKhachThue"][0]["phuThu"])) + '</td>' +
                    '<td>' + formatGiaTien(String(detailHD["dsChiTiet"]["dsSoLuongKhachThue"][0]["donGia"])) + '</td>' +
                    '<td>' + formatGiaTien(String(detailHD["dsChiTiet"]["dsSoLuongKhachThue"][0]["thanhTien"])) + '</td>' +
                    '<td>' + detailHD["dsChiTiet"]["dsSoLuongKhachThue"][0]["ghiChu"].replace("\n", "<br>") + '</td>' +
                    '<td rowspan="' + _lenSLTK + '">' + formatGiaTien(String(detailHD["dsChiTiet"]["phuThuCICO"])) + '<br>' + detailHD["dsChiTiet"]["ghiChu"].replace("\n", "<br>") + '</td>' +
                    '<td rowspan="' + _lenSLTK + '">' + formatGiaTien(String(detailHD["dsChiTiet"]["tongTienPhong"])) + '</td>' +
                    '</tr>';
                tongSoTien = tongSoTien + detailHD["dsChiTiet"]["tongTienPhong"];
                if (_lenSLTK >= 2) {
                    for (let j = 1; j < detailHD["dsChiTiet"]["dsSoLuongKhachThue"].length; j++) {
                        renderTable = renderTable + '<tr>' +
                            '<td>' + detailHD["dsChiTiet"]["dsSoLuongKhachThue"][j]["soKhachThue"] + '</td>' +
                            '<td>' + detailHD["dsChiTiet"]["dsSoLuongKhachThue"][j]["soNgayThue"] + '</td>' +
                            '<td>' + formatGiaTien(String(detailHD["dsChiTiet"]["dsSoLuongKhachThue"][j]["phuThu"])) + '</td>' +
                            '<td>' + formatGiaTien(String(detailHD["dsChiTiet"]["dsSoLuongKhachThue"][j]["donGia"])) + '</td>' +
                            '<td>' + formatGiaTien(String(detailHD["dsChiTiet"]["dsSoLuongKhachThue"][j]["thanhTien"])) + '</td>' +
                            '<td>' + detailHD["dsChiTiet"]["dsSoLuongKhachThue"]["ghiChu"].replace("\n", "<br>") + '</td></tr>';
                    }
                }
                document.querySelector("#receipt_export_print_popup_window_container_id .receipt_detail_table_content_receipt_export_print_popup_window tbody").innerHTML = renderTable;
                document.querySelector("#cal_price_value_receipt_export_print_popup_window_id").innerHTML = formatGiaTien(String(tongSoTien)) + " VNĐ";
                //Mở popup
                $('#receipt_export_print_popup_window_container_id').addClass("show");
                Load_Data_For_DanhSachThuePhong();
            })
        }
        else {
            toastMessage({ title: "Thông báo", message: "Vui lòng điền đầy đủ thông tin!", type: "fail" });
        }
    }
}
function Close_HoaDonBoPhong_Printed() {
    if ($('#receipt_export_print_popup_window_container_id').hasClass("show")) {
        $("#receipt_date_receipt_export_print_popup_window_id").html(' ');
        $("#receipt_customer_receipt_export_print_popup_window_id").html(' ');
        $("#receipt_staff_receipt_export_print_popup_window_id").html(' ');
        $("#number_receipt_receipt_export_print_popup_window_id").html(' ');
        document.querySelector("#receipt_export_print_popup_window_container_id .receipt_detail_table_content_receipt_export_print_popup_window tbody").innerHTML = ' ';
        document.querySelector("#cal_price_value_receipt_export_print_popup_window_id").innerHTML = "0 VNĐ";
        $('#receipt_export_print_popup_window_container_id').removeClass("show");
    }
}
function Get_PhieuThuePhongDaThanhToan(maCTHD) {
    return new Promise(function (myResolve) {
        let xhr_get_ptp = new XMLHttpRequest();
        let url_get_ptp = "https://localhost:5001/PhieuThuePhong/GetPhieuThuePhong?maCTHD=" + maCTHD + "&trangThai=dathanhtoan";
        xhr_get_ptp.open("GET", url_get_ptp, true);
        xhr_get_ptp.timeout = 50000;
        xhr_get_ptp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                let dataResponse = JSON.parse(this.response);
                myResolve(dataResponse);
            }
        }
        xhr_get_ptp.send();
    })
}
function Get_PhieuThuePhongDangThue(maCTHD) {
    return new Promise(function (myResolve) {
        let xhr_get_ptp = new XMLHttpRequest();
        let url_get_ptp = "https://localhost:5001/PhieuThuePhong/GetPhieuThuePhong?maCTHD=" + maCTHD + "&trangThai=dangthue";
        xhr_get_ptp.open("GET", url_get_ptp, true);
        xhr_get_ptp.timeout = 50000;
        xhr_get_ptp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                let dataResponse = JSON.parse(this.response);
                myResolve(dataResponse);
            }
        }
        xhr_get_ptp.send();
    });
}

function ThanhToan_PhieuThueBoPhong(macthd, doiTuongThanhToan) {
    return new Promise(function (myResolve) {
        let form_thanhtoan_ptp = new FormData();
        form_thanhtoan_ptp.append("macthd", macthd);
        form_thanhtoan_ptp.append("doiTuongThanhToan", doiTuongThanhToan);
        let xhr_thanhtoan_ptp = new XMLHttpRequest();
        let url_thanhtoan_ptp = "https://localhost:5001/PhieuThuePhong/ThanhToanPhieuThueBoPhong";
        xhr_thanhtoan_ptp.open("POST", url_thanhtoan_ptp, true);
        xhr_thanhtoan_ptp.timeout = 50000;
        xhr_thanhtoan_ptp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                let dataResponse = JSON.parse(this.response);
                myResolve(dataResponse);
            }
        }
        xhr_thanhtoan_ptp.send(form_thanhtoan_ptp);
    });
}
function Close_Detail_PhieuThuePhong() {
    document.querySelector("#thongtinchitietthuephong_popup_window_container_id .receipt_detail_table_content_thongtinchitietthuephong_popup_window tbody").innerHTML = '';
    document.querySelector("#thongtinchitietthuephong_popup_window_container_id .thongtinchung_content_thongtinchitietthuephong_popup_window div").innerHTML = '';

    if ($("#thongtinchitietthuephong_popup_window_container_id").hasClass("show"))
        $("#thongtinchitietthuephong_popup_window_container_id").removeClass("show");
}
function Open_Detail_PhieuThuePhong(e, statusPT) {
    let macthd = e.getAttribute("macthd");
    if (statusPT == "dathanhtoan") {
        Get_PhieuThuePhongDaThanhToan(macthd).then(function (value) {
            if (value != null) {
                let detailCTHD = value;
                let _lenSLTK = detailCTHD["dsSoLuongKhachThue"].length;
                let renderTable = '';
                renderTable = renderTable + '<tr>' +
                    '<td rowspan="' + _lenSLTK + '">' + detailCTHD["maPhong"] + '</td>' +
                    '<td>' + detailCTHD["dsSoLuongKhachThue"][0]["soKhachThue"] + '</td>' +
                    '<td>' + detailCTHD["dsSoLuongKhachThue"][0]["soNgayThue"] + '</td>' +
                    '<td>' + formatGiaTien(String(detailCTHD["dsSoLuongKhachThue"][0]["phuThu"])) + '</td>' +
                    '<td>' + formatGiaTien(String(detailCTHD["dsSoLuongKhachThue"][0]["donGia"])) + '</td>' +
                    '<td>' + formatGiaTien(String(detailCTHD["dsSoLuongKhachThue"][0]["thanhTien"])) + '</td>' +
                    '<td>' + detailCTHD["dsSoLuongKhachThue"][0]["ghiChu"].replace("\n", "<br>") + '</td>' +
                    '<td rowspan="' + _lenSLTK + '">' + formatGiaTien(String(detailCTHD["phuThuCICO"])) + '<br>' + detailCTHD["ghiChu"].replace("\n", "<br>") + '</td>' +
                    '<td rowspan="' + _lenSLTK + '">' + formatGiaTien(String(detailCTHD["tongTienPhong"])) + ' VNĐ</td>' +
                    '</tr>';
                if (_lenSLTK >= 2) {
                    for (let j = 1; j < detailCTHD["dsSoLuongKhachThue"].length; j++) {
                        renderTable = renderTable + '<tr>' +
                            '<td>' + detailCTHD["dsSoLuongKhachThue"][j]["soKhachThue"] + '</td>' +
                            '<td>' + detailCTHD["dsSoLuongKhachThue"][j]["soNgayThue"] + '</td>' +
                            '<td>' + formatGiaTien(String(detailCTHD["dsSoLuongKhachThue"][j]["phuThu"])) + '</td>' +
                            '<td>' + formatGiaTien(String(detailCTHD["dsSoLuongKhachThue"][j]["donGia"])) + '</td>' +
                            '<td>' + formatGiaTien(String(detailCTHD["dsSoLuongKhachThue"][j]["thanhTien"])) + '</td>' +
                            '<td>' + detailCTHD["dsSoLuongKhachThue"][j]["ghiChu"].replace("\n", "<br>") + '</td></tr>';
                    }
                }
                document.querySelector("#thongtinchitietthuephong_popup_window_container_id .receipt_detail_table_content_thongtinchitietthuephong_popup_window tbody").innerHTML = renderTable;
                document.querySelector("#thongtinchitietthuephong_popup_window_container_id .thongtinchung_content_thongtinchitietthuephong_popup_window div").innerHTML = '<div class="thongtinchung_detail_content_thongtinchitietthuephong_popup_window">' +
                    '<p> Thuộc hóa đơn số:</p>' +
                    '<p>' + detailCTHD["maHoaDon"] + '</p>' +
                    '</div>' +
                    '<div class="thongtinchung_detail_content_thongtinchitietthuephong_popup_window">' +
                    '<p> Từ ngày:</p>' +
                    '<p>' + detailCTHD["thoiGianNhanPhong"].replace("T", " ") + '</p>' +
                    '</div>' +
                    '<div class="thongtinchung_detail_content_thongtinchitietthuephong_popup_window">' +
                    '<p>Đến ngày:</p>' +
                    '<p>' + detailCTHD["thoiGianTraPhong"].replace("T", " ") + '</p>' +
                    '</div>' +
                    '<div class="thongtinchung_detail_content_thongtinchitietthuephong_popup_window" style="color: #17e500">' +
                    '<p>Trạng thái:</p>' +
                    '<p>Đã thanh toán</p>' +
                    '</div>';
                //Mở popup
                if (!$("#thongtinchitietthuephong_popup_window_container_id").hasClass("show"))
                    $("#thongtinchitietthuephong_popup_window_container_id").addClass("show");
            }
            else {
                toastMessage({ title: "Thông báo!", message: "Đã xảy ra lỗi!", type: "fail" });
            }
        });
    }
    else if (statusPT == "dangthue") {
        Get_PhieuThuePhongDangThue(macthd).then(function (value) {
            if (value != null) {
                let detailCTHD = value;
                let _lenSLTK = detailCTHD["dsSoLuongKhachThue"].length;
                let renderTable = '';
                renderTable = renderTable + '<tr>' +
                    '<td rowspan="' + _lenSLTK + '">' + detailCTHD["maPhong"] + '</td>' +
                    '<td>' + detailCTHD["dsSoLuongKhachThue"][0]["soKhachThue"] + '</td>' +
                    '<td>' + detailCTHD["dsSoLuongKhachThue"][0]["soNgayThue"] + '</td>' +
                    '<td>' + formatGiaTien(String(detailCTHD["dsSoLuongKhachThue"][0]["phuThu"])) + '</td>' +
                    '<td>' + formatGiaTien(String(detailCTHD["dsSoLuongKhachThue"][0]["donGia"])) + '</td>' +
                    '<td>' + formatGiaTien(String(detailCTHD["dsSoLuongKhachThue"][0]["thanhTien"])) + '</td>' +
                    '<td>' + detailCTHD["dsSoLuongKhachThue"][0]["ghiChu"].replace("\n", "<br>") + '</td>' +
                    '<td rowspan="' + _lenSLTK + '">' + formatGiaTien(String(detailCTHD["phuThuCICO"])) + '<br>' + detailCTHD["ghiChu"].replace("\n", "<br>") + '</td>' +
                    '<td rowspan="' + _lenSLTK + '">' + formatGiaTien(String(detailCTHD["tongTienPhong"])) + ' VNĐ</td>' +
                    '</tr>';
                if (_lenSLTK >= 2) {
                    for (let j = 1; j < detailCTHD["dsSoLuongKhachThue"].length; j++) {
                        renderTable = renderTable + '<tr>' +
                            '<td>' + detailCTHD["dsSoLuongKhachThue"][j]["soKhachThue"] + '</td>' +
                            '<td>' + detailCTHD["dsSoLuongKhachThue"][j]["soNgayThue"] + '</td>' +
                            '<td>' + formatGiaTien(String(detailCTHD["dsSoLuongKhachThue"][j]["phuThu"])) + '</td>' +
                            '<td>' + formatGiaTien(String(detailCTHD["dsSoLuongKhachThue"][j]["donGia"])) + '</td>' +
                            '<td>' + formatGiaTien(String(detailCTHD["dsSoLuongKhachThue"][j]["thanhTien"])) + '</td>' +
                            '<td>' + detailCTHD["dsSoLuongKhachThue"][j]["ghiChu"].replace("\n", "<br>") + '</td></tr>';
                    }
                }
                document.querySelector("#thongtinchitietthuephong_popup_window_container_id .receipt_detail_table_content_thongtinchitietthuephong_popup_window tbody").innerHTML = renderTable;
                document.querySelector("#thongtinchitietthuephong_popup_window_container_id .thongtinchung_content_thongtinchitietthuephong_popup_window div").innerHTML = '<div class="thongtinchung_detail_content_thongtinchitietthuephong_popup_window">' +
                    '<p> Từ ngày:</p>' +
                    '<p>' + detailCTHD["thoiGianNhanPhong"].replace("T", " ") + '</p>' +
                    '</div>' +
                    '<div class="thongtinchung_detail_content_thongtinchitietthuephong_popup_window" style="color: #da0000">' +
                    '<p>Trạng thái:</p>' +
                    '<p>Đang thuê</p>' +
                    '</div>';
                //Mở popup
                if (!$("#thongtinchitietthuephong_popup_window_container_id").hasClass("show"))
                    $("#thongtinchitietthuephong_popup_window_container_id").addClass("show");
            }
            else {
                toastMessage({ title: "Thông báo!", message: "Đã xảy ra lỗi!", type: "fail" });
            }
        });
    }
    else if (statusPT == "bophong") {

    }
}
function Add_Event_For_DSTP() {
    // Context menu của danh sách thuê phòng - begin
    document.body.addEventListener('click', function () {
        if (document.getElementById("context_menu_dstp_id"))
            document.getElementById("context_menu_dstp_id").style.display = "none";
    })
    document.body.addEventListener('contextmenu', function () {
        if (document.getElementById("context_menu_dstp_id"))
            document.getElementById("context_menu_dstp_id").style.display = "none";
    })
    document.querySelectorAll(".phieudathanhtoan").forEach(e => {
        e.addEventListener('contextmenu', function (ev) {
            ev.stopPropagation();
            ev.preventDefault();
            rightclick_phieudathanhtoan();
            return false;
        }, false);
    })
    document.querySelectorAll(".phieudangthue").forEach(e => {
        e.addEventListener('contextmenu', function (ev) {
            ev.stopPropagation();
            ev.preventDefault();
            rightclick_phieudangthue();
            return false;
        }, false);
    })
    document.querySelectorAll(".phieubophong").forEach(e => {
        e.addEventListener('contextmenu', function (ev) {
            ev.stopPropagation();
            ev.preventDefault();
            rightclick_phieubophong();
            return false;
        }, false);
    })
}

function Load_Data_For_DanhSachThuePhong() {
    let xhr_data_dstp = new XMLHttpRequest();
    let url_data_dstp = "https://localhost:5001/PhieuThuePhong/LoadDataForDSTP";
    xhr_data_dstp.open("GET", url_data_dstp, true);
    xhr_data_dstp.timeout = 50000;
    xhr_data_dstp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            let renderData = "";
            let dataResponse = JSON.parse(this.response);
            if (dataResponse.length > 0) {
                let trangThaiPTP = '';
                let thoiGianDenNgay;
                let stt;
                let classForContextMenu;
                for (let i = 0; i < dataResponse.length; i++) {
                    stt = i + 1;
                    if (dataResponse[i]["trangThai"] == 0) {
                        trangThaiPTP = "Đang thuê";
                        classForContextMenu = "phieudangthue";
                    }
                    else if (dataResponse[i]["trangThai"] == 1) {
                        trangThaiPTP = "Đã thanh toán";
                        classForContextMenu = "phieudathanhtoan";
                    }
                    else if (dataResponse[i]["trangThai"] == 2) {
                        trangThaiPTP = "Bỏ phòng";
                        classForContextMenu = "phieubophong";
                    }
                    if (dataResponse[i]["thoiGianTraPhong"] == null) {
                        thoiGianDenNgay = "----/--/-- --:--";
                    }
                    else {
                        thoiGianDenNgay = dataResponse[i]["thoiGianTraPhong"].replace("T", " ");
                    }
                    renderData = renderData + '<tr macthd = ' + dataResponse[i]["maCTHD"] + ' class = "' + classForContextMenu + '" maphong = ' + dataResponse[i]["maPhong"] + '>' +
                        '<td class="stt_list_thuephong_table_data_window_dsthuephong">' + stt + '</td>' +
                        '<td class="maphong_list_thuephong_table_data_window_dsthuephong">' + dataResponse[i]["maPhong"] + '</td>' +
                        '<td class="loaiphong_list_thuephong_table_data_window_dsthuephong">' + dataResponse[i]["tenLoaiPhong"] + '</td>' +
                        '<td class="tungay_list_thuephong_table_data_window_dsthuephong">' + dataResponse[i]["thoiGianNhanPhong"].replace("T", " ") + '</td>' +
                        '<td class="denngay_list_thuephong_table_data_window_dsthuephong">' + thoiGianDenNgay + '</td>' +
                        '<td class="trangthai_list_thuephong_table_data_window_dsthuephong">' + trangThaiPTP + '</td>' +
                        '</tr>';
                }
                document.querySelector("#dsthuephong_main_working_window_id .data_window_dsthuephong .list_thuephong_table_data_window_dsthuephong tbody").innerHTML = renderData;
                Add_Event_For_DSTP();
            }
            else document.querySelector("#dsthuephong_main_working_window_id .data_window_dsthuephong .list_thuephong_table_data_window_dsthuephong tbody").innerHTML = '<tr><td style="column-span: 6">Không có phiếu thuê phòng nào!</td>';
        }
    }
    xhr_data_dstp.send();
}
function Standardized_Filter_DSTP() {
    $("#date_of_thuephong_filter_dsthuephong_info").val(getToday());
    $("#date_of_thuephong_filter_dsthuephong_info").attr("max", getToday());
}
function Render_Data_For_DSTP_By_Filter(data) {
    let renderData = "";
    let dataResponse = data;
    if (dataResponse.length > 0) {
        let trangThaiPTP = '';
        let thoiGianDenNgay;
        let stt;
        let classForContextMenu;
        for (let i = 0; i < dataResponse.length; i++) {
            stt = i + 1;
            if (dataResponse[i]["trangThai"] == 0) {
                trangThaiPTP = "Đang thuê";
                classForContextMenu = "phieudangthue";
            }
            else if (dataResponse[i]["trangThai"] == 1) {
                trangThaiPTP = "Đã thanh toán";
                classForContextMenu = "phieudathanhtoan";
            }
            else if (dataResponse[i]["trangThai"] == 2) {
                trangThaiPTP = "Bỏ phòng";
                classForContextMenu = "phieubophong";
            }
            if (dataResponse[i]["thoiGianTraPhong"] == null) {
                thoiGianDenNgay = "----/--/-- --:--";
            }
            else {
                thoiGianDenNgay = dataResponse[i]["thoiGianTraPhong"].replace("T", " ");
            }
            renderData = renderData + '<tr macthd = ' + dataResponse[i]["maCTHD"] + ' class = "' + classForContextMenu + '">' +
                '<td class="stt_list_thuephong_table_data_window_dsthuephong">' + stt + '</td>' +
                '<td class="maphong_list_thuephong_table_data_window_dsthuephong">' + dataResponse[i]["maPhong"] + '</td>' +
                '<td class="loaiphong_list_thuephong_table_data_window_dsthuephong">' + dataResponse[i]["tenLoaiPhong"] + '</td>' +
                '<td class="tungay_list_thuephong_table_data_window_dsthuephong">' + dataResponse[i]["thoiGianNhanPhong"].replace("T", " ") + '</td>' +
                '<td class="denngay_list_thuephong_table_data_window_dsthuephong">' + thoiGianDenNgay + '</td>' +
                '<td class="trangthai_list_thuephong_table_data_window_dsthuephong">' + trangThaiPTP + '</td>' +
                '</tr>';
        }
        document.querySelector("#dsthuephong_main_working_window_id .data_window_dsthuephong .list_thuephong_table_data_window_dsthuephong tbody").innerHTML = renderData;
        Add_Event_For_DSTP();
        toastMessage({ title: "Thông báo!", message: "Tìm kiếm thành công!", type: "success" });
    }
    else {
        document.querySelector("#dsthuephong_main_working_window_id .data_window_dsthuephong .list_thuephong_table_data_window_dsthuephong tbody").innerHTML = '<tr><td style="column-span: 6">Không có phiếu thuê phòng nào!</td>';
        toastMessage({ title: "Thông báo!", message: "Không có phiếu thuê phòng nào!", type: "fail" });
    }
}

$(document).ready(function (e) {
    Load_Data_For_DanhSachThuePhong();
    Standardized_Filter_DSTP();

    $("#filter_dsthuephong_info").submit(function (e) {
        e.preventDefault(); // prevent actual form submit
        var form = $(this);
        var url = form.attr('action'); //get submit url [replace url here if desired]
        $.ajax({
            type: "POST",
            url: url,
            data: form.serialize(), // serializes form input
            success: function (data) {
                Render_Data_For_DSTP_By_Filter(data);
            }
        });
    });
});
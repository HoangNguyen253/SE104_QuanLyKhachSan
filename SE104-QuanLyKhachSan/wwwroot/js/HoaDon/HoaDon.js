function getToday() {
    let today = new Date();
    let dd = String(today.getDate()).padStart(2, '0');
    let mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    let yyyy = today.getFullYear();

    today = yyyy + '-' + mm + '-' + dd;
    return today;
}
function getTodayTimeLocal() {
    let currentdate = new Date();
    let datetime = String(currentdate.getFullYear()).padStart(2, '0') + "-"
        + String((currentdate.getMonth() + 1)).padStart(2, '0') + "-"
        + String(currentdate.getDate()).padStart(2, '0') + "T"
        + String(currentdate.getHours()).padStart(2, '0') + ":"
        + String(currentdate.getMinutes()).padStart(2, '0');
    return datetime;
}

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


function Load_Data_For_DanhSachHoaDon() {
    let xhr_data_dshd = new XMLHttpRequest();
    let url_data_dshd = "https://localhost:5001/HoaDon/LoadDataForDSHD";
    xhr_data_dshd.open("GET", url_data_dshd, true);
    xhr_data_dshd.timeout = 50000;
    xhr_data_dshd.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            let renderData = "";
            let dataResponse = JSON.parse(this.response);
            if (dataResponse.length > 0) {
                for (let i = 0; i < dataResponse.length; i++) {
                    renderData = renderData + '<tr mahoadon = ' + dataResponse[i]["maHoaDon"] + '>' +
                        '<td class="mahoadon_list_receipt_table_data_window_dshoadon">' + dataResponse[i]["maHoaDon"] + '</td>' +
                        '<td class="nhanvienthanhtoan_list_receipt_table_data_window_dshoadon">' + dataResponse[i]["hoTenNV"] + '</td>' +
                        '<td class="thoigianxuat_list_receipt_table_data_window_dshoadon">' + dataResponse[i]["thoiGianXuat"].replace("T", " ") + '</td>' +
                        '<td class="khachthanhtoan_list_receipt_table_data_window_dshoadon">' + dataResponse[i]["doiTuongThanhToan"] + '</td>' +
                        '<td class="tongsotien_list_receipt_table_data_window_dshoadon">' + formatGiaTien(String(dataResponse[i]["tongSoTien"])) + '</td>' +
                        '<td class="print_btn_list_receipt_table_data_window_dshoadon">' +
                        '<button class="btn_print_list_receipt_table_data_window_dshoadon" onclick="Open_Old_Receipt_ToViewPrint(this)">' +
                        '<i class="fa fa-print" style="font-size:1rem"></i>' +
                        '</button>' +
                        '</td>' +
                        '</tr>';
                }
                document.querySelector("#dshoadon_main_working_window_id .data_window_dshoadon .list_receipt_table_data_window_dshoadon tbody").innerHTML = renderData;
            }
            else document.querySelector("#dshoadon_main_working_window_id .data_window_dshoadon .list_receipt_table_data_window_dshoadon tbody").innerHTML = '<tr><td style="column-span: 6">Không có hóa đơn nào</td>';
        }
    }
    xhr_data_dshd.send();
}

function Standardized_Filter() {
    $("#date_of_receipt_filter_dshoadon_info").val(getToday());
}

function Get_Info_Cashier_Staff() {
    return new Promise(function (myResolve) {
        let xhr_Get_Info_Cashier_Staff = new XMLHttpRequest();
        let url_Get_Info_Cashier_Staff = "https://localhost:5001/Home/GetStaffOnBoard";
        xhr_Get_Info_Cashier_Staff.open("POST", url_Get_Info_Cashier_Staff, true);
        xhr_Get_Info_Cashier_Staff.timeout = 5000;
        xhr_Get_Info_Cashier_Staff.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                dataResponse = JSON.parse(this.response);
                myResolve(dataResponse)
            }
        }
        xhr_Get_Info_Cashier_Staff.send();
    });
}

function Open_ThemMoiHoaDon_Popup() {
    Get_Info_Cashier_Staff().then(function (value) { // Đọc thông tin của nhân viên thu ngân
        let infoStaff = value;
        if (infoStaff == null) {
            toastMessage({ title: "Thất bại!", message: "Đã xảy ra lỗi!", type: "fail" });
        }
        else {
            //Render thông tin của hóa đơn
            let infosReceiptInputs = document.querySelectorAll(".data_field_in_thongtinhoadon_info_body_add_receipt_window_popup input");
            infosReceiptInputs[0].value = infoStaff["hoTen"];
            infosReceiptInputs[0].setAttribute("maNV",infoStaff["maNhanVien"]);
            infosReceiptInputs[1].value = getTodayTimeLocal().replace("T", " ");

            //Mở popup
            $('#add_receipt_window_popup_container_id').addClass("show");
            //// Lưu lại mã phòng đang được đặt
            //$('#receive_room_window_green_container_id').attr("maPhong", infoRoom["maPhong"]);
        }
    });
}

function Close_ThemMoiHoaDon_Popup() {
    //Clear thông tin phòng trong bảng thanh toán
    document.querySelector('.receipt_table_in_info_body_add_receipt_window_popup tbody').innerHTML = ' ';

    //Clear các input thông tin hóa đơn
    let infosReceiptInputs = document.querySelectorAll(".data_field_in_thongtinhoadon_info_body_add_receipt_window_popup input");
    infosReceiptInputs[0].value = "";
    infosReceiptInputs[0].setAttribute("maNV", "");
    infosReceiptInputs[1].value = "";
    infosReceiptInputs[2].value = "";
    document.getElementById("idphong_need_paid_id").value = null;

    //Đóng popup
    if ($('#add_receipt_window_popup_container_id').hasClass("show")) {
        $('#add_receipt_window_popup_container_id').removeClass("show");
    }
}

function Get_CTHD_Pay_Room(roomCode) {
    return new Promise(function (myResolve) {
        let xhr_Get_CTHD_Pay_Room = new XMLHttpRequest();
        let url_Get_CTHD_Pay_Room = "https://localhost:5001/HoaDon/GetCTHDPayRoom?maPhong=" + roomCode;
        xhr_Get_CTHD_Pay_Room.open("POST", url_Get_CTHD_Pay_Room, true);
        xhr_Get_CTHD_Pay_Room.timeout = 5000;
        xhr_Get_CTHD_Pay_Room.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                dataResponse = JSON.parse(this.response);
                myResolve(dataResponse)
            }
        }
        xhr_Get_CTHD_Pay_Room.send();
    });
}

function Delete_Pay_Room_Row(e) {
    e.parentElement.parentElement.remove();
    $('.receipt_table_in_info_body_add_receipt_window_popup tbody tr td.stt_receipt_table_field_in_add_receipt_window_popup').text(function (i) {
        return i + 1;
    });
}

function Add_Pay_Room_Table() {
    let roomCodeInput = document.getElementById("idphong_need_paid_id");
    if (roomCodeInput.value == "" || roomCodeInput.value == null) {
        toastMessage({ title: "Thông báo", message: "Vui lòng nhập phòng cần thanh toán", type: "fail" });
    }
    else {
        let rows = $('.receipt_table_in_info_body_add_receipt_window_popup tbody tr');
        if (rows.length >= 0) {
            let daCheckin = "NO";
            for (let i of rows) {
                if (i.getAttribute("maPhong") == roomCodeInput.value) {
                    daCheckin = "YES";
                }
            }
            if (daCheckin == "NO") {
                Get_CTHD_Pay_Room(roomCodeInput.value).then(function (value) {
                    let infoCTHD = value;
                    if (infoCTHD != null && infoCTHD != "") {
                        let stt = document.querySelectorAll(".receipt_table_in_info_body_add_receipt_window_popup tbody tr").length + 1;
                        $('<tr maPhong = ' + infoCTHD["maPhong"] + '>' +
                            '<td class="stt_receipt_table_field_in_add_receipt_window_popup">' + stt + '</td >' +
                            '<td class="service_receipt_table_field_in_add_receipt_window_popup">' + infoCTHD["maPhong"] + '</td>' +
                            '<td class="roomtype_receipt_table_field_in_add_receipt_window_popup">' + infoCTHD["tenLoaiPhong"] + '</td>' +
                            '<td class="checkin_receipt_table_field_in_add_receipt_window_popup">' + infoCTHD["thoiGianNhanPhong"].replace("T", " ") + '</td>' +
                            '<td class="checkout_receipt_table_field_in_add_receipt_window_popup">----/--/-- --:--</td>' +
                            '<td class="function_receipt_table_field_in_add_receipt_window_popup">' +
                            '<i onclick="Delete_Pay_Room_Row(this)" class="fa fa-trash" style="cursor: pointer; font-size: 1.2rem;"></i>' +
                            '</td>' +
                            '</tr>').insertAfter(document.querySelector(".receipt_table_in_info_body_add_receipt_window_popup tbody").lastChild);
                        toastMessage({ title: "Thông báo", message: "Phòng được thêm vào hóa đơn", type: "success" });
                        roomCodeInput.value = null;
                    }
                    else {
                        toastMessage({ title: "Thông báo", message: "Mã phòng sai hoặc không có dịch vụ thuê!", type: "fail" });
                    }
                });
            }
            else if (daCheckin == "YES") {
                toastMessage({ title: "Thông báo", message: "Phòng đã được thêm vào bảng", type: "fail" });
            }
        }
    }
}

function Open_HoaDon_Printed(detailHD, tenNhanVien) {
    if (!$('#receipt_export_print_popup_window_container_id').hasClass("show")) {
        //Render thông tin của hóa đơn
        $("#receipt_date_receipt_export_print_popup_window_id").html(detailHD["thoiGianTraPhong"].substr(0, 19).replace("T", " "));
        $("#receipt_customer_receipt_export_print_popup_window_id").html(detailHD["doiTuongThanhToan"]);
        $("#receipt_staff_receipt_export_print_popup_window_id").html(tenNhanVien);
        $("#number_receipt_receipt_export_print_popup_window_id").html(detailHD["maHoaDon"]);

        let renderTable = "";
        let tongSoTien = 0;

        for (let i = 0; i < detailHD["dsChiTiet"].length; i++) {
            let _lenSLTK = detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"].length;
            renderTable = renderTable + '<tr>' +
                '<td rowspan = "' + _lenSLTK + '"> 1</td>' +
                '<td rowspan="' + _lenSLTK + '">' + detailHD["dsChiTiet"][i]["maPhong"] + '</td>' +
                '<td>' + detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][0]["soKhachThue"] + '</td>' +
                '<td>' + detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][0]["soNgayThue"] + '</td>' +
                '<td>' + formatGiaTien(String(detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][0]["phuThu"])) + '</td>' +
                '<td>' + formatGiaTien(String(detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][0]["donGia"])) + '</td>' +
                '<td>' + formatGiaTien(String(detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][0]["thanhTien"])) + '</td>' +
                '<td>' + detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][0]["ghiChu"].replace("\n", "<br>") + '</td>' +
                '<td rowspan="' + _lenSLTK + '">' + formatGiaTien(String(detailHD["dsChiTiet"][i]["phuThuCICO"])) + '<br>' + detailHD["dsChiTiet"][i]["ghiChu"].replace("\n", "<br>") + '</td>' +
                '<td rowspan="' + _lenSLTK + '">' + formatGiaTien(String(detailHD["dsChiTiet"][i]["tongTienPhong"])) + '</td>' +
                '</tr>';
            tongSoTien = tongSoTien + detailHD["dsChiTiet"][i]["tongTienPhong"];
            if (_lenSLTK >= 2) {
                for (let j = 1; j < detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"].length; j++) {
                    renderTable = renderTable + '<tr>' +
                        '<td>' + detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][j]["soKhachThue"] + '</td>' +
                        '<td>' + detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][j]["soNgayThue"] + '</td>' +
                        '<td>' + formatGiaTien(String(detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][j]["phuThu"])) + '</td>' +
                        '<td>' + formatGiaTien(String(detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][j]["donGia"])) + '</td>' +
                        '<td>' + formatGiaTien(String(detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][j]["thanhTien"])) + '</td>' +
                        '<td>' + detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][j]["ghiChu"].replace("\n", "<br>") + '</td></tr>';
                }
            }
        }
        document.querySelector("#receipt_export_print_popup_window_container_id .receipt_detail_table_content_receipt_export_print_popup_window tbody").innerHTML = renderTable;
        document.querySelector("#cal_price_value_receipt_export_print_popup_window_id").innerHTML = formatGiaTien(String(tongSoTien)) + " VNĐ";
    //Mở popup
        $('#receipt_export_print_popup_window_container_id').addClass("show");
    }
}

function Close_HoaDon_Printed() {
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

function Create_New_Receipt() {
    if ($('#add_receipt_window_popup_container_id').hasClass("show")) {
        let dsPhong = $('.receipt_table_in_info_body_add_receipt_window_popup tbody tr');
        let infosReceiptInputs = document.querySelectorAll(".data_field_in_thongtinhoadon_info_body_add_receipt_window_popup input");
        if (dsPhong.length > 0 && infosReceiptInputs[2].value != null && infosReceiptInputs[2].value != "") {
            let strPhongs = "";
            for (let phong of dsPhong) {
                strPhongs = strPhongs + phong.getAttribute("maPhong") + "@";
            }
            let form_Create_New_Receipt = new FormData();
            form_Create_New_Receipt.append("strPhongs", strPhongs);
            form_Create_New_Receipt.append("doiTuongThanhToan", infosReceiptInputs[2].value);
            form_Create_New_Receipt.append("maNhanVien", infosReceiptInputs[0].getAttribute("maNV"));
            let xhr_Create_New_Receipt = new XMLHttpRequest();
            let url_Create_New_Receipt = "https://localhost:5001/HoaDon/CreateNewReceipt";
            xhr_Create_New_Receipt.open("POST", url_Create_New_Receipt, true);
            xhr_Create_New_Receipt.timeout = 50000;
            xhr_Create_New_Receipt.onreadystatechange = function () {
                if (this.readyState == 4 && this.status == 200) {
                    dataResponse = JSON.parse(this.response);
                    if (dataResponse != null && dataResponse != "") {
                        toastMessage({ title: "Thông báo", message: "Xuất hóa đơn thành công", type: "success" });
                        Open_HoaDon_Printed(dataResponse, infosReceiptInputs[0].value);
                        Close_ThemMoiHoaDon_Popup();
                        Load_Data_For_DanhSachHoaDon();
                    }
                    else {
                        toastMessage({ title: "Thông báo", message: "Đã có lỗi xảy ra!", type: "fail" });
                    }
                }
            }
            xhr_Create_New_Receipt.send(form_Create_New_Receipt);
        }
        else {
            toastMessage({ title: "Thông báo", message: "Vui lòng nhập đầy đủ thông tin hóa đơn", type: "fail" });
        }
    }
}

function Get_Detail_Old_Receipt(maHoaDon) {
    return new Promise(function (myResolve) {
        let xhr_Get_Detail_Old_Receipt = new XMLHttpRequest();
        let url_Get_Detail_Old_Receipt = "https://localhost:5001/HoaDon/GetDetailOldReceipt?maHoaDon=" + maHoaDon;
        xhr_Get_Detail_Old_Receipt.open("POST", url_Get_Detail_Old_Receipt, true);
        xhr_Get_Detail_Old_Receipt.timeout = 50000;
        xhr_Get_Detail_Old_Receipt.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                dataResponse = JSON.parse(this.response);
                myResolve(dataResponse);
            }
        }
        xhr_Get_Detail_Old_Receipt.send();
    })
}

function Open_Old_Receipt_ToViewPrint(e) {
    let maHoaDon = e.parentElement.parentElement.getAttribute("mahoadon");
    Get_Detail_Old_Receipt(maHoaDon).then(function (value) {
        let detailHD = value;
        if (!$('#receipt_export_print_popup_window_container_id').hasClass("show")) {
            $("#receipt_date_receipt_export_print_popup_window_id").html(detailHD["thoiGianTraPhong"].substr(0, 19).replace("T", " "));
            $("#receipt_customer_receipt_export_print_popup_window_id").html(detailHD["doiTuongThanhToan"]);
            $("#receipt_staff_receipt_export_print_popup_window_id").html(detailHD["nhanVienThanhToan"]);
            $("#number_receipt_receipt_export_print_popup_window_id").html(detailHD["maHoaDon"]);

            let renderTable = "";

            for (let i = 0; i < detailHD["dsChiTiet"].length; i++) {
                let _lenSLTK = detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"].length;
                renderTable = renderTable + '<tr>' +
                    '<td rowspan = "' + _lenSLTK + '"> 1</td>' +
                    '<td rowspan="' + _lenSLTK + '">' + detailHD["dsChiTiet"][i]["maPhong"] + '</td>' +
                    '<td>' + detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][0]["soKhachThue"] + '</td>' +
                    '<td>' + detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][0]["soNgayThue"] + '</td>' +
                    '<td>' + formatGiaTien(String(detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][0]["phuThu"])) + '</td>' +
                    '<td>' + formatGiaTien(String(detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][0]["donGia"])) + '</td>' +
                    '<td>' + formatGiaTien(String(detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][0]["thanhTien"])) + '</td>' +
                    '<td>' + detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][0]["ghiChu"].replace("\n", "<br>") + '</td>' +
                    '<td rowspan="' + _lenSLTK + '">' + formatGiaTien(String(detailHD["dsChiTiet"][i]["phuThuCICO"])) + '<br>' + detailHD["dsChiTiet"][i]["ghiChu"].replace("\n", "<br>") + '</td>' +
                    '<td rowspan="' + _lenSLTK + '">' + formatGiaTien(String(detailHD["dsChiTiet"][i]["tongTienPhong"])) + '</td>' +
                    '</tr>';
                if (_lenSLTK >= 2) {
                    for (let j = 1; j < detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"].length; j++) {
                        renderTable = renderTable + '<tr>' +
                            '<td>' + detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][j]["soKhachThue"] + '</td>' +
                            '<td>' + detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][j]["soNgayThue"] + '</td>' +
                            '<td>' + formatGiaTien(String(detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][j]["phuThu"])) + '</td>' +
                            '<td>' + formatGiaTien(String(detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][j]["donGia"])) + '</td>' +
                            '<td>' + formatGiaTien(String(detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][j]["thanhTien"])) + '</td>' +
                            '<td>' + detailHD["dsChiTiet"][i]["dsSoLuongKhachThue"][j]["ghiChu"].replace("\n", "<br>") + '</td></tr>';
                    }
                }
            }

            document.querySelector("#receipt_export_print_popup_window_container_id .receipt_detail_table_content_receipt_export_print_popup_window tbody").innerHTML = renderTable;
            document.querySelector("#cal_price_value_receipt_export_print_popup_window_id").innerHTML = formatGiaTien(String(detailHD["tongSoTienHD"])) + " VNĐ";
            //Mở popup
            $('#receipt_export_print_popup_window_container_id').addClass("show");
        }
    })
}
function Export_ToPDF_From_Content(contentElement) {
    return new Promise(function (myResolve) {
        let number_receipt = contentElement.querySelector("#number_receipt_receipt_export_print_popup_window_id").innerHTML;
        let file_name = "HOADON" + number_receipt + ".pdf";
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
function Export_Receipt_ToPDF() {
    let contentElement = document.getElementById("content_receipt_export_print_popup_window_id");
    Export_ToPDF_From_Content(contentElement).then(function (value) {
        toastMessage({ title: "Thông báo", message: "Xuất thành công!", type: "success" });
        Close_HoaDon_Printed();
    });
}

function Render_Data_For_DSHD_By_Filter(data) {
    let dataResponse = data;
    if (dataResponse.length > 0) {
        let renderData = "";
        for (let i = 0; i < dataResponse.length; i++) {
            renderData = renderData + '<tr mahoadon = ' + dataResponse[i]["maHoaDon"] + '>' +
                '<td class="mahoadon_list_receipt_table_data_window_dshoadon">' + dataResponse[i]["maHoaDon"] + '</td>' +
                '<td class="nhanvienthanhtoan_list_receipt_table_data_window_dshoadon">' + dataResponse[i]["hoTenNV"] + '</td>' +
                '<td class="thoigianxuat_list_receipt_table_data_window_dshoadon">' + dataResponse[i]["thoiGianXuat"].replace("T", " ") + '</td>' +
                '<td class="khachthanhtoan_list_receipt_table_data_window_dshoadon">' + dataResponse[i]["doiTuongThanhToan"] + '</td>' +
                '<td class="tongsotien_list_receipt_table_data_window_dshoadon">' + formatGiaTien(String(dataResponse[i]["tongSoTien"])) + '</td>' +
                '<td class="print_btn_list_receipt_table_data_window_dshoadon">' +
                '<button class="btn_print_list_receipt_table_data_window_dshoadon" onclick="Open_Old_Receipt_ToViewPrint(this)">' +
                '<i class="fa fa-print" style="font-size:1rem"></i>' +
                '</button>' +
                '</td>' +
                '</tr>';
        }
        document.querySelector("#dshoadon_main_working_window_id .data_window_dshoadon .list_receipt_table_data_window_dshoadon tbody").innerHTML = renderData;
        toastMessage({ title: "Thông báo", message: "Tìm kiếm thành công!", type: "success" });
    }
    else {
        toastMessage({ title: "Thông báo", message: "Không có hóa đơn nào!", type: "fail" });
        document.querySelector("#dshoadon_main_working_window_id .data_window_dshoadon .list_receipt_table_data_window_dshoadon tbody").innerHTML = '<tr><td style="column-span: 6">Không có hóa đơn nào!</td>';
    }
}

$(document).ready(function (e) {
    Load_Data_For_DanhSachHoaDon();
    Standardized_Filter();

    $("#filter_dshoadon_info").submit(function (e) {
        e.preventDefault(); // prevent actual form submit
        var form = $(this);
        var url = form.attr('action'); //get submit url [replace url here if desired]
        $.ajax({
            type: "POST",
            url: url,
            data: form.serialize(), // serializes form input
            success: function (data) {
                Render_Data_For_DSHD_By_Filter(data);
            }
        });
    });
});
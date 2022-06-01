$(document).ready(function (e) {
    // List of variables
    let popupCoverAddBill = document.getElementById("popup_id");
    let popupAddBill = document.getElementById("popup__add_bill_id");
    let popupCoverPrintBill = document.getElementById("popup_2_id");
    let popupPrintBill = document.getElementById("popup__print_bill_id");
    let closeBtn = document.getElementsByClassName("close-btn");
    let cancelBtn = document.getElementsByClassName("cancel-btn");
    let openPrintBtn = document.getElementById('issue_invoice_btn');
    let printBtn = document.getElementById('print-btn');
    let listPrintBtn = document.getElementsByClassName('bill_body_item__print_button');

    var countRoom = 0;
    

    // Add new bill
    document.getElementById("add_bill_btn").addEventListener("click", () => {
        popupCoverAddBill.style.display = "flex";
        popupAddBill.style.display = "block";
    })

    // Close modal by click outside modal body
    popupCoverAddBill.onclick = function () {
        $.ajax({
            url: '/Bill/CancelDetailBill',
            success: function () {
                countRoom = 0;
            }
        });
        $('#tbody-add-room').empty();
        if (popupCoverAddBill.style.display == "flex") {
            popupCoverAddBill.style.display = "none";    
            popupAddBill.style.display = "none";
        }
    }

    // StopPropation when click
    popupAddBill.onclick = function(e) {
        e.stopPropagation();
    }

    popupPrintBill.onclick = (e) => {
        e.stopPropagation();
    }


    // Close modal by click close button
    for (let btn of closeBtn) {
        btn.onclick = () => {
            if (popupAddBill.style.display == "block") {
                popupAddBill.style.display = "none";
                popupCoverAddBill.style.display = "none"; 
            }
        }
    }

    // Cancel modal by click cancel button
    for (let btn of cancelBtn) {
        btn.onclick = () => {
            let popupParent = btn.parentElement.parentElement.parentElement.parentElement.parentElement;
            popupParent.style.display = 'none';
        }
    }

    //------------ Remove Table add room ---------------//
    $('#cancel-add-room').click(function () {
        $.ajax({
            url: '/Bill/CancelDetailBill',
            success: function () {

            }
        });
        $('#tbody-add-room').empty();
    })

    // Event click print button
    printBtn.onclick = () => {
        popupCoverPrintBill.style.display = 'none';
        if (popupCoverAddBill.style.display == 'flex') {
            popupCoverAddBill.style.display = 'none';
        }
    }

    // Variable to render content add room id
    var contentRoomID =
        `<tr>
            <td class="data_cell"></td>
            <td class="data_cell"></td>
            <td class="data_cell"></td>
            <td class="data_cell"></td>
            <td class="data_cell"></td>
        </tr>`;
    //-------------------------------------START: Them phong-----------------------------------//
    $('#add-roomID').click(function () {
        let roomID = $('#roomID').val();
        if (roomID == "") {
            $('.error_message').css({ display: "block" });
            setTimeout(function () {
                $('.error_message').css({ display: "none" });
            }, 1500);
        } else {
            let url_check = '/Bill/CheckInsertDetailToBill?roomID=' + roomID;
            $.ajax({
                url: url_check,
                success: function (data) {
                    let result = JSON.parse(data);
                    if (result == true) {
                        $('.success_message').css({ display: "block" });
                        setTimeout(function () {
                            $('.success_message').css({ display: "none" });
                        }, 1500);
                        $('#roomID').val("");

                        let pending_url = '/Bill/PendingDetail?roomID=' + roomID;
                        $.ajax({
                            url: pending_url,
                            success: function () {
                                let urlGetRoom = '/Bill/GetNullDetailByRoomID?roomID=' + roomID;
                                $.ajax({
                                    url: urlGetRoom,
                                    dataType: 'json',
                                    success: function (data2) {
                                        var resultRoom = data2;
                                        countRoom++;
                                        let str =
                                            `<tr>
                                                <td class="data_cell">${countRoom}</td>
                                                <td class="data_cell">${resultRoom.maPhong}</td>
                                                <td class="data_cell">${resultRoom.loaiPhong.tenLoaiPhong}</td>
                                                <td class="data_cell">${resultRoom.thoiGianNhanPhong.replace('T', ' ')}</td>
                                                <td class="data_cell">${resultRoom.thoiGianTraPhong.replace('T', ' ')}</td>
                                            </tr>`;
                                        $('#tbody-add-room').append(str);
                                    }
                                });
                            }
                        });
                    } else {
                        $('.error_message').css({ display: "block" });
                        setTimeout(function () {
                            $('.error_message').css({ display: "none" });
                        }, 1500);
                        $('#roomID').val("");
                    }
                }
            })
        }
    });
    //---------------------------------END: Them phong--------------------------------//

    //-------------------------------START: Huy them phong----------------------------//
    $('#cancel-add-room').click(function () {
        $.ajax({
            url: '/Bill/CancelDetailBill',
            success: function () {
                countRoom = 0;
            }
        })
    })
    //---------------------------------END: Huy them phong----------------------------//

    //-------------------------------START: Xuat hoa don -----------------------------//
    openPrintBtn.addEventListener('click', () => {
        let url_check = '/Bill/IsPendingListNotNull';
        $.ajax({
            url: url_check,
            success: function (data) {
                if (data == true) {
                    let manv = document.getElementById('staff-account').innerHTML;
                    let doituong = $('#name-customer').val();
                    let url_process = '/Bill/SaveListDetailBill?maNV=' + manv + '&doiTuong=' + doituong;
                    $.ajax({
                        url: url_process,
                        success: function (maHD) {
                            let maHoaDon = parseInt(maHD);
                            console.log(maHoaDon);
                            popupCoverPrintBill.style.display = 'flex';
                            popupPrintBill.style.display = 'block';
                            if (maHoaDon > 0) {
                                let url_getData = '/Bill/GetHoaDon?MaHoaDon=' + maHoaDon;
                                $.ajax({
                                    url: url_getData,
                                    success: function (_data) {
                                        let result = _data;
                                        let tongTien = result.tongSoTien;
                                        let chiTiet = result.chiTiet;
                                        let soPhong = Object.keys(chiTiet).length;
                                        let str = '';
                                        for (let i = 0; i < soPhong; i++) {
                                            let luongKhach = Object.keys(chiTiet[i].dsSoLuong).length;
                                            let count = i + 1;
                                            for (let j = 0; j < luongKhach; j++) {
                                                if (j == 0) {
                                                    str +=
                                                        `<tr>
	                                                        <td class="data_cell" rowspan="${luongKhach}">${count}</td>
	                                                        <td class="data_cell" rowspan="${luongKhach}">${chiTiet[i].maPhong}</td>
                                                        `;
                                                }
                                                str +=
                                                    `<td class="data_cell">${chiTiet[i].dsSoLuong[j].soKhachThue}</td >
	                                                 <td class="data_cell">${chiTiet[i].dsSoLuong[j].soNgayThue}</td>
	                                                 <td class="data_cell">
                                                            Phụ thu: ${chiTiet[i].dsSoLuong[j].phuThu}%<br>
                                                            Hệ số: ${chiTiet[i].dsSoLuong[j].heSoKhach}</td>`;
                                                if (j == 0) {
                                                    str +=
                                                        `<td class="data_cell" rowspan="${luongKhach}">${chiTiet[i].loaiPhong.giaTienCoBan.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',')} đ</td >`;
                                                }
                                                str +=
                                                    `<td class="data_cell">${chiTiet[i].dsSoLuong[j].thanhTien.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',')}</td>`
                                                if (j == 0) {
                                                    str +=
                                                        `<td class="data_cell" rowspan="${luongKhach}">${chiTiet[i].phuThuCICO.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',')} đ</td>
                                                        <td class="data_cell" rowspan="${luongKhach}">${chiTiet[i].tongTienPhong.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',')} đ</td>`
                                                }
                                                str += `</tr>`;
                                            }
                                        }
                                        $('#body-bill').append(str);
                                        $('#staff-name-bill').text(result.nv.hoTen);
                                        $('#name-customer-bill').text(result.doiTuongThanhToan);
                                        $('#total-bill').text(tongTien.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ','));
                                        $('#time-out-bill').text(result.thoiGianXuat.replace('T', ' '));

                                        let rowTable =
                                            `<tr>
                                                <td class="data_cell">${maHoaDon}</td>
                                                <td class="data_cell">${result.nv.HoTen}</td>
                                                <td class="data_cell">${result.thoiGianXuat.replace('T', ' ')}</td>
                                                <td class="data_cell">${result.doiTuongThanhToan}</td>
                                                <td class="data_cell">&#8363;${tongTien.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',')}</td>
                                                <td class="data_cell">
                                                    <button class="bill_body_item__print_button btn">
                                                        <i class="fa fa-print" aria-hidden="true"></i>
                                                    </button>
                                                </td>
                                            </tr>`;
                                        $('#list-bill-body-id').append(rowTable);
                                    }
                                })
                            }
                        }
                    })
                } else {
                    alert('Không có phòng nào để xuất hoá đơn');
                }
            }
        })
        /*popupCoverPrintBill.style.display = 'flex';
        popupPrintBill.style.display = 'block';*/
    })

    //--------------------------------END: Xuat hoa don ------------------------------//

    //---------------------------------START: Mo Bill --------------------------------//
    for (let btn of listPrintBtn) {
        btn.onclick = function () {
            let maHoaDon = parseInt(this.parentElement.previousElementSibling.previousElementSibling.previousElementSibling.previousElementSibling.previousElementSibling.innerHTML);
            console.log(maHoaDon);
            let url_getData = '/Bill/GetHoaDon?MaHoaDon=' + maHoaDon;
            popupCoverPrintBill.style.display = 'flex';
            popupPrintBill.style.display = 'block';
            $.ajax({
                url: url_getData,
                success: function (_data) {
                    let result = _data;
                    let tongTien = result.tongSoTien;
                    let chiTiet = result.chiTiet;
                    let soPhong = Object.keys(chiTiet).length;
                    let str = '';
                    for (let i = 0; i < soPhong; i++) {
                        let luongKhach = Object.keys(chiTiet[i].dsSoLuong).length;
                        let count = i + 1;
                        for (let j = 0; j < luongKhach; j++) {
                            if (j == 0) {
                                str +=
                                    `<tr>
	                                <td class="data_cell" rowspan="${luongKhach}">${count}</td>
	                                <td class="data_cell" rowspan="${luongKhach}">${chiTiet[i].maPhong}</td>`;
                            }
                            str +=
                                `<td class="data_cell">${chiTiet[i].dsSoLuong[j].soKhachThue}</td >
	                            <td class="data_cell">${chiTiet[i].dsSoLuong[j].soNgayThue}</td>
	                            <td class="data_cell">
                                    Phụ thu: ${chiTiet[i].dsSoLuong[j].phuThu}%<br>
                                    Hệ số: ${chiTiet[i].dsSoLuong[j].heSoKhach}</td>`;
                            if (j == 0) {
                                str +=
                                    `<td class="data_cell" rowspan="${luongKhach}">${chiTiet[i].loaiPhong.giaTienCoBan.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',')} đ</td>`;
                            }
                            str +=
                                `<td class="data_cell">${chiTiet[i].dsSoLuong[j].thanhTien.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',')}</td>`
                            if (j == 0) {
                                str +=
                                    `<td class="data_cell" rowspan="${luongKhach}">${chiTiet[i].phuThuCICO.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',')} đ</td>
                                    <td class="data_cell" rowspan="${luongKhach}">${chiTiet[i].tongTienPhong.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',')} đ</td>`
                            }
                            str += `</tr>`;
                        }                        
                    }
                    $('#body-bill').html(str);
                    $('#bill-id').text(maHoaDon);
                    $('#staff-name-bill').text(result.nv.hoTen);
                    $('#name-customer-bill').text(result.doiTuongThanhToan);
                    $('#total-bill').text(tongTien.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ','));
                    $('#time-out-bill').text(result.thoiGianXuat.replace('T', ' '));
                }
            })
        }
    }
    //----------------------------------END: Mo Bill ---------------------------------//


    // ----------------- Khi click Huy sau khi xuat bill -----------------------------//
    $('#close-bill-btn').click(function () {
        if (popupCoverAddBill.style.display != 'none' || popupAddBill.style.display != 'none') {
            popupAddBill.style.display = 'none';
            popupCoverAddBill.style.display = 'none';
        }
    })
    //---------------------------------------------------------------------------------//

    //---------------------START: Filter Bill ID ----------------------//
    $("#filter-btn").on("click", function () {
        let value = $("#input-filter").val().toLowerCase();
        $("#list-bill-body-id tr").filter(function () {
            $(this).toggle($(this).find("td:eq(0)").text().toLowerCase().indexOf(value) > -1)
        });
    });
    //-----------------------END: Filter Bill ID ----------------------//
})
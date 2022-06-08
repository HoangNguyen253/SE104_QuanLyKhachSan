function getDifferenceInDays(date1, date2) {
    let d1 = new Date(date1);
    let d2 = new Date(date2);
    d1.setHours(0, 0, 0, 0);
    d2.setHours(0, 0, 0, 0);
    let diffInMs = Math.abs(d2 - d1);
    return diffInMs / (1000 * 60 * 60 * 24);
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


function Add_Event_For_SDP() {
    // Context menu của sơ đồ phòng - begin
    document.body.addEventListener('click', function () {
        if (document.getElementById("context_menu_sdp_id"))
            document.getElementById("context_menu_sdp_id").style.display = "none";
    })
    document.body.addEventListener('contextmenu', function () {
        if (document.getElementById("context_menu_sdp_id"))
            document.getElementById("context_menu_sdp_id").style.display = "none";
    })
    document.querySelectorAll(".occupied_room").forEach(e => {
        e.addEventListener('contextmenu', function (ev) {
            ev.stopPropagation();
            ev.preventDefault();
            rightclick_occupiedroom();
            return false;
        }, false);
    })
    document.querySelectorAll(".available_room").forEach(e => {
        e.addEventListener('contextmenu', function (ev) {
            ev.stopPropagation();
            ev.preventDefault();
            rightclick_availableroom();
            return false;
        }, false);
    })
    document.querySelectorAll(".returned_room").forEach(e => {
        e.addEventListener('contextmenu', function (ev) {
            ev.stopPropagation();
            ev.preventDefault();
            rightclick_returnedroom();
            return false;
        }, false);
    })
    document.querySelectorAll(".outed_room").forEach(e => {
        e.addEventListener('contextmenu', function (ev) {
            ev.stopPropagation();
            ev.preventDefault();
            rightclick_outedroom();
            return false;
        }, false);
    })
    document.querySelectorAll(".fixed_room").forEach(e => {
        e.addEventListener('contextmenu', function (ev) {
            ev.stopPropagation();
            ev.preventDefault();
            rightclick_fixedroom();
            return false;
        }, false);
    })
}

function Load_Data_For_SoDoPhong() {
    let xhr_data_sdp = new XMLHttpRequest();
    let url_data_sdp = "https://localhost:5001/SoDoPhong/LoadDataForSoDoPhong";
    xhr_data_sdp.open("GET", url_data_sdp, true);
    xhr_data_sdp.timeout = 50000;
    xhr_data_sdp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            let renderData = "";
            let dataResponse = JSON.parse(this.response);
            let currentFloor = dataResponse[0]["tang"];
            let numberOfFloor = 1;
            renderData = '<div class="floor_container">' +
                '<div class="floor_name"> Tầng <span class="order_floor">' + currentFloor + '</span></div>' +
                '<div class="list_room_of_floor">';
            for (let i = 0; i < dataResponse.length; i++) {
                if (dataResponse[i]["tang"] != currentFloor) {
                    currentFloor = dataResponse[i]["tang"];
                    numberOfFloor = numberOfFloor + 1;
                    renderData = renderData + '</div></div>' + '<div class="floor_container">' +
                        '<div class="floor_name"> Tầng <span class="order_floor">' + currentFloor + '</span ></div>' +
                        '<div class="list_room_of_floor">';
                }
                if (dataResponse[i]["tang"] == currentFloor) {
                    if (dataResponse[i]["trangThai"] == 0) {
                        renderData = renderData + '<div class="room_in_list available_room" maPhong=' + dataResponse[i]["maPhong"] + '>' +
                            '<div class="room_info_status_field" >' +
                            '<div class="room_name_in_list">Phòng ' + dataResponse[i]["maPhong"] + '</div>' +
                            '<div class="room_status_in_list">Trống</div>' +
                            '</div>' +
                            '<div class="room_status_icon">' +
                            '<img class="status_icon">' +
                            '<p>Phòng trống</p>' +
                            '</div>' +
                            '<div class="room_detail_field">' +
                            '<div class="room_type_in_list">' + dataResponse[i]["tenLoaiPhong"] + '</div>' +
                            '<div class="room_number_people_in_list"><i class="fa fa-calendar"></i> 0 ngày</div>' +
                            '</div>' +
                            '</div>';
                    }
                    else if (dataResponse[i]["trangThai"] == 1) {
                        renderData = renderData + '<div class="room_in_list occupied_room" maPhong=' + dataResponse[i]["maPhong"] + '>' +
                            '<div class="room_info_status_field">' +
                            '<div class="room_name_in_list">Phòng ' + dataResponse[i]["maPhong"] + '</div>' +
                            '<div class="room_status_in_list">Đã nhận</div>' +
                            '</div>' +
                            '<div class="room_status_icon">' +
                            '<img class="status_icon">' +
                            '<p>Đang thuê</p>' +
                            '</div>' +
                            '<div class="room_detail_field">' +
                            '<div class="room_type_in_list">' + dataResponse[i]["tenLoaiPhong"] + '</div>' +
                            '<div class="room_number_people_in_list"><i class="fa fa-calendar"></i> ' + getDifferenceInDays(dataResponse[i]["thoiGianNhanPhong"], Date.now()) + ' ngày</div>' +
                            '</div>' +
                            '</div>';
                    }
                    else if (dataResponse[i]["trangThai"] == 2) {
                        renderData = renderData + '<div class="room_in_list fixed_room" maPhong=' + dataResponse[i]["maPhong"] + '>' +
                            '<div class="room_info_status_field" >' +
                            '<div class="room_name_in_list">Phòng ' + dataResponse[i]["maPhong"] + '</div>' +
                            '<div class="room_status_in_list">Sửa chữa</div>' +
                            '</div >' +
                            '<div class="room_status_icon">' +
                            '<img class="status_icon">' +
                            '<p>Sửa chữa</p>' +
                            '</div>' +
                            '<div class="room_detail_field">' +
                            '<div class="room_type_in_list">' + dataResponse[i]["tenLoaiPhong"] + '</div>' +
                            '<div class="room_number_people_in_list"><i class="fa fa-calendar"></i> 0 ngày</div>' +
                            '</div>' +
                            '</div>';
                    }
                    else if (dataResponse[i]["trangThai"] == 3) {
                        renderData = renderData + '<div class="room_in_list outed_room" maPhong=' + dataResponse[i]["maPhong"] + '>' +
                            '<div class="room_info_status_field">' +
                            '<div class="room_name_in_list">Phòng ' + dataResponse[i]["maPhong"] + '</div>' +
                            '<div class="room_status_in_list">Ra ngoài</div>' +
                            '</div>' +
                            '<div class="room_status_icon">' +
                            '<img class="status_icon">' +
                            '<p>Ra ngoài</p>' +
                            '</div>' +
                            '<div class="room_detail_field">' +
                            '<div class="room_type_in_list">' + dataResponse[i]["tenLoaiPhong"] + '</div>' +
                            '<div class="room_number_people_in_list"><i class="fa fa-calendar"></i> ' + getDifferenceInDays(dataResponse[i]["thoiGianNhanPhong"], Date.now()) + ' ngày</div>' +
                            '</div>' +
                            '</div>';
                    }
                    else if (dataResponse[i]["trangThai"] == 4) {
                        renderData = renderData + '<div class="room_in_list returned_room" maPhong=' + dataResponse[i]["maPhong"] + '>' +
                            '<div class="room_info_status_field">' +
                            '<div class="room_name_in_list">Phòng ' + dataResponse[i]["maPhong"] + '</div>' +
                            '<div class="room_status_in_list">Đã trả</div>' +
                            '</div>' +
                            '<div class="room_status_icon">' +
                            '<img class="status_icon">' +
                            '<p>Đang dọn dẹp</p>' +
                            '</div>' +
                            '<div class="room_detail_field">' +
                            '<div class="room_type_in_list">' + dataResponse[i]["tenLoaiPhong"] + '</div>' +
                            '<div class="room_number_people_in_list"><i class="fa fa-calendar"></i> 0 ngày</div>' +
                            '</div>' +
                            '</div>';
                    }
                }
            }
            renderData = renderData + "</div></div>";
            document.querySelector("#sdp_main_working_window_id .data_window_sodophong").innerHTML = renderData;
            Add_Event_For_SDP();
            renderData = '<option selected hidden value = 0>Tầng</option>';
            for (let i = 1; i <= numberOfFloor; i++) {
                renderData = renderData + '<option value = ' + i + '>' + i + '</option>';
            }
            $("#floor_room_filter_sodophong_info").html(renderData);
        }
    }
    xhr_data_sdp.send();
}
function rightclick_occupiedroom() {
    let e = window.event;
    let context_menu_sdp = document.getElementById("context_menu_sdp_id");
    context_menu_sdp.querySelector("ul").innerHTML = "<li id='thanhtoan_function_id'><p>Thanh toán</p></li><li id='thaydoikhacho_function_id'><p>Thay đổi khách ở</p></li><li><p>Hiện trạng</p></li><li id='bophong_function_id'><p>Bỏ phòng</p></li><li id='khachrangoai_function_id'><p>Khách ra ngoài</p></li>";
    context_menu_sdp.style.display = "block";
    context_menu_sdp.style.left = e.clientX + "px";
    context_menu_sdp.style.top = e.clientY + "px";
    $('#khachrangoai_function_id').click(function () {
        Convert_To_Outed_Room(e.target)
    });
    $('#bophong_function_id').click(function () {
        Update_KhachBoPhong_For_CTHD(e.target).then(function (value) {
            let status = value;
            if (status) Convert_To_Returned_Room(e.target)
            else {
                toastMessage({ title: "Thất bại!", message: "Đã xảy ra lỗi!", type: "fail" });
            }
        });
    });
    $('#thaydoikhacho_function_id').click(function () {
        Open_ThayDoiKhachO_Popup(e.target);
    })
    $('#thanhtoan_function_id').click(function () {
        $('#dshoadon_selection_name_icon_id').trigger("click");
    })
}
function rightclick_availableroom() {
    let e = window.event;
    let context_menu_sdp = document.getElementById("context_menu_sdp_id");
    context_menu_sdp.querySelector("ul").innerHTML = "<li id='datphong_function_id'><p>Đặt phòng</p></li><li id='phonghu_function_id'><p>Phòng hư</p></li>";
    context_menu_sdp.style.display = "block";
    context_menu_sdp.style.left = e.clientX + "px";
    context_menu_sdp.style.top = e.clientY + "px";
    $('#datphong_function_id').click(function () {
        Open_DatPhong_Popup(e.target);
    })
    $('#phonghu_function_id').click(function () {
        Convert_To_Fixed_Room(e.target)
    });
}
function rightclick_returnedroom() {
    let e = window.event;
    let context_menu_sdp = document.getElementById("context_menu_sdp_id");
    context_menu_sdp.querySelector("ul").innerHTML = "<li id='phongdadon_function_id'><p>Phòng đã dọn</p></li><li id='phonghu_function_id'><p>Phòng hư</p></li>";
    context_menu_sdp.style.display = "block";
    context_menu_sdp.style.left = e.clientX + "px";
    context_menu_sdp.style.top = e.clientY + "px";
    $('#phonghu_function_id').click(function () {
        Convert_To_Fixed_Room(e.target)
    });
    $('#phongdadon_function_id').click(function () {
        Convert_To_Available_Room(e.target);
    });
}
function rightclick_outedroom() {
    let e = window.event;
    let context_menu_sdp = document.getElementById("context_menu_sdp_id");
    context_menu_sdp.querySelector("ul").innerHTML = "<li id='khachtrolai_function_id'><p>Khách trở lại</p></li><li id='bophong_function_id'><p>Bỏ phòng</p></li>";
    context_menu_sdp.style.display = "block";
    context_menu_sdp.style.left = e.clientX + "px";
    context_menu_sdp.style.top = e.clientY + "px";
    $('#khachtrolai_function_id').click(function () {
        Convert_To_Occupied_Room(e.target)
    });
    $('#bophong_function_id').click(function () {
        Update_KhachBoPhong_For_CTHD(e.target).then(function (value) {
            let status = value;
            if (status) Convert_To_Returned_Room(e.target)
            else {
                toastMessage({ title: "Thất bại!", message: "Đã xảy ra lỗi!", type: "fail" });
            }
        });
    });
}
function rightclick_fixedroom() {
    let e = window.event;
    let context_menu_sdp = document.getElementById("context_menu_sdp_id");
    context_menu_sdp.querySelector("ul").innerHTML = "<li id='phongdasua_function_id'><p>Phòng đã sửa</p></li>";
    context_menu_sdp.style.display = "block";
    context_menu_sdp.style.left = e.clientX + "px";
    context_menu_sdp.style.top = e.clientY + "px";
    $('#phongdasua_function_id').click(function () {
        Convert_To_Available_Room(e.target);
    });
}
// Context menu của sơ đồ phòng - end

function Convert_To_Returned_Room(e) {
    let maPhong = e.getAttribute("maPhong");
    let xhr_Update_Returned_Status_Room = new XMLHttpRequest();
    let url_Update_Returned_Status_Room = "https://localhost:5001/SoDoPhong/UpdateStatusForRoom?maPhong=" + maPhong + "&trangThai=returned";
    xhr_Update_Returned_Status_Room.open("POST", url_Update_Returned_Status_Room, true);
    xhr_Update_Returned_Status_Room.timeout = 5000;
    xhr_Update_Returned_Status_Room.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            let statusResponse = this.responseText;
            if (statusResponse == "Success") {
                toastMessage({ title: "Thành công!", message: "Phòng " + maPhong + " đang dọn dẹp!", type: "success" });
                // Đổi giao diện của trạng thái
                if (e.classList.contains("outed_room"))
                    e.classList.remove("outed_room");
                else if (e.classList.contains("occupied_room"))
                    e.classList.remove("occupied_room");
                e.classList.add("returned_room");
                e.querySelector(".room_status_in_list").innerHTML = "Đang dọn dẹp";
                e.querySelector(".room_status_icon p").innerHTML = "Dọn dẹp";
                e.addEventListener('contextmenu', function (ev) {
                    ev.stopPropagation();
                    ev.preventDefault();
                    rightclick_returnedroom();
                    return false;
                }, false);
            }
            else {
                toastMessage({ title: "Thất bại!", message: "Đã xảy ra lỗi!", type: "fail" });
            }
        }
    }
    xhr_Update_Returned_Status_Room.send();
}
function Convert_To_Fixed_Room(e) {
    let maPhong = e.getAttribute("maPhong");
    let xhr_Update_Fixed_Status_Room = new XMLHttpRequest();
    let url_Update_Fixed_Status_Room = "https://localhost:5001/SoDoPhong/UpdateStatusForRoom?maPhong=" + maPhong + "&trangThai=fixed";
    xhr_Update_Fixed_Status_Room.open("POST", url_Update_Fixed_Status_Room, true);
    xhr_Update_Fixed_Status_Room.timeout = 5000;
    xhr_Update_Fixed_Status_Room.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            let statusResponse = this.responseText;
            if (statusResponse == "Success") {
                toastMessage({ title: "Thành công!", message: "Phòng " + maPhong + " đang sửa chữa!", type: "success" });
                // Đổi giao diện của trạng thái
                if (e.classList.contains("available_room"))
                    e.classList.remove("available_room");
                else if (e.classList.contains("returned_room"))
                    e.classList.remove("returned_room");
                e.classList.add("fixed_room");
                e.querySelector(".room_status_in_list").innerHTML = "Sửa chữa";
                e.querySelector(".room_status_icon p").innerHTML = "Sửa chữa";
                e.addEventListener('contextmenu', function (ev) {
                    ev.stopPropagation();
                    ev.preventDefault();
                    rightclick_fixedroom();
                    return false;
                }, false);
            }
            else {
                toastMessage({ title: "Thất bại!", message: "Đã xảy ra lỗi!", type: "fail" });
            }
        }
    }
    xhr_Update_Fixed_Status_Room.send();
}
function Convert_To_Available_Room(e) {
    let maPhong = e.getAttribute("maPhong");
    let xhr_Update_Available_Status_Room = new XMLHttpRequest();
    let url_Update_Available_Status_Room = "https://localhost:5001/SoDoPhong/UpdateStatusForRoom?maPhong=" + maPhong + "&trangThai=available";
    xhr_Update_Available_Status_Room.open("POST", url_Update_Available_Status_Room, true);
    xhr_Update_Available_Status_Room.timeout = 5000;
    xhr_Update_Available_Status_Room.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            let statusResponse = this.responseText;
            if (statusResponse == "Success") {
                toastMessage({ title: "Thành công!", message: "Phòng " + maPhong + " đã sẵn sàng!", type: "success" });
                // Cập nhật trạng thái giao diện của phòng
                if (e.classList.contains("returned_room"))
                    e.classList.remove("returned_room");
                else if (e.classList.contains("outed_room"))
                    e.classList.remove("outed_room");
                else if (e.classList.contains("occupied_room"))
                    e.classList.remove("occupied_room");
                else if (e.classList.contains("fixed_room"))
                    e.classList.remove("fixed_room");
                e.classList.add("available_room");
                e.querySelector(".room_status_in_list").innerHTML = "Trống";
                e.querySelector(".room_status_icon p").innerHTML = "Phòng trống";
                e.addEventListener('contextmenu', function (ev) {
                    ev.stopPropagation();
                    ev.preventDefault();
                    rightclick_availableroom();
                    return false;
                }, false);
            }
            else {
                toastMessage({ title: "Thất bại!", message: "Đã xảy ra lỗi!", type: "fail" });
            }
        }
    }
    xhr_Update_Available_Status_Room.send();
}
function Convert_To_Outed_Room(e) {
    let maPhong = e.getAttribute("maPhong");
    let xhr_Update_Outed_Status_Room = new XMLHttpRequest();
    let url_Update_Outed_Status_Room = "https://localhost:5001/SoDoPhong/UpdateStatusForRoom?maPhong=" + maPhong + "&trangThai=outed";
    xhr_Update_Outed_Status_Room.open("POST", url_Update_Outed_Status_Room, true);
    xhr_Update_Outed_Status_Room.timeout = 5000;
    xhr_Update_Outed_Status_Room.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            let statusResponse = this.responseText;
            if (statusResponse == "Success") {
                toastMessage({ title: "Thành công!", message: "Phòng " + maPhong + " đã cập nhật trạng thái ra ngoài!", type: "success" });
                // Cập nhật trạng thái giao diện của phòng
                if (e.classList.contains("occupied_room"))
                    e.classList.remove("occupied_room");
                e.classList.add("outed_room");
                e.querySelector(".room_status_in_list").innerHTML = "Ra ngoài";
                e.querySelector(".room_status_icon p").innerHTML = "Ra ngoài";
                e.addEventListener('contextmenu', function (ev) {
                    ev.stopPropagation();
                    ev.preventDefault();
                    rightclick_outedroom();
                    return false;
                }, false);
            }
            else {
                toastMessage({ title: "Thất bại!", message: "Đã xảy ra lỗi!", type: "fail" });
            }
        }
    }
    xhr_Update_Outed_Status_Room.send();
}
function Convert_To_Occupied_Room(e) {
    let maPhong = e.getAttribute("maPhong");
    let xhr_Update_Occupied_Status_Room = new XMLHttpRequest();
    let url_Update_Occupied_Status_Room = "https://localhost:5001/SoDoPhong/UpdateStatusForRoom?maPhong=" + maPhong + "&trangThai=occupied";
    xhr_Update_Occupied_Status_Room.open("POST", url_Update_Occupied_Status_Room, true);
    xhr_Update_Occupied_Status_Room.timeout = 5000;
    xhr_Update_Occupied_Status_Room.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            let statusResponse = this.responseText;
            if (statusResponse == "Success") {
                toastMessage({ title: "Thành công!", message: "Phòng " + maPhong + " đang trong trạng thái thuê!", type: "success" });
                // Cập nhật trạng thái giao diện của phòng
                if (e.classList.contains("outed_room"))
                    e.classList.remove("outed_room");
                e.classList.add("occupied_room");
                e.querySelector(".room_status_in_list").innerHTML = "Đã nhận";
                e.querySelector(".room_status_icon p").innerHTML = "Đang thuê";
                e.addEventListener('contextmenu', function (ev) {
                    ev.stopPropagation();
                    ev.preventDefault();
                    rightclick_occupiedroom();
                    return false;
                }, false);
            }
            else {
                toastMessage({ title: "Thất bại!", message: "Đã xảy ra lỗi!", type: "fail" });
            }
        }
    }
    xhr_Update_Occupied_Status_Room.send();
}
function Update_KhachBoPhong_For_CTHD(e) {
    return new Promise(function (myResolve) {
        let maPhong = e.getAttribute("maPhong");
        let xhr_Update_KhachBoPhong_For_CTHD = new XMLHttpRequest();
        let url_Update_KhachBoPhong_For_CTHD = "https://localhost:5001/SoDoPhong/UpdateStatusForCTHD?maPhong=" + maPhong + "&trangThai=khachbophong";
        xhr_Update_KhachBoPhong_For_CTHD.open("POST", url_Update_KhachBoPhong_For_CTHD, true);
        xhr_Update_KhachBoPhong_For_CTHD.timeout = 5000;
        xhr_Update_KhachBoPhong_For_CTHD.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                let statusResponse = this.responseText;
                (statusResponse == "Success") ? myResolve(true) : myResolve(false);
            }
        }
        xhr_Update_KhachBoPhong_For_CTHD.send();
    });
}

function Close_DatPhong_Popup() {
    //Clear thông tin khách ở trong bảng
    document.querySelector('#info_customer_table_green_id tbody').innerHTML = ' ';

    //Clear các input nhập thông tin khách ở
    let info_elements = document.querySelectorAll(".mini_grid input");
    info_elements.forEach(e => {
        e.value = '';
    })

    //Clear thông tin phòng
    let infosRoomInputs = document.querySelectorAll(".data_thongtinphong_receive_room_window_green input");
    infosRoomInputs[0].value = " ";
    infosRoomInputs[1].value = " ";
    infosRoomInputs[2].value = 0;
    infosRoomInputs[3].value = " VNĐ";

    //Đóng popup
    if ($('#receive_room_window_green_container_id').hasClass("show")) {
        $('#receive_room_window_green_container_id').removeClass("show");
        if ($('#receive_room_window_green_container_id').hasClass("datphong")) {
            $('#receive_room_window_green_container_id').removeClass("datphong");
        }
        else if ($('#receive_room_window_green_container_id').hasClass("thaydoikhacho")) {
            $('#receive_room_window_green_container_id').removeClass("thaydoikhacho")
        }
    }
    //Clear mã phòng đã được đặt
    $('#receive_room_window_green_container_id').attr("maPhong", "0000");

}

function Get_Info_Room_To_Order(e) {
    return new Promise(function (myResolve) {
        let maPhong = e.getAttribute("maPhong");
        let dataResponse = "";
        let xhr_Get_Info_Room_To_Order = new XMLHttpRequest();
        let url_Get_Info_Room_To_Order = "https://localhost:5001/SoDoPhong/GetInfoRoom?maPhong=" + maPhong + "&mucDich=datphong";
        xhr_Get_Info_Room_To_Order.open("POST", url_Get_Info_Room_To_Order, true);
        xhr_Get_Info_Room_To_Order.timeout = 5000;
        xhr_Get_Info_Room_To_Order.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                dataResponse = JSON.parse(this.response);
                myResolve(dataResponse)
            }
        }
        xhr_Get_Info_Room_To_Order.send();
    });
}

function Get_Info_Ordered_Room(e) {
    return new Promise(function (myResolve) {
        let maPhong = e.getAttribute("maPhong");
        let dataResponse = "";
        let xhr_Get_Info_Ordered_Room = new XMLHttpRequest();
        let url_Get_Info_Ordered_Room = "https://localhost:5001/SoDoPhong/GetInfoRoom?maPhong=" + maPhong + "&mucDich=thaydoikhacho";
        xhr_Get_Info_Ordered_Room.open("POST", url_Get_Info_Ordered_Room, true);
        xhr_Get_Info_Ordered_Room.timeout = 5000;
        xhr_Get_Info_Ordered_Room.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                dataResponse = JSON.parse(this.response);
                myResolve(dataResponse)
            }
        }
        xhr_Get_Info_Ordered_Room.send();
    });
}

function Get_LoaiKhachHang() {
    return new Promise(function (myResolve) {
        let dataResponse = "";
        let xhr_Get_LoaiKhachHangs = new XMLHttpRequest();
        let url_Get_LoaiKhachHangs = "https://localhost:5001/SoDoPhong/GetLoaiKhachHangs";
        xhr_Get_LoaiKhachHangs.open("GET", url_Get_LoaiKhachHangs, true);
        xhr_Get_LoaiKhachHangs.timeout = 5000;
        xhr_Get_LoaiKhachHangs.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                dataResponse = JSON.parse(this.response);
                myResolve(dataResponse);
            }
        }
        xhr_Get_LoaiKhachHangs.send();
    })
}

function DatPhong_From_SDP() {
    let myPromise = new Promise(function (myResolve) {
        if ($('#receive_room_window_green_container_id').hasClass("show")) {
            if ($('#receive_room_window_green_container_id').hasClass("datphong")) {
                let thoiGianNhanPhong = $('.thoigiannhanphong_receive_room_window_green input').first().val().replace("T", " ");
                let thongTinKhachThue_Str = "";
                let maPhong = $('#receive_room_window_green_container_id').attr("maPhong");
                let infos_KhachThues = document.querySelectorAll("#info_customer_table_green_id tbody tr");
                if (infos_KhachThues.length != 0 && thoiGianNhanPhong != null && maPhong != "0000") {
                    let detail_info;
                    for (let i = 0; i < infos_KhachThues.length; i++) {
                        detail_info = infos_KhachThues[i].querySelectorAll("td");
                        thongTinKhachThue_Str = thongTinKhachThue_Str + detail_info[1].textContent + "@" +
                            detail_info[2].textContent + "@" +
                            detail_info[3].getAttribute("maloai") + "@" +
                            detail_info[4].textContent + "@" +
                            detail_info[5].textContent + "@##";
                    }
                    let form_DatPhong = new FormData();
                    form_DatPhong.append("maPhong", maPhong);
                    form_DatPhong.append("thoiGianNhanPhong", thoiGianNhanPhong);
                    form_DatPhong.append("thongTinKhachThue_Str", thongTinKhachThue_Str);
                    let xhr_DatPhong_From_SDP = new XMLHttpRequest();
                    let url_DatPhong_From_SDP = "https://localhost:5001/SoDoPhong/DatPhongFromSDP";
                    xhr_DatPhong_From_SDP.open("POST", url_DatPhong_From_SDP, true);
                    xhr_DatPhong_From_SDP.timeout = 5000;
                    xhr_DatPhong_From_SDP.onreadystatechange = function () {
                        if (this.readyState == 4 && this.status == 200) {
                            let result = this.responseText;
                            myResolve(result);
                        }
                    }
                    xhr_DatPhong_From_SDP.send(form_DatPhong);
                }
                else myResolve("No Data");
            }
            else myResolve("");
        }
        else myResolve("");
    });
    myPromise.then(function (value) {
        let result = value;
        if (result == "success") {
            toastMessage({ title: "Thành công!", message: "Đặt phòng thành công!", type: "success" });
            Load_Data_For_SoDoPhong();
        }
        else if (result == "fail") {
            toastMessage({ title: "Thất bại!", message: "Đặt phòng thất bại!", type: "fail" });
        }
        else if (result == "") {
            toastMessage({ title: "Thất bại!", message: "Đã xảy ra lỗi!", type: "fail" });
        }
        else if (result == "No Data") {
            toastMessage({ title: "Thất bại!", message: "Không có thông tin khách ở!", type: "fail" });
        }
        Close_DatPhong_Popup();
    });
}

function Open_DatPhong_Popup(room) {
    Get_Info_Room_To_Order(room).then(function (value) { // Đọc thông tin của phòng
        let infoRoom = value;
        if (infoRoom == null) {
            toastMessage({ title: "Thất bại!", message: "Đã xảy ra lỗi!", type: "fail" });
        }
        else {
            //Render thông tin của phòng
            let infosRoomInputs = document.querySelectorAll(".data_thongtinphong_receive_room_window_green input");
            infosRoomInputs[0].value = infoRoom["tenLoaiPhong"];
            infosRoomInputs[1].value = infoRoom["soPhong"];
            infosRoomInputs[2].value = infoRoom["tang"];
            infosRoomInputs[3].value = formatGiaTien(String(infoRoom["giaPhong"])) + " VNĐ";

            //Render thời gian nhận phòng (ngày hôm nay)
            $('.thoigiannhanphong_receive_room_window_green input').first().val(getTodayTimeLocal());

            //Render thời gian ở từ ngày của khách (ngày giờ hôm nay)
            $("#tungay_khachthuephong").val(getTodayTimeLocal());

            //Render loại khách hàng cho select
            Get_LoaiKhachHang().then(function (value) {
                let dsLoaiKhachHang = value;
                let optionLoaiKhachHangs = "";
                for (let i = 0; i < dsLoaiKhachHang.length; i++) {
                    optionLoaiKhachHangs = optionLoaiKhachHangs + "<option value=" + dsLoaiKhachHang[i]["maLoaiKhachHang"] + ">" + dsLoaiKhachHang[i]["tenLoaiKhachHang"] + "</option>"
                }
                $(".mini_grid select").first().html(optionLoaiKhachHangs);
            })

            //Mở popup
            $('#receive_room_window_green_container_id').addClass("show").addClass("datphong");

            // Lưu lại mã phòng đang được đặt
            $('#receive_room_window_green_container_id').attr("maPhong", infoRoom["maPhong"]);


            
        }
    });
}

function Checkout_For_KhachThue(e) {
    let myPromise = new Promise(function (myResolve) {
        if ($("#info_customer_table_green_id tbody tr i.fa-sign-out").length > 1) {
            let maKhachThue = e.parentElement.getAttribute("maKhachThue");
            let xhr_Checkout_For_KhachThue = new XMLHttpRequest();
            let url_Checkout_For_KhachThue = "https://localhost:5001/SoDoPhong/CheckoutForKhachThue?maKhachThue=" + maKhachThue;
            xhr_Checkout_For_KhachThue.open("POST", url_Checkout_For_KhachThue, true);
            xhr_Checkout_For_KhachThue.timeout = 5000;
            xhr_Checkout_For_KhachThue.onreadystatechange = function () {
                if (this.readyState == 4 && this.status == 200) {
                    myResolve(this.response);
                }
            }
            xhr_Checkout_For_KhachThue.send();
        }
        else myResolve("Can't")
    })
    myPromise.then(function (value) {
        let checkOutTime = value;
        if (checkOutTime == "Can't") {
            toastMessage({ title: "Thông báo!", message: "Phòng phải có ít nhất 1 người ở!", type: "fail" });
        }
        else {
            e.innerHTML = '';
            e.onclick = null;
            e.previousSibling.innerHTML = checkOutTime.replace("T", " ").substr(1, 16);
            toastMessage({ title: "Thông báo!", message: "Đã checkout cho khách!", type: "success" });
        }
    })
}

function Open_ThayDoiKhachO_Popup(room) {
    Get_Info_Ordered_Room(room).then(function (value) {
        let infoOrderedRoom = value;
        if (infoOrderedRoom == null) {
            toastMessage({ title: "Thất bại!", message: "Đã xảy ra lỗi!", type: "fail" });
        }
        else {
            //Render thông tin của phòng
            let infosRoomInputs = document.querySelectorAll(".data_thongtinphong_receive_room_window_green input");
            infosRoomInputs[0].value = infoOrderedRoom[0]["tenLoaiPhong"];
            infosRoomInputs[1].value = infoOrderedRoom[0]["soPhong"];
            infosRoomInputs[2].value = infoOrderedRoom[0]["tang"];
            infosRoomInputs[3].value = formatGiaTien(String(infoOrderedRoom[0]["giaPhong"])) + " VNĐ";

            //Render thời gian nhận phòng
            $('.thoigiannhanphong_receive_room_window_green input').first().val(infoOrderedRoom[0]["thoiGianNhanPhong"]);

            //Render thời gian ở từ ngày của khách (ngày giờ hôm nay)
            $("#tungay_khachthuephong").val(getTodayTimeLocal());

            //Render loại khách hàng cho select
            Get_LoaiKhachHang().then(function (value) {
                let dsLoaiKhachHang = value;
                let optionLoaiKhachHangs = "";
                for (let i = 0; i < dsLoaiKhachHang.length; i++) {
                    optionLoaiKhachHangs = optionLoaiKhachHangs + "<option value=" + dsLoaiKhachHang[i]["maLoaiKhachHang"] + ">" + dsLoaiKhachHang[i]["tenLoaiKhachHang"] + "</option>"
                }
                $(".mini_grid select").first().html(optionLoaiKhachHangs);
            })

            //Mở popup
            $('#receive_room_window_green_container_id').addClass("show").addClass("thaydoikhacho");

            // Lưu lại mã phòng đang được đặt
            $('#receive_room_window_green_container_id').attr("maPhong", infoOrderedRoom[0]["maPhong"]);

            if (infoOrderedRoom.length >= 2) {
                let stt;
                let thoiGianCO;
                let func;
                let icon;
                for (let i = 1; i < infoOrderedRoom.length; i++) {
                    stt = document.querySelectorAll(".info_customer_table_green tbody tr").length + 1;
                    if (infoOrderedRoom[i]["thoiGianCheckOut"] == null) {
                        thoiGianCO = "----/--/-- --:--";
                        func = "Checkout_For_KhachThue(this)";
                        icon = '<i class="fa fa-sign-out" style="cursor: pointer; font-size: 1.2rem;"></i>';
                    }
                    else {
                        thoiGianCO = infoOrderedRoom[i]["thoiGianCheckOut"].replace("T", " ").substr(0, 16);
                        func = "";
                        icon = "";
                    }
                    $('<tr maKhachThue = ' + infoOrderedRoom[i]["maKhachThue"] + '><td class = "order">' + stt + '</td>' +
                        '<td>' + infoOrderedRoom[i]["hoTen"] + '</td>' +
                        '<td>' + infoOrderedRoom[i]["cccd"] + '</td>' +
                        '<td maloai = ' + infoOrderedRoom[i]["maLoaiKhachHang"] + '>' + infoOrderedRoom[i]["tenLoaiKhachHang"] + '</td>' +
                        '<td>' + infoOrderedRoom[i]["diaChi"] + '</td>' +
                        '<td>' + infoOrderedRoom[i]["thoiGianCheckIn"].replace("T", " ").substr(0, 16) + '</td>' +
                        '<td>' + thoiGianCO + '</td>' +
                        '<td onclick=' + func + '>' + icon + '</td></tr>').insertAfter(document.querySelector(".info_customer_table_green tbody").lastChild)
                }
            }
        }
    })
}

function ThayDoiKhachO_From_SDP() {
    let myPromise = new Promise(function (myResolve) {
        if ($('#receive_room_window_green_container_id').hasClass("show")) {
            if ($('#receive_room_window_green_container_id').hasClass("thaydoikhacho")) {
                let thongTinKhachThue_Str = "";
                let maPhong = $('#receive_room_window_green_container_id').attr("maPhong");
                let infos_KhachThues = document.querySelectorAll("#info_customer_table_green_id tbody tr.new_Insert");
                if (infos_KhachThues.length != 0 && maPhong != "0000") {
                    let detail_info;
                    for (let i = 0; i < infos_KhachThues.length; i++) {
                        detail_info = infos_KhachThues[i].querySelectorAll("td");
                        thongTinKhachThue_Str = thongTinKhachThue_Str + detail_info[1].textContent + "@" +
                            detail_info[2].textContent + "@" +
                            detail_info[3].getAttribute("maloai") + "@" +
                            detail_info[4].textContent + "@" +
                            detail_info[5].textContent + "@##";
                    }
                    let form_ThayDoiKhachO = new FormData();
                    form_ThayDoiKhachO.append("maPhong", maPhong);
                    form_ThayDoiKhachO.append("thongTinKhachThue_Str", thongTinKhachThue_Str);
                    let xhr_ThayDoiKhachO_From_SDP = new XMLHttpRequest();
                    let url_ThayDoiKhachO_From_SDP = "https://localhost:5001/SoDoPhong/ThayDoiKhachOFromSDP";
                    xhr_ThayDoiKhachO_From_SDP.open("POST", url_ThayDoiKhachO_From_SDP, true);
                    xhr_ThayDoiKhachO_From_SDP.timeout = 5000;
                    xhr_ThayDoiKhachO_From_SDP.onreadystatechange = function () {
                        if (this.readyState == 4 && this.status == 200) {
                            let result = this.responseText;
                            myResolve(result);
                        }
                    }
                    xhr_ThayDoiKhachO_From_SDP.send(form_ThayDoiKhachO);
                }
                else myResolve("");
            }
            else myResolve("");
        }
        else myResolve("");
    });
    myPromise.then(function (value) {
        let result = value;
        if (result == "success") {
            toastMessage({ title: "Thành công!", message: "Cập nhật thông tin khách thành công!", type: "success" });
            Load_Data_For_SoDoPhong();
        }
        else if (result == "fail") {
            toastMessage({ title: "Thất bại!", message: "Cập nhật thất bại!", type: "fail" });
        }
        else if (result == "") {
            toastMessage({ title: "Thất bại!", message: "Đã xảy ra lỗi!", type: "fail" });
        }
        Close_DatPhong_Popup();
    });
}



function Delete_Customer_Info(e) {
    e.parentElement.remove();
    $('.info_customer_table_green tbody tr td.order').text(function (i) {
        return i + 1;
    });
}

function CheckDieuKienSoKhachToiDa() {
    return new Promise(function (myResolve) {
        let dataResponse = 0;
        let xhr_Get_SoKhachToiDa = new XMLHttpRequest();
        let url_Get_SoKhachToiDa = "https://localhost:5001/SoDoPhong/GetSoKhachToiDa";
        xhr_Get_SoKhachToiDa.open("GET", url_Get_SoKhachToiDa, true);
        xhr_Get_SoKhachToiDa.timeout = 5000;
        xhr_Get_SoKhachToiDa.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                dataResponse = this.response;
                myResolve(dataResponse)
            }
        }
        xhr_Get_SoKhachToiDa.send();
    })
}
function Add_Info_Customer_Table() {
    let soKhachTrongPhong = document.querySelectorAll(".info_customer_table_green tbody tr.new_Insert").length + document.querySelectorAll(".info_customer_table_green tbody tr td i.fa-sign-out").length;
    CheckDieuKienSoKhachToiDa().then(function (value) {
        let result = value;
        if (Number(result) > Number(soKhachTrongPhong)) {
            let info_elements = document.querySelectorAll(".mini_grid input");
            if (info_elements[0].value == "" || info_elements[1].value == "" || info_elements[2].value == "" || info_elements[3].value == "") {
                toastMessage({ title: "Thông báo", message: "Vui lòng điền đầy đủ thông tin", type: "fail" });
            }
            else {
                let loaikhachhang = document.querySelector(".mini_grid select");
                let stt = document.querySelectorAll(".info_customer_table_green tbody tr").length + 1;
                $('<tr class = "new_Insert"><td class = "order">' + stt + '</td>' +
                    '<td>' + info_elements[1].value + '</td>' +
                    '<td>' + info_elements[0].value + '</td>' +
                    '<td maloai = ' + loaikhachhang.value + '>' + loaikhachhang.options[loaikhachhang.selectedIndex].text + '</td>' +
                    '<td>' + info_elements[2].value + '</td>' +
                    '<td>' + info_elements[3].value.replace("T", " ") + '</td>' +
                    '<td>' + "----/--/-- --:--" + '</td>' +
                    '<td onclick="Delete_Customer_Info(this)"><i class="fa fa-trash" style="cursor: pointer; font-size: 1.2rem;"></i></td></tr>').insertAfter(document.querySelector(".info_customer_table_green tbody").lastChild)
                info_elements.forEach(e => {
                    e.value = '';
                })
                info_elements[3].value = getTodayTimeLocal();
            }
        }
        else {
            toastMessage({ title: "Thất bại!", message: "Vượt quá số khách tối đa: " + result + "!", type: "fail" });
        }
    })
}

function Render_Data_For_SDP_By_Filter(data) {
    let dataResponse = data;
    if (dataResponse.length > 0) {
        let currentFloor = dataResponse[0]["tang"];
        let numberOfFloor = 1;
        renderData = '<div class="floor_container">' +
            '<div class="floor_name"> Tầng <span class="order_floor">' + currentFloor + '</span></div>' +
            '<div class="list_room_of_floor">';
        for (let i = 0; i < dataResponse.length; i++) {
            if (dataResponse[i]["tang"] != currentFloor) {
                currentFloor = dataResponse[i]["tang"];
                numberOfFloor = numberOfFloor + 1;
                renderData = renderData + '</div></div>' + '<div class="floor_container">' +
                    '<div class="floor_name"> Tầng <span class="order_floor">' + currentFloor + '</span ></div>' +
                    '<div class="list_room_of_floor">';
            }
            if (dataResponse[i]["tang"] == currentFloor) {
                if (dataResponse[i]["trangThai"] == 0) {
                    renderData = renderData + '<div class="room_in_list available_room" maPhong=' + dataResponse[i]["maPhong"] + '>' +
                        '<div class="room_info_status_field" >' +
                        '<div class="room_name_in_list">Phòng ' + dataResponse[i]["maPhong"] + '</div>' +
                        '<div class="room_status_in_list">Trống</div>' +
                        '</div>' +
                        '<div class="room_status_icon">' +
                        '<img class="status_icon">' +
                        '<p>Phòng trống</p>' +
                        '</div>' +
                        '<div class="room_detail_field">' +
                        '<div class="room_type_in_list">' + dataResponse[i]["tenLoaiPhong"] + '</div>' +
                        '<div class="room_number_people_in_list"><i class="fa fa-calendar"></i> 0 ngày</div>' +
                        '</div>' +
                        '</div>';
                }
                else if (dataResponse[i]["trangThai"] == 1) {
                    renderData = renderData + '<div class="room_in_list occupied_room" maPhong=' + dataResponse[i]["maPhong"] + '>' +
                        '<div class="room_info_status_field">' +
                        '<div class="room_name_in_list">Phòng ' + dataResponse[i]["maPhong"] + '</div>' +
                        '<div class="room_status_in_list">Đã nhận</div>' +
                        '</div>' +
                        '<div class="room_status_icon">' +
                        '<img class="status_icon">' +
                        '<p>Đang thuê</p>' +
                        '</div>' +
                        '<div class="room_detail_field">' +
                        '<div class="room_type_in_list">' + dataResponse[i]["tenLoaiPhong"] + '</div>' +
                        '<div class="room_number_people_in_list"><i class="fa fa-calendar"></i> ' + getDifferenceInDays(dataResponse[i]["thoiGianNhanPhong"], Date.now()) + ' ngày</div>' +
                        '</div>' +
                        '</div>';
                }
                else if (dataResponse[i]["trangThai"] == 2) {
                    renderData = renderData + '<div class="room_in_list fixed_room" maPhong=' + dataResponse[i]["maPhong"] + '>' +
                        '<div class="room_info_status_field" >' +
                        '<div class="room_name_in_list">Phòng ' + dataResponse[i]["maPhong"] + '</div>' +
                        '<div class="room_status_in_list">Sửa chữa</div>' +
                        '</div >' +
                        '<div class="room_status_icon">' +
                        '<img class="status_icon">' +
                        '<p>Sửa chữa</p>' +
                        '</div>' +
                        '<div class="room_detail_field">' +
                        '<div class="room_type_in_list">' + dataResponse[i]["tenLoaiPhong"] + '</div>' +
                        '<div class="room_number_people_in_list"><i class="fa fa-calendar"></i> 0 ngày</div>' +
                        '</div>' +
                        '</div>';
                }
                else if (dataResponse[i]["trangThai"] == 3) {
                    renderData = renderData + '<div class="room_in_list outed_room" maPhong=' + dataResponse[i]["maPhong"] + '>' +
                        '<div class="room_info_status_field">' +
                        '<div class="room_name_in_list">Phòng ' + dataResponse[i]["maPhong"] + '</div>' +
                        '<div class="room_status_in_list">Ra ngoài</div>' +
                        '</div>' +
                        '<div class="room_status_icon">' +
                        '<img class="status_icon">' +
                        '<p>Ra ngoài</p>' +
                        '</div>' +
                        '<div class="room_detail_field">' +
                        '<div class="room_type_in_list">' + dataResponse[i]["tenLoaiPhong"] + '</div>' +
                        '<div class="room_number_people_in_list"><i class="fa fa-calendar"></i> ' + getDifferenceInDays(dataResponse[i]["thoiGianNhanPhong"], Date.now()) + ' ngày</div>' +
                        '</div>' +
                        '</div>';
                }
                else if (dataResponse[i]["trangThai"] == 4) {
                    renderData = renderData + '<div class="room_in_list returned_room" maPhong=' + dataResponse[i]["maPhong"] + '>' +
                        '<div class="room_info_status_field">' +
                        '<div class="room_name_in_list">Phòng ' + dataResponse[i]["maPhong"] + '</div>' +
                        '<div class="room_status_in_list">Đã trả</div>' +
                        '</div>' +
                        '<div class="room_status_icon">' +
                        '<img class="status_icon">' +
                        '<p>Đang dọn dẹp</p>' +
                        '</div>' +
                        '<div class="room_detail_field">' +
                        '<div class="room_type_in_list">' + dataResponse[i]["tenLoaiPhong"] + '</div>' +
                        '<div class="room_number_people_in_list"><i class="fa fa-calendar"></i> 0 ngày</div>' +
                        '</div>' +
                        '</div>';
                }
            }
        }
        renderData = renderData + "</div></div>";
        document.querySelector("#sdp_main_working_window_id .data_window_sodophong").innerHTML = renderData;
        Add_Event_For_SDP();
    }
    else {
        toastMessage({ title: "Thất bại!", message: "Không có phòng nào đạt yêu cầu!", type: "fail" });
    }
}



$(document).ready(function (e) {
    Load_Data_For_SoDoPhong();
    $("#filter_sodophong_info").submit(function (e) {
        e.preventDefault(); // prevent actual form submit
        var form = $(this);
        var url = form.attr('action'); //get submit url [replace url here if desired]
        $.ajax({
            type: "POST",
            url: url,
            data: form.serialize(), // serializes form input
            success: function (data) {
                Render_Data_For_SDP_By_Filter(data);
            }
        });
    });
});

//if (typeof demo !== 'undefined') {
//    let demo = 1;
//}
let demo = 1;
$(document).ready(function (e) {
    // Context menu của sơ đồ phòng - begin
    document.body.addEventListener('click', function () {
        document.getElementById("context_menu_sdp_id").style.display = "none";
    })
    document.body.addEventListener('contextmenu', function () {
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
    function rightclick_occupiedroom() {
        let e = window.event;
        let context_menu_sdp = document.getElementById("context_menu_sdp_id");
        context_menu_sdp.querySelector("ul").innerHTML = "<li><p>Thanh toán</p></li><li id='thaydoikhacho_function_id'><p>Thay đổi khách ở</p></li><li><p>Hiện trạng</p></li><li id='bophong_function_id'><p>Bỏ phòng</p></li><li id='khachrangoai_function_id'><p>Khách ra ngoài</p></li>";
        context_menu_sdp.style.display = "block";
        context_menu_sdp.style.left = e.clientX + "px";
        context_menu_sdp.style.top = e.clientY + "px";
        $('#khachrangoai_function_id').click(function () {
            Convert_To_Outed_Room(e.target)
        });
        $('#bophong_function_id').click(function () {
            Convert_To_Available_Room(e.target)
        });
        $('#thaydoikhacho_function_id').click(function () {
            $('#receive_room_window_green_container_id').addClass("show").addClass("thaydoikhacho");
            document.querySelector('#info_customer_table_green_id tbody').innerHTML = ' ';
            let info_elements = document.querySelectorAll(".mini_grid input");
            info_elements.forEach(e => {
                e.value = '';
            })
            $('#header_close_receive_room_window_green_id').click(function () {
                $('#receive_room_window_green_container_id').removeClass("show").removeClass("thaydoikhacho");
            })
            $('#btn_cancel_receivephong_green').click(function () {
                $('#receive_room_window_green_container_id').removeClass("show").removeClass("thaydoikhacho");
            })
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
            $('#receive_room_window_green_container_id').addClass("show").addClass("datphong");
            document.querySelector('#info_customer_table_green_id tbody').innerHTML = ' ';
            let info_elements = document.querySelectorAll(".mini_grid input");
            info_elements.forEach(e => {
                e.value = '';
            })
            $('#header_close_receive_room_window_green_id').click(function () {
                $('#receive_room_window_green_container_id').removeClass("show").removeClass("datphong");
            })
            $('#btn_cancel_receivephong_green').click(function () {
                $('#receive_room_window_green_container_id').removeClass("show").removeClass("datphong");
            })
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
            Convert_To_Available_Room(e.target)
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
            Convert_To_Available_Room(e.target)
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
            Convert_To_Available_Room(e.target)
        });
    }
    // Context menu của sơ đồ phòng - end

    function Convert_To_Fixed_Room(e) {
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
    function Convert_To_Available_Room(e) {
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
    function Convert_To_Outed_Room(e) {
        if (e.classList.contains("occupied_room"))
            e.classList.remove("occupied_room");
        e.classList.add("outed_room");
        e.querySelector(".room_status_in_list").innerHTML = "Ra ngoài";
        e.querySelector(".room_status_icon p").innerHTML = "1h30p";
        e.addEventListener('contextmenu', function (ev) {
            ev.stopPropagation();
            ev.preventDefault();
            rightclick_outedroom();
            return false;
        }, false);
    }
    function Convert_To_Occupied_Room(e) {
        if (e.classList.contains("outed_room"))
            e.classList.remove("outed_room");
        e.classList.add("occupied_room");
        e.querySelector(".room_status_in_list").innerHTML = "Đã nhận";
        e.querySelector(".room_status_icon p").innerHTML = "Khách hàng";
        e.addEventListener('contextmenu', function (ev) {
            ev.stopPropagation();
            ev.preventDefault();
            rightclick_occupiedroom();
            return false;
        }, false);
    }
})
function Delete_Customer_Info(e) {
    e.parentElement.remove();
    $('.info_customer_table_green tbody tr td.order').text(function (i) {
        return i + 1;
    });
}
function Add_Info_Customer_Table() {
    let info_elements = document.querySelectorAll(".mini_grid input");
    let loaikhachhang = document.querySelector(".mini_grid select").value;
    let stt = document.querySelectorAll(".info_customer_table_green tbody tr").length + 1;
    $('<tr><td class = "order">' + stt + '</td>' +
        '<td>' + info_elements[1].value + '</td>' +
        '<td>' + info_elements[0].value + '</td>' +
        '<td>' + loaikhachhang + '</td>' +
        '<td>' + info_elements[2].value + '</td>' +
        '<td>' + info_elements[3].value + '</td>' +
        '<td>' + info_elements[4].value + '</td>' +
        '<td onclick="Delete_Customer_Info(this)"><i class="fa fa-trash" style="cursor: pointer; font-size: 1.2rem;"></i></td></tr>').insertAfter(document.querySelector(".info_customer_table_green tbody").lastChild)
    info_elements.forEach(e => {
        e.value = '';
    })
}



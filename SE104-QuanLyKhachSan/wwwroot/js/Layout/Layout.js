$(document).ready(function (e) {
    // dropdown option của tài khoản trên thanh header
    $('#dropdown_list_option_header_account_id').hide();
    $('#staff_info_field_id').click(function () {
        $('#dropdown_list_option_header_account_id').slideToggle(100);
    })
    $('#xemthongtin_option_header_account_id').click(function () {
        $('#staff_profile_popup_window_container_id').addClass("show");
        $('#dropdown_list_option_header_account_id').slideToggle(100);
        RenderInfoStaff();
    })
    $('#header_close_staff_profile_popup_window_id').click(function () {
        $('#staff_profile_popup_window_container_id').removeClass("show");
    })
    $('#btn_cancel_admin_in_staff_profile_popup_window_id').click(function () {
        $('#staff_profile_popup_window_container_id').removeClass("show");
    })

    document.getElementById("check_menu_burger_icon_id").addEventListener("click", () => {
        var t = document.getElementById("check_menu_burger_id").checked;
        if (t == true) {
            document.getElementsByClassName("menu_burger")[0].style.left = "-20.5%";
            document.getElementsByClassName("main_working_window")[0].style.marginLeft = "0";
        }
        else {
            document.getElementsByClassName("menu_burger")[0].style.left = "0";
            document.getElementsByClassName("main_working_window")[0].style.marginLeft = "20.5%";
        }
    })

    var options_burger_menu = document.querySelectorAll(".option_name_icon");
    options_burger_menu.forEach(function (option) {
        option.addEventListener("click", function (e) {
            var arrow_option_burger_menu = e.target.querySelector(".arrow_in_menu_burger");
            arrow_option_burger_menu.classList.toggle("rotate_90_icon_arrow");
        });
    });
    var selection_menu_burger = document.querySelectorAll(".selection_name_icon");
    selection_menu_burger.forEach(function (selection) {
        selection.addEventListener("click", function (e) {
            for (let s of selection_menu_burger) {
                if (s.classList.contains("chosen_selection_menu_burger")) {
                    s.classList.remove("chosen_selection_menu_burger");
                    break;
                }
            }
            e.target.classList.add("chosen_selection_menu_burger");
        })
    })
    let qltpSelectionField = document.querySelector("#selection_field_qltp_menu_burger_id");
    if (qltpSelectionField != null) {
        $('#selection_field_qltp_menu_burger_id').hide();

        $('#option_qltp_menu_burger_id').click(function () {
            $('#selection_field_qltp_menu_burger_id').slideToggle(200);
        })

        $('#sdp_selection_name_icon_id').click(function () {
            $.ajax({
                url: '/Home/SoDoPhong',
                success: function (data) {
                    $("#main_working_window_id").html(data);
                }
            });
        });

        $('#dshoadon_selection_name_icon_id').click(function () {
            $.ajax({
                url: '/HoaDon/DanhSachHoaDon',
                success: function (data, status) {
                    $('#main_working_window_id').html(data);
                }
            })
        });

        $('#tracuuthuephong_selection_name_icon_id').click(function () {
            $.ajax({
                url: '/PhieuThuePhong/DanhSachThuePhong',
                success: function (data, status) {
                    $('#main_working_window_id').html(data);
                }
            })
        });
    }
    let qlnvSelectionField = document.querySelector("#selection_field_qlnv_menu_burger_id");
    if (qlnvSelectionField != null) {
        $('#selection_field_qlnv_menu_burger_id').hide();
        $('#option_qlnv_menu_burger_id').click(function () {
            $('#selection_field_qlnv_menu_burger_id').slideToggle(200);
        })

        $('#tracuunhanvien_selection_name_icon_id').click(function () {
            $.ajax({
                url: '/Home/ListStaff',
                success: function (data, status) {
                    $('#main_working_window_id').html(data);
                    console.log(status);
                }
            })
        });
    }
    let qlphongSelectionField = document.querySelector("#selection_field_qlphong_menu_burger_id");
    if (qlphongSelectionField != null) {
        $('#selection_field_qlphong_menu_burger_id').hide();
        $('#option_qlphong_menu_burger_id').click(function () {
            $('#selection_field_qlphong_menu_burger_id').slideToggle(200);
        })

        $('#danhsachphong_selection_name_icon_id').click(function () {
            $.ajax({
                url: '/Home/ListRoom',
                success: function (data, status) {
                    $('#main_working_window_id').html(data);
                    console.log(status);
                }
            })
        });
    }
    let qldoanhthuSelectionField = document.querySelector("#selection_field_qldoanhthu_menu_burger_id");
    if (qldoanhthuSelectionField != null) {
        $('#selection_field_qldoanhthu_menu_burger_id').hide();
        $('#option_qldoanhthu_menu_burger_id').click(function () {
            $('#selection_field_qldoanhthu_menu_burger_id').slideToggle(200);
        })

        $('#doanhthutheoloaiphong_selection_name_icon_id').click(function () {
            $.ajax({
                url: '/Home/DTtheoLoaiPhong',
                success: function (data, status) {
                    $('#main_working_window_id').html(data);
                    console.log(status);
                }
            })
        });


        $('#doanhthutinhluong_selection_name_icon_id').click(function () {
            $.ajax({
                url: '/Home/DotLuong',
                success: function (data, status) {
                    $('#main_working_window_id').html(data);
                    console.log(status);
                }
            })
        });


        $('#doanhthutheochucvu_selection_name_icon_id').click(function () {
            var timeElapsed = Date.now();
            var today = new Date(timeElapsed);

            $.ajax({
                url: '/Home/LuongChucVu?Thang=' + today.toDateString(),
                success: function (data, status) {
                    $('#main_working_window_id').html(data);
                    console.log(status);
                }
            })
        });

        $('#doanhthubaocaotong_selection_name_icon_id').click(function () {
            var timeElapsed = Date.now();
            var today = new Date(timeElapsed);
            $.ajax({
                url: '/Home/ThongKeDoanhThu?ThangBaoCao=' + today.toDateString(),
                success: function (data, status) {
                    $('#main_working_window_id').html(data);
                    console.log(status);
                }
            })
        });
    }

    let qlhethongSelectionField = document.querySelector("#selection_field_qlhethong_menu_burger_id");
    if (qlhethongSelectionField != null) {
        $('#selection_field_qlhethong_menu_burger_id').hide();
        let qlhethongOption = document.querySelector("#option_qlhethong_menu_burger_id");

        if (qlhethongOption != null) {
            $('#option_qlhethong_menu_burger_id').click(function () {
                $('#selection_field_qlhethong_menu_burger_id').slideToggle(200);
            })
        }

        let thayDoiQuyDinhSelection = document.querySelector("#thaydoiquydinh_selection_name_icon_id");
        if (thayDoiQuyDinhSelection != null) {
            $('#thaydoiquydinh_selection_name_icon_id').click(function () {
                $.ajax({
                    url: '/Regulation/Index',
                    success: function (data, status) {
                        $('#main_working_window_id').html(data);
                    }
                })
            });
        }

        let phanquyenSelection = document.querySelector("#phanquyen_selection_name_icon_id");
        if (phanquyenSelection != null) {
            $('#phanquyen_selection_name_icon_id').click(function () {
                $.ajax({
                    url: '/Permission/Index',
                    success: function (data, status) {
                        $('#main_working_window_id').html(data);
                    }
                })
            });
        }


    }
})
/*    Toast Message Function: begin*/
function toastMessage({ title = '', message = '', type = 'success', duration = 3500 }) {
    let toastContainer = document.getElementById("toast_container");

    if (toastContainer) {
        let toastNotification = document.createElement('div');
        let icon = {
            success: "fa-check",
            fail: "fa-exclamation"
        }
        let delay = (duration / 1000).toFixed(2);
        let timeOutRemoveID = setTimeout(function () {
            toastContainer.removeChild(toastNotification);
        }, duration + 500)
        toastNotification.classList.add("toast_notification", "toast_" + type);
        toastNotification.style.animation = "slideInLeft cubic-bezier(0.68, -0.55, 0.265, 1.55) 0.5s, slideinRight linear 0.2s " + delay + "s forwards";
        toastNotification.innerHTML = '<div class="toast_content">' +
            '<i class="fas fa-solid ' + icon[type] + ' check"></i>' +
            '<div class="toast_message">' +
            '<span class="toast_text toast_text_1">' + title + '</span>' +
            '<span class="toast_text toast_text_2">' + message + '</span>' +
            '</div>' +
            '<i class="fa-solid fa-xmark close"></i>' +
            '<div class="toast_progress"></div>' +
            '</div>';
        toastNotification.querySelector(".close").onclick = function () {
            toastContainer.removeChild(toastNotification);
            clearTimeout(timeOutRemoveID);
        }
        toastNotification.querySelector(".toast_progress").style.setProperty("--toastBeforeAnimation", "progress " + delay + "s linear forwards");
        toastContainer.appendChild(toastNotification);
    }
}
/*    Toast Message Function: end*/

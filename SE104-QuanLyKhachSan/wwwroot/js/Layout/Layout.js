$(document).ready(function (e) {
    // dropdown option của tài khoản trên thanh header
    $('#dropdown_list_option_header_account_id').hide();
    $('#staff_info_field_id').click(function () {
        $('#dropdown_list_option_header_account_id').slideToggle(100);
    })
    $('#xemthongtin_option_header_account_id').click(function () {
        $('#staff_profile_popup_window_container_id').addClass("show");
        $('#dropdown_list_option_header_account_id').slideToggle(100);
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

    $('#selection_field_qltp_menu_burger_id').hide();
    $('#selection_field_qlnv_menu_burger_id').hide();
    $('#selection_field_qlphong_menu_burger_id').hide();
    $('#selection_field_qldoanhthu_menu_burger_id').hide();
    $('#selection_field_qlhethong_menu_burger_id').hide();

    $('#option_qltp_menu_burger_id').click(function () {
        $('#selection_field_qltp_menu_burger_id').slideToggle(200);
    })
    $('#option_qlnv_menu_burger_id').click(function () {
        $('#selection_field_qlnv_menu_burger_id').slideToggle(200);
    })
    $('#option_qlphong_menu_burger_id').click(function () {
        $('#selection_field_qlphong_menu_burger_id').slideToggle(200);
    })
    $('#option_qldoanhthu_menu_burger_id').click(function () {
        $('#selection_field_qldoanhthu_menu_burger_id').slideToggle(200);
    })
    $('#option_qlhethong_menu_burger_id').click(function () {
        $('#selection_field_qlhethong_menu_burger_id').slideToggle(200);
    })


    $('#sdp_selection_name_icon_id').click(function () {
        $.ajax({
            url: '/Home/SoDoPhong',
            success: function (data) {
                $("#main_working_window_id").html(data);
            }
        })
    });

    $('#thaydoiquydinh_selection_name_icon_id').click(function () {
        $.ajax({
            url: '/Regulation/Index',
            success: function (data) {
                $("#main_working_window_id").html(data);
            }
        })
    });
    $('#phanquyen_selection_name_icon_id').click(function () {
        $.ajax({
            url: '/Permission/Index',
            success: function (data) {
                $("#main_working_window_id").html(data);
            }
        })
    });
})
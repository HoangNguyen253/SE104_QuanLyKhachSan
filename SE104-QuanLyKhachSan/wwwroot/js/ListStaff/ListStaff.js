
$(document).ready(function (e) {
    document.getElementById("tracuunv").addEventListener("click", () => {
        document.getElementById("staff_profile_popup_add_window_container_id").classList.add('show');
    })
    document.getElementById("header_close_staff_profile_popup_add_window_id").addEventListener("click", () => {
        document.getElementById("staff_profile_popup_add_window_container_id").classList.remove('show');
    })
    document.getElementById("close_popup_add_window_staff_button").addEventListener("click", () => {
        document.getElementById("staff_profile_popup_add_window_container_id").classList.remove('show');
    })

   /* $('#xemthongtin_option_header_account_id_').click(function () {
        $('#staff_profile_popup_window_container_id_').addClass("show");
        $('#dropdown_list_option_header_account_id_').slideToggle(100);
    })
    $('#header_close_staff_profile_popup_window_id_').click(function () {
        $('#staff_profile_popup_window_container_id_').removeClass("show");
    })
    $('#btn_cancel_admin_in_staff_profile_popup_window_id_').click(function () {
        $('#staff_profile_popup_window_container_id_').removeClass("show");
    })*/

    document.getElementById("add_info_popup_add_window_staff_button").addEventListener("click", () => {

     
        let maNhanVien = document.getElementById("MaNhanVien").value;
        let matKhau = document.getElementById("MatKhau").value;
        let CCCD = document.getElementById("CCCD").value;
        let hoTen = document.getElementById("HoTen").value;
        let gioiTinh = document.getElementById("GioiTinh").value;
        let ngaySinh = document.getElementById("NgaySinh").value;
        let email = document.getElementById("Email").value;
        let soDienThoai = document.getElementById("SoDienThoai").value;
        let ngayVaoLam = document.getElementById("NgayVaoLam").value;
        let maChucVu = document.getElementById("MaChucVu").value;
        let hinhAnh = document.getElementById("HinhAnh").value;
        hinhAnh = "abcd";
        let luong = document.getElementById("Luong").value;

        let form = new FormData();
        let xhr_add_nhanvien = new XMLHttpRequest();
        let url_add_nhanvien = "/Home/addStaff";

        form.append("MaNhanVien", maNhanVien);
        form.append("MatKhau", matKhau);
        form.append("CCCD", CCCD);
        form.append("HoTen", hoTen);
        form.append("GioiTinh", gioiTinh);
        form.append("NgaySinh", ngaySinh);
        form.append("Email", email);
        form.append("SoDienThoai", soDienThoai);
        form.append("NgayVaoLam", ngayVaoLam);
        form.append("MaChucVu", maChucVu);
        form.append("HinhAnh", hinhAnh);
        form.append("Luong", luong);

        xhr_add_nhanvien.open("POST", url_add_nhanvien, true);
        xhr_add_nhanvien.send(form);
    })

    $(document).ready(function () {
        $('#TimKiemStaff').on('keyup', function (event) {
            event.preventDefault();
            /* Act on the event */
            var tukhoa = $(this).val().toLowerCase();
            var tukhoa2 = $('#TenChucVu_').val().toLowerCase();
            $('#myTableStaff tbody tr').filter(function () {
                $(this).toggle( $(this).text().toLowerCase().indexOf(tukhoa2) > -1 && $(this).text().toLowerCase().indexOf(tukhoa) > -1 );
            });
        });
    });

    $(document).ready(function () {
        $('#TenChucVu_').on('change', function (event) {
            event.preventDefault();
            /* Act on the event */
            
            var tukhoa2 = $('#TimKiemStaff').val().toLowerCase();
            var tukhoa = $(this).val().toLowerCase();
            $('#myTableStaff tbody tr').filter(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(tukhoa2) > -1 && $(this).text().toLowerCase().indexOf(tukhoa) > -1);
                });
          
        });
    });
})




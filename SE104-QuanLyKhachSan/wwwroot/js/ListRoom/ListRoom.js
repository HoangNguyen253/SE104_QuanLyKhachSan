$(document).ready(function (e) {

    $('#tracuuphong').click(function () {
        $('#room_profile_popup_window_container_id').addClass("show");
    })
    document.getElementById("header_close_room_profile_popup_window_id").addEventListener("click", () => {
        document.getElementById("room_profile_popup_window_container_id").classList.remove('show');
    })
    document.getElementById("close_popup_window_room_button").addEventListener("click", () => {
        document.getElementById("room_profile_popup_window_container_id").classList.remove('show');
    })
    document.getElementById("add_info_popup_window_room_button").addEventListener("click", () => {
        let maPhong = document.getElementById("MaPhong").value;
        let tang = document.getElementById("Tang").value;
        let ghiChu = document.getElementById("GhiChu").value;
        let trangThai = document.getElementById("TrangThai").value;
        let soPhong = document.getElementById("SoPhong").value;
        let loaiPhong = document.getElementById("LoaiPhong").value;

        let form = new FormData();
        let xhr_add_phong = new XMLHttpRequest();
        let url_add_phong = "/Home/addRoom";

        form.append("MaPhong", maPhong);
        form.append("Tang", tang);
        form.append("GhiChu", ghiChu);
        form.append("TrangThai", trangThai);
        form.append("SoPhong", soPhong);
        form.append("LoaiPhong", loaiPhong);

        xhr_add_phong.open("POST", url_add_phong, true);
        xhr_add_phong.send(form);
    })
    $(document).ready(function () {
        $('#TimKiem').on('keyup', function (event) {
            event.preventDefault();
            /* Act on the event */
            var tukhoa = $(this).val().toLowerCase();
            var tukhoa1 = $('#TrangThai_').val().toLowerCase();
            var tukhoa2 = $('#TenLoaiPhong_').val().toLowerCase();
            $('#myTable tbody tr').filter(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(tukhoa2) > -1
                    && $(this).text().toLowerCase().indexOf(tukhoa) > -1
                    && $(this).text().toLowerCase().indexOf(tukhoa1) > -1);
            });
        });
    });

    $(document).ready(function () {
        $('#TrangThai_').on('change', function (event) {
            event.preventDefault();
            /* Act on the event */
            var tukhoa = $(this).val().toLowerCase();
            var tukhoa1 = $('#TimKiem').val().toLowerCase();
            var tukhoa2 = $('#TenLoaiPhong_').val().toLowerCase();
            $('#myTable tbody tr').filter(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(tukhoa2) > -1
                              && $(this).text().toLowerCase().indexOf(tukhoa) > -1
                              && $(this).text().toLowerCase().indexOf(tukhoa1) > -1);
            });
        });
    });

    $(document).ready(function () {
        $('#TenLoaiPhong_').on('change', function (event) {
            event.preventDefault();
            /* Act on the event */

            var tukhoa = $(this).val().toLowerCase();
            var tukhoa1 = $('#TimKiem').val().toLowerCase();
            var tukhoa2 = $('#TrangThai_').val().toLowerCase();
            $('#myTable tbody tr').filter(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(tukhoa2) > -1
                    && $(this).text().toLowerCase().indexOf(tukhoa) > -1
                    && $(this).text().toLowerCase().indexOf(tukhoa1) > -1);
            });

        });
    });

   
      
})




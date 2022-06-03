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

var edit = document.querySelectorAll('#del_room');

for (let i = 0; i < edit.length; i++) {
    edit[i].addEventListener('click', () => {
        var answer = window.confirm("Bạn có muốn xóa");
        if (answer) {

            var xhr_login = new XMLHttpRequest();
            let url_login = "https://localhost:5001/Home/RemoveRoom?MaPhong=" + edit[i].parentElement.id;
            xhr_login.open("GET", url_login, true);
            xhr_login.timeout = 20000;
            xhr_login.onreadystatechange = function () {

                if (xhr_login.readyState == 4 && xhr_login.status == 200) {

                    var result = xhr_login.response;
                    if (result) {
                        toastMessage({ title: "Thành công!", message: "Xóa thành công", type: "success" });
                        edit[i].parentElement.remove();
                    }
                    else
                        toastMessage({ title: "Thất bại!", message: "Xóa thất bại", type: "fail" });
                }

            }
            xhr_login.send();
        }
    });
}

var listMaPhong = document.querySelectorAll('#MaPhong').value;

function validateForm() {
   
    
        var maPhong= document.getElementById("MaPhong").value;
        var tang = document.getElementById("Tang").value;
        var soPhong = document.getElementById("SoPhong").value;


        for (let i = 0; i < listMaPhong.length; i++) {
            if (maPhong == listMaPhong[i]) {
                toastMessage({ title: 'Lỗi Thêm Phòng', message: 'Đã tồn tại mã phòng', type: 'fail', duration: 3500 });
                return false;
            }

         }
    if (tang >= 100 || tang <= 0) {
        toastMessage({ title: 'Lỗi Thêm Phòng', message: 'Tầng nằm trong khoảng (0;100)', type: 'fail', duration: 3500 });
        return false;
    }
    if (soPhong <= 0) {
        toastMessage({ title: 'Lỗi Thêm Phòng', message: 'Số phòng phải dương', type: 'fail', duration: 3500 });
        return false;
    }
}




//----1. loai phong: end





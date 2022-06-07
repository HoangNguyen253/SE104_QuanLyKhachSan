﻿$(document).ready(function (e) {

    $('#tracuuphong').click(function () {
        $('#room_profile_popup_window_container_id').addClass("show");
    })
    document.getElementById("header_close_room_profile_popup_window_id").addEventListener("click", () => {
        document.getElementById("room_profile_popup_window_container_id").classList.remove('show');
    })
    document.getElementById("close_popup_window_room_button").addEventListener("click", () => {
        document.getElementById("room_profile_popup_window_container_id").classList.remove('show');
    })

    document.getElementById("header_close_room_profile_popup_window_id").addEventListener("click", () => {
        document.getElementById("room_profile_popup_window_container_id").classList.remove('show');
        if (document.getElementById("room_profile_popup_window_container_id").classList.contains('suaphong')) {
            document.getElementById("room_profile_popup_window_container_id").classList.remove('suaphong');

            document.getElementById('SoPhong').disabled = false;
            document.getElementById('Tang').disabled = false;

        }
    })
    document.getElementById("close_popup_window_room_button").addEventListener("click", () => {
        document.getElementById("room_profile_popup_window_container_id").classList.remove('show');
        if (document.getElementById("room_profile_popup_window_container_id").classList.contains('suaphong')) {
            document.getElementById("room_profile_popup_window_container_id").classList.remove('suaphong');

            document.getElementById('SoPhong').disabled = false;
            document.getElementById('Tang').disabled = false;

        }
    })
    document.getElementById("add_info_popup_window_room_button").addEventListener("click", () => {
        if (validateFormRoom() == true) {
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
            form.append("MaLoaiPhong", loaiPhong);

            xhr_add_phong.open("POST", url_add_phong, true);
            xhr_add_phong.send(form);
            toastMessage({ title: 'Thành công', message: 'Thêm phòng thành công', type: 'success', duration: 3500 });

        }
        else {
            toastMessage({ title: 'Lỗi Thêm Phòng', message: 'Thêm phòng thất bại', type: 'fail', duration: 3500 });
        }
  
    })
    document.getElementById("tracuuphong").addEventListener("click", () => {
        document.getElementById("MaPhong").value = "";

        document.getElementById("Tang").value = "";
        document.getElementById("SoPhong").value = "";

        document.getElementById("GhiChu").value = "";

        document.getElementById('SoPhong').disabled = false;
        document.getElementById('Tang').disabled = false;


    })
    $(document).ready(function () {
        $('#SoPhong').on('change', function (event) {
            event.preventDefault();
            /* Act on the event */
            let tang = "";
            let soPhong = "";

            tang = document.getElementById("Tang").value.toString();
            soPhong = document.getElementById("SoPhong").value.toString();
            

            if (document.getElementById("SoPhong").value < 10)
                if (document.getElementById("SoPhong").value == 0)
                    soPhong = "00";
                else
                    soPhong = 0 + soPhong;

            if (document.getElementById("Tang").value < 10)
                if (document.getElementById("Tang").value == 0)
                    tang = "00";
         

            let maPhong = document.querySelector('#MaPhong')
            maPhong.value = tang + soPhong;
            
        });
    });

    $(document).ready(function () {
        $('#Tang').on('change', function (event) {
            event.preventDefault();
            /* Act on the event */
            let tang = document.getElementById("Tang").value.toString();
            let soPhong = document.getElementById("SoPhong").value.toString();

            if (document.getElementById("SoPhong").value < 10)
                if (document.getElementById("SoPhong").value == 0)
                    soPhong = "00";
                else
                    soPhong = 0 + soPhong;
         

            if (document.getElementById("Tang").value < 10)
                if (document.getElementById("Tang").value == 0)
                    tang = "00";
               

            let maPhong = document.querySelector('#MaPhong')
            maPhong.value = tang + soPhong;
        });
    });

    $(document).ready(function () {
        $('#SoPhong').on('keyup', function (event) {
            event.preventDefault();
            /* Act on the event */
            let tang = "";
            let soPhong = "";

            tang = document.getElementById("Tang").value.toString();
            soPhong = document.getElementById("SoPhong").value.toString();


            if (document.getElementById("SoPhong").value < 10)
                if (document.getElementById("SoPhong").value == 0)
                    soPhong = "00";
                else
                    soPhong = 0 + soPhong;

            if (document.getElementById("Tang").value < 10)
                if (document.getElementById("Tang").value == 0)
                    tang = "00";
             




            let maPhong = document.querySelector('#MaPhong')
            maPhong.value = tang + soPhong;

        });
    });

    $(document).ready(function () {
        $('#Tang').on('keyup', function (event) {
            event.preventDefault();
            /* Act on the event */
            let tang = document.getElementById("Tang").value.toString();
            let soPhong = document.getElementById("SoPhong").value.toString();

            if (document.getElementById("SoPhong").value < 10)
                if (document.getElementById("SoPhong").value == 0)
                    soPhong = "00";
                else
                    soPhong = 0 + soPhong;

            if (document.getElementById("Tang").value < 10)
                if (document.getElementById("Tang").value == 0)
                    tang = "00";
              

            let maPhong = document.querySelector('#MaPhong')
            maPhong.value = tang + soPhong;
        });
    });



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
                        toastMessage({ title: "Thành công!", message: "Xóa phòng thành công", type: "success" });
                        edit[i].parentElement.remove();
                    }
                    else
                        toastMessage({ title: "Thất bại!", message: "Xóa phòng thất bại", type: "fail" });
                }

            }
            xhr_login.send();
        }
    });
}


function validateFormRoom() {
   
   
        var tang = document.getElementById("Tang").value;
        var soPhong = document.getElementById("SoPhong").value;
    if (tang == "" || soPhong == "") {
        toastMessage({ title: 'Lỗi Thêm Phòng', message: 'Không để trống phòng/tầng', type: 'fail', duration: 3500 });
        return false;
    }

    if (tang >= 100 || tang <= 0) {
        toastMessage({ title: 'Lỗi Thêm Phòng', message: 'Tầng nằm trong khoảng (0;100)', type: 'fail', duration: 3500 });
        return false;
    }
    if (soPhong <= 0 || soPhong >= 100) {
        toastMessage({ title: 'Lỗi Thêm Phòng', message: 'Số phòng nằm trong khoảng (0;100)', type: 'fail', duration: 3500 });
        return false;
    }
    return true;
 
}

function viewInfoRoom(maPhong) {


    let xhr_edit_room = new XMLHttpRequest();
    let url_edit_room = "/Home/GetChosenRoom/?MaPhong=" + maPhong;


    xhr_edit_room.open("POST", url_edit_room, true);
    xhr_edit_room.send();

    xhr_edit_room.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            let dataPhong = JSON.parse(this.response);

            document.getElementById('room_profile_popup_window_container_id').classList.add("show");
            document.getElementById('room_profile_popup_window_container_id').classList.add("suaphong");
            document.getElementById('SoPhong').disabled = true;
            document.getElementById('Tang').disabled = true;
           


            let inputDataPhong = document.querySelectorAll('.data_info_room_profile_popup_window input');

            let inputMaPhong = document.querySelectorAll('.code_room_profile_popup_window input');

            inputMaPhong[0].value = dataPhong["maPhong"];

            inputDataPhong[0].value = dataPhong["tang"];
            inputDataPhong[1].value = dataPhong["soPhong"];
            inputDataPhong[2].value = dataPhong["ghiChu"];


            let selecttDataPhong = document.querySelectorAll('.data_info_room_profile_popup_window select');
            selecttDataPhong[0].value = dataPhong["trangThai"];
            selecttDataPhong[1].value = dataPhong["maLoaiPhong"];
        }

    }

   

}



















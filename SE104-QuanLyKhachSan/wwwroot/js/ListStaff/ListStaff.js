﻿
$(document).ready(function (e) {
    document.getElementById("tracuunv").addEventListener("click", () => {
        document.getElementById("staff_profile_popup_add_window_container_id").classList.add('show');
       
    })
    document.getElementById("header_close_staff_profile_popup_add_window_id").addEventListener("click", () => {
        document.getElementById("staff_profile_popup_add_window_container_id").classList.remove('show');
        if (document.getElementById("staff_profile_popup_add_window_container_id").classList.contains('suanhanvien')) {
            document.getElementById("staff_profile_popup_add_window_container_id").classList.remove('suanhanvien');
            document.getElementById('MaNhanVien').disabled = false;

        }
    })
    document.getElementById("close_popup_add_window_staff_button").addEventListener("click", () => {
        document.getElementById("staff_profile_popup_add_window_container_id").classList.remove('show');
        if (document.getElementById("staff_profile_popup_add_window_container_id").classList.contains('suanhanvien'))
        {
            document.getElementById("staff_profile_popup_add_window_container_id").classList.remove('suanhanvien');
            document.getElementById('MaNhanVien').disabled = false;
            document.getElementById('MatKhau').hidden;
       
            }
    })

    document.getElementById("tracuunv").addEventListener("click", () => {
        document.getElementById("MaNhanVien").value = "";
        document.getElementById("MatKhau").value = "";
        document.getElementById("CCCD").value = "";
        document.getElementById("HoTen").value = "";
        document.getElementById("NgaySinh").value = "";
        document.getElementById("Email").value = "";
        document.getElementById("SoDienThoai").value = "";
        document.getElementById("NgayVaoLam").value = "";

  
        document.getElementById("Luong").value="";

    })

    document.getElementById("add_info_popup_add_window_staff_button").addEventListener("click", () => {
        if (validateForm() == true) {

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
      
            form.append("Luong", luong);

            xhr_add_nhanvien.open("POST", url_add_nhanvien, true);
            xhr_add_nhanvien.send(form);

            xhr_add_nhanvien.onreadystatechange = function () {
                if (this.readyState == 4 && this.status == 200) {
                    let status = this.response;
                    if (status == "success")
                    {
                        toastMessage({ title: 'Thành công', message: 'Thêm thành công. Đã gửi mật khẩu về mail.', type: 'success', duration: 3500 });
                        $.ajax({
                            url: '/Home/ListStaff',
                            success: function (data, status) {
                                $('#main_working_window_id').html(data);
                                console.log(status);
                            }
                        })
                    }
                  
                    else
                    {
                        toastMessage({ title: 'Thất bại', message: 'Xảy ra lỗi trùng dữ liệu (email, số điện thoại, cmnd/cccd không được trùng)', type: 'fail', duration: 3500 });
                    }
                }

            }
        }
        else {
    
        }
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




function validateForm() {
   


    var email = document.getElementById("Email").value;

    var maNhanVien = document.getElementById("MaNhanVien").value;
    var matKhau = document.getElementById("MatKhau").value;
    var CCCD = document.getElementById("CCCD").value;
    var hoTen = document.getElementById("HoTen").value;
    var ngaySinh = document.getElementById("NgaySinh").value;
    var soDienThoai = document.getElementById("SoDienThoai").value;
    var ngayVaoLam = document.getElementById("NgayVaoLam").value;
    var luong = document.getElementById("Luong").value;



    if ( hoTen == "" || ngaySinh == "" || ngayVaoLam == "" || luong == "" || CCCD=="" || email=="") {
        toastMessage({ title: 'Lỗi Nhân Viên', message: 'Điền đủ vào các trường bắt buộc', type: 'fail', duration: 3500 });
        return false;
    }

    if (ngaySinh > ngayVaoLam) {
        toastMessage({ title: 'Lỗi Nhân Viên', message: 'Ngày sinh phải nhỏ hơn ngày vào làm', type: 'fail', duration: 3500 });
        return false;
    }

    if (soDienThoai != "") {
        if (soDienThoai.length < 10 || soDienThoai.length > 11 || soDienThoai<0) {
            toastMessage({ title: 'Lỗi Nhân Viên', message: 'Số điện thoại không hợp lệ', type: 'fail', duration: 3500 });
            return false;
        }
    }
    if (luong < 0) {
        toastMessage({ title: 'Lỗi Nhân Viên', message: 'Lương không hợp lệ', type: 'fail', duration: 3500 });
        return false;
    }
    if (CCCD != "") {
        if (CCCD < 0  || (CCCD.length != 12 && CCCD.length !=9)) {
            toastMessage({ title: 'Lỗi Nhân Viên', message: 'CCCD/CMND không hợp lệ (CCCD: đủ 12 chữ số, CMND: đủ 9 chữ số)', type: 'fail', duration: 3500 });
            return false;
        }
    }

    return true;
 
}
function viewInfoStaff(maNhanVien) {
   
   
    let xhr_add_nhanvien = new XMLHttpRequest();
    let url_add_nhanvien = "/Home/GetChosenStaff?MaNhanVien="+ maNhanVien;

    
    xhr_add_nhanvien.open("POST", url_add_nhanvien, true);
    xhr_add_nhanvien.send();

    xhr_add_nhanvien.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            let dataNhanVien = JSON.parse(this.response);

            document.getElementById('staff_profile_popup_add_window_container_id').classList.add("show");
            document.getElementById('staff_profile_popup_add_window_container_id').classList.add("suanhanvien");
            document.getElementById('MaNhanVien').disabled = true;


            let inputDataNhanVien = document.querySelectorAll('.data_info_staff_profile_popup_add_window input');

            inputDataNhanVien[0].value = dataNhanVien["hoTen"];
            inputDataNhanVien[1].value = dataNhanVien["maNhanVien"];
            inputDataNhanVien[2].value = dataNhanVien["matKhau"];
            inputDataNhanVien[3].value = dataNhanVien["cccd"];
            inputDataNhanVien[4].value = dataNhanVien["soDienThoai"];
            inputDataNhanVien[5].value = dataNhanVien["ngaySinh"].substr(0, 10);
            inputDataNhanVien[6].value = dataNhanVien["email"];
     
            inputDataNhanVien[7].value = dataNhanVien["ngayVaoLam"].substr(0, 10);
            inputDataNhanVien[8].value = dataNhanVien["luong"];

            let selecttDataNhanVien = document.querySelectorAll('.data_info_staff_profile_popup_add_window select');
            selecttDataNhanVien[0].value = dataNhanVien["gioiTinh"];
            selecttDataNhanVien[1].value = dataNhanVien["maChucVu"];

          
        }
       
        
    }
}

function deleteInfoStaff(maNhanVien) {
    console.log(maNhanVien);
    var answer = window.confirm("Bạn có muốn xóa");
    if (answer) {
        let xhr_add_nhanvien = new XMLHttpRequest();
        let url_add_nhanvien = "/Home/DeleteStaff?MaNV=" + maNhanVien;


        xhr_add_nhanvien.open("POST", url_add_nhanvien, true);
        xhr_add_nhanvien.send();

        xhr_add_nhanvien.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                let status = this.response;
                if (status == "success") {
                    toastMessage({ title: 'Success', message: 'Xóa thành công', type: 'success', duration: 3500 });
                    $.ajax({
                        url: '/Home/ListStaff',
                        success: function (data, status) {
                            $('#main_working_window_id').html(data);
                            console.log(status);
                        }
                    })
                }
                else if (status == "fired") {
                    toastMessage({ title: 'Thành công', message: 'Sa thải thành công', type: 'success', duration: 3500 });
                    $.ajax({
                        url: '/Home/ListStaff',
                        success: function (data, status) {
                            $('#main_working_window_id').html(data);
                            console.log(status);
                        }
                    })
                }
                else if (status == "zxc") {
                    console.log(123);
                }
                else {
                    toastMessage({ title: 'Thất bại', message: 'Xóa thất bại', type: 'fail', duration: 3500 });
                }
            }

        }

    }
  
}

//test
var listbc = document.querySelectorAll('#del_staff');

for (let i = 0; i < listbc.length; i++) {
    listbc[i].addEventListener('click', () => {
        let mabc = listbc[i].parentElement.id;
        var answer = window.confirm("Bạn có muốn xóa");
        if (answer) {
            let xhr_add_nhanvien = new XMLHttpRequest();
            let url_add_nhanvien = "/Home/DeleteStaff?MaNV=" + mabc;


            xhr_add_nhanvien.open("POST", url_add_nhanvien, true);
            xhr_add_nhanvien.send();

            xhr_add_nhanvien.onreadystatechange = function () {
                if (this.readyState == 4 && this.status == 200) {
                    let status = this.response;
                    if (status == "success") {
                        toastMessage({ title: 'Thành công', message: 'Xóa thành công', type: 'success', duration: 3500 });
                        $.ajax({
                            url: '/Home/ListStaff',
                            success: function (data, status) {
                                $('#main_working_window_id').html(data);
                                console.log(status);
                            }
                        })
                    }
                    else if (status == "fired") {
                        toastMessage({ title: 'Thất bại', message: 'Không thể xóa nhân viên. Cập nhật chức vụ nhân viên thành "Đã sa thải".', type: 'fail', duration: 3500 });
                        $.ajax({
                            url: '/Home/ListStaff',
                            success: function (data, status) {
                                $('#main_working_window_id').html(data);
                                console.log(status);
                            }
                        })
                    }
                    else if (status == "zxc") {
                        console.log(123);
                    }
                    else {
                        toastMessage({ title: 'Thất bại', message: 'Xóa thất bại', type: 'fail', duration: 3500 });
                    }
                }

            }

        }
    });
}
function Reset_Password() {
    let email = document.getElementById('Email').value;
    loadingElement.show();



    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Home/Reset_Password?email=" + email;
    xhr.open("GET", url, true);
    xhr.timeout = 20000;
    xhr.onreadystatechange = function () {



        loadingElement.hide();



        if (xhr.readyState == 4 && xhr.status == 200) {

            let result = xhr.responseText;
            if (result == "success") {
                toastMessage({ title: 'Thành công', message: 'Đã gửi mật khẩu về mail', type: 'success', duration: 3500 });
            }
            else {
                toastMessage({ title: 'Thất bại', message: result, type: 'fail', duration: 3500 });
            }
        }

    }
    xhr.send();

}








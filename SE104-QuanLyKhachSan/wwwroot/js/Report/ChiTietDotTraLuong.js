var back_btn = document.getElementById("back_btn_id");
back_btn.addEventListener('click', () => {
    $.ajax({
        url: '/Home/DotLuong',
        success: function (data, status) {
            $('#main_working_window_id').html(data);
            console.log(status);
        }
    })
});

var print_btn = document.getElementById("print_btn_id");
print_btn.addEventListener('click', () => {
    $("#table_luong_id").table2excel({
        exclude: ".excludeThisClass",
        name: "Worksheet Name",
        filename: "BangLuong.xls", 
        preserveColors: true 
    });
});
var edit_btn = document.querySelectorAll('#Edit_btn');
var madottraluong;
var manhanvien;
for (let i = 0; i < edit_btn.length; i++) {
    edit_btn[i].addEventListener('click', () => {
        document.getElementById("staff_info_container_id").classList.add('show');
        
        madottraluong = edit_btn[i].parentElement.id;
        manhanvien = edit_btn[i].parentElement.parentElement.children[1].innerHTML;
        var hoten = edit_btn[i].parentElement.parentElement.children[2].innerHTML;
        var thuong = ThuongBanDau = edit_btn[i].parentElement.parentElement.children[5].innerHTML;
        var phat = PhatBanDau = edit_btn[i].parentElement.parentElement.children[6].innerHTML;
        var sotien = TongLuongBanDau = edit_btn[i].parentElement.parentElement.children[7].innerHTML;
        var ghichu = edit_btn[i].parentElement.parentElement.children[8].innerHTML;

        let workStaffInput = $(".Info_Luong_container input");
        workStaffInput[0].value = parseInt(thuong);
        workStaffInput[1].value = parseInt(phat);
        workStaffInput[2].value = parseInt(sotien);

        $("#manhanvien_id").html(manhanvien);
        $("#tennhanvien_id").html(hoten);

        let workStaffText = $(".Info_Luong_container textarea");
        workStaffText[0].value = ghichu;
    });
}

document.getElementById("Update_btn").addEventListener('click', () => {
    if ($('#staff_info_container_id').hasClass("show")) {
        let form_DataNhanVien = new FormData();
        let workStaffInput = $(".Info_Luong_container input");
        let workStaffText = $(".Info_Luong_container textarea");


        form_DataNhanVien.append("MaDotTraLuong", madottraluong);
        form_DataNhanVien.append("MaNhanVien", manhanvien);
        form_DataNhanVien.append("Thuong", workStaffInput[0].value);
        form_DataNhanVien.append("Phat", workStaffInput[1].value);
        form_DataNhanVien.append("GhiChu", workStaffText[0].value);
        form_DataNhanVien.append("SoTien", workStaffInput[2].value);

        let xhr_Update_Data_Staff = new XMLHttpRequest();
        let url_Update_Data_Staff = "https://localhost:5001/Home/UpdateLuongStaff";
        xhr_Update_Data_Staff.open("POST", url_Update_Data_Staff, true);
        xhr_Update_Data_Staff.timeout = 5000;
        xhr_Update_Data_Staff.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                let result = this.responseText;
                if (result => 1) {
                    toastMessage({ title: "Thành công!", message: "Cập nhật thành công", type: "success" });
                    var madottraluong = edit_btn[0].parentElement.id;
                    var t = document.getElementById("ThangBaoCao_id").innerHTML;
                    $.ajax({
                        url: '/Home/ChiTietDotLuong?MaBCDL=' + madottraluong + "&ThangBaoCao=" + t,
                        success: function (data, status) {
                            $('#main_working_window_id').html(data);
                            console.log(status);
                        }
                    });
                }        
                else
                    toastMessage({ title: "Thất bại!", message: "Cập nhật thất bại", type: "fail" });
            }
        }
        document.getElementById("staff_info_container_id").classList.remove('show');
        xhr_Update_Data_Staff.send(form_DataNhanVien);
    }
});

$(document).ready(function () {
    $('#search_input_id').on('keyup', function (event) {
        event.preventDefault();
        var tukhoa = $(this).val().toLowerCase();
        $('#table_luong_id tbody tr').filter(function () {
            $(this).toggle( $(this).text().toLowerCase().indexOf(tukhoa) > -1 );
        });
    });
});


let ThuongBanDau = 0;
let PhatBanDau = 0;
let TongLuongBanDau = 0;
let ThuongFinal = 0;
let PhatFinal = 0;
let TongLuongFinal = 0;

document.getElementById("ThuongInput_id").addEventListener('focusout', () => {
    ThuongFinal = parseInt(document.getElementById("ThuongInput_id").value);
    if (!ThuongFinal)
        document.getElementById("ThuongInput_id").value = 0;
});

document.getElementById("PhatInput_id").addEventListener('focusout', () => {
    PhatFinal = parseInt(document.getElementById("PhatInput_id").value);
    if (!PhatFinal)
        document.getElementById("PhatInput_id").value = 0;
});

document.getElementById("ThuongInput_id").addEventListener('focus', () => {
    ThuongBanDau = parseInt(document.getElementById("ThuongInput_id").value);
});

document.getElementById("PhatInput_id").addEventListener('focus', () => {
    PhatBanDau = parseInt(document.getElementById("PhatInput_id").value);
});


function CalcTienLuong(valueInput, method) {
    switch (method) {
        case 'thuong':
            ThuongFinal = valueInput;

            /*nếu value null thì = 0*/
            if (!ThuongFinal)
                ThuongFinal = 0;
            TongLuongBanDau = parseInt(document.getElementById("TongLuong_input_id").value);
            document.getElementById("TongLuong_input_id").value = TongLuongBanDau - parseInt(ThuongBanDau) + parseInt(ThuongFinal);
            if (ThuongFinal != 0)
                ThuongBanDau = parseInt(document.getElementById("ThuongInput_id").value);
            else
                ThuongBanDau = 0;
            break;
        case 'phat':
            PhatFinal = valueInput;
            if (!PhatFinal)
                PhatFinal = 0;
            TongLuongBanDau = parseInt(document.getElementById("TongLuong_input_id").value);
            document.getElementById("TongLuong_input_id").value = TongLuongBanDau + parseInt(PhatBanDau) - parseInt(PhatFinal);
            if (PhatFinal != 0)
                PhatBanDau = parseInt(document.getElementById("PhatInput_id").value);
            else
                PhatBanDau = 0;
            break;
    }
}





var list_dotluong = document.querySelectorAll('#list_BaoCaoDoanhThu_id');

for (let i = 0; i < list_dotluong.length; i++) {
    list_dotluong[i].addEventListener('click', () => {
        var mabc = list_dotluong[i].firstElementChild.innerHTML;
        var t = list_dotluong[i].children[1].firstElementChild.innerHTML;
        console.log(mabc);
        console.log(t);
        $.ajax({
            url: '/Home/ChiTietDotLuong?MaBCDL=' + mabc + "&ThangBaoCao=" + t,
            success: function (data, status) {
                $('#main_working_window_id').html(data);
                console.log(status);
            }
        });
    });
}

var m_picker = document.getElementById("month_picker");
document.getElementById("XemLuong_id").addEventListener('click', () => {
    if (m_picker.value.length != 0) {
        var t = convertFromStringToDate(m_picker.value);
        $.ajax({
            url: '/Home/ChiTietDotLuong?MaBCDL=' + 1 + '&ThangBaoCao=' + t[1] + '/' + t[0] + '&Thang=' + m_picker.value,
            success: function (data, status) {
                $('#main_working_window_id').html(data);
                console.log(status);
            }
        })
    }
    else {
        toastMessage({ title: "Alert", message: "Vui lòng chọn tháng", type: "fail" });
    }
});

function convertFromStringToDate(responseDate) {
    let datePieces = responseDate.split("-");
    return datePieces;
};

var del = document.querySelectorAll('#del_btn_id');

for (let i = 0; i < del.length; i++) {
    del[i].addEventListener('click', () => {
        var answer = window.confirm("Bạn có muốn xóa");
        if (answer) {
            var xhr_login = new XMLHttpRequest();
            let url_login = "https://localhost:5001/Home/RemoveDotLuong?MaBCDL=" + del[i].parentElement.id;
            xhr_login.open("GET", url_login, true);
            xhr_login.timeout = 20000;
            xhr_login.onreadystatechange = function () {

                if (xhr_login.readyState == 4 && xhr_login.status == 200) {

                    var result = xhr_login.response;
                    if (result > 0) {
                        console.log("Xóa thành công");
                        del[i].parentElement.remove();
                    }
                    else
                        console.log("Xóa thất bại");
                }

            }
            xhr_login.send();
        }
    });
}

var taoRP = document.getElementById("CreateBangLuong_id");

taoRP.addEventListener('click', () => {
    if (m_picker.value.length != 0) {
        var da = convertFromStringToDate(m_picker.value);
        var timeElapsed = Date.now();
        var today = new Date(timeElapsed);
        var xhr_login = new XMLHttpRequest();

        loadingElement.show();
        var xhr_login = new XMLHttpRequest();
        var url_login = "https://localhost:5001/Home/AddDotLuongReport?NgayTraLuong=" + "01-" + da[1] + "-" + da[0] + "&NgayLap=" + today.toDateString();

        xhr_login.open("GET", url_login, true);
        xhr_login.timeout = 20000;
        xhr_login.onreadystatechange = function () {

            if (xhr_login.readyState == 4 && xhr_login.status == 200) {
                var result = xhr_login.response;
                console.log(result);
                loadingElement.hide();
                if (result == 1)
                    toastMessage({ title: "Thành công!", message: "Tạo báo cáo thành công!", type: "success" });
                else
                    toastMessage({ title: "Thất bại!", message: "Tạo báo cáo thất bại!", type: "fail" });
                ;
            }
        }
        xhr_login.send();
    }
    else {
        toastMessage({ title: "Alert", message: "Vui lòng chọn tháng", type: "fail" });
    }
});
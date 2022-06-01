

var listbc = document.querySelectorAll('#list_BaoCaoDoanhThu_id');


for (let i = 0; i < listbc.length; i++) {
    listbc[i].addEventListener('click', () => {
        var mabc = listbc[i].firstElementChild.innerHTML;
        var t = listbc[i].children[1].firstElementChild.innerHTML;
        console.log(mabc);
        console.log(t);
        
        $.ajax({
            url: '/Home/ChiTietDoanhThuPhong?MaBCDoanhThu=' + mabc + "&ThangBaoCao="+ t,
            success: function (data, status) {
                $('#main_working_window_id').html(data);
                console.log(status);
            }
        });
    });
}

var del = document.querySelectorAll('#del_btn_id');

for (let i = 0; i < del.length; i++) {
    del[i].addEventListener('click', () => {
        var answer = window.confirm("Bạn có muốn xóa");
        if (answer) {
            
            var xhr_login = new XMLHttpRequest();
            let url_login = "https://localhost:5001/Home/RemoveMonthReport?MaBC=" + del[i].parentElement.id;
            xhr_login.open("GET", url_login, true);
            xhr_login.timeout = 20000;
            xhr_login.onreadystatechange = function () {
            
                if (xhr_login.readyState == 4 && xhr_login.status == 200) {
                    
                    var result = xhr_login.response;
                    if (result) {
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


var m_picker = document.getElementById("month_picker");
var taoRP = document.getElementById("CreateReport_btn_id");
var showRP = document.getElementById("ShowReport_btn_id");

showRP.addEventListener('click', () => {
    console.log(1);
    if (m_picker.value.length != 0) {
        var t = convertFromStringToDate(m_picker.value);
        $.ajax({
            url: '/Home/ChiTietDoanhThuPhong?MaBCDoanhThu=' + 1 + '&ThangBaoCao=' + t[1] + '/' + t[0]  +  '&Thang=' + m_picker.value,
            success: function (data, status) {
                $('#main_working_window_id').html(data);
                console.log(status);
            }
        })
    }
    else {
        alert("Vui lòng chọn tháng");
    }
});

taoRP.addEventListener('click', () => {
    if (m_picker.value.length != 0) {
        var da = convertFromStringToDate(m_picker.value);
        console.log("01-" + da[1] + "-" + da[0]);
        var timeElapsed = Date.now();
        var today = new Date(timeElapsed);
        console.log(today.toDateString());
        loadingElement.show();
        var xhr_login = new XMLHttpRequest();
        var url_login = "https://localhost:5001/Home/AddMonthReport?ThangBC=" + "01-" + da[1] + "-" + da[0] + "&NgayLap=" + today.toDateString();
        
        xhr_login.open("GET", url_login, true);
        xhr_login.timeout = 20000;
        xhr_login.onreadystatechange = function () {

            if (xhr_login.readyState == 4 && xhr_login.status == 200) {
                var result = xhr_login.response;
                console.log(result);
                loadingElement.hide();
                if (result == 1)
                    alert("Thêm thành công");
                else
                    alert("Thêm thất bại");
;           }
        }
        xhr_login.send();
    }   
    else {
        alert("Vui lòng chọn tháng!");
    }
});

function convertFromStringToDate(responseDate) {
    let datePieces = responseDate.split("-");
    return datePieces;
};




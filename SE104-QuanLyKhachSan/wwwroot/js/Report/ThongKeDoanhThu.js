

var printBtn = document.getElementById("PrintReport_btn_id");
printBtn.addEventListener('click', () => {
    const element = document.getElementById("ThongKeDoanhThu_container_id");

    html2pdf()
        .from(element)
        .save("BaoCaoChiTietDoanhThu.pdf");
})

var m_picker = document.getElementById("month_picker_id");
var showRP = document.getElementById("ShowReport_btn_id");


showRP.addEventListener('click', () => {
    console.log(1);
    if (m_picker.value.length != 0) {
        var t = convertFromStringToDate(m_picker.value);
        console.log(t[0] + '-' + t[1] + '-01');
        loadingElement.show();
        $.ajax({
            url: '/Home/ThongKeDoanhThu?ThangBaoCao=' + t[0] + '-' + t[1] + '-01',
            success: function (data, status) {
                $('#main_working_window_id').html(data);
                console.log(status);
                loadingElement.hide();
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
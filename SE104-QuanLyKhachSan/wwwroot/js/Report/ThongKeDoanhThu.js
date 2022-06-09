

var printBtn = document.getElementById("PrintReport_btn_id");
printBtn.addEventListener('click', () => {
    /*const element = document.getElementById("ThongKeDoanhThu_container_id");

    html2pdf()
        .from(element)
        .save("BaoCaoChiTietDoanhThu.pdf");*/
    Export_Receipt_ToPDF();
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

function Export_ToPDF_From_Content(contentElement) {
    return new Promise(function (myResolve) {
        let file_name = "ThongKeDoanhThu.pdf";
        html2pdf(
            contentElement,
            {
                margin: 2,
                filename: file_name,
                jsPDF: { unit: 'mm', format: 'a4', orientation: 'landscape' }
            }
        )
        myResolve()
    });
}
function Export_Receipt_ToPDF() {
    let contentElement = document.getElementById("ThongKeDoanhThu_container_id");
    Export_ToPDF_From_Content(contentElement).then(function (value) {
        toastMessage({ title: "Thông báo", message: "Xuất thành công!", type: "success" });
    });
}
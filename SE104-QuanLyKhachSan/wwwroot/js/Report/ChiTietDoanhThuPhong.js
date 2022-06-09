document.getElementById("back_btn_id").addEventListener('click', () => {
    $.ajax({
        url: '/Home/DTtheoLoaiPhong',
        success: function (data, status) {
            $('#main_working_window_id').html(data);
            console.log(status);
        }
    })
});

document.getElementById("print_btn_id").addEventListener('click', () => {
    /*const element = document.getElementById("ChiTietDoanhThu_container_id");
    
    html2pdf()
        .from(element)
        .save("BaoCaoChiTietDoanhThu.pdf");*/
    Export_Receipt_ToPDF();
});

var m_picker = document.getElementById("month_picker_id");

function Export_ToPDF_From_Content(contentElement) {
    return new Promise(function (myResolve) {
        let file_name = "BaoCaoChiTietDoanhThu.pdf";
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
    let contentElement = document.getElementById("ChiTietDoanhThu_container_id");
    Export_ToPDF_From_Content(contentElement).then(function (value) {
        toastMessage({ title: "Thông báo", message: "Xuất thành công!", type: "success" });
    });
}

function formatGiaTien(giaTien) {
    let giaTienAfter = "";
    let len = giaTien.length;
    for (let j = 1; j <= len; j++) {
        giaTienAfter = giaTien[len - j] + giaTienAfter;
        if (j % 3 == 0 && j != len) {
            giaTienAfter = "." + giaTienAfter;
        }
    }
    return giaTienAfter;
}


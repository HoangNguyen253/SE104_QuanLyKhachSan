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
    const element = document.getElementById("ChiTietDoanhThu_container_id");
    
    html2pdf()
        .from(element)
        .save("BaoCaoChiTietDoanhThu.pdf");
});

var m_picker = document.getElementById("month_picker_id");
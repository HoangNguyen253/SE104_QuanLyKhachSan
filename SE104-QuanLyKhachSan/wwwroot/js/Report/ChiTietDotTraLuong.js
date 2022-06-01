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

var edit_btn = document.getElementById("Edit_btn");
edit_btn.addEventListener('click', () => {
    var manhanvien = edit_btn.parentElement.parentElement.children[1].innerHTML;
    var hoten = edit_btn.parentElement.parentElement.children[2].innerHTML;
    var thuong = edit_btn.parentElement.parentElement.children[5].innerHTML;
    var phat = edit_btn.parentElement.parentElement.children[6].innerHTML;
    var sotien = edit_btn.parentElement.parentElement.children[7].innerHTML;
    console.log(sotien);
});




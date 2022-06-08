//---------------------------BUTTON_STEP-start
function addEventForButtonStep(button) {
    const subButton = button.querySelector(".subtractVal");
    const addButton = button.querySelector(".addVal");
    subButton.addEventListener('click', function () {
        const minVal = button.getAttribute("minVal")*1;
        let val = (button.querySelector("span").innerHTML)*1;
        if (val > minVal || minVal == 0)
        {  
            val-=1;
            button.querySelector("span").innerHTML = val;
        }
    });
    addButton.addEventListener('click', function () {
        const maxVal = button.getAttribute("maxVal")*1;
        let val = (button.querySelector("span").innerHTML)*1;
        if (val < maxVal || maxVal == 0)
        {  
            val+=1;
            button.querySelector("span").innerHTML = val;
        }
    });
}
//---------------------------BUTTON_STEP-end

//----------------------POPUP-LOAIPHONG:Start
//show popup

let popupContainer = document.querySelector(".main_working_window .popup-container");
let popup = document.querySelector(".main_working_window .popup-container .popup");

let subPopupContainer = popupContainer.querySelector(".sub-popup-container");
let subPopup = popupContainer.querySelector(".sub-popup-container .sub-popup");

let confirmPopupContainer = document.querySelector(".main_working_window .confirm-popup-container");
let confirmPopup = document.querySelector(".main_working_window .confirm-popup-container .confirm-popup");

let historyPopupContainer = document.querySelector(".main_working_window .history-popup-container");
let historyPopup = document.querySelector(".main_working_window .history-popup-container .history-popup");

let buttonChangeInPopup = popup.querySelector(".footer-popup button.blue-button");

function resetEventListenerForChangeButtonInPopup() {
    let old_element = popup.querySelector(".footer-popup button.blue-button");
    let new_element = old_element.cloneNode(true);
    old_element.parentNode.replaceChild(new_element, old_element);
}

function HIDEbuttonChangeInPopup() {
    popup.querySelector(".footer-popup button.blue-button").style.display = "none";
}

function SHOWbuttonChangeInPopup() {
    popup.querySelector(".footer-popup button.blue-button").style.display = "inline";
    resetEventListenerForChangeButtonInPopup();
}

function showPopup() {
    popupContainer.style.display = "flex";
    popup.style.display = "block";
}

function showHistoryPopup() {
    historyPopupContainer.style.display = "flex";
    historyPopup.style.display = "block";
}

function resetEventListenerForChangeButton() {
    let old_element = subPopup.querySelector(".footer-popup .blue-button");
    let new_element = old_element.cloneNode(true);
    old_element.parentNode.replaceChild(new_element, old_element);
}

function showSubPopup() {
    subPopupContainer.style.display = "flex";
    subPopup.style.display = "block";
    resetEventListenerForChangeButton();
}

function resetEventListenerForConfirmButton() {
    let old_element = confirmPopup.querySelector(".footer-popup .blue-button");
    let new_element = old_element.cloneNode(true);
    old_element.parentNode.replaceChild(new_element, old_element);
}

function showConfirmPopup() {
    confirmPopupContainer.style.display = "flex";
    confirmPopup.style.display = "block";
    resetEventListenerForConfirmButton();

}

function createElementFromHTML(htmlString) {
    var div = document.createElement('div');
    div.innerHTML = htmlString.trim();
    return div.firstChild;
}

function createElementFromHTMLForTrTag(htmlString) {
    var tbody = document.createElement('tbody');
    tbody.innerHTML = htmlString.trim();
    return tbody.firstChild;
}

function convertDDMMYYYYtoDateInputFormat(ddmmyyyy) {
    var dateParts = ddmmyyyy.split("/");
    return dateParts[2] + "-" + dateParts[1] + "-" + dateParts[0];
}

function convertDDMMYYYYHHMMSStoDateTimeInputFormat(ddmmyyyyhhmmss) {
    var dateTimeParts = ddmmyyyyhhmmss.split(", ");
    var dateParts = dateTimeParts[0].split("/");
    return dateParts[2] + "-" + dateParts[1] + "-" + dateParts[0] + "T" + dateTimeParts[1];
}

function closeConfirmPopup() {
    confirmPopupContainer.style.display = "none";
    confirmPopup.style.display = "none";
}

function closeSubPopup() {
    subPopupContainer.style.display = "none";
    subPopup.style.display = "none";
}

function closePopup() {
    popupContainer.style.display = "none";
    popup.style.display = "none";
}

function resetSTT(elementContainer) {
    let count = 1;
    elementContainer.querySelectorAll('.item-table').forEach(IT => {
        IT.firstChild.innerHTML = count;
        count++;
    });
}

function regulationClick(element) {
    showPopup();
    //title in header popup
    popup.querySelector(".header-popup div").innerHTML = (element.querySelector("p").innerHTML).toString().toUpperCase();
}

//add event for all close popup
let listClosePopup = document.querySelectorAll(".header-popup i");

listClosePopup.forEach((CP) =>{
    CP.addEventListener("click", () => {
        let container = CP.parentElement.parentElement.parentElement;
        let popup = CP.parentElement.parentElement;
        container.style.display = "none";
        popup.style.display = "none";
    });
});

//add event for all cancel popup
let listCancelButton = document.querySelectorAll(".main_working_window .footer-popup .cancel-button");

listCancelButton.forEach(CB => {
    CB.addEventListener('click', () => {
        let container = CB.parentElement.parentElement.parentElement;
        let popup = CB.parentElement.parentElement;
        container.style.display = "none";
        popup.style.display = "none";
    });
});

//show popup
let listRegulation = document.querySelectorAll(".main_working_window .regulation-item");

//LOAD DATA FOR POPUP

//----1. loai phong: start

function DontHaveRoomType() {
    let contentPopupHTML = "<div class='information'>Chưa có loại phòng nào!</div>" +
        "<div class='add-new'>" +
        "<button class='blue-button'>+ Thêm mới</button>" +
        "</div>";
    popup.querySelector(".content-popup").innerHTML = contentPopupHTML;

    popup.querySelector(".content-popup .add-new .blue-button").addEventListener("click", () => {
        showSubPopup();
        updateAndAddLoaiPhongClick(null, false);
    });
}

function updateLoaiPhong(maLoaiPhong, tenLoaiPhong, giaTienCoBan) {
    loadingElement.show();

    let xhr_update_loaiphong = new XMLHttpRequest();
    let url_update_loaiphong = "https://localhost:5001/Regulation/UpdateLoaiPhong?maLoaiPhong=" + maLoaiPhong + "&tenLoaiPhong=" + tenLoaiPhong + "&giaTienCoBan=" + giaTienCoBan;
    xhr_update_loaiphong.open("GET", url_update_loaiphong, true);
    xhr_update_loaiphong.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr_update_loaiphong.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr_update_loaiphong.readyState == 4 && xhr_update_loaiphong.status == 200) {

                let result = xhr_update_loaiphong.responseText;
                resolve(result);
            }
        }
        xhr_update_loaiphong.send();
    });
}

function insertLoaiPhong(tenLoaiPhong, giaTienCoBan) {
    loadingElement.show();

    let xhr_insert_loaiphong = new XMLHttpRequest();
    let url_insert_loaiphong = "https://localhost:5001/Regulation/InsertLoaiPhong?tenLoaiPhong=" + tenLoaiPhong + "&giaTienCoBan=" + giaTienCoBan;
    xhr_insert_loaiphong.open("GET", url_insert_loaiphong, true);
    xhr_insert_loaiphong.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr_insert_loaiphong.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr_insert_loaiphong.readyState == 4 && xhr_insert_loaiphong.status == 200) {

                let result = xhr_insert_loaiphong.responseText;
                resolve(result);
            }
        }
        xhr_insert_loaiphong.send();
    });
}


function updateAndAddLoaiPhongClick(item, isUpdate) {
    //title header popup
    let titlePopup = subPopup.querySelector(".header-popup div");
    let nameBlueButton = subPopup.querySelector(".footer-popup .blue-button");
    titlePopup.innerHTML = "THÊM LOẠI PHÒNG";
    nameBlueButton.innerHTML = "Thêm";

    //content sub popup
    let maLoaiPhong = "";
    let valueTenLoaiPhong = "";
    let valueGiaTien = "";
    let baseTenLoaiPhong = "";
    let baseGiaTienCoBan= "";
    
    if (isUpdate == true) 
    {
        titlePopup.innerHTML = "SỬA LOẠI PHÒNG";
        nameBlueButton.innerHTML = "Sửa";
        let listTD = item.querySelectorAll("td");
        baseTenLoaiPhong = listTD[1].innerHTML;
        baseGiaTienCoBan = listTD[2].innerHTML.toString().replace(/,/g, "")
        maLoaiPhong = "maloaiphong='" + item.getAttribute('maloaiphong') + "'";
        valueTenLoaiPhong = "value='" + baseTenLoaiPhong + "'";
        valueGiaTien = "value='" + baseGiaTienCoBan + "'";
    }

    let subPopupContentHTML = "<div class='table-container'>" +
                                    "<table " + maLoaiPhong + ">" + 
                                        "<tr>" + 
                                            "<td class='label'>Tên loại phòng: </td>" +
                                            "<td class='entry'>" +
                                                "<input class='ten-loai-phong' type='text' placeholder='Tên loại phòng' " + valueTenLoaiPhong + ">" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" + 
                                            "<td class='label'>Giá tiền (VND/đêm): </td>" +
                                            "<td class='entry'>" +
                                                "<input class='gia-tien' type='number' placeholder='Giá tiền' " + valueGiaTien + ">" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" + 
                                "</div>";

    subPopup.querySelector(".content-popup").innerHTML = subPopupContentHTML;
    
    let inputTenLoaiPhong = subPopup.querySelector(".content-popup .table-container table input.ten-loai-phong");
    let inputGiaTienCoBan = subPopup.querySelector(".content-popup .table-container table input.gia-tien");
    let buttonSubmit = subPopup.querySelector(".footer-popup .blue-button");
    buttonSubmit.disabled = false;

    if (isUpdate == true) {
        buttonSubmit.disabled = true;
        inputTenLoaiPhong.addEventListener("input", () => {
            if (inputTenLoaiPhong.value.trim() == baseTenLoaiPhong && inputGiaTienCoBan.value == baseGiaTienCoBan) {
                buttonSubmit.disabled = true;
                return;
            }
            buttonSubmit.disabled = false;
        })
        inputGiaTienCoBan.addEventListener("input", () => {
            if (inputTenLoaiPhong.value.trim() == baseTenLoaiPhong && inputGiaTienCoBan.value == baseGiaTienCoBan) {
                buttonSubmit.disabled = true;
                return;
            }
            buttonSubmit.disabled = false;
        })
    }

    //add event for change button
    buttonSubmit.addEventListener("click", (e) => {
        let maLoaiPhong = subPopup.querySelector(".content-popup .table-container table").getAttribute("maloaiphong");
        let tenLoaiPhong = inputTenLoaiPhong.value.trim();
        let giaTienCoBan = inputGiaTienCoBan.value.trim();
        if (tenLoaiPhong == "" || giaTienCoBan == 0) {
            toastMessage({ title: 'Lỗi', message: 'Trường giá tiền và tên loại phòng không được phép trống!', type: 'fail', duration: 3500 })
        } else {
            if (maLoaiPhong != null) {
                updateLoaiPhong(maLoaiPhong, tenLoaiPhong, giaTienCoBan).then(
                    function (value) {
                        if (value == "true") {
                            loadPopupLoaiPhong();
                            closeSubPopup();
                        } else {
                            toastMessage({ title: 'Sửa thất bại', message: 'Đã có loại phòng ' + tenLoaiPhong + ' trong hệ thống.', type: 'fail', duration: 3500 });
                        }
                    }
                );
            } else {
                insertLoaiPhong(tenLoaiPhong, giaTienCoBan).then(
                    function (value) {
                        if (value == "true") {
                            loadPopupLoaiPhong();
                            closeSubPopup();
                        } else {
                            toastMessage({ title: 'Thêm thất bại', message: 'Đã có loại phòng ' + tenLoaiPhong + ' trong hệ thống.', type: 'fail', duration: 3500 })
                        }
                    }
                );
            }
            
        }
    })
}

function deleteLoaiPhongConfirm(maLoaiPhong) {

    loadingElement.show();
    let xhr_delete_loaiphong = new XMLHttpRequest();
    let url_delete_loaiphong = "https://localhost:5001/Regulation/DeleteLoaiPhong?maLoaiPhong=" + maLoaiPhong;
    xhr_delete_loaiphong.open("GET", url_delete_loaiphong, true);
    xhr_delete_loaiphong.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr_delete_loaiphong.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr_delete_loaiphong.readyState == 4 && xhr_delete_loaiphong.status == 200) {

                let result = xhr_delete_loaiphong.responseText;
                resolve(result);
            }
        }
        xhr_delete_loaiphong.send();
    });
}

function GetLoaiPhong() {

    loadingElement.show();

    let xhr_get_loaiphong = new XMLHttpRequest();
    let url_get_loaiphong = "https://localhost:5001/Regulation/GetLoaiPhong";
    xhr_get_loaiphong.open("GET", url_get_loaiphong, true);
    xhr_get_loaiphong.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr_get_loaiphong.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr_get_loaiphong.readyState == 4 && xhr_get_loaiphong.status == 200) {
                let result = JSON.parse(xhr_get_loaiphong.response);
                resolve(result);
            }
        }
        xhr_get_loaiphong.send();
    });
}
let dataLoaiPhongSave = null;
function checkChangeAfterInsertUpdateLoaiPhong(maLoaiPhong, tenLoaiPhong, giaTienCoBan) {
    let checkChange = true;
    if (dataLoaiPhongSave == null) {
        return false;
    }
    dataLoaiPhongSave.forEach(ds => {
        if (ds["maLoaiPhong"] == maLoaiPhong
            && ds["tenLoaiPhong"] == tenLoaiPhong
            && ds["giaTienCoBan"] == giaTienCoBan) {
            checkChange = false;
            return;
        }
    })
    return checkChange;
}

function loadPopupLoaiPhong() {
    //get loaiphong
    GetLoaiPhong().then(
        function (value) { //success

            let dataLoaiPhong = value;

            //show popup and add title
            regulationClick(listRegulation[0]);

            //set width for popup
            popup.style.width = "60%";

            if (dataLoaiPhong == null || dataLoaiPhong.length == 0) {
                DontHaveRoomType();
                return;
            }

            //content popup
            let contentPopupHTML = "<table>" +
                "<tr class='header-table'>" +
                "<td>STT</td>" +
                "<td>Tên loại phòng</td>" +
                "<td>Giá tiền (VND/đêm)</td>" +
                "<td></td>" +
                "</tr>";

            //render loai phong
            let count = 1;
            dataLoaiPhong.forEach(LP => {
                let justAdd = "";
                if (checkChangeAfterInsertUpdateLoaiPhong(LP["maLoaiPhong"], LP["tenLoaiPhong"], LP["giaTienCoBan"])) {
                    justAdd = "just-add";
                }
                contentPopupHTML += "<tr class='item-table " + justAdd + "' maloaiphong='" + LP["maLoaiPhong"] + "'>" +
                    "<td>" + count + "</td>" +
                    "<td>" + LP["tenLoaiPhong"] + "</td>" +
                    "<td>" + LP["giaTienCoBan"].toLocaleString() + "</td>" +
                    "<td>" +
                    "<div>" +
                    "<button class='green-button'>Sửa</button>" +
                    "<button class='red-button'>Xóa</button>" +
                    "</div>" +
                    "</td>" +
                    "</tr>";
                count++;
            });

            //footer popup
            contentPopupHTML += "</table>" +
                "<div class='add-new'>" +
                "<button class='blue-button'>+ Thêm mới</button>" +
                "</div>";
            popup.querySelector(".content-popup").innerHTML = contentPopupHTML;

            //add event for update button
            let listUpdateRoomButton = popup.querySelectorAll(".content-popup table td button.green-button");
            listUpdateRoomButton.forEach((UB) => {
                UB.addEventListener('click', () => {
                    showSubPopup();

                    //pass ITEM-TABLE_LOAIPHONG to sub popup
                    updateAndAddLoaiPhongClick(UB.parentElement.parentElement.parentElement, true);
                });
            });

            //add event for delete button
            let listDeleteRoomButton = popup.querySelectorAll(".content-popup table td .red-button");
            listDeleteRoomButton.forEach(DRB => {
                DRB.addEventListener("click", () => {
                    showConfirmPopup();

                    //pass message and update title
                    confirmPopup.querySelector(".header-popup div").innerHTML = "XÓA LOẠI PHÒNG";
                    confirmPopup.querySelector(".content-popup .message").innerHTML = "Bạn có chắc chắn muốn xóa loại phòng này?";

                    let confirmButton = confirmPopup.querySelector(".footer-popup .blue-button");
                    confirmButton.addEventListener('click', () => {
                        let child = DRB.parentElement.parentElement.parentElement;
                        let tenLoaiPhong = child.querySelectorAll("td")[1].innerHTML;
                        deleteLoaiPhongConfirm(child.getAttribute("maloaiphong")).then(
                            function (value) {
                                closeConfirmPopup();
                                if (value == "true") {
                                    loadPopupLoaiPhong();
                                    toastMessage({ title: 'Xóa thành công', message: 'Xóa thành công loại phòng ' + tenLoaiPhong, type: 'success', duration: 3500 });
                                }
                                else {
                                    toastMessage({ title: 'Xóa thất bại', message: 'Loại phòng ' + tenLoaiPhong + ' đang được sử dụng trong hệ thống (phòng, báo cáo theo loại phòng)', type: 'fail', duration: 3500 });
                                }
                            }
                        );
                    });
                });
            });

            //add event for add new
            popup.querySelector(".content-popup .add-new .blue-button").addEventListener("click", () => {
                showSubPopup();
                updateAndAddLoaiPhongClick(null, false);
            });
            dataLoaiPhongSave = dataLoaiPhong;
        },
        function (error) { //fail
            toastMessage({ title: 'Lỗi kết nối', message: 'Lấy thông tin loại phòng thất bại', type: 'fail', duration: 3500 });
        }
    );
}
listRegulation[0].addEventListener("click", (e) => {

    HIDEbuttonChangeInPopup();

    loadPopupLoaiPhong();
});

//----1. loai phong: end

//-------------------------------------------------------------------------------------

//----2. loai khach: start
function DontHaveCustomerType() {
    let contentPopupHTML = "<div class='information'>Chưa có loại khách hàng nào!</div>" +
        "<div class='add-new'>" +
        "<button class='blue-button'>+ Thêm mới</button>" +
        "</div>";
    popup.querySelector(".content-popup").innerHTML = contentPopupHTML;

    popup.querySelector(".content-popup .add-new .blue-button").addEventListener("click", () => {
        showSubPopup();
        updateAndAddLoaiKhachHangClick(null, false);
    });
}

function updateLoaiKhachHang(maLoaiKhachHang, tenLoaiKhachHang) {

    loadingElement.show();

    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/UpdateLoaiKhachHang?maLoaiKhachHang=" + maLoaiKhachHang + "&tenLoaiKhachHang=" + tenLoaiKhachHang;
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = xhr.responseText;
                resolve(result);
            }
        }
        xhr.send();
    });
}

function insertLoaiKhachHang(tenLoaiKhachHang) {
    loadingElement.show();

    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/InsertLoaiKhachHang?tenLoaiKhachHang=" + tenLoaiKhachHang;
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = xhr.responseText;
                resolve(result);
            }
        }
        xhr.send();
    });
}

function updateAndAddLoaiKhachHangClick(item, isUpdate) {
    //title header popup
    let titlePopup = subPopup.querySelector(".header-popup div");
    let nameBlueButton = subPopup.querySelector(".footer-popup .blue-button");
    titlePopup.innerHTML = "THÊM LOẠI KHÁCH HÀNG";
    nameBlueButton.innerHTML = "Thêm";

    //content sub popup
    let maLoaiKhachHang = "";
    let valueTenLoaiKhachHang = "";
    let baseTenLoaiKH = "";
    
    if (isUpdate == true) 
    {
        titlePopup.innerHTML = "SỬA LOẠI KHÁCH HÀNG";
        nameBlueButton.innerHTML = "Sửa";

        let listTD = item.querySelectorAll("td");

        baseTenLoaiKH = listTD[1].innerHTML;

        maLoaiKhachHang = "maloaikhachhang='" + item.getAttribute('maloaikhachhang') + "'";
        valueTenLoaiKhachHang = "value='" + baseTenLoaiKH + "'";
    }

    let subPopupContentHTML = "<div class='table-container'>" +
                                    "<table " + maLoaiKhachHang + ">" + 
                                        "<tr>" + 
                                            "<td class='label'>Tên loại khách hàng: </td>" +
                                            "<td class='entry'>" +
                                                "<input class='ten-loai-khach-hang' type='text' placeholder='Tên loại khách hàng' " + valueTenLoaiKhachHang + ">" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" + 
                                "</div>";

    subPopup.querySelector(".content-popup").innerHTML = subPopupContentHTML;

    let inputTenLoaiLoaiKH= subPopup.querySelector(".content-popup .table-container table input.ten-loai-khach-hang");
    let buttonSubmit = subPopup.querySelector(".footer-popup .blue-button");
    buttonSubmit.disabled = false;

    if (isUpdate == true) {
        buttonSubmit.disabled = true;
        inputTenLoaiLoaiKH.addEventListener("input", () => {
            if (inputTenLoaiLoaiKH.value.trim() == baseTenLoaiKH) {
                buttonSubmit.disabled = true;
                return;
            }
            buttonSubmit.disabled = false;
        })
    }

    //add event for change button
    buttonSubmit.addEventListener("click", (e) => {
        let maLoaiKhachHang = subPopup.querySelector(".content-popup .table-container table").getAttribute("maloaikhachhang");
        let tenLoaiKhachHang = subPopup.querySelector(".content-popup .table-container table input.ten-loai-khach-hang").value.trim();
        if (tenLoaiKhachHang == "") {
            toastMessage({ title: 'Lỗi', message: 'Trường tên loại phòng không được phép trống!', type: 'fail', duration: 3500 });
        } else {
            if (maLoaiKhachHang != null) {
                updateLoaiKhachHang(maLoaiKhachHang, tenLoaiKhachHang).then(
                    function (value) {
                        if (value == "true") {
                            loadPopupLoaiKhachHang();
                            closeSubPopup();
                        } else {
                            toastMessage({ title: 'Sửa thất bại', message: 'Đã có loại khách hàng ' + tenLoaiKhachHang + ' trong hệ thống.', type: 'fail', duration: 3500 });
                        }
                    }
                );
            } else {
                insertLoaiKhachHang(tenLoaiKhachHang).then(
                    function (value) {
                        console.log(value);
                        if (value == "true") {
                            loadPopupLoaiKhachHang();
                            closeSubPopup();
                        } else {
                            toastMessage({ title: 'Thêm thất bại', message: 'Đã có loại khách hàng ' + tenLoaiKhachHang + ' trong hệ thống.', type: 'fail', duration: 3500 });
                        }
                    }
                );
            }

        }
    })
}

function getLoaiKhachHang() {
    loadingElement.show();

    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/GetLoaiKhachHang";
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = () => {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {
                let result = JSON.parse(xhr.response);
                resolve(result);
            }
        }
        xhr.send();
    });
}

function deleteLoaiKhachHangConfirm(maLoaiKhachHang) {
    loadingElement.show();
    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/DeleteLoaiKhachHang?maLoaiKhachHang=" + maLoaiKhachHang;
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = xhr.responseText;
                resolve(result);
            }
        }
        xhr.send();
    });
}
let dataLoaiKhachHangSave = null;
function checkChangeAfterInsertUpdateLoaiKhachHang(maLoaiKhachHang, tenLoaiKhachHang) {
    let checkChange = true;
    if (dataLoaiKhachHangSave == null) {
        return false;
    }
    dataLoaiKhachHangSave.forEach(ds => {
        if (ds["maLoaiKhachHang"] == maLoaiKhachHang
            && ds["tenLoaiKhachHang"] == tenLoaiKhachHang) {
            checkChange = false;
            return;
        }
    })
    return checkChange;
}

function loadPopupLoaiKhachHang() {
    getLoaiKhachHang().then(
        function (value) {
            let dataLoaiKhachHang = value;

            regulationClick(listRegulation[1]);

            //set width for popup
            popup.style.width = "40%";

            if (dataLoaiKhachHang == null || dataLoaiKhachHang.length == 0) {
                DontHaveCustomerType();
                return;
            }

            //content popup
            let contentPopupHTML = "<table>" +
                "<tr class='header-table'>" +
                "<td>STT</td>" +
                "<td>Tên loại khách hàng</td>" +
                "<td></td>" +
                "</tr>";

            //render loai phong
            let count = 1;
            dataLoaiKhachHang.forEach(LKH => {
                let justAdd = "";
                if (checkChangeAfterInsertUpdateLoaiKhachHang(LKH["maLoaiKhachHang"], LKH["tenLoaiKhachHang"])) {
                    justAdd = "just-add";
                }
                contentPopupHTML += "<tr class='item-table " + justAdd + "' maloaikhachhang='" + LKH["maLoaiKhachHang"] + "'>" +
                    "<td>" + count + "</td>" +
                    "<td>" + LKH["tenLoaiKhachHang"] + "</td>" +
                    "<td>" +
                    "<div>" +
                    "<button class='green-button'>Sửa</button>" +
                    "<button class='red-button'>Xóa</button>" +
                    "</div>" +
                    "</td>" +
                    "</tr>";
                count++;
            });

            //footer popup
            contentPopupHTML += "</table>" +
                "<div class='add-new'>" +
                "<button class='blue-button'>+ Thêm mới</button>" +
                "</div>";
            popup.querySelector(".content-popup").innerHTML = contentPopupHTML;

            //add event for update button
            let listUpdateRoomButton = popup.querySelectorAll(".content-popup table td button.green-button");
            listUpdateRoomButton.forEach((UB) => {
                UB.addEventListener('click', () => {
                    showSubPopup();

                    //pass ITEM-TABLE_LOAIPHONG to sub popup
                    updateAndAddLoaiKhachHangClick(UB.parentElement.parentElement.parentElement, true);
                });
            });

            //add event for delete button
            let listDeleteRoomButton = popup.querySelectorAll(".content-popup table td .red-button");
            listDeleteRoomButton.forEach(DRB => {
                DRB.addEventListener("click", () => {
                    showConfirmPopup();

                    //pass message and update title
                    confirmPopup.querySelector(".header-popup div").innerHTML = "XÓA LOẠI KHÁCH HÀNG";
                    confirmPopup.querySelector(".content-popup .message").innerHTML = "Bạn có chắc chắn muốn xóa loại khách hàng này?";

                    let confirmButton = confirmPopup.querySelector(".footer-popup .blue-button");
                    confirmButton.addEventListener('click', () => {
                        deleteLoaiKhachHangConfirm(DRB.parentElement.parentElement.parentElement.getAttribute("maloaikhachhang")).then(
                            function (value) {
                                closeConfirmPopup();
                                let child = DRB.parentElement.parentElement.parentElement;
                                let tenLoaiKhachHang = child.querySelectorAll("td")[1].innerHTML;
                                if (value == "true") {
                                    loadPopupLoaiKhachHang();
                                    toastMessage({ title: 'Xóa thành công', message: 'Xóa thành công loại khách hàng ' + tenLoaiKhachHang, type: 'success', duration: 3500 });

                                }
                                else {
                                    toastMessage({ title: 'Xóa thất bại', message: 'Loại khách hàng ' + tenLoaiKhachHang + ' đã được sử dụng trong hệ thống (hệ số phụ thu, khách thuê).', type: 'fail', duration: 3500 });

                                }
                            }
                        );
                    });
                });

            });

            //add event for add new
            popup.querySelector(".content-popup .add-new .blue-button").addEventListener("click", () => {
                showSubPopup();
                updateAndAddLoaiKhachHangClick(null, false);
            });

            dataLoaiKhachHangSave = dataLoaiKhachHang;
        }, function (error) {
            toastMessage({ title: 'Lỗi kết nối', message: 'Lấy thông tin loại khách hàng thất bại', type: 'fail', duration: 3500 });
        }
    );
}

listRegulation[1].addEventListener("click", () => {

    HIDEbuttonChangeInPopup();

    loadPopupLoaiKhachHang();
});
//----2. loai khach: end

//----3. so khach toi da: start

function getSoKhachToiDa() {
    loadingElement.show();
    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/GetSoKhachToiDa";
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = xhr.responseText;
                if (result*1 == -1) {
                    reject("Lỗi kết nối");
                } else {
                    resolve(result);
                }
            }
        }
        xhr.send();
    });
}

function updateSoKhachToiDa(soKhacToiDaNew) {
    loadingElement.show();
    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/UpdateSoKhachToiDa?soKhachToiDa=" + soKhacToiDaNew;
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = xhr.responseText;
                resolve(result);
            }
        }
        xhr.send();
    });
}

listRegulation[2].addEventListener("click", (e) => {

    SHOWbuttonChangeInPopup();

    getSoKhachToiDa().then(
        function (value) {
            let dataSoKhachToiDa = value;

            //set width for popup
            popup.style.width = "25%";

            //show popup and add title
            regulationClick(e.target);

            //content popup
            let contentPopupHTML = "<div class='information'>Số khách tối đa hiện tại: <b>" + dataSoKhachToiDa + "</b></div>" +
                "<div style='display: flex; justify-content: space-between;'>" +
                "<div>Số khách tối đa mới: </div>" +
                "<div class='buttonStep' minVal='1' >" +
                "<button class='subtractVal'>-</button>" +
                "<span>" + dataSoKhachToiDa +"</span>" +
                "<button class='addVal'>+</button>" +
                "</div>" +
                "</div>";

            popup.querySelector(".content-popup").innerHTML = contentPopupHTML;


            let buttonStep = popup.querySelector(".content-popup .buttonStep");
            addEventForButtonStep(buttonStep);

            let subtractVal = buttonStep.querySelector("button.subtractVal");
            let addVal = buttonStep.querySelector("button.addVal");
            let buttonSubmit = popup.querySelector(".footer-popup button.blue-button");

            buttonSubmit.disabled = true;
            subtractVal.addEventListener("click", () => {
                if (buttonStep.querySelector("span").innerHTML == dataSoKhachToiDa) {
                    buttonSubmit.disabled = true;
                    return;
                }
                buttonSubmit.disabled = false;
            })
            addVal.addEventListener("click", () => {
                if (buttonStep.querySelector("span").innerHTML == dataSoKhachToiDa) {
                    buttonSubmit.disabled = true;
                    return;
                }
                buttonSubmit.disabled = false;
            })

            buttonSubmit.addEventListener("click", () => {
                let soKhachToiDaNew = popup.querySelector(".content-popup .buttonStep span").innerHTML;
                updateSoKhachToiDa(soKhachToiDaNew).then(
                    function (value) {
                        if (value * 1 == 1) {
                            toastMessage({ title: 'Sửa thành công', message: 'Số khách tối đa được sửa thành: ' + soKhachToiDaNew, type: 'success', duration: 3500 });
                            closePopup();
                        } else {
                            if (value * 1 == 2) {
                                toastMessage({ title: 'Sửa thất bại', message: 'Tồn tại loại (phụ thu, hệ số phụ thu) có số khách áp dụng lớn hơn số khách tối đa mới', type: 'fail', duration: 3500 });
                            } else {
                                toastMessage({ title: 'Lỗi', message: 'Sửa không thành công.', type: 'fail', duration: 3500 });
                            }
                        }
                    }
                );
            });
        }, function (error) {
            toastMessage({ title: 'Lỗi kết nối', message: 'Lấy về số khách tối đa thất bại', type: 'fail', duration: 3500 });
        }
    );
    
});
//----3. so khach toi da: end

//----4. phu thu theo so khach: start

function DontHavePhuThuSoKhach(soKhachToiDa) {
    let contentPopupHTML = "<div class='information'>Chưa có loại phụ thu nào!</div>" +
        "<div class='add-new'>" +
        "<button class='blue-button'>+ Thêm mới</button>" +
        "</div>" +
        "<div class='history-link'>Lịch sử phụ thu</div>";
    popup.querySelector(".content-popup").innerHTML = contentPopupHTML;

    //add event for add new
    popup.querySelector(".content-popup .add-new .blue-button").addEventListener("click", () => {
        showSubPopup();
        updateAndAddPhuThuClick(null, false, soKhachToiDa);
    });
    //add event for add new
    popup.querySelector(".content-popup .history-link").addEventListener("click", () => {
        loadHisoryPopupChangePhuThu(0);
    });
}

function updatePhuThuSoKhach(soKhachApDung, tiLePhuThu, ngayApDung, maPhuThuSoKhach) {
    loadingElement.show();

    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/UpdatePhuThuSoKhach?soKhachApDung=" + soKhachApDung + "&tiLePhuThu=" + tiLePhuThu + "&ngayApDung=" + ngayApDung + "&maPhuThuSoKhach=" + maPhuThuSoKhach;
    xhr.timeout = 20000;
    xhr.open("GET", url, true);

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = () => {
            loadingElement.hide();
            if (xhr.readyState == 4 && xhr.status == 200) {
                let result = xhr.response;
                resolve(result);
            }
        }
        xhr.send();
    });
}

function insertPhuThuSoKhach(soKhachApDung, tiLePhuThu, thoiGianApDung) {
    loadingElement.show();

    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/InsertPhuThuSoKhach?soLuongApDung=" + soKhachApDung + "&tiLePhuThu=" + tiLePhuThu + "&thoiGianApDung=" + thoiGianApDung;
    xhr.timeout = 20000;
    xhr.open("GET", url, true);

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = () => {
            loadingElement.hide();
            if (xhr.readyState == 4 && xhr.status == 200) {
                let result = xhr.responseText;
                resolve(result);
            }
        }
        xhr.send();
    });
}

function updateAndAddPhuThuClick(item, isUpdate, soKhachToiDa) {
    //title header popup
    let titlePopup = subPopup.querySelector(".header-popup div");
    let nameBlueButton = subPopup.querySelector(".footer-popup .blue-button");
    titlePopup.innerHTML = "THÊM PHỤ THU";
    nameBlueButton.innerHTML = "Thêm";
    let isFuture = false;
    let listTD = null;

    if (isUpdate == true) {
        listTD = item.querySelectorAll("td");
        isFuture = item.classList.contains("future");
    }
    //defaul if insert 
    let defaultSoNguoiApDung = 1;
    let defaultThoiGianApDung = convertDDMMYYYYHHMMSStoDateTimeInputFormat(new Date().toLocaleString("en-GB"));

    let baseSoLuongApDung = "";
    let baseTiLePhuThu = "";
    let baseNgayApDung = "";

    let maPhuThu = "";
    if (isUpdate == true && isFuture) {
        maPhuThu = "maphuthu='" + item.getAttribute("maphuthusokhach") + "'";
        defaultSoNguoiApDung = listTD[1].innerHTML;
        baseSoLuongApDung = defaultSoNguoiApDung;
        defaultThoiGianApDung = convertDDMMYYYYHHMMSStoDateTimeInputFormat(listTD[3].innerHTML);
        baseNgayApDung = defaultThoiGianApDung;

    }
    //content sub popup
    let changeSoKhachApDungElement = "<tr>" +
        "<td class='label'>Số người áp dụng: </td>" +
        "<td class='entry'>" +
        "<div class='buttonStep' minVal='1' maxVal='" + soKhachToiDa + "'>" +
        "<button class='subtractVal'>-</button>" +
        "<span>" + defaultSoNguoiApDung + "</span>" +
        "<button class='addVal'>+</button>" +
        "</div>" +
        "</td>" +
        "</tr>";
    let ngayApDungElement = "<tr>" +
        "<td class='label'>Ngày áp dụng: </td>" +
        "<td class='entry'>" +
        "<input class='input-check' type='datetime-local' value='" + defaultThoiGianApDung + "'>" +
        "</td>" +
        "</tr>";
    if (isUpdate == true && !isFuture) {
        changeSoKhachApDungElement = "";
        ngayApDungElement = "";
    }
    // only update
    let valueSoKhachApDung = "";
    let valueTiLePhuThu = "";
    let information = "<div class ='information'>Số khách tối đa: <b>" + soKhachToiDa + "</b></div>";

    if (isUpdate == true) {
        titlePopup.innerHTML = "SỬA PHỤ THU";
        nameBlueButton.innerHTML = "Sửa";

        valueSoKhachApDung = "soluongapdung='" + listTD[1].innerHTML + "'";
        baseTiLePhuThu = listTD[2].innerHTML;
        valueTiLePhuThu = "value='" + baseTiLePhuThu + "'";
        if (!isFuture) {
            information = "<div class ='information'>Số khách áp dụng: <b>" + listTD[1].innerHTML + "</b></div>";
        }
    }

    let subPopupContentHTML = information +
                                "<div class='table-container'>" +
                                    "<table " + valueSoKhachApDung + " " + maPhuThu + ">" + 
                                        changeSoKhachApDungElement +
                                        "<tr>" + 
                                            "<td class='label'>Tỉ lệ phụ thu (%): </td>" +
                                            "<td class='entry'>" +
                                                "<input class='ti-le-phu-thu' type='number' placeholder='Tỉ lệ phụ thu' " + valueTiLePhuThu + ">" +
                                            "</td>" +
                                        "</tr>" +
                                        ngayApDungElement +
                                    "</table>" + 
                                "</div>";
    subPopup.querySelector(".content-popup").innerHTML = subPopupContentHTML;


    let inputTiLePhuThu = subPopup.querySelector(".content-popup .table-container table input.ti-le-phu-thu");
    let inputNgayApDung = subPopup.querySelector(".input-check");

    let buttonStep = subPopup.querySelector(".table-container table .buttonStep");
    let buttonSubmit = subPopup.querySelector(".footer-popup .blue-button");
    buttonSubmit.disabled = false;


    if (!(isUpdate == true && !isFuture)) {
        addEventForButtonStep(subPopup.querySelector(".table-container table .buttonStep"));
        inputNgayApDung.addEventListener("input", (e) => {
            if (new Date(e.target.value) < new Date()) {
                e.target.value = convertDDMMYYYYHHMMSStoDateTimeInputFormat(new Date().toLocaleString("en-GB"));
                toastMessage({ title: 'Lỗi', message: 'Thời gian áp dụng phải lớn hơn hoặc bằng hiện tại', type: 'fail', duration: 3500 });
            }
        });
    }

    if (isUpdate == true && isFuture) {

        let subtractVal = buttonStep.querySelector("button.subtractVal");
        let addVal = buttonStep.querySelector("button.addVal");

        buttonSubmit.disabled = true;
        subtractVal.addEventListener("click", () => {
            if (buttonStep.querySelector("span").innerHTML == baseSoLuongApDung &&
                inputTiLePhuThu.value == baseTiLePhuThu &&
                inputNgayApDung.value == baseNgayApDung) {
                buttonSubmit.disabled = true;
                return;
            }
            buttonSubmit.disabled = false;
        })
        addVal.addEventListener("click", () => {
            if (buttonStep.querySelector("span").innerHTML == baseSoLuongApDung &&
                inputTiLePhuThu.value == baseTiLePhuThu &&
                inputNgayApDung.value == baseNgayApDung) {
                buttonSubmit.disabled = true;
                return;
            }
            buttonSubmit.disabled = false;
        })

        inputNgayApDung.addEventListener("input", () => {
            if (buttonStep.querySelector("span").innerHTML == baseSoLuongApDung &&
                inputTiLePhuThu.value == baseTiLePhuThu &&
                inputNgayApDung.value == baseNgayApDung) {
                buttonSubmit.disabled = true;
                return;
            }
            buttonSubmit.disabled = false;
        })
    }

    if (isUpdate == true) {
        buttonSubmit.disabled = true;
        inputTiLePhuThu.addEventListener("input", () => {
            if ((buttonStep == null || buttonStep.querySelector("span").innerHTML == baseSoLuongApDung) &&
                inputTiLePhuThu.value == baseTiLePhuThu &&
                (inputNgayApDung == null || inputNgayApDung.value == baseNgayApDung)) {
                buttonSubmit.disabled = true;
                return;
            }
            buttonSubmit.disabled = false;
        })
    }

    //add event for change button
    subPopup.querySelector(".footer-popup .blue-button").addEventListener("click", (e) => {
        let soKhachApDung = subPopup.querySelector(".content-popup .table-container table").getAttribute("soluongapdung");
        let maPhuThu = subPopup.querySelector(".content-popup .table-container table").getAttribute("maphuthu");
        let tiLePhuThu = subPopup.querySelector(".content-popup .table-container table input.ti-le-phu-thu").value;
        let thoiGianApDung = "";
        if (!(isUpdate == true && !isFuture)) {
            soKhachApDung = subPopup.querySelector(".content-popup .table-container table .buttonStep span").innerHTML;
            thoiGianApDung = subPopup.querySelector(".content-popup .table-container table input.input-check").value.replace(/T/g, " ");
        }
        if (tiLePhuThu == 0) {
            toastMessage({ title: 'Lỗi', message: 'Trường tỉ lệ phụ thu không được phép trống hoặc bằng 0!', type: 'fail', duration: 3500 });
        } else {
            if (isUpdate == true) {
                updatePhuThuSoKhach(soKhachApDung, tiLePhuThu, thoiGianApDung, maPhuThu).then(
                    function (value) {
                        if (value == "true") {
                            loadPopupChangePhuThu(soKhachToiDa);
                            closeSubPopup();
                        } else {
                            toastMessage({ title: 'Sửa thất bại', message: 'Đã có phụ thu ' + soKhachApDung + ' người trong hệ thống', type: 'fail', duration: 3500 });
                        }
                    }
                );
            } else {
                insertPhuThuSoKhach(soKhachApDung, tiLePhuThu, thoiGianApDung).then(
                    function (value) {

                        if (value == "true") {
                            loadPopupChangePhuThu(soKhachToiDa);
                            closeSubPopup();
                        } else {
                            toastMessage({ title: 'Thêm thất bại', message: 'Đã có phụ thu ' + soKhachApDung + ' người trong hệ thống', type: 'fail', duration: 3500 });
                        }
                    }, function (value) {
                        toastMessage({ title: 'Lỗi kết nối', message: 'Thêm phụ thu thất bại', type: 'fail', duration: 3500 });
                    }
                );
            }

        }
    })
}

function getPhuThuSoKhach() {

    loadingElement.show();

    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/GetPhuThuSoKhach";
    xhr.timeout = 20000;
    xhr.open("GET", url, true);

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = () => {
            loadingElement.hide();
            if (xhr.readyState == 4 && xhr.status == 200) {
                let result = JSON.parse(xhr.responseText);
                resolve(result);
            }
        }
        xhr.send();
    });
}

function deletePhuThuConfirm(soLuongApDung, ngayApdung, maPhuThuSoKhach) {

    ngayApdung = convertDDMMYYYYHHMMSStoDateTimeInputFormat(ngayApdung);
    loadingElement.show();
    let xhr_delete_loaiphong = new XMLHttpRequest();
    let url_delete_loaiphong = "https://localhost:5001/Regulation/DeletePhuThuSoKhach?soKhachApDung=" + soLuongApDung + "&ngayApdung=" + ngayApdung + "&maPhuThuSoKhach=" + maPhuThuSoKhach;
    xhr_delete_loaiphong.open("GET", url_delete_loaiphong, true);
    xhr_delete_loaiphong.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr_delete_loaiphong.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr_delete_loaiphong.readyState == 4 && xhr_delete_loaiphong.status == 200) {

                let result = xhr_delete_loaiphong.responseText;
                resolve(result);
            }
        }
        xhr_delete_loaiphong.send();
    });
}

let dataSave = null;
function checkChangeAfterInsertUpdate(maPhuThu, soLuongApDung, tiLePhuThu, thoiGianApDung) {
    let checkChange = true;
    if (dataSave == null) {
        return false;
    }
    dataSave.forEach(ds => {
        if (ds["maPhuThu"] == maPhuThu
            && ds["soLuongApDung"] == soLuongApDung
            && ds["tiLePhuThu"] == tiLePhuThu
            && ds["thoiGianApDung"] == thoiGianApDung) {
            checkChange = false;
            return;
        }
    })
    return checkChange;
}

function loadPopupChangePhuThu(soKhachToiDa) {
    getPhuThuSoKhach().then(
        function (dtpt) {
            let dataPhuThu = dtpt;

            regulationClick(listRegulation[3]);

            //set width for popup
            popup.style.width = "60%";

            if (dataPhuThu == null || dataPhuThu.length == 0) {
                DontHavePhuThuSoKhach(soKhachToiDa);
                return;
            }

            //content popup
            let contentPopupHTML = "<div class ='information'>Số khách áp dụng phải nhỏ hơn số khách tối đa: <b>" + soKhachToiDa + "</b></div>" +
                "<table>" +
                "<tr class='header-table'>" +
                "<td>STT</td>" +
                "<td>Số khách áp dụng</td>" +
                "<td>Tỉ lệ phụ thu (%)</td>" +
                "<td>Ngày áp dụng</td>" +
                "<td></td>" +
                "</tr>";

            //render loai phong
            let count = 1;
            dataPhuThu.forEach(PT => {
                let date = new Date(PT["thoiGianApDung"]);
                let isFuture = "";
                let maPhuThuSoKhach = "";
                if (date > new Date()) {
                    isFuture = "future";
                    maPhuThuSoKhach = "maphuthusokhach='" + PT["maPhuThu"] + "'";
                }

                let justAdd = "";
                if (checkChangeAfterInsertUpdate(PT["maPhuThu"], PT["soLuongApDung"], PT["tiLePhuThu"], PT["thoiGianApDung"])) {
                    if (isFuture == "future") {
                        justAdd = "future-just-add";
                    } else {
                        justAdd = "just-add";

                    }
                }
                
                contentPopupHTML += "<tr class='item-table " + isFuture + " " + justAdd + "' " + maPhuThuSoKhach + ">" +
                    "<td>" + count + "</td>" +
                    "<td>" + PT["soLuongApDung"] + "</td>" +
                    "<td>" + PT["tiLePhuThu"] + "</td>" +
                    "<td>" + (date).toLocaleString('en-GB') + "</td>" +
                    "<td>" +
                    "<div>" +
                    "<button class='green-button'>Sửa</button>" +
                    "<button class='red-button'>Xóa</button>" +
                    "</div>" +
                    "</td>" +
                    "</tr>";
                count++;
            });

            //footer popup
            contentPopupHTML += "</table>" +
                "<div class='add-new'>" +
                "<button class='blue-button'>+ Thêm mới</button>" +
                "</div>" +
                "<div class='history-link'>Lịch sử phụ thu</div>";
            popup.querySelector(".content-popup").innerHTML = contentPopupHTML;
            //add event for update button
            let listUpdateRoomButton = popup.querySelectorAll(".content-popup table td button.green-button");
            listUpdateRoomButton.forEach((UB) => {
                UB.addEventListener('click', () => {
                    showSubPopup();

                    //pass ITEM-TABLE_LOAIPHONG to sub popup
                    updateAndAddPhuThuClick(UB.parentElement.parentElement.parentElement, true, soKhachToiDa);
                });
            });

            //add event for delete button
            let listDeleteRoomButton = popup.querySelectorAll(".content-popup table td .red-button");
            listDeleteRoomButton.forEach(DRB => {
                DRB.addEventListener("click", () => {
                    showConfirmPopup();

                    //pass message and update title 
                    confirmPopup.querySelector(".header-popup div").innerHTML = "XÓA PHỤ THU";
                    confirmPopup.querySelector(".content-popup .message").innerHTML = "Bạn có chắc chắn muốn xóa phụ thu này?";

                    let confirmButton = confirmPopup.querySelector(".footer-popup .blue-button");
                    confirmButton.addEventListener('click', () => {
                        let itemTable = DRB.parentElement.parentElement.parentElement;
                        let listTD = itemTable.querySelectorAll("td");
                        deletePhuThuConfirm(listTD[1].innerHTML, listTD[3].innerHTML, itemTable.getAttribute("maphuthusokhach")).then(
                            function (value) {
                                closeConfirmPopup();
                                if (value == "true") {
                                    loadPopupChangePhuThu(soKhachToiDa);
                                    toastMessage({ title: 'Xóa thành công', message: 'Xóa phụ thu thành công', type: 'success', duration: 3500 });
                                }
                                else {
                                    toastMessage({ title: 'Lỗi', message: 'Xóa phụ thu thất bại', type: 'fail', duration: 3500 });
                                }
                            }
                        );
                    });
                });
            });

            //add event for add new
            popup.querySelector(".content-popup .add-new .blue-button").addEventListener("click", () => {
                showSubPopup();
                updateAndAddPhuThuClick(null, false, soKhachToiDa);
            });
            popup.querySelector(".content-popup .history-link").addEventListener("click", () => {
                loadHisoryPopupChangePhuThu(0);
            });

            dataSave = dtpt;

        }, function (error) {
            toastMessage({ title: 'Lỗi kết nối', message: 'Lấy danh sách phụ thu thất bại', type: 'fail', duration: 3500 });
        }
    );
}

listRegulation[3].addEventListener("click", () => {

    HIDEbuttonChangeInPopup();

    getSoKhachToiDa().then(
        function (sktd) {
            let soKhachToiDa = sktd;

            loadPopupChangePhuThu(soKhachToiDa);
        }, function (error) {
            toastMessage({ title: 'Lỗi kết nối', message: 'Lấy thông tin số khách tối đa thất bại', type: 'fail', duration: 3500 });
        }
    );
    
});


function getLichSuPhuThuSoKhach() {

    loadingElement.show();

    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/GetLichSuPhuThuSoKhach";
    xhr.timeout = 20000;
    xhr.open("GET", url, true);

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = () => {
            loadingElement.hide();
            if (xhr.readyState == 4 && xhr.status == 200) {
                let result = JSON.parse(xhr.responseText);
                resolve(result);
            }
        }
        xhr.send();
    });
}

function loadHisoryPopupChangePhuThu(soLuongApDung) {
    getLichSuPhuThuSoKhach().then(
        function (dtpt) {
            let dataPhuThu = dtpt;
            if (soLuongApDung != 0) {
                let dataNew = [];
                dataPhuThu.forEach(PT => {
                    if (PT["soLuongApDung"] == soLuongApDung) {
                        dataNew.push(PT);
                    }
                })
                dataPhuThu = dataNew;
            }
            showHistoryPopup();
            //title in header popup
            historyPopup.querySelector(".header-popup div").innerHTML = "LỊCH SỬ PHỤ THU THEO SỐ KHÁCH";

            //set width for popup
            historyPopup.style.width = "40%";

            let contentPopupHTML = "<div class ='filter'>" +
                "<div class='so-luong-ap-dung'>" +
                "<div style='margin-right: 10px;'>Số khách áp dụng:</div>" +
                "<div class='buttonStep' minVal='1'>" +
                "<button class='subtractVal'>-</button>" +
                "<span>" + ((soLuongApDung == 0) ? 1 : soLuongApDung) + "</span>" +
                "<button class='addVal'>+</button>" +
                "</div>" +
                "</div>" +
                "<div class='validate'>" +
                "<button class='blue-button'>Lọc</button><button class='cancel-button'>Xóa bộ lọc</button>" +
                "</div>" +
                "</div> ";

            if (dataPhuThu == null || dataPhuThu.length == 0) {
                historyPopup.querySelector(".content-popup").innerHTML = contentPopupHTML + "<div class ='information'>Chưa có phụ thu nào</div>";
                addEventForButtonStep(historyPopup.querySelector(".buttonStep"));
                let filterClick = historyPopup.querySelector(".validate .blue-button");
                filterClick.addEventListener("click", () => {
                    loadHisoryPopupChangePhuThu(historyPopup.querySelector(".buttonStep span").innerHTML * 1);
                });

                let cancelFilterClick = historyPopup.querySelector(".validate .cancel-button");
                cancelFilterClick.addEventListener("click", () => {
                    loadHisoryPopupChangePhuThu(0);
                });
                return;
            }

            //content popup
            contentPopupHTML += "<table>" +
                "<tr class='header-table'>" +
                "<td>STT</td>" +
                "<td>Số khách áp dụng</td>" +
                "<td>Tỉ lệ phụ thu (%)</td>" +
                "<td>Ngày áp dụng</td>" +
                "</tr>";

            //render loai phong
            let count = 1;
            dataPhuThu.forEach(PT => {
                let date = new Date(PT["thoiGianApDung"]);
                let isFuture = "";
                let maPhuThuSoKhach = "";
                if (date > new Date()) {
                    isFuture = "future";
                    maPhuThuSoKhach = "maphuthusokhach='" + PT["maPhuThu"] + "'";
                }

                contentPopupHTML += "<tr class='item-table " + isFuture + "' " + maPhuThuSoKhach + ">" +
                    "<td>" + count + "</td>" +
                    "<td>" + PT["soLuongApDung"] + "</td>" +
                    "<td>" + PT["tiLePhuThu"] + "</td>" +
                    "<td>" + (date).toLocaleString('en-GB') + "</td>" +
                    "</tr>";
                count++;
            });

            //footer popup
            contentPopupHTML += "</table>";
            historyPopup.querySelector(".content-popup").innerHTML = contentPopupHTML;
            addEventForButtonStep(historyPopup.querySelector(".buttonStep"));
            let filterClick = historyPopup.querySelector(".validate .blue-button");
            filterClick.addEventListener("click", () => {
                loadHisoryPopupChangePhuThu(historyPopup.querySelector(".buttonStep span").innerHTML * 1);
            });

            let cancelFilterClick = historyPopup.querySelector(".validate .cancel-button");
            cancelFilterClick.addEventListener("click", () => {
                loadHisoryPopupChangePhuThu(0);
            });
        }, function (error) {
            toastMessage({ title: 'Lỗi kết nối', message: 'Lấy danh sách phụ thu thất bại', type: 'fail', duration: 3500 });
        }
    );
}
//----4. phu thu theo so khach: end


//----5. he so phu thu: start

function updateHeSoPhuThu(maLoaiKhachHang, soLuongApDung, heSoPhuThu, ngayApDung, maHeSoPhuThu) {
    loadingElement.show();
    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/UpdateHeSoPhuThu?maLoaiKhachHang=" + maLoaiKhachHang +
        "&soLuongApDung=" + soLuongApDung +
        "&heSoPhuThu=" + heSoPhuThu +
        "&ngayApDung=" + ngayApDung +
        "&maHeSoPhuThu=" + maHeSoPhuThu;
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = xhr.responseText;
                resolve(result);
            }
        }
        xhr.send();
    });
}

function insertHeSoPhuThu(maLoaiKhachHang, soLuongApDung, heSoPhuThu, ngayApDung) {
    loadingElement.show();
    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/InsertHeSoPhuThu?maLoaiKhachHang=" + maLoaiKhachHang +
        "&soLuongApDung=" + soLuongApDung +
        "&heSoPhuThu=" + heSoPhuThu +
        "&ngayApDung=" + ngayApDung;
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = xhr.responseText;
                resolve(result);
            }
        }
        xhr.send();
    });
}

function updateAndAddHeSoPhuThuClick(item, isUpdate, soKhachToiDa, maLoaiKhach) {

    getLoaiKhachHang().then(
        function (dtlkh) {

            let dataLoaiKhachHang = dtlkh;

            if ((dataLoaiKhachHang == null || dataLoaiKhachHang.length == 0) && !isUpdate) {
                toastMessage({ title: 'Không thể thêm', message: 'Chưa có loại khách hàng nào để áp dụng.', type: 'fail', duration: 3500 });
                closeSubPopup();
                return;
            }

            //title header popup
            let titlePopup = subPopup.querySelector(".header-popup div");
            let nameBlueButton = subPopup.querySelector(".footer-popup .blue-button");
            titlePopup.innerHTML = "THÊM HỆ SỐ PHỤ THU";
            nameBlueButton.innerHTML = "Thêm";

            let isFuture = false;
            let listTD = null;
            let baseLoaiKhachHang = maLoaiKhach;
            let baseSoLuongApDung= "";
            let baseHeSoPhuThu= "";
            let baseNgayApDung= "";

            if (isUpdate == true) {
                listTD = item.querySelectorAll("td.content-item");
                isFuture = item.classList.contains("future");
            }

            //default if insert
            let defaultSoNguoiApDung = 1;
            let defaultThoiGianApDung = convertDDMMYYYYHHMMSStoDateTimeInputFormat(new Date().toLocaleString("en-GB"));

            let maHeSoPhuThu = "";
            let isDisable = "";
            if (isUpdate == true && isFuture) {
                maHeSoPhuThu = "mahesophuthu='" + item.getAttribute("mahesophuthu") + "'";
                defaultSoNguoiApDung = listTD[0].innerHTML;
                baseSoLuongApDung = defaultSoNguoiApDung;
                defaultThoiGianApDung = convertDDMMYYYYHHMMSStoDateTimeInputFormat(listTD[2].innerHTML);
                baseNgayApDung = defaultThoiGianApDung;
            }
            if (isUpdate == true && !isFuture) {
                isDisable = "disabled";
            }
            //content sub popup
            let selectElement = "";
            selectElement += "<select " + isDisable + " >";
            dataLoaiKhachHang.forEach(LKH => {
                let isSelected = "";
                if (isUpdate == true && maLoaiKhach == LKH["maLoaiKhachHang"]) {
                    isSelected = "selected";
                }
                selectElement += "<option " + isSelected + " value='" + LKH["maLoaiKhachHang"] + "'>" + LKH["tenLoaiKhachHang"] + "</option>";
            });
            selectElement += "</select>";

            let informationElement = "<div class ='information'>Số khách tối đa: <b>" + soKhachToiDa + "</b></div>";
            let ngayApDungElement = "<tr>" +
                "<td class='label'>Ngày áp dụng: </td>" +
                "<td class='entry'>" +
                "<input class='input-check' type='datetime-local' value='" + defaultThoiGianApDung + "'>" +
                "</td>" +
                "</tr>";
            let soNguoiApDungElement = "<tr>" +
                "<td class='label'>Số người áp dụng: </td>" +
                "<td class='entry'>" +
                "<div class='buttonStep' minVal='1' maxVal='" + soKhachToiDa + "'>" +
                "<button class='subtractVal'>-</button>" +
                "<span>" + defaultSoNguoiApDung + "</span>" +
                "<button class='addVal'>+</button>" +
                "</div>" +
                "</td>" +
                "</tr>";
            if (isUpdate == true && !isFuture) {
                ngayApDungElement = "";
                soNguoiApDungElement = "";
                informationElement = "<div class ='information'>Số khách áp dụng: <b>" + listTD[0].innerHTML + "</b></div>"
            }

            let valueHeSoPhuThu = "";
            let soLuongApDung = "";

            if (isUpdate == true) {
                titlePopup.innerHTML = "SỬA HỆ SỐ PHỤ THU";
                nameBlueButton.innerHTML = "Sửa";
                soLuongApDung = listTD[0].innerHTML;

                baseHeSoPhuThu = listTD[1].innerHTML;
                valueHeSoPhuThu = "value='" + baseHeSoPhuThu + "'";
            }

            let subPopupContentHTML = informationElement +
                "<div class='table-container'>" +
                "<table " + maHeSoPhuThu + " soluongapdung='" + soLuongApDung +"'>" +
                "<tr>" +
                "<td class='label'>Loại khách: </td>" +
                "<td class='entry'>" +
                selectElement +
                "</td>" +
                "</tr>" +
                soNguoiApDungElement +
                "<tr>" +
                "<td class='label'>Hệ số phụ thu: </td>" +
                "<td class='entry'>" +
                "<input class='he-so-phu-thu' type='number' placeholder='Hệ số phụ thu' " + valueHeSoPhuThu + ">" +
                "</td>" +
                "</tr>" +
                ngayApDungElement +
                "</table>" +
                "</div>";
            subPopup.querySelector(".content-popup").innerHTML = subPopupContentHTML;

            let inputHeSoPhuThu = subPopup.querySelector(".content-popup .table-container table input.he-so-phu-thu");
            let selectLoaiKhachHang = subPopup.querySelector("select");
            let buttonStep = subPopup.querySelector(".content-popup .table-container table .buttonStep");
            let inputNgayApDung = subPopup.querySelector(".content-popup .table-container table input.input-check");

            let buttonSubmit = subPopup.querySelector(".footer-popup .blue-button");
            buttonSubmit.disabled = false;


            if (!(isUpdate == true && !isFuture)) {
                addEventForButtonStep(subPopup.querySelector(".table-container table .buttonStep"));
                subPopup.querySelector(".input-check").addEventListener("input", (e) => {
                    if (new Date(e.target.value) < new Date()) {
                        e.target.value = convertDDMMYYYYHHMMSStoDateTimeInputFormat(new Date().toLocaleString("en-GB"));
                        toastMessage({ title: 'Lỗi', message: 'Thời gian áp dụng phải lớn hơn hoặc bằng hiện tại', type: 'fail', duration: 3500 });
                    }
                });
            }

            if (isUpdate == true && isFuture) {

                let subtractVal = buttonStep.querySelector("button.subtractVal");
                let addVal = buttonStep.querySelector("button.addVal");

                buttonSubmit.disabled = true;
                selectLoaiKhachHang.addEventListener("input", () => {
                    if (buttonStep.querySelector("span").innerHTML == baseSoLuongApDung &&
                        inputHeSoPhuThu.value == baseHeSoPhuThu &&
                        selectLoaiKhachHang.value == baseLoaiKhachHang &&
                        inputNgayApDung.value == baseNgayApDung) {
                        buttonSubmit.disabled = true;
                        return;
                    }
                    buttonSubmit.disabled = false;
                })

                subtractVal.addEventListener("click", () => {
                    if (buttonStep.querySelector("span").innerHTML == baseSoLuongApDung &&
                        inputHeSoPhuThu.value == baseHeSoPhuThu &&
                        selectLoaiKhachHang.value == baseLoaiKhachHang &&
                        inputNgayApDung.value == baseNgayApDung) {
                        buttonSubmit.disabled = true;
                        return;
                    }
                    buttonSubmit.disabled = false;
                })
                addVal.addEventListener("click", () => {
                    if (buttonStep.querySelector("span").innerHTML == baseSoLuongApDung &&
                        inputHeSoPhuThu.value == baseHeSoPhuThu &&
                        selectLoaiKhachHang.value == baseLoaiKhachHang &&
                        inputNgayApDung.value == baseNgayApDung) {
                        buttonSubmit.disabled = true;
                        return;
                    }
                    buttonSubmit.disabled = false;
                })

                inputNgayApDung.addEventListener("input", () => {
                    if (buttonStep.querySelector("span").innerHTML == baseSoLuongApDung &&
                        inputHeSoPhuThu.value == baseHeSoPhuThu &&
                        selectLoaiKhachHang.value == baseLoaiKhachHang &&
                        inputNgayApDung.value == baseNgayApDung) {
                        buttonSubmit.disabled = true;
                        return;
                    }
                    buttonSubmit.disabled = false;
                })
            }

            if (isUpdate == true) {
                buttonSubmit.disabled = true;
                inputHeSoPhuThu.addEventListener("input", () => {
                    if ((buttonStep == null || buttonStep.querySelector("span").innerHTML == baseSoLuongApDung) &&
                        inputHeSoPhuThu.value == baseHeSoPhuThu &&
                        (selectLoaiKhachHang == null || selectLoaiKhachHang.value == baseLoaiKhachHang) &&
                        (inputNgayApDung == null || inputNgayApDung.value == baseNgayApDung)) {
                        buttonSubmit.disabled = true;
                        return;
                    }
                    buttonSubmit.disabled = false;
                })
            }

            //add event for change button
            buttonSubmit.addEventListener("click", (e) => {
                let soLuongApDung = subPopup.querySelector(".content-popup .table-container table").getAttribute("soluongapdung");
                let maHeSoPhuThu = subPopup.querySelector(".content-popup .table-container table").getAttribute("mahesophuthu");
                let heSoPhuThu = inputHeSoPhuThu.value.replace(/,/g, "");
                let thoiGianApDung = "";
                let maLoaiKhachHang = maLoaiKhach;
                if (!(isUpdate == true && !isFuture)) {
                    maLoaiKhachHang = selectLoaiKhachHang.value;
                    soLuongApDung = subPopup.querySelector(".content-popup .table-container table .buttonStep span").innerHTML;
                    thoiGianApDung = inputNgayApDung.value.replace(/T/g, " ");
                }
                if (heSoPhuThu == 0) {
                    toastMessage({ title: 'Lỗi', message: 'Trường hệ số phụ thu không được phép trống hoặc bằng 0.', type: 'fail', duration: 3500 });
                } else {
                    if (isUpdate == true) {
                        updateHeSoPhuThu(maLoaiKhachHang, soLuongApDung, heSoPhuThu, thoiGianApDung, maHeSoPhuThu).then(
                            function (value) {
                                if (value == "true") {
                                    loadChangeHeSoPhuThu(soKhachToiDa);
                                    closeSubPopup();
                                } else {
                                    toastMessage({ title: 'Sửa thất bại', message: 'Đã có phụ thu tương tự trong hệ thống', type: 'fail', duration: 3500 });
                                }
                            }
                        );
                    } else {
                        insertHeSoPhuThu(subPopup.querySelector("select").value, soLuongApDung, heSoPhuThu, thoiGianApDung).then(
                            function (value) {
                                if (value == "true") {
                                    loadChangeHeSoPhuThu(soKhachToiDa);
                                    closeSubPopup();
                                } else {
                                    toastMessage({ title: 'Thêm thất bại', message: 'Đã có phụ thu tương tự trong hệ thống', type: 'fail', duration: 3500 });
                                }
                            }
                        );
                    }

                }
            })
        }, function (error) {
            toastMessage({ title: 'Lỗi kết nối', message: 'Lấy thông tin loại khách hàng không thành công', type: 'fail', duration: 3500 });
        }
    );
}

function getHeSoPhuThu() {
    loadingElement.show();
    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/GetPhuThuLoaiKhachHang";
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = JSON.parse(xhr.responseText);
                resolve(result);
            }
        }
        xhr.send();
    });
}

function deleteHeSoPhuThuConfirm(maLoaiKhachHang, soLuongApDung, ngayApDung, maPhuThuLKH) {
    console.log(ngayApDung);
    ngayApDung = convertDDMMYYYYHHMMSStoDateTimeInputFormat(ngayApDung);

    loadingElement.show();
    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/DeleteHeSoPhuThu?maLoaiKhachHang=" + maLoaiKhachHang +
        "&soLuongApDung=" + soLuongApDung +
        "&ngayApDung=" + ngayApDung +
        "&maPhuThuLKH=" + maPhuThuLKH;
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = xhr.responseText;
                resolve(result);
            }
        }
        xhr.send();
    });
}

function DontHaveHeSoPhuThu(soKhachToiDa) {
    let contentPopupHTML = "<div class='information'>Chưa có hệ số phụ thu nào!</div>" +
        "<div class='add-new'>" +
        "<button class='blue-button'>+ Thêm mới</button>" +
        "</div>" + 
        "<div class='history-link'>Lịch sử phụ thu</div>";
    popup.querySelector(".content-popup").innerHTML = contentPopupHTML;

    popup.querySelector(".content-popup .add-new .blue-button").addEventListener("click", () => {
        showSubPopup();
        updateAndAddHeSoPhuThuClick(null, false, soKhachToiDa, null);
    });

    popup.querySelector(".content-popup .history-link").addEventListener("click", () => {
        loadLichSuChangeHeSoPhuThu(0, 0);
    });
}
let dataHeSoSave = null;
function checkChangeAfterInsertUpdateHeSoPhuThu(maPhuThuLKH, soLuongApDung, tenLoaiKhachHang, maLoaiKhachHang, heSoPhuThu, thoiGianApDung) {
    let checkChange = true;
    if (dataHeSoSave == null) {
        return false;
    }
    dataHeSoSave.forEach(ds => {
        if (ds["maPhuThuLKH"] == maPhuThuLKH
            && ds["soLuongApDung"] == soLuongApDung
            && ds["tenLoaiKhachHang"] == tenLoaiKhachHang
            && ds["maLoaiKhachHang"] == maLoaiKhachHang
            && ds["heSoPhuThu"] == heSoPhuThu
            && ds["thoiGianApDung"] == thoiGianApDung) {
            checkChange = false;
            return;
        }
    })
    return checkChange;
}

function loadChangeHeSoPhuThu(soKhachToiDa){
    getHeSoPhuThu().then(
        function (dths) {
            let dataHeSoPhuThu = dths;

            regulationClick(listRegulation[4]);

            //set width for popup
            popup.style.width = "60%";

            if (dataHeSoPhuThu == null || dataHeSoPhuThu.length == 0) {
                DontHaveHeSoPhuThu(soKhachToiDa);
                return;
            }

            //content popup
            let contentPopupHTML = "<div class ='information'>Số khách áp dụng phải nhỏ hơn số khách tối đa: <b>" + soKhachToiDa + "</b></div>" +
                "<table>" +
                "<tr class='header-table'>" +
                "<td>Loại khách</td>" +
                "<td>Số khách áp dụng</td>" +
                "<td>Hệ số phụ thu </td>" +
                "<td>Ngày áp dụng</td>" +
                "<td></td>" +
                "</tr>";

            //render loai phong
            let count = 0;
            let items = "";
            let oneTypeOfLKH = "";
            let firstItemofType= "";
            let firstIsFuture = "";
            let firstIsJustAdd = "";
            let firstMaPTLKH = "";
            let lengthArr = dataHeSoPhuThu.length;

            for (let i = 0; i < lengthArr; i++) {
                let date = new Date(dataHeSoPhuThu[i]["thoiGianApDung"]);
                let isFuture = "";

                let maHeSoPhuThuSoKhach = "";

                if (date > new Date()) {
                    isFuture = "future";
                    maHeSoPhuThuSoKhach = "mahesophuthu='" + dataHeSoPhuThu[i]["maPhuThuLKH"] + "'";
                }

                let justAdd = "";
                if (checkChangeAfterInsertUpdateHeSoPhuThu(dataHeSoPhuThu[i]["maPhuThuLKH"],
                    dataHeSoPhuThu[i]["soLuongApDung"],
                    dataHeSoPhuThu[i]["tenLoaiKhachHang"],
                    dataHeSoPhuThu[i]["maLoaiKhachHang"],
                    dataHeSoPhuThu[i]["heSoPhuThu"],
                    dataHeSoPhuThu[i]["thoiGianApDung"])) {
                    if (isFuture == "future") {
                        justAdd = "future-just-add";
                    } else {
                        justAdd = "just-add";

                    }
                }
                count++;
                if (count == 1) {
                    firstItemofType= ">" + dataHeSoPhuThu[i]["tenLoaiKhachHang"] + "</td>" +
                        "<td class='content-item'>" + dataHeSoPhuThu[i]["soLuongApDung"] + "</td>" +
                        "<td class='content-item'>" + dataHeSoPhuThu[i]["heSoPhuThu"] + "</td>" +
                        "<td class='content-item'>" + (date).toLocaleString('en-GB') + "</td>" +
                        "<td>" +
                        "<div>" +
                        "<button class='green-button' maloaikhachhang='" + dataHeSoPhuThu[i]["maLoaiKhachHang"] + "'>Sửa</button>" +
                        "<button class='red-button'  maloaikhachhang='" + dataHeSoPhuThu[i]["maLoaiKhachHang"] + "'>Xóa</button>" +
                        "</div>" +
                        "</td>" +
                        "</tr>";

                    firstIsFuture = isFuture;
                    firstIsJustAdd = justAdd;
                    firstMaPTLKH = maHeSoPhuThuSoKhach;
                }
                else {
                    oneTypeOfLKH += "<tr class='item-table " + isFuture + " " + justAdd + "' " + maHeSoPhuThuSoKhach + " >" +
                        "<td class='content-item'>" + dataHeSoPhuThu[i]["soLuongApDung"] + "</td>" +
                        "<td class='content-item'>" + dataHeSoPhuThu[i]["heSoPhuThu"] + "</td>" +
                            "<td class='content-item'>" + (date).toLocaleString('en-GB') + "</td>" +
                        "<td>" +
                        "<div>" +
                        "<button class='green-button' maloaikhachhang='" + dataHeSoPhuThu[i]["maLoaiKhachHang"] + "'>Sửa</button>" +
                        "<button class='red-button' maloaikhachhang='" + dataHeSoPhuThu[i]["maLoaiKhachHang"] + "'>Xóa</button>" +
                        "</div>" +
                        "</td>" +
                        "</tr>"
                }

                if (i != lengthArr - 1) {
                    if (dataHeSoPhuThu[i]["maLoaiKhachHang"] != dataHeSoPhuThu[i + 1]["maLoaiKhachHang"]) {
                        oneTypeOfLKH = "<tr class='item-table " + firstIsFuture + " " + firstIsJustAdd + "' " + firstMaPTLKH + "'> <td style='background-color: #fff!important' rowspan='" + count + "'" + firstItemofType + oneTypeOfLKH;
                        items += oneTypeOfLKH;

                        count = 0;
                        oneTypeOfLKH = "";
                    }
                }
                else {
                    oneTypeOfLKH = "<tr class='item-table " + firstIsFuture + " " + firstIsJustAdd + "' " + firstMaPTLKH + "' > <td style='background-color: #fff!important' rowspan='" + count + "'" + firstItemofType + oneTypeOfLKH;
                    items += oneTypeOfLKH;
                }
            }

            contentPopupHTML += items;
            //footer popup
            contentPopupHTML += "</table>" +
                "<div class='add-new'>" +
                "<button class='blue-button'>+ Thêm mới</button>" +
                "</div>" +
                "<div class='history-link'>Lịch sử phụ thu</div>";
            popup.querySelector(".content-popup").innerHTML = contentPopupHTML;

            //add event for update button
            let listUpdateRoomButton = popup.querySelectorAll(".content-popup table td button.green-button");
            listUpdateRoomButton.forEach((UB) => {
                UB.addEventListener('click', (e) => {
                    showSubPopup();

                    //pass ITEM-TABLE_LOAIPHONG to sub popup
                    updateAndAddHeSoPhuThuClick(UB.parentElement.parentElement.parentElement, true, soKhachToiDa, e.target.getAttribute("maloaikhachhang"));
                });
            });

            //add event for delete button
            let listDeleteRoomButton = popup.querySelectorAll(".content-popup table td .red-button");
            listDeleteRoomButton.forEach(DRB => {
                DRB.addEventListener("click", () => {
                    showConfirmPopup();

                    //pass message and update title 
                    confirmPopup.querySelector(".header-popup div").innerHTML = "XÓA HỆ SỐ PHỤ THU";
                    confirmPopup.querySelector(".content-popup .message").innerHTML = "Bạn có chắc chắn muốn xóa hệ số phụ thu này?";

                    let confirmButton = confirmPopup.querySelector(".footer-popup .blue-button");
                    confirmButton.addEventListener('click', () => {
                        let itemTable = DRB.parentElement.parentElement.parentElement;
                        let listTD = itemTable.querySelectorAll("td.content-item");
                        deleteHeSoPhuThuConfirm(DRB.getAttribute("maloaikhachhang"), listTD[0].innerHTML, listTD[2].innerHTML, itemTable.getAttribute("mahesophuthu")).then(
                            function (value) {
                                closeConfirmPopup();
                                if (value == "true") {
                                    loadChangeHeSoPhuThu(soKhachToiDa);
                                    toastMessage({ title: 'Xóa thành công', message: 'Xóa hệ số phụ thu thành công', type: 'success', duration: 3500 });
                                }
                                else {
                                    toastMessage({ title: 'Lỗi', message: 'Xóa hệ số phụ thu không thành công', type: 'fail', duration: 3500 });
                                }
                            }
                        );
                    });
                });
            });

            //add event for add new
            popup.querySelector(".content-popup .add-new .blue-button").addEventListener("click", () => {
                showSubPopup();
                updateAndAddHeSoPhuThuClick(null, false, soKhachToiDa, null);
            });

            popup.querySelector(".content-popup .history-link").addEventListener("click", () => {
                loadLichSuChangeHeSoPhuThu(0, 0);
            });

            dataHeSoSave = dths;
        }, function (error) {
            toastMessage({ title: 'Lỗi kết nối', message: 'Lấy về danh sách hệ số phụ thu thất bại', type: 'fail', duration: 3500 });
        }
    );
}

listRegulation[4].addEventListener("click", (e) => {
    HIDEbuttonChangeInPopup();

    getSoKhachToiDa().then(
        function (sktd) {
            let soKhachToiDa = sktd;

            loadChangeHeSoPhuThu(soKhachToiDa, false);
        }, function (error) {
            toastMessage({ title: 'Lỗi kết nối', message: 'Lấy thông tin số khách tối đa thất bại', type: 'fail', duration: 3500 });
        }
    );
});


function getLichSuPhuThuLoaiKhachHang() {

    loadingElement.show();

    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/GetLichSuPhuThuLoaiKhachHang";
    xhr.timeout = 20000;
    xhr.open("GET", url, true);

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = () => {
            loadingElement.hide();
            if (xhr.readyState == 4 && xhr.status == 200) {
                let result = JSON.parse(xhr.responseText);
                resolve(result);
            }
        }
        xhr.send();
    });
}

function loadLichSuChangeHeSoPhuThu(soLuongApDung, maLoaiKhachHang) {
    getLichSuPhuThuLoaiKhachHang().then(
        function (dths) {
            getLoaiKhachHang().then(
                function (dtlkh) {
                    let dataLoaiKhachHang = dtlkh;
                    let selectElement = "";
                    selectElement += "<select>";
                    dataLoaiKhachHang.forEach(LKH => {
                        let isSelected = "";
                        if (maLoaiKhachHang == LKH["maLoaiKhachHang"]) {
                            isSelected = "selected";
                        }
                        selectElement += "<option " + isSelected + " value='" + LKH["maLoaiKhachHang"] + "'>" + LKH["tenLoaiKhachHang"] + "</option>";
                    });
                    selectElement += "</select>";

                    let dataHeSoPhuThu = dths;

                    if (soLuongApDung != 0) {
                        let dataNew = [];
                        dataHeSoPhuThu.forEach(PT => {
                            if (PT["soLuongApDung"] == soLuongApDung) {
                                dataNew.push(PT);
                            }
                        })
                        dataHeSoPhuThu = dataNew;
                    }

                    if (maLoaiKhachHang != 0) {
                        let dataNew = [];
                        dataHeSoPhuThu.forEach(PT => {
                            if (PT["maLoaiKhachHang"] == maLoaiKhachHang) {
                                dataNew.push(PT);
                            }
                        })
                        dataHeSoPhuThu = dataNew;
                    }


                    showHistoryPopup();
                    //title in header popup
                    historyPopup.querySelector(".header-popup div").innerHTML = "LỊCH SỬ HỆ SỐ PHỤ THU THEO LOẠI KHÁCH";

                    //set width for popup
                    historyPopup.style.width = "50%";



                    let contentPopupHTML = "<div class ='filter'>" +
                        "<div class='so-luong-ap-dung'>" +
                        "<div style='margin-right: 10px;'>Số khách áp dụng:</div>" +
                        "<div class='buttonStep' minVal='1'>" +
                        "<button class='subtractVal'>-</button>" +
                        "<span>" + ((soLuongApDung == 0) ? 1 : soLuongApDung) + "</span>" +
                        "<button class='addVal'>+</button>" +
                        "</div>" +
                        "</div>" +
                        "<div class='type'>" +
                        "<div style='margin-right: 10px;'>Loại khách hàng:</div>" +
                        selectElement +
                        "</div>" +
                        "<div class='validate' style='display: flex;'>" +
                        "<button class='blue-button'>Lọc</button><button class='cancel-button'>Xóa bộ lọc</button>" +
                        "</div>" +
                        "</div> ";

                    if (dataHeSoPhuThu == null || dataHeSoPhuThu.length == 0) {
                        historyPopup.querySelector(".content-popup").innerHTML = contentPopupHTML + "<div class ='information'>Chưa có hệ số phụ thu nào</div>";
                        addEventForButtonStep(historyPopup.querySelector(".buttonStep"));
                        let filterClick = historyPopup.querySelector(".validate .blue-button");
                        filterClick.addEventListener("click", () => {
                            loadLichSuChangeHeSoPhuThu(historyPopup.querySelector(".buttonStep span").innerHTML * 1, historyPopup.querySelector("select").value);
                        });

                        let cancelFilterClick = historyPopup.querySelector(".validate .cancel-button");
                        cancelFilterClick.addEventListener("click", () => {
                            loadLichSuChangeHeSoPhuThu(0, 0);
                        });
                        return;
                    }

                    //content popup
                    contentPopupHTML += "<table>" +
                        "<tr class='header-table'>" +
                        "<td>Loại khách</td>" +
                        "<td>Số khách áp dụng</td>" +
                        "<td>Hệ số phụ thu </td>" +
                        "<td>Ngày áp dụng</td>" +
                        "</tr>";

                    //render loai phong
                    let count = 0;
                    let items = "";
                    let oneTypeOfLKH = "";
                    let firstItemofType = "";
                    let firstIsFuture = "";
                    let firstMaPTLKH = "";
                    let lengthArr = dataHeSoPhuThu.length;

                    for (let i = 0; i < lengthArr; i++) {
                        let date = new Date(dataHeSoPhuThu[i]["thoiGianApDung"]);
                        let isFuture = "";

                        let maHeSoPhuThuSoKhach = "";

                        if (date > new Date()) {
                            isFuture = "future";
                            maHeSoPhuThuSoKhach = "mahesophuthu='" + dataHeSoPhuThu[i]["maPhuThuLKH"] + "'";
                        }
                        count++;
                        if (count == 1) {
                            firstItemofType = ">" + dataHeSoPhuThu[i]["tenLoaiKhachHang"] + "</td>" +
                                "<td class='content-item'>" + dataHeSoPhuThu[i]["soLuongApDung"] + "</td>" +
                                "<td class='content-item'>" + dataHeSoPhuThu[i]["heSoPhuThu"] + "</td>" +
                                "<td class='content-item'>" + (date).toLocaleString('en-GB') + "</td>" +
                                "</tr>";

                            firstIsFuture = isFuture;
                            firstMaPTLKH = maHeSoPhuThuSoKhach;
                        }
                        else {
                            oneTypeOfLKH += "<tr class='item-table " + isFuture + "' " + maHeSoPhuThuSoKhach + " >" +
                                "<td class='content-item'>" + dataHeSoPhuThu[i]["soLuongApDung"] + "</td>" +
                                "<td class='content-item'>" + dataHeSoPhuThu[i]["heSoPhuThu"] + "</td>" +
                                "<td class='content-item'>" + (date).toLocaleString('en-GB') + "</td>" +
                                "</tr>"
                        }

                        if (i != lengthArr - 1) {
                            if (dataHeSoPhuThu[i]["maLoaiKhachHang"] != dataHeSoPhuThu[i + 1]["maLoaiKhachHang"]) {
                                oneTypeOfLKH = "<tr class='item-table " + firstIsFuture + "' " + firstMaPTLKH + "'> <td style='background-color: #fff!important' rowspan='" + count + "'" + firstItemofType + oneTypeOfLKH;
                                items += oneTypeOfLKH;

                                count = 0;
                                oneTypeOfLKH = "";
                            }
                        }
                        else {
                            oneTypeOfLKH = "<tr class='item-table " + firstIsFuture + "' " + firstMaPTLKH + "' > <td style='background-color: #fff!important' rowspan='" + count + "'" + firstItemofType + oneTypeOfLKH;
                            items += oneTypeOfLKH;
                        }
                    }

                    contentPopupHTML += items;
                    //footer popup
                    contentPopupHTML += "</table>";
                    historyPopup.querySelector(".content-popup").innerHTML = contentPopupHTML;

                    addEventForButtonStep(historyPopup.querySelector(".buttonStep"));
                    let filterClick = historyPopup.querySelector(".validate .blue-button");
                    filterClick.addEventListener("click", () => {
                        loadLichSuChangeHeSoPhuThu(historyPopup.querySelector(".buttonStep span").innerHTML * 1, historyPopup.querySelector("select").value);
                    });

                    let cancelFilterClick = historyPopup.querySelector(".validate .cancel-button");
                    cancelFilterClick.addEventListener("click", () => {
                        loadLichSuChangeHeSoPhuThu(0, 0);
                    });
                }
            );
        }, function (error) {
            toastMessage({ title: 'Lỗi kết nối', message: 'Lấy về danh sách hệ số phụ thu thất bại', type: 'fail', duration: 3500 });
        }
    );
}
//----5. he so phu thu: end


//----6. phu thu checkin checkout: start

function getLoaiPhuThuCICO() {
    loadingElement.show();

    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/GetLoaiPhuThuCICO";
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = () => {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {
                let result = JSON.parse(xhr.response);
                resolve(result);
            }
        }
        xhr.send();
    });
}

function updatePhuThuCICO(maLoaiPhuThu, soLuongApDung, tiLePhuThu, ngayApDung, maPhuThu) {
    loadingElement.show();
    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/UpdatePhuThuCICO?maLoaiPhuThu=" + maLoaiPhuThu +
        "&soLuongApDung=" + soLuongApDung +
        "&tiLePhuThu=" + tiLePhuThu +
        "&ngayApDung=" + ngayApDung +
        "&maPhuThu=" + maPhuThu;
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = xhr.responseText;
                resolve(result);
            }
        }
        xhr.send();
    });
}

function insertPhuThuCICO(maLoaiPhuThu, soLuongApDung, tiLePhuThu, ngayApDung) {
    loadingElement.show();
    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/InsertPhuThuCICO?maLoaiPhuThu=" + maLoaiPhuThu +
        "&soLuongApDung=" + soLuongApDung +
        "&tiLePhuThu=" + tiLePhuThu +
        "&ngayApDung=" + ngayApDung;
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = xhr.responseText;
                resolve(result);
            }
        }
        xhr.send();
    });
}

function updateAndAddCICOClick(item, isUpdate, info, maLoaiPhuThu) {

    getLoaiPhuThuCICO().then(
        function (dtcico) {

            let dataLoaiPhuThu = dtcico;

            //title header popup
            let titlePopup = subPopup.querySelector(".header-popup div");
            let nameBlueButton = subPopup.querySelector(".footer-popup .blue-button");
            titlePopup.innerHTML = "THÊM PHỤ THU CI-CO";
            nameBlueButton.innerHTML = "Thêm";

            let isFuture = false;
            let listTD = null;
            let baseTiLePhuThu = "";
            let baseMaLoaiPhuThu = maLoaiPhuThu;
            let baseNgayApDung = "";
            let baseSoLuongApDung= "";

            if (isUpdate == true) {
                listTD = item.querySelectorAll("td.content-item");
                isFuture = item.classList.contains("future");
            }

            //default if insert
            let defaultSoGioApDung = 1;
            let defaultThoiGianApDung = convertDDMMYYYYHHMMSStoDateTimeInputFormat(new Date().toLocaleString("en-GB"));

            let maPhuThu = "";
            let isDisable = "";
            if (isUpdate == true && isFuture) {
                maPhuThu = "maphuthu='" + item.getAttribute("maphuthu") + "'";
                defaultSoGioApDung = listTD[0].innerHTML;
                baseSoLuongApDung = defaultSoGioApDung;
                defaultThoiGianApDung = convertDDMMYYYYHHMMSStoDateTimeInputFormat(listTD[2].innerHTML);
                baseNgayApDung = defaultThoiGianApDung;
            }
            if (isUpdate == true && !isFuture) {
                isDisable = "disabled";
            }

            //content sub popup
            let selectElement = "";
            selectElement += "<select " + isDisable + " >";
            dataLoaiPhuThu.forEach(LPT => {
                let isSelected = "";
                if (isUpdate == true && maLoaiPhuThu == LPT["maLoaiPhuThu"]) {
                    isSelected = "selected";
                }
                selectElement += "<option " + isSelected + " value='" + LPT["maLoaiPhuThu"] + "'>" + LPT["tenLoaiPhuThu"] + "</option>";
            });
            selectElement += "</select>";

            let ngayApDungElement = "<tr>" +
                "<td class='label'>Ngày áp dụng: </td>" +
                "<td class='entry'>" +
                "<input class='input-check' type='datetime-local' value='" + defaultThoiGianApDung + "'>" +
                "</td>" +
                "</tr>";
            let soGioApDungElement = "<tr>" +
                "<td class='label'>Số giờ áp dụng: </td>" +
                "<td class='entry'>" +
                "<div class='buttonStep' minVal='1' maxVal='" + 24 + "'>" +
                "<button class='subtractVal'>-</button>" +
                "<span>" + defaultSoGioApDung + "</span>" +
                "<button class='addVal'>+</button>" +
                "</div>" +
                "</td>" +
                "</tr>";
            if (isUpdate == true && !isFuture) {
                ngayApDungElement = "";
                soGioApDungElement = "";
            }

            let valueSoLuongApDung = "";
            let valueTiLePhuThu = "";

            if (isUpdate == true) {
                titlePopup.innerHTML = "SỬA PHỤ THU CI-CO";
                nameBlueButton.innerHTML = "Sửa";
                valueSoLuongApDung = listTD[0].innerHTML;
                baseTiLePhuThu = listTD[1].innerHTML;

                valueTiLePhuThu = "value='" + baseTiLePhuThu + "'";
            }

            let subPopupContentHTML = "<div class ='information'>" + info + "</div>" +
                "<div class='table-container'>" +
                "<table " + maPhuThu + " soluongapdung='" + valueSoLuongApDung + "'>" +
                "<tr>" +
                "<td class='label'>Loại phụ thu: </td>" +
                "<td class='entry'>" +
                selectElement +
                "</td>" +
                "</tr>" +
                soGioApDungElement +
                "<tr>" +
                "<td class='label'>Tỉ lệ phụ thu: </td>" +
                "<td class='entry'>" +
                "<input class='ti-le-phu-thu' type='number' placeholder='Tỉ lệ phụ thu' " + valueTiLePhuThu + ">" +
                "</td>" +
                "</tr>" +
                ngayApDungElement +
                "</table>" +
                "</div>";
            subPopup.querySelector(".content-popup").innerHTML = subPopupContentHTML;
            
            let inputTiLePhuThu = subPopup.querySelector(".content-popup .table-container table input.ti-le-phu-thu");
            let selectLoaiPhuThu= subPopup.querySelector("select");
            let buttonStep = subPopup.querySelector(".content-popup .table-container table .buttonStep");
            let inputNgayApDung = subPopup.querySelector(".content-popup .table-container table input.input-check");

            let buttonSubmit = subPopup.querySelector(".footer-popup .blue-button");
            buttonSubmit.disabled = false;


            if (!(isUpdate == true && !isFuture)) {
                addEventForButtonStep(subPopup.querySelector(".table-container table .buttonStep"));
                subPopup.querySelector(".input-check").addEventListener("input", (e) => {
                    if (new Date(e.target.value) < new Date()) {
                        e.target.value = convertDDMMYYYYHHMMSStoDateTimeInputFormat(new Date().toLocaleString("en-GB"));
                        toastMessage({ title: 'Lỗi', message: 'Thời gian áp dụng phải lớn hơn hoặc bằng hiện tại', type: 'fail', duration: 3500 });
                    }
                });
            }

            if (isUpdate == true && isFuture) {

                let subtractVal = buttonStep.querySelector("button.subtractVal");
                let addVal = buttonStep.querySelector("button.addVal");

                buttonSubmit.disabled = true;
                selectLoaiPhuThu.addEventListener("input", () => {
                    if (buttonStep.querySelector("span").innerHTML == baseSoLuongApDung &&
                        inputTiLePhuThu.value == baseTiLePhuThu &&
                        selectLoaiPhuThu.value == baseMaLoaiPhuThu &&
                        inputNgayApDung.value == baseNgayApDung) {
                        buttonSubmit.disabled = true;
                        return;
                    }
                    buttonSubmit.disabled = false;
                })

                subtractVal.addEventListener("click", () => {
                    if (buttonStep.querySelector("span").innerHTML == baseSoLuongApDung &&
                        inputTiLePhuThu.value == baseTiLePhuThu &&
                        selectLoaiPhuThu.value == baseMaLoaiPhuThu &&
                        inputNgayApDung.value == baseNgayApDung) {
                        buttonSubmit.disabled = true;
                        return;
                    }
                    buttonSubmit.disabled = false;
                })
                addVal.addEventListener("click", () => {
                    if (buttonStep.querySelector("span").innerHTML == baseSoLuongApDung &&
                        inputTiLePhuThu.value == baseTiLePhuThu &&
                        selectLoaiPhuThu.value == baseMaLoaiPhuThu &&
                        inputNgayApDung.value == baseNgayApDung) {
                        buttonSubmit.disabled = true;
                        return;
                    }
                    buttonSubmit.disabled = false;
                })

                inputNgayApDung.addEventListener("input", () => {
                    if (buttonStep.querySelector("span").innerHTML == baseSoLuongApDung &&
                        inputTiLePhuThu.value == baseTiLePhuThu &&
                        selectLoaiPhuThu.value == baseMaLoaiPhuThu &&
                        inputNgayApDung.value == baseNgayApDung) {
                        buttonSubmit.disabled = true;
                        return;
                    }
                    buttonSubmit.disabled = false;
                })
            }

            if (isUpdate == true) {
                buttonSubmit.disabled = true;
                inputTiLePhuThu.addEventListener("input", () => {
                    if ((buttonStep == null || buttonStep.querySelector("span").innerHTML == baseSoLuongApDung) &&
                        inputTiLePhuThu.value == baseTiLePhuThu &&
                        (selectLoaiPhuThu == null || selectLoaiPhuThu.value == baseMaLoaiPhuThu) &&
                        (inputNgayApDung == null || inputNgayApDung.value == baseNgayApDung)) {
                        buttonSubmit.disabled = true;
                        return;
                    }
                    buttonSubmit.disabled = false;
                })
            }

            //add event for change button
            buttonSubmit.addEventListener("click", (e) => {
                let soLuongApDung = subPopup.querySelector(".content-popup .table-container table").getAttribute("soluongapdung");
                let maPhuThu = subPopup.querySelector(".content-popup .table-container table").getAttribute("maphuthu");
                let tiLePhuThu = inputTiLePhuThu.value;
                let maLoaiPhuThuNew = maLoaiPhuThu;
                let thoiGianApDung = "";
                if (!(isUpdate == true && !isFuture)) {
                    maLoaiPhuThuNew = selectLoaiPhuThu.value;
                    soLuongApDung = subPopup.querySelector(".content-popup .table-container table .buttonStep span").innerHTML;
                    thoiGianApDung = inputNgayApDung.value.replace(/T/g, " ");
                }
                if (tiLePhuThu == 0) {
                    toastMessage({ title: 'Lỗi', message: 'Trường tỉ lệ phụ thu không được trống hoặc bằng 0', type: 'fail', duration: 3500 });
                } else {
                    if (isUpdate == true) {
                        updatePhuThuCICO(maLoaiPhuThuNew, soLuongApDung, tiLePhuThu, thoiGianApDung, maPhuThu).then(
                            function (value) {
                                if (value == "true") {
                                    loadChangeCICO();
                                    closeSubPopup();
                                } else {
                                    toastMessage({ title: 'Sửa thất bại', message: 'Đã có phụ thu check in/out tương tự trong hệ thống', type: 'fail', duration: 3500 });
                                }
                            }
                        );
                    } else {
                        insertPhuThuCICO(subPopup.querySelector("select").value, soLuongApDung, tiLePhuThu, thoiGianApDung).then(
                            function (value) {
                                if (value == "true") {
                                    loadChangeCICO();
                                    closeSubPopup();
                                } else {
                                    toastMessage({ title: 'Thêm thất bại', message: 'Đã có phụ thu check in/out tương tự trong hệ thống', type: 'fail', duration: 3500 });
                                }
                            }
                        );
                    }

                }
            })
        }, function (error) {
            toastMessage({ title: 'Lỗi kết nối', message: 'Lấy về loại phụ thu thất bại', type: 'fail', duration: 3500 });
        }
    );
}

function updateGioCICO(gioCheckIn, gioCheckOut) {

    loadingElement.show();
    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/UpdateGioCICO?gioCheckIn=" + gioCheckIn +
        "&gioCheckOut=" + gioCheckOut;

    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = xhr.responseText;
                resolve(result);
            }
        }
        xhr.send();
    });
}

function updateTimeCheckInCheckOut (checkIn, checkOut) {
    //title header popup
    let titlePopup = subPopup.querySelector(".header-popup div");
    let nameBlueButton = subPopup.querySelector(".footer-popup .blue-button");
    titlePopup.innerHTML = "SỬA GIỜ CI-CO";
    nameBlueButton.innerHTML = "Sửa";

    //content sub popup

    let subPopupContentHTML = "<div class='table-container'>" +
                                    "<table>" + 
                                        "<tr>" + 
                                            "<td class='label'>Giờ check in cũ: <b>" + checkIn + "</b> </td>" +
                                            "<td></td>" +
                                            "<td class='label'>Giờ check out cũ: <b>" + checkOut + "</b> </td>" +
                                        "</tr>" +
                                        "<tr>" + 
                                            "<td class='entry'>" +
                                                "Giờ check in mới: <input class='checkin' type='time' value='" + checkIn + "'>" +
                                            "</td>" +
                                            "<td></td>" +
                                            "<td class='entry'>" +
                                                "Giờ check out mới: <input class='checkout' type='time' value='" + checkOut + "'>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" + 
                                "</div>";
    subPopup.querySelector(".content-popup").innerHTML = subPopupContentHTML;

    let inputCheckIn= subPopup.querySelector(".content-popup .table-container table input.checkin");
    let inputCheckOut= subPopup.querySelector(".content-popup .table-container table input.checkout");
    let buttonSubmit = subPopup.querySelector(".footer-popup .blue-button");
    buttonSubmit.disabled = true;

    inputCheckIn.addEventListener("input", () => {
        if (inputCheckIn.value == checkIn && inputCheckOut.value == checkOut) {
            buttonSubmit.disabled = true;
            return;
        }
        buttonSubmit.disabled = false;
    })
    inputCheckOut.addEventListener("input", () => {
        if (inputCheckIn.value == checkIn && inputCheckOut.value == checkOut) {
            buttonSubmit.disabled = true;
            return;
        }
        buttonSubmit.disabled = false;
    })

    buttonSubmit.addEventListener('click', () => {
        let content = subPopup.querySelector(".content-popup");
        updateGioCICO(content.querySelector("input.checkin").value, content.querySelector("input.checkout").value).then(
            function (success) {
                if (success == "true") {
                    closeSubPopup();
                    loadChangeCICO();
                    toastMessage({ title: 'Sửa thành công', message: 'Sửa giờ thành công', type: 'success', duration: 3500 });

                } else {
                    toastMessage({ title: 'Lỗi', message: 'Sửa giờ check in/out thất bại', type: 'fail', duration: 3500 });
                }
            }, function (error) {
                toastMessage({ title: 'Lỗi kết nối', message: 'Sửa giờ check in/out thất bại', type: 'fail', duration: 3500 });
            }
        );
    });

}

function getPhuThuCICO() {
    loadingElement.show();
    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/GetPhuThuCICO";
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = JSON.parse(xhr.responseText);
                resolve(result);
            }
        }
        xhr.send();
    });
}

function deletePhuThuCICOConfirm(maLoaiPhuThu, soLuongApDung, ngayApDung, maPhuThu) {
    ngayApDung = convertDDMMYYYYHHMMSStoDateTimeInputFormat(ngayApDung);

    loadingElement.show();
    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/DeletePhuThuCICO?maLoaiPhuThu=" + maLoaiPhuThu +
        "&soLuongApDung=" + soLuongApDung +
        "&ngayApDung=" + ngayApDung +
        "&maPhuThu=" + maPhuThu;
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = xhr.responseText;
                resolve(result);
            }
        }
        xhr.send();
    });
}

function DontHavePhuThuCICO(gioCheckIn, gioCheckOut) {
    let contentPopupHTML = "<div class ='CICO'>Giờ check in: <b>" + gioCheckIn + "</b><span style='display: inline-block; width:50px;'></span>Giờ check out: <b>" + gioCheckOut + "</b></div>" +
        "<div class='information'>Chưa có phụ thu check in/out nào!</div>" +
        "<div class='add-new'>" +
        "<button class='blue-button'>+ Thêm mới</button>" +
        "</div>" +
        "<div class='history-link'>Lịch sử phụ thu</div>";
    popup.querySelector(".content-popup").innerHTML = contentPopupHTML;

    //add event for add new
    popup.querySelector(".content-popup .add-new .blue-button").addEventListener("click", () => {
        showSubPopup();
        updateAndAddCICOClick(null, false, popup.querySelector(".content-popup .CICO").innerHTML, null);
    });

    //add event for change time check in check out
    popup.querySelector(".content-popup .CICO").addEventListener("click", () => {
        showSubPopup();
        updateTimeCheckInCheckOut(gioCheckIn, gioCheckOut);
    });

    popup.querySelector(".content-popup .history-link").addEventListener("click", () => {
        loadLichSuChangePhuThuCICO(0, 0);
    });
}

function getGioCheckInOut() {
    loadingElement.show();

    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/GetGioCICO";
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = JSON.parse(xhr.responseText);
                resolve(result);
            }
        }
        xhr.send();
    });
}
let dataCICOSave = null;

function checkChangeAfterInsertUpdatePhuThuCICO(maPhuThu, soLuongApDung, tenLoaiPhuThu, maLoaiPhuThu, tiLePhuThu, thoiGianApDung) {
    let checkChange = true;
    if (dataCICOSave == null) {
        return false;
    }
    dataCICOSave.forEach(ds => {
        if (ds["maPhuThu"] == maPhuThu
            && ds["soLuongApDung"] == soLuongApDung
            && ds["tenLoaiPhuThu"] == tenLoaiPhuThu
            && ds["maLoaiPhuThu"] == maLoaiPhuThu
            && ds["tiLePhuThu"] == tiLePhuThu
            && ds["thoiGianApDung"] == thoiGianApDung) {
            checkChange = false;
            return;
        }
    })
    return checkChange;
}

function loadChangeCICO() {
    getGioCheckInOut().then(
        function (dtgcico) {
            getPhuThuCICO().then(
                function (dtptcico) {

                    let dataCICO = dtgcico;
                    let dataPhuThuCICO = dtptcico;

                    regulationClick(listRegulation[5]);

                    if (dataPhuThuCICO == null || dataPhuThuCICO.length == 0) {
                        DontHavePhuThuCICO(dataCICO["gioCheckIn"], dataCICO["gioCheckOut"]);
                        return;
                    }

                    //set width for popup
                    popup.style.width = "60%";

                    //content popup
                    let contentPopupHTML = "<div class ='CICO'>Giờ check in: <b>" + dataCICO["gioCheckIn"] + "</b><span style='display: inline-block; width:50px;'></span>Giờ check out: <b>" + dataCICO["gioCheckOut"] + "</b></div>" +
                        "<table>" +
                        "<tr class='header-table'>" +
                        "<td>Loại phụ thu</td>" +
                        "<td>Số giờ sớm/trễ</td>" +
                        "<td>Tỉ lệ phụ thu </td>" +
                        "<td>Ngày áp dụng</td>" +
                        "<td></td>" +
                        "</tr>";

                    //render loai phong
                    let count = 0;
                    let items = "";
                    let oneTypeOfCICO = "";
                    let firstItemofType = "";
                    let firstIsFuture = "";
                    let firstIsJustAdd = "";
                    let firstMaPhuThu = "";
                    let lengthArr = dataPhuThuCICO.length;

                    for (let i = 0; i < lengthArr; i++) {

                        let date = new Date(dataPhuThuCICO[i]["thoiGianApDung"]);
                        let isFuture = "";

                        let maPhuThuCICO = "";

                        if (date > new Date()) {
                            isFuture = "future";
                            maPhuThuCICO = "maphuthu='" + dataPhuThuCICO[i]["maPhuThu"] + "'";
                        }

                        let justAdd = "";
                        if (checkChangeAfterInsertUpdatePhuThuCICO(dataPhuThuCICO[i]["maPhuThu"],
                            dataPhuThuCICO[i]["soLuongApDung"],
                            dataPhuThuCICO[i]["tenLoaiPhuThu"],
                            dataPhuThuCICO[i]["maLoaiPhuThu"],
                            dataPhuThuCICO[i]["tiLePhuThu"],
                            dataPhuThuCICO[i]["thoiGianApDung"])) {
                            if (isFuture == "future") {
                                justAdd = "future-just-add";
                            } else {
                                justAdd = "just-add";

                            }
                        }

                        count++;
                        if (count == 1) {
                            firstItemofType = ">" + dataPhuThuCICO[i]["tenLoaiPhuThu"] + "</td>" +
                                "<td class='content-item'>" + dataPhuThuCICO[i]["soLuongApDung"] + "</td>" +
                                "<td class='content-item'>" + dataPhuThuCICO[i]["tiLePhuThu"] + "</td>" +
                                "<td class='content-item'>" + (date).toLocaleString("en-GB") + "</td>" +
                                "<td class='content-item'>" +
                                "<div>" +
                                "<button class='green-button' maloaiphuthu='" + dataPhuThuCICO[i]["maLoaiPhuThu"] + "'>Sửa</button>" +
                                "<button class='red-button' maloaiphuthu='" + dataPhuThuCICO[i]["maLoaiPhuThu"] + "'>Xóa</button>" +
                                "</div>" +
                                "</td>" +
                                "</tr>";

                            firstIsFuture = isFuture;
                            firstIsJustAdd = justAdd;
                            firstMaPhuThu = maPhuThuCICO;
                        }
                        else {
                            oneTypeOfCICO += "<tr class='item-table " + isFuture + " " + justAdd + "' " + maPhuThuCICO + " >" +
                                "<td class='content-item'>" + dataPhuThuCICO[i]["soLuongApDung"] + "</td>" +
                                "<td class='content-item'>" + dataPhuThuCICO[i]["tiLePhuThu"] + "</td>" +
                                "<td class='content-item'>" + (date).toLocaleString("en-GB") + "</td>" +
                                "<td class='content-item'>" +
                                "<div>" +
                                "<button class='green-button' maloaiphuthu='" + dataPhuThuCICO[i]["maLoaiPhuThu"] + "'>Sửa</button>" +
                                "<button class='red-button' maloaiphuthu='" + dataPhuThuCICO[i]["maLoaiPhuThu"] + "'>Xóa</button>" +
                                "</div>" +
                                "</td>" +
                                "</tr>"
                        }

                        if (i != lengthArr - 1) {
                            if (dataPhuThuCICO[i]["maLoaiPhuThu"] != dataPhuThuCICO[i + 1]["maLoaiPhuThu"]) {
                                oneTypeOfCICO = "<tr class='item-table " + firstIsFuture + " " + firstIsJustAdd + "' " + firstMaPhuThu + "> <td style='background-color: #fff!important' rowspan='" + count + "'" + firstItemofType + oneTypeOfCICO;
                                items += oneTypeOfCICO;

                                count = 0;
                                oneTypeOfCICO = "";
                            }
                        }
                        else {
                            oneTypeOfCICO = "<tr class='item-table " + firstIsFuture + " " + firstIsJustAdd + "' " + firstMaPhuThu + "> <td style='background-color: #fff!important' rowspan='" + count + "'" + firstItemofType + oneTypeOfCICO;
                            items += oneTypeOfCICO;
                        }
                    }

                    contentPopupHTML += items;
                    //footer popup
                    contentPopupHTML += "</table>" +
                        "<div class='add-new'>" +
                        "<button class='blue-button'>+ Thêm mới</button>" +
                        "</div>" +
                        "<div class='history-link'>Lịch sử phụ thu</div>";
                    popup.querySelector(".content-popup").innerHTML = contentPopupHTML;

                    //add event for update button
                    let listUpdateRoomButton = popup.querySelectorAll(".content-popup table td button.green-button");
                    listUpdateRoomButton.forEach((UB) => {
                        UB.addEventListener('click', (e) => {
                            showSubPopup();

                            //pass ITEM-TABLE_LOAIPHONG to sub popup
                            updateAndAddCICOClick(UB.parentElement.parentElement.parentElement, true, popup.querySelector(".content-popup .CICO").innerHTML, e.target.getAttribute("maloaiphuthu"));
                        });
                    });

                    //add event for delete button
                    let listDeleteRoomButton = popup.querySelectorAll(".content-popup table td .red-button");
                    listDeleteRoomButton.forEach(DRB => {
                        DRB.addEventListener("click", () => {
                            showConfirmPopup();
                            //pass message and update title 
                            confirmPopup.querySelector(".header-popup div").innerHTML = "XÓA PHỤ THU CI-CO";
                            confirmPopup.querySelector(".content-popup .message").innerHTML = "Bạn có chắc chắn muốn xóa phụ thu này?";

                            let confirmButton = confirmPopup.querySelector(".footer-popup .blue-button");
                            confirmButton.addEventListener('click', () => {
                                let itemTable = DRB.parentElement.parentElement.parentElement;
                                let listTD = itemTable.querySelectorAll("td.content-item");
                                deletePhuThuCICOConfirm(DRB.getAttribute("maloaiphuthu"), listTD[0].innerHTML, listTD[2].innerHTML, itemTable.getAttribute("maphuthu")).then(
                                    function (value) {
                                        closeConfirmPopup();
                                        if (value == "true") {
                                            loadChangeCICO();
                                            toastMessage({ title: 'Xóa thành công', message: 'Xóa phụ thu check in/out thành công', type: 'success', duration: 3500 });
                                        }
                                        else {
                                            toastMessage({ title: 'Lỗi', message: 'Xóa phụ thu check in/out không thành công', type: 'fail', duration: 3500 });
                                        }
                                    }
                                );
                            });
                        });
                        
                    });

                    //add event for add new
                    popup.querySelector(".content-popup .add-new .blue-button").addEventListener("click", () => {
                        showSubPopup();
                        updateAndAddCICOClick(null, false, popup.querySelector(".content-popup .CICO").innerHTML, null);
                    });

                    //add event for change time check in check out
                    popup.querySelector(".content-popup .CICO").addEventListener("click", () => {
                        showSubPopup();
                        updateTimeCheckInCheckOut(dataCICO["gioCheckIn"], dataCICO["gioCheckOut"]);
                    });
                    popup.querySelector(".content-popup .history-link").addEventListener("click", () => {
                        loadLichSuChangePhuThuCICO(0, 0);
                    });

                    dataCICOSave = dataPhuThuCICO;

                }, function (error) {
                    toastMessage({ title: 'Lỗi kết nối', message: 'Lấy phụ thu check in/out không thành công', type: 'fail', duration: 3500 });
                }
            );
        }, function (error) {
            toastMessage({ title: 'Lỗi kết nối', message: 'Lấy dữ liệu giờ check in/out không thành công', type: 'fail', duration: 3500 });
        }
    );
}

listRegulation[5].addEventListener("click", (e) => {

    HIDEbuttonChangeInPopup();

    loadChangeCICO();
});

function getLichSuPhuThuCICO() {

    loadingElement.show();

    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/GetLichSuPhuThuCICO";
    xhr.timeout = 20000;
    xhr.open("GET", url, true);

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = () => {
            loadingElement.hide();
            if (xhr.readyState == 4 && xhr.status == 200) {
                let result = JSON.parse(xhr.responseText);
                resolve(result);
            }
        }
        xhr.send();
    });
}

function loadLichSuChangePhuThuCICO(soLuongApDung, maLoaiPhuThu) {
    getLichSuPhuThuCICO().then(
        function (dtptcico) {
            getLoaiPhuThuCICO().then(
                function (dtcico) {

                    let dataLoaiPhuThu = dtcico;

                    let selectElement = "";
                    selectElement += "<select>";
                    dataLoaiPhuThu.forEach(LPT => {
                        let isSelected = "";
                        if (maLoaiPhuThu == LPT["maLoaiPhuThu"]) {
                            isSelected = "selected";
                        }
                        selectElement += "<option " + isSelected + " value='" + LPT["maLoaiPhuThu"] + "'>" + LPT["tenLoaiPhuThu"] + "</option>";
                    });
                    selectElement += "</select>";


                    let dataPhuThuCICO = dtptcico;

                    if(soLuongApDung != 0) {
                        let dataNew = [];
                        dataPhuThuCICO.forEach(PT => {
                            if (PT["soLuongApDung"] == soLuongApDung) {
                                dataNew.push(PT);
                            }
                        })
                        dataPhuThuCICO = dataNew;
                    }

                    if (maLoaiPhuThu != 0) {
                        let dataNew = [];
                        dataPhuThuCICO.forEach(PT => {
                            if (PT["maLoaiPhuThu"] == maLoaiPhuThu) {
                                dataNew.push(PT);
                            }
                        })
                        dataPhuThuCICO = dataNew;
                    }

                    showHistoryPopup();
                    //title in header popup
                    historyPopup.querySelector(".header-popup div").innerHTML = "LỊCH SỬ PHỤ THU CHECK IN/OUT";

                    //set width for popup
                    historyPopup.style.width = "50%";

                    let contentPopupHTML = "<div class ='filter'>" +
                        "<div class='so-luong-ap-dung'>" +
                        "<div style='margin-right: 10px;'>Số khách áp dụng:</div>" +
                        "<div class='buttonStep' minVal='1'>" +
                        "<button class='subtractVal'>-</button>" +
                        "<span>" + ((soLuongApDung == 0) ? 1 : soLuongApDung) + "</span>" +
                        "<button class='addVal'>+</button>" +
                        "</div>" +
                        "</div>" +
                        "<div class='type'>" +
                        "<div style='margin-right: 10px;'>Loại khách hàng:</div>" +
                        selectElement +
                        "</div>" +
                        "<div class='validate' style='display: flex;'>" +
                        "<button class='blue-button'>Lọc</button><button class='cancel-button'>Xóa bộ lọc</button>" +
                        "</div>" +
                        "</div> ";

                    if (dataPhuThuCICO == null || dataPhuThuCICO.length == 0) {
                        historyPopup.querySelector(".content-popup").innerHTML = contentPopupHTML + "<div class ='information'>Chưa có phụ thu check in/out nào</div>";
                        addEventForButtonStep(historyPopup.querySelector(".buttonStep"));
                        let filterClick = historyPopup.querySelector(".validate .blue-button");
                        filterClick.addEventListener("click", () => {
                            loadLichSuChangePhuThuCICO(historyPopup.querySelector(".buttonStep span").innerHTML * 1, historyPopup.querySelector("select").value);
                        });

                        let cancelFilterClick = historyPopup.querySelector(".validate .cancel-button");
                        cancelFilterClick.addEventListener("click", () => {
                            loadLichSuChangePhuThuCICO(0, 0);
                        });
                        return;
                    }

                    //content popup
                    contentPopupHTML += "<table>" +
                        "<tr class='header-table'>" +
                        "<td>Loại phụ thu</td>" +
                        "<td>Số giờ sớm/trễ</td>" +
                        "<td>Tỉ lệ phụ thu </td>" +
                        "<td>Ngày áp dụng</td>" +
                        "</tr>";

                    //render loai phong
                    let count = 0;
                    let items = "";
                    let oneTypeOfCICO = "";
                    let firstItemofType = "";
                    let firstIsFuture = "";
                    let firstMaPhuThu = "";
                    let lengthArr = dataPhuThuCICO.length;

                    for (let i = 0; i < lengthArr; i++) {

                        let date = new Date(dataPhuThuCICO[i]["thoiGianApDung"]);
                        let isFuture = "";

                        let maPhuThuCICO = "";

                        if (date > new Date()) {
                            isFuture = "future";
                            maPhuThuCICO = "maphuthu='" + dataPhuThuCICO[i]["maPhuThu"] + "'";
                        }

                        count++;
                        if (count == 1) {
                            firstItemofType = ">" + dataPhuThuCICO[i]["tenLoaiPhuThu"] + "</td>" +
                                "<td class='content-item'>" + dataPhuThuCICO[i]["soLuongApDung"] + "</td>" +
                                "<td class='content-item'>" + dataPhuThuCICO[i]["tiLePhuThu"] + "</td>" +
                                "<td class='content-item'>" + (date).toLocaleString("en-GB") + "</td>" +
                                "</tr>";

                            firstIsFuture = isFuture;
                            firstMaPhuThu = maPhuThuCICO;
                        }
                        else {
                            oneTypeOfCICO += "<tr class='item-table " + isFuture  + "' " + maPhuThuCICO + " >" +
                                "<td class='content-item'>" + dataPhuThuCICO[i]["soLuongApDung"] + "</td>" +
                                "<td class='content-item'>" + dataPhuThuCICO[i]["tiLePhuThu"] + "</td>" +
                                "<td class='content-item'>" + (date).toLocaleString("en-GB") + "</td>" +
                                "</tr>"
                        }

                        if (i != lengthArr - 1) {
                            if (dataPhuThuCICO[i]["maLoaiPhuThu"] != dataPhuThuCICO[i + 1]["maLoaiPhuThu"]) {
                                oneTypeOfCICO = "<tr class='item-table " + firstIsFuture + "' " + firstMaPhuThu + "> <td style='background-color: #fff!important' rowspan='" + count + "'" + firstItemofType + oneTypeOfCICO;
                                items += oneTypeOfCICO;

                                count = 0;
                                oneTypeOfCICO = "";
                            }
                        }
                        else {
                            oneTypeOfCICO = "<tr class='item-table " + firstIsFuture + "' " + firstMaPhuThu + "> <td style='background-color: #fff!important' rowspan='" + count + "'" + firstItemofType + oneTypeOfCICO;
                            items += oneTypeOfCICO;
                        }
                    }

                    contentPopupHTML += items;
                    //footer popup
                    contentPopupHTML += "</table>";
                    historyPopup.querySelector(".content-popup").innerHTML = contentPopupHTML;

                    addEventForButtonStep(historyPopup.querySelector(".buttonStep"));
                    let filterClick = historyPopup.querySelector(".validate .blue-button");
                    filterClick.addEventListener("click", () => {
                        loadLichSuChangePhuThuCICO(historyPopup.querySelector(".buttonStep span").innerHTML * 1, historyPopup.querySelector("select").value);
                    });

                    let cancelFilterClick = historyPopup.querySelector(".validate .cancel-button");
                    cancelFilterClick.addEventListener("click", () => {
                        loadLichSuChangePhuThuCICO(0, 0);
                    });
                }
            );

        }, function (error) {
            toastMessage({ title: 'Lỗi kết nối', message: 'Lấy về danh sách hệ số phụ thu thất bại', type: 'fail', duration: 3500 });
        }
    );
}
//----6. phu thu checkin checkout: end


//----7. chuc vu: start

function DontHavePosition() {
    let contentPopupHTML = "<div class='information'>Chưa có chức vụ nào!</div>" +
        "<div class='add-new'>" +
        "<button class='blue-button'>+ Thêm mới</button>" +
        "</div>";
    popup.querySelector(".content-popup").innerHTML = contentPopupHTML;

    popup.querySelector(".content-popup .add-new .blue-button").addEventListener("click", () => {
        showSubPopup();
        updateAndAddChucVuClick(null, false);
    });
}

function updateChucVu(maChucVu, tenChucVu) {

    loadingElement.show();

    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/UpdateChucVu?maChucVu=" + maChucVu + "&tenChucVu=" + tenChucVu;
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = xhr.responseText;
                resolve(result);
            }
        }
        xhr.send();
    });
}

function insertChucVu(tenChucVu) {
    loadingElement.show();

    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/InsertChucVu?tenChucVu=" + tenChucVu;
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = xhr.responseText;
                resolve(result);
            }
        }
        xhr.send();
    });
}

function updateAndAddChucVuClick(item, isUpdate) {
    //title header popup
    let titlePopup = subPopup.querySelector(".header-popup div");
    let nameBlueButton = subPopup.querySelector(".footer-popup .blue-button");
    titlePopup.innerHTML = "THÊM CHỨC VỤ";
    nameBlueButton.innerHTML = "Thêm";

    //content sub popup
    let maChucVu = "";
    let valueTenChucVu = "";
    let baseTenChucVu = "";
    
    if (isUpdate == true) 
    {
        titlePopup.innerHTML = "SỬA CHỨC VỤ";
        nameBlueButton.innerHTML = "Sửa";


        let listTD = item.querySelectorAll("td");
        maChucVu = "machucvu='" + item.getAttribute('machucvu') + "'";
        baseTenChucVu = listTD[1].innerHTML;
        valueTenChucVu = "value = '" + baseTenChucVu + "'";
    }

    let subPopupContentHTML = "<div class='table-container'>" +
                                    "<table " + maChucVu + ">" +
                                        "<tr>" + 
                                            "<td class='label'>Tên chức vụ: </td>" +
                                            "<td class='entry'>" +
                                                "<input class='ten-chuc-vu' type='text' placeholder='Tên chức vụ' " + valueTenChucVu + ">" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" + 
                                "</div>";
    subPopup.querySelector(".content-popup").innerHTML = subPopupContentHTML;

    let inputTenChucVu = subPopup.querySelector(".content-popup .table-container table input.ten-chuc-vu");
    let buttonSubmit = subPopup.querySelector(".footer-popup .blue-button");
    buttonSubmit.disabled = false;

    if (isUpdate == true) {
        buttonSubmit.disabled = true;
        inputTenChucVu.addEventListener("input", () => {
            if (inputTenChucVu.value == baseTenChucVu) {
                buttonSubmit.disabled = true;
                return;
            }
            buttonSubmit.disabled = false;
        })
    }

    //add event for change button
    buttonSubmit.addEventListener("click", (e) => {
        let maChucVu= subPopup.querySelector(".content-popup .table-container table").getAttribute("machucvu");
        let tenChucVu= subPopup.querySelector(".content-popup .table-container table input.ten-chuc-vu").value;
        if (tenChucVu == "") {
            toastMessage({ title: 'Lỗi', message: 'Trường tên chức vụ không được phép trống', type: 'fail', duration: 3500 });
        } else {
            if (maChucVu != null) {
                updateChucVu(maChucVu, tenChucVu).then(
                    function (value) {
                        if (value == "true") {
                            loadPopupChucVu();
                            closeSubPopup();
                        } else {
                            toastMessage({ title: 'Sửa thất bại', message: 'Đã có chức vụ ' + tenChucVu, type: 'fail', duration: 3500 });
                        }
                    }
                );
            } else {
                insertChucVu(tenChucVu).then(
                    function (value) {
                        if (value == "true") {
                            loadPopupChucVu();
                            closeSubPopup();
                        } else {
                            toastMessage({ title: 'Thêm thất bại', message: 'Đã có chức vụ ' + tenChucVu, type: 'fail', duration: 3500 });
                        }
                    }
                );
            }

        }
    })
}

function getChucVu() {
    loadingElement.show();

    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/GetChucVu";
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = () => {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {
                let result = JSON.parse(xhr.response);
                resolve(result);
            }
        }
        xhr.send();
    });
}

function deleteChucVuConfirm(maChucVu) {
    loadingElement.show();
    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/DeleteChucVu?maChucVu=" + maChucVu;
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = xhr.responseText;
                resolve(result);
            }
        }
        xhr.send();
    });
}
let dataChucVuSave = null;
function checkChangeAfterUpdateInsertChucVu(maChucVu, tenChucVu) {
    let checkChange = true;
    if (dataChucVuSave == null) {
        return false;
    }
    dataChucVuSave.forEach(ds => {
        if (ds["maChucVu"] == maChucVu
            && ds["tenChucVu"] == tenChucVu) {
            checkChange = false;
            return;
        }
    })
    return checkChange;
}

function loadPopupChucVu() {
    getChucVu().then(
        function (dtcv) {

            let dataChucVu = dtcv;

            regulationClick(listRegulation[6]);

            if (dataChucVu == null || dataChucVu.length == 0) {
                DontHavePosition();
                return;
            }

            //set width for popup
            popup.style.width = "40%";

            //content popup
            let contentPopupHTML = "<table>" +
                "<tr class='header-table'>" +
                "<td>STT</td>" +
                "<td>Tên chức vụ</td>" +
                "<td></td>" +
                "</tr>";

            //render loai phong
            let count = 1;
            dataChucVu.forEach(CV => {
                let justAdd = "";
                let isDisabled = "";
                if (checkChangeAfterUpdateInsertChucVu(CV["maChucVu"], CV["tenChucVu"])) {
                    justAdd= "just-add"
                }
                if (CV["maChucVu"] == 1 || CV["maChucVu"] == 0) {
                    isDisabled = "disabled";
                }

                contentPopupHTML += "<tr class='item-table " + justAdd + "'  machucvu='" + CV["maChucVu"] + "'>" +
                    "<td>" + count + "</td>" +
                    "<td>" + CV["tenChucVu"] + "</td>" +
                    "<td>" +
                    "<div>" +
                    "<button " + isDisabled + " class='green-button'>Sửa</button>" +
                    "<button " + isDisabled + " class='red-button'>Xóa</button>" +
                    "</div>" +
                    "</td>" +
                    "</tr>";
                count++;
            });

            //footer popup
            contentPopupHTML += "</table>" +
                "<div class='add-new'>" +
                "<button class='blue-button'>+ Thêm mới</button>" +
                "</div>";
            popup.querySelector(".content-popup").innerHTML = contentPopupHTML;

            //add event for update button
            let listUpdateRoomButton = popup.querySelectorAll(".content-popup table td button.green-button");
            listUpdateRoomButton.forEach((UB) => {
                UB.addEventListener('click', (e) => {
                    showSubPopup();

                    //pass ITEM-TABLE_LOAIPHONG to sub popup
                    updateAndAddChucVuClick(UB.parentElement.parentElement.parentElement, true);
                });
            });

            //add event for delete button
            let listDeleteRoomButton = popup.querySelectorAll(".content-popup table td .red-button");
            listDeleteRoomButton.forEach(DRB => {
                DRB.addEventListener("click", () => {
                    showConfirmPopup();

                    //pass message and update title 
                    confirmPopup.querySelector(".header-popup div").innerHTML = "XÓA CHỨC VỤ";
                    confirmPopup.querySelector(".content-popup .message").innerHTML = "Bạn có chắc chắn muốn xóa chức vụ này?";

                    let confirmButton = confirmPopup.querySelector(".footer-popup .blue-button");
                    confirmButton.addEventListener('click', () => {
                        deleteChucVuConfirm(DRB.parentElement.parentElement.parentElement.getAttribute("machucvu")).then(
                            function (value) {
                                let tenChucVu = DRB.parentElement.parentElement.parentElement.querySelectorAll("td")[1].innerHTML;
                                closeConfirmPopup();
                                if (value == "true") {
                                    loadPopupChucVu();
                                    toastMessage({ title: 'Xóa thành công', message: 'Xóa chức vụ ' + tenChucVu + ' thành công', type: 'success', duration: 3500 });
                                }
                                else {
                                    toastMessage({ title: 'Xóa thất bại', message: 'Chức vụ ' + tenChucVu + ' đang được sử dụng trong hệ thống (báo cáo chi trả lương, nhân viên, phân quyền)', type: 'fail', duration: 3500 });
                                }
                            }
                        );
                    });
                });

                
            });

            //add event for add new
            popup.querySelector(".content-popup .add-new .blue-button").addEventListener("click", () => {
                showSubPopup();
                updateAndAddChucVuClick(null, false);
            });

            dataChucVuSave = dataChucVu;
        }, function (error) {
            toastMessage({ title: 'Lỗi kết nối', message: 'Lấy dữ liệu chức vụ thất bại', type: 'fail', duration: 3500 });
        }
    );
}

listRegulation[6].addEventListener("click", (e) => {

    HIDEbuttonChangeInPopup();

    loadPopupChucVu();
});
//----7. chuc vu: end

//----8. luong toi thieu vung: start

function getLuongToiThieuVung() {
    loadingElement.show();
    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/GetLuongToiThieuVung";
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = JSON.parse(xhr.responseText);
                resolve(result);
            }
        }
        xhr.send();
    });
}

function updateLuongToiThieuVung(luongToiThieuVung) {
    loadingElement.show();
    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Regulation/UpdateLuongToiThieuVung?luongToiThieuVung=" + luongToiThieuVung;
    xhr.open("GET", url, true);
    xhr.timeout = 20000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {

                let result = xhr.responseText;
                resolve(result);
            }
        }
        xhr.send();
    });
}

listRegulation[7].addEventListener("click", (e) => {

    SHOWbuttonChangeInPopup();
    getLuongToiThieuVung().then(
        function (lttv) {
            //set width for popup
            popup.style.width = "33%";

            regulationClick(e.target);

            //content popup
            let contentPopupHTML = "<div class='information'>Lương tối thiểu vùng hiện tại: <b>" + lttv.toLocaleString() + " VND</b></div>" +
                "<div style='display: flex; justify-content: space-between; align-items: center;'>" +
                "<div>Lương tối thiểu vùng mới: </div>" +
                "<input type='number' placeholder='Lương tối thiểu vùng' value='" + lttv + "'>" +
                "</div>";

            popup.querySelector(".content-popup").innerHTML = contentPopupHTML;

            let inputLuongToiThieuVung = popup.querySelector(".content-popup input");
            let buttonSubmit = popup.querySelector(".footer-popup button.blue-button");
            buttonSubmit.disabled = true;

            inputLuongToiThieuVung.addEventListener("input", () => {
                if (inputLuongToiThieuVung.value == lttv) {
                    buttonSubmit.disabled = true;
                    return;
                }
                buttonSubmit.disabled = false;
            })

            //add event for change button
            popup.querySelector(".footer-popup button.blue-button").addEventListener("click", () => {
                let luongToiThieuVungNew = inputLuongToiThieuVung.value;
                if (luongToiThieuVungNew == 0) {
                    toastMessage({ title: 'Lỗi', message: 'Lương tối thiểu vùng không được phép trống hoặc bằng 0', type: 'fail', duration: 3500 });
                    return;
                }
                updateLuongToiThieuVung(luongToiThieuVungNew).then(
                    function (value) {
                        if (value == "true") {
                            toastMessage({ title: 'Sửa thành công', message: 'Sửa lương tối thiểu vùng thành công', type: 'success', duration: 3500 });
                            closePopup();
                        } else {
                            toastMessage({ title: 'Lỗi', message: 'Sửa lương tối thiểu vùng thất bại', type: 'fail', duration: 3500 });
                        }
                    }
                );
            });
        }, function (error) {
            toastMessage({ title: 'Lỗi kết nối', message: 'Lấy dữ liệu lương tối thiểu vùng thất bại', type: 'fail', duration: 3500 });
        }
    );
    
});
//----8. luong toi thieu vung: end

//----------------------POPUP-LOAIPHONG:End
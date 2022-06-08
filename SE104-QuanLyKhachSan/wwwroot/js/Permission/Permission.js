let submitButton = document.querySelector(".main_working_window .footer button");
function submitPermission(stringPermission) {
    loadingElement.show();

    let formData = new FormData();
    formData.append("stringPermission", stringPermission);
    
    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Permission/SubmitPermission";
    xhr.open("POST", url, true);
    xhr.timeout = 50000;

    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {

            loadingElement.hide();

            if (xhr.readyState == 4 && xhr.status == 200) {
                let result = xhr.responseText;
                resolve(result);
            }
        }
        console.log(formData.get("stringPermission"));
        xhr.send(formData);
    });
}
submitButton.addEventListener("click", () => {
    let listChucVu = document.querySelectorAll(".main_working_window tbody tr");
    let count = 0;
    let stringPermission = "";
    listChucVu.forEach(tr => {
        if (count != 0 && count != 1) {
            let machucvu = tr.getAttribute("machucvu");
            let onePosition = machucvu + "$$"
            let listTD = tr.querySelectorAll("input");
            listTD.forEach(td => {
                if (td.checked == true) {
                    onePosition += "1#";
                } else {
                    onePosition += "0#";
                }
            })
            stringPermission += onePosition + "@@@";
        }
        count++;
    })
    submitPermission(stringPermission).then(
        function (success) {
            if (success == "true") {
                toastMessage({ title: 'Thành công', message: 'Phân quyền thành công', type: 'success', duration: 3500 });
            } else {
                toastMessage({ title: 'Lỗi', message: 'Phân quyền thất bại', type: 'fail', duration: 3500 });
            }
        }, function (error) {
            toastMessage({ title: 'Lỗi đường truyền', message: 'Phân quyền thất bại', type: 'fail', duration: 3500 });
        }
    );
})
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
        if (count != 0) {
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
                alert("Phân quyền thành công!");
            } else {
                alert("Lỗi! Phân quyền không thành công!");

            }
        }, function (error) {
            console.log(error);

            alert("Lỗi đường truyền!");
        }
    );
})
/*    Toast Message Function: begin*/
function toastMessage({ title = '', message = '', type = 'success', duration = 3500 }) {
    let toastContainer = document.getElementById("toast_container");

    if (toastContainer) {
        let toastNotification = document.createElement('div');
        let icon = {
            success: "fa-check",
            fail: "fa-exclamation"
        }
        let delay = (duration / 1000).toFixed(2);
        let timeOutRemoveID = setTimeout(function () {
            toastContainer.removeChild(toastNotification);
        }, duration + 500)
        toastNotification.classList.add("toast_notification", "toast_" + type);
        toastNotification.style.animation = "slideInLeft cubic-bezier(0.68, -0.55, 0.265, 1.55) 0.5s, slideinRight linear 0.2s " + delay + "s forwards";
        toastNotification.innerHTML = '<div class="toast_content">' +
            '<i class="fas fa-solid ' + icon[type] + ' check"></i>' +
            '<div class="toast_message">' +
            '<span class="toast_text toast_text_1">' + title + '</span>' +
            '<span class="toast_text toast_text_2">' + message + '</span>' +
            '</div>' +
            '<i class="fa-solid fa-xmark close"></i>' +
            '<div class="toast_progress"></div>' +
            '</div>';
        toastNotification.querySelector(".close").onclick = function () {
            toastContainer.removeChild(toastNotification);
            clearTimeout(timeOutRemoveID);
        }
        toastNotification.querySelector(".toast_progress").style.setProperty("--toastBeforeAnimation", "progress " + delay + "s linear forwards");
        toastContainer.appendChild(toastNotification);
    }
}
/*    Toast Message Function: end*/

/*------------------Variable--------------------*/
let userNameInput = document.querySelector('body main .container .content .username input');
let passwordInput = document.querySelector('body main .container .content .password input');
let emailInput = document.getElementById('email');
let otpInput = document.getElementById('otp');
let signInButton = document.querySelector('body main .container .content div .validate');
let forgetForward = document.getElementById("forgetForward");
let forgetButton = document.getElementById("forgetButton");
let loginForward = document.getElementById("loginForward");
let getOTPButton = document.getElementById("getOTPButton");
let formContainer = document.querySelector("main .container .content .form-container");
let waitImageContainer = document.querySelector(".form-container .forget-pass .getOTP-container > div.waitanimate");
let successImageContainer = document.querySelector(".form-container .forget-pass .getOTP-container > div.success");
let failImageContainer = document.querySelector(".form-container .forget-pass .getOTP-container > div.fail");
//let loadingElement = {
//    loadingContainer: document.querySelector('body .loading-container'),
//    hide: function () { this.loadingContainer.style.display = 'none'; },
//    show: function () { this.loadingContainer.style.display = 'flex'; }
//};

/*------------------Function--------------------*/
function checkInputIsEmptyLogin() {
    let listInput = document.querySelectorAll('body main .container .content .form-container .login input');
    for (let input of listInput) {
        let messElement = document.getElementById('error-' + input.getAttribute('id'));
        if (input.value == "") {
            messElement.innerHTML = 'Trường này không được phép để trống';
            return false;
        }
    }
    return true;
}

function checkInputIsEmptyForgetPass() {
    let listInput = document.querySelectorAll('body main .container .content .form-container .forget-pass input');
    for (let input of listInput) {
        let messElement = document.getElementById('error-' + input.getAttribute('id'));
        if (input.value == "") {
            messElement.innerHTML = 'Trường này không được phép để trống';
            return false;
        }
    }
    return true;
}

/*------------------Handle--------------------*/
passwordInput.addEventListener('input', function () {
    let pattern = /.{8,32}/;
    if (!pattern.test(passwordInput.value)) {
        document.getElementById('error-password').innerHTML = "Mật khẩu phải có độ dài từ 8-32 ký tự";
    } else {
        document.getElementById('error-password').innerHTML = "";
    }
});

signInButton.addEventListener('click', () => {
    loadingElement.show();
    if (checkInputIsEmptyLogin()) {

        let formLogin = new FormData();
        formLogin.append("userName", userNameInput.value.trim());
        formLogin.append("password", passwordInput.value.trim());

        let xhr_login = new XMLHttpRequest();
        let url_login = "https://localhost:5001/Login/CheckLogin";
        xhr_login.open("POST", url_login, true);
        xhr_login.timeout = 20000;
        xhr_login.onreadystatechange = function () {
            if (xhr_login.readyState == 4 && xhr_login.status == 200) {
                loadingElement.hide();
                let result = JSON.parse(xhr_login.response);
                if (result == true) {
                    window.location.replace("https://localhost:5001/Home/Index")
                }
                else {
                    document.getElementById('error-password').innerHTML = 'Đăng nhập thất bại: sai tài khoản hoặc mật khẩu';
                }
            }
        }
        xhr_login.send(formLogin);
    }
});


forgetForward.addEventListener("click", () => {
    formContainer.style.marginLeft = "-100%";
    emailInput.focus();
});

loginForward.addEventListener("click", () => {
    formContainer.style.marginLeft = "0";
    userNameInput.focus();
});

emailInput.addEventListener('input', function (event) {
    let pattern = /^[-.\w]+@([\w-]+\.)+[\w-]+$/;
    if (!pattern.test(emailInput.value)) {
        document.getElementById('error-email').innerHTML = "Email không hợp lệ";
    } else {
        document.getElementById('error-email').innerHTML = "";
    }
});

getOTPButton.addEventListener("click", () => {
    successImageContainer.style.display = "none";
    failImageContainer.style.display = "none";
    waitImageContainer.style.display = "block";

    let email = emailInput.value.trim();
    emailInput.disabled = true;
    let xhr = new XMLHttpRequest();
    let url = "https://localhost:5001/Login/GetOTP?email=" + email;
    xhr.open("GET", url, true);
    xhr.timeout = 50000;

    xhr.onreadystatechange = () => {
        if (xhr.readyState == 4 && xhr.status == 200) {
            waitImageContainer.style.display = "none";
            if (xhr.responseText == "true") {
                successImageContainer.style.display = "block";
                toastMessage({ title: "Lấy mã OTP thành công", message: "Mã OTP có hiệu lực trong vòng 3 phút", type: "success", duration: 3500 });
            } else {
                failImageContainer.style.display = "block";
                toastMessage({ title: "Lấy mã OTP thất bại", message: xhr.responseText, type: "fail", duration: 3500 });
                emailInput.disabled = false;
            }
        }
    }
    xhr.send();
});

forgetButton.addEventListener("click", () => {
    

    if (checkInputIsEmptyForgetPass()) {
        loadingElement.show();
        let formLogin = new FormData();
        formLogin.append("otp", otpInput.value.trim());
        formLogin.append("email", emailInput.value.trim());

        let xhr = new XMLHttpRequest();
        let url = "https://localhost:5001/Login/ResetPassword";
        xhr.open("POST", url, true);
        xhr.timeout = 20000;
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                loadingElement.hide();
                if (xhr.responseText == "true") {
                    toastMessage({ title: "Đổi mật khẩu thành công", message: "Mật khẩu được reset thành công", type: "success", duration: 3500 });
                    loginForward.click();
                } else {
                    toastMessage({ title: "Đổi mật khẩu thất bại", message: xhr.responseText, type: "fail", duration: 3500 });
                }
            }
        }
        xhr.send(formLogin);
    }
})
userNameInput.focus();
passwordInput.addEventListener("keypress", function (event) {
    if (event.key === "Enter") {
        event.preventDefault();
        signInButton.click();
    }
});
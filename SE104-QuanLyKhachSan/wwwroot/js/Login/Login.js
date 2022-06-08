/*------------------Variable--------------------*/
let userNameInput = document.querySelector('body main .container .content .username input');
let passwordInput = document.querySelector('body main .container .content .password input');
let signInButton = document.querySelector('body main .container .content div .validate');
//let loadingElement = {
//    loadingContainer: document.querySelector('body .loading-container'),
//    hide: function () { this.loadingContainer.style.display = 'none'; },
//    show: function () { this.loadingContainer.style.display = 'flex'; }
//};

/*------------------Function--------------------*/
function checkInputIsEmpty() {
    let listInput = document.querySelectorAll('body main .container .content input');
    for (let input of listInput) {
        let messElement = document.getElementById('error-' + input.getAttribute('id'));
        let preMess = messElement.innerHTML;
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
    if (checkInputIsEmpty()) {

        let formLogin = new FormData();
        formLogin.append("userName", userNameInput.value);
        formLogin.append("password", passwordInput.value);

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
userNameInput.focus();
passwordInput.addEventListener("keypress", function (event) {
    if (event.key === "Enter") {
        event.preventDefault();
        signInButton.click();
    }
});
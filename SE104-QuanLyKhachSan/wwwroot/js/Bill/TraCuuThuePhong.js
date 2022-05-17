$(document).ready(function (e) { 
    document.getElementById("check_menu_burger_icon_id").addEventListener("click", () => {
        var t = document.getElementById("check_menu_burger_id").checked;
        if (t == true) {
            document.getElementsByClassName("menu_burger")[0].style.left = "-20.5%";
            document.getElementsByClassName("main_working_window")[0].style.marginLeft = "0";
        }
        else {
            document.getElementsByClassName("menu_burger")[0].style.left = "0";
            document.getElementsByClassName("main_working_window")[0].style.marginLeft = "20.5%";
        }
    })
    
    var options_burger_menu = document.querySelectorAll(".option_name_icon");
    options_burger_menu.forEach(function (option) {
        option.addEventListener("click", function (e) {
            var arrow_option_burger_menu = e.target.querySelector(".arrow_in_menu_burger");
            arrow_option_burger_menu.classList.toggle("rotate_90_icon_arrow");
        });
    });

    $('#selection_field_qltp_menu_burger_id').hide();
    $('#selection_field_qlnv_menu_burger_id').hide();
    $('#selection_field_qlphong_menu_burger_id').hide();
    $('#selection_field_qldoanhthu_menu_burger_id').hide();
    $('#selection_field_qlhethong_menu_burger_id').hide();

    $('#option_qltp_menu_burger_id').click(function () {
        $('#selection_field_qltp_menu_burger_id').slideToggle(200);
    })
    $('#option_qlnv_menu_burger_id').click(function () {
        $('#selection_field_qlnv_menu_burger_id').slideToggle(200);
    })
    $('#option_qlphong_menu_burger_id').click(function () {
        $('#selection_field_qlphong_menu_burger_id').slideToggle(200);
    })
    $('#option_qldoanhthu_menu_burger_id').click(function () {
        $('#selection_field_qldoanhthu_menu_burger_id').slideToggle(200);
    })
    $('#option_qlhethong_menu_burger_id').click(function () {
        $('#selection_field_qlhethong_menu_burger_id').slideToggle(200);
    })

    var table = document.getElementsByClassName("list_rent_table")[0];
    document.onclick = hideContextMenu;
    table.oncontextmenu = rightClick;
    let menu = document.getElementById("contextMenu");

    let popupArea = document.getElementById('popup_id');
    let rentRoomPopup = document.getElementById('rent_room_info_popup');
    let statusContextMenu = document.getElementById('status');

    function hideContextMenu(e) {
        e.preventDefault();
        document.getElementById("contextMenu").style.display = "none";
    }

    for (let row of table.getElementsByTagName('tr')) {
        row.addEventListener('contextmenu', (e) => {
            // let thisHref = e.target.parentElement.getElementsByTagName('a')[0].getAttribute('href');
            // menu.getElementsByTagName('a')[2].href = thisHref;
            // console.log(row.parentElement.nodeName)
        })
    }

    function rightClick(e) {        
        if (document.getElementById("contextMenu").style.display == "block")
            hideContextMenu(e);
        else {
            console.log(e.target.parentElement.parentElement.nodeName);
            if (e.target.parentElement.parentElement.nodeName !== 'THEAD') {
                e.preventDefault();
                menu.style.display = "block";
                menu.style.left = e.pageX + "px";
                menu.style.top = e.pageY + "px";
            }
        }
    }

    // Click status context menu
    statusContextMenu.onclick = () => {
        popupArea.style.display = 'flex';
        rentRoomPopup.style.display = 'block';
    }

    // Event click outside popup 
    popupArea.onclick = () => {
        popupArea.style.display = 'none';
        if (rentRoomPopup.style.display == 'block')
            rentRoomPopup.style.display = 'none';
        
        rentRoomPopup.onclick = (e) => {
            e.stopPropagation();
        }
    }
})
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

    // List of variables
    let popupCoverAddBill = document.getElementById("popup_id");
    let popupAddBill = document.getElementById("popup__add_bill_id");
    let popupCoverPrintBill = document.getElementById("popup_2_id");
    let popupPrintBill = document.getElementById("popup__print_bill_id");
    let closeBtn = document.getElementsByClassName("close-btn");
    let cancelBtn = document.getElementsByClassName("cancel-btn");
    let openPrintBtn = document.getElementById('issue_invoice_btn');
    let printBtn = document.getElementById('print-btn');
    let listPrintBtn = document.getElementsByClassName('bill_body_item__print_button btn');
    

    // Add new bill
    document.getElementById("add_bill_btn").addEventListener("click", () => {
        popupCoverAddBill.style.display = "flex";
        popupAddBill.style.display = "block";
    })

    // Open modal print bill
    openPrintBtn.addEventListener('click', () => {
        popupCoverPrintBill.style.display = 'flex';
        popupPrintBill.style.display = 'block';
    })

    // Close modal by click outside modal body
    popupCoverAddBill.onclick = function () {
        if (popupCoverAddBill.style.display == "flex") {
            popupCoverAddBill.style.display = "none";    
            popupAddBill.style.display = "none";
        }
    }

    // StopPropation when click
    popupAddBill.onclick = function(e) {
        e.stopPropagation();
    }

    popupPrintBill.onclick = (e) => {
        e.stopPropagation();
    }


    // Close modal by click close button
    for (let btn of closeBtn) {
        btn.onclick = () => {
            if (popupAddBill.style.display == "block") {
                popupAddBill.style.display = "none";
                popupCoverAddBill.style.display = "none"; 
            }
        }
    }

    // Cancel modal by click cancel button
    for (let btn of cancelBtn) {
        btn.onclick = () => {
            let popupParent = btn.parentElement.parentElement.parentElement.parentElement.parentElement;
            popupParent.style.display = 'none';
        }
    }

    // Event click print button
    printBtn.onclick = () => {
        popupCoverPrintBill.style.display = 'none';
        if (popupCoverAddBill.style.display == 'flex') {
            popupCoverAddBill.style.display = 'none';
        }
    }

    // Click print button on list bill
    for (let btn of listPrintBtn) {
        btn.onclick = () => {
            popupCoverPrintBill.style.display = 'flex';
            popupPrintBill.style.display = 'block';
        }
    }

    // Variable to render content add room id
    var contentRoomID =
        `<tr>
            <td class="data_cell"></td>
            <td class="data_cell"></td>
            <td class="data_cell"></td>
            <td class="data_cell"></td>
            <td class="data_cell"></td>
        </tr>`;
})
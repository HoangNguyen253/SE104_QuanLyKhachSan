$(document).ready(function (ev) {
    
    var table = document.getElementsByClassName("list_rent_table")[0];
    document.onclick = hideContextMenu;
    table.oncontextmenu = rightClick;
    let menu = document.getElementById("contextMenuDetail");
    var mact;
    var status = ["Chưa thanh toán", "Đã thanh toán", "Khách ra ngoài", "Bỏ phòng"];
    let currentTarget;

    let popupArea = document.getElementById('popup_id');
    let rentRoomPopup = document.getElementById('rent_room_info_popup');
    let statusContextMenu = document.getElementById('status');

    function hideContextMenu(e) {
        document.getElementById("contextMenuDetail").style.display = "none";
    }

    for (let row of table.getElementsByTagName('tr')) {
        row.addEventListener('contextmenu', (e) => {
            let target = e.target.parentElement;
            mact = target.getElementsByTagName('td')[0].innerHTML;
            currentTarget = e.target;
            console.log(mact);
            console.log(currentTarget);
        })
    }

    function rightClick(e) {        
        if (document.getElementById("contextMenuDetail").style.display == "block") {
            hideContextMenu(e);
        }
        else {
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
        let url_get = '/BillDetail/GetDetailByDetailID?maCTHD=' + mact;
        $.ajax({
            url: url_get,
            success: function (data) {
                let result = data;
                let str = '';
                if (result.maHoaDon != null) {
                    str =
                        `<tr>
                            <td class="data_cell">${result.maCTHD}</td>
                            <td class="data_cell">${result.maPhong}</td>
                            <td class="data_cell">${result.giaPhong}</td>
                            <td class="data_cell">${result.phuThuCICO}</td>
                            <td class="data_cell">${result.tongTienPhong}</td>
                            <td class="data_cell">${status[result.trangThai]}</td>
                        </tr>`
                } else {
                    str =
                        `<tr>
                            <td class="data_cell">${result.maCTHD}</td>
                            <td class="data_cell">${result.maPhong}</td>
                            <td class="data_cell">${result.giaPhong}</td>
                            <td class="data_cell">Chưa kết toán</td>
                            <td class="data_cell">Chưa kết toán</td>
                            <td class="data_cell">${status[result.trangThai]}</td>
                        </tr>`
                }
                $('#body-status').html(str);
                popupArea.style.display = 'flex';
                rentRoomPopup.style.display = 'block';
            }
        });
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

    //----------------------------START: Bo phong -----------------------//
    $('#destroy_booking').click(function () {
        let url_process = '/BillDetail/UpdateCancelStatusDetail?maCTHD=' + mact;
        $.ajax({
            url: url_process,
            success: function () {
                currentTarget.parentElement.getElementsByClassName('data_cell')[5].innerHTML = 'Bỏ phòng';
            }
        })
    })
    //----------------------------END: Bo phong -------------------------//

    //----------------------START: Huy thue phong -----------------------//
    $('#delete_booking').click(function () {
        let url_process = '/BillDetail/DeleteDetailById?maCTHD=' + mact;
        $.ajax({
            url: url_process,
            success: function () {
                currentTarget.parentElement.remove();
            }
        })
    })
    //-----------------------END: Huy thue phong ------------------------//

    //---------------------START: Filter Detail ID ----------------------//
    $("#filter-btn").on("click", function () {
        let value = $("#text-filter").val().toLowerCase();
        $("#table-body-id tr").filter(function () {
            $(this).toggle($(this).find("td:eq(0)").text().toLowerCase().indexOf(value) > -1)
        });
    });
    //-----------------------END: Filter Detail ID ----------------------//

})
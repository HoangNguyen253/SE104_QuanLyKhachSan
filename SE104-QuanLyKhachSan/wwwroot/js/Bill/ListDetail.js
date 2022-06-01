$(document).ready(function (ev) { 

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
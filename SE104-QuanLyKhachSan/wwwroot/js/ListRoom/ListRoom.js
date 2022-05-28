$(document).ready(function (e) {

    $('#tracuuphong').click(function () {
        $('#room_profile_popup_window_container_id').addClass("show");
    })
    document.getElementById("header_close_room_profile_popup_window_id").addEventListener("click", () => {
        document.getElementById("room_profile_popup_window_container_id").classList.remove('show');
    })
    document.getElementById("close_popup_window_room_button").addEventListener("click", () => {
        document.getElementById("room_profile_popup_window_container_id").classList.remove('show');
    })
   
  
})



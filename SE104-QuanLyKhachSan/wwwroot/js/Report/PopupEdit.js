document.getElementById("Edit_btn").addEventListener("click", () => {
    document.getElementById("staff_profile_popup_window_container_id").classList.add('show');
})
document.getElementById("header_close_staff_profile_popup_window_id").addEventListener("click", () => {
    document.getElementById("staff_profile_popup_window_container_id").classList.remove('show');
})

document.getElementById("Cancel_btn").addEventListener("click", () => {
    document.getElementById("staff_profile_popup_window_container_id").classList.remove('show');
})

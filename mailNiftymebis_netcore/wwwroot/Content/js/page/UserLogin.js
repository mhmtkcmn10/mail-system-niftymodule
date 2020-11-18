
var icon_flash = "psi-flash";
var icon_trash = "psi-trash";
var icon_marker = "psi-marker";
var icon_cursor_click = "psi-cursor-click";
var icon_mail_send = "psi-mail-send";
var noty_type_success = "success";
var noty_type_danger = "danger";

$.NotificationTypeMessage = function (Title, Message, Icon, Type) {
    $.niftyNoty({
        type: '' + Type + '',
        container: 'floating',
        html: '<div class="media-left"><span class="icon-wrap icon-wrap-xs icon-circle alert-icon"><i class="' + Icon + ' icon-2x"></i></span></div><div class="media-body"><h4 class="alert-title">' + Title + '</h4><p class="alert-message">' + Message + '</p></div>',
        closeBtn: true,
        floating: {
            position: "top-right"
        },
        focus: true,
        timer: true ? 2000 : 0
    });
}
$(document).on('nifty.ready', function () {

    var btn_Login = $("#btnLogin")

    btn_Login.on("click", function (e) {
        e.preventDefault();

        var username = $("#usernamex").val();
        var password = $("#passwordx").val();

        if (username == "" || password == "") {
            
            $.NotificationTypeMessage("Hatalı Giriş", "Alanlar boş bırakılamaz", icon_flash, noty_type_danger);
        } else {

            var data = {
                "username": username,
                "password": password
            }
            var valdata = $("#registerFORM").serialize();
            $.ajax({
                url: "/UserLogin/UserLogin",
                type: "POST",
                data: { username: username, password: password },
                dataType: "json",
                contentType: /*"application/json"*/'application/x-www-form-urlencoded; charset=UTF-8',
                beforeSend: function () {
                    $("#loader").show();
                    $("#anagovde").css({ "opacity": "0.5" });
                },
                success: function (response) {
                    if (response.result == true) {

                        setTimeout(function () {
                            $.get("/Home/Index", function (data) {
                                window.location.href = "/Home/Index"
                            });
                        }, 1500);

                    } else {

                        $("#loader").hide();
                        $("#anagovde").css({ "opacity": "1" });

                        $.NotificationTypeMessage("Hatalı Giriş", "Kullanıcı bilgilerinizi kontrol ediniz", icon_flash, noty_type_danger);
                    }
                },
                error: function () {
                    $("#loader").hide();
                    $("#anagovde").css({ "opacity": "1" });
                    $.NotificationTypeMessage("Hata", "Hatalı İşlem", icon_flash, noty_type_danger);
                }
            })
        }
    })
})

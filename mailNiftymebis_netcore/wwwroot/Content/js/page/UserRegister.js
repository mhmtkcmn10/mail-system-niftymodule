var icon_star = "psi-star";
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

    var btn_Register = $("#btnRegister")

    btn_Register.on("click", function (e) {
        e.preventDefault();

        var name = $("#namex").val();
        var surname = $("#surnamex").val();
        var email = $("#emailx").val();
        var username = $("#usernamex").val();
        var password = $("#passwordx").val();

        if (name == "" || surname == "" || email == "" || username == "" || password == "") {
           
            $.NotificationTypeMessage("Hatalı Giriş", "Alanlar boş bırakılamaz", icon_flash, noty_type_danger);

        } else {
            var pattern = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i;

            if (!$("#emailx").val().match(pattern)) {
                
                $.NotificationTypeMessage("Hata", "Email formatını eksiksiz giriniz", icon_flash, noty_type_danger);
                return false;
            }

            var passw = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,15}$/;

            if (!$("#passwordx").val().match(passw)) {
                $.NotificationTypeMessage("Hata", "Şifreniz 8 ile 15 karakter arasında büyük harf, küçük harf, sayı ve simge içermelidir", icon_flash, noty_type_danger);
                return false;
            }
            if ($("#demo-form-checkbox").prop("checked")) {
                var data = {
                    "name": name,
                    "surname": surname,
                    "email": email,
                    "username": username,
                    "password": password
                }
                var valdata = $("#registerFORM").serialize();
                $.ajax({
                    url: "/UserRegister/UserRegister",
                    type: "POST",
                    data: { Name: name, Surname: surname, Email: email, Username: username, Password: password },
                    dataType: "json",
                    contentType: /*"application/json"*/'application/x-www-form-urlencoded; charset=UTF-8',
                    beforeSend: function () {
                        $("#loader").show();
                        $("#anagovde").css({ "opacity": "0.5" });
                    },
                    success: function (response) {
                        if (response.result == true) {
                            $.NotificationTypeMessage("Başarılı", "Kayıt işlemi gerçekleşti. Yönlendiriliyorsunuz...", icon_star, noty_type_success);
                            setTimeout(function () {
                                $.get("/UserLogin/UserLogin", function (data) {
                                    window.location.href = "/UserLogin/UserLogin"
                                });
                            }, 1500);
                        } else {
                            $("#loader").hide();
                            $("#anagovde").css({ "opacity": "1" });
                            $.NotificationTypeMessage("Hatalı Giriş", "Kayıtlı veri girişi mevcuttur", icon_flash, noty_type_danger);
                        }
                    },
                    error: function () {
                        $("#loader").hide();
                        $("#anagovde").css({ "opacity": "1" });
                        $.NotificationTypeMessage("Hata", "Hatalı İşlem", icon_flash, noty_type_danger);
                    }
                })
            } else {
                $("#loader").hide();
                $("#anagovde").css({ "opacity": "1" });
                $.NotificationTypeMessage("Hatalı Giriş", "İşaretlenecek alan mevcuttur", icon_flash, noty_type_danger);
            }
        }
    })
})

$('#summernoteMailCompose').summernote({
    placeholder: '',
    tabsize: 2,
    height: 250,
    toolbar: [
        ['style', ['style']],
        ['font', ['bold', 'underline', 'clear']],
        ['color', ['color']],
        ['para', ['ul', 'ol', 'paragraph']],
        ['table', ['table']],
        ['insert', ['link', 'picture', 'video']],
        ['view', ['fullscreen', 'codeview', 'help']]
    ]
});
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

//$(document).ready(function () {
//     $("#selectTest").chosen();
//});

var btn_compose = $("#btnCompose")

btn_compose.on("click", function (e) {
    e.preventDefault();
    var txtMailComposeTitle = $("#txtMailComposeTitle").val()
    var txtMailComposeSummernote = $('#summernoteMailCompose').summernote('code')
    var SecilenKullanicilar = [];
    $('#demo-cs-multiselect :selected').each(function (i, selected) {
        SecilenKullanicilar[i] = $(selected).val();
    });
    if (txtMailComposeTitle == "" || txtMailComposeSummernote == "<p><br></p>" || txtMailComposeSummernote == "" || txtMailComposeSummernote == null || SecilenKullanicilar == "") {

        $.NotificationTypeMessage("Hata", "Alanlar boş bırakılamaz", icon_flash, noty_type_danger);
    }
    else {
        var valdata = $("#mailComposeForm").serialize();
        $.ajax({
            url: "/MailCompose/MailCompose",
            type: "POST",
            data: { title: txtMailComposeTitle, message: txtMailComposeSummernote, taker: SecilenKullanicilar },
            dataType: "json",
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            success: function (response) {
                if (response.result == true) {
                    $.NotificationTypeMessage("Başarılı", "Mesajınız gönderildi", icon_mail_send, noty_type_success);
                    setTimeout(function () { location.reload() }, 2000);
                } else {

                    $.NotificationTypeMessage("Hatalı İşlem", "Bir hata ile karşılaşıldı", icon_flash, noty_type_danger);
                    setTimeout(function () { location.reload() }, 2000);
                }
            },
            error: function () {
                $.NotificationTypeMessage("Hatalı İşlem", "Hata Oluştu", icon_flash, noty_type_danger);

                setTimeout(function () { location.reload() }, 2000);
            }
        });
    }
});
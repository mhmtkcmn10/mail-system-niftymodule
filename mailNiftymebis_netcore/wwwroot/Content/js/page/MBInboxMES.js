var icon_flash = "psi-flash";
var icon_trash = "psi-trash";
var icon_marker = "psi-marker";
var icon_cursor_click = "psi-cursor-click";
var icon_mail_send = "psi-mail-send";
var noty_type_success = "success";
var noty_type_danger = "danger";
document.getElementById('loaderInbox').style.visibility = "hidden";

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

$.InboxMessageSummerNoteCustomize = function () {
    $('#summernoteMailInboxMessage').summernote({
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
    $('#demo-mail-textarea-inbox').hide();
    $('#btnInboxMailMessageCompose').removeClass('hide');
    $('#InboxMessageDirectOrRespondVisibility').removeClass('hide');
}

$('#btnInboxMessageRespond').on('click', function () {
    document.getElementById('InboxLoaderDirectOrRespondCustomize').style.visibility = "hidden";
    document.getElementById('loaderInbox').style.visibility = "visible";
    setTimeout(function () {
        $.InboxMessageSummerNoteCustomize();
        document.getElementById('InboxLoaderDirectOrRespondCustomize').style.visibility = "visible";
        document.getElementById('loaderInbox').style.visibility = "hidden";
    }, 1500);

});
$('#btnInboxMessageDirect').on('click', function () {
    document.getElementById('InboxLoaderDirectOrRespondCustomize').style.visibility = "hidden";
    document.getElementById('loaderInbox').style.visibility = "visible";
    setTimeout(function () {
        $.InboxMessageSummerNoteCustomize();
        document.getElementById('InboxLoaderDirectOrRespondCustomize').style.visibility = "visible";
        document.getElementById('loaderInbox').style.visibility = "hidden";
    }, 1500);
});
if ($('#demo-mail-textarea-inbox').length) {
    $('#demo-mail-textarea-inbox').on('click', function () {
        document.getElementById('InboxLoaderDirectOrRespondCustomize').style.visibility = "hidden";
        document.getElementById('loaderInbox').style.visibility = "visible";
        setTimeout(function () {
            $.InboxMessageSummerNoteCustomize();
            document.getElementById('InboxLoaderDirectOrRespondCustomize').style.visibility = "visible";
            document.getElementById('loaderInbox').style.visibility = "hidden";
        }, 1500);
    });
}

$("#btnInboxMessageRemove").click(function (e) {
    $("#mdlInboxMailMessageModal").modal('show');
});
var InboxDeleteMailMessage = function () {

    var ID = $('#btnInboxDeleteMailMessage').attr('name');
    $.ajax({
        url: '/MBInbox/MBInboxMesMailMessageDelete/' + ID,
        type: 'POST',
        data: ID,
        success: function (data) {
            if (data.result == true) {
                $("#mdlInboxMailMessageModal").modal('hide');
                $.NotificationTypeMessage("Başarılı", "Mesaj silindi", icon_trash, noty_type_success);
                setTimeout(function () { window.location.href = "/MBInbox/MBInbox" }, 2000);
            } else {
                $.NotificationTypeMessage("Başarısız", "Mesaj silinemedi", icon_flash, noty_type_danger);
                setTimeout(function () { location.reload() }, 2000);
            }
        },
        error: function () {
            $.NotificationTypeMessage("Hata", "Hatalı İşlem", icon_flash, noty_type_danger);
            setTimeout(function () { location.reload() }, 2000);
        }
    });
}
var btn_InboxMailMessageCompose = $("#btnInboxMailMessageCompose")

btn_InboxMailMessageCompose.on("click", function (e) {
    e.preventDefault();
    var txtTitle = $("#txtInboxMessageTitle").val()
    var txtsummer = $('#summernoteMailInboxMessage').summernote('code')
    var SecilenKullanicilar = [];

    $('#demo-cs-multiselect :selected').each(function (i, selected) {
        SecilenKullanicilar[i] = $(selected).val();
    });
    if (txtTitle == "" || txtsummer == "<p><br></p>" || txtsummer == "" || txtsummer == null || SecilenKullanicilar == "") {

        $.NotificationTypeMessage("Hata", "Alanlar boş bırakılamaz", icon_flash, noty_type_danger);
    }
    else {
        var data = {
            "title": txtTitle,
            "message": txtsummer,
            "taker": SecilenKullanicilar
        }
        $.ajax({
            url: "/MBInbox/MailInboxCompose",
            type: "POST",
            data: { title: txtTitle, message: txtsummer, taker: SecilenKullanicilar },
            dataType: "json",
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            success: function (response) {
                if (response.result == true) {
                    $.NotificationTypeMessage("Başarılı", "Mesajınız gönderildi", icon_mail_send, noty_type_success);
                    setTimeout(function () { location.reload() }, 2000);
                }
                else {
                    $.NotificationTypeMessage("Başarısız", "Mesajınız gönderilemedi", icon_flash, noty_type_danger);
                    setTimeout(function () { location.reload() }, 2000);
                }
            },
            error: function () {
                $.NotificationTypeMessage("Hata İşlem", "Hata Oluştu", icon_flash, noty_type_danger);
                setTimeout(function () { location.reload() }, 2000);
            }
        });
    }
})


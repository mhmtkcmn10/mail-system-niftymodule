var icon_flash = "psi-flash";
var icon_trash = "psi-trash";
var icon_marker = "psi-marker";
var icon_cursor_click = "psi-cursor-click";
var icon_mail_send = "psi-mail-send";
var noty_type_success = "success";
var noty_type_danger = "danger";
document.getElementById('loaderSend').style.visibility = "hidden";

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

$.SendMessageSummerNoteCustomize = function () {
    $('#summernoteMailSendMessage').summernote({
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
    $('#demo-mail-textarea-send').hide();
    $('#btnSendMailMessageCompose').removeClass('hide');
    $('#SendMessageDirectOrRespondVisibility').removeClass('hide');
}

$('#btnSendMessageRespond').on('click', function () {
    
    document.getElementById('SendLoaderDirectOrRespondCustomize').style.visibility = "hidden";
    document.getElementById('loaderSend').style.visibility = "visible";
    setTimeout(function () {
        $.SendMessageSummerNoteCustomize();
        document.getElementById('loaderSend').style.visibility = "hidden";
        document.getElementById('SendLoaderDirectOrRespondCustomize').style.visibility = "visible";
    }, 1500);
});

$('#btnSendMessageDirect').on('click', function () {
    document.getElementById('SendLoaderDirectOrRespondCustomize').style.visibility = "hidden";
    document.getElementById('loaderSend').style.visibility = "visible";
    setTimeout(function () {
        $.SendMessageSummerNoteCustomize();
        document.getElementById('loaderSend').style.visibility = "hidden";
        document.getElementById('SendLoaderDirectOrRespondCustomize').style.visibility = "visible";
    }, 1500);

});
if ($('#demo-mail-textarea-send').length) {
    $('#demo-mail-textarea-send').on('click', function () {
        document.getElementById('SendLoaderDirectOrRespondCustomize').style.visibility = "hidden";
        document.getElementById('loaderSend').style.visibility = "visible";
        setTimeout(function () {
            $.SendMessageSummerNoteCustomize();
            document.getElementById('loaderSend').style.visibility = "hidden";
            document.getElementById('SendLoaderDirectOrRespondCustomize').style.visibility = "visible";
        }, 1500);
    });
}
$("#btnMessageRemove").click(function (e) {
    $("#mdlSendMailMessageModal").modal('show');

});
var SendDeleteMailMessage = function () {

    var ID = $('#btnSendDeleteMailMessage').attr('name');
    $.ajax({
        url: '/MBSend/MBSendMesMailMessageDelete/' + ID,
        type: 'POST',
        data: ID,
        success: function (data) {
            if (data.result == true) {
                $("#mdlSendMailMessageModal").modal('hide');
                $.NotificationTypeMessage("Başarılı", "Mesaj silindi", icon_trash, noty_type_success);

                setTimeout(function () { window.location.href = "/MBSend/MBSend" }, 2000);
            }
            else {
                $.NotificationTypeMessage("Başarısız", "Mesaj silinmedi", icon_flash, noty_type_danger);

                setTimeout(function () { location.reload() }, 2000);
            }

        },
        error: function () {
            $.NotificationTypeMessage("Hata", "Hatalı İşlem", icon_flash, noty_type_danger);
            setTimeout(function () { location.reload() }, 2000);
        }
    });
}
var btn_SendMailMessageCompose = $("#btnSendMailMessageCompose")
btn_SendMailMessageCompose.on("click", function (e) {
    e.preventDefault();
    var txtsubject = $("#inputSubject").val()
    var txtsummer = $('#summernoteMailSendMessage').summernote('code')
    var firstid = $('#firstID').val()
    var SecilenKullanicilar = [];
    $('#demo-cs-multiselect :selected').each(function (i, selected) {
        SecilenKullanicilar[i] = $(selected).val();
    });
    if (txtsubject == "" || txtsummer == "<p><br></p>" || txtsummer == "" || SecilenKullanicilar == "") {

        $.NotificationTypeMessage("Hata", "Alanlar boş bırakılamaz", icon_flash, noty_type_danger);

    }
    else {
        var data = {
            "title": txtsubject,
            "message": txtsummer,
            "taker": SecilenKullanicilar,
            "firstID": firstid
        }
        $.ajax({
            url: "/MBSend/MailSendCompose",
            type: "POST",
            data: { title: txtsubject, message: txtsummer, taker: SecilenKullanicilar },
            dataType: "json",
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            success: function (response) {
                if (response.result == true) {
                    $.NotificationTypeMessage("Başarılı", "Mesajınız gönderildi", icon_mail_send, noty_type_success);

                    setTimeout(function () { location.reload() }, 2000);
                } else {
                    $.NotificationTypeMessage("Başarısız", "Mesaj gönderilemedi", icon_flash, noty_type_danger);

                    setTimeout(function () { location.reload() }, 2000);
                }
            },
            error: function () {
                $.NotificationTypeMessage("Hata", "Hatalı İşlem", icon_flash, noty_type_danger);
                setTimeout(function () { location.reload() }, 2000);
            }
        });
    }


})


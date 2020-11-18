
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
$(function () {
    $('#select_all_SEND').change(function () {
        var checkboxes = $(this).closest('#kutucuklar').find('.clssSendCheckBox');
        if ($(this).prop('checked')) {
            checkboxes.prop('checked', true);
        } else {
            checkboxes.prop('checked', false);
        }
    }
    );
});

$("#btnSendAll").click(function (e) {
    var checkboxes = $(this).closest('#kutucuklar').find(':checkbox');

    checkboxes.prop('checked', true);

});

$("#btnSendNone").click(function (e) {
    var checkboxes = $(this).closest('#kutucuklar').find(':checkbox');

    checkboxes.prop('checked', false);

});

$("#btnSendMailDelete").click(function (e) {
    var chckedlist = $("input:checkbox.clssSendCheckBox:checked");

    if (chckedlist.length > 0) {
        $("#mdlSendMailDeleteModal").modal('show');
    } else {
        $.NotificationTypeMessage("Hata", "Hatalı İşlem", icon_flash, noty_type_danger);
    }
});
var SendMailDeleteConfirm = function () {

    var deletedItems = new Array();
    $('input:checkbox.clssSendCheckBox').each(function () {
        if ($(this).prop('checked')) {
            deletedItems.push($(this).val());
        }
    });
    var postData = JSON.stringify({
        deletedItems
    });
    $.ajax({
        url: "/MBSend/MBSendMailDelete",
        type: "POST",
        data: { deletedItems: deletedItems },
        dataType: "json",
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (data) {
            if (data.result==true) {
                $('tr:has(input[name="checkBox"]:checked)').remove();
                $("#mdlSendMailDeleteModal").modal("hide");
                $.NotificationTypeMessage("Başarılı", "İşlem gerçekleşti", icon_trash, noty_type_success);
                setTimeout(function () { location.reload(); }, 2000);
            } else {
                $.NotificationTypeMessage("Başarısız", "İşlem gerçekleşemedi", icon_flash, noty_type_danger);
            }
        },
        error: function () {
            $.NotificationTypeMessage("Hata", "Hatalı İşlem", icon_flash, noty_type_danger);
        }
    });
}

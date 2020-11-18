//$(document).ready(function () {

//    $("#btnTestDenemeSend").click(function () {
//        $("#container-animation").load("/MBInbox/GetMailBoxSendPartial");
//    });
//});

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
    $('#select_all_INBOX').change(function () {
        var checkboxes = $(this).closest('#kutucuklar').find('.clssInboxCheckBox');
        var checkboxesunread = $(this).closest('#kutucuklar').find('.clssInboxCheckBoxunread');

        if ($(this).prop('checked')) {
            checkboxes.prop('checked', true);
            checkboxesunread.prop('checked', true);

        } else {
            checkboxes.prop('checked', false);
            checkboxesunread.prop('checked', false);

        }
    }
    );
});

$("#btnInboxAllDelete").click(function (e) {
    var chckedlist = $("input:checkbox.clssInboxCheckBox:checked");
    var chckedlistunread = $("input:checkbox.clssInboxCheckBoxunread:checked");

    if (chckedlist.length > 0 || chckedlistunread.length > 0) {
        $("#mdlInboxMailDeleteModal").modal('show');
    }
    else {
        $.NotificationTypeMessage("Hata", "Hatalı İşlem", icon_flash, noty_type_danger);
    }
});
var InboxMailDeleteConfirm = function () {
    var deletedItems = new Array();
    $('input:checkbox.clssInboxCheckBox').each(function () {
        if ($(this).prop('checked')) {
            deletedItems.push($(this).val());
        }
    });
    $('input:checkbox.clssInboxCheckBoxunread').each(function () {
        if ($(this).prop('checked')) {
            deletedItems.push($(this).val());
        }
    });
    var postData = JSON.stringify({
        deletedItems
    });

    $.ajax({
        url: "/MBInbox/MBInboxMailDelete",
        type: "POST",
        data: { deletedItems: deletedItems },
        dataType: "json",
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (response) {
            if (response.result==true) {
                $('tr:has(input[name="checkBox"]:checked)').remove();
                $("#mdlInboxMailDeleteModal").modal("hide");
                setTimeout(function () { location.reload(); }, 2000);
                $.NotificationTypeMessage("Başarılı", "İşlem Gerçekleşti", icon_trash, noty_type_success);
            } else {
                $.NotificationTypeMessage("Hata", "İşlem Başarısız", icon_flash, noty_type_danger);
            }
        },
        error: function () {
            $.NotificationTypeMessage("Hata", "Hatalı İşlem", icon_flash, noty_type_danger);
        }
    });
}
var checkboxes = $(this).closest('#kutucuklar').find('.clssInboxCheckBox');
var checkboxesunread = $(this).closest('#kutucuklar').find('.clssInboxCheckBoxunread');
$("#btnInboxAll").click(function (e) {
    checkboxesunread = $(this).closest('#kutucuklar').find('.clssInboxCheckBoxunread');
    checkboxes = $(this).closest('#kutucuklar').find('.clssInboxCheckBox');

    checkboxes.prop('checked', true);

    checkboxesunread.prop('checked', true);

});

$("#btnInboxNone").click(function (e) {
    checkboxesunread = $(this).closest('#kutucuklar').find('.clssInboxCheckBoxunread');
    checkboxes = $(this).closest('#kutucuklar').find('.clssInboxCheckBox');

    checkboxes.prop('checked', false);

    checkboxesunread.prop('checked', false);

});

$("#btnInboxAllUnRead").click(function (e) {
    checkboxesunread = $(this).closest('#kutucuklar').find('.clssInboxCheckBoxunread');
    checkboxes = $(this).closest('#kutucuklar').find('.clssInboxCheckBox');

    checkboxes.prop('checked', false);

    checkboxesunread.prop('checked', true);

});

$("#btnInboxAllRead").click(function (e) {
    checkboxesunread = $(this).closest('#kutucuklar').find('.clssInboxCheckBoxunread');
    checkboxes = $(this).closest('#kutucuklar').find('.clssInboxCheckBox');

    checkboxes.prop('checked', true);

    checkboxesunread.prop('checked', false);

});

$("#btnInboxUnRead").click(function (e) {
    var chckedlist = $("input:checkbox.clssInboxCheckBox:checked");
    var chckedlist2 = $("input:checkbox.clssInboxCheckBoxunread:checked");

    if (chckedlist.length > 0 || chckedlist2.length > 0) {
        var updatedItems = new Array();
        $('input:checkbox.clssInboxCheckBox').each(function () {
            if ($(this).prop('checked')) {
                updatedItems.push($(this).val());
            }
        });
        $('input:checkbox.clssInboxCheckBoxunread').each(function () {
            if ($(this).prop('checked')) {
                updatedItems.push($(this).val());
            }
        });
        var postData = JSON.stringify({
            updatedItems
        });
        $.ajax({
            url: "/MBInbox/MailUnRead",
            type: "POST",
            data: { updatedItems: updatedItems },
            dataType: "json",
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            success: function (data) {
                if (data.result==true) {
                    setTimeout(function () { location.reload(); }, 1500);
                    $.NotificationTypeMessage("Başarılı", "Okunmadı olarak işaretlendi", icon_marker, noty_type_success);
                    return false;
                } else {
                    setTimeout(function () { location.reload(); }, 1500);
                    $.NotificationTypeMessage("Başarısız", "Okunmadı olarak işaretlenemedi", icon_flash, noty_type_danger);
                }
            },
            error: function () {
                setTimeout(function () { location.reload(); }, 1500);
                $.NotificationTypeMessage("Hata", "Hatalı İşlem", icon_flash, noty_type_danger);
            }
        });
    }
    else {
        $.NotificationTypeMessage("Hata", "Lütfen mesaj seçiniz", icon_cursor_click, noty_type_danger);

    }
});

$("#btnInboxRead").click(function (e) {
    var chckedlist = $("input:checkbox.clssInboxCheckBox:checked");
    var chckedlist2 = $("input:checkbox.clssInboxCheckBoxunread:checked");

    if (chckedlist.length > 0 || chckedlist2.length > 0) {
        e.preventDefault();

        var updatedItems = new Array();
        $('input:checkbox.clssInboxCheckBox').each(function () {
            if ($(this).prop('checked')) {
                updatedItems.push($(this).val());
            }
        });
        $('input:checkbox.clssInboxCheckBoxunread').each(function () {
            if ($(this).prop('checked')) {
                updatedItems.push($(this).val());
            }
        });
        var postData = JSON.stringify({
            updatedItems
        });
        $.ajax({
            url: "/MBInbox/MailRead",
            type: "POST",
            data: { updatedItems: updatedItems },
            dataType: "json",
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            success: function (data) {
                if (data.result == true) {
                    $.NotificationTypeMessage("Başarılı", "Okundu olarak işaretlendi", icon_marker, noty_type_success);
                    setTimeout(function () { location.reload(); }, 1500);
                    return false;
                } else {
                    setTimeout(function () { location.reload(); }, 1500);
                    $.NotificationTypeMessage("Başarısız", "Okundu olarak işaretlenemedi", icon_flash, noty_type_danger);
                }
            },
            error: function () {
                setTimeout(function () { location.reload(); }, 1500);
                $.NotificationTypeMessage("Hata", "Hatalı İşlem", icon_flash, noty_type_danger);
            }
        });
    }
    else {
        $.NotificationTypeMessage("Hata", "Lütfen mesaj seçiniz", icon_cursor_click, noty_type_danger);

    }
});







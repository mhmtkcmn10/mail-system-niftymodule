setTimeout(function () {
    var url = document.URL;
    $("#myModalCookie").modal('show');

    $("#btnAuthenticationClose").click(function () {
        window.location.href = "/UserLogin/UserLogin";
    });

    $("#btnAuthenticationConfirm").click(function () {
        $.ajax({
            url: "/UserLogin/RefreshToken",
            type: "POST",
            success: function (response) {
                if (response.result == true) {
                    window.location.href = document.URL;
                } else {
                    window.location.href = "/UserLogin/UserLogin";
                }
            },
            error: function () {
                $.niftyNoty({
                    type: 'danger',
                    container: 'floating',
                    html: '<div class="media-left"><span class="icon-wrap icon-wrap-xs icon-circle alert-icon"><i class="psi-flash icon-2x"></i></span></div><div class="media-body"><h4 class="alert-title">Hata</h4><p class="alert-message">Hatalı ile karşılaşıldı ...</p></div>',
                    closeBtn: true,
                    floating: {
                        position: "top-right"
                    },
                    focus: true,
                    timer: true ? 2000 : 0
                });
                window.location.href = "/UserLogin/UserLogin";
            }
        });
    });

    $(function () {
        var saniye = 60;
        var sayacYeri = $("#sayac");

        $.sayimiBaslat = function () {

            if (saniye > 1) {
                saniye--;
                sayacYeri.text(saniye);
            } else {

                window.location.href = "/UserLogin/UserLogin";
            }
        }
        sayacYeri.text(saniye);
        setInterval("$.sayimiBaslat()", 1000);
    });

},500000);
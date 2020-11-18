document.onreadystatechange = function () {
    var state = document.readyState
    if (state == 'interactive') {
        document.getElementById('loader').style.visibility = "visible";

        document.getElementById('container-animation').style.visibility = "hidden";

    } else if (state == 'complete') {
        setTimeout(function () {
            document.getElementById('interactive');
            document.getElementById('loader').style.visibility = "hidden";
            document.getElementById('container-animation').style.visibility = "visible";
        }, 2000)
    }
}
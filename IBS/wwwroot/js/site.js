function ShowHideMsg(type, msg) {
    toastr[type](msg);
}

function ShowHideMsgNew(type, msg) {
    if (type) {
        toastr["success"](msg);
    }
    else {
        toastr["error"](msg);
    }
}

function isNumber(evt) {
    // Allow: backspace, delete, tab, escape, enter and .
    if ($.inArray(evt.keyCode, [46, 8, 9, 27, 13, 190]) !== -1 ||
        // Allow: Ctrl+A,Ctrl+C,Ctrl+V, Command+A
        ((evt.keyCode == 65 || evt.keyCode == 86 || evt.keyCode == 67) && (evt.ctrlKey === true || evt.metaKey === true)) ||
        // Allow: home, end, left, right, down, up
        (evt.keyCode >= 35 && evt.keyCode <= 40 && evt.keyCode == 16 && evt.keyCode == 110 && evt.keyCode == 78)) {
        // let it happen, don't do anything
        return;
    }

    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function isNumberWithDot(evt) {
    // Allow: backspace, delete, tab, escape, enter and .
    if ($.inArray(evt.keyCode, [8, 9, 27, 13, 190]) !== -1 ||
        // Allow: Ctrl+A,Ctrl+C,Ctrl+V, Command+A
        ((evt.keyCode == 65 || evt.keyCode == 86 || evt.keyCode == 67) && (evt.ctrlKey === true || evt.metaKey === true)) ||
        // Allow: home, end, left, right, down, up
        (evt.keyCode >= 35 && evt.keyCode <= 40 && evt.keyCode == 16 && evt.keyCode == 110 && evt.keyCode == 78)) {
        // let it happen, don't do anything
        return;
    }

    if (evt.which == 46 && evt.target.value.indexOf('.') != -1) { evt.preventDefault(); } // prevent if already dot

    var a = [46];
    var k = evt.which;
    for (i = 48; i < 58; i++)
        a.push(i);

    if (!($.inArray(k, a) >= 0))
        evt.preventDefault();
}

function IsValidDate(myInput) {
    var pattern = /(?:(?:(?:0[1-9]|1\d|2[0-8])\/(?:0[1-9]|1[0-2])|(?:29|30)\/(?:0[13-9]|1[0-2])|31\/(?:0[13578]|1[02]))\/[1-9]\d{3}|29\/02(?:\/[1-9]\d(?:0[48]|[2468][048]|[13579][26])|(?:[2468][048]|[13579][26])00))/;
    if (!pattern.test(myInput)) {
        return false;
    }
}


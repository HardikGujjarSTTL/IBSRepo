function ShowHideMsg(type, msg) {
    toastr[type](msg);
}

function gotoTab(tab) {
    $("#" + tab).css("pointer-events", "all");
    $("#" + tab).trigger("click");
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

function getContactEntryModal(ContactId, ContactTypeID, ContactRefrenceID) {
    $.post("/Contact/Manage", { ContactId, ContactTypeID, ContactRefrenceID }, function (response) {
        $("#contactEntryModal").find(".modal-body").html(response);
        $("#contactEntryModal").modal('show');
    });
}

function showMapModal() {
    $("#mapModal").modal('show');
}

function getCallEntryModal(CdId, CdRtId, CdRefType) {
    $.post("/CallDetail/Manage", { CdId, CdRtId, CdRefType }, function (response) {
        $("#callDetailEntryModal").find(".modal-body").html(response);
        $("#callDetailEntryModal").modal('show');
    });
}

function getNoteEntryModal(NdId, NdRtId, NdRefType) {
    $.post("/Notes/Manage", { NdId, NdRtId, NdRefType }, function (response) {
        $("#noteEntryModal").find(".modal-body").html(response);
        $("#noteEntryModal").modal('show');
    });
}


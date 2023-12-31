﻿function ShowHideMsg(type, msg) {
    toastr.remove();
    toastr[type](msg);
}

function ShowHideMsgNew(type, msg) {
    toastr.remove();
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

function getExportFileName(title) {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();
    var hh = today.getHours();
    hh = hh > 9 ? hh : '0' + hh;
    var mi = today.getMinutes();
    mi = mi > 9 ? mi : '0' + mi;
    var sec = today.getSeconds();
    sec = sec > 9 ? sec : '0' + sec;

    return title + '_' + dd + '_' + mm + '_' + yyyy + '_' + hh + '_' + mi + '_' + sec;
}

function FromToDateGreaterValidation(FromDate, ToDate) {   
    var result = true;
    var FDT = new Date(FromDate);
    var TDT = new Date(ToDate);

    if (FDT > TDT) {
        alert("FromDate should not be greater than ToDate.")
        result = false;
    }
    return result;
}

function GetFinancialYearStartEndDate() {
    var FISCAL_START_MONTH = 4;

    // Get the current date
    var currentDate = new Date();

    // Check if the current month is less than the fiscal start month
    if (currentDate.getMonth() + 1 < FISCAL_START_MONTH) {
        // If true, set the fiscal year start date to the previous year's fiscal start month
        var financialYearStartDate = new Date(currentDate.getFullYear() - 1, FISCAL_START_MONTH - 1, 1);
    } else {
        // If false, set the fiscal year start date to the current year's fiscal start month
        var financialYearStartDate = new Date(currentDate.getFullYear(), FISCAL_START_MONTH - 1, 1);
    }

    // Calculate fiscal year end date
    var financialYearEndDate = new Date(financialYearStartDate.getFullYear() + 1, FISCAL_START_MONTH - 1, 0);

    return {
        start: financialYearStartDate,
        end: financialYearEndDate
    };
}
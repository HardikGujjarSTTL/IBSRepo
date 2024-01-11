$('#txtMsg').keypress(function (e) {
    if (e.keyCode == 13) {  // detect the enter key
        SendMessage(2);
    }
});

$('#fileupload2').change(function (e) {
    handleFile('fileupload2');
});

$(".deleteSelect").click(function () {
    $(".fileUploadDelete").slideToggle();
});

function CloseFileUploadPopup() {
    $(".fileUploadedInfo").hide();

    $("#hdnBase64String").val("");
    $("#hdnFileName").val("");
    $("#hdnFileSize").val("");

    $("#FileSize").html("");
    $(".fileUploadFileName").html("");
}
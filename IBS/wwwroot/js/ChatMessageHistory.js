$('#txtMsg').keypress(function (e) {
    if (e.keyCode == 13) {  // detect the enter key
        e.preventDefault();
        SendMessage(2);
    }
});

$('#fileupload2').change(function (e) {
    handleFile('fileupload2');
});

if ($(".deleteSelectTrigger").length) {
    $(".deleteSelectTrigger").click(function () {
        if ($(".fileUploadDelete").is(":visible")) {
            $(this).next().slideUp();
        } else {
            $(this).next().slideDown();
            return false;
        }
    });
}
$(".chatBoxBody").on('click', function () {
   $(".fileUploadDelete").slideUp();
});
$(".sendText").click(function () {
    $(".fileUploadDelete").slideUp();
});

function CloseFileUploadPopup() {
    $(".fileUploadedInfo").hide();

    $("#hdnBase64String").val("");
    $("#hdnFileName").val("");
    $("#hdnFileSize").val("");

    $("#FileSize").html("");
    $(".fileUploadFileName").html("");
}

$("#btnDownload").click(function (e) {
    e.preventDefault(); 
    window.location.href = 'https://14.143.90.241/IBS2/ReadWriteData/CHAT_FILES/9f040b30-011a-4159-ab3c-c9c348e6fbb5.jpg';
    alert("download");
});
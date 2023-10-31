<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IBSReports._Default" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <script>

        function abc(xmldata, xmlfilename) {
            var xml = (xmldata);
            $.post("http://127.0.0.1:1620", { "response": xml }, function (exeResponse) {
                var status = $(exeResponse).find("status").text();

                if (status == "failed") {
                }
                else {
                    return;
                    $.post("/RBS/PKIDATA.aspx", { "file": exeResponse, "filename": xmlfilename }, function (res) {
                        window.location = "/RBS/IC_RPT_Intermediate.aspx?filename=" + res;
                    });
                }
            }).fail(function (xhr, textStatus, errorThrown) {                
                window.location = "/RBS/IC_RPT_Intermediate.aspx?filename=" + xmlfilename;
            });
        }
    </script>
</head>
<body>
    <div>
        <CR:CrystalReportViewer ID="cristalview" runat="server" AutoDataBind="true" HasExportButton="True"/>
    </div>
</body>
</html></asp:Content>
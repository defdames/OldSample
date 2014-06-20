<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEditTime.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.TimeClock.Edit.umEditTime" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport ID="ViewPort1" runat="server" Layout="BorderLayout">
            <Items>
                <ext:DateField ID="uxDateTimeInField" runat="server" FieldLabel="TimeIn"></ext:DateField>
                <ext:DateField ID="uxDateTimeOutField" runat="server" FieldLabel="TimeOut"></ext:DateField>
            </Items>
        </ext:Viewport>

    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umBudgetMain.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umBudgetMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
          <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" Namespace="App" ClientIDMode="Static" />         
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
        <ext:TabPanel runat="server" DeferredRender="true" Region="Center" ID="uxBudgetTabPanel" Padding="5" >

                </ext:TabPanel>
                </Items>
            </ext:Viewport>
    </form>
</body>
</html>

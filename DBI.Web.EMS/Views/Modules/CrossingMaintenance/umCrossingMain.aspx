<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCrossingMain.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umCrossingMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div></div>
     <ext:ResourceManager ID="ResourceManager2" runat="server" />

    <div>
     <ext:Viewport ID="Viewport1" runat="server" Layout="FitLayout">
            <Items>

            <ext:GridPanel ID="uxCrossingMainGrid" runat="server" Region="North" Layout="HBoxLayout">
                <Items>
                    
                    
                </Items>
            </ext:GridPanel>

            <ext:TabPanel ID="uxCrossingTab" runat="server" Region="Center" >
                <Items>


                </Items>
            </ext:TabPanel>

            </Items>
         </ext:Viewport>
    </div>
    </form>
</body>
</html>

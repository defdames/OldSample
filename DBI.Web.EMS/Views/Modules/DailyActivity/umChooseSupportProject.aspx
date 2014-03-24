<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umChooseSupportProject.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umChooseSupportProject" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="FitLayout">
            <Items>
                <ext:FormPanel runat="server" ID="uxChooseSupportProject" Layout="FormLayout" Region="Center" BodyPadding="5">
                    <Buttons>
                        <ext:Button ID="Button1" runat="server" Text="Submit">
                            <DirectEvents>
                                <Click OnEvent="deSupportProjectChoice" />
                            </DirectEvents>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

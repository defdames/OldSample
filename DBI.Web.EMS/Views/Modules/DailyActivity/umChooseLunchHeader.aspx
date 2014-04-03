<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umChooseLunchHeader.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umChooseLunchHeader" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
                <ext:FormPanel runat="server" ID="uxChooseLunchForm" Layout="FormLayout">
                    <Buttons>
                        <ext:Button runat="server" Text="Submit">
                            <DirectEvents>
                                <Click OnEvent="deStoreLunchChoice">
                                    <EventMask ShowMask="true" />
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
    </form>
</body>
</html>

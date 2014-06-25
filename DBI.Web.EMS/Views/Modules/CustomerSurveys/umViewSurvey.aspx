<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umViewSurvey.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umViewSurvey" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager runat="server" IsDynamic="false" />
        <ext:Viewport runat="server">
            <Items>
                <ext:FormPanel runat="server" ID="uxSurveyDisplay">
                    <Items>

                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxSubmitSurveyButton">

                        </ext:Button>
                        <ext:Button runat="server" ID="uxCancelSurveyButton">

                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCustomerSurveyAdmin.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umCustomerSurveyAdmin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" IsDynamic="false" />
        <ext:Viewport ID="uxAdminViewPort" runat="server" Layout="FitLayout">
            <Items>
                <ext:TabPanel runat="server" ID="uxAdminTabPanel">
                    <Items>
                        <ext:Panel ID="uxAdminFormsTab" runat="server" Title="Manage Forms">
                            <Loader Url="umManageForms.aspx" runat="server" AutoLoad="true" Mode="Frame">
                                <LoadMask ShowMask="true" />
                            </Loader>
                        </ext:Panel>
                        <ext:Panel ID="uxAdminQuestionsTab" runat="server" Title="Manage Questions">
                            <Loader Url="umManageQuestions.aspx" runat="server" AutoLoad="true" Mode="Frame">
                                <LoadMask ShowMask="true" />
                            </Loader>
                        </ext:Panel>
                        <ext:Panel ID="uxAdminOrgTab" runat="server" Title="Manage Organizations">
                            <Loader Url="umManageOrgs.aspx" runat="server" AutoLoad="true" Mode="Frame">
                                <LoadMask ShowMask="true" />
                            </Loader>
                        </ext:Panel>
                    </Items>
                </ext:TabPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

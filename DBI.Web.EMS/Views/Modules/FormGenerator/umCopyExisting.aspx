<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCopyExisting.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umCopyExisting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager runat="server" IsDynamic="false" />
        <ext:Panel runat="server" ID="uxContainerPanel" Width="650" Layout="FitLayout">
            <Items>
                <ext:GridPanel runat="server" ID="uxChooseFormGrid" Title="Choose a Form to Copy">
                    <Store>
                        <ext:Store runat="server" ID="uxChooseFormStore" OnReadData="deReadForms">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="FORM_ID" />
                                        <ext:ModelField Name="FORMS_NAME" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>

                        </Columns>
                    </ColumnModel>
                    <BottomBar>
                        <ext:PagingToolbar runat="server" />
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Panel>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSurveyCategories.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umSurveyCategories" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager runat="server" IsDynamic="false" />
        <ext:Viewport runat="server" Layout="FitLayout">
            <Items>
                <ext:GridPanel runat="server" ID="uxCategoriesGrid">
                    <Store>
                        <ext:Store runat="server" ID="uxCategoriesStore" OnReadData="deReadCategories" AutoDataBind="true" PageSize="20" RemoteSort="true">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="CATEGORY_ID" />
                                        <ext:ModelField Name="CATEGORY_NAME" Type="String" />
                                        <ext:ModelField Name="DESCRIPTION" Type="String" />
                                        <ext:ModelField Name="NUM_FORMS" Type="Int" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="Category Name" DataIndex="CATEGORY_NAME" />
                            <ext:Column runat="server" Text="Category Description" DataIndex="DESCRIPTION" />
                            <ext:Column runat="server" Text="Number of Forms" DataIndex="NUM_FORMS" />
                        </Columns>
                    </ColumnModel>
                    <DirectEvents>
                        <Select OnEvent="deLoadCategoryForm">
                            <ExtraParams>
                                <ext:Parameter Name="CategoryId" Value="#{uxCategoriesGrid}.getSelectionModel().getSelection()[0].data.CATEGORY_ID" Mode="Raw" />
                            </ExtraParams>
                        </Select>
                    </DirectEvents>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

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
                <ext:GridPanel runat="server" ID="uxCategoryGrid">
                    <Store>
                        <ext:Store runat="server" ID="uxCategoriesStore" OnReadData="deReadCategories" AutoDataBind="true" PageSize="20" RemoteSort="true">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="CATEGORY_ID" />
                                        <ext:ModelField Name="NAME" Type="String" />
                                        <ext:ModelField Name="DESCRIPTION" Type="String" />
                                        <ext:ModelField Name="NUM_FORMS" Type="Int" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Sorters>
                                <ext:DataSorter Property="NAME" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="Category Name" DataIndex="NAME" Flex="25" />
                            <ext:Column runat="server" Text="Category Description" DataIndex="DESCRIPTION" Flex="50" />
                            <ext:Column runat="server" Text="Number of Forms" DataIndex="NUM_FORMS" Flex="25" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                    </Plugins>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxAddCategoryButton" Text="Add" Icon="ApplicationAdd">
                                    <Listeners>
                                        <Click Handler="#{uxAddEditCategoryWindow}.show(); #{uxFormType}.setValue('Add')" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxEditCategoryButton" Text="Edit" Icon="ApplicationEdit" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deLoadEditCategoryWindow">
                                            <ExtraParams>
                                                <ext:Parameter Name="CategoryInfo" Value ="Ext.encode(#{uxCategoryGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxDeleteCategoryButton" Text="Delete" Icon="ApplicationDelete" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deDeleteCategory">
                                            <ExtraParams>
                                                <ext:Parameter Name="CategoryId" Value ="#{uxCategoryGrid}.getSelectionModel().getSelection()[0].data.CATEGORY_ID" Mode="Raw" />
                                            </ExtraParams>
                                            <Confirmation ConfirmRequest="true" Title="Really Delete?" Message="Do you really want to delete this category?" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <BottomBar>
                        <ext:PagingToolbar runat="server" />
                    </BottomBar>
                    <Listeners>
                        <Select Handler="#{uxEditCategoryButton}.enable(); #{uxDeleteCategoryButton}.enable()" />
                    </Listeners>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
        <ext:Window runat="server" ID="uxAddEditCategoryWindow" Width="250" Hidden="true">
            <Items>
                <ext:FormPanel runat="server" ID="uxCategoryForm" Layout="FormLayout">
                    <Items>
                        <ext:Hidden runat="server" ID="uxFormType" />
                        <ext:Hidden runat="server" ID="uxCategoryId" />
                        <ext:TextField runat="server" ID="uxCategoryName" FieldLabel="Category Name" AllowBlank="false" />
                        <ext:TextField runat="server" ID="uxDescription" FieldLabel="Description" AllowBlank="false" />
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxSaveCategoryButton" Text="Submit" Icon="Add" Disabled="true">
                            <DirectEvents>
                                <Click OnEvent="deSaveCategory">
                                    <EventMask ShowMask="true" />
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="uxCancelCategoryButton" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="#{uxAddEditCategoryWindow}.hide(); #{uxCategoryForm}.reset()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <Listeners>
                        <ValidityChange Handler="#{uxSaveCategoryButton}.setDisabled(!valid)" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>

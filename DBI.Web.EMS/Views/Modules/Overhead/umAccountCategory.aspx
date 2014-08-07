<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAccountCategory.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umAccountCategory" %>

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

                  <ext:GridPanel ID="uxBudgetTypeGridPanel" runat="server" Flex="1" SimpleSelect="true" Header="false" Padding="5" Region="Center">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxAssignBudgetType" Icon="ApplicationAdd" Text="Assign Category" Disabled="true"> 
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip1" runat="server" Html="This allows you to assign a budget type to this organization" UI="Info"></ext:ToolTip>
                                    </ToolTips>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxAccountCategoryStore"
                            AutoDataBind="true" RemoteSort="true" AutoLoad="true" OnReadData="deLoadAccounts">
                            <Model>
                                        <ext:Model ID="Model2" runat="server" IDProperty="SEGMENT5">
                                            <Fields>
                                                <ext:ModelField Name="SEGMENT5_DESC"  />
                                                <ext:ModelField Name="CATEGORY_NAME"  />
                                                <ext:ModelField Name="CATEGORY_ID"  />
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
                                    <ext:Column ID="Column2" runat="server" DataIndex="SEGMENT5_DESC" Text="Account Name" Flex="1" />
                                    <ext:Column ID="Column1" runat="server" DataIndex="CATEGORY_NAME" Text="Category Name" Flex="1" />
                                </Columns>
                            </ColumnModel>
                     <SelectionModel>
                         <ext:RowSelectionModel runat="server" Mode="Single" ID="uxBudgetTypeSelectionModel" AllowDeselect="true">
                             <Listeners>
                                 <Select Handler="if(#{uxBudgetTypeSelectionModel}.getCount() > 0 && (#{uxBudgetTypeStore}.getCount() == (index + 1))){#{uxDeleteBudgetType}.enable();}else {#{uxDeleteBudgetType}.disable();}"></Select>
                                        <Deselect Handler="if(#{uxBudgetTypeSelectionModel}.getCount() > 0 && (#{uxBudgetTypeStore}.getCount() == (index + 1))){#{uxDeleteBudgetType}.enable();}else {#{uxDeleteBudgetType}.disable();}"></Deselect>
                                 </Listeners>
                         </ext:RowSelectionModel>
                            </SelectionModel>
                            <BottomBar>
                                <ext:PagingToolbar ID="uxOrganizationGridPageBar" runat="server" />
                            </BottomBar>   
                    <View>
                        <ext:GridView ID="uxOrganizationsGridView" StripeRows="true" runat="server">
                        </ext:GridView>
                    </View>                       
                        </ext:GridPanel>

                </Items>
            </ext:Viewport>
    </form>
</body>
</html>

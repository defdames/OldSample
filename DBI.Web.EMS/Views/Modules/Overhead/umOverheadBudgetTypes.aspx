<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umOverheadBudgetTypes.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umOverheadBudgetTypes_" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
      <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form2" runat="server">
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout" Namespace="App">
            <Items>
                <ext:GridPanel ID="uxBudgetTypeGridPanel" runat="server" Flex="1" SimpleSelect="true" Header="false" Padding="5" Region="Center">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxAssignBudgetType" Icon="ApplicationAdd" Text="New Budget Type" Disabled="true">
                                     <DirectEvents>
                                        <Click OnEvent="deAddBudgetType" ><EventMask ShowMask="true"></EventMask></Click>
                                    </DirectEvents>
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip1" runat="server" Html="This allows you to assign a budget type to this organization" UI="Info"></ext:ToolTip>
                                    </ToolTips>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxDeleteBudgetType" Icon="ApplicationDelete" Text="Remove Type" Disabled="true">
                                     <DirectEvents>
                                        <Click OnEvent="deDeleteBudgetType" ><EventMask ShowMask="true"></EventMask><Confirmation ConfirmRequest="true" Message="Are you sure you want to remove this budget type?" /></Click>
                                    </DirectEvents>
                                      <ToolTips>
                                        <ext:ToolTip ID="ToolTip2" runat="server" Html="This allows you to unassign a budget type from this organization" UI="Info"></ext:ToolTip>
                                    </ToolTips>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxBudgetTypeStore"
                            AutoDataBind="true" RemoteSort="true" AutoLoad="false" OnReadData="deReadBudgetTypesByLegalEntity">
                            <Model>
                                        <ext:Model ID="Model2" runat="server" IDProperty="OVERHEAD_BUDGET_TYPE_ID">
                                            <Fields>
                                                <ext:ModelField Name="OVERHEAD_BUDGET_TYPE_ID"  />
                                                <ext:ModelField Name="PARENT_BUDGET_TYPE_ID"  />
                                                <ext:ModelField Name="BUDGET_NAME" />
                                                <ext:ModelField Name="BUDGET_DESCRIPTION" />
                                                <ext:ModelField Name="LE_ORG_ID" />
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
                                    <ext:Column ID="Column2" runat="server" DataIndex="BUDGET_NAME" Text="Budget Type" Flex="1" />
                                    <ext:Column ID="Column1" runat="server" DataIndex="BUDGET_DESCRIPTION" Text="Description" Flex="1" />
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

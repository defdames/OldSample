<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umBudgetTypesByLegalEntity.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.Views.umBudgetTypesByLegalEntity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
        <ext:Viewport runat="server" Layout="BorderLayout" Namespace="App">
            <Items>
                <ext:TreePanel ID="uxLegalEntityTreePanel"
                    runat="server"
                    Title="Overhead Legal Entities"
                    Width="300"
                    RootVisible="false"
                    SingleExpand="true"
                    Lines="false"
                    FolderSort="true"
                    Margin="5"
                    UseArrows="true"
                    Region="West"
                    Scroll="Vertical" Collapsible="false">
                    <Tools>
                        <ext:Tool runat="server" Type="Help">
                            <ToolTips>
                                <ext:ToolTip ID="ToolTip1" runat="server" Html="These are legal entities that have budget types assigned in oracle. If the legal entity is not displayed you must create a budget type for it in oracle and assign it to that organization." UI="Info">
                                </ext:ToolTip>
                            </ToolTips>
                        </ext:Tool>
                    </Tools>
                    <ToolTips>
                    </ToolTips>
                    <Store>
                        <ext:TreeStore ID="uxLegalEntityGridStore" runat="server" OnReadData="deLoadOverheadLegalEntities">
                            <Proxy>
                                <ext:PageProxy></ext:PageProxy>
                            </Proxy>
                            <DirectEvents>
                                <BeforeLoad>
                                    <EventMask ShowMask="true"></EventMask>
                                </BeforeLoad>
                            </DirectEvents>
                        </ext:TreeStore>
                    </Store>
                    <Root>
                        <ext:Node NodeID="0" Text="All Legal Entities" Expanded="true" />
                    </Root>
                    <SelectionModel>
                        <ext:TreeSelectionModel ID="uxLegalEntityTreeSelectionModel" runat="server" Mode="Single">
                        </ext:TreeSelectionModel>
                    </SelectionModel>
                     <DirectEvents>
                        <ItemClick OnEvent="deShowBudgetTypesByLegalEntity">
                            <ExtraParams>
                                <ext:Parameter Name="id" Value="record.data.id" Mode="Raw" />
                            </ExtraParams>
                        </ItemClick>
                    </DirectEvents>
                </ext:TreePanel>

                 <ext:GridPanel ID="uxBudgetTypeGridPanel" runat="server" Flex="1" SimpleSelect="true" Title="Budget Types By Legal Entity" Padding="5" Region="Center">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxAssignBudgetType" Icon="Add" Text="Assign" Disabled="true">
                                     <DirectEvents>
                                        <Click OnEvent="deAddEditBudgetType" ><EventMask ShowMask="true"></EventMask></Click>
                                    </DirectEvents>
                                </ext:Button>
                                 <ext:Button runat="server" ID="uxUnAssignBudgetType" Icon="Delete" Text="UnAssign" Disabled="true">
                                     <DirectEvents>
                                         <Click OnEvent="deUnassignBudgetType"><EventMask ShowMask="true"></EventMask><Confirmation ConfirmRequest="true" Message="Are you sure you want to unassign this budget type from this organization?"></Confirmation></Click>
                                     </DirectEvents>
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
                                                <ext:ModelField Name="CHILD_BUDGET_NAME"  />
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
                                     <ext:Column ID="Column3" runat="server" DataIndex="CHILD_BUDGET_NAME" Text="Next Budget Type" Flex="1" />
                                </Columns>
                            </ColumnModel>
                            <Plugins>
                                <ext:FilterHeader ID="uxBudgetTypeGridFilter" runat="server" Remote="true" />
                            </Plugins>
                            <SelectionModel>
                                <ext:RowSelectionModel runat="server" Mode="Single" ID="uxBudgetTypeSelectionModel">
                                    <Listeners>
                                        <Select Handler="if(#{uxBudgetTypeSelectionModel}.getCount() > 0){#{uxUnAssignBudgetType}.enable();}else {#{uxUnAssignBudgetType}.disable();}"></Select>
                                        <Deselect Handler="if(#{uxBudgetTypeSelectionModel}.getCount() > 0){#{uxUnAssignBudgetType}.enable();}else {#{uxUnAssignBudgetType}.disable();}"></Deselect>
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

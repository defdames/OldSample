<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umBudgetTypes.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umBudgetTypes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
      
        <ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout" IDMode="Explicit" Namespace="App" RenderXType="True">
            <Items>
                <ext:TreePanel ID="uxLegalEntityTreePanel"
                    runat="server"
                    Title="Legal Entities"
                    Width="300"
                    RootVisible="false"
                    SingleExpand="true"
                    Lines="false"
                    FolderSort="true"
                    Margin="5"
                    UseArrows="true"
                    Region="West"
                    Scroll="Vertical" Collapsible="false">
                    <Store>
                        <ext:TreeStore ID="uxLegalEntityGridStore" runat="server" OnReadData="LoadLegalEntitiesTreePanel">
                            <Proxy>
                                <ext:PageProxy></ext:PageProxy>
                            </Proxy>
                             <DirectEvents>
                                <BeforeLoad><EventMask ShowMask="true"></EventMask></BeforeLoad>
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
                        <ItemClick OnEvent="deShowBudgetTypesByBusinessUnit">
                            <ExtraParams>
                                <ext:Parameter Name="id" Value="record.data.id" Mode="Raw" />
                            </ExtraParams>
                        </ItemClick>
                    </DirectEvents>
                    <Listeners>
                        <Select Handler="#{uxEditBudgetType}.disable();"></Select>
                    </Listeners>
                </ext:TreePanel>
     
                <ext:GridPanel ID="uxBudgetTypeGridPanel" runat="server" Flex="1" SimpleSelect="true" Title="Budget Types By Business Unit" Padding="5" Region="Center">
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxAddBudgetType" Icon="ApplicationAdd" Text="Add" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deAddEditBudgetType" />
                                    </DirectEvents>
                                </ext:Button>
                                 <ext:Button runat="server" ID="uxEditBudgetType" Icon="ApplicationEdit" Text="Edit" Disabled="true">
                                      <DirectEvents>
                                        <Click OnEvent="deAddEditBudgetType" >
                                            <ExtraParams>
                                                <ext:Parameter Name="Edit" Value="True"></ext:Parameter>
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                 </ext:Button>
                                 <ext:Button runat="server" ID="uxDeleteBudgetType" Text="Delete" Icon="ApplicationDelete" Disabled="true">
                            <DirectEvents>
                                <Click OnEvent="deDeleteBudgetType" Success="#{uxBudgetTypeGridPanel}.getStore().load();"><Confirmation ConfirmRequest="true" Message="Are you sure you want to delete the selected budget type(s)?"></Confirmation><EventMask ShowMask="true"></EventMask></Click>
                            </DirectEvents>
                        </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxBudgetTypeStore"
                            AutoDataBind="true" RemoteSort="true" PageSize="10" OnReadData="deReadBudgetTypesByOrganization" AutoLoad="false">
                            <Model>
                                        <ext:Model ID="Model2" runat="server" IDProperty="OVERHEAD_BUDGET_TYPE_ID">
                                            <Fields>
                                                <ext:ModelField Name="OVERHEAD_BUDGET_TYPE_ID"  />
                                                <ext:ModelField Name="PARENT_BUDGET_TYPE_ID"  />
                                                <ext:ModelField Name="PARENT_BUDGET_DESCRIPTION"  />
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
                                    <ext:Column ID="Column3" runat="server" DataIndex="PARENT_BUDGET_DESCRIPTION" Text="Parent" Flex="1" />
                                </Columns>
                            </ColumnModel>
                            <Plugins>
                                <ext:FilterHeader ID="uxBudgetTypeGridFilter" runat="server" Remote="true" />
                            </Plugins>
                            <SelectionModel>
                                <ext:RowSelectionModel runat="server" Mode="Simple" ID="uxBudgetTypeSelectionModel">
                                    <Listeners>
                                        <Select Handler="if(#{uxBudgetTypeSelectionModel}.getCount() > 0){#{uxEditBudgetType}.enable();#{uxDeleteBudgetType}.enable();}else {#{uxEditBudgetType}.disable();#{uxDeleteBudgetType}.disable();}"></Select>
                                        <Deselect Handler="if(#{uxBudgetTypeSelectionModel}.getCount() > 0){#{uxEditBudgetType}.enable();#{uxDeleteBudgetType}.enable();} else {#{uxEditBudgetType}.disable();#{uxDeleteBudgetType}.disable();}"></Deselect>
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

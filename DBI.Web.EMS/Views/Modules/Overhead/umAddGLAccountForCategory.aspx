<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddGLAccountForCategory.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umAddGLAccountForCategory" %>

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
                <ext:GridPanel ID="uxGLAccountListGridPanel" runat="server" Flex="1"  Padding="5" Margins="5 5 5 5" Region="Center" Title="General Ledger Accounts" Header="false">
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxAssignAccountsToCategory" Icon="Accept" Text="Add Accounts" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deSaveAccountsToCategory" Success ="parent.Ext.getCmp('uxAccountMaintenanceWindow').close();"><Confirmation ConfirmRequest="true" Message="Are you sure you want to assign these accounts for this category?"></Confirmation><EventMask ShowMask="true"></EventMask></Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxCloseForm" Icon="Cancel" Text="Close Form">
                                    <Listeners>
                                        <Click Handler="parent.Ext.getCmp('uxAccountMaintenanceWindow').close();"></Click>
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxGLAccountListStore"
                            AutoDataBind="true" RemoteSort="true" AutoLoad="true" OnReadData="uxGLAccountListStore_ReadData">
                            <Model>
                                <ext:Model ID="Model3" runat="server" IDProperty="SEGMENT5">
                                    <Fields>
                                         <ext:ModelField Name="SEGMENT5" />
                                        <ext:ModelField Name="SEGMENT5_DESC" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Sorters>
                                <ext:DataSorter Direction="ASC" Property="SEGMENT5_DESC"></ext:DataSorter>
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column6" runat="server" DataIndex="SEGMENT5" Text="Account Name" Flex="1" />
                            <ext:Column ID="Column5" runat="server" DataIndex="SEGMENT5_DESC" Text="Account Description" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar2" runat="server" />
                    </BottomBar>
                    <View>
                        <ext:GridView ID="GridView2" StripeRows="true" runat="server">
                        </ext:GridView>
                    </View>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" Mode="Simple">
                             <Listeners>
                                 <Select Handler="if(#{CheckboxSelectionModel1}.getCount() > 0 ){#{uxAssignAccountsToCategory}.enable();}else {#{uxAssignAccountsToCategory}.disable();}"></Select>
                                        <Deselect Handler="if(#{CheckboxSelectionModel1}.getCount() > 0 ){#{uxAssignAccountsToCategory}.enable();}else {#{uxAssignAccountsToCategory}.disable();}"></Deselect>
                                 </Listeners>
                        </ext:CheckboxSelectionModel>
                    </SelectionModel>
                     <Plugins>
                        <ext:FilterHeader ID="uxOrganizationsGridFilter" runat="server" Remote="true" />
                    </Plugins>
                </ext:GridPanel>
                </Items>
            </ext:Viewport>
    </form>
</body>
</html>

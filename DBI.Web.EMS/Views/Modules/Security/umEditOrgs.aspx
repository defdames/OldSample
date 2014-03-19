<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEditOrgs.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Security.umEditOrgs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../../../Resources/Scripts/functions.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" />
        <ext:Viewport runat="server" ID="uxOrgsViewport" Layout="BorderLayout">
            <Items>
                <ext:GridPanel runat="server" ID="uxUsersGrid" Region="North" Layout="HBoxLayout">
                    <Store>
                        <ext:Store runat="server" OnReadData="deReadUsers" PageSize="10" AutoDataBind="true" ID="uxUsersStore" RemoteSort="true">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="USER_ID" />
                                        <ext:ModelField Name="USER_NAME" />
                                        <ext:ModelField Name="EMPLOYEE_NAME" />
                                        <ext:ModelField Name="EMPLOYEE_NUMBER" />
                                        <ext:ModelField Name="CURRENT_ORGANIZATION" />
                                        <ext:ModelField Name="JOB_NAME" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Sorters>
                                <ext:DataSorter Property="USER_NAME" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="User Id" DataIndex="USER_ID" Flex="1" />
                            <ext:Column runat="server" Text="User Name" DataIndex="USER_NAME" Flex="1" />
                            <ext:Column runat="server" Text="Employee Name" DataIndex="EMPLOYEE_NAME" Flex="1" />
                            <ext:Column runat="server" Text="Employee Number" DataIndex="EMPLOYEE_NUMBER" Flex="1" />
                            <ext:Column runat="server" Text="Organization" DataIndex="CURRENT_ORGANIZATION" Flex="1" />
                            <ext:Column runat="server" Text="Job Name" DataIndex="JOB_NAME" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                    </Plugins>
                    <BottomBar>
                        <ext:PagingToolbar runat="server" />
                    </BottomBar>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" Text="Edit User Orgs" Icon="ApplicationEdit">
                                    <DirectEvents>
                                        <Click OnEvent="deLoadUpdateOrgWindow" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:GridPanel>
                <ext:Panel runat="server" ID="uxTwoGridPanel" Region="Center" Layout="HBoxLayout">
                    <Items>
                        <ext:GridPanel runat="server" ID="uxAvailableOrgsGrid" Layout="HBoxLayout" Title="Available Orgs">
                            <Store>
                                <ext:Store runat="server" ID="uxAvailableOrgsStore">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                            <ColumnModel>
                                <Columns>
                                    
                                </Columns>
                            </ColumnModel>
                        </ext:GridPanel>
                        <ext:Panel runat="server" ID="uxButtonsPanel" Layout="VBoxLayout">
                            <Items>
                                <ext:Button runat="server" Icon="ResultsetNext" StyleSpec="margin-bottom:2px;">
                            <Listeners>
                                <Click Handler="TwoGridSelector.add(uxAvailableOrgsGrid, uxSelectedOrgsGrid);" />
                            </Listeners>
                            <ToolTips>
                                <ext:ToolTip runat="server" Title="Add" Html="Add Selected Rows" />
                            </ToolTips>
                        </ext:Button>
                        <ext:Button runat="server" Icon="ResultsetLast" StyleSpec="margin-bottom:2px;">
                            <Listeners>
                                <Click Handler="TwoGridSelector.addAll(uxAvailableOrgsGrid, uxSelectedOrgsGrid);" />
                            </Listeners>
                            <ToolTips>
                                <ext:ToolTip runat="server" Title="Add all" Html="Add All Rows" />
                            </ToolTips>
                        </ext:Button>
                        <ext:Button runat="server" Icon="ResultsetPrevious" StyleSpec="margin-bottom:2px;">
                            <Listeners>
                                <Click Handler="TwoGridSelector.remove(uxAvailableOrgsGrid, uxSelectedOrgsGrid);" />
                            </Listeners>
                            <ToolTips>
                                <ext:ToolTip runat="server" Title="Remove" Html="Remove Selected Rows" />
                            </ToolTips>
                        </ext:Button>
                        <ext:Button runat="server" Icon="ResultsetFirst" StyleSpec="margin-bottom:2px;">
                            <Listeners>
                                <Click Handler="TwoGridSelector.removeAll(uxAvailableOrgsGrid, uxSelectedOrgsGrid);" />
                            </Listeners>
                            <ToolTips>
                                <ext:ToolTip runat="server" Title="Remove all" Html="Remove All Rows" />
                            </ToolTips>
                        </ext:Button>
                            </Items>
                        </ext:Panel>
                        <ext:GridPanel runat="server" ID="uxSelectedOrgsGrid" Layout="HBoxLayout" Title="Available Orgs">
                            <Store>
                                <ext:Store runat="server">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                            <ColumnModel>
                                <Columns>
                                    
                                </Columns>
                            </ColumnModel>
                        </ext:GridPanel>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

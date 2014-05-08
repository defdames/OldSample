<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEditUsers.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Security.umEditUsers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../../../Resources/Scripts/functions.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" Namespace="" />
        <ext:Viewport runat="server" ID="uxEditUsersViewPort">
            <Items>
                <ext:GridPanel runat="server" ID="uxUsersGrid" Layout="HBoxLayout">
                    <Store>
                        <ext:Store runat="server" ID="uxUsersStore"
                            AutoDataBind="true" OnReadData="deReadUsers" PageSize="25" RemoteSort="true" RemotePaging="true">
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
                            <ext:Column runat="server" Text="Employee Name" DataIndex="EMPLOYEE_NAME" Flex="1"/>
                            <ext:Column runat="server" Text="Employee Number" DataIndex="EMPLOYEE_NUMBER" Flex="1" />
                            <ext:Column runat="server" Text="Organization" DataIndex="CURRENT_ORGANIZATION" Flex="1" />
                            <ext:Column runat="server" Text="Job Name" DataIndex="JOB_NAME" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                    </Plugins>
                    <BottomBar>
                        <ext:PagingToolbar ID="uxUsersPagingToolbar" runat="server" />
                    </BottomBar>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxEditUserButton" Text="Edit User" Icon="UserEdit" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deLoadEditUserForm">
                                            <ExtraParams>
                                                <ext:Parameter Value="#{uxUsersGrid}.getSelectionModel().getSelection()[0].data.USER_ID" Mode="Raw" Name="UserId" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" Text="Edit User Orgs" Icon="ApplicationEdit" Disabled="true" ID="uxEditOrgsButton">
                                    <DirectEvents>
                                        <Click OnEvent="deLoadUpdateOrgWindow">
                                            <ExtraParams>
                                                <ext:Parameter Name="UserId"  Value="#{uxUsersGrid}.getSelectionModel().getSelection()[0].data.USER_ID" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>                    
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxImpersonateButton" Text="Impersonate User" Icon="UserGo" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deImpersonate">
                                            <Confirmation ConfirmRequest="true" Message="Are you sure you want to Impersonate this user?" />
                                            <ExtraParams>
                                                <ext:Parameter Value="#{uxUsersGrid}.getSelectionModel().getSelection()[0].data.USER_ID" Mode="Raw" Name="UserId" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Listeners>
                        <Select Handler="#{uxEditUserButton}.enable();#{uxEditOrgsButton}.enable();#{uxImpersonateButton}.enable();" />
                    </Listeners>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
        <!--Hidden Window -->
        <ext:Window runat="server" ID="uxEditUserWindow" Hidden="true" Width="650">
            <Items>
                <ext:FormPanel runat="server" ID="uxEditUserForm">
                    <Items>
                        <ext:GridPanel runat="server" ID="uxEditUserGrid" Layout="HBoxLayout">
                            <Store>
                                <ext:Store runat="server" ID="uxEditUserStore"
                                    AutoDataBind="true">
                                    <Model>
                                        <ext:Model runat="server" IDProperty="PERMISSION_NAME">
                                            <Fields>
                                                <ext:ModelField Name="PERMISSION_ID" />
                                                <ext:ModelField Name="PERMISSION_NAME" Type="String"  />
                                                <ext:ModelField Name="PARENT_PERM_ID" />
                                                <ext:ModelField Name="DESCRIPTION" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Sorters>
                                        <ext:DataSorter Property="PERMISSION_NAME" Direction="ASC" />
                                    </Sorters>
                                </ext:Store>
                            </Store>
                            <ColumnModel>
                                <Columns>
                                    <ext:Column runat="server" Text="Name" DataIndex="PERMISSION_NAME" Flex="1" />
                                    <ext:Column runat="server" Text="Description" DataIndex="DESCRIPTION" Flex="1" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:CheckboxSelectionModel runat="server" AllowDeselect="true" Mode="Multi" />
                            </SelectionModel>
                        </ext:GridPanel>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" Text="Submit" Icon="Add">
                            <DirectEvents>
                                <Click OnEvent="deUpdateUserPermissions">
                                    <ExtraParams>
                                        <ext:Parameter Name="Rows" Value="Ext.encode(#{uxEditUserGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                                        <ext:Parameter Value="#{uxUsersGrid}.getSelectionModel().getSelection()[0].data.USER_ID" Mode="Raw" Name="UserId" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="#{uxEditUserGrid}.getSelectionModel().deselectAll(); #{uxEditUserWindow}.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>

                </ext:FormPanel>
            </Items>
        </ext:Window>
        <ext:Window runat="server" ID="uxTwoGridWindow" Layout="HBoxLayout" Width="700" Height="600" Hidden="true">
            <LayoutConfig>
                <ext:HBoxLayoutConfig Align="Stretch" />
            </LayoutConfig>
            <Items>
                <ext:GridPanel runat="server" ID="uxAvailableOrgsGrid" Title="Available Orgs" Flex="1">
                    <Store>
                        <ext:Store runat="server" ID="uxAvailableOrgsStore">
                            <Model>
                                <ext:Model ID="Model1" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ORG_HIER" />
                                        <ext:ModelField Name="ORG_ID" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" 
                                runat="server" 
                                Width="40" 
                                Sortable="false" />
                            <ext:Column ID="Column1" runat="server" Text="Name" DataIndex="ORG_HIER" Flex="1" />
                            <ext:Column ID="Column2" runat="server" Text="Business Unit" DataIndex="ORG_ID" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:BufferedRenderer ID="BufferedRenderer1" runat="server" />
                        <ext:FilterHeader ID="FilterHeader1" runat="server" />
                    </Plugins>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" TrackOver="false" />
                    </View>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" PruneRemoved="false" Mode="Multi" />
                    </SelectionModel>
                </ext:GridPanel>
                        
                <ext:Panel runat="server" ID="uxButtonsPanel" Layout="VBoxLayout">
                    <Items>
                        <ext:Button ID="Button2" runat="server" Icon="ResultsetNext" StyleSpec="margin-bottom:2px;">
                    <Listeners>
                        <Click Handler="TwoGridSelector.add(uxAvailableOrgsGrid, uxSelectedOrgsGrid);" />
                    </Listeners>
                    <ToolTips>
                        <ext:ToolTip ID="ToolTip1" runat="server" Title="Add" Html="Add Selected Rows" />
                    </ToolTips>
                </ext:Button>
                <ext:Button ID="Button3" runat="server" Icon="ResultsetLast" StyleSpec="margin-bottom:2px;">
                    <Listeners>
                        <Click Handler="TwoGridSelector.addAll(uxAvailableOrgsGrid, uxSelectedOrgsGrid);" />
                    </Listeners>
                    <ToolTips>
                        <ext:ToolTip ID="ToolTip2" runat="server" Title="Add all" Html="Add All Rows" />
                    </ToolTips>
                </ext:Button>
                <ext:Button ID="Button4" runat="server" Icon="ResultsetPrevious" StyleSpec="margin-bottom:2px;">
                    <Listeners>
                        <Click Handler="TwoGridSelector.remove(uxAvailableOrgsGrid, uxSelectedOrgsGrid);" />
                    </Listeners>
                    <ToolTips>
                        <ext:ToolTip ID="ToolTip3" runat="server" Title="Remove" Html="Remove Selected Rows" />
                    </ToolTips>
                </ext:Button>
                <ext:Button ID="Button5" runat="server" Icon="ResultsetFirst" StyleSpec="margin-bottom:2px;">
                    <Listeners>
                        <Click Handler="TwoGridSelector.removeAll(uxAvailableOrgsGrid, uxSelectedOrgsGrid);" />
                    </Listeners>
                    <ToolTips>
                        <ext:ToolTip ID="ToolTip4" runat="server" Title="Remove all" Html="Remove All Rows" />
                    </ToolTips>
                </ext:Button>
                    </Items>
                </ext:Panel>
                <ext:GridPanel runat="server" ID="uxSelectedOrgsGrid" Title="Selected Orgs" Flex="1" AutoScroll="true">
                    <Store>
                        <ext:Store runat="server" ID="uxSelectedOrgsStore">
                            <Model>
                                <ext:Model ID="Model2" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ORG_HIER" />
                                        <ext:ModelField Name="ORG_ID" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <Plugins>
                        <ext:BufferedRenderer ID="BufferedRenderer2" runat="server" />
                        <ext:FilterHeader ID="FilterHeader2" runat="server" />
                    </Plugins>
                    <ColumnModel ID="ColumnModel2" runat="server">
                        <Columns>
                            <ext:Column ID="Column3" runat="server" Text="Name" DataIndex="ORG_HIER" Flex="1" />
                            <ext:Column ID="Column4" runat="server" Text="Business Unit" DataIndex="ORG_ID" Flex="1" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
            </Items>
            <Buttons>
                <ext:Button runat="server" ID="uxSaveOrgsButton" Text="Submit" Icon="Add">
                    <DirectEvents>
                        <Click OnEvent="deSaveUserOrgs">
                            <ExtraParams>
                                <ext:Parameter Name="SelectedOrgs" Value="Ext.encode(#{uxSelectedOrgsGrid}.getRowsValues({selectedOnly: false}))" Mode="Raw" />
                                <ext:Parameter Name="NotSelectedOrgs" Value ="Ext.encode(#{uxAvailableOrgsGrid}.getRowsValues({selectedOnly: false}))" Mode="Raw" />
                                <ext:Parameter Name="UserId"  Value="#{uxUsersGrid}.getSelectionModel().getSelection()[0].data.USER_ID" Mode="Raw" />
                            </ExtraParams>
                            <EventMask ShowMask="true" />
                        </Click>
                    </DirectEvents>
                </ext:Button>
                <ext:Button runat="server" ID="uxCancelOrgsButton" Text="Cancel" Icon="Delete">
                    <Listeners>
                        <Click Handler="TwoGridSelector.removeAll(uxAvailableOrgsGrid, uxSelectedOrgsGrid); uxTwoGridWindow.hide()" />
                    </Listeners>
                </ext:Button>
            </Buttons> 
        </ext:Window>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEditUsers.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Security.umEditUsers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" />
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
                                <ext:Button runat="server" ID="uxEditUserButton" Text="Edit User">
                                    <DirectEvents>
                                        <Click OnEvent="deLoadEditUserForm">
                                            <ExtraParams>
                                                <ext:Parameter Value="#{uxUsersGrid}.getSelectionModel().getSelection()[0].data.USER_ID" Mode="Raw" Name="UserId" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxImpersonateButton" Text="Impersonate User">
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
    </form>
</body>
</html>

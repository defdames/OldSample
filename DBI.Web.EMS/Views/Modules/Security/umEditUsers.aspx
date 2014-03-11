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
                <ext:GridPanel runat="server" ID="uxUsersGrid">
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
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="User Id" DataIndex="USER_ID" />
                            <ext:Column runat="server" Text="User Name" DataIndex="USER_NAME" />
                            <ext:Column runat="server" Text="Employee Name" DataIndex="EMPLOYEE_NAME" />
                            <ext:Column runat="server" Text="Employee Number" DataIndex="EMPLOYEE_NUMBER" />
                            <ext:Column runat="server" Text="Organization" DataIndex="CURRENT_ORGANIZATION" />
                            <ext:Column runat="server" Text="Job Name" DataIndex="JOB_NAME" />
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
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
        <!--Hidden Window -->
        <ext:Window runat="server" ID="uxEditUserWindow" Hidden="true">
            <Items>
                <ext:FormPanel runat="server" ID="uxEditUserForm">
                    <Items>
                        <ext:GridPanel runat="server" ID="uxEditUserGrid">
                            <Store>
                                <ext:Store runat="server" ID="uxEditUserStore"
                                    AutoDataBind="true">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="PERMISSION_NAME" />
                                                <ext:ModelField Name="DESCRIPTION" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                            <ColumnModel>
                                <Columns>
                                    <ext:Column runat="server" Text="Name" DataIndex="PERMISSION_NAME" />
                                    <ext:Column runat="server" Text="Description" DataIndex="DESCRIPTION" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:CheckboxSelectionModel runat="server" AllowDeselect="true" />
                            </SelectionModel>
                        </ext:GridPanel>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" Text="Submit" Icon="Add">
                            <DirectEvents>
                                <Click OnEvent="deUpdateUserPermissions">

                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="#{uxEditUserGrid}.getSelectionModel().clearSelections(); #{uxEditUserWindow}.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>

                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>

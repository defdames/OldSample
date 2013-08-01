<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSecurityUsersList.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Security.umSecurityUsersList" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!-- This code is required in order to show a loading/reloading mask on the gridpanel durning a refresh that is not done via proxy -->
    <script type="text/javascript">
        var refreshHandler = function () {
            Ext.net.Mask.show({
                el: this.up('gridpanel'),
                msg: 'Loading...'
            });
            this.getStore().reload({
                callback: function () {
                    Ext.net.Mask.hide();
                }
            });
        };
    </script>
</head>
<body>
    <ext:ResourceManager runat="server" IsDynamic="False" />
    <form runat="server">
        <ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout" IDMode="Explicit" IsDynamic="False" Namespace="App" RenderXType="True">
            <Items>
                <ext:GridPanel ID="uxSecurityUserGridPanel"
                    runat="server"
                    Title="Security Users"
                    Padding="5"
                    Icon="User"
                    Region="Center"
                    Height="300"
                    Frame="true"
                    Margins="5 0 5 5" meta:resourcekey="uxSecurityUserGridPanelResource1" SelectionMemory="false">
                    <TopBar>
                        <ext:Toolbar runat="server" ID="uxSecurityUserGridPanelToolbar">
                            <Items>
                                <ext:Button runat="server" Text="Edit User" Icon="UserEdit" Disabled="true" ID="uxEditUser">
                                    <DirectEvents>
                                        <Click OnEvent="deEditUser">
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarSpacer runat="server"></ext:ToolbarSpacer>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store
                            ID="uxSecurityUserStore"
                            runat="server"
                            RemoteSort="true"
                            PageSize="25"
                            OnReadData="deUsersDataBind">
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Model>
                                <ext:Model ID="uxSecurityAddUserModel" runat="server" IDProperty="USER_ID">
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
                            <Sorters>
                                <ext:DataSorter Property="USER_NAME" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="uxSecurityAddUserColumns" runat="server">
                        <Columns>
                            <ext:Column ID="cUsername" runat="server" DataIndex="USER_NAME" Text="Username" Width="150" />
                            <ext:Column ID="cEmployeeName" runat="server" DataIndex="EMPLOYEE_NAME" Text="Employee Name" Flex="1" />
                            <ext:Column ID="cEmployeeNumber" runat="server" DataIndex="EMPLOYEE_NUMBER" Text="Employee Number" Flex="1" />
                            <ext:Column ID="cOrganization" runat="server" DataIndex="CURRENT_ORGANIZATION" Text="Organization" Flex="1" />
                            <ext:Column ID="cJobTitle" runat="server" DataIndex="JOB_NAME" Text="Job Title" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Features>
                        <ext:GridFilters ID="uxSecurityUserFilters" runat="server">
                            <Filters>
                                <ext:StringFilter DataIndex="USER_NAME" />
                                <ext:StringFilter DataIndex="EMPLOYEE_NAME" />
                                <ext:StringFilter DataIndex="EMPLOYEE_NUMBER" />
                                <ext:StringFilter DataIndex="CURRENT_ORGANIZATION" />
                                <ext:StringFilter DataIndex="JOB_NAME" />
                            </Filters>
                        </ext:GridFilters>
                    </Features>
                    <BottomBar>
                        <ext:PagingToolbar ID="uxSecurityUserPaging" runat="server" meta:resourcekey="uxSecurityUserPagingResource1" />
                    </BottomBar>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server">
                            <Listeners>
                                <Select Handler="#{uxEditUser}.enable();"></Select>
                                <Deselect Handler="#{uxEditUser}.disable();"></Deselect>
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>


        <!-- Hidden Windows (Edit User) -->
        <ext:Window runat="server" Resizable="false" Icon="UserEdit" DefaultButton="uxSaveUser" Hidden="true" Width="600" Height="500" Layout="BorderLayout" Header="true" Title="System User Maintenance" ID="uxSecurityUserDetailsWindow" Closable="true" CloseAction="Hide" Modal="true">
            <Items>
                <ext:FormPanel
                    ID="uxSecurityUserDetails"
                    runat="server"
                    Margins="5 5 5 5"
                    BodyPadding="2"
                    Frame="true"
                    DefaultAnchor="100%"
                    AutoScroll="True"
                    Height="150"
                    Region="North">
                    <Items>
                        <ext:TextField Name="USER_ID" ID="uxUserID" Hidden="true" runat="server" LabelWidth="130"></ext:TextField>
                        <ext:TextField Name="USER_NAME" ID="uxName" runat="server" FieldLabel="Username" ReadOnly="true" LabelWidth="130" />
                        <ext:TextField Name="EMPLOYEE_NAME" ID="uxEmployeeName" runat="server" FieldLabel="Employee Name" ReadOnly="true" LabelWidth="130" />
                        <ext:TextField Name="EMPLOYEE_NUMBER" ID="uxEmployeeNumber" runat="server" FieldLabel="Employee Number" ReadOnly="true" LabelWidth="130" />
                        <ext:TextField Name="CURRENT_ORGANIZATION" ID="uxOrganization" runat="server" FieldLabel="Organization" ReadOnly="true" LabelWidth="130" />
                        <ext:TextField Name="JOB_NAME" ID="uxJobName" runat="server" FieldLabel="Job Name" ReadOnly="true" LabelWidth="130" />
                    </Items>
                </ext:FormPanel>
                <ext:GridPanel ID="uxSecurityRoleGridPanel"
                    runat="server"
                    BodyPadding="2"
                    Frame="true"
                    Header="false"
                    Region="Center"
                    Margins="0 5 5 5"
                    SelectionMemory="false"
                    Height="350">
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button Icon="UserAdd" ID="uxAddRole" runat="server" Text="Add Role">
                                    <DirectEvents>
                                        <Click OnEvent="deShowAddUserRole"></Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarSpacer runat="server"></ext:ToolbarSpacer>
                                <ext:Button ID="uxDeleteRole" Icon="UserDelete" runat="server" Text="Delete Role" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deDeleteUserRole"><Confirmation ConfirmRequest="true" Message="Are you sure you want to delete this user role?"></Confirmation></Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store
                            ID="uxSecurityRoleStore"
                            runat="server"
                            OnReadData="deReloadUserSecurity">
                            <Model>
                                <ext:Model ID="uxSecurityRoleModel" runat="server" IDProperty="USER_ROLE_ID">
                                    <Fields>
                                        <ext:ModelField Name="USER_ROLE_ID" Type="Int" />
                                        <ext:ModelField Name="NAME" Type="String" />
                                        <ext:ModelField Name="DESCRIPTION" Type="String" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="NAME" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="uxSecurityRoleColumns" runat="server">
                        <Columns>
                            <ext:Column ID="cName" runat="server" DataIndex="NAME" Text="Name" Width="130" />
                            <ext:Column ID="cDescription" runat="server" DataIndex="DESCRIPTION" Text="Description" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Features>
                        <ext:GridFilters runat="server" ID="uxSecurityRoleGridFilters" Local="true">
                            <Filters>
                                <ext:StringFilter DataIndex="NAME" />
                                <ext:StringFilter DataIndex="DESCRIPTION" />
                            </Filters>
                        </ext:GridFilters>
                    </Features>
                    <BottomBar>
                        <ext:PagingToolbar ID="uxSecurityRolePaging" runat="server" RefreshHandler="refreshHandler" />
                    </BottomBar>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel runat="server" Mode="Single" ShowHeaderCheckbox="false" AllowDeselect="true">
                            <Listeners>
                                <Select Handler="#{uxDeleteRole}.enable();"></Select>
                                <Deselect Handler="#{uxDeleteRole}.disable();"></Deselect>
                            </Listeners>
                        </ext:CheckboxSelectionModel>
                    </SelectionModel>
                </ext:GridPanel>
            </Items>
            <Buttons>
                <ext:Button runat="server" ID="uxCancelUser" Text="Close">
                    <Listeners>
                        <Click Handler="#{uxSecurityUserDetails}.getForm().reset();#{uxSecurityUserDetailsWindow}.close();"></Click>
                    </Listeners>
                </ext:Button>
            </Buttons>
            <Listeners>
                <Close Handler="#{uxSecurityUserDetails}.getForm().reset();"></Close>
            </Listeners>
        </ext:Window>

        <!-- Hidden Windows (Edit User Roles) -->
        <ext:Window runat="server" Resizable="false" Icon="UserEdit" Hidden="true" Width="500" Height="375" Layout="FitLayout" Header="true" Title="System User Maintenance - Security Roles" ID="uxMaintainSecurityRoles" Closable="true" CloseAction="Hide" Modal="true">
            <Items>
                <ext:GridPanel ID="uxSecurityRoleList"
                    runat="server"
                    BodyPadding="2"
                    Frame="true"
                    Header="false"
                    Region="Center"
                    Margins="5 5 5 5"
                    SelectionMemory="true">
                    <Store>
                        <ext:Store
                            ID="uxSecurityRoleListStore"
                            runat="server"
                            OnReadData="deReloadUserRoleSecurity">
                            <Model>
                                <ext:Model ID="Model1" runat="server" IDProperty="ROLE_ID">
                                    <Fields>
                                        <ext:ModelField Name="ROLE_ID" Type="Int" />
                                        <ext:ModelField Name="NAME" Type="String" />
                                        <ext:ModelField Name="DESCRIPTION" Type="String" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="NAME" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="uxSecurityRoleListColumns" runat="server">
                        <Columns>
                            <ext:Column ID="Column1" runat="server" DataIndex="NAME" Text="Name" Width="130" />
                            <ext:Column ID="Column2" runat="server" DataIndex="DESCRIPTION" Text="Description" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Features>
                        <ext:GridFilters runat="server" ID="uxSecurityRoleListFilter" Local="true">
                            <Filters>
                                <ext:StringFilter DataIndex="NAME" />
                                <ext:StringFilter DataIndex="DESCRIPTION" />
                            </Filters>
                        </ext:GridFilters>
                    </Features>
                    <BottomBar>
                        <ext:PagingToolbar ID="uxSecurityRoleListPaging" runat="server" RefreshHandler="refreshHandler" />
                    </BottomBar>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel ID="uxSecurityRoleListSelection" runat="server" Mode="Single" ShowHeaderCheckbox="false" AllowDeselect="true">
                        </ext:CheckboxSelectionModel>
                    </SelectionModel>    
                </ext:GridPanel>
            </Items> 
              <Buttons>
                <ext:Button runat="server" ID="uxAddUserRoleAdd" Text="Add Role" Icon="Disk">
                    <DirectEvents>
                        <Click OnEvent="deAddUserRole"></Click>
                    </DirectEvents>
                </ext:Button>
                <ext:Button runat="server" ID="uxCancelUserRoleAdd" Text="Cancel">
                    <Listeners>
                        <Click Handler="#{uxMaintainSecurityRoles}.close();"></Click>
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>

    </form>
</body>
</html>

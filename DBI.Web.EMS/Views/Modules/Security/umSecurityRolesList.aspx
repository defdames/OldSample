<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSecurityRolesList.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Security.umSecurityRolesList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager runat="server"  IsDynamic="False" />
    <form id="test" runat="server">
        <ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout" IDMode="Explicit" IsDynamic="False" Namespace="App" RenderXType="True">
            <Items>
                <ext:GridPanel ID="uxSecurityRoleGridPanel"
                    runat="server"
                    Title="System Roles"
                    Padding="5"
                    Icon="User"
                    Region="Center"
                    Frame="true"
                    Margins="5 5 5 5"
                    SelectionMemory="false">
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" Text="Add Role" Icon="UserAdd" ID="uxAddRole">
                                    <Listeners>
                                        <Click Handler="#{uxSecurityAddRoleWindow}.show();#{uxName}.focus();"></Click>
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSeparator runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button runat="server" Text="Edit Role" Icon="UserEdit" Disabled="true" ID="uxEditRole">
                                     <DirectEvents>
                                        <Click OnEvent="deEditRole">
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button runat="server" Text="Delete Role" Icon="UserDelete" Disabled="true" ID="uxDeleteRole">
                                    <DirectEvents>
                                        <Click OnEvent="deDeleteRole">
                                            <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete this role?"></Confirmation>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store
                            ID="uxSecurityRoleStore"
                            runat="server" 
                            RemoteSort="true" 
                            PageSize="25"
                            OnReadData="RolesDatabind">
                        <Proxy>
                        <ext:PageProxy />
                        </Proxy>
                            <Model>
                                <ext:Model ID="uxSecurityRoleModel" runat="server" IDProperty="ROLE_ID">
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
                    <ColumnModel ID="uxSecurityRoleColumns" runat="server">
                        <Columns>
                            <ext:Column ID="cName" runat="server" DataIndex="NAME" Text="Name" Width="225" />
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
                        <ext:PagingToolbar ID="uxSecurityRolePaging" runat="server" />
                    </BottomBar>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="uxSecurityRoleSelectionModel" runat="server" Mode="Single">
                            <Listeners>
                                <Select Handler="#{uxEditRole}.enable();#{uxDeleteRole}.enable();"></Select>
                                <Deselect Handler="#{uxEditRole}.disable();#{uxDeleteRole}.disable();"></Deselect>
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>



        <!-- Hidden Window for Adding Security Roles -->
        <ext:Window runat="server" Resizable="false" Icon="UserAdd" DefaultButton="uxSaveRole" Hidden="true" Width="350" Height="150" Layout="FitLayout" Header="true" Title="Security Role Maintenance" ID="uxSecurityAddRoleWindow" Closable="true" CloseAction="Hide" Modal="true">
            <Items>
                <ext:FormPanel
                    ID="uxSecurityRoleDetails"
                    runat="server"
                    Margins="5 5 5 5"
                    BodyPadding="2"
                    Frame="true"
                    DefaultAnchor="100%"
                    AutoScroll="True">
                    <Items>
                        <ext:TextField Name="ROLE_ID" id="uxRoleID" Hidden="true" runat="server"></ext:TextField>
                        <ext:TextField Name="NAME" ID="uxName" runat="server" FieldLabel="Name" />
                        <ext:TextField Name="DESCRIPTION" ID="uxDescription" runat="server" FieldLabel="Description" />
                    </Items>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button runat="server" ID="uxSaveRole" Text="Save" Icon="Disk">
                    <DirectEvents>
                        <Click OnEvent="deSaveRole">
                            <EventMask ShowMask="true"></EventMask>
                        </Click>
                    </DirectEvents>
                </ext:Button>
                <ext:Button runat="server" ID="uxCancelUserRole" Text="Cancel">
                    <Listeners>
                        <Click Handler="#{uxSecurityRoleDetails}.getForm().reset();#{uxSecurityAddRoleWindow}.close();"></Click>
                    </Listeners>
                </ext:Button>
            </Buttons>
            <Listeners>
                <Close Handler="#{uxSecurityRoleDetails}.getForm().reset();"></Close>
            </Listeners>
        </ext:Window>

</form>
</body>
</html>
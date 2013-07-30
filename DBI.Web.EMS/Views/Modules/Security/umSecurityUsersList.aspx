<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSecurityUsersList.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Security.umSecurityUsersList" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager runat="server"  IsDynamic="False" />
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
                <TopBar><ext:Toolbar runat="server" ID="uxSecurityUserGridPanelToolbar">
                    <Items>
                        <ext:Button runat="server" Text="Edit User" Icon="UserEdit" Disabled="true" ID="uxEditUser">
                            <DirectEvents>
                                        <Click OnEvent="deEditUser">
                                        </Click>
                                    </DirectEvents>
                        </ext:Button>
                        <ext:ToolbarSpacer runat="server"></ext:ToolbarSpacer>
                    </Items>
                        </ext:Toolbar></TopBar>
                <Store>
                    <ext:Store
                        ID="uxSecurityUserStore"
                        runat="server" 
                        RemoteSort="true" 
                        PageSize="25"
                        OnReadData="UsersDatabind">
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
                        <ext:Column ID="cUsername" runat="server" DataIndex="USER_NAME" Text="Username" Width="150"  />
                        <ext:Column ID="cEmployeeName" runat="server" DataIndex="EMPLOYEE_NAME" Text="Employee Name" Flex="1"  />
                        <ext:Column ID="cEmployeeNumber" runat="server" DataIndex="EMPLOYEE_NUMBER" Text="Employee Number" Flex="1"   />
                        <ext:Column ID="cOrganization" runat="server" DataIndex="CURRENT_ORGANIZATION" Text="Organization" Flex="1"   />
                        <ext:Column ID="cJobTitle" runat="server" DataIndex="JOB_NAME" Text="Job Title" Flex="1"   />
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
                <SelectionModel><ext:RowSelectionModel runat="server">
                     <Listeners>
                                <Select Handler="#{uxEditUser}.enable();"></Select>
                                <Deselect Handler="#{uxEditUser}.disable();"></Deselect>
                            </Listeners>
                                </ext:RowSelectionModel></SelectionModel>          
            </ext:GridPanel>
        </Items>
    </ext:Viewport>

        <ext:Window runat="server" Resizable="false" Icon="UserEdit" DefaultButton="uxSaveUser" Hidden="true" Width="500" Height="500" Layout="FitLayout" Header="true" Title="System User Maintenance" ID="uxSecurityUserDetailsWindow" Closable="true" CloseAction="Hide" Modal="true">
            <Items>
                <ext:FormPanel
                    ID="uxSecurityUserDetails"
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
                <ext:Button runat="server" ID="uxSaveUser" Text="Save" Icon="Disk">
                   
                </ext:Button>
                <ext:Button runat="server" ID="uxCancelUser" Text="Cancel">
                    <Listeners>
                        <Click Handler="#{uxSecurityUserDetails}.getForm().reset();#{uxSecurityUserDetailsWindow}.close();"></Click>
                    </Listeners>
                </ext:Button>
            </Buttons>
            <Listeners>
                <Close Handler="#{uxSecurityUserDetails}.getForm().reset();"></Close>
            </Listeners>
        </ext:Window>

        </form>
</body>
</html>

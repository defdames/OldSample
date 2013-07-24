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
                <SelectionModel>
                    <ext:RowSelectionModel ID="uxSecurityUserSelectionModel" runat="server" Mode="Single" meta:resourcekey="uxSecurityUserSelectionModelResource1">
                        <DirectEvents>
                            <Select OnEvent="deUserRoleSelect" Buffer="250">
                                <EventMask ShowMask="true"></EventMask>
                                <ExtraParams>
                                    <ext:Parameter Name="upUserID" Value="record.getId()" Mode="Raw" />
                                </ExtraParams>
                            </Select>
                        </DirectEvents>
                    </ext:RowSelectionModel>
                </SelectionModel>
            </ext:GridPanel>
            <ext:GridPanel ID="uxSecurityRoleGridPanel"
                runat="server"
                Title="Security Roles"
                Padding="5"
                Icon="UserBrown"
                Region="South"
                Height="250"
                Frame="true"
                Margins="0 5 5 5">
                <Store>
                    <ext:Store
                        ID="uxSecurityRoleStore"
                        runat="server">
                        <Model>
                            <ext:Model ID="uxSecurityRoleModel" runat="server" IDProperty="ROLE_ID">
                                <Fields>
                                    <ext:ModelField Name="ROLE_ID" Type="Int" />
                                    <ext:ModelField Name="NAME" Type="String" />
                                    <ext:ModelField Name="DESCRIPTION" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                        <Sorters><ext:DataSorter Property="NAME" Direction="ASC"></ext:DataSorter>
                        </Sorters>
                    </ext:Store>
                </Store>
                <ColumnModel ID="uxSecurityRoleColumns" runat="server">
                    <Columns>
                        <ext:Column ID="cName" runat="server" DataIndex="NAME" Text="Name" Width="200" />
                        <ext:Column ID="cDescription" runat="server" DataIndex="DESCRIPTION" Text="Description" Flex="1" />
                    </Columns>
                </ColumnModel>
                <Features>
                    <ext:GridFilters runat="server" ID="uxSecurityRoleGridFilters">
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
                    <ext:CheckboxSelectionModel ID="uxSecurityRoleCheckSelectionModel" Mode="Multi" runat="server" ShowHeaderCheckbox="false">
                        <DirectEvents>
                             <Select OnEvent="deAddSecurityRole" Buffer="250">
                                <EventMask ShowMask="true"></EventMask>
                                <ExtraParams>
                                    <ext:Parameter Name="epRecordID" Value="record.internalId" Mode="Raw" />
                                </ExtraParams>
                                 <Confirmation ConfirmRequest="true" Message="Are you sure you want to add this security role to the user?"></Confirmation>
                         </Select>
                            <Deselect
                             OnEvent="deDeleteSecurityRole" Buffer="250">
                                <EventMask ShowMask="true"></EventMask>
                                <ExtraParams>
                                    <ext:Parameter Name="epRecordID" Value="record.internalId" Mode="Raw" />
                                </ExtraParams>
                                <Confirmation ConfirmRequest="true" Message="Are you sure you want to remove this security role from this user?"></Confirmation>
                            </Deselect>
                        </DirectEvents>
                    </ext:CheckboxSelectionModel>
                </SelectionModel>
            </ext:GridPanel>

        </Items>
    </ext:Viewport>

        </form>

</body>
</html>

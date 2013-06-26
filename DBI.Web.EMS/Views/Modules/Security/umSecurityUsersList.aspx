<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSecurityUsersList.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Security.umSecurityUsersList" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager runat="server" ID="ctl00" IsDynamic="False" />

    <ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout" IDMode="Explicit" IsDynamic="False" Namespace="App" RenderXType="True">
        <Items>
            <ext:GridPanel ID="uxSecurityUserGridPanel"
                runat="server"
                Title="System Users"
                Padding="5"
                Icon="User"
                Region="Center"
                Height="300"
                Frame="true"
                Margins="5 0 5 5" meta:resourcekey="uxSecurityUserGridPanelResource1">
                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Button ID="uxShowAllAccounts" runat="server" Text="Show active users" meta:resourcekey="uxShowAllAccountsResource1">
                                <ToolTips>
                                    <ext:ToolTip runat="server" Html="Filter list to show only active employees" meta:resourcekey="ToolTipResource1" />
                                </ToolTips>
                                <DirectEvents>
                                    <Click OnEvent="deShowActiveEmployees">
                                        <EventMask ShowMask="true"></EventMask>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="Button1" runat="server" Text="Clear Filters" Handler="this.up('grid').filters.clearFilters();" meta:resourcekey="Button1Resource1" />
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Store>
                    <ext:Store
                        ID="uxSecurityUserStore"
                        runat="server" meta:resourcekey="uxSecurityUserStoreResource1">
                        <Model>
                            <ext:Model ID="uxSecurityAddUserModel" runat="server" IDProperty="SYSTEM_USER_ID">
                                <Fields>
                                    <ext:ModelField Name="SYSTEM_USER_ID" />
                                    <ext:ModelField Name="USER_ID" />
                                    <ext:ModelField Name="USER_NAME" />
                                    <ext:ModelField Name="DESCRIPTION" />
                                    <ext:ModelField Name="FIRST_NAME" />
                                    <ext:ModelField Name="MIDDLE_NAMES" />
                                    <ext:ModelField Name="LAST_NAME" />
                                    <ext:ModelField Name="EMPLOYEE_NUMBER" />
                                    <ext:ModelField Name="CURRENT_EMPLOYEE_FLAG" />
                                    <ext:ModelField Name="ORGANIZATION_NAME" />
                                    <ext:ModelField Name="JOB_TITLE" />
                                    <ext:ModelField Name="ORACLE_ACCOUNT_STATUS" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>
                <ColumnModel ID="uxSecurityAddUserColumns" runat="server">
                    <Columns>
                        <ext:Column ID="cUsername" runat="server" DataIndex="USER_NAME" Text="Username" Width="150" meta:resourcekey="cUsernameResource1" />
                        <ext:Column ID="cFirstName" runat="server" DataIndex="FIRST_NAME" Text="First Name" Flex="1" meta:resourcekey="cFirstNameResource1" />
                        <ext:Column ID="cMiddleName" runat="server" DataIndex="MIDDLE_NAMES" Text="Middle Name" Flex="1" meta:resourcekey="cMiddleNameResource1" />
                        <ext:Column ID="cLastName" runat="server" DataIndex="LAST_NAME" Text="Last Name" Flex="1" meta:resourcekey="cLastNameResource1" />
                        <ext:Column ID="cEmployeeNumber" runat="server" DataIndex="EMPLOYEE_NUMBER" Text="Employee Number" Flex="1" Hidden="true" meta:resourcekey="cEmployeeNumberResource1" />
                        <ext:Column ID="cOrganization" runat="server" DataIndex="ORGANIZATION_NAME" Text="Organization" Flex="1" Hidden="true" meta:resourcekey="cOrganizationResource1" />
                        <ext:Column ID="cJobTitle" runat="server" DataIndex="JOB_TITLE" Text="Job Title" Flex="1" Hidden="true" meta:resourcekey="cJobTitleResource1" />
                        <ext:Column ID="cCurrentEmpFlag" runat="server" DataIndex="CURRENT_EMPLOYEE_FLAG" Text="Current Employee" Flex="1" Hidden="true" meta:resourcekey="cCurrentEmpFlagResource1" />
                        <ext:Column ID="cAccountStatus" runat="server" DataIndex="ORACLE_ACCOUNT_STATUS" Text="Oracle Account Status" Flex="1" Hidden="true" meta:resourcekey="cAccountStatusResource1" />
                    </Columns>
                </ColumnModel>
                <Features>
                    <ext:GridFilters runat="server" ID="uxSecurityGridFilters" Local="true">
                        <Filters>
                            <ext:StringFilter DataIndex="USER_NAME" />
                            <ext:StringFilter DataIndex="FIRST_NAME" />
                            <ext:StringFilter DataIndex="MIDDLE_NAMES" />
                            <ext:StringFilter DataIndex="LAST_NAME" />
                            <ext:StringFilter DataIndex="EMPLOYEE_NUMBER" />
                            <ext:StringFilter DataIndex="ORGANIZATION_NAME" />
                            <ext:StringFilter DataIndex="JOB_TITLE" />
                            <ext:ListFilter DataIndex="CURRENT_EMPLOYEE_FLAG" Options="Y,N" meta:resourcekey="ListFilterResource1" />
                            <ext:ListFilter DataIndex="ORACLE_ACCOUNT_STATUS" Options="1,0" meta:resourcekey="ListFilterResource2" />
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
                                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{uxSecurityUserDetails}" />
                                <ExtraParams>
                                    <ext:Parameter Name="upUserID" Value="record.getId()" Mode="Raw" />
                                </ExtraParams>
                            </Select>
                        </DirectEvents>
                    </ext:RowSelectionModel>
                </SelectionModel>
            </ext:GridPanel>
            <ext:FormPanel
                ID="uxSecurityUserDetails"
                runat="server"
                Region="East"
                Split="true"
                Margins="5 5 5 0"
                BodyPadding="2"
                Frame="true"
                Title="User Details"
                Width="380"
                Icon="User"
                DefaultAnchor="100%"
                AutoScroll="True"
                Collapsible="true"
                Collapsed="true" meta:resourcekey="uxSecurityUserDetailsResource1">
                <Items>
                    <ext:TextField Name="USER_NAME" runat="server" FieldLabel="Username" ReadOnly="true" meta:resourcekey="TextFieldResource1" />
                    <ext:TextField Name="FIRST_NAME" runat="server" FieldLabel="First Name" ReadOnly="true" meta:resourcekey="TextFieldResource2" />
                    <ext:TextField Name="MIDDLE_NAMES" runat="server" FieldLabel="Middle Name" ReadOnly="true" meta:resourcekey="TextFieldResource3" />
                    <ext:TextField Name="LAST_NAME" runat="server" FieldLabel="Last Name" ReadOnly="true" meta:resourcekey="TextFieldResource4" />
                    <ext:TextField Name="EMPLOYEE_NUMBER" runat="server" FieldLabel="Employee No" ReadOnly="true" meta:resourcekey="TextFieldResource5" />
                    <ext:TextField Name="ORGANIZATION_NAME" runat="server" FieldLabel="Organization" ReadOnly="true" meta:resourcekey="TextFieldResource6" />
                    <ext:TextField Name="JOB_TITLE" runat="server" FieldLabel="Job Title" ReadOnly="true" meta:resourcekey="TextFieldResource7" />
                    <ext:TextField Name="CURRENT_EMPLOYEE_FLAG" runat="server" FieldLabel="Current Employee" ReadOnly="true" meta:resourcekey="TextFieldResource8" />
                    <ext:TextField Name="ORACLE_ACCOUNT_STATUS" runat="server" FieldLabel="Oracle Status" ReadOnly="true" meta:resourcekey="TextFieldResource9" />
                </Items>
            </ext:FormPanel>
            <ext:GridPanel ID="uxSecurityRoles"
                runat="server"
                Title="Security Roles"
                Padding="5"
                Icon="User"
                Region="South"
                Height="300"
                Frame="true"
                Collapsible="true"
                Margins="0 5 5 5">
                <Store>
                    <ext:Store
                        ID="uxSecurityRolesStore"
                        runat="server">
                        <Model>
                            <ext:Model ID="Model1" runat="server" IDProperty="SYSTEM_USER_ID">
                                <Fields>
                                    <ext:ModelField Name="SYSTEM_USER_ID" />
                                    <ext:ModelField Name="USER_ID" />
                                    <ext:ModelField Name="USER_NAME" />
                                    <ext:ModelField Name="DESCRIPTION" />
                                    <ext:ModelField Name="FIRST_NAME" />
                                    <ext:ModelField Name="MIDDLE_NAMES" />
                                    <ext:ModelField Name="LAST_NAME" />
                                    <ext:ModelField Name="EMPLOYEE_NUMBER" />
                                    <ext:ModelField Name="CURRENT_EMPLOYEE_FLAG" />
                                    <ext:ModelField Name="ORGANIZATION_NAME" />
                                    <ext:ModelField Name="JOB_TITLE" />
                                    <ext:ModelField Name="ORACLE_ACCOUNT_STATUS" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>
                <ColumnModel ID="uxSecurityRolesColumnModel" runat="server">
                    <Columns>
                        <ext:Column ID="Column1" runat="server" DataIndex="USER_NAME" Text="Username" Width="150" />
                        <ext:Column ID="Column2" runat="server" DataIndex="FIRST_NAME" Text="First Name" Flex="1" />
                        <ext:Column ID="Column3" runat="server" DataIndex="MIDDLE_NAMES" Text="Middle Name" Flex="1" />
                        <ext:Column ID="Column4" runat="server" DataIndex="LAST_NAME" Text="Last Name" Flex="1" />
                        <ext:Column ID="Column5" runat="server" DataIndex="EMPLOYEE_NUMBER" Text="Employee Number" Flex="1" Hidden="true" />
                        <ext:Column ID="Column6" runat="server" DataIndex="ORGANIZATION_NAME" Text="Organization" Flex="1" Hidden="true" />
                        <ext:Column ID="Column7" runat="server" DataIndex="JOB_TITLE" Text="Job Title" Flex="1" Hidden="true" />
                        <ext:Column ID="Column8" runat="server" DataIndex="CURRENT_EMPLOYEE_FLAG" Text="Current Employee" Flex="1" Hidden="true" />
                        <ext:Column ID="Column9" runat="server" DataIndex="ORACLE_ACCOUNT_STATUS" Text="Oracle Account Status" Flex="1" Hidden="true" />
                    </Columns>
                </ColumnModel>
                <Features>
                    <ext:GridFilters runat="server" ID="uxSecurityRolesGridFilters" Local="true">
                        <Filters>
                            <ext:StringFilter DataIndex="USER_NAME" />
                            <ext:StringFilter DataIndex="FIRST_NAME" />
                            <ext:StringFilter DataIndex="MIDDLE_NAMES" />
                            <ext:StringFilter DataIndex="LAST_NAME" />
                            <ext:StringFilter DataIndex="EMPLOYEE_NUMBER" />
                            <ext:StringFilter DataIndex="ORGANIZATION_NAME" />
                            <ext:StringFilter DataIndex="JOB_TITLE" />
                            <ext:ListFilter DataIndex="CURRENT_EMPLOYEE_FLAG" Options="Y,N" />
                            <ext:ListFilter DataIndex="ORACLE_ACCOUNT_STATUS" Options="1,0" />
                        </Filters>
                    </ext:GridFilters>
                </Features>
                <BottomBar>
                    <ext:PagingToolbar ID="uxSecurityRolesPaging" runat="server" />
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>


</body>
</html>

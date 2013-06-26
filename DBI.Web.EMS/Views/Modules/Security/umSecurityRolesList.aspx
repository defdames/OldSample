<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSecurityRolesList.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Security.umSecurityRolesList" %>

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
                Margins="5 5 5 5" >
                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Button runat="server" Text="Add Role" Icon="UserAdd">
                                <DirectEvents>
                                    <Click OnEvent="deAddRole"></Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator runat="server">
                            </ext:ToolbarSeparator>
                            <ext:Button runat="server" Text="Edit Role" Icon="UserEdit"></ext:Button>
                             <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                            </ext:ToolbarSeparator>
                            <ext:Button runat="server" Text="Delete Role" Icon="UserDelete"></ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Store>
                    <ext:Store
                        ID="uxSecurityRoleStore"
                        runat="server" OnReadData="deSecurityRoleRefresh">
                        <Model>
                            <ext:Model ID="uxSecurityRoleModel" runat="server" IDProperty="ROLE_ID">
                                <Fields>
                                    <ext:ModelField Name="ROLE_ID" />
                                    <ext:ModelField Name="NAME" />
                                    <ext:ModelField Name="DESCRIPTION" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>
                <ColumnModel ID="uxSecurityRoleColumns" runat="server">
                    <Columns>
                        <ext:Column ID="cID" runat="server" DataIndex="ROLE_ID" Text="Id" Width="150"  />
                        <ext:Column ID="cName" runat="server" DataIndex="NAME" Text="Name" Flex="1"  />
                        <ext:Column ID="cDescription" runat="server" DataIndex="DESCRIPTION" Text="Description" Flex="1"  />
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
                    <ext:PagingToolbar ID="uxSecurityRolePaging" runat="server"  />
                </BottomBar>
            </ext:GridPanel>
            
        </Items>
    </ext:Viewport>

</form>
</body>
</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSecurityUsers.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Security.umSecurityUsers" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager runat="server" />

    <ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout">
        <Items>
             <ext:Panel ID="Panel1" 
                runat="server"
                Title="North" 
                Region="North"
                Split="true"
                Height="150"
                BodyPadding="6"
                Html="North"
                Collapsible="true"
                />
            <ext:GridPanel ID="uxSecurityRoleGridPanel"
                runat="server"
                Title="Security Roles"
                Padding="5"
                Icon="UserEdit"
                Region="Center"
                Height="300">
                <Store>
                    <ext:Store
                        ID="uxSecurityRoleStore"
                        runat="server">
                        <Model>
                            <ext:Model ID="uxSecurityRoleModel" runat="server" IDProperty="ID">
                                <Fields>
                                    <ext:ModelField Name="ID" />
                                    <ext:ModelField Name="NAME" />
                                    <ext:ModelField Name="DESCRIPTION" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>
                <ColumnModel ID="uxSecurityRoleColumns" runat="server">
                    <Columns>
                        <ext:Column ID="uxID" runat="server" DataIndex="ID" Text="Id" Width="60" />
                        <ext:Column ID="uxName" runat="server" DataIndex="NAME" Text="Name" Width="200" />
                        <ext:Column ID="uxDescription" runat="server" DataIndex="DESCRIPTION" Text="Description" Flex="1" />
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="uxSecurityRolePaging" runat="server" />
                </BottomBar>
                <SelectionModel>
                  <ext:RowSelectionModel runat="server" Mode="Single"></ext:RowSelectionModel>
                </SelectionModel>
            </ext:GridPanel>
            
        </Items>
    </ext:Viewport>


</body>
</html>

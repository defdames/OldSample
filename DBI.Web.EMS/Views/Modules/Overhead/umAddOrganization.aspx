<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddOrganization.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umAddOrganization" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />         
        <ext:Viewport runat="server" Layout="FitLayout">
            <Items>
            <ext:GridPanel ID="uxOrganizationsGrid" runat="server" Flex="1" SimpleSelect="true" Title="Organization Security By Hierarchy" Padding="5" Region="Center">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxOrganizationSecurityStore"
                            AutoDataBind="true" PageSize="10" RemoteSort="true">
                            <Model>
                                <ext:Model ID="Model2" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="OVERHEAD_ORG_ID" />
                                        <ext:ModelField Name="ORGANIZATION_ID" />
                                        <ext:ModelField Name="HIERARCHY_NAME" />
                                        <ext:ModelField Name="ORGANIZATION_NAME" />
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
                             <ext:Column ID="Column8" runat="server" DataIndex="HIERARCHY_NAME" Text="Hierarchy Name" Flex="1" />
                            <ext:Column ID="Column1" runat="server" DataIndex="ORGANIZATION_NAME" Text="Organization Name" Flex="1" />
                          </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="FilterHeader2" runat="server" Remote="true" />
                    </Plugins>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel runat="server" Mode="Multi"></ext:CheckboxSelectionModel>
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar2" runat="server" />
                    </BottomBar>
                <TopBar>
                   <ext:Toolbar ID="Toolbar1" runat="server"><Items> <ext:Button ID="Button1" runat="server" Text="Add Selected" Icon="Add"></ext:Button></Items></ext:Toolbar>
                </TopBar>
                </ext:GridPanel>
                </Items>
        </ext:Viewport>
   
    </form>
</body>
</html>

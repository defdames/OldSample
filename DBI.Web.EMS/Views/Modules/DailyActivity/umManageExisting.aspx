<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umManageExisting.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umManageExisting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
        <ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout" IDMode="Explicit" IsDynamic="False" Namespace="App" RenderXType="True">
            <Items>
                <ext:MenuPanel runat="server" ID="uxMenuPanel" Region="West">
                    <Menu>
                        <Items>
                            <ext:MenuItem Text="Create Activity" runat="server" ID="uxCreate" />
                            <ext:MenuItem Text="Manage Existing" runat="server" ID="uxManage" />
                        </Items>
                    </Menu>
                </ext:MenuPanel>
                <ext:GridPanel runat="server" ID="uxManageGrid" Region="Center">
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" />
                    </SelectionModel>
                    <Store>
                        <ext:Store runat="server" AutoDataBind="true" DataSource="uxManageGridDataSource">
                            <Fields>
                                <ext:ModelField Name="name" />
                            </Fields>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umInventoryReport.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umInventoryReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" />
        <ext:Viewport runat="server" ID="uxInventoryReportViewport">
            <Items>
                <ext:GridPanel runat="server" ID="uxReportGrid">
                    <Store>
                        <ext:Store runat="server" ID="uxReportStore" OnReadData="deReadReport"  AutoDataBind="true" RemoteSort="true">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="OrgName" />
                                        <ext:ModelField Name="HeaderId" />
                                        <ext:ModelField Name="ProjectId" />
                                        <ext:ModelField Name="ProjectDescription" />
                                        <ext:ModelField Name="TaskNumber" />
                                        <ext:ModelField Name="ActivityDate" />
                                        <ext:ModelField Name="ItemNumber" />
                                        <ext:ModelField Name="ItemDescription" />
                                        <ext:ModelField Name="Total" />
                                        <ext:ModelField Name="Units" />
                                        <ext:ModelField Name="Inventory" />
                                        <ext:ModelField Name="SubInventory" />
                                        <ext:ModelField Name="State" />
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
                            <ext:Column runat="server" Text="Project Org" DataIndex="OrgName" Flex="15" />
                            <ext:Column runat="server" Text="DRS Number" DataIndex="HeaderId" Flex="3"/>
                            <ext:Column runat="server" Text="Project Number" DataIndex="ProjectId" Flex="5" />
                            <ext:Column runat="server" Text="Project Name" DataIndex="ProjectDescription" Flex="21" />
                            <ext:Column runat="server" Text="Task Num." DataIndex="TaskNumber" Flex="5" />
                            <ext:DateColumn runat="server" Text="Date" DataIndex="ActivityDate" Format="MM-dd-yyyy" Flex="5" />
                            <ext:Column runat="server" Text="Item Number" DataIndex="ItemNumber" Flex="5" />
                            <ext:Column runat="server" Text="Item Description" DataIndex="ItemDescription" Flex="14" />
                            <ext:Column runat="server" Text="UOM" DataIndex="Units" Flex="2" />
                            <ext:Column runat="server" Text="Quantity" DataIndex="Total" Flex="5" />
                            <ext:Column runat="server" Text="Inventory Org" DataIndex="Inventory" Flex="7" />
                            <ext:Column runat="server" Text="Sub Inventory" DataIndex="SubInventory" Flex="7" />
                            <ext:Column runat="server" Text="State" DataIndex="State" Flex="6" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                    </Plugins>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

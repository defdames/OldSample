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
                <ext:MenuPanel runat="server" ID="uxMenuPanel" Region="West" Weight="60">
                    <Menu runat="server">
                        <Items>
                            <ext:MenuItem Text="Create Activity" runat="server" ID="uxCreate" Href="umDailyActivity.aspx"/>
                            <ext:MenuItem Text="Manage Existing" runat="server" ID="uxManage" />
                        </Items>
                    </Menu>
                </ext:MenuPanel>
                <ext:GridPanel runat="server" ID="uxManageGrid" Region="North" Layout="HBoxLayout">
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" />
                    </SelectionModel>
                    <Store>
                        <ext:Store runat="server" AutoDataBind="true" ID="uxManageGridStore">
                            <Fields>
                                <ext:ModelField Name="HEADER_ID" />
                                <ext:ModelField Name="PROJECT_ID" />
                                <ext:ModelField Name="DA_DATE" />
                                <ext:ModelField Name="SEGMENT1" />
                                <ext:ModelField Name="LONG_NAME" />
                            </Fields>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>                            
                            <ext:DateColumn runat="server" Text="Activity Date" DataIndex="DA_DATE" Flex="10"/>
                            <ext:Column ID="Column1" runat="server" Text="Project" DataIndex="SEGMENT1" Flex="20"/>
                            <ext:Column runat="server" Text="Project Name" DataIndex="LONG_NAME" Flex="50" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
                <ext:TabPanel runat="server" ID="uxTabPanel" Region="Center">
                    <Items>
                        <ext:Panel runat="server" 
                            Title="Header"
                            ID="uxHeaderTab">
                            <Loader runat="server" ID="uxHeaderLoader">
                                <LoadMask ShowMask="true" />
                            </Loader>                                                        
                        </ext:Panel>
                        <ext:Panel runat="server"
                            Title="Equipment"
                            ID="uxEquipmentTab">
                            <Loader runat="server" ID="uxEquipmentLoader">
                                <LoadMask ShowMask="true" />
                            </Loader>
                        </ext:Panel>
                        <ext:Panel runat="server"
                            Title="Production"
                            ID="uxProductionTab">
                            <Loader runat="server" ID="uxProductionLoader">
                                <LoadMask ShowMask="true" />
                            </Loader>
                        </ext:Panel>
                        <ext:Panel runat="server"
                            Title="Employees"
                            ID="uxEmployeeTab">
                            <Loader runat="server" ID="uxEmployeesLoader">
                                <LoadMask ShowMask="true" />
                            </Loader>
                        </ext:Panel>
                        <ext:Panel runat="server"
                            Title="Chemical Mix"
                            ID="uxChemicalTab">
                            <Loader runat="server" ID="uxChemicalLoader">
                                <LoadMask ShowMask="true" />
                            </Loader>
                        </ext:Panel>
                    </Items>
                </ext:TabPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

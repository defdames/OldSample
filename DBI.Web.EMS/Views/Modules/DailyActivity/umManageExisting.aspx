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
                            <ext:MenuItem Text="Create Activity" Icon="ApplicationAdd" runat="server" ID="uxCreate" Href="umDailyActivity.aspx"/>
                            <ext:MenuItem Text="Manage Existing" Icon="ApplicationEdit" runat="server" ID="uxManage" />
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
                                <ext:ModelField Name="DA_DATE" Type="Date" />
                                <ext:ModelField Name="SEGMENT1" />
                                <ext:ModelField Name="LONG_NAME" />
                            </Fields>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>                            
                            <ext:DateColumn runat="server" Text="Activity Date" DataIndex="DA_DATE" Flex="10" Format="MM-dd-yyyy"/>
                            <ext:Column ID="Column1" runat="server" Text="Project" DataIndex="SEGMENT1" Flex="20"/>
                            <ext:Column runat="server" Text="Project Name" DataIndex="LONG_NAME" Flex="50" />
                        </Columns>
                    </ColumnModel>
                    <DirectEvents>
                        <SelectionChange OnEvent="deSelectHeader">
                            <ExtraParams>
                                <ext:Parameter Name="HeaderId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.HEADER_ID" Mode="Raw" />
                            </ExtraParams>
                        </SelectionChange>
                    </DirectEvents>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server"
                                    ID="uxSubmitActivityButton"
                                    Text="Submit for Approval"
                                    Icon="ApplicationGo">
                                    <DirectEvents>
                                        <Click OnEvent="deSubmitActivity">
                                            <ExtraParams>
                                                <ext:Parameter Name="HeaderId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.HEADER_ID" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>    
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:GridPanel>
                <ext:TabPanel runat="server" ID="uxTabPanel" Region="Center">
                    <Items>
                        <ext:Panel runat="server"
                            Title="Home"
                            ID="uxCombinedTab">
                            <Loader runat="server"
                                ID="uxCombinedTabLoader" Mode="Frame" AutoLoad="false" ReloadOnEvent="true">
                                <LoadMask ShowMask="true" />
                            </Loader>
                            <Listeners>
                                <Activate Handler="#{uxCombinedTab}.reload()" />
                            </Listeners>
                        </ext:Panel>
                        <ext:Panel runat="server" 
                            Title="Header"
                            ID="uxHeaderTab">
                            <Loader runat="server"
                                ID="uxHeaderLoader" Mode="Frame" AutoLoad="false" ReloadOnEvent="true"> 
                                <LoadMask ShowMask="true" />
                            </Loader>
                            <Listeners>
                                <Activate Handler="#{uxHeaderTab}.reload()" />
                            </Listeners>                                                    
                        </ext:Panel>
                        <ext:Panel runat="server"
                            Title="Equipment"
                            ID="uxEquipmentTab">
                            <Loader runat="server"
                                ID="uxEquipmentLoader" Mode="Frame" AutoLoad="false" ReloadOnEvent="true">
                                <LoadMask ShowMask="true"  />
                            </Loader>
                            <Listeners>
                                <Activate Handler="#{uxEquipmentTab}.reload()" />
                            </Listeners>
                        </ext:Panel>
                        <ext:Panel runat="server"
                            Title="Employees"
                            ID="uxEmployeeTab">
                            <Loader ID="uxEmployeeLoader" runat="server" Mode="Frame" AutoLoad="false" ReloadOnEvent="true">
                                <LoadMask ShowMask="true" />
                            </Loader>   
                            <Listeners>
                                <Activate Handler="#{uxEmployeeTab}.reload()" />
                            </Listeners>
                        </ext:Panel>
                        <ext:Panel runat="server"
                            Title="Weather"
                            ID="uxWeatherTab">
                            <Loader ID="uxWeatherLoader" runat="server" Mode="Frame" AutoLoad="false" ReloadOnEvent="true">
                                <LoadMask ShowMask="true" />
                            </Loader>
                            <Listeners>
                                <Activate Handler="#{uxWeatherTab}.reload()" />
                            </Listeners>
                        </ext:Panel>
                        <ext:Panel runat="server"
                            Title="Chemical Mix"
                            ID="uxChemicalTab">
                            <Loader ID="uxChemicalLoader" runat="server" Mode="Frame" AutoLoad="false" ReloadOnEvent="true">
                                <LoadMask ShowMask="true" />
                            </Loader>   
                            <Listeners>
                                <Activate Handler="#{uxChemicalTab}.reload()" />
                            </Listeners>
                        </ext:Panel>
                        <ext:Panel runat="server"
                            Title="Inventory"
                            ID="uxInventoryTab">
                            <Loader ID="uxInventoryLoader" runat="server" Mode="Frame" AutoLoad="false" ReloadOnEvent="true">
                                <LoadMask ShowMask="true" />
                            </Loader>   
                            <Listeners>
                                <Activate Handler="#{uxInventoryTab}.reload()" />
                            </Listeners>
                        </ext:Panel>
                        <ext:Panel runat="server"
                            Title="Production"
                            ID="uxProductionTab">
                            <Loader ID="uxProductionLoader" runat="server" Mode="Frame" AutoLoad="false" ReloadOnEvent="true">
                                <LoadMask ShowMask="true" />
                            </Loader>   
                            <Listeners>
                                <Activate Handler="#{uxProductionTab}.reload()" />
                            </Listeners>
                        </ext:Panel>                        
                    </Items>
                </ext:TabPanel>
                <ext:Window runat="server"
                    ID="uxSubmitActivityWindow"
                    Title="Submit Activity"
                    Hidden="true"
                    Width="650"
                    Shadow="true">
                    <Loader runat="server"
                        ID="uxSubmitActivityLoader"
                        Mode="Frame"
                        AutoLoad="false" />
                </ext:Window>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

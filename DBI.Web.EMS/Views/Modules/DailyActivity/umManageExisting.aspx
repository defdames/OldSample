<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umManageExisting.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umManageExisting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Resources/StyleSheets/main.css" rel="stylesheet" />
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
        <ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout" IDMode="Explicit" IsDynamic="False" Namespace="App" RenderXType="True">
            <Items>
                <ext:GridPanel runat="server" ID="uxManageGrid" Region="North" Layout="HBoxLayout">
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="true" Mode="Single" />
                    </SelectionModel>
                    <Store>
                        <ext:Store runat="server" AutoDataBind="true" ID="uxManageGridStore" OnReadData="deReadHeaderData" PageSize="10">
                            <Fields>
                                <ext:ModelField Name="HEADER_ID" Type="String" />
                                <ext:ModelField Name="PROJECT_ID" Type="String" />
                                <ext:ModelField Name="DA_DATE" Type="Date" />
                                <ext:ModelField Name="SEGMENT1" Type="String" />
                                <ext:ModelField Name="LONG_NAME" Type="String" />
                                <ext:ModelField Name="STATUS_VALUE" Type="String" />
                            </Fields>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>                            
                            <ext:DateColumn runat="server" Text="Activity Date" DataIndex="DA_DATE" Flex="10" Format="MM-dd-yyyy">
                                <HeaderItems>
                                    <ext:DateField runat="server" Format="MM-dd-yyyy" />
                                </HeaderItems>
                            </ext:DateColumn>
                            <ext:Column ID="Column1" runat="server" Text="Project" DataIndex="SEGMENT1" Flex="20"/>
                            <ext:Column runat="server" Text="Project Name" DataIndex="LONG_NAME" Flex="50" />
                            <ext:Column runat="server" Text="Status" DataIndex="STATUS_VALUE" Flex="30" />
                        </Columns>
                    </ColumnModel>
                    <BottomBar>
                        <ext:PagingToolbar runat="server" />
                    </BottomBar>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" DateFormat="MM-dd-yyyy" />
                    </Plugins>
                    <DirectEvents>
                        <SelectionChange OnEvent="deUpdateUrlAndButtons">
                            <ExtraParams>
                                <ext:Parameter Name="HeaderId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.HEADER_ID" Mode="Raw" />
                            </ExtraParams>
                        </SelectionChange>
                        <Deselect OnEvent="deDeselectHeader" />
                    </DirectEvents>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server"
                                    ID="uxCreateActivityButton"
                                    Text="Create Activity"
                                    Icon="ApplicationAdd">
                                    <DirectEvents>
                                        <Click OnEvent="deLoadCreateActivity" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarSpacer runat="server" />
                                <ext:Button runat="server"
                                    ID="uxSubmitActivityButton"
                                    Text="Submit for Approval"
                                    Icon="ApplicationGo"
                                    Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deSubmitActivity">
                                            <ExtraParams>
                                                <ext:Parameter Name="HeaderId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.HEADER_ID" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>    
                                </ext:Button>
                                <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
                                <ext:Button runat="server"
                                    ID="uxInactiveActivityButton"
                                    Text="Set Inactive"
                                    Icon="ApplicationStop"
                                    Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deSetHeaderInactive">
                                            <ExtraParams>
                                                <ext:Parameter Name="HeaderId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.HEADER_ID" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarSpacer runat="server" />
                                <ext:Button runat="server"
                                    ID="uxApproveActivityButton"
                                    Text="Approve"
                                    Icon="ApplicationPut"
                                    Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deApproveActivity">
                                            <ExtraParams>
                                                <ext:Parameter Name="HeaderId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.HEADER_ID" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" />
                                <ext:Button runat="server"
                                    ID="uxPostActivityButton"
                                    Text="Post to Oracle"
                                    Icon="ApplicationGet"
                                    Disabled="true">

                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="uxExportToPDF"
                                    Text="Export to PDF"
                                    Icon="PageWhiteAcrobat"
                                    Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deExportToPDF" IsUpload="true">
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
                            ID="uxCombinedTab"
                            Disabled="true">
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
                            ID="uxHeaderTab"
                            Disabled="true">
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
                            ID="uxEquipmentTab"
                            Disabled="true">
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
                            ID="uxEmployeeTab"
                            Disabled="true">
                            <Loader ID="uxEmployeeLoader" runat="server" Mode="Frame" AutoLoad="false" ReloadOnEvent="true">
                                <LoadMask ShowMask="true" />
                            </Loader>   
                            <Listeners>
                                <Activate Handler="#{uxEmployeeTab}.reload()" />
                            </Listeners>
                        </ext:Panel>
                        <ext:Panel runat="server"
                            Title="Weather"
                            ID="uxWeatherTab"
                            Disabled="true">
                            <Loader ID="uxWeatherLoader" runat="server" Mode="Frame" AutoLoad="false" ReloadOnEvent="true">
                                <LoadMask ShowMask="true" />
                            </Loader>
                            <Listeners>
                                <Activate Handler="#{uxWeatherTab}.reload()" />
                            </Listeners>
                        </ext:Panel>
                        <ext:Panel runat="server"
                            Title="Chemical Mix"
                            ID="uxChemicalTab"
                            Disabled="true">
                            <Loader ID="uxChemicalLoader" runat="server" Mode="Frame" AutoLoad="false" ReloadOnEvent="true">
                                <LoadMask ShowMask="true" />
                            </Loader>   
                            <Listeners>
                                <Activate Handler="#{uxChemicalTab}.reload()" />
                            </Listeners>
                        </ext:Panel>
                        <ext:Panel runat="server"
                            Title="Inventory"
                            ID="uxInventoryTab"
                            Disabled="true">
                            <Loader ID="uxInventoryLoader" runat="server" Mode="Frame" AutoLoad="false" ReloadOnEvent="true">
                                <LoadMask ShowMask="true" />
                            </Loader>   
                            <Listeners>
                                <Activate Handler="#{uxInventoryTab}.reload()" />
                            </Listeners>
                        </ext:Panel>
                        <ext:Panel runat="server"
                            Title="Production"
                            ID="uxProductionTab"
                            Disabled="true">
                            <Loader ID="uxProductionLoader" runat="server" Mode="Frame" AutoLoad="false" ReloadOnEvent="true">
                                <LoadMask ShowMask="true" />
                            </Loader>   
                            <Listeners>
                                <Activate Handler="#{uxProductionTab}.reload()" />
                            </Listeners>
                        </ext:Panel>                        
                    </Items>
                </ext:TabPanel>
                <%-- Hidden Windows --%>
                <ext:Window runat="server"
                    ID="uxSubmitActivityWindow"
                    Title="Submit Activity"
                    Hidden="true"
                    Width="650"
                    Shadow="true"
                    Y="50">
                    <Loader runat="server"
                        ID="uxSubmitActivityLoader"
                        Mode="Frame"
                        AutoLoad="false" />
                </ext:Window>
                <ext:Window runat="server"
                    ID="uxCreateActivityWindow"
                    Title="Create Activity"
                    Hidden="true"
                    Width="650"
                    Shadow="true"
                    Y="50">
                    <Loader runat="server"
                        ID="uxCreateActivityLoader"
                        Url="umDailyActivity.aspx"
                        Mode="Frame"
                        AutoLoad="false" />
                </ext:Window>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

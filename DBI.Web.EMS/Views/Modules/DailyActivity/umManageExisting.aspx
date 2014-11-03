<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umManageExisting.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umManageExisting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Resources/StyleSheets/main.css" rel="stylesheet" />
    <script type="text/javascript">
        var colorErrors = function (value, metadata, record) {
            if (record.data.WARNING == "Error") {
                metadata.style = "background-color: red;";
            }
            else if (record.data.WARNING == "Warning") {
                metadata.style = "background-color: yellow;";
            }
            return value;
        };

        var onFilterRender = function (field) {
            field.getPicker().el.on(
                "mousedown",
                function (e) {
                    // generally, just e.stopEvent() should work instead of the all code below. I will investigate.
                    var browserEvent = e.browserEvent;

                    if (browserEvent) {
                        Ext.EventManager.stopPropagation(browserEvent);
                    }
                    e.preventDefault();
                });
        };

        var onShow = function (toolTip, grid) {
            var view = grid.getView(),
				record = view.getRecord(toolTip.triggerElement),
				data = record.data.WARNING_TYPE;

            toolTip.update(data);
        };

        var beforeShow = function (toolTip, grid) {
            var view = grid.getView(),
				record = view.getRecord(toolTip.triggerElement),
				data = record.data.WARNING_TYPE;
            if (data == "") {
                return false;
            }
            return true;
        };

        var setDDValue = function () {
            if (App.uxStartFilterDate.value && App.uxEndFilterDate.value) {
                var start = new Date(App.uxStartFilterDate.value);
                var startDayOfMonth = start.getDate();
                if (startDayOfMonth < 10) {
                    startDayOfMonth = "0" + startDayOfMonth;
                }
                var end = new Date(App.uxEndFilterDate.value);
                var endDayOfMonth = end.getDate();
                if (endDayOfMonth < 10) {
                    endDayOfMonth = "0" + endDayOfMonth;
                }
                
                App.uxDropDownFilter.setValue(">= " + (start.getMonth() + 1) + "-" + startDayOfMonth + "-" + start.getFullYear() + " <= " + (end.getMonth() + 1) + "-" + endDayOfMonth + "-" + end.getFullYear());
            }
            else if (!App.uxStartFilterDate.value && App.uxEndFilterDate.value) {
                var end = new Date(App.uxEndFilterDate.value);
                var endDayOfMonth = end.getDate();
                if (endDayOfMonth < 10) {
                    endDayOfMonth = "0" + endDayOfMonth;
                }
                
                App.uxDropDownFilter.setValue("<= " + (end.getMonth() + 1) + "-" + endDayOfMonth + "-" + end.getFullYear());
            }
            else if(!App.uxEndFilterDate.value && App.uxStartFilterDate.value){
                var start = new Date(App.uxStartFilterDate.value);
                var startDayOfMonth = start.getDate();
                if (startDayOfMonth < 10) {
                    startDayOfMonth = "0" + startDayOfMonth;
                }
                App.uxDropDownFilter.setValue(">= " + (start.getMonth() + 1) + "-" + startDayOfMonth + "-" + start.getFullYear());
            }
            
        };

        var clearDates = function () {
            App.uxStartFilterDate.clear();
            App.uxEndFilterDate.clear();
        };
    </script>
    <style type="text/css">
        .red-warning .x-grid-cell, .red-warning .x-grid-rowwrap-div {
            background-color: #FF0000 !important;
        }

        .yellow-warning .x-grid-cell, .yellow-warning .x-grid-rowwrap-div {
            background: #ffff00 !important;
        }

        .x-toolbar div.ux-bold-text-item {
            font-weight: bold;
        }
    </style>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
        <ext:Hidden ID="uxDeactivate" runat="server" />
        <ext:Viewport runat="server" ID="uxViewPort" Layout="FitLayout" IDMode="Explicit" Namespace="App" RenderXType="True">
            <Items>
                <ext:TabPanel runat="server" ID="uxTabPanel">
                    <Items>
                        <ext:GridPanel runat="server" ID="uxManageGrid" Layout="FitLayout" SelectionMemoryEvents="false" SelectionMemory="true" Title="DRS List">
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="false" Mode="Single" />
                            </SelectionModel>
                            <Store>
                                <ext:Store runat="server" AutoDataBind="true" ID="uxManageGridStore" OnReadData="deReadHeaderData" PageSize="20" RemoteSort="true" IsPagingStore="true">
                                    <Fields>
                                        <ext:ModelField Name="HEADER_ID" Type="String" />
                                        <ext:ModelField Name="ORG_ID" Type="String" />
                                        <ext:ModelField Name="PROJECT_ID" Type="String" />
                                        <ext:ModelField Name="DA_DATE" Type="Date" />
                                        <ext:ModelField Name="SEGMENT1" Type="String" />
                                        <ext:ModelField Name="LONG_NAME" Type="String" />
                                        <ext:ModelField Name="STATUS_VALUE" Type="String" />
                                        <ext:ModelField Name="DA_HEADER_ID" Type="String" />
                                        <ext:ModelField Name="WARNING" Type="String" />
                                        <ext:ModelField Name="WARNING_TYPE" Type="String" />
                                        <ext:ModelField Name="STATUS" Type="Int" />
                                    </Fields>
                                    <Proxy>
                                        <ext:PageProxy />
                                    </Proxy>
                                </ext:Store>
                            </Store>
                            <ColumnModel>
                                <Columns>
                                    <ext:Column runat="server" Text="DRS Number" DataIndex="HEADER_ID" Flex="10" />
                                    <ext:Column ID="Column2" runat="server" Text="Oracle Header Id" DataIndex="DA_HEADER_ID" Flex="10" />
                                    <ext:DateColumn runat="server" Text="Activity Date" DataIndex="DA_DATE" Flex="10" Format="MM-dd-yyyy">
                                        <HeaderItems>
                                            <ext:DropDownField ID="uxDropDownFilter" runat="server" Editable="true">
                                                <Plugins>
                                                    <ext:ClearButton runat="server">
                                                        <Listeners>
                                                            <Clear Fn="clearDates" />
                                                        </Listeners>
                                                    </ext:ClearButton>
                                                </Plugins>
                                                <Component>
                                                    <ext:Panel ID="Panel1"
                                                        runat="server"
                                                        Layout="AnchorLayout"
                                                        DefaultAnchor="100%">
                                                        <Items>
                                                            <ext:DateField
                                                                ID="uxStartFilterDate"
                                                                runat="server"
                                                                Editable="false"
                                                                LabelWidth="30"
                                                                FieldLabel="From">
                                                                <Listeners>
                                                                    <Render Fn="onFilterRender" />
                                                                    <Select Fn="setDDValue" />
                                                                </Listeners>
                                                            </ext:DateField>
                                                            <ext:DateField
                                                                ID="uxEndFilterDate"
                                                                runat="server"
                                                                Editable="false"
                                                                LabelWidth="30"
                                                                FieldLabel="To">
                                                                <Listeners>
                                                                    <Render Fn="onFilterRender" />
                                                                    <Select Fn="setDDValue" />
                                                                </Listeners>
                                                            </ext:DateField>
                                                        </Items>
                                                    </ext:Panel>
                                                </Component>
                                            </ext:DropDownField>
                                        </HeaderItems>
                                    </ext:DateColumn>
                                    <ext:Column ID="Column1" runat="server" Text="Project" DataIndex="SEGMENT1" Flex="10" />
                                    <ext:Column runat="server" Text="Project Name" DataIndex="LONG_NAME" Flex="45" />
                                    <ext:Column runat="server" Text="Status" DataIndex="STATUS_VALUE" Flex="15">
                                        <Renderer Fn="colorErrors" />
                                    </ext:Column>
                                </Columns>
                            </ColumnModel>
                            <BottomBar>
                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <ext:Hidden runat="server" ID="uxDirtyRow" />
                                        <ext:Hidden runat="server" ID="uxHiddenApprove" />
                                        <ext:Button runat="server"
                                            ID="uxCreateActivityButton"
                                            Text="Create Activity"
                                            Icon="ApplicationAdd">
                                            <DirectEvents>
                                                <Click OnEvent="deLoadCreateActivity" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="uxPostMultipleButton" runat="server"
                                            Text="Post Multiple Headers"
                                            Icon="ApplicationGet">
                                            <DirectEvents>
                                                <Click OnEvent="deOpenPostMultipleWindow" />
                                            </DirectEvents>
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
                                        <ext:Button runat="server"
                                            ID="uxExportMultiple"
                                            Text="Export Multple"
                                            Icon="PageWhiteAcrobat">
                                            <DirectEvents>
                                                <Click OnEvent="deOpenExportMultipleWindow" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button runat="server"
                                            ID="uxEmailPdf"
                                            Text="Email Copy"
                                            Icon="EmailAttach"
                                            Disabled="true">
                                            <DirectEvents>
                                                <Click OnEvent="deSendPDF" IsUpload="true">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="HeaderId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.HEADER_ID" Mode="Raw" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:ToolbarSpacer runat="server" />
                                        <ext:ToolbarSeparator runat="server" />
                                        <ext:ToolbarSpacer runat="server" />
                                        <ext:Checkbox runat="server" ID="uxTogglePosted" BoxLabel="Show Posted" BoxLabelAlign="After">
                                            <Listeners>
                                                <Change Handler="#{uxManageGridStore}.reload()" />
                                            </Listeners>
                                        </ext:Checkbox>
                                        <ext:ToolbarSpacer runat="server" />
                                        <ext:Checkbox runat="server" ID="uxToggleInactive" BoxLabel="Show Inactive" BoxLabelAlign="After">
                                            <Listeners>
                                                <Change Handler="#{uxManageGridStore}.reload()" />
                                            </Listeners>
                                        </ext:Checkbox>
                                        <ext:ToolbarFill runat="server" />
                                        <ext:PagingToolbar ID="uxManageGridPaging" runat="server" StoreID="uxManageGridStore" />
                                    </Items>
                                </ext:Toolbar>
                            </BottomBar>
                            <Plugins>
                                <ext:FilterHeader runat="server" Remote="true" DateFormat="MM-dd-yyyy" />
                            </Plugins>
                            <DirectEvents>
                                <Select OnEvent="deUpdateUrlAndButtons">
                                    <ExtraParams>
                                        <ext:Parameter Name="HeaderId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.HEADER_ID" Mode="Raw" />
                                        <ext:Parameter Name="OrgId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.ORG_ID" Mode="Raw" />
                                        <ext:Parameter Name="Status" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.STATUS_VALUE" Mode="Raw" />
                                        <ext:Parameter Name="CurrentPage" Value="#{uxManageGridPaging}.getPageData().currentPage" Mode="Raw" />
                                        <ext:Parameter Name="TotalRecords" Value="#{uxManageGridStore}.getTotalCount()" Mode="Raw" />
                                    </ExtraParams>
                                    <EventMask ShowMask="true" />
                                </Select>
                            </DirectEvents>
                            <Listeners>
                                <Select Handler="#{uxTabPanel}.addTab(#{uxTabPanel}.bin[0]);#{uxTabPanel}.setActiveTab(#{uxDetailsPanel})" />
                            </Listeners>
                            <ToolTips>
                                <ext:ToolTip ID="uxWarningToolTip"
                                    runat="server"
                                    Delegate="tr.x-grid-row"
                                    TrackMouse="true"
                                    UI="Warning"
                                    Width="400">
                                    <Listeners>
                                        <BeforeShow Handler="return beforeShow(this, #{uxManageGrid});" />
                                        <Show Handler="onShow(this, #{uxManageGrid});" />
                                    </Listeners>
                                </ext:ToolTip>
                            </ToolTips>
                        </ext:GridPanel>
                    </Items>
                    <Bin>
                        <ext:Panel runat="server" ID="uxDetailsPanel" CloseAction="Hide" Layout="FitLayout" Title="DRS Details" Hidden="true" Closable="false">
                            <Loader ID="Loader1" runat="server" Mode="Frame">
                                <LoadMask ShowMask="true" />
                            </Loader>
                            <TopBar>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:ToolbarFill runat="server" />
                                        <ext:Button runat="server" Text="Previous DRS" Icon="ArrowLeft" ID="uxLoadPreviousButton">
                                            <DirectEvents>
                                                <Click OnEvent="deLoadPreviousActivity">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="CurrentPage" Value="#{uxManageGridPaging}.getPageData().currentPage" Mode="Raw" />
                                                    </ExtraParams>
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:ToolbarSpacer runat="server" />
                                        <ext:ToolbarTextItem runat="server" ID="uxTotalRecords" Cls="ux-bold-text-item" />
                                        <ext:ToolbarSpacer runat="server" />
                                        <ext:Button runat="server" Text="Next DRS" Icon="ArrowRight" IconAlign="Right" ID="uxLoadNextButton">
                                            <DirectEvents>
                                                <Click OnEvent="deLoadNextActivity">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="FromRecord" Value="#{uxManageGridPaging}.getPageData().fromRecord" Mode="Raw" />
                                                        <ext:Parameter Name="ToRecord" Value="#{uxManageGridPaging}.getPageData().toRecord" Mode="Raw" />
                                                        <ext:Parameter Name="TotalRecords" Value="#{uxManageGridStore}.getTotalCount()" Mode="Raw" />
                                                    </ExtraParams>
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <BottomBar>
                                <ext:Toolbar ID="Toolbar2" runat="server">
                                    <Items>
                                        <ext:Button runat="server" ID="uxCancelButton" Text="Cancel" Icon="Delete">
                                            <Listeners>
                                                <Click Handler="#{uxTabPanel}.closeTab(#{uxDetailsPanel})" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:ToolbarSeparator runat="server" />
                                        <ext:Button runat="server"
                                            ID="uxExportPDFCombined"
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
                                        <ext:ToolbarSpacer ID="ToolbarSpacer9" runat="server" />
                                        <ext:Button runat="server"
                                            ID="uxEmailPDFCombined"
                                            Text="Email Copy"
                                            Icon="EmailAttach"
                                            Disabled="true">
                                            <DirectEvents>
                                                <Click OnEvent="deSendPDF" IsUpload="true">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="HeaderId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.HEADER_ID" Mode="Raw" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:ToolbarFill runat="server" />
                                        <ext:Toolbar ID="Toolbar3" runat="server">
                                            <Items>
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
                                                            <EventMask ShowMask="true" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
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
                                                <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" />
                                                <ext:Button runat="server"
                                                    ID="uxPostActivityButton"
                                                    Text="Post to Oracle"
                                                    Icon="ApplicationGet"
                                                    Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="dePostToOracle">
                                                            <ExtraParams>
                                                                <ext:Parameter Name="HeaderId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.HEADER_ID" Mode="Raw" />
                                                            </ExtraParams>
                                                            <EventMask ShowMask="true" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:ToolbarSpacer ID="ToolbarSpacer4" runat="server" />
                                                <ext:Button runat="server"
                                                    ID="uxMarkAsPostedButton"
                                                    Text="Mark as Posted"
                                                    Icon="PencilGo"
                                                    Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="deMarkAsPosted">
                                                            <ExtraParams>
                                                                <ext:Parameter Name="HeaderId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.HEADER_ID" Mode="Raw" />
                                                            </ExtraParams>
                                                            <Confirmation Message="Mark DRS as posted." ConfirmRequest="true" />
                                                            <EventMask ShowMask="true" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" />
                                            </Items>
                                        </ext:Toolbar>
                                    </Items>
                                </ext:Toolbar>
                            </BottomBar>
                            <TopBar>
                            </TopBar>
                        </ext:Panel>
                    </Bin>
                </ext:TabPanel>
                <%-- Hidden Windows --%>
                <ext:Window runat="server"
                    ID="uxPlaceholderWindow"
                    Hidden="true"
                    Width="650"
                    Height="300"
                    Y="50"
                    Modal="true">
                    <Loader runat="server"
                        ID="uxPlaceholderLoader"
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

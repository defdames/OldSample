<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEditBudgetVersion.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umEditBudgetVersion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        Ext.util.Format.CurrencyFactory = function (dp, dSeparator, tSeparator, symbol) {
            return function (n) {
                if (n == 0) {
                    return " ";
                }

                var template = '<span style="color:{0};">{1}</span>';

                dp = Math.abs(dp) + 1 ? dp : 2;
                dSeparator = dSeparator || ".";
                tSeparator = tSeparator || ",";

                var m = /(\d+)(?:(\.\d+)|)/.exec(n + ""),
                    x = m[1].length > 3 ? m[1].length % 3 : 0;


                var r = (n < 0 ? '(' : '') // preserve minus sign
                        + (x ? m[1].substr(0, x) + tSeparator : "")
                        + m[1].substr(x).replace(/(\d{3})(?=\d)/g, "$1" + tSeparator)
                        + (dp ? dSeparator + (+m[2] || 0).toFixed(dp).substr(2) : "")
                        + (n < 0 ? ')' : '') + " " + symbol;

                return Ext.String.format(template, (n >= 0) ? "black" : "red", r);
            };
        };


        Ext.util.Format.CurrencyFactorySUM = function (dp, dSeparator, tSeparator, symbol) {
            return function (n) {

                var template = '<span style="color:{0};">{1}</span>';

                dp = Math.abs(dp) + 1 ? dp : 2;
                dSeparator = dSeparator || ".";
                tSeparator = tSeparator || ",";

                var m = /(\d+)(?:(\.\d+)|)/.exec(n + ""),
                    x = m[1].length > 3 ? m[1].length % 3 : 0;


                var r = (n < 0 ? '(' : '') // preserve minus sign
                        + (x ? m[1].substr(0, x) + tSeparator : "")
                        + m[1].substr(x).replace(/(\d{3})(?=\d)/g, "$1" + tSeparator)
                        + (dp ? dSeparator + (+m[2] || 0).toFixed(dp).substr(2) : "")
                        + (n < 0 ? ')' : '') + " " + symbol;

                return Ext.String.format(template, (n >= 0) ? "black" : "red", r);
            };
        };

        var submitValue = function (grid, hiddenFormat, format) {
            hiddenFormat.setValue(format);
            grid.submitData(false, { isUpload: true });
        };
    </script>

    <style>
        .x-grid-row-summary .x-grid-cell-inner
        {
            font-weight: bold;
            font-size: 11px;
            background-color: #E0E0D1;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />

        <ext:Hidden ID="FormatType" runat="server" />

        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
                <ext:GridPanel ID="uxOrganizationAccountGridPanel" runat="server" Flex="1" Title="General Ledger Accounts by Budget" Header="false" Margin="5" Region="Center" Scroll="Both">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button runat="server" Icon="MagnifierZoomIn" Text="Account Inquery" ID="uxViewActuals" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="viewActuals">
                                            <ExtraParams>
                                                <ext:Parameter Value="#{uxOrganizationAccountGridPanel}.getView().getSelectionModel().getSelection()[0].data.ACCOUNT_DESCRIPTION" Mode="Raw" Name="ACCOUNT_DESCRIPTION"></ext:Parameter>
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></ext:ToolbarSeparator>
                                <ext:Button runat="server" Icon="DatabaseCopy" Text="Import Actuals" ID="uxImportActualsButton">
                                    <DirectEvents>
                                        <Click OnEvent="importActuals"></Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server"></ext:ToolbarSeparator>
                                <ext:Button runat="server" Icon="NoteEdit" Text="Budget Notes" ID="uxBudgetNotes">
                                    <DirectEvents>
                                        <Click OnEvent="editBudgetNotes"></Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server"></ext:ToolbarSeparator>
                                <ext:Button runat="server" Icon="Accept" Text="Complete Budget" ID="uxCompleteBudget">
                                    <DirectEvents>
                                        <Click OnEvent="deCompleteBudget">
                                            <Confirmation ConfirmRequest="true" Message="Are you sure you want to complete this budget? This will lock your budget for this forecast and only finance will be allowed to unlock it." />
                                            <EventMask ShowMask="true"></EventMask>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server"></ext:ToolbarFill>
                                <ext:Button ID="Button3" runat="server" Text="To Excel" Icon="PageExcel" Disabled="true">
                                    <Listeners>
                                        <Click Handler="submitValue(#{uxOrganizationAccountGridPanel}, #{FormatType}, 'xls');" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server"></ext:ToolbarSeparator>
                                <ext:Checkbox runat="server" HideLabel="true" BoxLabel="Hide Blank Lines" ID="uxHideBlankLinesCheckbox">
                                    <DirectEvents>
                                        <Change OnEvent="deHideBlankLines">
                                            <EventMask ShowMask="true"></EventMask>
                                        </Change>
                                    </DirectEvents>
                                </ext:Checkbox>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Plugins>
                        <ext:FilterHeader ID="uxGlAccountSecurityGridFilter" runat="server" Remote="true" OnCreateFilterableField="OnCreateFilterableField" />
                    </Plugins>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxOrganizationAccountStore"
                            AutoDataBind="true" RemoteSort="true" OnReadData="loadBudgetDetails" AutoLoad="true" GroupField="CATEGORY_NAME" RemoteGroup="true" OnSubmitData="deExportData">
                            <Model>
                                <ext:Model ID="Model2" runat="server" IDProperty="CODE_COMBINATION_ID">
                                    <Fields>
                                        <ext:ModelField Name="CATEGORY_NAME" />
                                        <ext:ModelField Name="CATEGORY_SORT_ORDER" />
                                        <ext:ModelField Name="ACCOUNT_ORDER" />
                                        <ext:ModelField Name="ACCOUNT_DESCRIPTION" />
                                        <ext:ModelField Name="ACCOUNT_SEGMENT" />
                                        <ext:ModelField Name="TOTAL" />
                                        <ext:ModelField Name="AMOUNT1" />
                                        <ext:ModelField Name="AMOUNT2" />
                                        <ext:ModelField Name="AMOUNT3" />
                                        <ext:ModelField Name="AMOUNT4" />
                                        <ext:ModelField Name="AMOUNT5" />
                                        <ext:ModelField Name="AMOUNT6" />
                                        <ext:ModelField Name="AMOUNT7" />
                                        <ext:ModelField Name="AMOUNT8" />
                                        <ext:ModelField Name="AMOUNT9" />
                                        <ext:ModelField Name="AMOUNT10" />
                                        <ext:ModelField Name="AMOUNT11" />
                                        <ext:ModelField Name="AMOUNT12" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Sorters>
                                <ext:DataSorter Property="CATEGORY_SORT_ORDER" Direction="ASC" />
                                <ext:DataSorter Property="SORT_ORDER" Direction="ASC" />
                                <ext:DataSorter Property="ACCOUNT_SEGMENT" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>

                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column14" runat="server" DataIndex="ACCOUNT_DESCRIPTION" Text="Account Name" Width="250" Locked="true" Sortable="false" Draggable="false">
                                <SummaryRenderer Handler="return ('Total');" />
                            </ext:Column>
                            <ext:Column ID="Column54" runat="server" Text="Total" Flex="1" Align="Right" DataIndex="TOTAL" SummaryType="Sum" StyleSpec="font-size:7pt;" Filterable="false" Sortable="false" Lockable="false" Hideable="false" Draggable="false">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactorySUM(2,'.',',','')" />
                            </ext:Column>
                            <ext:Column ID="Column1" runat="server" Flex="1" Align="Right" DataIndex="AMOUNT1" SummaryType="Sum" StyleSpec="font-size:7pt;" Filterable="false" Sortable="false" Lockable="false" Hideable="false" Draggable="false">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactorySUM(2,'.',',','')" />
                            </ext:Column>
                            <ext:Column ID="Column2" runat="server" Flex="1" Align="Right" DataIndex="AMOUNT2" SummaryType="Sum" StyleSpec="font-size:7pt;" Filterable="false" Sortable="false" Lockable="false" Hideable="false" Draggable="false">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactorySUM(2,'.',',','')" />
                            </ext:Column>
                            <ext:Column ID="Column3" runat="server" Flex="1" Align="Right" DataIndex="AMOUNT3" SummaryType="Sum" StyleSpec="font-size:7pt;" Filterable="false" Sortable="false" Lockable="false" Hideable="false" Draggable="false">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactorySUM(2,'.',',','')" />
                            </ext:Column>
                            <ext:Column ID="Column4" runat="server" Flex="1" Align="Right" DataIndex="AMOUNT4" SummaryType="Sum" StyleSpec="font-size:7pt;" Filterable="false" Sortable="false" Lockable="false" Hideable="false" Draggable="false">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactorySUM(2,'.',',','')" />
                            </ext:Column>
                            <ext:Column ID="Column5" runat="server" Flex="1" Align="Right" DataIndex="AMOUNT5" SummaryType="Sum" StyleSpec="font-size:7pt;" Filterable="false" Sortable="false" Lockable="false" Hideable="false" Draggable="false">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactorySUM(2,'.',',','')" />
                            </ext:Column>
                            <ext:Column ID="Column6" runat="server" Flex="1" Align="Right" DataIndex="AMOUNT6" SummaryType="Sum" StyleSpec="font-size:7pt;" Filterable="false" Sortable="false" Lockable="false" Hideable="false" Draggable="false">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactorySUM(2,'.',',','')" />
                            </ext:Column>
                            <ext:Column ID="Column7" runat="server" Flex="1" Align="Right" DataIndex="AMOUNT7" SummaryType="Sum" StyleSpec="font-size:7pt;" Filterable="false" Sortable="false" Lockable="false" Hideable="false" Draggable="false">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactorySUM(2,'.',',','')" />
                            </ext:Column>
                            <ext:Column ID="Column8" runat="server" Flex="1" Align="Right" DataIndex="AMOUNT8" SummaryType="Sum" StyleSpec="font-size:7pt;" Filterable="false" Sortable="false" Lockable="false" Hideable="false" Draggable="false">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactorySUM(2,'.',',','')" />
                            </ext:Column>
                            <ext:Column ID="Column9" runat="server" Flex="1" Align="Right" DataIndex="AMOUNT9" SummaryType="Sum" StyleSpec="font-size:7pt;" Filterable="false" Sortable="false" Lockable="false" Hideable="false" Draggable="false">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactorySUM(2,'.',',','')" />
                            </ext:Column>
                            <ext:Column ID="Column10" runat="server" Flex="1" Align="Right" DataIndex="AMOUNT10" SummaryType="Sum" StyleSpec="font-size:7pt;" Filterable="false" Sortable="false" Lockable="false" Hideable="false" Draggable="false">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactorySUM(2,'.',',','')" />
                            </ext:Column>
                            <ext:Column ID="Column11" runat="server" Flex="1" Align="Right" DataIndex="AMOUNT11" SummaryType="Sum" StyleSpec="font-size:7pt;" Filterable="false" Sortable="false" Lockable="false" Hideable="false" Draggable="false">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactorySUM(2,'.',',','')" />
                            </ext:Column>
                            <ext:Column ID="Column12" runat="server" Flex="1" Align="Right" DataIndex="AMOUNT12" SummaryType="Sum" StyleSpec="font-size:7pt;" Filterable="false" Sortable="false" Lockable="false" Hideable="false" Draggable="false">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactorySUM(2,'.',',','')" />
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <Features>
                        <ext:Summary ID="uxSummary" runat="server" Dock="Bottom" ShowSummaryRow="true" />
                        <ext:GroupingSummary
                            ID="GroupingSummary1"
                            runat="server"
                            GroupHeaderTplString='{name} ({rows.length} Item{[values.rows.length > 1 ? "s" : ""]})'
                            HideGroupedHeader="true"
                            EnableGroupingMenu="false" />
                    </Features>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" ID="uxOrganizationAccountSelectionModel">
                            <Listeners>
                                <Select Handler="if(#{uxOrganizationAccountSelectionModel}.getCount() > 0){#{uxViewActuals}.enable();}else {#{uxViewActuals}.disable();}"></Select>
                                <Deselect Handler="if(#{uxOrganizationAccountSelectionModel}.getCount() > 0){#{uxViewActuals}.enable();}else {#{uxViewActuals}.disable();}"></Deselect>
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <DirectEvents>
                        <ItemDblClick OnEvent="deItemMaintenance">
                            <ExtraParams>
                                <ext:Parameter Value="#{uxOrganizationAccountGridPanel}.getView().getSelectionModel().getSelection()[0].data.ACCOUNT_DESCRIPTION" Mode="Raw" Name="ACCOUNT_DESCRIPTION"></ext:Parameter>
                            </ExtraParams>
                        </ItemDblClick>
                    </DirectEvents>
                    <View>
                        <ext:GridView ID="GridView2" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View>

                </ext:GridPanel>
            </Items>
        </ext:Viewport>

        <ext:Window runat="server" Stateful="false" Width="550" Height="450" Title="Budget Notes" Layout="FitLayout" Header="true" Resizable="false" Frame="true" Hidden="true" ID="uxBudgetNotesWindow" CloseAction="Hide" Closable="true" Modal="true" DefaultButton="uxSaveBudgetNote">
            <Items>
                <ext:FormPanel ID="FormPanel2" runat="server" Header="false" BodyPadding="5"
                    Margins="5 5 5 5" Region="Center" Title="Comments" Layout="FitLayout" Flex="1">
                    <Items>
                        <ext:FieldContainer ID="FieldContainer2"
                            runat="server"
                            LabelStyle="font-weight:bold;padding:0;"
                            Layout="FitLayout">
                            <Items>
                                <ext:TextArea ID="uxBudgetComments" runat="server" Flex="1" Grow="true">
                                </ext:TextArea>
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button runat="server" ID="uxSaveBudgetNote" Icon="Accept" Text="Save">
                    <DirectEvents>
                        <Click OnEvent="saveBudgetNotes">
                            <EventMask ShowMask="true"></EventMask>
                        </Click>
                    </DirectEvents>
                </ext:Button>
                <ext:Button runat="server" ID="uxCancelSaveBudgetNote" Icon="Cancel" Text="Cancel">
                    <Listeners>
                        <Click Handler="#{uxBudgetNotesWindow}.close();"></Click>
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>
    </form>
</body>
</html>

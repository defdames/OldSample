<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umViewBudget.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umViewBudget" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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

        function printWindow(url) {
            window.open(url, "PrintWindow", "menubar=0,resizable=1");
        }


        var getRowClass = function (record, rowIndex, rowParams, store) {
            if (record.data.GROUPED == "Y") {
                return "blue-row";
            };
        }
      </script>


    <style>
        .x-grid-row-summary .x-grid-cell-inner
        {
            font-weight: bold;
            font-size: 11px;
            background-color: #E0E0D1;
        }

           .blue-row .x-grid-cell, .blue-row .x-grid-rowwrap-div .blue-row .myBoldClass.x-grid3-row td  {
            background-color: #E0FFFF;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
      <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" Namespace="App" ClientIDMode="Static" />         
       
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
                <ext:GridPanel ID="uxOrganizationAccountGridPanel" runat="server" Flex="1" Title="General Ledger Accounts by Budget" Header="false" Margin="5" Region="Center" Scroll="Both">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                 <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server"></ext:ToolbarSpacer>
                                 <ext:ComboBox runat="server" ID="uxBudgetName" Editable="true" TypeAhead="true"
                                FieldLabel="Budget Name" DisplayField="BUDGET_DESCRIPTION"
                                ValueField="OVERHEAD_BUDGET_TYPE_ID"  TriggerAction="All" LabelWidth="75"
                                MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;"  >
                                <Store>
                                    <ext:Store runat="server" ID="uxBudgetNameStore" OnReadData="deLoadBudgetNames" AutoLoad="true" >
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                        <Model>
                                            <ext:Model ID="Model5" runat="server" IDProperty="OVERHEAD_BUDGET_TYPE_ID">
                                                <Fields>
                                                    <ext:ModelField Name="BUDGET_DESCRIPTION" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>
                               <ext:ToolbarSpacer runat="server"></ext:ToolbarSpacer>
                                 <ext:ComboBox runat="server" ID="uxFiscalYear" Editable="true" TypeAhead="true" LabelWidth="75"
                                FieldLabel="Fiscal Year"  DisplayField="ID_NAME" 
                                ValueField="ID_NAME" TriggerAction="All" 
                                MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;"  >
                                <Store>
                                    <ext:Store runat="server" ID="uxFiscalYearsStore" OnReadData="deLoadFiscalYears" AutoLoad="true" AutoDataBind="true" >
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                        <Model>
                                            <ext:Model ID="Model1" runat="server" IDProperty="ID_NAME">
                                                <Fields>
                                                    <ext:ModelField Name="ID_NAME" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                              <DirectEvents>
                                  <Select OnEvent="deLoadData"></Select>
                              </DirectEvents>
                            </ext:ComboBox>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server"></ext:ToolbarFill>
                               
                                <ext:Button ID="uxPrintReport" runat="server" Text="Print" Icon="Printer" Disabled="false">
                                    <DirectEvents>
                                        <Click OnEvent="showPrintWindow" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server"></ext:ToolbarSeparator>
                                <ext:Button ID="Button1" runat="server" Text="Export" Icon="PageExcel">
                                    <DirectEvents>
                                        <Click OnEvent="ExportToExcel" IsUpload="true"><EventMask ShowMask="true" Msg="Generating Export, Please Wait..." /></Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Plugins>
                        <ext:FilterHeader ID="uxGlAccountSecurityGridFilter" runat="server" Remote="true" OnCreateFilterableField="OnCreateFilterableField" />
                    </Plugins>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxOrganizationAccountStore"
                            AutoDataBind="true" RemoteSort="true" OnReadData="loadBudgetDetails" AutoLoad="false" GroupField="CATEGORY_NAME" RemoteGroup="true" >
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
                                        <ext:ModelField Name="GROUPED" />
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
                        <ext:Summary ID="uxSummary" runat="server" ShowSummaryRow="true" />
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
                    <View>
                        <ext:GridView ID="GridView2" StripeRows="true" runat="server" TrackOver="true">
                            <GetRowClass Fn="getRowClass"></GetRowClass>
                        </ext:GridView>
                    </View>

                </ext:GridPanel>
            </Items>
        </ext:Viewport>


        <ext:Window ID="uxPrintWindow"
            runat="server"
            Title="Print Budget"
            Width="275"
            Height="200"
            BodyPadding="10"
            AutoScroll="true"
            Hidden="true"
            Layout="FormLayout"
            Modal="true"
            CloseAction="Hide"
            Closable="true"
            Resizable ="false"
            Frame="true">
            <Items> 
                 <ext:FieldContainer ID="FieldContainer4" 
                        runat="server"
                        LabelStyle="font-weight:bold;padding:0;"
                        Layout="FitLayout">
                        <Items>
                         <ext:Checkbox ID="uxPrintNote" runat="server" BoxLabel="Print Notes" HideLabel="true"></ext:Checkbox>
                        </Items>
                    </ext:FieldContainer> 
            </Items>
            <Buttons>
                <ext:Button ID="Button2" runat="server" Text="Print" Icon="Printer" Disabled="false">
                    <DirectEvents>
                        <Click OnEvent="printOverheadBudget" Timeout="500000" Success="#{uxPrintWindow}.close();">
                            <EventMask ShowMask="true" Msg="Generating Report, Please Wait..." />
                        </Click>
                    </DirectEvents>
                </ext:Button>
                 <ext:Button ID="Button3" runat="server" Text="Cancel" Icon="Cancel" Disabled="false">
                    <Listeners>
                        <Click Handler="#{uxPrintWindow}.close();"></Click>
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umYearRollupSummary.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.umYearRollupSummary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .grandTotalBackground {
            background-color: black;
        }

        .grandTotalForeground .x-form-display-field {
            /*font-weight: bold;*/
            color: white;
        }
    </style>
    <script type="text/javascript">
        var isEditAdjustmentAllowed = function (e) {
            if (e.originalValue == 12345678910) {
                return false;
            }
        }
        var editAdjustment = function (editor, e) {
            if (e.originalValue != e.value) {
                SaveRecord.deSaveAdjustments(e.record.data.ADJ_ID, e.field, e.value);
            }
        }
        Ext.util.Format.CurrencyFactory = function (dp, dSeparator, tSeparator, symbol, red) {
            return function (n) {
                if (n == 12345678910) {
                    return " ";
                }

                var template = '<span style="color:{0};">{1}</span>';

                dp = Math.abs(dp) + 1 ? dp : 2;
                dSeparator = dSeparator || ".";
                tSeparator = tSeparator || ",";

                var m = /(\d+)(?:(\.\d+)|)/.exec(n + ""),
                    x = m[1].length > 3 ? m[1].length % 3 : 0;


                var r = (n < 0 ? '-' : '') // preserve minus sign
                        + (x ? m[1].substr(0, x) + tSeparator : "")
                        + m[1].substr(x).replace(/(\d{3})(?=\d)/g, "$1" + tSeparator)
                        + (dp ? dSeparator + (+m[2] || 0).toFixed(dp).substr(2) : "")
                        + symbol;

                return Ext.String.format(template, (n >= 0 || red == false) ? "black" : "red", r);
            };
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>

                <%-------------------------------------------------- Toolbar --------------------------------------------------%>
                <ext:Toolbar ID="uxMainToolbar" runat="server" Region="North">
                    <Items>

                        <ext:ToolbarFill />
                        <ext:ComboBox ID="uxRollupReports"
                            runat="server"
                            ValueField="ID_NAME"
                            DisplayField="ID_NAME"
                            Width="300"
                            EmptyText="-- Reports/Export --"
                            Editable="false">
                            <Store>
                                <ext:Store ID="uxRollupReportsStore" runat="server" OnReadData="deLoadRollupReports" AutoLoad="false">
                                    <Model>
                                        <ext:Model ID="Model6" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="ID_NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:PageProxy />
                                    </Proxy>
                                </ext:Store>
                            </Store>
                            <DirectEvents>
                                <Select OnEvent="deChooseRollupReport">
                                    <EventMask ShowMask="true" Msg="Processing..." />
                                </Select>
                            </DirectEvents>
                        </ext:ComboBox>

                    </Items>
                </ext:Toolbar>

                <%-------------------------------------------------- Top Summary Panel --------------------------------------------------%>
                <ext:GridPanel ID="uxSummaryGrid" runat="server" Region="North" Height="300">
                    <SelectionModel>
                        <ext:RowSelectionModel ID="uxGridRowModel" runat="server" AllowDeselect="false" Mode="Single" />
                    </SelectionModel>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxSummaryGridStore"
                            OnReadData="deReadSummaryGridData"
                            AutoDataBind="true"
                            WarningOnDirty="false">
                            <Model>
                                <ext:Model ID="Model1" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="NAME" />
                                        <ext:ModelField Name="GROSS_REC" />
                                        <ext:ModelField Name="MAT_USAGE" />
                                        <ext:ModelField Name="GROSS_REV" />
                                        <ext:ModelField Name="DIR_EXP" />
                                        <ext:ModelField Name="OP" />
                                        <ext:ModelField Name="OP_PERC" />
                                        <ext:ModelField Name="OH" />
                                        <ext:ModelField Name="NET_CONT" />
                                        <ext:ModelField Name="OP_VAR" />
                                        <ext:ModelField Name="NET_CONT_VAR" />
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
                            <ext:Column ID="Column1" runat="server" DataIndex="NAME" Text="" Flex="6" />
                            <ext:NumberColumn ID="Column5" runat="server" DataIndex="GROSS_REC" Text="Gross Receipts" Flex="2" Align="Right">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="Column6" runat="server" DataIndex="MAT_USAGE" Text="Material Usage" Flex="2" Align="Right">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="Column7" runat="server" DataIndex="GROSS_REV" Text="Gross Revenue" Flex="2" Align="Right">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="Column8" runat="server" DataIndex="DIR_EXP" Text="Direct Expenses" Flex="2" Align="Right">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="Column9" runat="server" DataIndex="OP" Text="OP" Flex="2" Align="Right">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="Column10" runat="server" DataIndex="OP_PERC" Text="OP %" Flex="2" Align="Right">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','%',false)" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn12" runat="server" DataIndex="OH" Text="Overhead" Flex="2" Align="Right">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn1" runat="server" DataIndex="NET_CONT" Text="Net Contribution" Flex="2" Align="Right">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn2" runat="server" DataIndex="OP_VAR" Text="OP +/-" Flex="2" Align="Right">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn7" runat="server" DataIndex="NET_CONT_VAR" Text="Net Cont. +/-" Flex="2" Align="Right">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                            </ext:NumberColumn>
                        </Columns>
                    </ColumnModel>
                    <DockedItems>
                        <ext:FieldContainer ID="uxTotal" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="grandTotalBackground">
                            <Items>
                                <ext:DisplayField ID="DisplayField6" runat="server" Width="10" />
                                <ext:DisplayField ID="DisplayField7" runat="server" Text="Total:" Flex="6" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxTGrossRec" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxTMatUsage" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxTGrossRev" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxTDirects" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxTOP" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxTOPPerc" runat="server" Text="0.00 %" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxTOH" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxTNetCont" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxTOPPlusMinus" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxTNetContPlusMinus" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField24" runat="server" Width="20" />
                            </Items>
                        </ext:FieldContainer>
                    </DockedItems>
                </ext:GridPanel>

                <%--<ext:GridPanel ID="uxAdjustmentsGrid" runat="server" Region="North" HideHeaders="true">
                    <SelectionModel>
                        <ext:RowSelectionModel ID="uxAdjustmentGridRowModel" runat="server" AllowDeselect="false" Mode="Single" />
                    </SelectionModel>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxAdjustmentGridStore"
                            OnReadData="deReadAdjustmentGridData"
                            AutoDataBind="true"
                            WarningOnDirty="false">
                            <Model>
                                <ext:Model ID="Model8" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ADJ_ID" />
                                        <ext:ModelField Name="ADJUSTMENT" />
                                        <ext:ModelField Name="BLANKCOL1" />
                                        <ext:ModelField Name="BLANKCOL2" />
                                        <ext:ModelField Name="BLANKCOL3" />
                                        <ext:ModelField Name="BLANKCOL4" />
                                        <ext:ModelField Name="MAT_ADJ" />
                                        <ext:ModelField Name="BLANKCOL5" />
                                        <ext:ModelField Name="WEATHER_ADJ" />
                                        <ext:ModelField Name="BLANKCOL6" />
                                        <ext:ModelField Name="BLANKCOL7" />
                                        <ext:ModelField Name="BLANKCOL8" />
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
                            <ext:Column ID="Column15" runat="server" DataIndex="ADJUSTMENT" Text="Adjustment" Flex="6" />
                            <ext:NumberColumn ID="Column24" runat="server" DataIndex="BLANKCOL4" Text="Blank" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="Column25" runat="server" DataIndex="MAT_ADJ" Text="Material" Flex="2" Align="Right">
                                <Editor>
                                    <ext:NumberField ID="NumberField2" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                </Editor>
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="Column26" runat="server" DataIndex="BLANKCOL5" Text="Blank" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="Column27" runat="server" DataIndex="WEATHER_ADJ" Text="Weather" Flex="2" Align="Right">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                <Editor>
                                    <ext:NumberField ID="NumberField1" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                </Editor>
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="Column21" runat="server" DataIndex="BLANKCOL1" Text="Blank" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="Column22" runat="server" DataIndex="BLANKCOL2" Text="Blank" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="Column23" runat="server" DataIndex="BLANKCOL3" Text="Blank" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="Column28" runat="server" DataIndex="BLANKCOL6" Text="Blank" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="Column29" runat="server" DataIndex="BLANKCOL7" Text="Blank" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="Column30" runat="server" DataIndex="BLANKCOL8" Text="Blank" Flex="2" Align="Right" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:CellEditing ID="CellEditing1" runat="server">
                            <Listeners>
                                <BeforeEdit Handler="return isEditAdjustmentAllowed(e);" />
                                <Edit Fn="editAdjustment" />
                            </Listeners>
                        </ext:CellEditing>
                    </Plugins>
                    <DockedItems>
                        <ext:FieldContainer ID="uxGrandTotal" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="grandTotalBackground">
                            <Items>
                                <ext:DisplayField ID="DisplayField11" runat="server" Width="10" />
                                <ext:DisplayField ID="DisplayField12" runat="server" Text="Grand Total:" Flex="6" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTGrossRec" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTMatUsage" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTGrossRev" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTDirects" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTOP" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTOPPerc" runat="server" Text="0.00 %" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField5" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField8" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTOPPlusMinus" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField4" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField31" runat="server" Width="20" />
                            </Items>
                        </ext:FieldContainer>
                    </DockedItems>
                </ext:GridPanel>--%>

                <ext:GridPanel ID="uxOverheadGrid" runat="server" Region="North" HideHeaders="true">
                    <SelectionModel>
                        <ext:RowSelectionModel ID="uxOverheadGridRowModel" runat="server" AllowDeselect="false" Mode="Single" />
                    </SelectionModel>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxOverheadGridStore"
                            OnReadData="deReadOverheadGridData"
                            AutoDataBind="true"
                            WarningOnDirty="false">
                            <Model>
                                <ext:Model ID="Model10" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ADJUSTMENT" />
                                        <ext:ModelField Name="OH" />
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
                            <ext:Column ID="Column34" runat="server" DataIndex="BLANK" Text="Blank" Width="10" />
                            <ext:Column ID="Column31" runat="server" DataIndex="ADJUSTMENT" Text="Adjustment" Flex="6" />
                            <ext:Column ID="Column32" runat="server" DataIndex="BLANK" Text="Blank" Flex="2" />
                            <ext:NumberColumn ID="NumberColumn3" runat="server" DataIndex="BLANK" Text="Blank" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="NumberColumn4" runat="server" DataIndex="BLANK" Text="Blank" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="NumberColumn5" runat="server" DataIndex="BLANK" Text="Blank" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="NumberColumn6" runat="server" DataIndex="BLANK" Text="Blank" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="NumberColumn61" runat="server" DataIndex="BLANK" Text="Blank" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="uxOH" runat="server" DataIndex="OH" Text="Overhead" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="NumberColumn8" runat="server" DataIndex="BLANK" Text="Blank" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="NumberColumn9" runat="server" DataIndex="BLANK" Text="Blank" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="NumberColumn10" runat="server" DataIndex="BLANK" Text="Blank" Flex="2" Align="Right" />
                            <ext:Column ID="Column2" runat="server" DataIndex="BLANK" Text="Blank" Width="20" />
                        </Columns>
                    </ColumnModel>
                    <DockedItems>
                        <ext:FieldContainer ID="FieldContainer19" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="grandTotalBackground">
                            <Items>
                                <%--                                <ext:DisplayField ID="DisplayField32" runat="server" Width="10" />
                                <ext:DisplayField ID="DisplayField33" runat="server" Text="Net Contribution:" Flex="6" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField34" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField37" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField38" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField39" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField40" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField42" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField1" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="uxNetCont" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField2" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField3" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField44" runat="server" Width="20" />--%>
                                <ext:DisplayField ID="DisplayField1" runat="server" Width="10" />
                                <ext:DisplayField ID="DisplayField2" runat="server" Text="Grand Total:" Flex="6" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTGrossRec" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTMatUsage" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTGrossRev" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTDirects" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTOP" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTOPPerc" runat="server" Text="0.00 %" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTOH" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTNetCont" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTOPPlusMinus" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTNetContPlusMinus" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField15" runat="server" Width="20" />
                            </Items>
                        </ext:FieldContainer>
                    </DockedItems>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

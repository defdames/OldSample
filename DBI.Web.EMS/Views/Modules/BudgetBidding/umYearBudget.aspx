<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umYearBudget.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.umYearBudget" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .activeBackground {
            background-color: darkolivegreen;
        }

        .inactiveBackground {
            background-color: gray;
        }

        .grandTotalBackground {
            background-color: black;
        }

        .activeForeground .x-form-display-field {
            /*font-weight: bold;*/
            color: white;
        }

        .inactiveForeground .x-form-display-field {
            /*font-weight: bold;*/
            color: black;
        }

        .grandTotalForeground .x-form-display-field {
            /*font-weight: bold;*/
            color: white;
        }

        .detailBackground {
            background-color: lightgray;
        }

        .detailForeground {
            font-weight: bold;
        }

        .detailForegroundCenter {
            font-weight: bold;
            text-align: center;
        }

        .textRightAlign .x-form-text {
            text-align: right;
        }

        .labelRightAlign {
            text-align: right;
        }

        .labelCenterAlign {
            text-align: center;
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

                        <ext:ComboBox ID="uxActions"
                            runat="server"
                            ValueField="ID_NAME"
                            DisplayField="ID_NAME"
                            Width="253"
                            EmptyText="-- Actions --"
                            Editable="false">
                            <Store>
                                <ext:Store ID="uxActionsStore" runat="server" OnReadData="deLoadSummaryActions" AutoLoad="false">
                                    <Model>
                                        <ext:Model ID="Model4" runat="server">
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
                                <Select OnEvent="deChooseSummaryAction">
                                    <EventMask ShowMask="true" Msg="Processing..." />
                                </Select>
                            </DirectEvents>
                        </ext:ComboBox>

                        <ext:Label ID="uxSpace1" runat="server" Width="10" />

                        <ext:ComboBox ID="uxSummaryReports"
                            runat="server"
                            ValueField="ACTION_NAME"
                            DisplayField="ACTION_NAME"
                            Width="200"
                            EmptyText="-- Reports/Export --"
                            Editable="false">
                            <Store>
                                <ext:Store ID="uxSummaryReportsStore" runat="server" OnReadData="deLoadReports" AutoLoad="false">
                                    <Model>
                                        <ext:Model ID="Model6" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="ACTION_ID" />
                                                <ext:ModelField Name="ACTION_NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:PageProxy />
                                    </Proxy>
                                </ext:Store>
                            </Store>
                            <DirectEvents>
                                <Select OnEvent="deChooseReport">
                                    <EventMask ShowMask="true" Msg="Processing..." />
                                </Select>
                            </DirectEvents>
                        </ext:ComboBox>

                        <ext:Label ID="uxSpace2" runat="server" Width="5" />

                        <ext:Button ID="uxUpdateAllActuals" runat="server" Text="Update All Actual" Icon="BookEdit">
                            <DirectEvents>
                                <Click OnEvent="deUpdateAllActuals">
                                    <EventMask ShowMask="true" Msg="Processing..." />
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>


                <%-------------------------------------------------- Top Summary Panel --------------------------------------------------%>
                <ext:GridPanel ID="uxSummaryGrid" runat="server" Region="North" Height="170">
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
                                        <ext:ModelField Name="BUD_BID_PROJECTS_ID" />
                                        <ext:ModelField Name="PROJECT_ID" />
                                        <ext:ModelField Name="PROJECT_NUM" />
                                        <ext:ModelField Name="TYPE" />
                                        <ext:ModelField Name="PROJECT_NAME" />
                                        <ext:ModelField Name="STATUS" />
                                        <ext:ModelField Name="ACRES" />
                                        <ext:ModelField Name="DAYS" />
                                        <ext:ModelField Name="GROSS_REC" />
                                        <ext:ModelField Name="MAT_USAGE" />
                                        <ext:ModelField Name="GROSS_REV" />
                                        <ext:ModelField Name="DIR_EXP" />
                                        <ext:ModelField Name="OP" />
                                        <ext:ModelField Name="OP_PERC" />
                                        <ext:ModelField Name="OP_VAR" />
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
                            <ext:Column ID="Column1" runat="server" DataIndex="PROJECT_NAME" Text="Project Long Name" Flex="6" />
                            <ext:Column ID="Column2" runat="server" DataIndex="STATUS" Text="Status" Flex="2" />
                            <ext:NumberColumn ID="Column3" runat="server" DataIndex="ACRES" Text="Acres" Flex="1" Align="Right">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="Column4" runat="server" DataIndex="DAYS" Text="Days" Flex="1" Align="Right">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                            </ext:NumberColumn>
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
                            <ext:NumberColumn ID="Column11" runat="server" DataIndex="OP_VAR" Text="OP +/-" Flex="2" Align="Right">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                            </ext:NumberColumn>
                        </Columns>
                    </ColumnModel>
                    <DirectEvents>
                        <Select OnEvent="deGetFormData">
                            <ExtraParams>
                                <ext:Parameter Name="BudBidProjectID" Value="#{uxSummaryGrid}.getSelectionModel().getSelection()[0].data.BUD_BID_PROJECTS_ID" Mode="Raw" />
                                <ext:Parameter Name="ProjectNumID" Value="#{uxSummaryGrid}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
                                <ext:Parameter Name="Type" Value="#{uxSummaryGrid}.getSelectionModel().getSelection()[0].data.TYPE" Mode="Raw" />
                                <ext:Parameter Name="ProjectNum" Value="#{uxSummaryGrid}.getSelectionModel().getSelection()[0].data.PROJECT_NUM" Mode="Raw" />
                                <ext:Parameter Name="ProjectName" Value="#{uxSummaryGrid}.getSelectionModel().getSelection()[0].data.PROJECT_NAME" Mode="Raw" />
                            </ExtraParams>
                            <EventMask ShowMask="true" />
                        </Select>
                    </DirectEvents>
                    <DockedItems>
                        <ext:FieldContainer ID="uxTotal" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="grandTotalBackground">
                            <Items>
                                <ext:DisplayField ID="DisplayField6" runat="server" Width="10" />
                                <ext:DisplayField ID="DisplayField7" runat="server" Text="Total Combined:" Flex="6" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField8" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField9" runat="server" Text="" Flex="1" />
                                <ext:DisplayField ID="DisplayField10" runat="server" Text="" Flex="1" />
                                <ext:DisplayField ID="uxTGrossRec" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxTMatUsage" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxTGrossRev" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxTDirects" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxTOP" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxTOPPerc" runat="server" Text="0.00 %" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxTOPPlusMinus" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField24" runat="server" Width="20" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="uxInactiveTotal" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="inactiveBackground">
                            <Items>
                                <ext:DisplayField ID="DisplayField1" runat="server" Width="10" />
                                <ext:DisplayField ID="DisplayField2" runat="server" Text="Total Inactive:" Flex="6" Cls="inactiveForeground" />
                                <ext:DisplayField ID="DisplayField3" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField4" runat="server" Text="" Flex="1" />
                                <ext:DisplayField ID="DisplayField5" runat="server" Text="" Flex="1" />
                                <ext:DisplayField ID="uxIGrossRec" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="inactiveForeground" />
                                <ext:DisplayField ID="uxIMatUsage" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="inactiveForeground" />
                                <ext:DisplayField ID="uxIGrossRev" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="inactiveForeground" />
                                <ext:DisplayField ID="uxIDirects" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="inactiveForeground" />
                                <ext:DisplayField ID="uxIOP" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="inactiveForeground" />
                                <ext:DisplayField ID="uxIOPPerc" runat="server" Text="0.00%" Flex="2" FieldStyle="text-align:right" Cls="inactiveForeground" />
                                <ext:DisplayField ID="uxIOPPlusMinus" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="inactiveForeground" />
                                <ext:DisplayField ID="DisplayField13" runat="server" Width="20" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="uxActiveTotal" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="activeBackground">
                            <Items>
                                <ext:DisplayField ID="DisplayField14" runat="server" Width="10" />
                                <ext:DisplayField ID="DisplayField15" runat="server" Text="Total Active:" Flex="6" Cls="activeForeground" />
                                <ext:DisplayField ID="DisplayField16" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField17" runat="server" Text="" Flex="1" />
                                <ext:DisplayField ID="DisplayField18" runat="server" Text="" Flex="1" />
                                <ext:DisplayField ID="uxAGrossRec" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="activeForeground" />
                                <ext:DisplayField ID="uxAMatUsage" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="activeForeground" />
                                <ext:DisplayField ID="uxAGrossRev" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="activeForeground" />
                                <ext:DisplayField ID="uxADirects" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="activeForeground" />
                                <ext:DisplayField ID="uxAOP" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="activeForeground" />
                                <ext:DisplayField ID="uxAOPPerc" runat="server" Text="0.00%" Flex="2" FieldStyle="text-align:right" Cls="activeForeground" />
                                <ext:DisplayField ID="uxAOPPlusMinus" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="activeForeground" />
                                <ext:DisplayField ID="DisplayField26" runat="server" Width="20" />
                            </Items>
                        </ext:FieldContainer>
                    </DockedItems>
                </ext:GridPanel>

                <ext:GridPanel ID="uxAdjustmentsGrid" runat="server" Region="North" HideHeaders="true" Visible="false">
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
                            <ext:Column ID="Column21" runat="server" DataIndex="BLANKCOL1" Text="Blank" Flex="2" />
                            <ext:NumberColumn ID="Column22" runat="server" DataIndex="BLANKCOL2" Text="Blank" Flex="1" Align="Right" />
                            <ext:NumberColumn ID="Column23" runat="server" DataIndex="BLANKCOL3" Text="Blank" Flex="1" Align="Right" />
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
                            <ext:NumberColumn ID="Column28" runat="server" DataIndex="BLANKCOL6" Text="Blank" Flex="2" Align="Right" />
                            <ext:Column ID="Column29" runat="server" DataIndex="BLANKCOL7" Text="Blank" Flex="2" Align="Right" />
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
                                <ext:DisplayField ID="DisplayField19" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField20" runat="server" Text="" Flex="1" />
                                <ext:DisplayField ID="DisplayField21" runat="server" Text="" Flex="1" />
                                <ext:DisplayField ID="uxGTGrossRec" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTMatUsage" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTGrossRev" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTDirects" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTOP" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTOPPerc" runat="server" Text="0.00 %" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="uxGTOPPlusMinus" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField31" runat="server" Width="20" />
                            </Items>
                        </ext:FieldContainer>
                    </DockedItems>
                </ext:GridPanel>

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
                            <ext:Column ID="Column31" runat="server" DataIndex="ADJUSTMENT" Text="Adjustment" Flex="6" />
                            <ext:Column ID="Column32" runat="server" DataIndex="BLANK" Text="Blank" Flex="2" />
                            <ext:NumberColumn ID="NumberColumn1" runat="server" DataIndex="BLANK" Text="Blank" Flex="1" Align="Right" />
                            <ext:NumberColumn ID="NumberColumn2" runat="server" DataIndex="BLANK" Text="Blank" Flex="1" Align="Right" />
                            <ext:NumberColumn ID="NumberColumn3" runat="server" DataIndex="BLANK" Text="Blank" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="NumberColumn4" runat="server" DataIndex="BLANK" Text="Blank" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="NumberColumn5" runat="server" DataIndex="BLANK" Text="Blank" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="NumberColumn6" runat="server" DataIndex="BLANK" Text="Blank" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="uxOH" runat="server" DataIndex="OH" Text="Overhead" Flex="2" Align="Right" />
                            <ext:Column ID="Column33" runat="server" DataIndex="BLANK" Text="Blank" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="NumberColumn8" runat="server" DataIndex="BLANK" Text="Blank" Flex="2" Align="Right" />
                        </Columns>
                    </ColumnModel>
                    <DockedItems>
                        <ext:FieldContainer ID="FieldContainer19" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="grandTotalBackground">
                            <Items>
                                <ext:DisplayField ID="DisplayField32" runat="server" Width="10" />
                                <ext:DisplayField ID="DisplayField33" runat="server" Text="Net Contribution:" Flex="6" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField34" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField35" runat="server" Text="" Flex="1" />
                                <ext:DisplayField ID="DisplayField36" runat="server" Text="" Flex="1" />
                                <ext:DisplayField ID="DisplayField37" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField38" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField39" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField40" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="uxNetCont" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField42" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField43" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField44" runat="server" Width="20" />
                            </Items>
                        </ext:FieldContainer>
                    </DockedItems>
                </ext:GridPanel>


                <%-------------------------------------------------- Bottom Form Panel --------------------------------------------------%>
                <ext:FormPanel ID="uxProjectInfo"
                    runat="server"
                    Region="Center"
                    AutoScroll="true"
                    BodyPadding="20"
                    Disabled="true">
                    <Items>
                        <ext:FieldContainer ID="FieldContainer3"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label1" runat="server" Width="120" Text="Project Number:" />
                                <ext:DropDownField ID="uxProjectNum" runat="server" Width="110" Mode="ValueText" Editable="false">
                                    <Listeners>
                                        <Expand Handler="this.picker.setWidth(500);" />
                                    </Listeners>
                                    <Component>
                                        <ext:GridPanel runat="server"
                                            ID="uxProjectList"
                                            Width="500"
                                            Layout="HBoxLayout"
                                            Frame="true"
                                            ForceFit="true">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxProjectNumStore"
                                                    PageSize="10"
                                                    RemoteSort="true"
                                                    OnReadData="deLoadProjectDropdown">
                                                    <Model>
                                                        <ext:Model ID="Model7" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="PROJECT_ID" />
                                                                <ext:ModelField Name="PROJECT_NUM" Type="String" />
                                                                <ext:ModelField Name="PROJECT_NAME" />
                                                                <ext:ModelField Name="TYPE" />
                                                                <ext:ModelField Name="ORDERKEY" />
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
                                                    <ext:Column ID="Column13" runat="server" Text="Project Number" DataIndex="PROJECT_NUM" Flex="1" />
                                                    <ext:Column ID="Column14" runat="server" Text="Project Long Name" DataIndex="PROJECT_NAME" Flex="3" />
                                                </Columns>
                                            </ColumnModel>
                                            <BottomBar>
                                                <ext:PagingToolbar ID="PagingToolbar3" runat="server" />
                                            </BottomBar>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel3" runat="server" Mode="Single" />
                                            </SelectionModel>
                                            <DirectEvents>
                                                <SelectionChange OnEvent="deSelectProject">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ProjectID" Value="#{uxProjectList}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
                                                        <ext:Parameter Name="ProjectNum" Value="#{uxProjectList}.getSelectionModel().getSelection()[0].data.PROJECT_NUM" Mode="Raw" />
                                                        <ext:Parameter Name="ProjectName" Value="#{uxProjectList}.getSelectionModel().getSelection()[0].data.PROJECT_NAME" Mode="Raw" />
                                                        <ext:Parameter Name="Type" Value="#{uxProjectList}.getSelectionModel().getSelection()[0].data.TYPE" Mode="Raw" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" />
                                                </SelectionChange>
                                                <SelectionChange OnEvent="deCheckAllowSave" />
                                            </DirectEvents>
                                            <Plugins>
                                                <ext:FilterHeader runat="server" ID="uxProjectFilter" Remote="true" />
                                            </Plugins>
                                        </ext:GridPanel>
                                    </Component>
                                </ext:DropDownField>
                                <ext:Label ID="Label5" runat="server" Width="300" />

                                <ext:Checkbox ID="uxCompareOverride" runat="server" BoxLabel="Compare to Override" Width="200">
                                    <DirectEvents>
                                        <Change OnEvent="deCompareCheck" />
                                    </DirectEvents>
                                </ext:Checkbox>
                                <ext:Label ID="Label6" runat="server" Width="65" />
                                <ext:Label ID="Label4" runat="server" Width="40" Text="Acres:" />
                                <ext:TextField ID="uxAcres" runat="server" Width="110" ReadOnly="false" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" SelectOnFocus="true">
                                    <Listeners>
                                        <Focus Handler="this.setValue(this.getValue().replace(/,/g, ''));" />
                                    </Listeners>
                                    <DirectEvents>
                                        <Blur OnEvent="deFormatNumber" />
                                    </DirectEvents>
                                </ext:TextField>
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer4"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label7" runat="server" Width="120" Text="Project Long Name:" />
                                <ext:TextField ID="uxProjectName" runat="server" Width="380" ReadOnly="true" SelectOnFocus="true" MaxLength="200" EnforceMaxLength="true">
                                    <DirectEvents>
                                        <Change OnEvent="deCheckAllowSave" />
                                    </DirectEvents>
                                </ext:TextField>
                                <ext:Label ID="Label8" runat="server" Width="30" />
                                <ext:Label ID="uxVersionLabel" runat="server" Width="115" Text="Final Draft OP:" />
                                <ext:TextField ID="uxCompareOP" runat="server" Width="110" ReadOnly="false" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" Disabled="true" SelectOnFocus="true">
                                    <Listeners>
                                        <Focus Handler="this.setValue(this.getValue().replace(/,/g, ''));" />
                                    </Listeners>
                                    <DirectEvents>
                                        <Blur OnEvent="deUpdateCompareNums" />
                                    </DirectEvents>
                                </ext:TextField>
                                <ext:Label ID="Label10" runat="server" Width="40" />
                                <ext:Label ID="Label11" runat="server" Width="40" Text="Days:" />
                                <ext:TextField ID="uxDays" runat="server" Width="110" ReadOnly="false" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" SelectOnFocus="true">
                                    <Listeners>
                                        <Focus Handler="this.setValue(this.getValue().replace(/,/g, ''));" />
                                    </Listeners>
                                    <DirectEvents>
                                        <Blur OnEvent="deFormatNumber" />
                                    </DirectEvents>
                                </ext:TextField>
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer5"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label13" runat="server" Width="120" Text="Status:" />
                                <ext:ComboBox ID="uxStatus"
                                    runat="server"
                                    ValueField="ID"
                                    DisplayField="ID_NAME"
                                    Width="110"
                                    EmptyText="-- Select --"
                                    Editable="false">
                                    <Store>
                                        <ext:Store ID="uxStatusStore" runat="server" OnReadData="deLoadStatusDropdown" AutoLoad="false">
                                            <Model>
                                                <ext:Model ID="Model9" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
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
                                        <Select OnEvent="deSelectStatus" />
                                        <Select OnEvent="deCheckAllowSave" />
                                    </DirectEvents>
                                </ext:ComboBox>
                                <ext:Label ID="Label14" runat="server" Width="300" />
                                <ext:Label ID="Label15" runat="server" Width="100" Text="Variance:" />
                                <ext:Label ID="uxCompareVar" runat="server" Width="121" Text="0.00" Cls="labelRightAlign" />
                                <ext:Label ID="Label2" runat="server" Width="194" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer6"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label18" runat="server" Width="120" Text="Comments:" />
                                <ext:TextArea ID="uxComments" runat="server" Width="530" SelectOnFocus="true" />
                                <ext:Label ID="Label20" runat="server" Width="75" Text="" />
                                <ext:FieldContainer ID="FieldContainer7"
                                    runat="server"
                                    Layout="VBoxLayout">
                                    <Items>
                                        <ext:FieldContainer ID="FieldContainer10"
                                            runat="server"
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:Checkbox ID="uxLiabilityCheckbox" runat="server" BoxLabel="Liability:" Width="85">
                                                    <DirectEvents>
                                                        <Change OnEvent="deLiabilityCheck" />
                                                    </DirectEvents>
                                                </ext:Checkbox>
                                                <ext:Label ID="Label21" runat="server" Width="25" Icon="Information">
                                                    <ToolTips>
                                                        <ext:ToolTip ID="ToolTip2"
                                                            runat="server"
                                                            Target="Label21"
                                                            Anchor="top"
                                                            TrackMouse="true"
                                                            Html="Please enter any relevant notes in the comments section to the left." />
                                                    </ToolTips>
                                                </ext:Label>
                                                <ext:TextField ID="uxLiabilityAmount" runat="server" Width="110" ReadOnly="false" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" Disabled="true" SelectOnFocus="true">
                                                    <Listeners>
                                                        <Focus Handler="this.setValue(this.getValue().replace(/,/g, ''));" />
                                                    </Listeners>
                                                    <DirectEvents>
                                                        <Blur OnEvent="deFormatNumber" />
                                                    </DirectEvents>
                                                </ext:TextField>
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer ID="FieldContainer8"
                                            runat="server"
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:Label ID="Label17" runat="server" Width="85" Text="App Type:" />
                                                <ext:TextField ID="uxAppType" runat="server" Width="135" SelectOnFocus="true" MaxLength="50" EnforceMaxLength="true" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer ID="FieldContainer9"
                                            runat="server"
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:Label ID="Label23" runat="server" Width="85" Text="Chemical Mix:" />
                                                <ext:TextField ID="uxChemMix" runat="server" Width="135" SelectOnFocus="true" MaxLength="50" EnforceMaxLength="true" />
                                            </Items>
                                        </ext:FieldContainer>
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer11"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label36" runat="server" Width="60" />
                            </Items>
                        </ext:FieldContainer>

                        <%----- Begin Detail Sheet Section -----%>
                        <ext:FieldSet ID="FieldSet1" runat="server" Width="930" Padding="10" Cls="detailBackground">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer12"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label25" runat="server" Width="150" Text="Actuals Through" Cls="detailForeground" />
                                        <ext:Label ID="Label29" runat="server" Width="8" />
                                        <ext:Label ID="Label26" runat="server" Width="110" Text="Gross Receipts" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label27" runat="server" Width="110" Text="Material Usage" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label30" runat="server" Width="110" Text="Gross Revenue" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label28" runat="server" Width="110" Text="Direct Expenses" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label31" runat="server" Width="110" Text="OP" Cls="detailForegroundCenter" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer13"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:ComboBox ID="uxJCDate"
                                            runat="server"
                                            ValueField="ID_NAME"
                                            DisplayField="ID_NAME"
                                            Width="110"
                                            EmptyText="-- Select --"
                                            Editable="false">
                                            <Store>
                                                <ext:Store ID="uxJCDateStore" runat="server" OnReadData="deLoadJCDateDropdown" AutoLoad="false">
                                                    <Model>
                                                        <ext:Model ID="Model5" runat="server">
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
                                                <Select OnEvent="deSelectJCDate">
                                                    <EventMask ShowMask="true" />
                                                </Select>
                                            </DirectEvents>
                                        </ext:ComboBox>
                                        <ext:Label ID="Label34" runat="server" Width="50" />
                                        <ext:TextField ID="uxSGrossRec" runat="server" Width="110" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" SelectOnFocus="true">
                                            <Listeners>
                                                <Focus Handler="this.setValue(this.getValue().replace(/,/g, ''));" />
                                            </Listeners>
                                            <DirectEvents>
                                                <Blur OnEvent="deCalcGRandOP">
                                                    <EventMask ShowMask="true" Msg="Processing..." />
                                                </Blur>
                                            </DirectEvents>
                                        </ext:TextField>
                                        <ext:TextField ID="uxSMatUsage" runat="server" Width="110" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" SelectOnFocus="true">
                                            <Listeners>
                                                <Focus Handler="this.setValue(this.getValue().replace(/,/g, ''));" />
                                            </Listeners>
                                            <DirectEvents>
                                                <Blur OnEvent="deCalcGRandOP">
                                                    <EventMask ShowMask="true" Msg="Processing..." />
                                                </Blur>
                                            </DirectEvents>
                                        </ext:TextField>
                                        <ext:TextField ID="uxSGrossRev" runat="server" Width="110" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" SelectOnFocus="true" TabIndex="-1" />
                                        <ext:TextField ID="uxSDirects" runat="server" Width="110" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" SelectOnFocus="true">
                                            <Listeners>
                                                <Focus Handler="this.setValue(this.getValue().replace(/,/g, ''));" />
                                            </Listeners>
                                            <DirectEvents>
                                                <Blur OnEvent="deCalcGRandOP">
                                                    <EventMask ShowMask="true" Msg="Processing..." />
                                                </Blur>
                                            </DirectEvents>
                                        </ext:TextField>
                                        <ext:TextField ID="uxSOP" runat="server" Width="110" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" SelectOnFocus="true" TabIndex="-2" />
                                        <ext:Label ID="Label35" runat="server" Width="40" />
                                        <ext:ComboBox ID="uxDetailActions"
                                            runat="server"
                                            ValueField="ID_NAME"
                                            DisplayField="ID_NAME"
                                            Width="150"
                                            EmptyText="-- Actions --"
                                            Editable="false">
                                            <Store>
                                                <ext:Store ID="uxDetailActionsStore" runat="server" OnReadData="deLoadDetailActions" AutoLoad="false">
                                                    <Model>
                                                        <ext:Model ID="Model3" runat="server">
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
                                                <Select OnEvent="deChooseDetailAction">
                                                    <EventMask ShowMask="true" Msg="Processing..." />
                                                </Select>
                                            </DirectEvents>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer14"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:GridPanel ID="uxDetailSheets" runat="server" Width="730" HideHeaders="true" Height="70">
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" AllowDeselect="false" Mode="Single" />
                                            </SelectionModel>
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxSummaryDetailStore"
                                                    OnReadData="deReadDetailGridData"
                                                    AutoDataBind="true"
                                                    WarningOnDirty="false">
                                                    <Model>
                                                        <ext:Model ID="Model2" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DETAIL_TASK_ID" />
                                                                <ext:ModelField Name="DETAIL_NAME" />
                                                                <ext:ModelField Name="SHEET_ORDER" />
                                                                <ext:ModelField Name="GROSS_REC" />
                                                                <ext:ModelField Name="MAT_USAGE" />
                                                                <ext:ModelField Name="GROSS_REV" />
                                                                <ext:ModelField Name="DIR_EXP" />
                                                                <ext:ModelField Name="OP" />
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
                                                    <ext:Column ID="Column12" runat="server" DataIndex="DETAIL_NAME" Text="Detail Sheet" Width="160" />
                                                    <ext:NumberColumn ID="Column16" runat="server" DataIndex="GROSS_REC" Text="Gross Receipts" Width="110" Align="Right">
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="Column17" runat="server" DataIndex="MAT_USAGE" Text="Material Usage" Width="110" Align="Right">
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="Column18" runat="server" DataIndex="GROSS_REV" Text="Gross Revenue" Width="110" Align="Right">
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="Column19" runat="server" DataIndex="DIR_EXP" Text="Direct Expenses" Width="110" Align="Right">
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="Column20" runat="server" DataIndex="OP" Text="OP" Width="110" Align="Right">
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                </Columns>
                                            </ColumnModel>
                                            <DirectEvents>
                                                <Select OnEvent="deSelectDetailSheet">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="DetailSheetID" Value="#{uxDetailSheets}.getSelectionModel().getSelection()[0].data.DETAIL_TASK_ID" Mode="Raw" />
                                                        <ext:Parameter Name="DetailSheetOrder" Value="#{uxDetailSheets}.getSelectionModel().getSelection()[0].data.SHEET_ORDER" Mode="Raw" />
                                                        <ext:Parameter Name="DetailSheetName" Value="#{uxDetailSheets}.getSelectionModel().getSelection()[0].data.DETAIL_NAME" Mode="Raw" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" />
                                                </Select>
                                            </DirectEvents>
                                        </ext:GridPanel>
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer15"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label32" runat="server" Width="160" />
                                        <ext:TextField ID="uxEGrossRec" runat="server" Width="110" ReadOnly="true" Text="0.00" Cls="textRightAlign" SelectOnFocus="true" TabIndex="-3"/>
                                        <ext:TextField ID="uxEMatUsage" runat="server" Width="110" ReadOnly="true" Text="0.00" Cls="textRightAlign" SelectOnFocus="true" TabIndex="-4"/>
                                        <ext:TextField ID="uxEGrossRev" runat="server" Width="110" ReadOnly="true" Text="0.00" Cls="textRightAlign" SelectOnFocus="true" TabIndex="-5"/>
                                        <ext:TextField ID="uxEDirects" runat="server" Width="110" ReadOnly="true" Text="0.00" Cls="textRightAlign" SelectOnFocus="true" TabIndex="-6"/>
                                        <ext:TextField ID="uxEOP" runat="server" Width="110" ReadOnly="true" Text="0.00" Cls="textRightAlign" SelectOnFocus="true" TabIndex="-7"/>
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer16"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label37" runat="server" Width="60" />
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:FieldSet>
                        <ext:FieldContainer ID="FieldContainer18"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label12" runat="server" Width="60" />
                            </Items>
                        </ext:FieldContainer>
                        <%----- End Detail Sheet Section -----%>

                        <ext:FieldContainer ID="FieldContainer17"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label3" runat="server" Width="770" />
                                <ext:Button ID="uxSave" runat="server" Text="Save" Icon="Add" Width="75">
                                    <DirectEvents>
                                        <Click OnEvent="deSave">
                                            <EventMask ShowMask="true" Msg="Saving..." />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Label ID="Label16" runat="server" Width="5" />
                                <ext:Button ID="uxCancel" runat="server" Text="Cancel" Icon="Delete" Width="75">
                                    <DirectEvents>
                                        <Click OnEvent="deCancel">
                                            <EventMask ShowMask="true" Msg="Processing..." />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                </ext:FormPanel>

                <%-------------------------------------------------- Diagnostic Panel --------------------------------------------------%>
                <%-- Uncomment to Use --%>
                <%--<ext:FormPanel ID="FormPanel1"
                    runat="server"
                    Region="East"
                    Width="130"
                    AutoScroll="true"
                    BodyPadding="10">
                    <Items>
                        <ext:FieldContainer ID="FieldContainer20"
                            runat="server"
                            Layout="VBoxLayout">
                            <Items>
                                <ext:Label ID="Label42" runat="server" Width="100" Text="uxHidBudBidID" Cls="labelCenterAlign" />
                                <ext:TextField ID="uxHidBudBidID" runat="server" Width="100" />
                                <ext:Label ID="Label43" runat="server" Width="100" Text="uxHidProjectNumID" Cls="labelCenterAlign" />
                                <ext:TextField ID="uxHidProjectNumID" runat="server" Width="100" />
                                <ext:Label ID="Label44" runat="server" Width="100" Text="uxHidType" Cls="labelCenterAlign" />
                                <ext:TextField ID="uxHidType" runat="server" Width="100" />
                                <ext:Label ID="Label45" runat="server" Width="100" Text="uxHidStatusID" Cls="labelCenterAlign" />
                                <ext:TextField ID="uxHidStatusID" runat="server" Width="100" />
                                <ext:Label ID="Label46" runat="server" Width="100" Text="uxHidPrevYear" Cls="labelCenterAlign" />
                                <ext:TextField ID="uxHidPrevYear" runat="server" Width="100" />
                                <ext:Label ID="Label47" runat="server" Width="100" Text="uxHidPrevVer" Cls="labelCenterAlign" />
                                <ext:TextField ID="uxHidPrevVer" runat="server" Width="100" />
                                <ext:Label ID="Label19" runat="server" Width="100" Text="uxHidFormEnabled" Cls="labelCenterAlign" />
                                <ext:TextField ID="uxHidFormEnabled" runat="server" Width="100" />
                                <ext:Label ID="Label9" runat="server" Width="100" Text="uxHidOldBudBidID" Cls="labelCenterAlign" />
                                <ext:TextField ID="uxHidOldBudBidID" runat="server" Width="100" />
                                <ext:Label ID="Label22" runat="server" Width="100" Text="uxHidDetailSheetID" Cls="labelCenterAlign" />
                                <ext:TextField ID="uxHidDetailSheetID" runat="server" Width="100" />
                                <ext:Label ID="Label24" runat="server" Width="100" Text="uxHidDetailSheetOrder" Cls="labelCenterAlign" />
                                <ext:TextField ID="uxHidDetailSheetOrder" runat="server" Width="100" />
                                <ext:Label ID="Label33" runat="server" Width="100" Text="uxHidDetailSheetName" Cls="labelCenterAlign" />
                                <ext:TextField ID="uxHidDetailSheetName" runat="server" Width="100" />
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                </ext:FormPanel>--%>
                <%-- Uncomment to Use --%>

                <%-- Uncomment to Use --%>
                <ext:Hidden ID="uxHidBudBidID" runat="server" />
                <ext:Hidden ID="uxHidProjectNumID" runat="server" />
                <ext:Hidden ID="uxHidType" runat="server" />
                <ext:Hidden ID="uxHidStatusID" runat="server" />
                <ext:Hidden ID="uxHidPrevYear" runat="server" />
                <ext:Hidden ID="uxHidPrevVer" runat="server" />
                <ext:Hidden ID="uxHidFormEnabled" runat="server" />
                <ext:Hidden ID="uxHidOldBudBidID" runat="server" />
                <ext:Hidden ID="uxHidDetailSheetID" runat="server" />
                <ext:Hidden ID="uxHidDetailSheetOrder" runat="server" />
                <ext:Hidden ID="uxHidDetailSheetName" runat="server" />
                <%-- Uncomment to Use --%>
                <%-------------------------------------------------- Diagnostic Panel --------------------------------------------------%>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

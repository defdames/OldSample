<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umDetailSheet.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.umDetailSheet" %>

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

        .detailBackground {
            background-color: lightgray;
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
    </style>
    <script type="text/javascript">
        var editRecord = function (editor, e) {
            if (e.originalValue != e.value) {
                SaveRecord.deSaveSubGridData(e.record.data.DETAIL_SHEET_ID, e.record.data.REC_TYPE, e.field, e.value);
            }
        }
        var closeWindow = function () {
            parent.App.direct.CloseDetailWindow(App.uxDetailName.value);
            parent.Ext.getCmp('uxAddEditDetailSheet').close();
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
        // EXAMPLE
        //var colorErrors = function (value, metadata, record) {
        //    if (record.data.WARNING == "Error") {
        //        metadata.style = "background-color: red;";
        //    }
        //    else if (record.data.WARNING == "Warning") {
        //        metadata.style = "background-color: yellow;";
        //    }
        //    return value;
        //};
        var colorSubGridOverride = function (value, metadata, record) {
            if (record.data.OVERRIDDEN == 1) {
                metadata.style = "background-color: #FFFF99;";
            }

            if (isNaN(value) || isNaN(parseInt(value))) { return value; }

            var template = '<span style="color:{0};">{1}</span>';

            var dp = 2;
            var dSeparator = ".";
            var tSeparator = ",";
            var symbol = '';
            var red = false;

            var m = /(\d+)(?:(\.\d+)|)/.exec(value + ""),
                x = m[1].length > 3 ? m[1].length % 3 : 0;
            var r = (value < 0 ? '-' : '') // preserve minus sign
                    + (x ? m[1].substr(0, x) + tSeparator : "")
                    + m[1].substr(x).replace(/(\d{3})(?=\d)/g, "$1" + tSeparator)
                    + (dp ? dSeparator + (+m[2] || 0).toFixed(dp).substr(2) : "")
                    + symbol;

            return Ext.String.format(template, (value >= 0 || red == false) ? "black" : "red", r);
        };
        var colorSubGridOverrideMaterial = function (value, metadata, record) {
            if (record.data.OVERRIDDEN == 1) {
                metadata.style = "background-color: #FFFF99;";
            }

            if (isNaN(value) || isNaN(parseInt(value))) { return value; }

            var template = '<span style="color:{0};">{1}</span>';

            var dp = 4;
            var dSeparator = ".";
            var tSeparator = ",";
            var symbol = '';
            var red = false;

            var m = /(\d+)(?:(\.\d+)|)/.exec(value + ""),
                x = m[1].length > 3 ? m[1].length % 3 : 0;
            var r = (value < 0 ? '-' : '') // preserve minus sign
                    + (x ? m[1].substr(0, x) + tSeparator : "")
                    + m[1].substr(x).replace(/(\d{3})(?=\d)/g, "$1" + tSeparator)
                    + (dp ? dSeparator + (+m[2] || 0).toFixed(dp).substr(2) : "")
                    + symbol;

            return Ext.String.format(template, (value >= 0 || red == false) ? "black" : "red", r);
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>


                <%-------------------------------------------------- Top --------------------------------------------------%>
                <ext:FormPanel ID="FormPanel2"
                    runat="server"
                    Region="North"
                    BodyPadding="10"
                    Disabled="false">
                    <Items>
                        <ext:FieldContainer ID="FieldContainer3"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="uxYearVersion" runat="server" Width="200" Text="2014 2nd Reforecast" />
                                <ext:Label ID="Label1" runat="server" Width="245" />
                                <ext:Label ID="uxWeekEnding" runat="server" Width="220" Text="Week Ending:  N/A" Cls="labelRightAlign" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer6"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label12" runat="server" Width="665" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer5"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label2" runat="server" Width="144" Text="Project Name:" />
                                <ext:Label ID="uxProjectName" runat="server" Width="480" Text="" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer4"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="uxDetailNameLabel" runat="server" Width="140" Text="Detail Sheet (1 of 1): " />
                                <ext:TextField ID="uxDetailName" runat="server" Width="480" ReadOnly="false" Text="" SelectOnFocus="true" MaxLength="200" EnforceMaxLength="true" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;">
                                    <DirectEvents>
                                        <Change OnEvent="deCheckAllowDetailSave" />
                                    </DirectEvents>
                                </ext:TextField>
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer7"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label7" runat="server" Width="665" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldSet ID="FieldSet1" runat="server" Width="665" Padding="10" Cls="detailBackground">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer12"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label4" runat="server" Width="77" />
                                        <ext:Label ID="Label26" runat="server" Width="100" Text="Gross Receipts" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label27" runat="server" Width="100" Text="Material Usage" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label30" runat="server" Width="100" Text="Gross Revenue" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label28" runat="server" Width="100" Text="Direct Expenses" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label31" runat="server" Width="100" Text="OP" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label5" runat="server" Width="77" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer13"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label9" runat="server" Width="77" />
                                        <ext:TextField ID="uxSGrossRec" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" TabIndex="-1" />
                                        <ext:TextField ID="uxSMatUsage" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" TabIndex="-2" />
                                        <ext:TextField ID="uxSGrossRev" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" TabIndex="-3" />
                                        <ext:TextField ID="uxSDirects" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" TabIndex="-4" />
                                        <ext:TextField ID="uxSOP" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" TabIndex="-5" />
                                        <ext:Label ID="Label10" runat="server" Width="77" />
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:FieldSet>
                    </Items>
                </ext:FormPanel>


                <%-------------------------------------------------- Middle --------------------------------------------------%>
                <ext:TabPanel ID="uxTabs" runat="server" Region="Center">
                    <Items>

                        <%--------------------------- Main ---------------------------%>
                        <ext:Panel runat="server"
                            Title="Main"
                            ID="uxMain"
                            Disabled="false"
                            BodyPadding="10"
                            Layout="FitLayout">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer33"
                                    runat="server"
                                    Layout="VBoxLayout">
                                    <Items>
                                        <ext:FieldContainer ID="FieldContainer34"
                                            runat="server"
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:Label ID="Label36" runat="server" Width="180" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer ID="FieldContainer31"
                                            runat="server"
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:Label ID="Label6" runat="server" Width="180" Text="Total Receipts Remaining:" />
                                                <ext:TextField ID="uxRecRemaining" runat="server" Width="100" ReadOnly="false" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" SelectOnFocus="true" TabIndex="2">
                                                    <DirectEvents>
                                                        <Blur OnEvent="deSaveMainTabField">
                                                            <ExtraParams>
                                                                <ext:Parameter Name="RecType" Value="RECREMAIN" Mode="Value" />
                                                                <ext:Parameter Name="FieldText" Value="#{uxRecRemaining}.value" Mode="Raw" />
                                                            </ExtraParams>
                                                            <EventMask ShowMask="true" Msg="Updating..." />
                                                        </Blur>
                                                    </DirectEvents>
                                                </ext:TextField>
                                                <ext:Label ID="Label29" runat="server" Width="35" />
                                                <ext:Label ID="Label32" runat="server" Width="250" Text="Total Days Remaining (including resprays):" />
                                                <ext:TextField ID="uxDaysRemaining" runat="server" Width="100" ReadOnly="false" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" SelectOnFocus="true" TabIndex="3">
                                                    <DirectEvents>
                                                        <Blur OnEvent="deSaveMainTabField">
                                                            <ExtraParams>
                                                                <ext:Parameter Name="RecType" Value="DAYSREMAIN" Mode="Value" />
                                                                <ext:Parameter Name="FieldText" Value="#{uxDaysRemaining}.value" Mode="Raw" />
                                                            </ExtraParams>
                                                            <EventMask ShowMask="true" Msg="Updating..." />
                                                        </Blur>
                                                    </DirectEvents>
                                                </ext:TextField>
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer ID="FieldContainer32"
                                            runat="server"
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:Label ID="Label44" runat="server" Width="180" Text="Total Days Per Week Worked:" />
                                                <ext:TextField ID="uxDaysWorked" runat="server" Width="100" ReadOnly="false" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" SelectOnFocus="true" TabIndex="4">
                                                    <DirectEvents>
                                                        <Blur OnEvent="deSaveMainTabField">
                                                            <ExtraParams>
                                                                <ext:Parameter Name="RecType" Value="DAYSWORKED" Mode="Value" />
                                                                <ext:Parameter Name="FieldText" Value="#{uxDaysWorked}.value" Mode="Raw" />
                                                            </ExtraParams>
                                                            <EventMask ShowMask="true" Msg="Updating..." />
                                                        </Blur>
                                                    </DirectEvents>
                                                </ext:TextField>
                                                <ext:Label ID="Label35" runat="server" Width="35" />
                                                <ext:Label ID="Label33" runat="server" Width="250" Text="Total Units Remaining (including resprays):" />
                                                <ext:TextField ID="uxUnitsRemaining" runat="server" Width="100" ReadOnly="false" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" SelectOnFocus="true" TabIndex="5">
                                                    <DirectEvents>
                                                        <Blur OnEvent="deSaveMainTabField">
                                                            <ExtraParams>
                                                                <ext:Parameter Name="RecType" Value="UNITREMAIN" Mode="Value" />
                                                                <ext:Parameter Name="FieldText" Value="#{uxUnitsRemaining}.value" Mode="Raw" />
                                                            </ExtraParams>
                                                            <EventMask ShowMask="true" Msg="Updating..." />
                                                        </Blur>
                                                    </DirectEvents>
                                                </ext:TextField>
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer ID="FieldContainer35"
                                            runat="server"
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:Label ID="Label49" runat="server" Width="670" Text="" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer ID="FieldContainer38"
                                            runat="server"
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:Label ID="Label45" runat="server" Width="180" Text="Comments:" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer ID="FieldContainer36"
                                            runat="server"
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextArea ID="uxComments" runat="server" Height="40" Width="665" ReadOnly="false" SelectOnFocus="true" TabIndex="6">
                                                    <DirectEvents>
                                                        <Blur OnEvent="deSaveMainTabField">
                                                            <ExtraParams>
                                                                <ext:Parameter Name="RecType" Value="COMMENTS" Mode="Value" />
                                                                <ext:Parameter Name="FieldText" Value="#{uxComments}.value" Mode="Raw" />
                                                            </ExtraParams>
                                                            <EventMask ShowMask="true" Msg="Updating..." />
                                                        </Blur>
                                                    </DirectEvents>
                                                </ext:TextArea>
                                            </Items>
                                        </ext:FieldContainer>
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Panel>

                        <%--------------------------- Material ---------------------------%>
                        <ext:Panel runat="server"
                            Title="Material"
                            ID="uxMaterial"
                            Disabled="false"
                            BodyPadding="10"
                            Layout="FitLayout">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer1"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:GridPanel ID="uxMaterialGridPanel" runat="server" Height="160" Width="660" Margin="5" Layout="FitLayout">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxMaterialGridStore" OnReadData="deReadGridData"
                                                    AutoDataBind="true" WarningOnDirty="false">
                                                    <Model>
                                                        <ext:Model ID="Model1" Name="Material" IDProperty="DETAIL_SHEET_ID" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DETAIL_SHEET_ID" />
                                                                <ext:ModelField Name="REC_TYPE" />
                                                                <ext:ModelField Name="AMT_3" />
                                                                <ext:ModelField Name="DESC_1" />
                                                                <ext:ModelField Name="AMT_1" />
                                                                <ext:ModelField Name="DESC_2" />
                                                                <ext:ModelField Name="AMT_2" />
                                                                <ext:ModelField Name="TOTAL" />
                                                                <ext:ModelField Name="OVERRIDDEN" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                    <Proxy>
                                                        <ext:PageProxy />
                                                    </Proxy>
                                                    <Parameters>
                                                        <ext:StoreParameter Name="RecordType" Value="MATERIAL" Mode="Value" />
                                                    </Parameters>
                                                </ext:Store>
                                            </Store>
                                            <ColumnModel>
                                                <Columns>
                                                    <ext:NumberColumn ID="NumberColumn26" runat="server" DataIndex="AMT_3" Text="Qty" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="NumberField1" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" DecimalPrecision="4" />
                                                        </Editor>
                                                        <Renderer Fn="colorSubGridOverrideMaterial" />
                                                    </ext:NumberColumn>
                                                    <ext:Column ID="Column4" runat="server" DataIndex="DESC_1" Text="Material" Flex="2">
                                                        <Editor>
                                                            <ext:DropDownField ID="uxMaterialPicker" runat="server" Width="110" Mode="Text" Editable="true" MaxLength="200" EnforceMaxLength="true" SelectOnFocus="true">
                                                                <Listeners>
                                                                    <Expand Handler="this.picker.setWidth(500);" />
                                                                </Listeners>
                                                                <Component>
                                                                    <ext:GridPanel runat="server"
                                                                        ID="uxMaterialList"
                                                                        Width="500"
                                                                        Layout="HBoxLayout"
                                                                        Frame="true"
                                                                        ForceFit="true">
                                                                        <Store>
                                                                            <ext:Store runat="server"
                                                                                ID="uxMaterialStore"
                                                                                PageSize="10"
                                                                                RemoteSort="true"
                                                                                OnReadData="deLoadMaterialDropdown">
                                                                                <Model>
                                                                                    <ext:Model ID="Model9" runat="server">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="DESCRIPTION" />
                                                                                            <ext:ModelField Name="UOM_CODE" />
                                                                                            <ext:ModelField Name="ITEM_COST" />
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
                                                                                <ext:Column ID="Column20" runat="server" Text="Material" DataIndex="DESCRIPTION" Flex="4" />
                                                                                <ext:Column ID="Column21" runat="server" Text="Unit Cost" DataIndex="ITEM_COST" Flex="1" />
                                                                                <ext:Column ID="Column22" runat="server" Text="UOM" DataIndex="UOM_CODE" Flex="1" />
                                                                            </Columns>
                                                                        </ColumnModel>
                                                                        <BottomBar>
                                                                            <ext:PagingToolbar ID="PagingToolbar3" runat="server" />
                                                                        </BottomBar>
                                                                        <SelectionModel>
                                                                            <ext:RowSelectionModel ID="RowSelectionModel9" runat="server" Mode="Single" />
                                                                        </SelectionModel>
                                                                        <DirectEvents>
                                                                            <SelectionChange OnEvent="deSelectMaterial">
                                                                                <ExtraParams>
                                                                                    <ext:Parameter Name="Material" Value="#{uxMaterialList}.getSelectionModel().getSelection()[0].data.DESCRIPTION" Mode="Raw" />
                                                                                    <ext:Parameter Name="UnitCost" Value="#{uxMaterialList}.getSelectionModel().getSelection()[0].data.ITEM_COST" Mode="Raw" />
                                                                                    <ext:Parameter Name="UOM" Value="#{uxMaterialList}.getSelectionModel().getSelection()[0].data.UOM_CODE" Mode="Raw" />
                                                                                </ExtraParams>
                                                                            </SelectionChange>
                                                                        </DirectEvents>
                                                                        <Plugins>
                                                                            <ext:FilterHeader runat="server" ID="uxMaterialFilter" Remote="true" />
                                                                        </Plugins>
                                                                    </ext:GridPanel>
                                                                </Component>
                                                            </ext:DropDownField>
                                                        </Editor>
                                                        <Renderer Fn="colorSubGridOverride" />
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="NumberField4" runat="server" DataIndex="AMT_1" Text="Unit Cost" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxMaterialUnitCost" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" DecimalPrecision="4" />
                                                        </Editor>
                                                        <Renderer Fn="colorSubGridOverrideMaterial" />
                                                    </ext:NumberColumn>
                                                    <ext:Column ID="NumberField5" runat="server" DataIndex="DESC_2" Text="UOM" Flex="1">
                                                        <Editor>
                                                            <ext:TextField ID="uxMaterialUOM" runat="server" SelectOnFocus="true" MaxLength="200" EnforceMaxLength="true" />
                                                        </Editor>
                                                        <Renderer Fn="colorSubGridOverride" />
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="Column3" runat="server" DataIndex="AMT_2" Text="Qty" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxMaterialQuantity" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" DecimalPrecision="4" />
                                                        </Editor>
                                                        <Renderer Fn="colorSubGridOverrideMaterial" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="Column5" runat="server" DataIndex="TOTAL" Text="Total" Flex="1" Align="Right">
                                                        <Renderer Fn="colorSubGridOverride" />
                                                    </ext:NumberColumn>
                                                </Columns>
                                            </ColumnModel>
                                            <DirectEvents>
                                                <Select OnEvent="deGetMatRecID">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="SelRecordID" Value="#{uxMaterialGridPanel}.getSelectionModel().getSelection()[0].data.DETAIL_SHEET_ID" Mode="Raw" />
                                                    </ExtraParams>
                                                </Select>
                                            </DirectEvents>
                                            <Plugins>
                                                <ext:CellEditing ID="CellEditing2" runat="server">
                                                    <Listeners>
                                                        <Edit Fn="editRecord" />
                                                    </Listeners>
                                                </ext:CellEditing>
                                            </Plugins>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" />
                                            </SelectionModel>
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar2" runat="server" Region="North">
                                                    <Items>
                                                        <ext:Button ID="uxAddNewMaterial" runat="server" Text="Add New" Icon="Add">
                                                            <DirectEvents>
                                                                <Click OnEvent="deAddNewRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordType" Value="MATERIAL" Mode="Value" />
                                                                    </ExtraParams>
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:Button ID="uxAddNewBOM" runat="server" Text="Add BOM Items" Icon="PackageAdd" Visible="false">
                                                            <DirectEvents>
                                                                <Click OnEvent="deAddNewBOM">
                                                                    <EventMask ShowMask="true" Msg="Processing..." />
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:Button ID="uxDeleteMaterial" runat="server" Text="Delete Selected" Icon="Delete">
                                                            <DirectEvents>
                                                                <Click OnEvent="deDeleteRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordID" Value="#{uxMaterialGridPanel}.getSelectionModel().getSelection()[0].data.DETAIL_SHEET_ID" Mode="Raw" />
                                                                    </ExtraParams>
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <DockedItems>
                                                <ext:FieldContainer ID="uxTotalMaterialBar" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="grandTotalBackground">
                                                    <Items>
                                                        <ext:DisplayField ID="DisplayField8" runat="server" Width="10" />
                                                        <ext:DisplayField ID="DisplayField7" runat="server" Text="Total Material:" Flex="2" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField6" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField1" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField2" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="uxTotalMaterial" runat="server" Text="0.0000" Flex="1" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField26" runat="server" Width="10" />
                                                    </Items>
                                                </ext:FieldContainer>
                                            </DockedItems>
                                        </ext:GridPanel>
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Panel>

                        <%--------------------------- Equipment ---------------------------%>
                        <ext:Panel runat="server"
                            Title="Equipment"
                            ID="uxEquipment"
                            Disabled="false"
                            BodyPadding="10"
                            Layout="FitLayout">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer2"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:GridPanel ID="uxEquipmentGridPanel" runat="server" Height="160" Width="660" Margin="5" Layout="FitLayout">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxEquipmentGridStore" OnReadData="deReadGridData"
                                                    AutoDataBind="true" WarningOnDirty="false">
                                                    <Model>
                                                        <ext:Model ID="Model2" Name="Equipment" IDProperty="DETAIL_SHEET_ID" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DETAIL_SHEET_ID" />
                                                                <ext:ModelField Name="REC_TYPE" />
                                                                <ext:ModelField Name="AMT_1" />
                                                                <ext:ModelField Name="DESC_1" />
                                                                <ext:ModelField Name="AMT_2" />
                                                                <ext:ModelField Name="AMT_3" />
                                                                <ext:ModelField Name="TOTAL" />
                                                                <ext:ModelField Name="OVERRIDDEN" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                    <Proxy>
                                                        <ext:PageProxy />
                                                    </Proxy>
                                                    <Parameters>
                                                        <ext:StoreParameter Name="RecordType" Value="EQUIPMENT" Mode="Value" />
                                                    </Parameters>
                                                </ext:Store>
                                            </Store>
                                            <ColumnModel>
                                                <Columns>
                                                    <ext:NumberColumn ID="NumberColumn1" runat="server" DataIndex="AMT_1" Text="Qty" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxEquipmentQuantity" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                                        </Editor>
                                                        <Renderer Fn="colorSubGridOverride" />
                                                    </ext:NumberColumn>
                                                    <ext:Column ID="Column1" runat="server" DataIndex="DESC_1" Text="Equipment/Equipment Travel" Flex="2">
                                                        <Editor>
                                                            <ext:DropDownField ID="uxEquipmentPicker" runat="server" Width="110" Mode="Text" Editable="true" MaxLength="200" EnforceMaxLength="true" SelectOnFocus="true">
                                                                <Listeners>
                                                                    <Expand Handler="this.picker.setWidth(500);" />
                                                                </Listeners>
                                                                <Component>
                                                                    <ext:GridPanel runat="server"
                                                                        ID="uxEquipmentList"
                                                                        Width="500"
                                                                        Layout="HBoxLayout"
                                                                        Frame="true"
                                                                        ForceFit="true">
                                                                        <Store>
                                                                            <ext:Store runat="server"
                                                                                ID="uxEquipmentStore"
                                                                                PageSize="10"
                                                                                RemoteSort="true"
                                                                                OnReadData="deLoadEquipmentDropdown">
                                                                                <Model>
                                                                                    <ext:Model ID="Model10" runat="server">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="EXPENDITURE_TYPE" />
                                                                                            <ext:ModelField Name="COST_RATE" />
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
                                                                                <ext:Column ID="Column2" runat="server" Text="Equipment" DataIndex="EXPENDITURE_TYPE" Flex="4" />
                                                                                <ext:Column ID="Column6" runat="server" Text="Cost per Hour" DataIndex="COST_RATE" Flex="1" />
                                                                            </Columns>
                                                                        </ColumnModel>
                                                                        <BottomBar>
                                                                            <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
                                                                        </BottomBar>
                                                                        <SelectionModel>
                                                                            <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" Mode="Single" />
                                                                        </SelectionModel>
                                                                        <DirectEvents>
                                                                            <SelectionChange OnEvent="deSelectEquipment">
                                                                                <ExtraParams>
                                                                                    <ext:Parameter Name="Equipment" Value="#{uxEquipmentList}.getSelectionModel().getSelection()[0].data.EXPENDITURE_TYPE" Mode="Raw" />
                                                                                    <ext:Parameter Name="CostPerHour" Value="#{uxEquipmentList}.getSelectionModel().getSelection()[0].data.COST_RATE" Mode="Raw" />
                                                                                </ExtraParams>
                                                                            </SelectionChange>
                                                                        </DirectEvents>
                                                                        <Plugins>
                                                                            <ext:FilterHeader runat="server" ID="uxEquipmentFilter" Remote="true" />
                                                                        </Plugins>
                                                                    </ext:GridPanel>
                                                                </Component>
                                                            </ext:DropDownField>
                                                        </Editor>
                                                        <Renderer Fn="colorSubGridOverride" />
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="NumberColumn2" runat="server" DataIndex="AMT_2" Text="Hours" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxEquipmentHours" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                                        </Editor>
                                                        <Renderer Fn="colorSubGridOverride" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn22" runat="server" DataIndex="AMT_3" Text="Cost per Hour" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxEquipmentCostPerHour" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                                        </Editor>
                                                        <Renderer Fn="colorSubGridOverride" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn3" runat="server" DataIndex="TOTAL" Text="Total" Flex="1" Align="Right">
                                                        <Renderer Fn="colorSubGridOverride" />
                                                    </ext:NumberColumn>
                                                </Columns>
                                            </ColumnModel>
                                            <DirectEvents>
                                                <Select OnEvent="deGetEquipRecID">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="SelRecordID" Value="#{uxEquipmentGridPanel}.getSelectionModel().getSelection()[0].data.DETAIL_SHEET_ID" Mode="Raw" />
                                                    </ExtraParams>
                                                </Select>
                                            </DirectEvents>
                                            <Plugins>
                                                <ext:CellEditing ID="CellEditing1" runat="server">
                                                    <Listeners>
                                                        <Edit Fn="editRecord" />
                                                    </Listeners>
                                                </ext:CellEditing>
                                            </Plugins>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel10" runat="server" />
                                            </SelectionModel>
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar1" runat="server" Region="North">
                                                    <Items>
                                                        <ext:Button ID="uxAddNewEquipment" runat="server" Text="Add New" Icon="Add">
                                                            <DirectEvents>
                                                                <Click OnEvent="deAddNewRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordType" Value="EQUIPMENT" Mode="Value" />
                                                                    </ExtraParams>
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:Button ID="uxDeleteEquipment" runat="server" Text="Delete Selected" Icon="Delete">
                                                            <DirectEvents>
                                                                <Click OnEvent="deDeleteRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordID" Value="#{uxEquipmentGridPanel}.getSelectionModel().getSelection()[0].data.DETAIL_SHEET_ID" Mode="Raw" />
                                                                    </ExtraParams>
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <DockedItems>
                                                <ext:FieldContainer ID="FieldContainer15" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="grandTotalBackground">
                                                    <Items>
                                                        <ext:DisplayField ID="DisplayField3" runat="server" Width="10" />
                                                        <ext:DisplayField ID="DisplayField4" runat="server" Text="Total Equipment:" Flex="2" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField5" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField9" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField10" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="uxTotalEquipment" runat="server" Text="0.00" Flex="1" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField32" runat="server" Width="10" />
                                                    </Items>
                                                </ext:FieldContainer>
                                            </DockedItems>
                                        </ext:GridPanel>
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Panel>

                        <%--------------------------- Personnel ---------------------------%>
                        <ext:Panel runat="server"
                            Title="Personnel"
                            ID="uxPersonnel"
                            Disabled="false"
                            BodyPadding="10"
                            Layout="FitLayout">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer18"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:GridPanel ID="uxPersonnelGridPanel" runat="server" Height="160" Width="660" Margin="5" Layout="FitLayout">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxPersonnelGridStore" OnReadData="deReadGridData"
                                                    AutoDataBind="true" WarningOnDirty="false">
                                                    <Model>
                                                        <ext:Model ID="Model3" Name="Personnel" IDProperty="DETAIL_SHEET_ID" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DETAIL_SHEET_ID" />
                                                                <ext:ModelField Name="REC_TYPE" />
                                                                <ext:ModelField Name="AMT_1" />
                                                                <ext:ModelField Name="DESC_1" />
                                                                <ext:ModelField Name="AMT_2" />
                                                                <ext:ModelField Name="AMT_3" />
                                                                <ext:ModelField Name="TOTAL" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                    <Proxy>
                                                        <ext:PageProxy />
                                                    </Proxy>
                                                    <Parameters>
                                                        <ext:StoreParameter Name="RecordType" Value="PERSONNEL" Mode="Value" />
                                                    </Parameters>
                                                </ext:Store>
                                            </Store>
                                            <ColumnModel>
                                                <Columns>
                                                    <ext:NumberColumn ID="NumberColumn4" runat="server" DataIndex="AMT_1" Text="Qty" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxPersonnelQuantity" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                                        </Editor>
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                    <ext:Column ID="Column7" runat="server" DataIndex="DESC_1" Text="Position" Flex="2">
                                                        <Editor>
                                                            <ext:DropDownField ID="uxPersonnelPicker" runat="server" Width="110" Mode="Text" Editable="true" MaxLength="200" EnforceMaxLength="true" SelectOnFocus="true">
                                                                <Listeners>
                                                                    <Expand Handler="this.picker.setWidth(500);" />
                                                                </Listeners>
                                                                <Component>
                                                                    <ext:GridPanel runat="server"
                                                                        ID="uxPersonnelList"
                                                                        Width="500"
                                                                        Layout="HBoxLayout"
                                                                        Frame="true"
                                                                        ForceFit="true">
                                                                        <Store>
                                                                            <ext:Store runat="server"
                                                                                ID="uxPersonnelStore"
                                                                                PageSize="10"
                                                                                RemoteSort="true"
                                                                                OnReadData="deLoadPersonnelDropdown">
                                                                                <Model>
                                                                                    <ext:Model ID="Model11" runat="server">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="POSITION" />
                                                                                            <ext:ModelField Name="COST_PER_HR" />
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
                                                                                <ext:Column ID="Column8" runat="server" Text="Position" DataIndex="POSITION" Flex="4" />
                                                                                <ext:Column ID="Column9" runat="server" Text="Cost per Hour" DataIndex="COST_PER_HR" Flex="1" />
                                                                            </Columns>
                                                                        </ColumnModel>
                                                                        <BottomBar>
                                                                            <ext:PagingToolbar ID="PagingToolbar2" runat="server" />
                                                                        </BottomBar>
                                                                        <SelectionModel>
                                                                            <ext:RowSelectionModel ID="RowSelectionModel3" runat="server" Mode="Single" />
                                                                        </SelectionModel>
                                                                        <DirectEvents>
                                                                            <SelectionChange OnEvent="deSelectPosition">
                                                                                <ExtraParams>
                                                                                    <ext:Parameter Name="Position" Value="#{uxPersonnelList}.getSelectionModel().getSelection()[0].data.POSITION" Mode="Raw" />
                                                                                    <ext:Parameter Name="CostPerHour" Value="#{uxPersonnelList}.getSelectionModel().getSelection()[0].data.COST_PER_HR" Mode="Raw" />
                                                                                </ExtraParams>
                                                                            </SelectionChange>
                                                                        </DirectEvents>
                                                                        <Plugins>
                                                                            <ext:FilterHeader runat="server" ID="uxPersonnelFilter" Remote="true" />
                                                                        </Plugins>
                                                                    </ext:GridPanel>
                                                                </Component>
                                                            </ext:DropDownField>
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="NumberColumn5" runat="server" DataIndex="AMT_2" Text="Hours" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxPersonnelHours" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                                        </Editor>
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn6" runat="server" DataIndex="AMT_3" Text="Cost per Hour" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxPersonnelCostPerHour" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                                        </Editor>
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn23" runat="server" DataIndex="TOTAL" Text="Total" Flex="1" Align="Right">
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                </Columns>
                                            </ColumnModel>
                                            <DirectEvents>
                                                <Select OnEvent="deGetPersRecID">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="SelRecordID" Value="#{uxPersonnelGridPanel}.getSelectionModel().getSelection()[0].data.DETAIL_SHEET_ID" Mode="Raw" />
                                                    </ExtraParams>
                                                </Select>
                                            </DirectEvents>
                                            <Plugins>
                                                <ext:CellEditing ID="CellEditing3" runat="server">
                                                    <Listeners>
                                                        <Edit Fn="editRecord" />
                                                    </Listeners>
                                                </ext:CellEditing>
                                            </Plugins>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel11" runat="server" />
                                            </SelectionModel>
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar3" runat="server" Region="North">
                                                    <Items>
                                                        <ext:Button ID="uxAddNewPersonnel" runat="server" Text="Add New" Icon="Add">
                                                            <DirectEvents>
                                                                <Click OnEvent="deAddNewRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordType" Value="PERSONNEL" Mode="Value" />
                                                                    </ExtraParams>
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:Button ID="uxDeletePersonnel" runat="server" Text="Delete Selected" Icon="Delete">
                                                            <DirectEvents>
                                                                <Click OnEvent="deDeleteRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordID" Value="#{uxPersonnelGridPanel}.getSelectionModel().getSelection()[0].data.DETAIL_SHEET_ID" Mode="Raw" />
                                                                    </ExtraParams>
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <DockedItems>
                                                <ext:FieldContainer ID="FieldContainer19" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="grandTotalBackground">
                                                    <Items>
                                                        <ext:DisplayField ID="DisplayField11" runat="server" Width="10" />
                                                        <ext:DisplayField ID="DisplayField12" runat="server" Text="Total Personnel:" Flex="2" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField13" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField14" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField15" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="uxTotalPersonnel" runat="server" Text="0.00" Flex="1" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField38" runat="server" Width="10" />
                                                    </Items>
                                                </ext:FieldContainer>
                                            </DockedItems>
                                        </ext:GridPanel>
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Panel>

                        <%--------------------------- Per Diem ---------------------------%>
                        <ext:Panel runat="server"
                            Title="Per Diem"
                            ID="uxPerDiem"
                            Disabled="false"
                            BodyPadding="10"
                            Layout="FitLayout">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer20"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:GridPanel ID="uxPerDiemGridPanel" runat="server" Height="160" Width="660" Margin="5" Layout="FitLayout">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxPerDiemGridStore" OnReadData="deReadGridData"
                                                    AutoDataBind="true" WarningOnDirty="false">
                                                    <Model>
                                                        <ext:Model ID="Model4" Name="PerDiem" IDProperty="DETAIL_SHEET_ID" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DETAIL_SHEET_ID" />
                                                                <ext:ModelField Name="REC_TYPE" />
                                                                <ext:ModelField Name="AMT_1" />
                                                                <ext:ModelField Name="AMT_2" />
                                                                <ext:ModelField Name="AMT_3" />
                                                                <ext:ModelField Name="TOTAL" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                    <Proxy>
                                                        <ext:PageProxy />
                                                    </Proxy>
                                                    <Parameters>
                                                        <ext:StoreParameter Name="RecordType" Value="PERDIEM" Mode="Value" />
                                                    </Parameters>
                                                </ext:Store>
                                            </Store>
                                            <ColumnModel>
                                                <Columns>
                                                    <ext:NumberColumn ID="NumberColumn7" runat="server" DataIndex="AMT_1" Text="Rate" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxPerDiemRate" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                                        </Editor>
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn8" runat="server" DataIndex="AMT_2" Text="# of Days" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxPerDiemNumOfDays" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                                        </Editor>
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn9" runat="server" DataIndex="AMT_3" Text="# of Per Diems" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxPerDiemNumOfPerDiems" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                                        </Editor>
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn24" runat="server" DataIndex="TOTAL" Text="Total" Flex="1" Align="Right">
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                </Columns>
                                            </ColumnModel>
                                            <Plugins>
                                                <ext:CellEditing ID="CellEditing4" runat="server">
                                                    <Listeners>
                                                        <Edit Fn="editRecord" />
                                                    </Listeners>
                                                </ext:CellEditing>
                                            </Plugins>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel12" runat="server" />
                                            </SelectionModel>
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar4" runat="server" Region="North">
                                                    <Items>
                                                        <ext:Button ID="uxAddNewPerDiem" runat="server" Text="Add New" Icon="Add">
                                                            <DirectEvents>
                                                                <Click OnEvent="deAddNewRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordType" Value="PERDIEM" Mode="Value" />
                                                                    </ExtraParams>
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:Button ID="uxDeletePerDiem" runat="server" Text="Delete Selected" Icon="Delete">
                                                            <DirectEvents>
                                                                <Click OnEvent="deDeleteRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordID" Value="#{uxPerDiemGridPanel}.getSelectionModel().getSelection()[0].data.DETAIL_SHEET_ID" Mode="Raw" />
                                                                    </ExtraParams>
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <DockedItems>
                                                <ext:FieldContainer ID="FieldContainer21" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="grandTotalBackground">
                                                    <Items>
                                                        <ext:DisplayField ID="DisplayField16" runat="server" Width="10" />
                                                        <ext:DisplayField ID="DisplayField17" runat="server" Text="Total Per Diems:" Flex="2" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField18" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField19" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField20" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="uxTotalPerDiem" runat="server" Text="0.00" Flex="1" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField45" runat="server" Width="10" />
                                                    </Items>
                                                </ext:FieldContainer>
                                            </DockedItems>
                                        </ext:GridPanel>
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Panel>

                        <%--------------------------- Travel ---------------------------%>
                        <ext:Panel runat="server"
                            Title="Travel"
                            ID="uxTravel"
                            Disabled="false"
                            BodyPadding="10"
                            Layout="FitLayout">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer22"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:GridPanel ID="uxTravelGridPanel" runat="server" Height="160" Width="660" Margin="5" Layout="FitLayout">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxTravelGridStore" OnReadData="deReadGridData"
                                                    AutoDataBind="true" WarningOnDirty="false">
                                                    <Model>
                                                        <ext:Model ID="Model5" Name="Travel" IDProperty="DETAIL_SHEET_ID" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DETAIL_SHEET_ID" />
                                                                <ext:ModelField Name="REC_TYPE" />
                                                                <ext:ModelField Name="AMT_1" />
                                                                <ext:ModelField Name="AMT_2" />
                                                                <ext:ModelField Name="TOTAL" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                    <Proxy>
                                                        <ext:PageProxy />
                                                    </Proxy>
                                                    <Parameters>
                                                        <ext:StoreParameter Name="RecordType" Value="TRAVEL" Mode="Value" />
                                                    </Parameters>
                                                </ext:Store>
                                            </Store>
                                            <ColumnModel>
                                                <Columns>
                                                    <ext:NumberColumn ID="NumberColumn10" runat="server" DataIndex="AMT_1" Text="Travel Pay" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxTravelPay" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                                        </Editor>
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn11" runat="server" DataIndex="AMT_2" Text="Hours" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxTravelHours" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                                        </Editor>
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn25" runat="server" DataIndex="TOTAL" Text="Total" Flex="1" Align="Right">
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                </Columns>
                                            </ColumnModel>
                                            <Plugins>
                                                <ext:CellEditing ID="CellEditing5" runat="server">
                                                    <Listeners>
                                                        <Edit Fn="editRecord" />
                                                    </Listeners>
                                                </ext:CellEditing>
                                            </Plugins>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel4" runat="server" />
                                            </SelectionModel>
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar5" runat="server" Region="North">
                                                    <Items>
                                                        <ext:Button ID="uxAddNewTravel" runat="server" Text="Add New" Icon="Add">
                                                            <DirectEvents>
                                                                <Click OnEvent="deAddNewRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordType" Value="TRAVEL" Mode="Value" />
                                                                    </ExtraParams>
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:Button ID="uxDeleteTravel" runat="server" Text="Delete Selected" Icon="Delete">
                                                            <DirectEvents>
                                                                <Click OnEvent="deDeleteRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordID" Value="#{uxTravelGridPanel}.getSelectionModel().getSelection()[0].data.DETAIL_SHEET_ID" Mode="Raw" />
                                                                    </ExtraParams>
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <DockedItems>
                                                <ext:FieldContainer ID="FieldContainer23" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="grandTotalBackground">
                                                    <Items>
                                                        <ext:DisplayField ID="DisplayField21" runat="server" Width="10" />
                                                        <ext:DisplayField ID="DisplayField22" runat="server" Text="Total Travel:" Flex="2" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField23" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField24" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField25" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="uxTotalTravel" runat="server" Text="0.00" Flex="1" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField46" runat="server" Width="10" />
                                                    </Items>
                                                </ext:FieldContainer>
                                            </DockedItems>
                                        </ext:GridPanel>
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Panel>

                        <%--------------------------- Motels ---------------------------%>
                        <ext:Panel runat="server"
                            Title="Motels"
                            ID="uxMotels"
                            Disabled="false"
                            BodyPadding="10"
                            Layout="FitLayout">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer24"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:GridPanel ID="uxMotelsGridPanel" runat="server" Height="160" Width="660" Margin="5" Layout="FitLayout">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxMotelsGridStore" OnReadData="deReadGridData"
                                                    AutoDataBind="true" WarningOnDirty="false">
                                                    <Model>
                                                        <ext:Model ID="Model6" Name="Motels" IDProperty="DETAIL_SHEET_ID" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DETAIL_SHEET_ID" />
                                                                <ext:ModelField Name="REC_TYPE" />
                                                                <ext:ModelField Name="AMT_1" />
                                                                <ext:ModelField Name="AMT_2" />
                                                                <ext:ModelField Name="AMT_3" />
                                                                <ext:ModelField Name="TOTAL" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                    <Proxy>
                                                        <ext:PageProxy />
                                                    </Proxy>
                                                    <Parameters>
                                                        <ext:StoreParameter Name="RecordType" Value="MOTELS" Mode="Value" />
                                                    </Parameters>
                                                </ext:Store>
                                            </Store>
                                            <ColumnModel>
                                                <Columns>
                                                    <ext:NumberColumn ID="NumberColumn12" runat="server" DataIndex="AMT_1" Text="Rate" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxMotelRate" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                                        </Editor>
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn13" runat="server" DataIndex="AMT_2" Text="# of Days" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxMotelNumOfDays" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                                        </Editor>
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn14" runat="server" DataIndex="AMT_3" Text="# of Rooms" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxMotelNumOfRooms" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                                        </Editor>
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn15" runat="server" DataIndex="TOTAL" Text="Total" Flex="1" Align="Right">
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                </Columns>
                                            </ColumnModel>
                                            <Plugins>
                                                <ext:CellEditing ID="CellEditing6" runat="server">
                                                    <Listeners>
                                                        <Edit Fn="editRecord" />
                                                    </Listeners>
                                                </ext:CellEditing>
                                            </Plugins>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel5" runat="server" />
                                            </SelectionModel>
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar6" runat="server" Region="North">
                                                    <Items>
                                                        <ext:Button ID="uxAddNewMotel" runat="server" Text="Add New" Icon="Add">
                                                            <DirectEvents>
                                                                <Click OnEvent="deAddNewRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordType" Value="MOTELS" Mode="Value" />
                                                                    </ExtraParams>
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:Button ID="uxDeleteMotel" runat="server" Text="Delete Selected" Icon="Delete">
                                                            <DirectEvents>
                                                                <Click OnEvent="deDeleteRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordID" Value="#{uxMotelsGridPanel}.getSelectionModel().getSelection()[0].data.DETAIL_SHEET_ID" Mode="Raw" />
                                                                    </ExtraParams>
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <DockedItems>
                                                <ext:FieldContainer ID="FieldContainer25" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="grandTotalBackground">
                                                    <Items>
                                                        <ext:DisplayField ID="DisplayField27" runat="server" Width="10" />
                                                        <ext:DisplayField ID="DisplayField28" runat="server" Text="Total Motels:" Flex="2" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField29" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField30" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField31" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="uxTotalMotels" runat="server" Text="0.00" Flex="1" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField47" runat="server" Width="10" />
                                                    </Items>
                                                </ext:FieldContainer>
                                            </DockedItems>
                                        </ext:GridPanel>
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Panel>

                        <%--------------------------- Misc ---------------------------%>
                        <ext:Panel runat="server"
                            Title="Misc."
                            ID="uxMisc"
                            Disabled="false"
                            BodyPadding="10"
                            Layout="FitLayout">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer26"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:GridPanel ID="uxMiscGridPanel" runat="server" Height="160" Width="660" Margin="5" Layout="FitLayout">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxMiscGridStore" OnReadData="deReadGridData"
                                                    AutoDataBind="true" WarningOnDirty="false">
                                                    <Model>
                                                        <ext:Model ID="Model7" Name="Travel" IDProperty="DETAIL_SHEET_ID" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DETAIL_SHEET_ID" />
                                                                <ext:ModelField Name="REC_TYPE" />
                                                                <ext:ModelField Name="DESC_1" />
                                                                <ext:ModelField Name="AMT_1" />
                                                                <ext:ModelField Name="AMT_2" />
                                                                <ext:ModelField Name="TOTAL" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                    <Proxy>
                                                        <ext:PageProxy />
                                                    </Proxy>
                                                    <Parameters>
                                                        <ext:StoreParameter Name="RecordType" Value="MISC" Mode="Value" />
                                                    </Parameters>
                                                </ext:Store>
                                            </Store>
                                            <ColumnModel>
                                                <Columns>
                                                    <ext:Column ID="Column16" runat="server" DataIndex="DESC_1" Text="Description" Flex="2">
                                                        <Editor>
                                                            <ext:TextField ID="uxMiscDesc" runat="server" MaxLength="200" EnforceMaxLength="true" SelectOnFocus="true" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="NumberColumn16" runat="server" DataIndex="AMT_1" Text="Qty" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxMiscQuantity" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                                        </Editor>
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn17" runat="server" DataIndex="AMT_2" Text="Cost" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxMiscCost" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                                        </Editor>
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn18" runat="server" DataIndex="TOTAL" Text="Total" Flex="1" Align="Right">
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                </Columns>
                                            </ColumnModel>
                                            <Plugins>
                                                <ext:CellEditing ID="CellEditing7" runat="server">
                                                    <Listeners>
                                                        <Edit Fn="editRecord" />
                                                    </Listeners>
                                                </ext:CellEditing>
                                            </Plugins>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel6" runat="server" />
                                            </SelectionModel>
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar7" runat="server" Region="North">
                                                    <Items>
                                                        <ext:Button ID="uxAddNewMisc" runat="server" Text="Add New" Icon="Add">
                                                            <DirectEvents>
                                                                <Click OnEvent="deAddNewRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordType" Value="MISC" Mode="Value" />
                                                                    </ExtraParams>
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:Button ID="uxDeleteMisc" runat="server" Text="Delete Selected" Icon="Delete">
                                                            <DirectEvents>
                                                                <Click OnEvent="deDeleteRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordID" Value="#{uxMiscGridPanel}.getSelectionModel().getSelection()[0].data.DETAIL_SHEET_ID" Mode="Raw" />
                                                                    </ExtraParams>
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <DockedItems>
                                                <ext:FieldContainer ID="FieldContainer27" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="grandTotalBackground">
                                                    <Items>
                                                        <ext:DisplayField ID="DisplayField33" runat="server" Width="10" />
                                                        <ext:DisplayField ID="DisplayField34" runat="server" Text="Total Misc:" Flex="2" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField35" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField36" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField37" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="uxTotalMisc" runat="server" Text="0.00" Flex="1" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField48" runat="server" Width="10" />
                                                    </Items>
                                                </ext:FieldContainer>
                                            </DockedItems>
                                        </ext:GridPanel>
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Panel>

                        <%--------------------------- Lump Sum ---------------------------%>
                        <ext:Panel runat="server"
                            Title="Lump Sum"
                            ID="uxLumpSum"
                            Disabled="false"
                            BodyPadding="10"
                            Layout="FitLayout">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer28"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:GridPanel ID="uxLumpSumGridPanel" runat="server" Height="160" Width="660" Margin="5" Layout="FitLayout">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxLumpSumGridStore" OnReadData="deReadGridData"
                                                    AutoDataBind="true" WarningOnDirty="false">
                                                    <Model>
                                                        <ext:Model ID="Model8" Name="Travel" IDProperty="DETAIL_SHEET_ID" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DETAIL_SHEET_ID" />
                                                                <ext:ModelField Name="REC_TYPE" />
                                                                <ext:ModelField Name="DESC_1" />
                                                                <ext:ModelField Name="AMT_1" />
                                                                <ext:ModelField Name="AMT_2" />
                                                                <ext:ModelField Name="TOTAL" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                    <Proxy>
                                                        <ext:PageProxy />
                                                    </Proxy>
                                                    <Parameters>
                                                        <ext:StoreParameter Name="RecordType" Value="LUMPSUM" Mode="Value" />
                                                    </Parameters>
                                                </ext:Store>
                                            </Store>
                                            <ColumnModel>
                                                <Columns>
                                                    <ext:Column ID="Column10" runat="server" DataIndex="DESC_1" Text="Description" Flex="2">
                                                        <Editor>
                                                            <ext:TextField ID="uxLumpSumDesc" runat="server" MaxLength="200" EnforceMaxLength="true" SelectOnFocus="true" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="NumberColumn19" runat="server" DataIndex="AMT_1" Text="Qty" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxLumpSumQuantity" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                                        </Editor>
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn20" runat="server" DataIndex="AMT_2" Text="Cost" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:NumberField ID="uxLumpSumCost" runat="server" SelectOnFocus="true" MinValue="-9999999999.99" MaxValue="9999999999.99" HideTrigger="true" />
                                                        </Editor>
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn21" runat="server" DataIndex="TOTAL" Text="Total" Flex="1" Align="Right">
                                                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                                                    </ext:NumberColumn>
                                                </Columns>
                                            </ColumnModel>
                                            <Plugins>
                                                <ext:CellEditing ID="CellEditing8" runat="server">
                                                    <Listeners>
                                                        <Edit Fn="editRecord" />
                                                    </Listeners>
                                                </ext:CellEditing>
                                            </Plugins>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel7" runat="server" />
                                            </SelectionModel>
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar8" runat="server" Region="North">
                                                    <Items>
                                                        <ext:Button ID="uxAddNewLumpSum" runat="server" Text="Add New" Icon="Add">
                                                            <DirectEvents>
                                                                <Click OnEvent="deAddNewRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordType" Value="LUMPSUM" Mode="Value" />
                                                                    </ExtraParams>
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:Button ID="uxDeleteLumpSum" runat="server" Text="Delete Selected" Icon="Delete">
                                                            <DirectEvents>
                                                                <Click OnEvent="deDeleteRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordID" Value="#{uxLumpSumGridPanel}.getSelectionModel().getSelection()[0].data.DETAIL_SHEET_ID" Mode="Raw" />
                                                                    </ExtraParams>
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <DockedItems>
                                                <ext:FieldContainer ID="FieldContainer29" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="grandTotalBackground">
                                                    <Items>
                                                        <ext:DisplayField ID="DisplayField39" runat="server" Width="10" />
                                                        <ext:DisplayField ID="DisplayField40" runat="server" Text="Total Misc:" Flex="2" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField41" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField42" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField43" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="uxTotalLumpSum" runat="server" Text="0.00" Flex="1" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField49" runat="server" Width="10" />
                                                    </Items>
                                                </ext:FieldContainer>
                                            </DockedItems>
                                        </ext:GridPanel>
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Panel>

                    </Items>
                </ext:TabPanel>


                <%-------------------------------------------------- Bottom --------------------------------------------------%>
                <ext:FormPanel ID="FormPanel1"
                    runat="server"
                    Region="South"
                    BodyPadding="10"
                    Disabled="false">
                    <Items>

                        <ext:FieldContainer ID="FieldContainer10"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="uxLaborBurdenLabel" runat="server" Width="180" Text="Labor Burden @ XX.XX%:" />
                                <ext:TextField ID="uxLaborBurden" runat="server" Width="100" ReadOnly="true" Text="0.00" Cls="textRightAlign" TabIndex="-6" />
                                <ext:Label ID="Label22" runat="server" Width="105" />
                                <ext:Label ID="Label23" runat="server" Width="180" Text="Average Units per Day:" />
                                <ext:TextField ID="uxAvgUnitsPerDay" runat="server" Width="100" ReadOnly="true" Text="0.00" Cls="textRightAlign" TabIndex="-7" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer11"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label24" runat="server" Width="180" Text="Total Weekly Direct Expense:" />
                                <ext:TextField ID="uxTotalWklyDirects" runat="server" Width="100" ReadOnly="true" Text="0.00" Cls="textRightAlign" TabIndex="-8" />
                                <ext:Label ID="Label38" runat="server" Width="105" />
                                <ext:Label ID="Label39" runat="server" Width="180" Text="Total Direct Expenses Left:" />
                                <ext:TextField ID="uxTotalDirectsLeft" runat="server" Width="100" ReadOnly="true" Text="0.00" Cls="textRightAlign" TabIndex="-9" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer14"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label25" runat="server" Width="180" Text="Total Direct Expenses per Day:" />
                                <ext:TextField ID="uxTotalDirectsPerDay" runat="server" Width="100" ReadOnly="true" Text="0.00" Cls="textRightAlign" TabIndex="-10" />
                                <ext:Label ID="Label41" runat="server" Width="105" />
                                <ext:Label ID="Label42" runat="server" Width="180" Text="Total Material Expense Left:" />
                                <ext:TextField ID="uxTotalMaterialLeft" runat="server" Width="100" ReadOnly="true" Text="0.00" Cls="textRightAlign" TabIndex="-11" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer16"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label34" runat="server" Width="665" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldSet ID="FieldSet2" runat="server" Width="665" Padding="10" Cls="detailBackground">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer8"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label11" runat="server" Width="77" />
                                        <ext:Label ID="Label13" runat="server" Width="100" Text="Gross Receipts" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label14" runat="server" Width="100" Text="Material Usage" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label15" runat="server" Width="100" Text="Gross Revenue" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label16" runat="server" Width="100" Text="Direct Expenses" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label17" runat="server" Width="100" Text="OP" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label18" runat="server" Width="77" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer9"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label19" runat="server" Width="77" />
                                        <ext:TextField ID="uxEGrossRec" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" TabIndex="-12" />
                                        <ext:TextField ID="uxEMatUsage" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" TabIndex="-13" />
                                        <ext:TextField ID="uxEGrossRev" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" TabIndex="-14" />
                                        <ext:TextField ID="uxEDirects" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" TabIndex="-15" />
                                        <ext:TextField ID="uxEOP" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" TabIndex="-16" />
                                        <ext:Label ID="Label20" runat="server" Width="77" />
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:FieldSet>

                        <ext:FieldContainer ID="FieldContainer17"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label3" runat="server" Width="590" />
                                <ext:Button ID="uxCloseDetailSheet" runat="server" Text="Close Form" Width="75">
                                    <Listeners>
                                        <Click Fn="closeWindow" />
                                    </Listeners>
                                    <DirectEvents>
                                        <Click>
                                            <EventMask ShowMask="true" Msg="Closing..." />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:FieldContainer>

                    </Items>
                </ext:FormPanel>

                <ext:Hidden ID="uxHidSelMatRecID" runat="server" />
                <ext:Hidden ID="uxHidSelEquipRecID" runat="server" />
                <ext:Hidden ID="uxHidSelPersRecID" runat="server" />

                <ext:Hidden ID="uxHidDelRecord" runat="server" />
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

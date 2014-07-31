<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umDetailSheet.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.umDetailSheet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                if (e.record.data.DETAIL_SHEET_ID == null) {
                    SaveRecord.deSaveSubGridData(e.record.data, 0);
                }
                else {
                    SaveRecord.deSaveSubGridData(e.record.data, e.record.data.DETAIL_SHEET_ID);
                }
            }
        }
        var closeWindow = function () {
            parent.App.direct.CloseDetailWindow(App.uxDetailName.value);
            parent.Ext.getCmp('uxAddEditDetailSheet').close();
        }
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
                    Height="225"
                    BodyPadding="20"
                    Disabled="false">
                    <Items>
                        <ext:FieldContainer ID="FieldContainer3"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="uxYearVersion" runat="server" Width="200" Text="2014 2nd Reforecast" />
                                <ext:Label ID="Label1" runat="server" Width="225" />
                                <ext:Label ID="uxWeekEnding" runat="server" Width="200" Text="Week Ending:  N/A" Cls="labelRightAlign" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer6"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label8" runat="server" Width="900" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer5"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label2" runat="server" Width="200" Text="BBProject Name:  " />
                                <ext:Label ID="uxProjectName" runat="server" Width="425" Text="" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer4"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="uxDetailNameLabel" runat="server" Width="200" Text="Detail Sheet (1 of 1): " />
                                <ext:TextField ID="uxDetailName" runat="server" Width="425" ReadOnly="false" Text="">
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
                                <ext:Label ID="Label7" runat="server" Width="625" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldSet ID="FieldSet1" runat="server" Width="625" Padding="10" Cls="detailBackground">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer12"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label4" runat="server" Width="52" />
                                        <ext:Label ID="Label26" runat="server" Width="100" Text="Gross Receipts" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label27" runat="server" Width="100" Text="Material Usage" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label30" runat="server" Width="100" Text="Gross Revenue" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label28" runat="server" Width="100" Text="Direct Expenses" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label31" runat="server" Width="100" Text="OP" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label5" runat="server" Width="52" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer13"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label9" runat="server" Width="52" />
                                        <ext:TextField ID="uxSGrossRec" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" />
                                        <ext:TextField ID="uxSMatUsage" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" />
                                        <ext:TextField ID="uxSGrossRev" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" />
                                        <ext:TextField ID="uxSDirects" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" />
                                        <ext:TextField ID="uxSOP" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" />
                                        <ext:Label ID="Label10" runat="server" Width="52" />
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:FieldSet>
                    </Items>
                </ext:FormPanel>


                <%-------------------------------------------------- Middle --------------------------------------------------%>
                <ext:TabPanel ID="uxTabs" runat="server" Height="50" Region="Center">
                    <Items>

                        <%--------------------------- Main ---------------------------%>
                        <ext:Panel runat="server"
                            Title="Main"
                            ID="uxMain"
                            Disabled="false"
                            BodyPadding="20"
                            Layout="FitLayout">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer33"
                                    runat="server"
                                    Layout="VBoxLayout">
                                    <Items>
                                        <ext:FieldContainer ID="FieldContainer31"
                                            runat="server"
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:Label ID="Label6" runat="server" Width="300" Text="Total Receipts Remaining:" />
                                                <ext:TextField ID="uxRecRemaining" runat="server" Width="100" ReadOnly="false" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign">
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
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer ID="FieldContainer32"
                                            runat="server"
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:Label ID="Label32" runat="server" Width="300" Text="Total Days Remaining (including resprays):" />
                                                <ext:TextField ID="uxDaysRemaining" runat="server" Width="100" ReadOnly="false" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign">
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
                                        <ext:FieldContainer ID="FieldContainer37"
                                            runat="server"
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:Label ID="Label33" runat="server" Width="300" Text="Total Units Remaining (including resprays):" />
                                                <ext:TextField ID="uxUnitsRemaining" runat="server" Width="100" ReadOnly="false" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign">
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
                                        <ext:FieldContainer ID="FieldContainer34"
                                            runat="server"
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:Label ID="Label44" runat="server" Width="300" Text="Total Days Per Week Worked:" />
                                                <ext:TextField ID="uxDaysWorked" runat="server" Width="100" ReadOnly="false" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign">
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
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer ID="FieldContainer35"
                                            runat="server"
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:Label ID="Label49" runat="server" Width="600" Text="" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer ID="FieldContainer38"
                                            runat="server"
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:Label ID="Label45" runat="server" Width="300" Text="Comments:" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer ID="FieldContainer36"
                                            runat="server"
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextArea ID="uxComments" runat="server" Width="600" ReadOnly="false">
                                                    <DirectEvents>
                                                        <Blur OnEvent="deSaveMainTabField">
                                                            <ExtraParams>
                                                                <%--<ext:Parameter Name="RecType" Value="RECREMAIN" Mode="Value" />
                                                                <ext:Parameter Name="FieldText" Value="#{uxComments}.value" Mode="Raw" />--%>
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
                            BodyPadding="20"
                            Layout="FitLayout">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer1"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:GridPanel ID="uxMaterialGridPanel" runat="server" Region="West" Width="600" Margin="5" Layout="FitLayout">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxMaterialGridStore" OnReadData="deReadGridData"
                                                    AutoDataBind="true" WarningOnDirty="false">
                                                    <Model>
                                                        <ext:Model ID="Model1" Name="Material" IDProperty="DETAIL_SHEET_ID" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DETAIL_SHEET_ID" />
                                                                <ext:ModelField Name="REC_TYPE" DefaultValue="1" />
                                                                <ext:ModelField Name="DESC_1" />
                                                                <ext:ModelField Name="AMT_1" />
                                                                <ext:ModelField Name="DESC_2" />
                                                                <ext:ModelField Name="AMT_2" />
                                                                <ext:ModelField Name="TOTAL" />
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
                                                    <ext:Column ID="Column4" runat="server" DataIndex="DESC_1" Text="Material" Flex="2">
                                                        <Editor>
                                                            <ext:TextField ID="TextField6" runat="server" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="Column1" runat="server" DataIndex="AMT_1" Text="Unit Cost" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:TextField ID="TextField7" runat="server" />
                                                        </Editor>
                                                    </ext:NumberColumn>
                                                    <ext:Column ID="Column2" runat="server" DataIndex="DESC_2" Text="UOM" Flex="1">
                                                        <Editor>
                                                            <ext:TextField ID="TextField8" runat="server" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="Column3" runat="server" DataIndex="AMT_2" Text="Qty" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:TextField ID="TextField9" runat="server" />
                                                        </Editor>
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="Column5" runat="server" DataIndex="TOTAL" Text="Total" Flex="1" Align="Right">
                                                    </ext:NumberColumn>
                                                </Columns>
                                            </ColumnModel>
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
                                                            <Listeners>
                                                                <Click Handler="#{uxMaterialGridStore}.insert(0, new Material());" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:Button ID="uxDeleteMaterial" runat="server" Text="Delete Selected" Icon="Delete">
                                                            <DirectEvents>
                                                                <Click OnEvent="deDeleteRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordID" Value="#{uxMaterialGridPanel}.getSelectionModel().getSelection()[0].data.DETAIL_SHEET_ID" Mode="Raw" />
                                                                    </ExtraParams>
                                                                    <EventMask ShowMask="true" Msg="Deleting..." />
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
                                                        <ext:DisplayField ID="uxTotalMaterial" runat="server" Text="0.00" Flex="1" FieldStyle="text-align:right" Cls="grandTotalForeground" />
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
                            BodyPadding="20"
                            Layout="FitLayout">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer2"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:GridPanel ID="uxEquipmentGridPanel" runat="server" Region="West" Width="600" Margin="5" Layout="FitLayout">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxEquipmentGridStore" OnReadData="deReadGridData"
                                                    AutoDataBind="true" WarningOnDirty="false">
                                                    <Model>
                                                        <ext:Model ID="Model2" Name="Equipment" IDProperty="DETAIL_SHEET_ID" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DETAIL_SHEET_ID" />
                                                                <ext:ModelField Name="REC_TYPE" DefaultValue="2" />
                                                                <ext:ModelField Name="DESC_1" />
                                                                <ext:ModelField Name="AMT_1" />
                                                                <ext:ModelField Name="DESC_2" />
                                                                <ext:ModelField Name="AMT_2" />
                                                                <ext:ModelField Name="TOTAL" />
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
                                                    <ext:Column ID="Column6" runat="server" DataIndex="DESC_1" Text="Equipment/Equipment Travel" Flex="2">
                                                        <Editor>
                                                            <ext:TextField ID="TextField1" runat="server" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="NumberColumn1" runat="server" DataIndex="AMT_1" Text="Unit Cost" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:TextField ID="TextField2" runat="server" />
                                                        </Editor>
                                                    </ext:NumberColumn>
                                                    <ext:Column ID="Column7" runat="server" DataIndex="DESC_2" Text="UOM" Flex="1">
                                                        <Editor>
                                                            <ext:TextField ID="TextField3" runat="server" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="NumberColumn2" runat="server" DataIndex="AMT_2" Text="Qty" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:TextField ID="TextField4" runat="server" />
                                                        </Editor>
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn3" runat="server" DataIndex="TOTAL" Text="Total" Flex="1" Align="Right">
                                                    </ext:NumberColumn>
                                                </Columns>
                                            </ColumnModel>
                                            <Plugins>
                                                <ext:CellEditing ID="CellEditing1" runat="server">
                                                    <Listeners>
                                                        <Edit Fn="editRecord" />
                                                    </Listeners>
                                                </ext:CellEditing>
                                            </Plugins>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" />
                                            </SelectionModel>
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar1" runat="server" Region="North">
                                                    <Items>
                                                        <ext:Button ID="Button1" runat="server" Text="Add New" Icon="Add">
                                                            <Listeners>
                                                                <Click Handler="#{uxEquipmentGridStore}.insert(0, new Equipment());" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:Button ID="Button2" runat="server" Text="Delete Selected" Icon="Delete">
                                                            <DirectEvents>
                                                                <Click OnEvent="deDeleteRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordID" Value="#{uxEquipmentGridPanel}.getSelectionModel().getSelection()[0].data.DETAIL_SHEET_ID" Mode="Raw" />
                                                                    </ExtraParams>
                                                                    <EventMask ShowMask="true" Msg="Deleting..." />
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
                            BodyPadding="20"
                            Layout="FitLayout">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer19"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:GridPanel ID="uxPersonnelGridPanel" runat="server" Region="West" Width="600" Margin="5" Layout="FitLayout">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxPersonnelGridStore" OnReadData="deReadGridData"
                                                    AutoDataBind="true" WarningOnDirty="false">
                                                    <Model>
                                                        <ext:Model ID="Model3" Name="Personnel" IDProperty="DETAIL_SHEET_ID" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DETAIL_SHEET_ID" />
                                                                <ext:ModelField Name="REC_TYPE" DefaultValue="3" />
                                                                <ext:ModelField Name="DESC_1" />
                                                                <ext:ModelField Name="AMT_1" />
                                                                <ext:ModelField Name="DESC_2" />
                                                                <ext:ModelField Name="AMT_2" />
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
                                                    <ext:Column ID="Column8" runat="server" DataIndex="DESC_1" Text="Position" Flex="2">
                                                        <Editor>
                                                            <ext:TextField ID="TextField5" runat="server" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="NumberColumn4" runat="server" DataIndex="AMT_1" Text="Unit Cost" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:TextField ID="TextField10" runat="server" />
                                                        </Editor>
                                                    </ext:NumberColumn>
                                                    <ext:Column ID="Column9" runat="server" DataIndex="DESC_2" Text="UOM" Flex="1">
                                                        <Editor>
                                                            <ext:TextField ID="TextField11" runat="server" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="NumberColumn5" runat="server" DataIndex="AMT_2" Text="Qty" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:TextField ID="TextField12" runat="server" />
                                                        </Editor>
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn6" runat="server" DataIndex="TOTAL" Text="Total" Flex="1" Align="Right">
                                                    </ext:NumberColumn>
                                                </Columns>
                                            </ColumnModel>
                                            <Plugins>
                                                <ext:CellEditing ID="CellEditing3" runat="server">
                                                    <Listeners>
                                                        <Edit Fn="editRecord" />
                                                    </Listeners>
                                                </ext:CellEditing>
                                            </Plugins>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel3" runat="server" />
                                            </SelectionModel>
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar3" runat="server" Region="North">
                                                    <Items>
                                                        <ext:Button ID="Button3" runat="server" Text="Add New" Icon="Add">
                                                            <Listeners>
                                                                <Click Handler="#{uxPersonnelGridStore}.insert(0, new Personnel());" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:Button ID="Button4" runat="server" Text="Delete Selected" Icon="Delete">
                                                            <DirectEvents>
                                                                <Click OnEvent="deDeleteRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordID" Value="#{uxPersonnelGridPanel}.getSelectionModel().getSelection()[0].data.DETAIL_SHEET_ID" Mode="Raw" />
                                                                    </ExtraParams>
                                                                    <EventMask ShowMask="true" Msg="Deleting..." />
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <DockedItems>
                                                <ext:FieldContainer ID="FieldContainer20" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="grandTotalBackground">
                                                    <Items>
                                                        <ext:DisplayField ID="DisplayField11" runat="server" Width="10" />
                                                        <ext:DisplayField ID="DisplayField12" runat="server" Text="Total Personnel:" Flex="2" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField13" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField14" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField15" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="uxTotalPersonnel" runat="server" Text="0.00" Flex="1" FieldStyle="text-align:right" Cls="grandTotalForeground" />
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
                            BodyPadding="20"
                            Layout="FitLayout">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer21"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:GridPanel ID="uxPerDiemGridPanel" runat="server" Region="West" Width="600" Margin="5" Layout="FitLayout">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxPerDiemGridStore" OnReadData="deReadGridData"
                                                    AutoDataBind="true" WarningOnDirty="false">
                                                    <Model>
                                                        <ext:Model ID="Model4" Name="PerDiem" IDProperty="DETAIL_SHEET_ID" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DETAIL_SHEET_ID" />
                                                                <ext:ModelField Name="REC_TYPE" DefaultValue="4" />
                                                                <ext:ModelField Name="DESC_1" />
                                                                <ext:ModelField Name="AMT_1" />
                                                                <ext:ModelField Name="DESC_2" />
                                                                <ext:ModelField Name="AMT_2" />
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
                                                    <ext:Column ID="Column10" runat="server" DataIndex="DESC_1" Text="Rate" Flex="2">
                                                        <Editor>
                                                            <ext:TextField ID="TextField13" runat="server" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="NumberColumn7" runat="server" DataIndex="AMT_1" Text="Unit Cost" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:TextField ID="TextField14" runat="server" />
                                                        </Editor>
                                                    </ext:NumberColumn>
                                                    <ext:Column ID="Column11" runat="server" DataIndex="DESC_2" Text="UOM" Flex="1">
                                                        <Editor>
                                                            <ext:TextField ID="TextField15" runat="server" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="NumberColumn8" runat="server" DataIndex="AMT_2" Text="Qty" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:TextField ID="TextField16" runat="server" />
                                                        </Editor>
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn9" runat="server" DataIndex="TOTAL" Text="Total" Flex="1" Align="Right">
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
                                                <ext:RowSelectionModel ID="RowSelectionModel4" runat="server" />
                                            </SelectionModel>
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar4" runat="server" Region="North">
                                                    <Items>
                                                        <ext:Button ID="Button5" runat="server" Text="Add New" Icon="Add">
                                                            <Listeners>
                                                                <Click Handler="#{uxPerDiemGridStore}.insert(0, new PerDiem());" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:Button ID="Button6" runat="server" Text="Delete Selected" Icon="Delete">
                                                            <DirectEvents>
                                                                <Click OnEvent="deDeleteRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordID" Value="#{uxPerDiemGridPanel}.getSelectionModel().getSelection()[0].data.DETAIL_SHEET_ID" Mode="Raw" />
                                                                    </ExtraParams>
                                                                    <EventMask ShowMask="true" Msg="Deleting..." />
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <DockedItems>
                                                <ext:FieldContainer ID="FieldContainer22" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="grandTotalBackground">
                                                    <Items>
                                                        <ext:DisplayField ID="DisplayField16" runat="server" Width="10" />
                                                        <ext:DisplayField ID="DisplayField17" runat="server" Text="Total Per Diem:" Flex="2" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField18" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField19" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField20" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="uxTotalPerDiem" runat="server" Text="0.00" Flex="1" FieldStyle="text-align:right" Cls="grandTotalForeground" />
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
                            BodyPadding="20"
                            Layout="FitLayout">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer23"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:GridPanel ID="uxTravelGridPanel" runat="server" Region="West" Width="600" Margin="5" Layout="FitLayout">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxTravelGridStore" OnReadData="deReadGridData"
                                                    AutoDataBind="true" WarningOnDirty="false">
                                                    <Model>
                                                        <ext:Model ID="Model5" Name="Travel" IDProperty="DETAIL_SHEET_ID" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DETAIL_SHEET_ID" />
                                                                <ext:ModelField Name="REC_TYPE" DefaultValue="5" />
                                                                <ext:ModelField Name="DESC_1" />
                                                                <ext:ModelField Name="AMT_1" />
                                                                <ext:ModelField Name="DESC_2" />
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
                                                    <ext:Column ID="Column12" runat="server" DataIndex="DESC_1" Text="Rate" Flex="2">
                                                        <Editor>
                                                            <ext:TextField ID="TextField17" runat="server" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="NumberColumn10" runat="server" DataIndex="AMT_1" Text="Unit Cost" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:TextField ID="TextField18" runat="server" />
                                                        </Editor>
                                                    </ext:NumberColumn>
                                                    <ext:Column ID="Column13" runat="server" DataIndex="DESC_2" Text="UOM" Flex="1">
                                                        <Editor>
                                                            <ext:TextField ID="TextField19" runat="server" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="NumberColumn11" runat="server" DataIndex="AMT_2" Text="Qty" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:TextField ID="TextField20" runat="server" />
                                                        </Editor>
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn12" runat="server" DataIndex="TOTAL" Text="Total" Flex="1" Align="Right">
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
                                                <ext:RowSelectionModel ID="RowSelectionModel5" runat="server" />
                                            </SelectionModel>
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar5" runat="server" Region="North">
                                                    <Items>
                                                        <ext:Button ID="Button7" runat="server" Text="Add New" Icon="Add">
                                                            <Listeners>
                                                                <Click Handler="#{uxTravelGridStore}.insert(0, new Travel());" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:Button ID="Button8" runat="server" Text="Delete Selected" Icon="Delete">
                                                            <DirectEvents>
                                                                <Click OnEvent="deDeleteRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordID" Value="#{uxTravelGridPanel}.getSelectionModel().getSelection()[0].data.DETAIL_SHEET_ID" Mode="Raw" />
                                                                    </ExtraParams>
                                                                    <EventMask ShowMask="true" Msg="Deleting..." />
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <DockedItems>
                                                <ext:FieldContainer ID="FieldContainer24" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="grandTotalBackground">
                                                    <Items>
                                                        <ext:DisplayField ID="DisplayField21" runat="server" Width="10" />
                                                        <ext:DisplayField ID="DisplayField22" runat="server" Text="Total Travel:" Flex="2" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField23" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField24" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField25" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="uxTotalTravel" runat="server" Text="0.00" Flex="1" FieldStyle="text-align:right" Cls="grandTotalForeground" />
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
                            BodyPadding="20"
                            Layout="FitLayout">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer25"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:GridPanel ID="uxMotelsGridPanel" runat="server" Region="West" Width="600" Margin="5" Layout="FitLayout">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxMotelsGridStore" OnReadData="deReadGridData"
                                                    AutoDataBind="true" WarningOnDirty="false">
                                                    <Model>
                                                        <ext:Model ID="Model6" Name="Motels" IDProperty="DETAIL_SHEET_ID" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DETAIL_SHEET_ID" />
                                                                <ext:ModelField Name="REC_TYPE" DefaultValue="6" />
                                                                <ext:ModelField Name="DESC_1" />
                                                                <ext:ModelField Name="AMT_1" />
                                                                <ext:ModelField Name="DESC_2" />
                                                                <ext:ModelField Name="AMT_2" />
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
                                                    <ext:Column ID="Column14" runat="server" DataIndex="DESC_1" Text="Rate" Flex="2">
                                                        <Editor>
                                                            <ext:TextField ID="TextField21" runat="server" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="NumberColumn13" runat="server" DataIndex="AMT_1" Text="Unit Cost" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:TextField ID="TextField22" runat="server" />
                                                        </Editor>
                                                    </ext:NumberColumn>
                                                    <ext:Column ID="Column15" runat="server" DataIndex="DESC_2" Text="UOM" Flex="1">
                                                        <Editor>
                                                            <ext:TextField ID="TextField23" runat="server" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="NumberColumn14" runat="server" DataIndex="AMT_2" Text="Qty" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:TextField ID="TextField24" runat="server" />
                                                        </Editor>
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn15" runat="server" DataIndex="TOTAL" Text="Total" Flex="1" Align="Right">
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
                                                <ext:RowSelectionModel ID="RowSelectionModel6" runat="server" />
                                            </SelectionModel>
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar6" runat="server" Region="North">
                                                    <Items>
                                                        <ext:Button ID="Button9" runat="server" Text="Add New" Icon="Add">
                                                            <Listeners>
                                                                <Click Handler="#{uxMotelsGridStore}.insert(0, new Motels());" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:Button ID="Button10" runat="server" Text="Delete Selected" Icon="Delete">
                                                            <DirectEvents>
                                                                <Click OnEvent="deDeleteRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordID" Value="#{uxMotelsGridPanel}.getSelectionModel().getSelection()[0].data.DETAIL_SHEET_ID" Mode="Raw" />
                                                                    </ExtraParams>
                                                                    <EventMask ShowMask="true" Msg="Deleting..." />
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <DockedItems>
                                                <ext:FieldContainer ID="FieldContainer26" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="grandTotalBackground">
                                                    <Items>
                                                        <ext:DisplayField ID="DisplayField27" runat="server" Width="10" />
                                                        <ext:DisplayField ID="DisplayField28" runat="server" Text="Total Motels:" Flex="2" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField29" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField30" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField31" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="uxTotalMotels" runat="server" Text="0.00" Flex="1" FieldStyle="text-align:right" Cls="grandTotalForeground" />
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
                            BodyPadding="20"
                            Layout="FitLayout">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer27"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:GridPanel ID="uxMiscGridPanel" runat="server" Region="West" Width="600" Margin="5" Layout="FitLayout">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxMiscGridStore" OnReadData="deReadGridData"
                                                    AutoDataBind="true" WarningOnDirty="false">
                                                    <Model>
                                                        <ext:Model ID="Model7" Name="Misc" IDProperty="DETAIL_SHEET_ID" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DETAIL_SHEET_ID" />
                                                                <ext:ModelField Name="REC_TYPE" DefaultValue="7" />
                                                                <ext:ModelField Name="DESC_1" />
                                                                <ext:ModelField Name="AMT_1" />
                                                                <ext:ModelField Name="DESC_2" />
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
                                                    <ext:Column ID="Column16" runat="server" DataIndex="DESC_1" Text="Rate" Flex="2">
                                                        <Editor>
                                                            <ext:TextField ID="TextField25" runat="server" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="NumberColumn16" runat="server" DataIndex="AMT_1" Text="Unit Cost" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:TextField ID="TextField26" runat="server" />
                                                        </Editor>
                                                    </ext:NumberColumn>
                                                    <ext:Column ID="Column17" runat="server" DataIndex="DESC_2" Text="UOM" Flex="1">
                                                        <Editor>
                                                            <ext:TextField ID="TextField27" runat="server" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="NumberColumn17" runat="server" DataIndex="AMT_2" Text="Qty" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:TextField ID="TextField28" runat="server" />
                                                        </Editor>
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn18" runat="server" DataIndex="TOTAL" Text="Total" Flex="1" Align="Right">
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
                                                <ext:RowSelectionModel ID="RowSelectionModel7" runat="server" />
                                            </SelectionModel>
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar7" runat="server" Region="North">
                                                    <Items>
                                                        <ext:Button ID="Button11" runat="server" Text="Add New" Icon="Add">
                                                            <Listeners>
                                                                <Click Handler="#{uxMiscGridStore}.insert(0, new Misc());" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:Button ID="Button12" runat="server" Text="Delete Selected" Icon="Delete">
                                                            <DirectEvents>
                                                                <Click OnEvent="deDeleteRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordID" Value="#{uxMiscGridPanel}.getSelectionModel().getSelection()[0].data.DETAIL_SHEET_ID" Mode="Raw" />
                                                                    </ExtraParams>
                                                                    <EventMask ShowMask="true" Msg="Deleting..." />
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <DockedItems>
                                                <ext:FieldContainer ID="FieldContainer28" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="grandTotalBackground">
                                                    <Items>
                                                        <ext:DisplayField ID="DisplayField33" runat="server" Width="10" />
                                                        <ext:DisplayField ID="DisplayField34" runat="server" Text="Total Misc:" Flex="2" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField35" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField36" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField37" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="uxTotalMisc" runat="server" Text="0.00" Flex="1" FieldStyle="text-align:right" Cls="grandTotalForeground" />
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
                            BodyPadding="20"
                            Layout="FitLayout">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer29"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:GridPanel ID="uxLumpSumGridPanel" runat="server" Region="West" Width="600" Margin="5" Layout="FitLayout">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxLumpSumGridStore" OnReadData="deReadGridData"
                                                    AutoDataBind="true" WarningOnDirty="false">
                                                    <Model>
                                                        <ext:Model ID="Model8" Name="LumpSum" IDProperty="DETAIL_SHEET_ID" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DETAIL_SHEET_ID" />
                                                                <ext:ModelField Name="REC_TYPE" DefaultValue="8" />
                                                                <ext:ModelField Name="DESC_1" />
                                                                <ext:ModelField Name="AMT_1" />
                                                                <ext:ModelField Name="DESC_2" />
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
                                                    <ext:Column ID="Column18" runat="server" DataIndex="DESC_1" Text="Rate" Flex="2">
                                                        <Editor>
                                                            <ext:TextField ID="TextField29" runat="server" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="NumberColumn19" runat="server" DataIndex="AMT_1" Text="Unit Cost" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:TextField ID="TextField30" runat="server" />
                                                        </Editor>
                                                    </ext:NumberColumn>
                                                    <ext:Column ID="Column19" runat="server" DataIndex="DESC_2" Text="UOM" Flex="1">
                                                        <Editor>
                                                            <ext:TextField ID="TextField31" runat="server" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="NumberColumn20" runat="server" DataIndex="AMT_2" Text="Qty" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:TextField ID="TextField32" runat="server" />
                                                        </Editor>
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="NumberColumn21" runat="server" DataIndex="TOTAL" Text="Total" Flex="1" Align="Right">
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
                                                <ext:RowSelectionModel ID="RowSelectionModel8" runat="server" />
                                            </SelectionModel>
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar8" runat="server" Region="North">
                                                    <Items>
                                                        <ext:Button ID="Button13" runat="server" Text="Add New" Icon="Add">
                                                            <Listeners>
                                                                <Click Handler="#{uxLumpSumGridStore}.insert(0, new LumpSum());" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:Button ID="Button14" runat="server" Text="Delete Selected" Icon="Delete">
                                                            <DirectEvents>
                                                                <Click OnEvent="deDeleteRecord">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="RecordID" Value="#{uxLumpSumridPanel}.getSelectionModel().getSelection()[0].data.DETAIL_SHEET_ID" Mode="Raw" />
                                                                    </ExtraParams>
                                                                    <EventMask ShowMask="true" Msg="Deleting..." />
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <DockedItems>
                                                <ext:FieldContainer ID="FieldContainer30" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="grandTotalBackground">
                                                    <Items>
                                                        <ext:DisplayField ID="DisplayField39" runat="server" Width="10" />
                                                        <ext:DisplayField ID="DisplayField40" runat="server" Text="Total Lump Sum:" Flex="2" Cls="grandTotalForeground" />
                                                        <ext:DisplayField ID="DisplayField41" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField42" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="DisplayField43" runat="server" Flex="1" />
                                                        <ext:DisplayField ID="uxTotalLumpSum" runat="server" Text="0.00" Flex="1" FieldStyle="text-align:right" Cls="grandTotalForeground" />
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
                    Height="250"
                    BodyPadding="20"
                    Disabled="false">
                    <Items>

                        <ext:FieldContainer ID="FieldContainer10"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label21" runat="server" Width="180" Text="Labor Burden @ 40%:" />
                                <ext:Label ID="uxLaborBurden" runat="server" Width="100" Text="$1,000.00" Cls="labelRightAlign" />
                                <ext:Label ID="Label22" runat="server" Width="65" />
                                <ext:Label ID="Label23" runat="server" Width="180" Text="Average Units per Day:" />
                                <ext:Label ID="uxAvgUnitsPerDay" runat="server" Width="100" Text="$1,000.00" Cls="labelRightAlign" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer11"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label24" runat="server" Width="180" Text="Total Weekly Direct Expense:" />
                                <ext:Label ID="uxTotalWklyDirects" runat="server" Width="100" Text="$1,000.00" Cls="labelRightAlign" />
                                <ext:Label ID="Label38" runat="server" Width="65" />
                                <ext:Label ID="Label39" runat="server" Width="180" Text="Total Direct Expenses Left:" />
                                <ext:Label ID="uxTotalDirectsLeft" runat="server" Width="100" Text="$1,000.00" Cls="labelRightAlign" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer14"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label25" runat="server" Width="180" Text="Total Direct Expenses per Day:" />
                                <ext:Label ID="uxTotalDirectsPerDay" runat="server" Width="100" Text="$1,000.00" Cls="labelRightAlign" />
                                <ext:Label ID="Label41" runat="server" Width="65" />
                                <ext:Label ID="Label42" runat="server" Width="180" Text="Total Material Expense Left:" />
                                <ext:Label ID="uxTotalMaterialLeft" runat="server" Width="100" Text="$1,000.00" Cls="labelRightAlign" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer16"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label34" runat="server" Width="625" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldSet ID="FieldSet2" runat="server" Width="625" Padding="10" Cls="detailBackground">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer8"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label11" runat="server" Width="52" />
                                        <ext:Label ID="Label13" runat="server" Width="100" Text="Gross Receipts" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label14" runat="server" Width="100" Text="Material Usage" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label15" runat="server" Width="100" Text="Gross Revenue" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label16" runat="server" Width="100" Text="Direct Expenses" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label17" runat="server" Width="100" Text="OP" Cls="detailForegroundCenter" />
                                        <ext:Label ID="Label18" runat="server" Width="52" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer9"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label19" runat="server" Width="52" />
                                        <ext:TextField ID="uxEGrossRec" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" />
                                        <ext:TextField ID="uxEMatUsage" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" />
                                        <ext:TextField ID="uxEGrossRev" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" />
                                        <ext:TextField ID="uxEDirects" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" />
                                        <ext:TextField ID="uxEOP" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" />
                                        <ext:Label ID="Label20" runat="server" Width="52" />
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:FieldSet>

                        <ext:FieldContainer ID="FieldContainer18"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label12" runat="server" Width="625" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer17"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label3" runat="server" Width="550" />
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

            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

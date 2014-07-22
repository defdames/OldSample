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
        var editMaterial = function (editor, e) {
            if (e.originalValue != e.value) {
                if (e.record.data.DETAIL_SHEET_ID == null) {
                    SaveRecord.deSaveMaterial(e.record.data, 0);
                }
                else {
                    SaveRecord.deSaveMaterial(e.record.data, e.record.data.DETAIL_SHEET_ID);
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
                                <ext:Label ID="uxWeekEnding" runat="server" Width="200" Text="Week Ending N/A" Cls="labelRightAlign" />
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
                                <ext:Label ID="Label2" runat="server" Width="200" Text="Project Name:  " />
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
                        <ext:Panel runat="server"
                            Title="Material"
                            ID="uxMaterial"
                            Disabled="false"
                            BodyPadding="20"
                            Layout="FitLayout">
                            <Items>


                                <%--------------------------- Material ---------------------------%>
                                <ext:FieldContainer ID="FieldContainer1"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:GridPanel ID="uxMaterialGridPanel" runat="server" Region="West" Width="600" Margin="5" Layout="FitLayout">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxMaterialGridStore" OnReadData="deReadMaterialGridData"
                                                    AutoDataBind="true" WarningOnDirty="false">
                                                    <Model>
                                                        <ext:Model ID="Model1" Name="Material" IDProperty="DETAIL_SHEET_ID" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DETAIL_SHEET_ID" />
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
                                                </ext:Store>
                                            </Store>
                                            <ColumnModel>
                                                <Columns>
                                                    <ext:Column ID="Column4" runat="server" DataIndex="DESC_1" Text="Material" Flex="2">
                                                        <Editor>
                                                            <ext:TextField ID="TextField6" runat="server" EmptyText="Material" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="Column1" runat="server" DataIndex="AMT_1" Text="Unit Cost" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:TextField ID="TextField7" runat="server" EmptyText="Unit Cost" />
                                                        </Editor>
                                                    </ext:NumberColumn>
                                                    <ext:Column ID="Column2" runat="server" DataIndex="DESC_2" Text="UOM" Flex="1">
                                                        <Editor>
                                                            <ext:TextField ID="TextField8" runat="server" EmptyText="UOM" />
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:NumberColumn ID="Column3" runat="server" DataIndex="AMT_2" Text="Qty" Flex="1" Align="Right">
                                                        <Editor>
                                                            <ext:TextField ID="TextField9" runat="server" EmptyText="Qty" />
                                                        </Editor>
                                                    </ext:NumberColumn>
                                                    <ext:NumberColumn ID="Column5" runat="server" DataIndex="TOTAL" Text="Total" Flex="1" Align="Right">
                                                        <Renderer Handler="return record.data.AMT_1*record.data.AMT_2;" />
                                                        <%--<Renderer Fn="Ext.util.Format.numberRenderer('0,000.00')" />--%>
                                                    </ext:NumberColumn>
                                                </Columns>
                                            </ColumnModel>
                                            <Plugins>
                                                <ext:CellEditing ID="CellEditing1" runat="server" ClicksToEdit="2">
                                                    <Listeners>
                                                        <Edit Fn="editMaterial" />
                                                    </Listeners>
                                                </ext:CellEditing>
                                            </Plugins>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" Mode="Single" />
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
                                                            <Listeners>
                                                                <Click Handler="#{uxMaterialGridPanel}.deleteSelected(); #{UserForm}.getForm().reset();" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <%--<ext:Button ID="uxSaveRRButton" runat="server" Text="Save Railroad" Icon="Add">--%>

                                                        <%--                                                            <DirectEvents>
                                                                <Click OnEvent="deSaveRailRoad" Before="#{uxMaterialGridStore}.isDirty()">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="rrdata" Value="#{uxMaterialGridStore}.getChangedData()" Mode="Raw" Encode="true" />
                                                                    </ExtraParams>
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>--%>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <%--                                            <DirectEvents>
                                                <Select OnEvent="deLoadStores">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="RailroadId" Value="#{uxMaterialGridPanel}.getSelectionModel().getSelection()[0].data.RAILROAD_ID" Mode="Raw" />
                                                    </ExtraParams>
                                                </Select>
                                            </DirectEvents>--%>
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
                            Layout="FitLayout">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer2"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
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
                                <ext:Label ID="Label35" runat="server" Width="100" Text="$1,000.00" Cls="labelRightAlign" />
                                <ext:Label ID="Label22" runat="server" Width="65" />
                                <ext:Label ID="Label23" runat="server" Width="180" Text="Average Units per Day:" />
                                <ext:Label ID="Label36" runat="server" Width="100" Text="$1,000.00" Cls="labelRightAlign" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer11"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label24" runat="server" Width="180" Text="Total Weekly Direct Expense%:" />
                                <ext:Label ID="Label37" runat="server" Width="100" Text="$1,000.00" Cls="labelRightAlign" />
                                <ext:Label ID="Label38" runat="server" Width="65" />
                                <ext:Label ID="Label39" runat="server" Width="180" Text="Total Direct Expenses Left:" />
                                <ext:Label ID="Label40" runat="server" Width="100" Text="$1,000.00" Cls="labelRightAlign" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer14"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label25" runat="server" Width="180" Text="Total Direct Expenses per Day:" />
                                <ext:Label ID="Label29" runat="server" Width="100" Text="$1,000.00" Cls="labelRightAlign" />
                                <ext:Label ID="Label41" runat="server" Width="65" />
                                <ext:Label ID="Label42" runat="server" Width="180" Text="Total Material Expense Left:" />
                                <ext:Label ID="Label43" runat="server" Width="100" Text="$1,000.00" Cls="labelRightAlign" />
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
                                        <ext:TextField ID="TextField1" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" />
                                        <ext:TextField ID="TextField2" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" />
                                        <ext:TextField ID="TextField3" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" />
                                        <ext:TextField ID="TextField4" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" />
                                        <ext:TextField ID="TextField5" runat="server" Width="100" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" />
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

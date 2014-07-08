<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umYearBudget.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.umYearBudget" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .activeBackground {
            background-color: darkgreen;
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
            if (e.originalValue == null || e.record.data.ADJUSTMENT == "Overhead") {
                return false;
            }
        }
        var editAdjustment = function (editor, e) {
            SaveRecord.deSaveAdjustments(e.record.data.ADJ_ID, e.field, e.value);
        }
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
                            <Listeners>
                                <Activate Handler="#{uxActionsStore}.store.reload();" />
                            </Listeners>
                            <DirectEvents>
                                <Select OnEvent="deChooseSummaryAction">
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
                                <ext:Store ID="uxSummaryReportsStore" runat="server">
                                    <%--OnReadData="deLoadReports" AutoLoad="false">--%>
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
                            <Listeners>
                                <Activate Handler="#{uxSummaryReportsStore}.store.reload();" />
                            </Listeners>
                            <%--<DirectEvents>
                                <Select OnEvent="deChooseReport">
                                </Select>
                            </DirectEvents>--%>
                        </ext:ComboBox>

                        <ext:Label ID="uxSpace2" runat="server" Width="5" />

                        <ext:Button ID="uxUpdateAllActuals" runat="server" Text="Update All Actuals" Icon="BookEdit" />
                    </Items>
                </ext:Toolbar>


                <%-------------------------------------------------- Top Summary Panel --------------------------------------------------%>
                <ext:GridPanel ID="uxSummaryGrid" runat="server" Region="North" Flex="4">
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
                            <ext:Column ID="Column1" runat="server" DataIndex="PROJECT_NAME" Text="Project Name" Flex="6" />
                            <ext:Column ID="Column2" runat="server" DataIndex="STATUS" Text="Status" Flex="2" />
                            <ext:NumberColumn ID="Column3" runat="server" DataIndex="ACRES" Text="Acres" Flex="1" Align="Right" />
                            <ext:NumberColumn ID="Column4" runat="server" DataIndex="DAYS" Text="Days" Flex="1" Align="Right" />
                            <ext:NumberColumn ID="Column5" runat="server" DataIndex="GROSS_REC" Text="Gross Receipts" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="Column6" runat="server" DataIndex="MAT_USAGE" Text="Material Usage" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="Column7" runat="server" DataIndex="GROSS_REV" Text="Gross Revenue" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="Column8" runat="server" DataIndex="DIR_EXP" Text="Direct Expenses" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="Column9" runat="server" DataIndex="OP" Text="OP" Flex="2" Align="Right" />
                            <ext:Column ID="Column10" runat="server" DataIndex="OP_PERC" Text="OP %" Flex="2" Align="Right">
                                <Renderer Fn="Ext.util.Format.numberRenderer('0,000.00 %')" />
                            </ext:Column>
                            <ext:NumberColumn ID="Column11" runat="server" DataIndex="OP_VAR" Text="OP +/-" Flex="2" Align="Right" />
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
                        </Select>
                        <ItemDblClick OnEvent="deAllowFormEditing">
                            <ExtraParams>
                                <ext:Parameter Name="BudBidProjectID" Value="#{uxSummaryGrid}.getSelectionModel().getSelection()[0].data.BUD_BID_PROJECTS_ID" Mode="Raw" />
                            </ExtraParams>
                        </ItemDblClick>
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
                                <ext:DisplayField ID="uxIOPPerc" runat="server" Text="0.00 %" Flex="2" FieldStyle="text-align:right" Cls="inactiveForeground" />
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
                                <ext:DisplayField ID="uxAOPPerc" runat="server" Text="0.00 %" Flex="2" FieldStyle="text-align:right" Cls="activeForeground" />
                                <ext:DisplayField ID="uxAOPPlusMinus" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="activeForeground" />
                                <ext:DisplayField ID="DisplayField26" runat="server" Width="20" />
                            </Items>
                        </ext:FieldContainer>
                    </DockedItems>
                </ext:GridPanel>

                <ext:GridPanel ID="uxAdjustmentsGrid" runat="server" Region="North" HideHeaders="true" Flex="2">
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
                                        <ext:ModelField Name="MAT_ADJ" />
                                        <ext:ModelField Name="WEATHER_ADJ" />
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
                            <ext:Column ID="Column15" runat="server" DataIndex="ADJUSTMENT" Text="Project Name" Flex="6" />
                            <ext:Column ID="Column21" runat="server" DataIndex="STATUS" Text="Status" Flex="2" />
                            <ext:NumberColumn ID="Column22" runat="server" DataIndex="ACRES" Text="Acres" Flex="1" Align="Right" />
                            <ext:NumberColumn ID="Column23" runat="server" DataIndex="DAYS" Text="Days" Flex="1" Align="Right" />
                            <ext:NumberColumn ID="Column24" runat="server" DataIndex="GROSS_REC" Text="Gross Receipts" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="Column25" runat="server" DataIndex="MAT_ADJ" Text="Material Usage" Flex="2" Align="Right">
                                <Editor>
                                    <ext:NumberField ID="NumberField2" runat="server" AllowBlank="false" />
                                </Editor>
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="Column26" runat="server" DataIndex="GROSS_REV" Text="Gross Revenue" Flex="2" Align="Right" />
                            <ext:NumberColumn ID="Column27" runat="server" DataIndex="WEATHER_ADJ" Text="Direct Expenses" Flex="2" Align="Right">
                                <Editor>
                                    <ext:NumberField ID="NumberField1" runat="server" AllowBlank="false" />
                                </Editor>
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="Column28" runat="server" DataIndex="OP" Text="OP" Flex="2" Align="Right" />
                            <ext:Column ID="Column29" runat="server" DataIndex="OP_PERC" Text="OP %" Flex="2" Align="Right">
                                <Renderer Fn="Ext.util.Format.numberRenderer('0,000.00 %')" />
                            </ext:Column>
                            <ext:NumberColumn ID="Column30" runat="server" DataIndex="OP_VAR" Text="OP +/-" Flex="2" Align="Right" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:CellEditing ID="CellEditing1" runat="server">
                            <Listeners>
                                <%--new new new--%>
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
                                <ext:DisplayField ID="DisplayField22" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField23" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField25" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField27" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField28" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField29" runat="server" Text="0.00 %" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField30" runat="server" Text="0.00" Flex="2" FieldStyle="text-align:right" Cls="grandTotalForeground" />
                                <ext:DisplayField ID="DisplayField31" runat="server" Width="20" />
                            </Items>
                        </ext:FieldContainer>
                    </DockedItems>
                </ext:GridPanel>


                <%-------------------------------------------------- Bottom Form Panel --------------------------------------------------%>
                <ext:FormPanel ID="uxProjectDetail"
                    runat="server"
                    Region="Center"
                    Flex="10"
                    AutoScroll="true"
                    BodyPadding="20"
                    Disabled="true">
                    <Items>
                        <ext:FieldContainer ID="FieldContainer3"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label1" runat="server" Width="100" Text="Project Number:" />
                                <ext:DropDownField ID="uxProjectNum" runat="server" Width="110" Mode="ValueText" Editable="false">
                                    <Listeners>
                                        <Expand Handler="this.picker.setWidth(500);" />
                                    </Listeners>
                                    <Component>
                                        <ext:GridPanel runat="server"
                                            ID="uxProjectInfo"
                                            Width="500"
                                            Layout="HBoxLayout"
                                            Frame="true"
                                            ForceFit="true">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxProjectNumStore"
                                                    PageSize="10"
                                                    RemoteSort="true"
                                                    OnReadData="deLoadOrgProjects">
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
                                                        <ext:Parameter Name="ProjectID" Value="#{uxProjectInfo}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
                                                        <ext:Parameter Name="ProjectNum" Value="#{uxProjectInfo}.getSelectionModel().getSelection()[0].data.PROJECT_NUM" Mode="Raw" />
                                                        <ext:Parameter Name="ProjectName" Value="#{uxProjectInfo}.getSelectionModel().getSelection()[0].data.PROJECT_NAME" Mode="Raw" />
                                                        <ext:Parameter Name="Type" Value="#{uxProjectInfo}.getSelectionModel().getSelection()[0].data.TYPE" Mode="Raw" />
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
                                <ext:Label ID="Label5" runat="server" Width="320" />

                                <ext:Checkbox ID="uxCompareOverride" runat="server" BoxLabel="Compare to Override" Width="200">
                                    <DirectEvents>
                                        <%--<Change OnEvent="deLiabilityCheck" />--%>
                                    </DirectEvents>
                                </ext:Checkbox>
                                <ext:Label ID="Label6" runat="server" Width="50" />
                                <ext:Label ID="Label4" runat="server" Width="40" Text="Acres:" />
                                <ext:TextField ID="uxAcres" runat="server" Width="110" ReadOnly="false" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign">
                                    <Listeners>
                                        <Focus Handler="this.setValue(this.getValue().replace(',', ''));" />
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
                                <ext:Label ID="Label7" runat="server" Width="100" Text="Project Name:" />
                                <ext:TextField ID="uxProjectName" runat="server" Width="400" ReadOnly="true">
                                    <DirectEvents>
                                        <Change OnEvent="deCheckAllowSave" />
                                    </DirectEvents>
                                </ext:TextField>
                                <ext:Label ID="Label8" runat="server" Width="30" />
                                <ext:Label ID="Label9" runat="server" Width="100" Text="Final Draft OP:" />
                                <ext:TextField ID="uxCompareOP" runat="server" Width="110" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign">
                                    <Listeners>
                                        <Focus Handler="this.setValue(this.getValue().replace(',', ''));" />
                                    </Listeners>
                                    <DirectEvents>
                                        <Blur OnEvent="deFormatNumber" />
                                    </DirectEvents>
                                </ext:TextField>
                                <ext:Label ID="Label10" runat="server" Width="40" />
                                <ext:Label ID="Label11" runat="server" Width="40" Text="Days:" />
                                <ext:TextField ID="uxDays" runat="server" Width="110" ReadOnly="false" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign">
                                    <Listeners>
                                        <Focus Handler="this.setValue(this.getValue().replace(',', ''));" />
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
                                <ext:Label ID="Label13" runat="server" Width="100" Text="Status:" />
                                <ext:ComboBox ID="uxStatus"
                                    runat="server"
                                    ValueField="ID"
                                    DisplayField="ID_NAME"
                                    Width="110"
                                    EmptyText="-- Select --"
                                    Editable="false">
                                    <Store>
                                        <ext:Store ID="uxStatusStore" runat="server" OnReadData="deLoadStatuses" AutoLoad="false">
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
                                    <Listeners>
                                        <Activate Handler="#{uxStatusStore}.store.reload();" />
                                    </Listeners>
                                    <DirectEvents>
                                        <Select OnEvent="deSelectStatus" />
                                        <Select OnEvent="deCheckAllowSave" />
                                    </DirectEvents>
                                </ext:ComboBox>
                                <ext:Label ID="Label14" runat="server" Width="320" />
                                <ext:Label ID="Label15" runat="server" Width="100" Text="Variance:" />
                                <ext:Label ID="uxCompareVar" runat="server" Width="106" Text="0.00" Cls="labelRightAlign" />
                                <ext:Label ID="Label2" runat="server" Width="194" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer6"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label18" runat="server" Width="100" Text="Comments:" />
                                <ext:TextArea ID="uxComments" runat="server" Width="550" />
                                <ext:Label ID="Label20" runat="server" Width="60" Text="" />
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
                                                <ext:TextField ID="uxLiabilityAmount" runat="server" Width="110" ReadOnly="false" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign" Disabled="true">
                                                    <Listeners>
                                                        <Focus Handler="this.setValue(this.getValue().replace(',', ''));" />
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
                                                <ext:TextField ID="uxAppType" runat="server" Width="135" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer ID="FieldContainer9"
                                            runat="server"
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:Label ID="Label23" runat="server" Width="85" Text="Chemical Mix:" />
                                                <ext:TextField ID="uxChemMix" runat="server" Width="135" />
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
                        <ext:FieldSet runat="server" Width="930" Padding="10" Cls="detailBackground">
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
                                                <ext:Store ID="uxJCDateStore" runat="server" OnReadData="deLoadJCDates" AutoLoad="false">
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
                                            <Listeners>
                                                <Activate Handler="#{uxJCDateStore}.store.reload();" />
                                            </Listeners>
                                            <DirectEvents>
                                                <Select OnEvent="deSelectJCDate">
                                                    <EventMask ShowMask="true" />
                                                </Select>
                                            </DirectEvents>
                                        </ext:ComboBox>
                                        <ext:Label ID="Label34" runat="server" Width="50" />
                                        <ext:TextField ID="uxSGrossRec" runat="server" Width="110" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign">
                                            <Listeners>
                                                <Focus Handler="this.setValue(this.getValue().replace(',', ''));" />
                                            </Listeners>
                                            <DirectEvents>
                                                <Blur OnEvent="deFormatNumber" />
                                                <Blur OnEvent="deCalcGRandOP" />
                                            </DirectEvents>
                                        </ext:TextField>
                                        <ext:TextField ID="uxSMatUsage" runat="server" Width="110" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign">
                                            <Listeners>
                                                <Focus Handler="this.setValue(this.getValue().replace(',', ''));" />
                                            </Listeners>
                                            <DirectEvents>
                                                <Blur OnEvent="deFormatNumber" />
                                                <Blur OnEvent="deCalcGRandOP" />
                                            </DirectEvents>
                                        </ext:TextField>
                                        <ext:TextField ID="uxSGrossRev" runat="server" Width="110" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign">
                                            <Listeners>
                                                <Focus Handler="this.setValue(this.getValue().replace(',', ''));" />
                                            </Listeners>
                                        </ext:TextField>
                                        <ext:TextField ID="uxSDirects" runat="server" Width="110" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign">
                                            <Listeners>
                                                <Focus Handler="this.setValue(this.getValue().replace(',', ''));" />
                                            </Listeners>
                                            <DirectEvents>
                                                <Blur OnEvent="deFormatNumber" />
                                                <Blur OnEvent="deCalcGRandOP" />
                                            </DirectEvents>
                                        </ext:TextField>
                                        <ext:TextField ID="uxSOP" runat="server" Width="110" ReadOnly="true" Text="0.00" MaskRe="/[0-9\.\-]/" Cls="textRightAlign">
                                            <Listeners>
                                                <Focus Handler="this.setValue(this.getValue().replace(',', ''));" />
                                            </Listeners>
                                        </ext:TextField>
                                        <ext:Label ID="Label35" runat="server" Width="40" />
                                        <ext:ComboBox ID="uxSummaryActions"
                                            runat="server"
                                            DisplayField="name"
                                            Width="150"
                                            EmptyText="-- Actions --"
                                            QueryMode="Local"
                                            TypeAhead="true">
                                            <Store>
                                                <ext:Store ID="Store3" runat="server" AutoDataBind="true">
                                                    <Model>
                                                        <ext:Model ID="Model3" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="abbr" />
                                                                <ext:ModelField Name="name" />
                                                                <ext:ModelField Name="slogan" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                    <Reader>
                                                        <ext:ArrayReader />
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
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
                                                    AutoDataBind="true" WarningOnDirty="false">
                                                    <Model>
                                                        <ext:Model ID="Model2" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DETAIL_SHEET_ID" />
                                                                <ext:ModelField Name="SHEET_NAME" />
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
                                                    <ext:Column ID="Column12" runat="server" DataIndex="SHEET_NAME" Text="Detail Sheet" Width="160" />
                                                    <ext:NumberColumn ID="Column16" runat="server" DataIndex="GROSS_REC" Text="Gross Receipts" Width="110" Align="Right" />
                                                    <ext:NumberColumn ID="Column17" runat="server" DataIndex="MAT_USAGE" Text="Material Usage" Width="110" Align="Right" />
                                                    <ext:NumberColumn ID="Column18" runat="server" DataIndex="GROSS_REV" Text="Gross Revenue" Width="110" Align="Right" />
                                                    <ext:NumberColumn ID="Column19" runat="server" DataIndex="DIR_EXP" Text="Direct Expenses" Width="110" Align="Right" />
                                                    <ext:NumberColumn ID="Column20" runat="server" DataIndex="OP" Text="OP" Width="110" Align="Right" />
                                                </Columns>
                                            </ColumnModel>
                                            <DirectEvents>
                                                <ItemDblClick OnEvent="Test">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="SheetName" Value="#{GridPanel1}.getSelectionModel().getSelection()[0].data.SHEET_NAME" Mode="Raw" />
                                                    </ExtraParams>
                                                </ItemDblClick>
                                            </DirectEvents>
                                        </ext:GridPanel>
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer15"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label32" runat="server" Width="160" />
                                        <ext:TextField ID="uxEGrossRec" runat="server" Width="110" ReadOnly="true" Text="0.00" Cls="textRightAlign" />
                                        <ext:TextField ID="uxEMatUsage" runat="server" Width="110" ReadOnly="true" Text="0.00" Cls="textRightAlign" />
                                        <ext:TextField ID="uxEGrossRev" runat="server" Width="110" ReadOnly="true" Text="0.00" Cls="textRightAlign" />
                                        <ext:TextField ID="uxEDirects" runat="server" Width="110" ReadOnly="true" Text="0.00" Cls="textRightAlign" />
                                        <ext:TextField ID="uxEOP" runat="server" Width="110" ReadOnly="true" Text="0.00" Cls="textRightAlign" />
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
                                        <Click OnEvent="deSave" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Label ID="Label16" runat="server" Width="5" />
                                <ext:Button ID="uxCancel" runat="server" Text="Cancel" Icon="Delete" Width="75">
                                    <DirectEvents>
                                        <Click OnEvent="deCancel" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                </ext:FormPanel>


                <%-------------------------------------------------- Diagnostic Panel --------------------------------------------------%>
                <%-- Uncomment to Use --%>
                <ext:FormPanel ID="uxDiagnostic"
                    runat="server"
                    Region="South"
                    Flex="2"
                    AutoScroll="true"
                    BodyPadding="20">
                    <Items>
                        <ext:FieldContainer ID="FieldContainer2"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label19" runat="server" Width="100" Text="uxHidNewProject" Cls="labelCenterAlign" />
                                <ext:Label ID="Label22" runat="server" Width="100" Text="uxHidBudBidID" Cls="labelCenterAlign" />
                                <ext:Label ID="Label24" runat="server" Width="100" Text="uxHidProjectNumID" Cls="labelCenterAlign" />
                                <ext:Label ID="Label38" runat="server" Width="100" Text="uxHidType" Cls="labelCenterAlign" />
                                <ext:Label ID="Label40" runat="server" Width="100" Text="uxHidStatusID" Cls="labelCenterAlign" />
                                <ext:Label ID="Label33" runat="server" Width="100" Text="uxHidPrevYear" Cls="labelCenterAlign" />
                                <ext:Label ID="Label41" runat="server" Width="100" Text="uxHidPrevVer" Cls="labelCenterAlign" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer1"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:TextField ID="uxHidNewProject" runat="server" Width="100" />
                                <ext:TextField ID="uxHidBudBidID" runat="server" Width="100" />
                                <ext:TextField ID="uxHidProjectNumID" runat="server" Width="100" />
                                <ext:TextField ID="uxHidType" runat="server" Width="100" />
                                <ext:TextField ID="uxHidStatusID" runat="server" Width="100" />
                                <ext:TextField ID="uxHidPrevYear" runat="server" Width="100" />
                                <ext:TextField ID="uxHidPrevVer" runat="server" Width="100" />
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                </ext:FormPanel>
                <%-- Uncomment to Use --%>

                <%-- Comment to Use --%>
                <%--<ext:Hidden ID="uxHidNewProject" runat="server" />
                <ext:Hidden ID="uxHidBudBidID" runat="server" />
                <ext:Hidden ID="uxHidProjectNumID" runat="server" />
                <ext:Hidden ID="uxHidType" runat="server" />
                <ext:Hidden ID="uxHidStatusID" runat="server" />
                <ext:Hidden ID="uxHidPrevYear" runat="server" />
                <ext:Hidden ID="uxHidPrevVer" runat="server" />--%>
                <%-- Comment to Use --%>
                <%-------------------------------------------------- Diagnostic Panel --------------------------------------------------%>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

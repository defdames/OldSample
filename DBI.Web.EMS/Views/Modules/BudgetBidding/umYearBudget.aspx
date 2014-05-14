<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umYearBudget.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.umYearBudget" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .activeBackground .x-form-item-body {
            background-color:  darkgreen;
        }

        .activeForeground .x-form-display-field {
            font-weight: bold;
            color: whitesmoke;
        }

        .inactiveForeground .x-form-display-field {
            font-weight: bold;
            color: whitesmoke;
        }

        .inactiveBackground .x-form-item-body {
            background-color:  black;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>


                <%-------------------------------------------------- Top panel --------------------------------------------------%>
                <ext:GridPanel ID="uxSummaryGrid" runat="server" Region="North" Flex="2">
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="false" Mode="Single" />
                    </SelectionModel>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxSummaryGridStore"
                            OnReadData="deReadSummaryGridData"
                            AutoDataBind="true" WarningOnDirty="false">
                            <Model>
                                <ext:Model ID="Model1" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="PROJ_ID" />
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
                            <ext:Column ID="Column3" runat="server" DataIndex="ACRES" Text="Acres" Flex="1" Align="Right" />
                            <ext:Column ID="Column4" runat="server" DataIndex="DAYS" Text="Days" Flex="1" Align="Right" />
                            <ext:Column ID="Column5" runat="server" DataIndex="GROSS_REC" Text="Gross Receipts" Flex="2" Align="Right" />
                            <ext:Column ID="Column6" runat="server" DataIndex="MAT_USAGE" Text="Material Usage" Flex="2" Align="Right" />
                            <ext:Column ID="Column7" runat="server" DataIndex="GROSS_REV" Text="Gross Revenue" Flex="2" Align="Right" />
                            <ext:Column ID="Column8" runat="server" DataIndex="DIR_EXP" Text="Direct Expenses" Flex="2" Align="Right" />
                            <ext:Column ID="Column9" runat="server" DataIndex="OP" Text="OP" Flex="2" Align="Right" />
                            <ext:Column ID="Column10" runat="server" DataIndex="OP_PERC" Text="OP %" Flex="2" Align="Right" />
                            <ext:Column ID="Column11" runat="server" DataIndex="OP_VAR" Text="OP +/-" Flex="2" Align="Right" />
                        </Columns>
                    </ColumnModel>
                    <DirectEvents>
                        <Select OnEvent="GetFormData">
                            <ExtraParams>
                                <ext:Parameter Name="CrossingId" Value="#{uxSummaryGrid}.getSelectionModel().getSelection()[0].data.PROJECT_NAME" Mode="Raw" />
                            </ExtraParams>
                        </Select>
                    </DirectEvents>
                    <DockedItems>
                        <ext:FieldContainer ID="Container1" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="inactiveBackground">
                            <Items>
                                <ext:DisplayField ID="DisplayField1" runat="server" Width="10" />
                                <ext:DisplayField ID="DisplayField2" runat="server" Text="Total Inactive" Flex="6" Cls="inactiveForeground" />
                                <ext:DisplayField ID="DisplayField3" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField4" runat="server" Text="" Flex="1" />
                                <ext:DisplayField ID="DisplayField5" runat="server" Text="" Flex="1" />
                                <ext:DisplayField ID="DisplayField6" runat="server" Text="$3,000.00" Flex="2" FieldStyle="text-align:right" Cls="inactiveForeground" />
                                <ext:DisplayField ID="DisplayField7" runat="server" Text="$3,000.00" Flex="2" FieldStyle="text-align:right" Cls="inactiveForeground" />
                                <ext:DisplayField ID="DisplayField8" runat="server" Text="$3,000.00" Flex="2" FieldStyle="text-align:right" Cls="inactiveForeground" />
                                <ext:DisplayField ID="DisplayField9" runat="server" Text="$3,000.00" Flex="2" FieldStyle="text-align:right" Cls="inactiveForeground" />
                                <ext:DisplayField ID="DisplayField10" runat="server" Text="$3,000.00" Flex="2" FieldStyle="text-align:right" Cls="inactiveForeground" />
                                <ext:DisplayField ID="DisplayField11" runat="server" Text="10.5%" Flex="2" FieldStyle="text-align:right" Cls="inactiveForeground" />
                                <ext:DisplayField ID="DisplayField12" runat="server" Text="$3,000.00" Flex="2" FieldStyle="text-align:right" Cls="inactiveForeground" />
                                <ext:DisplayField ID="DisplayField13" runat="server" Width="20" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer15" runat="server" Layout="HBoxLayout" Dock="Bottom" Cls="activeBackground">
                            <Items>
                                <ext:DisplayField ID="DisplayField14" runat="server" Width="10" />
                                <ext:DisplayField ID="DisplayField15" runat="server" Text="Total Active" Flex="6" Cls="activeForeground" />
                                <ext:DisplayField ID="DisplayField16" runat="server" Text="" Flex="2" />
                                <ext:DisplayField ID="DisplayField17" runat="server" Text="" Flex="1" />
                                <ext:DisplayField ID="DisplayField18" runat="server" Text="" Flex="1" />
                                <ext:DisplayField ID="DisplayField19" runat="server" Text="$3,000.00" Flex="2" FieldStyle="text-align:right" Cls="activeForeground" />
                                <ext:DisplayField ID="DisplayField20" runat="server" Text="$3,000.00" Flex="2" FieldStyle="text-align:right" Cls="activeForeground" />
                                <ext:DisplayField ID="DisplayField21" runat="server" Text="$3,000.00" Flex="2" FieldStyle="text-align:right" Cls="activeForeground" />
                                <ext:DisplayField ID="DisplayField22" runat="server" Text="$3,000.00" Flex="2" FieldStyle="text-align:right" Cls="activeForeground" />
                                <ext:DisplayField ID="DisplayField23" runat="server" Text="$3,000.00" Flex="2" FieldStyle="text-align:right" Cls="activeForeground" />
                                <ext:DisplayField ID="DisplayField24" runat="server" Text="10.5%" Flex="2" FieldStyle="text-align:right" Cls="activeForeground" />
                                <ext:DisplayField ID="DisplayField25" runat="server" Text="$3,000.00" Flex="2" FieldStyle="text-align:right" Cls="activeForeground" />
                                <ext:DisplayField ID="DisplayField26" runat="server" Width="20" />
                            </Items>
                        </ext:FieldContainer>
                    </DockedItems>
                </ext:GridPanel>


                <%-------------------------------------------------- Bottom panel --------------------------------------------------%>
                <ext:FormPanel ID="FormPanel1"
                    runat="server"
                    Region="Center"
                    Flex="4"
                    BodyPadding="20">
                    <Items>
                        <ext:FieldContainer ID="FieldContainer12"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label1" runat="server" Width="100" Text="Project Number:" />
                                <ext:ComboBox ID="uxProjectNum" runat="server" Width="100" />
                                <ext:Label ID="Label5" runat="server" Width="250" />

                                <ext:Label ID="Label2" runat="server" Width="170" Text="Compare to Project Number:" />
                                <ext:ComboBox ID="ComboBox1" runat="server" Width="100" />
                                <ext:Label ID="Label6" runat="server" Width="50" />

                                <ext:Label ID="Label4" runat="server" Width="40" Text="Acres:" />
                                <ext:TextField ID="TextField33" runat="server" Width="50" />
                                <ext:Label ID="Label3" runat="server" Width="1" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer1"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label7" runat="server" Width="100" Text="Project Name:" />
                                <ext:TextField ID="TextField34" runat="server" Width="300" />
                                <ext:Label ID="Label8" runat="server" Width="50" />

                                <ext:Label ID="Label9" runat="server" Width="170" Text="Final Draft OP:" />
                                <ext:TextField ID="TextField2" runat="server" Width="100" />
                                <ext:Label ID="Label10" runat="server" Width="50" />

                                <ext:Label ID="Label11" runat="server" Width="40" Text="Days:" />
                                <ext:TextField ID="TextField1" runat="server" Width="50" />
                                <ext:Label ID="Label12" runat="server" Width="1" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer2"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label13" runat="server" Width="100" Text="Status:" />
                                <ext:ComboBox ID="ComboBox3" runat="server" Width="100" />
                                <ext:Label ID="Label14" runat="server" Width="250" />

                                <ext:Label ID="Label15" runat="server" Width="170" Text="Variance:" />
                                <ext:Label ID="Label19" runat="server" Width="100" Text="$12,000.00" />
                                <ext:Label ID="Label16" runat="server" Width="30" />

                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer3"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label18" runat="server" Width="100" Text="Comments:" />
                                <ext:TextArea ID="TextArea1" runat="server" Width="550" />
                                <ext:Label ID="Label20" runat="server" Width="60" Text="" />

                                <ext:FieldContainer ID="FieldContainer13"
                                    runat="server"
                                    Layout="VBoxLayout">
                                    <Items>
                                        <ext:FieldContainer ID="FieldContainer4"
                                            runat="server"
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:Label ID="Label17" runat="server" Width="85" Text="App Type:" />
                                                <ext:TextField ID="TextField3" runat="server" Width="100" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer ID="FieldContainer14"
                                            runat="server"
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:Label ID="Label23" runat="server" Width="85" Text="Chemical Mix:" />
                                                <ext:TextField ID="TextField5" runat="server" Width="100" />
                                            </Items>
                                        </ext:FieldContainer>



                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer6"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label33" runat="server" Width="60" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer10"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label36" runat="server" Width="60" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldSet runat="server" Width="895" Padding="10">
                            <Items>





                                <ext:FieldContainer ID="FieldContainer5"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label25" runat="server" Width="100" Text="Actuals Through" />
                                        <ext:Label ID="Label29" runat="server" Width="60" />
                                        <ext:Label ID="Label26" runat="server" Width="100" Text="Gross Receipts" />
                                        <ext:Label ID="Label27" runat="server" Width="100" Text="Material Usage" />
                                        <ext:Label ID="Label30" runat="server" Width="100" Text="Gross Revenue" />
                                        <ext:Label ID="Label28" runat="server" Width="100" Text="Direct Expenses" />
                                        <ext:Label ID="Label31" runat="server" Width="100" Text="OP" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer7"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField ID="DateField2" runat="server" Width="100" />
                                        <ext:Label ID="Label34" runat="server" Width="60" />
                                        <ext:TextField ID="TextField4" runat="server" Width="100" ReadOnly="true" />
                                        <ext:TextField ID="TextField6" runat="server" Width="100" />
                                        <ext:TextField ID="TextField7" runat="server" Width="100" />
                                        <ext:TextField ID="TextField8" runat="server" Width="100" />
                                        <ext:TextField ID="TextField9" runat="server" Width="100" />
                                        <ext:Label ID="Label35" runat="server" Width="60" />
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

                                <ext:FieldContainer ID="FieldContainer9"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:GridPanel ID="GridPanel1" runat="server" Width="680" HideHeaders="true" Height="70">
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
                                                    <ext:Column ID="Column16" runat="server" DataIndex="GROSS_REC" Text="Gross Receipts" Width="100" Align="Right" />
                                                    <ext:Column ID="Column17" runat="server" DataIndex="MAT_USAGE" Text="Material Usage" Width="100" Align="Right" />
                                                    <ext:Column ID="Column18" runat="server" DataIndex="GROSS_REV" Text="Gross Revenue" Width="100" Align="Right" />
                                                    <ext:Column ID="Column19" runat="server" DataIndex="DIR_EXP" Text="Direct Expenses" Width="100" Align="Right" />
                                                    <ext:Column ID="Column20" runat="server" DataIndex="OP" Text="OP" Width="100" Align="Right" />
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

                                <ext:FieldContainer ID="FieldContainer8"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label32" runat="server" Width="160" />
                                        <ext:TextField ID="TextField10" runat="server" Width="100" />
                                        <ext:TextField ID="TextField11" runat="server" Width="100" />
                                        <ext:TextField ID="TextField12" runat="server" Width="100" />
                                        <ext:TextField ID="TextField13" runat="server" Width="100" />
                                        <ext:TextField ID="TextField14" runat="server" Width="100" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer ID="FieldContainer11"
                                    runat="server"
                                    Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label37" runat="server" Width="60" />
                                    </Items>
                                </ext:FieldContainer>

                            </Items>
                        </ext:FieldSet>

                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

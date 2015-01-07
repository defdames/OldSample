<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddOverheadDetailLine.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umEdmitMonthRow" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        Ext.util.Format.CurrencyFactory = function (dp, dSeparator, tSeparator, symbol) {
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

        var beforeCellEditHandler = function (e) {
            if (e.record.data.ACTUALS_IMPORTED_FLAG == "Y") {
                return false;
            }
        }

        var getRowClass = function (record, rowIndex, rowParams, store) {
            if (record.data.ACTUALS_IMPORTED_FLAG == "Y") {
                return "red-row";
            }
        }

    </script>

    <style>
        .x-grid-row-summary .x-grid-cell-inner {
            font-weight: bold;
            font-size: 11px;
            background-color: #E0E0D1;
        }

        .red-row .x-grid-cell, .red-row .x-grid-rowwrap-div .red-row .myBoldClass.x-grid3-row td {
            color: red !important;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">

        <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" Namespace="App" ClientIDMode="Static">
        </ext:ResourceManager>
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>

                <ext:Toolbar ID="Toolbar1" runat="server" Region="North" Padding="5" Margins="5 5 5 5">
                    <Items>
                        <ext:ComboBox ID="uxBudgetsOrActuals"
                            runat="server"
                            ValueField="ID_NAME"
                            DisplayField="ID_NAME"
                            Width="120"
                            Editable="false">
                            <Store>
                                <ext:Store ID="uxBudgetsOrActualsStore" runat="server" OnReadData="deLoadBudgetsOrActuals" AutoLoad="false">
                                    <Model>
                                        <ext:Model ID="Model1" runat="server">
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
                                <Select OnEvent="deSelectBudgetsOrActuals">
                                </Select>
                            </DirectEvents>
                        </ext:ComboBox>
                    </Items>
                </ext:Toolbar>
                <ext:GridPanel ID="GridPanel3" runat="server" Flex="1" Title=" " Header="true" Padding="5" Region="Center" Frame="true" Margins="5 5 5 0">
                    <KeyMap ID="KeyMap1" runat="server">
                        <Binding>
                            <ext:KeyBinding Handler="#{uxSaveDetailLineButton}.fireEvent('click');">
                                <Keys>
                                    <ext:Key Code="ENTER" />
                                    <ext:Key Code="RETURN" />
                                </Keys>
                            </ext:KeyBinding>
                        </Binding>
                    </KeyMap>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxDetailStore"
                            AutoDataBind="true" RemoteSort="true" OnReadData="deLoadDetailLinesStore" AutoLoad="true">
                            <Model>
                                <ext:Model ID="Model4" runat="server" IDProperty="BUDGET_DETAIL_ID">
                                    <Fields>
                                        <ext:ModelField Name="ID_FIELD"></ext:ModelField>
                                        <ext:ModelField Name="PROJECT_ID"></ext:ModelField>
                                        <ext:ModelField Name="DETAIL_TASK_ID"></ext:ModelField>
                                        <ext:ModelField Name="LINE_ID"></ext:ModelField>
                                        <ext:ModelField Name="MONTH"></ext:ModelField>
                                        <ext:ModelField Name="AMOUNT"></ext:ModelField>
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Listeners>
                                <Load Handler="#{uxGridEditor}.startEdit(0, 1);" Delay="250" />
                            </Listeners>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column116" runat="server" DataIndex="MONTH" Text="Month" Flex="1">
                                <SummaryRenderer Handler="return ('Total');" />
                            </ext:Column>
                            <ext:Column ID="Column117" runat="server" DataIndex="AMOUNT" Text="Amount" Flex="1" SummaryType="Sum" Align="Right">
                                <Editor>
                                    <ext:NumberField runat="server" AllowBlank="false" ID="uxEditAmount" SelectOnFocus="true" TabIndex="1" KeyNavEnabled="false" HideTrigger="true">
                                        <Listeners>
                                            <Show Handler="this.el.dom.select();" Delay="150" />
                                        </Listeners>
                                    </ext:NumberField>
                                </Editor>
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <View>
                        <ext:GridView ID="GridView4" StripeRows="true" runat="server" TrackOver="true">
                            <GetRowClass Fn="getRowClass"></GetRowClass>
                        </ext:GridView>
                    </View>

                    <Features>
                        <ext:Summary ID="Summary1" runat="server" />
                    </Features>
                    <Plugins>
                        <ext:CellEditing runat="server" ClicksToEdit="1" ID="uxGridEditor">
                            <Listeners>
                                <Edit Handler="#{uxSaveDetailLineButton}.focus();" />
                                <BeforeEdit Handler="return beforeCellEditHandler(e);"></BeforeEdit>
                            </Listeners>
                        </ext:CellEditing>

                    </Plugins>
                    <Buttons>
                        <ext:Button runat="server" ID="uxSaveDetailLineButton" Text="Save" Icon="Disk" AutoFocus="true" TabIndex="2">
                            <DirectEvents>
                                <Click OnEvent="deSaveDetailLine" Success="parent.Ext.getCmp('uxMonthDetail').getStore().load();parent.Ext.getCmp('uxDetailLineMaintenance').close();">
                                    <Confirmation ConfirmRequest="true" Message="Are you sure you want to save this detail line?"></Confirmation>
                                    <EventMask ShowMask="true"></EventMask>
                                    <ExtraParams>
                                        <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel3}.getRowsValues())" Mode="Raw" />
                                        <ext:Parameter Name="IDField" Value="#{GridPanel3}.getSelectionModel().getSelection()[0].data.ID_FIELD" Mode="Raw" />
                                        <ext:Parameter Name="Project" Value="#{GridPanel3}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
                                        <ext:Parameter Name="DetailTask" Value="#{GridPanel3}.getSelectionModel().getSelection()[0].data.DETAIL_TASK_ID" Mode="Raw" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="uxCloseDetailLineButton" Text="Cancel" Icon="Cancel">
                            <Listeners>
                                <Click Handler="parent.Ext.getCmp('uxDetailLineMaintenance').close();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:GridPanel>

            </Items>
        </ext:Viewport>
    </form>
</body>
</html>



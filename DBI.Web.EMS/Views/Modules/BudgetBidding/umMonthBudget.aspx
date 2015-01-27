<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umMonthBudget.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.umMonthBudget" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .activeBackground {
            background-color: darkolivegreen;
        }

        .inactiveBackground {
            background-color: lightgray;
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
        var colorTotalLineName = function (value, metadata, record) {
            if (record.data.TOTAL == "Y") {
                metadata.style = "background-color: #E0E0D1;";
            }
            return value;
        };
        var colorTotalLineNumber = function (value, metadata, record) {
            if (record.data.TOTAL == "Y") {
                metadata.style = "background-color: #E0E0D1;";
            }

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
        var colorOverride = function (value, metadata, record) {
            if (record.data.TYPE == "OVERRIDE") {
                metadata.style = "background-color: #FFFF99;";
            }
            else if (record.data.TYPE == "NA") {
                metadata.style = "font-weight: bold; background-color: #000000; color: white;";
            }
            return value;
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
                        <ext:ComboBox ID="uxSummaryReports"
                            runat="server"
                            ValueField="ID_NAME"
                            DisplayField="ID_NAME"
                            Width="253"
                            EmptyText="-- Reports/Export --"
                            Editable="false">
                            <Store>
                                <ext:Store ID="uxReportsStore" runat="server" OnReadData="deLoadSummaryReports" AutoLoad="false">
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
                                <Select OnEvent="deChooseSummaryReport">
                                    <EventMask ShowMask="true" Msg="Processing..." />
                                </Select>
                            </DirectEvents>
                        </ext:ComboBox>

                        <ext:Label ID="uxSpace2" runat="server" Width="5" />

                        <ext:Button ID="uxUpdateAllActuals" runat="server" Text="Update All Actuals" Icon="BookEdit">
                            <DirectEvents>
                                <Click OnEvent="deUpdateAllActuals">
                                    <EventMask ShowMask="true" Msg="Processing..." />
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>


                <%-------------------------------------------------- Top Panel --------------------------------------------------%>
                <ext:Panel ID="FieldContainer31"
                    runat="server"
                    Region="North"
                    Layout="HBoxLayout">
                    <Items>
                        <ext:GridPanel ID="uxProjects" runat="server" Flex="1" Height="180" HideHeaders="false">
                            <TopBar>

                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <ext:ToolbarFill />
                                        <ext:ComboBox ID="uxProjectActions"
                                            runat="server"
                                            ValueField="ID_NAME"
                                            DisplayField="ID_NAME"
                                            Width="200"
                                            EmptyText="-- Actions --"
                                            Editable="false">
                                            <Store>
                                                <ext:Store ID="uxProjectActionsStore" runat="server" OnReadData="deLoadProjectActions" AutoLoad="false">
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
                                                <Select OnEvent="deChooseProjectAction">
                                                    <EventMask ShowMask="true" Msg="Processing..." />
                                                </Select>
                                            </DirectEvents>
                                        </ext:ComboBox>
                                        <ext:Label ID="uxSpace1" runat="server" Width="10" />
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="rowSelectionProject" runat="server" AllowDeselect="false" Mode="Single" />
                            </SelectionModel>
                            <Store>
                                <ext:Store runat="server"
                                    ID="uxProjectsStore"
                                    OnReadData="deReadProjectGridData"
                                    AutoDataBind="true"
                                    WarningOnDirty="false">
                                    <Model>
                                        <ext:Model ID="Model2" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="BUD_BID_PROJECTS_ID" />
                                                <ext:ModelField Name="PROJECT_ID" />
                                                <ext:ModelField Name="PROJECT_NUM" />
                                                <ext:ModelField Name="PROJECT_NAME" />
                                                <ext:ModelField Name="TYPE" />
                                                <ext:ModelField Name="DISPLAY_PROJECT_NUM" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:PageProxy />
                                    </Proxy>
                                    <Listeners>
                                        <Load Handler="#{rowSelectionProject}.select(0)" />
                                    </Listeners>
                                </ext:Store>
                            </Store>
                            <ColumnModel>
                                <Columns>
                                    <ext:Column ID="Column12" runat="server" DataIndex="DISPLAY_PROJECT_NUM" Text="Project Number" Flex="1" />
                                    <ext:Column ID="Column4" runat="server" DataIndex="PROJECT_NAME" Text="Project Name" Flex="4">
                                        <Renderer Fn="colorOverride" />
                                    </ext:Column>
                                </Columns>
                            </ColumnModel>
                            <DirectEvents>
                                <Select OnEvent="deSelectProject">
                                    <ExtraParams>
                                        <ext:Parameter Name="BudBidProjectID" Value="#{uxProjects}.getSelectionModel().getSelection()[0].data.BUD_BID_PROJECTS_ID" Mode="Raw" />
                                        <ext:Parameter Name="ProjectID" Value="#{uxProjects}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
                                        <ext:Parameter Name="ProjectNum" Value="#{uxProjects}.getSelectionModel().getSelection()[0].data.PROJECT_NUM" Mode="Raw" />
                                        <ext:Parameter Name="ProjectName" Value="#{uxProjects}.getSelectionModel().getSelection()[0].data.PROJECT_NAME" Mode="Raw" />
                                        <ext:Parameter Name="Type" Value="#{uxProjects}.getSelectionModel().getSelection()[0].data.TYPE" Mode="Raw" />
                                    </ExtraParams>
                                    <EventMask ShowMask="true" />
                                </Select>
                            </DirectEvents>
                        </ext:GridPanel>

                        <ext:GridPanel ID="uxTasks" runat="server" Flex="1" Height="180" HideHeaders="false">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar2" runat="server">
                                    <Items>
                                        <ext:ToolbarFill />
                                        <ext:ComboBox ID="uxTaskActions"
                                            runat="server"
                                            ValueField="ID_NAME"
                                            DisplayField="ID_NAME"
                                            Width="200"
                                            EmptyText="-- Actions --"
                                            Editable="false">
                                            <Store>
                                                <ext:Store ID="uxTaskActionsStore" runat="server" OnReadData="deLoadTaskActions" AutoLoad="false">
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
                                                <Select OnEvent="deChooseTaskAction">
                                                    <EventMask ShowMask="true" Msg="Processing..." />
                                                </Select>
                                            </DirectEvents>
                                        </ext:ComboBox>
                                        <ext:Label ID="Label1" runat="server" Width="10" />
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="rowSelectionTask" runat="server" AllowDeselect="false" Mode="Single" />
                            </SelectionModel>
                            <Store>
                                <ext:Store runat="server"
                                    ID="uxTasksStore"
                                    OnReadData="deReadTaskGridData"
                                    AutoDataBind="true"
                                    WarningOnDirty="false"
                                    AutoLoad="false">
                                    <Model>
                                        <ext:Model ID="Model1" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="DETAIL_TASK_ID" />
                                                <ext:ModelField Name="DETAIL_ID" />
                                                <ext:ModelField Name="DETAIL_NUM" />
                                                <ext:ModelField Name="DESCRIPTION" />
                                                <ext:ModelField Name="TYPE" />
                                                <ext:ModelField Name="DISPLAY_TASK_NUM" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:PageProxy />
                                    </Proxy>
                                    <Listeners>
                                        <Load Handler="#{rowSelectionTask}.select(0)" />
                                    </Listeners>
                                </ext:Store>
                            </Store>
                            <ColumnModel>
                                <Columns>
                                    <ext:Column ID="Column1" runat="server" DataIndex="DISPLAY_TASK_NUM" Text="Task Number" Flex="1" />
                                    <ext:Column ID="Column3" runat="server" DataIndex="DESCRIPTION" Text="Task Name" Flex="4">
                                        <Renderer Fn="colorOverride" />
                                    </ext:Column>
                                </Columns>
                            </ColumnModel>
                            <DirectEvents>
                                <Select OnEvent="deSelectTask">
                                    <ExtraParams>
                                        <ext:Parameter Name="DetailTaskID" Value="#{uxTasks}.getSelectionModel().getSelection()[0].data.DETAIL_TASK_ID" Mode="Raw" />
                                        <ext:Parameter Name="DetailID" Value="#{uxTasks}.getSelectionModel().getSelection()[0].data.DETAIL_ID" Mode="Raw" />
                                        <ext:Parameter Name="DetailNum" Value="#{uxTasks}.getSelectionModel().getSelection()[0].data.DETAIL_NUM" Mode="Raw" />
                                        <ext:Parameter Name="DetailName" Value="#{uxTasks}.getSelectionModel().getSelection()[0].data.DESCRIPTION" Mode="Raw" />
                                        <ext:Parameter Name="Type" Value="#{uxTasks}.getSelectionModel().getSelection()[0].data.TYPE" Mode="Raw" />
                                    </ExtraParams>
                                    <EventMask ShowMask="true" />
                                </Select>
                            </DirectEvents>
                        </ext:GridPanel>
                    </Items>
                </ext:Panel>


                <%-------------------------------------------------- Middle Panel --------------------------------------------------%>
                <ext:GridPanel ID="uxMonthDetail" runat="server" HideHeaders="false" Region="Center">
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectionModel3" runat="server" AllowDeselect="false" Mode="Single" />
                    </SelectionModel>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxMonthDetailStore"
                            OnReadData="deReadMainGridData"
                            AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model3" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="LINE_ID" />
                                        <ext:ModelField Name="TOTAL" />
                                        <ext:ModelField Name="LINE_DESC" />
                                        <ext:ModelField Name="REFORECAST" />
                                        <ext:ModelField Name="NOV_BUDGET" />
                                        <ext:ModelField Name="NOV_ACTUAL" />
                                        <ext:ModelField Name="NOV_VAR" />
                                        <ext:ModelField Name="NOV_YTD" />
                                        <ext:ModelField Name="DEC_BUDGET" />
                                        <ext:ModelField Name="DEC_ACTUAL" />
                                        <ext:ModelField Name="DEC_VAR" />
                                        <ext:ModelField Name="DEC_YTD" />
                                        <ext:ModelField Name="JAN_BUDGET" />
                                        <ext:ModelField Name="JAN_ACTUAL" />
                                        <ext:ModelField Name="JAN_VAR" />
                                        <ext:ModelField Name="JAN_YTD" />
                                        <ext:ModelField Name="FEB_BUDGET" />
                                        <ext:ModelField Name="FEB_ACTUAL" />
                                        <ext:ModelField Name="FEB_VAR" />
                                        <ext:ModelField Name="FEB_YTD" />
                                        <ext:ModelField Name="MAR_BUDGET" />
                                        <ext:ModelField Name="MAR_ACTUAL" />
                                        <ext:ModelField Name="MAR_VAR" />
                                        <ext:ModelField Name="MAR_YTD" />
                                        <ext:ModelField Name="APR_BUDGET" />
                                        <ext:ModelField Name="APR_ACTUAL" />
                                        <ext:ModelField Name="APR_VAR" />
                                        <ext:ModelField Name="APR_YTD" />
                                        <ext:ModelField Name="MAY_BUDGET" />
                                        <ext:ModelField Name="MAY_ACTUAL" />
                                        <ext:ModelField Name="MAY_VAR" />
                                        <ext:ModelField Name="MAY_YTD" />
                                        <ext:ModelField Name="JUN_BUDGET" />
                                        <ext:ModelField Name="JUN_ACTUAL" />
                                        <ext:ModelField Name="JUN_VAR" />
                                        <ext:ModelField Name="JUN_YTD" />
                                        <ext:ModelField Name="JUL_BUDGET" />
                                        <ext:ModelField Name="JUL_ACTUAL" />
                                        <ext:ModelField Name="JUL_VAR" />
                                        <ext:ModelField Name="JUL_YTD" />
                                        <ext:ModelField Name="AUG_BUDGET" />
                                        <ext:ModelField Name="AUG_ACTUAL" />
                                        <ext:ModelField Name="AUG_VAR" />
                                        <ext:ModelField Name="AUG_YTD" />
                                        <ext:ModelField Name="SEP_BUDGET" />
                                        <ext:ModelField Name="SEP_ACTUAL" />
                                        <ext:ModelField Name="SEP_VAR" />
                                        <ext:ModelField Name="SEP_YTD" />
                                        <ext:ModelField Name="OCT_BUDGET" />
                                        <ext:ModelField Name="OCT_ACTUAL" />
                                        <ext:ModelField Name="OCT_VAR" />
                                        <ext:ModelField Name="OCT_YTD" />
                                        <ext:ModelField Name="TOTAL_PLAN" />
                                        <ext:ModelField Name="TOTAL_ACTUAL" />
                                        <ext:ModelField Name="TOTAL_VAR" />
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
                            <ext:Column ID="Column2" runat="server" DataIndex="LINE_DESC" Text="" Width="160" Locked="true">
                                <Renderer Fn="colorTotalLineName" />
                            </ext:Column>
                            <ext:NumberColumn ID="NumberColumn6" runat="server" DataIndex="REFORECAST" Text="Reforecast" Width="110" Align="Right" Locked="true">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn1" runat="server" DataIndex="NOV_BUDGET" Text="Nov - Budget" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn2" runat="server" DataIndex="NOV_ACTUAL" Text="Nov - Actual" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn3" runat="server" DataIndex="NOV_VAR" Text="Nov - Variance" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn4" runat="server" DataIndex="NOV_YTD" Text="Nov - YTD" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn5" runat="server" DataIndex="DEC_BUDGET" Text="Dec - Budget" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn7" runat="server" DataIndex="DEC_ACTUAL" Text="Dec - Actual" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn8" runat="server" DataIndex="DEC_VAR" Text="Dec - Variance" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn9" runat="server" DataIndex="DEC_YTD" Text="Dec - YTD" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn10" runat="server" DataIndex="JAN_BUDGET" Text="Jan - Budget" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn11" runat="server" DataIndex="JAN_ACTUAL" Text="Jan - Actual" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn12" runat="server" DataIndex="JAN_VAR" Text="Jan - Variance" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn13" runat="server" DataIndex="JAN_YTD" Text="Jan - YTD" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn14" runat="server" DataIndex="FEB_BUDGET" Text="Feb - Budget" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn15" runat="server" DataIndex="FEB_ACTUAL" Text="Feb - Actual" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn16" runat="server" DataIndex="FEB_VAR" Text="Feb - Variance" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn17" runat="server" DataIndex="FEB_YTD" Text="Feb - YTD" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn18" runat="server" DataIndex="MAR_BUDGET" Text="Mar - Budget" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn19" runat="server" DataIndex="MAR_ACTUAL" Text="Mar - Actual" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn20" runat="server" DataIndex="MAR_VAR" Text="Mar - Variance" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn21" runat="server" DataIndex="MAR_YTD" Text="Mar - YTD" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn22" runat="server" DataIndex="APR_BUDGET" Text="Apr - Budget" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn23" runat="server" DataIndex="APR_ACTUAL" Text="Apr - Actual" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn24" runat="server" DataIndex="APR_VAR" Text="Apr - Variance" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn25" runat="server" DataIndex="APR_YTD" Text="Apr - YTD" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn26" runat="server" DataIndex="MAY_BUDGET" Text="May - Budget" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn27" runat="server" DataIndex="MAY_ACTUAL" Text="May - Actual" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn28" runat="server" DataIndex="MAY_VAR" Text="May - Variance" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn29" runat="server" DataIndex="MAY_YTD" Text="May - YTD" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn30" runat="server" DataIndex="JUN_BUDGET" Text="Jun - Budget" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn31" runat="server" DataIndex="JUN_ACTUAL" Text="Jun - Actual" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn32" runat="server" DataIndex="JUN_VAR" Text="Jun - Variance" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn33" runat="server" DataIndex="JUN_YTD" Text="Jun - YTD" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn34" runat="server" DataIndex="JUL_BUDGET" Text="Jul - Budget" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn35" runat="server" DataIndex="JUL_ACTUAL" Text="Jul - Actual" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn36" runat="server" DataIndex="JUL_VAR" Text="Jul - Variance" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn37" runat="server" DataIndex="JUL_YTD" Text="Jul - YTD" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn38" runat="server" DataIndex="AUG_BUDGET" Text="Aug - Budget" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn39" runat="server" DataIndex="AUG_ACTUAL" Text="Aug - Actual" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn40" runat="server" DataIndex="AUG_VAR" Text="Aug - Variance" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn41" runat="server" DataIndex="AUG_YTD" Text="Aug - YTD" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn42" runat="server" DataIndex="SEP_BUDGET" Text="Sep - Budget" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn43" runat="server" DataIndex="SEP_ACTUAL" Text="Sep - Actual" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn44" runat="server" DataIndex="SEP_VAR" Text="Sep - Variance" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn45" runat="server" DataIndex="SEP_YTD" Text="Sep - YTD" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn46" runat="server" DataIndex="OCT_BUDGET" Text="Oct - Budget" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn47" runat="server" DataIndex="OCT_ACTUAL" Text="Oct - Actual" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn48" runat="server" DataIndex="OCT_VAR" Text="Oct - Variance" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn49" runat="server" DataIndex="OCT_YTD" Text="Oct - YTD" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn50" runat="server" DataIndex="TOTAL_PLAN" Text="Total Plan" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn51" runat="server" DataIndex="TOTAL_ACTUAL" Text="Total Actual" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="NumberColumn52" runat="server" DataIndex="TOTAL_VAR" Text="Variance" Width="110" Align="Right">
                                <Renderer Fn="colorTotalLineNumber" />
                            </ext:NumberColumn>
                        </Columns>
                    </ColumnModel>
                    <DirectEvents>
                        <ItemDblClick OnEvent="deEditSelectedRow">
                            <ExtraParams>
                                <ext:Parameter Name="LineID" Value="#{uxMonthDetail}.getSelectionModel().getSelection()[0].data.LINE_ID" Mode="Raw" />
                                <ext:Parameter Name="LineDesc" Value="#{uxMonthDetail}.getSelectionModel().getSelection()[0].data.LINE_DESC" Mode="Raw" />
                            </ExtraParams>
                        </ItemDblClick>
                    </DirectEvents>
                </ext:GridPanel>


                <%-------------------------------------------------- Bottom Form Panel --------------------------------------------------%>
                <ext:FormPanel ID="FormPanel1"
                    runat="server"
                    Region="South"
                    BodyPadding="20"
                    Height="120"
                    Disabled="false">
                    <Items>
                        <ext:FieldContainer ID="FieldContainer1"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label18" runat="server" Width="120" Text="Comments:" />
                                <ext:TextArea ID="uxComments" runat="server" Width="800" SelectOnFocus="true" />
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                </ext:FormPanel>

                <%--<ext:Hidden ID="uxHidBudBidID" runat="server" />
                <ext:Hidden ID="uxHidProjectID" runat="server" />
                <ext:Hidden ID="uxHidProjectNum" runat="server" />
                <ext:Hidden ID="uxHidProjectName" runat="server" />
                <ext:Hidden ID="uxHidType" runat="server" />
                <ext:Hidden ID="uxHidDetailTaskID" runat="server" />
                <ext:Hidden ID="uxHidDetailID" runat="server" />
                <ext:Hidden ID="uxHidDetailNum" runat="server" />
                <ext:Hidden ID="uxHidDetailName" runat="server" />
                <ext:Hidden ID="uxHidDetailType" runat="server" />--%>

                <ext:FormPanel ID="FormPanel2"
                    runat="server"
                    Region="South"
                    BodyPadding="20"
                    Height="50"
                    Disabled="false">
                    <Items>
                        <ext:FieldContainer ID="FieldContainer2"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:TextField ID="uxHidBudBidID" runat="server" Width="100" />
                                <ext:TextField ID="uxHidProjectID" runat="server" Width="100" />
                                <ext:TextField ID="uxHidProjectNum" runat="server" Width="100" />
                                <ext:TextField ID="uxHidProjectName" runat="server" Width="100" />
                                <ext:TextField ID="uxHidType" runat="server" Width="100" />
                                <ext:Label ID="Label2" runat="server" Width="50" Text="" />
                                <ext:TextField ID="uxHidDetailTaskID" runat="server" Width="100" />
                                <ext:TextField ID="uxHidDetailID" runat="server" Width="100" />
                                <ext:TextField ID="uxHidDetailNum" runat="server" Width="100" />
                                <ext:TextField ID="uxHidDetailName" runat="server" Width="100" />
                                <ext:TextField ID="uxHidDetailType" runat="server" Width="100" />
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

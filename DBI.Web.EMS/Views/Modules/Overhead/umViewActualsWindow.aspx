<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umViewActualsWindow.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umViewActualsWindow" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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


                var r = (n < 0 ? '-' : '') // preserve minus sign
                        + (x ? m[1].substr(0, x) + tSeparator : "")
                        + m[1].substr(x).replace(/(\d{3})(?=\d)/g, "$1" + tSeparator)
                        + (dp ? dSeparator + (+m[2] || 0).toFixed(dp).substr(2) : "")
                        + " " + symbol;

                return Ext.String.format(template, (n >= 0) ? "black" : "red", r);
            };
        };

        var submitValue = function (grid, hiddenFormat, format) {
            hiddenFormat.setValue(format);
            grid.submitData(false, { isUpload: true });
        };

    </script>

    <style>
        .x-grid-row-summary .x-grid-cell-inner {
            font-weight      : bold;
            font-size        : 11px;
            background-color : #E0E0D1;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" Namespace="App" ClientIDMode="Static" />         

          <ext:Hidden ID="FormatType" runat="server" />

        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
                 <ext:GridPanel ID="uxPeriodImportGridPanel" runat="server" Flex="1" Header="true" Title="Actuals By Period" Padding="5" Region="Center" Frame="true" Margins="5 5 5 5">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxDetailStore"
                            AutoDataBind="true" RemoteSort="true" AutoLoad="true" OnReadData="uxDetailStore_ReadData" >
                            <Model>
                                <ext:Model ID="Model4" runat="server" IDProperty="PERIOD_NUM">
                                    <Fields>
                                        <ext:ModelField Name="ENTERED_PERIOD_NAME"></ext:ModelField>
                                        <ext:ModelField Name="PERIOD_NUM"></ext:ModelField>
                                        <ext:ModelField Name="PERIOD_DR"></ext:ModelField>
                                        <ext:ModelField Name="PERIOD_CR"></ext:ModelField>
                                        <ext:ModelField Name="PERIOD_TOTAL"></ext:ModelField>
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
                            <ext:Column ID="Column116" runat="server" DataIndex="ENTERED_PERIOD_NAME" Text="Period" Flex="1"  >
                                <SummaryRenderer Handler="return ('Total');" />  
                            </ext:Column>
                            <ext:Column ID="Column3" runat="server" DataIndex="PERIOD_DR" Text="Debit" Flex="1" SummaryType="Sum" >
                               <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                            </ext:Column>
                            <ext:Column ID="Column4" runat="server" DataIndex="PERIOD_CR" Text="Credit" Flex="1" SummaryType="Sum" >
                               <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                            </ext:Column>
                            <ext:Column ID="Column2" runat="server" DataIndex="PERIOD_TOTAL" Text="Total" Flex="1" SummaryType="Sum" >
                               <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                     <Features>               
                     <ext:Summary ID="Summary1" runat="server" Dock="Bottom" />
                    </Features>  
                    <View>
                        <ext:GridView ID="GridView4" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View> 
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" Mode="Single" AllowDeselect="true" ID="uxPeriodSelectionModel">
                            <DirectEvents>
                                <Select OnEvent="deLoadDetails"><EventMask ShowMask="true"></EventMask></Select>
                            </DirectEvents>
                        </ext:RowSelectionModel>
                    </SelectionModel>
   
                </ext:GridPanel>

                 <ext:GridPanel ID="GridPanel1" runat="server" Flex="1" Header="false" Padding="5" Region="South" Frame="true"  Margins="5 5 5 5" Disabled="true">
                      <Plugins>
                        <ext:FilterHeader ID="uxGlAccountSecurityGridFilter" runat="server" Remote="true" />
                    </Plugins>
                     <TopBar>
                         <ext:Toolbar runat="server">
                             <Items>
                                 <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                                  <ext:Button ID="Button3" runat="server" Text="To Excel" Icon="PageExcel">
                              <Listeners>
                                  <Click Handler="submitValue(#{GridPanel1}, #{FormatType}, 'xls');" />
                              </Listeners>
                          </ext:Button>
                             </Items>
                         </ext:Toolbar>
                     </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="Store1"
                            AutoDataBind="true" RemoteSort="true" AutoLoad="false" OnReadData="Store1_ReadData" OnSubmitData="Store1_SubmitData" >
                            <Model>
                                <ext:Model ID="Model1" runat="server" IDProperty="ROW_ID">
                                    <Fields>
                                        <ext:ModelField Name="LINE_REFERENCE"></ext:ModelField>
                                        <ext:ModelField Name="LINE_DESCRIPTION"></ext:ModelField>
                                          <ext:ModelField Name="TRANSACTION_DATE" Type="Date"></ext:ModelField>
                                           <ext:ModelField Name="POSTED_DATE" Type="Date"></ext:ModelField>
                                          <ext:ModelField Name="CATEGORY"></ext:ModelField>
                                          <ext:ModelField Name="DEBIT"></ext:ModelField>
                                          <ext:ModelField Name="CREDIT"></ext:ModelField>
                                        <ext:ModelField Name="TOTAL"></ext:ModelField>
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
                            <ext:Column ID="Column1" runat="server" DataIndex="LINE_REFERENCE" Text="Reference" Flex="1"  >
                                  <SummaryRenderer Handler="return ('Total');" />  
                            </ext:Column>
                           <ext:Column ID="Column5" runat="server" DataIndex="LINE_DESCRIPTION" Text="Description" Flex="1"  >
                            </ext:Column>
                           <ext:Column ID="Column7" runat="server" DataIndex="CATEGORY" Text="Category" Flex="1"  >
                            </ext:Column>
                           <ext:DateColumn ID="Column6" runat="server" DataIndex="TRANSACTION_DATE" Text="Transaction Date" Flex="1" Format="MM-dd-yyyy"  >
                            </ext:DateColumn>
                           <ext:DateColumn ID="Column11" runat="server" DataIndex="POSTED_DATE" Text="Posted Date" Flex="1" Format="MM-dd-yyyy"  >
                            </ext:DateColumn>
                           <ext:Column ID="Column8" runat="server" DataIndex="DEBIT" Text="Debit" Flex="1" SummaryType="Sum" >
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                            </ext:Column>
                           <ext:Column ID="Column9" runat="server" DataIndex="CREDIT" Text="Credit" Flex="1" SummaryType="Sum"  >
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                            </ext:Column>
                           <ext:Column ID="Column10" runat="server" DataIndex="TOTAL" Text="Total" Flex="1" SummaryType="Sum" >
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                      <Features>               
                     <ext:Summary ID="Summary2" runat="server" Dock="Bottom" />
                    </Features>  
                    <View>
                        <ext:GridView ID="GridView1" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View>                     
                </ext:GridPanel>
                </Items>
            </ext:Viewport>
    </form>
</body>
</html>

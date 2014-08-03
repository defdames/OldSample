<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddOverheadDetailLine.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umAddOverheadDetailLine" %>

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
    </script>
</head>
<body>
    <form id="form1" runat="server">
    
        <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" Namespace="App" ClientIDMode="Static" >
        </ext:ResourceManager>         
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
                <ext:GridPanel ID="GridPanel3" runat="server" Flex="1" Title="Line Detail" Header="false" Padding="5" Region="Center">
                    <KeyMap runat="server">
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
                                        <ext:ModelField Name="PERIOD_NAME"></ext:ModelField>
                                        <ext:ModelField Name="AMOUNT"></ext:ModelField>
                                        <ext:ModelField Name="CODE_COMBINATION_ID"></ext:ModelField>
                                        <ext:ModelField Name="PERIOD_NUM"></ext:ModelField>
                                        <ext:ModelField Name="ORG_BUDGET_ID"></ext:ModelField>
                                        <ext:ModelField Name="DETAIL_TYPE"></ext:ModelField>
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
                            <ext:Column ID="Column116" runat="server" DataIndex="PERIOD_NAME" Text="Period" Flex="1" />
                            <ext:Column ID="Column117" runat="server" DataIndex="AMOUNT" Text="Amount" Flex="1" >
                                <Editor>
                                    <ext:NumberField runat="server" AllowBlank="false" ID="uxEditAmount" SelectOnFocus="true">
                                         <Listeners>
                                              <Show Handler="this.el.dom.select();" Delay="150" />
                                          </Listeners>
                                      </ext:NumberField>
                                </Editor>
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <View>
                        <ext:GridView ID="GridView4" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View> 
                    <Plugins>
                           <ext:CellEditing runat="server" ClicksToEdit="1">
                              </ext:CellEditing>
                                               </Plugins>
                       <Buttons>
                           <ext:Button runat="server" ID="uxSaveDetailLineButton" Text="Save" Icon="Disk" AutoFocus="true">
                                       <DirectEvents>
                                           <Click OnEvent="deSaveDetailLine" Success="parent.Ext.getCmp('uxDetailLineMaintenance').close();"><Confirmation ConfirmRequest="true" Message="Are you sure you want to save this detail line?"></Confirmation><EventMask ShowMask="true"></EventMask>
                                               <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel3}.getRowsValues())" Mode="Raw" />
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



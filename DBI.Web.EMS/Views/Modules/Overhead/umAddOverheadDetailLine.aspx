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
    
        <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" Namespace="App" ClientIDMode="Static" >
        </ext:ResourceManager>         
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
                <ext:Toolbar runat="server" Region="North" Padding="5" Margins="5 5 5 5">
                    <Items>
                        <ext:Button runat="server" Icon="CalculatorEdit" Text="Cost Allocation">
                            <Listeners>
                                <Click Handler="#{uxDispersementForm}.reset();#{uxDisbursementDetailsWindow}.show();"></Click>
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>

                <ext:GridPanel ID="GridPanel3" runat="server" Flex="1" Title="Account Totals" Header="true" Padding="5" Region="East" Frame="true"   Margins="5 5 5 0">
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
                                         <ext:ModelField Name="CREATED_BY"></ext:ModelField>
                                        <ext:ModelField Name="CREATE_DATE"></ext:ModelField>
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
                            <ext:Column ID="Column116" runat="server" DataIndex="PERIOD_NAME" Text="Period" Flex="1"  >
                                <SummaryRenderer Handler="return ('Total');" />  
                            </ext:Column>
                            <ext:Column ID="Column117" runat="server" DataIndex="AMOUNT" Text="Amount" Flex="1" SummaryType="Sum" >
                                <Editor>
                                    <ext:NumberField runat="server" AllowBlank="false" ID="uxEditAmount" SelectOnFocus="true" TabIndex="1">
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
                        </ext:GridView>
                    </View> 
                    
                    <Features>               
                     <ext:Summary ID="Summary1" runat="server" Dock="Bottom" />
            </Features>  
                    <Plugins>
                           <ext:CellEditing runat="server" ClicksToEdit="1">
                               <Listeners>
                                   <Edit Handler="#{uxSaveDetailLineButton}.focus();" />
                               </Listeners>
                              </ext:CellEditing>
                        
                                               </Plugins>
                       <Buttons>
                           <ext:Button runat="server" ID="uxSaveDetailLineButton" Text="Save" Icon="Disk" AutoFocus="true" TabIndex="2">
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

                <ext:FormPanel ID="FormPanel2" runat="server" Header="true" BodyPadding="5" Frame="true"
                    Margins="5 5 5 5" Region="Center" Title="Comments" Layout="FitLayout" Flex="1" >
                    <Items>
                      <ext:FieldContainer ID="FieldContainer2" 
                        runat="server"
                        LabelStyle="font-weight:bold;padding:0;"
                        Layout="FitLayout">
                        <Items>
                         <ext:TextArea ID="uxAccountComments" runat="server" Flex="1" Grow="true">
                         </ext:TextArea>
                        </Items>
                    </ext:FieldContainer> 
                    </Items>
                    </ext:FormPanel>

                </Items>
            </ext:Viewport>

        <ext:Window runat="server" Width="350" Height="150" Title="Disbursement Details" Layout="FitLayout" Header="true" Hidden="true" ID="uxDisbursementDetailsWindow" CloseAction="Hide" Closable="true" Modal="true" DefaultButton="uxCalculate">
            <Items>
                <ext:FormPanel ID="uxDispersementForm" runat="server" Header="false" BodyPadding="10" DefaultButton="uxAddBudgetType" Frame="true"
                    Margins="5 5 5 5" >
                    <Items>
                        
                         <ext:FieldContainer ID="FieldContainer1" 
                        runat="server"
                        LabelStyle="font-weight:bold;padding:0;"
                        Layout="FitLayout">
                        <Items>
                         <ext:ComboBox runat="server" ID="uxDispersementType" Editable="true" TypeAhead="true"
                                FieldLabel="Type" TriggerAction="All" 
                                MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;"  >
                                <Items>
                                    <ext:ListItem Text="Annual (By Month)" Value="A" />
                                     <ext:ListItem Text="Annual (By Week)" Value="AW" />
                                    <ext:ListItem Text="Monthly" Value="M" />
                                    <ext:ListItem Text="Weekly" Value="W" />
                                </Items>
                             <Listeners >
                                 <Select Handler=" #{uxAmountCalculator}.enable();#{uxCalculate}.enable();"></Select>
                             </Listeners>
                            </ext:ComboBox>
                        </Items>
                    </ext:FieldContainer> 

                             <ext:FieldContainer ID="FieldContainer3" 
                        runat="server"
                        LabelStyle="font-weight:bold;padding:0;"
                        Layout="FitLayout">
                        <Items>
                         <ext:Numberfield AllowDecimals="true" ID="uxAmountCalculator" runat="server" FieldLabel="Amount" FieldStyle="background-color: #EFF7FF; background-image: none;" Disabled="true" >
                         </ext:Numberfield>
                        </Items>
                    </ext:FieldContainer>

                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxCalculate" Text="Disperse Amount" Disabled="true" icon="CalculatorEdit">
                            <DirectEvents>
                                <Click OnEvent="deCalcuateAmount"><EventMask ShowMask="true"></EventMask></Click>
                            </DirectEvents>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>



<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEditBudget.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umEditBudget" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        Ext.util.Format.CurrencyFactory = function (dp, dSeparator, tSeparator, symbol) {
            return function (n) {
                if (n == 0) {
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
                        + " " + symbol;

                return Ext.String.format(template, (n > 0) ? "black" : "red", r);
            };
        };
    </script>

    <style>
        .x-grid-row-summary .x-grid-cell-inner {
            font-weight      : bold;
            font-size        : 11px;
            background-color : #9EC3E8;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
          <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
          <ext:GridPanel ID="uxOrganizationAccountGridPanel" runat="server" Flex="1" Title="General Ledger Accounts by Budget" Header="false" Margin="5" Region="Center" Scroll="Both"  >
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server"></ext:ToolbarFill>
                                <ext:Button runat="server" Icon="Accept" Text="Complete Budget" ID="uxCompleteBudget">
                                    <DirectEvents>
                                        <Click OnEvent="deCompleteBudget"><Confirmation ConfirmRequest="true" Message="Are you sure you want to complete this budget? This will lock your budget for this forecast and only finance will be allowed to unlock it." /><EventMask ShowMask="true"></EventMask></Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></ext:ToolbarSeparator>
                                 <ext:Button runat="server" Icon="NoteEdit" Text="Budget Notes" ID="uxBudgetNotes">
                                    <DirectEvents>
                                        <Click OnEvent="deEditBudgetNotes"></Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator runat="server"></ext:ToolbarSeparator>
                               <ext:Button Icon="MagifierZoomOut" Text="Hide Blank Lines" runat="server" ID="uxHideBlankLinesButton" EnableToggle="true" >
                                   <DirectEvents>
                                       <Toggle OnEvent="deHideBlankLines"><EventMask ShowMask="true"></EventMask></Toggle>
                                   </DirectEvents>
                               </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxOrganizationAccountStore"
                            AutoDataBind="true" RemoteSort="true" OnReadData="deLoadOrganizationAccounts" AutoLoad="true">
                            <Model>
                                <ext:Model ID="Model2" runat="server" IDProperty="CODE_COMBINATION_ID">
                                    <Fields>
                                        <ext:ModelField Name="ACCOUNT_DESCRIPTION" />
                                          <ext:ModelField Name="TOTAL" />
                                        <ext:ModelField Name="AMOUNT1" />
                                        <ext:ModelField Name="AMOUNT2" />
                                        <ext:ModelField Name="AMOUNT3" />
                                        <ext:ModelField Name="AMOUNT4" />
                                        <ext:ModelField Name="AMOUNT5" />
                                        <ext:ModelField Name="AMOUNT6" />
                                        <ext:ModelField Name="AMOUNT7" />
                                        <ext:ModelField Name="AMOUNT8" />
                                        <ext:ModelField Name="AMOUNT9" />
                                        <ext:ModelField Name="AMOUNT10" />
                                        <ext:ModelField Name="AMOUNT11" />
                                        <ext:ModelField Name="AMOUNT12" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                             <Sorters>
                                <ext:DataSorter Property="ACCOUNT_DESCRIPTION" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>

              <ColumnModel>
                  <Columns>
                      <ext:Column ID="Column14" runat="server" DataIndex="ACCOUNT_DESCRIPTION" Text="Account Name" Width="250" Locked="true" >
                          <SummaryRenderer Handler="return ('Budget Totals');" />                            
                      </ext:Column>
                      <ext:Column ID="Column54" runat="server" Text="Totals" Flex="1" Align="Center" DataIndex="TOTAL" SummaryType="Sum">
                          <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                          <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                      </ext:Column>
                      <ext:Column ID="Column1" runat="server" Text="November" Flex="1" Align="Center" DataIndex="AMOUNT1" SummaryType="Sum">
                              <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                          <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                      </ext:Column>
                       <ext:Column ID="Column2" runat="server" Text="December" Flex="1" Align="Center" DataIndex="AMOUNT2" SummaryType="Sum">
                               <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                           <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                      </ext:Column>
                       <ext:Column ID="Column3" runat="server" Text="January" Flex="1" Align="Center" DataIndex="AMOUNT3" SummaryType="Sum">
                              <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                           <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                      </ext:Column>
                       <ext:Column ID="Column4" runat="server" Text="February" Flex="1" Align="Center" DataIndex="AMOUNT4" SummaryType="Sum">
                              <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                           <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                      </ext:Column>
                       <ext:Column ID="Column5" runat="server" Text="March" Flex="1" Align="Center" DataIndex="AMOUNT5" SummaryType="Sum">
                              <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                           <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                      </ext:Column>
                       <ext:Column ID="Column6" runat="server" Text="April" Flex="1" Align="Center" DataIndex="AMOUNT6" SummaryType="Sum">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                           <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                      </ext:Column>
                       <ext:Column ID="Column7" runat="server" Text="May" Flex="1" Align="Center" DataIndex="AMOUNT7" SummaryType="Sum">
                               <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                           <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                      </ext:Column>
                       <ext:Column ID="Column8" runat="server" Text="June" Flex="1" Align="Center" DataIndex="AMOUNT8" SummaryType="Sum">
                               <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                           <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                      </ext:Column>
                       <ext:Column ID="Column9" runat="server" Text="July" Flex="1" Align="Center" DataIndex="AMOUNT9" SummaryType="Sum">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                           <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                      </ext:Column>
                       <ext:Column ID="Column10" runat="server" Text="August" Flex="1" Align="Center" DataIndex="AMOUNT10" SummaryType="Sum">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                           <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                      </ext:Column>
                       <ext:Column ID="Column11" runat="server" Text="September" Flex="1" Align="Center" DataIndex="AMOUNT11" SummaryType="Sum">
                               <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                           <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                      </ext:Column>
                       <ext:Column ID="Column12" runat="server" Text="October" Flex="1" Align="Center" DataIndex="AMOUNT12" SummaryType="Sum"> 
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                                <SummaryRenderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','')" />
                      </ext:Column>
                  </Columns>
              </ColumnModel>
                <Features>               
                     <ext:Summary ID="uxSummary" runat="server" Dock="Bottom" ShowSummaryRow="false"/>
            </Features>  
              <SelectionModel>
                        <ext:RowSelectionModel runat="server" ID="uxOrganizationAccountSelectionModel">
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <DirectEvents>
                        <ItemDblClick OnEvent="deItemMaintenance">
                            <ExtraParams>
                                <ext:Parameter Value="#{uxOrganizationAccountGridPanel}.getView().getSelectionModel().getSelection()[0].data.ACCOUNT_DESCRIPTION" Mode="Raw" Name="ACCOUNT_DESCRIPTION"></ext:Parameter>
                            </ExtraParams>
                        </ItemDblClick>
                    </DirectEvents>
                     <View>
                        <ext:GridView ID="GridView2" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View> 
                     
                </ext:GridPanel>
            </Items>
        </ext:Viewport>

        <ext:Window runat="server" Stateful="false" Width="550" Height="450" Title="Budget Notes" Layout="FitLayout" Header="true" Resizable="false" Frame="true" Hidden="true" ID="uxBudgetNotesWindow" CloseAction="Hide" Closable="true" Modal="true">
            <Items>
                 <ext:FormPanel ID="FormPanel2" runat="server" Header="false" BodyPadding="5"
                    Margins="5 5 5 5" Region="Center" Title="Comments" Layout="FitLayout" Flex="1" >
                    <Items>
                      <ext:FieldContainer ID="FieldContainer2" 
                        runat="server"
                        LabelStyle="font-weight:bold;padding:0;"
                        Layout="FitLayout">
                        <Items>
                         <ext:TextArea ID="uxBudgetComments" runat="server" Flex="1" Grow="true">
                         </ext:TextArea>
                        </Items>
                    </ext:FieldContainer> 
                    </Items>
                    </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button runat="server" ID="uxSaveBudgetNote" Icon="Accept" Text="Save">
                    <DirectEvents>
                        <Click OnEvent="deSaveBudgetNotes"><EventMask ShowMask="true"></EventMask></Click>
                    </DirectEvents>
                </ext:Button>
                  <ext:Button runat="server" ID="uxCancelSaveBudgetNote" Icon="Cancel" Text="Cancel">
                      <Listeners>
                          <Click Handler="#{uxBudgetNotesWindow}.close();"></Click>
                      </Listeners>
                  </ext:Button>
            </Buttons>
        </ext:Window>
    </form>
</body>
</html>

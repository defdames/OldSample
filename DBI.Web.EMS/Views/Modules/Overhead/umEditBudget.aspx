<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEditBudget.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umEditBudget" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
                               <ext:Button Icon="MagifierZoomOut" Text="Hide Blank Lines" runat="server" ID="uxHideBlankLinesButton" EnableToggle="true" ></ext:Button>
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
                                        <ext:ModelField Name="ACTUAL_TOTAL" />
                                        <ext:ModelField Name="BUDGET_TOTAL" />
                                        <ext:ModelField Name="ACTUAL_AMOUNT1" />
                                        <ext:ModelField Name="BUDGET_AMOUNT1" />
                                        <ext:ModelField Name="ACTUAL_AMOUNT2" />
                                        <ext:ModelField Name="BUDGET_AMOUNT2" />
                                        <ext:ModelField Name="ACTUAL_AMOUNT3" />
                                        <ext:ModelField Name="BUDGET_AMOUNT3" />
                                        <ext:ModelField Name="ACTUAL_AMOUNT4" />
                                        <ext:ModelField Name="BUDGET_AMOUNT4" />
                                        <ext:ModelField Name="ACTUAL_AMOUNT5" />
                                        <ext:ModelField Name="BUDGET_AMOUNT5" />
                                        <ext:ModelField Name="ACTUAL_AMOUNT6" />
                                        <ext:ModelField Name="BUDGET_AMOUNT6" />
                                        <ext:ModelField Name="ACTUAL_AMOUNT7" />
                                        <ext:ModelField Name="BUDGET_AMOUNT7" />
                                        <ext:ModelField Name="ACTUAL_AMOUNT8" />
                                        <ext:ModelField Name="BUDGET_AMOUNT8" />
                                        <ext:ModelField Name="ACTUAL_AMOUNT9" />
                                        <ext:ModelField Name="BUDGET_AMOUNT9" />
                                        <ext:ModelField Name="ACTUAL_AMOUNT10" />
                                        <ext:ModelField Name="BUDGET_AMOUNT10" />
                                        <ext:ModelField Name="ACTUAL_AMOUNT11" />
                                        <ext:ModelField Name="BUDGET_AMOUNT11" />
                                        <ext:ModelField Name="ACTUAL_AMOUNT12" />
                                        <ext:ModelField Name="BUDGET_AMOUNT12" />
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
                            <ext:Column ID="Column14" runat="server" DataIndex="ACCOUNT_DESCRIPTION" Text="Account Name" Width="250" Locked="true" />
                            <ext:Column ID="Column54" runat="server"  Text="Totals" Flex="1" Align="Center">
                                <Columns>
                                     <ext:Column ID="Column55" runat="server" Text="Actual" DataIndex="ACTUAL_TOTAL" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column56" runat="server" DataIndex="BUDGET_TOTAL"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                            <ext:Column ID="Column1" runat="server" Text="Novemeber" Flex="1" Align="Center">
                                <Columns>
                                        <ext:Column ID="Column18" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column19" runat="server" DataIndex="BUDGET_AMOUNT11"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                            <ext:Column ID="Column2" runat="server"  Text="December" Flex="1" Align="Center">
                                  <Columns>
                                       <ext:Column ID="Column20" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column21" runat="server" DataIndex="BUDGET_AMOUNT12"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                            <ext:Column ID="Column8" runat="server" Text="January" Flex="1" Align="Center">
                                  <Columns>
                                    <ext:Column ID="Column22" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column23" runat="server" DataIndex="BUDGET_AMOUNT1"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                            <ext:Column ID="Column9" runat="server"  Text="February" Flex="1" Align="Center">
                                  <Columns>
                                     <ext:Column ID="Column24" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column25" runat="server" DataIndex="BUDGET_AMOUNT3"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column6" runat="server"  Text="March" Flex="1" Align="Center">
                                    <Columns>
                                      <ext:Column ID="Column26" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column27" runat="server" DataIndex="BUDGET_AMOUNT3"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column7" runat="server"  Text="April" Flex="1" Align="Center">
                                    <Columns>
                                       <ext:Column ID="Column28" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column29" runat="server" DataIndex="BUDGET_AMOUNT4"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column10" runat="server"  Text="May" Flex="1" Align="Center">
                                    <Columns>
                                       <ext:Column ID="Column30" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column31" runat="server" DataIndex="BUDGET_AMOUNT5"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column11" runat="server"  Text="June" Flex="1" Align="Center">
                                    <Columns>
                                       <ext:Column ID="Column32" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column33" runat="server" DataIndex="BUDGET_AMOUNT6"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column12" runat="server"  Text="July" Flex="1" Align="Center">
                                    <Columns>
                                      <ext:Column ID="Column34" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column35" runat="server" DataIndex="BUDGET_AMOUNT7"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column13" runat="server"  Text="August" Flex="1" Align="Center">
                                    <Columns>
                                     <ext:Column ID="Column36" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column37" runat="server" DataIndex="BUDGET_AMOUNT8"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column15" runat="server"  Text="September" Flex="1" Align="Center">
                                    <Columns>
                                        <ext:Column ID="Column40" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column41" runat="server" DataIndex="BUDGET_AMOUNT9"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column16" runat="server" Text="October" Flex="1" Align="Center">
                                    <Columns>
                                        <ext:Column ID="Column38" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column39" runat="server" DataIndex="BUDGET_AMOUNT10"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" ID="uxOrganizationAccountSelectionModel">
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <DirectEvents>
                        <ItemDblClick OnEvent="deItemMaintenance">
                        </ItemDblClick>
                    </DirectEvents>
                     <View>
                        <ext:GridView ID="GridView2" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View> 
                     
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

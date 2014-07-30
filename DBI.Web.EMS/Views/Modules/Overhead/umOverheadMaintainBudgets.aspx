<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umOverheadMaintainBudgets.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umOverheadMaintainBudgets" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
      

        <ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout" IDMode="Explicit" Namespace="App" RenderXType="True"> 
            <Items>
                <ext:GridPanel ID="uxGlAccountSecurityGrid" runat="server" Flex="1" Title="Budget Versions By Organization" Region="Center" CollapseDirection="Top" Collapsible="true" Margin="5" >
                   <TopBar>
                       <ext:Toolbar runat="server">
                           <Items>
                               <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                               <ext:Button runat="server" Text="Hide Closed" Icon="BookMagnify" EnableToggle="true" Pressed="true" ID="uxViewAllToggleButton">
                                   <DirectEvents>
                                       <Toggle OnEvent="deToggleView"><EventMask ShowMask="true"></EventMask></Toggle>
                                   </DirectEvents>
                               </ext:Button>
                           </Items>
                       </ext:Toolbar>
                   </TopBar>
                     <Store>
                        <ext:Store runat="server"
                            ID="uxBudgetVersionByOrganizationStore"
                            AutoDataBind="true" RemoteSort="true"  AutoLoad="true" OnReadData="deLoadOrganizationsForUser">
                            <Model>
                                <ext:Model ID="Model1" runat="server" IDProperty="ORG_BUDGET_ID">
                                    <Fields>
                                        <ext:ModelField Name="FISCAL_YEAR" />
                                        <ext:ModelField Name="BUDGET_DESCRIPTION" />
                                        <ext:ModelField Name="BUDGET_STATUS" />
                                        <ext:ModelField Name="ORGANIZATION_NAME" />
                                        <ext:ModelField Name="ORGANIZATION_ID" />
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
                            <ext:Column ID="Column17" runat="server" DataIndex="ORGANIZATION_NAME" Text="Organization Name" Flex="1" />
                            <ext:Column ID="Column4" runat="server" DataIndex="FISCAL_YEAR" Text="Fiscal Year" Flex="1" />
                            <ext:Column ID="Column3" runat="server" DataIndex="BUDGET_DESCRIPTION" Text="Budget Draft" Flex="1" />
                             <ext:Column ID="Column5" runat="server" DataIndex="BUDGET_STATUS" Text="Status" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="uxGlAccountSecurityGridFilter" runat="server" Remote="true" />
                    </Plugins>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" AllowDeselect="true" Mode="Single" ID="uxBudgetVersionByOrganizationSelectionModel">
                            <DirectEvents>
                                <Select OnEvent="deSelectOrganization">
                                    <ExtraParams>
                                        <ext:Parameter Mode="Raw" Name="ORGANIZATION_ID" Value="record.data.ORGANIZATION_ID"></ext:Parameter>
                                         <ext:Parameter Mode="Raw" Name="FISCAL_YEAR" Value="record.data.FISCAL_YEAR"></ext:Parameter>
                                    </ExtraParams>
                                </Select>
                                <Deselect OnEvent="deDeSelectOrganization"></Deselect>
                            </DirectEvents>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
                    </BottomBar>
                     <View>
                        <ext:GridView ID="GridView1" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View> 
                     
                </ext:GridPanel>

                <ext:GridPanel ID="uxOrganizationAccountGridPanel" runat="server" Flex="1" Title="General Ledger Accounts by Budget" Margin="5" Region="South" Scroll="Both"  >
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                               <ext:Button Icon="BookMagnify" Text="Hide Empty Lines" runat="server" ID="uxHideEmptyLines" EnableToggle="true" Pressed="true"></ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxOrganizationAccountStore"
                            AutoDataBind="true" RemoteSort="true" OnReadData="deOrganizationList" AutoLoad="false" WarningOnDirty="false">
                            <Model>
                                <ext:Model ID="Model2" runat="server" IDProperty="CODE_COMBINATION_ID">
                                    <Fields>
                                        <ext:ModelField Name="CODE_COMBINATION_ID" />
                                        <ext:ModelField Name="SEGMENT5_DESC" />
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
                                        <ext:ModelField Name="SEGMENT7_DESC" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                             <Sorters>
                                <ext:DataSorter Property="SEGMENT5_DESC" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                   
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column14" runat="server" DataIndex="SEGMENT5_DESC" Text="Account Name" Width="250" Locked="true" />
                            <ext:Column ID="Column54" runat="server"  Text="Totals" Flex="1" Align="Center">
                                <Columns>
                                     <ext:Column ID="Column55" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column56" runat="server" DataIndex="AMOUNT1"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                            <ext:Column ID="Column1" runat="server" Text="11-2013" Flex="1" Align="Center">
                                <Columns>
                                        <ext:Column ID="Column18" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column19" runat="server" DataIndex="AMOUNT1"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                            <ext:Column ID="Column2" runat="server"  Text="12-2013" Flex="1" Align="Center">
                                  <Columns>
                                       <ext:Column ID="Column20" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column21" runat="server" DataIndex="AMOUNT1"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                            <ext:Column ID="Column8" runat="server" Text="01-2014" Flex="1" Align="Center">
                                  <Columns>
                                    <ext:Column ID="Column22" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column23" runat="server" DataIndex="AMOUNT1"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                            <ext:Column ID="Column9" runat="server"  Text="02-2014" Flex="1" Align="Center">
                                  <Columns>
                                     <ext:Column ID="Column24" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column25" runat="server" DataIndex="AMOUNT1"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column6" runat="server"  Text="03-2014" Flex="1" Align="Center">
                                    <Columns>
                                      <ext:Column ID="Column26" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column27" runat="server" DataIndex="AMOUNT1"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column7" runat="server"  Text="04-2014" Flex="1" Align="Center">
                                    <Columns>
                                       <ext:Column ID="Column28" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column29" runat="server" DataIndex="AMOUNT1"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column10" runat="server"  Text="05-2014" Flex="1" Align="Center">
                                    <Columns>
                                       <ext:Column ID="Column30" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column31" runat="server" DataIndex="AMOUNT1"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column11" runat="server"  Text="06-2014" Flex="1" Align="Center">
                                    <Columns>
                                       <ext:Column ID="Column32" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column33" runat="server" DataIndex="AMOUNT1"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column12" runat="server"  Text="07-2014" Flex="1" Align="Center">
                                    <Columns>
                                      <ext:Column ID="Column34" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column35" runat="server" DataIndex="AMOUNT1"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column13" runat="server"  Text="08-2014" Flex="1" Align="Center">
                                    <Columns>
                                     <ext:Column ID="Column36" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column37" runat="server" DataIndex="AMOUNT1"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column15" runat="server"  Text="09-2014" Flex="1" Align="Center">
                                    <Columns>
                                        <ext:Column ID="Column40" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column41" runat="server" DataIndex="AMOUNT1"  Width="125" Text="Budget"  Align="Center">
                                     </ext:Column>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column16" runat="server" Text="10-2014" Flex="1" Align="Center">
                                    <Columns>
                                        <ext:Column ID="Column38" runat="server" Text="Actual" Width="125" Align="Center" Hidden="true"/>
                                     <ext:Column ID="Column39" runat="server" DataIndex="AMOUNT1"  Width="125" Text="Budget"  Align="Center">
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

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

                <ext:GridPanel ID="GridPanel1" runat="server" Flex="1" Title="General Ledger Accounts by Budget" Margin="5" Region="South" Scroll="Both"  >
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button ID="Button1" runat="server" Text="Add Detail" Icon="DatabaseGear">
                                    <Listeners><Click Handler="#{uxAccountDetailsWindow}.show();" /></Listeners>
                                </ext:Button>
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
                        <ext:RowSelectionModel runat="server">
                        </ext:RowSelectionModel>
                    </SelectionModel>
                     <View>
                        <ext:GridView ID="GridView2" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View> 
                     
                </ext:GridPanel>
            </Items>
        </ext:Viewport>

        <ext:Window ID="uxAccountDetailsWindow" runat="server" Hidden="true" Closable="true" CloseAction="Hide" Title="Account Details" Modal="true" Width="950" Height="650" Layout="BorderLayout">
            <Items>
                 <ext:GridPanel ID="GridPanel2" runat="server" Flex="1" Title="Account Detail Lines" Padding="5" Region="North" >
                     <TopBar>
                         <ext:Toolbar ID="Toolbar2" runat="server">
                             <Items>
                                 <ext:Button ID="Button2" runat="server" Text="Add" icon="ApplicationAdd"></ext:Button>
                                   <ext:Button ID="Button3" runat="server" Text="Delete" icon="ApplicationDelete"></ext:Button>
                                   <ext:Button ID="Button4" runat="server" Text="Edit" Icon="ApplicationEdit"></ext:Button>
                             </Items>
                         </ext:Toolbar>
                     </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="Store2"
                            AutoDataBind="true" RemoteSort="true"  AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model3" runat="server" IDProperty="OVERHEAD_GL_ID">
                                    <Fields>
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
                            <ext:Column ID="Column110" runat="server" DataIndex="SEGMENT1" Text="Description" Flex="2" />
                             <ext:Column ID="Column113" runat="server" DataIndex="SEGMENT2" Text="Amount" Flex="1" />
                            <ext:Column ID="Column111" runat="server" DataIndex="SEGMENT2" Text="Spread Type" Flex="1" />
                            <ext:Column ID="Column112" runat="server" DataIndex="SEGMENT2" Text="Effective Start Date" Flex="1" />
                            <ext:Column ID="Column115" runat="server" DataIndex="SEGMENT2" Text="Effective End Date" Flex="1" />
                            <ext:ComponentColumn ID="ComponentColumn1" runat="server" DataIndex="SEGMENT2" Text="Comments" Flex="2">
                                <Component>
                                    <ext:TextArea ID="TextArea1" runat="server"></ext:TextArea>
                                </Component>
                            </ext:ComponentColumn>
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
                    </Plugins>
                    
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar2" runat="server" />
                    </BottomBar>
                     <View>
                        <ext:GridView ID="GridView3" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View> 
                </ext:GridPanel>
                   <ext:GridPanel ID="GridPanel3" runat="server" Flex="1" Title="Line Detail" Padding="5" Region="Center" >
                     <TopBar>
                         <ext:Toolbar ID="Toolbar3" runat="server">
                             <Items>
                                   <ext:Button ID="Button7" runat="server" Text="Edit" Icon="ApplicationEdit"></ext:Button>
                             </Items>
                         </ext:Toolbar>
                     </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="Store3"
                            AutoDataBind="true" RemoteSort="true"  AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model4" runat="server" IDProperty="OVERHEAD_GL_ID">
                                    <Fields>
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
                            <ext:Column ID="Column116" runat="server" DataIndex="SEGMENT1" Text="Line Date" Flex="1" />
                            <ext:Column ID="Column117" runat="server" DataIndex="SEGMENT2" Text="Amount" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="FilterHeader2" runat="server" Remote="false" />
                    </Plugins>
                     <View>
                        <ext:GridView ID="GridView4" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View> 
                </ext:GridPanel>
            </Items>
        </ext:Window>
    
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umViewBudgets.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umViewBudgets" %>

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
                    <Store>
                        <ext:Store runat="server"
                            ID="uxBudgetVersionByOrganizationStore"
                            AutoDataBind="true" RemoteSort="true"  AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model1" runat="server" IDProperty="OVERHEAD_GL_ID">
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
                            <ext:Column ID="Column17" runat="server" DataIndex="SEGMENT1" Text="Organization Name" Flex="1" />
                            <ext:Column ID="Column4" runat="server" DataIndex="SEGMENT1" Text="Fiscal Year" Flex="1" />
                            <ext:Column ID="Column3" runat="server" DataIndex="SEGMENT2" Text="Budget Draft" Flex="1" />
                             <ext:Column ID="Column5" runat="server" DataIndex="SEGMENT2" Text="Status" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="uxGlAccountSecurityGridFilter" runat="server" Remote="true" />
                    </Plugins>
                    
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
                    </BottomBar>
                     <View>
                        <ext:GridView ID="GridView1" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View> 
                     
                </ext:GridPanel>

                <ext:GridPanel ID="GridPanel1" runat="server" Flex="1" Title="General Ledger Accounts by Budget" Margin="5" Region="South" Scroll="Both" >
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                   <ext:Button ID="Button6" runat="server" Text="Display Settings" Icon="DatabaseGear"></ext:Button>
                                <ext:Button ID="Button1" runat="server" Text="Account Details" Icon="DatabaseGear"></ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="Store1"
                            AutoDataBind="true" RemoteSort="true"  AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model2" runat="server" IDProperty="OVERHEAD_GL_ID">
                                    <Fields>
                                        <ext:ModelField Name="OVERHEAD_GL_ID" />
                                        <ext:ModelField Name="CODE_COMBINATION_ID" />
                                        <ext:ModelField Name="SEGMENT1" />
                                        <ext:ModelField Name="SEGMENT2" />
                                        <ext:ModelField Name="SEGMENT3" />
                                        <ext:ModelField Name="SEGMENT4" />
                                        <ext:ModelField Name="SEGMENT5" />
                                        <ext:ModelField Name="SEGMENT5DESC" />
                                        <ext:ModelField Name="SEGMENT6" />
                                        <ext:ModelField Name="SEGMENT7" />
                                        <ext:ModelField Name="SEGMENT1DESC" />
                                        <ext:ModelField Name="SEGMENT2DESC" />
                                        <ext:ModelField Name="SEGMENT3DESC" />
                                        <ext:ModelField Name="SEGMENT4DESC" />
                                        <ext:ModelField Name="SEGMENT6DESC" />
                                        <ext:ModelField Name="SEGMENT7DESC" />
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
                            <ext:Column ID="Column14" runat="server" DataIndex="SEGMENT5DESC" Text="Account Name" Width="200" Locked="true" />
                            <ext:Column ID="Column54" runat="server" DataIndex="SEGMENT1" Text="Totals" Flex="1" Align="Center">
                                <Columns>
                                     <ext:Column ID="Column55" runat="server" DataIndex="SEGMENT2" Text="Actual" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column56" runat="server" DataIndex="SEGMENT2" Text="Budget" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column84" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column57" runat="server" DataIndex="SEGMENT2" Text="Forecast" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column58" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column59" runat="server" DataIndex="SEGMENT2" Text="Prev Yr" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column85" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                </Columns>
                            </ext:Column>
                            <ext:Column ID="Column1" runat="server" DataIndex="SEGMENT1" Text="11-2013" Flex="1" Align="Center">
                                <Columns>
                                       <ext:Column ID="Column18" runat="server" DataIndex="SEGMENT2" Text="Actual" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column19" runat="server" DataIndex="SEGMENT2" Text="Budget" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column20" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column60" runat="server" DataIndex="SEGMENT2" Text="Forecast" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column61" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column86" runat="server" DataIndex="SEGMENT2" Text="Prev Yr" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column87" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                </Columns>
                            </ext:Column>
                            <ext:Column ID="Column2" runat="server" DataIndex="SEGMENT2" Text="12-2013" Flex="1" Align="Center">
                                  <Columns>
                                        <ext:Column ID="Column21" runat="server" DataIndex="SEGMENT2" Text="Actual" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column22" runat="server" DataIndex="SEGMENT2" Text="Budget" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column23" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column62" runat="server" DataIndex="SEGMENT2" Text="Forecast" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column63" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column88" runat="server" DataIndex="SEGMENT2" Text="Prev Yr" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column89" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                </Columns>
                            </ext:Column>
                            <ext:Column ID="Column8" runat="server" DataIndex="SEGMENT3" Text="01-2014" Flex="1" Align="Center">
                                  <Columns>
                                       <ext:Column ID="Column24" runat="server" DataIndex="SEGMENT2" Text="Actual" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column25" runat="server" DataIndex="SEGMENT2" Text="Budget" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column26" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column64" runat="server" DataIndex="SEGMENT2" Text="Forecast" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column65" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column90" runat="server" DataIndex="SEGMENT2" Text="Prev Yr" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column91" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                </Columns>
                            </ext:Column>
                            <ext:Column ID="Column9" runat="server" DataIndex="SEGMENT4" Text="02-2014" Flex="1" Align="Center">
                                  <Columns>
                                        <ext:Column ID="Column27" runat="server" DataIndex="SEGMENT2" Text="Actual" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column28" runat="server" DataIndex="SEGMENT2" Text="Budget" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column29" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column66" runat="server" DataIndex="SEGMENT2" Text="Forecast" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column67" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column92" runat="server" DataIndex="SEGMENT2" Text="Prev Yr" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column93" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column6" runat="server" DataIndex="SEGMENT4" Text="03-2014" Flex="1" Align="Center">
                                    <Columns>
                                      <ext:Column ID="Column30" runat="server" DataIndex="SEGMENT2" Text="Actual" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column31" runat="server" DataIndex="SEGMENT2" Text="Budget" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column32" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column68" runat="server" DataIndex="SEGMENT2" Text="Forecast" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column69" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column94" runat="server" DataIndex="SEGMENT2" Text="Prev Yr" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column95" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column7" runat="server" DataIndex="SEGMENT4" Text="04-2014" Flex="1" Align="Center">
                                    <Columns>
                                       <ext:Column ID="Column33" runat="server" DataIndex="SEGMENT2" Text="Actual" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column34" runat="server" DataIndex="SEGMENT2" Text="Budget" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column35" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column70" runat="server" DataIndex="SEGMENT2" Text="Forecast" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column71" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column96" runat="server" DataIndex="SEGMENT2" Text="Prev Yr" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column97" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column10" runat="server" DataIndex="SEGMENT4" Text="05-2014" Flex="1" Align="Center">
                                    <Columns>
                                        <ext:Column ID="Column36" runat="server" DataIndex="SEGMENT2" Text="Actual" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column37" runat="server" DataIndex="SEGMENT2" Text="Budget" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column38" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column72" runat="server" DataIndex="SEGMENT2" Text="Forecast" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column73" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column98" runat="server" DataIndex="SEGMENT2" Text="Prev Yr" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column99" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column11" runat="server" DataIndex="SEGMENT4" Text="06-2014" Flex="1" Align="Center">
                                    <Columns>
                                        <ext:Column ID="Column39" runat="server" DataIndex="SEGMENT2" Text="Actual" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column40" runat="server" DataIndex="SEGMENT2" Text="Budget" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column41" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column74" runat="server" DataIndex="SEGMENT2" Text="Forecast" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column75" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column100" runat="server" DataIndex="SEGMENT2" Text="Prev Yr" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column101" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column12" runat="server" DataIndex="SEGMENT4" Text="07-2014" Flex="1" Align="Center">
                                    <Columns>
                                      <ext:Column ID="Column42" runat="server" DataIndex="SEGMENT2" Text="Actual" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column43" runat="server" DataIndex="SEGMENT2" Text="Budget" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column44" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column76" runat="server" DataIndex="SEGMENT2" Text="Forecast" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column77" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column102" runat="server" DataIndex="SEGMENT2" Text="Prev Yr" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column103" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column13" runat="server" DataIndex="SEGMENT4" Text="08-2014" Flex="1" Align="Center">
                                    <Columns>
                                    <ext:Column ID="Column45" runat="server" DataIndex="SEGMENT2" Text="Actual" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column46" runat="server" DataIndex="SEGMENT2" Text="Budget" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column47" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column78" runat="server" DataIndex="SEGMENT2" Text="Forecast" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column79" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column104" runat="server" DataIndex="SEGMENT2" Text="Prev Yr" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column105" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column15" runat="server" DataIndex="SEGMENT4" Text="09-2014" Flex="1" Align="Center">
                                    <Columns>
                                        <ext:Column ID="Column48" runat="server" DataIndex="SEGMENT2" Text="Actual" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column49" runat="server" DataIndex="SEGMENT2" Text="Budget" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column50" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column80" runat="server" DataIndex="SEGMENT2" Text="Forecast" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column81" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column106" runat="server" DataIndex="SEGMENT2" Text="Prev Yr" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column107" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                </Columns>
                            </ext:Column>
                              <ext:Column ID="Column16" runat="server" DataIndex="SEGMENT4" Text="10-2014" Flex="1" Align="Center">
                                    <Columns>
                                        <ext:Column ID="Column51" runat="server" DataIndex="SEGMENT2" Text="Actual" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column52" runat="server" DataIndex="SEGMENT2" Text="Budget" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column53" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column82" runat="server" DataIndex="SEGMENT2" Text="Forecast" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column83" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column108" runat="server" DataIndex="SEGMENT2" Text="Prev Yr" Flex="1" Align="Center"/>
                                     <ext:Column ID="Column109" runat="server" DataIndex="SEGMENT2" Text="Variance %" Flex="1" Align="Center"/>
                                </Columns>
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
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

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umIncidentReports.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umIncidentReports" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <ext:ResourceManager ID="ResourceManager1" runat="server" />

       
        <ext:GridPanel
            ID="uxIncidentGrid"
            runat="server"
            Title="Application Date Report"
            Icon="Report"
            Frame="false"
            Resizable="false"
            Collapsible="false">


            <Store>
                <ext:Store ID="uxIncidentStore"
                    runat="server"
                    GroupField="SUB_DIVISION" OnReadData="deIncidentGrid" RemoteSort="true">
                    <Model>
                        <ext:Model ID="Model1" runat="server">
                            <Fields>
                                <ext:ModelField Name="CROSSING_ID" />
                                <ext:ModelField Name="CROSSING_NUMBER" Type="String" />
                                <ext:ModelField Name="INCIDENT_ID" />
                                <ext:ModelField Name="MILE_POST" />
                                <ext:ModelField Name="DOT" />
                                <ext:ModelField Name="STATE" />
                                <ext:ModelField Name="CITY" />
                                <ext:ModelField Name="SUB_DIVISION" />
                                <ext:ModelField Name="INCIDENT_NUMBER" />
                                <ext:ModelField Name="SLOW_ORDER" />
                                <ext:ModelField Name="DATE_REPORTED" Type="Date" />
                                <ext:ModelField Name="DATE_CLOSED" Type="Date" />
                                <ext:ModelField Name="REMARKS" />
                               

                            </Fields>
                        </ext:Model>
                    </Model>
                    <Proxy>
                        <ext:PageProxy />
                    </Proxy>
                 
                    <Sorters>
                        <ext:DataSorter Property="SUB_DIVISION" />

                    </Sorters>

                </ext:Store>
            </Store>

            <ColumnModel ID="ColumnModel1" runat="server">
                <Columns>
                    <ext:Column ID="uxSubDiv" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />
                    <ext:Column ID="uxMainCrossingNum" runat="server" DataIndex="CROSSING_NUMBER" Text="Crossing #" Flex="1" />
                    <ext:Column ID="Column2" runat="server" Text="State" Flex="1" DataIndex="STATE" />  
                    <ext:Column ID="Column1" runat="server" Text="MP" Flex="1" DataIndex="MILE_POST" />
                    <ext:Column ID="Column3" runat="server" Text="DOT" Flex="1" DataIndex="DOT" />
                    <ext:Column ID="Column11" runat="server" Text="Incident #" DataIndex="INCIDENT_NUMBER" Flex="1"  />
                    <ext:DateColumn ID="DateColumn4" runat="server" DataIndex="DATE_REPORTED" Text="Date Reported" Flex="1" Format="MM/dd/yyyy" />                
                    <ext:DateColumn ID="DateColumn8" runat="server" DataIndex="DATE_CLOSED" Text="Date Closed" Flex="1" Format="MM/dd/yyyy" />
                     <ext:Column ID="Column5" runat="server" DataIndex="SLOW_ORDER" Text="Slow Order" Flex="1" />
                </Columns>
            </ColumnModel>
            <Features>
                <ext:Grouping ID="Grouping1"
                    runat="server"
                    HideGroupedHeader="true" />
            </Features>

            <Plugins>
                <ext:FilterHeader ID="FilterHeader2" runat="server" Remote="true"/>
            </Plugins>
            
          
            <TopBar>
                <ext:Toolbar ID="Toolbar2" runat="server">
                    <Items>
                        <ext:Button ID="Button1" runat="server" Text="Sub-Division Filter" Icon="Find">
                            <Menu>
                                <ext:Menu ID="Menu1" runat="server">
                                    <Items>
                                        <ext:MenuItem ID="MenuItem1" runat="server" Text="Show Filter">
                                            <Listeners>
                                                <Click Handler="#{uxSubDiv}.show();" />
                                            </Listeners>
                                        </ext:MenuItem>
                                        <ext:MenuItem ID="MenuItem2" runat="server" Text="Hide Filter">
                                            <Listeners>
                                                <Click Handler="#{uxSubDiv}.hide();" />
                                            </Listeners>
                                        </ext:MenuItem>
                                    </Items>
                                </ext:Menu>
                            </Menu>

                        </ext:Button>
                        <ext:Button ID="Button2"
                    runat="server"
                    Text="Print"
                    Icon="Printer"
                    OnClientClick="window.print();" />
              
                          <ext:Button runat="server"
                    ID="uxExportToPDF"
                    Text="Export to PDF"
                    Icon="PageWhiteAcrobat">
                    <%--<DirectEvents>
                        <Click OnEvent="deExportToPDF" IsUpload="true">
                            <ExtraParams>
                                <ext:Parameter Name="CrossingId" Value="#{GridPanel1}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                            </ExtraParams>
                        </Click>
                    </DirectEvents>--%>
                </ext:Button>
                        <ext:Button runat="server"
									ID="uxEmailPdf"
									Text="Email Copy"
									Icon ="EmailAttach"
									Disabled="false">
									<%--<DirectEvents>
										<Click OnEvent="deSendPDF" IsUpload="true">
											<ExtraParams>
												<ext:Parameter Name="CrossingId" Value="#{GridPanel1}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
											</ExtraParams>
										</Click>
									</DirectEvents>--%>
								</ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <BottomBar>
                <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
            </BottomBar>
        </ext:GridPanel>
    </div>
    </form>
</body>
</html>

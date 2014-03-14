<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAppDate.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umAppDate" %>

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
            ID="GridPanel1"
            runat="server"
            Title="Application Date Report"
            Icon="Report"
            Frame="false"
            Resizable="false"
            Collapsible="false">


            <Store>
                <ext:Store ID="uxAppDateStore"
                    runat="server"
                    GroupField="SUB_DIVISION" OnReadData="deAppDateGrid" RemoteSort="true">
                    <Model>
                        <ext:Model ID="Model1" runat="server">
                            <Fields>
                                <ext:ModelField Name="CROSSING_ID" />
                                <ext:ModelField Name="CROSSING_NUMBER" Type="String" />
                                <ext:ModelField Name="APPLICATION_DATE" Type="Date"/>
                                <ext:ModelField Name="MILE_POST" />
                                <ext:ModelField Name="DOT" />
                                <ext:ModelField Name="STATE" />
                                <ext:ModelField Name="TRUCK_NUMBER" />
                                <ext:ModelField Name="CITY" />
                                <ext:ModelField Name="STREET" />
                                <ext:ModelField Name="SUB_DIVISION" />
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
                    <ext:DateColumn runat="server" Text="Date" DataIndex="APPLICATION_DATE" Flex="1" Format="MM/dd/yyyy" />
                    <ext:Column ID="Column4" runat="server" DataIndex="TRUCK_NUMBER" Text="Truck #" Flex="1" />                
                    <ext:Column ID="Column8" runat="server" DataIndex="REMARKS" Text="Remarks" Flex="3" />
                </Columns>
            </ColumnModel>
            <Features>
                <ext:Grouping ID="Grouping1"
                    runat="server"
                    HideGroupedHeader="true"
                    Collapsible="false"
                    BodyStyle="background-color: Lightgrey;" />
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

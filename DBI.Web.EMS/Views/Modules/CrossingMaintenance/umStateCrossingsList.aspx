<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umStateCrossingsList.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umStateCrossingsList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <style type="text/css">
         .rowBodyCls .x-grid-cell-rowbody {
            border-style: solid;
            border-width: 0px 0px 1px;
            border-color: black;
        }
        
       .x-grid-group-title {
        color: #000000;
        font: bold 11px/13px tahoma,arial,verdana,sans-serif;
        }
       .x-grid-group-hd {
        border-width: 0 0 1px 0;
        border-style: solid;
        border-color: #000000;
        padding: 10px 4px 4px 4px;
        background: white;
        cursor: pointer;
        }
          
    </style>
     <script type="text/javascript">
      
         var GetAdditionalData = function(data, rowIndex, record, orig) {
             var headerCt = this.view.headerCt,
             colspan  = headerCt.getColumnCount();
             return {
                 rowBody: data.SPECIAL_INSTRUCTIONS,
                 rowBodyCls: this.rowBodyCls,
                 rowBodyColspan: colspan,
                 
                 
             };
         };
    </script>    
</head>
<body>
    <form id="form1" runat="server">
        <div></div>
        <ext:ResourceManager ID="ResourceManager1" runat="server" />

       
        <ext:GridPanel
            ID="GridPanel1"
            runat="server"
            Title="State Crossing List Report"
            Icon="Report"
            Frame="false"
            Resizable="false"
            Collapsible="false" Cls="my.grouped-header" >


            <Store>
                <ext:Store ID="uxStateCrossingListStore"
                    runat="server"
                    GroupField="SUB_DIVISION" OnReadData="deStateCrossingListGrid" PageSize="5" RemoteSort="true">
                    <Model>
                        <ext:Model ID="Model1" runat="server">
                            <Fields>
                                <ext:ModelField Name="CROSSING_ID" />
                                <ext:ModelField Name="CROSSING_NUMBER" Type="String" />
                                <ext:ModelField Name="MILE_POST" />
                                <ext:ModelField Name="DOT" />
                                <ext:ModelField Name="STATE" />
                                <ext:ModelField Name="COUNTY" />
                                <ext:ModelField Name="CITY" />
                                <ext:ModelField Name="STREET" />
                                <ext:ModelField Name="ROWNE" />
                                <ext:ModelField Name="ROWNW" />
                                <ext:ModelField Name="ROWSE" />
                                <ext:ModelField Name="ROWSW" />
                                <ext:ModelField Name="SUB_CONTRACTED" />
                                <ext:ModelField Name="LONGITUDE" />
                                <ext:ModelField Name="LATITUDE" />
                                <ext:ModelField Name="SUB_DIVISION" />
                                <ext:ModelField Name="SPECIAL_INSTRUCTIONS" />

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
                    <%--<ext:Column ID="uxMainCrossingNum" runat="server" DataIndex="CROSSING_NUMBER" Text="Crossing #" Flex="1" />--%>
                    <ext:Column ID="uxSubDiv" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />
                    <ext:Column ID="Column1" runat="server" Text="MP" Flex="1" DataIndex="MILE_POST" />
                    <ext:Column ID="Column3" runat="server" Text="DOT" Flex="1" DataIndex="DOT" />
                    <ext:Column ID="Column2" runat="server" Text="State" Flex="1" DataIndex="STATE" />
                    <ext:Column ID="Column4" runat="server" Text="City" Flex="1" DataIndex="CITY" />
                    <ext:Column ID="Column5" runat="server" Text="Street" Flex="1" DataIndex="STREET" />
                     <ext:TemplateColumn ID="TemplateColumn1" runat="server" DataIndex="" MenuDisabled="true" Header="NE / NW / SE / SW" Flex="1">
                        <Template ID="Template1" runat="server">
                            <Html>
						        <tpl for=".">
							        {ROWNE}  -
								    {ROWNW}  -
								    {ROWSE}  -
                                    {ROWSW} 
						        </tpl>
					        </Html>
                        </Template>
                    </ext:TemplateColumn>
                <%--    <ext:Column ID="Column6" runat="server" Text="NE" Flex="1" DataIndex="ROWNE" />
                    <ext:Column ID="Column7" runat="server" Text="NW" Flex="1" DataIndex="ROWNW" />
                    <ext:Column ID="Column9" runat="server" Text="SE" Flex="1" DataIndex="ROWSE" />
                    <ext:Column ID="Column10" runat="server" Text="SW" Flex="1" DataIndex="ROWSW" />--%>
                    <ext:Column ID="Column13" runat="server" Text="Subcontracted" Flex="1" DataIndex="SUB_CONTRACTED" />
                    <ext:Column ID="Column14" runat="server" Text="Latitude" Flex="1" DataIndex="LATITUDE" />
                    <ext:Column ID="Column15"  runat="server" Text="Longitude" Flex="1" DataIndex="LONGITUDE" />
                   
                 
                </Columns>
            </ColumnModel>
              <Features>
            <ext:RowBody ID="RowBody1" runat="server" RowBodyCls="rowBodyCls" >
                
                <GetAdditionalData Fn="GetAdditionalData"  />
                   
                
            </ext:RowBody>
        </Features>
            <Features>
                <ext:Grouping ID="Grouping1"
                    runat="server"
                    HideGroupedHeader="true"  Collapsible="false"  Cls="x-grid-group-title; x-grid-group-hd"
                    />
                
            </Features>
       
            <Plugins>
                <ext:FilterHeader ID="FilterHeader2" runat="server" Remote="true" />
            </Plugins>
           
            <TopBar>
                <ext:Toolbar ID="Toolbar2" runat="server">
                    <Items>
                        <ext:Button runat="server" Text="Sub-Division Filter" Icon="Find">
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
                        <ext:Button ID="Button1"
                    runat="server"
                    Text="Print"
                    Icon="Printer"
                    OnClientClick="window.print();" />
              
                          <ext:Button runat="server"
                    ID="uxExportToPDF"
                    Text="Export to PDF"
                    Icon="PageWhiteAcrobat">
                    <DirectEvents>
                        <Click OnEvent="deExportToPDF" IsUpload="true">
                            <ExtraParams>
                                <ext:Parameter Name="CrossingId" Value="#{GridPanel1}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                               <ext:Parameter Name="selectedCrossings" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw" />
                            </ExtraParams>
                        </Click>
                    </DirectEvents>
                </ext:Button>
                        <ext:Button runat="server"
									ID="uxEmailPdf"
									Text="Email Copy"
									Icon ="EmailAttach"
									Disabled="false">
									<DirectEvents>
										<Click OnEvent="deSendPDF" IsUpload="true">
											<ExtraParams>
												<ext:Parameter Name="CrossingId" Value="#{GridPanel1}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
											   <%--<ext:Parameter Name="CrossingId" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw" />--%>
                                            </ExtraParams>
										</Click>
									</DirectEvents>
								</ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <BottomBar>
                <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
            </BottomBar>
          
        </ext:GridPanel>

    </form>
</body>
</html>

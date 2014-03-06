<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umStateCrossingsList.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umStateCrossingsList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <div></div>
        <ext:ResourceManager ID="ResourceManager1" runat="server" />

        <ext:Toolbar ID="Toolbar1" runat="server">
            <Items>
                <ext:Button ID="Button1"
                    runat="server"
                    Text="Print"
                    Icon="Printer"
                    OnClientClick="window.print();" />
                <ext:Button runat="server"
                    ID="uxExportToPDF"
                    Text="Export to PDF"
                    Icon="PageWhiteAcrobat" />

            </Items>
        </ext:Toolbar>

        <ext:GridPanel
            ID="GridPanel1"
            runat="server"
            Title="State Crossing List Report"
            Icon="Report"
            Frame="false"
            Resizable="false"
            Collapsible="false">


            <Store>
                <ext:Store ID="uxStateCrossingListStore"
                    runat="server"
                    GroupField="SUB_DIVISION" OnReadData="deStateCrossingListGrid">
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
                    <ext:Column ID="Column11" runat="server" Text="County" Flex="1" DataIndex="COUNTY" />
                    <ext:Column ID="Column4" runat="server" Text="City" Flex="1" DataIndex="CITY" />
                    <ext:Column ID="Column5" runat="server" Text="Street" Flex="1" DataIndex="STREET" />
                    <ext:Column ID="Column6" runat="server" Text="NE" Flex="1" DataIndex="ROWNE" />
                    <ext:Column ID="Column7" runat="server" Text="NW" Flex="1" DataIndex="ROWNW" />
                    <ext:Column ID="Column9" runat="server" Text="SE" Flex="1" DataIndex="ROWSE" />
                    <ext:Column ID="Column10" runat="server" Text="SW" Flex="1" DataIndex="ROWSW" />
                    <ext:Column ID="Column13" runat="server" Text="Subcontracted" Flex="1" DataIndex="SUB_CONTRACTED" />
                    <ext:Column ID="Column14" runat="server" Text="Latitude" Flex="1" DataIndex="LATITUDE" />
                    <ext:Column ID="Column15" runat="server" Text="Longitude" Flex="1" DataIndex="LONGITUDE" />
                    <ext:Column ID="Column8" runat="server" DataIndex="SPECIAL_INSTRUCTIONS" Text="Spec. Instructions" Flex="2" />
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
                <ext:FilterHeader ID="FilterHeader2" runat="server" />                   
                </Plugins>
            <TopBar>
                <ext:Toolbar ID="Toolbar2" runat="server">
                    <Items>
                        <ext:Button runat="server" Text="Sub-Division Filter">
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

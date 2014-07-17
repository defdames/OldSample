<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umGeneralLedgerAccounts.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.Views.umGeneralLedgerAccounts" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
     <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" Namespace="App" />
    <form id="form1" runat="server">
   <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>

                <ext:TreePanel
                    ID="uxOrgPanel"
                    runat="server"
                    Title="Organizations"
                    BodyPadding="6"
                    Region="West"
                    Weight="100"
                    Width="300"
                    AutoScroll="true"
                    RootVisible="true"
                    SingleExpand="true"
                    Lines="false"
                    UseArrows="true">
                    <Store>
                        <ext:TreeStore ID="TreeStore1" runat="server" OnReadData="deLoadOrgTree">
                            <Proxy>
                                <ext:PageProxy></ext:PageProxy>
                            </Proxy>
                        </ext:TreeStore>
                    </Store>
                    <Root>
                        <ext:Node NodeID="0" Text="All Companies" Expanded="true" />
                    </Root>
                    <SelectionModel>
                        <ext:TreeSelectionModel ID="uxCompanySelectionModel" runat="server" Mode="Single"></ext:TreeSelectionModel>
                    </SelectionModel>
                </ext:TreePanel>



                <ext:GridPanel ID="uxOrganizationsGrid" runat="server" Flex="1" SimpleSelect="true" Title="Overhead Organizations" Padding="5" Region="North">
                    
                    <Store>
                        <ext:Store runat="server"
                            ID="uxOrganizationSecurityStore"
                            AutoDataBind="true" RemoteSort="true" PageSize="25" AutoLoad="true" OnReadData="deLoadActiveOrganizations">
                            <Model>
                                <ext:Model ID="Model2" runat="server" IDProperty="ORGANIZATION_ID">
                                    <Fields>
                                        <ext:ModelField Name="ORGANIZATION_ID" />
                                        <ext:ModelField Name="ORGANIZATION_NAME" Type="String" />
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
                            <ext:Column ID="Column2" runat="server" DataIndex="ORGANIZATION_NAME" Text="Name" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="uxOrganizationsGridFilter" runat="server" Remote="true" />
                    </Plugins>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" Mode="Single" AllowDeselect="true" ID="uxOrganizationsGridRowSelection">
                            <DirectEvents>
                                <Select OnEvent="deViewOrganizationGlAccounts"></Select>
                            </DirectEvents>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="uxOrganizationGridPageBar" runat="server" />
                    </BottomBar>
                </ext:GridPanel>

                <ext:GridPanel ID="uxGLAccountRangeGridPanel" runat="server" Flex="1" Title="General Ledger Account Range" Margin="5" Region="Center" >
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxShowGLRangeWindow" Text="Add Range" Icon="Add" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deShowRangeWindow"></Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxDeleteGLRangeDelete" Text="Delete Range" Icon="Delete" Disabled="true">                        
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxGLAccountRangeStore"
                            AutoDataBind="true" RemoteSort="true" OnReadData="deLoadGLAccountRange"  AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model1" runat="server" IDProperty="GL_RANGE_ID">
                                    <Fields>
                                        <ext:ModelField Name="ORGANIZATION_ID" />
                                        <ext:ModelField Name="SRSEGMENTS" />
                                        <ext:ModelField Name="ERSEGMENTS" />
                                        <ext:ModelField Name="INCLUDE_EXCLUDE" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Sorters>
                                <ext:DataSorter Property="ORGANIZATION_ID" Direction="ASC" />
                            </Sorters>
                            <Listeners><Load Handler="#{uxDeleteGLRangeDelete}.disable();"></Load></Listeners>
                        </ext:Store>
                    </Store>
                   
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column4" runat="server" DataIndex="SRSEGMENTS" Text="Starting Account Range" Flex="1" />
                            <ext:Column ID="Column3" runat="server" DataIndex="ERSEGMENTS" Text="Ending Account Range" Flex="1" />
                            <ext:Column ID="Column5" runat="server" DataIndex="INCLUDE_EXCLUDE" Text="Included / Excluded" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="uxGLAccountRangeFilter" runat="server" Remote="true" />
                    </Plugins>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="uxGLAccountRangeSelectionModel" runat="server" Mode="Simple">
                            <Listeners>
                                <Select Handler="if(#{uxGLAccountRangeSelectionModel}.getCount() > 0){#{uxDeleteGLRangeDelete}.enable();}else {#{uxDeleteGLRangeDelete}.disable();}"></Select>
                                <Deselect Handler="if(#{uxGLAccountRangeSelectionModel}.getCount() > 0){#{uxDeleteGLRangeDelete}.enable();} else {#{uxDeleteGLRangeDelete}.disable();}"></Deselect>
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
                    </BottomBar>
                     <View>
                        <ext:GridView ID="uxGLAccountRangeGridView" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View> 
                </ext:GridPanel>
         
            </Items>
       </ext:Viewport>
    </form>
</body>
</html>

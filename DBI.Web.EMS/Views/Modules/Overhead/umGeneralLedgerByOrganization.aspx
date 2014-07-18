<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umGeneralLedgerByOrganization.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umGeneralLedgerByOrganization" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
                <ext:GridPanel ID="uxGLAccountRangeGridPanel" runat="server" Flex="1" Title="General Ledger Account Range" Margin="5" Region="North" >
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxShowGLRangeWindow" Text="Add Range" Icon="Add" Disabled="true">
                                  
                                </ext:Button>
                                <ext:Button runat="server" ID="uxDeleteGLRangeDelete" Text="Delete Range" Icon="Delete" Disabled="true">                        
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxGLAccountRangeStore"
                            AutoDataBind="true" RemoteSort="true"  AutoLoad="false">
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


                 <ext:GridPanel ID="uxGlAccountSecurityGrid" runat="server" Flex="1" Title="General Ledger Accounts" Margin="5" Region="Center" >
                    <TopBar>
                        <ext:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxShowGLAccoutsWindow" Text="Assign" Icon="Add" Disabled="true">

                                </ext:Button>
                                <ext:Button runat="server" ID="uxGlAccountDelete" Text="Unassign" Icon="Delete" Disabled="true">

                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxGlAccountSecurityStore"
                            AutoDataBind="true" RemoteSort="true"   AutoLoad="false">
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
                                        <ext:ModelField Name="SEGMENT5_DESC" />
                                        <ext:ModelField Name="SEGMENT6" />
                                        <ext:ModelField Name="SEGMENT7" />
                                        <ext:ModelField Name="SEGMENT1_DESC" />
                                        <ext:ModelField Name="SEGMENT2_DESC" />
                                        <ext:ModelField Name="SEGMENT3_DESC" />
                                        <ext:ModelField Name="SEGMENT4_DESC" />
                                        <ext:ModelField Name="SEGMENT6_DESC" />
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
                            <Listeners><Load Handler="#{uxGlAccountDelete}.disable();"></Load></Listeners>
                        </ext:Store>
                    </Store>
                   
                    <ColumnModel>
                        <Columns>
                             <ext:Column ID="Column7" runat="server" DataIndex="SEGMENT5_DESC" Text="Account Name" Flex="2" />
                            <ext:Column ID="Column1" runat="server" DataIndex="SEGMENT1" Text="Company" Flex="1" />
                            <ext:Column ID="Column2" runat="server" DataIndex="SEGMENT2" Text="Location" Flex="1" />
                            <ext:Column ID="Column6" runat="server" DataIndex="SEGMENT3" Text="Division" Flex="1" />
                            <ext:Column ID="Column8" runat="server" DataIndex="SEGMENT4" Text="Branch" Flex="1" />
                            <ext:Column ID="Column10" runat="server" DataIndex="SEGMENT5" Text="Account" Flex="1" />
                            <ext:Column ID="Column11" runat="server" DataIndex="SEGMENT6" Text="Type" Flex="1" />
                            <ext:Column ID="Column12" runat="server" DataIndex="SEGMENT7" Text="Future" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="uxGlAccountSecurityGridFilter" runat="server" Remote="true" />
                    </Plugins>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="uxGlAccountSecurityGridSelectionModel" runat="server" Mode="Simple">
                            <Listeners>
                                <Select Handler="if(#{uxGlAccountSecurityGridSelectionModel}.getCount() > 0){#{uxGlAccountDelete}.enable();}else {#{uxGlAccountDelete}.disable();}"></Select>
                                <Deselect Handler="if(#{uxGlAccountSecurityGridSelectionModel}.getCount() > 0){#{uxGlAccountDelete}.enable();} else {#{uxGlAccountDelete}.disable();}"></Deselect>
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar2" runat="server" />
                    </BottomBar>
                     <View>
                        <ext:GridView ID="GridView1" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View> 
                     <ToolTips>
                        <ext:ToolTip ID="uxToolTip"
            runat="server"
            Target="uxGlAccountSecurityGrid"
            Delegate=".x-grid-row"
            TrackMouse="true"
                            UI="Info"
                           Width="300">
            <Listeners>
                <Show Handler="onShow(this, #{uxGlAccountSecurityGrid});" /> 
            </Listeners>
        </ext:ToolTip>  
                    </ToolTips>
                </ext:GridPanel>
        </Items>
            </ext:Viewport>
    </form>
</body>
</html>

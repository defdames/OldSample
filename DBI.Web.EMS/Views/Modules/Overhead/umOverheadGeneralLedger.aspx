<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umOverheadGeneralLedger.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umOverheadGeneralLedger" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
		.red-row .x-grid-cell, .red-row .x-grid-rowwrap-div .red-row .myBoldClass.x-grid3-row td  {
            color: red !important;
		}


	</style>

      <script type="text/javascript">
          var getRowClass = function (record, rowIndex, rowParams, store) {
              if (record.data.INCLUDE_EXCLUDE == "Excluded") {
                  return "red-row";
              };
          }

          var getAccountRowClass = function (record, rowIndex, rowParams, store) {
              if (record.data.INCLUDED_EXCLUDED == "Excluded") {
                  return "red-row";
              };
          }

          var onShow = function (toolTip, grid) {
              var view = grid.getView(),
                  record = view.getRecord(toolTip.triggerElement),
                  data = "General Ledger Description</br>" + record.data.SEGMENT1_DESC + "." + record.data.SEGMENT2_DESC + "." + record.data.SEGMENT3_DESC + "." + record.data.SEGMENT4_DESC + "." + record.data.SEGMENT5_DESC + "." + record.data.SEGMENT6_DESC + "." + record.data.SEGMENT7_DESC;
              toolTip.update(data);
          };
          </script>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
                <ext:GridPanel ID="uxGLAccountRangeGridPanel" runat="server" Flex="1" Title="General Ledger Account Range" Margin="5" Region="North">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxShowGLRangeWindow" Text="Add Range" Icon="Add" Disabled="false">
                                    <DirectEvents>
                                        <Click OnEvent="deAddGLRange">
                                            <EventMask ShowMask="true"></EventMask>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxDeleteGLRangeDelete" Text="Delete Range" Icon="Delete" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deDeleteGLRange"><Confirmation ConfirmRequest="true" Message="Are you sure you want to delete the selected record(s)?" /><EventMask ShowMask="true"></EventMask></Click>
                                    </DirectEvents>
                                </ext:Button>
                               
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxGLAccountRangeStore"
                            AutoDataBind="true" RemoteSort="true" OnReadData="deReadGLRange" AutoLoad="true">
                            <Model>
                                <ext:Model ID="Model1" runat="server" IDProperty="GL_RANGE_ID">
                                    <Fields>
                                        <ext:ModelField Name="ORGANIZATION_ID" />
                                        <ext:ModelField Name="SRSEGMENTS" Type="String" />
                                        <ext:ModelField Name="ERSEGMENTS" Type="String" />
                                        <ext:ModelField Name="INCLUDE_EXCLUDE" Type="String" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Sorters>
                                <ext:DataSorter Property="GL_RANGE_ID" Direction="ASC" />
                            </Sorters>
                            <Listeners>
                                <Load Handler="#{uxDeleteGLRangeDelete}.disable();"></Load>
                            </Listeners>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column5" runat="server" DataIndex="INCLUDE_EXCLUDE" Text="Status" Flex="1" />
                            <ext:Column ID="Column4" runat="server" DataIndex="SRSEGMENTS" Text="Starting Account Range" Flex="1" />
                            <ext:Column ID="Column3" runat="server" DataIndex="ERSEGMENTS" Text="Ending Account Range" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="uxGLAccountRangeFilter" runat="server" Remote="true" />
                    </Plugins>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="uxGLAccountRangeSelectionModel" runat="server" Mode="Single">
                            <DirectEvents>
                                <Select OnEvent="deSelectRange">
                                    <ExtraParams>
                                        <ext:Parameter Mode="Raw" Name="INCLUDE_EXCLUDE" Value="record.data.INCLUDE_EXCLUDE" />
                                    </ExtraParams>
                                </Select>
                                <Deselect OnEvent="deDeSelectRange"></Deselect>
                            </DirectEvents>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
                    </BottomBar>
                    <View>
                        <ext:GridView ID="uxGLAccountRangeGridView" StripeRows="true" runat="server" TrackOver="true">
                                   <GetRowClass Fn="getRowClass" />
                        </ext:GridView>
                    </View>
                </ext:GridPanel>


                <ext:GridPanel ID="uxGlAccountSecurityGrid" runat="server" Flex="1" Title="General Ledger Accounts" Margin="5" Region="Center">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxIncludeAccount" Text="Include Account" icon="Add" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deIncludeAccount"><Confirmation ConfirmRequest="true" Message="Are you sure you want to include this account from this account range?"></Confirmation></Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxExcludeAccount" Text="Exclude Account" Icon="Delete" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deExcludeAccount"><Confirmation ConfirmRequest="true" Message="Are you sure you want to exclude this account from this account range?"></Confirmation></Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxGlAccountSecurityStore"
                            AutoDataBind="true" RemoteSort="true" OnReadData="deReadAccountsForRanges" AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model2" runat="server" IDProperty="CODE_COMBINATION_ID">
                                    <Fields>
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
                                        <ext:ModelField Name="INCLUDED_EXCLUDED" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Sorters>
                                <ext:DataSorter Property="SEGMENT5_DESC" Direction="ASC" />
                            </Sorters>
                            <Listeners>
                                <Load Handler="#{uxExcludeAccount}.disable();#{uxIncludeAccount}.disable();"></Load>
                            </Listeners>
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
                            <DirectEvents>
                                <Select OnEvent="deSelectAccount">
                                </Select>
                                <Deselect OnEvent="deDeSelectAccount"></Deselect>
                            </DirectEvents>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar2" runat="server" />
                    </BottomBar>
                    <View>
                        <ext:GridView ID="GridView1" StripeRows="true" runat="server" TrackOver="true">
                            <GetRowClass Fn="getAccountRowClass"></GetRowClass>
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

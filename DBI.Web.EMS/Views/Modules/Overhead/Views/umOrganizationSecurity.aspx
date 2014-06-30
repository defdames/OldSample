<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umOrganizationSecurity.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.Views.umOrganizationSecurity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
		.blue-row .x-grid-cell, .blue-row .x-grid-rowwrap-div .blue-row .myBoldClass.x-grid3-row td  {
            color: #0042A3 !important;
		}


	</style>

      <script type="text/javascript">
          var getRowClass = function (record, rowIndex, rowParams, store) {
              if (record.data.ORGANIZATION_STATUS == "Allowed") {
                  return "blue-row";
              };
          }
          </script>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
        <ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout" IDMode="Explicit" Namespace="App" RenderXType="True">
            <Items>
                <ext:TreePanel ID="uxLegalEntityTreePanel"
                    runat="server"
                    Title="Hierarchies By Legal Entity"
                    Width="300"
                    RootVisible="false"
                    SingleExpand="true"
                    Lines="false"
                    FolderSort="true"
                    Margin="5"
                    UseArrows="true"
                    Region="West"
                    Scroll="Vertical" Collapsible="false">
                    <Store>
                        <ext:TreeStore ID="uxLegalEntityStore" runat="server" OnReadData="deLoadOracleHierarchy">
                            <Proxy>
                                <ext:PageProxy></ext:PageProxy>
                            </Proxy>
                            <DirectEvents>
                                <BeforeLoad>
                                    <EventMask ShowMask="true"></EventMask>
                                </BeforeLoad>
                            </DirectEvents>
                        </ext:TreeStore>
                    </Store>
                    <Root>
                        <ext:Node NodeID="0" Expanded="true" />
                    </Root>
                    <SelectionModel>
                        <ext:TreeSelectionModel ID="uxLegalEntitySelectionModel" runat="server" Mode="Single"></ext:TreeSelectionModel>
                    </SelectionModel>
                    <DirectEvents>
                        <ItemClick OnEvent="deShowOrganizationsByHierarchy">
                            <ExtraParams>
                                <ext:Parameter Name="id" Value="record.data.id" Mode="Raw" />
                            </ExtraParams>
                        </ItemClick>
                    </DirectEvents>
                    <View>
                        <ext:TreeView ID="uxLegalEntityTreeView" runat="server" LoadMask="true">
                        </ext:TreeView>
                    </View>
                </ext:TreePanel>

                <ext:GridPanel ID="uxOrganizationsGrid" runat="server" Flex="1" SimpleSelect="true" Title="Organizations By Hierarchy" Padding="5" Region="Center">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxOrganizationSecurityStore"
                            AutoDataBind="true" RemoteSort="true" PageSize="10" OnReadData="deLoadOrganizationsByHierarchy" AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model2" runat="server" IDProperty="ORGANIZATION_ID">
                                    <Fields>
                                        <ext:ModelField Name="HIER_LEVEL" />
                                        <ext:ModelField Name="ORGANIZATION_ID" />
                                        <ext:ModelField Name="ORGANIZATION_STATUS" />
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
                            <ext:Column ID="Column2" runat="server" DataIndex="ORGANIZATION_NAME" Text="Name" Flex="4" />
                            <ext:Column ID="Column1" runat="server" DataIndex="ORGANIZATION_STATUS" Text="Current Status" Flex="1" />
                            <ext:CommandColumn ID="CommandColumn1" runat="server" flex="1">
                        <Commands>
                               <ext:GridCommand Icon="NoteEdit" CommandName="EditStatus" Text="Modify Status" ToolTip-Text="Modify the status of this organization to either include or exclude it from the overhead budget system.">
                            </ext:GridCommand>
                        </Commands>
                                <DirectEvents>
                                    <Command OnEvent="deModifyOverheadStatus">
                                        <ExtraParams>
                                            <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                                            <ext:Parameter Name="orgID" Value="record.data.ORGANIZATION_ID" Mode="Raw"></ext:Parameter>
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                    </ext:CommandColumn>
                            
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="uxOrganizationsGridFilter" runat="server" Remote="true" />
                    </Plugins>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" Mode="Single" ID="uxOrganizationSelectionModel" AllowDeselect="true">
                            <Listeners>
                                <Select Handler="if(#{uxOrganizationSelectionModel}.getCount() > 0){#{uxShowGLAccoutsWindow}.enable();#{uxGlAccountDelete}.enable();}else {#{uxShowGLAccoutsWindow}.disable();#{uxGlAccountDelete}.disable();}"></Select>
                                <Deselect Handler="if(#{uxOrganizationSelectionModel}.getCount() > 0){#{uxShowGLAccoutsWindow}.enable();#{uxGlAccountDelete}.enable();} else {#{uxShowGLAccoutsWindow}.disable();#{uxGlAccountDelete}.disable();}"></Deselect>
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="uxOrganizationGridPageBar" runat="server" />
                    </BottomBar>
                    <View>
                        <ext:GridView ID="uxOrganizationsGridView" StripeRows="true" runat="server">
                             <GetRowClass Fn="getRowClass" />
                        </ext:GridView>
                    </View>
                </ext:GridPanel>

                <ext:GridPanel ID="uxGlAccountSecurityGrid" runat="server" Flex="1" Title="General Ledger Accounts" Margin="5" Region="South" >
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxShowGLAccoutsWindow" Text="Assign" Icon="Add" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deShowGLAccounts"></Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxGlAccountDelete" Text="Unassign" Icon="Delete" Disabled="true">
                                     <DirectEvents>
                                        <Click ><Confirmation Message="Are you sure you want to unassign these general ledger accounts for this organization?" Title="Unassign GL Accounts" ConfirmRequest="true"></Confirmation></Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxGlAccountSecurityStore"
                            AutoDataBind="true" RemoteSort="true"  AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model1" runat="server" IDProperty="OVERHEAD_GL_ID">
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
                            <ext:Column ID="Column4" runat="server" DataIndex="SEGMENT1" Text="Company" Flex="1" />
                            <ext:Column ID="Column3" runat="server" DataIndex="SEGMENT2" Text="Location" Flex="1" />
                            <ext:Column ID="Column5" runat="server" DataIndex="SEGMENT3" Text="Division" Flex="1" />
                            <ext:Column ID="Column6" runat="server" DataIndex="SEGMENT4" Text="Branch" Flex="1" />
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
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
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

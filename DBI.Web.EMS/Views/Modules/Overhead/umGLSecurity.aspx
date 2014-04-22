<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umGLSecurity.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umGLSecurity" %>

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
              if (record.data.GL_ASSIGNED == "Active") {
                  return "blue-row";
              };
          }

          var onShow = function (toolTip, grid) {
              var view = grid.getView(),
                  record = view.getRecord(toolTip.triggerElement),
                  data = "General Ledger Description</br>" + record.data.SEGMENT1DESC + "." + record.data.SEGMENT2DESC + "." + record.data.SEGMENT3DESC + "." + record.data.SEGMENT4DESC + "." + record.data.SEGMENT5DESC + "." + record.data.SEGMENT6DESC + "." + record.data.SEGMENT7DESC;
              toolTip.update(data);
          };

        </script>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
      

        <ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout" IDMode="Explicit" Namespace="App" RenderXType="True">
            <Items>
                <ext:TreePanel ID="uxHierarchyTree"
                    runat="server"
                    Title="Hierarchy List By Legal Entity"
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
                        <ext:TreeStore ID="TreeStore1" runat="server" OnReadData="LoadHierarchyTree">
                            <Proxy>
                                <ext:PageProxy></ext:PageProxy>
                            </Proxy>
                        </ext:TreeStore>
                    </Store>
                    <Root>
                        <ext:Node NodeID="0" Text="All Legal Entities" Expanded="true" />
                    </Root>
                    <SelectionModel>
                        <ext:TreeSelectionModel ID="uxHierarchyTreeSelectionModel" runat="server" Mode="Single"></ext:TreeSelectionModel>
                    </SelectionModel>
                    <DirectEvents>
                        <ItemClick OnEvent="deShowOrganizationsByHierarchy">
                            <ExtraParams>
                                <ext:Parameter Name="id" Value="record.data.id" Mode="Raw" />
                            </ExtraParams>
                        </ItemClick>
                    </DirectEvents>
                    <Listeners>
                        <Select Handler="#{uxShowGLAccoutsWindow}.disable();"></Select>
                    </Listeners>
                     <View>
                        <ext:TreeView ID="TreeView1" runat="server" LoadMask="true">
                        </ext:TreeView>
                    </View>
                </ext:TreePanel>
                <ext:TreePanel ID="uxOrganizationTreeGrid" runat="server" Flex="1" Title="Organization Hierarchy View" Margin="5" Region="Center" UseArrows="true"
            RootVisible="false"
            MultiSelect="false"
            SingleExpand="true"
            FolderSort="true" Collapsible="false">
                    <Store>
                        <ext:TreeStore ID="uxOrganizationTreeGridStore" runat="server" OnReadData="LoadOrganizationTree" AutoLoad="false">
                            <Proxy>
                                <ext:PageProxy></ext:PageProxy>
                            </Proxy>
                            <Fields>
                                <ext:ModelField Name="GL_ASSIGNED" Type="String" />
                                <ext:ModelField Name="ORGANIZATION_ID" />
                                <ext:ModelField Name="ORGANIZATION_NAME" Type="String" />
                            </Fields>
                        </ext:TreeStore>
                    </Store>
                    <ColumnModel>
                <Columns>
                    <ext:TreeColumn ID="TreeColumn1" 
                        runat="server"
                        Text="Orgnaization Name" 
                        Flex="4" 
                        Sortable="false"
                        DataIndex="ORGANIZATION_NAME" />
                     <ext:Column ID="TreeColumn2" 
                        runat="server"
                        Text="Organization Status" 
                        Flex="1" 
                        Sortable="false"
                        DataIndex="GL_ASSIGNED" />
                </Columns>    

            </ColumnModel>
                    <Root>
                        <ext:Node NodeID="0" Text="All Organizations" Expanded="true" />
                    </Root>
                    <SelectionModel>
                        <ext:TreeSelectionModel ID="TreeSelectionModel1" runat="server" Mode="Single">
                             <DirectEvents>
                                       <Select OnEvent="deOrganizationSelect">
                                       </Select>
                                   </DirectEvents>
                                   <Listeners>
                                        <Select Handler="#{uxShowGLAccoutsWindow}.enable();"></Select>
                                    </Listeners>
                        </ext:TreeSelectionModel>
                    </SelectionModel>
                    <View>
                        <ext:TreeView runat="server" LoadMask="true">
                            <GetRowClass Fn="getRowClass" />
                        </ext:TreeView>
                    </View>
                </ext:TreePanel>
                               
                <ext:GridPanel ID="uxGlAccountSecurityGrid" runat="server" Flex="1" Title="General Ledger Accounts" Margin="5" Region="South" >
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxShowGLAccoutsWindow" Text="Add" Icon="Add" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deShowGLAccounts"></Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxGlAccountDelete" Text="Delete" Icon="Delete" Disabled="true">
                                     <DirectEvents>
                                        <Click OnEvent="deDeleteGLAccounts"><Confirmation Message="Are you sure you want to unassign these general ledger accounts for this organization?" Title="Unassign GL Accounts" ConfirmRequest="true"></Confirmation></Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxGlAccountSecurityStore"
                            AutoDataBind="true" RemoteSort="true" OnReadData="deReadGLSecurityByOrganization"  AutoLoad="false">
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
                            <Sorters>
                                <ext:DataSorter Property="SEGMENT5DESC" Direction="ASC" />
                            </Sorters>
                            <Listeners><Load Handler="#{uxGlAccountDelete}.disable();"></Load></Listeners>
                        </ext:Store>
                    </Store>
                   
                    <ColumnModel>
                        <Columns>
                             <ext:Column ID="Column7" runat="server" DataIndex="SEGMENT5DESC" Text="Account Name" Flex="2" />
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

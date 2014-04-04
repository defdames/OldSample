<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSecurity.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umSecurity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    	<script type="text/javascript">
    	    var getRowClass = function (record, rowIndex, rowParams, store) {
    	        if (record.data.GL_ASSIGNED == "Y") {
    	            return "green-row";
    	        };
    	    }
	</script>
	<style type="text/css">
		.red-warning .x-grid-cell, .green-row .x-grid-rowwrap-div {
			background-color: green !important;
		}

	</style>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">


        <ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout" IDMode="Explicit" Namespace="App" RenderXType="True">
            <Items>
                        <ext:TreePanel ID="uxHierarchyTree"
                            runat="server"
                            Title="Hierarchy List"
                            Width="300"
                            padding="5"
                            RootVisible="false"
                            Lines="true"
                            UseArrows="false"
                            Region="West" >
                            <Store>
                                <ext:TreeStore runat="server" OnReadData="LoadHierarchyTree"></ext:TreeStore>
                            </Store>
                            <Root>
                                <ext:Node NodeID="0" Text="Root" />
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
                            </ext:TreePanel>
                <ext:GridPanel ID="uxOrganizationsGrid" runat="server" Flex="1" SimpleSelect="true" Title="Organization Security By Hierarchy" Padding="5" Region="Center">
                    <View>
                        <ext:GridView ID="uxOrganizationsGridView" runat="server">
                            <GetRowClass Fn="getRowClass" />
                        </ext:GridView>
                    </View>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxOrganizationSecurityStore"
                            AutoDataBind="true" RemoteSort="true" PageSize="10" OnReadData="deReadOrganizationsByHierarchy" AutoLoad="false">
                            <Model>
                                        <ext:Model ID="Model2" runat="server" IDProperty="ORGANIZATION_ID">
                                            <Fields>
                                                <ext:ModelField Name="GL_ASSIGNED" />
                                                <ext:ModelField Name="ORGANIZATION_ID" />
                                                <ext:ModelField Name="ORGANIZATION_NAME" />
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
                                    <ext:Column ID="Column1" runat="server" DataIndex="ORGANIZATION_NAME" Text="Organization Name" Flex="1" />
                                </Columns>
                            </ColumnModel>
                            <Plugins>
                                <ext:FilterHeader ID="uxOrganizationsGridFilter" runat="server" Remote="true" />
                            </Plugins>
                            <SelectionModel>
                               <ext:RowSelectionModel runat="server" Mode="Single" ID="uxOrganizationSelectionModel">
                                   <DirectEvents>
                                       <Select OnEvent="deOrganizationSelect">
                                       </Select>
                                   </DirectEvents>
                                   <Listeners>
                                        <Select Handler="#{uxShowGLAccoutsWindow}.enable();"></Select>
                                    </Listeners>
                               </ext:RowSelectionModel>
                            </SelectionModel>
                            <BottomBar>
                                <ext:PagingToolbar ID="uxOrganizationGridPageBar" runat="server" />
                            </BottomBar>                          
                        </ext:GridPanel>
                <ext:GridPanel ID="uxGlAccountSecurityGrid" runat="server" Flex="1" Title="General Ledger Accounts" Padding="5" Region="South" >
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
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Listeners><Load Handler="#{uxGlAccountDelete}.disable();"></Load></Listeners>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column4" runat="server" DataIndex="SEGMENT1" Text="Company" Flex="1" />
                            <ext:Column ID="Column3" runat="server" DataIndex="SEGMENT2" Text="Location" Flex="1" />
                            <ext:Column ID="Column5" runat="server" DataIndex="SEGMENT3" Text="Division" Flex="1" />
                            <ext:Column ID="Column6" runat="server" DataIndex="SEGMENT4" Text="Branch" Flex="1" />
                            <ext:Column ID="Column10" runat="server" DataIndex="SEGMENT5" Text="Account" Flex="1" />
                            <ext:Column ID="Column7" runat="server" DataIndex="SEGMENT5DESC" Text="Account Name" Flex="1" />
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
                                <Select Handler="if(#{uxGlAccountSecurityGridSelectionModel}.getCount() > 0){#{uxGlAccountDelete}.enable();}"></Select>
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

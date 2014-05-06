<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEditOrgs.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Security.umEditOrgs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../../../Resources/Scripts/functions.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" Namespace="" />
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
                    <View>
                        <ext:TreeView ID="TreeView1" runat="server" LoadMask="true">
                        </ext:TreeView>
                    </View>
                    <Listeners>
                        <ItemClick Handler="#{uxOrganizationSecurityStore}.reload()" />
                    </Listeners>
                </ext:TreePanel>
                <ext:GridPanel ID="uxOrganizationsGrid" runat="server" Flex="1" SimpleSelect="true" Title="Organizations By Hierarchy" Padding="5" Region="Center">
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" Text="Add" Icon="ApplicationAdd">
                                    <DirectEvents>
                                        <Click OnEvent="deAddOrganizations">
                                            <ExtraParams>
                                                <ext:Parameter Name="OrgsToAdd" Value="Ext.encode(#{uxOrganizationsGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                                                <ext:Parameter Name="OrgsAdded" Value="Ext.encode(#{uxAssignedOrgsGrid}.getRowsValues({selectedOnly: false}))" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxOrganizationSecurityStore"
                            AutoDataBind="true" PageSize="10" OnReadData="deReadOrganizationsByHierarchy" AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model2" runat="server" IDProperty="ORGANIZATION_ID">
                                    <Fields>
                                        <ext:ModelField Name="HIER_LEVEL" />
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
                            <ext:Column ID="Column2" runat="server" DataIndex="ORGANIZATION_NAME" Text="Organization Name" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="uxOrganizationsGridFilter" runat="server" />
                        <ext:BufferedRenderer runat="server" />
                    </Plugins>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel runat="server" Mode="Multi" />
                    </SelectionModel>
                </ext:GridPanel>

                <ext:GridPanel ID="uxAssignedOrgsGrid" runat="server" Flex="1" Title="Assigned Orgs" Margin="5" Region="South">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxSaveOrgsButton" Text="Save" Icon="Add" Disabled="true">
                                    <DirectEvents>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxRemoveOrgsButton" Text="Remove" Icon="Delete" Disabled="true">
                                    <Listeners>
                                        <Click Handler="#{uxAssignedOrgsStore}.remove(#{uxAssignedOrgsGrid}.getSelectionModel().getSelection())" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxAssignedOrgsStore"
                            AutoDataBind="true" RemoteSort="true" AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model1" runat="server" IDProperty="ORGANIZATION_ID" Name="HIERARCHY_TREEVIEW">
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
                            <ext:Column runat="server" DataIndex="ORGANIZATION_NAME" Text="Organization Name" Flex="1" />
                            <ext:Column runat="server" DataIndex="ORGANIZATION_ID" Text="Business Unit" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                    </Plugins>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel runat="server" Mode="Multi" />
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
                        </ext:ToolTip>
                    </ToolTips>
                    <Listeners>
                        <Select Handler="#{uxRemoveOrgsButton}.enable()" />
                    </Listeners>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

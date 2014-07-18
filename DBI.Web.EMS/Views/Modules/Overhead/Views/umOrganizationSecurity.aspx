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
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" Text="Enable" icon="Add" ID="uxEnableOrganizationButton" Disabled="true" >
                                    <ToolTips>
                                        <ext:ToolTip runat="server" UI="Info" Html="Enables an organization so it can be used for the budget overhead system."></ext:ToolTip>
                                    </ToolTips>
                                    <DirectEvents>
                                        <Click OnEvent="deEnableOrganization"><Confirmation Message="Are you sure you want to enable these organizations for use in the overhead budget system?" ConfirmRequest="true"></Confirmation></Click>
                                    </DirectEvents>
                                </ext:Button>
                                  <ext:Button runat="server" Text="Disable" icon="Decline" ID="uxDisableOrganizationButton" Disabled="true" >
                                    <ToolTips>
                                        <ext:ToolTip runat="server" UI="Info" Html="Disables an organization so that it can't be used for the budget overhead system."></ext:ToolTip>
                                    </ToolTips>
                                       <DirectEvents>
                                        <Click OnEvent="deDisableOrganization"><Confirmation Message="Are you sure you want to disable these organizations for use in the overhead budget system?" ConfirmRequest="true"></Confirmation></Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxOrganizationSecurityStore"
                            AutoDataBind="true" RemoteSort="true" PageSize="25" OnReadData="deLoadOrganizationsByHierarchy" AutoLoad="false">
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
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="uxOrganizationsGridFilter" runat="server" Remote="true" />
                    </Plugins>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel runat="server" Mode="Simple" ID="uxOrganizationsGridSelectionModel">
                           <Listeners>
                                <Select Handler="if(#{uxOrganizationsGridSelectionModel}.getCount() > 0){#{uxEnableOrganizationButton}.enable();#{uxDisableOrganizationButton}.enable();}else {#{uxEnableOrganizationButton}.disable();#{uxDisableOrganizationButton}.disable();}"></Select>
                                <Deselect Handler="if(#{uxOrganizationsGridSelectionModel}.getCount() > 0){#{uxEnableOrganizationButton}.enable();#{uxDisableOrganizationButton}.enable();}else {#{uxEnableOrganizationButton}.disable();#{uxDisableOrganizationButton}.disable();}"></Deselect>
                           </Listeners>
                        </ext:CheckboxSelectionModel>
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
            </Items>
        </ext:Viewport>
    </form>

</body>
</html>

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
                                    <Command OnEvent="deModifyOverheadStatus"><EventMask ShowMask="true"></EventMask>
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
                        <ext:RowSelectionModel runat="server" Mode="Single" ID="uxOrganizationSelectionModel">
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

            </Items>
        </ext:Viewport>
    </form>

</body>
</html>

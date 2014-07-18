<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umOverheadOrganizationSecurity.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umOverheadOrganizationSecurity" %>

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
              if (record.data.ORGANIZATION_STATUS == "Budgeting Allowed") {
                  return "blue-row";
              };
          }
          </script>
</head>
<body>
   <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout" Namespace="App">
            <Items>
                  <ext:GridPanel ID="uxOrganizationsGrid" runat="server" Flex="1" SimpleSelect="true" Header="false" Padding="5" Region="Center">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button runat="server" Text="Enable" icon="Add" ID="uxEnableOrganizationButton" Disabled="true" >
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip1" runat="server" UI="Info" Html="Enables an organization so it can be used for the budget overhead system."></ext:ToolTip>
                                    </ToolTips>
                                    <DirectEvents>
                                        <Click OnEvent="deEnableOrganization"><Confirmation Message="Are you sure you want to enable these organizations for use in the overhead budget system?" ConfirmRequest="true"></Confirmation></Click>
                                    </DirectEvents>
                                </ext:Button>
                                  <ext:Button runat="server" Text="Disable" icon="Decline" ID="uxDisableOrganizationButton" Disabled="true" >
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip2" runat="server" UI="Info" Html="Disables an organization so that it can't be used for the budget overhead system."></ext:ToolTip>
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

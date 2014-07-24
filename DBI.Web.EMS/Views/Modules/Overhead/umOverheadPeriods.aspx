<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umOverheadPeriods.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umOverheadPeriods" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout" Namespace="App" IDMode="Explicit">
            <Items>
                <ext:GridPanel ID="uxOrganizationsGrid" runat="server" Flex="1" SimpleSelect="true" Header="false" Padding="5" Region="Center">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxOrganizationSecurityStore"
                            AutoDataBind="true" RemoteSort="true" PageSize="25" OnReadData="deLoadAllowedBudgetOrganizations" AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model2" runat="server" IDProperty="ORGANIZATION_ID">
                                    <Fields>
                                        <ext:ModelField Name="ORGANIZATION_ID" />
                                        <ext:ModelField Name="CURRENT_BUDGET" />
                                        <ext:ModelField Name="STATUS" />
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
                            <ext:Column ID="Column2" runat="server" DataIndex="ORGANIZATION_NAME" Text="Name" Flex="3" />
                            <ext:Column ID="Column1" runat="server" DataIndex="CURRENT_BUDGET" Text="Budget Forecast" Flex="1" />
                            <ext:Column ID="Column3" runat="server" DataIndex="STATUS" Text="Status" Flex="1" />
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
                        </ext:GridView>
                    </View>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

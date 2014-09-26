<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umOverheadBudgetPeriods.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umOverheadBudgetPeriods" %>

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
                <ext:GridPanel ID="uxForecastPeriodsByOrganizationGridPanel" runat="server" Flex="1" SimpleSelect="true" Header="false" Title="Forecast Periods By Organization" Padding="5" Region="Center">
                      <TopBar>
                        <ext:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <ext:Button runat="server" Text="New Budget Period" Icon="Add" ID="uxCreateBudget"  >
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip1" runat="server" UI="Info" Html="Creates a new period for an organization so it can be used for the budget overhead system."></ext:ToolTip>
                                    </ToolTips>
                                    <DirectEvents>
                                        <Click OnEvent="deCreateBudgetPeriod">
                                        </Click>
                                    </DirectEvents>      
                                </ext:Button>
                                 <ext:ToolbarSeparator ID="ToolbarSeparator9" runat="server"></ext:ToolbarSeparator>
                                <ext:Button runat="server" Text="Import Actuals" Icon="CalculatorLink" Disabled="true" ID="uxImportActuals">
                                      <DirectEvents>
                                    <Click OnEvent="deImportActuals">
                                         <ExtraParams>
                                                <ext:Parameter Mode="Raw" Name="ORG_BUDGET_ID" Value="#{uxForecastPeriodsByOrganizationGridPanel}.getView().getSelectionModel().getSelection()[0].data.ORG_BUDGET_ID"></ext:Parameter>
                                                <ext:Parameter Mode="Raw" Name="FISCAL_YEAR" Value="#{uxForecastPeriodsByOrganizationGridPanel}.getView().getSelectionModel().getSelection()[0].data.FISCAL_YEAR"></ext:Parameter>
                                           </ExtraParams>
                                    </Click>
                                </DirectEvents>
                                </ext:Button>
                                  <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></ext:ToolbarSeparator>
                                <ext:Button runat="server" Text="Edit Budget" Icon="BookOpenMark" ID="uxEditBudget" Disabled="true" >
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip2" runat="server" UI="Info" Html="Allows you to edit this budget version."></ext:ToolTip>
                                    </ToolTips>
                                    <DirectEvents>
                                        <Click OnEvent="deEditBudget"><EventMask ShowMask="true"></EventMask>
                                            <ExtraParams>
                                                 <ext:Parameter Mode="Raw" Name="ORG_BUDGET_ID" Value="#{uxForecastPeriodsByOrganizationGridPanel}.getView().getSelectionModel().getSelection()[0].data.ORG_BUDGET_ID"></ext:Parameter>
                                                 <ext:Parameter Mode="Raw" Name="BUDGET_DESCRIPTION" Value="#{uxForecastPeriodsByOrganizationGridPanel}.getView().getSelectionModel().getSelection()[0].data.BUDGET_DESCRIPTION"></ext:Parameter>
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>      
                                </ext:Button>
                                 <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server"></ext:ToolbarSeparator>
                                <ext:Button runat="server" Text="Open Period" Icon="BookOpen" ID="uxOpenPeriod" Disabled="true" >
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip3" runat="server" UI="Info" Html="Opens a period for an organization so it can be used for the budget overhead system."></ext:ToolTip>
                                    </ToolTips>
                                    <DirectEvents>
                                        <Click OnEvent="deOpenPeriod"><EventMask ShowMask="true"></EventMask><Confirmation ConfirmRequest="true" Message="Are you sure you want to open these budget version(s)?"></Confirmation></Click>
                                    </DirectEvents>      
                                </ext:Button>
                                  <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server"></ext:ToolbarSeparator>
                                  <ext:Button runat="server" Text="Close Period"  Icon="Book" ID="uxClosePeriod" Disabled="true" >
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip4" runat="server" UI="Info" Html="Close a period organization so that it can't be used for the budget overhead system."></ext:ToolTip>
                                    </ToolTips>
                                      <DirectEvents>
                                        <Click OnEvent="deClosePeriod"><EventMask ShowMask="true"></EventMask><Confirmation ConfirmRequest="true" Message="Are you sure you want to close these budget version(s)?"></Confirmation></Click>
                                    </DirectEvents> 
                                </ext:Button>
                                  <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server"></ext:ToolbarSeparator>
                                <ext:Button runat="server" Text="Delete Period" Icon="Delete" ID="uxDelete" Disabled="true" >
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip5" runat="server" UI="Info" Html="Deletes a budget from the overhead system."></ext:ToolTip>
                                    </ToolTips>
                                      <DirectEvents>
                                        <Click OnEvent="deDeletePeriod"><EventMask ShowMask="true"></EventMask><Confirmation ConfirmRequest="true" Message="Are you sure you want to delete these budget version(s)?"></Confirmation></Click>
                                    </DirectEvents> 
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxForecastPeriodsByOrganization"
                            AutoDataBind="true" RemoteSort="true" PageSize="25" AutoLoad="true" OnReadData="deLoadForcastPeriodsByOrganization">
                            <Model>
                                <ext:Model ID="Model1" runat="server" IDProperty="ORG_BUDGET_ID">
                                    <Fields>
                                        <ext:ModelField Name="FISCAL_YEAR" />
                                        <ext:ModelField Name="BUDGET_DESCRIPTION" />
                                        <ext:ModelField Name="OVERHEAD_BUDGET_TYPE_ID" />
                                        <ext:ModelField Name="BUDGET_STATUS" />
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
                            <ext:Column ID="Column3" runat="server" DataIndex="FISCAL_YEAR" Text="Fiscal Year" Flex="1" />
                            <ext:Column ID="Column4" runat="server" DataIndex="BUDGET_DESCRIPTION" Text="Budget Forecast" Flex="1" />
                            <ext:Column ID="Column5" runat="server" DataIndex="BUDGET_STATUS" Text="Status" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
                    </Plugins>
                     <SelectionModel><ext:RowSelectionModel ID="uxForecastPeriodsByOrganizationSelectionModel" runat="server" Mode="Single" AllowDeselect="true">
                         <DirectEvents>
                             <Select OnEvent="deSelectForecast">
                                          <ExtraParams>
                                                 <ext:Parameter Mode="Raw" Name="ORG_BUDGET_ID" Value="#{uxForecastPeriodsByOrganizationGridPanel}.getView().getSelectionModel().getSelection()[0].data.ORG_BUDGET_ID"></ext:Parameter>
                                          </ExtraParams>
                             </Select>
                             <Deselect OnEvent="deDeSelectForecast" />
                         </DirectEvents>
                     </ext:RowSelectionModel>
                     </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
                    </BottomBar>
                    <View>
                        <ext:GridView ID="GridView1" StripeRows="true" runat="server">
                        </ext:GridView>
                    </View>
                </ext:GridPanel>
            </Items>
            </ext:Viewport>
    </div>
    </form>
</body>
</html>

﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umOverheadBudgetPeriods.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umOverheadBudgetPeriods" %>

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
                                 <ext:Button runat="server" Text="Create Budget" Icon="BookAdd" ID="uxCreateBudget" Disabled="true" >
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip1" runat="server" UI="Info" Html="Creates a new period for an organization so it can be used for the budget overhead system."></ext:ToolTip>
                                    </ToolTips>
                                    <DirectEvents>
                                        <Click OnEvent="deCreateBudgetPeriod">
                                            <ExtraParams>
                                            <ext:Parameter Value="#{uxForecastPeriodsByOrganizationGridPanel}.getView().getSelectionModel().getSelection()[0].data.ORG_BUDGET_ID" Mode="Raw" Name="ORG_BUDGET_ID"></ext:Parameter>
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>      
                                </ext:Button>
                                <ext:Button runat="server" Text="Open" Icon="BookOpen" ID="uxOpenPeriod" Disabled="true" >
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip3" runat="server" UI="Info" Html="Opens a period for an organization so it can be used for the budget overhead system."></ext:ToolTip>
                                    </ToolTips>
                                    <DirectEvents>
                                        <Click OnEvent="deOpenPeriod"><EventMask ShowMask="true"></EventMask><Confirmation ConfirmRequest="true" Message="Are you sure you want to open these budget(s)?"></Confirmation></Click>
                                    </DirectEvents>      
                                </ext:Button>
                                  <ext:Button runat="server" Text="Close"  Icon="Book" ID="uxClosePeriod" Disabled="true" >
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip4" runat="server" UI="Info" Html="Close a period an organization so that it can't be used for the budget overhead system."></ext:ToolTip>
                                    </ToolTips>
                                      <DirectEvents>
                                        <Click OnEvent="deClosePeriod"><EventMask ShowMask="true"></EventMask><Confirmation ConfirmRequest="true" Message="Are you sure you want to close these budget(s)?"></Confirmation></Click>
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
                     <SelectionModel><ext:CheckboxSelectionModel ID="uxForecastPeriodsByOrganizationSelectionModel" runat="server" Mode="Simple" AllowDeselect="true">
                         <DirectEvents>
                             <Select OnEvent="deSelectForecast"></Select>
                             <Deselect OnEvent="deDeSelectForecast" />
                         </DirectEvents>
                     </ext:CheckboxSelectionModel>
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

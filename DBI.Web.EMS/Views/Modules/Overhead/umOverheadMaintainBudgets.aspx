<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umOverheadMaintainBudgets.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umOverheadMaintainBudgets" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
      

        <ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout" IDMode="Explicit" Namespace="App" RenderXType="True"> 
            <Items>
                <ext:GridPanel ID="uxGlAccountSecurityGrid" runat="server" Flex="1" Header="false" Title="Budget Versions By Organization" Region="Center" CollapseDirection="Top"  Margin="5" >
                   <TopBar>
                       <ext:Toolbar runat="server">
                           <Items>
                               <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                               <ext:Button runat="server" Text="Hide Closed" Icon="BookMagnify" EnableToggle="true" Pressed="true" ID="uxViewAllToggleButton">
                                   <DirectEvents>
                                       <Toggle OnEvent="deToggleView"><EventMask ShowMask="true"></EventMask></Toggle>
                                   </DirectEvents>
                               </ext:Button>
                           </Items>
                       </ext:Toolbar>
                   </TopBar>
                     <Store>
                        <ext:Store runat="server"
                            ID="uxBudgetVersionByOrganizationStore"
                            AutoDataBind="true" RemoteSort="true"  AutoLoad="true" OnReadData="deLoadOrganizationsForUser">
                            <Model>
                                <ext:Model ID="Model1" runat="server" IDProperty="ORG_BUDGET_ID">
                                    <Fields>
                                        <ext:ModelField Name="FISCAL_YEAR" />
                                        <ext:ModelField Name="BUDGET_DESCRIPTION" />
                                        <ext:ModelField Name="BUDGET_STATUS" />
                                        <ext:ModelField Name="ORGANIZATION_NAME" />
                                        <ext:ModelField Name="ORGANIZATION_ID" />
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
                            <ext:Column ID="Column17" runat="server" DataIndex="ORGANIZATION_NAME" Text="Organization Name" Flex="1" />
                            <ext:Column ID="Column4" runat="server" DataIndex="FISCAL_YEAR" Text="Fiscal Year" Flex="1" />
                            <ext:Column ID="Column3" runat="server" DataIndex="BUDGET_DESCRIPTION" Text="Budget Draft" Flex="1" />
                             <ext:Column ID="Column5" runat="server" DataIndex="BUDGET_STATUS" Text="Status" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="uxGlAccountSecurityGridFilter" runat="server" Remote="true" />
                    </Plugins>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" AllowDeselect="true" Mode="Single" ID="uxBudgetVersionByOrganizationSelectionModel">
                            <DirectEvents>
                                <Select OnEvent="deSelectOrganization">
                                    <ExtraParams>
                                        <ext:Parameter Mode="Raw" Name="ORGANIZATION_ID" Value="record.data.ORGANIZATION_ID"></ext:Parameter>
                                         <ext:Parameter Mode="Raw" Name="FISCAL_YEAR" Value="record.data.FISCAL_YEAR"></ext:Parameter>
                                        <ext:Parameter Mode="Raw" Name="ORGANIZATION_NAME" Value="record.data.ORGANIZATION_NAME"></ext:Parameter>
                                        <ext:Parameter Mode="Raw" Name="BUDGET_DESCRIPTION" Value="record.data.BUDGET_DESCRIPTION"></ext:Parameter>
                                    </ExtraParams>
                                </Select>
                            </DirectEvents>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
                    </BottomBar>
                     <View>
                        <ext:GridView ID="GridView1" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View> 
                     
                </ext:GridPanel>
                </Items>
            </ext:Viewport>
          

 
    </form>
</body>
</html>

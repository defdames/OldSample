<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umGeneralLedgerAccounts.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.Views.umGeneralLedgerAccounts" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title></title>
    <script type="text/javascript">
        var onShow = function (toolTip, grid) {
            var view = grid.getView(),
				record = view.getRecord(toolTip.triggerElement),
				data = "General Ledger Description</br>" + record.data.SEGMENT1_DESC + "." + record.data.SEGMENT2_DESC + "." + record.data.SEGMENT3_DESC + "." + record.data.SEGMENT4_DESC + "." + record.data.SEGMENT5_DESC + "." + record.data.SEGMENT6_DESC + "." + record.data.SEGMENT7_DESC;
            toolTip.update(data);
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False"  />         
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>

                <ext:GridPanel ID="uxGlAccountSecurityGrid" runat="server" Flex="1" SimpleSelect="true" Title="General Ledger Accounts" Padding="5" Region="Center">
                   <Store>
                        <ext:Store runat="server"
                            ID="uxGlAccountSecurityStore"
                            AutoDataBind="true" OnReadData="deReadGLSecurityCodes" RemoteSort="true" PageSize="10" AutoLoad="true">
                            <Model>
                                <ext:Model ID="Model1" runat="server" IDProperty="CODE_COMBINATION_ID">
                                    <Fields>
                                        <ext:ModelField Name="CODE_COMBINATION_ID" />
                                        <ext:ModelField Name="SEGMENT1" />
                                        <ext:ModelField Name="SEGMENT2" />
                                        <ext:ModelField Name="SEGMENT3" />
                                        <ext:ModelField Name="SEGMENT4" />
                                        <ext:ModelField Name="SEGMENT5" />
                                        <ext:ModelField Name="SEGMENT5_DESC" />
                                        <ext:ModelField Name="SEGMENT6" />
                                        <ext:ModelField Name="SEGMENT7" />
                                        <ext:ModelField Name="SEGMENT1_DESC" />
                                        <ext:ModelField Name="SEGMENT2_DESC" />
                                        <ext:ModelField Name="SEGMENT3_DESC" />
                                        <ext:ModelField Name="SEGMENT4_DESC" />
                                        <ext:ModelField Name="SEGMENT5_DESC" />
                                        <ext:ModelField Name="SEGMENT6_DESC" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Sorters>
                                <ext:DataSorter Direction="ASC" Property="SEGMENT5_DESC" />
                            </Sorters>

                        </ext:Store>
                   </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column1" runat="server" DataIndex="SEGMENT5_DESC" Text="Account Description" Flex="3" />
                            <ext:Column ID="Column2" runat="server" DataIndex="SEGMENT1" Text="Company" Flex="1" />
                            <ext:Column ID="Column3" runat="server" DataIndex="SEGMENT2" Text="Location" Flex="1" />
                            <ext:Column ID="Column4" runat="server" DataIndex="SEGMENT3" Text="Division" Flex="1" />
                            <ext:Column ID="Column5" runat="server" DataIndex="SEGMENT4" Text="Branch" Flex="1" />
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
                                <Select Handler="if(#{uxGlAccountSecurityGridSelectionModel}.getCount() > 0){#{uxAddGLCodeButton}.enable();}else {#{uxAddGLCodeButton}.disable();}"></Select>
                                       <Deselect Handler="if(#{uxGlAccountSecurityGridSelectionModel}.getCount() > 0){#{uxAddGLCodeButton}.enable();}else {#{uxAddGLCodeButton}.disable();}"></Deselect>
                            </Listeners>
                               </ext:RowSelectionModel>
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar2" runat="server" />
                    </BottomBar>
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button ID="uxAddGLCodeButton" runat="server" Text="Include" Icon="Add" Disabled="true">
                                    <DirectEvents>
                                        <Click >
                                            <EventMask ShowMask="true"></EventMask>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                 <ext:Button ID="uxExcludeGLCodeButton" runat="server" Text="Exclude" Icon="Delete" Disabled="true">
                                    <DirectEvents>
                                        <Click >
                                            <EventMask ShowMask="true"></EventMask>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
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
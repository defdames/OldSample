<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umPricing.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umPricing" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
      <ext:ResourceManager runat="server" ID="ResourceManager2" />
        <div>
            <%--<PricingTab>--%>
            <ext:GridPanel ID="uxContactMainGrid" runat="server" Layout="HBoxLayout" Collapsible="true" Title="PRICING MAINTENANCE">
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="false" Mode="Single" />
                </SelectionModel>
                <Store>
                    <ext:Store runat="server"
                        ID="uxCurrentPriceStore"
                        
                        AutoDataBind="true" WarningOnDirty="false" PageSize="25">
                        <Model>
                            <ext:Model ID="Model1" runat="server">
                                <Fields>
                                    <ext:ModelField Name="PRICING_ID" />
                                    <ext:ModelField Name="RAILROAD" />
                                    <ext:ModelField Name="STATE" />
                                    <ext:ModelField Name="PROJECT_NUMBER" />
                                    <ext:ModelField Name="PROJECT_NAME" />
                                    <ext:ModelField Name="SERVICE_CATEGORY" />
                                    <ext:ModelField Name="PRICE" />
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
                        <ext:Column ID="uxNameCON" runat="server" DataIndex="SERVICE_CATEGORY" Text="Manager Name" Flex="1" />
                        <ext:Column runat="server" ID="uxWorkNumCON" Text="Price" DataIndex="PRICE" Flex="1" />
                        <ext:Column runat="server" ID="uxCellNumCON" Text="Railroad" DataIndex="RAILROAD" Flex="1" />
                        <ext:Column ID="uxRRCON" runat="server" DataIndex="STATE" Text="State" Flex="1" />
                    </Columns>
                </ColumnModel>
                  <Plugins>
                     <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
                  </Plugins>
             
                   <TopBar>
                    <ext:Toolbar ID="Toolbar2" runat="server" Region="North">
                            <Items>
                              <ext:Button ID="uxAddServiceUnitButton" runat="server" Text="Add Pricing" Icon="ApplicationAdd" >

                               <Listeners>
                                        <Click Handler="#{uxServiceUnitStore}.insert(0, new ServiceUnit());" />
                                    </Listeners>
                                </ext:Button>
                               <ext:Button ID="uxSaveServiceUnitButton" runat="server" Text="Save Pricing" Icon="Add" >

                                    <%--<DirectEvents>
                                        <Click OnEvent="deSaveServiceUnit" Before="#{uxServiceUnitStore}.isDirty()">
                                            <ExtraParams>
                                                <ext:Parameter Name="sudata" Value="#{uxServiceUnitStore}.getChangedData()" Mode="Raw" Encode="true" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>--%>
                                </ext:Button>
                                 <ext:Button ID="uxUpdateSubDivButton" runat="server" Text="Edit Pricing" Icon="ApplicationEdit">
                                 <Listeners>
                                     <Click Handler="#{uxUpdateSubDivWindow}.show()" />
                                 </Listeners>
                                   <%--  <DirectEvents>
                                         <Click OnEvent="deLoadServiceUnitStores" />
                                     </DirectEvents>--%>
                                 </ext:Button>
                               <%--  <ext:Button ID="uxRemoveServiceUnitButton" runat="server" Text="Remove Service Unit" Icon="Delete" >

                                  
                                </ext:Button>--%>
                               
                            </Items>
                        </ext:Toolbar>
          </TopBar>
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True">
                    </ext:PagingToolbar>
                </BottomBar>
                 <Listeners>
				<Select Handler="#{uxEditContactButton}.enable(); #{uxDeleteContact}.enable();" /> 
                            
			</Listeners>
            </ext:GridPanel>
         
    </div>
    </form>
</body>
</html>

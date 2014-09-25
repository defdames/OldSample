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
            <ext:GridPanel ID="uxPricingMainGrid" runat="server" Layout="HBoxLayout" Collapsible="true" Title="PRICING MAINTENANCE">
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="false" Mode="Single" />
                </SelectionModel>
                <Store>
                    <ext:Store runat="server"
                        ID="uxPriceStore"
                        OnReadData="deReadPricing"
                        AutoDataBind="true" WarningOnDirty="false" PageSize="30">
                        <Model>
                            <ext:Model ID="Model1" runat="server" Name="Pricing" IDProperty="PRICING_ID">
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
                          <Sorters>
                                <ext:DataSorter Direction="ASC" Property="STATE" />
                            </Sorters>
                    </ext:Store>
                </Store>
                <ColumnModel>
                    <Columns>
                        <ext:Column ID="uxNameCON" runat="server" DataIndex="SERVICE_CATEGORY" Text="Service Category" Flex="1" >
                        <Editor>
                          <ext:TextField ID="TextField1" EmptyText="Service Category" runat="server" />
                        </Editor>
                        </ext:Column>
                        <ext:Column runat="server" ID="uxWorkNumCON" Text="Price" DataIndex="PRICE" Flex="1" >
                        <Editor>
                          <ext:TextField ID="TextField2" EmptyText="Price" runat="server" />
                        </Editor>
                        </ext:Column>
                        <ext:Column runat="server" ID="uxCellNumCON" Text="Railroad" DataIndex="RAILROAD" Flex="1" >
                        <Editor>
                          <ext:ComboBox ID="uxRailRoadCI"
                                                runat="server"
                                                LabelAlign="Right"
                                                DisplayField="RAILROAD"
                                                ValueField="RAILROAD"
                                                QueryMode="Local"
                                                TypeAhead="true" Editable="false" >
                                                <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxSelectRailRoadStore">
                                                        <Model>
                                                            <ext:Model ID="Model4" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="RAILROAD_ID" />
                                                                    <ext:ModelField Name="RAILROAD" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                    </ext:Store>
                                                </Store>                                              
                                            </ext:ComboBox>
                        </Editor>
                        </ext:Column>
                        <ext:Column ID="uxRRCON" runat="server" DataIndex="STATE" Text="State" Flex="1" >
                        <Editor>
                           <ext:ComboBox runat="server"
                                                    ID="uxAddStateComboBox"
                                                    LabelAlign="Right"
                                                    DisplayField="name"
                                                    ValueField="name"
                                                    QueryMode="Local"
                                                    TypeAhead="true"
                                                    AllowBlank="false"
                                                    ForceSelection="true" TabIndex="5">
                                                    <Store>
                                                        <ext:Store ID="uxAddStateList" runat="server" AutoDataBind="true">
                                                            <Model>
                                                                <ext:Model ID="Model10" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="abbr" />
                                                                        <ext:ModelField Name="name" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                            <Reader>
                                                                <ext:ArrayReader />
                                                            </Reader>
                                                        </ext:Store>
                                                    </Store>
                                                </ext:ComboBox>
                        </Editor>
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                  <Plugins>
                     <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
                        <ext:CellEditing ID="CellEditing1" runat="server" ClicksToEdit="2" />
                  </Plugins>
             
                   <TopBar>
                    <ext:Toolbar ID="Toolbar2" runat="server" Region="North">
                            <Items>
                              <ext:Button ID="uxAddPricingButton" runat="server" Text="Add Pricing" Icon="ApplicationAdd" >

                               <Listeners>
                                        <Click Handler="#{uxPriceStore}.insert(0, new Pricing());" />
                                    </Listeners>
                                </ext:Button>
                               <ext:Button ID="uxSavePricingButton" runat="server" Text="Save Pricing" Icon="Add" >

                                    <DirectEvents>
                                        <Click OnEvent="deSavePricing" Before="#{uxPriceStore}.isDirty()">
                                            <ExtraParams>
                                                <ext:Parameter Name="PRIdata" Value="#{uxPriceStore}.getChangedData()" Mode="Raw" Encode="true" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                              <ext:Button ID="uxRemoveSDButton" runat="server" Text="Remove Pricing" Icon="Delete" Disabled="false" >

                                    <DirectEvents>
                                        <Click OnEvent="deRemovePricing" >
                                   <Confirmation ConfirmRequest="true" Title="Delete?" Message="Are you sure you want to delete the selected pricing?" />

                                            <ExtraParams>
                                                <ext:Parameter Name="PricingInfo" Value="Ext.encode(#{uxPricingMainGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                               
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

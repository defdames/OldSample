<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umInventoryTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umInventoryTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
        <ext:Store runat="server"
            ID="uxAddInventoryItemStore"
            RemoteSort="true"
            OnReadData="deReadItems"
            PageSize="15">
            <Model>
                <ext:Model ID="Model1" runat="server">
                    <Fields>
                        <ext:ModelField Name="ITEM_ID" Type="String" />
                        <ext:ModelField Name="SEGMENT1" Type="String" />
                        <ext:ModelField Name="DESCRIPTION" Type="String" />
                        <ext:ModelField Name="UOM_CODE" Type="String" />
                        <ext:ModelField Name="ENABLED_FLAG" />
                        <ext:ModelField Name="ACTIVE" />
                        <ext:ModelField Name="ITEM_COST" Type="String" />
                        <ext:ModelField Name="LAST_UPDATE_DATE" />
                        <ext:ModelField Name="LE" />
                        <ext:ModelField Name="ATTRIBUTE2" />
                        <ext:ModelField Name="INV_NAME" />
                        <ext:ModelField Name="INV_LOCATION" />
                        <ext:ModelField Name="ORGANIZATION_ID" />
                    </Fields>
                </ext:Model>
            </Model>
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Parameters>
                <ext:StoreParameter Name="Type" Value="Add" />
            </Parameters>
        </ext:Store>
        <ext:Store runat="server"
            ID="uxEditInventoryItemStore"
            RemoteSort="true"
            OnReadData="deReadItems"
            PageSize="15">
            <Model>
                <ext:Model ID="Model2" runat="server">
                    <Fields>
                        <ext:ModelField Name="ITEM_ID" Type="String" />
                        <ext:ModelField Name="SEGMENT1" Type="String" />
                        <ext:ModelField Name="DESCRIPTION" Type="String" />
                        <ext:ModelField Name="UOM_CODE" Type="String" />
                        <ext:ModelField Name="ENABLED_FLAG" />
                        <ext:ModelField Name="ACTIVE" />
                        <ext:ModelField Name="ITEM_COST" Type="String" />
                        <ext:ModelField Name="LAST_UPDATE_DATE" />
                        <ext:ModelField Name="LE" />
                        <ext:ModelField Name="ATTRIBUTE2" />
                        <ext:ModelField Name="INV_NAME" />
                        <ext:ModelField Name="INV_LOCATION" />
                        <ext:ModelField Name="ORGANIZATION_ID" />
                    </Fields>
                </ext:Model>
            </Model>
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Parameters>
                <ext:StoreParameter Name="Type" Value="Edit" />
            </Parameters>
        </ext:Store>
        <ext:GridPanel runat="server"
            ID="uxCurrentInventoryGrid"
            Layout="HBoxLayout">
            <Store>
                <ext:Store runat="server"
                    ID="uxCurrentInventoryStore">
                    <Model>
                        <ext:Model runat="server">
                            <Fields>
                                <ext:ModelField Name="INVENTORY_ID" />
                            </Fields>
                        </ext:Model>
                    </Model>
                </ext:Store>
            </Store>
            <ColumnModel>
                <Columns>
                    <ext:Column runat="server"
                        DataIndex="INVENTORY_ID"
                        Text="Inventory ID" />
                </Columns>
            </ColumnModel>
            <TopBar>
                <ext:Toolbar runat="server">
                    <Items>
                        <ext:Button runat="server"
                            ID="uxAddInventoryButton"
                            Text="Add Inventory"
                            Icon="ApplicationAdd">
                            <DirectEvents>
                                <Click OnEvent="dePopulateInventory">
                                    <ExtraParams>
                                        <ext:Parameter Name="Type" Value="Add" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                            <Listeners>
                                <Click Handler="#{uxAddInventoryWindow}.show()" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button runat="server"
                            ID="uxEditInventoryButton"
                            Text="Edit Inventory"
                            Icon="ApplicationEdit">
                            <DirectEvents>
                                <Click OnEvent="deEditInventoryForm">
                                    <ExtraParams>
                                        <ext:Parameter Name="InventoryInfo" Value="Ext.encode(#{uxCurrentInventoryGrid}.getRowsValues({selectedOnly : true}))" Mode="Raw" />
                                        <ext:Parameter Name="Type" Value="Edit" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                            <Listeners>
                                <Click Handler="#{uxEditInventoryWindow}.show()" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button runat="server"
                            ID="uxRemoveInventoryButton"
                            Text="Remove Inventory"
                            Icon="ApplicationDelete">
                            <DirectEvents>
                                <Click OnEvent="deRemoveInventory">
                                    <ExtraParams>
                                        <ext:Parameter Name="InventoryId" Value="#{uxCurrentInventoryGrid}.getSelectionModel().getSelection()[0].data.INVENTORY_ID" Mode="Raw" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <SelectionModel>
                <ext:RowSelectionModel runat="server" Mode="Single" />
            </SelectionModel>
        </ext:GridPanel>   
        <ext:Window runat="server"
            ID="uxAddInventoryWindow"
            Layout="FormLayout"
            Width="650"
            Hidden="true">
            <Items>
                <ext:FormPanel runat="server"
                    ID="uxAddInventoryForm"
                    Layout="FormLayout">
                    <Items>
                        <ext:DropDownField runat="server"
                            ID="uxAddInventoryMix"
                            Mode="ValueText"
                            FieldLabel="Select Mix">
                            <Component>
                                <ext:GridPanel runat="server"
                                    ID="uxAddInventoryMixGrid"  
                                    Layout="HBoxLayout">
                                    <Store>
                                        <ext:Store runat="server"
                                            ID="uxAddInventoryMixStore">
                                            <Model>
                                                <ext:Model runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="CHEMICAL_MIX_ID" />
                                                        <ext:ModelField Name="CHEMICAL_MIX_NUMBER" />
                                                        <ext:ModelField Name="HEADER_ID" />
                                                        <ext:ModelField Name="TARGET_ARE" />
                                                        <ext:ModelField Name="GALLON_ACRE" />
                                                        <ext:ModelField Name="GALLON_STARTING" />
                                                        <ext:ModelField Name="GALLON_MIXED" />
                                                        <ext:ModelField Name="GALLON_TOTAL" />
                                                        <ext:ModelField Name="GALLON_REMAINING" />
                                                        <ext:ModelField Name="GALLON_USED" />
                                                        <ext:ModelField Name="ACRES_SPRAYED" />
                                                        <ext:ModelField Name="STATE" />
                                                        <ext:ModelField Name="COUNTY" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>                                            
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column runat="server"
                                                DataIndex="CHEMICAL_MIX_NUMBER"
                                                Text="Mix #" Flex="20" />                                            
                                            <ext:Column runat="server"
                                                DataIndex="GALLON_ACRE"
                                                Text="Gallons / Acre" Flex="40"  />                                            
                                            <ext:Column runat="server"
                                                DataIndex="GALLON_USED"
                                                Text="Gallons Used" Flex="40" />                                            
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel runat="server" Mode="Single" />
                                    </SelectionModel>
                                </ext:GridPanel>
                            </Component>
                        </ext:DropDownField>
                        <ext:ComboBox runat="server"
                            ID="uxAddInventoryRegion"
                            FieldLabel="Select Region"
                            DisplayField="INV_NAME"
                            ValueField="ORGANIZATION_ID">
                            <Store>
                                <ext:Store runat="server" 
                                    ID="uxAddInventoryRegionStore">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="INV_NAME" />
                                                <ext:ModelField Name="ORGANIZATION_ID" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                            <DirectEvents>
                                <Select OnEvent="deLoadSubinventory">
                                    <ExtraParams>
                                        <ext:Parameter Name="Type" Value="Add" />
                                    </ExtraParams>
                                </Select>
                            </DirectEvents>
                            <Listeners>
                                <Select Handler="#{uxAddInventoryItemStore}.load()" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:ComboBox runat="server"
                            ID="uxAddInventorySub"
                            FieldLabel="Select Subinventory"
                            DisplayField="DESCRIPTION"
                            ValueField="SECONDARY_INV_NAME">
                            <Store>
                                <ext:Store runat="server"
                                    ID="uxAddInventorySubStore">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="SECONDARY_INV_NAME" />
                                                <ext:ModelField Name="DESCRIPTION" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                        </ext:ComboBox>
                        <ext:DropDownField runat="server"
                            ID="uxAddInventoryItem"
                            FieldLabel="Select Item">
                            <Component>
                                <ext:GridPanel runat="server"
                                    ID="uxAddInventoryItemGrid" StoreID="uxAddInventoryItemStore">
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column runat="server"
                                                DataIndex="SEGMENT1"
                                                Text="Item Id" />
                                            <ext:Column runat="server"
                                                DataIndex="DESCRIPTION"
                                                Text="Name" />
                                            <ext:Column runat="server"
                                                DataIndex="UOM_CODE"
                                                Text="Measure" />
                                        </Columns>
                                    </ColumnModel>
                                    <Plugins>
                                        <ext:FilterHeader runat="server" ID="uxAddInventoryItemHeadFilter" Remote="true" />
                                    </Plugins>
                                    <BottomBar>
                                        <ext:PagingToolbar runat="server" ID="uxAddInventoryItemPaging" />
                                    </BottomBar>
                                    <SelectionModel>
                                        <ext:RowSelectionModel runat="server" Mode="Single" />
                                    </SelectionModel>
                                </ext:GridPanel>
                            </Component>                            
                            <DirectEvents>
                                <Change OnEvent="deGetUnitOfMeasure">
                                    <ExtraParams>
                                        <ext:Parameter Name="uomCode" Value="#{uxAddInventoryItemGrid}.getSelectionModel().getSelection()[0].data.UOM_CODE" />
                                        <ext:Parameter Name="Type" Value="Add" />
                                    </ExtraParams>
                                </Change>
                            </DirectEvents>
                        </ext:DropDownField>
                        <ext:TextField runat="server"
                            ID="uxAddInventoryRate"
                            FieldLabel="Rate" />
                        <ext:ComboBox runat="server"
                            ID="uxAddInventoryMeasure"
                            FieldLabel="Unit of Measure"
                            ValueField="UOM_CODE"
                            DisplayField="UNIT_OF_MEASURE">
                            <Store>
                                <ext:Store runat="server"
                                    ID="uxAddInventoryMeasureStore">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="UOM_CODE" />
                                                <ext:ModelField Name="UNIT_OF_MEASURE" />
                                                <ext:ModelField Name="UOM_CLASS" />
                                                <ext:ModelField Name="BASE_UOM_FLAG" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                        </ext:ComboBox>
                        <ext:TextField runat="server"
                            ID="uxAddInventoryTotal"
                            Disabled="true"
                            FieldLabel="Total" />
                        <ext:TextField runat="server"
                            ID="uxAddInventoryEPA"
                            FieldLabel="EPA Number" />
                    </Items>
                    <Buttons>
                        <ext:Button runat="server"
                            ID="uxAddInventorySubmit"
                            Icon="ApplicationGo"
                            Text="Submit">
                            <DirectEvents>
                                <Click OnEvent="deAddInventory" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server"
                            ID="uxAddInventoryCancel"
                            Icon="ApplicationStop"
                            Text="Cancel">
                            <Listeners>
                                <Click Handler="#{uxAddInventoryWindow}.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
        <ext:Window runat="server"
            ID="uxEditInventoryWindow"
            Layout="FormLayout"
            Width="650"
            Hidden="true">
            <Items>
                <ext:FormPanel runat="server"
                    ID="uxEditInventoryForm"
                    Layout="FormLayout">
                    <Items>
                        <ext:DropDownField runat="server"
                            ID="uxEditInventoryMix"
                            Mode="ValueText"
                            FieldLabel="Select Mix">
                            <Component>
                                <ext:GridPanel runat="server"
                                    ID="uxEditInventoryMixGrid">
                                    <Store>
                                        <ext:Store runat="server"
                                            ID="uxEditInventoryMixStore">
                                            <Model>
                                                <ext:Model runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="CHEMICAL_MIX_ID" />
                                                        <ext:ModelField Name="CHEMICAL_MIX_NUMBER" />
                                                        <ext:ModelField Name="HEADER_ID" />
                                                        <ext:ModelField Name="TARGET_ARE" />
                                                        <ext:ModelField Name="GALLON_ACRE" />
                                                        <ext:ModelField Name="GALLON_STARTING" />
                                                        <ext:ModelField Name="GALLON_MIXED" />
                                                        <ext:ModelField Name="GALLON_TOTAL" />
                                                        <ext:ModelField Name="GALLON_REMAINING" />
                                                        <ext:ModelField Name="GALLON_USED" />
                                                        <ext:ModelField Name="ACRES_SPRAYED" />
                                                        <ext:ModelField Name="STATE" />
                                                        <ext:ModelField Name="COUNTY" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>                                            
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column runat="server"
                                                DataIndex="CHEMICAL_MIX_NUMBER"
                                                Text="Mix #" />
                                            <ext:Column runat="server"
                                                DataIndex="GALLON_ACRE"
                                                Text="Gallons / Acre" />
                                            <ext:Column runat="server"
                                                DataIndex="GALLON_USED"
                                                Text="Gallons Used" />
                                            </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel runat="server" Mode="Single" />
                                    </SelectionModel>
                                </ext:GridPanel>
                            </Component>
                        </ext:DropDownField>
                        <ext:ComboBox runat="server"
                            ID="uxEditInventoryRegion"
                            FieldLabel="Select Region"
                            DisplayField="INV_NAME"
                            ValueField="ORGANIZATION_ID" >
                            <Store>
                                <ext:Store runat="server" 
                                    ID="uxEditInventoryRegionStore">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="ORGANIZATION_ID" />
                                                <ext:ModelField Name="INV_NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                            <DirectEvents>
                                <Select OnEvent="deLoadSubinventory">
                                    <ExtraParams>
                                        <ext:Parameter Name="Type" Value="Edit" />
                                    </ExtraParams>
                                </Select>
                            </DirectEvents>
                            <Listeners>
                                <Select Handler="#{uxEditInventoryItemStore}.load()" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:ComboBox runat="server"
                            ID="uxEditInventorySub"
                            FieldLabel="Select Subinventory"
                            ValueField="DESCRIPTION"
                            DisplayField="SECONDARY_INV_NAME">
                            <Store>
                                <ext:Store runat="server"
                                    ID="uxEditInventorySubStore">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="DESCRIPTION" />
                                                <ext:ModelField Name="SECONDARY_INV_NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                            <%--<DirectEvents>
                                <Select OnEvent="deLoadItems">
                                    <ExtraParams>
                                        <ext:Parameter Name="Type" Value="Edit" />
                                    </ExtraParams>
                                </Select>
                            </DirectEvents>--%>
                        </ext:ComboBox>
                        <ext:DropDownField runat="server"
                            ID="uxEditInventoryItem"
                            FieldLabel="Select Item">
                            <Component>
                                <ext:GridPanel runat="server"
                                    ID="uxEditInventoryItemGrid"
                                    StoreId="uxEditInventoryItemStore">
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column runat="server"
                                                DataIndex="SEGMENT1"
                                                Text="Item Id" />
                                            <ext:Column runat="server"
                                                DataIndex="DESCRIPTION"
                                                Text="Name" />
                                            <ext:Column runat="server"
                                                DataIndex="UOM_CODE"
                                                Text="Measure" />
                                        </Columns>
                                    </ColumnModel>
                                    <Plugins>
                                        <ext:FilterHeader runat="server" ID="uxEditInventoryItemFilter" Remote="true" />
                                    </Plugins>
                                    <BottomBar>
                                        <ext:PagingToolbar runat="server" ID="uxEditInventoryItemPaging" />
                                    </BottomBar>
                                </ext:GridPanel>
                            </Component>
                            <DirectEvents>
                                <Change OnEvent="deGetUnitOfMeasure">
                                    <ExtraParams>
                                        <ext:Parameter Name="uomCode" Value="#{uxEditInventoryItemGrid}.getSelectionModel().getSelection()[0].data.UOM_CODE" />
                                        <ext:Parameter Name="Type" Value="Edit" />
                                    </ExtraParams>
                                </Change>
                            </DirectEvents>
                        </ext:DropDownField>
                        <ext:TextField runat="server"
                            ID="uxEditInventoryRate"
                            FieldLabel="Rate" />
                        <ext:ComboBox runat="server"
                            ID="uxEditInventoryMeasure"
                            FieldLabel="Unit of Measure">
                            <Store>
                                <ext:Store runat="server"
                                    ID="uxEditInventoryMeasureStore">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="name" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                        </ext:ComboBox>
                        <ext:TextField runat="server"
                            ID="uxEditInventoryTotal"
                            Disabled="true"
                            FieldLabel="Total" />
                        <ext:TextField runat="server"
                            ID="uxEditInventoryEPA"
                            FieldLabel="EPA Number" />
                    </Items>
                    <Buttons>
                        <ext:Button runat="server"
                            ID="uxEditInventorySubmit"
                            Icon="ApplicationGo"
                            Text="Submit">
                            <DirectEvents>
                                <Click OnEvent="deEditInventory" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server"
                            ID="uxEditInventoryCancel"
                            Icon="ApplicationStop"
                            Text="Cancel">
                            <Listeners>
                                <Click Handler="#{uxEditInventoryWindow}.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>

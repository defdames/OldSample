<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umInventoryTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umInventoryTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
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
                            FieldLabel="Select Region">
                            <Store>
                                <ext:Store runat="server" 
                                    ID="uxAddInventoryRegionStore">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="REGION_ID" />
                                                <ext:ModelField Name="NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                        </ext:ComboBox>
                        <ext:ComboBox runat="server"
                            ID="uxAddInventorySub"
                            FieldLabel="Select Subinventory">
                            <Store>
                                <ext:Store runat="server"
                                    ID="uxAddInventorySubStore">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="SUBINVENTORY_ID" />
                                                <ext:ModelField Name="NAME" />
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
                                    ID="uxAddInventoryGrid">
                                    <Store>
                                        <ext:Store runat="server"
                                            ID="uxAddInventoryStore">
                                            <Model>
                                                <ext:Model runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="name" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column runat="server"
                                                DataIndex="name" />
                                        </Columns>
                                    </ColumnModel>
                                </ext:GridPanel>
                            </Component>
                        </ext:DropDownField>
                        <ext:TextField runat="server"
                            ID="uxAddInventoryRate"
                            FieldLabel="Rate" />
                        <ext:ComboBox runat="server"
                            ID="uxAddInventoryMeasure"
                            FieldLabel="Unit of Measure">
                            <Store>
                                <ext:Store runat="server"
                                    ID="uxAddInventoryMeasureStore">
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
            Hidden="true"
            StyleSpec="z-index:10000">
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
                                    ID="uxEditInventoryMixGrid"
                                    StyleSpec="z-index: 100000">
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
                            FieldLabel="Select Region">
                            <Store>
                                <ext:Store runat="server" 
                                    ID="uxEditInventoryRegionStore">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="REGION_ID" />
                                                <ext:ModelField Name="NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                        </ext:ComboBox>
                        <ext:ComboBox runat="server"
                            ID="uxEditInventorySub"
                            FieldLabel="Select Subinventory">
                            <Store>
                                <ext:Store runat="server"
                                    ID="uxEditInventorySubStore">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="SUBINVENTORY_ID" />
                                                <ext:ModelField Name="NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                        </ext:ComboBox>
                        <ext:DropDownField runat="server"
                            ID="uxEditInventoryItem"
                            FieldLabel="Select Item">
                            <Component>
                                <ext:GridPanel runat="server"
                                    ID="uxEditInventoryItemGrid">
                                    <Store>
                                        <ext:Store runat="server"
                                            ID="uxEditInventoryItemStore">
                                            <Model>
                                                <ext:Model runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="name" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column runat="server"
                                                DataIndex="name" />
                                        </Columns>
                                    </ColumnModel>
                                </ext:GridPanel>
                            </Component>
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

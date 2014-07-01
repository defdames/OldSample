<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddEditInventory_DBI.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umAddEditInventory_DBI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var doAddMath = function () {
            var gallonStart = parseInt(App.uxAddInventoryMixGrid.getSelectionModel().getSelection()[0].data.GALLON_STARTING);
            var gallonMixed = parseInt(App.uxAddInventoryMixGrid.getSelectionModel().getSelection()[0].data.GALLON_MIXED);
            var gallonRemain = parseInt(App.uxAddInventoryMixGrid.getSelectionModel().getSelection()[0].data.GALLON_REMAINING);
            var gallonAcre = parseInt(App.uxAddInventoryMixGrid.getSelectionModel().getSelection()[0].data.GALLON_ACRE);

            var gallonsUsed = gallonStart + gallonMixed - gallonRemain;
            var acresSprayed = gallonsUsed / gallonAcre;
            var rate = parseInt(App.uxAddInventoryRate.value);
            App.uxAddInventoryTotal.setValue(rate * acresSprayed);
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
        <ext:Store runat="server"
            ID="uxAddInventoryItemStore"
            RemoteSort="true"
            OnReadData="deReadItems"
            PageSize="10"
            AutoLoad="false">
            <Model>
                <ext:Model ID="uxAddInventoryItemModel" runat="server">
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
                <ext:StoreParameter Name="OrgId" Value="#{uxAddInventoryRegion}.value" Mode="Raw" />
            </Parameters>
        </ext:Store>
        <ext:FormPanel runat="server"
            ID="uxAddInventoryForm"
            DefaultButton="uxAddInventorySubmit">
            <Items>
                <ext:Hidden runat="server" ID="uxFormType" />
                <ext:DropDownField runat="server" Editable="false"
                    ID="uxAddInventoryMix"
                    Mode="ValueText"
                    FieldLabel="Select Mix"
                    AllowBlank="false" Width="500">
                    <Component>
                        <ext:GridPanel runat="server"
                            ID="uxAddInventoryMixGrid"
                            Layout="HBoxLayout">
                            <Store>
                                <ext:Store runat="server"
                                    ID="uxAddInventoryMixStore">
                                    <Model>
                                        <ext:Model ID="Model1" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="CHEMICAL_MIX_ID" />
                                                <ext:ModelField Name="CHEMICAL_MIX_NUMBER" />
                                                <ext:ModelField Name="HEADER_ID" />
                                                <ext:ModelField Name="TARGET_AREA" />
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
                                    <ext:Column ID="Column1" runat="server"
                                        DataIndex="CHEMICAL_MIX_NUMBER"
                                        Text="Mix #" Flex="20" />
                                    <ext:Column ID="Column2" runat="server"
                                        DataIndex="TARGET_AREA"
                                        Text="Target Area" Flex="40" />
                                    <ext:Column ID="Column3" runat="server"
                                        DataIndex="GALLON_ACRE"
                                        Text="Gallons / Acre" Flex="40" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" Mode="Single" />
                            </SelectionModel>
                            <DirectEvents>
                                <Select OnEvent="deStoreChemicalData">
                                    <ExtraParams>
                                        <ext:Parameter Name="MixId" Value="#{uxAddInventoryMixGrid}.getSelectionModel().getSelection()[0].data.CHEMICAL_MIX_ID" Mode="Raw" />
                                        <ext:Parameter Name="MixNumber" Value="#{uxAddInventoryMixGrid}.getSelectionModel().getSelection()[0].data.CHEMICAL_MIX_NUMBER" Mode="Raw" />
                                        <ext:Parameter Name="Type" Value="Add" />
                                    </ExtraParams>
                                </Select>
                            </DirectEvents>
                            <Listeners>
                                <Select Fn="doAddMath" />
                            </Listeners>
                        </ext:GridPanel>
                    </Component>
                </ext:DropDownField>
                <ext:ComboBox runat="server"
                    ID="uxAddInventoryRegion"
                    FieldLabel="Select Region"
                    DisplayField="INV_NAME"
                    ValueField="ORGANIZATION_ID"
                    QueryMode="Local"
                    TypeAhead="true"
                    AllowBlank="false"
                    ForceSelection="true" Width="500">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxAddInventoryRegionStore">
                            <Model>
                                <ext:Model ID="Model2" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="INV_NAME" />
                                        <ext:ModelField Name="ORGANIZATION_ID" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <DirectEvents>
                        <Select OnEvent="deLoadSubinventory" />
                    </DirectEvents>
                </ext:ComboBox>
                <ext:ComboBox runat="server"
                    ID="uxAddInventorySub"
                    FieldLabel="Select Subinventory"
                    ValueField="ORG_ID"
                    DisplayField="SECONDARY_INV_NAME"
                    QueryMode="Local"
                    TypeAhead="true"
                    AllowBlank="false"
                    ForceSelection="true" Width="500">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxAddInventorySubStore">
                            <Model>
                                <ext:Model ID="Model3" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ORG_ID" />
                                        <ext:ModelField Name="SECONDARY_INV_NAME" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                </ext:ComboBox>
                <ext:DropDownField runat="server" Editable="false"
                    ID="uxAddInventoryItem"
                    FieldLabel="Select Item"
                    Mode="ValueText"
                    AllowBlank="false" Width="500">
                    <Component>
                        <ext:GridPanel runat="server"
                            ID="uxAddInventoryItemGrid" StoreID="uxAddInventoryItemStore">
                            <ColumnModel>
                                <Columns>
                                    <ext:Column ID="Column4" runat="server"
                                        DataIndex="SEGMENT1"
                                        Text="Item Id" Flex="20" />
                                    <ext:Column ID="Column5" runat="server"
                                        DataIndex="DESCRIPTION" Flex="50"
                                        Text="Name" />
                                    <ext:Column ID="Column6" runat="server"
                                        DataIndex="UOM_CODE"
                                        Text="Measure" Flex="30" />
                                </Columns>
                            </ColumnModel>
                            <Plugins>
                                <ext:FilterHeader runat="server" ID="uxAddInventoryItemHeadFilter" Remote="true" />
                            </Plugins>
                            <BottomBar>
                                <ext:PagingToolbar runat="server" ID="uxAddInventoryItemPaging" />
                            </BottomBar>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" Mode="Single" />
                            </SelectionModel>
                            <DirectEvents>
                                <Select OnEvent="deStoreGridValue">
                                    <ExtraParams>
                                        <ext:Parameter Name="ItemId" Value="#{uxAddInventoryItemGrid}.getSelectionModel().getSelection()[0].data.ITEM_ID" Mode="Raw" />
                                        <ext:Parameter Name="Description" Value="#{uxAddInventoryItemGrid}.getSelectionModel().getSelection()[0].data.DESCRIPTION" Mode="Raw" />
                                    </ExtraParams>
                                </Select>
                            </DirectEvents>
                        </ext:GridPanel>
                    </Component>
                    <DirectEvents>
                        <Change OnEvent="deGetUnitOfMeasure">
                            <ExtraParams>
                                <ext:Parameter Name="uomCode" Value="#{uxAddInventoryItemGrid}.getSelectionModel().getSelection()[0].data.UOM_CODE" Mode="Raw" />
                            </ExtraParams>
                        </Change>
                    </DirectEvents>
                    <Listeners>
                        <Expand Handler="#{uxAddInventoryItemStore}.reload()" />
                    </Listeners>
                </ext:DropDownField>
                <ext:NumberField runat="server"
                    ID="uxAddInventoryRate"
                    FieldLabel="Rate"
                    AllowBlank="false" Width="500">
                    <Listeners>
                        <Change Fn="doAddMath" />
                    </Listeners>
                </ext:NumberField>
                <ext:ComboBox runat="server"
                    ID="uxAddInventoryMeasure"
                    FieldLabel="Unit of Measure"
                    ValueField="UOM_CODE"
                    DisplayField="UNIT_OF_MEASURE"
                    QueryMode="Local"
                    TypeAhead="true"
                    AllowBlank="false"
                    ForceSelection="true" Width="500">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxAddInventoryMeasureStore">
                            <Model>
                                <ext:Model ID="Model4" runat="server">
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
                <ext:NumberField runat="server"
                    ID="uxAddInventoryTotal"
                    FieldLabel="Total" Width="500" />
                <ext:TextField runat="server"
                    ID="uxAddInventoryEPA"
                    FieldLabel="EPA Number"
                    AllowBlank="false" Width="500" />
                <ext:Checkbox runat="server" FieldLabel="Customer Material" ID="uxAddInventoryContractor" />
            </Items>
            <Buttons>
                <ext:Button runat="server"
                    ID="uxAddInventorySubmit"
                    Icon="Add"
                    Text="Save"
                    Disabled="true">
                    <DirectEvents>
                        <Click OnEvent="deProcessForm">
                            <ExtraParams>
                                <ext:Parameter Name="SecondaryInvName" Value="#{uxAddInventorySub}.getRawValue()" Mode="Raw" />
                            </ExtraParams>
                            <EventMask ShowMask="true" />
                        </Click>
                    </DirectEvents>
                </ext:Button>
                <ext:Button runat="server"
                    ID="uxAddInventoryCancel"
                    Icon="Delete"
                    Text="Cancel">
                    <Listeners>
                        <Click Handler="#{uxAddInventoryForm}.reset();
									parentAutoLoadControl.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
            <Listeners>
                <ValidityChange Handler="#{uxAddInventorySubmit}.setDisabled(!valid);" />
                <AfterRender
                    Handler="var win = parentAutoLoadControl.target || parentAutoLoadControl, //you can use just 'parentAutoLoadControl' after update to Ext.NET v2 beta.
									size = this.getSize();
                                size.height += 34;
								size.width += 24;
								win.setSize(size);"
                    Delay="100" />
            </Listeners>
        </ext:FormPanel>
    </form>
</body>
</html>

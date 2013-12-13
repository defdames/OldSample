<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umInventoryTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umInventoryTab" %>

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
    <link href="../../../Resources/StyleSheets/main.css" rel="stylesheet" />
</head>
<body>
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
	<ext:Store runat="server"
		ID="uxAddInventoryItemStore"
		RemoteSort="true"
		OnReadData="deReadItems"
		PageSize="10">
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
		</Parameters>
	</ext:Store>
	<ext:Store runat="server"
		ID="uxEditInventoryItemStore"
		RemoteSort="true"
		OnReadData="deReadItems"
		PageSize="10">
		<Model>
			<ext:Model ID="uxEditInventoryItemModel" runat="server">
				<Fields>
					<ext:ModelField Name="ITEM_ID" Type="String" />
					<ext:ModelField Name="SEGMENT1" Type="String" />
					<ext:ModelField Name="DESCRIPTION" Type="String" />
					<ext:ModelField Name="UOM_CODE" Type="String" />
					<ext:ModelField Name="ENABLED_FLAG" />
					<ext:ModelField Name="ACTIVE" />
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
	<form id="form1" runat="server">
		<ext:GridPanel runat="server"
			ID="uxCurrentInventoryGrid"
			Layout="HBoxLayout">
			<Store>
				<ext:Store runat="server"
					ID="uxCurrentInventoryStore"
					AutoDataBind="true">
					<Model>
						<ext:Model runat="server" ID="uxCurrentInventoryModel">
							<Fields>
								<ext:ModelField Name="INVENTORY_ID" Type="Int" />
								<ext:ModelField Name="CHEMICAL_MIX_ID" Type="Int" />
								<ext:ModelField Name="CHEMICAL_MIX_NUMBER" Type="String" />
								<ext:ModelField Name="SUB_INVENTORY_SECONDARY_NAME" Type="String" />
								<ext:ModelField Name="SUB_INVENTORY_ORG_ID" Type="Float" />
								<ext:ModelField Name="SEGMENT1" Type="String" />
								<ext:ModelField Name="ITEM_ID" Type="Float" />
								<ext:ModelField Name="DESCRIPTION" Type="String" />
								<ext:ModelField Name="RATE" Type="Float" />
								<ext:ModelField Name="UOM_CODE" Type="String" />
								<ext:ModelField Name="UNIT_OF_MEASURE" Type="String" />
								<ext:ModelField Name="EPA_NUMBER" Type="String" />
								<ext:ModelField Name="INV_NAME" Type="String" />
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
					<ext:Column runat="server"
						DataIndex="CHEMICAL_MIX_ID"
						Text="Mix Id" Hidden="true" />
					<ext:Column runat="server"
						DataIndex="CHEMICAL_MIX_NUMBER"
						Text="Mix Number" />
					<ext:Column runat="server"
						DataIndex="SUB_INVENTORY_SECONDARY_NAME"
						Text="Subinventory Name" />
					<ext:Column runat="server"
						DataIndex="SUB_INVENTORY_ORG_ID"
						Text="Org Id" Hidden="true" />
					<ext:Column runat="server"
						DataIndex="SEGMENT1"
						Text="Inventory Number" Hidden="true" />
					<ext:Column runat="server"
						DataIndex="ITEM_ID"
						Text="Item Id" Hidden="true" />
					<ext:Column runat="server"
						DataIndex="DESCRIPTION"
						Text="Item" />
					<ext:Column runat="server"
						DataIndex="RATE"
						Text="Rate" />
					<ext:Column runat="server"
						DataIndex="UOM_CODE"
						Text="Unit of Measure Code" Hidden="true" />
					<ext:Column runat="server"
						DataIndex="UNIT_OF_MEASURE"
						Text="Unit" />
					<ext:Column runat="server"
						DataIndex="EPA_NUMBER"
						Text="EPA Number" />
					<ext:Column runat="server"
						DataIndex="INV_NAME"
						Text="Inventory Name" Hidden="true" />
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
						<ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" />
						<ext:Button runat="server"
							ID="uxEditInventoryButton"
							Text="Edit Inventory"
							Icon="ApplicationEdit"
							Disabled="true">
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
						<ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
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
				<ext:RowSelectionModel runat="server" Mode="Single" AllowDeselect="true" />
			</SelectionModel>
			<Listeners>
				<Select Handler="#{uxEditInventoryButton}.enable()" />
				<Deselect Handler="#{uxEditInventoryButton}.disable()" />
			</Listeners>
		</ext:GridPanel>   
		<%-- Hidden Windows --%>
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
							FieldLabel="Select Mix"
							AllowBlank="false">
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
											<ext:Column runat="server"
												DataIndex="CHEMICAL_MIX_NUMBER"
												Text="Mix #" Flex="20" />
											<ext:Column ID="Column1" runat="server"
												DataIndex="TARGET_AREA" 
												Text="Target Area" Flex="40" />                 
											<ext:Column runat="server"
												DataIndex="GALLON_ACRE"
												Text="Gallons / Acre" Flex="40"  />                                                             
										</Columns>
									</ColumnModel>
									<SelectionModel>
										<ext:RowSelectionModel runat="server" Mode="Single" />
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
							AllowBlank="false">
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
							ValueField="ORG_ID"
							DisplayField="SECONDARY_INV_NAME"
							QueryMode="Local"
							TypeAhead="true"
							AllowBlank="false">
							<Store>
								<ext:Store runat="server"
									ID="uxAddInventorySubStore">
									<Model>
										<ext:Model runat="server">
											<Fields>
												<ext:ModelField Name="ORG_ID" />
												<ext:ModelField Name="SECONDARY_INV_NAME" />
											</Fields>
										</ext:Model>
									</Model>
								</ext:Store>
							</Store>
						</ext:ComboBox>
						<ext:DropDownField runat="server"
							ID="uxAddInventoryItem"
							FieldLabel="Select Item"
							Mode="ValueText"
							AllowBlank="false">
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
									<DirectEvents>
										<Select OnEvent="deStoreGridValue">
											<ExtraParams>
												<ext:Parameter Name="ItemId" Value="#{uxAddInventoryItemGrid}.getSelectionModel().getSelection()[0].data.ITEM_ID" Mode="Raw" />
												<ext:Parameter Name="Description" Value="#{uxAddInventoryItemGrid}.getSelectionModel().getSelection()[0].data.DESCRIPTION" Mode="Raw" />
												<ext:Parameter Name="Type" Value="Add" />
											</ExtraParams>
										</Select>
									</DirectEvents>
								</ext:GridPanel>
							</Component>                            
							<DirectEvents>
								<Change OnEvent="deGetUnitOfMeasure">
									<ExtraParams>
										<ext:Parameter Name="uomCode" Value="#{uxAddInventoryItemGrid}.getSelectionModel().getSelection()[0].data.UOM_CODE" Mode="Raw" />
										<ext:Parameter Name="Type" Value="Add" />
									</ExtraParams>
								</Change>
							</DirectEvents>
						</ext:DropDownField>
						<ext:TextField runat="server"
							ID="uxAddInventoryRate"
							FieldLabel="Rate"
							AllowBlank="false">
							<Listeners>
								<Change Handler="doAddMath" />
							</Listeners>
						</ext:TextField>
						<ext:ComboBox runat="server"
							ID="uxAddInventoryMeasure"
							FieldLabel="Unit of Measure"
							ValueField="UOM_CODE"
							DisplayField="UNIT_OF_MEASURE"
							QueryMode="Local"
							TypeAhead="true"
							AllowBlank="false">
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
							FieldLabel="EPA Number"
							AllowBlank="false" />
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxAddInventorySubmit"
							Icon="Add"
							Text="Submit"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deAddInventory">
									<ExtraParams>
										<ext:Parameter Name="SecondaryInvName" Value="#{uxAddInventorySub}.getRawValue()" Mode="Raw" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxAddInventoryCancel"
							Icon="Delete"
							Text="Cancel">
							<Listeners>
								<Click Handler="#{uxAddInventoryForm}.reset();
									#{uxAddInventoryWindow}.hide()" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxAddInventorySubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>
			</Items>
			<Listeners>
				<Show Handler="#{uxAddInventoryMix}.focus()" />
			</Listeners>
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
						<ext:TextField runat="server"
							ID="uxEditInventoryMix"
							Disabled="true" 
							FieldLabel="Mix Number"
							AllowBlank="false" />
						<ext:ComboBox runat="server"
							ID="uxEditInventoryRegion"
							FieldLabel="Select Region"
							DisplayField="INV_NAME"
							ValueField="ORGANIZATION_ID"
							QueryMode="Local"
							TypeAhead="true"
							AllowBlank="false" >
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
							DisplayField="SECONDARY_INV_NAME"
							QueryMode="Local"
							TypeAhead="true"
							AllowBlank="false">
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
						</ext:ComboBox>
						<ext:DropDownField runat="server"
							ID="uxEditInventoryItem"
							FieldLabel="Select Item"
							Mode="ValueText"
							AllowBlank="false">
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
									<DirectEvents>
										<Select OnEvent="deStoreGridValue">
											<ExtraParams>
												<ext:Parameter Name="ItemId" Value="#{uxEditInventoryItemGrid}.getSelectionModel().getSelection()[0].data.ITEM_ID" Mode="Raw" />
												<ext:Parameter Name="Description" Value="#{uxEditInventoryItemGrid}.getSelectionModel().getSelection()[0].data.DESCRIPTION" Mode="Raw" />
												<ext:Parameter Name="Type" Value="Edit" />
											</ExtraParams>
										</Select>
									</DirectEvents>
								</ext:GridPanel>
							</Component>
							<DirectEvents>
								<Change OnEvent="deGetUnitOfMeasure">
									<ExtraParams>
										<ext:Parameter Name="uomCode" Value="#{uxEditInventoryItemGrid}.getSelectionModel().getSelection()[0].data.UOM_CODE" Mode="Raw" />
										<ext:Parameter Name="Type" Value="Edit" />
									</ExtraParams>
								</Change>
							</DirectEvents>
						</ext:DropDownField>
						<ext:TextField runat="server"
							ID="uxEditInventoryRate"
							FieldLabel="Rate"
							AllowBlank="false">
							<DirectEvents>
								<Change OnEvent="deEditMath" />
							</DirectEvents>
						</ext:TextField>
						<ext:ComboBox runat="server"
							ID="uxEditInventoryMeasure"
							FieldLabel="Unit of Measure"
							DisplayField="UNIT_OF_MEASURE"
							ValueField="UOM_CODE"
							QueryMode="Local"
							TypeAhead="true"
							AllowBlank="false">
							<Store>
								<ext:Store runat="server"
									ID="uxEditInventoryMeasureStore">
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
							ID="uxEditInventoryTotal"
							Disabled="true"
							FieldLabel="Total" />
						<ext:TextField runat="server"
							ID="uxEditInventoryEPA"
							FieldLabel="EPA Number"
							AllowBlank="false" />
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxEditInventorySubmit"
							Icon="Add"
							Text="Submit">
							<DirectEvents>
								<Click OnEvent="deEditInventory">
									<ExtraParams>
										<ext:Parameter Name="InventoryId" Value="#{uxCurrentInventoryGrid}.getSelectionModel().getSelection()[0].data.INVENTORY_ID" Mode="Raw" />
										<ext:Parameter Name="ChemicalId" Value="#{uxCurrentInventoryGrid}.getSelectionModel().getSelection()[0].data.CHEMICAL_MIX_ID" Mode="Raw" />
										<ext:Parameter Name="SecondaryInvName" Value="#{uxEditInventorySub}.getRawValue()" Mode="Raw" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxEditInventoryCancel"
							Icon="Delete"
							Text="Cancel">
							<Listeners>
								<Click Handler="#{uxEditInventoryForm}.reset();
									#{uxEditInventoryWindow}.hide()" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxEditEmployeeSubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>
			</Items>
			<Listeners>
				<Show Handler="#{uxEditInventoryMix}.focus()" />
			</Listeners>
		</ext:Window>
	</form>
</body>
</html>

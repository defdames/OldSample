<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddEditInventory_IRM.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umAddEditInventory_IRM" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
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
		<ext:Panel runat="server" ID="uxAddEditPanel" Layout="FitLayout">
			<Items>
				<ext:FormPanel runat="server"
					ID="uxAddInventoryForm"
					Layout="FormLayout"
					Hidden="true">
					<Items>
						<ext:ComboBox runat="server"
							ID="uxAddInventoryRegion"
							FieldLabel="Select Region"
							DisplayField="INV_NAME"
							ValueField="ORGANIZATION_ID"
							QueryMode="Local"
							TypeAhead="true"
							AllowBlank="false"
							ForceSelection="true">
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
							AllowBlank="false"
							ForceSelection="true">
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
											<ext:Column ID="Column17" runat="server"
												DataIndex="SEGMENT1"
												Text="Item Id" />
											<ext:Column ID="Column18" runat="server"
												DataIndex="DESCRIPTION"
												Text="Name" />
											<ext:Column ID="Column19" runat="server"
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
										<ext:RowSelectionModel ID="RowSelectionModel3" runat="server" Mode="Single" />
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
						<ext:NumberField runat="server"
							ID="uxAddInventoryRate"
							FieldLabel="Quantity"
							AllowBlank="false">
						</ext:NumberField>
						<ext:ComboBox runat="server"
							ID="uxAddInventoryMeasure"
							FieldLabel="Unit of Measure"
							ValueField="UOM_CODE"
							DisplayField="UNIT_OF_MEASURE"
							QueryMode="Local"
							TypeAhead="true"
							AllowBlank="false"
							ForceSelection="true">
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
									parentAutoLoadControl.hide();" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxAddInventorySubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>
				<ext:FormPanel runat="server"
					ID="uxEditInventoryForm"
					Layout="FormLayout"
					Hidden="true">
					<Items>
						<ext:ComboBox runat="server"
							ID="uxEditInventoryRegion"
							FieldLabel="Select Region"
							DisplayField="INV_NAME"
							ValueField="ORGANIZATION_ID"
							QueryMode="Local"
							TypeAhead="true"
							AllowBlank="false"
							ForceSelection="true" >
							<Store>
								<ext:Store runat="server" 
									ID="uxEditInventoryRegionStore">
									<Model>
										<ext:Model ID="Model5" runat="server">
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
							AllowBlank="false"
							ForceSelection="true">
							<Store>
								<ext:Store runat="server"
									ID="uxEditInventorySubStore">
									<Model>
										<ext:Model ID="Model6" runat="server">
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
											<ext:Column ID="Column20" runat="server"
												DataIndex="SEGMENT1"
												Text="Item Id" />
											<ext:Column ID="Column21" runat="server"
												DataIndex="DESCRIPTION"
												Text="Name" />
											<ext:Column ID="Column22" runat="server"
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
						</ext:DropDownField>
						<ext:NumberField runat="server"
							ID="uxEditInventoryRate"
							FieldLabel="Quantity"
							AllowBlank="false" />
						<ext:ComboBox runat="server"
							ID="uxEditInventoryMeasure"
							FieldLabel="Unit of Measure"
							ValueField="UOM_CODE"
							DisplayField="UNIT_OF_MEASURE"
							QueryMode="Local"
							TypeAhead="true"
							AllowBlank="false"
							ForceSelection="true">
							<Store>
								<ext:Store runat="server"
									ID="uxEditInventoryMeasureStore">
									<Model>
										<ext:Model ID="Model1" runat="server">
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
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxEditInventorySubmit"
							Icon="Add"
							Text="Submit">
							<DirectEvents>
								<Click OnEvent="deEditInventory">
									<ExtraParams>
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
									parentAutoLoadControl.hide();" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxEditEmployeeSubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>
			</Items>
			<Listeners>
				<AfterRender
					Handler="var win = parentAutoLoadControl.target || parentAutoLoadControl, //you can use just 'parentAutoLoadControl' after update to Ext.NET v2 beta.
									size = this.getSize();
 
								size.height += 250;
								size.width += 12;
								win.setSize(size);"
					Delay="100" />
			</Listeners>
		</ext:Panel>
	</form>
</body>
</html>

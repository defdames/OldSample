<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umInventoryTab_DBI.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umInventoryTab_DBI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<link href="../../../Resources/StyleSheets/main.css" rel="stylesheet" />
</head>
<body>
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
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
								<ext:ModelField Name="TOTAL" Type="Float" />
								<ext:ModelField Name="UOM_CODE" Type="String" />
								<ext:ModelField Name="UNIT_OF_MEASURE" Type="String" />
								<ext:ModelField Name="EPA_NUMBER" Type="String" />
								<ext:ModelField Name="INV_NAME" Type="String" />
								<ext:ModelField Name="CONTRACTOR_SUPPLIED" Type="Boolean"  />
							</Fields>
						</ext:Model>
					</Model>
				</ext:Store>
			</Store>
			<ColumnModel>
				<Columns>
					<ext:Column runat="server"
						DataIndex="SEGMENT1"
						Text="Item ID" />
					<ext:Column runat="server"
						DataIndex="CHEMICAL_MIX_ID"
						Text="Mix Id" Hidden="true" />
					<ext:Column runat="server"
						DataIndex="CHEMICAL_MIX_NUMBER"
						Text="Mix Number" />
					<ext:Column runat="server"
						DataIndex="INV_NAME"
						Text="Inventory Org" />
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
						DataIndex="TOTAL"
						Text="Total" />
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
					<ext:CheckColumn runat="server"
						DataIndex="CONTRACTOR_SUPPLIED"
						Text="Contractor Supplied" />
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
								<Click OnEvent="deLoadInventoryWindow">
									<ExtraParams>
										<ext:Parameter Name="type" Value="Add" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" />
						<ext:Button runat="server"
							ID="uxEditInventoryButton"
							Text="Edit Inventory"
							Icon="ApplicationEdit"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deLoadInventoryWindow">
									<ExtraParams>
										<ext:Parameter Name="InventoryId" Value="#{uxCurrentInventoryGrid}.getSelectionModel().getSelection()[0].data.INVENTORY_ID" Mode="Raw" />
										<ext:Parameter Name="type" Value="Edit" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
						<ext:Button runat="server"
							ID="uxRemoveInventoryButton"
							Text="Remove Inventory"
							Icon="ApplicationDelete"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deRemoveInventory">
									<Confirmation ConfirmRequest="true" Title="Really Delete?" Message="Do you really want to delete?" />
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
				<Select Handler="#{uxEditInventoryButton}.enable();
					#{uxRemoveInventoryButton}.enable()" />
				<Deselect Handler="#{uxEditInventoryButton}.disable();
					#{uxRemoveInventoryButton}.disable()" />
			</Listeners>
		</ext:GridPanel>
	</form>
</body>
</html>

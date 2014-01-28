<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umChemicalTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umChemicalTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<script type="text/javascript">
		var updateAddTotalAndUsed = function () {
			App.uxAddChemicalGallonTotal.setValue(parseInt(App.uxAddChemicalGallonStart.value) + parseInt(App.uxAddChemicalGallonMixed.value));
			App.uxAddChemicalGallonUsed.setValue(parseInt(App.uxAddChemicalGallonTotal.value) - parseInt(App.uxAddChemicalGallonRemain.value));
			App.uxAddChemicalAcresSprayed.setValue(parseInt(App.uxAddChemicalGallonUsed.value) / parseInt(App.uxAddChemicalGallonAcre.value));
		};

		var updateEditTotalAndUsed = function () {
			App.uxEditChemicalGallonTotal.setValue(parseInt(App.uxEditChemicalGallonStart.value) + parseInt(App.uxEditChemicalGallonMixed.value));
			App.uxEditChemicalGallonUsed.setValue(parseInt(App.uxEditChemicalGallonTotal.value) - parseInt(App.uxEditChemicalGallonRemain.value));
			App.uxEditChemicalAcresSprayed.setValue(parseInt(App.uxEditChemicalGallonUsed.value) / parseInt(App.uxEditChemicalGallonAcre.value));
		};

		var doMath = function () {
			var models = App.uxCurrentChemicalStore.getRange();
			var count = App.uxCurrentChemicalGrid.getStore().getCount();
			for (var i = 0; i < count; i++){
				var total = App.uxCurrentChemicalStore.getAt(i).data.GALLON_STARTING + App.uxCurrentChemicalStore.getAt(i).data.GALLON_MIXED;
				models[i].set("GALLON_TOTAL", total);
				var used = total - App.uxCurrentChemicalStore.getAt(i).data.GALLON_REMAINING;
				models[i].set("GALLON_USED", used);
				var sprayed = used / App.uxCurrentChemicalStore.getAt(i).data.GALLON_ACRE;
				models[i].set("ACRES_SPRAYED", sprayed);

			}
		};
	</script>
	<link href="../../../Resources/StyleSheets/main.css" rel="stylesheet" />
</head>
<body>
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
	<form id="form1" runat="server">
		<ext:GridPanel runat="server"
			ID="uxCurrentChemicalGrid"
			Layout="HBoxLayout">
			<Store>
				<ext:Store runat="server"
					ID="uxCurrentChemicalStore"
					AutoDataBind="true" WarningOnDirty="false">
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
					<Listeners>
						<Load Fn="doMath" />
					</Listeners>
				</ext:Store>
			</Store>
			<ColumnModel>
				<Columns>
					<ext:Column runat="server" DataIndex="CHEMICAL_MIX_NUMBER" Text="Mix Number" Flex="1"/>
					<ext:Column runat="server" DataIndex="TARGET_AREA" Text="Target" Flex="1" />
					<ext:Column runat="server" DataIndex="GALLON_ACRE" Text="Gallons / Acre" Flex="1" />
					<ext:Column runat="server" DataIndex="GALLON_STARTING" Text="Gallons Starting" Flex="1" />
					<ext:Column runat="server" DataIndex="GALLON_MIXED" Text="Gallons Mixed" Flex="1" />
					<ext:Column runat="server" ID="uxGallonTotalGrid" Text="Gallons Total" DataIndex="GALLON_TOTAL" Flex="1" />
					<ext:Column runat="server" DataIndex="GALLON_REMAINING" Text="Gallons Remaining" Flex="1" />
					<ext:Column runat="server" ID="uxGallonUsedGrid" Text="Gallons Used" DataIndex="GALLON_USED" Flex="1" />
					<ext:Column runat="server" ID="uxAcresSprayedGrid" Text="Acres Sprayed" DataIndex="ACRES_SPRAYED" Flex="1" />
					<ext:Column runat="server" DataIndex="STATE" Text="State" Flex="1" />
					<ext:Column runat="server" DataIndex="COUNTY" Text="County" Flex="1" />
				</Columns>
			</ColumnModel>
			<TopBar>
				<ext:Toolbar runat="server">
					<Items>
						<ext:Button runat="server"
							ID="uxAddChemicalButton"
							Icon="ApplicationAdd"
							Text="Add Chemical Mix">
							<DirectEvents>
								<Click OnEvent="deLoadChemicalWindow">
									<ExtraParams>
										<ext:Parameter Name="type" Value="Add" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" />
						<ext:Button runat="server"
							ID="uxEditChemicalButton"
							Icon="ApplicationEdit"
							Text="Edit Chemical Mix"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deLoadChemicalWindow">
									<ExtraParams>
										<ext:Parameter Name="type" Value="Edit" />
										<ext:Parameter Name="ChemicalId" Value="#{uxCurrentChemicalGrid}.getSelectionModel().getSelection()[0].data.CHEMICAL_MIX_ID" Mode="Raw" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
						<ext:Button runat="server"
							ID="uxRemoveChemicalButton"
							Icon="ApplicationDelete"
							Text="Remove Chemical Mix"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deRemoveChemical">
									<Confirmation ConfirmRequest="true" Title="Remove?" Message="Do you really want to remove?" />
									<ExtraParams>
										<ext:Parameter Name="ChemicalId" Value="#{uxCurrentChemicalGrid}.getSelectionModel().getSelection()[0].data.CHEMICAL_MIX_ID" Mode="Raw" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
					</Items>
				</ext:Toolbar>
			</TopBar>
			<SelectionModel>
				<ext:RowSelectionModel runat="server" AllowDeselect="true" Mode="Single" />
			</SelectionModel>
			<Listeners>
				<Select Handler="#{uxEditChemicalButton}.enable();
					#{uxRemoveChemicalButton}.enable()" />
				<Deselect Handler="#{uxEditChemicalButton}.disable();
					#{uxRemoveChemicalButton}.disable()" />
			</Listeners>
		</ext:GridPanel>
	</form>
</body>
</html>

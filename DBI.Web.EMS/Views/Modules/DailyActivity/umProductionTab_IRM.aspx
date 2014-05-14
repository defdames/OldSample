<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umProductionTab_IRM.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umProductionTab_IRM" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title></title>
	<script type="text/javascript">
		var onShow = function (toolTip, grid) {
			var view = grid.getView(),
				store = grid.getStore(),
				record = view.getRecord(view.findItemByChild(toolTip.triggerElement)),
				column = view.getHeaderByCell(toolTip.triggerElement),
				data = record.get(column.dataIndex);

			toolTip.update(data);
		};
	</script>
	<link href="../../../Resources/StyleSheets/main.css" rel="stylesheet" />
</head>
<body>
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
	<form id="form1" runat="server">
		<ext:GridPanel runat="server"
			ID="uxCurrentProductionGrid"
			Layout="HBoxLayout">
			<Store>
				<ext:Store runat="server"
					ID="uxCurrentProductionStore"
					AutoDataBind="true">
					<Model>
						<ext:Model ID="Model1" runat="server">
							<Fields>
								<ext:ModelField Name="PRODUCTION_ID" />
								<ext:ModelField Name="PROJECT_ID" />
								<ext:ModelField Name="LONG_NAME" />
								<ext:ModelField Name="TASK_ID" />
								<ext:ModelField Name="DESCRIPTION" />
								<ext:ModelField Name="STATION" />
								<ext:ModelField Name="EXPENDITURE_TYPE" />
								<ext:ModelField Name="BILL_RATE" />
								<ext:ModelField Name="UNIT_OF_MEASURE" />
								<ext:ModelField Name="SURFACE_TYPE" />
								<ext:ModelField Name="COMMENTS" />
								<ext:ModelField Name="QUANTITY" />
							</Fields>
						</ext:Model>
					</Model>
				</ext:Store>
			</Store>
			<ColumnModel>
				<Columns>
					<ext:Column runat="server"
						DataIndex="LONG_NAME"
						Text="Project Name"
						Flex="2" />
					<ext:Column runat="server"
						DataIndex="DESCRIPTION"
						Text="Task Name"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="STATION"
						Text="Station"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="EXPENDITURE_TYPE"
						Text="Expenditure Type"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="QUANTITY"
						Text="Quantity"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="BILL_RATE"
						Text="Bill Rate"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="UNIT_OF_MEASURE"
						Text="Unit Of Measure"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="SURFACE_TYPE"
						Text="Surface Type"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="COMMENTS"
						Text="Comments"
						Flex="1" />
				</Columns>
			</ColumnModel>
			<TopBar>
				<ext:Toolbar ID="Toolbar1" runat="server">
					<Items>
						<ext:Button runat="server"
							ID="uxAddProductionButton"
							Text="Add Production"
							Icon="ApplicationAdd">
							<DirectEvents>
								<Click OnEvent="deLoadProductionWindow">
									<ExtraParams>
										<ext:Parameter Name="Type" Value="Add" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" />
						<ext:Button runat="server"
							ID="uxEditProductionButton"
							Text="Edit Production"
							Icon="ApplicationEdit"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deLoadProductionWindow">
									<ExtraParams>
										<ext:Parameter Name="ProductionId" Value="#{uxCurrentProductionGrid}.getSelectionModel().getSelection()[0].data.PRODUCTION_ID" Mode="Raw" />
										<ext:Parameter Name="Type" Value="Edit" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
						<ext:Button runat="server"
							ID="uxRemoveProductionButton"
							Text="Remove Production"
							Icon="ApplicationDelete"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deRemoveProduction">
									<Confirmation ConfirmRequest="true" Title="Really?" Message="Do you really want to remove?" />
									<ExtraParams>
										<ext:Parameter Name="ProductionId" Value="#{uxCurrentProductionGrid}.getSelectionModel().getSelection()[0].data.PRODUCTION_ID" Mode="Raw" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
					</Items>
				</ext:Toolbar>
			</TopBar>
			<SelectionModel>
				<ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="true" Mode="Single" />
			</SelectionModel>
			<Listeners>
				<Select Handler="#{uxEditProductionButton}.enable();
					#{uxRemoveProductionButton}.enable()" />
				<Deselect Handler="#{uxEditProductionButton}.disable();
					#{uxRemoveProductionButton}.disable()" />
			</Listeners>
		</ext:GridPanel>
		<ext:ToolTip ID="ToolTip1" 
			runat="server" 
			Target="={#{uxCurrentProductionGrid}.getView().el}"
			Delegate=".x-grid-cell"
			TrackMouse="true">
			<Listeners>
				<Show Handler="onShow(this, #{uxCurrentProductionGrid});" /> 
			</Listeners>
		</ext:ToolTip>
	</form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umProductionTab_DBI.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umProductionTab_DBI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
						<ext:Model runat="server">
							<Fields>
								<ext:ModelField Name="PRODUCTION_ID" />
								<ext:ModelField Name="PROJECT_ID" />
								<ext:ModelField Name="LONG_NAME" />
								<ext:ModelField Name="TASK_ID" />
								<ext:ModelField Name="DESCRIPTION" />
								<ext:ModelField Name="WORK_AREA" />
								<ext:ModelField Name="POLE_FROM" />
								<ext:ModelField Name="POLE_TO" />
								<ext:ModelField Name="ACRES_MILE" />
								<ext:ModelField Name="QUANTITY" />
							</Fields>
						</ext:Model>
					</Model>
				</ext:Store>
			</Store>
			<ColumnModel>
				<Columns>
					<ext:Column runat="server"
						DataIndex="PROJECT_ID"
						Text="Project Id"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="LONG_NAME"
						Text="Project Name"
						Flex="2" />
					<ext:Column runat="server"
						DataIndex="DESCRIPTION"
						Text="Task Name"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="WORK_AREA"
						Text="Spray/Work Area"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="POLE_FROM"
						Text="Pole/MP From"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="POLE_TO"
						Text="Pole/MP To"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="ACRES_MILE"
						Text="Acres/Mile"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="QUANTITY"
						Text="Gallons"
						Flex="1" />
				</Columns>
			</ColumnModel>
			<TopBar>
				<ext:Toolbar runat="server">
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
				<ext:RowSelectionModel runat="server" AllowDeselect="true" Mode="Single" />
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

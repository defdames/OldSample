<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEmployeesTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umEmployeesTab" %>

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
			ID="uxCurrentEmployeeGrid"
			Layout="HBoxLayout">
			<Store>
				<ext:Store runat="server"
					ID="uxCurrentEmployeeStore"
					AutoDataBind="true">
					<Model>
						<ext:Model runat="server">
							<Fields>
								<ext:ModelField Name="EMPLOYEE_ID" />
								<ext:ModelField Name="PERSON_ID" />
								<ext:ModelField Name="EMPLOYEE_NAME" Type="String"/>
								<ext:ModelField Name="HEADER_ID" Type="Int" />
								<ext:ModelField Name="EQUIPMENT_ID" Type="Int" />
								<ext:ModelField Name="NAME" Type="String"  />
								<ext:ModelField Name="TIME_IN" Type="Date" />
								<ext:ModelField Name="TIME_OUT" Type="Date" />
								<ext:ModelField Name="TRAVEL_TIME_FORMATTED" Type="String" />
								<ext:ModelField Name="DRIVE_TIME_FORMATTED" Type="String"  />
								<ext:ModelField Name="SHOPTIME_AM_FORMATTED" Type="String"  />
								<ext:ModelField Name="SHOPTIME_PM_FORMATTED" Type="String"  />
								<ext:ModelField Name="SUPPORT_PROJECT" />
								<ext:ModelField Name="PER_DIEM" Type="String"  />
								<ext:ModelField Name="COMMENTS" Type="String"  />
								<ext:ModelField Name="ROLE_TYPE" Type="String" />
							</Fields>
						</ext:Model>
					</Model>
				</ext:Store>                
			</Store>
			<ColumnModel>
				<Columns>
					<ext:Column runat="server"
						Text="Employee ID"
						DataIndex="EMPLOYEE_ID"
						Flex="1" />
					<ext:Column runat="server"
						Text="Name"
						DataIndex="EMPLOYEE_NAME"
						Flex="2" />
					<ext:Column runat="server"
						Text="Equipment Name"
						DataIndex="NAME"
						Flex="1" />
					<ext:DateColumn runat="server"
						Text="Time In"
						Format="M/d/yyyy h:mm tt"
						DataIndex="TIME_IN"
						Flex="1" />
					<ext:DateColumn runat="server"
						Text="Time Out"
						Format="M/d/yyyy h:mm tt"
						DataIndex="TIME_OUT"
						Flex="1" />
					<ext:Column runat="server"
						Text="Travel Time"
						Dataindex="TRAVEL_TIME_FORMATTED"
						Flex="1" />
					<ext:Column runat="server" ID="uxDriveTimeColumn"
						Text="Drive Time"
						DataIndex="DRIVE_TIME_FORMATTED"
						Flex="1" Hidden="true" />
					<ext:Column ID="uxShopTimeAMColumn" runat="server"
						Text="Shop Time AM"
						DataIndex="SHOPTIME_AM_FORMATTED"
						Flex="1"
						Hidden="true" />
					<ext:Column ID="uxShopTimePMColumn" runat="server"
						Text="Shop Time PM"
						DataIndex="SHOPTIME_PM_FORMATTED"
						Flex="1"
						Hidden="true" />
					<ext:Column runat="server"
						ID="uxSupportProjectColumn"
						Text="Support Project"
						DataIndex="SUPPORT_PROJECT"
						Flex="1"
						Hidden="true" />
					<ext:Column runat="server"
						Text="Per Diem"
						DataIndex="PER_DIEM"
						Flex="1" />
					<ext:Column runat="server"
						Text="Comments"
						DataIndex="COMMENTS"
						Flex="1" />
					<ext:Column runat="server"
						Text="Role Type"
						DataIndex="ROLE_TYPE"
						Flex="1" />
				</Columns>
			</ColumnModel>
			<TopBar>
				<ext:Toolbar runat="server">
					<Items>
						<ext:Button runat="server"
							ID="uxAddEmployee"
							Icon="ApplicationAdd"
							Text="Add Employee">
							<DirectEvents>
								<Click OnEvent="deLoadEmployeeWindow">
									<ExtraParams>
										<ext:Parameter Name="type" Value="Add" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" />
						<ext:Button runat="server"
							ID="uxEditEmployee"
							Icon="ApplicationEdit"
							Text="Edit Employee"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deLoadEmployeeWindow">
									<ExtraParams>
										<ext:Parameter Name="type" Value="Edit" />
										<ext:Parameter Name="EmployeeId" Value="#{uxCurrentEmployeeGrid}.getSelectionModel().getSelection()[0].data.EMPLOYEE_ID" Mode="Raw" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
						<ext:Button runat="server"
							ID="uxRemoveEmployee"
							Icon="ApplicationDelete"
							Text="Remove Employee"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deRemoveEmployee">
									<Confirmation Title="Remove?" ConfirmRequest="true" Message="Do you really want to remove the Employee?" />
									<ExtraParams>
										<ext:Parameter Name="EmployeeID" Value="#{uxCurrentEmployeeGrid}.getSelectionModel().getSelection()[0].data.EMPLOYEE_ID" Mode="Raw" />
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
			<DirectEvents>
				<Select OnEvent="deEnableEdit" />
			</DirectEvents>
			<Listeners>
				<Deselect Handler="#{uxEditEmployee}.disable();
					#{uxRemoveEmployee}.disable()" />
			</Listeners>
		</ext:GridPanel>
		<ext:ToolTip ID="ToolTip1" 
			runat="server" 
			Target="={#{uxCurrentEmployeeGrid}.getView().el}"
			Delegate=".x-grid-cell"
			TrackMouse="true">
			<Listeners>
				<Show Handler="onShow(this, #{uxCurrentEmployeeGrid});" /> 
			</Listeners>
		</ext:ToolTip>
	</form>
</body>
</html>

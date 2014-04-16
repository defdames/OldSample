<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCombinedTab_IRM.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umCombinedTab_IRM" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title></title>
	<link href="../../../Resources/StyleSheets/main.css" rel="stylesheet" />
	<script type="text/javascript">
		var setIcon = function (value, metadata, record) {
			var tpl = "<img src='{0}' />";
			if (value == "Error") {
				return "<img src='" + App.uxRedWarning.getValue() + "' />";
			}
			else if (value == "Warning") {
				return "<img src='" + App.uxYellowWarning.getValue() + "' />";
			}
			else {
				return "";
			}
		};
		var onShow = function (toolTip, grid) {
			var view = grid.getView(),
				store = grid.getStore(),
				record = view.getRecord(view.findItemByChild(toolTip.triggerElement)),
				column = view.getHeaderByCell(toolTip.triggerElement),
				data = record.get(column.dataIndex);

			toolTip.update(data);
		};
	</script>
</head>
<body>
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
	<form id="form1" runat="server">
		<ext:Panel runat="server" ID="uxMainContainer" Layout="AutoLayout">
			<Items>
				<ext:Hidden ID="uxYellowWarning" runat="server" />
				<ext:Hidden ID="uxRedWarning" runat="server" />
				<ext:FormPanel runat="server"
					ID="uxHeaderPanel" Padding="10" BodyPadding="5" MaxWidth="1100" Layout="FormLayout">
					<Items>
						<ext:TextField runat="server" ID="uxDateField" FieldLabel="Date" Width="200" ReadOnly="true" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxHeaderField" FieldLabel="DRS Id" Width="200" ReadOnly="true" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxOracleField" FieldLabel="Oracle DRS Id" Width="200" ReadOnly="true" />
						<ext:TextField runat="server" ID="uxProjectField" FieldLabel="Project" Width="600" ReadOnly="true" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxSubDivisionField" FieldLabel="Sub-Division" Width="300" ReadOnly="true" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxSupervisorField" FieldLabel="Supervisor/Area Manager" Width="500" ReadOnly="true" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxLicenseField" FieldLabel="Business License" Width="250" ReadOnly="true" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxStateField" FieldLabel="State" Width="250" ReadOnly="true" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxTypeField" FieldLabel="Type of Work" Width="250" ReadOnly="true" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxDensityField" FieldLabel="Density" Width="200" ReadOnly="true" LabelWidth="100" />
					</Items>
				</ext:FormPanel>
				<ext:GridPanel runat="server"
					ID="uxWarningGrid"
					Title="Warnings/Errors"
					Padding="10"
					MaxWidth="1000">
					<Store>
						<ext:Store runat="server" ID="uxWarningStore">
							<Model>
								<ext:Model ID="Model1" runat="server">
									<Fields>
										<ext:ModelField Name="WarningType" />
										<ext:ModelField Name="RecordType" />
										<ext:ModelField Name="AdditionalInformation" />
									</Fields>
								</ext:Model>
							</Model>
							<Sorters>
								<ext:DataSorter Property="WarningType" Direction="ASC" />
							</Sorters>
						</ext:Store>
					</Store>
					<ColumnModel ID="ColumnModel1" runat="server">
						<Columns>
							<ext:Column runat="server" ID="uxWarningColumn" DataIndex="WarningType">
								<Renderer Fn="setIcon" />
							</ext:Column>
							<ext:Column ID="Column1" runat="server" DataIndex="WarningType" Text="Warning Type" Flex="25" />
							<ext:Column ID="Column2" runat="server" DataIndex="RecordType" Text="Record" Flex="25" />
							<ext:Column ID="Column3" runat="server" DataIndex="AdditionalInformation" Text=" Additional Information" Flex="50" />
						</Columns>
					</ColumnModel>
				</ext:GridPanel>
				<ext:GridPanel runat="server"
					ID="uxEmployeeGrid"
					Title="Employees"
					Padding="10" MaxWidth="1100">
					<Store>
						<ext:Store runat="server"
							ID="uxEmployeeStore">
							<Model>
								<ext:Model ID="Model2" runat="server">
									<Fields>
										<ext:ModelField Name="EMPLOYEE_NAME" />
										<ext:ModelField Name="NAME" />
										<ext:ModelField Name="TIME_IN" Type="Date" />
										<ext:ModelField Name="TIME_OUT" Type="Date" />
										<ext:ModelField Name="TRAVEL_TIME_FORMATTED" />
										<ext:ModelField Name="DRIVE_TIME_FORMATTED" />
										<ext:ModelField Name="SHOPTIME_AM_FORMATTED" />
										<ext:ModelField Name="SHOPTIME_PM_FORMATTED" />
										<ext:ModelField Name="SUPPORT_PROJECT" />
										<ext:ModelField Name="PER_DIEM" />
										<ext:ModelField Name="COMMENTS" />
									</Fields>
								</ext:Model>
							</Model>
						</ext:Store>
					</Store>
					<ColumnModel>
						<Columns>
							<ext:Column ID="Column9" runat="server" DataIndex="EMPLOYEE_NAME" Text="Employee Name" />
							<ext:Column ID="Column10" runat="server" DataIndex="NAME" Text="Equipment Name" />
							<ext:DateColumn ID="DateColumn2" runat="server" DataIndex="TIME_IN" Text="Time In" Format="M/d/yyyy h:mm tt" />
							<ext:DateColumn ID="DateColumn3" runat="server" DataIndex="TIME_OUT" Text="Time Out" Format="M/d/yyyy h:mm tt" />
							<ext:Column ID="Column11" runat="server" DataIndex="TRAVEL_TIME_FORMATTED" Text="Travel Time" />
							<ext:Column ID="Column12" runat="server" DataIndex="DRIVE_TIME_FORMATTED" Text="Drive Time" />
							<ext:Column runat="server" DataIndex="SHOPTIME_AM_FORMATTED" Text="Shoptime AM" />
							<ext:Column runat="server" DataIndex="SHOPTIME_PM_FORMATTED" Text="Shoptime PM" />
							<ext:Column runat="server" DataIndex="SUPPORT_PROJECT" Text="Support Project" />
							<ext:Column ID="Column13" runat="server" DataIndex="PER_DIEM" Text="Per Diem" />
							<ext:Column ID="Column14" runat="server" DataIndex="COMMENTS" Text="Comments" />
						</Columns>
					</ColumnModel>
				</ext:GridPanel>
				<ext:GridPanel runat="server" ID="uxEquipmentGrid"
					Title="Equipment"
					Padding="10" MaxWidth="1100">
					<Store>
						<ext:Store runat="server"
							ID="uxEquipmentStore">
							<Model>
								<ext:Model ID="Model5" runat="server">
									<Fields>
										<ext:ModelField Name="EQUIPMENT_ID" />
										<ext:ModelField Name="CLASS_CODE" />
										<ext:ModelField Name="ORGANIZATION_NAME" />
										<ext:ModelField Name="ODOMETER_START" />
										<ext:ModelField Name="ODOMETER_END" />
										<ext:ModelField Name="PROJECT_ID" />
										<ext:ModelField Name="NAME" />
										<ext:ModelField Name="HEADER_ID" />
									</Fields>
								</ext:Model>
							</Model>
						</ext:Store>
					</Store>
					<ColumnModel>
						<Columns>
							<ext:Column ID="Column47" runat="server"
								DataIndex="PROJECT_ID"
								Text="Project ID" />
							<ext:Column ID="Column48" runat="server"
								DataIndex="NAME"
								Text="Name" />
							<ext:Column ID="Column49" runat="server"
								DataIndex="CLASS_CODE"
								Text="Class Code" />
							<ext:Column ID="Column50" runat="server"
								DataIndex="ORGANIZATION_NAME"
								Text="Organization Name" />
							<ext:Column ID="Column51" runat="server"
								DataIndex="ODOMETER_START"
								Text="Meter Start" />
							<ext:Column ID="Column52" runat="server"
								DataIndex="ODOMETER_END"
								Text="Meter End" />
						</Columns>
					</ColumnModel>
				</ext:GridPanel>
				<ext:GridPanel runat="server"
					ID="uxProductionGrid"
					Title="Production"
					Padding="10" MaxWidth="1100">
					<Store>
						<ext:Store runat="server"
							ID="uxProductionStore">
							<Model>
								<ext:Model ID="Model3" runat="server">
									<Fields>
										<ext:ModelField Name="DESCRIPTION" />
										<ext:ModelField Name="WORK_AREA" />
										<ext:ModelField Name="QUANTITY" />
										<ext:ModelField Name="STATION" />
										<ext:ModelField Name="EXPENDITURE_TYPE" />
										<ext:ModelField Name="BILL_RATE" />
										<ext:ModelField Name="UNIT_OF_MEASURE" />
										<ext:ModelField Name="SURFACE_TYPE" />
										<ext:ModelField Name="COMMENTS" />
									</Fields>
								</ext:Model>
							</Model>
						</ext:Store>
					</Store>
					<ColumnModel>
						<Columns>
							<ext:Column runat="server" DataIndex="DESCRIPTION" Text="Task Name" />
							<ext:Column runat="server" DataIndex="WORK_AREA" Text="Spray/Work Area" />
							<ext:Column runat="server" DataIndex="QUANTITY" Text="Quantity" />
							<ext:Column runat="server" DataIndex="STATION" Text="Station" />
							<ext:Column runat="server" DataIndex="EXPENDITURE_TYPE" Text="Expenditure Type" />
							<ext:Column runat="server" DataIndex="BILL_RATE" Text="Bill Rate" />
							<ext:Column runat="server" DataIndex="UNIT_OF_MEASURE" Text="Unit of Measure" />
							<ext:Column runat="server" DataIndex="SURFACE_TYPE" Text="Surface Type" />
							<ext:Column runat="server" DataIndex="COMMENTS" Text="Comments" />
						</Columns>
					</ColumnModel>
				</ext:GridPanel>
				<ext:GridPanel runat="server"
					ID="uxWeatherGrid"
					Title="Weather"
					Padding="10" MaxWidth="1100">
					<Store>
						<ext:Store runat="server"
							ID="uxWeatherStore">
							<Model>
								<ext:Model ID="Model4" runat="server">
									<Fields>
										<ext:ModelField Name="WEATHER_DATE_TIME" Type="Date" />
										<ext:ModelField Name="WIND_DIRECTION" />
										<ext:ModelField Name="WIND_VELOCITY" />
										<ext:ModelField Name="TEMP" />
										<ext:ModelField Name="HUMIDITY" />
										<ext:ModelField Name="COMMENTS" />
									</Fields>
								</ext:Model>
							</Model>
						</ext:Store>
					</Store>
					<ColumnModel>
						<Columns>
							<ext:DateColumn runat="server" DataIndex="WEATHER_DATE_TIME" Text="Date/Time" Format="M/d/yyyy h:mm tt" />
							<ext:Column runat="server" DataIndex="WIND_DIRECTION" Text="Wind Direction" />
							<ext:Column runat="server" DataIndex="WIND_VELOCITY" Text="Wind Velocity" />
							<ext:Column runat="server" DataIndex="TEMP" Text="Temperature" />
							<ext:Column runat="server" DataIndex="HUMIDITY" Text="Humidity" />
							<ext:Column runat="server" DataIndex="COMMENTS" Text="Comments" />
						</Columns>
					</ColumnModel>
				</ext:GridPanel>
				<ext:GridPanel runat="server"
					ID="uxInventoryGrid"
					Title="Inventory"
					Padding="10" MaxWidth="1100">
					<Store>
						<ext:Store runat="server"
							ID="uxInventoryStore">
							<Model>
								<ext:Model ID="Model6" runat="server">
									<Fields>
										<ext:ModelField Name="INV_NAME" />
										<ext:ModelField Name="SUB_INVENTORY_SECONDARY_NAME" />
										<ext:ModelField Name="DESCRIPTION" />
										<ext:ModelField Name="RATE" />
									</Fields>
								</ext:Model>
							</Model>
						</ext:Store>
					</Store>
					<ColumnModel>
						<Columns>
							<ext:Column runat="server" DataIndex="INV_NAME" Text="Inventory Org" />
							<ext:Column runat="server" DataIndex="SUB_INVENTORY_SECONDARY_NAME" Text="Sub-Inv Name" />
							<ext:Column runat="server" DataIndex="DESCRIPTION" Text="Item" />
							<ext:Column runat="server" DataIndex="RATE" Text="Quantity" />
						</Columns>
					</ColumnModel>
				</ext:GridPanel>
				<ext:FormPanel runat="server" ID="uxFooterPanel" Padding="10" BodyPadding="5" MaxWidth="1100">
					<Items>
						<ext:TextField runat="server" ID="uxReasonForNoWorkField" FieldLabel="Reason for no work" Width="700" ReadOnly="true" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxHotelField" FieldLabel="Hotel" ReadOnly="true" LabelWidth="100" Width="400" />
						<ext:TextField runat="server" ID="uxCityField" FieldLabel="City" ReadOnly="true" LabelWidth="100" Width="300" />
						<ext:TextField runat="server" ID="uxFooterStateField" FieldLabel="State" ReadOnly="true" LabelWidth="100" Width="300" />
						<ext:TextField runat="server" ID="uxPhoneField" FieldLabel="Phone" ReadOnly="true" LabelWidth="100" Width="300" />
						<ext:TextField runat="server" ID="uxForemanNameField" FieldLabel="Foreman Name" ReadOnly="true" LabelWidth="100" Width="500" />
						<ext:FieldContainer ID="FieldContainer1" runat="server" FieldLabel="Foreman Signature" LabelWidth="100">
							<Items>
								<ext:Image runat="server" Height="214" ID="uxForemanImage" Width="320" />
							</Items>
						</ext:FieldContainer>
						<ext:TextField runat="server" ID="uxContractNameField" FieldLabel="Contract Rep Name" Width="500" ReadOnly="true" LabelWidth="100" />
						<ext:FieldContainer ID="FieldContainer2" runat="server" FieldLabel="Contract Rep Signature" LabelWidth="100">
							<Items>
								<ext:Image runat="server" Height="214" ID="uxContractImage" Width="320" />
							</Items>
						</ext:FieldContainer>
						<ext:TextField runat="server" ID="uxDOTRep" ReadOnly="true" FieldLabel="DOT Rep Name" LabelWidth="100" Width="500" />
						<ext:FieldContainer runat="server" FieldLabel="DOT Rep Signature" LabelWidth="100">
							<Items>
								<ext:Image runat="server" Height="214" ID="uxDOTImage" Width="320" />
							</Items>
						</ext:FieldContainer>
					</Items>
				</ext:FormPanel>
				<ext:ToolTip ID="ToolTip1" 
						runat="server" 
						Target="={#{uxWarningGrid}.getView().el}"
						Delegate=".x-grid-cell"
						TrackMouse="true"
						UI="Warning"
						Width="400">
						<Listeners>
							<Show Handler="onShow(this, #{uxWarningGrid});" /> 
						</Listeners>
				</ext:ToolTip>
				<ext:ToolTip ID="ToolTip2" 
						runat="server" 
						Target="={#{uxEmployeeGrid}.getView().el}"
						Delegate=".x-grid-cell"
						TrackMouse="true"
						UI="Warning"
						Width="400">
						<Listeners>
							<Show Handler="onShow(this, #{uxEmployeeGrid});" /> 
						</Listeners>
				</ext:ToolTip>
				<ext:ToolTip ID="ToolTip3" 
						runat="server" 
						Target="={#{uxEquipmentGrid}.getView().el}"
						Delegate=".x-grid-cell"
						TrackMouse="true"
						UI="Warning"
						Width="400">
						<Listeners>
							<Show Handler="onShow(this, #{uxEquipmentGrid});" /> 
						</Listeners>
				</ext:ToolTip>
				<ext:ToolTip ID="ToolTip4" 
						runat="server" 
						Target="={#{uxInventoryGrid}.getView().el}"
						Delegate=".x-grid-cell"
						TrackMouse="true"
						UI="Warning"
						Width="400">
						<Listeners>
							<Show Handler="onShow(this, #{uxInventoryGrid});" /> 
						</Listeners>
				</ext:ToolTip>
				<ext:ToolTip ID="ToolTip5" 
						runat="server" 
						Target="={#{uxWeatherGrid}.getView().el}"
						Delegate=".x-grid-cell"
						TrackMouse="true"
						UI="Warning"
						Width="400">
						<Listeners>
							<Show Handler="onShow(this, #{uxWeatherGrid});" /> 
						</Listeners>
				</ext:ToolTip>
				<ext:ToolTip ID="ToolTip6" 
						runat="server" 
						Target="={#{uxProductionGrid}.getView().el}"
						Delegate=".x-grid-cell"
						TrackMouse="true"
						UI="Warning"
						Width="400">
						<Listeners>
							<Show Handler="onShow(this, #{uxProductionGrid});" /> 
						</Listeners>
				</ext:ToolTip>
			</Items>
		</ext:Panel>
	</form>
</body>
</html>

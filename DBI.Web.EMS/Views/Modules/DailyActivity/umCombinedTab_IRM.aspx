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

		var showButtons = function () {
		    App.uxSaveFooterButton.show();
		    App.uxSaveHeaderButton.show();
		};

		var disableOnError = function () {
		    parent.App.uxPostActivityButton.disable();
		    parent.App.uxApproveActivityButton.disable();
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
					ID="uxHeaderPanel" Padding="10" BodyPadding="5" MaxWidth="1100">
					<Items>
						<ext:DateField runat="server" ID="uxDateField" FieldLabel="Date" AllowBlank="false" LabelWidth="100" Width="200" />
						<ext:TextField runat="server" ID="uxHeaderField" FieldLabel="DRS Id" Width="200" LabelWidth="100" ReadOnly="true" />
						<ext:TextField runat="server" ID="uxOracleField" FieldLabel="Oracle DRS Id" Width="200" ReadOnly="true" />
						<ext:DropDownField runat="server"
							ID="uxProjectField"
							FieldLabel="Project"
							Mode="ValueText"
							AllowBlank="false" Editable="false" Width="600" LabelWidth="100">
							<Component>
								<ext:GridPanel runat="server"
									ID="uxFormProjectGrid"
									Layout="HBoxLayout">
									<Store>
										<ext:Store runat="server"
											ID="uxFormProjectStore"
											OnReadData="deReadProjectData"
											PageSize="10"
											RemoteSort="true">
											<Model>
												<ext:Model runat="server"
													ID="uxFormProjectModel">
													<Fields>
														<ext:ModelField Name="PROJECT_ID" Type="Int" />
														<ext:ModelField Name="ORGANIZATION_NAME" Type="String" />
														<ext:ModelField Name="SEGMENT1" Type="String" />
														<ext:ModelField Name="LONG_NAME" Type="String" />
													</Fields>
												</ext:Model>
											</Model>
											<Proxy>
												<ext:PageProxy />
											</Proxy>
										</ext:Store>
									</Store>
									<ColumnModel>
										<Columns>
											<ext:Column runat="server"
												ID="uxFormSegment"
												DataIndex="SEGMENT1"
												Text="Project #" Flex="15" />
											<ext:Column runat="server"
												ID="uxFormLong"
												DataIndex="LONG_NAME"
												Text="Project Name" Flex="50" />
											<ext:Column runat="server"
												ID="uxFormOrg"
												DataIndex="ORGANIZATION_NAME"
												Text="Organization Name" Flex="35" />
										</Columns>
									</ColumnModel>
									<SelectionModel>
										<ext:RowSelectionModel ID="uxFormProjectSelection" runat="server" Mode="Single" />
									</SelectionModel>
									<DirectEvents>
										<SelectionChange OnEvent="deStoreProjectValue">
											<ExtraParams>
												<ext:Parameter Name="ProjectId" Value="#{uxFormProjectGrid}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
												<ext:Parameter Name="LongName" Value="#{uxFormProjectGrid}.getSelectionModel().getSelection()[0].data.LONG_NAME" Mode="Raw" />
											</ExtraParams>
										</SelectionChange>
									</DirectEvents>
									<Plugins>
										<ext:FilterHeader ID="uxFormProjectFilter" runat="server" Remote="true" />
									</Plugins>
									<TopBar>
										<ext:Toolbar runat="server"
											ID="uxFormProjectTop">
											<Items>
												<ext:Button runat="server"
													ID="uxFormProjectToggleOrg"
													EnableToggle="true"
													Text="All Regions"
													Icon="Group">
													<DirectEvents>
														<Toggle OnEvent="deReloadStore">
															<ExtraParams>
																<ext:Parameter Name="Type" Value="Project" />
															</ExtraParams>
														</Toggle>
													</DirectEvents>
												</ext:Button>
											</Items>
										</ext:Toolbar>
									</TopBar>
									<BottomBar>
										<ext:PagingToolbar ID="uxFormProjectPaging" runat="server" />
									</BottomBar>
								</ext:GridPanel>
							</Component>
						</ext:DropDownField>
						<ext:TextField runat="server" ID="uxSubDivisionField" FieldLabel="Sub-Division" Width="300" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxContractorField" FieldLabel="Contractor" Width="500" LabelWidth="100" />
						<ext:DropDownField runat="server"
							ID="uxSupervisorField"
							FieldLabel="Supervisor/Area Manager"
							Mode="ValueText"
							AllowBlank="false" Editable="false" Width="500" LabelWidth="100">
							<Component>
								<ext:GridPanel runat="server"
									ID="uxFormEmployeeGrid"
									Layout="HBoxLayout">
									<Store>
										<ext:Store runat="server"
											ID="uxFormEmployeeStore"
											OnReadData="deLoadEmployees"
											PageSize="10"
											RemoteSort="true">
											<Model>
												<ext:Model ID="uxFormEmployeeModel" runat="server">
													<Fields>
														<ext:ModelField Name="PERSON_ID" Type="Int" />
														<ext:ModelField Name="EMPLOYEE_NAME" Type="String" />
														<ext:ModelField Name="JOB_NAME" Type="String" />
													</Fields>
												</ext:Model>
											</Model>
											<Proxy>
												<ext:PageProxy />
											</Proxy>
										</ext:Store>
									</Store>
									<ColumnModel>
										<Columns>
											<ext:Column runat="server" Text="Person ID" ID="uxFormPersonId" DataIndex="PERSON_ID" Flex="20" />
											<ext:Column runat="server" Text="Employee Name" ID="uxFormEmployeeName" DataIndex="EMPLOYEE_NAME" Flex="35" />
											<ext:Column runat="server" Text="Job Name" ID="uxFormJobName" DataIndex="JOB_NAME" Flex="35" />
										</Columns>
									</ColumnModel>
									<Plugins>
										<ext:FilterHeader runat="server"
											ID="uxFormEmployeeFilter"
											Remote="true" />
									</Plugins>
									<DirectEvents>
										<SelectionChange OnEvent="deStoreEmployee">
											<ExtraParams>
												<ext:Parameter Name="EmployeeName" Value="#{uxFormEmployeeGrid}.getSelectionModel().getSelection()[0].data.EMPLOYEE_NAME" Mode="Raw" />
												<ext:Parameter Name="PersonID" Value="#{uxFormEmployeeGrid}.getSelectionModel().getSelection()[0].data.PERSON_ID" Mode="Raw" />
											</ExtraParams>
										</SelectionChange>
									</DirectEvents>
									<SelectionModel>
										<ext:RowSelectionModel ID="uxFormEmployeeSelection" runat="server" Mode="Single" />
									</SelectionModel>
									<TopBar>
										<ext:Toolbar ID="Toolbar1" runat="server">
											<Items>
												<ext:Button runat="server"
													ID="uxFormEmployeeToggleOrg"
													EnableToggle="true"
													Text="All Regions"
													Icon="Group">
													<DirectEvents>
														<Toggle OnEvent="deReloadStore">
															<ExtraParams>
																<ext:Parameter Name="Type" Value="Employee" />
															</ExtraParams>
														</Toggle>
													</DirectEvents>
												</ext:Button>
											</Items>
										</ext:Toolbar>
									</TopBar>
									<BottomBar>
										<ext:PagingToolbar ID="uxFormEmployeePaging" runat="server" />
									</BottomBar>
								</ext:GridPanel>
							</Component>
						</ext:DropDownField>
						<ext:TextField runat="server" ID="uxLicenseField" FieldLabel="Business License" Width="250" LabelWidth="100" />
						<ext:ComboBox runat="server"
							ID="uxStateField"
							FieldLabel="Business License State"
							DisplayField="name"
							ValueField="name"
							QueryMode="Local"
							TypeAhead="true"
							AllowBlank="true"
							ForceSelection="true">
							<Store>
								<ext:Store ID="uxStateStore" runat="server" AutoDataBind="true">
									<Model>
										<ext:Model ID="Model8" runat="server">
											<Fields>
												<ext:ModelField Name="abbr" />
												<ext:ModelField Name="name" />
											</Fields>
										</ext:Model>
									</Model>
									<Reader>
										<ext:ArrayReader />
									</Reader>
								</ext:Store>
							</Store>
						</ext:ComboBox>
						<ext:TextField runat="server" ID="uxTypeField" FieldLabel="Type of Work" Width="250" LabelWidth="100" />
						<ext:ComboBox runat="server"
							ID="uxDensityField"
							FieldLabel="Density"
							QueryMode="Local"
							TypeAhead="true"
							AllowBlank="true"
							ForceSelection="true"
							Width="200"
							LabelWidth="100">
							<Items>
								<ext:ListItem Text="Low" Value="LOW" />
								<ext:ListItem Text="Medium" Value="MEDIUM" />
								<ext:ListItem Text="High" Value="HIGH" />
							</Items>
						</ext:ComboBox>
					</Items>
					<Buttons>
						<ext:Button runat="server" ID="uxSaveHeaderButton" Icon="Add" Text="Save" Hidden="true" Disabled="true">
							<DirectEvents>
								<Click OnEvent="deUpdateHeader">
									<EventMask ShowMask="true" />
								</Click>
							</DirectEvents>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxSaveHeaderButton}.setDisabled(!valid)" />
					</Listeners>
				</ext:FormPanel>
				<ext:GridPanel runat="server"
					ID="uxWarningGrid"
					Title="Warnings/Errors"
					Padding="10"
					MaxWidth="1100">
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
										<ext:ModelField Name="EMPLOYEE_ID" />
										<ext:ModelField Name="EMPLOYEE_NAME" />
										<ext:ModelField Name="NAME" />
										<ext:ModelField Name="TIME_IN" Type="Date" />
										<ext:ModelField Name="TIME_OUT" Type="Date" />
										<ext:ModelField Name="TRAVEL_TIME_FORMATTED" />
										<ext:ModelField Name="DRIVE_TIME_FORMATTED" />
										<ext:ModelField Name="SHOPTIME_AM_FORMATTED" />
										<ext:ModelField Name="SHOPTIME_PM_FORMATTED" />
										<ext:ModelField Name="PER_DIEM" />
										<ext:ModelField Name="COMMENTS" />
									</Fields>
								</ext:Model>
							</Model>
						</ext:Store>
					</Store>
					<ColumnModel>
						<Columns>
							<ext:Column ID="Column9" runat="server" DataIndex="EMPLOYEE_NAME" Text="Employee Name" Flex="1" />
							<ext:Column ID="Column10" runat="server" DataIndex="NAME" Text="Equipment Name" Flex="1" />
							<ext:DateColumn ID="DateColumn2" runat="server" DataIndex="TIME_IN" Text="Time In" Format="M/d/yyyy h:mm tt" Flex="1" />
							<ext:DateColumn ID="DateColumn3" runat="server" DataIndex="TIME_OUT" Text="Time Out" Format="M/d/yyyy h:mm tt" Flex="1" />
							<ext:Column ID="Column11" runat="server" DataIndex="TRAVEL_TIME_FORMATTED" Text="Travel Time" Flex="1" />
							<ext:Column ID="Column12" runat="server" DataIndex="DRIVE_TIME_FORMATTED" Text="Drive Time" Flex="1" />
							<ext:Column runat="server" DataIndex="SHOPTIME_AM_FORMATTED" Text="Shoptime AM" Flex="1" />
							<ext:Column runat="server" DataIndex="SHOPTIME_PM_FORMATTED" Text="Shoptime PM" Flex="1" />
							<ext:Column ID="Column13" runat="server" DataIndex="PER_DIEM" Text="Per Diem" Flex="1" />
							<ext:Column ID="Column14" runat="server" DataIndex="COMMENTS" Text="Comments" Flex="1" />
						</Columns>
					</ColumnModel>
					<TopBar>
						<ext:Toolbar ID="uxEmployeeToolbar" runat="server">
							<Items>
								<ext:Button ID="uxAddEmployeeButton" runat="server" Text="Add" Icon="ApplicationAdd">
									<Listeners>
										<Click Handler="parent.App.direct.dmLoadEmployeeWindow('Add',App.uxHeaderField.value, '')" />
									</Listeners>
								</ext:Button>
								<ext:Button ID="uxEditEmployeeButton" runat="server" Text="Edit" Icon="ApplicationEdit" Disabled="true">
									<Listeners>
										<Click Handler="parent.App.direct.dmLoadEmployeeWindow('Edit', App.uxHeaderField.value, App.uxEmployeeGrid.getSelectionModel().getSelection()[0].data.EMPLOYEE_ID)" />
									</Listeners>
								</ext:Button>
								<ext:Button ID="uxDeleteEmployeeButton" runat="server" Text="Delete" Icon="ApplicationDelete" Disabled="true">
									<DirectEvents>
										<Click OnEvent="deRemoveEmployee">
											<Confirmation Title="Remove?" ConfirmRequest="true" Message="Do you really want to remove the Employee?" />
											<ExtraParams>
												<ext:Parameter Name="EmployeeID" Value="#{uxEmployeeGrid}.getSelectionModel().getSelection()[0].data.EMPLOYEE_ID" Mode="Raw" />
											</ExtraParams>
										</Click>
									</DirectEvents>
								</ext:Button>
							</Items>
						</ext:Toolbar>
					</TopBar>
					<Listeners>
						<Select Handler="#{uxEditEmployeeButton}.enable(); #{uxDeleteEmployeeButton}.enable()" />
					</Listeners>
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
										<ext:ModelField Name="SEGMENT1" />
									</Fields>
								</ext:Model>
							</Model>
						</ext:Store>
					</Store>
					<ColumnModel>
						<Columns>
							<ext:Column ID="Column47" runat="server"
								DataIndex="SEGMENT1"
								Text="Project Number" Flex="1" />
							<ext:Column ID="Column48" runat="server"
								DataIndex="NAME"
								Text="Name" Flex="1" />
							<ext:Column ID="Column49" runat="server"
								DataIndex="CLASS_CODE"
								Text="Class Code" Flex="1" />
							<ext:Column ID="Column50" runat="server"
								DataIndex="ORGANIZATION_NAME"
								Text="Organization Name" Flex="1" />
							<ext:Column ID="Column51" runat="server"
								DataIndex="ODOMETER_START"
								Text="Starting Units" Flex="1" />
							<ext:Column ID="Column52" runat="server"
								DataIndex="ODOMETER_END"
								Text="Ending Units" Flex="1" />
						</Columns>
					</ColumnModel>
					<TopBar>
						<ext:Toolbar ID="uxEquipmentToolbar" runat="server">
							<Items>
								<ext:Button ID="uxAddEquipmentButton" runat="server" Text="Add" Icon="ApplicationAdd">
									<Listeners>
										<Click Handler="parent.App.direct.dmLoadEquipmentWindow('Add',App.uxHeaderField.value, '')" />
									</Listeners>
								</ext:Button>
								<ext:Button ID="uxEditEquipmentButton" runat="server" Text="Edit" Icon="ApplicationEdit" Disabled="true">
									<Listeners>
										<Click Handler="parent.App.direct.dmLoadEquipmentWindow('Edit',App.uxHeaderField.value, App.uxEquipmentGrid.getSelectionModel().getSelection()[0].data.EQUIPMENT_ID)" />
									</Listeners>
								</ext:Button>
								<ext:Button ID="uxDeleteEquipmentButton" runat="server" Text="Delete" Icon="ApplicationDelete" Disabled="true">
									<DirectEvents>
										<Click OnEvent="deRemoveEquipment">
											<Confirmation ConfirmRequest="true" Title="Remove?" Message="Do you really want to remove?" />
											<ExtraParams>
												<ext:Parameter Name="EquipmentId" Value="#{uxEquipmentGrid}.getSelectionModel().getSelection()[0].data.EQUIPMENT_ID" Mode="Raw" />
											</ExtraParams>
										</Click>
									</DirectEvents>
								</ext:Button>
							</Items>
						</ext:Toolbar>
					</TopBar>
					<Listeners>
						<Select Handler="#{uxEditEquipmentButton}.enable(); #{uxDeleteEquipmentButton}.enable()" />
					</Listeners>
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
										<ext:ModelField Name="PRODUCTION_ID" />
										<ext:ModelField Name="TASK_NUMBER" />
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
							<ext:Column runat="server" DataIndex="TASK_NUMBER" Text="Task Number" Flex="1" />
							<ext:Column runat="server" DataIndex="DESCRIPTION" Text="Task Name" Flex="1" />
							<ext:Column runat="server" DataIndex="WORK_AREA" Text="Spray/Work Area" Flex="1" />
							<ext:Column runat="server" DataIndex="QUANTITY" Text="Quantity" Flex="1" />
							<ext:Column runat="server" DataIndex="STATION" Text="Station" Flex="1" />
							<ext:Column runat="server" DataIndex="EXPENDITURE_TYPE" Text="Expenditure Type" Flex="1" />
							<ext:Column runat="server" DataIndex="BILL_RATE" Text="Bill Rate" Flex="1" />
							<ext:Column runat="server" DataIndex="UNIT_OF_MEASURE" Text="Unit of Measure" Flex="1" />
							<ext:Column runat="server" DataIndex="SURFACE_TYPE" Text="Surface Type" Flex="1" />
							<ext:Column runat="server" DataIndex="COMMENTS" Text="Comments" Flex="1" />
						</Columns>
					</ColumnModel>
					<TopBar>
						<ext:Toolbar ID="uxProductionToolbar" runat="server">
							<Items>
								<ext:Button ID="uxAddProductionButton" runat="server" Text="Add" Icon="ApplicationAdd">
									<Listeners>
										<Click Handler="parent.App.direct.dmLoadProductionWindow_IRM('Add',App.uxHeaderField.value, '')" />
									</Listeners>
								</ext:Button>
								<ext:Button ID="uxEditProductionButton" runat="server" Text="Edit" Icon="ApplicationEdit" Disabled="true">
									<Listeners>
										<Click Handler="parent.App.direct.dmLoadProductionWindow_IRM('Edit',App.uxHeaderField.value, App.uxProductionGrid.getSelectionModel().getSelection()[0].data.PRODUCTION_ID)" />
									</Listeners>
								</ext:Button>
								<ext:Button ID="uxDeleteProductionButton" runat="server" Text="Delete" Icon="ApplicationDelete" Disabled="true">
									<DirectEvents>
										<Click OnEvent="deRemoveProduction">
											<Confirmation ConfirmRequest="true" Title="Really?" Message="Do you really want to remove?" />
											<ExtraParams>
												<ext:Parameter Name="ProductionId" Value="#{uxProductionGrid}.getSelectionModel().getSelection()[0].data.PRODUCTION_ID" Mode="Raw" />
											</ExtraParams>
										</Click>
									</DirectEvents>
								</ext:Button>
							</Items>
						</ext:Toolbar>
					</TopBar>
					<Listeners>
						<Select Handler="#{uxEditProductionButton}.enable(); #{uxDeleteProductionButton}.enable()" />
					</Listeners>
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
										<ext:ModelField Name="WEATHER_ID" />
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
					<TopBar>
						<ext:Toolbar ID="uxWeatherToolbar" runat="server">
							<Items>
								<ext:Button ID="uxAddWeatherButton" runat="server" Text="Add" Icon="ApplicationAdd">
									<Listeners>
										<Click Handler="parent.App.direct.dmLoadWeatherWindow('Add',App.uxHeaderField.value, '')" />
									</Listeners>
								</ext:Button>
								<ext:Button ID="uxEditWeatherButton" runat="server" Text="Edit" Icon="ApplicationEdit" Disabled="true">
									<Listeners>
										<Click Handler="parent.App.direct.dmLoadWeatherWindow('Edit', App.uxHeaderField.value, App.uxWeatherGrid.getSelectionModel().getSelection()[0].data.WEATHER_ID)" />
									</Listeners>
								</ext:Button>
								<ext:Button ID="uxDeleteWeatherButton" runat="server" Text="Delete" Icon="ApplicationDelete" Disabled="true">
									<DirectEvents>
										<Click OnEvent="deRemoveWeather">
											<Confirmation ConfirmRequest="true" Title="Remove?" Message="Do you really want to remove the weather?" />
											<ExtraParams>
												<ext:Parameter Name="WeatherId" Value="#{uxCurrentWeatherGrid}.getSelectionModel().getSelection()[0].data.WEATHER_ID" Mode="Raw" />
											</ExtraParams>
										</Click>
									</DirectEvents>
								</ext:Button>
							</Items>
						</ext:Toolbar>
					</TopBar>
					<Listeners>
						<Select Handler="#{uxEditWeatherButton}.enable(); #{uxDeleteWeatherButton}.enable()" />
					</Listeners>
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
										<ext:ModelField Name="INVENTORY_ID" />
										<ext:ModelField Name="INV_NAME" />
										<ext:ModelField Name="SUB_INVENTORY_SECONDARY_NAME" />
										<ext:ModelField Name="SEGMENT1" />
										<ext:ModelField Name="DESCRIPTION" />
										<ext:ModelField Name="RATE" />
									</Fields>
								</ext:Model>
							</Model>
						</ext:Store>
					</Store>
					<ColumnModel>
						<Columns>
							<ext:Column runat="server" DataIndex="INV_NAME" Text="Inventory Org" Flex="1" />
							<ext:Column runat="server" DataIndex="SUB_INVENTORY_SECONDARY_NAME" Text="Sub-Inv Name" Flex="1" />
							<ext:Column runat="server" DataIndex="SEGMENT1" Text="Item ID" Flex="1" />
							<ext:Column runat="server" DataIndex="DESCRIPTION" Text="Item" Flex="1" />
							<ext:Column runat="server" DataIndex="RATE" Text="Quantity" Flex="1" />
						</Columns>
					</ColumnModel>
					<TopBar>
						<ext:Toolbar ID="uxInventoryToolbar" runat="server">
							<Items>
								<ext:Button ID="uxAddInventoryButton" runat="server" Text="Add" Icon="ApplicationAdd">
									<Listeners>
										<Click Handler="parent.App.direct.dmLoadInventoryWindow_IRM('Add',App.uxHeaderField.value, '')" />
									</Listeners>
								</ext:Button>
								<ext:Button ID="uxEditInventoryButton" runat="server" Text="Edit" Icon="ApplicationEdit" Disabled="true">
									<Listeners>
										<Click Handler="parent.App.direct.dmLoadInventoryWindow_IRM('Edit', App.uxHeaderField.value, App.uxInventoryGrid.getSelectionModel().getSelection()[0].data.INVENTORY_ID)" />
									</Listeners>
								</ext:Button>
								<ext:Button ID="uxDeleteInventoryButton" runat="server" Text="Delete" Icon="ApplicationDelete" Disabled="true">
									<DirectEvents>
										<Click OnEvent="deRemoveInventory">
											<Confirmation ConfirmRequest="true" Title="Really Delete?" Message="Do you really want to delete?" />
											<ExtraParams>
												<ext:Parameter Name="InventoryId" Value="#{uxInventoryGrid}.getSelectionModel().getSelection()[0].data.INVENTORY_ID" Mode="Raw" />
											</ExtraParams>
										</Click>
									</DirectEvents>
								</ext:Button>
							</Items>
						</ext:Toolbar>
					</TopBar>
					<Listeners>
						<Select Handler="#{uxEditInventoryButton}.enable(); #{uxDeleteInventoryButton}.enable()" />
					</Listeners>
				</ext:GridPanel>
				<ext:FormPanel runat="server" ID="uxFooterPanel" Padding="10" BodyPadding="5" MaxWidth="1100">
					<Items>
						<ext:TextField runat="server" ID="uxReasonForNoWorkField" FieldLabel="Reason for no work" Width="700" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxHotelField" FieldLabel="Hotel" LabelWidth="100" Width="400" />
						<ext:TextField runat="server" ID="uxCityField" FieldLabel="City" LabelWidth="100" Width="300" />
						<ext:ComboBox runat="server"
							ID="uxFooterStateField"
							FieldLabel="State"
							DisplayField="name"
							ValueField="name"
							QueryMode="Local"
							TypeAhead="true"
							ForceSelection="true">
							<Store>
								<ext:Store ID="uxStateList" runat="server" AutoDataBind="true">
									<Model>
										<ext:Model ID="Model7" runat="server">
											<Fields>
												<ext:ModelField Name="abbr" />
												<ext:ModelField Name="name" />
											</Fields>
										</ext:Model>
									</Model>
									<Reader>
										<ext:ArrayReader />
									</Reader>
								</ext:Store>
							</Store>
						</ext:ComboBox>
						<ext:TextField runat="server" ID="uxPhoneField" FieldLabel="Phone" LabelWidth="100" Width="300" />
						<ext:TextField runat="server" ID="uxForemanNameField" FieldLabel="Foreman Name" ReadOnly="true" LabelWidth="100" Width="500" />
						<ext:FileUploadField runat="server"
							ID="uxForemanImageField"
							FieldLabel="Foreman Signature" />
						<ext:FieldContainer ID="FieldContainer1" runat="server" FieldLabel="Foreman Signature" LabelWidth="100">
							<Items>
								<ext:Image runat="server" Height="214" ID="uxForemanImage" Width="320" />
							</Items>
						</ext:FieldContainer>
						<ext:TextField runat="server" ID="uxContractNameField" FieldLabel="Contract Rep Name" Width="500" LabelWidth="100" />
						<ext:FileUploadField runat="server"
							ID="uxContractImageField"
							FieldLabel="Contract Representative" />
						<ext:FieldContainer ID="FieldContainer2" runat="server" FieldLabel="Contract Rep Signature" LabelWidth="100">
							<Items>
								<ext:Image runat="server" Height="214" ID="uxContractImage" Width="320" />
							</Items>
						</ext:FieldContainer>
						<ext:TextField runat="server" ID="uxDOTRep" FieldLabel="DOT Rep Name" LabelWidth="100" Width="500" />
						<ext:FileUploadField runat="server"
							ID="uxDotRepImageField"
							FieldLabel="DOT Representative" />
						<ext:FieldContainer runat="server" FieldLabel="DOT Rep Signature" LabelWidth="100">
							<Items>
								<ext:Image runat="server" Height="214" ID="uxDOTImage" Width="320" />
							</Items>
						</ext:FieldContainer>
					</Items>
					<Buttons>
						<ext:Button runat="server" ID="uxSaveFooterButton" Text="Save" Icon="Add" Hidden="true">
							<DirectEvents>
								<Click OnEvent="deUpdateFooter">
									<EventMask ShowMask="true" />
								</Click>
							</DirectEvents>
						</ext:Button>
					</Buttons>
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

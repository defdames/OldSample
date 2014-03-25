<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCombinedTab_IRM.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umCombinedTab_IRM" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title></title>
	<link href="../../../Resources/StyleSheets/main.css" rel="stylesheet" />
</head>
<body>
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
	<form id="form1" runat="server">
		<ext:Panel runat="server" ID="uxMainContainer" Layout="AutoLayout">
			<Items>
				<ext:FormPanel runat="server"
					ID="uxHeaderPanel" Padding="10" BodyPadding="5" MaxWidth="1000">
					<Items>
						<ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout" AnchorHorizontal="100%">
							<Items>
								<ext:TextField runat="server" ID="uxDateField" FieldLabel="Date" Flex="25" ReadOnly="true" />
								<ext:TextField runat="server" ID="uxHeaderField" FieldLabel="DRS Id" Flex="25" ReadOnly="true" />
								<ext:TextField runat="server" ID="uxOracleField" FieldLabel="Oracle DRS Id" Flex="25" ReadOnly="true" />
								<ext:Component ID="Component1" runat="server" Flex="25" />
							</Items>
						</ext:FieldContainer>
						<ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout" AnchorHorizontal="100%">
							<Items>
								<ext:TextField runat="server" ID="uxProjectField" FieldLabel="Project" Flex="50" LabelPad="2" ReadOnly="true"  />
								<ext:TextField runat="server" ID="uxSubDivisionField" FieldLabel="Sub-Division" Flex="25" ReadOnly="true" />
								<ext:Component ID="Component2" runat="server" Flex="25" />
							</Items>
						</ext:FieldContainer>
						<ext:FieldContainer ID="FieldContainer3" runat="server" Layout="HBoxLayout" AnchorHorizontal="100%">
							<Items>
								<ext:TextField runat="server" ID="uxLicenseField" FieldLabel="State License #" Flex="25" ReadOnly="true" />
								<ext:TextField runat="server" ID="uxStateField" LabelWidth="50" FieldLabel="State" Flex="15" ReadOnly="true" />
								<ext:TextField runat="server" ID="uxSupervisorField" FieldLabel="Supervisor/Area Manager" LabelWidth="150" Flex="60" MaxLength="50" ReadOnly="true" />
							</Items>
						</ext:FieldContainer>
						<ext:FieldContainer ID="FieldContainer4" runat="server" Layout="HBoxLayout" AnchorHorizontal="100%">
							<Items>
								<ext:TextField runat="server" ID="uxTypeField" FieldLabel="Type of Work" Flex="20" ReadOnly="true" />
								<ext:TextField runat="server" ID="uxDensityField" FieldLabel="Density" Flex="25"  ReadOnly="true"/>
								<ext:Component ID="Component3" runat="server" Flex="65" />
							</Items>
						</ext:FieldContainer>
					</Items>
				</ext:FormPanel>                    
				<ext:GridPanel runat="server"
					ID="uxEmployeeGrid"
					Title="Employees"
					Padding="10" MaxWidth="1000">
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
							<ext:Column ID="Column13" runat="server" DataIndex="PER_DIEM" Text="Per Diem" />
							<ext:Column ID="Column14" runat="server" DataIndex="COMMENTS" Text="Comments" />
						</Columns>
					</ColumnModel>
				</ext:GridPanel>
				<ext:GridPanel runat="server" ID="uxEquipmentGrid"
					Title="Equipment"
					Padding="10" MaxWidth="1000">
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
								Text="Class Code"/>
							<ext:Column ID="Column50" runat="server"
								DataIndex="ORGANIZATION_NAME" 
								Text="Organization Name"/>
							<ext:Column ID="Column51" runat="server"
								DataIndex="ODOMETER_START" 
								Text="Meter Start"/>
							<ext:Column ID="Column52" runat="server"
								DataIndex="ODOMETER_END"
								Text="Meter End" />
						</Columns>
					</ColumnModel>
				</ext:GridPanel>
				<ext:GridPanel runat="server"
					ID="uxProductionGrid"
					Title="Production"
					Padding="10" MaxWidth="1000">
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
					Padding="10" MaxWidth="1000">
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
					Padding="10" MaxWidth="1000">
					<Store>
						<ext:Store runat="server"
							ID="uxInventoryStore">
							<Model>
								<ext:Model ID="Model6" runat="server">
									<Fields>
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
							<ext:Column runat="server" DataIndex="SUB_INVENTORY_SECONDARY_NAME" Text="Sub-Inv Name" />
							<ext:Column runat="server" DataIndex="DESCRIPTION" Text="Item" />
							<ext:Column runat="server" DataIndex="RATE" Text="Quantity" />
						</Columns>
					</ColumnModel>
				</ext:GridPanel>
                <ext:FormPanel runat="server" ID="uxFooterPanel" Padding="10" BodyPadding="5" MaxWidth="1000">
					<Items>
						<ext:FieldContainer ID="FieldContainer5" runat="server" Layout="HBoxLayout" AnchorHorizontal="100%">
							<Items>
								<ext:TextField runat="server" ID="uxReasonForNoWorkField" FieldLabel="Reason for no work" Flex="75" ReadOnly="true" />
								<ext:Component ID="Component4" runat="server" Flex="25" />
							</Items>
						</ext:FieldContainer>
						<ext:FieldContainer ID="FieldContainer6" runat="server" Layout="HBoxLayout" AnchorHorizontal="100%">
							<Items>
								<ext:TextField runat="server" ID="uxHotelField" FieldLabel="Hotel" Flex="25" ReadOnly="true" LabelWidth="50" />
								<ext:TextField runat="server" ID="uxCityField" FieldLabel="City" Flex="25" ReadOnly="true" LabelWidth="50" />
								<ext:TextField runat="server" ID="uxFooterStateField" FieldLabel="State" Flex="25" ReadOnly="true" LabelWidth="50" />
								<ext:TextField runat="server" ID="uxPhoneField" FieldLabel="Phone" Flex="25" ReadOnly="true" LabelWidth="50" />
							</Items>
						</ext:FieldContainer>
						<ext:FieldContainer ID="FieldContainer7" runat="server" Layout="HBoxLayout" AnchorHorizontal="100%">
							<Items>
								<ext:Component ID="Component5" runat="server" Flex="50" />
								<ext:TextField runat="server" ID="uxContractNameField" FieldLabel="Contract Rep Name" Flex="50" ReadOnly="true" />
							</Items>
						</ext:FieldContainer>
						<ext:FieldContainer ID="FieldContainer8" runat="server" Layout="HBoxLayout" AnchorHorizontal="100%">
							<Items>
								<ext:Image runat="server" Height="240" ID="uxForemanImage" Flex="50" />
								<ext:Image runat="server" Height="240" ID="uxContractImage" Flex="50" />
							</Items>
						</ext:FieldContainer>
                        <ext:FieldContainer runat="server" Layout="HBoxLayout" AnchorHorizontal="100%">
                            <Items>
                                <ext:TextField runat="server" Flex="50" ID="uxDOTRep" ReadOnly="true" FieldLabel="DOT Rep Name" />
                                <ext:Image runat="server" Height="240" ID="uxDOTImage" Flex="50" />
                            </Items>
                        </ext:FieldContainer>
					 </Items>
				</ext:FormPanel>
			</Items>
		</ext:Panel>
	</form>
</body>
</html>

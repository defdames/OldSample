<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCombinedTab_DBI.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umCombinedTab_DBI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<link href="../../../Resources/StyleSheets/main.css" rel="stylesheet" />
</head>
<body>
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
	<form id="form1" runat="server">
		<ext:Panel runat="server" ID="uxMainContainer" Layout="AutoLayout">
			<Items>
				<ext:FormPanel runat="server"
					ID="uxHeaderPanel" Padding="10" BodyPadding="5" MaxWidth="1000" Layout="FormLayout">
					<Items>
						<ext:TextField runat="server" ID="uxDateField" FieldLabel="Date" Width="200"  ReadOnly="true" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxHeaderField" FieldLabel="DRS Id" Width="200" ReadOnly="true" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxOracleField" FieldLabel="Oracle DRS Id" Width="200" ReadOnly="true" />
						<ext:TextField runat="server" ID="uxProjectField" FieldLabel="Project" Width="600" ReadOnly="true" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxSubDivisionField" FieldLabel="Sub-Division" Width="300" ReadOnly="true" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxSupervisorField" FieldLabel="Supervisor/Area Manager" Width="500" ReadOnly="true" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxContractorField" FieldLabel="Contractor" Width="500" ReadOnly="true" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxLicenseField" FieldLabel="Business License" Width="250" ReadOnly="true" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxStateField" FieldLabel="State" Width="250" ReadOnly="true" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxTypeField" FieldLabel="Type of Work" Width="250" ReadOnly="true" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxDensityField" FieldLabel="Density" Width="200" ReadOnly="true" LabelWidth="100" />
					</Items>
				</ext:FormPanel>                    
				<ext:GridPanel runat="server"
					ID="uxEmployeeGrid"
					Title="Employees"
					Padding="10"
					MaxWidth="1000">
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
										<ext:ModelField Name="PER_DIEM" />
										<ext:ModelField Name="COMMENTS" />
									</Fields>
								</ext:Model>
							</Model>
						</ext:Store>
					</Store>
					<ColumnModel>
						<Columns>
							<ext:Column ID="Column9" runat="server" DataIndex="EMPLOYEE_NAME" Text="Employee Name" Flex="8" />
							<ext:Column ID="Column10" runat="server" DataIndex="NAME" Text="Equipment Name" Flex="9" />
							<ext:DateColumn ID="DateColumn2" runat="server" DataIndex="TIME_IN" Text="Time In" Flex="6" Format="M/d/yyyy h:mm tt" />
							<ext:DateColumn ID="DateColumn3" runat="server" DataIndex="TIME_OUT" Text="Time Out" Flex="6" Format="M/d/yyyy h:mm tt" />
							<ext:Column ID="Column11" runat="server" DataIndex="TRAVEL_TIME_FORMATTED" Text="Travel Time" Flex="6" />
							<ext:Column ID="Column12" runat="server" DataIndex="DRIVE_TIME_FORMATTED" Text="Drive Time" Flex="6" />
							<ext:Column ID="Column13" runat="server" DataIndex="PER_DIEM" Text="Per Diem" Flex="5" />
							<ext:Column ID="Column14" runat="server" DataIndex="COMMENTS" Text="Comments" Flex="9" />
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
								<ext:Model runat="server">
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
								Text="Project Number" />
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
								Text="Starting Units"/>
							<ext:Column ID="Column52" runat="server"
								DataIndex="ODOMETER_END"
								Text="Ending Units" />
						</Columns>
					</ColumnModel>
				</ext:GridPanel>
				<ext:GridPanel runat="server"
					ID="uxProductionGrid"
					Title="Production"
					Padding="10"
					MaxWidth="1000">
					<Store>
						<ext:Store runat="server"
							ID="uxProductionStore">
							<Model>
								<ext:Model ID="Model3" runat="server">
									<Fields>
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
							<ext:Column ID="Column15" runat="server" DataIndex="DESCRIPTION" Text="Task Name" />
							<ext:Column ID="Column16" runat="server" DataIndex="WORK_AREA" Text="Spray/Work Area" />
							<ext:Column ID="Column17" runat="server" DataIndex="POLE_FROM" Text="Pole From" />
							<ext:Column ID="Column18" runat="server" DataIndex="POLE_TO" Text="Pole To" />
							<ext:Column ID="Column19" runat="server" DataIndex="ACRES_MILE" Text="Acres/Mile" />
							<ext:Column ID="Column20" runat="server" DataIndex="QUANTITY" Text="Gallons" />
						</Columns>
					</ColumnModel>
				</ext:GridPanel>
				<ext:GridPanel runat="server"
					ID="uxWeatherGrid"
					Title="Weather"
					Padding="10"
					MaxWidth="1000">
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
							<ext:DateColumn ID="DateColumn6" runat="server" DataIndex="WEATHER_DATE_TIME" Text="Date/Time" Format="M/d/yyyy h:mm tt" />
							<ext:Column ID="Column21" runat="server" DataIndex="WIND_DIRECTION" Text="Wind Direction" />
							<ext:Column ID="Column22" runat="server" DataIndex="WIND_VELOCITY" Text="Wind Velocity" />
							<ext:Column ID="Column23" runat="server" DataIndex="TEMP" Text="Temperature" />
							<ext:Column ID="Column24" runat="server" DataIndex="HUMIDITY" Text="Humidity" />
							<ext:Column ID="Column25" runat="server" DataIndex="COMMENTS" Text="Comments" />
						</Columns>
					</ColumnModel>
				</ext:GridPanel>
				<ext:GridPanel runat="server"
					ID="uxChemicalGrid"
					Title="Chemical Mix"
					Padding="10"
					MaxWidth="1000">
					<Store>
						<ext:Store runat="server"
							ID="uxChemicalStore">
							<Model>
								<ext:Model ID="Model5" runat="server">
									<Fields>
										<ext:ModelField Name="CHEMICAL_MIX_NUMBER" />
										<ext:ModelField Name="TARGET_AREA" />
										<ext:ModelField Name="GALLON_ACRE" />
										<ext:ModelField Name="GALLON_STARTING" />
										<ext:ModelField Name="GALLON_MIXED" />
										<ext:ModelField Name="GALLON_REMAINING" />
										<ext:ModelField Name="STATE" />
										<ext:ModelField Name="COUNTY" />
										<ext:ModelField Name="TOTAL" />
										<ext:ModelField Name="USED" />
										<ext:ModelField Name="ACRES_SPRAYED" />
									</Fields>
								</ext:Model>
							</Model>
						</ext:Store>
					</Store>
					<ColumnModel>
						<Columns>
							<ext:Column ID="Column26" runat="server" DataIndex="CHEMICAL_MIX_NUMBER" Text="Mix #" Flex="1" />
							<ext:Column ID="Column27" runat="server" DataIndex="TARGET_AREA" Text="Target Area" Flex="1" />
							<ext:Column ID="Column28" runat="server" DataIndex="GALLON_ACRE" Text="Gallons/Acre" Flex="1" />
							<ext:Column ID="Column29" runat="server" DataIndex="GALLON_STARTING" Text="Gallons Starting" Flex="1" />
							<ext:Column ID="Column30" runat="server" DataIndex="GALLON_MIXED" Text="Gallon Mixed" Flex="1" />
							<ext:Column runat="server" DataIndex="GALLON_REMAINING" Text="Gallon Remaining" Flex="1" />
							<ext:Column ID="Column31" runat="server" DataIndex="TOTAL" Text="Total Gallons" Flex="1" />
							<ext:Column ID="Column32" runat="server" DataIndex="USED" Text="Gallons Used" Flex="1" />
							<ext:Column ID="Column33" runat="server" DataIndex="ACRES_SPRAYED" Text="Acres Sprayed" Flex="1" />
							<ext:Column ID="Column34" runat="server" DataIndex="STATE" Text="State" Flex="1" />
							<ext:Column ID="Column35" runat="server" DataIndex="COUNTY" Text="County" Flex="1" />
						</Columns>
					</ColumnModel>
				</ext:GridPanel>
				<ext:GridPanel runat="server"
					ID="uxInventoryGrid"
					Title="Inventory"
					Padding="10"
					MaxWidth="1000">
					<Store>
						<ext:Store runat="server"
							ID="uxInventoryStore">
							<Model>
								<ext:Model ID="Model6" runat="server">
									<Fields>
										<ext:ModelField Name="CHEMICAL_MIX_NUMBER" />
										<ext:ModelField Name="INV_NAME" />
										<ext:ModelField Name="SUB_INVENTORY_SECONDARY_NAME" />
										<ext:ModelField Name="DESCRIPTION" />
										<ext:ModelField Name="RATE" />
										<ext:ModelField Name="TOTAL" />
										<ext:ModelField Name="UNIT_OF_MEASURE" />
										<ext:ModelField Name="EPA_NUMBER" />
									</Fields>
								</ext:Model>
							</Model>
						</ext:Store>
					</Store>
					<ColumnModel>
						<Columns>
							<ext:Column ID="Column36" runat="server" DataIndex="CHEMICAL_MIX_NUMBER" Text="Mix Number" />
							<ext:Column runat="server" DataIndex="INV_NAME" Text="Inventory Org" />
							<ext:Column ID="Column37" runat="server" DataIndex="SUB_INVENTORY_SECONDARY_NAME" Text="Sub-Inv Name" />
							<ext:Column ID="Column38" runat="server" DataIndex="DESCRIPTION" Text="Item" />
							<ext:Column ID="Column39" runat="server" DataIndex="RATE" Text="Rate" />
							<ext:Column runat="server" DataIndex="TOTAL" Text="Total" />
							<ext:Column ID="Column40" runat="server" DataIndex="UNIT_OF_MEASURE" Text="Unit" />
							<ext:Column ID="Column41" runat="server" DataIndex="EPA_NUMBER" Text="EPA Number" />
						</Columns>
					</ColumnModel>
				</ext:GridPanel>
				<ext:FormPanel runat="server" ID="uxFooterPanel" Padding="10" BodyPadding="5" MaxWidth="1000">
					<Items>
						<ext:TextField runat="server" ID="uxReasonForNoWorkField" FieldLabel="Reason for no work" Width="700" ReadOnly="true" LabelWidth="100" />
						<ext:TextField runat="server" ID="uxHotelField" FieldLabel="Hotel" ReadOnly="true" LabelWidth="100" Width="400" />
						<ext:TextField runat="server" ID="uxCityField" FieldLabel="City" ReadOnly="true" LabelWidth="100" Width="300" />
						<ext:TextField runat="server" ID="uxFooterStateField" FieldLabel="State" ReadOnly="true" LabelWidth="100" Width="300" />
						<ext:TextField runat="server" ID="uxPhoneField" FieldLabel="Phone" ReadOnly="true" LabelWidth="100" Width="300" />
						<ext:TextField runat="server" ID="uxForemanNameField" FieldLabel="Foreman Name" LabelWidth="100" Width="500" ReadOnly="true" />
						<ext:FieldContainer runat="server" FieldLabel="Foreman Signature" LabelWidth="100">
							<Items>
								<ext:Image runat="server" Height="214" ID="uxForemanImage" Width="320" />
							</Items>
						</ext:FieldContainer>
						<ext:TextField runat="server" ID="uxContractNameField" FieldLabel="Contract Rep Name" Width="500" ReadOnly="true" LabelWidth="100" />
						<ext:FieldContainer runat="server" FieldLabel="Contract Rep Signature" LabelWidth="100">
							<Items>
								<ext:Image runat="server" Height="214" ID="uxContractImage" Width="320" />
							</Items>
						</ext:FieldContainer>
					 </Items>
				</ext:FormPanel>
			</Items>
		</ext:Panel>
	</form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEmployeesTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umEmployeesTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
		<script>
			var valDateTime = function () {
				var me = this,
					v = me.getValue(),
					field;

				if (me.startDateField) {
					field = Ext.getCmp(me.startDateField);
					field.setMaxValue(v);
					me.timeRangeMax = v;
				} else if (me.endDateField) {
					field = Ext.getCmp(me.endDateField);
					field.setMinValue(v);
					me.timeRangeMin = v;
				}

				field.validate();
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
								<ext:ModelField Name="TRAVEL_TIME" Type="Float"  />
								<ext:ModelField Name="DRIVE_TIME" Type="Float" />
								<ext:ModelField Name="PER_DIEM" Type="String"  />
								<ext:ModelField Name="COMMENTS" Type="String"  />
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
						Dataindex="TRAVEL_TIME"
						Flex="1" />
					<ext:Column runat="server"
						Text="Drive Time"
						DataIndex="DRIVE_TIME"
						Flex="1" />
					<ext:Column runat="server"
						Text="Per Diem"
						DataIndex="PER_DIEM"
						Flex="1" />
					<ext:Column runat="server"
						Text="Comments"
						DataIndex="COMMENTS"
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
							<Listeners>
								<Click Handler="#{uxAddEmployeeWindow}.show()" />
							</Listeners>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" />
						<ext:Button runat="server"
							ID="uxEditEmployee"
							Icon="ApplicationEdit"
							Text="Edit Employee"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deEditEmployeeForm">
									<ExtraParams>
										<ext:Parameter Name="EmployeeInfo" Value="Ext.encode(#{uxCurrentEmployeeGrid}.getRowsValues({selectedOnly : true}))" Mode="Raw" />
									</ExtraParams>
								</Click>                                
							</DirectEvents>
							<Listeners>
								<Click Handler="#{uxEditEmployeeWindow}.show()" />
							</Listeners>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
						<ext:Button runat="server"
							ID="uxRemoveEmployee"
							Icon="ApplicationDelete"
							Text="Remove Employee">
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
			<Listeners>
				<Select Handler="#{uxEditEmployee}.enable()" />
				<Deselect Handler="#{uxEditEmployee}.disable()" />
			</Listeners>
		</ext:GridPanel>
		
		<%-- Hidden Windows --%>
		<ext:Window runat="server"
			ID="uxAddEmployeeWindow"
			Layout="FormLayout"
			Hidden="true"
			Width="650">
			<Items>
				<ext:FormPanel runat="server"
					ID="uxAddEmployeeForm"
					Layout="FormLayout">
					<Items>
						<ext:DropDownField runat="server"
							ID="uxAddEmployeeEmpDropDown"
							Mode="ValueText"
							FieldLabel="Employee"
							AllowBlank="false"
							TabIndex="1">
							<Component>
								<ext:GridPanel runat="server"
									ID="uxAddEmployeeEmpGrid"
									Layout="HBoxLayout">
									<Store>
										<ext:Store runat="server"
											ID="uxAddEmployeeEmpStore"
											PageSize="10"
											RemoteSort="true"
											OnReadData="deReadEmployeeData">
											<Model>
												<ext:Model runat="server">
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
											<Parameters>
												<ext:StoreParameter Name="Form" Value="EmployeeAdd" />
											</Parameters>
										</ext:Store>
									</Store>
									<ColumnModel>
										<Columns>
											<ext:Column runat="server" Text="Person ID" DataIndex="PERSON_ID" />
											<ext:Column runat="server" Text="Name" DataIndex="EMPLOYEE_NAME" />
											<ext:Column runat="server" Text="Job Name" DataIndex="JOB_NAME" />
										</Columns>
									</ColumnModel>
									<TopBar>
										<ext:Toolbar runat="server">
											<Items>
												<ext:Button runat="server"
													ID="uxAddEmployeeRegion"
													Icon="Group"
													Text="All Regions"
													EnableToggle="true">
													<DirectEvents>
														<Click OnEvent="deRegionToggle">
															<ExtraParams>
																<ext:Parameter Name="Type" Value="EmployeeAdd" />
															</ExtraParams>
														</Click>
													</DirectEvents>
												</ext:Button>
											</Items>
										</ext:Toolbar>
									</TopBar>
									<BottomBar>
										<ext:PagingToolbar runat="server" />
									</BottomBar>
									<SelectionModel>
										<ext:RowSelectionModel runat="server" Mode="Single" />
									</SelectionModel>
									<DirectEvents>
										<SelectionChange OnEvent="deStoreGridValue">
											<ExtraParams>
												<ext:Parameter Name="PersonId" Value="#{uxAddEmployeeEmpGrid}.getSelectionModel().getSelection()[0].data.PERSON_ID" Mode="Raw" />
												<ext:Parameter Name="Name" Value="#{uxAddEmployeeEmpGrid}.getSelectionModel().getSelection()[0].data.EMPLOYEE_NAME" Mode="Raw" />
												<ext:Parameter Name="Type" Value="EmployeeAdd" />
											</ExtraParams>
										</SelectionChange>
									</DirectEvents>         
									<Plugins>
										<ext:FilterHeader runat="server" ID="uxAddEmployeeEmpFilter" Remote="true" />
									</Plugins>                                    
								</ext:GridPanel>
							</Component>
						</ext:DropDownField>
						<ext:DropDownField runat="server"
							ID="uxAddEmployeeEqDropDown"
							Mode="ValueText"
							FieldLabel="Equipment"
							AllowBlank="true"
							TabIndex="2">
							<Component>
								<ext:GridPanel runat="server"
									ID="uxAddEmployeeEqGrid"
									Layout="HBoxLayout">
									<Store>
										<ext:Store runat="server"
											ID="uxAddEmployeeEqStore"
											OnReadData="deReadEquipmentData"
											AutoDataBind="true">
											<Model>
												<ext:Model runat="server">
													<Fields>
														<ext:ModelField Name="EQUIPMENT_ID" Type="Int" />
														<ext:ModelField Name="NAME" Type="String" />
														<ext:ModelField Name="PROJECT_ID" Type="Int" />
													</Fields>
												</ext:Model>
											</Model>
											<Parameters>
												<ext:StoreParameter Name="Form" Value="EquipmentAdd" />
											</Parameters>
											<Proxy>
												<ext:PageProxy />
											</Proxy>
										</ext:Store>
									</Store>
									<ColumnModel>
										<Columns>
											<ext:Column runat="server" Text="Equipment Id" DataIndex="EQUIPMENT_ID" />
											<ext:Column runat="server" Text="Name" DataIndex="NAME" />
											<ext:Column runat="server" Text="Project Id" DataIndex="PROJECT_ID" />
										</Columns>
									</ColumnModel>
									<SelectionModel>
										<ext:RowSelectionModel Mode="Single" />
									</SelectionModel>
									<DirectEvents>
										<SelectionChange OnEvent="deStoreGridValue">
											<ExtraParams>
												<ext:Parameter Name="EquipmentId" Value="#{uxAddEmployeeEqGrid}.getSelectionModel().getSelection()[0].data.EQUIPMENT_ID" Mode="Raw" />
												<ext:Parameter Name="Name" Value="#{uxAddEmployeeEqGrid}.getSelectionModel().getSelection()[0].data.NAME" Mode="Raw" />
												<ext:Parameter Name="Type" Value="EquipmentAdd" />
											</ExtraParams>
										</SelectionChange>
									</DirectEvents>
								</ext:GridPanel>
							</Component>
						</ext:DropDownField>
						<ext:FieldContainer runat="server"
							FieldLabel="Time In"
							MsgTarget="Side">
							<Items>
								<ext:DateField runat="server"
									ID="uxAddEmployeeTimeInDate"
									Vtype="daterange"
									EndDateField="uxAddEmployeeTimeOutDate"
									EnableKeyEvents="true"
									AllowBlank="false"
									TabIndex="3">
									<Listeners>
										<KeyUp Fn="valDateTime" />
										<Change Handler="#{uxAddEmployeeTimeOutDate}.setValue(#{uxAddEmployeeTimeInDate}.value)" />
									</Listeners>
								</ext:DateField>
								<ext:TimeField runat="server"
									ID="uxAddEmployeeTimeInTime"
									Vtype="daterange"
									EndDateField="uxAddEmployeeTimeOutTime"
									EnableKeyEvents="true"
									Increment="30" 
									SelectedTime="09:00" 
									Format="H:mm"
									AllowBlank="false"
									TabIndex="4">
									<Listeners>
										<KeyUp Fn="valDateTime" />
									</Listeners>
								</ext:TimeField>
							</Items>
							<Defaults>
								<ext:Parameter Name="Flex" Value="1" Mode="Raw" />
								<ext:Parameter Name="HideLabel" Value="true" Mode="Raw" />
							</Defaults>
						</ext:FieldContainer>
						<ext:FieldContainer ID="FieldContainer1" runat="server"
							FieldLabel="Time Out"
							MsgTarget="Side" >
							<Defaults>
								<ext:Parameter Name="Flex" Value="1" Mode="Raw" />
								<ext:Parameter Name="HideLabel" Value="true" Mode="Raw" />
							</Defaults>
							<Items>
								<ext:DateField runat="server"
									ID="uxAddEmployeeTimeOutDate"
									Vtype="daterange"
									StartDateField="uxAddEmployeeTimeInDate"
									EnableKeyEvents="true"
									AllowBlank="false"
									TabIndex="5" >
									<Listeners>
										<KeyUp Fn="valDateTime" />
									</Listeners>
								</ext:DateField>
								<ext:TimeField runat="server"
									ID="uxAddEmployeeTimeOutTime"
									Increment="30" 
									SelectedTime="09:00" 
									Format="H:mm"
									Vtype="daterange"
									StartDateField="uxAddEmployeeTimeInTime"
									EnableKeyEvents="true"
									AllowBlank="false"
									TabIndex="6">
									<Listeners>
										<KeyUp Fn="valDateTime" />
									</Listeners>
								</ext:TimeField>
							</Items>
						</ext:FieldContainer>
						<ext:TextField runat="server"
							ID="uxAddEmployeeTravelTime"
							FieldLabel="Travel Time"
							AllowBlank="true"
							TabIndex="7" />
						<ext:TextField runat="server"
							ID="uxAddEmployeeDriveTime"
							FieldLabel="Drive Time"
							AllowBlank="true"
							TabIndex="8" />
						<ext:Checkbox runat="server"
							ID="uxAddEmployeePerDiem"
							FieldLabel="Per Diem"
							TabIndex="9" />
						<ext:TextArea runat="server"
						   FieldLabel="Comments"
						   ID="uxAddEmployeeComments"
						   AllowBlank="true"
							TabIndex="10" />
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxAddEmployeeSubmit"
							Icon="Add"
							Text="Submit"
							Disabled="true"
							TabIndex="11">
							<DirectEvents>
								<Click OnEvent="deAddEmployee" />
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxAddEmployeeCancel"
							Icon="Delete"
							Text="Cancel"
							TabIndex="12">
							<Listeners>
								<Click Handler="#{uxAddEmployeeForm}.reset();
									#{uxAddEmployeeWindow}.hide()" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxAddEmployeeSubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>
			</Items>
			<Listeners>
				<Show Handler="#{uxAddEmployeeEmpDropDown}.focus()" />
			</Listeners>
		</ext:Window>
		<ext:Window runat="server"
			ID="uxEditEmployeeWindow"
			Layout="FormLayout"
			Hidden="true"
			Width="650"
			Title="Edit Employee">
			<Items>
				<ext:FormPanel runat="server"
					ID="uxEditEmployeeForm"
					Layout="FormLayout">
					<Items>
						<ext:DropDownField runat="server"
							ID="uxEditEmployeeEmpDropDown"
							Mode="ValueText"
							FieldLabel="Employee"
							AllowBlank="false"
							TabIndex="1">
							<Component>
								<ext:GridPanel runat="server"
									ID="uxEditEmployeeEmpGrid"
									Layout="HBoxLayout">
									<Store>
										<ext:Store runat="server"
											ID="uxEditEmployeeEmpStore"
											PageSize="10"
											RemoteSort="true"
											OnReadData="deReadEmployeeData">
											<Model>
												<ext:Model runat="server">
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
											<Parameters>
												<ext:StoreParameter Name="Form" Value="EmployeeEdit" />
											</Parameters>
										</ext:Store>
									</Store>
									<ColumnModel>
										<Columns>
											<ext:Column runat="server" Text="Person ID" DataIndex="PERSON_ID" />
											<ext:Column runat="server" Text="Name" DataIndex="EMPLOYEE_NAME" />
											<ext:Column runat="server" Text="Job Name" DataIndex="JOB_NAME" />
										</Columns>
									</ColumnModel>
									<TopBar>
										<ext:Toolbar runat="server">
											<Items>
												<ext:Button runat="server"
													ID="uxEditEmployeeEmpRegion"
													Icon="Group"
													Text="All Regions"
													EnableToggle="true">
													<DirectEvents>
														<Click OnEvent="deRegionToggle">
															<ExtraParams>
																<ext:Parameter Name="Type" Value="EmployeeEdit"/>
															</ExtraParams>
														</Click>
													</DirectEvents>
												</ext:Button>
											</Items>
										</ext:Toolbar>
									</TopBar>
									<BottomBar>
										<ext:PagingToolbar runat="server" />
									</BottomBar>
									<SelectionModel>
										<ext:RowSelectionModel runat="server" Mode="Single" />
									</SelectionModel>
									<DirectEvents>
										<SelectionChange OnEvent="deStoreGridValue">
											<ExtraParams>
												<ext:Parameter Name="PersonId" Value="#{uxEditEmployeeEmpGrid}.getSelectionModel().getSelection()[0].data.PERSON_ID" Mode="Raw" />
												<ext:Parameter Name="Name" Value="#{uxEditEmployeeEmpGrid}.getSelectionModel().getSelection()[0].data.EMPLOYEE_NAME" Mode="Raw" />
												<ext:Parameter Name="Type" Value="EmployeeEdit" />
											</ExtraParams>
										</SelectionChange>
									</DirectEvents>         
									<Plugins>
										<ext:FilterHeader runat="server" ID="uxEditEmployeeEmpFilter" Remote="true" />
									</Plugins>                                    
								</ext:GridPanel>
							</Component>
							<Listeners>
								<Show Handler="#{uxEditEmployeeEmpDropDown}.focus()" />
							</Listeners>
						</ext:DropDownField>
						<ext:DropDownField runat="server"
							ID="uxEditEmployeeEqDropDown"
							Mode="ValueText"
							FieldLabel="Equipment"
							AllowBlank="true"
							TabIndex="2">
							<Component>
								<ext:GridPanel runat="server"
									ID="uxEditEmployeeEqGrid"
									Layout="HBoxLayout">
									<Store>
										<ext:Store runat="server"
											ID="uxEditEmployeeEqStore"
											OnReadData="deReadEquipmentData"
											AutoDataBind="true">
											<Model>
												<ext:Model runat="server">
													<Fields>
														<ext:ModelField Name="EQUIPMENT_ID" />
														<ext:ModelField Name="NAME" />
														<ext:ModelField Name="PROJECT_ID" />
													</Fields>
												</ext:Model>
											</Model>
											<Parameters>
												<ext:StoreParameter Name="Form" Value="EquipmentEdit" />
											</Parameters>	
											<Proxy>
												<ext:PageProxy />
											</Proxy>										
										</ext:Store>
									</Store>
									<ColumnModel>
										<Columns>
											<ext:Column runat="server" Text="Equipment Id" DataIndex="EQUIPMENT_ID"  />
											<ext:Column runat="server" Text="Name" DataIndex="NAME" />
											<ext:Column runat="server" Text="Project Id" DataIndex="PROJECT_ID" />
										</Columns>
									</ColumnModel>
									<SelectionModel>
										<ext:RowSelectionModel Mode="Single" />
									</SelectionModel>
									<DirectEvents>
										<SelectionChange OnEvent="deStoreGridValue">
											<ExtraParams>
												<ext:Parameter Name="EquipmentId" Value="#{uxEditEmployeeEqGrid}.getSelectionModel().getSelection()[0].data.EQUIPMENT_ID" Mode="Raw" />
												<ext:Parameter Name="Name" Value="#{uxEditEmployeeEqGrid}.getSelectionModel().getSelection()[0].data.NAME" Mode="Raw" />
												<ext:Parameter Name="Type" Value="EquipmentEdit" />
											</ExtraParams>
										</SelectionChange>
									</DirectEvents>                                        
								</ext:GridPanel>
							</Component>
						</ext:DropDownField>
						<ext:FieldContainer runat="server"
							FieldLabel="Time In"
							MsgTarget="Side">
							<Items>
								<ext:DateField runat="server"
									ID="uxEditEmployeeTimeInDate"
									Vtype="daterange"
									EndDateField="uxEditEmployeeTimeOutDate"
									EnableKeyEvents="true"
									AllowBlank="false"
									TabIndex="3">
									<Listeners>
										<KeyUp Fn="valDateTime" />
									</Listeners>
								</ext:DateField>
								<ext:TimeField runat="server"
									ID="uxEditEmployeeTimeInTime"
									Vtype="daterange"
									EndDateField="uxEditEmployeeTimeOutTime"
									EnableKeyEvents="true"
									Increment="30" 
									SelectedTime="09:00" 
									Format="H:mm"
									AllowBlank="false"
									TabIndex="4">
									<Listeners>
										<KeyUp Fn="valDateTime" />
									</Listeners>
								</ext:TimeField>
							</Items>
							<Defaults>
								<ext:Parameter Name="Flex" Value="1" Mode="Raw" />
								<ext:Parameter Name="HideLabel" Value="true" Mode="Raw" />
							</Defaults>
						</ext:FieldContainer>
						<ext:FieldContainer runat="server"
							FieldLabel="Time Out"
							MsgTarget="Side" >
							<Defaults>
								<ext:Parameter Name="Flex" Value="1" Mode="Raw" />
								<ext:Parameter Name="HideLabel" Value="true" Mode="Raw" />
							</Defaults>
							<Items>
								<ext:DateField runat="server"
									ID="uxEditEmployeeTimeOutDate"
									Vtype="daterange"
									StartDateField="uxEditEmployeeTimeInDate"
									EnableKeyEvents="true"
									AllowBlank="false"
									TabIndex="5">
									<Listeners>
										<KeyUp Fn="valDateTime" />
									</Listeners>
								</ext:DateField>
								<ext:TimeField runat="server"
									ID="uxEditEmployeeTimeOutTime"
									Increment="30" 
									SelectedTime="09:00" 
									Format="H:mm"
									Vtype="daterange"
									StartDateField="uxEditmployeeTimeInTime"
									EnableKeyEvents="true"
									AllowBlank="false"
									TabIndex="6">
									<Listeners>
										<KeyUp Fn="valDateTime" />
									</Listeners>
								</ext:TimeField>
							</Items>
						</ext:FieldContainer>
						<ext:TextField runat="server"
							ID="uxEditEmployeeDriveTime"
							FieldLabel="Drive Time"
							AllowBlank="true"
							TabIndex="7" />
						<ext:TextField runat="server"
							ID="uxEditEmployeeTravelTime"
							FieldLabel="Travel Time"
							AllowBlank="true"
							TabIndex="8" />
						<ext:Checkbox runat="server"
							ID="uxEditEmployeePerDiem"
							FieldLabel="Per Diem"
							AllowBlank="true"
							TabIndex="9" />
					   <ext:TextArea runat="server"
						   FieldLabel="Comments"
						   ID="uxEditEmployeeComments"
						   AllowBlank="true"
						   TabIndex="10" />
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxEditEmployeeSubmit"
							Icon="Add"
							Text="Submit"
							Disabled="true"
							TabIndex="11">
							<DirectEvents>
								<Click OnEvent="deEditEmployee">
									<ExtraParams>
										<ext:Parameter Name="EmployeeID" Value="#{uxCurrentEmployeeGrid}.getSelectionModel().getSelection()[0].data.EMPLOYEE_ID" Mode="Raw" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxEditEmployeeCancel"
							Icon="Delete"
							Text="Cancel"
							TabIndex="12">
							<Listeners>
								<Click Handler="#{uxEditEmployeeForm}.reset();
									#{uxEditEmployeeWindow}.hide()" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxEditEmployeeSubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>                    
			</Items>
			<Listeners>
				<Show Handler="#{uxEditEmployeeEmpDropDown}.focus()" />
			</Listeners>
		</ext:Window>
	</form>
</body>
</html>

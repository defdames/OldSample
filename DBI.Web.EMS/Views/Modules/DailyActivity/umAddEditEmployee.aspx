﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddEditEmployee.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umAddEditEmployee" %>

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
</head>
<body>
	<form id="form1" runat="server">
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
		<%-- Hidden Windows --%>
		<ext:Panel runat="server"
			ID="uxAddEmployeeWindow"
			Layout="FormLayout"
			Width="650">
			<Items>
				<ext:FormPanel runat="server"
					ID="uxAddEmployeeForm"
					Layout="FormLayout"
					Hidden="true">
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
												<ext:Model ID="Model1" runat="server">
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
											<ext:Column ID="Column1" runat="server" Text="Person ID" DataIndex="PERSON_ID" />
											<ext:Column ID="Column2" runat="server" Text="Name" DataIndex="EMPLOYEE_NAME" />
											<ext:Column ID="Column3" runat="server" Text="Job Name" DataIndex="JOB_NAME" />
										</Columns>
									</ColumnModel>
									<TopBar>
										<ext:Toolbar ID="Toolbar1" runat="server">
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
										<ext:PagingToolbar ID="PagingToolbar1" runat="server" />
									</BottomBar>
									<SelectionModel>
										<ext:RowSelectionModel ID="RowSelectionModel1" runat="server" Mode="Single" />
									</SelectionModel>
									<DirectEvents>
										<SelectionChange OnEvent="deStoreGridValue">
											<ExtraParams>
												<ext:Parameter Name="PersonId" Value="#{uxAddEmployeeEmpGrid}.getSelectionModel().getSelection()[0].data.PERSON_ID" Mode="Raw" />
												<ext:Parameter Name="Name" Value="#{uxAddEmployeeEmpGrid}.getSelectionModel().getSelection()[0].data.EMPLOYEE_NAME" Mode="Raw" />
												<ext:Parameter Name="Type" Value="EmployeeAdd" />
											</ExtraParams>
										</SelectionChange>
										<SelectionChange OnEvent="deCheckExistingPerDiem">
											<ExtraParams>
												<ext:Parameter Name="PersonId" Value="#{uxAddEmployeeEmpGrid}.getSelectionModel().getSelection()[0].data.PERSON_ID" Mode="Raw" />
												<ext:Parameter Name="Form" Value="Add" />
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
												<ext:Model ID="Model2" runat="server">
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
											<ext:Column ID="Column4" runat="server" Text="Equipment Id" DataIndex="EQUIPMENT_ID" />
											<ext:Column ID="Column5" runat="server" Text="Name" DataIndex="NAME" />
											<ext:Column ID="Column6" runat="server" Text="Project Id" DataIndex="PROJECT_ID" />
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
						<ext:FieldContainer ID="FieldContainer1" runat="server"
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
						<ext:FieldContainer ID="FieldContainer2" runat="server"
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
						<ext:DropDownField runat="server"
							ID="uxAddEmployeeRole"
							Mode="ValueText"
							FieldLabel="Role"
							AllowBlank="true"
							TabIndex="11"
							Hidden="true">
							<Component>
								<ext:GridPanel runat="server"
									ID="uxAddEmployeeRoleGrid"
									Layout="HBoxLayout">
									<Store>
										<ext:Store runat="server"
											ID="uxAddEmployeeRoleStore"
											OnReadData="deReadRoleData"
											AutoDataBind="true">
											<Model>
												<ext:Model ID="Model5" runat="server">
													<Fields>
														<ext:ModelField Name="MEANING" Type="Int" />
														<ext:ModelField Name="COUNTY" Type="String" />
														<ext:ModelField Name="STATE" Type="Int" />
													</Fields>
												</ext:Model>
											</Model>
											<Parameters>
												<ext:StoreParameter Name="Form" Value="Add" />
											</Parameters>
											<Proxy>
												<ext:PageProxy />
											</Proxy>
										</ext:Store>
									</Store>
									<ColumnModel>
										<Columns>
											<ext:Column ID="Column13" runat="server" Text="Role Name" DataIndex="MEANING" />
											<ext:Column ID="Column14" runat="server" Text="County" DataIndex="COUNTY" />
											<ext:Column ID="Column15" runat="server" Text="State" DataIndex="STATE" />
										</Columns>
									</ColumnModel>
									<SelectionModel>
										<ext:RowSelectionModel Mode="Single" />
									</SelectionModel>
									<DirectEvents>
										<SelectionChange OnEvent="deStoreRoleGridValue">
											<ExtraParams>
												<ext:Parameter Name="Meaning" Value="#{uxAddEmployeeRoleGrid}.getSelectionModel().getSelection()[0].data.MEANING" Mode="Raw" />
												<ext:Parameter Name="County" Value="#{uxAddEmployeeRoleGrid}.getSelectionModel().getSelection()[0].data.COUNTY" Mode="Raw" />
												<ext:Parameter Name="State" Value="#{uxAddEmployeeRoleGrid}.getSelectionModel().getSelection()[0].data.STATE" Mode="Raw" />
												<ext:Parameter Name="Type" Value="Add" />
											</ExtraParams>
										</SelectionChange>
									</DirectEvents>
								</ext:GridPanel>
							</Component>
						</ext:DropDownField>
						<ext:Hidden runat="server"  ID="uxAddEmployeeState" />
						<ext:Hidden runat="server"  ID="uxAddEmployeeCounty" />
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
									parentAutoLoadControl.hide();" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxAddEmployeeSubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>
				<ext:FormPanel runat="server"
					ID="uxEditEmployeeForm"
					Layout="FormLayout"
					Hidden="true">
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
												<ext:Model ID="Model3" runat="server">
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
											<ext:Column ID="Column7" runat="server" Text="Person ID" DataIndex="PERSON_ID" />
											<ext:Column ID="Column8" runat="server" Text="Name" DataIndex="EMPLOYEE_NAME" />
											<ext:Column ID="Column9" runat="server" Text="Job Name" DataIndex="JOB_NAME" />
										</Columns>
									</ColumnModel>
									<TopBar>
										<ext:Toolbar ID="Toolbar2" runat="server">
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
										<ext:PagingToolbar ID="PagingToolbar2" runat="server" />
									</BottomBar>
									<SelectionModel>
										<ext:RowSelectionModel ID="RowSelectionModel2" runat="server" Mode="Single" />
									</SelectionModel>
									<DirectEvents>
										<SelectionChange OnEvent="deStoreGridValue">
											<ExtraParams>
												<ext:Parameter Name="PersonId" Value="#{uxEditEmployeeEmpGrid}.getSelectionModel().getSelection()[0].data.PERSON_ID" Mode="Raw" />
												<ext:Parameter Name="Name" Value="#{uxEditEmployeeEmpGrid}.getSelectionModel().getSelection()[0].data.EMPLOYEE_NAME" Mode="Raw" />
												<ext:Parameter Name="Type" Value="EmployeeEdit" />
											</ExtraParams>
										</SelectionChange>
										<SelectionChange OnEvent="deCheckExistingPerDiem">
											<ExtraParams>
												<ext:Parameter Name="PersonId" Value="#{uxEditEmployeeEmpGrid}.getSelectionModel().getSelection()[0].data.PERSON_ID" Mode="Raw" />
												<ext:Parameter Name="Form" Value="Edit" />
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
												<ext:Model ID="Model4" runat="server">
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
											<ext:Column ID="Column10" runat="server" Text="Equipment Id" DataIndex="EQUIPMENT_ID"  />
											<ext:Column ID="Column11" runat="server" Text="Name" DataIndex="NAME" />
											<ext:Column ID="Column12" runat="server" Text="Project Id" DataIndex="PROJECT_ID" />
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
						<ext:FieldContainer ID="FieldContainer3" runat="server"
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
						<ext:FieldContainer ID="FieldContainer4" runat="server"
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
									Vtype="daterange"
									StartDateField="uxEditEmployeeTimeInTime"
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
						<ext:DropDownField runat="server"
							ID="uxEditEmployeeRole"
							FieldLabel="Role"
							Mode="ValueText"
							AllowBlank="true"
							TabIndex="11"
							Hidden="true">
							<Component>
								<ext:GridPanel runat="server"
									ID="uxEditEmployeeRoleGrid"
									Layout="HBoxLayout">
									<Store>
										<ext:Store runat="server"
											ID="uxEditEmployeeRoleStore"
											OnReadData="deReadRoleData"
											AutoDataBind="true">
											<Model>
												<ext:Model ID="Model6" runat="server">
													<Fields>
														<ext:ModelField Name="MEANING" Type="String" />
														<ext:ModelField Name="COUNTY" Type="String" />
														<ext:ModelField Name="STATE" Type="String" />
													</Fields>
												</ext:Model>
											</Model>
											<Parameters>
												<ext:StoreParameter Name="Form" Value="Edit" />
											</Parameters>
											<Proxy>
												<ext:PageProxy />
											</Proxy>
										</ext:Store>
									</Store>
									<ColumnModel>
										<Columns>
											<ext:Column ID="Column16" runat="server" Text="Role Name" DataIndex="MEANING" />
											<ext:Column ID="Column17" runat="server" Text="County" DataIndex="COUNTY" />
											<ext:Column ID="Column18" runat="server" Text="State" DataIndex="STATE" />
										</Columns>
									</ColumnModel>
									<SelectionModel>
										<ext:RowSelectionModel Mode="Single" />
									</SelectionModel>
									<DirectEvents>
										<SelectionChange OnEvent="deStoreRoleGridValue">
											<ExtraParams>
												<ext:Parameter Name="Meaning" Value="#{uxEditEmployeeRoleGrid}.getSelectionModel().getSelection()[0].data.MEANING" Mode="Raw" />
												<ext:Parameter Name="County" Value="#{uxEditEmployeeRoleGrid}.getSelectionModel().getSelection()[0].data.COUNTY" Mode="Raw" />
												<ext:Parameter Name="State" Value="#{uxEditEmployeeRoleGrid}.getSelectionModel().getSelection()[0].data.STATE" Mode="Raw" />
												<ext:Parameter Name="Type" Value="Edit" />
											</ExtraParams>
										</SelectionChange>
									</DirectEvents>
								</ext:GridPanel>
							</Component>
						</ext:DropDownField>
						<ext:Hidden runat="server"  ID="uxEditEmployeeState" />
						<ext:Hidden runat="server"  ID="uxEditEmployeeCounty" />
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxEditEmployeeSubmit"
							Icon="Add"
							Text="Submit"
							Disabled="true"
							TabIndex="11">
							<DirectEvents>
								<Click OnEvent="deEditEmployee" />
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxEditEmployeeCancel"
							Icon="Delete"
							Text="Cancel"
							TabIndex="12">
							<Listeners>
								<Click Handler="parentAutoLoadControl.hide();" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxEditEmployeeSubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>                    
			</Items>
			<Listeners>
				<AfterRender
					Handler="var win = parentAutoLoadControl.target || parentAutoLoadControl, //you can use just 'parentAutoLoadControl' after update to Ext.NET v2 beta.
									size = this.getSize();
 
								size.height += 100;
								size.width += 12;
								win.setSize(size);"
					Delay="100" />
			</Listeners>
		</ext:Panel>
	</form>
</body>
</html>

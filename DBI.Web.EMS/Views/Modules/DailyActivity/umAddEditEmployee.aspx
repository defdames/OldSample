<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddEditEmployee.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umAddEditEmployee" %>

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
				<ext:FormPanel runat="server"
					ID="uxAddEmployeeForm"
					DefaultAnchor="100%" Border="false"
					Width="600" DefaultButton="uxAddEmployeeSubmit" Padding="0">
					<Items>
                        <ext:Hidden runat="server" ID="uxFormType" />
						<ext:DropDownField runat="server"
							ID="uxAddEmployeeEmpDropDown"
							Mode="ValueText"
							FieldLabel="Employee"
							AllowBlank="false"
							Editable="false" Width="500">
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
											<ext:Column ID="Column1" runat="server" Text="Person ID" DataIndex="PERSON_ID" Flex="20" />
											<ext:Column ID="Column2" runat="server" Text="Name" DataIndex="EMPLOYEE_NAME" Flex="40" />
											<ext:Column ID="Column3" runat="server" Text="Job Name" DataIndex="JOB_NAME" Flex="40" />
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
						<ext:DropDownField runat="server" Editable="false"
							ID="uxAddEmployeeEqDropDown"
							Mode="ValueText"
							FieldLabel="Equipment"
							AllowBlank="true" Width="500">
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
														<ext:ModelField Name="SEGMENT1" Type="String" />
                                                        <ext:ModelField Name="ORGANIZATION_NAME" />
                                                        <ext:ModelField Name="CLASS_CODE" />
                                                        <ext:ModelField Name="ODOMETER_START" />
                                                        <ext:ModelField Name="ODOMETER_END" />
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
											<ext:Column ID="Column5" runat="server" Text="Name" DataIndex="NAME" Flex="15" />
                                            <ext:Column runat="server" Text="Class Code" DataIndex="CLASS_CODE" Flex="35" />
											<ext:Column ID="Column6" runat="server" Text="Project Number" DataIndex="SEGMENT1" Flex="20" />
                                            <ext:Column runat="server" DataIndex="ODOMETER_START" Text="Starting Units" Flex="15" />
                                            <ext:Column runat="server" DataIndex="ODOMETER_END" Text="Ending Units" Flex="15" />
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
							MsgTarget="Side" Width="500" Layout="HBoxLayout" >
							<Items>
								<ext:TextField runat="server"
									ID="uxAddEmployeeTimeInDate"
									IsRemoteValidation="true"
									AllowBlank="false"
									ReadOnly="true">
									<RemoteValidation OnValidation="ValidateDateTime">
										<ExtraParams>
											<ext:Parameter Name="Type" Value="Add" />
										</ExtraParams>
									</RemoteValidation>
								</ext:TextField>
								<ext:TimeField runat="server"
									ID="uxAddEmployeeTimeInTime"
									IsRemoteValidation="true"
									EnableKeyEvents="true"
									Increment="30"
									SelectedTime="09:00"
									AllowBlank="false">
									<RemoteValidation OnValidation="ValidateDateTime">
										<ExtraParams>
											<ext:Parameter Name="Type" Value="Add" />
										</ExtraParams>
									</RemoteValidation>
								</ext:TimeField>
							</Items>
							<Defaults>
								<ext:Parameter Name="Flex" Value="1" Mode="Raw" />
								<ext:Parameter Name="HideLabel" Value="true" Mode="Raw" />
							</Defaults>
						</ext:FieldContainer>
						<ext:FieldContainer ID="FieldContainer2" runat="server"
							FieldLabel="Time Out"
							MsgTarget="Side" Width="500" Layout="HBoxLayout">
							<Defaults>
								<ext:Parameter Name="Flex" Value="1" Mode="Raw" />
								<ext:Parameter Name="HideLabel" Value="true" Mode="Raw" />
							</Defaults>
							<Items>
								<ext:DateField runat="server"
									ID="uxAddEmployeeTimeOutDate"
									EnableKeyEvents="true"
									IsRemoteValidation="true"
									AllowBlank="false">
									<RemoteValidation OnValidation="ValidateDateTime">
										<ExtraParams>
											<ext:Parameter Name="Type" Value="Add" />
										</ExtraParams>
									</RemoteValidation>
								</ext:DateField>
								<ext:TimeField runat="server"
									ID="uxAddEmployeeTimeOutTime"
									Increment="30"
									IsRemoteValidation="true"
									SelectedTime="09:00"
									EnableKeyEvents="true"
									AllowBlank="false">
									<RemoteValidation OnValidation="ValidateDateTime">
										<ExtraParams>
											<ext:Parameter Name="Type" Value="Add" />
										</ExtraParams>
									</RemoteValidation>
								</ext:TimeField>
							</Items>
						</ext:FieldContainer>
                        <ext:FieldContainer runat="server" Layout="HBoxLayout" FieldLabel="Travel Time">
                            <Items>
                                <ext:NumberField runat="server"
					                ID="uxAddEmployeeTravelTimeHours"
					                FieldLabel="Hours"
					                AllowBlank="false"
					                ConstrainEmptyValue="true"
					                Text="0"
					                MinValue="0"
					                MaxValue="12" Width="200" Flex="1" />
				                <ext:NumberField runat="server"
					                ID="uxAddEmployeeTravelTimeMinutes"
					                FieldLabel="Minutes"
					                AllowBlank="false"
					                ConstrainEmptyValue="true"
					                Text="0"
					                MinValue="0"
					                MaxValue="59" Width="200" Flex="1" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer runat="server" Layout="HBoxLayout" FieldLabel="Drive Time" ID="uxAddEmployeeDriveTimeContainer" Hidden="true">
                            <Items>
                                <ext:NumberField runat="server"
					                ID="uxAddEmployeeDriveTimeHours"
					                FieldLabel="Hours"
					                AllowBlank="false"
					                ConstrainEmptyValue="true"
					                Text="0"
					                MinValue="0"
					                MaxValue="12" Width="200" Flex="1" />
				                <ext:NumberField runat="server"
					                ID="uxAddEmployeeDriveTimeMinutes"
					                FieldLabel="Minutes"
					                AllowBlank="false"
					                ConstrainEmptyValue="true"
					                Text="0"
					                MinValue="0"
					                MaxValue="59" Width="200" Flex="1" />
                            </Items>
                        </ext:FieldContainer>
				        <ext:FieldContainer runat="server" Layout="HBoxLayout" FieldLabel="Shop Time AM" ID="uxShopTimeAMContainer" Hidden="true">
                            <Items>
                                <ext:NumberField runat="server"
					                ID="uxAddEmployeeShopTimeAMHours"
					                FieldLabel="Hours"
					                AllowBlank="false"
					                ConstrainEmptyValue="true"
					                Text="0"
					                MinValue="0"
					                MaxValue="12" Width="200" Flex="1" />
				                <ext:NumberField runat="server"
					                ID="uxAddEmployeeShopTimeAMMinutes"
					                FieldLabel="Minutes"
					                AllowBlank="false"
					                ConstrainEmptyValue="true"
					                Text="0"
					                MinValue="0"
					                MaxValue="59" Width="200" Flex="1" />
                            </Items>
				        </ext:FieldContainer>
				        <ext:FieldContainer runat="server" Layout="HBoxLayout" FieldLabel="Shop Time PM" ID="uxShopTimePMContainer" Hidden="true">
                            <Items>
                                <ext:NumberField runat="server"
					                ID="uxAddEmployeeShopTimePMHours"
					                FieldLabel="Hours"
					                AllowBlank="false"
					                ConstrainEmptyValue="true"
					                Text="0"
					                MinValue="0"
					                MaxValue="12" Width="200" Flex="1" />
				                <ext:NumberField runat="server"
					                ID="uxAddEmployeeShopTimePMMinutes"
					                FieldLabel="Minutes"
					                AllowBlank="false"
					                ConstrainEmptyValue="true"
					                Text="0"
					                MinValue="0"
					                MaxValue="59" Width="200" Flex="1" />
                            </Items>
				        </ext:FieldContainer>
				
				<ext:Checkbox runat="server"
					ID="uxAddEmployeePerDiem"
					FieldLabel="Per Diem" />
				<ext:TextField runat="server"
					FieldLabel="License"
					ID="uxAddEmployeeLicense" Width="500" />
				<ext:TextArea runat="server"
					FieldLabel="Comments"
					ID="uxAddEmployeeComments" Width="500"
                    Rows="5"
					AllowBlank="true" />
				<ext:DropDownField runat="server"
					ID="uxAddEmployeeRole" Editable="false"
					Mode="ValueText"
					FieldLabel="Role"
					AllowBlank="true"
					Hidden="true" Width="500">
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
												<ext:ModelField Name="MEANING" />
												<ext:ModelField Name="COUNTY" Type="String" />
												<ext:ModelField Name="STATE"  />
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
									<ext:Column ID="Column13" runat="server" Text="Role Name" DataIndex="MEANING" Flex="33" />
									<ext:Column ID="Column14" runat="server" Text="County" DataIndex="COUNTY" Flex="34" />
									<ext:Column ID="Column15" runat="server" Text="State" DataIndex="STATE" Flex="33" />
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
						<ext:Hidden runat="server" ID="uxAddEmployeeState" />
						<ext:Hidden runat="server" ID="uxAddEmployeeCounty" />
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxAddEmployeeSubmit"
							Icon="Add"
							Text="Save"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deProcessForm">
									<EventMask ShowMask="true" />
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxAddEmployeeCancel"
							Icon="Delete"
							Text="Cancel">
							<Listeners>
								<Click Handler="#{uxAddEmployeeForm}.reset();
							parentAutoLoadControl.close();" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxAddEmployeeSubmit}.setDisabled(!valid);" />
                        <AfterRender
					Handler="var win = parentAutoLoadControl.target || parentAutoLoadControl, //you can use just 'parentAutoLoadControl' after update to Ext.NET v2 beta.
									size = this.getSize();
                                size.height += 34;
								size.width += 24;
								win.setSize(size);"
					Delay="100" />
					</Listeners>
				</ext:FormPanel>
	</form>
</body>
</html>

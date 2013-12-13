<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEquipmentTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umEquipmentTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" />
	<script>
		Ext.apply(Ext.form.VTypes, {
			numberrange: function (val, field) {
				if (!val) {
					return;
				}

				if (field.startNumberField && (!field.numberRangeMax || (val != field.numberRangeMax))) {
					var start = Ext.getCmp(field.startNumberField);

					if (start) {
						start.setMaxValue(val);
						field.numberRangeMax = val;
						start.validate();
					}
				} else if (field.endNumberField && (!field.numberRangeMin || (val != field.numberRangeMin))) {
					var end = Ext.getCmp(field.endNumberField);

					if (end) {
						end.setMinValue(val);
						field.numberRangeMin = val;
						end.validate();
					}
				}

				return true;
			}
		});
	</script>
</head>
<body>
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
	<form id="form1" runat="server">
		<ext:GridPanel runat="server"
			ID="uxCurrentEquipment" Layout="HBoxLayout">
			<TopBar>
				<ext:Toolbar runat="server"
					ID="uxCurrentEquipmentTop">
					<Items>
						<ext:Button runat="server"
							ID="uxAddEquipmentButton"
							Icon="ApplicationAdd"
							Text="Add Equipment">
							<Listeners>
								<Click Handler="#{uxAddEquipmentWindow}.show()" />
							</Listeners>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" />
						<ext:Button runat="server"
							ID="uxEditEquipmentButton"
							Icon="ApplicationEdit"
							Text="Edit Equipment"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deEditEquipmentForm">
									<ExtraParams>
										<ext:Parameter Name="EquipmentDetails" Value="Ext.encode(#{uxCurrentEquipment}.getRowsValues({selectedOnly : true}))" Mode="Raw"/>
									</ExtraParams>                                    
								</Click>                                
							</DirectEvents>
							<Listeners>
								<Click Handler="#{uxEditEquipmentWindow}.show()" />
							</Listeners>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
						<ext:Button runat="server"
							ID="uxRemoveEquipmentButton"
							Icon="ApplicationDelete"
							Text="Remove Equipment">
							<DirectEvents>
								<Click OnEvent="deRemoveEquipment">
									<Confirmation ConfirmRequest="true" Title="Remove?" Message="Do you really want to remove?" />
									<ExtraParams>
										<ext:Parameter Name="EquipmentId" Value="#{uxCurrentEquipment}.getSelectionModel().getSelection()[0].data.EQUIPMENT_ID" Mode="Raw" />
									</ExtraParams>
								</Click>                                
							</DirectEvents>
						</ext:Button>
					</Items>
				</ext:Toolbar>
			</TopBar>
			<Store>
				<ext:Store runat="server"
					ID="uxCurrentEquipmentStore"
					AutoDataBind="true">
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
							</Fields>
						</ext:Model>
					</Model>                    
				</ext:Store>
			</Store>
			<ColumnModel>
				<Columns>
					<ext:Column runat="server"
						DataIndex="PROJECT_ID"
						Text="Project ID" />
					<ext:Column runat="server"
						DataIndex="CLASS_CODE" 
						Text="Class Code"/>
					<ext:Column runat="server"
						DataIndex="ORGANIZATION_NAME" 
						Text="Organization Name"/>
					<ext:Column runat="server"
						DataIndex="ODOMETER_START" 
						Text="Meter Start"/>
					<ext:Column runat="server"
						DataIndex="ODOMETER_END"
						Text="Meter End" />
				</Columns>
			</ColumnModel>
			<SelectionModel>
				<ext:RowSelectionModel runat="server" AllowDeselect="true" Mode="Single" />
			</SelectionModel>
			<Listeners>
				<Select Handler="#{uxEditEquipmentButton}.enable()" />
				<Deselect Handler="#{uxEditEquipmentButton}.disable()" />
			</Listeners>
		</ext:GridPanel>
		<%-- Hidden Windows --%>
		<ext:Window runat="server"
			ID="uxAddEquipmentWindow"
			Layout="FormLayout"
			Hidden="true"
			Width="650"
			Title="Add Equipment">
			<Items>
				<ext:FormPanel runat="server"
					ID="uxAddEquipmentForm"
					Layout="FormLayout">
					<Items>
						<ext:DropDownField runat="server"
							ID="uxAddEquipmentDropDown"
							FieldLabel="Choose Equipment"
							Mode="ValueText"
							AllowBlank="false">
							<Component>
								<ext:GridPanel runat="server"
									ID="uxEquipmentGrid" 
									Layout="HBoxLayout">
									<Store>
										<ext:Store runat="server"
											ID="uxEquipmentStore"
											OnReadData="deReadGrid"
											PageSize="10"
											RemoteSort="true">
											<Model>
												<ext:Model runat="server">
													<Fields>
														<ext:ModelField Name="CLASS_CODE" Type="String"/>
														<ext:ModelField Name="NAME" Type="String"/>
														<ext:ModelField Name="ORG_ID" />
														<ext:ModelField Name="ORGANIZATION_ID" Type="Int" />
														<ext:ModelField Name="ORGANIZATION_NAME" Type="String" />
														<ext:ModelField Name="PROJECT_ID" Type="Int" />
														<ext:ModelField Name="PROJECT_STATUS_CODE" />
														<ext:ModelField Name="SEGMENT1" Type="Int" />
													</Fields>
												</ext:Model>
											</Model>											
											<Proxy>
												<ext:PageProxy />
											</Proxy>   
											<Parameters>
												<ext:StoreParameter Name="Form" Value="Add" />
											</Parameters>
										</ext:Store>
									</Store>
									<ColumnModel>
										<Columns>
											<ext:Column runat="server"
												ID="uxEquipmentClassCode"
												DataIndex="CLASS_CODE"
												Text="Class Code" />
											<ext:Column runat="server"
												ID="uxEquipmentName"
												DataIndex="NAME" 
												Text="Equipment Name"/>
											<ext:Column runat="server"
												ID="uxEquipmentOrgName"
												DataIndex="ORGANIZATION_NAME"
												Text="Organization Name" />           
											<ext:Column runat="server"
												ID="uxEquipmentOrganization"
												DataIndex="ORGANIZATION_ID"
												Text="Organization ID" />                
											<ext:Column runat="server"
												ID="uxEquipmentSegment"
												DataIndex="SEGMENT1" />
										</Columns>
									</ColumnModel>
									<Plugins>
										<ext:FilterHeader ID="uxAddEquipmentFilter" runat="server" Remote="true"  />
									</Plugins>									
									<TopBar>
										<ext:Toolbar runat="server"
											ID="uxEquipmentBar">
											<Items>
												<ext:Button runat="server"
													ID="uxAddEquipmentToggleOrg"
													EnableToggle="true"
													Text="All Regions"
													Icon="Group">
													<DirectEvents>
														<Toggle OnEvent="deReloadStore">
															<ExtraParams>
																<ext:Parameter Name="Type" Value="Equipment" />
															</ExtraParams>
														</Toggle>
													</DirectEvents>
												</ext:Button>
											</Items>
										</ext:Toolbar>
									</TopBar>
									<BottomBar>
										<ext:PagingToolbar runat="server"
											ID="uxAddEquipmentPaging" />
									</BottomBar>
									<SelectionModel>
										<ext:RowSelectionModel runat="server" Mode="Single" />
									</SelectionModel>
									<DirectEvents>
										<SelectionChange OnEvent="deStoreGridValue">
											<ExtraParams>
												<ext:Parameter Name="ProjectId" Value="#{uxEquipmentGrid}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
												<ext:Parameter Name="EquipmentName" Value="#{uxEquipmentGrid}.getSelectionModel().getSelection()[0].data.NAME" Mode="Raw" />
												<ext:Parameter Name="Form" Value="Add" />
											</ExtraParams>
										</SelectionChange>
									</DirectEvents>
								</ext:GridPanel>
							</Component>
						</ext:DropDownField>
						<ext:NumberField runat="server"
							ID="uxAddEquipmentStart"
							FieldLabel="Starting Meter"
							Vtype="numberrange" 
							EndNumberField="uxAddEquipmentEnd" />
						<ext:NumberField runat="server"
							ID="uxAddEquipmentEnd"
							FieldLabel="Ending Meter"
							Vtype="numberrange"
							StartNumberField="uxAddEquipmentStart" />
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxAddEquipmentSubmit"
							Text="Submit"
							Icon="Add"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deAddEquipment" />                                    
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxAddEquipmentCancel"
							Text="Cancel"
							Icon="Delete">
							<Listeners>
								<Click Handler ="#{uxAddEquipmentForm}.reset();
									#{uxAddEquipmentWindow}.hide()" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxAddEquipmentSubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>
			</Items>			
			<Listeners>
				<Show Handler="#{uxAddEquipmentDropDown}.focus()" />
			</Listeners>
		</ext:Window>
		<ext:Window runat="server"
			ID="uxEditEquipmentWindow"
			Layout="FormLayout"
			Hidden="true"
			Width="650"
			Title="Edit Equipment">
			<Items>
				<ext:FormPanel runat="server"
					ID="uxEditEquipmentForm"
					Layout="FormLayout">
					<Items>
						<ext:DropDownField runat="server"
							ID="uxEditEquipmentProject"
							FieldLabel="Choose Equipment"
							Mode="ValueText"
							AllowBlank="false">
							<Component>
								<ext:GridPanel runat="server"
									ID="uxEditEquipmentProjectGrid">
									<Store>
										<ext:Store runat="server"
											ID="uxEditEquipmentProjectStore"
											OnReadData="deReadGrid"
											PageSize="10"
											RemoteSort="true">
											<Model>
												<ext:Model runat="server">
													<Fields>
														<ext:ModelField Name="CLASS_CODE" Type="String"/>
														<ext:ModelField Name="NAME" Type="String"/>
														<ext:ModelField Name="HEADER_ID" />
														<ext:ModelField Name="ORG_ID" />
														<ext:ModelField Name="ORGANIZATION_ID" Type="Int" />
														<ext:ModelField Name="ORGANIZATION_NAME" Type="String" />
														<ext:ModelField Name="PROJECT_ID" Type="Int" />
														<ext:ModelField Name="PROJECT_STATUS_CODE" />
														<ext:ModelField Name="SEGMENT1" Type="Int" />
													</Fields>
												</ext:Model>
											</Model>
											<Proxy>
												<ext:PageProxy />
											</Proxy>
											<Parameters>
												<ext:StoreParameter Name="Form" Value="Edit" />
											</Parameters>
										</ext:Store>
									</Store>
									<ColumnModel>
										<Columns>
											<ext:Column runat="server"
												ID="Column1"
												DataIndex="CLASS_CODE"
												Text="Class Code" />
											<ext:Column runat="server"
												ID="Column2"
												DataIndex="NAME" 
												Text="Equipment Name"/>
											<ext:Column runat="server"
												ID="Column3"
												DataIndex="ORGANIZATION_NAME"
												Text="Organization Name" />           
											<ext:Column runat="server"
												ID="Column4"
												DataIndex="ORGANIZATION_ID"
												Text="Organization ID" />                
											<ext:Column runat="server"
												ID="Column5"
												DataIndex="SEGMENT1" />
										</Columns>
									</ColumnModel>
									<Plugins>
										<ext:FilterHeader runat="server" ID="uxEditEquipmentFilter" />
									</Plugins>
									<TopBar>
										<ext:Toolbar runat="server">
											<Items>
												<ext:Button runat="server"
													ID="uxEditRegion"
													EnableToggle="true"
													Text="All Regions">
													<DirectEvents>
														<Toggle OnEvent="deReloadStore">
															<ExtraParams>
																<ext:Parameter Name="Type" Value="Edit" />
															</ExtraParams>
														</Toggle>
													</DirectEvents>
												</ext:Button>
											</Items>
										</ext:Toolbar>
									</TopBar>
									<BottomBar>
										<ext:PagingToolbar runat="server" />
									</BottomBar>
									<DirectEvents>
										<SelectionChange OnEvent="deStoreGridValue">
											<ExtraParams>
												<ext:Parameter Name="ProjectId" Value="#{uxEditEquipmentProjectGrid}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
												<ext:Parameter Name="EquipmentName" Value="#{uxEditEquipmentProjectGrid}.getSelectionModel().getSelection()[0].data.NAME" Mode="Raw" />
												<ext:Parameter Name="Form" Value="Edit" />
											</ExtraParams>
										</SelectionChange>
									</DirectEvents>
								</ext:GridPanel>
							</Component>
						</ext:DropDownField>
						<ext:NumberField runat="server"
							ID="uxEditEquipmentStart"
							FieldLabel="Starting Meter"
							AllowBlank="true"
							Vtype="numberrange"
							EndNumberField="uxEditEquipmentEnd" />
						<ext:NumberField runat="server"
							ID="uxEditEquipmentEnd"
							FieldLabel="Ending Meter"
							AllowBlank="true"
							Vtype="numberrange"
							StartNumberField="uxEditEquipmentStart" />
					</Items>
					<Listeners>
				<Show Handler="#{uxEditEquipmentProject}.focus()" />
			</Listeners>
					<Buttons>
						<ext:Button runat="server"
							ID="uxEditEquipmentSubmit"
							Icon="Add"
							Text="Submit"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deEditEquipment">
									<ExtraParams>
										<ext:Parameter Name="EquipmentId" Value="#{uxCurrentEquipment}.getSelectionModel().getSelection()[0].data.EQUIPMENT_ID" Mode="Raw" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxEditEquipmentCancel"
							Icon="Delete"
							Text="Cancel">
							<Listeners>
								<Click Handler="#{uxEditEquipmentForm}.reset();
									#{uxEditEquipmentWindow}.hide()" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxEditEquipmentSubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>
			</Items>
		</ext:Window>			 
	</form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddEditEquipment.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umAddEditEquipment" %>

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
	<form id="form1" runat="server">
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
		<ext:FormPanel runat="server"
			ID="uxAddEquipmentForm"
			Layout="FormLayout"
			Hidden="true">
			<Items>
				<ext:DropDownField runat="server" Editable="false"
					ID="uxAddEquipmentDropDown"
					FieldLabel="Choose Equipment"
					Mode="ValueText"
					AllowBlank="false" Width="500">
					<Component>
						<ext:GridPanel runat="server"
							ID="uxEquipmentGrid" 
							Layout="HBoxLayout">
							<Store>
								<ext:Store runat="server"
									ID="uxEquipmentStore"
									OnReadData="deReadGrid"
									PageSize="10"
									RemoteSort="true"
									AutoDataBind="true">
									<Model>
										<ext:Model ID="Model1" runat="server">
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
										ID="uxEquipmentSegment"
										DataIndex="SEGMENT1"
										Text="Project Number" />
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
								<ext:RowSelectionModel ID="RowSelectionModel1" runat="server" Mode="Single" />
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
					Vtype="numberrange" Width="500"
					EndNumberField="uxAddEquipmentEnd" />
				<ext:NumberField runat="server"
					ID="uxAddEquipmentEnd"
					FieldLabel="Ending Meter"
					Vtype="numberrange" Width="500"
					StartNumberField="uxAddEquipmentStart" />
			</Items>
			<Buttons>
				<ext:Button runat="server"
					ID="uxAddEquipmentSubmit"
					Text="Submit"
					Icon="Add"
					Disabled="true">
					<DirectEvents>
						<Click OnEvent="deAddEquipment">
							<EventMask ShowMask="true" />
						</Click>
					</DirectEvents>
				</ext:Button>
				<ext:Button runat="server"
					ID="uxAddEquipmentCancel"
					Text="Cancel"
					Icon="Delete">
					<Listeners>
						<Click Handler ="#{uxAddEquipmentForm}.reset();
							parentAutoLoadControl.hide();" />
					</Listeners>
				</ext:Button>
			</Buttons>
			<Listeners>
				<ValidityChange Handler="#{uxAddEquipmentSubmit}.setDisabled(!valid);" />
			</Listeners>
		</ext:FormPanel>
		<ext:FormPanel runat="server"
			ID="uxEditEquipmentForm"
			Layout="FormLayout"
			Hidden="true">
			<Items>
				<ext:DropDownField runat="server" Editable="false"
					ID="uxEditEquipmentProject"
					FieldLabel="Choose Equipment"
					Mode="ValueText"
					AllowBlank="false" Width="500">
					<Component>
						<ext:GridPanel runat="server"
							ID="uxEditEquipmentProjectGrid">
							<Store>
								<ext:Store runat="server"
									ID="uxEditEquipmentProjectStore"
									OnReadData="deReadGrid"
									PageSize="10"
									RemoteSort="true"
									AutoDataBind="true">
									<Model>
										<ext:Model ID="Model2" runat="server">
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
										ID="Column5"
										DataIndex="SEGMENT1"
										Text="Project Number" />
								</Columns>
							</ColumnModel>
							<Plugins>
								<ext:FilterHeader runat="server" ID="uxEditEquipmentFilter" Remote="true" />
							</Plugins>
							<TopBar>
								<ext:Toolbar ID="Toolbar1" runat="server">
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
								<ext:PagingToolbar ID="PagingToolbar1" runat="server" />
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
					Vtype="numberrange" Width="500"
					EndNumberField="uxEditEquipmentEnd" />
				<ext:NumberField runat="server"
					ID="uxEditEquipmentEnd"
					FieldLabel="Ending Meter"
					AllowBlank="true"
					Vtype="numberrange" Width="500"
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
							<EventMask ShowMask="true" />
						</Click>
					</DirectEvents>
				</ext:Button>
				<ext:Button runat="server"
					ID="uxEditEquipmentCancel"
					Icon="Delete"
					Text="Cancel">
					<Listeners>
						<Click Handler="#{uxEditEquipmentForm}.reset();
							parentAutoLoadControl.hide();" />
					</Listeners>
				</ext:Button>
			</Buttons>
			<Listeners>
				<ValidityChange Handler="#{uxEditEquipmentSubmit}.setDisabled(!valid);" />
			</Listeners>
		</ext:FormPanel>		 
	</form>
</body>
</html>

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
            Border="false"
			Width="600" Height="400" DefaultButton="uxAddEquipmentSubmit" Padding="0">
			<Items>
                <ext:Hidden runat="server" ID="uxFormType" />
				<ext:DropDownField runat="server" Editable="false"
					ID="uxAddEquipmentDropDown"
					FieldLabel="Choose Equipment"
					Mode="ValueText"
					AllowBlank="false" Width="500" >
					<Component>
						<ext:GridPanel runat="server"
							ID="uxEquipmentGrid" 
							Layout="HBoxLayout" Floatable="true" Floating="true">
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
					FieldLabel="Starting Units"
					Vtype="numberrange" Width="500"
					EndNumberField="uxAddEquipmentEnd" />
				<ext:NumberField runat="server"
					ID="uxAddEquipmentEnd"
					FieldLabel="Ending Units"
					Vtype="numberrange" Width="500"
					StartNumberField="uxAddEquipmentStart" />
			</Items>
			<Buttons>
				<ext:Button runat="server"
					ID="uxAddEquipmentSubmit"
					Text="Save"
					Icon="Add"
					Disabled="true">
					<DirectEvents>
						<Click OnEvent="deProcessForm">
							<EventMask ShowMask="true" />
						</Click>
					</DirectEvents>
				</ext:Button>
				<ext:Button runat="server"
					ID="uxAddEquipmentCancel"
					Text="Cancel"
					Icon="Delete">
					<Listeners>
						<Click Handler ="
							parentAutoLoadControl.close();" />
					</Listeners>
				</ext:Button>
			</Buttons>
			<Listeners>
				<ValidityChange Handler="#{uxAddEquipmentSubmit}.setDisabled(!valid);" />
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

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEquipmentTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umEquipmentTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
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
						<ext:Button runat="server"
							ID="uxEditEquipmentButton"
							Icon="ApplicationEdit"
							Text="Edit Equipment">
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
						Text="Odometer Start"/>
					<ext:Column runat="server"
						DataIndex="ODOMETER_END"
						Text="Odometer End" />
				</Columns>
			</ColumnModel>
		</ext:GridPanel>
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
											PageSize="25"
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
							FieldLabel="Starting Odometer"
							AllowBlank="true"
							IsRemoteValidation="true">
							<RemoteValidation OnValidation="valOdometer">
								<ExtraParams>
									<ext:Parameter Name="Type" Value="Add" />
									<ext:Parameter Name="Start" Value="Start" />
								</ExtraParams>
							</RemoteValidation>
						</ext:NumberField>
						<ext:NumberField runat="server"
							ID="uxAddEquipmentEnd"
							FieldLabel="Ending Odometer"
							AllowBlank="true"
							IsRemoteValidation="true">
							<RemoteValidation OnValidation="valOdometer">
								<ExtraParams>
									<ext:Parameter Name="Type" Value="Add" />
									<ext:Parameter Name="Start" Value="End" />
								</ExtraParams>
							</RemoteValidation>
						</ext:NumberField>
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxAddEquipmentSubmit"
							Text="Submit"
							Icon="ApplicationGo"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deAddEquipment" />                                    
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxAddEquipmentCancel"
							Text="Cancel"
							Icon="ApplicationStop">
							<Listeners>
								<Click Handler ="#{uxAddEquipmentWindow}.hide()" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxAddEquipmentSubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>
			</Items>			
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
											PageSize="25"
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
							FieldLabel="Starting Odometer"
							AllowBlank="true"
							IsRemoteValidation="true">
							<RemoteValidation OnValidation="valOdometer">
								<ExtraParams>
									<ext:Parameter Name="Type" Value="Edit" />
									<ext:Parameter Name="Start" Value="Start" />
								</ExtraParams>
							</RemoteValidation>
						</ext:NumberField>
						<ext:NumberField runat="server"
							ID="uxEditEquipmentEnd"
							FieldLabel="Ending Odometer"
							AllowBlank="true"
							IsRemoteValidation="true">
							<RemoteValidation OnValidation="valOdometer">
								<ExtraParams>
									<ext:Parameter Name="Type" Value="Edit" />
									<ext:Parameter Name="Start" Value="End" />
								</ExtraParams>
							</RemoteValidation>
						</ext:NumberField>
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxEditEquipmentSubmit"
							Icon="ApplicationGo"
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
							Icon="ApplicationStop"
							Text="Cancel">
							<Listeners>
								<Click Handler="#{uxEditEquipmentWindow}.hide()" />
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

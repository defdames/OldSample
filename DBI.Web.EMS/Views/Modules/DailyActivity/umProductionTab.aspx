<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umProductionTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umProductionTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
	<form id="form1" runat="server">
		<ext:GridPanel runat="server"
			ID="uxCurrentProductionGrid"
			Layout="HBoxLayout">
			<Store>
				<ext:Store runat="server"
					ID="uxCurrentProductionStore"
					AutoDataBind="true">
					<Model>
						<ext:Model runat="server">
							<Fields>
								<ext:ModelField Name="PRODUCTION_ID" />
								<ext:ModelField Name="PROJECT_ID" />
								<ext:ModelField Name="LONG_NAME" />
								<ext:ModelField Name="TASK_ID" />
								<ext:ModelField Name="DESCRIPTION" />
								<ext:ModelField Name="TIME_IN" />
								<ext:ModelField Name="TIME_OUT" />
								<ext:ModelField Name="WORK_AREA" />
								<ext:ModelField Name="POLE_FROM" />
								<ext:ModelField Name="POLE_TO" />
								<ext:ModelField Name="ACRES_MILE" />
								<ext:ModelField Name="GALLONS" />
							</Fields>
						</ext:Model>
					</Model>
				</ext:Store>
			</Store>
			<ColumnModel>
				<Columns>
					<ext:Column runat="server"
						DataIndex="PROJECT_ID"
						Text="Project Id" />
					<ext:Column runat="server"
						DataIndex="LONG_NAME"
						Text="Project Name" />
					<ext:Column runat="server"
						DataIndex="DESCRIPTION"
						Text="Task Name" />
					<ext:Column runat="server"
						DataIndex="TIME_IN"
						Text="Time In" />
					<ext:Column runat="server"
						DataIndex="TIME_OUT"
						Text="Time Out" />
					<ext:Column runat="server"
						DataIndex="WORK_AREA"
						Text="Spray/Work Area" />
					<ext:Column runat="server"
						DataIndex="POLE_FROM"
						Text="Pole/MP From" />
					<ext:Column runat="server"
						DataIndex="POLE_TO"
						Text="Pole/MP To" />
					<ext:Column runat="server"
						DataIndex="ACRES_MILE"
						Text="Acres/Mile" />
					<ext:Column runat="server"
						DataIndex="GALLONS"
						Text="Gallons" />
				</Columns>
			</ColumnModel>
			<TopBar>
				<ext:Toolbar runat="server">
					<Items>
						<ext:Button runat="server"
							ID="uxAddProductionButton"
							Text="Add Production"
							Icon="ApplicationAdd">
							<Listeners>
								<Click Handler="#{uxAddProductionWindow}.show()" />
							</Listeners>
							<DirectEvents>
								<Click OnEvent="deGetTaskList">
									<ExtraParams>
										<ext:Parameter Name="Type" Value="Add" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxEditProductionButton"
							Text="Edit Production"
							Icon="ApplicationEdit">
							<DirectEvents>
								<Click OnEvent="deEditProductionForm">
									<ExtraParams>
										<ext:Parameter Name="ProductionInfo" Value="Ext.encode(#{uxCurrentProductionGrid}.getRowsValues({selectedOnly : true}))" Mode="Raw" />
										<ext:Parameter Name="Type" Value="Edit" />
									</ExtraParams>
								</Click>
							</DirectEvents>
							<Listeners>
								<Click Handler="#{uxEditProductionWindow}.show()" />
							</Listeners>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxRemoveProductionButton"
							Text="Remove Production"
							Icon="ApplicationDelete">
							<DirectEvents>
								<Click OnEvent="deRemoveProduction">
									<Confirmation ConfirmRequest="true" Title="Really?" Message="Do you really want to remove?" />
									<ExtraParams>
										<ext:Parameter Name="ProductionId" Value="#{uxCurrentProductionGrid}.getSelectionModel().getSelection()[0].data.PRODUCTION_ID" Mode="Raw" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
					</Items>
				</ext:Toolbar>
			</TopBar>
		</ext:GridPanel>
		<ext:Window runat="server"
			ID="uxAddProductionWindow"
			Layout="FormLayout"
			Hidden="true"
			Width="650">
			<Items>
				<ext:FormPanel runat="server"
					ID="uxAddProductionForm"
					Layout="FormLayout">
					<Items>
						<ext:ComboBox runat="server"
							ID="uxAddProductionTask"
							ValueField="TASK_ID"
							DisplayField="DESCRIPTION"
							FieldLabel="Select Task"
							AllowBlank="false">
							<Store>
								<ext:Store runat="server"
									ID="uxAddProductionTaskStore">
									<Model>
										<ext:Model runat="server">
											<Fields>
												<ext:ModelField Name="TASK_ID" />
												<ext:ModelField Name="DESCRIPTION" />
											</Fields>
										</ext:Model>
									</Model>
								</ext:Store>
							</Store>
						</ext:ComboBox>
						<ext:FieldContainer runat="server"
							ID="uxAddProductionTimeInContainer"
							FieldLabel="Time In">
							<Items>
								<ext:DateField runat="server"
									ID="uxAddProductionDateIn"
									AllowBlank="false" >
									<RemoteValidation OnValidation="valDate">
										<ExtraParams>
											<ext:Parameter Name="Type" Value="Add" />
											<ext:Parameter Name="InOut" Value="In" />
										</ExtraParams>
									</RemoteValidation>
								</ext:DateField>
								<ext:TimeField runat="server"
									ID="uxAddProductionTimeIn"
									AllowBlank="false" >
									<RemoteValidation OnValidation="valTime">
										<ExtraParams>
											<ext:Parameter Name="Type" Value="Add" />
											<ext:Parameter Name="InOut" Value="In" />
										</ExtraParams>
									</RemoteValidation>
								</ext:TimeField>
							</Items>
						</ext:FieldContainer>
						<ext:FieldContainer runat="server"
							ID="uxAddProductionTimeOutContainer"
							FieldLabel="Time Out">
							<Items>
								<ext:DateField runat="server"
									ID="uxAddProductionDateOut"
									AllowBlank="false" IsRemoteValidation="true">
									<RemoteValidation OnValidation="valDate">
										<ExtraParams>
											<ext:Parameter Name="Type" Value="Add" />
											<ext:Parameter Name="InOut" Value="Out" />
										</ExtraParams>
									</RemoteValidation>
								</ext:DateField>
								<ext:TimeField runat="server"
									ID="uxAddProductionTimeOut"
									AllowBlank="false" IsRemoteValidation="true">
									<RemoteValidation OnValidation="valTime">
										<ExtraParams>
											<ext:Parameter Name="Type" Value="Add" />
											<ext:Parameter Name="InOut" Value="Out" />
										</ExtraParams>
									</RemoteValidation>
								</ext:TimeField>
							</Items>
						</ext:FieldContainer>
						<ext:TextField runat="server"
							ID="uxAddProductionWorkArea"
							FieldLabel="Spray/Work Area"
							AllowBlank="false" />
						<ext:TextField runat="server"
							ID="uxAddProductionPoleFrom"
							FieldLabel="Pole/MP From"
							AllowBlank="false" />
						<ext:TextField runat="server"
							ID="uxAddProductionPoleTo"
							FieldLabel="Pole/MP To"
							AllowBlank="false" />
						<ext:TextField runat="server"
							ID="uxAddProductionAcresPerMile"
							FieldLabel="Acres/Mile"
							AllowBlank="false" />
						<ext:TextField runat="server"
							ID="uxAddProductionGallons"
							FieldLabel="Gallons"
							AllowBlank="false" />
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxAddProductionSubmit"
							Text="Submit"
							Icon="ApplicationGo"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deAddProduction" />
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxAddProductionCancel"
							Text="Cancel"
							Icon="ApplicationStop">
							<Listeners>
								<Click Handler="#{uxAddProductionForm}.reset();
									#{uxAddProductionWindow}.hide();" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxAddProductionSubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>
			</Items>
		</ext:Window>
		<ext:Window runat="server"
			ID="uxEditProductionWindow"
			Layout="FormLayout"
			Hidden="true"
			Width="650">
			<Items>
				<ext:FormPanel runat="server"
					ID="uxEditProductionForm"
					Layout="FormLayout">
					<Items>
						<ext:ComboBox runat="server"
							ID="uxEditProductionTask"
							ValueField="TASK_ID"
							DisplayField="DESCRIPTION"
							FieldLabel="Select Task"
							AllowBlank="false">
							<Store>
								<ext:Store runat="server"
									ID="uxEditProductionTaskStore">
									<Model>
										<ext:Model runat="server">
											<Fields>
												<ext:ModelField Name="TASK_ID" />
												<ext:ModelField Name="DESCRIPTION" />
											</Fields>
										</ext:Model>
									</Model>
								</ext:Store>
							</Store>
						</ext:ComboBox>
						<ext:FieldContainer runat="server"
							ID="uxEditProductionTimeInContainer"
							FieldLabel="Time In">
							<Items>
								<ext:DateField runat="server"
									ID="uxEditProductionDateIn"
									AllowBlank="false" >
									<RemoteValidation OnValidation="valDate">
										<ExtraParams>
											<ext:Parameter Name="Type" Value="Edit" />
											<ext:Parameter Name="InOut" Value="In" />
										</ExtraParams>
									</RemoteValidation>
								</ext:DateField>
								<ext:TimeField runat="server"
									ID="uxEditProductionTimeIn"
									AllowBlank="false">
									<RemoteValidation OnValidation="valTime">
										<ExtraParams>
											<ext:Parameter Name="Type" Value="Edit" />
											<ext:Parameter Name="InOut" Value="In" />
										</ExtraParams>
									</RemoteValidation>
								</ext:TimeField>
							</Items>
						</ext:FieldContainer>
						<ext:FieldContainer runat="server"
							ID="uxEditProductionTimeOutContainer"
							FieldLabel="Time Out">
							<Items>
								<ext:DateField runat="server"
									ID="uxEditProductionDateOut"
									AllowBlank="false" >
									<RemoteValidation OnValidation="valDate">
										<ExtraParams>
											<ext:Parameter Name="Type" Value="Edit" />
											<ext:Parameter Name="InOut" Value="Out" />
										</ExtraParams>
									</RemoteValidation>
								</ext:DateField>
								<ext:TimeField runat="server"
									ID="uxEditProductionTimeOut"
									AllowBlank="false" >
									<RemoteValidation OnValidation="valTime">
										<ExtraParams>
											<ext:Parameter Name="Type" Value="Edit" />
											<ext:Parameter Name="InOut" Value="Out" />
										</ExtraParams>
									</RemoteValidation>
								</ext:TimeField>
							</Items>
						</ext:FieldContainer>
						<ext:TextField runat="server"
							ID="uxEditProductionWorkArea"
							FieldLabel="Spray/Work Area"
							AllowBlank="false" />
						<ext:TextField runat="server"
							ID="uxEditProductionPoleFrom"
							FieldLabel="Pole/MP From"
							AllowBlank="false" />
						<ext:TextField runat="server"
							ID="uxEditProductionPoleTo"
							FieldLabel="Pole/MP To"
							AllowBlank="false" />
						<ext:TextField runat="server"
							ID="uxEditProductionAcresPerMile"
							FieldLabel="Acres/Mile"
							AllowBlank="false" />
						<ext:TextField runat="server"
							ID="uxEditProductionGallons"
							FieldLabel="Gallons"
							AllowBlank="false" />
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxEditProductionSubmit"
							Text="Submit"
							Icon="ApplicationGo"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deEditProduction">
									<ExtraParams>
										<ext:Parameter Name="ProductionId" Value="#{uxCurrentProductionGrid}.getSelectionModel().getSelection()[0].data.PRODUCTION_ID" Mode="Raw" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxEditProductionCancel"
							Text="Cancel"
							Icon="ApplicationStop">
							<Listeners>
								<Click Handler="#{uxEditProductionForm}.reset();
									#{uxEditProductionWindow}.hide();" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxEditProductionSubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>
			</Items>
		</ext:Window>
	</form>
</body>
</html>

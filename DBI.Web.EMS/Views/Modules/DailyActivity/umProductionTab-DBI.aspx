﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umProductionTab-DBI.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umProductionTab_DBI" %>

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
					<ext:Column runat="server"
						DataIndex="PROJECT_ID"
						Text="Project Id"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="LONG_NAME"
						Text="Project Name"
						Flex="2" />
					<ext:Column runat="server"
						DataIndex="DESCRIPTION"
						Text="Task Name"
						Flex="1" />
					<ext:DateColumn runat="server"
						DataIndex="TIME_IN"
						Format="M/d/yyyy h:mm tt"
						Text="Time In"
						Flex="1" />
					<ext:DateColumn runat="server"
						DataIndex="TIME_OUT"
						Format="M/d/yyyy h:mm tt"
						Text="Time Out"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="WORK_AREA"
						Text="Spray/Work Area"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="POLE_FROM"
						Text="Pole/MP From"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="POLE_TO"
						Text="Pole/MP To"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="ACRES_MILE"
						Text="Acres/Mile"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="QUANTITY"
						Text="Gallons"
						Flex="1" />
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
						<ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" />
						<ext:Button runat="server"
							ID="uxEditProductionButton"
							Text="Edit Production"
							Icon="ApplicationEdit"
							Disabled="true">
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
						<ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
						<ext:Button runat="server"
							ID="uxRemoveProductionButton"
							Text="Remove Production"
							Icon="ApplicationDelete"
							Disabled="true">
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
			<SelectionModel>
				<ext:RowSelectionModel runat="server" AllowDeselect="true" Mode="Single" />
			</SelectionModel>
			<Listeners>
				<Select Handler="#{uxEditProductionButton}.enable();
					#{uxRemoveProductionButton}.enable()" />
				<Deselect Handler="#{uxEditProductionButton}.disable();
					#{uxRemoveProductionButton}.disable()" />
			</Listeners>
		</ext:GridPanel>
		<%-- Hidden Windows --%>
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
							QueryMode="Local"
							TypeAhead="true"
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
							Icon="Add"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deAddProduction" />
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxAddProductionCancel"
							Text="Cancel"
							Icon="Delete">
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
			<Listeners>
				<Show Handler="#{uxAddProductionTask}.focus()" />
			</Listeners>
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
							QueryMode="Local"
							TypeAhead="true"
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
							Icon="Add"
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
							Icon="Delete">
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
			<Listeners>
				<Show Handler="#{uxEditProductionTask}.focus()" />
			</Listeners>
		</ext:Window>
	</form>
</body>
</html>

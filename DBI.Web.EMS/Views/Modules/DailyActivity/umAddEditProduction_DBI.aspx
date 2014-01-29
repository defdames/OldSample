<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddEditProduction_DBI.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umAddEditProduction_DBI" %>

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
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
	<form id="form1" runat="server">
		<ext:Panel runat="server" ID="uxAddEditPanel" Layout="FitLayout">
			<Items>
				<ext:FormPanel runat="server"
					ID="uxAddProductionForm"
					Layout="FormLayout"
					Hidden="true">
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
										<ext:Model ID="Model1" runat="server">
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
									parentAutoLoadControl.hide();" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxAddProductionSubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>
				<ext:FormPanel runat="server"
					ID="uxEditProductionForm"
					Layout="FormLayout"
					Hidden="true">
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
										<ext:Model ID="Model2" runat="server">
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
								<Click OnEvent="deEditProduction" />
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxEditProductionCancel"
							Text="Cancel"
							Icon="Delete">
							<Listeners>
								<Click Handler="#{uxEditProductionForm}.reset();
									parentAutoLoadControl.hide();" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxEditProductionSubmit}.setDisabled(!valid);" />
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

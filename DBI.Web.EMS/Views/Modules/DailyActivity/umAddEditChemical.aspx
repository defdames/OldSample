<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddEditChemical.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umAddEditChemical" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<script type="text/javascript">
		var updateAddTotalAndUsed = function () {
			App.uxAddChemicalGallonTotal.setValue(parseInt(App.uxAddChemicalGallonStart.value) + parseInt(App.uxAddChemicalGallonMixed.value));
			App.uxAddChemicalGallonUsed.setValue(parseInt(App.uxAddChemicalGallonTotal.value) - parseInt(App.uxAddChemicalGallonRemain.value));
			App.uxAddChemicalAcresSprayed.setValue(parseInt(App.uxAddChemicalGallonUsed.value) / parseInt(App.uxAddChemicalGallonAcre.value));
		};

		var updateEditTotalAndUsed = function () {
			App.uxEditChemicalGallonTotal.setValue(parseInt(App.uxEditChemicalGallonStart.value) + parseInt(App.uxEditChemicalGallonMixed.value));
			App.uxEditChemicalGallonUsed.setValue(parseInt(App.uxEditChemicalGallonTotal.value) - parseInt(App.uxEditChemicalGallonRemain.value));
			App.uxEditChemicalAcresSprayed.setValue(parseInt(App.uxEditChemicalGallonUsed.value) / parseInt(App.uxEditChemicalGallonAcre.value));
		};

	</script>
</head>
<body>
	<form id="form1" runat="server">
		<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
		<ext:Panel runat="server" ID="uxAddEditPanel" Layout="FitLayout">
			<Items>
				<ext:FormPanel runat="server"
					ID="uxAddChemicalForm"
					Layout="FormLayout"
					Hidden="true">
					<Items>
						<ext:TextField runat="server"
							ID="uxAddChemicalTargetAre"
							FieldLabel="Target"
							AllowBlank="false" />
						<ext:NumberField runat="server"
							ID="uxAddChemicalGallonAcre"
							FieldLabel="Gallons / Acre"
							AllowBlank="false" />
						<ext:NumberField runat="server"
							ID="uxAddChemicalGallonStart"
							FieldLabel="Gallons Start"
							AllowBlank="false">
							<Listeners>
								<Change Fn="updateAddTotalAndUsed" />
							</Listeners>
						</ext:NumberField>
						<ext:NumberField runat="server"
							ID="uxAddChemicalGallonMixed"
							FieldLabel="Gallons Mixed"
							AllowBlank="false">
							<Listeners>
								<Change Fn="updateAddTotalAndUsed" />
							</Listeners>
						</ext:NumberField>
						<ext:NumberField runat="server"
							ID="uxAddChemicalGallonTotal"
							FieldLabel="Gallons Total" Disabled="true" />
						<ext:NumberField runat="server"
							ID="uxAddChemicalGallonRemain"
							FieldLabel="Gallons Remaining"
							AllowBlank="false">
							<Listeners>
								<Change Fn="updateAddTotalAndUsed" />
							</Listeners>
						</ext:NumberField>
						<ext:NumberField runat="server"
							ID="uxAddChemicalGallonUsed"
							FieldLabel="Gallons Used" Disabled="true" />
						<ext:NumberField runat="server"
							ID="uxAddChemicalAcresSprayed"
							FieldLabel="Acres Sprayed" Disabled="true" />
						<ext:ComboBox runat="server"
							ID="uxAddChemicalState"
							FieldLabel="State"
							DisplayField="name"
							ValueField="name"
							QueryMode="Local"
							TypeAhead="true"
							AllowBlank="false"
							ForceSelection="true">
							<Store>
								<ext:Store ID="uxAddStateList" runat="server" AutoDataBind="true">
									<Model>
										<ext:Model ID="Model1" runat="server">
											<Fields>
												<ext:ModelField Name="abbr" />
												<ext:ModelField Name="name" />
											</Fields>
										</ext:Model>
									</Model>
									<Reader>
										<ext:ArrayReader />
									</Reader>
								</ext:Store>
							</Store>
						</ext:ComboBox>
						<ext:TextField runat="server"
							ID="uxAddChemicalCounty"
							FieldLabel="County"
							AllowBlank="false" />
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxAddChemicalSubmit"
							Icon="Add"
							Text="Submit"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deAddChemical">
									<EventMask ShowMask="true" />
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxAddChemicalCancel"
							Icon="Delete"
							Text="Cancel">
							<Listeners>
								<Click Handler="#{uxAddChemicalForm}.reset();
									parentAutoLoadControl.hide();" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxAddChemicalSubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>
				<ext:FormPanel runat="server"
					ID="uxEditChemicalForm"
					Layout="FormLayout" Hidden="true">
					<Items>
						<ext:TextField runat="server"
							ID="uxEditChemicalTargetAre"
							FieldLabel="Target"
							AllowBlank="false" />
						<ext:NumberField runat="server"
							ID="uxEditChemicalGallonAcre"
							FieldLabel="Gallons / Acre"
							AllowBlank="false">
							<Listeners>
								<Change Fn="updateEditTotalAndUsed" />
							</Listeners>
						</ext:NumberField>
						<ext:NumberField runat="server"
							ID="uxEditChemicalGallonStart"
							FieldLabel="Gallons Start"
							AllowBlank="false">
							<Listeners>
								<Change Fn="updateEditTotalAndUsed" />
							</Listeners>
						</ext:NumberField>
						<ext:NumberField runat="server"
							ID="uxEditChemicalGallonMixed"
							FieldLabel="Gallons Mixed"
							AllowBlank="false">
							<Listeners>
								<Change Fn="updateEditTotalAndUsed" />
							</Listeners>
						</ext:NumberField>
						<ext:NumberField runat="server"
							ID="uxEditChemicalGallonTotal"
							FieldLabel="Gallons Total" Disabled="true" />
						<ext:NumberField runat="server"
							ID="uxEditChemicalGallonRemain"
							FieldLabel="Gallons Remaining"
							AllowBlank="false">
							<Listeners>
								<Change Fn="updateEditTotalAndUsed" />
							</Listeners>
						</ext:NumberField>
						<ext:NumberField runat="server"
							ID="uxEditChemicalGallonUsed"
							FieldLabel="Gallons Used" Disabled="true" />
						<ext:NumberField runat="server"
							ID="uxEditChemicalAcresSprayed"
							FieldLabel="Acres Sprayed" Disabled="true" />
						<ext:ComboBox runat="server"
							ID="uxEditChemicalState"
							FieldLabel="State"
							DisplayField="name"
							ValueField="name"
							QueryMode="Local"
							TypeAhead="true"
							AllowBlank="false"
							ForceSelection="true">
							<Store>
								<ext:Store ID="uxEditStateList" runat="server" AutoDataBind="true">
									<Model>
										<ext:Model ID="Model2" runat="server">
											<Fields>
												<ext:ModelField Name="abbr" />
												<ext:ModelField Name="name" />
											</Fields>
										</ext:Model>
									</Model>
									<Reader>
										<ext:ArrayReader />
									</Reader>
								</ext:Store>
							</Store>
						</ext:ComboBox>
						<ext:TextField runat="server"
							ID="uxEditChemicalCounty"
							FieldLabel="County"
							AllowBlank="false" />
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxEditChemicalSubmit"
							Icon="Add"
							Text="Submit"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deEditChemical">
									<EventMask ShowMask="true" />
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxEditChemicalCancel"
							Icon="Delete"
							Text="Cancel">
							<Listeners>
								<Click Handler="#{uxEditChemicalForm}.reset();
							parentAutoLoadControl.hide();" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxEditChemicalSubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>
			</Items>
			<Listeners>
				<AfterRender
					Handler="var win = parentAutoLoadControl.target || parentAutoLoadControl, //you can use just 'parentAutoLoadControl' after update to Ext.NET v2 beta.
									size = this.getSize();
 
								size.height += 34;
								size.width += 12;
								win.setSize(size);"
					Delay="100" />
			</Listeners>
		</ext:Panel>
	</form>
</body>
</html>

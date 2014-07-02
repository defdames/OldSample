<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddEditChemical.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umAddEditChemical" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<script type="text/javascript">
		var updateAddTotalAndUsed = function () {
			App.uxAddChemicalGallonTotal.setValue(parseFloat(App.uxAddChemicalGallonStart.value) + parseFloat(App.uxAddChemicalGallonMixed.value));
			App.uxAddChemicalGallonUsed.setValue(parseFloat(App.uxAddChemicalGallonTotal.value) - parseFloat(App.uxAddChemicalGallonRemain.value));
			App.uxAddChemicalAcresSprayed.setValue(parseFloat(App.uxAddChemicalGallonUsed.value) / parseFloat(App.uxAddChemicalGallonAcre.value));
		};
	</script>
</head>
<body>
	<form id="form1" runat="server">
		<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
		<ext:FormPanel runat="server"
			ID="uxAddChemicalForm"
			Width="600" DefaultButton="uxAddChemicalSubmit">
			<Items>
                <ext:Hidden runat="server" ID="uxFormType" />
				<ext:TextField runat="server"
					ID="uxAddChemicalTargetAre"
					FieldLabel="Target"
					AllowBlank="false" Width="500" />
				<ext:NumberField runat="server"
					ID="uxAddChemicalGallonAcre"
					FieldLabel="Gallons / Acre"
					AllowBlank="false" Width="500" />
				<ext:NumberField runat="server"
					ID="uxAddChemicalGallonStart"
					FieldLabel="Gallons Start"
					AllowBlank="false" Width="500">
					<Listeners>
						<Change Fn="updateAddTotalAndUsed" />
					</Listeners>
				</ext:NumberField>
				<ext:NumberField runat="server"
					ID="uxAddChemicalGallonMixed"
					FieldLabel="Gallons Mixed"
					AllowBlank="false" Width="500">
					<Listeners>
						<Change Fn="updateAddTotalAndUsed" />
					</Listeners>
				</ext:NumberField>
				<ext:NumberField runat="server"
					ID="uxAddChemicalGallonTotal"
					FieldLabel="Gallons Total" Disabled="true" Width="500" />
				<ext:NumberField runat="server"
					ID="uxAddChemicalGallonRemain"
					FieldLabel="Gallons Remaining"
					AllowBlank="false" Width="500">
					<Listeners>
						<Change Fn="updateAddTotalAndUsed" />
					</Listeners>
				</ext:NumberField>
				<ext:NumberField runat="server"
					ID="uxAddChemicalGallonUsed"
					FieldLabel="Gallons Used" Disabled="true" Width="500" />
				<ext:NumberField runat="server"
					ID="uxAddChemicalAcresSprayed"
					FieldLabel="Acres Sprayed" Disabled="true" Width="500" />
				<ext:ComboBox runat="server"
					ID="uxAddChemicalState"
					FieldLabel="State"
					DisplayField="name"
					ValueField="name"
					QueryMode="Local"
					TypeAhead="true"
					AllowBlank="false"
					ForceSelection="true"
						Width="500">
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
					AllowBlank="false" Width="500" />
			</Items>
			<Buttons>
				<ext:Button runat="server"
					ID="uxAddChemicalSubmit"
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
					ID="uxAddChemicalCancel"
					Icon="Delete"
					Text="Cancel">
					<Listeners>
						<Click Handler="parentAutoLoadControl.close();" />
					</Listeners>
				</ext:Button>
			</Buttons>
			<Listeners>
				<ValidityChange Handler="#{uxAddChemicalSubmit}.setDisabled(!valid);" />
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

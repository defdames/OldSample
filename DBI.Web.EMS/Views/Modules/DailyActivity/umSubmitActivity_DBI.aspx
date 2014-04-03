<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSubmitActivity_DBI.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umSubmitActivity_DBI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<link href="../../../Resources/StyleSheets/main.css" rel="stylesheet" />
</head>
<body>
	<form id="form1" runat="server">
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
		<ext:FormPanel runat="server"
			ID="uxSubmitActivityForm"
			Layout="FormLayout"
			Padding="5"
			MaxWidth="1000">
			<Items>
				<ext:TextField runat="server"
					ID="uxSubmitReasonForNoWork"
					AllowBlank="true"
					FieldLabel="Reason for no work" />
				<ext:TextField runat="server"
					ID="uxSubmitHotel"
					AllowBlank="true"
					FieldLabel="Hotel" />
				<ext:TextField runat="server"
					ID="uxSubmitCity"
					AllowBlank="true"
					FieldLabel="City" />
				<ext:ComboBox runat="server"
					ID="uxSubmitState"
					FieldLabel="State"
					DisplayField="name"
					ValueField="name"
					QueryMode="Local"
					TypeAhead="true"
					AllowBlank="false"
					ForceSelection="true">
					<Store>
						<ext:Store ID="uxStateList" runat="server" AutoDataBind="true">
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
					ID="uxSubmitPhone"
					FieldLabel="Phone #" />
				<ext:FileUploadField runat="server"
					ID="uxSubmitSignature"
					FieldLabel="Foreman Signature" />
				<ext:Image runat="server"
					ID="uxForemanSignatureImage"
					Width="240"
					Height="160"
					Hidden="true" />
				<ext:TextField runat="server"
					ID="uxContractRepresentative"
					FieldLabel="Contarct Representative Name" />
				<ext:FileUploadField runat="server"
					ID="uxSubmitContract"
					FieldLabel="Contract Representative" />
				<ext:Image runat="server"
					ID="uxContractRepresentativeImage"
					Width="240"
					Height="160"
					Hidden="true" />
			</Items>
			<Buttons>
				<ext:Button runat="server"
					ID="uxSaveOnlyButton"
					Text="Submit"
					Icon="Add">
					<DirectEvents>
						<Click OnEvent="deStoreFooter" />
					</DirectEvents>
				</ext:Button>
				<ext:Button runat="server"
					ID="uxCancelButton"
					Text="Cancel"
					Icon="Delete">
					<Listeners>
						<Click Handler="#{uxSubmitActivityForm}.reset();" />
					</Listeners>
				</ext:Button>
			</Buttons>
		</ext:FormPanel>
	</form>
</body>
</html>

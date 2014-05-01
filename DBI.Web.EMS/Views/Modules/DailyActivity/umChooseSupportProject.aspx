<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umChooseSupportProject.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umChooseSupportProject" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />

		<ext:FormPanel runat="server" ID="uxChooseSupportProject" Layout="FormLayout" Region="Center" BodyPadding="5">
			<Items>
				<ext:ComboBox runat="server" ID="uxSupportProject" QueryMode="Local" TypeAhead="true" AllowBlank="false"
					ForceSelection="true" ValueField="PROJECT_ID" DisplayField="LONG_NAME" FieldLabel="Support Project">
					<Store>
						<ext:Store runat="server" ID="uxSupportProjectStore" AutoDataBind="true" OnReadData="deReadSupportProjects">
							<Model>
								<ext:Model runat="server">
									<Fields>
										<ext:ModelField Name="PROJECT_ID" />
										<ext:ModelField Name="LONG_NAME" />
									</Fields>
								</ext:Model>
							</Model>
							<Proxy>
								<ext:PageProxy />
							</Proxy>
						</ext:Store>
					</Store>
				</ext:ComboBox>
			</Items>
			<Buttons>
				<ext:Button ID="uxSupportProjectButton" runat="server" Text="Submit" Disabled="true">
					<DirectEvents>
						<Click OnEvent="deSupportProjectChoice">
							<EventMask ShowMask="true" />
						</Click>
					</DirectEvents>
				</ext:Button>
				<ext:Button runat="server" ID="uxSupportProjectCancel" Text="Cancel">
					<Listeners>
						<Click Handler="#{uxChooseSupportProject}.reset(); parentAutoLoadControl.hide()" />
					</Listeners>
				</ext:Button>
			</Buttons>
			<Listeners>
				<AfterRender
					Handler="var win = parentAutoLoadControl.target || parentAutoLoadControl, //you can use just 'parentAutoLoadControl' after update to Ext.NET v2 beta.
									size = this.getSize();
 
								size.height += 250;
								size.width += 12;
								win.setSize(size);"
					Delay="100" />
				<ValidityChange Handler="#{uxSupportProjectButton}.setDisabled(!valid)" />
			</Listeners>
		</ext:FormPanel>
	</form>
</body>
</html>

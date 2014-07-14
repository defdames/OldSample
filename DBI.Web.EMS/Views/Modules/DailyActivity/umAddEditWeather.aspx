<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddEditWeather.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umAddEditWeather" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
				<ext:FormPanel runat="server"
					ID="uxAddWeatherForm"
					DefaultButton="uxAddWeatherSubmit"
					Border="false" Width="600">
					<Items>
                        <ext:Hidden runat="server" ID="uxFormType" />
						<ext:FieldContainer runat="server"
							FieldLabel="Date and Time" ID="ctl69" Width="500">
							<Items>
								<ext:DateField runat="server"
									ID="uxAddWeatherDate"
									AllowBlank="false" />
								<ext:TimeField runat="server"
									ID="uxAddWeatherTime"
									Increment="30"
									SelectedTime="09:00"
									AllowBlank="false" />
							</Items>
						</ext:FieldContainer>
						<ext:NumberField runat="server"
							ID="uxAddWeatherTemp"
							FieldLabel="Temperature"
							AllowBlank="false" Width="500"
							MinValue="-50"
							MaxValue ="130"   />
						<ext:ComboBox runat="server"
							ID="uxAddWeatherWindDirection"
							FieldLabel="Wind Direction"
							DisplayField="name"
							ValueField="abbr"
							QueryMode="Local"
							TypeAhead="true"
							AllowBlank="false"
							ForceSelection="true" Width="500">
							<Store>
								<ext:Store runat="server"
									ID="uxAddWeatherWindStore">
									<Model>
										<ext:Model runat="server" ID="ctl74">
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
						<ext:NumberField runat="server"
							ID="uxAddWeatherWindVelocity"
							FieldLabel="Wind Velocity"
							AllowBlank="false" Width="500" MinValue="0" MaxValue="150" />
						<ext:NumberField runat="server"
							ID="uxAddWeatherHumidity"
							FieldLabel="Humidity"
							AllowBlank="false" Width="500" MinValue="0" MaxValue="100" />
						<ext:TextArea runat="server"
							ID="uxAddWeatherComments"
							FieldLabel="Comments"
							AllowBlank="true" Width="500" />
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxAddWeatherSubmit"
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
							ID="uxAddWeatherCancel"
							Text="Cancel"
							Icon="Delete">
							<Listeners>
								<Click Handler="parentAutoLoadControl.close();" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxAddWeatherSubmit}.setDisabled(!valid);" />
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

﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddEditWeather.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umAddEditWeather" %>

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
			Layout="FormLayout"
			Hidden="true">
			<Items>
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
					AllowBlank="false" Width="500" />
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
					AllowBlank="false" Width="500" />
				<ext:NumberField runat="server"
					ID="uxAddWeatherHumidity"
					FieldLabel="Humidity"
					AllowBlank="false" Width="500" />
				<ext:TextArea runat="server"
					ID="uxAddWeatherComments"
					FieldLabel="Comments"
					AllowBlank="true" Width="500" />
			</Items>
			<Buttons>
				<ext:Button runat="server"
					ID="uxAddWeatherSubmit"
					Text="Submit"
					Icon="Add"
					Disabled="true">
					<DirectEvents>
						<Click OnEvent="deAddWeather">
							<EventMask ShowMask="true" />
						</Click>
					</DirectEvents>
				</ext:Button>
				<ext:Button runat="server"
					ID="uxAddWeatherCancel"
					Text="Cancel"
					Icon="Delete">
					<Listeners>
						<Click Handler="#{uxAddWeatherForm}.reset();
							parentAutoLoadControl.hide();" />
					</Listeners>
				</ext:Button>
			</Buttons>
			<Listeners>
				<ValidityChange Handler="#{uxAddWeatherSubmit}.setDisabled(!valid);" />
			</Listeners>
		</ext:FormPanel>
		<ext:FormPanel runat="server"
				ID="uxEditWeatherForm"
				Layout="FormLayout"
				Hidden="true">
				<Items>
				<ext:FieldContainer runat="server"
					FieldLabel="Date and Time" ID="ctl75" Width="500">
					<Items>
						<ext:DateField runat="server"
							ID="uxEditWeatherDate"
							AllowBlank="false" />
						<ext:TimeField runat="server"
							ID="uxEditWeatherTime"
							Format="H:mm"
							Increment="30"
							SelectedTime="09:00"
							AllowBlank="false" />
					</Items>                            
				</ext:FieldContainer>
				<ext:NumberField runat="server"
					ID="uxEditWeatherTemp"
					FieldLabel="Temperature"
					AllowBlank="false" Width="500" />
				<ext:ComboBox runat="server"
					ID="uxEditWeatherWindDirection"
					FieldLabel="Wind Direction"
					DisplayField="name"
					ValueField="abbr"
					QueryMode="Local"
					TypeAhead="true"
					AllowBlank="false"
					ForceSelection="true" Width="500">
					<Store>
						<ext:Store runat="server"
							ID="uxEditWeatherWindStore">
							<Model>
								<ext:Model runat="server" ID="ctl80">
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
					ID="uxEditWeatherWindVelocity"
					FieldLabel="Wind Velocity"
					AllowBlank="false" Width="500" />
				<ext:NumberField runat="server"
					ID="uxEditWeatherHumidity"
					FieldLabel="Humidity"
					AllowBlank="false" Width="500" />
				<ext:TextArea runat="server"
					ID="uxEditWeatherComments"
					FieldLabel="Comments"
					AllowBlank="true" Width="500" />
			</Items>
				<Buttons>
					<ext:Button runat="server"
						ID="uxEditWeatherSubmit"
						Text="Submit"
						Icon="Add"
						Disabled="true">
						<DirectEvents>
							<Click OnEvent="deEditWeather">
								<EventMask ShowMask="true" />
							</Click>
						</DirectEvents>
					</ext:Button>
					<ext:Button runat="server"
						ID="uxEditWeatherCancel"
						Text="Cancel"
						Icon="Delete">
						<Listeners>
							<Click Handler="#{uxEditWeatherForm}.reset();
								parentAutoLoadControl.hide();" />
						</Listeners>
					</ext:Button>
				</Buttons>
				<Listeners>
				<ValidityChange Handler="#{uxEditWeatherSubmit}.setDisabled(!valid);" />
			</Listeners>
			</ext:FormPanel>            
	</form>
</body>
</html>

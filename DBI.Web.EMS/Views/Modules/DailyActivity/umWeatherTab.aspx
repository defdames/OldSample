<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umWeatherTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umWeatherTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
	<form id="form1" runat="server">
		<ext:GridPanel runat="server"
			ID="uxCurrentWeatherGrid"
			Layout="HBoxLayout">
			<Store>
				<ext:Store runat="server"
					ID="uxCurrentWeatherStore"
					AutoDataBind="true">
					<Model>
						<ext:Model runat="server">
							<Fields>
								<ext:ModelField Name="WEATHER_ID" />
								<ext:ModelField Name="HEADER_ID" />
								<ext:ModelField Name="WEATHER_DATE_TIME" Type="Date" />
								<ext:ModelField Name="TEMP" />
								<ext:ModelField Name="WIND_DIRECTION" />
								<ext:ModelField Name="WIND_VELOCITY" />
								<ext:ModelField Name="HUMIDITY" />
								<ext:ModelField Name="COMMENTS" />
							</Fields>
						</ext:Model>
					</Model>
				</ext:Store>
			</Store>
			<ColumnModel>
				<Columns>
					<ext:DateColumn runat="server"
						Text="Date" Format="M/d/yyyy h:mm tt" DataIndex="WEATHER_DATE_TIME" />
					<ext:Column runat="server"
						Text="Temperature" DataIndex="TEMP" />
					<ext:Column runat="server"
						Text="Wind Direction" DataIndex="WIND_DIRECTION" />
					<ext:Column runat="server"
						Text="Wind Velocity" DataIndex="WIND_VELOCITY" />
					<ext:Column runat="server"
						Text="Humidity" DataIndex="HUMIDITY" />
					<ext:Column runat="server"
						Text="Comments" DataIndex="COMMENTS" />
				</Columns>
			</ColumnModel>
			<TopBar>
				<ext:Toolbar runat="server">
					<Items>
						<ext:Button runat="server"
							ID="uxAddWeatherButton"
							Icon="ApplicationAdd"
							Text="Add Weather">
							<Listeners>
								<Click Handler="#{uxAddWeatherWindow}.show()" />
							</Listeners>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" />
						<ext:Button runat="server"
							ID="uxEditWeatherButton"
							Icon="ApplicationEdit"
							Text="Edit Weather"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deEditWeatherForm">
									<ExtraParams>
										<ext:Parameter Name="WeatherInfo" Value="Ext.encode(#{uxCurrentWeatherGrid}.getRowsValues({selectedOnly : true}))" Mode="Raw" />
									</ExtraParams>
								</Click>
							</DirectEvents>
							<Listeners>
								<Click Handler="#{uxEditWeatherWindow}.show()" />
							</Listeners>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
						<ext:Button runat="server"
							ID="uxRemoveWeatherButton"
							Icon="ApplicationDelete"
							Text="Remove Weather">
							<DirectEvents>
								<Click OnEvent="deRemoveWeather">
									<Confirmation ConfirmRequest="true" Title="Remove?" Message="Do you really want to remove the weather?" />
									<ExtraParams>
										<ext:Parameter Name="WeatherId" Value="#{uxCurrentWeatherGrid}.getSelectionModel().getSelection()[0].data.WEATHER_ID" Mode="Raw" />
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
				<Select Handler="#{uxEditWeatherButton}.enable()" />
				<Deselect Handler="#{uxEditWeatherButton}.disable()" />
			</Listeners>
		</ext:GridPanel>

		<%-- Hidden Windows --%>
		<ext:Window runat="server"
			ID="uxAddWeatherWindow"
			Layout="FormLayout"
			Hidden="true"
			Width="650">
			<Items>
				<ext:FormPanel runat="server"
					ID="uxAddWeatherForm"
					Layout="FormLayout">
					<Items>
						<ext:FieldContainer runat="server"
							FieldLabel="Date and Time" ID="ctl69">
							<Items>
								<ext:DateField runat="server"
									ID="uxAddWeatherDate"
									AllowBlank="false" />
								<ext:TimeField runat="server"
									ID="uxAddWeatherTime"
									Format="H:mm"
									Increment="30"
									SelectedTime="09:00"
									AllowBlank="false" />
							</Items>                            
						</ext:FieldContainer>
						<ext:TextField runat="server"
							ID="uxAddWeatherTemp"
							FieldLabel="Temperature"
							AllowBlank="false" />
						<ext:ComboBox runat="server"
							ID="uxAddWeatherWindDirection"
							FieldLabel="Wind Direction"
							DisplayField="name"
							ValueField="abbr"
							QueryMode="Local"
							TypeAhead="true"
							AllowBlank="false">
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
						<ext:TextField runat="server"
							ID="uxAddWeatherWindVelocity"
							FieldLabel="Wind Velocity"
							AllowBlank="false" />
						<ext:TextField runat="server"
							ID="uxAddWeatherHumidity"
							FieldLabel="Humidity"
							AllowBlank="false" />
						<ext:TextArea runat="server"
							ID="uxAddWeatherComments"
							FieldLabel="Comments"
							AllowBlank="true" />
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxAddWeatherSubmit"
							Text="Submit"
							Icon="Add"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deAddWeather" />
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxAddWeatherCancel"
							Text="Cancel"
							Icon="Delete">
							<Listeners>
								<Click Handler="#{uxAddWeatherForm}.reset();
									#{uxAddWeatherWindow}.hide()" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxAddWeatherSubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>
			</Items>
			<Listeners>
				<Show Handler="#{uxAddWeatherDate}.focus()" />
			</Listeners>
		</ext:Window>
		<ext:Window runat="server"
			ID="uxEditWeatherWindow"
			Layout="FormLayout"
			Hidden="true"
			Width="650">
			 <Items>
				 <ext:FormPanel runat="server"
					 ID="uxEditWeatherForm"
					 Layout="FormLayout">
					 <Items>
						<ext:FieldContainer runat="server"
							FieldLabel="Date and Time" ID="ctl75">
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
						<ext:TextField runat="server"
							ID="uxEditWeatherTemp"
							FieldLabel="Temperature"
							AllowBlank="false" />
						<ext:ComboBox runat="server"
							ID="uxEditWeatherWindDirection"
							FieldLabel="Wind Direction"
							DisplayField="name"
							ValueField="abbr"
							QueryMode="Local"
							TypeAhead="true"
							AllowBlank="false">
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
						<ext:TextField runat="server"
							ID="uxEditWeatherWindVelocity"
							FieldLabel="Wind Velocity"
							AllowBlank="false" />
						<ext:TextField runat="server"
							ID="uxEditWeatherHumidity"
							FieldLabel="Humidity"
							AllowBlank="false" />
						<ext:TextArea runat="server"
							ID="uxEditWeatherComments"
							FieldLabel="Comments"
							AllowBlank="true" />
					</Items>
					 <Buttons>
						 <ext:Button runat="server"
							 ID="uxEditWeatherSubmit"
							 Text="Submit"
							 Icon="Add"
							 Disabled="true">
							 <DirectEvents>
								 <Click OnEvent="deEditWeather">
									 <ExtraParams>
										 <ext:Parameter Name="WeatherId" Value="#{uxCurrentWeatherGrid}.getSelectionModel().getSelection()[0].data.WEATHER_ID" Mode="Raw" />
									 </ExtraParams>
								 </Click>
							 </DirectEvents>
						 </ext:Button>
						 <ext:Button runat="server"
							 ID="uxEditWeatherCancel"
							 Text="Cancel"
							 Icon="Delete">
							 <Listeners>
								 <Click Handler="#{uxEditWeatherForm}.reset();
									 #{uxEditWeatherWindow}.hide()" />
							 </Listeners>
						 </ext:Button>
					 </Buttons>
					 <Listeners>
						<ValidityChange Handler="#{uxEditWeatherSubmit}.setDisabled(!valid);" />
					</Listeners>
				 </ext:FormPanel>            
			</Items>
			<Listeners>
				<Show Handler="#{uxEditWeatherDate}.focus()" />
			</Listeners>
		</ext:Window>
	</form>
</body>
</html>

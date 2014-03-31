<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umWeatherTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umWeatherTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<script type="text/javascript">
		var onShow = function (toolTip, grid) {
			var view = grid.getView(),
				store = grid.getStore(),
				record = view.getRecord(view.findItemByChild(toolTip.triggerElement)),
				column = view.getHeaderByCell(toolTip.triggerElement),
				data = record.get(column.dataIndex);

			toolTip.update(data);
		};
	</script>
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
						Text="Date/Time" Format="M/d/yyyy h:mm tt" DataIndex="WEATHER_DATE_TIME" />
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
			<View>
				<ext:GridView runat="server" StripeRows="true" TrackOver="true" />
			</View>
			<TopBar>
				<ext:Toolbar runat="server">
					<Items>
						<ext:Button runat="server"
							ID="uxAddWeatherButton"
							Icon="ApplicationAdd"
							Text="Add Weather">
							<DirectEvents>
								<Click OnEvent="deLoadWeatherWindow">
									<ExtraParams>
										<ext:Parameter Name="Type" Value="Add" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" />
						<ext:Button runat="server"
							ID="uxEditWeatherButton"
							Icon="ApplicationEdit"
							Text="Edit Weather"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deLoadWeatherWindow">
									<ExtraParams>
										<ext:Parameter Name="WeatherId" Value="#{uxCurrentWeatherGrid}.getSelectionModel().getSelection()[0].data.WEATHER_ID" Mode="Raw" />
										<ext:Parameter Name="Type" Value="Edit" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
						<ext:Button runat="server"
							ID="uxRemoveWeatherButton"
							Icon="ApplicationDelete"
							Text="Remove Weather"
							Disabled="true">
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
				<Select Handler="#{uxEditWeatherButton}.enable();
					#{uxRemoveWeatherButton}.enable()" />
				<Deselect Handler="#{uxEditWeatherButton}.disable();
					#{uxRemoveWeatherButton}.disable()" />
			</Listeners>
		</ext:GridPanel>
		<ext:ToolTip ID="ToolTip1" 
			runat="server" 
			Target="={#{uxCurrentWeatherGrid}.getView().el}"
			Delegate=".x-grid-cell"
			TrackMouse="true">
			<Listeners>
				<Show Handler="onShow(this, #{uxCurrentWeatherGrid});" /> 
			</Listeners>
		</ext:ToolTip>
	</form>
</body>
</html>

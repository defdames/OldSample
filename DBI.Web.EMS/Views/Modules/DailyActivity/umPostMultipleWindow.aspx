<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umPostMultipleWindow.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umPostMultipleWindow" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
		<ext:Viewport runat="server" Layout="FitLayout">
			<Items>
				<ext:GridPanel runat="server" ID="uxHeaderPostGrid"
					Layout="HBoxLayout">
					<Store>
						<ext:Store runat="server" ID="uxHeaderPostStore"
							AutoDataBind="true"
							OnReadData="deReadPostableData"
							PageSize="10"
							RemoteSort="true">
							<Model>
								<ext:Model runat="server">
									<Fields>
										<ext:ModelField Name="HEADER_ID" />
										<ext:ModelField Name="DA_DATE" Type="Date" />
										<ext:ModelField Name="SEGMENT1" />
										<ext:ModelField Name="LONG_NAME" />
									</Fields>
								</ext:Model>
							</Model>
							<Proxy>
								<ext:PageProxy />
							</Proxy>
						</ext:Store>
					</Store>
					<ColumnModel>
						<Columns>
							<ext:DateColumn runat="server" DataIndex="DA_DATE" Format="MM-dd-yyyy" Text="Activity Date" />
							<ext:Column runat="server" DataIndex="SEGMENT1" Text="Project" />
							<ext:Column runat="server" DataIndex="LONG_NAME" Text="Project Name" />
						</Columns>
					</ColumnModel>
					<SelectionModel>
						<ext:CheckboxSelectionModel runat="server" Mode="Multi" />
					</SelectionModel>
					<BottomBar>
						<ext:PagingToolbar runat="server" />
					</BottomBar>
					<Plugins>
						<ext:FilterHeader runat="server" Remote="true" DateFormat="MM-dd-yyyy" />
					</Plugins>
					<Buttons>
						<ext:Button runat="server" ID="uxPostMultipleButton" Text="Submit">
							<DirectEvents>
								<Click OnEvent="dePostData">
									<ExtraParams>
										<ext:Parameter Name="RowsToPost" Value="Ext.encode(#{uxHeaderPostGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
									</ExtraParams>
									<EventMask ShowMask="true" />
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server" ID="uxCancelPostButton" Text="Cancel">
							<Listeners>
								<Click Handler="parentAutoLoadControl.hide();" />
							</Listeners>
						</ext:Button>
					</Buttons>
				</ext:GridPanel>
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
		</ext:Viewport>
	</form>
</body>
</html>

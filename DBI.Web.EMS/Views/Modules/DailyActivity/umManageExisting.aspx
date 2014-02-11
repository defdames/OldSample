<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umManageExisting.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umManageExisting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<link href="../../../Resources/StyleSheets/main.css" rel="stylesheet" />
	<script type="text/javascript">
		var getRowClass = function (record, rowIndex, rowParams, store) {
			if (record.data.WARNING == "Error") {
				return "red-warning";
			}
			else if(record.data.WARNING == "Warning"){
				return "yellow-warning";
			}
		};

		var onShow = function (toolTip, grid) {
			var view = grid.getView(),
				record = view.getRecord(toolTip.triggerElement),
				data = record.data.WARNING_TYPE;

			toolTip.update(data);
		};

		var beforeShow = function (toolTip, grid) {
			var view = grid.getView(),
				record = view.getRecord(toolTip.triggerElement),
				data = record.data.WARNING_TYPE;
			if (data == "") {
				return false;
			}
			return true;
		};
	</script>
	<style type="text/css">
		.red-warning .x-grid-cell, .red-warning .x-grid-rowwrap-div {
			background-color: #FF0000 !important;
		}

		.yellow-warning .x-grid-cell, .yellow-warning .x-grid-rowwrap-div {
			background: #ffff00 !important;
		}
	</style>
</head>
<body>
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
	<form id="form1" runat="server">
		<ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout" IDMode="Explicit" IsDynamic="False" Namespace="App" RenderXType="True">
			<Items>
				<ext:GridPanel runat="server" ID="uxManageGrid" Region="North" Layout="HBoxLayout">
					<SelectionModel>
						<ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="false" Mode="Single" />
					</SelectionModel>
					<Store>
						<ext:Store runat="server" AutoDataBind="true" ID="uxManageGridStore" OnReadData="deReadHeaderData" PageSize="10" RemoteSort="true">
							<Fields>
								<ext:ModelField Name="HEADER_ID" Type="String" />
								<ext:ModelField Name="ORG_ID" Type="String" />
								<ext:ModelField Name="PROJECT_ID" Type="String" />
								<ext:ModelField Name="DA_DATE" Type="Date" />
								<ext:ModelField Name="SEGMENT1" Type="String" />
								<ext:ModelField Name="LONG_NAME" Type="String" />
								<ext:ModelField Name="STATUS_VALUE" Type="String" />
								<ext:ModelField Name="DA_HEADER_ID" Type="String" />
								<ext:ModelField Name="WARNING" Type="String" />
								<ext:ModelField Name="WARNING_TYPE" Type="String" />
							</Fields>
							<Proxy>
								<ext:PageProxy />
							</Proxy>
						</ext:Store>
					</Store>
					<ColumnModel>
						<Columns>                            
							<ext:DateColumn runat="server" Text="Activity Date" DataIndex="DA_DATE" Flex="10" Format="MM-dd-yyyy">
								<HeaderItems>
									<ext:DateField runat="server" Format="MM-dd-yyyy" />
								</HeaderItems>
							</ext:DateColumn>
							<ext:Column ID="Column1" runat="server" Text="Project" DataIndex="SEGMENT1" Flex="20"/>
							<ext:Column runat="server" Text="Project Name" DataIndex="LONG_NAME" Flex="50" />
							<ext:Column runat="server" Text="Status" DataIndex="STATUS_VALUE" Flex="30" />
							<ext:Column runat="server" Text="Oracle Header Id" DataIndex="DA_HEADER_ID" Flex="30" />
						</Columns>
					</ColumnModel>
					<View>
						<ext:GridView runat="server" StripeRows="true" TrackOver="true">
							<GetRowClass Fn="getRowClass" />
						</ext:GridView>
					</View>
					<BottomBar>
						<ext:PagingToolbar ID="uxManageGridPaging" runat="server" />
					</BottomBar>
					<Plugins>
						<ext:FilterHeader runat="server" Remote="true" DateFormat="MM-dd-yyyy" />
					</Plugins>
					<DirectEvents>
						<Select OnEvent="deUpdateUrlAndButtons">
							<ExtraParams>
								<ext:Parameter Name="HeaderId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.HEADER_ID" Mode="Raw" />
								<ext:Parameter Name="OrgId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.ORG_ID" Mode="Raw" />
							</ExtraParams>
						</Select>
					</DirectEvents>
					<ToolTips>
						<ext:ToolTip ID="uxWarningTooltip" 
							runat="server" 
							Delegate="tr.x-grid-row"
							TrackMouse="true">
							<Listeners>
								<BeforeShow Handler="return beforeShow(this, #{uxManageGrid});" />
								<Show Handler="onShow(this, #{uxManageGrid});" />
							</Listeners>
						</ext:ToolTip>
					</ToolTips>
					<TopBar>
						<ext:Toolbar runat="server">
							<Items>
								<ext:Button runat="server"
									ID="uxCreateActivityButton"
									Text="Create Activity"
									Icon="ApplicationAdd">
									<DirectEvents>
										<Click OnEvent="deLoadCreateActivity" />
									</DirectEvents>
								</ext:Button>
								<ext:ToolbarSpacer runat="server" />
								<ext:Button runat="server"
									ID="uxSubmitActivityButton"
									Text="Submit for Approval"
									Icon="ApplicationGo"
									Disabled="true">
									<DirectEvents>
										<Click OnEvent="deSubmitActivity">
											<ExtraParams>
												<ext:Parameter Name="HeaderId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.HEADER_ID" Mode="Raw" />
											</ExtraParams>
										</Click>
									</DirectEvents>    
								</ext:Button>
								<ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
								<ext:Button runat="server"
									ID="uxInactiveActivityButton"
									Text="Set Inactive"
									Icon="ApplicationStop"
									Disabled="true">
									<DirectEvents>
										<Click OnEvent="deSetHeaderInactive">
											<ExtraParams>
												<ext:Parameter Name="HeaderId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.HEADER_ID" Mode="Raw" />
											</ExtraParams>
										</Click>
									</DirectEvents>
								</ext:Button>
								<ext:ToolbarSpacer runat="server" />
								<ext:Button runat="server"
									ID="uxApproveActivityButton"
									Text="Approve"
									Icon="ApplicationPut"
									Disabled="true">
									<DirectEvents>
										<Click OnEvent="deApproveActivity">
											<ExtraParams>
												<ext:Parameter Name="HeaderId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.HEADER_ID" Mode="Raw" />
											</ExtraParams>
										</Click>
									</DirectEvents>
								</ext:Button>
								<ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" />
								<ext:Button runat="server"
									ID="uxPostActivityButton"
									Text="Post to Oracle"
									Icon="ApplicationGet"
									Disabled="true">
									<DirectEvents>
										<Click OnEvent="dePostToOracle">
											<ExtraParams>
												<ext:Parameter Name="HeaderId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.HEADER_ID" Mode="Raw" />
											</ExtraParams>
										</Click>
									</DirectEvents>
								</ext:Button>
								<ext:Button runat="server"
									ID="uxExportToPDF"
									Text="Export to PDF"
									Icon="PageWhiteAcrobat"
									Disabled="true">
									<DirectEvents>
										<Click OnEvent="deExportToPDF" IsUpload="true">
											<ExtraParams>
												<ext:Parameter Name="HeaderId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.HEADER_ID" Mode="Raw" />
											</ExtraParams>
										</Click>
									</DirectEvents>
								</ext:Button>
								<ext:Button runat="server"
									ID="uxEmailPdf"
									Text="Email Copy"
									Icon ="EmailAttach"
									Disabled="true">
									<DirectEvents>
										<Click OnEvent="deSendPDF" IsUpload="true">
											<ExtraParams>
												<ext:Parameter Name="HeaderId" Value="#{uxManageGrid}.getSelectionModel().getSelection()[0].data.HEADER_ID" Mode="Raw" />
											</ExtraParams>
										</Click>
									</DirectEvents>
								</ext:Button>
							</Items>
						</ext:Toolbar>
					</TopBar>
					
				</ext:GridPanel>				
				<ext:TabPanel runat="server" ID="uxTabPanel" Region="Center">
					<Items>
						<ext:Panel runat="server"
							Title="Home"
							ID="uxCombinedTab"
							Disabled="true">
							<Loader runat="server"
								ID="uxCombinedTabLoader" Mode="Frame" AutoLoad="false" ReloadOnEvent="true">
								<LoadMask ShowMask="true" />
							</Loader>
							<Listeners>
								<Activate Handler="#{uxCombinedTab}.reload()" />
							</Listeners>
						</ext:Panel>
						<ext:Panel runat="server" 
							Title="Header"
							ID="uxHeaderTab"
							Disabled="true">
							<Loader runat="server"
								ID="uxHeaderLoader" Mode="Frame" AutoLoad="false" ReloadOnEvent="true"> 
								<LoadMask ShowMask="true" />
							</Loader>
							<Listeners>
								<Activate Handler="#{uxHeaderTab}.reload()" />
							</Listeners>                                                    
						</ext:Panel>
						<ext:Panel runat="server"
							Title="Equipment"
							ID="uxEquipmentTab"
							Disabled="true">
							<Loader runat="server"
								ID="uxEquipmentLoader" Mode="Frame" AutoLoad="false" ReloadOnEvent="true">
								<LoadMask ShowMask="true"  />
							</Loader>
							<Listeners>
								<Activate Handler="#{uxEquipmentTab}.reload()" />
							</Listeners>
						</ext:Panel>
						<ext:Panel runat="server"
							Title="Employees"
							ID="uxEmployeeTab"
							Disabled="true">
							<Loader ID="uxEmployeeLoader" runat="server" Mode="Frame" AutoLoad="false" ReloadOnEvent="true">
								<LoadMask ShowMask="true" />
							</Loader>   
							<Listeners>
								<Activate Handler="#{uxEmployeeTab}.reload()" />
							</Listeners>
						</ext:Panel>
						<ext:Panel runat="server"
							Title="Weather"
							ID="uxWeatherTab"
							Disabled="true">
							<Loader ID="uxWeatherLoader" runat="server" Mode="Frame" AutoLoad="false" ReloadOnEvent="true">
								<LoadMask ShowMask="true" />
							</Loader>
							<Listeners>
								<Activate Handler="#{uxWeatherTab}.reload()" />
							</Listeners>
						</ext:Panel>
						<ext:Panel runat="server"
							Title="Chemical Mix"
							ID="uxChemicalTab"
							Disabled="true">
							<Loader ID="uxChemicalLoader" runat="server" Mode="Frame" AutoLoad="false" ReloadOnEvent="true">
								<LoadMask ShowMask="true" />
							</Loader>   
							<Listeners>
								<Activate Handler="#{uxChemicalTab}.reload()" />
							</Listeners>
						</ext:Panel>
						<ext:Panel runat="server"
							Title="Inventory"
							ID="uxInventoryTab"
							Disabled="true">
							<Loader ID="uxInventoryLoader" runat="server" Mode="Frame" AutoLoad="false" ReloadOnEvent="true">
								<LoadMask ShowMask="true" />
							</Loader>   
							<Listeners>
								<Activate Handler="#{uxInventoryTab}.reload()" />
							</Listeners>
						</ext:Panel>
						<ext:Panel runat="server"
							Title="Production"
							ID="uxProductionTab"
							Disabled="true">
							<Loader ID="uxProductionLoader" runat="server" Mode="Frame" AutoLoad="false" ReloadOnEvent="true">
								<LoadMask ShowMask="true" />
							</Loader>   
							<Listeners>
								<Activate Handler="#{uxProductionTab}.reload()" />
							</Listeners>
						</ext:Panel>                        
					</Items>
				</ext:TabPanel>
				<%-- Hidden Windows --%>
				<ext:Window runat="server"
					ID="uxPlaceholderWindow"
					Hidden="true"
					Width="650"
					Y="50">
					<Loader runat="server"
						ID="uxPlaceholderLoader"
						Mode="Frame"
						AutoLoad="false" />
				</ext:Window>
				<ext:Window runat="server"
					ID="uxCreateActivityWindow"
					Title="Create Activity"
					Hidden="true"
					Width="650"
					Shadow="true"
					Y="50">
					<Loader runat="server"
						ID="uxCreateActivityLoader"
						Url="umDailyActivity.aspx"
						Mode="Frame"
						AutoLoad="false" />
				</ext:Window>
			</Items>
		</ext:Viewport>

	</form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umViewCompleted.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umViewCompleted" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
    <script type="text/javascript">
        function printWindow() {
            var PrintWin = window.open("umViewSurvey.aspx?CompletionId=" + App.uxCompletedGrid.getSelectionModel().getSelection()[0].data.COMPLETION_ID + "&FormId=" + App.uxCompletedGrid.getSelectionModel().getSelection()[0].data.FORM_ID + "&Print=print");
            
            PrintWin.focus();
        }
    </script>
</head>
<body>
	<form id="form1" runat="server">
		<ext:ResourceManager runat="server" IsDynamic="false" />
		<ext:Viewport runat="server" Layout="FitLayout">
			<Items>
				<ext:TabPanel runat="server" ID="uxTabPanel">
					<Items>
						<ext:Panel runat="server" Title="Survey List" Layout="BorderLayout">
							<Items>
								<ext:TreePanel
									ID="uxOrgPanel"
									runat="server"
									Title="Organizations"
									BodyPadding="6"
									Region="West"
									Weight="100"
									Width="300"
									AutoScroll="true"
									RootVisible="true"
									SingleExpand="true"
									Lines="false"
									UseArrows="true">
									<Store>
										<ext:TreeStore ID="TreeStore1" runat="server" OnReadData="deLoadOrgTree">
											<Proxy>
												<ext:PageProxy></ext:PageProxy>
											</Proxy>
										</ext:TreeStore>
									</Store>
									<Root>
										<ext:Node NodeID="0" Text="All Companies" Expanded="true" />
									</Root>
									<SelectionModel>
										<ext:TreeSelectionModel ID="uxCompanySelectionModel" runat="server" Mode="Single" />
									</SelectionModel>
									<Listeners>
										<ItemClick Handler="#{uxProjectStore}.reload()" />
									</Listeners>
								</ext:TreePanel>
								<ext:GridPanel runat="server" ID="uxProjectGrid" Margin="5" Region="Center">
									<Store>
										<ext:Store runat="server" ID="uxProjectStore" AutoDataBind="true" OnReadData="deReadProjects" PageSize="25" RemoteSort="true" AutoLoad="false">
											<Model>
												<ext:Model runat="server">
													<Fields>
														<ext:ModelField Name="PROJECT_ID" />
														<ext:ModelField Name="SEGMENT1" />
														<ext:ModelField Name="LONG_NAME" />
														<ext:ModelField Name="CARRYING_OUT_ORGANIZATION_ID" />
													</Fields>
												</ext:Model>
											</Model>
											<Proxy>
												<ext:PageProxy />
											</Proxy>
											<Sorters>
												<ext:DataSorter Property="SEGMENT1" Direction="ASC" />
											</Sorters>
										</ext:Store>
									</Store>
									<ColumnModel>
										<Columns>
											<ext:Column runat="server" DataIndex="SEGMENT1" Text="Project Number" Flex="15" />
											<ext:Column runat="server" DataIndex="LONG_NAME" Text="Project Name" Flex="85" />
										</Columns>
									</ColumnModel>
									<BottomBar>
										<ext:PagingToolbar runat="server" />
									</BottomBar>
									<Plugins>
										<ext:FilterHeader runat="server" Remote="true" />
									</Plugins>
									<Listeners>
										<Select Handler="#{uxCompletedSurveyPanel}.clearContent();#{uxTabPanel}.addTab(#{uxTabPanel}.bin[0]);#{uxTabPanel}.setActiveTab(#{uxCompletedGrid});#{uxCompletedStore}.reload();" />
									</Listeners>
								</ext:GridPanel>
							</Items>
						</ext:Panel>
					</Items>
					<Bin>
						<ext:Panel runat="server" ID="uxHiddenTab" Hidden="true" Closable="true" CloseAction="Hide" Title="View Surveys" Layout="BorderLayout">
							<Items>
								<ext:GridPanel runat="server" ID="uxCompletedGrid" Height="200" Region="North">
									<Store>
										<ext:Store runat="server" ID="uxCompletedStore" AutoDataBind="true" OnReadData="deReadCompletions" PageSize="5" RemoteSort="true" AutoLoad="false">
											<Model>
												<ext:Model ID="Model1" runat="server">
													<Fields>
														<ext:ModelField Name="COMPLETION_ID" />
                                                        <ext:ModelField Name="FORM_ID" />
														<ext:ModelField Name="FORMS_NAME" />
														<ext:ModelField Name="FILLED_BY" />
														<ext:ModelField Name="FILLED_ON" />
														<ext:ModelField Name="LONG_NAME" />
													</Fields>
												</ext:Model>
											</Model>
											<Proxy>
												<ext:PageProxy />
											</Proxy>
											<Sorters>
												<ext:DataSorter Property="FILLED_ON" Direction="ASC" />
											</Sorters>
											<Parameters>
												<ext:StoreParameter Name="ProjectId" Value="#{uxProjectGrid}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
												<ext:StoreParameter Name="OrgId" Value="#{uxProjectGrid}.getSelectionModel().getSelection()[0].data.CARRYING_OUT_ORGANIZATION_ID" Mode="Raw" />
											</Parameters>
										</ext:Store>
									</Store>
									<ColumnModel>
										<Columns>
											<ext:DateColumn ID="DateColumn1" runat="server" DataIndex="FILLED_ON" Text="Date Completed" Flex="15" />
											<ext:Column ID="Column1" runat="server" DataIndex="LONG_NAME" Text="Project Name" Flex="50" />
											<ext:Column ID="Column2" runat="server" DataIndex="FORMS_NAME" Text="Form Name" Flex="25" />
											<ext:Column ID="Column3" runat="server" DataIndex="FILLED_BY" Text="Filled By" Flex="10" />
										</Columns>
									</ColumnModel>
									<BottomBar>
										<ext:PagingToolbar ID="PagingToolbar1" runat="server" />
									</BottomBar>
									<Plugins>
										<ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
									</Plugins>
									<DirectEvents>
										<Select OnEvent="deLoadForm">
											<EventMask ShowMask="true" />
											<ExtraParams>
												<ext:Parameter Name="CompletionId" Value="#{uxCompletedGrid}.getSelectionModel().getSelection()[0].data.COMPLETION_ID" Mode="Raw" />
											</ExtraParams>
										</Select>
									</DirectEvents>
								</ext:GridPanel>
								<ext:Panel runat="server" ID="uxCompletedSurveyPanel" Layout="FormLayout" Region="Center">
									<Loader ID="Loader1" runat="server" Mode="Frame">
										<LoadMask ShowMask="true" />
									</Loader>
									<TopBar>
										<ext:Toolbar runat="server" ID="uxPrintToolbar">
											<Items>
												<ext:Button runat="server" ID="uxPrintButton" Text="Print" Icon="Printer">
                                                    <Listeners>
                                                        <Click Fn="printWindow" />
                                                    </Listeners>
												</ext:Button>
											</Items>
										</ext:Toolbar>
									</TopBar>
								</ext:Panel>
							</Items>
						</ext:Panel>
					</Bin>
				</ext:TabPanel>
			</Items>
		</ext:Viewport>
	</form>
</body>
</html>

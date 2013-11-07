<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umDailyActivity.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umDailyActivity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<ext:ResourceManager ID="ResourceManager1" runat="server"  IsDynamic="False" RethrowAjaxExceptions="true">
	</ext:ResourceManager>
	<form id="form1" runat="server">
		<ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout" IDMode="Explicit" IsDynamic="False" Namespace="App" RenderXType="True">
			<Items>
				<ext:MenuPanel ID="uxMenuPanel" runat="server" Region="West">
					<Menu runat="server">
						<Items>
							<ext:MenuItem ID="uxCreate" Icon="ApplicationAdd" Text="Create Activity" />
							<ext:MenuItem ID="uxManage" Icon="ApplicationEdit" Text="Manage Existing" />
						</Items>
					</Menu>
				</ext:MenuPanel>
				<ext:Panel runat="server" Region="Center" ID="uxCenterPanel" Layout="FitLayout">
					<Items>
						<ext:FormPanel ID="uxFormPanel" runat="server" Layout="AnchorLayout" BodyPadding="5" DefaultAnchor="50%" Title="Add Activity" ButtonAlign="Left">
							<Items>
								<ext:DropDownField runat="server"
									ID="uxFormProject"
									PageSize="25"
									FieldLabel="Select a Project">
									<Component>
										<ext:GridPanel runat="server" 
											ID="uxFormProjectGrid">
											<Store>
												<ext:Store runat="server" 
													ID="uxFormProjectStore"
													OnReadData="deReadData"
													PageSize="25"
													RemoteSort="true">													
													<Model>
														<ext:Model runat="server"
															ID="uxFormProjectModel">
															<Fields>
																<ext:ModelField Name="ORGANIZATION_NAME" Type="String" />
																<ext:ModelField Name="SEGMENT1" Type="String"/>
																<ext:ModelField Name="LONG_NAME" Type="String"/>
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
													<ext:Column runat="server"
														ID="uxFormSegment"
														DataIndex="SEGMENT1"
														Text="Project Number" />
													<ext:Column runat="server"
														ID="uxFormLong"
														DataIndex="LONG_NAME"
														Text="Project Name" />
													<ext:Column runat="server"
														ID="uxFormOrg"
														DataIndex="ORGANIZATION_NAME"
														Text="Organization Name" />
												</Columns>
											</ColumnModel>
											<SelectionModel>
												<ext:RowSelectionModel ID="RowSelectionModel1" runat="server" Mode="Single" />
											</SelectionModel>
                                            <DirectEvents>
                                                <SelectionChange OnEvent="deStoreValue">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="Segment" Value="#{uxFormProjectGrid}.getSelectionModel().getSelection()[0].data.SEGMENT1" Mode="Raw" />
                                                    </ExtraParams>
                                                </SelectionChange>
                                            </DirectEvents>
											<Plugins>
												<ext:FilterHeader ID="uxFormProjectFilter" runat="server" Remote="true"  />
											</Plugins>
											<BottomBar>
												<ext:PagingToolbar ID="uxFormPaging" runat="server" />
											</BottomBar>
										</ext:GridPanel>

									</Component>
								</ext:DropDownField>
								<%--<ext:ComboBox runat="server" 
									ID="uxFormProject" 
									FieldLabel="Select a Project" 
									PageSize="25"
									TypeAhead="false"
									Width="570"
									MinChars="0"
									TriggerAction="Query"
									DisplayField="LONG_NAME"
									ValueField="SEGMENT1">
									<ListConfig  LoadingText="Searching...">
										<ItemTpl ID="ItemTpl1" runat="server">
											<Html>
												<div class="search-item">
													<h3><span>{SEGMENT1}</span> {LONG_NAME}</h3>
													{ORGANIZATION_NAME}
												</div>
											</Html>
										</ItemTpl>
									</ListConfig>
									<Store>
										<ext:Store runat="server"
											AutoDataBind="true">
											<Proxy>
												<ext:AjaxProxy Url="~/Views/Handlers/WebProjects.ashx">
													<ActionMethods Read="POST" />
													<Reader>
														<ext:JsonReader Root="Projects" TotalProperty="TOTAL" />
													</Reader>
												</ext:AjaxProxy>
											</Proxy>
											<Model>
												<ext:Model runat="server">
													<Fields>
														<ext:ModelField Name="SEGMENT1" />
														<ext:ModelField Name="LONG_NAME" />
														<ext:ModelField Name="ORGANIZATION_NAME" />
													</Fields>
												</ext:Model>
											</Model>
											<Sorters>
												<ext:DataSorter Property="ORGANIZATION_NAME" Direction="DESC" />
											</Sorters>
										</ext:Store>                                        
									</Store>
								</ext:ComboBox>--%>
								<ext:DateField runat="server" ID="uxFormDate" FieldLabel="Date" />
								<ext:TextField runat="server" ID="uxFormSubDivision" FieldLabel="Subdivision"  />
								<ext:TextField runat="server" ID="uxFormContractor" FieldLabel="Contractor"  />
								<%--<ext:ComboBox runat="server" ID="uxFormEmployee" FieldLabel="Supervisor/Area Manager">
									<Store>
										<ext:Store runat="server" DataSource="uxFormEmployeeDataSource" AutoDataBind="true">
											<Model>
												<ext:Model runat="server">
													<Fields>
														<ext:ModelField Name="name" />
													</Fields>
												</ext:Model>
											</Model>
										</ext:Store>
									</Store>
								</ext:ComboBox>--%>
								<ext:TextField runat="server" ID="uxFormLicense" FieldLabel="License #" />
								<ext:ComboBox runat="server" ID="uxFormState" FieldLabel="State" DisplayField="name" ValueField="abbr">
									<Store>
										<ext:Store runat="server" Data="<%# StateList %>" AutoDataBind="true">
											<Model>
												<ext:Model runat="server">
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
								<ext:TextField runat="server" ID="uxFormType" FieldLabel="Type" />
								<ext:ComboBox runat="server" ID="uxFormDensity" FieldLabel="Density">
									<Items>
										<ext:ListItem Text="Low" Value="low" />
										<ext:ListItem Text="Medium" Value="medium" />
										<ext:ListItem Text="High" Value="high" />
									</Items>
								</ext:ComboBox>
							</Items>
							<Buttons>
								<ext:Button runat="server" ID="uxFormSubmit" Text="Submit">
									<DirectEvents>
										<Click OnEvent="deStoreActivity" />
									</DirectEvents>    
								</ext:Button>
								<ext:Button runat="server" ID="uxFormClear" Text="Clear">
									<Listeners>
										<Click Handler="#{uxFormPanel}.reset()" />
									</Listeners>
								</ext:Button>
							</Buttons>
						</ext:FormPanel>
					</Items>
				</ext:Panel>
			</Items>
		</ext:Viewport>
	</form>
</body>
</html>

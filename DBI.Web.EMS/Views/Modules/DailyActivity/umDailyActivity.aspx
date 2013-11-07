﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umDailyActivity.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umDailyActivity" %>

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
											ID="uxFormProjectGrid"
											Layout="HBoxLayout">
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
														Text="Project Number" Flex="15" />
													<ext:Column runat="server"
														ID="uxFormLong"
														DataIndex="LONG_NAME"
														Text="Project Name" Flex="45" />
													<ext:Column runat="server"
														ID="uxFormOrg"
														DataIndex="ORGANIZATION_NAME"
														Text="Organization Name" Flex="30" />
												</Columns>
											</ColumnModel>
											<SelectionModel>
												<ext:RowSelectionModel ID="uxFormProjectSelection" runat="server" Mode="Single" />
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
												<ext:PagingToolbar ID="uxFormProjectPaging" runat="server" />
											</BottomBar>
										</ext:GridPanel>
									</Component>
								</ext:DropDownField>
								<ext:DateField runat="server" ID="uxFormDate" FieldLabel="Date" />
								<ext:TextField runat="server" ID="uxFormSubDivision" FieldLabel="Subdivision"  />
								<ext:TextField runat="server" ID="uxFormContractor" FieldLabel="Contractor"  />
								<ext:DropDownField runat="server" 
									ID="uxFormEmployee" 
									FieldLabel="Supervisor/Area Manager">
									<Component>
										<ext:GridPanel runat="server" 
											ID="uxFormEmployeeGrid">
											<Store>
												<ext:Store runat="server"
													ID="uxFormEmployeeStore"
													OnReadData="deLoadEmployees"
													PageSize="10"
													RemoteSort="true">
													<Model>
														<ext:Model ID="uxFormEmployeeModel" runat="server">
															<Fields>
																<ext:ModelField Name="PERSON_ID" Type="Int" />
																<ext:ModelField Name="EMPLOYEE_NAME" Type="String" />
																<ext:ModelField Name="JOB_NAME" Type="String" />
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
													<ext:Column runat="server" Text="Person ID" ID="uxFormPersonId" DataIndex="PERSON_ID" />
													<ext:Column runat="server" Text="Employee Name" ID="uxFormEmployeeName" DataIndex="EMPLOYEE_NAME" />
													<ext:Column runat="server" Text="Job Name" ID="uxFormJobName" DataIndex="JOB_NAME" />
												</Columns>
											</ColumnModel>
											<Plugins>
												<ext:FilterHeader runat="server"
													ID="uxFormEmployeeFilter"
													Remote="true" />
											</Plugins>
											<DirectEvents>
												<SelectionChange OnEvent="deStoreEmployee">
													<ExtraParams>
														<ext:Parameter Name="EmployeeName" Value="#{uxFormEmployeeGrid}.getSelectionModel().getSelection()[0].data.EMPLOYEE_NAME" Mode="Raw" />
													</ExtraParams>
												</SelectionChange>
											</DirectEvents>
											<SelectionModel>
												<ext:RowSelectionModel ID="uxFormEmployeeSelection" runat="server" Mode="Single" />
											</SelectionModel>
											<BottomBar>
												<ext:PagingToolbar ID="uxFormEmployeePaging" runat="server" />
											</BottomBar>
										</ext:GridPanel>
									</Component>
								</ext:DropDownField>
								<ext:TextField runat="server" ID="uxFormLicense" FieldLabel="License #" />
								<ext:ComboBox runat="server" ID="uxFormState" FieldLabel="State" DisplayField="name" ValueField="abbr">
									<Store>
										<ext:Store ID="uxStateList" runat="server" AutoDataBind="true">
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

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umChooseSupportProject.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umChooseSupportProject" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="FitLayout">
            <Items>
                <ext:FormPanel runat="server" ID="uxChooseSupportProjectFormPanel" Layout="FormLayout" Region="Center" BodyPadding="5" Height="350">
                    <Items>
                       <ext:DropDownField runat="server"
					ID="uxFormProject"
					FieldLabel="Select a Project"
					Mode="ValueText"
					AllowBlank="false">
					<Component>
						<ext:GridPanel runat="server" 
							ID="uxFormProjectGrid"
							Layout="HBoxLayout">
							<Store>
								<ext:Store runat="server" 
									ID="uxFormProjectStore"
									OnReadData="deReadData"
									PageSize="10"
									RemoteSort="true">													
									<Model>
										<ext:Model runat="server"
											ID="uxFormProjectModel">
											<Fields>
												<ext:ModelField Name="PROJECT_ID" Type="Int" />
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
										<ext:Parameter Name="ProjectId" Value="#{uxFormProjectGrid}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
										<ext:Parameter Name="LongName" Value="#{uxFormProjectGrid}.getSelectionModel().getSelection()[0].data.LONG_NAME" Mode="Raw" />
									</ExtraParams>
								</SelectionChange>
							</DirectEvents>
							<Plugins>
								<ext:FilterHeader ID="uxFormProjectFilter" runat="server" Remote="true"  />
							</Plugins>
							<TopBar>
								<ext:Toolbar runat="server"
									ID="uxFormProjectTop">
									<Items>
										<ext:Button runat="server" 
											ID="uxFormProjectToggleOrg" 
											EnableToggle="true" 
											Text="All Regions"
											Icon="Group">
											<DirectEvents>
												<Toggle OnEvent="deReloadStore">
													<ExtraParams>
														<ext:Parameter Name="Type" Value="Project" />
													</ExtraParams>
												</Toggle>
											</DirectEvents>															
										</ext:Button>
									</Items>
								</ext:Toolbar>
							</TopBar>
							<BottomBar>
								<ext:PagingToolbar ID="uxFormProjectPaging" runat="server" />
							</BottomBar>
						</ext:GridPanel>
					</Component>
				</ext:DropDownField>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" Id="uxChoosePerDiemSubmitButton" Text="Submit">
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

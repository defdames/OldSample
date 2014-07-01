<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umChooseLunchHeader.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umChooseLunchHeader" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
				<ext:FormPanel runat="server" ID="uxChooseLunchForm" Layout="FormLayout" DefaultButton="uxStoreLunchButton" Height="250">
					<Items>
                        <ext:DropDownField runat="server" ID="uxLunchDRS" Mode="ValueText" Editable="false" FieldLabel="Choose a DRS" AllowBlank="false">
                            <Component>
                                <ext:GridPanel runat="server" ID="uxLunchDRSGrid">
                                    <Store>
								        <ext:Store runat="server" ID="uxLunchHeaderStore" OnReadData="deReadLunchHeaders" AutoDataBind="true">
									        <Model>
										        <ext:Model ID="Model1" runat="server">
											        <Fields>
												        <ext:ModelField Name="ProjectName" Type="String" />
												        <ext:ModelField Name="HeaderId" Type="Int" />
                                                        <ext:ModelField Name="TaskNumber" Type="String" />
                                                        <ext:ModelField Name="TaskName" Type="String" />
                                                        <ext:ModelField Name="TaskId" Type="Int" />
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
                                            <ext:Column runat="server" DataIndex="HeaderId" Text="DRS Id" Flex="10" />
                                            <ext:Column runat="server" DataIndex="ProjectName" Text="Project" Flex="50" />
                                            <ext:Column runat="server" DataIndex="TaskNumber" Text="Task Number" Flex="15" />
                                            <ext:Column runat="server" DataIndex="TaskName" Text="Task Name" Flex="25" />
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel runat="server" Mode="Single" />
                                    </SelectionModel>
                                    <DirectEvents>
                                        <SelectionChange OnEvent="deStoreValues">
                                            <ExtraParams>
                                                <ext:Parameter Name="selectedInfo" Value="Ext.encode(#{uxLunchDRSGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                                            </ExtraParams>
                                        </SelectionChange>
                                    </DirectEvents>
                                </ext:GridPanel>
                            </Component>
                        </ext:DropDownField>
					    <ext:Hidden runat="server" ID="uxHiddenTask" />
					</Items>
					<Buttons>
						<ext:Button runat="server" Text="Save" ID="uxStoreLunchButton" Disabled="true" Icon="Add">
							<DirectEvents>
								<Click OnEvent="deStoreLunchChoice">
									<EventMask ShowMask="true" />
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server" Text="Cancel" Icon="Delete">
							<Listeners>
								<Click Handler="parentAutoLoadControl.close()" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<AfterRender
							Handler="var win = parentAutoLoadControl.target || parentAutoLoadControl, //you can use just 'parentAutoLoadControl' after update to Ext.NET v2 beta.
							size = this.getSize();
							size.height += 34;
							size.width += 12;
							win.setSize(size);"
						Delay="100" />
						<ValidityChange Handler="#{uxStoreLunchButton}.setDisabled(!valid)" />
					</Listeners>
				</ext:FormPanel>
	</form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddEditProduction_DBI.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umAddEditProduction_DBI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<script>
		var valDateTime = function () {
			var me = this,
				v = me.getValue(),
				field;

			if (me.startDateField) {
				field = Ext.getCmp(me.startDateField);
				field.setMaxValue(v);
				me.timeRangeMax = v;
			} else if (me.endDateField) {
				field = Ext.getCmp(me.endDateField);
				field.setMinValue(v);
				me.timeRangeMin = v;
			}

			field.validate();
		};
	</script>
</head>
<body>
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
	<form id="form1" runat="server">
		<ext:Panel runat="server">
			<Items>
				<ext:FormPanel runat="server"
					ID="uxAddProductionForm"
					Layout="FormLayout"
					Hidden="true" Width="600">
					<Items>
						<ext:DropDownField runat="server" Editable="false"
							ID="uxAddProductionTask"
							Mode="ValueText"
							AllowBlank="false"
							FieldLabel="Select Task" Width="500">
							<Component>
								<ext:GridPanel runat="server"
									ID="uxAddProductionTaskGrid"
									Layout="HBoxLayout">
									<Store>
										<ext:Store runat="server"
											ID="uxAddProductionTaskStore">
											<Model>
												<ext:Model ID="Model1" runat="server">
													<Fields>
														<ext:ModelField Name="TASK_ID" />
														<ext:ModelField Name="TASK_NUMBER" />
														<ext:ModelField Name="DESCRIPTION" />
													</Fields>
												</ext:Model>
											</Model>
										</ext:Store>
									</Store>
									<ColumnModel>
										<Columns>
											<ext:Column ID="Column7" runat="server" DataIndex="TASK_NUMBER" Text="Task Number" />
											<ext:Column ID="Column10" runat="server" DataIndex="DESCRIPTION" Text="Name" />
										</Columns>
									</ColumnModel>
									<DirectEvents>
										<SelectionChange OnEvent="deStoreTask">
											<ExtraParams>
												<ext:Parameter Name="TaskId" Value="#{uxAddProductionTaskGrid}.getSelectionModel().getSelection()[0].data.TASK_ID" Mode="Raw" />
												<ext:Parameter Name="Description" Value="#{uxAddProductionTaskGrid}.getSelectionModel().getSelection()[0].data.DESCRIPTION" Mode="Raw" />
												<ext:Parameter Name="Type" Value="Add" />
											</ExtraParams>
										</SelectionChange>
									</DirectEvents>
									<SelectionModel>
										<ext:RowSelectionModel ID="RowSelectionModel3" runat="server" Mode="Single" />
									</SelectionModel>
									<Plugins>
										<ext:FilterHeader ID="FilterHeader3" runat="server" Remote="true" />
									</Plugins>
									<BottomBar>
										<ext:PagingToolbar ID="PagingToolbar3" runat="server" />
									</BottomBar>
								</ext:GridPanel>
							</Component>
						</ext:DropDownField>
						<ext:TextArea runat="server"
							ID="uxAddProductionWorkArea"
							FieldLabel="Spray/Work Area"
							AllowBlank="false" Width="500" />
						<ext:TextField runat="server"
							ID="uxAddProductionPoleFrom"
							FieldLabel="Pole/MP From" Width="500" />
						<ext:TextField runat="server"
							ID="uxAddProductionPoleTo"
							FieldLabel="Pole/MP To" Width="500" />
						<ext:NumberField runat="server"
							ID="uxAddProductionAcresPerMile"
							FieldLabel="Acres/Mile"
							AllowBlank="false" Width="500" MinValue="0" />
						<ext:NumberField runat="server"
							ID="uxAddProductionGallons"
							FieldLabel="Gallons"
							AllowBlank="false" Width="500" MinValue="0" />
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxAddProductionSubmit"
							Text="Submit"
							Icon="Add"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deAddProduction">
									<EventMask ShowMask="true" />
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxAddProductionCancel"
							Text="Cancel"
							Icon="Delete">
							<Listeners>
								<Click Handler="#{uxAddProductionForm}.reset();
							parentAutoLoadControl.hide();" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxAddProductionSubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>
				<ext:FormPanel runat="server"
					ID="uxEditProductionForm"
					Layout="FormLayout"
					Hidden="true" Width="600">
					<Items>
						<ext:DropDownField runat="server" Editable="false"
							ID="uxEditProductionTask"
							Mode="ValueText"
							AllowBlank="false"
							FieldLabel="Select Task" Width="500">
							<Component>
								<ext:GridPanel runat="server"
									ID="uxEditProductionTaskGrid"
									Layout="HBoxLayout">
									<Store>
										<ext:Store runat="server"
											ID="uxEditProductionTaskStore">
											<Model>
												<ext:Model ID="Model3" runat="server">
													<Fields>
														<ext:ModelField Name="TASK_ID" />
														<ext:ModelField Name="TASK_NUMBER" />
														<ext:ModelField Name="DESCRIPTION" />
													</Fields>
												</ext:Model>
											</Model>
										</ext:Store>
									</Store>
									<ColumnModel>
										<Columns>
											<ext:Column ID="Column8" runat="server" DataIndex="TASK_NUMBER" Text="Task Number" />
											<ext:Column ID="Column9" runat="server" DataIndex="DESCRIPTION" Text="Name" />
										</Columns>
									</ColumnModel>
									<DirectEvents>
										<SelectionChange OnEvent="deStoreTask">
											<ExtraParams>
												<ext:Parameter Name="TaskId" Value="#{uxEditProductionTaskGrid}.getSelectionModel().getSelection()[0].data.TASK_ID" Mode="Raw" />
												<ext:Parameter Name="Description" Value="#{uxEditProductionTaskGrid}.getSelectionModel().getSelection()[0].data.DESCRIPTION" Mode="Raw" />
												<ext:Parameter Name="Type" Value="Edit" />
											</ExtraParams>
										</SelectionChange>
									</DirectEvents>
									<SelectionModel>
										<ext:RowSelectionModel ID="RowSelectionModel4" runat="server" Mode="Single" />
									</SelectionModel>
									<Plugins>
										<ext:FilterHeader ID="FilterHeader4" runat="server" Remote="true" />
									</Plugins>
									<BottomBar>
										<ext:PagingToolbar ID="PagingToolbar4" runat="server" />
									</BottomBar>
								</ext:GridPanel>
							</Component>
						</ext:DropDownField>
						<ext:TextArea runat="server"
							ID="uxEditProductionWorkArea"
							FieldLabel="Spray/Work Area"
							AllowBlank="false" Width="500" />
						<ext:TextField runat="server"
							ID="uxEditProductionPoleFrom"
							FieldLabel="Pole/MP From" Width="500" />
						<ext:TextField runat="server"
							ID="uxEditProductionPoleTo"
							FieldLabel="Pole/MP To" Width="500" />
						<ext:NumberField runat="server"
							ID="uxEditProductionAcresPerMile"
							FieldLabel="Acres/Mile"
							AllowBlank="false" Width="500" MinValue="0" />
						<ext:NumberField runat="server"
							ID="uxEditProductionGallons"
							FieldLabel="Gallons"
							AllowBlank="false" Width="500" MinValue="0" />
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxEditProductionSubmit"
							Text="Submit"
							Icon="Add"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deEditProduction">
									<EventMask ShowMask="true" />
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxEditProductionCancel"
							Text="Cancel"
							Icon="Delete">
							<Listeners>
								<Click Handler="#{uxEditProductionForm}.reset();
							parentAutoLoadControl.hide();" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxEditProductionSubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>
			</Items>
			<Listeners>
				<AfterRender
					Handler="var win = parentAutoLoadControl.target || parentAutoLoadControl, //you can use just 'parentAutoLoadControl' after update to Ext.NET v2 beta.
									size = this.getSize();
 
								size.height += 250;
								size.width += 12;
								win.setSize(size);"
					Delay="100" />
			</Listeners>
		</ext:Panel>
	</form>
</body>
</html>

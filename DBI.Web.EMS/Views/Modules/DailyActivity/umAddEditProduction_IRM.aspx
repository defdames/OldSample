<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddEditProduction_IRM.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umAddEditProduction_IRM" %>

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
	<form id="form1" runat="server">
		<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
		<ext:Panel runat="server" ID="uxAddEditPanel" Layout="FitLayout">
			<Items>
				<ext:FormPanel runat="server"
					ID="uxAddProductionForm"
					Layout="FormLayout"
					Hidden="true">
					<Items>
						<ext:DropDownField runat="server"
							ID="uxAddProductionTask"
							Mode="ValueText"
							AllowBlank="false"
							FieldLabel="Select Task">
							<Component>
								<ext:GridPanel runat="server"
									ID="uxAddProductionTaskGrid"
									Layout="HBoxLayout">
									<Store>
										<ext:Store runat="server"
											ID="uxAddProductionTaskStore">
											<Model>
												<ext:Model ID="Model2" runat="server">
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
												<ext:Parameter Name="Description" Value ="#{uxAddProductionTaskGrid}.getSelectionModel().getSelection()[0].data.DESCRIPTION" Mode="Raw" />
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
						<ext:DropDownField runat="server"
							ID="uxAddProductionExpenditureType"
							Mode="ValueText"
							AllowBlank="false"
							FieldLabel="Expenditure Type" >
							<Component>
								<ext:GridPanel runat="server"
									ID="uxAddProductionExpenditureGrid"
									Layout="HBoxLayout">
									<Store>
										<ext:Store runat="server"
											ID="uxAddProductionExpenditureStore"
											onReadData="deReadExpenditures"
											PageSize="10"
											RemoteSort="true"
											AutoDataBind="true">
											<Model>
												<ext:Model ID="Model1" runat="server">
													<Fields>
														<ext:ModelField Name="EXPENDITURE_TYPE" />
														<ext:ModelField Name="DESCRIPTION" />
														<ext:ModelField Name="UNIT_OF_MEASURE" />
														<ext:ModelField Name="BILL_RATE" />
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
											<ext:Column ID="Column1" runat="server" DataIndex="EXPENDITURE_TYPE" Text="Expenditure Type" />
											<ext:Column runat="server" DataIndex="DESCRIPTION" Text="Description" />
											<ext:Column ID="Column2" runat="server" DataIndex="BILL_RATE" Text="Bill Rate" />
											<ext:Column ID="Column3" runat="server" DataIndex="UNIT_OF_MEASURE" Text="Unit Of Measure" />
										</Columns>
									</ColumnModel>
									<DirectEvents>
										<SelectionChange OnEvent="deStoreExpenditureType">
											<ExtraParams>
												<ext:Parameter Name="ExpenditureType" Value="#{uxAddProductionExpenditureGrid}.getSelectionModel().getSelection()[0].data.EXPENDITURE_TYPE" Mode="Raw" />
												<ext:Parameter Name="BillRate" Value="#{uxAddProductionExpenditureGrid}.getSelectionModel().getSelection()[0].data.BILL_RATE" Mode="Raw" />
												<ext:Parameter Name="UnitOfMeasure" Value="#{uxAddProductionExpenditureGrid}.getSelectionModel().getSelection()[0].data.UNIT_OF_MEASURE" Mode="Raw" />
											</ExtraParams>
										</SelectionChange>
									</DirectEvents>
									<SelectionModel>
										<ext:RowSelectionModel ID="RowSelectionModel1" runat="server" Mode="Single" />
									</SelectionModel>
									<Plugins>
										<ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
									</Plugins>
									<BottomBar>
										<ext:PagingToolbar ID="PagingToolbar1" runat="server" />
									</BottomBar>
								</ext:GridPanel>
							</Component>
						</ext:DropDownField>
						<ext:Hidden runat="server" ID="uxAddProductionBillRate" />
						<ext:Hidden runat="server" ID="uxAddProductionUOM" />
						<ext:TextField runat="server"
							ID="uxAddProductionStation"
							FieldLabel="Station"
							AllowBlank="false" />
						<ext:TextField runat="server"
							ID="uxAddProductionQuantity"
							FieldLabel="Quantity"
							AllowBlank="false" />
						<ext:TextArea runat="server"
							ID="uxAddProductionComments"
							FieldLabel="Comments" />
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxAddProductionSubmit"
							Text="Submit"
							Icon="Add"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deAddProduction" />
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
					Hidden="true">
					<Items>
						<ext:DropDownField runat="server"
							ID="uxEditProductionTask"
							Mode="ValueText"
							AllowBlank="false"
							FieldLabel="Select Task">
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
												<ext:Parameter Name="Description" Value ="#{uxEditProductionTaskGrid}.getSelectionModel().getSelection()[0].data.DESCRIPTION" Mode="Raw" />
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
						<ext:DropDownField runat="server"
							ID="uxEditProductionExpenditureType"
							Mode="ValueText"
							AllowBlank="false"
							FieldLabel="Expenditure Type" >
							<Component>
								<ext:GridPanel runat="server"
									ID="uxEditProductionExpenditureGrid"
									Layout="HBoxLayout">
									<Store>
										<ext:Store runat="server"
											ID="uxEditProductionExpenditureStore"
											onReadData="deReadExpenditures"
											PageSize="10"
											RemoteSort="true"
											AutoDataBind="true">
											<Model>
												<ext:Model ID="Model4" runat="server">
													<Fields>
														<ext:ModelField Name="EXPENDITURE_TYPE" />
														<ext:ModelField Name="DESCRIPTION" />
														<ext:ModelField Name="BILL_RATE" />
														<ext:ModelField Name="UNIT_OF_MEASURE" />
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
											<ext:Column ID="Column4" runat="server" DataIndex="EXPENDITURE_TYPE" Text="Expenditure Type" />
											<ext:Column runat="server" DataIndex="DESCRIPTION" Text="Description" />
											<ext:Column ID="Column5" runat="server" DataIndex="BILL_RATE" Text="Bill Rate" />
											<ext:Column ID="Column6" runat="server" DataIndex="UNIT_OF_MEASURE" Text="Unit Of Measure" />
										</Columns>
									</ColumnModel>
									<DirectEvents>
										<SelectionChange OnEvent="deStoreExpenditureType">
											<ExtraParams>
												<ext:Parameter Name="ExpenditureType" Value="#{uxEditProductionExpenditureGrid}.getSelectionModel().getSelection()[0].data.EXPENDITURE_TYPE" Mode="Raw" />
												<ext:Parameter Name="BillRate" Value="#{uxEditProductionExpenditureGrid}.getSelectionModel().getSelection()[0].data.BILL_RATE" Mode="Raw" />
												<ext:Parameter Name="UnitOfMeasure" Value="#{uxEditProductionExpenditureGrid}.getSelectionModel().getSelection()[0].data.UNIT_OF_MEASURE" Mode="Raw" />
												<ext:Parameter Name="Type" Value="Edit" />
											</ExtraParams>
										</SelectionChange>
									</DirectEvents>
									<SelectionModel>
										<ext:RowSelectionModel ID="RowSelectionModel2" runat="server" Mode="Single" />
									</SelectionModel>
									<Plugins>
										<ext:FilterHeader ID="FilterHeader2" runat="server" Remote="true" />
									</Plugins>
									<BottomBar>
										<ext:PagingToolbar ID="PagingToolbar2" runat="server" />
									</BottomBar>
								</ext:GridPanel>
							</Component>
						</ext:DropDownField>
						<ext:TextField runat="server"
							ID="uxEditProductionStation"
							FieldLabel="Station"
							AllowBlank="false" />
						<ext:TextField runat="server"
							ID="uxEditProductionQuantity"
							FieldLabel="Quantity"
							AllowBlank="false" />
						<ext:Hidden runat="server"
							ID="uxEditProductionBillRate"
							FieldLabel="Bill Rate"
							AllowBlank="false" />
						<ext:Hidden runat="server"
							ID="uxEditProductionUOM"
							FieldLabel="Unit of Measure"
							AllowBlank="false" />
						<ext:TextArea runat="server"
							ID="uxEditProductionComments"
							FieldLabel="Comments" />
					</Items>
					<Buttons>
						<ext:Button runat="server"
							ID="uxEditProductionSubmit"
							Text="Submit"
							Icon="Add"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deEditProduction" />
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
 
								size.height += 150;
								size.width += 12;
								win.setSize(size);"
					Delay="100" />
			</Listeners>
		</ext:Panel>
	</form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umProductionTab_IRM.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umProductionTab_IRM" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
	<link href="../../../Resources/StyleSheets/main.css" rel="stylesheet" />
</head>
<body>
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
	<form id="form1" runat="server">
		<ext:GridPanel runat="server"
			ID="uxCurrentProductionGrid"
			Layout="HBoxLayout">
			<Store>
				<ext:Store runat="server"
					ID="uxCurrentProductionStore"
					AutoDataBind="true">
					<Model>
						<ext:Model ID="Model1" runat="server">
							<Fields>
								<ext:ModelField Name="PRODUCTION_ID" />
								<ext:ModelField Name="PROJECT_ID" />
								<ext:ModelField Name="LONG_NAME" />
								<ext:ModelField Name="TASK_ID" />
								<ext:ModelField Name="DESCRIPTION" />
								<ext:ModelField Name="STATION" />
								<ext:ModelField Name="EXPENDITURE_TYPE" />
								<ext:ModelField Name="BILL_RATE" />
								<ext:ModelField Name="UNIT_OF_MEASURE" />
								<ext:ModelField Name="COMMENTS" />
								<ext:ModelField Name="QUANTITY" />
							</Fields>
						</ext:Model>
					</Model>
				</ext:Store>
			</Store>
			<ColumnModel>
				<Columns>
					<ext:Column runat="server"
						DataIndex="PROJECT_ID"
						Text="Project Id"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="LONG_NAME"
						Text="Project Name"
						Flex="2" />
					<ext:Column runat="server"
						DataIndex="DESCRIPTION"
						Text="Task Name"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="STATION"
						Text="Station"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="EXPENDITURE_TYPE"
						Text="Expenditure Type"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="QUANTITY"
						Text="Quantity"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="BILL_RATE"
						Text="Bill Rate"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="UNIT_OF_MEASURE"
						Text="Unit Of Measure"
						Flex="1" />
					<ext:Column runat="server"
						DataIndex="COMMENTS"
						Text="Comments"
						Flex="1" />
				</Columns>
			</ColumnModel>
			<TopBar>
				<ext:Toolbar ID="Toolbar1" runat="server">
					<Items>
						<ext:Button runat="server"
							ID="uxAddProductionButton"
							Text="Add Production"
							Icon="ApplicationAdd">
							<Listeners>
								<Click Handler="#{uxAddProductionWindow}.show()" />
							</Listeners>
							<DirectEvents>
								<Click OnEvent="deGetTaskList">
									<ExtraParams>
										<ext:Parameter Name="Type" Value="Add" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" />
						<ext:Button runat="server"
							ID="uxEditProductionButton"
							Text="Edit Production"
							Icon="ApplicationEdit"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deEditProductionForm">
									<ExtraParams>
										<ext:Parameter Name="ProductionInfo" Value="Ext.encode(#{uxCurrentProductionGrid}.getRowsValues({selectedOnly : true}))" Mode="Raw" />
										<ext:Parameter Name="Type" Value="Edit" />
									</ExtraParams>
								</Click>
							</DirectEvents>
							<Listeners>
								<Click Handler="#{uxEditProductionWindow}.show()" />
							</Listeners>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
						<ext:Button runat="server"
							ID="uxRemoveProductionButton"
							Text="Remove Production"
							Icon="ApplicationDelete"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deRemoveProduction">
									<Confirmation ConfirmRequest="true" Title="Really?" Message="Do you really want to remove?" />
									<ExtraParams>
										<ext:Parameter Name="ProductionId" Value="#{uxCurrentProductionGrid}.getSelectionModel().getSelection()[0].data.PRODUCTION_ID" Mode="Raw" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
					</Items>
				</ext:Toolbar>
			</TopBar>
			<SelectionModel>
				<ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="true" Mode="Single" />
			</SelectionModel>
			<Listeners>
				<Select Handler="#{uxEditProductionButton}.enable();
					#{uxRemoveProductionButton}.enable()" />
				<Deselect Handler="#{uxEditProductionButton}.disable();
					#{uxRemoveProductionButton}.disable()" />
			</Listeners>
		</ext:GridPanel>
		<%-- Hidden Windows --%>
		<ext:Window runat="server"
			ID="uxAddProductionWindow"
			Layout="FormLayout"
			Hidden="true"
			Width="650">
			<Items>
				<ext:FormPanel runat="server"
					ID="uxAddProductionForm"
					Layout="FormLayout">
					<Items>
						<ext:ComboBox runat="server"
							ID="uxAddProductionTask"
							ValueField="TASK_ID"
							DisplayField="DESCRIPTION"
							FieldLabel="Select Task"
							QueryMode="Local"
							TypeAhead="true"
							AllowBlank="false">
							<Store>
								<ext:Store runat="server"
									ID="uxAddProductionTaskStore">
									<Model>
										<ext:Model ID="Model2" runat="server">
											<Fields>
												<ext:ModelField Name="TASK_ID" />
												<ext:ModelField Name="DESCRIPTION" />
											</Fields>
										</ext:Model>
									</Model>
								</ext:Store>
							</Store>
						</ext:ComboBox>
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
												<ext:Model runat="server">
													<Fields>
														<ext:ModelField Name="EXPENDITURE_TYPE" />
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
											<ext:Column runat="server" DataIndex="EXPENDITURE_TYPE" Text="Expenditure Type" />
											<ext:Column runat="server" DataIndex="BILL_RATE" Text="Bill Rate" />
											<ext:Column runat="server" DataIndex="UNIT_OF_MEASURE" Text="Unit Of Measure" />
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
										<ext:RowSelectionModel runat="server" Mode="Single" />
									</SelectionModel>
									<Plugins>
										<ext:FilterHeader runat="server" Remote="true" />
									</Plugins>
									<BottomBar>
										<ext:PagingToolbar runat="server" />
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
									#{uxAddProductionWindow}.hide();" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxAddProductionSubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>
			</Items>
			<Listeners>
				<Show Handler="#{uxAddProductionTask}.focus()" />
			</Listeners>
		</ext:Window>
		<ext:Window runat="server"
			ID="uxEditProductionWindow"
			Layout="FormLayout"
			Hidden="true"
			Width="650">
			<Items>
				<ext:FormPanel runat="server"
					ID="uxEditProductionForm"
					Layout="FormLayout">
					<Items>
						<ext:ComboBox runat="server"
							ID="uxEditProductionTask"
							ValueField="TASK_ID"
							DisplayField="DESCRIPTION"
							FieldLabel="Select Task"
							QueryMode="Local"
							TypeAhead="true"
							AllowBlank="false">
							<Store>
								<ext:Store runat="server"
									ID="uxEditProductionTaskStore">
									<Model>
										<ext:Model ID="Model3" runat="server">
											<Fields>
												<ext:ModelField Name="TASK_ID" />
												<ext:ModelField Name="DESCRIPTION" />
											</Fields>
										</ext:Model>
									</Model>
								</ext:Store>
							</Store>
						</ext:ComboBox>
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
												<ext:Model runat="server">
													<Fields>
														<ext:ModelField Name="EXPENDITURE_TYPE" />
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
											<ext:Column runat="server" DataIndex="EXPENDITURE_TYPE" Text="Expenditure Type" />
											<ext:Column runat="server" DataIndex="BILL_RATE" Text="Bill Rate" />
											<ext:Column runat="server" DataIndex="UNIT_OF_MEASURE" Text="Unit Of Measure" />
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
										<ext:RowSelectionModel runat="server" Mode="Single" />
									</SelectionModel>
									<Plugins>
										<ext:FilterHeader runat="server" Remote="true" />
									</Plugins>
									<BottomBar>
										<ext:PagingToolbar runat="server" />
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
								<Click OnEvent="deEditProduction">
									<ExtraParams>
										<ext:Parameter Name="ProductionId" Value="#{uxCurrentProductionGrid}.getSelectionModel().getSelection()[0].data.PRODUCTION_ID" Mode="Raw" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server"
							ID="uxEditProductionCancel"
							Text="Cancel"
							Icon="Delete">
							<Listeners>
								<Click Handler="#{uxEditProductionForm}.reset();
									#{uxEditProductionWindow}.hide();" />
							</Listeners>
						</ext:Button>
					</Buttons>
					<Listeners>
						<ValidityChange Handler="#{uxEditProductionSubmit}.setDisabled(!valid);" />
					</Listeners>
				</ext:FormPanel>
			</Items>
			<Listeners>
				<Show Handler="#{uxEditProductionTask}.focus()" />
			</Listeners>
		</ext:Window>
	</form>
</body>
</html>

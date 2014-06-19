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
        <ext:FormPanel runat="server"
            ID="uxAddProductionForm"
            DefaultButton="uxAddProductionSubmit" Height="300">
            <Items>
                <ext:Hidden ID="uxFormType" runat="server" />
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
                                    <ext:Column ID="Column7" runat="server" DataIndex="TASK_NUMBER" Text="Task Number" Flex="25" />
                                    <ext:Column ID="Column10" runat="server" DataIndex="DESCRIPTION" Text="Name" Flex="75" />
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
                <ext:DropDownField runat="server" Editable="false"
                    ID="uxAddProductionExpenditureType"
                    Mode="ValueText"
                    AllowBlank="false"
                    FieldLabel="Expenditure Type" Width="500">
                    <Component>
                        <ext:GridPanel runat="server"
                            ID="uxAddProductionExpenditureGrid"
                            Layout="HBoxLayout">
                            <Store>
                                <ext:Store runat="server"
                                    ID="uxAddProductionExpenditureStore"
                                    OnReadData="deReadExpenditures"
                                    PageSize="10"
                                    RemoteSort="true"
                                    AutoDataBind="true">
                                    <Model>
                                        <ext:Model ID="Model1" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="EXPENDITURE_TYPE" />
                                                <ext:ModelField Name="EXPENDITURE_DESCRIPTION" />
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
                                    <ext:Column runat="server" DataIndex="EXPENDITURE_DESCRIPTION" Text="Description" />
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
                    AllowBlank="false" Width="500" />
                <ext:NumberField runat="server"
                    ID="uxAddProductionQuantity"
                    FieldLabel="Quantity"
                    AllowBlank="false" Width="500" />
                <ext:ComboBox runat="server" ID="uxAddProductionSurfaceType"
                    FieldLabel="Surface Type"
                    DisplayField="type"
                    ValueField="type"
                    QueryMode="Local"
                    TypeAhead="true"
                    AllowBlank="false"
                    ForceSelection="true" Width="500">
                    <Store>
                        <ext:Store runat="server" ID="uxAddProductionSurfaceTypeStore" AutoDataBind="true">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="type" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Reader>
                                <ext:ArrayReader />
                            </Reader>
                        </ext:Store>
                    </Store>
                </ext:ComboBox>
                <ext:TextArea runat="server"
                    ID="uxAddProductionComments"
                    FieldLabel="Comments" Width="500" />
            </Items>
            <Buttons>
                <ext:Button runat="server"
                    ID="uxAddProductionSubmit"
                    Text="Save"
                    Icon="Add"
                    Disabled="true">
                    <DirectEvents>
                        <Click OnEvent="deProcessForm">
                            <EventMask ShowMask="true" />
                        </Click>
                    </DirectEvents>
                </ext:Button>
                <ext:Button runat="server"
                    ID="uxAddProductionCancel"
                    Text="Cancel"
                    Icon="Delete">
                    <Listeners>
                        <Click Handler="parentAutoLoadControl.close();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
            <Listeners>
                <ValidityChange Handler="#{uxAddProductionSubmit}.setDisabled(!valid);" />
                <AfterRender
                    Handler="var win = parentAutoLoadControl.target || parentAutoLoadControl, //you can use just 'parentAutoLoadControl' after update to Ext.NET v2 beta.
									size = this.getSize();
                                size.height += 34; 
								size.width += 24;
								win.setSize(size);"
                    Delay="100" />
            </Listeners>
        </ext:FormPanel>

    </form>
</body>
</html>

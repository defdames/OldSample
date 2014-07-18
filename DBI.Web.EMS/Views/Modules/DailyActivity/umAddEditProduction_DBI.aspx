﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddEditProduction_DBI.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umAddEditProduction_DBI" %>

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
        <ext:FormPanel runat="server"
            ID="uxAddProductionForm"
            Border="false"
            Width="600" DefaultButton="uxAddProductionSubmit" Padding="0">
            <Items>
                <ext:Hidden runat="server" ID="uxFormType" />
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
                                    <ext:Column ID="Column7" runat="server" DataIndex="TASK_NUMBER" Text="Task Number" Flex="25"/>
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
                <ext:TextArea runat="server"
                    ID="uxAddProductionWorkArea"
                    FieldLabel="Spray/Work Area"
                    Rows="5"
                    AllowBlank="false" Width="500" />
                <ext:TextField runat="server"
                    ID="uxAddProductionPoleFrom"
                    FieldLabel="Pole/MP From" Width="500" />
                <ext:TextField runat="server"
                    ID="uxAddProductionPoleTo"
                    FieldLabel="Pole/MP To" Width="500" />
                <ext:NumberField runat="server"
                    ID="uxAddProductionAcresPerMile"
                    FieldLabel="Acres/Mile" DecimalPrecision="3"
                    AllowBlank="false" Width="500" MinValue="0" />
                <ext:NumberField runat="server"
                    ID="uxAddProductionGallons"
                    FieldLabel="Gallons"
                    AllowBlank="false" Width="500" MinValue="0" />
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

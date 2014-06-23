﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umPayrollView.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.TimeClock.WebForm3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>
        var onGroupCommand = function (column, command, group) {
            if (command === 'SelectGroup') {
                var isSelected = App.uxEmployeeHoursGrid.getSelectionModel().isSelected(group.children[0]);

                if (!isSelected) {
                    column.grid.getSelectionModel().select(group.children, true);
                } else {
                    column.grid.getSelectionModel().deselect(group.children, true);
                };
            }

        };
    </script>
</head>
<body>

    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <form runat="server" id="form1">
    <ext:Viewport ID="ViewPort1" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel
                ID="uxPayrollAuditGrid"
                runat="server"
                Frame="true"
                Title="Payroll Audit" 
                Width ="1200"
                Resizeable="true"
                Collapsable="true">
                <Store>
                    <ext:Store ID="uxPayrollAuditStore" runat="server" AutoDataBind="true" GroupField="EMPLOYEE_NAME" OnReadData="deGetEmployeesHourData" PageSize="20">
                        <Model>
                            <ext:Model runat="server" IDProperty="TIME_CLOCK_ID">
                                <Fields>
                                    <ext:ModelField Name="TIME_CLOCK_ID" />
                                    <ext:ModelField Name="EMPLOYEE_NAME" />
                                    <ext:ModelField Name="TIME_IN" Type="Date" />
                                    <ext:ModelField Name="TIME_OUT" Type="Date"/>
                                    <ext:ModelField Name="ADJUSTED_HOURS" />
                                    <ext:ModelField Name="ADJUSTED_HOURS_GRID" />
                                    <ext:ModelField Name="ACTUAL_HOURS" />
                                    <ext:ModelField Name="ACTUAL_HOURS_GRID" />
                                    <ext:ModelField Name="APPROVED" />
                                    <ext:ModelField Name="SUBMITTED" />
                                </Fields>
                            </ext:Model>
                        </Model>
                        <Proxy>
						   <ext:PageProxy  />
					    </Proxy>
                        <Sorters>
                            <ext:DataSorter Property="TIME_IN" Direction="DESC"/>
                        </Sorters>
                    </ext:Store>
                </Store>
                <ColumnModel>
                    <Columns>
                        <ext:DateColumn ID="DateColumn1" runat="server" Text="Time In" DataIndex="TIME_IN" Flex="1" Format="M/d/yyyy h:mm tt"/>
                        <ext:DateColumn ID="DateColumn2" runat="server" Text="Time Out" DataIndex="TIME_OUT" Flex="1" Format="M/d/yyyy h:mm tt"/>
                        <ext:Column ID="Column1" runat="server" Text="Actual Time" Flex="1" DataIndex="ACTUAL_HOURS_GRID"/>
                        <ext:Column ID="Column2" runat="server" Text="Adjusted Time" Flex="1" DataIndex="ADJUSTED_HOURS_GRID"/>
                        <ext:Column ID="Approved" runat="server" Text="Approved" Flex="1" DataIndex="APPROVED" />
                        <ext:Column ID="Submitted" runat="server" Text="Submitted" Flex="1" DataIndex="SUBMITTED" />
                        <ext:CommandColumn ID="ccEditTime" runat="server">
                        <Commands>
                            <ext:GridCommand Icon="NoteEdit" CommandName="Edit" Text="Edit"/>
                        </Commands>
                        <DirectEvents>
                            <Command OnEvent="deEditTime">
                                <EventMask ShowMask="true">
                                </EventMask>
                               <ExtraParams>
                                    <ext:Parameter Name="id" Value="record.data.TIME_CLOCK_ID" Mode="Raw"></ext:Parameter>
                                    <ext:Parameter Name="command" Value="command" Mode="Raw" ></ext:Parameter>
                                </ExtraParams>
                            </Command>
                        </DirectEvents>
                    </ext:CommandColumn>
                        <ext:CommandColumn ID="CommandColumn1" runat="server" Hidden="true">
                        <GroupCommands>
                            <ext:GridCommand Icon="TableRow" CommandName="SelectGroup">
                                <ToolTip Title="Select" Text="Select all rows of the group" />
                            </ext:GridCommand>
                            <ext:CommandFill />
                        </GroupCommands>
                        <Listeners>
                            <GroupCommand Fn="onGroupCommand" />
                        </Listeners>
                    </ext:CommandColumn>
                    </Columns>
                </ColumnModel>
                <Features>
                <ext:Grouping ID="Grouping1"
                    runat="server"
                    HideGroupHeader="true"
                    GroupHeaderTplString="Employee: {name}"
                    StartCollapsed="true"/>                               
                </Features>
                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" Mode="Multi" />
                </SelectionModel>                
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>



 
</body>
</html>

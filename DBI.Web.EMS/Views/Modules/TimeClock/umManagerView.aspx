<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umManagerView.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.TimeClock.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
    
        <ext:ResourceManager ID="ResourceManager1" runat="server">
            <CustomDirectEvents>


            </CustomDirectEvents>
        </ext:ResourceManager>
    <form id="form1" runat="server">
    <ext:viewport ID="Viewport1" runat="server" Layout="Fit">
        
        
       <Items>
        <ext:GridPanel ID="uxEmployeeHoursGrid" runat="server" Frame="true" Title="Employee Hours" Icon ="Table" Width ="1200" Resizeable="true" Collapsable="true">
            <Store>
                <ext:Store ID="uxEmployeeHoursStore" runat="server" AutoDataBind="true" GroupField="EMPLOYEE_NAME" OnReadData="deGetEmployeeHoursData" PageSize="20" >
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
            <ColumnModel runat="server">
                <Columns>
                    <ext:DateColumn runat="server" Text="Time In" DataIndex="TIME_IN" Flex="1" Format="M/d/yyyy h:mm tt"/>
                    <ext:DateColumn runat="server" Text="Time Out" DataIndex="TIME_OUT" Flex="1" Format="M/d/yyyy h:mm tt"/>
                    <ext:Column ID="ActualTime" runat="server" Text="Actual Time" Flex="1" DataIndex="ACTUAL_HOURS_GRID"/>
                    <ext:Column ID="AdjustedTime" runat="server" Text="Adjusted Time" Flex="1" DataIndex="ADJUSTED_HOURS_GRID"/>
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
            
            <TopBar>
                <ext:Toolbar runat="server">
                    <Items>
                        <ext:Button runat="server" ID="uxApproveButton" Text="Approve" Icon="ApplicationPut">
                            <DirectEvents>
                                <Click OnEvent="deApproveTime">
                                    <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <%--<ext:Parameter Name="TimeClockId" Value="#{uxEmployeeHoursGrid}.getSelectionModel().getSelection()[0].data.TIME_CLOCK_ID" Mode="Raw" />
                                            <ext:Parameter Name="AdjustedHoursGrid" Value="#{uxEmployeeHoursGrid}.getSelectionModel().getSelection()[0].data.ADJUSTED_HOURS_GRID" Mode="Raw" />--%>
                                            <ext:Parameter Name="ApprovedTime" Value="Ext.encode(#{uxEmployeeHoursGrid}.getRowsValues({selectedOnly : true}))" Mode="Raw" />
                                            <ext:Parameter Name="NewTime" Value="#{uxEmployeeHoursStore}.getChangedData()" Mode="Raw" Encode="true" />
                                        </ExtraParams>
                               </Click> 
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button runat="server" ID="uxEdit" Text="Edit" Icon="ApplicationEdit">
                        <DirectEvents>
                            <Click OnEvent="deEditTime">
                                <ExtraParams>
                                    <ext:Parameter Name="Edit" Value="True"></ext:Parameter>
                                </ExtraParams>
                                <EventMask ShowMask="true" />
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:ToolbarSpacer runat="server" />
                    <ext:Checkbox runat="server" ID="uxToggleApproved" BoxLabel="Show Approved" BoxLabelAlign="After" >
					    <Listeners>
					        <Change Handler="#{uxEmployeeHoursStore}.reload()" />
				        </Listeners>
				    </ext:Checkbox>
                        </Items>
                </ext:Toolbar>
            </TopBar>
            <Features>
                <ext:Grouping
                    runat="server"
                    HideGroupHeader="true"
                    GroupHeaderTplString="Employee: {name}"
                    StartCollapsed="true"/>                               
            </Features>
            <SelectionModel>
                <ext:CheckboxSelectionModel ID="uxTimeClockSelectionModel" runat="server" Mode="Multi"/>
            </SelectionModel>
        </ext:GridPanel>
           </Items>
        </ext:viewport>
        </form>
        
    
    
</body>
</html>

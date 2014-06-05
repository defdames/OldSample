<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umManagerView.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.TimeClock.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    
        <ext:ResourceManager ID="ResourceManager1" runat="server"/>
    <form id="form1" runat="server">
    <ext:viewport ID="Viewport1" runat="server" Layout="Fit">
        
        
       <Items>
        <ext:GridPanel ID="uxEmployeeHoursGrid" runat="server" Frame="true" Title="Employee Hours" Icon ="Table" Width ="1200" Resizeable="true" Collapsable="true">
            <Store>
                <ext:Store ID="uxEmployeeHoursStore" runat="server" AutoDataBind="true" GroupField="EMPLOYEE_NAME" OnReadData="deGetEmployeeHoursData" PageSize="20">
                    <Model>
                        <ext:Model runat="server">
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
                </Columns>
            </ColumnModel>
            <TopBar>
                <ext:Toolbar runat="server">
                    <Items>
                        <ext:Button runat="server" ID="uxApproveButton" Text="Approve" Icon="ApplicationPut">
                            <DirectEvents>
                                <Click OnEvent="deApproveTime">
                                    <%--<EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="TimeClockId" Value="#{uxEmployeeHoursGrid}.getSelectionModel().getSelection()[0].data.TIME_CLOCK_ID" Mode="Raw" />
                                            <ext:Parameter Name="AdjustedHoursGrid" Value="#{uxEmployeeHoursGrid}.getSelectionModel().getSelection()[0].data.ADJUSTED_HOURS_GRID" Mode="Raw" />
                                            <ext:Parameter Name="ApprovedTime" Value="Ext.encode(#{uxEmployeeHoursGrid}.getRowsValues({selectedOnly : true}))" Mode="Raw" />
                                            <ext:Parameter Name="NewTime" Value="#{uxEmployeeHoursStore}.getChangedData()" Mode="Raw" Encode="true" />
                                        </ExtraParams>--%>
                               </Click> 
                        </DirectEvents>
                    </ext:Button>
                    <ext:ToolbarSpacer runat="server" />
                    <ext:Checkbox runat="server" ID="uxToggleApproved" FieldLabel="Show Approved" >
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
                <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" Mode="Multi" />
            </SelectionModel>
            <Plugins>
                <ext:CellEditing runat="server" ClicksToEdit="2"/>
            </Plugins> 
        </ext:GridPanel>
           </Items>
        </ext:viewport>
        </form>
        
    
    
</body>
</html>

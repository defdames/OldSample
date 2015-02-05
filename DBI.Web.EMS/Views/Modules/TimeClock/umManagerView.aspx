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
		var prepare = function (grid, toolbar, rowIndex, record) {
		    var firstButton = toolbar.items.get(0);

		    if (record.data.SUBMITTED == "Y") {
		        firstButton.setDisabled(true);
		        firstButton.setTooltip("Disabled");
		    }
		};
		var colorErrors = function (value, metadata, record) {
		    if (record.data.ACTUAL_HOURS >= 12) {
		        metadata.style = "background-color: red;";
		    }
		    return value;
		};

		

		var onShow = function (toolTip, grid) {
		        var view = grid.getView(),
                store = grid.getStore(),
                record = view.getRecord(view.findItemByChild(toolTip.triggerElement)),
                data = record.data.ACTUAL_HOURS;
                
		    if (data >= 12) {

		        toolTip.update("OVER 12 HOURS");
		    } else {

		        return false;
		    }
		    
		};
	</script>
</head>
<body>
	
		<ext:ResourceManager ID="ResourceManager1" runat="server">
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
                                <ext:ModelField Name ="ADJUSTED_LUNCH" />
                                <ext:ModelField Name ="ADJUSTED_LUNCH_GRID" />
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
					<ext:Column ID="ActualTime" runat="server" Text="Actual Time" Flex="1" DataIndex="ACTUAL_HOURS_GRID">
                        <Renderer Fn="colorErrors" />
                    </ext:Column>
					<%--<ext:Column ID="AdjustedTime" runat="server" Text="Adjusted Time" Flex="1" DataIndex="ADJUSTED_HOURS_GRID">
                        <Renderer Fn="colorErrors" />
                    </ext:Column>
                    <ext:Column ID="AdjustedLunch" runat="server" Text="Adjusted Lunch" Flex="1" DataIndex="ADJUSTED_LUNCH_GRID">
                        <Renderer Fn="colorErrors" />
                    </ext:Column>--%>
					<ext:Column ID="Approved" runat="server" Text="Approved" Flex="1" DataIndex="APPROVED" />
					<ext:Column ID="Submitted" runat="server" Text="Submitted" Flex="1" DataIndex="SUBMITTED" />
					<ext:CommandColumn ID="ccEditTime" runat="server">
						<Commands>
							<ext:GridCommand Icon="NoteEdit" CommandName="Edit" Text="Edit"/>
						</Commands>
                        <PrepareToolbar Fn="prepare"/>
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
                    <ext:CommandColumn ID="ccDeleteTime" runat="server">
                        <Commands>
                            <ext:GridCommand Icon="Delete" CommandName="Delete" Text="Delete" />
                        </Commands>
                        <PrepareToolbar Fn="prepare"/>
                        <DirectEvents>
                            <Command OnEvent="deDeleteTime">
                                <EventMask ShowMask="true">
                                </EventMask>
                                <ExtraParams>
                                    <ext:Parameter Name="delId" Value="record.data.TIME_CLOCK_ID" Mode="Raw"></ext:Parameter>
                                    <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                                </ExtraParams>
                            </Command>
                        </DirectEvents>
                    </ext:CommandColumn>
					<ext:CommandColumn ID="CommandColumn1" runat="server" Hidden="true">
						<GroupCommands>
							<ext:GridCommand Icon="TableRow" CommandName="SelectGroup">
								<ToolTip Title="Select" Text="Select all rows of the group"  />
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
                        <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" />
                        <ext:Button runat="server" ID="uxAddTime" Text="Add Date" Icon="Add">
                            <DirectEvents>
                                <Click OnEvent="deAddTime">
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
                    <ext:ToolbarSpacer runat="server" />
                    <ext:Checkbox runat="server" ID="uxToggleEmployees" BoxLabel="Show All Employees" BoxLabelAlign="After" >
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
					StartCollapsed="false"/>                               
			</Features>
			<SelectionModel>
				<ext:CheckboxSelectionModel ID="uxTimeClockSelectionModel" runat="server" Mode="Multi"/>
			</SelectionModel>
            <View>
                <ext:GridView ID="GridView1" runat="server" StripeRows="true" TrackOver="true" />
            </View>
		</ext:GridPanel>
           <ext:ToolTip
               ID="ToolTip1" 
            runat="server" 
            Target="={#{uxEmployeeHoursGrid}.getView().el}"
            Delegate=".x-grid-cell"
            TrackMouse="true">
            <Listeners>
                <BeforeShow Handler="return onShow(this, #{uxEmployeeHoursGrid});" />
            </Listeners>
        </ext:ToolTip>
		   </Items>
		</ext:viewport>
		</form>
		
	
	
</body>
</html>

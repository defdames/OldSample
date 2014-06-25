<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="um12HourDayReport.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.TimeClock.um12HourDayReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
   <div></div>
        <ext:ResourceManager ID="ResourceManager1" runat="server">
		</ext:ResourceManager>
        <ext:Viewport ID="Viewport1" runat="server" Layout="Fit">
            <Items>
                <ext:GridPanel ID="uxUnapprovedHoursList" runat="server" Layout="FitLayout" Region="Center" AutoDataBind="true" >
					<Store>
						<ext:Store ID="uxHoursStore" runat="server" AutoDataBind="true" GroupField="EMPLOYEE_NAME" OnReadData="deGetEmployeeHoursData" PageSize="25">
							<Model>
								<ext:Model runat="server" ID="Model1">
									<Fields>
                                       <ext:ModelField Name="EMPLOYEE_NAME" />
								<ext:ModelField Name="TIME_IN" Type="Date" />
								<ext:ModelField Name="TIME_OUT" Type="Date"/>
								<ext:ModelField Name="ADJUSTED_HOURS" />
								<ext:ModelField Name="ADJUSTED_HOURS_GRID" />
								<ext:ModelField Name="ACTUAL_HOURS" />
								<ext:ModelField Name="ACTUAL_HOURS_GRID"/>
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
					<ColumnModel ID="ColumnModel1" runat="server">
						<Columns>
							<ext:DateColumn ID="colTimeIn"
								runat="server" 
								Text="Time In"
								Format="MM/dd/yyyy hh:mm tt" 
								DataIndex="TIME_IN"
								Flex="1"/>
							<ext:DateColumn ID="colTimeOut" 
								runat="server" 
								Text="Time Out"
								Format="MM/dd/yyyy hh:mm tt" 
								DataIndex="TIME_OUT" 
								Flex="1"/>
                            <ext:Column ID="TotalTime" 
                                runat="server" 
                                Text="Total Time" 
                                Flex="1" 
                                DataIndex="ACTUAL_HOURS_GRID"/>
                            </Columns>
					</ColumnModel>
					<BottomBar>
						<ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="true" DisplayMsg="Records {0} - {1} of {2}"/>
					</BottomBar>
                    <Features>
				            <ext:Grouping ID="Grouping1"
					        runat="server"
					        HideGroupHeader="true"
					        GroupHeaderTplString="Employee: {name}"
					        StartCollapsed="false"/>                               
			        </Features>
				</ext:GridPanel>
            </Items>

        </ext:Viewport>
    </form>
</body>
</html>

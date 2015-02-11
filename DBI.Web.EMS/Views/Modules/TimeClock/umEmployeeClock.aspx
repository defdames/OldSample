<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEmployeeClock.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.TimeClock.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title></title>
	
	<script type="text/javascript">
		var getTime = function()
		{   // For todays date;
			Date.prototype.today = function () {
				//return ((this.getDate() < 10)?"0":"") + this.getDate() +"/"+(((this.getMonth()+1) < 10)?"0":"") + (this.getMonth()+1) +"/"+ this.getFullYear();
				return (((this.getMonth() + 1) < 10) ? "0" : "") + (this.getMonth() + 1) + "/" + ((this.getDate() < 10) ? "0" : "") + this.getDate() + "/" + this.getFullYear();
			}
			// For the time now
			Date.prototype.timeNow = function () {
				//return ((this.getHours() < 10) ? "0" : "") + this.getHours() + ":" + ((this.getMinutes() < 10) ? "0" : "") + this.getMinutes() + ":" + ((this.getSeconds() < 10) ? "0" : "") + this.getSeconds();
				return ((this.getHours() > 12) ? (this.getHours() - 12)  : this.getHours()) + ":" + ((this.getMinutes() < 10) ? "0" : "") + this.getMinutes() + " " + ((this.getHours() >= 12) ? ('pm') : 'am');
			}
				var datetime = new Date().today() + " " + new Date().timeNow();
			   
				document.getElementById("uxDateTime").value = datetime
	   }
	</script>
</head>
<body>
	<form id="form1" runat="server">
   
		<ext:ResourceManager ID="resourceManager1" runat="server"/>

		   
			   <ext:Panel
				   runat="server"
				   BodyPadding="5"
				   Width="1200">
					<Items>
						<ext:Formpanel runat="server" 
							ButtonAlign="Left"
							Title="Clock"
							BodyPadding="5"
							Region="North"
							Layout="FormLayout">
							<Items>
								<ext:TextField Id="uxTime_InTextBox" runat="server" FieldLabel="Time In" readonly="true"/>
								<ext:TextField ID="uxTime_OutTextBox" runat="server" FieldLabel="Time Out" readonly="true"/>
								<ext:TextField ID="uxUser_NameTextBox" runat="server" FieldLabel="Name"   readonly="true" />
								<ext:Hidden runat="server" ID="uxDateTime" />
							</Items>
						
						 <Buttons>
								<ext:Button runat="server" ID="uxTimeButton" Text="Clock In" >
									<DirectEvents>
										<Click OnEvent="deSetTime"/>
									</DirectEvents>
									<Listeners>
										<Click Fn="getTime" />
									</Listeners>
									
								</ext:Button>
							</Buttons>

							</ext:Formpanel>
					

				<ext:GridPanel ID="uxEmployeeHoursList" runat="server" Layout="FitLayout" Region="Center" AutoDataBind="true" >
					<Store>
						<ext:Store ID="uxHoursStore" runat="server" PageSize="25">
							<Model>
								<ext:Model runat="server" ID="Model1">
									<Fields>
										<ext:ModelField Name="TIME_IN" Type="Date"/>
										<ext:ModelField Name="TIME_OUT" Type="Date"/>
										<ext:ModelField Name="TOTAL_HOURS" />
										<ext:ModelField Name="MODIFIED_BY" />
                                        <ext:ModelField Name="APPROVED" />
									</Fields>
								</ext:Model>
							</Model>
							<Sorters>
								<ext:DataSorter Property="TIME_IN" Direction="DESC"/>
							</Sorters>
						</ext:Store>
					</Store>
					<ColumnModel runat="server">
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
							<ext:Column ID="colTotalHours"
								runat="server"
								Text="Total Time" 
								DataIndex="TOTAL_HOURS"
								Flex="1"/>
							<ext:Column ID="colModifiedBy"
								runat="server"
								Text="Modified by"
								DataIndex="MODIFIED_BY"
								Flex="1" />
                            <ext:Column ID="colApproved"
                                runat="server"
                                Text="Approved"
                                DataIndex="APPROVED"
                                Flwx="1" />
						    </Columns>
                            
					</ColumnModel>
					<BottomBar>
						<ext:PagingToolbar runat="server" DisplayInfo="true" DisplayMsg="Records {0} - {1} of {2}"/>
					</BottomBar>
				</ext:GridPanel>

			</Items>
		  </ext:Panel>
		 


		
	</form>
</body>
</html>

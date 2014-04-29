<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEmployeeClock.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.TimeClock.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
   

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
                            </Items>
                        
                         <Buttons>
                                <ext:Button runat="server" ID="uxTimeButton" Text="Clock In" >
                                    <DirectEvents>
                                        <Click OnEvent="deSetTime"/>
                                    </DirectEvents>
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
                                Format="M/d/yyyy h:mm tt" 
                                DataIndex="TIME_IN"
                                Flex="1"/>
                            <ext:DateColumn ID="colTimeOut" 
                                runat="server" 
                                Text="Time Out"
                                Format="M/d/yyyy h:mm tt" 
                                DataIndex="TIME_OUT" 
                                Flex="1"/>
                            <ext:Column ID="colTotalHours"
                                runat="server"
                                Text="Total Time" 
                                DataIndex="TOTAL_HOURS"
                                Flex="1"/>
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

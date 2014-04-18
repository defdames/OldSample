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
                   Width="800"
                   BodyPadding="5">
                    <Items>
                        <ext:Formpanel runat="server"
                            Title="Clock"
                            BodyPadding="5"
                            Width="350"
                            region="North"
                            layout="FormLayout">
                            <Items>
                                <ext:TextField Id="uxTime_InTextBox" runat="server" FieldLabel="Time In" readonly="true"/>
                                <ext:TextField ID="uxTime_OutTextBox" runat="server" FieldLabel="Time Out" readonly="true"/>
                                <ext:TextField ID="uxUser_NameTextBox" runat="server" FieldLabel="Name"   readonly="true" />
                            </Items>
                        
                         <Buttons>
                                <ext:Button runat="server" ID="uxTimeButton" Text="Clock In">
                                    <DirectEvents>
                                        <Click OnEvent="deSetTime"/>
                                    </DirectEvents>
                                </ext:Button>
                            </Buttons>

                            </ext:Formpanel>
                    

                <ext:GridPanel ID="uxEmployeeHoursList" runat="server" Layout="FitLayout" Region="Center" AutoDataBind="true">
                    <Store>
                        <ext:Store ID="uxHoursStore" runat="server">
                            <Model>
                                <ext:Model runat="server" ID="Model1">
                                    <Fields>
                                        <ext:ModelField Name="TIME_IN" Type="Date"/>
                                        <ext:ModelField Name="TIME_OUT" Type="Date"/>
                                        <ext:ModelField Name="TOTAL_HOURS" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel runat="server">
                        <Columns>
                            <ext:DateColumn ID="colTimeIn"
                                runat="server" 
                                Text="Time In"
                                Format="M/d/yyyy h:mm tt" 
                                DataIndex="TIME_IN"/>
                            <ext:DateColumn ID="colTimeOut" 
                                runat="server" 
                                Text="Time Out"
                                Format="M/d/yyyy h:mm tt" 
                                DataIndex="TIME_OUT" />
                            <ext:Column ID="colTotalHours"
                                runat="server"
                                Text="Total Hours" 
                                DataIndex="TOTAL_HOURS"/>
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>

            </Items>
          </ext:Panel>
         


        
    </form>
</body>
</html>

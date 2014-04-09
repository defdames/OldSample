﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEmployeeClock.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.TimeClock.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
   

</head>
<body>
    <form id="form1" runat="server">
    <div></div>
        <ext:ResourceManager ID="resourceManager1" runat="server"/>

            <div>
                <ext:Viewport ID="ViewPort1" runat="server" layout="BorderLayout">
                    <Items>
                        <ext:FormPanel ID="FormPanel1" runat="server"
                            Title="Clock"
                            BodyPadding="5"
                            Width="350">

                            <Items>
                                <ext:TextField Id="uxTime_InTextBox" runat="server" FieldLabel="Time In" readonly="true"/>
                                <ext:TextField ID="uxTime_OutTextBox" runat="server" FieldLabel="Time Out" readonly="true"/>
                                <ext:TextField ID="uxUser_NameTextBox" runat="server" FieldLabel="Name"   readonly="true" />
                            </Items>
                            <Buttons>
                                <ext:Button runat="server" ID="uxTimeInButton" Text="Time In">
                                    <DirectEvents>
                                        <Click OnEvent="deSetTimeIn"/>
                                    </DirectEvents>
                                </ext:Button>

                                <ext:Button runat="server" ID="uxTimeOutButton" Text="Time Out">
                                    <DirectEvents>
                                        <Click OnEvent="deSetTimeOut"/>
                                    </DirectEvents>
                                </ext:Button>
                            </Buttons>


                        </ext:FormPanel>


                    </Items>
                </ext:Viewport>


            </div>


        
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddEditProfileOptions.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Security.Options.AddEdit.umAddEditProfileOptions" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False"  />         
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>

                <ext:FormPanel ID="uxFormPanel" runat="server" Header="false" BodyPadding="10"
                    Margins="5 5 5 5" Region="Center">
                    <Items>
                        <ext:FieldContainer ID="uxFieldContainer2"
                            runat="server"
                            LabelStyle="font-weight:bold;padding:0;"
                            Layout="HBoxLayout">
                             <Defaults>
                                <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />
                                <ext:Parameter Name="MsgTarget" />
                            </Defaults>
                            <Items>
                                <ext:TextField runat="server" ID="uxProfileKey" FieldLabel="Name" AnchorHorizontal="55%" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" MaxLengthText="250" >
                                </ext:TextField>
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer ID="uxFieldContainer1"
                            runat="server"
                            LabelStyle="font-weight:bold;padding:0;"
                            Layout="HBoxLayout">
                             <Defaults>
                                <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />
                                <ext:Parameter Name="MsgTarget"/>
                            </Defaults>
                            <Items>
                               <ext:TextArea runat="server" ID="uxProfileDescription" FieldLabel="Description" AnchorHorizontal="55%" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" MaxLengthText="2000" Height="125"  />
                            </Items>
                        </ext:FieldContainer> 
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxAddProfileOptionButton" Text="Save" Disabled="true" FormBind="true" Icon="ApplicationAdd">
                            <DirectEvents>
                                <Click OnEvent="deSaveProfileOption" Success="parent.Ext.getCmp('uxAddEditProfileOptionWindow').close();">
                                    <EventMask ShowMask="true"></EventMask>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="uxCloseButton" runat="server" Text="Close Form">
                            <Listeners>
                                <Click Handler="parent.Ext.getCmp('uxAddEditProfileOptionWindow').close();"></Click>
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
   </form>
</body>
</html>

﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="uxDefault.aspx.cs" Inherits="DBI.Web.EMS.Views.uxDefault" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Resources/StyleSheets/main.css" rel="stylesheet" />
</head>
<body>
    <ext:ResourceManager ID="uxResourceManager" runat="server" >
    </ext:ResourceManager>
    <form id="form1" runat="server">
        <ext:Viewport ID="uxViewPort" runat="server" Layout="border">
            <Items>
                <ext:Panel ID="uxNorth" runat="server" Collapsible="false" Height="77" Region="North" BodyPadding="5" Split="true" Margins="5px 5px 0px 5px" BodyStyle="background-color: black;" Layout="HBoxLayout">
                  <Defaults>
                <ext:Parameter Name="margins" Value="0 5 0 0" Mode="Value" />
            </Defaults>
            <LayoutConfig>
                <ext:HBoxLayoutConfig  Align="Middle" />
            </LayoutConfig>
            <Items>
                <ext:Image ID="uxLogo" runat="server" ImageUrl="~/Resources/Images/dbis_logo.png" Align="Left" Width="108.54" Height="67"></ext:Image>
                <ext:Panel ID="uxToolBarPanel" runat="server" BaseCls="x-plain" Flex="1"/>
                <ext:Container runat="server" cls="my-container" BaseCls="x-plain">
                    <Items>
                        <ext:Toolbar ID="uxToolBar" runat="server" Border="false" cls="my-toolbar">
                            <Items>
                                <ext:LinkButton ID="uxHelp" runat="server" Text="Help" CtCls="header-actions-button"></ext:LinkButton>
                                 <ext:ToolbarSpacer runat="server"></ext:ToolbarSpacer>
                                <ext:LinkButton ID="uxLogout" runat="server" Text="Logout" CtCls="header-actions-button">
                                    <DirectEvents><Click OnEvent="deLogout"><Confirmation ConfirmRequest="true" Message="Are you sure you want to logoff?"></Confirmation></Click></DirectEvents>
                                </ext:LinkButton>
                            </Items>
                        </ext:Toolbar>
                    </Items>
                </ext:Container>
            </Items>
                </ext:Panel>
                <ext:Panel ID="uxWest" runat="server" Layout="accordion" Region="West" Collapsible="true" Split="true" Width="230" Margins="0px 0px 5px 5px">
                    <Items>
                        <ext:Panel ID="uxApplications" runat="server" Border="false"  Title="Applications" AutoScroll="true" Icon="ApplicationForm">
                        </ext:Panel>
                        <ext:Panel ID="uxSystem" runat="server" Border="false" Title="System Administration" AutoScroll="true" Icon="Server">
                            <Items>
                                <ext:Menu ID="uxMenu"
                                    runat="server"
                                    Floating="false"
                                    Layout="VBoxLayout"
                                    ShowSeparator="false" Border="false">
                                    <Defaults>
                                        <ext:Parameter Name="MenuAlign" Value="tl-bl?" Mode="Value" />
                                    </Defaults>
                                    <Items>
                                         <ext:MenuItem ID="uxSecurityUsers" runat="server" Text="Security Users" Icon="User">
                                           <DirectEvents>
                                               <Click OnEvent="deLoadSecurityUsers" ></Click>
                                           </DirectEvents>
                                        </ext:MenuItem>
                                         <ext:MenuItem ID="uxSecurityRoles" runat="server" Text="Security Roles" Icon="UserBrown">
                                           <DirectEvents>
                                               <Click OnEvent="deLoadSecurityRoles" ></Click>
                                           </DirectEvents>
                                        </ext:MenuItem>
                                    </Items>
                                </ext:Menu>
                            </Items>
                        </ext:Panel>
                    </Items>
                </ext:Panel>
                <ext:Panel ID="uxCenter" runat="server" Region="Center" Header="true" Margins="0px 5px 5px 0px">
                    <Items>
                    </Items> 
                </ext:Panel>
            </Items>
        </ext:Viewport>
    </form>
    
</body>
</html>

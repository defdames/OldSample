﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="uxDefault.aspx.cs" Inherits="DBI.Web.EMS.Views.uxDefault" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Resources/StyleSheets/main.css" rel="stylesheet" />
</head>
<body>
    <ext:ResourceManager ID="uxResourceManager" runat="server">
    </ext:ResourceManager>
    <form id="uxForm" runat="server">
        <ext:Viewport ID="uxViewPort" runat="server" Layout="border">
            <Items>
                <ext:Panel ID="uxNorth" runat="server" Collapsible="false" Height="77" Region="North" BodyPadding="5" Split="true" Margins="5px 5px 0px 5px" BodyStyle="background-color: black;" Layout="HBoxLayout">
                    <Defaults>
                        <ext:Parameter Name="margins" Value="0 5 0 0" Mode="Value" />
                    </Defaults>
                    <LayoutConfig>
                        <ext:HBoxLayoutConfig Align="Middle" />
                    </LayoutConfig>
                    <Items>
                        <ext:Image ID="uxLogo" runat="server" ImageUrl="~/Resources/Images/dbis_logo.png" Align="Left" Width="108.54" Height="67"></ext:Image>
                        <ext:Panel ID="uxToolBarPanel" runat="server" BaseCls="x-plain" Flex="1" />

                        <ext:Container runat="server" Cls="my-container" BaseCls="x-plain" Layout="VBoxLayout">
                            <LayoutConfig>
                                <ext:VBoxLayoutConfig Align="Stretch" />
                            </LayoutConfig>
                            <Items>
                                <ext:Toolbar ID="uxToolbarWelcome" runat="server" Border="false" Cls="my-toolbar">
                                    <Items>
                                        <ext:ToolbarFill></ext:ToolbarFill>
                                        <ext:LinkButton ID="uxWelcomeName" runat="server">
                                            <DirectEvents>
                                                <Click OnEvent="deRemoveImpersonate">
                                                    <Confirmation ConfirmRequest="true" Message="Are you sure you want to stop impersonating this user?"></Confirmation>
                                                </Click>
                                            </DirectEvents>
                                        </ext:LinkButton>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="uxToolbarTime" runat="server" Border="false" Cls="my-toolbar">
                                    <Items>
                                        <ext:ToolbarFill></ext:ToolbarFill>
                                        <ext:Label ID="uxWelcomeTime" runat="server" CtCls="welcome-orange"></ext:Label>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="uxToolBarLinks" runat="server" Border="false" Cls="my-toolbar">
                                    <Items>
                                        <ext:ToolbarFill></ext:ToolbarFill>
                                        <ext:LinkButton ID="uxHelp" runat="server" Text="Help" CtCls="header-actions-button"></ext:LinkButton>
                                        <ext:ToolbarSpacer></ext:ToolbarSpacer>
                                        <ext:LinkButton ID="uxLogout" runat="server" Text="Logout" CtCls="header-actions-button">
                                            <DirectEvents>
                                                <Click OnEvent="deLogout">
                                                    <Confirmation ConfirmRequest="true" Message="Are you sure you want to logoff?"></Confirmation>
                                                </Click>
                                            </DirectEvents>
                                        </ext:LinkButton>
                                    </Items>
                                </ext:Toolbar>
                            </Items>
                        </ext:Container>

                    </Items>
                </ext:Panel>
                <ext:Panel ID="uxWest" runat="server" Layout="accordion" Region="West" Collapsible="true" Split="true" Width="230" Margins="0px 0px 5px 5px" />
                <ext:Panel ID="uxCenter" runat="server" Region="Center" Header="true" Margins="0px 5px 5px 0px">
                    <Items>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

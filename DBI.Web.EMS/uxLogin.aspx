<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="uxLogin.aspx.cs" Inherits="DBI.Web.EMS.uxLogin" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Enterprise Management System</title>
    <style type="text/css">
        body
        {
            background: #3263A8 url("Resources/Images/bgimage.jpg");
            background-position: center center;
        }

        #container #banner
        {
            border-bottom-width: 2px;
            border-bottom-style: solid;
            border-bottom-color: #F08840;
            position: relative;
            z-index: 1;
        }

        .icon-combo-item
        {
            background-repeat: no-repeat !important;
            background-position: 3px 50% !important;
            padding-left: 24px !important;
        }
    </style>
    <script src="Resources/Scripts/jquery-2.0.1.js"></script>
    <script type="text/javascript">
        <!-- This code will allow the login box to fade images in and out -->
    $(document).ready(function () {
        $('#banner img:gt(0)').hide();
        setInterval(function () {
            $('#banner :first-child').delay('500').fadeOut(function () {
                $(this).next('img').fadeIn(1500).delay(2500).end().appendTo('#banner');
            });
        }, 1000)
    });
    </script>

</head>
<body>
    <ext:ResourceManager ID="uxResourceManager" runat="server" IsDynamic="False">
    </ext:ResourceManager>
    <ext:Label runat="server" ID="uxDatabaseVer" StyleSpec="position:fixed;bottom: 1em; right: 1em; color:lightgray;"></ext:Label>
    <ext:Label runat="server" ID="uxCopyRight" Text="Copyright 2013 DBI Services, All Rights Reserved." StyleSpec="position:fixed;bottom: 1em; left: 1em; color: lightgray;"></ext:Label>
    <ext:Image runat="server" ID="uxLogo" ImageUrl="~/Resources/Images/dbis_logo.png" StyleSpec="position:fixed;bottom: 3em; right: 1em;"></ext:Image>
    <ext:Viewport ID="uxViewPort" runat="server" IDMode="Explicit" IsDynamic="False" Namespace="App" RenderXType="True">
        <LayoutConfig>
            <ext:VBoxLayoutConfig Align="Center" Pack="Center" />
        </LayoutConfig>
        <Items>
            <ext:FormPanel ID="uxFormPanel"
                runat="server"
                Title="Enterprise Management System"
                Width="530"
                Frame="true"
                BodyPadding="13"
                Height="370"
                DefaultAnchor="100%" meta:resourcekey="loginFormPanelResource1">
                <Items>
                    <ext:Panel ID="uxBannerContainer" runat="server" Frame="true" Header="false" MarginSpec="0 0 10 0">
                        <Content>
                            <div id="banner">
                                <img src="Resources/Images/highways.png" width="497" height="184" />
                                <img src="Resources/Images/striping.jpg" width="497" height="184" />
                                <img src="Resources/Images/train-on-tracks-rs.jpg" width="497" height="184" />
                                <img src="Resources/Images/wwbnight.png" width="497" height="184" />
                            </div>
                        </Content>
                    </ext:Panel>
                    <ext:TextField ID="uxUsername"
                        runat="server"
                        FieldLabel="Username"
                        Name="user"
                        AutoFocus="true"
                        EmptyText="username" meta:resourcekey="usernameFieldResource1" />

                    <ext:TextField ID="uxPassword"
                        runat="server"
                        FieldLabel="Password"
                        Name="pass"
                        EmptyText="password"
                        InputType="Password" meta:resourcekey="passwordFieldResource1" />
                    <ext:ComboBox
                        ID="uxRegion"
                        runat="server"
                        Editable="false"
                        FieldLabel="Region"
                        DisplayField="name"
                        ValueField="name"
                        QueryMode="Local"
                        EmptyText="Select a region" meta:resourcekey="cboRegionResource1">
                        <Store>
                            <ext:Store runat="server" meta:resourcekey="regionStoreResource1">
                                <Model>
                                    <ext:Model ID="umRegionModel" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="iconCls" />
                                            <ext:ModelField Name="name" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ListConfig>
                            <ItemTpl ID="uxTemplate" runat="server">
                                <Html>
                                    <div class="icon-combo-item {iconCls}">
                            {name}
                        </div>
                                </Html>
                            </ItemTpl>
                        </ListConfig>
                        <Listeners>
                            <Select Handler="#{DirectMethods}.dmChangeRegion(#{uxRegion}.getValue(),this.valueModels[0].get('iconCls'));" />
                            <Change Handler="if(this.valueModels.length>0){this.setIconCls(this.valueModels[0].get('iconCls'));}" />
                        </Listeners>
                    </ext:ComboBox>
                </Items>
                <Buttons>
                    <ext:Button ID="uxLoginButton" runat="server" Text="Login" meta:resourcekey="loginButtonResource1">
                        <DirectEvents>
                            <Click OnEvent="deLogin">
                                <EventMask ShowMask="true" Msg=""></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>

    </ext:Viewport>
</body>
</html>

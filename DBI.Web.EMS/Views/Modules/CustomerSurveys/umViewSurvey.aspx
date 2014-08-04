﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umViewSurvey.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umViewSurvey" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .test {
            background-color: grey;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="false" />
        <ext:Viewport ID="Viewport1" runat="server" Cls="test" >
            <Items>
                <ext:Container runat="server" AutoScroll="true" ID="uxSurveyContainer" Hidden="true">
                    <Items>
                        <ext:FormPanel runat="server" ID="uxSurveyDisplay" Layout="FormLayout" MaxWidth="1000" BodyPadding="10" Flex="500" ManageHeight="true">
                            <Items>
                                <ext:Image ID="Image2" runat="server" ImageUrl="/Resources/Images/dbis_black_logo.png" MaxWidth="250" MaxHeight="154" StyleSpec="text-align: center" />
                                <ext:FieldSet runat="server" Title="Form Code" Margin="5">
                                    <Items>
                                        <ext:TextField runat="server" ID="uxFormCode" FieldLabel="Enter Form Code" AllowBlank="false" LabelWidth="150" />
                                    </Items>
                                </ext:FieldSet>
                            </Items>
                            <Buttons>
                                <ext:Button runat="server" ID="uxSubmitSurveyButton" Text="Submit" Icon="Add" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deSaveSurvey" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxCancelSurveyButton" Text="Cancel" Icon="Delete">
                                    <Listeners>
                                        <Click Handler="#{uxSurveyDisplay}.reset()" />
                                    </Listeners>
                                </ext:Button>
                            </Buttons>
                            <Listeners>
                                <ValidityChange Handler="#{uxSubmitSurveyButton}.setDisabled(!valid)" />
                            </Listeners>
                        </ext:FormPanel>
                    </Items>
                    <LayoutConfig>
                        <ext:HBoxLayoutConfig Pack="Center" ReserveScrollbar="true" />
                    </LayoutConfig>
                </ext:Container>
            </Items>
            <LayoutConfig>
                <ext:FitLayoutConfig ReserveScrollbar="true" />
            </LayoutConfig>
        </ext:Viewport>
    </form>
</body>
</html>

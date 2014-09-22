<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umViewSurvey.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umViewSurvey" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
   
    <style type="text/css">
        .test {
            background-color: grey;
        }
        .allowBlank-field {
            background-color: #EFF7FF !important;
            background-image: none;
        }
        .testing {
            background-color: #EFF7FF !important;
            background-image: none;
        }
    </style>
</head>
<body style="overflow:visible">
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="false" />
        <ext:Viewport ID="Viewport1" runat="server" Cls="test" Layout="FitLayout" >
            <Items>
                <ext:Container runat="server" AutoScroll="true" ID="uxSurveyContainer" Hidden="true">
                    <Items>
                        <ext:Container runat="server" Width="1000" StyleSpec="background-color: white" Padding="10" Border="false" Layout="FormLayout" ID="uxLogoContainer">
                            <Items>
                                <ext:Image ID="uxLogoImage" runat="server" ImageUrl="/Resources/Images/dbis_black_logo.png" Width="250" MaxWidth="250" MaxHeight="154" StyleSpec="text-align: center" />
                            </Items>
                        </ext:Container>
                        <ext:FormPanel runat="server" ID="uxSurveyDisplay" Layout="VBoxLayout" MaxWidth="1000" BodyPadding="10" Flex="500" ManageHeight="true" Border="false" >
                            <Items>
                                <ext:FieldSet runat="server" Title="Form Code" Margin="5" ID="uxCodeFieldset">
                                    <Items>
                                        <ext:TextField runat="server" ID="uxFormCode" FieldLabel="Enter Form Code" AllowBlank="false" InvalidCls="allowBlank" LabelWidth="150" MsgTarget="Side" IndicatorIcon="BulletRed" >
                                            <DirectEvents>
                                                <Blur OnEvent="deLoadCustomer" />
                                            </DirectEvents>
                                        </ext:TextField>
                                        <ext:DisplayField runat="server" ID="uxCustomerField" />
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
                            <LayoutConfig>
                                <ext:VBoxLayoutConfig Align="Stretch" />
                            </LayoutConfig>
                        </ext:FormPanel>
                    </Items>
                </ext:Container>
            </Items>
            <LayoutConfig>
                <ext:FitLayoutConfig ReserveScrollbar="true" />
            </LayoutConfig>
        </ext:Viewport>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umViewSurvey.aspx.cs" Inherits="DBI.Web.EMS.PublicPages.umViewSurvey" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .test {
            background-color: grey;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" IsDynamic="false" />
        <ext:Viewport runat="server" Cls="test" >
            <Items>
                <ext:Container ID="uxCompletedContainer" runat="server" AutoScroll="true" Hidden="true">
                    <Items>
                        <ext:Panel runat="server" ID="uxCompletedPanel" AutoScroll="true" MaxWidth="1000" BodyPadding="10" Flex="500" Height="1000" Layout="HBoxLayout">
                            <Items>
                                <ext:Container runat="server" Flex="1" />
                                <ext:Image ID="Image1" runat="server" ImageUrl="/Resources/Images/dbis_black_logo.png" MaxWidth="250" MaxHeight="154" StyleSpec="text-align: center" Flex="500" />                       
                                <ext:Container runat="server" Flex="1" />
                            </Items>
                            <Content>
                                <br /><br /> <br /> <br /><p style="text-align: center">Your form has been submitted successfully.  Thank you for your time.</p>
                            </Content>
                            
                        </ext:Panel>
                    </Items>
                    <LayoutConfig>
                        <ext:HBoxLayoutConfig Pack="Center" ReserveScrollbar="true" />
                    </LayoutConfig>
                </ext:Container>
                <ext:Container runat="server" AutoScroll="true" ID="uxSurveyContainer" Hidden="true">
                    <Items>
                        <ext:FormPanel runat="server" ID="uxSurveyDisplay" Layout="FormLayout" MaxWidth="1000" BodyPadding="10" Flex="500" ManageHeight="true">
                            <Items>
                                <ext:Image ID="Image2" runat="server" ImageUrl="/Resources/Images/dbis_black_logo.png" MaxWidth="250" MaxHeight="154" StyleSpec="text-align: center" />                       
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

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
        <ext:Viewport runat="server" Layout="FitLayout" Cls="test" >
            <Items>
                <ext:Container runat="server">
                    <Items>
                        <ext:FormPanel runat="server" ID="uxSurveyDisplay" Layout="FormLayout" MaxWidth="1000" BodyPadding="10" Flex="500" AutoScroll="true" ManageHeight="true">
                    <LayoutConfig>
                        <ext:FormLayoutConfig ReserveScrollbar="true" />
                    </LayoutConfig>
                    <Items>
                        <ext:Image ID="Image2" runat="server" ImageUrl="/Resources/Images/dbis_black_logo.png" MaxWidth="250" MaxHeight="154" StyleSpec="text-align: center" />
                        <ext:FieldSet ID="FieldSet1" runat="server" Title="Form Code Entry" Margin="5">
                            <Items>
                                <ext:TextField runat="server" ID="uxFormCode" FieldLabel="Form Entry Code" LabelWidth="150" />
                            </Items>
                        </ext:FieldSet>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxSubmitSurveyButton" Text="Submit" Icon="Add" Disabled="true">
                        </ext:Button>
                        <ext:Button runat="server" ID="uxCancelSurveyButton" Text="Cancel" Icon="Delete">
                            
                        </ext:Button>
                    </Buttons>
                    <Listeners>
                        <ValidityChange Handler="#{uxSubmitSurveyButton}.setDisabled(!valid)" />
                    </Listeners>
                </ext:FormPanel>
                    </Items>
                    <LayoutConfig>
                        <ext:HBoxLayoutConfig Pack="Center" />
                    </LayoutConfig>
                </ext:Container>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

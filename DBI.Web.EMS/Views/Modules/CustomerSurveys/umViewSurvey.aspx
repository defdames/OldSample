﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umViewSurvey.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umViewSurvey" %>

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
        <ext:Viewport runat="server" Layout="HBoxLayout" Cls="test">
            <Items>
                <ext:Panel Flex="1" runat="server" BodyStyle="background-color: grey" />
                <ext:FormPanel runat="server" ID="uxSurveyDisplay" MaxWidth="1000" Layout="FormLayout" BodyPadding="10" Flex="500">
                    <Items>
                        <ext:Image ID="Image2" runat="server" ImageUrl="/Resources/Images/dbis_black_logo.png" MaxWidth="250" MaxHeight="154" StyleSpec="text-align: center" />
                        <ext:FieldSet runat="server" Title="Form Code Entry" Margin="5">
                            <Items>
                                <ext:TextField runat="server" ID="uxFormCode" FieldLabel="Form Entry Code" LabelWidth="150" />
                            </Items>
                        </ext:FieldSet>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxSubmitSurveyButton" Text="Submit" Icon="Add" Disabled="true">
                        </ext:Button>
                        <ext:Button runat="server" ID="uxCancelSurveyButton" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="parentAutoLoadControl.close()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <Listeners>
                        <%--<AfterRender
                            Handler="var win = parentAutoLoadControl.target || parentAutoLoadControl, //you can use just 'parentAutoLoadControl' after update to Ext.NET v2 beta.
									size = this.getSize();
 
								size.height += 34;
								size.width += 12;
								win.setSize(size);"
                            Delay="100" />--%>
                        <ValidityChange Handler="#{uxSubmitSurveyButton}.setDisabled(!valid)" />
                    </Listeners>
                </ext:FormPanel>
                <ext:Panel runat="server" Flex="1" />
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

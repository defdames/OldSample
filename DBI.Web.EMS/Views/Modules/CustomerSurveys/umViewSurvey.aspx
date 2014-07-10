<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umViewSurvey.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umViewSurvey" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" IsDynamic="false" />
        <ext:Viewport runat="server">
            <Items>
                <ext:FormPanel runat="server" ID="uxSurveyDisplay" Width="1000" Layout="FormLayout" BodyPadding="10">
                    <Items>
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
                        <AfterRender
                            Handler="var win = parentAutoLoadControl.target || parentAutoLoadControl, //you can use just 'parentAutoLoadControl' after update to Ext.NET v2 beta.
									size = this.getSize();
 
								size.height += 34;
								size.width += 12;
								win.setSize(size);"
                            Delay="100" />
                        <ValidityChange Handler="#{uxSubmitSurveyButton}.setDisabled(!valid)" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

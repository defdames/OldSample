<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umDeleteTime.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.TimeClock.Edit.umDeleteTime" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Panel runat="server">
            <Items>
                <ext:FormPanel ID="frmDelComment" runat="server" ButtonAlign="Left" Title="Delete Time" BodyPadding="5" Region="North" Layout="FormLayout">
                    <Items>
                        <ext:TextArea ID="txtDelComment" runat="server" AllowBlank="false" FieldLabel="Comment">

                        </ext:TextArea>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxDeleteButton" Text="Delete" Disabled="true">
							<DirectEvents>
								<Click OnEvent="deDeleteTime" Success="parent.Ext.getCmp('uxDeleteTime').close();"></Click>
							</DirectEvents>
						</ext:Button>
                    </Buttons>
                    <Listeners>
                        <ValidityChange Handler="#{uxDeleteButton}.setDisabled(!valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Panel>
    </form>
</body>
</html>

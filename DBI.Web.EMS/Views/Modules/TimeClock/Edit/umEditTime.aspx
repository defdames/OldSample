<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEditTime.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.TimeClock.Edit.umEditTime" %>

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
                <ext:FormPanel ID="frmPanelIn" runat="server" ButtonAlign="Left" Title="Edit Time In" BodyPadding="5" Region="North" Layout="FormLayout">
                    <Items>
                        <ext:DateField ID="uxDateInField" runat="server" FieldLabel="Date In"></ext:DateField>
                
                        <ext:TimeField ID="uxTimeInField" runat="server" FieldLabel="Time In"></ext:TimeField>
                    </Items>
                </ext:FormPanel>
                 <ext:FormPanel ID="frmPanelOut" runat="server" Title="Edit Time Out" BodyPadding="5" Region="North" Layout="FormLayout">
                    <Items>
                        <ext:DateField ID="uxDateOutField" runat="server" FieldLabel="Date Out"></ext:DateField>
                
                        <ext:TimeField ID="uxTimeOutField" runat="server" FieldLabel="Time Out"></ext:TimeField>
                    </Items>
                        <Buttons>
						    <ext:Button runat="server" ID="uxEditButton" Text="Save">
								<DirectEvents>
									<Click OnEvent="deEditTime" Success="parent.Ext.getCmp('uxAddEditTime').close();"></Click>
								</DirectEvents>
							</ext:Button>
						</Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Panel>
          


    </form>
</body>
</html>

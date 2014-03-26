<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umChooseLunchHeader.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umChooseLunchHeader" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
        <ext:Panel runat="server" Layout="FitLayout" AutoScroll="true">
            <Items>
                <ext:FormPanel runat="server" ID="uxChooseLunchForm" Layout="FormLayout">
                    <Buttons>
                        <ext:Button runat="server" Text="Submit">
                            <DirectEvents>
                                <Click OnEvent="deStoreLunchChoice" />
                            </DirectEvents>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
            <Listeners>
				<AfterRender
					Handler="var win = parentAutoLoadControl.target || parentAutoLoadControl, //you can use just 'parentAutoLoadControl' after update to Ext.NET v2 beta.
									size = this.getSize();
 
								size.height += 250;
								size.width += 12;
								win.setSize(size);"
					Delay="100" />
			</Listeners>
        </ext:Panel>
    </form>
</body>
</html>

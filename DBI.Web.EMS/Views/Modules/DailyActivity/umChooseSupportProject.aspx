<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umChooseSupportProject.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umChooseSupportProject" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="FitLayout">
            <Items>
                <ext:FormPanel runat="server" ID="uxChoosePerDiemFormPanel" Layout="FormLayout" Region="Center">
                    <Items>
                        <ext:ComboBox runat="server" ID="uxChoosePerDiemHeaderId" DisplayField="LONG_NAME" ValueField="HEADER_ID" FieldLabel="Choose Project for Per Diem">
                            <Store>
                                <ext:Store runat="server" ID="uxChoosePerDiemHeaderIdStore">
                                    <Model>
                                        <ext:Model ID="Model1" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="HEADER_ID" />
                                                <ext:ModelField Name="LONG_NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                        </ext:ComboBox>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" Id="uxChoosePerDiemSubmitButton" Text="Submit">
                            <DirectEvents>
                                <Click OnEvent="deUpdatePerDiem" />
                            </DirectEvents>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

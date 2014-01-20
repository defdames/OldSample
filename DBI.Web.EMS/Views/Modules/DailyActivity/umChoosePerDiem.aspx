<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umChoosePerDiem.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umChoosePerDiem" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
        <ext:FormPanel runat="server" ID="uxChoosePerDiemFormPanel" Layout="FormLayout">
            <Items>
                <ext:ComboBox runat="server" ID="uxChoosePerDiemHeaderId" DisplayField="LONG_NAME" ValueField="HEADER_ID" FieldLabel="Choose Project for Per Diem">
                    <Store>
                        <ext:Store runat="server" ID="uxChoosePerDiemHeaderIdStore">
                            <Model>
                                <ext:Model runat="server">
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
    </form>
</body>
</html>

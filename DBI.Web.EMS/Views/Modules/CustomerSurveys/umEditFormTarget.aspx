<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEditFormTarget.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umEditFormTarget" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../../Resources/StyleSheets/main.css" />
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager runat="server" IsDynamic="false" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel runat="server" ID="uxTargetForm" Region="Center">
                    <Items>
                        <ext:TextField runat="server" ID="uxTargetName" FieldLabel="Target Name" />
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxSaveTargetButton" Text="Save" Icon="Add">
                            <DirectEvents>
                                <Click OnEvent="deSaveTarget" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="uxCancelButton" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="parentAutoLoadControl.close();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umUpdateAllActuals.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.umUpdateAllActuals" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var closeCancel = function () {
            parent.Ext.getCmp('uxUpdateAllActualsForm').close();
        }
        var closeUpdate = function () {
            parent.App.direct.CloseUpdateAllActualsWindow();
            //parent.App.uxSummaryGridStore.reload();
            parent.Ext.getCmp('uxUpdateAllActualsForm').close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>

                <ext:FormPanel ID="FormPanel1"
                    runat="server"
                    Region="Center"
                    BodyPadding="20"
                    Disabled="false">
                    <Items>

                        <ext:FieldContainer ID="FieldContainer1"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label2" runat="server" Width="100" Text="Please select:" />
                                <ext:ComboBox ID="uxWEDate"
                                    runat="server"
                                    ValueField="ID_NAME"
                                    DisplayField="ID_NAME"
                                    Width="110"
                                    EmptyText="-- Select --"
                                    Editable="false">
                                    <Store>
                                        <ext:Store ID="uxWEDateStore" runat="server" OnReadData="deLoadWEDateDropdown" AutoLoad="false">
                                            <Model>
                                                <ext:Model ID="Model5" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="ID_NAME" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                        </ext:Store>
                                    </Store>
                                    <DirectEvents>
                                        <Change OnEvent="deCheckAllowUpdate" />
                                    </DirectEvents>
                                </ext:ComboBox>
                                <ext:Label ID="Label1" runat="server" Width="150" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer2"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label4" runat="server" Width="360" Height="90" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer17"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label3" runat="server" Width="185" />
                                <ext:Button ID="uxUpdate" runat="server" Text="Update" Icon="Add" Width="75" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deUpdate">
                                            <EventMask ShowMask="true" Msg="Updating..." />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Label ID="Label16" runat="server" Width="5" />
                                <ext:Button ID="uxCancel" runat="server" Text="Cancel" Icon="Delete" Width="75">
                                    <Listeners>
                                        <Click Fn="closeCancel" />
                                    </Listeners>
                                    <DirectEvents>
                                        <Click>
                                            <EventMask ShowMask="true" Msg="Canceling..." />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                </ext:FormPanel>

            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

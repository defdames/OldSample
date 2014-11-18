<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umReportCompareTo.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.umReportCompareTo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var closeCancel = function () {
            parent.Ext.getCmp('uxReport').close();
        }
        var closeCompare = function () {
            parent.App.direct.LoadOHCompareReport(App.uxHidReport.getValue(), App.uxYear.getValue(), App.uxVersion.getValue());
            //parent.App.uxSummaryGridStore.reload();
            parent.Ext.getCmp('uxReport').close();
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
                                <ext:Label ID="Label2" runat="server" Width="100" Text="Year:" />
                                <ext:ComboBox ID="uxYear"
                                    runat="server"
                                    ValueField="ID_NAME"
                                    DisplayField="ID_NAME"
                                    QueryMode="Local"
                                    Width="120"
                                    EmptyText="-- Select --"
                                    Editable="false">
                                    <Store>
                                        <ext:Store ID="uxYearStore" runat="server" AutoDataBind="true">
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
                                        <Change OnEvent="deCheckAllowRun" />
                                    </DirectEvents>
                                </ext:ComboBox>
                                <ext:Label ID="Label1" runat="server" Width="140" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer4"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label7" runat="server" Width="360" Height="10" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer3"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label5" runat="server" Width="100" Text="Version:" />
                                <ext:ComboBox ID="uxVersion"
                                    runat="server"
                                    ValueField="ID"
                                    DisplayField="ID_NAME"
                                    QueryMode="Local"
                                    Width="120"
                                    EmptyText="-- Select --"
                                    Editable="false">
                                    <Store>
                                        <ext:Store ID="uxVersionStore" runat="server" AutoDataBind="true">
                                            <Model>
                                                <ext:Model ID="Model1" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
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
                                        <Change OnEvent="deCheckAllowRun" />
                                    </DirectEvents>
                                </ext:ComboBox>
                                <ext:Label ID="Label6" runat="server" Width="140" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer2"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label4" runat="server" Width="360" Height="50" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer17"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label3" runat="server" Width="185" />
                                <ext:Button ID="uxRun" runat="server" Text="Run" Icon="Add" Width="75" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deRun">
                                            <EventMask ShowMask="true" Msg="Processing..." />
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

                <ext:Hidden ID="uxHidReport" runat="server" />

            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

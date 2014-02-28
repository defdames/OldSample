<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umStateCrossingsList.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umStateCrossingsList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div></div>
        <ext:ResourceManager ID="ResourceManager1" runat="server" />





        <ext:Toolbar ID="Toolbar1" runat="server">
            <Items>
                <ext:Button ID="Button1"
                    runat="server"
                    Text="Print"
                    Icon="Printer"
                    OnClientClick="window.print();" />
                <ext:Button runat="server"
                    ID="uxExportToPDF"
                    Text="Export to PDF"
                    Icon="PageWhiteAcrobat" />

            </Items>
        </ext:Toolbar>
        <ext:FormPanel runat="server" ID="uxCrossingForm" Layout="FormLayout">
            <Items>

                <ext:FieldSet ID="FieldSet1" runat="server" Title="State Crossing List Filter">
                    <Items>
                        <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                            <Items>
                                <ext:ComboBox ID="uxCrossingNumCI" runat="server" FieldLabel="Crossing #" LabelAlign="Right" AnchorHorizontal="100%" DisableKeyFilter="False" Width="350" />
                                <ext:ComboBox ID="TextField1" runat="server" FieldLabel="Sub-Division" LabelAlign="Right" AnchorHorizontal="100%" Width="350" />
                                <ext:ComboBox ID="TextField2" runat="server" FieldLabel="Manager" LabelAlign="Right" AnchorHorizontal="100%" Width="350" />
                            </Items>
                        </ext:FieldContainer>
                         <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                            <Items>
                                <ext:ComboBox ID="uxStateCI" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right" Width="350"/>
                               
                            </Items>
                        </ext:FieldContainer>
                         <ext:FieldContainer ID="FieldContainer3" runat="server" Layout="HBoxLayout">
                            <Items>
                        
                            </Items>
                        </ext:FieldContainer>
                         <ext:FieldContainer ID="FieldContainer4" runat="server" Layout="HBoxLayout">
                            <Items>
                            
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                </ext:FieldSet>

               
            </Items>
        </ext:FormPanel>



    </form>
</body>
</html>

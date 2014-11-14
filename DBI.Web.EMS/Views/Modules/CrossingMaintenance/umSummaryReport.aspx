<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSummaryReport.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umSummaryReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <ext:ResourceManager ID="ResourceManager1" runat="server" />
         <ext:Hidden ID="Hidden1" runat="server" Hidden="true" />
         <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
                <Items>
         <ext:FormPanel ID="uxFilterForm" runat="server" Margin="5" Region="North" Title="Filter Supplemental Billing Report">
                <Items>
                    <ext:FieldSet ID="FieldSet1" runat="server" Title="Filter">
                        <Items>
                            <ext:TextField ID="uxRRCI" runat="server" FieldLabel="Railroad" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" Hidden="true" />

                           <ext:DateField ID="uxStartDate" runat="server" AnchorHorizontal="25%" FieldLabel="Start Date" LabelAlign="Right" Editable="false" TabIndex="2" EmptyText="ALL" >
                                <Plugins>
                                <ext:ClearButton ID="ClearButton1" runat="server" />
                            </Plugins>
                               </ext:DateField>
                            <ext:DateField ID="uxEndDate" runat="server" AnchorHorizontal="25%" FieldLabel="End Date" LabelAlign="Right"  Editable="false" TabIndex="3" EmptyText="ALL" >
                                 <Plugins>
                                <ext:ClearButton ID="ClearButton2" runat="server" />
                            </Plugins>
                                </ext:DateField>
                        </Items>
                    </ext:FieldSet>
                </Items>
                <BottomBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Button runat="server"
                                ID="Button4"
                                Text="Run"
                                Icon="PlayGreen">
                                   <DirectEvents>
                                <Click OnEvent="deSupplementalReportGrid" >
                                    </Click>
                            </DirectEvents>
                        
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="Button3"
                                Text="Cancel"
                                Icon="StopRed">
                             <DirectEvents>
                            <Click OnEvent="deClearFilters" />
                            </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </BottomBar>
            </ext:FormPanel>
            <ext:Panel runat="server" ID="uxCenterPanel" Region="Center">
              <LayoutConfig>
                <ext:FitLayoutConfig />
              </LayoutConfig>
                    <Items>
                        <ext:Panel ID="Panel" runat="server" ManageHeight="true">
                        </ext:Panel>
                    </Items>
                </ext:Panel>
       
       
                    </Items>
             </ext:Viewport>
    </div>
    </form>
</body>
</html>

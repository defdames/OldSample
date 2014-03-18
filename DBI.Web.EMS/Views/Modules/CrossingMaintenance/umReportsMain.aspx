<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umReportsMain.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umReportsMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <ext:ResourceManager ID="ResourceManager2" runat="server" />

        <div>
            <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
                <Items>

                    <ext:TabPanel ID="uxReportsTab" runat="server" Region="Center">
                        <Items>
                            <ext:Panel runat="server"
                                Title="State Crossing List"
                                ID="uxStateCrossingsList"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader5" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umStateCrossingsList.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>

                            </ext:Panel>

                            <ext:Panel runat="server"
                                Title="Application Date"
                                ID="uxAppDate"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="uxHeaderLoader" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umAppDate.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>

                            </ext:Panel>
                             <ext:Panel runat="server"
                                Title="Incidents"
                                ID="uxIncidents"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader1" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umIncidentReports.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>

                            </ext:Panel>
                           

                        </Items>
                    </ext:TabPanel>

                </Items>
            </ext:Viewport>
        </div>
    </div>
    </form>
</body>
</html>

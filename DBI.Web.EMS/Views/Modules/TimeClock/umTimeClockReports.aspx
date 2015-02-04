<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umTimeClockReports.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.TimeClock.umTimeClockReports" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div></div>
        <ext:ResourceManager ID="resourcemanager" runat="server"></ext:ResourceManager>
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
            <ext:TabPanel ID="uxCrossingTab" runat="server" Region="Center">
                        <Items>
                            <ext:Panel runat="server"
                                Title="Unapproved Hours"
                                ID="uxUnapprovedHours"
                                Disabled="false">                                                              
                                <Loader runat="server"
                                    ID="Loader2" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umUnapprovedHoursReport.aspx">
                                    <LoadMask ShowMask="true" />                                
                                </Loader>
                            </ext:Panel>
                            <%--<ext:Panel runat="server"
                                Title="Actual vs Adjusted"
                                ID="uxAcutalVerseAdjusted"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader5" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umAdjustedverseActualHoursreport.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>
                            </ext:Panel>--%>

                            <ext:Panel runat="server"
                                Title="12 Hour Day"
                                ID="ux12HourDay"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="uxHeaderLoader" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="um12HourDayReport.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>
                            </ext:Panel>
                            <%--<ext:Panel runat="server"
                                Title="No Lunch"
                                ID="uxNoLunch"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader1" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umNoLunchReport.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>
                            </ext:Panel>--%>
                        </Items>
                    </ext:TabPanel>
                </Items>

        </ext:Viewport>
    </form>
</body>
</html>

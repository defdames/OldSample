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
                            <ext:Panel runat="server"
                                Title="Crossing Security"
                                ID="uxCrossingSecurity"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader5" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umCrossingSecurity.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>
                                 <Listeners>
                                 <BeforeActivate Handler="#{Toolbar1}.show()" />                            
                                </Listeners>
                            </ext:Panel>

                            <ext:Panel runat="server"
                                Title="Crossing Information"
                                ID="uxCrossingInfoTab"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="uxHeaderLoader" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umCrossingInfoTab.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>
                                 <Listeners>
                                 <BeforeActivate Handler="#{Toolbar1}.show()" />                            
                                </Listeners>
                            </ext:Panel>
                            <ext:Panel runat="server"
                                Title="Contacts"
                                ID="uxContactsTab"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader1" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umContactsTab.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>
                                 <Listeners>
                                 <BeforeActivate Handler="#{Toolbar1}.show()" />                            
                                </Listeners>
                            </ext:Panel>
                             <ext:Panel runat="server"
                                Title="Manage KCS"
                                ID="uxManageKCS"
                                Disabled="true" >
                                <Loader runat="server"
                                    ID="Loader3" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="ManageKCS.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>
                                 <Listeners>
                                 <BeforeActivate Handler="#{Toolbar1}.show()" />                            
                                </Listeners>
                            </ext:Panel>



                        </Items>
                    </ext:TabPanel>
                </Items>

        </ext:Viewport>
    </form>
</body>
</html>

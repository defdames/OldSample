<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCrossingMain.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umCrossingMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div></div>
        <ext:ResourceManager ID="ResourceManager2" runat="server" />

        <div>
            <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
                <Items>

                    <ext:TabPanel ID="uxCrossingTab" runat="server" Region="Center">
                        <Items>
                            <ext:Panel runat="server"
                                Title="Crossing Security"
                                ID="uxCrossingSecurity"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader5" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umCrossingSecurity.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>

                            </ext:Panel>

                            <ext:Panel runat="server"
                                Title="Crossing Information"
                                ID="uxCrossingInfoTab"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="uxHeaderLoader" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umCrossingInfoTab.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>

                            </ext:Panel>
                            <ext:Panel runat="server"
                                Title="Contacts"
                                ID="uxContactsTab"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader1" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umContactsTab.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>

                            </ext:Panel>
                           
                            <ext:Panel runat="server"
                                Title="Data Entry"
                                ID="uxDataEntryTab"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader4" Mode="Frame" AutoLoad="false" ReloadOnEvent="false" Url="umDataEntryTab.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>

                            </ext:Panel>

                        </Items>
                    </ext:TabPanel>

                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>

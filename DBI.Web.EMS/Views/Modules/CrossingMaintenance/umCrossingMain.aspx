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

                    <ext:GridPanel ID="uxCrossingMainGrid" runat="server" Region="North" Layout="HBoxLayout" Collapsible="true">
                        <Store>
                            <ext:Store runat="server"
                                ID="Store1"
                                AutoDataBind="true" WarningOnDirty="false">
                                <Model>
                                    <ext:Model ID="Model2" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="" />
                                            <ext:ModelField Name="" />
                                            <ext:ModelField Name="" />
                                        </Fields>
                                    </ext:Model>
                                </Model>

                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:Column ID="uxProjectName" runat="server" DataIndex="" Text="Project Name" Flex="1" />
                                <ext:Column ID="uxMainCrossingNum" runat="server" DataIndex="" Text="Crossing #" Flex="1" />
                                <ext:Column ID="uxMainState" runat="server" DataIndex="" Text="State" Flex="1" />

                            </Columns>
                        </ColumnModel>
                        <TopBar>

                            <ext:Toolbar ID="Toolbar3" runat="server">
                                <Items>
                                    <ext:Button ID="uxAddProjectButton" runat="server" Text="Add Project" Icon="ApplicationAdd" />
                                    <ext:Button ID="uxEditProjectButton" runat="server" Text="Edit Project" Icon="ApplicationEdit" />

                                </Items>

                            </ext:Toolbar>
                        </TopBar>
                        <Plugins>
                            <ext:FilterHeader ID="FilterHeader1" runat="server" />
                        </Plugins>
                        <BottomBar>
                            <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True">
                            </ext:PagingToolbar>
                        </BottomBar>

                    </ext:GridPanel>

                    <ext:TabPanel ID="uxCrossingTab" runat="server" Region="Center">
                        <Items>
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
                                Title="Sub-Divisions"
                                ID="uxSubDivisionsTab"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader2" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umSubDivisionsTab.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>

                            </ext:Panel>
                            <ext:Panel runat="server"
                                Title="Service Units"
                                ID="uxSerivceUnitsTab"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader3" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umServiceUnitsTab.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>

                            </ext:Panel>
                            <ext:Panel runat="server"
                                Title="Data Entry"
                                ID="uxDataEntryTab"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader4" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umDataEntryTab.aspx">
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

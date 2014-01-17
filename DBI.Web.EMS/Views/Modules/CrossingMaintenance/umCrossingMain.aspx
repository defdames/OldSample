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

                   <%-- <ext:GridPanel ID="uxCrossingMainGrid" runat="server" Region="North" Layout="HBoxLayout" Collapsible="true">
                         <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="true" Mode="Single" />
                          </SelectionModel>
                        <Store>
                            <ext:Store runat="server"
                                ID="uxCurrentCrossingStore"
                                OnReadData="deMainGridData"
                                PageSize="10"
                                AutoDataBind="true" WarningOnDirty="false">
                                <Model>
                                    <ext:Model ID="Model2" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="CROSSING_ID" />
                                            <ext:ModelField Name="CROSSING_NUMBER" Type="String" />
                                             <ext:ModelField Name="" />
                                            <ext:ModelField Name="" />
                                            <ext:ModelField Name="" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                                <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>

                                <ext:Column ID="uxMainCrossingNum" runat="server" DataIndex="CROSSING_NUMBER" Text="Crossing #" Flex="1" />
                                <ext:Column ID="uxProjectName" runat="server" DataIndex="" Text="Project Name" Flex="1" />
                                <ext:Column ID="uxProjectNum" runat="server" DataIndex="" Text="Project #" Flex="1" />
                                <ext:Column ID="uxCrossingManager" runat="server" DataIndex="" Text="Manager" Flex="1" />

                            </Columns>
                        </ColumnModel>
                      
                        <Plugins>
                            <ext:FilterHeader ID="FilterHeader1" runat="server" />
                        </Plugins>
                         <DirectEvents>
                        <Select OnEvent="deUpdateUrlAndButtons">
                            <ExtraParams>
                                <ext:Parameter Name="CrossingId" Value="#{uxCrossingMainGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                            </ExtraParams>
                        </Select>                  
                    </DirectEvents>
                        <BottomBar>
                            <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True">
                            </ext:PagingToolbar>
                        </BottomBar>

                    </ext:GridPanel>--%>

                    <ext:TabPanel ID="uxCrossingTab" runat="server" Region="Center">
                        <Items>
                            <ext:Panel runat="server"
                                Title="Crossing Security"
                                ID="uxCrossingSecurity"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader5" Mode="Frame" AutoLoad="false" ReloadOnEvent="false" Url="umCrossingSecurity.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>

                            </ext:Panel>

                            <ext:Panel runat="server"
                                Title="Crossing Information"
                                ID="uxCrossingInfoTab"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="uxHeaderLoader" Mode="Frame" AutoLoad="true" ReloadOnEvent="false" Url="umCrossingInfoTab.aspx">
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
                                ID="uxServiceUnitsTab"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader3" Mode="Frame" AutoLoad="false" ReloadOnEvent="false" Url="umServiceUnitsTab.aspx">
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

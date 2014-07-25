﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSurveyDashboard.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umSurveyDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var percentage = function (value) {
            return value + "%";
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" IsDynamic="false" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:TreePanel
                    ID="uxOrgPanel"
                    runat="server"
                    Title="Organizations"
                    BodyPadding="6"
                    Region="West"
                    Weight="100"
                    Width="300"
                    AutoScroll="true"
                    RootVisible="true"
                    SingleExpand="true"
                    Lines="false"
                    UseArrows="true">
                    <Store>
                        <ext:TreeStore ID="TreeStore1" runat="server" OnReadData="deLoadOrgTree">
                            <Proxy>
                                <ext:PageProxy></ext:PageProxy>
                            </Proxy>
                        </ext:TreeStore>
                    </Store>
                    <Root>
                        <ext:Node NodeID="0" Text="All Companies" Expanded="true" />
                    </Root>
                    <SelectionModel>
                        <ext:TreeSelectionModel ID="uxCompanySelectionModel" runat="server" Mode="Single" />
                    </SelectionModel>
                    <Listeners>
                        <ItemClick Handler="#{uxDashboardStore}.reload()" />
                    </Listeners>
                </ext:TreePanel>
                <ext:GridPanel runat="server" ID="uxDashboardGrid" Layout="HBoxLayout" Region="Center">
                    <Store>
                        <ext:Store runat="server" ID="uxDashboardStore" AutoDataBind="true" OnReadData="deReadDashboard" PageSize="20">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="PROJECT_NUMBER" />
                                        <ext:ModelField Name="PROJECT_NAME" />
                                        <ext:ModelField Name="PERCENTAGE" />
                                        <ext:ModelField Name="THRESHOLD" />
                                        <ext:ModelField Name="THRESHOLD_ID" />
                                        <ext:ModelField Name="PROJECT_ID" />
                                        <ext:ModelField Name="ORG_ID" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Sorters>
                                <ext:DataSorter Property="PROJECT_NAME" Direction="DESC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" />
                    </SelectionModel>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="Project Number" DataIndex="PROJECT_NUMBER" Flex="20" />
                            <ext:Column runat="server" Text="Project Name" DataIndex="PROJECT_NAME" Flex="40" />
                            <ext:Column runat="server" Text="Current %" DataIndex="PERCENTAGE" Flex="30">
                                <Renderer Fn="percentage" />
                            </ext:Column>
                            <ext:Column runat="server" Text="Threshold %" DataIndex="THRESHOLD" Flex="30">
                                <Renderer Fn="percentage" />
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                    </Plugins>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxEmailSurveyButton" Text="Email Survey Link" Icon="EmailLink">
                                    <DirectEvents>
                                        <Click OnEvent="deEmailLink">
                                            <ExtraParams>
                                                <ext:Parameter Name="RowValues" Value="Ext.encode(#{uxDashboardGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxEmailPDFSurveyButton" Text="Email PDF Survey" Icon="EmailAttach">
                                    <DirectEvents>
                                        <Click OnEvent="deEmailPDF" IsUpload="true">
                                            <ExtraParams>
                                                <ext:Parameter Name="RowValues" Value="Ext.encode(#{uxDashboardGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxPrintPDFButton" Text="Print PDF" Icon="PageWhiteAcrobat">
                                    <DirectEvents>
                                        <Click OnEvent="dePrintPDF">
                                            <ExtraParams>
                                                <ext:Parameter Name="RowValues" Value="Ext.encode(#{uxDashboardGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <BottomBar>
                        <ext:PagingToolbar runat="server" />
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSurveyDashboard.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umSurveyDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
                        <ItemClick Handler="#{uxDollarStore}.reload(); #{uxAddDollarButton}.enable();" />
                    </Listeners>
                </ext:TreePanel>
                <ext:GridPanel runat="server" ID="uxDashboardGrid" Layout="HBoxLayout" Region="Center">
                    <Store>
                        <ext:Store runat="server" ID="uxDashboardStore" AutoDataBind="true" OnReadData="deReadDashboard" PageSize="20">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ProjectNumber" />
                                        <ext:ModelField Name="ProjectName" />
                                        <ext:ModelField Name="CurrentPercentage" />
                                        <ext:ModelField Name="ThresholdPercentage" />
                                        <ext:ModelField Name="Difference" />
                                        <ext:ModelField Name="Upcoming" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Sorters>
                                <ext:DataSorter Property="Difference" Direction="DESC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="Project Number" DataIndex="ProjectNumber" Flex="20" />
                            <ext:Column runat="server" Text="Project Name" DataIndex="ProjectName" Flex="40" />
                            <ext:Column runat="server" Text="Current %" DataIndex="CurrentPercentage" Flex="30" />
                            <ext:Column runat="server" Text="Threshold %" DataIndex="ThresholdPercentage" Flex="30" />
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
                                                <ext:Parameter Name="PROJECT_ID" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxEmailPDFSurveyButton" Text="Email PDF Survey" Icon="EmailAttach">
                                    <DirectEvents>
                                        <Click OnEvent="deEmailPDF">
                                            <ExtraParams>
                                                <ext:Parameter Name="PROJECT_ID" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxPrintPDFButton" Text="Print PDF" Icon="PageWhiteAcrobat">
                                    <DirectEvents>
                                        <Click OnEvent="dePrintPDF">
                                            <ExtraParams>
                                                <ext:Parameter Name="PROJECT_ID" />
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

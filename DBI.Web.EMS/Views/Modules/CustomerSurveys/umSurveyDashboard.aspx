<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSurveyDashboard.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umSurveyDashboard" %>

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
                <ext:TabPanel runat="server" ID="uxTabPanel" Region="Center">
                    <Items>
                        <ext:GridPanel runat="server" ID="uxDashboardGrid" Layout="FitLayout" Title="Dashboard">
                            <Store>
                                <ext:Store runat="server" ID="uxDashboardStore" AutoDataBind="true" OnReadData="deReadDashboard" PageSize="20">
                                    <Model>
                                        <ext:Model ID="Model1" runat="server">
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
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" />
                            </SelectionModel>
                            <ColumnModel>
                                <Columns>
                                    <ext:Column ID="Column1" runat="server" Text="Project Number" DataIndex="PROJECT_NUMBER" Flex="20" />
                                    <ext:Column ID="Column2" runat="server" Text="Project Name" DataIndex="PROJECT_NAME" Flex="40" />
                                    <ext:Column ID="Column3" runat="server" Text="Current %" DataIndex="PERCENTAGE" Flex="30">
                                        <Renderer Fn="percentage" />
                                    </ext:Column>
                                    <ext:Column ID="Column4" runat="server" Text="Threshold %" DataIndex="THRESHOLD" Flex="30">
                                        <Renderer Fn="percentage" />
                                    </ext:Column>
                                </Columns>
                            </ColumnModel>
                            <Plugins>
                                <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
                            </Plugins>
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server">
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
                                                <Click OnEvent="dePrintPDF" IsUpload="true">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="RowValues" Value="Ext.encode(#{uxDashboardGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                                                    </ExtraParams>
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button runat="server" ID="uxManualEntryButton" Text="Manual Survey Entry" Icon="PencilAdd">
                                            <Listeners>
                                                <Click Handler="#{uxChooseProjectWindow}.show()" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
                            </BottomBar>
                        </ext:GridPanel>
                    </Items>
                </ext:TabPanel>

            </Items>
        </ext:Viewport>
        <ext:Window runat="server" Hidden="true" ID="uxChooseProjectWindow" Width="650">
            <Items>
                <ext:GridPanel runat="server" ID="uxProjectsGrid" Layout="FitLayout" Title="Choose a Project">
                    <Store>
                        <ext:Store runat="server" ID="uxProjectsStore" AutoDataBind="true" OnReadData="deReadProjects" PageSize="10">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="PROJECT_ID" />
                                        <ext:ModelField Name="CARRYING_OUT_ORGANIZATION_ID" />
                                        <ext:ModelField Name="SEGMENT1" />
                                        <ext:ModelField Name="LONG_NAME" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Sorters>
                                <ext:DataSorter Property="SEGMENT1" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="Project Number" DataIndex="SEGMENT1" Flex="2" />
                            <ext:Column runat="server" Text="Name" DataIndex="LONG_NAME" Flex="8" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                    </Plugins>
                    <Buttons>
                        <ext:Button runat="server" ID="uxChooseProjectButton" Icon="Add" Text="Submit">
                            <DirectEvents>
                                <Click OnEvent="deLoadPanel">
                                    <ExtraParams>
                                        <ext:Parameter Name="OrgId" Value="#{uxProjectsGrid}.getSelectionModel().getSelection()[0].data.CARRYING_OUT_ORGANIZATION_ID" Mode="Raw" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="#{uxChooseProjectWindow}.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:GridPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>

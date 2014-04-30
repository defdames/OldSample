<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umBudgetBiddingMain.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.umBudgetBiddingMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>


                <%-------------------------------------------------- Org panel --------------------------------------------------%>
                <ext:TreePanel
                    ID="uxOrgPanel"
                    runat="server"
                    Region="West"
                    Weight="100"
                    Title="Organizations"
                    Collapsible="true"
                    Width="225"
                    BodyPadding="6"
                    AutoScroll="true">
                    <TopBar>
                        <ext:Toolbar ID="uxOrgToolbar" runat="server">
                            <Items>
                                <ext:Button ID="uxExpandAll" runat="server" Text="Expand All" Icon="BulletToggleMinus">
                                    <Listeners>
                                        <Click Handler="#{uxOrgPanel}.expandAll();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="uxCollapseAll" runat="server" Text="Collapse All" Icon="BulletTogglePlus">
                                    <Listeners>
                                        <Click Handler="#{uxOrgPanel}.collapseAll();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>

                    <%-- NEW --%>
                    <Store>
                        <ext:TreeStore ID="TreeStore1" runat="server" OnReadData="LoadHierarchyTree">
                            <Proxy>
                                <ext:PageProxy></ext:PageProxy>
                            </Proxy>
                        </ext:TreeStore>
                    </Store>
                    <Root>
                        <ext:Node NodeID="0" Text="All Legal Entities" Expanded="true" />
                    </Root>
                    <SelectionModel>
                        <ext:TreeSelectionModel ID="uxHierarchyTreeSelectionModel" runat="server" Mode="Single"></ext:TreeSelectionModel>
                    </SelectionModel>
                    <%-- NEW --%>


                    <%--<Root>
                        <ext:Node Text="DBI" Expanded="true" Icon="BulletBlack">
                            <Children>
                                <ext:Node Text="Region 1" Icon="BulletGreen">
                                    <Children>
                                        <ext:Node Text="Butler Branch" Icon="BulletBlue">
                                            <Children>
                                                <ext:Node Text="Butler DOT" Icon="BulletPurple" Leaf="true" />
                                                <ext:Node Text="Butler IVM" Icon="BulletPurple" Leaf="true" />
                                                <ext:Node Text="Butler SW" Icon="BulletPurple" Leaf="true" />
                                            </Children>
                                        </ext:Node>
                                        <ext:Node Text="Lewistown Branch" Icon="BulletBlue">
                                            <Children>
                                                <ext:Node Text="Lewistown DOT" Icon="BulletPurple" Leaf="true" />
                                                <ext:Node Text="Lewistown IVM" Icon="BulletPurple" Leaf="true" />
                                                <ext:Node Text="Lewistown SW" Icon="BulletPurple" Leaf="true" />
                                            </Children>
                                        </ext:Node>
                                        <ext:Node Text="Hazleton IVM" Icon="BulletRed" Leaf="true">
                                        </ext:Node>
                                        <ext:Node Text="Hazleton DOT" Icon="BulletRed" Leaf="true">
                                            <Children>
                                            </Children>
                                        </ext:Node>
                                    </Children>
                                </ext:Node>
                                <ext:Node Text="Region 2" Icon="BulletGreen" Leaf="true">
                                </ext:Node>
                            </Children>
                        </ext:Node>
                    </Root>--%>
                    <DirectEvents>
                        <Select OnEvent="deLoadCorrectBudgetType" />
                    </DirectEvents>
                </ext:TreePanel>


                <%-------------------------------------------------- Top Spacer --------------------------------------------------%>
                <ext:Panel ID="uxSpacerBar" runat="server" Region="North" Title="Budget" />


                <%-------------------------------------------------- Toolbar --------------------------------------------------%>
                <ext:Toolbar ID="uxMainToolbar" runat="server" Region="North">
                    <Items>
                        <ext:ComboBox ID="uxFiscalYear"
                            runat="server"
                            DisplayField="ORG_HIER"
                            ValueField="ORG_HIER"
                            Width="120"
                            EmptyText="-- Year --"
                            Editable="false">
                            <Store>
                                <ext:Store ID="uxFiscalYearStore" runat="server" OnReadData="Test" AutoLoad="false">
                                    <Model>
                                        <ext:Model ID="Model1" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="ORG_HIER" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:PageProxy />
                                    </Proxy>
                                </ext:Store>
                            </Store>
                            <Listeners>
                                <Activate Handler="#{uxFiscalYearStore}.store.reload();" />
                            </Listeners>
<%--                            <DirectEvents>
                                <Select OnEvent="deLoadCorrectBudgetType">
                                </Select>
                            </DirectEvents>--%>
                        </ext:ComboBox>

                        <%--                        <ext:ComboBox ID="uxFiscalYear"
                            runat="server"
                            DisplayField="END_DATE"
                            ValueField="END_DATE"
                            Width="120"
                            EmptyText="-- Year --"
                            Editable="false">
                            <Store>
                                <ext:Store ID="uxFiscalYearStore" runat="server" OnReadData="deReadFiscalYears" AutoLoad="false">
                                    <Model>
                                        <ext:Model ID="Model1" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="END_DATE" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:PageProxy />
                                    </Proxy>
                                </ext:Store>
                            </Store>
                            <Listeners>
                                <Activate Handler="#{uxFiscalYearStore}.store.reload();" />
                            </Listeners>
                            <DirectEvents>
                                <Select OnEvent="deLoadCorrectBudgetType">
                                </Select>
                            </DirectEvents>
                        </ext:ComboBox>--%>

                        <ext:Label ID="uxSpace1" runat="server" Width="10" />

                        <ext:ComboBox ID="uxVersion"
                            runat="server"
                            DisplayField="BUD_VERSION"
                            ValueField="VER_ID"
                            Width="120"
                            EmptyText="-- Versions --"
                            Editable="false">
                            <Store>
                                <ext:Store ID="uxVersionStore" runat="server" OnReadData="deReadBudgetVersions" AutoLoad="false">
                                    <Model>
                                        <ext:Model ID="Model2" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="VER_ID" />
                                                <ext:ModelField Name="BUD_VERSION" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:PageProxy />
                                    </Proxy>
                                </ext:Store>
                            </Store>
                            <Listeners>
                                <Activate Handler="#{uxVersionStore}.store.reload();" />
                            </Listeners>
                            <DirectEvents>
                                <Select OnEvent="deLoadCorrectBudgetType">
                                </Select>
                            </DirectEvents>
                        </ext:ComboBox>

                        <ext:Label ID="uxSpace2" runat="server" Width="10" />

                        <ext:ComboBox ID="uxSummaryActions"
                            runat="server"
                            DisplayField="name"
                            Width="150"
                            EmptyText="-- Actions --"
                            QueryMode="Local"
                            TypeAhead="true">
                            <Store>
                                <ext:Store ID="uxSummaryActionsStore" runat="server" AutoDataBind="true">
                                    <Model>
                                        <ext:Model ID="Model3" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="abbr" />
                                                <ext:ModelField Name="name" />
                                                <ext:ModelField Name="slogan" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Reader>
                                        <ext:ArrayReader />
                                    </Reader>
                                </ext:Store>
                            </Store>
                        </ext:ComboBox>

                        <ext:ToolbarFill />

                        <ext:ComboBox ID="uxSummaryReports"
                            runat="server"
                            DisplayField="name"
                            Width="200"
                            EmptyText="-- Reports/Export --"
                            QueryMode="Local"
                            TypeAhead="true">
                            <Store>
                                <ext:Store ID="uxSummaryReportsStore" runat="server" AutoDataBind="true">
                                    <Model>
                                        <ext:Model ID="Model4" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="abbr" />
                                                <ext:ModelField Name="name" />
                                                <ext:ModelField Name="slogan" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Reader>
                                        <ext:ArrayReader />
                                    </Reader>
                                </ext:Store>
                            </Store>
                        </ext:ComboBox>

                        <ext:Label ID="uxSpace3" runat="server" Width="5" />

                        <ext:Button ID="uxUpdateAllActuals" runat="server" Text="Update All Actuals" Icon="BookEdit" />
                        <ext:Button ID="uxOrgSettings" runat="server" Text="Org Settings" Icon="Cog">
                            <DirectEvents>
                            </DirectEvents>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>


                <%-------------------------------------------------- Budget Panel --------------------------------------------------%>
                <ext:Panel
                    ID="uxBudgetPanel"
                    runat="server"
                    Region="Center">
                    <Loader ID="Loader1"
                        runat="server"
                        Url="umBlankBudget.aspx"
                        Mode="Frame">
                        <LoadMask ShowMask="true" />
                    </Loader>
                </ext:Panel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

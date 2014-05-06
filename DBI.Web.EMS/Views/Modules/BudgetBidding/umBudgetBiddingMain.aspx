<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umBudgetBiddingMain.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.umBudgetBiddingMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>


                <%-------------------------------------------------- Company and org panels --------------------------------------------------%>
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
                    FolderSort="true"
                    UseArrows="true">
                    <Store>
                        <ext:TreeStore ID="TreeStore1" runat="server" OnReadData="LoadOrgTree">
                            <Proxy>
                                <ext:PageProxy></ext:PageProxy>
                            </Proxy>
                        </ext:TreeStore>
                    </Store>
                    <Root>
                        <ext:Node NodeID="0" Text="All Companies" Expanded="true" />
                    </Root>
                    <SelectionModel>
                        <ext:TreeSelectionModel ID="uxCompanySelectionModel" runat="server" Mode="Single"></ext:TreeSelectionModel>
                    </SelectionModel>
                    <DirectEvents>
                        <ItemClick OnEvent="deLoadCorrectBudgetType">
                            <ExtraParams>
                                <ext:Parameter Name="id" Value="record.data.id" Mode="Raw" />
                            </ExtraParams>
                        </ItemClick>
                    </DirectEvents>
                </ext:TreePanel>


                <%-------------------------------------------------- Top Spacer --------------------------------------------------%>
                <ext:Panel ID="uxSpacerBar" runat="server" Region="North" Title="Budget" />


                <%-------------------------------------------------- Toolbar --------------------------------------------------%>
                <ext:Toolbar ID="uxMainToolbar" runat="server" Region="North">
                    <Items>
                        <ext:ComboBox ID="uxFiscalYear"
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
                        </ext:ComboBox>

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

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umViewCompleted.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umViewCompleted" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" IsDynamic="false" />
        <ext:Viewport runat="server" Layout="FitLayout">
            <Items>
                <ext:TabPanel runat="server">
                    <Items>
                        <ext:Panel runat="server" Title="Survey List" Layout="BorderLayout">
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
                                        <ItemClick Handler="#{uxCompletedStore}.reload()" />
                                    </Listeners>
                                </ext:TreePanel>
                                <ext:GridPanel runat="server" ID="uxCompletedGrid" Margin="5" Region="Center">
                                    <Store>
                                        <ext:Store runat="server" ID="uxCompletedStore" AutoDataBind="true" OnReadData="deReadCompletions" PageSize="5" RemoteSort="true" AutoLoad="false">
                                            <Model>
                                                <ext:Model ID="Model1" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="COMPLETION_ID" />
                                                        <ext:ModelField Name="FORMS_NAME" />
                                                        <ext:ModelField Name="FILLED_BY" />
                                                        <ext:ModelField Name="FILLED_ON" />
                                                        <ext:ModelField Name="SEGMENT1" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Sorters>
                                                <ext:DataSorter Property="FILLED_ON" Direction="ASC" />
                                            </Sorters>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel>
                                        <Columns>
                                            <ext:DateColumn ID="DateColumn1" runat="server" DataIndex="FILLED_ON" Text="Date Completed" Flex="15" />
                                            <ext:Column ID="Column1" runat="server" DataIndex="SEGMENT1" Text="Project Name" Flex="50" />
                                            <ext:Column ID="Column2" runat="server" DataIndex="FORMS_NAME" Text="Form Name" Flex="25" />
                                            <ext:Column ID="Column3" runat="server" DataIndex="FILLED_BY" Text="Filled By" Flex="10" />
                                        </Columns>
                                    </ColumnModel>
                                    <TopBar>
                                        <ext:Toolbar ID="Toolbar1" runat="server" />
                                    </TopBar>
                                    <BottomBar>
                                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
                                    </BottomBar>
                                    <Plugins>
                                        <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
                                    </Plugins>
                                </ext:GridPanel>
                            </Items>
                        </ext:Panel>
                    </Items>
                </ext:TabPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

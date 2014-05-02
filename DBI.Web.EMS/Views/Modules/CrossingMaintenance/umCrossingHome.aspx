<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCrossingHome.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umCrossingHome" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <div>
              <ext:Toolbar ID="Toolbar2" runat="server" Region="North" >
                        <Items>
                             <ext:ComboBox ID="uxRailRoadCI"
                                                runat="server"
                                                FieldLabel="Rail Road"
                                                LabelAlign="Right"
                                                DisplayField="RAILROAD"
                                                ValueField="RAILROAD_ID"
                                                QueryMode="Local"
                                                TypeAhead="true" Editable="false" ForceSelection="true">
                                                <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxAddRailRoadStore">
                                                        <Model>
                                                            <ext:Model ID="Model4" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="RAILROAD_ID" />
                                                                    <ext:ModelField Name="RAILROAD" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                    </ext:Store>
                                                </Store>
                                            
                                            </ext:ComboBox>
                            
                        </Items>
                    </ext:Toolbar>
            <ext:GridPanel ID="uxCrossingHomeGrid" Title="CROSSING LIST" runat="server" Region="North" Layout="HBoxLayout" Collapsible="true">
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="true" Mode="Single" />
                </SelectionModel>
                <Store>
                    <ext:Store runat="server"
                        ID="uxCrossingHomeStore"
                        OnReadData="deCrossingHomeGridData"
                        AutoDataBind="true" WarningOnDirty="false" PageSize="20">
                        <Model>
                            <ext:Model ID="Model2" runat="server">
                                <Fields>
                                    <ext:ModelField Name="CROSSING_ID" />
                                    <ext:ModelField Name="CONTACT_ID" />
                                    <ext:ModelField Name="CROSSING_NUMBER" Type="String" />
                                    <ext:ModelField Name="PROJECT_ID" />

                                    <ext:ModelField Name="RAILROAD" />
                                    <ext:ModelField Name="SERVICE_UNIT" />
                                    <ext:ModelField Name="SUB_DIVISION" />
                                    <ext:ModelField Name="STATE" />
                                    <ext:ModelField Name="CONTACT_NAME" />

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
                        <ext:Column ID="Column1" runat="server" DataIndex="RAILROAD" Text="RailRoad" Flex="1" />
                        <ext:Column ID="Column12" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" />
                        <ext:Column ID="uxSubDiv" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />
                        <ext:Column ID="Column2" runat="server" DataIndex="STATE" Text="State" Flex="1" />
                        <ext:Column ID="uxMTM" runat="server" DataIndex="CONTACT_NAME" Text="Manager" Flex="1" />

                    </Columns>
                </ColumnModel>
                <Plugins>
                    <ext:FilterHeader ID="FilterHeader1" runat="server" />
                </Plugins>
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>

                            <ext:Button ID="uxProjectListButton" runat="server" Text="Project List" Icon="ApplicationViewDetail" Disabled="true">
                                <Listeners>
                                    <Click Handler="#{uxProjectListWindow}.show()" />
                                </Listeners>
                                <DirectEvents>
                                    <Click OnEvent="deGetProjectList">
                                        <ExtraParams>
                                            <ext:Parameter Name="CrossingId" Value="#{uxCrossingHomeGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>


                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True">
                    </ext:PagingToolbar>
                </BottomBar>
                <Listeners>
                    <Select Handler="#{uxProjectListButton}.enable();" />
                </Listeners>
            </ext:GridPanel>
            <ext:Window runat="server"
                ID="uxProjectListWindow"
                Layout="FormLayout"
                Hidden="true"
                Title="Project List"
                Width="650"
                Closable="true" Modal="true">
                <Items>
                    <ext:GridPanel ID="uxProjectGrid" Height="350" runat="server" Flex="1" SimpleSelect="true" Margins="0 2 0 0" EmptyText="No Projects Assigned To This Crossing.">
                        <Store>
                            <ext:Store runat="server"
                                ID="uxProjectListStore"
                                AutoDataBind="true" WarningOnDirty="false">
                                <Model>
                                    <ext:Model ID="Model12" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PROJECT_ID" />
                                            <ext:ModelField Name="LONG_NAME" />
                                            <ext:ModelField Name="SEGMENT1" />
                                            <ext:ModelField Name="ORGANIZATION_NAME" />
                                        </Fields>
                                    </ext:Model>
                                </Model>

                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:Column ID="Column3" runat="server" DataIndex="SEGMENT1" Text="Project" Flex="1" />
                                <ext:Column ID="Column8" runat="server" DataIndex="LONG_NAME" Text="Project Name" Flex="2" />
                                <ext:Column ID="Column9" runat="server" DataIndex="ORGANIZATION_NAME" Text="Organization Name" Flex="1" />
                            </Columns>
                        </ColumnModel>

                    </ext:GridPanel>
                </Items>
            </ext:Window>
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umReportsMain.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umReportsMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Resources/StyleSheets/main.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <ext:ResourceManager ID="ResourceManager2" runat="server" />

        <div>
            <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
                <Items>
                     <ext:Toolbar ID="Toolbar1" runat="server" Region="North">
                        <Items>
                             <ext:ComboBox ID="uxRailRoadCI"
                                                runat="server"
                                                FieldLabel="Railroad"
                                                LabelAlign="Right"
                                                DisplayField="RAILROAD"
                                                ValueField="RAILROAD_ID"
                                                QueryMode="Local"
                                                TypeAhead="true" Editable="false">
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
                      <ext:Window
                        runat="server"
                        ID="uxChangeRailroadWindow"
                        Hidden="true"
                        Width="350"
                        Height="350"
                        Modal="true" Closable="false">
                        <Items>

                            <ext:GridPanel ID="uxRailroadGrid" runat="server" Title="Select Railroad" Height="325">
                              
                                <Store>
                                    <ext:Store runat="server"
                                        ID="uxRailRoadStore" OnReadData="deReadRRTypes" AutoDataBind="true"
                                        WarningOnDirty="false">
                                        <Model>
                                            <ext:Model ID="Model1" runat="server" IDProperty="RAILROAD_ID">
                                                <Fields>
                                                    <ext:ModelField Name="RAILROAD_ID" />
                                                    <ext:ModelField Name="RAILROAD" />
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
                                        <ext:Column ID="Column7" runat="server" DataIndex="RAILROAD" Text="Railroad" Flex="1" />
                                    </Columns>
                                </ColumnModel>
                               <DirectEvents>
                                   <Select OnEvent="deLoadRR" />
                               </DirectEvents>
                                    <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" />
                                </SelectionModel>
                            </ext:GridPanel>
                        </Items>
                    </ext:Window>
                    <ext:TabPanel ID="uxReportsTab" runat="server" Region="Center">
                        <Items>
                            <ext:Panel runat="server" 
                                Title="State Crossing List"
                                ID="uxStateCrossingsList"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader5" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umStateCrossingsList.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>

                            </ext:Panel>

                            <ext:Panel runat="server"
                                Title="Application Date"
                                ID="uxAppDate"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="uxHeaderLoader" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umAppDate.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>

                            </ext:Panel>
                             <ext:Panel runat="server"
                                Title="Incidents"
                                ID="uxIncidents"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader1" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umIncidentReports.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>

                            </ext:Panel>
                           

                        </Items>
                    </ext:TabPanel>

                </Items>
            </ext:Viewport>
        </div>
    </div>
    </form>
</body>
</html>

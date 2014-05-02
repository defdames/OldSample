<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umRailRoadProject.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umRailRoadProject" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager2" runat="server" />
        <%-- <div>      </div>--%>
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
                       
                        
                    
                <ext:GridPanel ID="uxRailRoadGridPanel" runat="server" Region="West" Width="225" Margin="5" Layout="FitLayout">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxRailRoadStore" OnReadData="deReadRailRoad"                      
                            AutoDataBind="true" WarningOnDirty="false">
                            <Model>
                                <ext:Model Name="RailRoad" IDProperty="RAILROAD_ID" runat="server">
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
                            <ext:Column ID="Column4" runat="server" DataIndex="RAILROAD" Text="RailRoad" Flex="1">
                                <Editor>
                                    <ext:TextField runat="server" />
                                </Editor>
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="FilterHeader1" runat="server" />
                        <ext:CellEditing ID="CellEditing1" runat="server" ClicksToEdit="2" />
                    </Plugins>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" Mode="Single" />
                    </SelectionModel>

                    <TopBar>
                    <ext:Toolbar ID="Toolbar2" runat="server" Region="North">
                            <Items>
                              <ext:Button ID="uxAddRRButton" runat="server" Text="Add Railroad" Icon="ApplicationAdd" >

                               <Listeners>
                                        <Click Handler="#{uxRailRoadStore}.insert(0, new RailRoad());" />
                                    </Listeners>
                                </ext:Button>
                               <ext:Button ID="uxSaveRRButton" runat="server" Text="Save Railroad" Icon="Add" >

                                    <DirectEvents>
                                        <Click OnEvent="deSaveRailRoad" Before="#{uxRailRoadStore}.isDirty()">
                                            <ExtraParams>
                                                <ext:Parameter Name="rrdata" Value="#{uxRailRoadStore}.getChangedData()" Mode="Raw" Encode="true" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                  <DirectEvents>
                <Select OnEvent="deLoadStores">
                    <ExtraParams>
                        <ext:Parameter Name="RailroadId" Value="#{uxRailRoadGridPanel}.getSelectionModel().getSelection()[0].data.RAILROAD_ID" Mode="Raw" />
                    </ExtraParams>
                </Select>
            </DirectEvents>
                </ext:GridPanel>


                <ext:GridPanel ID="uxProjectGrid" runat="server" Region="Center" Flex="10" Margin="5">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxCurrentSecurityProjectStore"
                            OnReadData="deSecurityProjectGrid"
                            PageSize="15"
                            AutoLoad="false"
                            AutoDataBind="true" WarningOnDirty="false">
                               <Parameters>
                            <ext:StoreParameter Name="RailroadId" Value="#{uxRailRoadGridPanel}.getSelectionModel().getSelection()[0].data.RAILROAD_ID" Mode="Raw" />
                        </Parameters>
                            <Model>
                                <ext:Model ID="Model2" runat="server">
                                    <Fields>
                                        
                                        <ext:ModelField Name="PROJECT_ID" />
                                        <ext:ModelField Name="LONG_NAME" />
                                        <ext:ModelField Name="SEGMENT1" />
                                        <ext:ModelField Name="ORGANIZATION_NAME" />
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
                            <ext:Column ID="Column1" runat="server" DataIndex="SEGMENT1" Text="Project" Flex="1" />
                            <ext:Column ID="Column2" runat="server" DataIndex="LONG_NAME" Text="Project Name" Flex="2" />
                            <ext:Column ID="Column3" runat="server" DataIndex="ORGANIZATION_NAME" Text="Organization Name" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="FilterHeader2" runat="server" Remote="true" />
                    </Plugins>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel ID="CheckboxSelectionModel2" runat="server" Mode="Multi" />
                    </SelectionModel>

                    <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server" >
                            <Items>
                            
                                <ext:Button ID="uxAddProjectButton" runat="server" Text="Assign Projects To Railroad" Icon="ArrowJoin" Disabled="true" >
                    <DirectEvents>
                        <Click OnEvent="deAssociateProject">
                            <Confirmation ConfirmRequest="true" Title="Associate?" Message="Are you sure you want to associate the selected projects with the selected railroad?" />
                            <ExtraParams>
                                <ext:Parameter Name="RailroadId" Value="#{uxRailRoadGridPanel}.getSelectionModel().getSelection()[0].data.RAILROAD_ID" Mode="Raw" />
                                <ext:Parameter Name="selectedProjects" Value="Ext.encode(#{uxProjectGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />

                            </ExtraParams>
                        </Click>


                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                   
                    <Listeners>
                        <Select Handler="#{uxAddProjectButton}.enable()" />
                    </Listeners>
                </ext:GridPanel>

                 <ext:GridPanel ID="uxAssignedProjectGrid" runat="server" Region="South" Flex="7" Margin="5">
                    <Store>
                        <ext:Store runat="server"
                            ID="Store2"
                            OnReadData="GetProjectsGridData"
                            AutoLoad="false"
                            AutoDataBind="true"
                            PageSize="15"
                            WarningOnDirty="false">
                             <Parameters>
                            <ext:StoreParameter Name="RailroadId" Value="#{uxRailRoadGridPanel}.getSelectionModel().getSelection()[0].data.RAILROAD_ID" Mode="Raw" />
                        </Parameters>
                            <Model>
                                <ext:Model ID="Model3" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="RAILROAD_ID" />
                                        
                                        <ext:ModelField Name="PROJECT_ID" />
                                        <ext:ModelField Name="LONG_NAME" />
                                        <ext:ModelField Name="SEGMENT1" />
                                        <ext:ModelField Name="ORGANIZATION_NAME" />
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
                            
                            <ext:Column ID="Column5" runat="server" DataIndex="SEGMENT1" Text="Project" Flex="1" />
                            <ext:Column ID="Column6" runat="server" DataIndex="LONG_NAME" Text="Project Name" Flex="2" />
                            <ext:Column ID="Column7" runat="server" DataIndex="ORGANIZATION_NAME" Text="Organization Name" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectionModel3" runat="server" Mode="Single" />
                    </SelectionModel>
                     <TopBar>
                         <ext:Toolbar runat="server" >
                             <Items>
                   <ext:Button ID="Button2" runat="server" Text="Remove Project From Railroad" Icon="ApplicationDelete" Disabled="true">
                      <DirectEvents>
                        <Click OnEvent="deRemoveProjectFromRailroad">
                            <Confirmation ConfirmRequest="true" Title="Remove?" Message="Are you sure you want to remove this project from this railroad?" />

                            <ExtraParams>
                                <ext:Parameter Name="RailroadId" Value="#{uxRailRoadGridPanel}.getSelectionModel().getSelection()[0].data.RAILROAD_ID" Mode="Raw" />
                               <ext:Parameter Name="ProjectsAssigned" Value="Ext.encode(#{uxAssignedProjectGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                            </ExtraParams>
                        </Click>
                    </DirectEvents>
                </ext:Button>
                       </Items>
                    </ext:Toolbar>
                     </TopBar>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
                    </BottomBar>
                    <Listeners>
                        <Select Handler="#{Button2}.enable()" />
                    </Listeners>
                </ext:GridPanel>
            </Items>

        </ext:Viewport>

    </form>
</body>
</html>

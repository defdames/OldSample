<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCrossingSecurity.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umCrossingSecurity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
      <script type="text/javascript" src="../../../Resources/Scripts/functions.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <div></div>
         <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
                <Items>
        <ext:GridPanel ID="uxProjectGrid" runat="server" Region="North" Title="Select Project" Margins="0 2 0 0">
            <Store>
                <ext:Store runat="server"
                    ID="uxCurrentSecurityProjectStore"
                    OnReadData="deSecurityProjectGrid"
                    PageSize="10"
                    AutoDataBind="true" WarningOnDirty="false">
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
                    <Sorters>
                        <ext:DataSorter Direction="ASC" Property="ORGANIZATION_NAME" />
                    </Sorters>
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
                <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" Mode="Single" />
            </SelectionModel>
      
          <DirectEvents>
              <SelectionChange OnEvent="deLoadStore" />
          </DirectEvents>
            <BottomBar>
                <ext:PagingToolbar ID="PagingToolbar2" runat="server" />                
            </BottomBar>
              <Listeners>
                <Select Handler="#{Button1}.enable()" />
            </Listeners>
        </ext:GridPanel>
        <%--  ---------------------------------------------------------------------------------------------------------------------%>
         <ext:GridPanel ID="uxAssignedCrossingGrid" runat="server" Margins="0 2 0 0"  Region="Center">
            <Store>
                <ext:Store runat="server"
                    ID="uxAssignedCrossingStore" OnReadData="GetCrossingsGridData" AutoDataBind="true" AutoLoad="false" PageSize="20">
                     <Parameters>
                        <ext:StoreParameter Name="ProjectId" Value="#{uxProjectGrid}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
                    </Parameters>
                    <Model>
                        <ext:Model ID="Model3" runat="server">
                            <Fields>
                                <ext:ModelField Name="CROSSING_ID" />
                                <ext:ModelField Name="CROSSING_NUMBER" />
                                <ext:ModelField Name="PROJECT_ID" />
                                <ext:ModelField Name="RAILROAD" />
                                <ext:ModelField Name="SERVICE_UNIT" />
                                <ext:ModelField Name="SUB_DIVISION" />
                                <ext:ModelField Name="STATE" />

                            </Fields>
                        </ext:Model>
                    </Model>
                   <Proxy>
                       <ext:PageProxy />
                   </Proxy>
                    <Sorters>
                        <ext:DataSorter Direction="ASC" Property="SERVICE_UNIT" />
                    </Sorters>
                </ext:Store>
            </Store>
            <ColumnModel>
                <Columns>
                    <ext:Column ID="Column5" runat="server" DataIndex="CROSSING_NUMBER" Text="Crossing #" Flex="1" />
                    <ext:Column ID="Column8" runat="server" DataIndex="RAILROAD" Text="RailRoad" Flex="1" />
                    <ext:Column ID="Column9" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" />
                    <ext:Column ID="Column10" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />
                    <ext:Column ID="Column11" runat="server" DataIndex="STATE" Text="State" Flex="1" />
                    

                </Columns>
            </ColumnModel>
            
          
             <TopBar>
                 <ext:Toolbar ID="Toolbar2" runat="server">
            <Items>
              
                
                <ext:Button ID="Button1" runat="server" Text="Assign New Crossings to Project" Icon="ApplicationAdd" Disabled="true">
                    <Listeners>
                       <Click Handler="#{uxAssignCrossingWindow}.show()" />
                      </Listeners>
          
                </ext:Button>
                <ext:Button ID="Button2" runat="server" Text="Remove Crossing From Project" Icon="ApplicationDelete" Disabled="true">
                      <DirectEvents>
                        <Click OnEvent="deRemoveCrossingFromProject">
                            <Confirmation ConfirmRequest="true" Title="Remove?" Message="Are you sure you want to remove this crossing from this project?" />

                            <ExtraParams>
                                <ext:Parameter Name="projectId" Value="#{uxProjectGrid}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
                               <ext:Parameter Name="CrossingsAssigned" Value="Ext.encode(#{uxAssignedCrossingGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                            </ExtraParams>
                        </Click>
                    </DirectEvents>
                </ext:Button>
            </Items>
        </ext:Toolbar>
             </TopBar>
              <BottomBar>
                <ext:PagingToolbar ID="PagingToolbar3" runat="server" />
            </BottomBar>
              <Listeners>
                <Select Handler="#{Button2}.enable()" />
            </Listeners>
        </ext:GridPanel>

         <ext:Window runat="server" ID="uxAssignCrossingWindow" Hidden="true" Width="800" Layout="FormLayout" Height="620" Modal="true">
             <Items>
                  <ext:Panel ID="uxTransferCrossingPanel" runat="server" Width="790" Height="553">
                   <LayoutConfig>
                            <ext:HBoxLayoutConfig Align="Stretch" Padding="5" />
                        </LayoutConfig>

                        <Items>

          <ext:GridPanel ID="uxSubDivGrid" runat="server" Title="Select State" Margin="5" Flex="1" >
            <Store>
                <ext:Store runat="server"
                    ID="uxSubDivStore" OnReadData="deReadSubDiv" AutoDataBind="true" PageSize="20"
                    WarningOnDirty="false">
                   
                    <Model>
                        <ext:Model ID="Model4" runat="server">
                            <Fields>
                                
                                <ext:ModelField Name="STATE" />

                            </Fields>
                        </ext:Model>
                    </Model>
                     <Proxy>
                       <ext:PageProxy />
                     </Proxy>
                    <Sorters>
                        <ext:DataSorter Direction="ASC" Property="STATE" /> 
                    </Sorters>
                </ext:Store>
            </Store>
            <ColumnModel>
                <Columns>
                    <ext:Column ID="Column14" runat="server" DataIndex="STATE" Text="State" Flex="1" />

                </Columns>
            </ColumnModel>
            <Plugins>
                <ext:FilterHeader ID="FilterHeader3" runat="server" Remote="true" />
            </Plugins>
            <SelectionModel>
                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" Mode="Single" />
            </SelectionModel>
             <Listeners>
                <Select Handler="#{uxCurrentSecurityCrossingStore}.load() " />
            </Listeners>
            <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True" DisplayInfo="false">
                    </ext:PagingToolbar>
                </BottomBar>
        </ext:GridPanel>  
              
                
        <ext:GridPanel ID="uxCrossingGrid" runat="server" Title="Apply Selected Crossing to Project" Flex="3" Margin="5" >
            <Store>
                <ext:Store runat="server"
                    ID="uxCurrentSecurityCrossingStore"
                    WarningOnDirty="false" AutoLoad="false" OnReadData="deSecurityCrossingGridData" AutoDataBind="true">
                    <Parameters>                           
                        <ext:StoreParameter Name="ProjectId" Value="#{uxProjectGrid}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
                         <ext:StoreParameter Name="State" Value="#{uxSubDivGrid}.getSelectionModel().getSelection()[0].data.STATE" Mode="Raw" />
                    </Parameters>
                    <Model>
                        <ext:Model ID="Model1" runat="server">
                            <Fields>
                                <ext:ModelField Name="CROSSING_ID" />
                                <ext:ModelField Name="CROSSING_NUMBER" />
                                <ext:ModelField Name="PROJECT_ID" />
                                <ext:ModelField Name="RAILROAD" />
                                <ext:ModelField Name="SERVICE_UNIT" />
                                <ext:ModelField Name="SUB_DIVISION" />

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
                    <ext:Column ID="uxNameCON" runat="server" DataIndex="CROSSING_NUMBER" Text="Crossing #" Flex="1" />

                    <ext:Column ID="Column7" runat="server" DataIndex="RAILROAD" Text="RailRoad" Flex="1" />
                    <ext:Column ID="Column6" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" />
                    <ext:Column ID="Column4" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />

                </Columns>
            </ColumnModel>
            <Plugins>
                <ext:FilterHeader ID="FilterHeader1" runat="server"  />
            </Plugins>
            <SelectionModel>
                <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" Mode="Multi" />
            </SelectionModel>
             <Listeners>
                <Select Handler="#{uxApplyButtonCS}.enable() " />
            </Listeners>
        
        </ext:GridPanel>

               </Items>
             </ext:Panel>
                
                 <ext:StatusBar ID="StatusBar1" runat="server">
            <Items>
               <%-- <ext:ToolbarFill ID="ToolbarFill1" runat="server" />--%>

                <ext:Button ID="uxApplyButtonCS" runat="server" Text="Associate" Icon="ArrowJoin" Disabled="true">
                    <DirectEvents>
                        <Click OnEvent="deAssociateCrossings">
                            <Confirmation ConfirmRequest="true" Title="Associate?" Message="Are you sure you want to associate the selected crossings with the selected project?" />
                            <ExtraParams>
                                <ext:Parameter Name="projectId" Value="#{uxProjectGrid}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
                                <ext:Parameter Name="selectedCrossings" Value="Ext.encode(#{uxCrossingGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />

                            </ExtraParams>
                        </Click>


                    </DirectEvents>
                </ext:Button>
                <ext:Button runat="server" ID="CancelCrossing" Text="Cancel" Icon="Delete">
                    <DirectEvents>
                        <Click OnEvent="deCloseAssignScreen" />
                    </DirectEvents>
                            </ext:Button>
                 
            </Items>
        </ext:StatusBar>
              
            </Items>

        </ext:Window>
        </Items>
             </ext:Viewport>

    </form>
</body>
</html>

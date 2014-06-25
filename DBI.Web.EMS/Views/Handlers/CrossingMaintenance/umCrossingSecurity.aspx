<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCrossingSecurity.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umCrossingSecurity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <div></div>
        <ext:GridPanel ID="uxProjectGrid" runat="server" Flex="1" SimpleSelect="true" Title="Select Project" Margins="0 2 0 0" >
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
                <Select OnEvent="GetCrossingsGridData">
                    <ExtraParams>
                        <ext:Parameter Name="ProjectId" Value="#{uxProjectGrid}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
                    </ExtraParams>
                </Select>
            </DirectEvents>
          
            <BottomBar>
                <ext:PagingToolbar ID="PagingToolbar2" runat="server" />
            </BottomBar>
              <Listeners>
                <Select Handler="#{Button1}.enable()" />
            </Listeners>
        </ext:GridPanel>
        <%--  ---------------------------------------------------------------------------------------------------------------------%>
         <ext:GridPanel ID="uxAssignedCrossingGrid" runat="server" Margins="0 2 0 0" >
            <Store>
                <ext:Store runat="server"
                    ID="uxAssignedCrossingStore"
                  >
                    <Model>
                        <ext:Model ID="Model3" runat="server">
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
                   
                </ext:Store>
            </Store>
            <ColumnModel>
                <Columns>
                    <ext:Column ID="Column5" runat="server" DataIndex="CROSSING_NUMBER" Text="Crossing #" Flex="1" />
                    <ext:Column ID="Column8" runat="server" DataIndex="RAILROAD" Text="RailRoad" Flex="1" />
                    <ext:Column ID="Column9" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" />
                    <ext:Column ID="Column10" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />

                </Columns>
            </ColumnModel>
            
          
             <TopBar>
                 <ext:Toolbar ID="Toolbar2" runat="server">
            <Items>
              

                <ext:Button ID="Button1" runat="server" Text="Assign New Crossings to Project" Icon="ApplicationAdd" Disabled="true">
                    <Listeners>
                       <Click Handler="#{uxAssignCrossingWindow}.show()" />
                      </Listeners>
                 <DirectEvents>
                
                <CLick OnEvent="deSecurityCrossingGridData">
                    <ExtraParams>
                        <ext:Parameter Name="ProjectId" Value="#{uxProjectGrid}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
                    </ExtraParams>
                </CLick>
            </DirectEvents>
           
                   
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
              <Listeners>
                <Select Handler="#{Button2}.enable()" />
            </Listeners>
        </ext:GridPanel>

         <ext:Window runat="server" ID="uxAssignCrossingWindow" Hidden="true" Width="650" Height="450" Modal="true">
           
              
                    <Items>
        <ext:GridPanel ID="uxCrossingGrid" runat="server" Title="Apply Selected Crossing to Project" Height="390" >
            <Store>
                <ext:Store runat="server"
                    ID="uxCurrentSecurityCrossingStore"
                    WarningOnDirty="false">
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
                                <Listeners>
                                    <Click Handler="#{uxCurrentSecurityCrossingStore}.reload();
									#{uxAssignCrossingWindow}.hide()" />
                                </Listeners>
                            </ext:Button>
                 
            </Items>
        </ext:StatusBar>
             
            </Items>
        </ext:Window>


    </form>
</body>
</html>

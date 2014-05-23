<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageKCS.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.ManageKCS" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div> </div>
        <ext:ResourceManager ID="ResourceManager2" runat="server" />
         
            <%-- <ext:Panel ID="uxAssignContactPanel" runat="server" >
                 <LayoutConfig>
                     <ext:BorderLayoutConfig Padding="5" />
                 </LayoutConfig>
                        <Items>  --%>         
          <ext:GridPanel ID="uxServiceUnitGridPanel" runat="server" Margin="5" Title="SERVICE UNIT">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxServiceUnitStore" OnReadData="deReadServiceUnit"                      
                            AutoDataBind="true" WarningOnDirty="false">
                            <Model>
                                <ext:Model ID="Model1" Name="ServiceUnit" IDProperty="SERVICE_UNIT_ID" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="SERVICE_SUB_ID" />
                                        <ext:ModelField Name="SERVICE_UNIT_ID" />
                                        <ext:ModelField Name="SERVICE_UNIT_NAME" />

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
                            <ext:Column ID="Column4" runat="server" DataIndex="SERVICE_UNIT_NAME" Text="Service Unit" Flex="1">
                                <Editor>
                                    <ext:TextField ID="TextField1" runat="server" />
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
                              <ext:Button ID="uxAddServiceUnitButton" runat="server" Text="Add Service Unit" Icon="ApplicationAdd" >

                               <Listeners>
                                        <Click Handler="#{uxServiceUnitStore}.insert(0, new ServiceUnit());" />
                                    </Listeners>
                                </ext:Button>
                               <ext:Button ID="uxSaveServiceUnitButton" runat="server" Text="Save Service Unit" Icon="Add" >

                                    <DirectEvents>
                                        <Click OnEvent="deSaveServiceUnit" Before="#{uxServiceUnitStore}.isDirty()">
                                            <ExtraParams>
                                                <ext:Parameter Name="sudata" Value="#{uxServiceUnitStore}.getChangedData()" Mode="Raw" Encode="true" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                 <ext:Button ID="uxRemoveServiceUnitButton" runat="server" Text="Remove Service Unit" Icon="Delete" >

                                   <%-- <DirectEvents>
                                        <Click OnEvent="deSaveServiceUnit" Before="#{uxRailRoadStore}.isDirty()">
                                            <ExtraParams>
                                                <ext:Parameter Name="rrdata" Value="#{uxRailRoadStore}.getChangedData()" Mode="Raw" Encode="true" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>--%>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                <%--  <DirectEvents>
                <Select OnEvent="deLoadSubDiv">
                    <ExtraParams>
                        <ext:Parameter Name="ServiceUnitId" Value="#{uxServiceUnitGridPanel}.getSelectionModel().getSelection()[0].data.SERVICE_UNIT_ID" Mode="Raw" />
                    </ExtraParams>
                </Select>
            </DirectEvents>--%>
                </ext:GridPanel>
     <%--  ------------------------------------------------------------------------------------------------------------------------------------------------------%>
   
          <ext:GridPanel ID="uxSubDivGridPanel" runat="server" Margin="5" Title="SUB DIVISION">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxSubDivStore"                      
                            AutoDataBind="true" WarningOnDirty="false">
                            <Model>
                                <ext:Model ID="Model2" Name="SubDiv" IDProperty="SUB_DIVISION_ID" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="SUB_DIVISION_ID" />
                                        <ext:ModelField Name="SUB_DIVISION_NAME" />

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
                            <ext:Column ID="Column1" runat="server" DataIndex="SUB_DIVISION_NAME" Text="Subdivision" Flex="1">
                                <Editor>
                                    <ext:TextField ID="TextField2" runat="server" />
                                </Editor>
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="FilterHeader2" runat="server" />
                        <ext:CellEditing ID="CellEditing2" runat="server" ClicksToEdit="2" />
                    </Plugins>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" Mode="Single" />
                    </SelectionModel>

                    <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server" Region="North">
                            <Items>
                              <ext:Button ID="uxAddSubDivButton" runat="server" Text="Add Subdivision" Icon="ApplicationAdd" >

                               <Listeners>
                                        <Click Handler="#{uxSubDivStore}.insert(0, new SubDiv());" />
                                    </Listeners>
                                </ext:Button>
                               <ext:Button ID="uxSaveSubDivButton" runat="server" Text="Save Subdivision" Icon="Add" >

                                    <DirectEvents>
                                        <Click OnEvent="deSaveSubDiv" Before="#{uxSubDivStore}.isDirty()">
                                            <ExtraParams>
                                                <ext:Parameter Name="subdivdata" Value="#{uxSubDivStore}.getChangedData()" Mode="Raw" Encode="true" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                 <ext:Button ID="uxSaveRRButton" runat="server" Text="Remove Subdivision" Icon="Delete" >

                                   <%-- <DirectEvents>
                                        <Click OnEvent="deSaveSubDiv" Before="#{uxSubDivStore}.isDirty()">
                                            <ExtraParams>
                                                <ext:Parameter Name="subdivdata" Value="#{uxSubDivStore}.getChangedData()" Mode="Raw" Encode="true" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>--%>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                <%--  <DirectEvents>
                <Select OnEvent="deLoadStores">
                    <ExtraParams>
                        <ext:Parameter Name="RailroadId" Value="#{uxRailRoadGridPanel}.getSelectionModel().getSelection()[0].data.RAILROAD_ID" Mode="Raw" />
                    </ExtraParams>
                </Select>
            </DirectEvents>--%>
                </ext:GridPanel>
              <%--</Items>
             </ext:Panel>--%>
    </form>
</body>
</html>

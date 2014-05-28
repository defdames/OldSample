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
         
            <ext:Viewport ID="uxAdminViewPort" runat="server" Layout="BorderLayout" >
            <Items>      
          <ext:GridPanel ID="uxServiceUnitGridPanel" runat="server" Margin="10" Region="West" Title="SERVICE UNIT" Width="750">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxServiceUnitStore" OnReadData="deReadServiceUnit"                      
                            AutoDataBind="true" WarningOnDirty="false">
                            <Model>
                                <ext:Model ID="Model1" Name="ServiceUnit" IDProperty="SERVICE_UNIT_ID" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="RAILROAD_ID" />
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
                                    <ext:TextField ID="TextField1" EmptyText="Service Unit Name" runat="server" />
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
                               <%--  <ext:Button ID="uxRemoveServiceUnitButton" runat="server" Text="Remove Service Unit" Icon="Delete" >

                                  
                                </ext:Button>--%>
                               
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
               <DirectEvents>
                <Select OnEvent="deLoadSubDiv">
                </Select>
            </DirectEvents>
               <Listeners>
                   <Select Handler="#{uxAddSubDivButton}.enable(); #{uxSaveSubDivButton}.enable()" />
               </Listeners>
                </ext:GridPanel>
     <%--  ------------------------------------------------------------------------------------------------------------------------------------------------------%>
   
          <ext:GridPanel ID="uxSubDivGridPanel" runat="server" Margin="10" Region="Center" Title="SUB DIVISION" Width="400" >
                    <Store>
                        <ext:Store runat="server"
                            ID="uxSubDivStore" OnReadData="deReadSubDiv"                     
                            AutoDataBind="true" WarningOnDirty="false" AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model2" Name="SubDiv" IDProperty="SUB_DIVISION_ID" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="SUB_DIVISION_ID" />
                                        <ext:ModelField Name="SUB_DIVISION_NAME" />
                                        <ext:ModelField Name="SERVICE_UNIT_ID" />

                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Parameters>
                               <ext:StoreParameter Name="ServiceUnitId" Value="#{uxServiceUnitGridPanel}.getSelectionModel().getSelection()[0].data.SERVICE_UNIT_ID" Mode="Raw" />
                            </Parameters>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column1" runat="server" DataIndex="SUB_DIVISION_NAME" Text="Subdivision" Flex="1">
                                <Editor>
                                    <ext:TextField ID="TextField2" EmptyText="Subdivision Name" runat="server" />
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
                              <ext:Button ID="uxAddSubDivButton" runat="server" Text="Add Subdivision" Icon="ApplicationAdd" Disabled="true" >

                               <Listeners>
                                        <Click Handler="#{uxSubDivStore}.insert(0, new SubDiv());" />
                                    </Listeners>
                                </ext:Button>
                               <ext:Button ID="uxSaveSubDivButton" runat="server" Text="Save Subdivision" Icon="Add" Disabled="true" >

                                    <DirectEvents>
                                        <Click OnEvent="deSaveSubDiv" Before="#{uxSubDivStore}.isDirty()">
                                            <ExtraParams>
                                                <ext:Parameter Name="ServiceUnitId" Value="#{uxServiceUnitGridPanel}.getSelectionModel().getSelection()[0].data.SERVICE_UNIT_ID" Mode="Raw" />
                                                <ext:Parameter Name="subdivdata" Value="#{uxSubDivStore}.getChangedData()" Mode="Raw" Encode="true" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                 <ext:Button ID="uxRemoveSDButton" runat="server" Text="Remove Subdivision" Icon="Delete" Disabled="true" >

                                    <DirectEvents>
                                        <Click OnEvent="deRemoveSubDiv" >
                                            <ExtraParams>
                                                <ext:Parameter Name="SDInfo" Value="Ext.encode(#{uxSubDivGridPanel}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Listeners>
                        <Select Handler="#{uxRemoveSDButton}.enable()" />
                    </Listeners>
                </ext:GridPanel>
              </Items>
             </ext:Viewport>
    </form>
</body>
</html>

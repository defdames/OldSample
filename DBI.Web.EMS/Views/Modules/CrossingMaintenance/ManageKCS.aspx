<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageKCS.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.ManageKCS" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <script type="text/javascript">

         var getDragDropText = function () {
             var buf = [];

             buf.push("<ul>");

             Ext.each(this.view.panel.getSelectionModel().getSelection(), function (record) {
                 buf.push("<li>" + record.data.SUB_DIVISION_NAME + "</li>");
             });

             buf.push("</ul>");

             return buf.join("");
         };
    </script>
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
                                 <ext:Button ID="uxUpdateSubDivButton" runat="server" Text="Update Subdivisions" Icon="TransmitGo">
                                 <Listeners>
                                     <Click Handler="#{uxUpdateSubDivWindow}.show()" />
                                 </Listeners>
                                     <DirectEvents>
                                         <Click OnEvent="deLoadServiceUnitStores" />
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

                 <ext:Window runat="server"
                ID="uxUpdateSubDivWindow"
                Layout="FormLayout"
                Hidden="true"
                Title="Select Service Units To Update"
                Width="550" Height="120" Closable="false" Modal="true">
                <Items>
                    <ext:FormPanel ID="uxUpdateSubDivForm" runat="server" Layout="FormLayout">
                        <Items>
                           <ext:ComboBox ID="uxCurrentServiceUnit"
                            runat="server" FieldLabel="Current Unit"
                            LabelAlign="Right"
                            AnchorHorizontal="25%"
                            DisplayField="SERVICE_UNIT_NAME"
                            ValueField="SERVICE_UNIT_ID"
                            QueryMode="Local" TypeAhead="true" TabIndex="2" AllowBlank="false" >
                            <Store>
                                <ext:Store runat="server"
                                    ID="uxCurrentServiceUnitStore" OnReadData="deCurrentServiceUnitGrid" >
                                    <Model>
                                        <ext:Model ID="Model5" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="SERVICE_UNIT_ID" />
                                                <ext:ModelField Name="SERVICE_UNIT_NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                      <Sorters>
                                        <ext:DataSorter Direction="ASC" Property="SERVICE_UNIT_NAME" />
                                    </Sorters>
                                </ext:Store>
                            </Store>
                            
                        </ext:ComboBox>
                            <ext:ComboBox ID="uxNewServiceUnit"
                            runat="server" FieldLabel="New Service Unit"
                            LabelAlign="Right"
                            AnchorHorizontal="25%"
                            DisplayField="SERVICE_UNIT_NAME"
                            ValueField="SERVICE_UNIT_ID"
                            QueryMode="Local" TypeAhead="true" TabIndex="2" AllowBlank="false" >
                            <Store>
                                <ext:Store runat="server"
                                    ID="uxNewServiceUnitStore" OnReadData="deNewServiceUnitGrid">
                                    <Model>
                                        <ext:Model ID="Model6" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="SERVICE_UNIT_ID" />
                                                <ext:ModelField Name="SERVICE_UNIT_NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Sorters>
                                        <ext:DataSorter Direction="ASC" Property="SERVICE_UNIT_NAME" />
                                    </Sorters>
                                </ext:Store>
                            </Store>
                            
                        </ext:ComboBox>
                        </Items>
                        <Buttons>
                            <ext:Button runat="server" ID="uxSelectSubDivToUpdate" Text="Select Subdivisions to Update" Icon="Add" Disabled="true">

                                <DirectEvents>
                                    <Click OnEvent="deShowGrid">
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button runat="server" ID="uxCancelCrossingToUpdate" Text="Cancel Selection" Icon="Delete">
                                <Listeners>
                                    <Click Handler="#{uxUpdateSubDivForm}.reset();
									#{uxUpdateSubDivWindow}.hide()" />
                                </Listeners>

                            </ext:Button>
                        </Buttons>
                        <Listeners>
                            <ValidityChange Handler="#{uxSelectSubDivToUpdate}.setDisabled(!valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
            </ext:Window>
            <%------------------------------------------------------------------------------------------------------------------------------------------%>

            <ext:Window runat="server"
                ID="uxTransferSubDivWindow"
                Layout="HBoxLayout"
                Height="593"
                Width="725"
                Hidden="true"
                BodyPadding="5"
                BodyBorder="0"
                Title="Drag and Drop Subdivisions That Are To Be Updated" Modal="true">
                <Items>
                    <ext:Panel ID="uxTransferCrossingPanel" runat="server" Width="700" Height="553">
                        <LayoutConfig>
                            <ext:HBoxLayoutConfig Align="Stretch" Padding="5" />
                        </LayoutConfig>

                        <Items>

                            <ext:GridPanel
                                ID="uxTransferOldSubDivGrid"
                                runat="server"
                                MultiSelect="true"
                                Flex="1"
                                Title="Current Subdivision"
                                Margins="0 2 0 0" >

                                <Store>
                                    <ext:Store ID="uxCurrentSubDivStore" runat="server">
                                        <Model>
                                            <ext:Model ID="Model3" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="SUB_DIVISION_ID" />
                                                    <ext:ModelField Name="SERVICE_UNIT_ID" />
                                                    <ext:ModelField Name="SUB_DIVISION_NAME" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>

                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="Column2" runat="server" Text="Subdivision" Width="280" DataIndex="SUB_DIVISION_NAME" Flex="1" />
                                    </Columns>
                                </ColumnModel>

                                <View>
                                    <ext:GridView>

                                        <Plugins>
                                            <ext:GridDragDrop ID="GridDragDrop1" runat="server" DragGroup="firstGridDDGroup" DropGroup="secondGridDDGroup" />
                                        </Plugins>

                                        <Listeners>
                                            <AfterRender Handler="this.plugins[0].dragZone.getDragText = getDragDropText;" Delay="1" />
                                            <Drop Handler="var dropOn = overModel ? ' ' + dropPosition + ' ' + overModel.get('SUB_DIVISION_NAME') : ' Back To Current Contact'; 
                                             Ext.net.Notification.show({title:'Drag from right to left', html:'Transferred ' + data.records[0].get('SUB_DIVISION_NAME') + dropOn});" />
                                        </Listeners>

                                    </ext:GridView>
                                </View>
                            </ext:GridPanel>
                            <ext:Panel
                                ID="Panel2"
                                runat="server"
                                Width="35"
                                BodyStyle="background-color: Lightgrey;"
                                Border="false"
                                Layout="Anchor">

                                <Items>
                                    <ext:Panel ID="Panel1" runat="server" Border="false" BodyStyle="background-color: Lightgrey;" AnchorVertical="40%" />
                                    <ext:Panel ID="Panel3" runat="server" Border="false" BodyStyle="background-color: Lightgrey;" BodyPadding="5">

                                        <Items>
                                            <ext:Button ID="Button1" runat="server" Icon="ResultsetNext">

                                                <Listeners>
                                                    <Click Handler="#{uxTransferNewServiceUnitStore}.loadData(#{uxCurrentSubDivStore}.proxy.data); #{uxCurrentSubDivStore}.removeAll(); #{uxCurrentSubDivStore}.add();" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Label ID="Label1" runat="server" />
                                            <ext:Button ID="ResetTransfer" runat="server" Icon="ResultsetPrevious">
                                                <Listeners>
                                                    <Click Handler="#{uxCurrentSubDivStore}.loadData(#{uxCurrentSubDivStore}.proxy.data); #{uxTransferNewServiceUnitStore}.removeAll();" />
                                                </Listeners>
                                            </ext:Button>

                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            <ext:GridPanel
                                ID="deTransferNewSubDivGrid"
                                runat="server"
                                MultiSelect="true"
                                Title="New Subdivision"
                                Flex="1"
                                Margins="0 0 0 3" AllowDeselect="false">
                                <Store>
                                    <ext:Store ID="uxTransferNewServiceUnitStore" runat="server">
                                        <Model>
                                            <ext:Model ID="Model4" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="SUB_DIVISION_ID" />
                                                    <ext:ModelField Name="SERVICE_UNIT_ID" />
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
                                        <ext:Column ID="Column6" runat="server" Text="Subdivision" Width="280" DataIndex="SUB_DIVISION_NAME" Flex="1" />
                                    </Columns>
                                </ColumnModel>
                                <View>
                                    <ext:GridView ID="GridView2" runat="server">
                                        <Plugins>
                                            <ext:GridDragDrop ID="GridDragDrop2" runat="server" DragGroup="secondGridDDGroup" DropGroup="firstGridDDGroup" />
                                        </Plugins>
                                        <Listeners>
                                            <Select Handler="#{TransferCrossingtoContact}.enable()" />
                                        </Listeners>
                                        <Listeners>
                                            <AfterRender Handler="this.plugins[0].dragZone.getDragText = getDragDropText;" Delay="1" />
                                            <Drop Handler="var dropOn = overModel ? ' ' + dropPosition + ' ' + overModel.get('SUB_DIVISION_NAME') : ' To New Contact'; 
                                               Ext.net.Notification.show({title:'Drag from left to right', html:'Transferred ' + data.records[0].get('SUB_DIVISION_NAME') + dropOn});" />
                                        </Listeners>
                                    </ext:GridView>
                                </View>
                            </ext:GridPanel>
                        </Items>
                        <BottomBar>
                            <ext:Toolbar ID="Toolbar3" runat="server">
                                <Items>
                                    <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                    <ext:Button ID="TransferCrossingtoContact" runat="server" Text="Update" Icon="TransmitGo">
                                        <DirectEvents>
                                            <Click OnEvent="AssociateTransfer">
                                                <Confirmation ConfirmRequest="true" Title="Associate?" Message="Are you sure you want to transfer the selected subdivision(s) to the new service unit?" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="ServiceUnitId" Value="#{deTransferNewSubDivGrid}.getSelectionModel().getSelection()[0].data.SERVICE_UNIT_ID" Mode="Raw" />
                                                    <ext:Parameter Name="selectedSubdivs" Value="Ext.encode(#{deTransferNewSubDivGrid}.getRowsValues())" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="CancelTransfer" runat="server" Text="Cancel" Icon="Delete">
                                        <Listeners>
                                            <Click Handler="#{uxTransferNewServiceUnitStore}.reload();
									                    #{uxTransferSubDivWindow}.hide()" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </BottomBar>
                    </ext:Panel>
                </Items>

            </ext:Window>
              </Items>
             </ext:Viewport>
    </form>
</body>
</html>

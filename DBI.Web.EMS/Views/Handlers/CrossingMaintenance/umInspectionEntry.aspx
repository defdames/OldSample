﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umInspectionEntry.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.InspectionEntry" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
     <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <div></div>
         <ext:GridPanel ID="uxInspectionCrossingGrid" Title="CROSSING LIST FOR INSPECTION ENTRY" runat="server" Layout="HBoxLayout" Collapsible="true">
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" Mode="Single" />
                </SelectionModel>
                <Store>
                    <ext:Store runat="server"
                        ID="uxInspectionEntryCrossingStore"
                        OnReadData="deInspectionGridData"
                        PageSize="10"
                        AutoDataBind="true" WarningOnDirty="false">
                        <Model>
                            <ext:Model ID="Model2" runat="server">
                                <Fields>
                                    <ext:ModelField Name="CROSSING_ID" />
                                    <ext:ModelField Name="CONTACT_ID" />
                                    <ext:ModelField Name="CROSSING_NUMBER" Type="String" />
                                    <ext:ModelField Name="PROJECT_ID" />
                                     <ext:ModelField Name="LONG_NAME" />
                                    <ext:ModelField Name="SERVICE_UNIT" />
                                    <ext:ModelField Name="SUB_DIVISION" />
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

                        <ext:Column ID="uxMainCrossing" runat="server" DataIndex="CROSSING_NUMBER" Text="Crossing #" Flex="1" />
                        <ext:Column ID="Column8" runat="server" DataIndex="LONG_NAME" Text="Project Name" Flex="2" />
                        <ext:Column ID="Column12" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" />
                        <ext:Column ID="uxSubDiv" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />
                        <ext:Column ID="uxMTM" runat="server" DataIndex="CONTACT_NAME" Text="Manager" Flex="1" />

                    </Columns>
                </ColumnModel>
                <Plugins>
                    <ext:FilterHeader ID="FilterHeader1" runat="server" />
                </Plugins>
                 <DirectEvents>
                    <Select OnEvent="GetInspectionGridData">
                        <ExtraParams>
                            <ext:Parameter Name="CrossingId" Value="#{uxInspectionCrossingGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                        </ExtraParams>
                    </Select>
                </DirectEvents>
             
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True">
                    </ext:PagingToolbar>
                </BottomBar>
              <Listeners>
				<Select Handler="#{uxAddInspectButton}.enable();" /> 
                            
			</Listeners>
           
            </ext:GridPanel>
        <ext:Toolbar ID="Toolbar1" runat="server">
                  <Items>
                        <ext:Button ID="uxAddInspectButton" runat="server" Text="Add Entry" Icon="ApplicationAdd" Disabled="true">
                            <Listeners>
								<Click Handler="#{uxAddInspectionEntryWindow}.show()" />
							</Listeners>
                        </ext:Button>
                        <ext:Button ID="uxEditInspectButton" runat="server" Text="Edit Entry" Icon="ApplicationEdit" Disabled="true" >
                            <Listeners>
								<Click Handler="#{uxEditInspectionEntryWindow}.show()" />
							</Listeners>
                              <DirectEvents>
                             <Click OnEvent="deEditInspectionForm">
                                 <ExtraParams>
						            <ext:Parameter Name="InspectionInfo" Value="Ext.encode(#{uxInspectionEntryGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
					                 </ExtraParams>
                             </Click>
                            </DirectEvents>

                        </ext:Button>
                        <ext:Button ID="uxDeleteInspectButton" runat="server" Text="Delete Entry" Icon="ApplicationDelete" Disabled="true" >
                            <DirectEvents>
								<Click OnEvent="deRemoveInspectionEntry">
									<Confirmation ConfirmRequest="true" Title="Remove?" Message="Are you sure you want to delete this inspection entry?" />
						             <ExtraParams>
						            <ext:Parameter Name="InspectionInfo" Value="Ext.encode(#{uxInspectionEntryGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
					                </ExtraParams>
                                     </Click>
                                     
							</DirectEvents>
                        </ext:Button>                     
                </Items>                       
        </ext:Toolbar>
             
                <ext:GridPanel ID="uxInspectionEntryGrid" Title="INSPECTION ENTRIES" runat="server" Region="North" Layout="HBoxLayout" Collapsible="true">
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" AllowDeselect="true" Mode="Single" />
                </SelectionModel>
                <Store>
                    <ext:Store runat="server"
                        ID="uxInspectionStore"
                        PageSize="10"
                        >
                        <Model>
                            <ext:Model ID="Model1" runat="server">
                                <Fields>
                                    <ext:ModelField Name="CROSSING_ID" />
                                    <ext:ModelField Name="INSPECTION_ID" />
                                    <ext:ModelField Name="INSPECTION_NUMBER"  />
                                    <ext:ModelField Name="INSPECTION_DATE" Type="Date"/>
                                     <ext:ModelField Name="TRUCK_NUMBER" />
                                    <ext:ModelField Name="SPRAY" />
                                    <ext:ModelField Name="CUT" />
                                    <ext:ModelField Name="INSPECT" />
                                    <ext:ModelField Name="REMARKS" />

                                </Fields>
                            </ext:Model>
                        </Model>
                        
                    </ext:Store>
                </Store>
                <ColumnModel>
                    <Columns>

                        <ext:Column ID="Column1" runat="server" DataIndex="INSPECTION_NUMBER" Text="Inspection #" Flex="1" />
                        <ext:DateColumn ID="DateColumn2" runat="server" DataIndex="INSPECTION_DATE" Text="Date" Flex="1" Format="MM/dd/yyyy" />
                        <ext:Column ID="Column11" runat="server" DataIndex="TRUCK_NUMBER" Text="Truck" Flex="1" />
                        <ext:Column ID="Column7" runat="server" DataIndex="SPRAY" Text="Spray" Flex="1" />
                        <ext:Column ID="Column9" runat="server" DataIndex="CUT" Text="Cut" Flex="1" />
                        <ext:Column ID="Column10" runat="server" DataIndex="INSPECT" Text="Inspect" Flex="1" />
                        <ext:Column ID="Column5" runat="server" DataIndex="REMARKS" Text="Remarks" Flex="3" />

                    </Columns>
                </ColumnModel>
              <Listeners>
				<Select Handler="#{uxEditInspectButton}.enable();
					#{uxDeleteInspectButton}.enable()" />
				<Deselect Handler="#{uxEditInspectButton}.disable();
					#{uxDeleteInspectButton}.disable()" />
               
			</Listeners>

                

            </ext:GridPanel>
                        
  <%---------------------------------------Hidden Windows-----------------------------------%>      
      
                   <ext:Window runat="server"
			ID="uxAddInspectionEntryWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Add Inspection Entry"
			Width="720">
            <Items>
                 <ext:FormPanel ID="uxAddInspectionForm" runat="server" Layout="FormLayout">
                   <Items>

                        
                        <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                                  <Items>
                                  
                                  <ext:TextField ID="uxAddInspectEntryNumber" runat="server"  FieldLabel="Number" LabelAlign="Right" Width="300" />
                                        
                                  <ext:Checkbox ID="uxAddInspectEntrySprayBox" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="250" />
                                  </Items>
                            </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer0" runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField ID="uxAddInspectEntryDate" runat="server" FieldLabel="Date" LabelAlign="Right" Width="300"/>
                                    
                                    <ext:Checkbox ID="uxAddInspectEntryCutBox" runat="server" FieldLabel="Cut" LabelAlign="Right" Width="250" />
                                    </Items>
                                </ext:FieldContainer>

                            
                                <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                                    <Items>
                                  
                                         <ext:ComboBox ID="uxAddInspectionTruckComboBox"
                                                runat="server"
                                                FieldLabel="Truck #"
                                                LabelAlign="Right"
                                                DisplayField="NAME"
                                                ValueField="NAME"
                                                QueryMode="Local"
                                                TypeAhead="true"  Width="300" >
                                                    <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxAddInspectionTruckStore" AutoDataBind="true" >
                                                        <Model>
                                                            <ext:Model ID="Model5" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="NAME" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                     
                                                    </ext:Store>
                                                </Store>                                                      
                                            </ext:ComboBox> 
                                     <ext:Checkbox ID="uxAddInspectEntryInspectBox" runat="server" FieldLabel="Inspect" LabelAlign="Right" Width="250" />
                                    </Items>                                  
                                </ext:FieldContainer>

                                 <ext:TextArea ID="uxAddInspectEntryRemarks" FieldLabel="Remarks" runat="server" LabelAlign="Right" />
                              </Items>
                        </ext:FormPanel>
                    </Items>
                        <Buttons>
                            <ext:Button ID="uxAddInspectionEntryButton" runat="server" Text="Add" Icon="Add" >
                                <DirectEvents>
                                    <Click OnEvent="deAddInspection" >
                                      <ExtraParams>
                                           <ext:Parameter Name="CrossingId" Value="#{uxInspectionCrossingGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                      </ExtraParams>
                                        </Click>
                                </DirectEvents>
                                </ext:Button>
                            <ext:Button ID="uxCancelInspectionEntryButton" runat="server" Text="Cancel" Icon="Delete" >
                                 <Listeners>
                                    <Click Handler="#{uxAddInspectionForm}.reset();
									#{uxAddInspectionEntryWindow}.hide();" />
                                </Listeners>
                            </ext:Button>
                        </Buttons>               
                </ext:Window>
    <%-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------%>

         <ext:Window runat="server"
			ID="uxEditInspectionEntryWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Edit Inspection Entry"
			Width="720">
            <Items>
                 <ext:FormPanel ID="uxEditInspectionForm" runat="server" Layout="FormLayout">
                   <Items>
                        
                        <ext:FieldContainer ID="FieldContainer3" runat="server" Layout="HBoxLayout">
                                  <Items>
                                 <ext:TextField ID="uxEditInspectEntryNumber" runat="server"  FieldLabel="Number" LabelAlign="Right" Width="300" />
                               
                                <ext:Checkbox ID="uxEditInspectEntrySprayBox" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="250" />
                                  </Items>
                        </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer4" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:DateField ID="uxEditInspectEntryDate" runat="server" FieldLabel="Date" LabelAlign="Right" Width="300"/>
                                
                                     <ext:Checkbox ID="uxEditInspectEntryCutBox" runat="server" FieldLabel="Cut" LabelAlign="Right" Width="250" />
                                    </Items>
                                </ext:FieldContainer>

                            
                                <ext:FieldContainer ID="FieldContainer5" runat="server" Layout="HBoxLayout">
                                    <Items>
                                
                                         <ext:ComboBox ID="uxEditInspectionTruckNumber"
                                                runat="server"
                                                FieldLabel="Truck #"
                                                LabelAlign="Right"
                                                DisplayField="NAME"
                                                ValueField="NAME"
                                                QueryMode="Local"
                                                TypeAhead="true"  Width="300" >
                                                    <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxEditInspectionTruckStore" AutoDataBind="true" >
                                                        <Model>
                                                            <ext:Model ID="Model6" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="NAME" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                        
                                                    </ext:Store>
                                                </Store>
                                                   
                                            </ext:ComboBox>
                                       
                                     <ext:Checkbox ID="uxEditInspectEntryInspectBox" runat="server" FieldLabel="Inspect" LabelAlign="Right" Width="250" />
                                    </Items>                                 
                                </ext:FieldContainer>

                                <ext:TextArea ID="uxEditInspectEntryRemarks" FieldLabel="Remarks" runat="server" LabelAlign="Right" />
                       </Items>
                    </ext:FormPanel>
                 </Items>
                        <Buttons>
                            <ext:Button ID="uxUpdateInspectEntryButton" runat="server" Text="Update" Icon="Add" >
                                 <DirectEvents>
                                    <Click OnEvent="deEditInspection" >
                                      <ExtraParams>
                                           <ext:Parameter Name="InspectionId" Value="#{uxInspectionEntryGrid}.getSelectionModel().getSelection()[0].data.INSPECTION_ID" Mode="Raw" />
                                      </ExtraParams>
                                        </Click>
                                </DirectEvents>
                                </ext:Button>
                            <ext:Button ID="uxCancelUpdateInspectEntryButton" runat="server" Text="Cancel" Icon="Delete" >
                                 <Listeners>
                                    <Click Handler="#{uxEditInspectionForm}.reset();
									#{uxEditInspectionEntryWindow}.hide();" />
                                </Listeners>
                            </ext:Button>
                        </Buttons>              
                </ext:Window>
    </form>
</body>
</html>

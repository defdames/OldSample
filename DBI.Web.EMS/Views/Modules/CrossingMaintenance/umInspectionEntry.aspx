<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umInspectionEntry.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.InspectionEntry" %>

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
         <ext:GridPanel ID="uxCrossingMainGrid" Title="CROSSING LIST FOR INSPECTION ENTRY" runat="server" Region="North" Layout="HBoxLayout" Collapsible="true">
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="true" Mode="Single" />
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
               <%-- <DirectEvents>
                    <Select OnEvent="GetFormData">
                        <ExtraParams>
                            <ext:Parameter Name="CrossingId" Value="#{uxCrossingMainGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                        </ExtraParams>
                    </Select>
                </DirectEvents>
                <DirectEvents>
                    <Select OnEvent="deEditCrossingForm">
                        <ExtraParams>
                            <ext:Parameter Name="CrossingId" Value="#{uxCrossingMainGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                        </ExtraParams>
                    </Select>
                </DirectEvents>--%>

                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True">
                    </ext:PagingToolbar>
                </BottomBar>

            </ext:GridPanel>
        <ext:Toolbar ID="Toolbar1" runat="server">
                  <Items>
                        <ext:Button ID="uxAddAppButton" runat="server" Text="Add Entry" Icon="ApplicationAdd" >
                            <Listeners>
								<Click Handler="#{uxAddNewApplicationEntryWindow}.show()" />
							</Listeners>
                        </ext:Button>
                        <ext:Button ID="uxEditAppButton" runat="server" Text="Edit Entry" Icon="ApplicationEdit" >
                            <Listeners>
								<Click Handler="#{uxEditApplicationEntryWindow}.show()" />
							</Listeners>
                        </ext:Button>
                        <ext:Button ID="uxDeleteAppButton" runat="server" Text="Delete Entry" Icon="ApplicationDelete" >
                           <%-- <DirectEvents>
								<Click OnEvent="deRemoveApplicationEntry">
									<Confirmation ConfirmRequest="true" Title="Remove?" Message="Are you sure you want to delete this application entry?" />
						       </Click>
							</DirectEvents>--%>
                        </ext:Button>                     
                </Items>                       
        </ext:Toolbar>
             
                <ext:GridPanel ID="GridPanel1" Title="INSPECTION ENTRIES" runat="server" Region="North" Layout="HBoxLayout" Collapsible="true">
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" AllowDeselect="true" Mode="Single" />
                </SelectionModel>
                <Store>
                    <ext:Store runat="server"
                        ID="Store1"
                        PageSize="10"
                        AutoDataBind="true" WarningOnDirty="false">
                        <Model>
                            <ext:Model ID="Model1" runat="server">
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

                        <ext:Column ID="Column1" runat="server" DataIndex="" Text="Inspection #" Flex="1" />
                        <ext:Column ID="Column2" runat="server" DataIndex="" Text="Date" Flex="1" />
                        <ext:Column ID="Column11" runat="server" DataIndex="" Text="Truck" Flex="1" />
                        <ext:Column ID="Column3" runat="server" DataIndex="" Text="RailRoad" Flex="1" />
                        <ext:Column ID="Column6" runat="server" DataIndex="" Text="Service Unit" Flex="1" />
                        <ext:Column ID="Column4" runat="server" DataIndex="" Text="Sub-Division" Flex="1" />
                        <ext:Column ID="Column7" runat="server" DataIndex="" Text="Spray" Flex="1" />
                        <ext:Column ID="Column9" runat="server" DataIndex="" Text="Cut" Flex="1" />
                        <ext:Column ID="Column10" runat="server" DataIndex="" Text="Inspect" Flex="1" />
                        <ext:Column ID="Column5" runat="server" DataIndex="" Text="Remarks" Flex="1" />

                    </Columns>
                </ColumnModel>
               
               <%-- <DirectEvents>
                    <Select OnEvent="GetFormData">
                        <ExtraParams>
                            <ext:Parameter Name="CrossingId" Value="#{uxCrossingMainGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                        </ExtraParams>
                    </Select>
                </DirectEvents>
                <DirectEvents>
                    <Select OnEvent="deEditCrossingForm">
                        <ExtraParams>
                            <ext:Parameter Name="CrossingId" Value="#{uxCrossingMainGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                        </ExtraParams>
                    </Select>
                </DirectEvents>--%>

                

            </ext:GridPanel>
                        
  <%---------------------------------------Hidden Windows-----------------------------------%>      
      
                   <ext:Window runat="server"
			ID="uxAddNewApplicationEntryWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Add New Entry"
			Width="720">
            <Items>
                 <ext:FormPanel ID="FormPanel2" runat="server" Layout="FormLayout">
                   <Items>

                        
                        <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                                  <Items>
                                  
                                  <ext:TextField ID="uxAddInspectEntryNumber" runat="server"  FieldLabel="Number" LabelAlign="Right" />
                                        <ext:ComboBox ID="uxAddRailRoadINSPECT"
                                                runat="server"
                                                FieldLabel="Rail Road"
                                                LabelAlign="Right"
                                                DisplayField="project"
                                                ValueField="project"
                                                QueryMode="Local"
                                                TypeAhead="true" TabIndex="2" >
                                                    <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxAddRailRoadStore" >
                                                        <Model>
                                                            <ext:Model ID="Model4" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="project" />
                                                                    <ext:ModelField Name="project" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                    </ext:Store>
                                                </Store>
                                                     <%--<DirectEvents>
                                                         <Select OnEvent="deLoadUnit">
                                                                 <ExtraParams>
										                            <ext:Parameter Name="Type" Value="Add" />
									                            </ExtraParams>   
                                                         </Select>
                                                     </DirectEvents>     
                                            <Listeners>
                                                    <Select Handler="#{uxAddServiceUnitStore}.load()" />
                                                </Listeners>--%>
                                            </ext:ComboBox>
                                  <ext:Checkbox ID="uxAddInspectEntrySprayBox" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="250" />
                                  </Items>
                            </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer0" runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField ID="uxAddInspectEntryDate" runat="server" FieldLabel="Date" LabelAlign="Right" />
                                     <ext:ComboBox ID="uxAddServiceUnitINSPECT" 
                                                runat="server" FieldLabel="Service Unit" 
                                                LabelAlign="Right" 
                                                DisplayField="service_unit"
                                                ValueField="service_unit"
                                                QueryMode="Local" TypeAhead="true" TabIndex="3">
                                                 <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxAddServiceUnitStore"  >
                                                        <Model>
                                                            <ext:Model ID="Model5" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="service_unit"  />
                                                                    <ext:ModelField Name="service_unit" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>                                                  
                                                    </ext:Store>
                                                     </Store>                                        
                                             <%--  <DirectEvents>
                                                   <Select OnEvent="deLoadSubDiv">
                                                       <ExtraParams>
										                  <ext:Parameter Name="Type" Value="Add" />
									                    </ExtraParams>
                                                   </Select>
                                               </DirectEvents>
                                                <Listeners>
                                                    <Select Handler="#{uxAddSubDivStore}.load()" />
                                                </Listeners>--%>
                                            </ext:ComboBox>
                                
                                    <ext:Checkbox ID="uxAddInspectEntryCutBox" runat="server" FieldLabel="Cut" LabelAlign="Right" Width="250" />
                                    </Items>
                                </ext:FieldContainer>

                            
                                <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:TextField ID="uxAddInspectEntryTruckNum" runat="server" FieldLabel="Truck #" AnchorHorizontal="100%" LabelAlign="Right" />
                                         <ext:ComboBox ID="uxAddSubDivINSPECT"
                                                 runat="server" 
                                                FieldLabel="Sub-Division" 
                                                LabelAlign="Right" 
                                                AnchorHorizontal="100%" 
                                                DisplayField="sub_division"
                                                ValueField="sub_division"
                                                TypeAhead="true" TabIndex="4">
                                                 <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxAddSubDivStore"  >
                                                        <Model>
                                                            <ext:Model ID="Model7" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="sub_division"  />
                                                                    <ext:ModelField Name="sub_division" />
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
                            <ext:Button ID="uxAddInspectionEntryButton" runat="server" Text="Add" Icon="Add" />
                            <ext:Button ID="uxCancelInspectionEntryButton" runat="server" Text="Cancel" Icon="Delete" />
                        </Buttons>               
                </ext:Window>
    <%-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------%>

         <ext:Window runat="server"
			ID="uxEditApplicationEntryWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Edit Existing Entry"
			Width="720">
            <Items>
                 <ext:FormPanel ID="FormPanel1" runat="server" Layout="FormLayout">
                   <Items>
                        
                        <ext:FieldContainer ID="FieldContainer3" runat="server" Layout="HBoxLayout">
                                  <Items>
                                 <ext:TextField ID="uxEditInspectEntryNumber" runat="server"  FieldLabel="Number" LabelAlign="Right" />
                                <ext:ComboBox ID="uxEditRRINSPECT" runat="server" FieldLabel="Rail Road" AnchorHorizontal="100%" LabelAlign="Right"
                                                 
                                                DisplayField="project"
                                                ValueField="project"
                                                QueryMode="Local"
                                                TypeAhead="true"  TabIndex="2">
                                                    <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxEditRRStore" >
                                                        <Model>
                                                            <ext:Model ID="Model3" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="project" />
                                                                    <ext:ModelField Name="project" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                    </ext:Store>
                                                </Store>
                                                    <%-- <DirectEvents>
                                                         <Select OnEvent="deLoadUnit">
                                                             <ExtraParams>
										                  <ext:Parameter Name="Type" Value="Edit" />
									                    </ExtraParams>
                                                               </Select>
                                                     </DirectEvents>                                                           
                                            <Listeners>
                                                    <Select Handler="#{uxEditServiceUnitStore}.load()" />
                                                </Listeners>--%>
                                            </ext:ComboBox>
                                <ext:Checkbox ID="uxEditInspectEntrySprayBox" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="250" />
                                  </Items>
                        </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer4" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:DateField ID="uxEditInspectEntryDate" runat="server" FieldLabel="Date" LabelAlign="Right" />
                                 <ext:ComboBox ID="uxEditServiceUnitINSPECT"
                                                 runat="server" 
                                                FieldLabel="Service Unit"
                                                DisplayField="service_unit"
                                                ValueField="service_unit"
                                                 LabelAlign="Right" 
                                                AnchorHorizontal="100%" TabIndex="3" >
                                                <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxEditServiceUnitStore" >
                                                        <Model>
                                                            <ext:Model ID="Model8" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="service_unit" />
                                                                    <ext:ModelField Name="service_unit" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                    </ext:Store>
                                                </Store>
                                                    <%-- <DirectEvents>
                                                         <Select OnEvent="deLoadSubDiv">
                                                              <ExtraParams>
										                  <ext:Parameter Name="Type" Value="Edit" />
									                    </ExtraParams>
                                                         </Select>
                                                     </DirectEvents>     
                                            <Listeners>
                                                    <Select Handler="#{uxEditSubDivStore}.load()" />
                                                </Listeners>--%>
                                            </ext:ComboBox>
                                     <ext:Checkbox ID="uxEditInspectEntryCutBox" runat="server" FieldLabel="Cut" LabelAlign="Right" Width="250" />
                                    </Items>
                                </ext:FieldContainer>

                            
                                <ext:FieldContainer ID="FieldContainer5" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:DropDownFIeld ID="uxEditInspectEntryTruckNum" runat="server" FieldLabel="Truck #" AnchorHorizontal="100%" LabelAlign="Right" />
                                         <ext:ComboBox ID="uxEditSubDivINSPECT" 
                                                runat="server" FieldLabel="Sub-Division"
                                                 LabelAlign="Right"
                                                 DisplayField="sub_division"
                                                 ValueField="sub_division"
                                                 AnchorHorizontal="100%" TabIndex="4" >
                                                <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxEditSubDivStore" >
                                                        <Model>
                                                            <ext:Model ID="Model9" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="sub_division" />
                                                                    <ext:ModelField Name="sub_division" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                    </ext:Store>
                                                </Store>
                                                    
                                            </ext:ComboBox>
                                     <ext:Checkbox ID="uxEditInspectEntryInspectNum" runat="server" FieldLabel="Inspect" LabelAlign="Right" Width="250" />
                                    </Items>                                 
                                </ext:FieldContainer>

                                <ext:TextArea ID="uxEditInspectEntryRemarks" FieldLabel="Remarks" runat="server" LabelAlign="Right" />
                       </Items>
                    </ext:FormPanel>
                 </Items>
                        <Buttons>
                            <ext:Button ID="uxUpdateInspectEntryButton" runat="server" Text="Update" Icon="Add" />
                            <ext:Button ID="uxCancelUpdateInspectEntryButton" runat="server" Text="Cancel" Icon="Delete" />
                        </Buttons>              
                </ext:Window>
    </form>
</body>
</html>

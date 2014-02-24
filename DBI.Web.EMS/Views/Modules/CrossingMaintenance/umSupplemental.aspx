﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSupplemental.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umSupplemental" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
         <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <div> </div>
     <%-- <Service Units Tab>--%>
         <ext:GridPanel ID="uxSupplementalCrossingGrid" Title="CROSSING LIST FOR SUPPLEMENTAL ENTRY" runat="server" Layout="HBoxLayout" Collapsible="true">
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="true" Mode="Single" />
                </SelectionModel>
                <Store>
                    <ext:Store runat="server"
                        ID="uxSupplementalCrossingStore"
                        OnReadData="deSupplementalGridData"
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
                    <Select OnEvent="GetSupplementalGridData">
                        <ExtraParams>
                            <ext:Parameter Name="CrossingId" Value="#{uxSupplementalCrossingGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                        </ExtraParams>
                    </Select>
                </DirectEvents>
               

                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True">
                    </ext:PagingToolbar>
                </BottomBar>

            </ext:GridPanel>
        <ext:Toolbar ID="Toolbar1" runat="server">
                  <Items>
                        <ext:Button ID="uxAddAppButton" runat="server" Text="Add Supplemental" Icon="ApplicationAdd" >
                            <Listeners>
								<Click Handler="#{uxAddNewSupplementalWindow}.show()" />
							</Listeners>
                        </ext:Button>
                        <ext:Button ID="uxEditSuppButton" runat="server" Text="Edit Supplemental" Icon="ApplicationEdit" >
                            <Listeners>
								<Click Handler="#{uxEditSupplementalWindow}.show()" />
							</Listeners>
                            <DirectEvents>
                             <Click OnEvent="deEditSupplementalForm">
                        <ExtraParams>
						<ext:Parameter Name="SupplementalInfo" Value="Ext.encode(#{uxSupplementalGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
					    </ExtraParams>
                             </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="uxDeleteAppButton" runat="server" Text="Delete Supplemental" Icon="ApplicationDelete" >
                            <%--<DirectEvents>
								<Click OnEvent="deRemoveSupplemental">
									<Confirmation ConfirmRequest="true" Title="Remove?" Message="Are you sure you want to delete this supplemental entry?" />
                                    <ExtraParams>
                                    <ext:Parameter Name="CrossingId" Value="#{uxSupplementalCrossingGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                     </ExtraParams>
						       </Click>
							</DirectEvents>--%>
                        </ext:Button>                     
                </Items>                       
        </ext:Toolbar>
             
                <ext:GridPanel ID="uxSupplementalGrid" Title="SUPPLEMENTAL ENTRIES" runat="server" Layout="HBoxLayout" >
               
                <Store>
                    <ext:Store runat="server"
                        ID="uxSupplementalStore">
                        <Model>
                            <ext:Model ID="Model1" runat="server">
                                <Fields>
                                    <ext:ModelField Name="CROSSING_ID" />
                                    <ext:ModelField Name="SUPPLEMENTAL_ID" />
                                    <ext:ModelField Name="APPROVED_DATE" />
                                    <ext:ModelField Name="COMPLETED_DATE" />
                                    <ext:ModelField Name="SERVICE_TYPE" />
                                    <ext:ModelField Name="TRUCK_NUMBER" />
                                    <ext:ModelField Name="INSPECT_START" />
                                    <ext:ModelField Name="INSPECT_END"  />
                                    <ext:ModelField Name="SPRAY" />
                                    <ext:ModelField Name="CUT" />
                                    <ext:ModelField Name="MAINTAIN" />
                                    <ext:ModelField Name="INSPECT" />
                                    <ext:ModelField Name="RECURRING" />
                                     <ext:ModelField Name="REMARKS" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>
                <ColumnModel>
                    <Columns>

                        <ext:Column ID="Column1" runat="server" DataIndex="APPROVED_DATE" Text="Approved Date" Flex="1" />
                        <ext:Column ID="Column2" runat="server" DataIndex="COMPLETED_DATE" Text="Completed Date" Flex="1" />
                        <ext:Column ID="Column6" runat="server" DataIndex="SERVICE_TYPE" Text="Service Type" Flex="1" />
                        <ext:Column ID="Column3" runat="server" DataIndex="TRUCK_NUMBER" Text="Truck" Flex="1" />
                        <ext:Column ID="Column4" runat="server" DataIndex="INSPECT_START" Text="Inspection Start" Flex="1" />
                        <ext:Column ID="Column7" runat="server" DataIndex="INSPECT_END" Text="Inspection End" Flex="1" />
                        <ext:Column ID="Column10" runat="server" DataIndex="SPRAY" Text="Spray" Flex="1" />
                        <ext:Column ID="Column9" runat="server" DataIndex="CUT" Text="Cut" Flex="1" />
                        <ext:Column ID="Column11" runat="server" DataIndex="MAINTAIN" Text="Maintain" Flex="1" />
                        <ext:Column ID="Column14" runat="server" DataIndex="INSPECT" Text="Inspect" Flex="1" />
                        <ext:Column ID="Column13" runat="server" DataIndex="RECURRING" Text="Recurring" Flex="1" />                       
                        <ext:Column ID="Column5" runat="server" DataIndex="REMARKS" Text="Remarks" Flex="1" />

                    </Columns>
                </ColumnModel>
                 <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" AllowDeselect="true" Mode="Single" />
                </SelectionModel>
                    
              

                

            </ext:GridPanel>
                        
                 
  <%-------------------------------------------------Hidden Windows------------------------------------------------------%>

             <ext:Window runat="server"
			ID="uxAddNewSupplementalWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Add New Supplemental"
			Width="770">
            <Items>
                 <ext:FormPanel ID="uxAddSupplementalForm" runat="server" Layout="FormLayout">
                   <Items>
                            <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                                  <Items>
                                 <ext:DateField ID="uxAddApprovedDateField" runat="server" FieldLabel="Approved Date" AnchorHorizontal="100%" LabelAlign="Right" />
                                 <ext:TextField ID="uxAddServiceType" runat="server"  FieldLabel="Service Type" LabelAlign="Right" />  
                                 <ext:DateField ID="uxAddInspectStartDateField" runat="server" FieldLabel="Inspection Start" AnchorHorizontal="100%" LabelAlign="Right" />                        
                                  </Items>
                            </ext:FieldContainer>
                    
                            <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                               <Items>
                              
                               <ext:DateField ID="uxAddCompleteDateField" runat="server" FieldLabel="Completed Date" AnchorHorizontal="100%" LabelAlign="Right" />
                               <ext:TextField ID="uxAddTruck" runat="server" FieldLabel="Truck" AnchorHorizontal="100%" LabelAlign="Right"/>
                               <ext:DateField ID="uxAddInspectEndDateField" runat="server" FieldLabel="Inspection End" AnchorHorizontal="100%" LabelAlign="Right" />
                               
                              
                              </Items>                                  
                            </ext:FieldContainer>

                            <ext:FieldContainer ID="FieldContainer3" runat="server" Layout="HBoxLayout">
                               <Items>
                               <ext:Checkbox ID="uxAddSpray" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="100" />
                               <ext:Checkbox ID="uxAddCut" runat="server" FieldLabel="Cut" LabelAlign="Right" Width="100" />
                               <ext:Checkbox ID="uxAddMaintainBox" runat="server" FieldLabel="Maintain" LabelAlign="Right"  />
                               <ext:Checkbox ID="uxAddInspect" runat="server" FieldLabel="Inspect" LabelAlign="Right"  />
                               <ext:Checkbox ID="uxAddRecurringBox" runat="server" FieldLabel="Recurring" LabelAlign="Right" />
                               </Items>
                             </ext:FieldContainer>
                                   
                             <ext:TextArea ID="uxAddRemarks" runat="server" FieldLabel="Remarks" AnchorHorizontal="92%" LabelAlign="Right" />
                          </Items>
                        <Buttons>
                            <ext:Button ID="uxAddNewSupplementalButton" runat="server" Text="Add" Icon="Add" >
                                <DirectEvents>
                                    <Click OnEvent="deAddSupplemental" >
                                      <ExtraParams>
                                           <ext:Parameter Name="CrossingId" Value="#{uxSupplementalCrossingGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                      </ExtraParams>
                                        </Click>
                                </DirectEvents>
                                </ext:Button>
                            <ext:Button ID="uxCancelNewSupplementalButton" runat="server" Text="Cancel" Icon="Delete" >
                                 <Listeners>
                                    <Click Handler="#{uxAddSupplementalForm}.reset();
									#{uxAddNewSupplementalWindow}.hide()" />
                                </Listeners>
                            </ext:Button>
                        </Buttons>
                    </ext:FormPanel>
                   </Items>
                </ext:Window>

                <%---------------------------------------------------------------------------------------------------------%>
                  <ext:Window runat="server"
			ID="uxEditSupplementalWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Edit Supplemental"
			Width="770">
            <Items>
                 <ext:FormPanel ID="uxEditSupplementalForm" runat="server" Layout="FormLayout">
                   <Items>
                        <ext:FieldContainer ID="FieldContainer5" runat="server" Layout="HBoxLayout">
                                  <Items>
                               <ext:DateField ID="uxEditApprovedDateField" runat="server" FieldLabel="Approved Date" AnchorHorizontal="100%" LabelAlign="Right" />
                                <ext:TextField ID="uxEditServiceType" runat="server"  FieldLabel="Service Type" LabelAlign="Right" />  
                                <ext:DateField ID="uxEditInspectStartDateField" runat="server" FieldLabel="Inspection Start" AnchorHorizontal="100%" LabelAlign="Right" />                              
                                  </Items>
                            </ext:FieldContainer>    
                            
                                <ext:FieldContainer ID="FieldContainer7" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     
                                     <ext:DateField ID="uxEditCompletedDateField" runat="server" FieldLabel="Completed Date" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:TextField ID="uxEditTruckNumber" runat="server" FieldLabel="Truck" AnchorHorizontal="100%" LabelAlign="Right"/>
                                     <ext:DateField ID="uxEditInspectEndDateField" runat="server" FieldLabel="Inspection End" AnchorHorizontal="100%" LabelAlign="Right" />
                                     
                                    </Items>                                
                                </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer8" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:Checkbox ID="uxEditSprayBox" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="100"/>
                                    <ext:Checkbox ID="uxEditCut" runat="server" FieldLabel="Cut" LabelAlign="Right" Width="100" />
                                    <ext:Checkbox ID="uxEditMaintain" runat="server" FieldLabel="Maintain" LabelAlign="Right" />
                                    <ext:Checkbox ID="uxEditInspectBox" runat="server" FieldLabel="Inspect" LabelAlign="Right"  />
                                    <ext:Checkbox ID="uxEditRecurringBox" runat="server" FieldLabel="Recurring" LabelAlign="Right" />
                                    </Items>
                                 </ext:FieldContainer>

                                
                                   
                                 <ext:TextArea ID="uxEditRemarks" runat="server" FieldLabel="Remarks" AnchorHorizontal="92%" LabelAlign="Right" />
                             </Items>
                        <Buttons>
                            <ext:Button ID="uxEditSupplemental" runat="server" Text="Update" Icon="Add" >
                                <DirectEvents>
                                    <Click OnEvent="deEditSupplemental" >
                                      <ExtraParams>
                                           <ext:Parameter Name="CrossingId" Value="#{uxSupplementalCrossingGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                      </ExtraParams>
                                        </Click>
                                </DirectEvents>
                                </ext:Button>
                            <ext:Button ID="uxCancelSupplemental" runat="server" Text="Cancel" Icon="Delete" >
                                 <Listeners>
                                    <Click Handler="#{uxEditSupplementalForm}.reset();
									#{uxEditSupplementalWindow}.hide()" />
                                </Listeners>
                            </ext:Button>
                        </Buttons>
                    </ext:FormPanel>
                   </Items>
                </ext:Window>
   
    </form>
</body>
</html>

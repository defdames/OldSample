<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSupplemental.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umSupplemental" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
         <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <div>
     <%-- <Service Units Tab>--%>
         <ext:GridPanel ID="uxCrossingMainGrid" Title="CROSSING LIST FOR SUPPLEMENTAL ENTRY" runat="server" Region="North" Layout="HBoxLayout" Collapsible="true">
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="true" Mode="Single" />
                </SelectionModel>
                <Store>
                    <ext:Store runat="server"
                        ID="uxSupplementalCrossingStore"
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
                        <ext:Button ID="uxAddAppButton" runat="server" Text="Add Supplemental" Icon="ApplicationAdd" >
                            <Listeners>
								<Click Handler="#{uxAddNewSupplementalWindow}.show()" />
							</Listeners>
                        </ext:Button>
                        <ext:Button ID="uxEditAppButton" runat="server" Text="Edit Supplemental" Icon="ApplicationEdit" >
                            <Listeners>
								<Click Handler="#{uxEditSupplementalWindow}.show()" />
							</Listeners>
                        </ext:Button>
                        <ext:Button ID="uxDeleteAppButton" runat="server" Text="Delete Supplemental" Icon="ApplicationDelete" >
                           <%-- <DirectEvents>
								<Click OnEvent="deRemoveApplicationEntry">
									<Confirmation ConfirmRequest="true" Title="Remove?" Message="Are you sure you want to delete this application entry?" />
						       </Click>
							</DirectEvents>--%>
                        </ext:Button>                     
                </Items>                       
        </ext:Toolbar>
             
                <ext:GridPanel ID="GridPanel1" Title="SUPPLEMENTAL ENTRIES" runat="server" Region="North" Layout="HBoxLayout" Collapsible="true">
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
                                    <ext:ModelField Name="" />
                                    <ext:ModelField Name="" />
                                    <ext:ModelField Name="" />
                                    <ext:ModelField Name="" />
                                     <ext:ModelField Name="" />
                                    <ext:ModelField Name="" />
                                    <ext:ModelField Name="" />
                                    <ext:ModelField Name="" />

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

                        <ext:Column ID="Column1" runat="server" DataIndex="" Text="Approved Date" Flex="1" />
                        <ext:Column ID="Column2" runat="server" DataIndex="" Text="Completed Date" Flex="1" />
                        <ext:Column ID="Column6" runat="server" DataIndex="" Text="Service Type" Flex="1" />
                        <ext:Column ID="Column3" runat="server" DataIndex="" Text="Truck" Flex="1" />
                        <ext:Column ID="Column4" runat="server" DataIndex="" Text="Inspection Start" Flex="1" />
                        <ext:Column ID="Column7" runat="server" DataIndex="" Text="Inspection End" Flex="1" />
                        <ext:Column ID="Column10" runat="server" DataIndex="" Text="Spray" Flex="1" />
                        <ext:Column ID="Column9" runat="server" DataIndex="" Text="Cut" Flex="1" />
                        <ext:Column ID="Column11" runat="server" DataIndex="" Text="Maintain" Flex="1" />
                        <ext:Column ID="Column14" runat="server" DataIndex="" Text="Inspect" Flex="1" />
                        <ext:Column ID="Column13" runat="server" DataIndex="" Text="Recurring" Flex="1" />                       
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
                        
                 
  <%-------------------------------------------------Hidden Windows------------------------------------------------------%>

             <ext:Window runat="server"
			ID="uxAddNewSupplementalWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Add New Supplemental"
			Width="770">
            <Items>
                 <ext:FormPanel ID="FormPanel2" runat="server" Layout="FormLayout">
                   <Items>
                            <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                                  <Items>
                                 <ext:DateField ID="uxAddNewSUApprovedDateField" runat="server" FieldLabel="Approved Date" AnchorHorizontal="100%" LabelAlign="Right" />
                                 <ext:TextField ID="uxAddNewSUServiceType" runat="server"  FieldLabel="Service Type" LabelAlign="Right" />  
                                 <ext:DateField ID="uxAddNewSUInspectStartDateField" runat="server" FieldLabel="Inspection Start" AnchorHorizontal="100%" LabelAlign="Right" />                        
                                  </Items>
                            </ext:FieldContainer>
                    
                            <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                               <Items>
                              
                               <ext:DateField ID="uxAddNewSUCompleteDateField" runat="server" FieldLabel="Completed Date" AnchorHorizontal="100%" LabelAlign="Right" />
                               <ext:TextField ID="uxAddNewSUTruck" runat="server" FieldLabel="Truck" AnchorHorizontal="100%" LabelAlign="Right"/>
                               <ext:DateField ID="uxAddNewSUInspectEndDateField" runat="server" FieldLabel="Inspection End" AnchorHorizontal="100%" LabelAlign="Right" />
                               
                              
                              </Items>                                  
                            </ext:FieldContainer>

                            <ext:FieldContainer ID="FieldContainer3" runat="server" Layout="HBoxLayout">
                               <Items>
                               <ext:Checkbox ID="uxAddNewSUSpray" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="100" />
                               <ext:Checkbox ID="uxAddNewSUCut" runat="server" FieldLabel="Cut" LabelAlign="Right" Width="100" />
                               <ext:Checkbox ID="uxAddNewSUMaintainBox" runat="server" FieldLabel="Maintain" LabelAlign="Right"  />
                               <ext:Checkbox ID="uxAddNewSUInspect" runat="server" FieldLabel="Inspect" LabelAlign="Right"  />
                               <ext:Checkbox ID="uxAddNewSURecurringBox" runat="server" FieldLabel="Recurring" LabelAlign="Right" />
                               </Items>
                             </ext:FieldContainer>
                                   
                             <ext:TextArea ID="uxAddNewServiceUnitRemarks" runat="server" FieldLabel="Remarks" AnchorHorizontal="92%" LabelAlign="Right" />
                          </Items>
                        <Buttons>
                            <ext:Button ID="uxAddNewServiceUnitButton" runat="server" Text="Add" Icon="Add" />
                            <ext:Button ID="uxCancelNewServiceUnitButton" runat="server" Text="Cancel" Icon="Delete" />
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
                 <ext:FormPanel ID="FormPanel3" runat="server" Layout="FormLayout">
                   <Items>
                        <ext:FieldContainer ID="FieldContainer5" runat="server" Layout="HBoxLayout">
                                  <Items>
                               <ext:DateField ID="uxEditSUApprovedDateField" runat="server" FieldLabel="Approved Date" AnchorHorizontal="100%" LabelAlign="Right" />
                                <ext:TextField ID="uxEditSUServiceType" runat="server"  FieldLabel="Service Type" LabelAlign="Right" />  
                                <ext:DateField ID="uxEditSUInspectStartDateField" runat="server" FieldLabel="Inspection Start" AnchorHorizontal="100%" LabelAlign="Right" />                              
                                  </Items>
                            </ext:FieldContainer>    
                            
                                <ext:FieldContainer ID="FieldContainer7" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     
                                     <ext:DateField ID="uxEditSUCompleteDateField" runat="server" FieldLabel="Completed Date" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:TextField ID="uxEditSUTruck" runat="server" FieldLabel="Truck" AnchorHorizontal="100%" LabelAlign="Right"/>
                                     <ext:DateField ID="uxEditSUInspectEndDateField" runat="server" FieldLabel="Inspection End" AnchorHorizontal="100%" LabelAlign="Right" />
                                     
                                    </Items>                                
                                </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer8" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:Checkbox ID="UxEditSUSprayBox" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="100"/>
                                    <ext:Checkbox ID="uxEditSUCut" runat="server" FieldLabel="Cut" LabelAlign="Right" Width="100" />
                                    <ext:Checkbox ID="uxEditSUMaintain" runat="server" FieldLabel="Maintain" LabelAlign="Right" />
                                    <ext:Checkbox ID="uxEditSUInspectBox" runat="server" FieldLabel="Inspect" LabelAlign="Right"  />
                                    <ext:Checkbox ID="uxEditSURecurringBox" runat="server" FieldLabel="Recurring" LabelAlign="Right" />
                                    </Items>
                                 </ext:FieldContainer>

                                
                                   
                                 <ext:TextArea ID="uxEditSURemarks" runat="server" FieldLabel="Remarks" AnchorHorizontal="92%" LabelAlign="Right" />
                             </Items>
                        <Buttons>
                            <ext:Button ID="Button1" runat="server" Text="Update" Icon="Add" />
                            <ext:Button ID="Button2" runat="server" Text="Cancel" Icon="Delete" />
                        </Buttons>
                    </ext:FormPanel>
                   </Items>
                </ext:Window>
    </div>
    </form>
</body>
</html>

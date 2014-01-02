<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umServiceUnitsTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umServiceUnitsTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" />
            <div>
        <%-- <Service Units Tab>--%>
                 <ext:FormPanel ID="FormPanel1" runat="server" Layout="FormLayout">
                   <Items>
                        <ext:Toolbar ID="Toolbar4" runat="server">
                        <Items>              
                        <ext:Button ID="Button6" runat="server" Text="Add Service Unit" Icon="ApplicationAdd" >
                             <Listeners>
								<Click Handler="#{uxAddNewServiceUnitWindow}.show()" />     
							</Listeners>
                        </ext:Button>
                        <ext:Button ID="Button7" runat="server" Text="Edit Service Unit" Icon="ApplicationEdit" >
                            <Listeners>
								<Click Handler="#{uxEditServiceUnitWindow}.show()" />     
							</Listeners>
                        </ext:Button>
                        <ext:Button ID="Button8" runat="server" Text="Delete Service Unit" Icon="ApplicationDelete" >
                            <DirectEvents>
								<Click OnEvent="deRemoveServiceUnit">
									<Confirmation ConfirmRequest="true" Title="Remove?" Message="Are you sure you want to delete this service unit?" />								
								</Click>
							</DirectEvents>
                        </ext:Button>                      
                  </Items>
                         </ext:Toolbar>

                                <ext:FieldContainer ID="FieldContainer9" runat="server" Layout="HBoxLayout">
                                  <Items>
                                <ext:TextField ID="uxCrossingNumSUTextField" runat="server" FieldLabel="Crossing #" LabelAlign="Right" />
                                <ext:TextField ID="uxServiceTypeSUTextField" runat="server" FieldLabel="Service Type" LabelAlign="Right" />                             
                                <ext:TextField ID="uxProjectNumSU" runat="server" FieldLabel="Project #" AnchorHorizontal="100%" LabelAlign="Right"/>
                                <ext:TextField ID="uxSubDivisionSU" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" />                           
                                  </Items>
                                </ext:FieldContainer>
                          
                                <ext:FieldContainer ID="FieldContainer11" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:TextField ID="uxApprovedDateSU" runat="server" FieldLabel="Approved Date" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:TextField ID="uxCompletedDateSU" runat="server" FieldLabel="Completed Date" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:Checkbox ID="Checkbox1" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="250" />
                                     <ext:Checkbox ID="Checkbox3" runat="server" FieldLabel="Maintain" LabelAlign="Right" Width="250" />
                                    </Items>                                  
                                </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer14" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxTruckField" runat="server" FieldLabel="Truck" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    <ext:TextField ID="uxSquareFeet" runat="server" FieldLabel="Square Feet" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:Checkbox ID="Checkbox2" runat="server" FieldLabel="Cut" LabelAlign="Right" Width="250" />
                                    <ext:Checkbox ID="Checkbox4" runat="server" FieldLabel="Inspect" LabelAlign="Right" Width="250" />
                                    </Items>
                                 </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer15" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxInspectStartSU" runat="server" FieldLabel="Inspection Start" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxInspectEndSU" runat="server" FieldLabel="Inspection End" AnchorHorizontal="100%" LabelAlign="Right" />
                                   <ext:Checkbox ID="Checkbox5" runat="server" FieldLabel="Recurring" LabelAlign="Right" Width="250" />
                                    </Items>
                                 </ext:FieldContainer>
                                   
                                 <ext:TextArea ID="TextArea3" runat="server" FieldLabel="Remarks" AnchorHorizontal="92%" LabelAlign="Right" />
                                     
                   </Items>
               </ext:FormPanel>
  <%-------------------------------------------------Hidden Windows------------------------------------------------------%>

             <ext:Window runat="server"
			ID="uxAddNewServiceUnitWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Add New Service Unit"
			Width="720">
            <Items>
                 <ext:FormPanel ID="FormPanel2" runat="server" Layout="FormLayout">
                   <Items>
                            <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                                  <Items>
                                <ext:TextField ID="uxAddNewSUCrossingNum" runat="server" FieldLabel="Crossing #" LabelAlign="Right" />
                                <ext:TextField ID="uxAddNewSUServiceType" runat="server"  FieldLabel="Service Type" LabelAlign="Right" />
                                <ext:Checkbox ID="uxAddNewSUSpray" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="250" />
                                  </Items>
                            </ext:FieldContainer>

                            <ext:FieldContainer ID="FieldContainer0" runat="server" Layout="HBoxLayout">
                              <Items>
                               <ext:TextField ID="uxAddNewSUProjectNum" runat="server" FieldLabel="Project #" AnchorHorizontal="100%" LabelAlign="Right"/>
                               <ext:TextField ID="uxAddNewSUSubdivision" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" />
                               <ext:Checkbox ID="uxAddNewSUCut" runat="server" FieldLabel="Cut" LabelAlign="Right" Width="250" />
                              </Items>
                            </ext:FieldContainer>
                    
                            <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                               <Items>
                               <ext:TextField ID="uxAddNewSUApprovedDate" runat="server" FieldLabel="Approved Date" AnchorHorizontal="100%" LabelAlign="Right" />
                               <ext:TextField ID="uxAddNewSUCompleteDate" runat="server" FieldLabel="Completed Date" AnchorHorizontal="100%" LabelAlign="Right" />
                               <ext:Checkbox ID="uxAddNewSUMaintainBox" runat="server" FieldLabel="Maintain" LabelAlign="Right" Width="250" />
                               </Items>                                  
                            </ext:FieldContainer>

                            <ext:FieldContainer ID="FieldContainer3" runat="server" Layout="HBoxLayout">
                               <Items>
                               <ext:TextField ID="uxAddNewSUTruck" runat="server" FieldLabel="Truck" AnchorHorizontal="100%" LabelAlign="Right"/>
                               <ext:TextField ID="uxAddNewSUSquareFeet" runat="server" FieldLabel="Square Feet" AnchorHorizontal="100%" LabelAlign="Right" />
                               <ext:Checkbox ID="uxAddNewSUInspect" runat="server" FieldLabel="Inspect" LabelAlign="Right" Width="250" />
                               </Items>
                             </ext:FieldContainer>

                             <ext:FieldContainer ID="FieldContainer4" runat="server" Layout="HBoxLayout">
                                <Items>
                                <ext:TextField ID="uxAddNewSUInspectStart" runat="server" FieldLabel="Inspection Start" AnchorHorizontal="100%" LabelAlign="Right" />
                                <ext:TextField ID="uxAddNewSUInspectEnd" runat="server" FieldLabel="Inspection End" AnchorHorizontal="100%" LabelAlign="Right" />
                                <ext:Checkbox ID="uxAddNewSURecurringBox" runat="server" FieldLabel="Recurring" LabelAlign="Right" Width="250" />
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
			ID="uxEditServiceUnitWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Edit Service Unit"
			Width="720">
            <Items>
                 <ext:FormPanel ID="FormPanel3" runat="server" Layout="FormLayout">
                   <Items>
                        <ext:FieldContainer ID="FieldContainer5" runat="server" Layout="HBoxLayout">
                                  <Items>
                                <ext:TextField ID="uxEditSUCrossingNum" runat="server" FieldLabel="Crossing #" LabelAlign="Right" />
                                <ext:TextField ID="uxEditSUServiceType" runat="server"  FieldLabel="Service Type" LabelAlign="Right" />
                                <ext:Checkbox ID="UxEditSUSprayBox" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="250" />
                                  </Items>
                            </ext:FieldContainer>

                            <ext:FieldContainer ID="FieldContainer6" runat="server" Layout="HBoxLayout">
                                    <Items>
                               <ext:TextField ID="uxEditSUProjectNum" runat="server" FieldLabel="Project #" AnchorHorizontal="100%" LabelAlign="Right"/>
                               <ext:TextField ID="uxEditSUSubdivision" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" />
                               <ext:Checkbox ID="uxEditSUCut" runat="server" FieldLabel="Cut" LabelAlign="Right" Width="250" />
                                    </Items>
                                </ext:FieldContainer>
                            
                                <ext:FieldContainer ID="FieldContainer7" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:TextField ID="uxEditSUApprovedDate" runat="server" FieldLabel="Approved Date" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:TextField ID="uxEditSUCompleteDate" runat="server" FieldLabel="Completed Date" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:Checkbox ID="uxEditSUMaintain" runat="server" FieldLabel="Maintain" LabelAlign="Right" Width="250" />
                                    </Items>                                
                                </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer8" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxEditSUTruck" runat="server" FieldLabel="Truck" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    <ext:TextField ID="uxEditSUSquareFeet" runat="server" FieldLabel="Square Feet" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:Checkbox ID="uxEditSUInspectBox" runat="server" FieldLabel="Inspect" LabelAlign="Right" Width="250" />
                                    </Items>
                                 </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer10" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxEditSUInspectStart" runat="server" FieldLabel="Inspection Start" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditSUInspectEnd" runat="server" FieldLabel="Inspection End" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:Checkbox ID="uxEditSURecurringBox" runat="server" FieldLabel="Recurring" LabelAlign="Right" Width="250" />
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

﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCrossingInfoTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umCrossings" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

         <ext:ResourceManager ID="ResourceManager1" runat="server" />

    <div>

 <%----------------------------------------------------- <CrossingInfo Tab>----------------------------------------------------------------------%>
               <ext:FormPanel runat="server"  Layout="FormLayout">
                   <Items>
                       <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                   
                        <ext:Button ID="uxAddCrossingButton" runat="server" Text="Add New Crossing" Icon="ApplicationAdd" >
                            <Listeners>
								<Click Handler="#{uxAddCrossingWindow}.show()" />     
							</Listeners>
                        </ext:Button>
                            
                        <ext:Button ID="uxEditCrossingsButton" runat="server" Text="Edit Crossing" Icon="ApplicationEdit" >
                            <Listeners>
								<Click Handler="#{uxEditCrossingWindow}.show()" />
							</Listeners>
                        </ext:Button>
                        <ext:Button ID="uxActivateCrossingButton" runat="server" Text="Activate Crossing" Icon="ApplicationGo" />
                        <ext:Button ID="uxDeleteCrossingButton" runat="server" Text="Delete Crossing" Icon="ApplicationDelete" >
                            <DirectEvents>
								<Click OnEvent="deRemoveCrossing">
									<Confirmation ConfirmRequest="true" Title="Remove?" Message="Are you sure you want to delete crossing?" />	
								</Click>
							</DirectEvents>
                        </ext:Button>   
                        </Items>
                       </ext:Toolbar>
                 
                        <ext:FieldSet ID="FieldSet1" runat="server" Title="Crossing Details" >
                            <Items>

                            
                                <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                                <Items>
                                 <ext:TextField ID="uxCrossingNumCI" runat="server" FieldLabel="Crossing #" LabelAlign="Right" AnchorHorizontal="100%" />                                          
                                 <ext:TextField ID="uxRouteCI" runat="server" FieldLabel="Route" AnchorHorizontal="100%" LabelAlign="Right"/>
                                 <ext:TextField ID="uxDOTNumCI" runat="server" FieldLabel="DOT #" LabelAlign="Right" AnchorHorizontal="100%" />                       
                                </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                                    <Items>
                                         <ext:TextField ID="uxProjectNumCI" runat="server" FieldLabel="Project #" AnchorHorizontal="100%" LabelAlign="Right"/>                                       
                                         <ext:TextField ID="uxStreetCI" runat="server" FieldLabel="Street" AnchorHorizontal="100%" LabelAlign="Right" /> 
                                         <ext:TextField ID="uxMPCI" runat="server" FieldLabel="MP" LabelAlign="Right" />                                                
                                    </Items>
                                </ext:FieldContainer>
                                                   
                                <ext:FieldContainer ID="FieldContainer4" runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField ID="uxStateCI" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right"/>                                
                                        <ext:TextField ID="uxCityCI" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right"/>
                                        <ext:TextField ID="uxLatCI" runat="server" FieldLabel="Latitude"  AnchorHorizontal="92%" LabelAlign="Right" />
                                       
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer3" runat="server" Layout="HBoxLayout">
                                    <Items>
                                       <ext:TextField ID="uxSubDivCI" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" />                                        
                                       <ext:TextField ID="uxCountyCI" runat="server" FieldLabel="County" AnchorHorizontal="100%" LabelAlign="Right" />
                                       <ext:TextField ID="uxLongCI" runat="server" FieldLabel="Longitude" AnchorHorizontal="92%" LabelAlign="Right" />
                                    </Items>
                                </ext:FieldContainer>
                                </Items>

                                    </ext:FieldSet>
                                <ext:FieldSet ID="FieldSet2" runat="server" Title="Measurements">
                                    <Items>
                                 <ext:FieldContainer ID="FieldContainer5"  runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label2" runat="server" Text="" Width="140"  />
                                        <ext:Label ID="Label8" runat="server" Text="ROW Widths" Width="130"  />
                                        <ext:Label ID="Label9" runat="server" Text="Extensions" Width="170"  />
                                        </Items>
                                 </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer6" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxNECI" runat="server" FieldLabel="NE" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    <ext:TextField ID="uxNEextCI" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxRowWidthCI" runat="server" FieldLabel="ROW Width" AnchorHorizontal="100%" LabelAlign="Right" />
                                    </Items>
                                 </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer7" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxNWCI" runat="server" FieldLabel="NW" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxNWextCI" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                      <ext:TextField ID="uxPropertyTypeCI" runat="server" FieldLabel="Property Type" AnchorHorizontal="100%" LabelAlign="Right" />
                                    </Items>
                                 </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer8" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxSECI" runat="server" FieldLabel="SE" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxSEextCI" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                   <ext:TextField ID="uxSurfaceCI" runat="server" FieldLabel="Surface" AnchorHorizontal="100%" LabelAlign="Right" /> 
                                    </Items>
                                 </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer33" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxSWCI" runat="server" FieldLabel="SW" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxSWextCI" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />  
                                    <ext:TextField ID="uxCrossingWarningDevice" runat="server" FieldLabel="Warning Device" LabelAlign="Right" />                         
                                    </Items>
                                 </ext:FieldContainer>
                                     </Items>
                                </ext:FieldSet>
                                                   
                             <ext:FieldSet ID="FieldSet3" runat="server" Title="Special Instructions">
                             <Items>
                                <ext:FieldContainer ID="FieldContainer34" runat="server" Layout="HBoxLayout">
                                    <Items>
                                   <ext:TextField ID="uxMTMCI" runat="server" FieldLabel="Manager" AnchorHorizontal="100%" LabelAlign="Right" />                             
                                   <ext:TextField ID="uxMainTracksCI" runat="server" FieldLabel="Main Tracks" AnchorHorizontal="100%" LabelAlign="Right" />
                                   <ext:Checkbox ID="uxSubConCI" runat="server" FieldLabel="Subcontracted" LabelAlign="Right" Width="110" /> 
                                   <ext:Checkbox ID="uxRestrictedBoxCI" runat="server" FieldLabel="Restricted" LabelAlign="Right" Width="550" />                                                     
                                    </Items>
                                    </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer35" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:TextField ID="uxMTMCellCI" runat="server" FieldLabel="MTM Cell #" AnchorHorizontal="100%" LabelAlign="Right"/>
                                     <ext:TextField ID="uxOtherTracksCI" runat="server" FieldLabel="Other Tracks" AnchorHorizontal="100%" LabelAlign="Right"/>
                                     <ext:Checkbox ID="uxFenceEncroachCI" runat="server" FieldLabel="Enchroachment" LabelAlign="Right" Width="110" />
                                     <ext:Checkbox ID="uxOnSpurCI" runat="server" FieldLabel="On Spur" LabelAlign="Right" Width="110" />                                                                                                            
                                    </Items>
                                </ext:FieldContainer>
                    
                                <ext:FieldContainer ID="FieldContainer36" runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField ID="uxMTMOfficeCI" runat="server" FieldLabel="MTM Office #" AnchorHorizontal="100%" LabelAlign="Right" />
                                         <ext:TextField ID="uxMaxSpeedCI" runat="server" FieldLabel="Max Speed" AnchorHorizontal="100%" LabelAlign="Right" />
                                                                       
                                    </Items>
                                </ext:FieldContainer>
                                   
                                  
                                 <ext:TextArea ID="uxSpecialInstructCI" runat="server" FieldLabel="Special Instructions" AnchorHorizontal="92%" LabelAlign="Right" />                         
                                 </Items>
                        </ext:FieldSet>
                                
                       </Items>
                   </ext:FormPanel>
                      
<%-- -------------------------------------------------------------------------------------------------------------------------------------------------------   --%>           
       <%-- -----Hidden Window-------%>
                 <ext:Window runat="server"
			ID="uxAddCrossingWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Add New Crossing"
			Width="850">
            <Items>
                <ext:FormPanel runat="server" Layout="FormLayout">
                    <Items>
               <ext:FieldSet ID="FieldSet4" runat="server" Title="Crossing Details" >
                            <Items>                        
                                <ext:FieldContainer ID="FieldContainer9" runat="server" Layout="HBoxLayout">
                                <Items>
                                 <ext:TextField ID="uxAddCrossingNumCI" runat="server" FieldLabel="Crossing #" LabelAlign="Right" AnchorHorizontal="100%" />                                          
                                 <ext:TextField ID="uxAddRouteCI" runat="server" FieldLabel="Route" AnchorHorizontal="100%" LabelAlign="Right"/>
                                 <ext:TextField ID="uxAddDOTNumCI" runat="server" FieldLabel="DOT #" LabelAlign="Right" AnchorHorizontal="100%" />                       
                                </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer10" runat="server" Layout="HBoxLayout">
                                    <Items>
                                         <ext:TextField ID="uxAddProjectCI" runat="server" FieldLabel="Project #" AnchorHorizontal="100%" LabelAlign="Right"/>                                       
                                         <ext:TextField ID="uxAddStreetCI" runat="server" FieldLabel="Street" AnchorHorizontal="100%" LabelAlign="Right" /> 
                                         <ext:TextField ID="uxAddMPCI" runat="server" FieldLabel="MP" LabelAlign="Right" />                                                
                                    </Items>
                                </ext:FieldContainer>
                                                   
                                <ext:FieldContainer ID="FieldContainer11" runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField ID="uxAddStateCI" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right"/>       
                                        <ext:TextField ID="uxAddCityCI" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right"/>
                                           <ext:TextField ID="uxAddLatCI" runat="server" FieldLabel="Latitude"  AnchorHorizontal="92%" LabelAlign="Right" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer12" runat="server" Layout="HBoxLayout">
                                    <Items>
                                       <ext:TextField ID="uxAddSubDivCI" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" />                                        
                                       <ext:TextField ID="uxAddCountyCI" runat="server" FieldLabel="County" AnchorHorizontal="100%" LabelAlign="Right" />
                                       <ext:TextField ID="uxAddLongCI" runat="server" FieldLabel="Longitude" AnchorHorizontal="92%" LabelAlign="Right" />
                                    </Items>
                                </ext:FieldContainer>
                                </Items>
                                    </ext:FieldSet>

                                <ext:FieldSet ID="FieldSet5" runat="server" Title="Measurements">
                                    <Items>
                                 <ext:FieldContainer ID="FieldContainer13"  runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label1" runat="server" Text="" Width="140"  />
                                        <ext:Label ID="Label3" runat="server" Text="ROW Widths" Width="130"  />
                                        <ext:Label ID="Label4" runat="server" Text="Extensions" Width="170"  />
                                        </Items>
                                 </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer14" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxAddNECI" runat="server" FieldLabel="NE" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    <ext:TextField ID="uxAddNEextCI" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxAddRowWidthCI" runat="server" FieldLabel="ROW Width" AnchorHorizontal="100%" LabelAlign="Right" />
                                    </Items>
                                 </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer15" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxAddNWCI" runat="server" FieldLabel="NW" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxAddNWextCI" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                      <ext:TextField ID="uxAddPropertyTypeCI" runat="server" FieldLabel="Property Type" AnchorHorizontal="100%" LabelAlign="Right" />
                                    </Items>
                                 </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer16" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxAddSECI" runat="server" FieldLabel="SE" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxAddSEextCI" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                   <ext:TextField ID="uxAddSurfaceCI" runat="server" FieldLabel="Surface" AnchorHorizontal="100%" LabelAlign="Right" /> 
                                    </Items>
                                 </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer17" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxAddSWCI" runat="server" FieldLabel="SW" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxAddSWextCI" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />  
                                    <ext:TextField ID="uxAddWarningDeviceCI" runat="server" FieldLabel="Warning Device" LabelAlign="Right" />                         
                                    </Items>
                                 </ext:FieldContainer>
                                     </Items>
                                </ext:FieldSet>
                                                   
                             <ext:FieldSet ID="FieldSet6" runat="server" Title="Special Instructions">
                             <Items>
                                <ext:FieldContainer ID="FieldContainer18" runat="server" Layout="HBoxLayout">
                                    <Items>
                                   <ext:TextField ID="uxAddMTMCI" runat="server" FieldLabel="Manager" AnchorHorizontal="100%" LabelAlign="Right" />                             
                                   <ext:TextField ID="uxAddMainTracksCI" runat="server" FieldLabel="Main Tracks" AnchorHorizontal="100%" LabelAlign="Right" />
                                   <ext:Checkbox ID="uxAddSubConCI" runat="server" FieldLabel="Subcontracted" LabelAlign="Right" Width="110" /> 
                                   <ext:Checkbox ID="uxAddRestrictedCI" runat="server" FieldLabel="Restricted" LabelAlign="Right" Width="550" />                                                     
                                    </Items>
                                    </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer19" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:TextField ID="uxAddMTMCellCI" runat="server" FieldLabel="MTM Cell #" AnchorHorizontal="100%" LabelAlign="Right"/>
                                     <ext:TextField ID="uxAddOtherTracksCI" runat="server" FieldLabel="Other Tracks" AnchorHorizontal="100%" LabelAlign="Right"/>
                                     <ext:Checkbox ID="uxAddFenceEnchroachCI" runat="server" FieldLabel="Enchroachment" LabelAlign="Right" Width="110" />
                                     <ext:Checkbox ID="uxAddOnSpurCI" runat="server" FieldLabel="On Spur" LabelAlign="Right" Width="110" />                                                                                                            
                                    </Items>
                                </ext:FieldContainer>
                    
                                <ext:FieldContainer ID="FieldContainer20" runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField ID="uxAddMTMOfficeCI" runat="server" FieldLabel="MTM Office #" AnchorHorizontal="100%" LabelAlign="Right" />
                                         <ext:TextField ID="uxAddMaxSpeedCI" runat="server" FieldLabel="Max Speed" AnchorHorizontal="100%" LabelAlign="Right" />
                                    </Items>
                                </ext:FieldContainer>
                                  
                                 <ext:TextArea ID="uxAddSpecialInstructCI" runat="server" FieldLabel="Special Instructions" AnchorHorizontal="92%" LabelAlign="Right" />                         
                                 </Items>
                        </ext:FieldSet>
                              </Items>  
                           
                            <Buttons>
                                <ext:Button runat="server" ID="AddCrossingWindowButton" Text="Add" Icon="Add"  />
                                <ext:Button runat="server" ID="CancelCrossingWindowButton" Text="Cancel" Icon="Delete"  />
                            </Buttons>
               </ext:FormPanel>
            </Items>
         </ext:Window>
<%-----------------------------------------------------------------------------------------------------------------------------------------------------%>
                                    
            <ext:Window runat="server"
			ID="uxEditCrossingWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Edit Crossing"
			Width="850">
            <Items>
                  <ext:FormPanel ID="FormPanel1" runat="server" Layout="FormLayout">
                    <Items>
               <ext:FieldSet ID="FieldSet7" runat="server" Title="Crossing Details" >
                            <Items>                        
                                <ext:FieldContainer ID="FieldContainer21" runat="server" Layout="HBoxLayout">
                                <Items>
                                 <ext:TextField ID="uxEditCrossingNumCI" runat="server" FieldLabel="Crossing #" LabelAlign="Right" AnchorHorizontal="100%" />                                          
                                 <ext:TextField ID="uxEditRouteCI" runat="server" FieldLabel="Route" AnchorHorizontal="100%" LabelAlign="Right"/>
                                 <ext:TextField ID="uxEditDOTNumCI" runat="server" FieldLabel="DOT #" LabelAlign="Right" AnchorHorizontal="100%" />                       
                                </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer22" runat="server" Layout="HBoxLayout">
                                    <Items>
                                         <ext:TextField ID="uxEditProjectNumCI" runat="server" FieldLabel="Project #" AnchorHorizontal="100%" LabelAlign="Right"/>                                       
                                         <ext:TextField ID="uxEditStreetCI" runat="server" FieldLabel="Street" AnchorHorizontal="100%" LabelAlign="Right" /> 
                                         <ext:TextField ID="uxEditMPCI" runat="server" FieldLabel="MP" LabelAlign="Right" />                                                
                                    </Items>
                                </ext:FieldContainer>
                                                   
                                <ext:FieldContainer ID="FieldContainer23" runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField ID="uxEditStateCI" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right"/>  
                                        <ext:TextField ID="uxEditCityCI" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right"/>
                                        <ext:TextField ID="uxEditLatCI" runat="server" FieldLabel="Latitude"  AnchorHorizontal="92%" LabelAlign="Right" />                                 
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer24" runat="server" Layout="HBoxLayout">
                                    <Items>
                                       <ext:TextField ID="uxEditSubDivCI" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" />                                        
                                       <ext:TextField ID="uxEditCountyCI" runat="server" FieldLabel="County" AnchorHorizontal="100%" LabelAlign="Right" />
                                       <ext:TextField ID="uxEditLongCI" runat="server" FieldLabel="Longitude" AnchorHorizontal="92%" LabelAlign="Right" />
                                    </Items>
                                </ext:FieldContainer>
                                </Items>

                                    </ext:FieldSet>
                                <ext:FieldSet ID="FieldSet8" runat="server" Title="Measurements">
                                    <Items>
                                 <ext:FieldContainer ID="FieldContainer25"  runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label5" runat="server" Text="" Width="140"  />
                                        <ext:Label ID="Label6" runat="server" Text="ROW Widths" Width="130"  />
                                        <ext:Label ID="Label7" runat="server" Text="Extensions" Width="170"  />
                                        </Items>
                                 </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer26" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxEditNECI" runat="server" FieldLabel="NE" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    <ext:TextField ID="uxEditNEextCI" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditRowWidthCI" runat="server" FieldLabel="ROW Width" AnchorHorizontal="100%" LabelAlign="Right" />
                                    </Items>
                                 </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer27" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxEditNWCI" runat="server" FieldLabel="NW" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditNWextCI" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                      <ext:TextField ID="uxEditPropertyTypeCI" runat="server" FieldLabel="Property Type" AnchorHorizontal="100%" LabelAlign="Right" />
                                    </Items>
                                 </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer28" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxEditSECI" runat="server" FieldLabel="SE" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditSEextCI" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                   <ext:TextField ID="uxEditSurfaceCI" runat="server" FieldLabel="Surface" AnchorHorizontal="100%" LabelAlign="Right" /> 
                                    </Items>
                                 </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer29" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxEditSWCI" runat="server" FieldLabel="SW" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditSWextCI" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />  
                                    <ext:TextField ID="uxEditWarningDeviceCI" runat="server" FieldLabel="Warning Device" LabelAlign="Right" />                         
                                    </Items>
                                 </ext:FieldContainer>
                                     </Items>
                                </ext:FieldSet>
                                                   
                             <ext:FieldSet ID="FieldSet9" runat="server" Title="Special Instructions">
                             <Items>
                                <ext:FieldContainer ID="FieldContainer30" runat="server" Layout="HBoxLayout">
                                    <Items>
                                   <ext:TextField ID="uxEditMTMCI" runat="server" FieldLabel="Manager" AnchorHorizontal="100%" LabelAlign="Right" />                             
                                   <ext:TextField ID="uxEditMainTracksCI" runat="server" FieldLabel="Main Tracks" AnchorHorizontal="100%" LabelAlign="Right" />
                                   <ext:Checkbox ID="uxEditSubConCI" runat="server" FieldLabel="Subcontracted" LabelAlign="Right" Width="110" /> 
                                   <ext:Checkbox ID="uxEditRestrictedCI" runat="server" FieldLabel="Restricted" LabelAlign="Right" Width="550" />                                                     
                                    </Items>
                                    </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer31" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:TextField ID="uxEditMTMCellCI" runat="server" FieldLabel="MTM Cell #" AnchorHorizontal="100%" LabelAlign="Right"/>
                                     <ext:TextField ID="uxEditOtherTracksCI" runat="server" FieldLabel="Other Tracks" AnchorHorizontal="100%" LabelAlign="Right"/>
                                     <ext:Checkbox ID="uxEditFenceEnchroachCI" runat="server" FieldLabel="Enchroachment" LabelAlign="Right" Width="110" />
                                     <ext:Checkbox ID="uxEditOnSpurCI" runat="server" FieldLabel="On Spur" LabelAlign="Right" Width="110" />                                                                                                            
                                    </Items>
                                </ext:FieldContainer>
                    
                                <ext:FieldContainer ID="FieldContainer32" runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField ID="uxEditMTMOfficeCI" runat="server" FieldLabel="MTM Office #" AnchorHorizontal="100%" LabelAlign="Right" />
                                         <ext:TextField ID="uxEditMaxSpeedCI" runat="server" FieldLabel="Max Speed" AnchorHorizontal="100%" LabelAlign="Right" />
                                    </Items>
                                </ext:FieldContainer>
                                   
                                <ext:TextArea ID="uxEditSpecialInstructCI" runat="server" FieldLabel="Special Instructions" AnchorHorizontal="92%" LabelAlign="Right" />                         
                                 </Items>
                        </ext:FieldSet>
                              </Items>  
                           
                            <Buttons>
                                <ext:Button runat="server" ID="uxEditCrossingButton" Text="Update" Icon="Add"  />
                                <ext:Button runat="server" ID="uxCancelEditCrossingButton" Text="Cancel" Icon="Delete"  />
                            </Buttons>
               </ext:FormPanel>
            </Items>
             </ext:Window>    
             
    </div>
    </form>
</body>
</html>

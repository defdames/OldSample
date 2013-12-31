<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCrossingInfoTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umCrossings" %>

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

                                 <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:TextField ID="uxCrossingNumTextField" runat="server" FieldLabel="Crossing #" LabelAlign="Right" />
                                     <ext:TextField ID="uxStateTextField" runat="server" FieldLabel="State" LabelAlign="Right" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxSubdivision" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" />
                                   
                                    <ext:TextField ID="uxCity" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    </Items>

                                </ext:FieldContainer>
                            
                                <ext:FieldContainer ID="FieldContainer8" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:TextField ID="uxCounty" runat="server" FieldLabel="County" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxStreet" runat="server" FieldLabel="Street" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxRoute" runat="server" FieldLabel="Route" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    </Items>
                                    
                                </ext:FieldContainer>
                           
                                  <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                      <Items>
                                    <ext:TextField ID="uxMP" runat="server" FieldLabel="MP" LabelAlign="Right" />
                                    <ext:TextField ID="uxLongitude" runat="server" FieldLabel="Longitude" AnchorHorizontal="92%" LabelAlign="Right" />
                                    <ext:TextField ID="uxLatitude" runat="server" FieldLabel="Latitude"  AnchorHorizontal="92%" LabelAlign="Right" />
                                    </Items>

                                    </ext:FieldContainer>

                                
                                 <ext:FieldContainer  runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label runat="server" Text="" Width="140"  />
                                        <ext:Label runat="server" Text="Row Widths" Width="130"  />
                                        <ext:Label ID="Label2" runat="server" Text="Extensions" Width="170"  />
                                        </Items>
                                 </ext:FieldContainer>


                                 <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxRowNE" runat="server" FieldLabel="NE" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    <ext:TextField ID="uxExtensionNE" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:Checkbox ID="uxSubContractedCheck" runat="server" FieldLabel="SubContracted" LabelAlign="Right" Width="250" />
                                    </Items>
                                 </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxRowNW" runat="server" FieldLabel="NW" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxExtensionNW" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:Checkbox ID="uxThirdAppReq" runat="server" FieldLabel="Third App Required" LabelAlign="Right" Width="250" />
                                    </Items>
                                 </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer3" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxRowSE" runat="server" FieldLabel="SE" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxExtensionSE" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:Checkbox ID="uxFenceEncroach" runat="server" FieldLabel="Fence Enchroachment" LabelAlign="Right" Width="250" />
                                    </Items>
                                 </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer4" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxRowSW" runat="server" FieldLabel="SW" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxExtensionSW" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:Checkbox ID="uxOnSpur" runat="server" FieldLabel="On Spur" LabelAlign="Right" Width="250" />                                   
                                    </Items>
                                 </ext:FieldContainer>
                                   
                                <ext:FieldContainer ID="FieldContainer5" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxRowWidth" runat="server" FieldLabel="ROW Width" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxPropertyType" runat="server" FieldLabel="Property Type" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxSurface" runat="server" FieldLabel="Surface" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxWarningDevice" runat="server" FieldLabel="Warning Device" AnchorHorizontal="100%" LabelAlign="Right" />
                                    </Items>
                                </ext:FieldContainer>
                   
                                 <ext:FieldContainer ID="FieldContainer6" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxMainTracks" runat="server" FieldLabel="Main Tracks" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxOtherTracks" runat="server" FieldLabel="Other Tracks" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    <ext:TextField ID="uxMaxSpeed" runat="server" FieldLabel="Max Speed" AnchorHorizontal="100%" LabelAlign="Right" />
                                    </Items>
                                </ext:FieldContainer>
                    
                                <ext:FieldContainer ID="FieldContainer7" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxMTM" runat="server" FieldLabel="MTM" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxMTMCellNum" runat="server" FieldLabel="MTM Cell #" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    <ext:TextField ID="uxMTMOfficeNum" runat="server" FieldLabel="MTM Office #" AnchorHorizontal="100%" LabelAlign="Right" />
                                    </Items>
                                </ext:FieldContainer>

                                 <ext:TextArea ID="uxRemarks" runat="server" FieldLabel="Remarks" AnchorHorizontal="92%" LabelAlign="Right" />
                                     <ext:TextArea ID="uxSpecInstruct" runat="server" FieldLabel="Special Instructions" AnchorHorizontal="92%" LabelAlign="Right" />
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
                <ext:FormPanel runat="server" ID="HiddenAddForm" Layout="FormLayout" AutoScroll="false">
                    <Items>
                  <ext:FieldContainer ID="FieldContainer9" runat="server" Layout="HBoxLayout">
                           <Items>
                            <ext:TextField ID="uxAddNewCrossingNum" runat="server" FieldLabel="Crossing #" LabelAlign="Right" AnchorHorizontal="100%" />
                                   
                             <ext:TextField ID="uxAddNewCrossingState" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right"/>
                               </Items>
                         </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer10" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:TextField ID="uxAddNewSubdivision" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" />
                                   
                                    <ext:TextField ID="uxAddNewCity" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    </Items>

                                </ext:FieldContainer>
                            
                                <ext:FieldContainer ID="FieldContainer11" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:TextField ID="uxAddNewCounty" runat="server" FieldLabel="County" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxAddNewStreet" runat="server" FieldLabel="Street" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxAddNewCrossingRoute" runat="server" FieldLabel="Route" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    </Items>
                                    
                                </ext:FieldContainer>
                           
                                  <ext:FieldContainer ID="FieldContainer12" runat="server" Layout="HBoxLayout">
                                      <Items>
                                    <ext:TextField ID="uxAddNewCrossingMP" runat="server" FieldLabel="MP" LabelAlign="Right" />
                                    <ext:TextField ID="uxAddNewCrossingLong" runat="server" FieldLabel="Longitude" AnchorHorizontal="92%" LabelAlign="Right" />
                                    <ext:TextField ID="uxAddNewCrossingLat" runat="server" FieldLabel="Latitude"  AnchorHorizontal="92%" LabelAlign="Right" />
                                    </Items>

                                    </ext:FieldContainer>

                                
                                 <ext:FieldContainer ID="FieldContainer13"  runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label1" runat="server" Text="" Width="140"  />
                                        <ext:Label ID="Label3" runat="server" Text="Row Widths" Width="130"  />
                                        <ext:Label ID="Label4" runat="server" Text="Extensions" Width="170"  />
                                        </Items>
                                 </ext:FieldContainer>


                                 <ext:FieldContainer ID="FieldContainer14" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxAddNewCrossingNE" runat="server" FieldLabel="NE" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    <ext:TextField ID="uxAddNewCrossingExtNE" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:Checkbox ID="Checkbox1" runat="server" FieldLabel="SubContracted" LabelAlign="Right" Width="250" />
                                    </Items>
                                 </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer15" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxAddNewCrossingNW" runat="server" FieldLabel="NW" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxAddNewCrossingExtNW" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:Checkbox ID="uxAddNewCrossingThirdAppReq" runat="server" FieldLabel="Third App Required" LabelAlign="Right" Width="250" />
                                    </Items>
                                 </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer16" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxAddNewCrossingSE" runat="server" FieldLabel="SE" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxAddNewCrossingExtSE" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:Checkbox ID="uxAddNewCrossingFence" runat="server" FieldLabel="Fence Enchroachment" LabelAlign="Right" Width="250" />
                                    </Items>
                                 </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer17" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxAddNewCrossingSW" runat="server" FieldLabel="SW" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxAddNewCrossingExtSW" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:Checkbox ID="uxAddNewCrossingOnSpur" runat="server" FieldLabel="On Spur" LabelAlign="Right" Width="110" />    
                                    <ext:TextField ID="uxAddNewCrossingWarningDevice" runat="server" FieldLabel="Warning Device" AnchorHorizontal="100%" LabelAlign="Right" />                            
                                    </Items>
                                 </ext:FieldContainer>
                                   
                                <ext:FieldContainer ID="FieldContainer18" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxAddNewCrossingRowWidth" runat="server" FieldLabel="ROW Width" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxAddNewCrossingPropertyType" runat="server" FieldLabel="Property Type" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxAddNewCrossingSurface" runat="server" FieldLabel="Surface" AnchorHorizontal="100%" LabelAlign="Right" />
                                    
                                    </Items>
                                </ext:FieldContainer>
                   
                                 <ext:FieldContainer ID="FieldContainer19" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxAddNewCrossingMainTracks" runat="server" FieldLabel="Main Tracks" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxAddNewCrossingOtherTracks" runat="server" FieldLabel="Other Tracks" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    <ext:TextField ID="uxAddNewCrossingMaxSpeed" runat="server" FieldLabel="Max Speed" AnchorHorizontal="100%" LabelAlign="Right" />
                                    </Items>
                                </ext:FieldContainer>
                    
                                <ext:FieldContainer ID="FieldContainer20" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxAddNewCrossingMTM" runat="server" FieldLabel="MTM" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxAddNewCrossingMTMCell" runat="server" FieldLabel="MTM Cell #" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    <ext:TextField ID="uxAddNewCrossingMTMOffice" runat="server" FieldLabel="MTM Office #" AnchorHorizontal="100%" LabelAlign="Right" />
                                    </Items>
                                </ext:FieldContainer>

                                 <ext:TextArea ID="uxAddNewCrossingRemarks" runat="server" FieldLabel="Remarks" AnchorHorizontal="92%" LabelAlign="Right" />
                                     <ext:TextArea ID="uxAddNewCrossingSpecInstruct" runat="server" FieldLabel="Special Instructions" AnchorHorizontal="92%" LabelAlign="Right" />
                     
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
                <ext:FormPanel runat="server" ID="FormPanel1" Layout="FormLayout" AutoScroll="false">
                    <Items>
                  <ext:FieldContainer ID="FieldContainer21" runat="server" Layout="HBoxLayout">
                           <Items>
                            <ext:TextField ID="uxEditCrossingCrossingNum" runat="server" FieldLabel="Crossing #" LabelAlign="Right" AnchorHorizontal="100%" />
                            <ext:TextField ID="uxEditCrossingState" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right"/>
                           </Items>
                         </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer22" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:TextField ID="uxEditCrossingSubdivison" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" />
                                     <ext:TextField ID="uxEditCrossingCity" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    </Items>

                                </ext:FieldContainer>
                            
                                <ext:FieldContainer ID="FieldContainer23" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxEditCrossingCounty" runat="server" FieldLabel="County" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditCrossingStreet" runat="server" FieldLabel="Street" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditCrossingRoute" runat="server" FieldLabel="Route" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    </Items>
                                    
                                </ext:FieldContainer>
                           
                                  <ext:FieldContainer ID="FieldContainer24" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxEditCrossingMP" runat="server" FieldLabel="MP" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditCrossingLong" runat="server" FieldLabel="Longitude" AnchorHorizontal="92%" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditCrossingLat" runat="server" FieldLabel="Latitude"  AnchorHorizontal="92%" LabelAlign="Right" />
                                    </Items>

                                    </ext:FieldContainer>

                                
                                 <ext:FieldContainer ID="FieldContainer25"  runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label ID="Label5" runat="server" Text="" Width="140"  />
                                        <ext:Label ID="Label6" runat="server" Text="Row Widths" Width="130"  />
                                        <ext:Label ID="Label7" runat="server" Text="Extensions" Width="170"  />
                                        </Items>
                                 </ext:FieldContainer>


                                 <ext:FieldContainer ID="FieldContainer26" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxEditCrossingNE" runat="server" FieldLabel="NE" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    <ext:TextField ID="uxEditCrossingExtNE" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:Checkbox ID="uxEditCrossingSubConBox" runat="server" FieldLabel="SubContracted" LabelAlign="Right" Width="250" />
                                    </Items>
                                 </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer27" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxEditCrossingNW" runat="server" FieldLabel="NW" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditCrossingExtNW" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:Checkbox ID="uxEditCrossingThirdAppReq" runat="server" FieldLabel="Third App Required" LabelAlign="Right" Width="250" />
                                    </Items>
                                 </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer28" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxEditCrossingSE" runat="server" FieldLabel="SE" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditCrossingExtSE" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:Checkbox ID="uxEditCrossingFence" runat="server" FieldLabel="Fence Enchroachment" LabelAlign="Right" Width="250" />
                                    </Items>
                                 </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer29" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxEditCrossingSW" runat="server" FieldLabel="SW" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditCrossingExtSW" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:Checkbox ID="uxEditCrossingOnSpur" runat="server" FieldLabel="On Spur" LabelAlign="Right" Width="110" />    
                                    <ext:TextField ID="uxEditCrossingWarningDevice" runat="server" FieldLabel="Warning Device" AnchorHorizontal="100%" LabelAlign="Right" />                            
                                    </Items>
                                 </ext:FieldContainer>
                                   
                                <ext:FieldContainer ID="FieldContainer30" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxEditCrossingRowWidth" runat="server" FieldLabel="ROW Width" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditCrossingPropertyType" runat="server" FieldLabel="Property Type" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditCrossingSurface" runat="server" FieldLabel="Surface" AnchorHorizontal="100%" LabelAlign="Right" />
                                    
                                    </Items>
                                </ext:FieldContainer>
                   
                                 <ext:FieldContainer ID="FieldContainer31" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxEditCrossingMainTrack" runat="server" FieldLabel="Main Tracks" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditCrossingOtherTrack" runat="server" FieldLabel="Other Tracks" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    <ext:TextField ID="uxEditCrossingMaxSpeed" runat="server" FieldLabel="Max Speed" AnchorHorizontal="100%" LabelAlign="Right" />
                                    </Items>
                                </ext:FieldContainer>
                    
                                <ext:FieldContainer ID="FieldContainer32" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxEditCrossingMTM" runat="server" FieldLabel="MTM" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditCrossingMTMCell" runat="server" FieldLabel="MTM Cell #" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    <ext:TextField ID="uxEditCrossingMTMOffice" runat="server" FieldLabel="MTM Office #" AnchorHorizontal="100%" LabelAlign="Right" />
                                    </Items>
                                </ext:FieldContainer>

                                 <ext:TextArea ID="uxEditCrossingRemarks" runat="server" FieldLabel="Remarks" AnchorHorizontal="92%" LabelAlign="Right" />
                                     <ext:TextArea ID="uxEditCrossingSpecInstruct" runat="server" FieldLabel="Special Instructions" AnchorHorizontal="92%" LabelAlign="Right" />
                     
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

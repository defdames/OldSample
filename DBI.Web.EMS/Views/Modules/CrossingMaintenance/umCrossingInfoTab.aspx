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
     <ext:Viewport ID="Viewport1" runat="server" Layout="FitLayout">
            <Items>
                <ext:TabPanel ID="TabPanel1" runat="server" >
                    <Items>


                       <%-- <CrossingInfo Tab>--%>
               <ext:FormPanel runat="server" Title="Crossing Information" Layout="FormLayout">
                   <Items>
                       <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                   
                        <ext:Button ID="Button1" runat="server" Text="Add New Crossing" Icon="ApplicationAdd" />
                        <ext:Button ID="Button2" runat="server" Text="Activate Crossing" Icon="ApplicationGo" />
                        <ext:Button ID="Button3" runat="server" Text="Delete Crossing" Icon="ApplicationDelete" />
                        <ext:Button ID="Button4" runat="server" Text="Supplemental" Icon="BulletLightning" />
                        <ext:Button ID="Button5" runat="server" Text="Incidents"  />
                       
                        </Items>
                         </ext:Toolbar>

                       <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                           <Items>
                             <ext:DropDownField ID="uxCrossingNum" runat="server" TriggerIcon="SimpleArrowDown" FieldLabel="Crossing #" LabelAlign="Right">
                                        
                             </ext:DropDownField>

                             <ext:DropDownField ID="uxState" runat="server" TriggerIcon="SimpleArrowDown" FieldLabel="State" LabelAlign="Right" >
                            
                                 </ext:DropDownField>
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
                                    <ext:Checkbox ID="uxThirdAppReq" runat="server" FieldLabel="Third App Requested" LabelAlign="Right" Width="250" />
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
                                    <ext:TextField ID="uxRowSW" runat="server" FieldLabel="NE" AnchorHorizontal="100%" LabelAlign="Right" />
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

                                 <ext:TextArea ID="TextArea1" runat="server" FieldLabel="Remarks" AnchorHorizontal="92%" LabelAlign="Right" />
                                     <ext:TextArea ID="TextArea2" runat="server" FieldLabel="Special Instructions" AnchorHorizontal="92%" LabelAlign="Right" />
                </Items>
               </ext:FormPanel>
<%-- -------------------------------------------------------------------------------------------------------------------------------------------------------   --%>           

                        <%--<ContactsTab>--%>
               <ext:GridPanel runat="server" Title="Contacts" Layout="HBoxLayout">
                   <Store>
				<ext:Store runat="server"
					ID="uxCurrentContactStore"
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
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
							</Fields>
						</ext:Model>
					</Model>
					<%--<Listeners>
						<Load Fn="doMath" />
					</Listeners>--%>
				</ext:Store>
			</Store>
			<ColumnModel>
				<Columns>
					<ext:Column ID="uxNameCON" runat="server" DataIndex="" Text="Manager Name" Flex="1"/>
					<ext:Column ID="uxAddress1CON" runat="server" DataIndex="" Text="Address 1" Flex="1" />
					<ext:Column ID="uxAddress2CON" runat="server" DataIndex="" Text="Address 2" Flex="1" />
					<ext:Column ID="uxCityCON" runat="server" DataIndex="" Text="City" Flex="1" />
					<ext:Column ID="uxStateCON" runat="server" DataIndex="" Text="State" Flex="1" />
					<ext:Column ID="uxZipCON" runat="server"  Text="Zip" DataIndex="" Flex="1" />
					<ext:Column ID="uxEmailCON" runat="server" DataIndex="" Text="Email" Flex="1" />
					<ext:Column runat="server" ID="uxWorkNumCON" Text="Work #" DataIndex="" Flex="1" />
					<ext:Column runat="server" ID="uxCellNumCON" Text="Cell #" DataIndex="" Flex="1" />
					<ext:Column ID="uxRRCON" runat="server" DataIndex="" Text="RR" Flex="1" />
					
				</Columns>
			</ColumnModel>
                   <TopBar>
                   
                        <ext:Toolbar ID="Toolbar2" runat="server">
                        <Items>
                        <ext:Button ID="uxAddContactButton" runat="server" Text="Add New Contact" Icon="ApplicationAdd" />
                        <ext:Button ID="uxEditContactButton" runat="server" Text="Edit Contact" Icon="ApplicationEdit" />
                        <ext:Button ID="uxAssignContactButton" runat="server" Text="Assign Crossing to Contact" Icon="ApplicationGo" />
                        <ext:Button ID="uxDeleteContact" runat="server" Text="Delete Contact" Icon="ApplicationDelete" />
                        </Items>
                            
                         </ext:Toolbar>
                </TopBar>
                   
               </ext:GridPanel>
<%-----------------------------------------------------------------------------------------------------------------------------------------------------%>
                                        <%--  <Sub-Divsons Tab>--%>
                    <ext:FormPanel runat="server" Title="Sub-Divisons" Layout="FormLayout">
                   <Items>

                   </Items>
               </ext:FormPanel>


                                        <%-- <Service Units Tab>--%>
                 <ext:FormPanel ID="FormPanel1" runat="server" Title="Service Units" Layout="FormLayout">
                   <Items>
                        <ext:Toolbar ID="Toolbar4" runat="server">
                        <Items>
                   
                        <ext:Button ID="Button6" runat="server" Text="Add Service Unit" Icon="ApplicationAdd" />
                        <ext:Button ID="Button7" runat="server" Text="Edit Service Unit" Icon="ApplicationGo" />
                        <ext:Button ID="Button8" runat="server" Text="Delete Service Unit" Icon="ApplicationDelete" />
                      
                       
                        </Items>
                         </ext:Toolbar>

                       <ext:FieldContainer ID="FieldContainer9" runat="server" Layout="HBoxLayout">
                           <Items>
                             <ext:DropDownField ID="uxCrossingNumSU" runat="server" TriggerIcon="SimpleArrowDown" FieldLabel="Crossing #" LabelAlign="Right">
                                        
                             </ext:DropDownField>

                             <ext:DropDownField ID="uxServiceTypeSU" runat="server" TriggerIcon="SimpleArrowDown" FieldLabel="Service Type" LabelAlign="Right" >
                            
                                 </ext:DropDownField>
                                </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer10" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxSubDivisionSU" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" />
                                   
                                    <ext:TextField ID="uxProjectNumSU" runat="server" FieldLabel="Project #" AnchorHorizontal="100%" LabelAlign="Right"/>
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
<%---------------------------------------------------------------------------------------------------------------------------------------------------------%>
                                     <%--<Applications data entry tab>--%>
                  <ext:GridPanel ID="GridPanel1" runat="server" Title="Data Entry" Layout="HBoxLayout">
                   <Store>
				<ext:Store runat="server"
					ID="Store1"
					AutoDataBind="true" WarningOnDirty="false">
					<Model>
						<ext:Model ID="Model2" runat="server">
							<Fields>
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
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
					<%--<Listeners>
						<Load Fn="doMath" />
					</Listeners>--%>
				</ext:Store>
			</Store>
			<ColumnModel>
				<Columns>
					<ext:Column ID="uxAppDateDE" runat="server" DataIndex="" Text="Application Date" Flex="1"/>
					<ext:Column ID="uxTruckNumDE" runat="server" DataIndex="" Text="Truck #" Flex="1" />
					<ext:Column ID="uxSprayDE" runat="server" DataIndex="" Text="Spray" Flex="1" />
					<ext:Column ID="uxCutDE" runat="server" DataIndex="" Text="Cut" Flex="1" />
					<ext:Column ID="uxInspectDE" runat="server" DataIndex="" Text="Inspect" Flex="1" />
					<ext:Column runat="server" ID="uxStateDE" Text="State" DataIndex="" Flex="1" />
					<ext:Column ID="uxSubDivisonDE" runat="server" DataIndex="" Text="SubDivision" Flex="1" />
					<ext:Column runat="server" ID="uxCrossingNumDE" Text="Crossing #" DataIndex="" Flex="1" />
					
					
				</Columns>
			</ColumnModel>
                   <TopBar>
                   
                        <ext:Toolbar ID="Toolbar3" runat="server">
                        <Items>
                        <ext:Button ID="uxAddAppButton" runat="server" Text="Add Application Entry" Icon="ApplicationAdd" />
                        <ext:Button ID="uxEditAppButton" runat="server" Text="Edit Application Entry" Icon="ApplicationEdit" />
                        <ext:Button ID="uxDeleteAppButton" runat="server" Text="Delete Application Entry" Icon="ApplicationDelete" />
                       
                        </Items>
                            
                         </ext:Toolbar>
                </TopBar>
                   
               </ext:GridPanel>
                      
                        
               </Items>
                </ext:TabPanel>
                
            </Items>
               
        </ext:Viewport>
        
    </div>
    </form>
</body>
</html>

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
					<ext:Column ID="Column1" runat="server" DataIndex="" Text="Name" Flex="1"/>
					<ext:Column ID="Column2" runat="server" DataIndex="" Text="Address 1" Flex="1" />
					<ext:Column ID="Column3" runat="server" DataIndex="" Text="Address 2" Flex="1" />
					<ext:Column ID="Column4" runat="server" DataIndex="" Text="City" Flex="1" />
					<ext:Column ID="Column5" runat="server" DataIndex="" Text="State" Flex="1" />
					<ext:Column runat="server" ID="uxGallonTotalGrid" Text="Zip" DataIndex="" Flex="1" />
					<ext:Column ID="Column6" runat="server" DataIndex="" Text="Email" Flex="1" />
					<ext:Column runat="server" ID="uxGallonUsedGrid" Text="Work #" DataIndex="" Flex="1" />
					<ext:Column runat="server" ID="uxAcresSprayedGrid" Text="Cell #" DataIndex="" Flex="1" />
					<ext:Column ID="Column7" runat="server" DataIndex="" Text="RR" Flex="1" />
					
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
               <ext:FormPanel runat="server" Title="Sub-Divisons" Layout="FormLayout">
                   <Items>

                   </Items>
               </ext:FormPanel>
                 <ext:FormPanel ID="FormPanel1" runat="server" Title="Service Units" Layout="FormLayout">
                   <Items>

                   </Items>
               </ext:FormPanel>
                    <ext:FormPanel ID="FormPanel2" runat="server" Title="Data Entry" Layout="FormLayout">
                   <Items>

                   </Items>
               </ext:FormPanel>
                      
                        
               </Items>
                </ext:TabPanel>
                
            </Items>
               
        </ext:Viewport>
        
    </div>
    </form>
</body>
</html>

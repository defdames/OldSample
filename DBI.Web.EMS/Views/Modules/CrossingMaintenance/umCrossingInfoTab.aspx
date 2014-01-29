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
            <ext:GridPanel ID="uxCrossingMainGrid" Title="CROSSING INFORMATION" runat="server" Region="North" Layout="HBoxLayout" Collapsible="true">
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="true" Mode="Single" />
                </SelectionModel>
                <Store>
                    <ext:Store runat="server"
                        ID="uxCurrentCrossingStore"
                        OnReadData="deCrossingGridData"
                        PageSize="10"
                        AutoDataBind="true" WarningOnDirty="false">
                        <Model>
                            <ext:Model ID="Model2" runat="server">
                                <Fields>
                                    <ext:ModelField Name="CROSSING_ID" />
                                    <ext:ModelField Name="CONTACT_ID" />
                                    <ext:ModelField Name="CROSSING_NUMBER" Type="String" />
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

                        <ext:Column ID="uxMainCrossingNum" runat="server" DataIndex="CROSSING_NUMBER" Text="Crossing #" Flex="1" />
                        <ext:Column ID="Column1" runat="server" DataIndex="" Text="Project Name" Flex="1" />
                        <ext:Column ID="uxSubDiv" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />
                        <ext:Column ID="uxMTM" runat="server" DataIndex="CONTACT_NAME" Text="Manager" Flex="1" />
                        
                    </Columns>
                </ColumnModel>
                <Plugins>
                    <ext:FilterHeader ID="FilterHeader1" runat="server" />
                </Plugins>
                <DirectEvents>
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
                </DirectEvents>
               
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True">
                    </ext:PagingToolbar>
                </BottomBar>

            </ext:GridPanel>

            <%-- -----------------------------------------------------------------------------------------------------------------------  --%>
            <ext:FormPanel runat="server" ID="uxCrossingForm" Layout="FormLayout">
                <Items>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                          <ext:Button ID="uxAddCrossingButton" runat="server" Text="Add New Crossing" Icon="ApplicationAdd">
                                <Listeners>
                                    <Click Handler="#{uxAddCrossingWindow}.show()" />
                                </Listeners>
                            </ext:Button>
                         <ext:Button ID="uxEditCrossingsButton" runat="server" Text="Edit Crossing" Icon="ApplicationEdit">
                                <Listeners>
                                    <Click Handler="#{uxEditCrossingWindow}.show()" />
                                </Listeners>
                                <DirectEvents>
                                    <Click OnEvent="deEditCrossingForm">
                                        <ExtraParams>
                                            <ext:Parameter Name="CrossingId" Value="#{uxCrossingMainGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>

                            <ext:Button ID="uxActivateCrossingButton" runat="server" Text="Activate Crossing" Icon="ApplicationGo" />
                            <ext:Button ID="uxDeleteCrossingButton" runat="server" Text="Delete Crossing" Icon="ApplicationDelete">
                                <DirectEvents>
                                    <Click OnEvent="deRemoveCrossing">
                                        <Confirmation ConfirmRequest="true" Title="Remove?" Message="Are you sure you want to delete crossing?" />
                                        <ExtraParams>
                                            <ext:Parameter Name="CrossingId" Value="#{uxCrossingMainGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>

                    <ext:FieldSet ID="FieldSet1" runat="server" Title="Crossing Details">
                        <Items>
                            <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxCrossingNumCI" runat="server" FieldLabel="Crossing #" LabelAlign="Right" AnchorHorizontal="100%" DisableKeyFilter="False" />
                                    <ext:TextField ID="uxRouteCI" runat="server" FieldLabel="Route" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxDOTNumCI" runat="server" FieldLabel="DOT #" LabelAlign="Right" AnchorHorizontal="100%" />
                                </Items>
                            </ext:FieldContainer>

                            <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxProjectNumCI" runat="server" FieldLabel="Project #" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxStreetCI" runat="server" FieldLabel="Street" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxMPCI" runat="server" FieldLabel="MP" LabelAlign="Right" />
                                </Items>
                            </ext:FieldContainer>

                            <ext:FieldContainer ID="FieldContainer4" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxStateCI" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxCityCI" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxLatCI" runat="server" FieldLabel="Latitude" AnchorHorizontal="92%" LabelAlign="Right" />

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
                            <ext:FieldContainer ID="FieldContainer5" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:Label ID="Label2" runat="server" Text="" Width="140" />
                                    <ext:Label ID="Label8" runat="server" Text="ROW Widths" Width="130" />
                                    <ext:Label ID="Label9" runat="server" Text="Extensions" Width="170" />
                                </Items>
                            </ext:FieldContainer>

                            <ext:FieldContainer ID="FieldContainer6" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxNECI" runat="server" FieldLabel="NE" AnchorHorizontal="100%" LabelAlign="Right" />
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
                                    <ext:TextField ID="uxAddManagerCI" runat="server" FieldLabel="Manager" AnchorHorizontal="100%" LabelAlign="Right" Width="475" />
                                    
                                </Items>
                            </ext:FieldContainer>

                            <ext:FieldContainer ID="FieldContainer35" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxMainTracksCI" runat="server" FieldLabel="Main Tracks" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:Checkbox ID="uxSubConCI" runat="server" FieldLabel="Subcontracted" LabelAlign="Right" Width="110"  />
                                    <ext:Checkbox ID="uxRestrictedBoxCI" runat="server" FieldLabel="Restricted" LabelAlign="Right" Width="550" />

                                   
                                </Items>
                            </ext:FieldContainer>

                            <ext:FieldContainer ID="FieldContainer36" runat="server" Layout="HBoxLayout">
                                <Items>
                                     <ext:TextField ID="uxOtherTracksCI" runat="server" FieldLabel="Other Tracks" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:Checkbox ID="uxFenceEncroachCI" runat="server" FieldLabel="Encroachment" LabelAlign="Right" Width="110" />
                                    <ext:Checkbox ID="uxOnSpurCI" runat="server" FieldLabel="On Spur" LabelAlign="Right" Width="110" />                                 
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                <Items>
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
                Width="850" Closable="false">
                <Items>
                    <ext:FormPanel runat="server" ID="uxAddCrossingForm" Layout="FormLayout">
                        <Items>
                            <ext:FieldSet ID="FieldSet4" runat="server" Title="Crossing Details">
                                <Items>
                                    <ext:FieldContainer ID="FieldContainer9" runat="server" Layout="HBoxLayout">
                                        <Items>
                                            <ext:TextField ID="uxAddCrossingNumCI" runat="server" FieldLabel="Crossing #" LabelAlign="Right" AnchorHorizontal="100%" />
                                            <ext:TextField ID="uxAddRouteCI" runat="server" FieldLabel="Route" AnchorHorizontal="100%" LabelAlign="Right" />
                                            <ext:TextField ID="uxAddDOTNumCI" runat="server" FieldLabel="DOT #" LabelAlign="Right" AnchorHorizontal="100%" />
                                        </Items>
                                    </ext:FieldContainer>

                                    <ext:FieldContainer ID="FieldContainer10" runat="server" Layout="HBoxLayout">
                                        <Items>
                                            <ext:TextField ID="uxAddProjectCI" runat="server" FieldLabel="Project #" AnchorHorizontal="100%" LabelAlign="Right" />
                                            <ext:TextField ID="uxAddStreetCI" runat="server" FieldLabel="Street" AnchorHorizontal="100%" LabelAlign="Right" />
                                            <ext:TextField ID="uxAddMPCI" runat="server" FieldLabel="MP" LabelAlign="Right" />
                                        </Items>
                                    </ext:FieldContainer>

                                    <ext:FieldContainer ID="FieldContainer11" runat="server" Layout="HBoxLayout">
                                        <Items>
                                            <ext:TextField ID="uxAddStateCI" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right" />
                                            <ext:TextField ID="uxAddCityCI" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right" />
                                            <ext:TextField ID="uxAddLatCI" runat="server" FieldLabel="Latitude" AnchorHorizontal="92%" LabelAlign="Right" />
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
                                    <ext:FieldContainer ID="FieldContainer13" runat="server" Layout="HBoxLayout">
                                        <Items>
                                            <ext:Label ID="Label1" runat="server" Text="" Width="140" />
                                            <ext:Label ID="Label3" runat="server" Text="ROW Widths" Width="130" />
                                            <ext:Label ID="Label4" runat="server" Text="Extensions" Width="170" />
                                        </Items>
                                    </ext:FieldContainer>

                                    <ext:FieldContainer ID="FieldContainer14" runat="server" Layout="HBoxLayout">
                                        <Items>
                                            <ext:TextField ID="uxAddNECI" runat="server" FieldLabel="NE" AnchorHorizontal="100%" LabelAlign="Right" />
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
                                          
                                <ext:DropDownField ID="uxAddManagerCIDropDownField" runat="server" FieldLabel="Manager" AnchorHorizontal="100%" LabelAlign="Right" Width="475" Mode="ValueText" >
                                <Component>
                                    <ext:GridPanel runat="server"
									ID="uxAddManager"
									Layout="HBoxLayout">
									<Store>
										<ext:Store runat="server"
											ID="uxAddManagerStore"
											PageSize="10"
											RemoteSort="true"
											OnReadData="deAddManagerGrid">
											<Model>
												<ext:Model ID="Model6" runat="server">
													<Fields>
                                                        
														<ext:ModelField Name="CONTACT_ID" />
														<ext:ModelField Name="CONTACT_NAME" Type="String" />
                                                        <ext:ModelField Name="CELL_NUMBER" />
                                                        <ext:ModelField Name="WORK_NUMBER" />
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
											<ext:Column ID="Column3" runat="server" Text="Manager Name" DataIndex="CONTACT_NAME" Flex="1"  />
                                            <ext:Column ID="Column2" runat="server" Text="Work Number" DataIndex="WORK_NUMBER" Flex="1" />
                                            <ext:Column ID="Column4" runat="server" Text="Cell Number" DataIndex="CELL_NUMBER" Flex="1" />
										</Columns>
									</ColumnModel>
									<BottomBar>
										<ext:PagingToolbar ID="PagingToolbar3" runat="server" />
									</BottomBar>
									<SelectionModel>
										<ext:RowSelectionModel ID="RowSelectionModel3" runat="server" Mode="Single" />
									</SelectionModel>
                                        <DirectEvents>
										<SelectionChange OnEvent="deStoreAddManagerValue">
											<ExtraParams>
												<ext:Parameter Name="ContactId" Value="#{uxAddManager}.getSelectionModel().getSelection()[0].data.CONTACT_ID" Mode="Raw" />
                                                <ext:Parameter Name="ContactName" Value="#{uxAddManager}.getSelectionModel().getSelection()[0].data.CONTACT_NAME" Mode="Raw" />
												<ext:Parameter Name="Type" Value="AddManager" />
											</ExtraParams>
										</SelectionChange>
                                         </DirectEvents>
									<Plugins>
										<ext:FilterHeader runat="server" ID="uxAddManagerFilter" Remote="true" />
									</Plugins>                                    
								</ext:GridPanel>
                                </Component>
                                </ext:DropDownField>
                                           
                                        </Items>
                                    </ext:FieldContainer>

                                    <ext:FieldContainer ID="FieldContainer19" runat="server" Layout="HBoxLayout">
                                        <Items>
                                            <ext:TextField ID="uxAddMainTracksCI" runat="server" FieldLabel="Main Tracks" AnchorHorizontal="100%" LabelAlign="Right" />
                                            <ext:Checkbox ID="uxAddSubConCI" runat="server" FieldLabel="Subcontracted" LabelAlign="Right" Width="110" />
                                            <ext:Checkbox ID="uxAddRestrictedCI" runat="server" FieldLabel="Restricted" LabelAlign="Right" Width="550" />                           
                                       </Items>
                                    </ext:FieldContainer>

                                    <ext:FieldContainer ID="FieldContainer20" runat="server" Layout="HBoxLayout">
                                        <Items>                                                                  
                                            <ext:TextField ID="uxAddOtherTracksCI" runat="server" FieldLabel="Other Tracks" AnchorHorizontal="100%" LabelAlign="Right" />
                                            <ext:Checkbox ID="uxAddFenceEnchroachCI" runat="server" FieldLabel="Encroachment" LabelAlign="Right" Width="110" />
                                            <ext:Checkbox ID="uxAddOnSpurCI" runat="server" FieldLabel="On Spur" LabelAlign="Right" Width="110" />
                                        </Items>
                                    </ext:FieldContainer>
                                    <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                        <Items>
                                              <ext:NumberField ID="uxAddMaxSpeedCINumberField" runat="server" FieldLabel="Max Speed" AnchorHorizontal="100%" LabelAlign="Right" />                                           
                                        </Items>
                                    </ext:FieldContainer>

                                    <ext:TextArea ID="uxAddSpecialInstructCI" runat="server" FieldLabel="Special Instructions" AnchorHorizontal="92%" LabelAlign="Right" />
                                </Items>
                            </ext:FieldSet>
                        </Items>

                        <Buttons>
                            <ext:Button runat="server" ID="deAddCrossing" Text="Add" Icon="Add">
                                <DirectEvents>
                                    <Click OnEvent="deAddCrossings" />
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button runat="server" ID="CancelCrossing" Text="Cancel" Icon="Delete" >
                                <Listeners>
								<Click Handler="#{uxAddCrossingForm}.reset();
									#{uxAddCrossingWindow}.hide()" />
							</Listeners>
						</ext:Button>
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
                Width="850" Closable="false">
                <Items>
                    <ext:FormPanel ID="uxEditCrossingForm" runat="server" Layout="FormLayout">
                        <Items>
                            <ext:FieldSet ID="FieldSet7" runat="server" Title="Crossing Details">
                                <Items>
                                    <ext:FieldContainer ID="FieldContainer21" runat="server" Layout="HBoxLayout">
                                        <Items>
                                            <ext:TextField ID="uxEditCrossingNumCI" runat="server" FieldLabel="Crossing #" LabelAlign="Right" AnchorHorizontal="100%" />
                                            <ext:TextField ID="uxEditRouteCI" runat="server" FieldLabel="Route" AnchorHorizontal="100%" LabelAlign="Right" />
                                            <ext:TextField ID="uxEditDOTNumCI" runat="server" FieldLabel="DOT #" LabelAlign="Right" AnchorHorizontal="100%" />
                                        </Items>
                                    </ext:FieldContainer>

                                    <ext:FieldContainer ID="FieldContainer22" runat="server" Layout="HBoxLayout">
                                        <Items>
                                            <ext:TextField ID="uxEditProjectNumCI" runat="server" FieldLabel="Project #" AnchorHorizontal="100%" LabelAlign="Right" />
                                            <ext:TextField ID="uxEditStreetCI" runat="server" FieldLabel="Street" AnchorHorizontal="100%" LabelAlign="Right" />
                                            <ext:TextField ID="uxEditMPCI" runat="server" FieldLabel="MP" LabelAlign="Right" />
                                        </Items>
                                    </ext:FieldContainer>

                                    <ext:FieldContainer ID="FieldContainer23" runat="server" Layout="HBoxLayout">
                                        <Items>
                                            <ext:TextField ID="uxEditStateCI" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right" />
                                            <ext:TextField ID="uxEditCityCI" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right" />
                                            <ext:TextField ID="uxEditLatCI" runat="server" FieldLabel="Latitude" AnchorHorizontal="92%" LabelAlign="Right" />
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
                                    <ext:FieldContainer ID="FieldContainer25" runat="server" Layout="HBoxLayout">
                                        <Items>
                                            <ext:Label ID="Label5" runat="server" Text="" Width="140" />
                                            <ext:Label ID="Label6" runat="server" Text="ROW Widths" Width="130" />
                                            <ext:Label ID="Label7" runat="server" Text="Extensions" Width="170" />
                                        </Items>
                                    </ext:FieldContainer>

                                    <ext:FieldContainer ID="FieldContainer26" runat="server" Layout="HBoxLayout">
                                        <Items>
                                            <ext:TextField ID="uxEditNECI" runat="server" FieldLabel="NE" AnchorHorizontal="100%" LabelAlign="Right" />
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
                                          <ext:DropDownField ID="uxEditManagerCI" runat="server" FieldLabel="Manager" AnchorHorizontal="100%" LabelAlign="Right" Width="475" Mode="ValueText" >
                                <Component>
                                    <ext:GridPanel runat="server"
									ID="uxEditManager"
									Layout="HBoxLayout">
									<Store>
										<ext:Store runat="server"
											ID="uxEditManagerStore"
											PageSize="10"
											RemoteSort="true"
											OnReadData="deEditManagerGrid">
											<Model>
												<ext:Model ID="Model1" runat="server">
													<Fields>
														<ext:ModelField Name="CONTACT_ID" Type="Int" />
														<ext:ModelField Name="CONTACT_NAME" Type="String" />
                                                        <ext:ModelField Name="CELL_NUMBER" />
                                                        <ext:ModelField Name="WORK_NUMBER" />
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
											<ext:Column ID="Column5" runat="server" Text="Manager Name" DataIndex="CONTACT_NAME" Flex="1"  />
                                            <ext:Column ID="Column6" runat="server" Text="Work Number" DataIndex="WORK_NUMBER" Flex="1" />
                                            <ext:Column ID="Column7" runat="server" Text="Cell Number" DataIndex="CELL_NUMBER" Flex="1" />
										</Columns>
									</ColumnModel>
									<BottomBar>
										<ext:PagingToolbar ID="PagingToolbar2" runat="server" />
									</BottomBar>
									<SelectionModel>
										<ext:RowSelectionModel ID="RowSelectionModel2" runat="server" Mode="Single" />
									</SelectionModel>
                                        <DirectEvents>
										<SelectionChange OnEvent="deStoreEditManagerValue">
											<ExtraParams>
												<ext:Parameter Name="ContactId" Value="#{uxEditManager}.getSelectionModel().getSelection()[0].data.CONTACT_ID" Mode="Raw" />
												<ext:Parameter Name="ContactName" Value="#{uxEditManager}.getSelectionModel().getSelection()[0].data.CONTACT_NAME" Mode="Raw" />
                                                <ext:Parameter Name="Type" Value="EditManager" />
											</ExtraParams>
										</SelectionChange>
                                         </DirectEvents>
									<Plugins>
										<ext:FilterHeader runat="server" ID="uxEditManagerFilter" Remote="true" />
									</Plugins>                                    
								</ext:GridPanel>
                                </Component>
                                </ext:DropDownField>
                                        </Items>
                                    </ext:FieldContainer>

                                    <ext:FieldContainer ID="FieldContainer31" runat="server" Layout="HBoxLayout">
                                        <Items>
                                              <ext:TextField ID="uxEditMainTracksCI" runat="server" FieldLabel="Main Tracks" AnchorHorizontal="100%" LabelAlign="Right" />
                                            <ext:Checkbox ID="uxEditSubConCI" runat="server" FieldLabel="Subcontracted" LabelAlign="Right" Width="110" />
                                            <ext:Checkbox ID="uxEditRestrictedCI" runat="server" FieldLabel="Restricted" LabelAlign="Right" Width="550" />
                                           
                                            
                                        </Items>
                                    </ext:FieldContainer>

                                    <ext:FieldContainer ID="FieldContainer32" runat="server" Layout="HBoxLayout">
                                        <Items>
                                            <ext:TextField ID="uxEditOtherTracksCI" runat="server" FieldLabel="Other Tracks" AnchorHorizontal="100%" LabelAlign="Right" />
                                            <ext:Checkbox ID="uxEditFenceEnchroachCI" runat="server" FieldLabel="Encroachment" LabelAlign="Right" Width="110" />
                                            <ext:Checkbox ID="uxEditOnSpurCI" runat="server" FieldLabel="On Spur" LabelAlign="Right" Width="110" />
                                         
                                        </Items>
                                    </ext:FieldContainer>
                                    <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                        <Items>
                                               <ext:NumberField ID="uxEditMaxSpeedCINumberField" runat="server" FieldLabel="Max Speed" AnchorHorizontal="100%" LabelAlign="Right" />
                                        </Items>
                                    </ext:FieldContainer>

                                    <ext:TextArea ID="uxEditSpecialInstructCI" runat="server" FieldLabel="Special Instructions" AnchorHorizontal="92%" LabelAlign="Right" />
                                </Items>
                            </ext:FieldSet>
                        </Items>

                        <Buttons>
                            <ext:Button runat="server" ID="deEditCrossing" Text="Update" Icon="Add">
                                <DirectEvents>
                                    <Click OnEvent="deEditCrossings">
                                        <ExtraParams>
                                            <ext:Parameter Name="CrossingId" Value="#{uxCrossingMainGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button runat="server" ID="deCancelEditCrossing" Text="Cancel" Icon="Delete" >
                                 <Listeners>
								<Click Handler="#{uxEditCrossingForm}.reset();
									#{uxEditCrossingWindow}.hide()" />
							</Listeners>
						</ext:Button>
                        </Buttons>
                    </ext:FormPanel>
                </Items>
            </ext:Window>
        </div>
    </form>
</body>
</html>

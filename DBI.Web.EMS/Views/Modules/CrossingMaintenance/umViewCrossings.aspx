<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umViewCrossings.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umViewCrossings" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
   <ext:ResourceManager ID="ResourceManager1" runat="server" />
           <div></div>
       
         <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
                <Items>
     
            <%----------------------------------------------------- <CrossingInfo Tab>----------------------------------------------------------------------%>
            <ext:GridPanel ID="uxCrossingMainGrid" Title="CROSSING INFORMATION" runat="server" Region="North" Layout="HBoxLayout" Collapsible="true">
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="false" Mode="Single" />
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
                                    <ext:ModelField Name="PROJECT_ID" />
                                    <ext:ModelField Name="LONG_NAME" />
                                    <ext:ModelField Name="SERVICE_UNIT" />
                                    <ext:ModelField Name="SUB_DIVISION" />
                                    <ext:ModelField Name="STATE" />
                                    <ext:ModelField Name="STATUS" />
                                    <ext:ModelField Name="RAILROAD" />
                                    <ext:ModelField Name="RAILROAD_ID" />


                                </Fields>
                            </ext:Model>
                        </Model>
                        <Proxy>
                            <ext:PageProxy />
                        </Proxy>
                         <Sorters>
                            <ext:DataSorter Direction="ASC" Property="SERVICE_UNIT" />
                         </Sorters>
                    </ext:Store>
                </Store>
                <ColumnModel>
                    <Columns>

                        <ext:Column ID="uxMainCrossingNum" runat="server" DataIndex="CROSSING_NUMBER" Text="DOT #" Flex="1" />
                        <ext:Column ID="Column12" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" />
                        <ext:Column ID="uxSubDiv" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />
                        <ext:Column ID="uxMTM" runat="server" DataIndex="STATE" Text="State" Flex="1" />
                        <ext:Column ID="Column10" runat="server" DataIndex="STATUS" Text="Status" Flex="1" />


                    </Columns>
                </ColumnModel>
                <Plugins>
                    <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true"/>
                </Plugins>
                <DirectEvents>
                    <Select OnEvent="GetFormData" >
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
            <ext:FormPanel runat="server" ID="uxCrossingForm" Region="Center" Layout="FormLayout" AutoScroll="true">
                <Items>
                    

                    <ext:FieldSet ID="FieldSet1" runat="server" Title="Crossing Details">
                        <Items>
                             <ext:FieldContainer ID="FieldContainer39" runat="server">
                            <Items>
                             <%--<ext:TextField ID="uxCrossingNumCI" runat="server" FieldLabel="Crossing #" LabelAlign="Right" AnchorHorizontal="100%" DisableKeyFilter="False" Width="300" ReadOnly="true" />--%>
                            </Items>
                                 </ext:FieldContainer>
                                 <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxRRCI" runat="server" FieldLabel="Railroad" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true"/>
                                    <ext:TextField ID="uxRouteCI" runat="server" FieldLabel="Route" AnchorHorizontal="100%" LabelAlign="Right"  ReadOnly="true"/>
                                    <ext:TextField ID="uxDOTCI" runat="server" FieldLabel="DOT #" AnchorHorizontal="100%" LabelAlign="Right"  ReadOnly="true"/>
                                    
                                   

                                </Items>
                            </ext:FieldContainer>

                            <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxServiceUnitCI" runat="server" FieldLabel="Service Unit" LabelAlign="Right" AnchorHorizontal="100%" ReadOnly="true"/>
                                    <ext:TextField ID="uxStreetCI" runat="server" FieldLabel="Street" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                    <ext:TextField ID="uxMPCI" runat="server" FieldLabel="MP" LabelAlign="Right" ReadOnly="true" />
                                </Items>
                            </ext:FieldContainer>

                            <ext:FieldContainer ID="FieldContainer4" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxSubDivCI" runat="server" FieldLabel="Sub-Division" LabelAlign="Right" AnchorHorizontal="100%" ReadOnly="true"/>
                                    <ext:TextField ID="uxCityCI" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                    <ext:TextField ID="uxLatCI" runat="server" FieldLabel="Latitude" AnchorHorizontal="92%" LabelAlign="Right" ReadOnly="true"/>
                                </Items>
                            </ext:FieldContainer>

                            <ext:FieldContainer ID="FieldContainer3" runat="server" Layout="HBoxLayout">
                                <Items>
                                   
                                    <ext:TextField ID="uxStateCI" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                    <ext:TextField ID="uxCountyCI" runat="server" FieldLabel="County" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                    <ext:TextField ID="uxLongCI" runat="server" FieldLabel="Longitude" AnchorHorizontal="92%" LabelAlign="Right" ReadOnly="true" />
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
                                    <ext:TextField ID="uxNECI" runat="server" FieldLabel="NE" AnchorHorizontal="100%" LabelAlign="Right"  ReadOnly="true" />
                                    <ext:TextField ID="uxNEextCI" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right"  ReadOnly="true" />
                                    <ext:TextField ID="uxRowWidthCI" runat="server" FieldLabel="ROW Width" AnchorHorizontal="100%" LabelAlign="Right"  ReadOnly="true" />
                                </Items>
                            </ext:FieldContainer>

                            <ext:FieldContainer ID="FieldContainer7" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxNWCI" runat="server" FieldLabel="NW" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                    <ext:TextField ID="uxNWextCI" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                    <ext:TextField ID="uxSurfaceCI" runat="server" FieldLabel="Surface" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                </Items>
                            </ext:FieldContainer>

                            <ext:FieldContainer ID="FieldContainer8" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxSECI" runat="server" FieldLabel="SE" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                    <ext:TextField ID="uxSEextCI" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                    <ext:TextField ID="uxCrossingWarningDevice" runat="server" FieldLabel="Warning Device" LabelAlign="Right" ReadOnly="true" />
                                </Items>
                            </ext:FieldContainer>

                            <ext:FieldContainer ID="FieldContainer33" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxSWCI" runat="server" FieldLabel="SW" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                    <ext:TextField ID="uxSWextCI" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                    <ext:TextField ID="uxPropertyTypeCI" runat="server" FieldLabel="Crossing Type" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                </Items>
                            </ext:FieldContainer>
                        </Items>
                    </ext:FieldSet>

                    <ext:FieldSet ID="FieldSet3" runat="server" Title="Special Instructions">
                        <Items>
                            <ext:FieldContainer ID="FieldContainer34" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxAddManagerCI" runat="server" FieldLabel="Manager" AnchorHorizontal="100%" LabelAlign="Right" Width="475" ReadOnly="true" />

                                </Items>
                            </ext:FieldContainer>

                            <ext:FieldContainer ID="FieldContainer35" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxMainTracksCI" runat="server" FieldLabel="Main Tracks" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                      <ext:Label ID="Label10" runat="server" Text="" Width="25" />
                                    <ext:Checkbox ID="uxSubConCI" runat="server" BoxLabel="Subcontracted" BoxLabelAlign="After" Width="110" ReadOnly="true" />
                                    <ext:Checkbox ID="uxRestrictedBoxCI" runat="server" BoxLabel="Restricted" BoxLabelAlign="After" ReadOnly="true" />
                                </Items>
                            </ext:FieldContainer>

                            <ext:FieldContainer ID="FieldContainer36" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxOtherTracksCI" runat="server" FieldLabel="Other Tracks" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                      <ext:Label ID="Label11" runat="server" Text="" Width="25" />
                                    <ext:Checkbox ID="uxFenceEncroachCI" runat="server" BoxLabel="Encroachment" BoxLabelAlign="After" Width="110" ReadOnly="true" />
                                    <ext:Checkbox ID="uxOnSpurCI" runat="server" BoxLabelAlign="After" BoxLabel="On Spur" Width="110" ReadOnly="true" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer ID="FieldContainer9" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxMaxSpeedCI" runat="server" FieldLabel="Max Speed" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:TextArea ID="uxSpecialInstructCI" runat="server" FieldLabel="Special Instructions" AnchorHorizontal="92%" LabelAlign="Right" ReadOnly="true" />
                               </Items>
                    </ext:FieldSet>

                </Items>
            </ext:FormPanel>
                    </Items>
             </ext:Viewport>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCrossingInfoTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umCrossings" %>

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

                            <ext:Column ID="uxMainCrossingNum" runat="server" DataIndex="CROSSING_NUMBER" Text="Crossing #" Flex="1" />
                            <ext:Column ID="Column12" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" />
                            <ext:Column ID="uxSubDiv" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />
                            <ext:Column ID="uxMTM" runat="server" DataIndex="STATE" Text="State" Flex="1" />
                            <ext:Column ID="Column10" runat="server" DataIndex="STATUS" Text="Status" Flex="1" />


                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
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
                    <Listeners>
                        <Select Handler="#{uxEditCrossingsButton}.enable(); #{uxProjectListButton}.enable(); #{uxDeleteCrossingButton}.enable(); #{uxReactivateCrossingButton}.enable()" />

                    </Listeners>
                </ext:GridPanel>

                <%-- -----------------------------------------------------------------------------------------------------------------------  --%>
                <ext:FormPanel runat="server" ID="uxCrossingForm" Region="Center" Layout="FormLayout" AutoScroll="true">
                    <Items>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button ID="uxAddCrossingButton" runat="server" Text="Add New Crossing" Icon="ApplicationAdd">
                                    <DirectEvents>
                                        <Click OnEvent="deGetRRType">
                                            <ExtraParams>
                                                <ext:Parameter Name="Type" Value="Add" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                   <DirectEvents>
                                       <Click OnEvent="deAddProject" />
                                   </DirectEvents>
                                    <Listeners>
                                        <Click Handler="#{uxAddCrossingWindow}.show()" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="uxEditCrossingsButton" runat="server" Text="Edit Crossing" Icon="ApplicationEdit" Disabled="true">
                                    <Listeners>
                                        <Click Handler="#{uxEditCrossingWindow}.show()" />
                                    </Listeners>
                                    <DirectEvents>
                                        <Click OnEvent="deGetRRType">
                                            <ExtraParams>
                                                <ext:Parameter Name="Type" Value="Edit" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>

                                    <DirectEvents>
                                        <Click OnEvent="deEditCrossingForm">
                                            <ExtraParams>
                                                <ext:Parameter Name="CrossingId" Value="#{uxCrossingMainGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="uxDeleteCrossingButton" runat="server" Text="Delete Crossing" Icon="ApplicationDelete" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deDeleteCrossing">
                                            <Confirmation ConfirmRequest="true" Title="Delete?" Message="Are you sure you want to delete the selected crossing?" />

                                            <ExtraParams>
                                                <ext:Parameter Name="CrossingId" Value="#{uxCrossingMainGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="uxReactivateCrossingButton" runat="server" Text="Reactivate Crossing" Icon="Add" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deReactivateCrossing">
                                            <Confirmation ConfirmRequest="true" Title="Reactivate?" Message="Are you sure you want to reactivate the selected crossing?" />

                                            <ExtraParams>
                                                <ext:Parameter Name="CrossingId" Value="#{uxCrossingMainGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="uxProjectListButton" runat="server" Text="Project List" Icon="ApplicationViewDetail" Disabled="true">
                                    <Listeners>
                                        <Click Handler="#{uxProjectListWindow}.show()" />
                                    </Listeners>
                                    <DirectEvents>
                                        <Click OnEvent="deGetProjectList">
                                            <ExtraParams>
                                                <ext:Parameter Name="CrossingId" Value="#{uxCrossingMainGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Checkbox runat="server" ID="uxToggleClosed" BoxLabel="Unassigned Crossings" BoxLabelAlign="After">
                                    <Listeners>
                                        <Change Handler="#{uxCurrentCrossingStore}.reload()" />
                                    </Listeners>
                                </ext:Checkbox>

                            </Items>
                        </ext:Toolbar>

                        <ext:FieldSet ID="FieldSet1" runat="server" Title="Crossing Details">
                            <Items>
                                <ext:FieldContainer ID="FieldContainer39" runat="server">
                                    <Items>
                                        <%--<ext:TextField ID="uxCrossingNumCI" runat="server" FieldLabel="Crossing #" LabelAlign="Right" AnchorHorizontal="100%" DisableKeyFilter="False" Width="300" ReadOnly="true" />--%>
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField ID="uxRRCI" runat="server" FieldLabel="Railroad" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                        <ext:TextField ID="uxRouteCI" runat="server" FieldLabel="Route" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                        <ext:TextField ID="uxDOTCI" runat="server" FieldLabel="DOT #" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />



                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField ID="uxServiceUnitCI" runat="server" FieldLabel="Service Unit" LabelAlign="Right" AnchorHorizontal="100%" ReadOnly="true" />
                                        <ext:TextField ID="uxStreetCI" runat="server" FieldLabel="Street" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                        <ext:TextField ID="uxMPCI" runat="server" FieldLabel="MP" LabelAlign="Right" ReadOnly="true" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer4" runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField ID="uxSubDivCI" runat="server" FieldLabel="Sub-Division" LabelAlign="Right" AnchorHorizontal="100%" ReadOnly="true" />
                                        <ext:TextField ID="uxCityCI" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                        <ext:TextField ID="uxLatCI" runat="server" FieldLabel="Latitude" AnchorHorizontal="92%" LabelAlign="Right" ReadOnly="true" />
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
                                        <ext:TextField ID="uxNECI" runat="server" FieldLabel="NE" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                        <ext:TextField ID="uxNEextCI" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                        <ext:TextField ID="uxRowWidthCI" runat="server" FieldLabel="ROW Width" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
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
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField ID="uxMaxSpeedCI" runat="server" FieldLabel="Max Speed" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:TextArea ID="uxSpecialInstructCI" runat="server" FieldLabel="Special Instructions" AnchorHorizontal="92%" LabelAlign="Right" ReadOnly="true" />
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
                    Title="Add New Crossing" Height="645" AutoScroll="true"
                    Width="850" Closable="false" Modal="true">
                    <Items>
                        <ext:FormPanel runat="server" ID="uxAddCrossingForm" Layout="FormLayout" >
                       
                            <Items>
                                <ext:FieldSet ID="FieldSet4" runat="server" Title="Crossing Details">
                                    <Items>

                                        <ext:FieldContainer ID="FieldContainer9" runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextField runat="server" ID="uxAddRailRoadCITextField" LabelAlign="Right" FieldLabel="Railroad" AnchorHorizontal="100%" TabIndex="1" />
                                                <ext:TextField ID="uxAddRouteCI" runat="server" FieldLabel="Route" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="5" />
                                                <ext:TextField ID="uxAddDotCI" runat="server" FieldLabel="DOT #" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="9" AllowBlank="false" />
                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer ID="FieldContainer10" runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:ComboBox ID="uxAddServiceUnitCI"
                                                    runat="server" FieldLabel="Service Unit"
                                                    LabelAlign="Right"
                                                    DisplayField="service_unit"
                                                    ValueField="service_unit"
                                                    QueryMode="Local" TypeAhead="true" TabIndex="2" AllowBlank="false">
                                                    <Store>
                                                        <ext:Store runat="server"
                                                            ID="uxAddServiceUnitStore" AutoLoad="false">
                                                            <Model>
                                                                <ext:Model ID="Model5" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="service_unit" />
                                                                        <ext:ModelField Name="service_unit" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <DirectEvents>
                                                        <Select OnEvent="deLoadSubDiv">
                                                            <ExtraParams>
                                                                <ext:Parameter Name="Type" Value="Add" />
                                                            </ExtraParams>
                                                        </Select>
                                                    </DirectEvents>
                                                    <Listeners>
                                                        <Select Handler="#{uxAddSubDivStore}.load()" />
                                                    </Listeners>
                                                </ext:ComboBox>

                                                <ext:TextField ID="uxAddStreetCI" runat="server" FieldLabel="Street" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="6" />

                                                <ext:NumberField ID="uxAddMPCINumberField" runat="server" FieldLabel="MP" LabelAlign="Right" TabIndex="10" AllowBlank="false" />
                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer ID="FieldContainer11" runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:ComboBox ID="uxAddSubDivCI"
                                                    runat="server"
                                                    FieldLabel="Sub-Division"
                                                    LabelAlign="Right"
                                                    AnchorHorizontal="100%"
                                                    DisplayField="sub_division"
                                                    ValueField="sub_division"
                                                    TypeAhead="true" TabIndex="3" AllowBlank="false">
                                                    <Store>
                                                        <ext:Store runat="server"
                                                            ID="uxAddSubDivStore">
                                                            <Model>
                                                                <ext:Model ID="Model7" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="sub_division" />
                                                                        <ext:ModelField Name="sub_division" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>

                                                </ext:ComboBox>

                                                <ext:TextField ID="uxAddCityCI" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="7" />
                                                <ext:NumberField ID="uxAddLatCINumberField" runat="server" FieldLabel="Latitude" AnchorHorizontal="92%" LabelAlign="Right" TabIndex="11" />
                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer ID="FieldContainer12" runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:ComboBox runat="server"
                                                    ID="uxAddStateComboBox"
                                                    FieldLabel="State"
                                                    LabelAlign="Right"
                                                    DisplayField="name"
                                                    ValueField="name"
                                                    QueryMode="Local"
                                                    TypeAhead="true"
                                                    AllowBlank="false"
                                                    ForceSelection="true" TabIndex="4">
                                                    <Store>
                                                        <ext:Store ID="uxAddStateList" runat="server" AutoDataBind="true">
                                                            <Model>
                                                                <ext:Model ID="Model10" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="abbr" />
                                                                        <ext:ModelField Name="name" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                            <Reader>
                                                                <ext:ArrayReader />
                                                            </Reader>
                                                        </ext:Store>
                                                    </Store>
                                                </ext:ComboBox>
                                                <ext:TextField ID="uxAddCountyCI" runat="server" FieldLabel="County" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="8" />

                                                <ext:NumberField ID="uxAddLongCINumberField" runat="server" FieldLabel="Longitude" AnchorHorizontal="92%" LabelAlign="Right" TabIndex="12" />
                                            </Items>
                                        </ext:FieldContainer>
                                        </Items>
                                        </ext:FieldSet>

                                   
                                         <ext:FieldSet ID="FieldSet10" runat="server" Title="Add Project(s)">
                                                 <Items>
                                         
                                                        <ext:GridPanel ID="uxAddProjectGrid" runat="server" Width="700" Margin="5" SelectionMemory="true" TabIndex="35">
                                                            <Store>
                                                                <ext:Store runat="server"
                                                                    ID="uxCurrentSecurityProjectStore"
                                                                    OnReadData="deAddProjectGrid"
                                                                    PageSize="5"
                                                                    AutoLoad="false"
                                                                    AutoDataBind="true" WarningOnDirty="false" RemoteSort="true">
                                                                 
                                                                    <Model>
                                                                        <ext:Model ID="Model3" runat="server" IDProperty="PROJECT_ID">
                                                                            <Fields>

                                                                                <ext:ModelField Name="PROJECT_ID" />
                                                                                <ext:ModelField Name="LONG_NAME" />
                                                                                <ext:ModelField Name="SEGMENT1" />
                                                                                <ext:ModelField Name="ORGANIZATION_NAME" />
                                            
                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>
                                                                    <Proxy>
                                                                        <ext:PageProxy />
                                                                    </Proxy>
                                                                    <Sorters>
                                                                        <ext:DataSorter Direction="ASC" Property="SEGMENT1" />
                                                                    </Sorters>

                                                                </ext:Store>
                                                            </Store>
                                                            <ColumnModel>
                                                                <Columns>
                                                                    <ext:Column ID="Column11" runat="server" DataIndex="SEGMENT1" Text="Project" Flex="1" />
                                                                    <ext:Column ID="Column13" runat="server" DataIndex="LONG_NAME" Text="Project Name" Flex="2" />
                                                                    <ext:Column ID="Column14" runat="server" DataIndex="ORGANIZATION_NAME" Text="Organization Name" Flex="1" />
                                                                </Columns>
                                                            </ColumnModel>
                                                            <Plugins>
                                                                <ext:FilterHeader ID="uxAddProjectFilter" runat="server" Remote="true" />
                                                            </Plugins>
                                                            <SelectionModel>
                                                                <ext:CheckboxSelectionModel ID="CheckboxSelectionModel2" runat="server" Mode="Simple" AllowDeselect="true" />
                                                            </SelectionModel>       
                                                                                                       
                                                                                                                                      
                                                            <BottomBar>
                                                                <ext:PagingToolbar ID="PagingToolbar11" runat="server" HideRefresh="True">
                                                                </ext:PagingToolbar>
                                                            </BottomBar>
                                                          
                                                          <%--  <Listeners>
                                                                <Select Handler="#{uxAddProjectButton}.enable()" />
                                                            </Listeners>--%>
                                                        </ext:GridPanel>
                                                   <%-- </Component>
                                                </ext:DropDownField>--%>
                                      
                                    </Items>
                                </ext:FieldSet>


                                <ext:FieldSet ID="FieldSet5" runat="server" Title="Measurements">
                                    <Items>
                                        <ext:FieldContainer ID="FieldContainer13" runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:Label ID="Label1" runat="server" Text="" Width="140" TabIndex="13" />
                                                <ext:Label ID="Label3" runat="server" Text="ROW Widths" Width="130" TabIndex="17" />
                                                <ext:Label ID="Label4" runat="server" Text="Extensions" Width="170" TabIndex="21" />
                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer ID="FieldContainer14" runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:NumberField ID="uxAddNECINumberField" runat="server" FieldLabel="NE" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="14" />
                                                <ext:NumberField ID="uxAddNEextCINumberField" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="18" />
                                                <ext:TextField ID="uxAddRowWidthCI" runat="server" FieldLabel="ROW Width" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="22" />
                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer ID="FieldContainer15" runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:NumberField ID="uxAddNWCINumberField" runat="server" FieldLabel="NW" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="15" />
                                                <ext:NumberField ID="uxAddNWextCINumberField" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="19" />
                                                <ext:TextField ID="uxAddSurfaceCI" runat="server" FieldLabel="Surface" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="24" />
                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer ID="FieldContainer16" runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:NumberField ID="uxAddSECINumberField" runat="server" FieldLabel="SE" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="16" />
                                                <ext:NumberField ID="uxAddSEextCINumberField" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="20" />
                                                <ext:TextField ID="uxAddWarningDeviceCI" runat="server" FieldLabel="Warning Device" LabelAlign="Right" TabIndex="25" />
                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer ID="FieldContainer17" runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:NumberField ID="uxAddSWCINumberField" runat="server" FieldLabel="SW" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="17" />
                                                <ext:NumberField ID="uxAddSWextCINumberField" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="21" />
                                                <ext:ComboBox runat="server"
                                                    ID="uxAddPropertyTypeComboBox"
                                                    FieldLabel="Crossing Type"
                                                    LabelAlign="Right"
                                                    DisplayField="name"
                                                    ValueField="name"
                                                    QueryMode="Local"
                                                    TypeAhead="true"
                                                    ForceSelection="true" TabIndex="26" AllowBlank="false">
                                                    <Store>
                                                        <ext:Store ID="uxAddPropertyType" runat="server" AutoDataBind="true">
                                                            <Model>
                                                                <ext:Model ID="Model13" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="abbr" />
                                                                        <ext:ModelField Name="name" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                            <Reader>
                                                                <ext:ArrayReader />
                                                            </Reader>
                                                        </ext:Store>
                                                    </Store>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:FieldContainer>
                                    </Items>
                                </ext:FieldSet>

                                <ext:FieldSet ID="FieldSet6" runat="server" Title="Special Instructions">
                                    <Items>
                                         
                                        <ext:FieldContainer ID="FieldContainer18" runat="server" Layout="HBoxLayout">
                                            <Items>
                                               

                                                <%----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------%>

                                                <ext:DropDownField ID="uxAddManagerCIDropDownField" runat="server" FieldLabel="Manager" AnchorHorizontal="100%" LabelAlign="Right" Width="475" Mode="ValueText" TabIndex="26" Editable="false">
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
                                                                    <ext:Column ID="Column3" runat="server" Text="Manager Name" DataIndex="CONTACT_NAME" Flex="1" />
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
                                                <ext:NumberField ID="uxAddMainTracksCINumberField" runat="server" FieldLabel="Main Tracks" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="27" />
                                                <ext:Label ID="Label13" runat="server" Text="" Width="25" />
                                                <ext:Checkbox ID="uxAddSubConCI" runat="server" BoxLabel="Subcontracted" BoxLabelAlign="After" Width="110" TabIndex="30" />
                                                <ext:Checkbox ID="uxAddRestrictedCI" runat="server" BoxLabel="Restricted" BoxLabelAlign="After" Width="550" TabIndex="31" />
                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer ID="FieldContainer20" runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:NumberField ID="uxAddOtherTracksCINumberField" runat="server" FieldLabel="Other Tracks" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="28" />
                                                <ext:Label ID="Label12" runat="server" Text="" Width="25" />
                                                <ext:Checkbox ID="uxAddFenceEnchroachCI" runat="server" BoxLabel="Encroachment" BoxLabelAlign="After" Width="110" TabIndex="32" />
                                                <ext:Checkbox ID="uxAddOnSpurCI" runat="server" BoxLabel="On Spur" BoxLabelAlign="After" Width="110" TabIndex="33" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:NumberField ID="uxAddMaxSpeedCINumberField" runat="server" FieldLabel="Max Speed" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="29" />
                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:TextArea ID="uxAddSpecialInstructCI" runat="server" FieldLabel="Special Instructions" AnchorHorizontal="92%" LabelAlign="Right" TabIndex="34" />
                                    </Items>
                                </ext:FieldSet>
                            </Items>
                            <BottomBar>
                                <ext:Toolbar runat="server" >
                            <Items>
                         
                                <ext:Button runat="server" ID="deAddCrossing" Text="Add" Icon="Add" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deAddCrossings" />

                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="CancelCrossing" Text="Cancel"  Icon="Delete">
                                    <Listeners>
                                        <Click Handler="#{uxAddCrossingForm}.reset();
									#{uxAddCrossingWindow}.hide()" />
                                    </Listeners>
                                    <DirectEvents>
                                        <Click OnEvent="deValidateCancelButton" />
                                    </DirectEvents>
                                </ext:Button>
                         
                                </Items>
                                    </ext:Toolbar>
                              </BottomBar>
                              <Listeners>
                                <ValidityChange Handler="#{deAddCrossing}.setDisabled(!valid);" />
                            </Listeners>
                        </ext:FormPanel>
                    </Items>
                </ext:Window>
                <%-----------------------------------------------------------------------------------------------------------------------------------------------------%>

                <ext:Window runat="server"
                    ID="uxEditCrossingWindow"
                    Layout="FormLayout"
                    Hidden="true"
                    Title="Edit Crossing"
                    Width="850" Closable="false" Modal="true">
                    <Items>
                        <ext:FormPanel ID="uxEditCrossingForm" runat="server" Layout="FormLayout">
                            <Items>
                                <ext:FieldSet ID="FieldSet7" runat="server" Title="Crossing Details">
                                    <Items>
                                        <ext:FieldContainer runat="server">
                                            <Items>
                                                <%--<ext:TextField ID="uxEditCrossingNumCI" runat="server" LabelAlign="Right" AnchorHorizontal="50%" TabIndex="1" AllowBlank="false" Width="300" ReadOnly="true" />--%>
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer ID="FieldContainer21" runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextField ID="uxEditRRCI" runat="server" FieldLabel="Railroad" LabelAlign="Right" AnchorHorizontal="100%" TabIndex="2" />
                                                <ext:TextField ID="uxEditRouteCI" runat="server" FieldLabel="Route" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="6" />
                                                <ext:TextField ID="uxEditDotCI" runat="server" FieldLabel="DOT #" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="10" AllowBlank="false" />

                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer ID="FieldContainer22" runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:ComboBox ID="uxEditServiceUnitCI"
                                                    runat="server"
                                                    FieldLabel="Service Unit"
                                                    DisplayField="service_unit"
                                                    ValueField="service_unit"
                                                    LabelAlign="Right"
                                                    AnchorHorizontal="100%" TabIndex="3" AllowBlank="false">
                                                    <Store>
                                                        <ext:Store runat="server"
                                                            ID="uxEditServiceUnitStore">
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
                                                    <DirectEvents>
                                                        <Select OnEvent="deLoadSubDiv">
                                                            <ExtraParams>
                                                                <ext:Parameter Name="Type" Value="Edit" />
                                                            </ExtraParams>
                                                        </Select>
                                                    </DirectEvents>

                                                    <Listeners>
                                                        <Select Handler="#{uxEditSubDivStore}.load()" />
                                                    </Listeners>

                                                </ext:ComboBox>
                                                <ext:TextField ID="uxEditStreetCI" runat="server" FieldLabel="Street" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="7" />

                                                <ext:NumberField ID="uxEditMPCINumberField" runat="server" FieldLabel="MP" LabelAlign="Right" TabIndex="11" AllowBlank="false" />
                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer ID="FieldContainer23" runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:ComboBox ID="uxEditSubDivCIBox"
                                                    runat="server" FieldLabel="Sub-Division"
                                                    LabelAlign="Right"
                                                    DisplayField="sub_division"
                                                    ValueField="sub_division"
                                                    AnchorHorizontal="100%" TabIndex="4" AllowBlank="false" ForceSelection="false">
                                                    <Store>
                                                        <ext:Store runat="server"
                                                            ID="uxEditSubDivStore">
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
                                                <ext:TextField ID="uxEditCountyCI" runat="server" FieldLabel="County" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="8" />

                                                <ext:NumberField ID="uxEditLatCINumberField" runat="server" FieldLabel="Latitude" AnchorHorizontal="92%" LabelAlign="Right" TabIndex="12" />
                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer ID="FieldContainer24" runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:ComboBox runat="server"
                                                    ID="uxEditStateComboBox"
                                                    FieldLabel="State"
                                                    LabelAlign="Right"
                                                    DisplayField="name"
                                                    ValueField="name"
                                                    QueryMode="Local"
                                                    TypeAhead="true"
                                                    AllowBlank="false" ForceSelection="true" TabIndex="5">
                                                    <Store>
                                                        <ext:Store ID="uxEditStateList" runat="server" AutoDataBind="true">
                                                            <Model>
                                                                <ext:Model ID="Model11" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="abbr" />
                                                                        <ext:ModelField Name="name" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                            <Reader>
                                                                <ext:ArrayReader />
                                                            </Reader>
                                                        </ext:Store>
                                                    </Store>
                                                </ext:ComboBox>

                                                <ext:TextField ID="uxEditCityCI" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="9" />
                                                <ext:NumberField ID="uxEditLongCINumberField" runat="server" FieldLabel="Longitude" AnchorHorizontal="92%" LabelAlign="Right" TabIndex="12" />
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
                                                <ext:NumberField ID="uxEditNECINumberField" runat="server" FieldLabel="NE" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="13" />
                                                <ext:NumberField ID="uxEditNEextCINumberField" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="17" />
                                                <ext:TextField ID="uxEditRowWidthCI" runat="server" FieldLabel="ROW Width" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="21" />
                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer ID="FieldContainer27" runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:NumberField ID="uxEditNWCINumberField" runat="server" FieldLabel="NW" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="14" />
                                                <ext:NumberField ID="uxEditNWextCINumberField" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="18" />
                                                <ext:TextField ID="uxEditSurfaceCI" runat="server" FieldLabel="Surface" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="23" />

                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer ID="FieldContainer28" runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:NumberField ID="uxEditSECINumberField" runat="server" FieldLabel="SE" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="15" />
                                                <ext:NumberField ID="uxEditSEextCINumberField" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="19" />
                                                <ext:TextField ID="uxEditWarningDeviceCI" runat="server" FieldLabel="Warning Device" LabelAlign="Right" TabIndex="24" />

                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer ID="FieldContainer29" runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:NumberField ID="uxEditSWCINumberField" runat="server" FieldLabel="SW" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="16" />
                                                <ext:NumberField ID="uxEditSWextCINumberField" runat="server" FieldLabel="" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="20" />
                                                <ext:ComboBox runat="server"
                                                    ID="uxEditPropertyTypeComboBox"
                                                    FieldLabel="Crossing Type"
                                                    LabelAlign="Right"
                                                    DisplayField="name"
                                                    ValueField="name"
                                                    QueryMode="Local"
                                                    TypeAhead="true"
                                                    ForceSelection="true" TabIndex="26" AllowBlank="false">
                                                    <Store>
                                                        <ext:Store ID="uxEditPropertyType" runat="server" AutoDataBind="true">
                                                            <Model>
                                                                <ext:Model ID="Model14" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="abbr" />
                                                                        <ext:ModelField Name="name" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                            <Reader>
                                                                <ext:ArrayReader />
                                                            </Reader>
                                                        </ext:Store>
                                                    </Store>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:FieldContainer>
                                    </Items>
                                </ext:FieldSet>

                                <ext:FieldSet ID="FieldSet9" runat="server" Title="Special Instructions">
                                    <Items>
                                        <ext:FieldContainer ID="FieldContainer30" runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:DropDownField ID="uxEditManagerCI" runat="server" FieldLabel="Manager" AnchorHorizontal="100%" LabelAlign="Right" Width="475" Mode="ValueText" TabIndex="26">
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
                                                                    <ext:Column ID="Column5" runat="server" Text="Manager Name" DataIndex="CONTACT_NAME" Flex="1" />
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
                                                <ext:NumberField ID="uxEditMainTracksCINumberField" runat="server" FieldLabel="Main Tracks" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="26" />
                                                <ext:Label ID="Label14" runat="server" Text="" Width="25" />
                                                <ext:Checkbox ID="uxEditSubConCI" runat="server" BoxLabel="Subcontracted" BoxLabelAlign="After" Width="110" TabIndex="30" />
                                                <ext:Checkbox ID="uxEditRestrictedCI" runat="server" BoxLabel="Restricted" BoxLabelAlign="After" Width="550" TabIndex="31" />


                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer ID="FieldContainer32" runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:NumberField ID="uxEditOtherTracksCINumberField" runat="server" FieldLabel="Other Tracks" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="27" />
                                                <ext:Label ID="Label15" runat="server" Text="" Width="25" />
                                                <ext:Checkbox ID="uxEditFenceEnchroachCI" runat="server" BoxLabel="Encroachment" BoxLabelAlign="After" Width="110" TabIndex="32" />
                                                <ext:Checkbox ID="uxEditOnSpurCI" runat="server" BoxLabel="On Spur" BoxLabelAlign="After" Width="110" TabIndex="33" />

                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:NumberField ID="uxEditMaxSpeedCINumberField" runat="server" FieldLabel="Max Speed" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="28" />
                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:TextArea ID="uxEditSpecialInstructCI" runat="server" FieldLabel="Special Instructions" AnchorHorizontal="92%" LabelAlign="Right" TabIndex="34" />
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
                                <ext:Button runat="server" ID="deCancelEditCrossing" Text="Cancel" Icon="Delete">
                                    <Listeners>
                                        <Click Handler="#{uxEditCrossingForm}.reset();
									#{uxEditCrossingWindow}.hide()" />
                                    </Listeners>
                                </ext:Button>
                            </Buttons>
                            <Listeners>
                                <ValidityChange Handler="#{deEditCrossing}.setDisabled(!valid);" />
                            </Listeners>
                        </ext:FormPanel>
                    </Items>
                </ext:Window>
                <%-----------------------------------------------------------------------------------------------------------------------------------%>

                <%--------------------------------------------------------------------------------------------------------------------------------------%>

                <ext:Window runat="server"
                    ID="uxProjectListWindow"
                    Layout="FormLayout"
                    Hidden="true"
                    Title="Project List"
                    Width="650"
                    Closable="true" Modal="true">
                    <Items>
                        <ext:GridPanel ID="uxProjectGrid" Height="350" runat="server" Flex="1" SimpleSelect="true" Margins="0 2 0 0" EmptyText="No Projects Assigned To This Crossing.">
                            <Store>
                                <ext:Store runat="server"
                                    ID="uxProjectListStore"
                                    AutoDataBind="true" WarningOnDirty="false">
                                    <Model>
                                        <ext:Model ID="Model12" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="PROJECT_ID" />
                                                <ext:ModelField Name="LONG_NAME" />
                                                <ext:ModelField Name="SEGMENT1" />
                                                <ext:ModelField Name="ORGANIZATION_NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>

                                </ext:Store>
                            </Store>
                            <ColumnModel>
                                <Columns>
                                    <ext:Column ID="Column1" runat="server" DataIndex="SEGMENT1" Text="Project" Flex="1" />
                                    <ext:Column ID="Column8" runat="server" DataIndex="LONG_NAME" Text="Project Name" Flex="2" />
                                    <ext:Column ID="Column9" runat="server" DataIndex="ORGANIZATION_NAME" Text="Organization Name" Flex="1" />
                                </Columns>
                            </ColumnModel>

                        </ext:GridPanel>
                    </Items>
                </ext:Window>


            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

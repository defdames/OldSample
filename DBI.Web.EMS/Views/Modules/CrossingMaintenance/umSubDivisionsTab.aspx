﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSubDivisionsTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umSubDivisionsTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager3" runat="server" />
        <div>

             <ext:GridPanel ID="uxSubDivMainGrid" Title="SUB-DIVISIONS" runat="server" Region="North" Layout="HBoxLayout" Collapsible="true">
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="true" Mode="Single" />
                </SelectionModel>
                <Store>
                    <ext:Store runat="server"
                        ID="uxCurrentSubDivStore"
                        OnReadData="deSubDivMainGrid"
                        PageSize="10"
                        AutoDataBind="true" WarningOnDirty="false">
                        <Model>
                            <ext:Model ID="Model2" runat="server">
                                <Fields>
                                    <ext:ModelField Name="SUB_DIVISION_ID" />
                                    <ext:ModelField Name="SUB_DIVISION_NAME"  />
                                    <ext:ModelField Name="STATE" />
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

                        <ext:Column ID="uxMainCrossingNum" runat="server" DataIndex="SUB_DIVISION_NAME" Text="Sub-Division" Flex="1" />
                        <ext:Column ID="uxSubConGrid" runat="server" DataIndex="STATE" Text="State" Flex="1" />

                    </Columns>
                </ColumnModel>
                  <DirectEvents>
                    <Select OnEvent="GetSubDivGridData">
                        <ExtraParams>
                            <ext:Parameter Name="SubDivId" Value="#{uxSubDivMainGrid}.getSelectionModel().getSelection()[0].data.SUB_DIVISION_ID" Mode="Raw" />
                        </ExtraParams>
                    </Select>
                </DirectEvents>    
                   <DirectEvents>
                    <Select OnEvent="deEditSubDivForm">
                        <ExtraParams>
                            <ext:Parameter Name="SubDivId" Value="#{uxSubDivMainGrid}.getSelectionModel().getSelection()[0].data.SUB_DIVISION_ID" Mode="Raw" />
                        </ExtraParams>
                    </Select>
                </DirectEvents>                    
                <Plugins>
                    <ext:FilterHeader ID="FilterHeader1" runat="server" />
                </Plugins>
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True">
                    </ext:PagingToolbar>
                </BottomBar>

            </ext:GridPanel>


            <ext:FormPanel ID="uxSubDivForm" runat="server" Layout="FormLayout">
                <Items>
                    <ext:Toolbar ID="Toolbar4" runat="server">
                        <Items>
                            <ext:Button ID="uxAddSubDiv" runat="server" Text="Add Subdivsion" Icon="ApplicationAdd">
                                <Listeners>
                                    <Click Handler="#{uxAddSubdivisionWindow}.show()" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button ID="uxEditSubDiv" runat="server" Text="Edit Existing Subdivsion" Icon="ApplicationEdit">
                                <Listeners>
                                    <Click Handler="#{uxEditSubdivisionWindow}.show()" />
                                </Listeners>
                                <DirectEvents>
                                 <Click OnEvent="deEditSubDivForm">
                                  <ExtraParams>
                                    <ext:Parameter Name="SubDivId" Value="#{uxSubDivMainGrid}.getSelectionModel().getSelection()[0].data.SUB_DIVISION_ID" Mode="Raw" />
                                </ExtraParams>
                                 </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="uxDeleteSubDiv" runat="server" Text="Delete Subdivison" Icon="ApplicationDelete">
                                <DirectEvents>
                                    <Click OnEvent="deRemoveSubdivision">
                                        <Confirmation ConfirmRequest="true" Title="Remove?" Message="Are you sure you want to delete this sub-division?" />
                                        <ExtraParams>
                                    <ext:Parameter Name="SubDivId" Value="#{uxSubDivMainGrid}.getSelectionModel().getSelection()[0].data.SUB_DIVISION_ID" Mode="Raw" />
                                    </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>

                    <ext:TextField ID="uxSubDivisionSDTextField" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" Width="300" />
                    <ext:FieldContainer runat="server" Layout="HBoxLayout">
                        <Items>
                            <ext:TextField ID="uxRouteSD" runat="server" FieldLabel="Route" LabelAlign="Right" />
                            <ext:TextField ID="uxStateSD" runat="server" FieldLabel="State" LabelAlign="Right" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                        <Items>
                            <ext:TextField ID="uxStreetSD" runat="server" FieldLabel="Street" AnchorHorizontal="100%" LabelAlign="Right" />
                            <ext:TextField ID="uxDotNumSD" runat="server" FieldLabel="DOT #" LabelAlign="Right" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                        <Items>
                            <ext:TextField ID="uxCitySD" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right" />
                            <ext:TextField ID="uxMPSD" runat="server" FieldLabel="MP" AnchorHorizontal="100%" LabelAlign="Right" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" Layout="HBoxLayout">
                        <Items>
                            <ext:TextField ID="uxCountySD" runat="server" FieldLabel="County" LabelAlign="Right" />
                            <ext:TextField ID="uxServiceTypeSD" runat="server" FieldLabel="Service Type" LabelAlign="Right" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:TextArea ID="uxSubDivRemarks" runat="server" FieldLabel="Remarks" AnchorHorizontal="92%" LabelAlign="Right" />
                </Items>
            </ext:FormPanel>
            <%------------------------------------Hidden Windows--------------------------------------%>

            <ext:Window runat="server"
                ID="uxAddSubdivisionWindow"
                Layout="FormLayout"
                Hidden="true"
                Title="Add New Sub-Division"
                Width="500">
                <Items>
                    <ext:FormPanel runat="server" ID="uxAddWindowFormPanel" Layout="FormLayout">
                        <Items>
                            <ext:TextField ID="uxAddNewSubDivSD" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" Width="300" />
                            <ext:FieldContainer ID="FieldContainer3" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxAddNewRouteSD" runat="server" FieldLabel="Route" LabelAlign="Right" />
                                    <ext:TextField ID="uxAddNewStateSDTextField" runat="server" FieldLabel="State" LabelAlign="Right" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer ID="FieldContainer4" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxAddNewStreetSD" runat="server" FieldLabel="Street" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxAddNewDOTNumSD" runat="server" FieldLabel="DOT #" LabelAlign="Right" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer ID="FieldContainer5" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxAddNewCitySD" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxAddNewMPSD" runat="server" FieldLabel="MP" AnchorHorizontal="100%" LabelAlign="Right" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer ID="FieldContainer6" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxAddNewCountySD" runat="server" FieldLabel="County" LabelAlign="Right" />
                                    <ext:TextField ID="uxAddNewServiceTypeSD" runat="server" FieldLabel="Service Type" LabelAlign="Right" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:TextArea ID="uxAddNewRemarksSD" runat="server" FieldLabel="Remarks" AnchorHorizontal="92%" LabelAlign="Right" />

                        </Items>
                        <Buttons>
                            <ext:Button ID="uxAddNewSubdivisionButton" runat="server" Text="Add" Icon="Add" >
                                  <DirectEvents>
								<Click OnEvent="deAddSubDiv" />
							</DirectEvents>
                                </ext:Button>
                            <ext:Button ID="uxCancelNNewSubdivisionButton" runat="server" Text="Cancel" Icon="Delete" />
                        </Buttons>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <%-------------------------------------------------------------------------------------------------------------------------------%>
            <ext:Window runat="server"
                ID="uxEditSubdivisionWindow"
                Layout="FormLayout"
                Hidden="true"
                Title="Edit Existing Sub-Division"
                Width="500">
                <Items>
                    <ext:FormPanel runat="server" ID="FormPanel2" Layout="FormLayout">
                        <Items>
                            <ext:TextField ID="uxEditSubDivSD" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" Width="300" />
                            <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxEditRouteSD" runat="server" FieldLabel="Route" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditStateSDTextField" runat="server" FieldLabel="State" LabelAlign="Right" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer ID="FieldContainer8" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxEditStreetSD" runat="server" FieldLabel="Street" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditDOTNumSD" runat="server" FieldLabel="DOT #" LabelAlign="Right" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxEditCitySD" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditMPSD" runat="server" FieldLabel="MP" AnchorHorizontal="100%" LabelAlign="Right" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxEditCountySD" runat="server" FieldLabel="County" LabelAlign="Right" />
                                    <ext:TextField ID="uxEditServiceTypeSD" runat="server" FieldLabel="Service Type" LabelAlign="Right" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:TextArea ID="uxEditRemarksSD" runat="server" FieldLabel="Remarks" AnchorHorizontal="92%" LabelAlign="Right" />
                        </Items>
                        <Buttons>
                            <ext:Button ID="uxUpdateSubDivButton" runat="server" Text="Update" Icon="Add" >
                              <DirectEvents>
								<Click OnEvent="deEditSubDivs" >
                                   <ExtraParams>
                                    <ext:Parameter Name="SubDivId" Value="#{uxSubDivMainGrid}.getSelectionModel().getSelection()[0].data.SUB_DIVISION_ID" Mode="Raw" />
                                </ExtraParams>
                                    </Click>
							</DirectEvents>
                                </ext:Button>
                            <ext:Button ID="uxCancelUpdateSubDivButton" runat="server" Text="Cancel" Icon="Delete" />
                        </Buttons>
                    </ext:FormPanel>
                </Items>
            </ext:Window>
        </div>
    </form>
</body>
</html>

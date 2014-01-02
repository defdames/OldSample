<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSubDivisionsTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umSubDivisionsTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager3" runat="server" />
    <div>
    <ext:FormPanel ID="FormPanel1" runat="server" Layout="FormLayout">
                   <Items>
                        <ext:Toolbar ID="Toolbar4" runat="server">
                        <Items>
                        <ext:Button ID="uxAddSubDiv" runat="server" Text="Add Subdivsion" Icon="ApplicationAdd" >
                             <Listeners>
								<Click Handler="#{uxAddNewSubdivisionWindow}.show()" />     
							</Listeners>
                        </ext:Button>
                        <ext:Button ID="uxEditSubDiv" runat="server" Text="Edit Existing Subdivsion" Icon="ApplicationEdit" >
                            <Listeners>
								<Click Handler="#{uxEditSubdivisionWindow}.show()" />     
							</Listeners>
                        </ext:Button>
                        <ext:Button ID="uxDeleteSubDiv" runat="server" Text="Delete Subdivison" Icon="ApplicationDelete" >
                            <DirectEvents>
								<Click OnEvent="deRemoveSubdivision">
									<Confirmation ConfirmRequest="true" Title="Remove?" Message="Are you sure you want to delete this sub-division?" />
							    </Click>
							</DirectEvents>
                        </ext:Button>                      
                            </Items>
                             </ext:Toolbar>
                                 <ext:FieldContainer ID="FieldContainer9" runat="server" Layout="HBoxLayout">
                                  <Items>
                                   <ext:DropDownField ID="uxSubDivisionSD" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" />
                                   <ext:TextField ID="uxStateSD" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right" />
                                   <ext:TextField ID="uxCountySD" runat="server" FieldLabel="County" AnchorHorizontal="100%" LabelAlign="Right" />                        
                                  </Items>
                                </ext:FieldContainer>
                       
                                <ext:FieldContainer ID="FieldContainer11" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxCitySD" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    <ext:TextField ID="uxStreetSD" runat="server" FieldLabel="Street" AnchorHorizontal="100%" LabelAlign="Right" />
                                    <ext:TextField ID="uxRouteSD" runat="server" FieldLabel="Route" LabelAlign="Right" />
                                    </Items>   
                                </ext:FieldContainer>

                                 <ext:FieldContainer ID="FieldContainer14" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxDotNumSD" runat="server" FieldLabel="DOT #" LabelAlign="Right" />
                                    <ext:TextField ID="uxMPSD" runat="server" FieldLabel="MP" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    <ext:TextField ID="uxServiceTypeSD" runat="server" FieldLabel="Service Type" LabelAlign="Right" />
                                    </Items>
                                 </ext:FieldContainer>  

                                 <ext:TextArea ID="uxSubDivRemarks" runat="server" FieldLabel="Remarks" AnchorHorizontal="92%" LabelAlign="Right" />                                    
                   </Items>
     </ext:FormPanel>
    <%------------------------------------Hidden Windows--------------------------------------%>
    
         <ext:Window runat="server"
			ID="uxAddNewSubdivisionWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Add New Sub-Division"
			Width="300">
            <Items>
                 <ext:FormPanel runat="server" ID="uxAddWindowFormPanel" Layout="FormLayout" >
                 <Items>
                   <ext:TextField ID="uxAddNewSubDiv" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" />
                   <ext:DropDownField ID="uxAddNewStateSD" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right" />
                   <ext:DropDownField ID="uxAddNewCountySD" runat="server" FieldLabel="County" AnchorHorizontal="100%" LabelAlign="Right" />                                      
                   <ext:DropDownField ID="uxAddNewCitySD" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right"/>
                   <ext:TextField ID="uxAddNewStreetSD" runat="server" FieldLabel="Street" AnchorHorizontal="100%" LabelAlign="Right" />
                   <ext:TextField ID="uxAddNewRouteSD" runat="server" FieldLabel="Route" LabelAlign="Right" />              
                   <ext:TextField ID="uxAddNewDOTNumSD" runat="server" FieldLabel="DOT #" LabelAlign="Right" />
                   <ext:TextField ID="uxAddNewMPSD" runat="server" FieldLabel="MP" AnchorHorizontal="100%" LabelAlign="Right"/>
                   <ext:TextField ID="uxAddNewServiceTypeSD" runat="server" FieldLabel="Service Type" LabelAlign="Right" />                 
                   <ext:TextArea ID="uxAddNewRemarksSD" runat="server" FieldLabel="Remarks" AnchorHorizontal="92%" LabelAlign="Right" />                    
               </Items>
                      <Buttons>
                            <ext:Button ID="uxAddNewSubdivisionButton" runat="server" Text="Add" Icon="Add" />
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
			Width="300">
            <Items>
                 <ext:FormPanel runat="server" ID="FormPanel2" Layout="FormLayout" >
                 <Items>
                   <ext:TextField ID="uxEditSubDivSD" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" />
                   <ext:DropDownField ID="uxEditStateSD" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right" />
                   <ext:DropDownField ID="uxEditCountySD" runat="server" FieldLabel="County" AnchorHorizontal="100%" LabelAlign="Right" />                                      
                   <ext:DropDownField ID="uxEditCitySD" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right"/>
                   <ext:TextField ID="uxEditStreetSD" runat="server" FieldLabel="Street" AnchorHorizontal="100%" LabelAlign="Right" />
                   <ext:TextField ID="uxEditRouteSD" runat="server" FieldLabel="Route" LabelAlign="Right" />              
                   <ext:TextField ID="uxEditDOTNumSD" runat="server" FieldLabel="DOT #" LabelAlign="Right" />
                   <ext:TextField ID="uxEditMPSD" runat="server" FieldLabel="MP" AnchorHorizontal="100%" LabelAlign="Right"/>
                   <ext:TextField ID="uxEditServiceTypeSD" runat="server" FieldLabel="Service Type" LabelAlign="Right" />                 
                   <ext:TextArea ID="uxEditRemarksSD" runat="server" FieldLabel="Remarks" AnchorHorizontal="92%" LabelAlign="Right" />                    
               </Items>
                      <Buttons>
                            <ext:Button ID="uxUpdateSubDivButton" runat="server" Text="Update" Icon="Add" />
                            <ext:Button ID="uxCancelUpdateSubDivButton" runat="server" Text="Cancel" Icon="Delete" />
                        </Buttons>
               </ext:FormPanel>
             </Items>
        </ext:Window>
    </div>
    </form>
</body>
</html>

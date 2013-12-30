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
                   
                        <ext:Button ID="Button6" runat="server" Text="Add Service Unit" Icon="ApplicationAdd" />
                        <ext:Button ID="Button7" runat="server" Text="Edit Service Unit" Icon="ApplicationEdit" />
                        <ext:Button ID="Button8" runat="server" Text="Delete Service Unit" Icon="ApplicationDelete" />
                      
                       
                            </Items>
                             </ext:Toolbar>

                                 <ext:FieldContainer ID="FieldContainer9" runat="server" Layout="HBoxLayout">
                                  <Items>
                                <ext:DropDownField ID="uxCrossingNumSU" runat="server" TriggerIcon="SimpleArrowDown" FieldLabel="Crossing #" LabelAlign="Right">
                                        
                                 </ext:DropDownField>

                                 <ext:DropDownField ID="uxServiceTypeSU" runat="server" TriggerIcon="SimpleArrowDown" FieldLabel="Service Type" LabelAlign="Right" >
                            
                                 </ext:DropDownField>
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
    </div>
    </form>
</body>
</html>

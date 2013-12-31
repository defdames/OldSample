<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umDataEntryTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umDataEntryTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" />
    <div></div>

       
           <ext:RadioGroup ID="RadioGroup1" runat="server">
               <Items>
                   <ext:Radio ID="uxAppEntryRadioButton" FieldLabel="Application Entry" runat="server" LabelAlign="Right"/>
                   <ext:Radio ID="uxInspectEntryRadioButton" FieldLabel="Inspection Entry" runat="server" LabelAlign="Right" />
               </Items>
           </ext:RadioGroup>
       

        <ext:Toolbar ID="Toolbar3" runat="server">
                        <Items>
                        <ext:Button ID="uxAddAppButton" runat="server" Text="Add Application Entry" Icon="ApplicationAdd" >
                            <Listeners>
								<Click Handler="#{uxAddNewApplicationEntryWindow}.show()" />
							</Listeners>
                        </ext:Button>
                        <ext:Button ID="uxEditAppButton" runat="server" Text="Edit Application Entry" Icon="ApplicationEdit" >
                            <Listeners>
								<Click Handler="#{uxEditApplicationEntryWindow}.show()" />
							</Listeners>
                        </ext:Button>
                        <ext:Button ID="uxDeleteAppButton" runat="server" Text="Delete Application Entry" Icon="ApplicationDelete" >
                            <DirectEvents>
								<Click OnEvent="deRemoveApplicationEntry">
									<Confirmation ConfirmRequest="true" Title="Remove?" Message="Are you sure you want to delete this application entry?" />
									
								</Click>
							</DirectEvents>
                        </ext:Button>
                       
                        </Items>
                            
                         </ext:Toolbar>
             
                <ext:FormPanel ID="FormPanel3" runat="server" Layout="FormLayout">
                   <Items>
                        <ext:FieldContainer ID="FieldContainer6" runat="server" Layout="HBoxLayout">
                                  <Items>
                                <ext:DateField ID="DateField2" runat="server" FieldLabel="Application Date" LabelAlign="Right" />
                                <ext:TextField ID="TextField3" runat="server"  FieldLabel="Application #" LabelAlign="Right" />
                                <ext:Checkbox ID="Checkbox4" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="250" />
                                  </Items>
                            </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer7" runat="server" Layout="HBoxLayout">
                                    <Items>
                               <ext:DropDownField ID="DropDownField4" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right"/>
                               <ext:DropDownField ID="DropDownField5" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" />
                               <ext:Checkbox ID="Checkbox5" runat="server" FieldLabel="Cut" LabelAlign="Right" Width="250" />
                                    </Items>
                                </ext:FieldContainer>

                            
                                <ext:FieldContainer ID="FieldContainer8" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:TextField ID="TextField4" runat="server" FieldLabel="Crossing #" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:TextField ID="TextField5" runat="server" FieldLabel="Truck #" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:Checkbox ID="Checkbox6" runat="server" FieldLabel="Inspect" LabelAlign="Right" Width="250" />
                                    </Items>
                                    
                                </ext:FieldContainer>

                                 <ext:TextArea ID="TextArea2" FieldLabel="Remarks" runat="server" LabelAlign="Right" />
                       </Items>
                    </ext:FormPanel>
     
                   
                   
                        
  <%---------------------------------------Hidden Windows-----------------------------------%>      
      
                   <ext:Window runat="server"
			ID="uxAddNewApplicationEntryWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Add New Application Entry"
			Width="720">
            <Items>
                 <ext:FormPanel ID="FormPanel2" runat="server" Layout="FormLayout">
                   <Items>
                        <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                                  <Items>
                                <ext:DateField ID="uxAddNewAppEntryDateField" runat="server" FieldLabel="Application Date" LabelAlign="Right" />
                                <ext:TextField ID="uxAddNewAppEntryNumber" runat="server"  FieldLabel="Application #" LabelAlign="Right" />
                                <ext:Checkbox ID="uxAddNewSUSpray" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="250" />
                                  </Items>
                            </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer0" runat="server" Layout="HBoxLayout">
                                    <Items>
                               <ext:DropDownField ID="uxAddNewAppEntryStateDropDownField" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right"/>
                               <ext:DropDownField ID="uxAddNewAppEntrySubdivisionDropDownField" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" />
                               <ext:Checkbox ID="uxAddNewSUCut" runat="server" FieldLabel="Cut" LabelAlign="Right" Width="250" />
                                    </Items>
                                </ext:FieldContainer>

                            
                                <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:TextField ID="uxAddNewAppEntryCrossingNum" runat="server" FieldLabel="Crossing #" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:TextField ID="uxAddTruckNum" runat="server" FieldLabel="Truck #" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:Checkbox ID="uxAddNewSUMaintainBox" runat="server" FieldLabel="Inspect" LabelAlign="Right" Width="250" />
                                    </Items>
                                    
                                </ext:FieldContainer>

                                 <ext:TextArea ID="TextArea1" FieldLabel="Remarks" runat="server" LabelAlign="Right" />
                       </Items>
                    </ext:FormPanel>
                 </Items>
                        <Buttons>
                            <ext:Button ID="uxAddApplicationEntryButton" runat="server" Text="Add" Icon="Add" />
                            <ext:Button ID="uxCancelNewApplicationEntryButton" runat="server" Text="Cancel" Icon="Delete" />
                        </Buttons>
                  
                </ext:Window>
    <%-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------%>

         <ext:Window runat="server"
			ID="uxEditApplicationEntryWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Edit Existing Application Entry"
			Width="720">
            <Items>
                 <ext:FormPanel ID="FormPanel1" runat="server" Layout="FormLayout">
                   <Items>
                        <ext:FieldContainer ID="FieldContainer3" runat="server" Layout="HBoxLayout">
                                  <Items>
                                <ext:DateField ID="DateField1" runat="server" FieldLabel="Application Date" LabelAlign="Right" />
                                <ext:TextField ID="TextField1" runat="server"  FieldLabel="Application #" LabelAlign="Right" />
                                <ext:Checkbox ID="Checkbox1" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="250" />
                                  </Items>
                            </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer4" runat="server" Layout="HBoxLayout">
                                    <Items>
                               <ext:DropDownField ID="DropDownField1" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right"/>
                               <ext:DropDownField ID="DropDownField2" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" />
                               <ext:Checkbox ID="Checkbox2" runat="server" FieldLabel="Cut" LabelAlign="Right" Width="250" />
                                    </Items>
                                </ext:FieldContainer>

                            
                                <ext:FieldContainer ID="FieldContainer5" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:TextField ID="TextField2" runat="server" FieldLabel="Crossing #" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:DropDownFIeld ID="uxEditTruckNum" runat="server" FieldLabel="Truck #" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:Checkbox ID="Checkbox3" runat="server" FieldLabel="Inspect" LabelAlign="Right" Width="250" />
                                    </Items>
                                    
                                </ext:FieldContainer>

                                <ext:TextArea ID="uxAppEntryRemarks" FieldLabel="Remarks" runat="server" LabelAlign="Right" />
                       </Items>
                    </ext:FormPanel>
                 </Items>
                        <Buttons>
                            <ext:Button ID="uxUpdateAppEntryButton" runat="server" Text="Update" Icon="Add" />
                            <ext:Button ID="uxCancelUpdateAppEntryButton" runat="server" Text="Cancel" Icon="Delete" />
                        </Buttons>
                  
                </ext:Window>
    </form>
</body>
</html>

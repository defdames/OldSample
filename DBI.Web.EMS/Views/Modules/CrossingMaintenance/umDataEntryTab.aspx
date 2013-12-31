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

        <ext:Toolbar ID="Toolbar3" runat="server">
                  <Items>
                        <ext:Button ID="uxAddAppButton" runat="server" Text="Add Entry" Icon="ApplicationAdd" >
                            <Listeners>
								<Click Handler="#{uxAddNewApplicationEntryWindow}.show()" />
							</Listeners>
                        </ext:Button>
                        <ext:Button ID="uxEditAppButton" runat="server" Text="Edit Entry" Icon="ApplicationEdit" >
                            <Listeners>
								<Click Handler="#{uxEditApplicationEntryWindow}.show()" />
							</Listeners>
                        </ext:Button>
                        <ext:Button ID="uxDeleteAppButton" runat="server" Text="Delete Entry" Icon="ApplicationDelete" >
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
                            <ext:DropDownField ID="uxAppORInspect" runat="server" FieldLabel="Entry Type" LabelAlign="Right" Width="300" />

                                <ext:FieldContainer ID="FieldContainer6" runat="server" Layout="HBoxLayout">
                                  <Items>
                                    <ext:TextField ID="uxEntryDate" runat="server" FieldLabel="Date" LabelAlign="Right" />
                                    <ext:TextField ID="uxEntryNumber" runat="server"  FieldLabel="Number" LabelAlign="Right" />
                                    <ext:Checkbox ID="uxEntrySprayBox" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="250" />
                                  </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer7" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField ID="uxEntryState" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right"/>
                                    <ext:TextField ID="uxEntrySubDiv" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" />
                                    <ext:Checkbox ID="uxEntryCutBox" runat="server" FieldLabel="Cut" LabelAlign="Right" Width="250" />
                                    </Items>
                                </ext:FieldContainer>

                            
                                <ext:FieldContainer ID="FieldContainer8" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:TextField ID="uxEntryCrossingNum" runat="server" FieldLabel="Crossing #" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:TextField ID="uxEntryTruckNum" runat="server" FieldLabel="Truck #" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:Checkbox ID="uxEntryInspectBox" runat="server" FieldLabel="Inspect" LabelAlign="Right" Width="250" />
                                    </Items>
                                    
                                </ext:FieldContainer>

                                 <ext:TextArea ID="uxEntryRemarks" FieldLabel="Remarks" runat="server" LabelAlign="Right" />
                       </Items>
                    </ext:FormPanel>       
                        
  <%---------------------------------------Hidden Windows-----------------------------------%>      
      
                   <ext:Window runat="server"
			ID="uxAddNewApplicationEntryWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Add New Entry"
			Width="720">
            <Items>
                 <ext:FormPanel ID="FormPanel2" runat="server" Layout="FormLayout">
                   <Items>
                        <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                                  <Items>
                                <ext:DateField ID="uxAddEntryDate" runat="server" FieldLabel="Date" LabelAlign="Right" />
                                <ext:TextField ID="uxAddEntryNumber" runat="server"  FieldLabel="Number" LabelAlign="Right" />
                                <ext:Checkbox ID="uxAddEntrySprayBox" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="250" />
                                  </Items>
                            </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer0" runat="server" Layout="HBoxLayout">
                                    <Items>
                               <ext:DropDownField ID="uxAddEntryState" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right"/>
                               <ext:DropDownField ID="uxAddEntrySubDiv" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" />
                               <ext:Checkbox ID="uxAddEntryCutBox" runat="server" FieldLabel="Cut" LabelAlign="Right" Width="250" />
                                    </Items>
                                </ext:FieldContainer>

                            
                                <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:TextField ID="uxAddEntryCrossingNum" runat="server" FieldLabel="Crossing #" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:TextField ID="uxAddEntryTruckNum" runat="server" FieldLabel="Truck #" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:Checkbox ID="uxAddEntryInspectBox" runat="server" FieldLabel="Inspect" LabelAlign="Right" Width="250" />
                                    </Items>
                                    
                                </ext:FieldContainer>

                                 <ext:TextArea ID="uxAddEntryRemarks" FieldLabel="Remarks" runat="server" LabelAlign="Right" />
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
            Title="Edit Existing Entry"
			Width="720">
            <Items>
                 <ext:FormPanel ID="FormPanel1" runat="server" Layout="FormLayout">
                   <Items>
                        <ext:FieldContainer ID="FieldContainer3" runat="server" Layout="HBoxLayout">
                                  <Items>
                                <ext:DateField ID="uxEditEntryDate" runat="server" FieldLabel="Date" LabelAlign="Right" />
                                <ext:TextField ID="uxEditEntryNumber" runat="server"  FieldLabel="Number" LabelAlign="Right" />
                                <ext:Checkbox ID="uxEditEntrySprayBox" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="250" />
                                  </Items>
                            </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer4" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:DropDownField ID="uxEditEntryState" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right"/>
                                     <ext:DropDownField ID="uxEditEntrySubDiv" runat="server" FieldLabel="Subdivision" LabelAlign="Right" AnchorHorizontal="100%" />
                                     <ext:Checkbox ID="uxEditEntryCutBox" runat="server" FieldLabel="Cut" LabelAlign="Right" Width="250" />
                                    </Items>
                                </ext:FieldContainer>

                            
                                <ext:FieldContainer ID="FieldContainer5" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:TextField ID="uxEditEntryCrossingNum" runat="server" FieldLabel="Crossing #" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:DropDownFIeld ID="uxEditEntryTruckNum" runat="server" FieldLabel="Truck #" AnchorHorizontal="100%" LabelAlign="Right" />
                                     <ext:Checkbox ID="uxEditEntryInspectNum" runat="server" FieldLabel="Inspect" LabelAlign="Right" Width="250" />
                                    </Items>
                                    
                                </ext:FieldContainer>

                                <ext:TextArea ID="uxEditEntryRemarks" FieldLabel="Remarks" runat="server" LabelAlign="Right" />
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

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
         <ext:GridPanel ID="uxApplicationCrossingGrid" Title="CROSSING LIST FOR APPLICATION ENTRY" runat="server" Region="North" Layout="HBoxLayout" Collapsible="true">
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="true" Mode="Single" />
                </SelectionModel>
                <Store>
                    <ext:Store runat="server"
                        ID="uxAppEntryCrossingStore"
                        OnReadData="deApplicationGridData"
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

                        <ext:Column ID="uxMainCrossing" runat="server" DataIndex="CROSSING_NUMBER" Text="Crossing #" Flex="1" />
                        <ext:Column ID="Column8" runat="server" DataIndex="LONG_NAME" Text="Project Name" Flex="2" />
                        <ext:Column ID="Column12" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" />
                        <ext:Column ID="uxSubDiv" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />
                        <ext:Column ID="uxMTM" runat="server" DataIndex="CONTACT_NAME" Text="Manager" Flex="1" />

                    </Columns>
                </ColumnModel>
                <Plugins>
                    <ext:FilterHeader ID="FilterHeader1" runat="server" />
                </Plugins>
              <DirectEvents>
                    <Select OnEvent="GetApplicationGridData">
                        <ExtraParams>
                            <ext:Parameter Name="CrossingId" Value="#{uxApplicationCrossingGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                        </ExtraParams>
                    </Select>
                </DirectEvents>
             
              

                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True">
                    </ext:PagingToolbar>
                </BottomBar>

            </ext:GridPanel>
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
                             <DirectEvents>
                             <Click OnEvent="deEditApplicationForm">
                                 <ExtraParams>
						            <ext:Parameter Name="ApplicationInfo" Value="Ext.encode(#{uxApplicationEntryGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
					                 </ExtraParams>
                             </Click>
                            </DirectEvents>

                        </ext:Button>
                        <ext:Button ID="uxDeleteAppButton" runat="server" Text="Delete Entry" Icon="ApplicationDelete" >
                            <DirectEvents>
								<Click OnEvent="deRemoveApplicationEntry">
									<Confirmation ConfirmRequest="true" Title="Remove?" Message="Are you sure you want to delete this application entry?" />
						       <ExtraParams>
						            <ext:Parameter Name="ApplicationInfo" Value="Ext.encode(#{uxApplicationEntryGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
					                </ExtraParams>
                                     </Click>
							</DirectEvents>
                        </ext:Button>                     
                </Items>                       
        </ext:Toolbar>
              <ext:GridPanel ID="uxApplicationEntryGrid" Title="APPLICATION ENTRIES" runat="server" Region="North" Layout="HBoxLayout" Collapsible="true">
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" AllowDeselect="true" Mode="Single" />
                </SelectionModel>
                <Store>
                    <ext:Store runat="server"
                        ID="uxApplicationStore">
                        <Model>
                            <ext:Model ID="Model1" runat="server">
                                <Fields>
                                    <ext:ModelField Name="CROSSING_ID" />
                                  
                                   
                                    <ext:ModelField Name="APPLICATION_ID" />
                                    <ext:ModelField Name="APPLICATION_NUMBER" />
                                    <ext:ModelField Name="APPLICATION_DATE" Type="Date" />
                                    <ext:ModelField Name="APPLICATION_REQUESTED" />
                                    <ext:ModelField Name="TRUCK_NUMBER" />
                                    <ext:ModelField Name="SPRAY" />
                                    <ext:ModelField Name="CUT" />
                                    <ext:ModelField Name="INSPECT" />
                                    <ext:ModelField Name="REMARKS" />

                                </Fields>
                            </ext:Model>
                        </Model>
                       
                    </ext:Store>
                </Store>
                <ColumnModel>
                    <Columns>

                        <ext:Column ID="Column1" runat="server" DataIndex="APPLICATION_NUMBER" Text="Application #" Flex="1" />
                        <ext:DateColumn ID="DateColumn2" runat="server" DataIndex="APPLICATION_DATE" Text="Date" Flex="1" Format="MM/dd/yyyy" />
                        <ext:Column ID="Column3" runat="server" DataIndex="TRUCK_NUMBER" Text="Truck #" Flex="1" />
                         <ext:Column ID="Column2" runat="server" DataIndex="APPLICATION_REQUESTED" Text="App Requested" Flex="1" />
                        <ext:Column ID="Column7" runat="server" DataIndex="SPRAY" Text="Spray" Flex="1" />
                        <ext:Column ID="Column9" runat="server" DataIndex="CUT" Text="Cut" Flex="1" />
                        <ext:Column ID="Column10" runat="server" DataIndex="INSPECT" Text="Inspect" Flex="1" />
                        <ext:Column ID="Column5" runat="server" DataIndex="REMARKS" Text="Remarks" Flex="3" />

                    </Columns>
                </ColumnModel>               

            </ext:GridPanel>
              
                        
  <%---------------------------------------Hidden Windows-----------------------------------%>      
      
                   <ext:Window runat="server"
			ID="uxAddNewApplicationEntryWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Add Application Entry"
			Width="650">
            <Items>
                 <ext:FormPanel ID="uxAddApplicationForm" runat="server" Layout="FormLayout">
                   <Items>

                       
                        <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                                  <Items>
                                  <ext:TextField ID="uxAddEntryNumber" runat="server"  FieldLabel=" Application #" LabelAlign="Right" />
                                  
                                  <ext:Checkbox ID="uxAddEntrySprayBox" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="250" />
                                  </Items>
                            </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer0" runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:DateField ID="uxAddEntryDate" runat="server" FieldLabel="Date" LabelAlign="Right" />
                                        
                                    <ext:Checkbox ID="uxAddEntryCutBox" runat="server" FieldLabel="Cut" LabelAlign="Right" Width="250" />
                                    </Items>
                                </ext:FieldContainer>

                            
                                <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:ComboBox ID="uxAddAppReqeusted"
                                                runat="server"
                                                FieldLabel="App Requested"
                                                LabelAlign="Right"
                                                DisplayField="type"
                                                ValueField="type"
                                                QueryMode="Local"
                                                TypeAhead="true" >
                                                    <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxAddAppRequestedStore" AutoDataBind="true" >
                                                        <Model>
                                                            <ext:Model ID="Model3" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="type" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                        <Reader>
										            <ext:ArrayReader />
									                    </Reader>
                                                    </ext:Store>
                                                </Store>                                                      
                                            </ext:ComboBox> 
                                     
                                     <ext:Checkbox ID="uxAddEntryInspectBox" runat="server" FieldLabel="Inspect" LabelAlign="Right" Width="250" />
                                    </Items>                                  
                                </ext:FieldContainer>
                         <ext:ComboBox ID="uxAddApplicationTruckComboBox"
                                                runat="server"
                                                FieldLabel="Truck #"
                                                LabelAlign="Right"
                                                DisplayField="NAME"
                                                ValueField="NAME"
                                                QueryMode="Local"
                                                TypeAhead="true"  Width="355" >
                                                    <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxAddApplicationTruckStore" AutoDataBind="true" >
                                                        <Model>
                                                            <ext:Model ID="Model5" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="NAME" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                     
                                                    </ext:Store>
                                                </Store>                                                      
                                            </ext:ComboBox> 
                                    
                                 <ext:TextArea ID="uxAddEntryRemarks" FieldLabel="Remarks" runat="server" LabelAlign="Right" />
                              </Items>
                        </ext:FormPanel>
                    </Items>
                        <Buttons>
                            <ext:Button ID="uxAddApplicationEntryButton" runat="server" Text="Add" Icon="Add" >
                                <DirectEvents>
                                    <Click OnEvent="deAddApplication" >
                                      <ExtraParams>
                                           <ext:Parameter Name="CrossingId" Value="#{uxApplicationCrossingGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                      </ExtraParams>
                                        </Click>
                                </DirectEvents>
                                </ext:Button>
                            <ext:Button ID="uxCancelNewApplicationEntryButton" runat="server" Text="Cancel" Icon="Delete" >
                                <Listeners>
                                    <Click Handler="#{uxAddApplicationForm}.reset();
									#{uxAddNewApplicationEntryWindow}.hide()" />
                                </Listeners>
                            </ext:Button>
                        </Buttons>               
                </ext:Window>
    <%-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------%>

         <ext:Window runat="server"
			ID="uxEditApplicationEntryWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Edit Application Entry"
			Width="650">
            <Items>
                 <ext:FormPanel ID="uxEditApplicationForm" runat="server" Layout="FormLayout">
                   <Items>
                        
                        <ext:FieldContainer ID="FieldContainer3" runat="server" Layout="HBoxLayout">
                                  <Items>
                                <ext:TextField ID="uxEditEntryNumber" runat="server"  FieldLabel="Application #" LabelAlign="Right" />
                               
                                <ext:Checkbox ID="uxEditEntrySprayBox" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="250" />
                                  </Items>
                        </ext:FieldContainer>

                                <ext:FieldContainer ID="FieldContainer4" runat="server" Layout="HBoxLayout">
                                    <Items>
                                     <ext:DateField ID="uxEditEntryDate" runat="server" FieldLabel="Date" LabelAlign="Right" />
                                    
                                     <ext:Checkbox ID="uxEditEntryCutBox" runat="server" FieldLabel="Cut" LabelAlign="Right" Width="250" />
                                    </Items>
                                </ext:FieldContainer>

                            
                                <ext:FieldContainer ID="FieldContainer5" runat="server" Layout="HBoxLayout">
                                    <Items>
                                          <ext:ComboBox ID="uxEditAppRequested"
                                                runat="server"
                                                FieldLabel="App Requested"
                                                LabelAlign="Right"
                                                DisplayField="type"
                                                ValueField="type"
                                                QueryMode="Local"
                                                TypeAhead="true" >
                                                    <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxEditAppRequestedStore" AutoDataBind="true" >
                                                        <Model>
                                                            <ext:Model ID="Model4" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="type" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                        <Reader>
										            <ext:ArrayReader />
									                    </Reader>
                                                    </ext:Store>
                                                </Store>                                                      
                                            </ext:ComboBox> 
                                         
                                     <ext:Checkbox ID="uxEditEntryInspectBox" runat="server" FieldLabel="Inspect" LabelAlign="Right" Width="250" />
                                    </Items>                                 
                                </ext:FieldContainer>
                                <ext:ComboBox ID="uxEditApplicationTruckNumber"
                                                runat="server"
                                                FieldLabel="Truck #"
                                                LabelAlign="Right"
                                                DisplayField="NAME"
                                                ValueField="NAME"
                                                QueryMode="Local"
                                                TypeAhead="true"  Width="355" >
                                                    <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxEditApplicationTruckStore" AutoDataBind="true" >
                                                        <Model>
                                                            <ext:Model ID="Model6" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="NAME" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                        
                                                    </ext:Store>
                                                </Store>
                                                   
                                            </ext:ComboBox>
                                <ext:TextArea ID="uxEditEntryRemarks" FieldLabel="Remarks" runat="server" LabelAlign="Right" />
                       </Items>
                    </ext:FormPanel>
                 </Items>
                        <Buttons>
                            <ext:Button ID="uxUpdateAppEntryButton" runat="server" Text="Update" Icon="Add" >
                                <DirectEvents>
                                    <Click OnEvent="deEditApplication" >
                                      <ExtraParams>
                                           <ext:Parameter Name="ApplicationId" Value="#{uxApplicationEntryGrid}.getSelectionModel().getSelection()[0].data.APPLICATION_ID" Mode="Raw" />
                                      </ExtraParams>
                                        </Click>
                                </DirectEvents>
                                </ext:Button>
                            <ext:Button ID="uxCancelUpdateAppEntryButton" runat="server" Text="Cancel" Icon="Delete" >
                                  <Listeners>
                                    <Click Handler="#{uxEditApplicationForm}.reset();
									#{uxEditApplicationEntryWindow}.hide();" />
                                </Listeners>
                            </ext:Button>
                        </Buttons>              
                </ext:Window>
    </form>
</body>
</html>

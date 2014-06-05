<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umDataEntryTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umDataEntryTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <style type="text/css">
        .rowBodyCls .x-grid-cell-rowbody {
            border-style: solid;
            border-width: 0px 0px 1px;
            border-color: black;
        }

        .x-grid-group-title {
            color: #000000;
            font: bold 11px/13px tahoma,arial,verdana,sans-serif;
        }

        .x-grid-group-hd {
            border-width: 0 0 1px 0;
            border-style: solid;
            border-color: #000000;
            padding: 10px 4px 4px 4px;
            background: white;
            cursor: pointer;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" />
        <div></div>
        <ext:GridPanel ID="uxApplicationCrossingGrid" Title="CROSSING LIST FOR APPLICATION ENTRY" runat="server" Region="North" Layout="HBoxLayout" Collapsible="true">
           
            <Store>
                <ext:Store runat="server"
                    ID="uxAppEntryCrossingStore"
                    OnReadData="deApplicationGridData"
                    PageSize="20"
                    AutoDataBind="true" WarningOnDirty="false">
                    <Model>
                        <ext:Model ID="Model2" runat="server">
                            <Fields>
                                <ext:ModelField Name="CROSSING_ID" />
                                <ext:ModelField Name="CONTACT_ID" />
                                <ext:ModelField Name="CROSSING_NUMBER" Type="String" />
                                <ext:ModelField Name="PROJECT_ID" />
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
                    <ext:Column ID="Column12" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" />
                    <ext:Column ID="uxSubDiv" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />
                    <ext:Column ID="uxMTM" runat="server" DataIndex="CONTACT_NAME" Text="Manager" Flex="1" />

                </Columns>
            </ColumnModel>
            <Plugins>
                <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
            </Plugins>
            <SelectionModel>
                <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" Mode="Multi"  />
            </SelectionModel>
            <Listeners>
                <Select Handler="#{uxAddAppButton}.enable(); #{uxRemoveAppButton}.enable()" />
               
            </Listeners>

             <DirectEvents>
                    <SelectionChange OnEvent="GetApplicationGridData">
                      <%--  <ExtraParams>
                            <ext:Parameter Name="CrossingId" Value="#{uxApplicationCrossingGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                        </ExtraParams>--%>
                          <ExtraParams>
                                <ext:Parameter Name="crossingId" Value="Ext.encode(#{uxApplicationCrossingGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                            </ExtraParams>
                    </SelectionChange>
                </DirectEvents>
            <BottomBar>
                <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True">
                </ext:PagingToolbar>
            </BottomBar>

        </ext:GridPanel>
        <ext:Toolbar ID="Toolbar3" runat="server">
            <Items>
                <ext:Button ID="uxAddAppButton" runat="server" Text="Add Entry" Icon="ApplicationAdd" Disabled="true">
                    <Listeners>
                        <Click Handler="#{uxAddNewApplicationEntryWindow}.show()" />
                    </Listeners>
                    <%--  <DirectEvents>
                                    <Click OnEvent="deGetRRType">
                                     
                                    </Click>
                                </DirectEvents>--%>
                </ext:Button>
                <ext:Button ID="uxRemoveAppButton" runat="server" Text="Delete Application" Icon="ApplicationDelete" Disabled="true">
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
        <ext:GridPanel ID="uxApplicationEntryGrid" Title="APPLICATION ENTRIES" runat="server" Region="North" Frame="false" Collapsible="true" MultiSelect="true" >
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" AllowDeselect="true" Mode="Single" />
                </SelectionModel>
                <Store>
                    <ext:Store runat="server"
                        ID="uxApplicationStore" GroupField="CROSSING_NUMBER">
                        <Model>
                            <ext:Model ID="Model1" runat="server">
                                <Fields>
                                    <ext:ModelField Name="CROSSING_ID" />  
                                    <ext:ModelField Name="CROSSING_NUMBER" />                                
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
                        <Sorters>
                        <ext:DataSorter Property="APPLICATION_DATE" Direction="ASC" />

                    </Sorters>

                    </ext:Store>
                </Store>

                <ColumnModel>
                    <Columns>
                         <ext:Column ID="Column1" runat="server" DataIndex="CROSSING_NUMBER" Text="Crossing Number" Flex="1" />
                        <ext:Column ID="Column2" runat="server" DataIndex="APPLICATION_REQUESTED" Text="Appplication #" Flex="1" />
                        <ext:DateColumn ID="DateColumn2" runat="server" DataIndex="APPLICATION_DATE" Text="Date" Flex="1" Format="MM/dd/yyyy" />
                        <ext:Column ID="Column3" runat="server" DataIndex="TRUCK_NUMBER" Text="Truck #" Flex="1" />                     
                        <ext:Column ID="Column7" runat="server" DataIndex="SPRAY" Text="Spray" Flex="1" />
                        <ext:Column ID="Column9" runat="server" DataIndex="CUT" Text="Cut" Flex="1" />
                        <ext:Column ID="Column10" runat="server" DataIndex="INSPECT" Text="Inspect" Flex="1" />
                        <ext:Column ID="Column5" runat="server" DataIndex="REMARKS" Text="Remarks" Flex="3" />

                    </Columns>
                </ColumnModel>               
             <Features>
                <ext:Grouping ID="Grouping1"
                    runat="server"
                    HideGroupedHeader="true" Collapsible="false" Cls="x-grid-group-title; x-grid-group-hd" />
            </Features>
            </ext:GridPanel>


        <%---------------------------------------Hidden Windows-----------------------------------%>

        <ext:Window runat="server"
            ID="uxAddNewApplicationEntryWindow"
            Layout="FormLayout"
            Hidden="true"
            Title="Add Application Entry"
            Width="650" Modal="true">
            <Items>
                <ext:FormPanel ID="uxAddApplicationForm" runat="server" Layout="FormLayout">
                    <Items>


                        <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                            <Items>
                                <ext:ComboBox ID="uxAddAppReqeusted"
                                    runat="server"
                                    FieldLabel="App Requested"
                                    LabelAlign="Right"
                                    DisplayField="type"
                                    ValueField="type"
                                    QueryMode="Local"
                                    TypeAhead="true" Width="300" AllowBlank="false" ForceSelection="true" TabIndex="1">
                                    <Store>
                                        <ext:Store runat="server"
                                            ID="uxAddAppRequestedStore" AutoDataBind="true">
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
                                  <ext:Label ID="Label3" runat="server" Text="" Width="25" />
                                <ext:Checkbox ID="uxAddEntrySprayBox" runat="server" BoxLabel="Spray" BoxLabelAlign="After" Width="250" TabIndex="4" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer0" runat="server" Layout="HBoxLayout">
                            <Items>
                                <ext:DateField ID="uxAddEntryDate" runat="server" FieldLabel="Date" LabelAlign="Right" Width="300" AllowBlank="false" Editable="false" TabIndex="2" />
                                  <ext:Label ID="Label1" runat="server" Text="" Width="25" />
                                <ext:Checkbox ID="uxAddEntryCutBox" runat="server" BoxLabel="Cut" BoxLabelAlign="After" TabIndex="5" />

                            </Items>
                        </ext:FieldContainer>


                        <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                            <Items>
                               <%-- <ext:ComboBox ID="uxAddApplicationTruckComboBox"
                                    runat="server"
                                    FieldLabel="Truck #"
                                    LabelAlign="Right"
                                    DisplayField="NAME"
                                    ValueField="NAME"
                                    QueryMode="Local"
                                    TypeAhead="true" Width="300" AllowBlank="false" ForceSelection="true" TabIndex="3">
                                    <Store>
                                        <ext:Store runat="server"
                                            ID="uxAddApplicationTruckStore" AutoDataBind="true">
                                            <Model>
                                                <ext:Model ID="Model5" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="NAME" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>

                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>--%>
                                 <ext:DropDownField runat="server" Editable="false"
					ID="uxAddEquipmentDropDown"
					FieldLabel="Choose Equipment"
					Mode="ValueText" LabelAlign="Right"
					AllowBlank="false" Width="500">
					<Component>
						<ext:GridPanel runat="server"
							ID="uxEquipmentGrid" 
							Layout="HBoxLayout">
							<Store>
								<ext:Store runat="server"
									ID="uxEquipmentStore"
									OnReadData="deReadGrid"
									PageSize="10"
									RemoteSort="true"
									AutoDataBind="true">
									<Model>
										<ext:Model ID="Model4" runat="server">
											<Fields>
												<ext:ModelField Name="CLASS_CODE" Type="String"/>
												<ext:ModelField Name="NAME" Type="String"/>
												<ext:ModelField Name="ORG_ID" />
												<ext:ModelField Name="ORGANIZATION_ID" Type="Int" />
												<ext:ModelField Name="ORGANIZATION_NAME" Type="String" />
												<ext:ModelField Name="PROJECT_ID" Type="Int" />
												<ext:ModelField Name="PROJECT_STATUS_CODE" />
												<ext:ModelField Name="SEGMENT1" Type="Int" />
											</Fields>
										</ext:Model>
									</Model>											
									<Proxy>
										<ext:PageProxy />
									</Proxy>   
									<Parameters>
										<ext:StoreParameter Name="Form" Value="Add" />
									</Parameters>
								</ext:Store>
							</Store>
							<ColumnModel>
								<Columns>
									<ext:Column runat="server"
										ID="uxEquipmentClassCode"
										DataIndex="CLASS_CODE"
										Text="Class Code" />
									<ext:Column runat="server"
										ID="uxEquipmentName"
										DataIndex="NAME" 
										Text="Equipment Name"/>
									<ext:Column runat="server"
										ID="uxEquipmentOrgName"
										DataIndex="ORGANIZATION_NAME"
										Text="Organization Name" />                     
									<ext:Column runat="server"
										ID="uxEquipmentSegment"
										DataIndex="SEGMENT1"
										Text="Project Number" />
								</Columns>
							</ColumnModel>
							<Plugins>
								<ext:FilterHeader ID="uxAddEquipmentFilter" runat="server" Remote="true"  />
							</Plugins>									
							<TopBar>
								<ext:Toolbar runat="server"
									ID="uxEquipmentBar">
									<Items>
										<ext:Button runat="server"
											ID="uxAddEquipmentToggleOrg"
											EnableToggle="true"
											Text="All Regions"
											Icon="Group">
											<DirectEvents>
												<Toggle OnEvent="deReloadStore">
													<ExtraParams>
														<ext:Parameter Name="Type" Value="Equipment" />
													</ExtraParams>
												</Toggle>
											</DirectEvents>
										</ext:Button>
									</Items>
								</ext:Toolbar>
							</TopBar>
							<BottomBar>
								<ext:PagingToolbar runat="server"
									ID="uxAddEquipmentPaging" />
							</BottomBar>
							<SelectionModel>
								<ext:RowSelectionModel ID="RowSelectionModel3" runat="server" Mode="Single" />
							</SelectionModel>
							<DirectEvents>
								<SelectionChange OnEvent="deStoreGridValue">
									<ExtraParams>
										<ext:Parameter Name="ProjectId" Value="#{uxEquipmentGrid}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
										<ext:Parameter Name="EquipmentName" Value="#{uxEquipmentGrid}.getSelectionModel().getSelection()[0].data.NAME" Mode="Raw" />
										<ext:Parameter Name="Form" Value="Add" />
									</ExtraParams>
								</SelectionChange>
							</DirectEvents>
						</ext:GridPanel>
					</Component>
				</ext:DropDownField>
                                  <ext:Label ID="Label2" runat="server" Text="" Width="25" />
                                <ext:Checkbox ID="uxAddEntryInspectBox" runat="server" BoxLabel="Inspect" BoxLabelAlign="After" Width="250" TabIndex="6" />
                            </Items>
                        </ext:FieldContainer>

                       
                        <ext:TextArea ID="uxAddEntryRemarks" FieldLabel="Remarks" runat="server" LabelAlign="Right" TabIndex="7"/>
                    </Items>
                    <Buttons>
                        <ext:Button ID="uxAddApplicationEntryButton" runat="server" Text="Add" Icon="Add">
                            <DirectEvents>
                                <Click OnEvent="deAddApplication">
                                    <ExtraParams>
                                        <ext:Parameter Name="CrossingId" Value="#{uxApplicationCrossingGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                        <ext:Parameter Name="selectedCrossings" Value="Ext.encode(#{uxApplicationCrossingGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="uxCancelNewApplicationEntryButton" runat="server" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="#{uxAddApplicationForm}.reset();
									#{uxAddNewApplicationEntryWindow}.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <Listeners>
                        <ValidityChange Handler="#{uxAddApplicationEntryButton}.setDisabled(!valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>


        </ext:Window>
        <%-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------%>

      
    </form>
</body>
</html>

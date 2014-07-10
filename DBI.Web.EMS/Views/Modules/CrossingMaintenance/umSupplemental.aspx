<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSupplemental.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umSupplemental" %>

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
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <div></div>
        
        <ext:GridPanel ID="uxSupplementalCrossingGrid" Title="CROSSING LIST FOR SUPPLEMENTAL ENTRY" runat="server" Layout="HBoxLayout" Collapsible="true">
            <SelectionModel>
                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="true" Mode="Multi" />
            </SelectionModel>
            <Store>
                <ext:Store runat="server"
                    ID="uxSupplementalCrossingStore"
                    OnReadData="deSupplementalGridData"
                    PageSize="10"
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
         
            <BottomBar>
                <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True">
                </ext:PagingToolbar>
            </BottomBar>
            <Listeners>
                <Select Handler="#{uxSupplementalStore}.reload(); #{uxAddSuppButton}.enable()" />
                <Deselect Handler="#{uxAddSuppButton}.disable();" />
            </Listeners>

        </ext:GridPanel>
        <ext:Toolbar ID="Toolbar1" runat="server">
            <Items>
                <ext:Button ID="uxAddSuppButton" runat="server" Text="Add Supplemental" Icon="ApplicationAdd" Disabled="true">
                    <Listeners>
                        <Click Handler="#{uxAddNewSupplementalWindow}.show()" />
                    </Listeners>
                </ext:Button>
               
                <ext:Button ID="uxRemoveSuppButton" runat="server" Text="Delete Supplemental" Icon="ApplicationDelete" Disabled="true">
                    <DirectEvents>
                        <Click OnEvent="deRemoveSupplemental">
                            <Confirmation ConfirmRequest="true" Title="Remove?" Message="Are you sure you want to delete this supplemental entry?" />

                            <ExtraParams>
                                <ext:Parameter Name="SupplementalInfo" Value="Ext.encode(#{uxSupplementalGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                            </ExtraParams>
                          <%--    <ExtraParams>
                                    <ext:Parameter Name="SupplementalId" Value="#{uxSupplementalCrossingGrid}.getSelectionModel().getSelection()[0].data.SUPPLEMENTAL_ID" Mode="Raw" />
                                </ExtraParams>--%>
                        </Click>
                    </DirectEvents>

                </ext:Button>
            </Items>
        </ext:Toolbar>

        <ext:GridPanel ID="uxSupplementalGrid" Title="SUPPLEMENTAL ENTRIES" runat="server" Layout="FitLayout">

            <Store>
                <ext:Store runat="server"
                    ID="uxSupplementalStore" OnReadData="GetSupplementalGridData" AutoDataBind="true" AutoLoad="false" GroupField="CROSSING_NUMBER">
                    <Parameters>
                        <ext:StoreParameter Name="crossingId" Value="Ext.encode(#{uxSupplementalCrossingGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                    </Parameters>
                    <Model>
                        <ext:Model ID="Model1" runat="server">
                            <Fields>
                               

                                <ext:ModelField Name="CROSSING_ID" />
                                <ext:ModelField Name="CROSSING_NUMBER" />
                                <ext:ModelField Name="SUPPLEMENTAL_ID" />
                                <ext:ModelField Name="APPROVED_DATE" Type="Date" />
                                <ext:ModelField Name="SERVICE_TYPE" />
                                <ext:ModelField Name="TRUCK_NUMBER" />
                                <ext:ModelField Name="SQUARE_FEET" />
                                <ext:ModelField Name="RECURRING" />
                                <ext:ModelField Name="REMARKS" />
                            </Fields>
                        </ext:Model>
                    </Model>
                     <Sorters>
                        <ext:DataSorter Property="APPROVED_DATE" Direction="ASC" />
                    </Sorters>
                </ext:Store>
            </Store>
            

            <ColumnModel>
                <Columns>
                     <ext:Column ID="Column2" runat="server" DataIndex="CROSSING_NUMBER" Text="Crossing Number" Flex="1" />
                    <ext:DateColumn ID="DateColumn1" runat="server" DataIndex="APPROVED_DATE" Text="Approved Date" Flex="1" Format="MM/dd/yyyy" />
                    <ext:Column ID="Column6" runat="server" DataIndex="SERVICE_TYPE" Text="Service Type" Flex="1" />
                    <ext:Column ID="Column3" runat="server" DataIndex="TRUCK_NUMBER" Text="Truck" Flex="1" />
                    <ext:Column ID="Column1" runat="server" DataIndex="SQUARE_FEET" Text="Square Feet" Flex="1" />
                    <ext:Column ID="Column13" runat="server" DataIndex="RECURRING" Text="Recurring" Flex="1" />
                    <ext:Column ID="Column5" runat="server" DataIndex="REMARKS" Text="Remarks" Flex="3" />
                   
                </Columns>
            </ColumnModel>
            <SelectionModel>
                <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" AllowDeselect="true" Mode="Single" />
            </SelectionModel>
            
            <Listeners>
                <Select Handler="#{uxRemoveSuppButton}.enable();" />
                <Deselect Handler="#{uxRemoveSuppButton}.disable();" />
            </Listeners>
             <Features>
                <ext:Grouping ID="Grouping1"
                    runat="server"
                    HideGroupedHeader="true" Collapsible="false" Cls="x-grid-group-title; x-grid-group-hd" />
            </Features>
        </ext:GridPanel>


        <%-------------------------------------------------Hidden Windows------------------------------------------------------%>

        <ext:Window runat="server"
            ID="uxAddNewSupplementalWindow"
            Layout="FormLayout"
            Hidden="true"
            Title="Add New Supplemental"
            Width="800" Modal="true">
            <Items>
                <ext:FormPanel ID="uxAddSupplementalForm" runat="server" Layout="FormLayout">
                    <Items>
                        <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                            <Items>
                                <ext:DateField ID="uxAddApprovedDateField" runat="server" FieldLabel="Approved Date" AnchorHorizontal="100%" LabelAlign="Right" AllowBlank="false" Editable="false"/>

                                <ext:ComboBox ID="uxAddServiceType"
                                    runat="server"
                                    FieldLabel="Service Type"
                                    LabelAlign="Right"
                                    DisplayField="type"
                                    ValueField="type"
                                    QueryMode="Local"
                                    TypeAhead="true" TabIndex="2" Width="300" AllowBlank="false" ForceSelection="true">
                                    <Store>
                                        <ext:Store runat="server"
                                            ID="uxAddServiceTypeStore" AutoDataBind="true">
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

                            </Items>
                        </ext:FieldContainer>

                      
                                <ext:DropDownField runat="server" Editable="false"
					ID="uxAddEquipmentDropDown"
					FieldLabel="Choose Equipment"
					Mode="ValueText" LabelAlign="Right"
					AllowBlank="false" Width="540">
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
										<ext:Model ID="Model3" runat="server">
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
                              <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                            <Items>

                                <ext:TextField runat="server" ID="uxAddSquareFeet" FieldLabel="Square Feet" LabelAlign="Right" AnchorHorizontal="100%" AllowBlank="false" />
                                <ext:Label ID="Label1" runat="server" Width="65" />
                                   <ext:Checkbox ID="uxAddRecurringBox" runat="server" BoxLabel="Recurring" BoxLabelAlign="After" AllowBlank="false" />

                            </Items>
                        </ext:FieldContainer> 


                        <ext:TextArea ID="uxAddRemarks" runat="server" FieldLabel="Remarks" AnchorHorizontal="92%" LabelAlign="Right" />
                    </Items>
                    <Buttons>
                        <ext:Button ID="uxAddNewSupplementalButton" runat="server" Text="Add" Icon="Add">
                            <DirectEvents>
                                <Click OnEvent="deAddSupplemental">
                                    <ExtraParams>
                                        <ext:Parameter Name="CrossingId" Value="#{uxSupplementalCrossingGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="uxCancelNewSupplementalButton" runat="server" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="#{uxAddSupplementalForm}.reset();
									#{uxAddNewSupplementalWindow}.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <Listeners>
						<ValidityChange Handler="#{uxAddNewSupplementalButton}.setDisabled(!valid);" />
					</Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Window>

        <%---------------------------------------------------------------------------------------------------------%>
      

    </form>
</body>
</html>

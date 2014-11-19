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
         .allowBlank-field {
            background-color: #EFF7FF !important;
            background-image: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <div></div>
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
                <Items>
        <ext:GridPanel ID="uxSupplementalCrossingGrid" Title="CROSSING LIST FOR SUPPLEMENTAL ENTRY" Region="North" runat="server" Layout="HBoxLayout" Collapsible="true" SimpleSelect="true">
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
                                <ext:ModelField Name="STATE" />

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

                    <ext:Column ID="uxMainCrossing" runat="server" DataIndex="CROSSING_NUMBER" Text="DOT #" Flex="1" />
                    <ext:Column ID="Column12" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" />
                    <ext:Column ID="uxSubDiv" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />
                    <ext:Column ID="uxMTM" runat="server" DataIndex="STATE" Text="State" Flex="1" />

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
                <Select Handler="#{uxAddSuppButton}.enable(); #{uxSupplementalProjectStore}.reload();" />
                <Deselect Handler="#{uxAddSuppButton}.disable();" />
            </Listeners>
           
        </ext:GridPanel>
       

        <ext:GridPanel ID="uxSupplementalGrid" Title="SUPPLEMENTAL ENTRIES" runat="server" Region="Center" Layout="FitLayout">

            <Store>
                <ext:Store runat="server"
                    ID="uxSupplementalStore" OnReadData="GetSupplementalGridData" AutoDataBind="true" AutoLoad="true" PageSize="10" GroupField="CROSSING_NUMBER">
                   <%-- <Parameters>
                        <ext:StoreParameter Name="crossingId" Value="Ext.encode(#{uxSupplementalCrossingGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                    </Parameters>--%>
                    <Model>
                        <ext:Model ID="Model1" runat="server">
                            <Fields>
                                <ext:ModelField Name="CROSSING_ID" />
                                <ext:ModelField Name="CROSSING_NUMBER" />
                                <ext:ModelField Name="SUPPLEMENTAL_ID" />
                                <ext:ModelField Name="APPROVED_DATE" Type="Date" />
                                <ext:ModelField Name="CUT_TIME" Type="Date" />
                                <ext:ModelField Name="SERVICE_TYPE" />
                                <ext:ModelField Name="TRUCK_NUMBER" />
                                <ext:ModelField Name="SQUARE_FEET" />
                                <ext:ModelField Name="RECURRING" />
                                <ext:ModelField Name="REMARKS" />
                            </Fields>
                        </ext:Model>
                    </Model>
                      <Proxy>
                            <ext:PageProxy />
                        </Proxy>
                     <Sorters>
                        <ext:DataSorter Property="APPROVED_DATE" Direction="ASC" />
                    </Sorters>
                </ext:Store>
            </Store>
            

            <ColumnModel>
                <Columns>
                     <ext:Column ID="Column2" runat="server" DataIndex="CROSSING_NUMBER" Text="DOT #" Flex="1" />
                    <ext:DateColumn ID="DateColumn1" runat="server" DataIndex="APPROVED_DATE" Text="Approved Date" Flex="1" Format="MM/dd/yyyy" />
                    <ext:DateColumn ID="DateColumn2" runat="server" DataIndex="CUT_TIME" Text="Cut Date" Flex="1" Format="MM/dd/yyyy" />
                    <ext:Column ID="Column6" runat="server" DataIndex="SERVICE_TYPE" Text="Service Type" Flex="1" />
                    <ext:Column ID="Column1" runat="server" DataIndex="SQUARE_FEET" Text="Square Feet" Flex="1" />
                    <ext:Column ID="Column13" runat="server" DataIndex="RECURRING" Text="Recurring" Flex="1" />
                    <ext:Column ID="Column5" runat="server" DataIndex="REMARKS" Text="Remarks" Flex="3" />
                   
                </Columns>
            </ColumnModel>
            <SelectionModel>
                <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" AllowDeselect="true" Mode="Single" />
            </SelectionModel>
             <TopBar>
             <ext:Toolbar ID="Toolbar1" runat="server">
            <Items>
                <ext:Button ID="uxAddSuppButton" runat="server" Text="Add Supplemental" Icon="ApplicationAdd" Disabled="true">
                    <Listeners>
                        <Click Handler="#{uxAddNewSupplementalWindow}.show()" />
                    </Listeners>
                    <DirectEvents>
                        <Click OnEvent="deSetFocus" />
                    </DirectEvents>
                </ext:Button>
               <ext:Button ID="uxUpdateSuppButton" runat="server" Text="Update Cut Date" Icon="ApplicationEdit" Disabled="true">
                    <Listeners>
                        <Click Handler="#{uxUpdateSupplementalWindow}.show()" />
                    </Listeners>
                   <DirectEvents>
                    <Click OnEvent="deUpdateSupplemental">
                        <ExtraParams>
                            <ext:Parameter Name="SupplementalId" Value="#{uxSupplementalGrid}.getSelectionModel().getSelection()[0].data.SUPPLEMENTAL_ID" Mode="Raw" />
                        </ExtraParams>
                    </Click>
                </DirectEvents>
                    <DirectEvents>
                        <Click OnEvent="deFocusUpdate" />
                    </DirectEvents>
                </ext:Button>
                <ext:Button ID="uxRemoveSuppButton" runat="server" Text="Delete Supplemental" Icon="ApplicationDelete" Disabled="true">
                    <DirectEvents>
                        <Click OnEvent="deRemoveSupplemental">
                            <Confirmation ConfirmRequest="true" Title="Remove?" Message="Are you sure you want to delete this supplemental entry?" />

                            <ExtraParams>
                                <ext:Parameter Name="SupplementalInfo" Value="Ext.encode(#{uxSupplementalGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                            </ExtraParams>
                       
                        </Click>
                    </DirectEvents>

                </ext:Button>
                 <ext:Checkbox runat="server" ID="uxToggleClosed" BoxLabel="Include Completed Crossings" BoxLabelAlign="After">
                        <Listeners>
                            <Change Handler="#{uxSupplementalStore}.reload()" />
                        </Listeners>
                    </ext:Checkbox>
            </Items>
        </ext:Toolbar>
                </TopBar>
            <Listeners>
                <Select Handler="#{uxRemoveSuppButton}.enable(); #{uxUpdateSuppButton}.enable()" />
                <Deselect Handler="#{uxRemoveSuppButton}.disable(); #{uxUpdateSuppButton}.disable();" />
            </Listeners>
             <Features>
                <ext:Grouping ID="Grouping1"
                    runat="server"
                    HideGroupedHeader="true" Collapsible="false" Cls="x-grid-group-title; x-grid-group-hd" />
            </Features>
            <BottomBar>
                <ext:PagingToolbar runat="server" HideRefresh="true" />
            </BottomBar>
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
                                <ext:DateField ID="uxAddApprovedDateField" runat="server" FieldLabel="Approved Date" AnchorHorizontal="100%" LabelAlign="Right" AllowBlank="false"  TabIndex="1"  InvalidCls="allowBlank" IndicatorIcon="BulletRed"  MsgTarget="Side"/>
                                <ext:DateField ID="uxAddCutDateField" runat="server" FieldLabel="Cut Date" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="2" />                       
                                  <ext:Label ID="Label2" runat="server" Text="" Width="70" />                                
                                <ext:Checkbox ID="uxAddRecurringBox" runat="server" BoxLabel="Recurring" BoxLabelAlign="After" AllowBlank="false" TabIndex="3" />

                            </Items>
                        </ext:FieldContainer>
                                                     
                              <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                            <Items>

                                <ext:TextField runat="server" ID="uxAddSquareFeet" FieldLabel="Square Feet" LabelAlign="Right" AnchorHorizontal="100%" TabIndex="4" />
                               
                                 <ext:DropDownField ID="uxAddPricingGrid" runat="server" FieldLabel="Service Type" AnchorHorizontal="100%" LabelAlign="Right" Width="480" TabIndex="5" Mode="ValueText" Editable="false" AllowBlank="false"  InvalidCls="allowBlank" IndicatorIcon="BulletRed"  MsgTarget="Side">
                                                    <Component>
                                                        <ext:GridPanel runat="server"
                                                            ID="uxSupplementalPricingGrid"
                                                            Layout="HBoxLayout">
                                                            <Store>
                                                                <ext:Store runat="server"
                                                                    ID="uxSupplementalPricingStore"
                                                                    PageSize="10"
                                                                    RemoteSort="true"
                                                                    OnReadData="deAddPricingGrid">
                                                                   
                                                                    <Model>
                                                                        <ext:Model ID="Model3" runat="server">
                                                                            <Fields>
                                                                                <ext:ModelField Name="PRICING_ID" />
                                                                                <ext:ModelField Name="SERVICE_CATEGORY" />
                                                                                <ext:ModelField Name="PRICE" />
                                                                             

                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>
                                                                    <Proxy>
                                                                        <ext:PageProxy />
                                                                    </Proxy>
                                                       <Sorters>
                                                             <ext:DataSorter Property="PRICE" Direction="ASC" />
                                                       </Sorters>
                                                                </ext:Store>
                                                            </Store>
                                                            <ColumnModel>
                                                                <Columns>
                                                                    <ext:Column ID="Column3" runat="server" DataIndex="SERVICE_CATEGORY" Text="Service Category" Flex="1" />
                                                                    <ext:Column ID="Column7" runat="server" DataIndex="PRICE" Text="Price" Flex="1" />
                                                                 
                                                                </Columns>
                                                            </ColumnModel>
                                                            <BottomBar>
                                                                <ext:PagingToolbar ID="PagingToolbar2" runat="server" />
                                                            </BottomBar>
                                                            <SelectionModel>
                                                                <ext:RowSelectionModel ID="RowSelectionModel4" runat="server" Mode="Single" />
                                                            </SelectionModel>
                                                            <DirectEvents>
                                                                <SelectionChange OnEvent="deAddPricingValue">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="PricingId" Value="#{uxSupplementalPricingGrid}.getSelectionModel().getSelection()[0].data.PRICING_ID" Mode="Raw" />
                                                                        <ext:Parameter Name="ServiceCategory" Value="#{uxSupplementalPricingGrid}.getSelectionModel().getSelection()[0].data.SERVICE_CATEGORY" Mode="Raw" />
                                                                        <ext:Parameter Name="Type" Value="Add" />
                                                                    </ExtraParams>
                                                                </SelectionChange>
                                                            </DirectEvents>
                                                            <Plugins>
                                                                <ext:FilterHeader runat="server" ID="uxAddPricingFilter" Remote="true" />
                                                            </Plugins>
                                                        </ext:GridPanel>
                                                    </Component>
                                                </ext:DropDownField>
                                <ext:Label ID="Label1" runat="server" Width="65" />

                            </Items>
                        </ext:FieldContainer> 
                        
                       
                           <ext:DropDownField ID="uxAddProjectDropDownField" runat="server" FieldLabel="Choose Project" AnchorHorizontal="100%" LabelAlign="Right" Width="755" TabIndex="6" Mode="ValueText" Editable="false" AllowBlank="false"  InvalidCls="allowBlank" IndicatorIcon="BulletRed"  MsgTarget="Side">
                                                    <Component>
                                                        <ext:GridPanel runat="server"
                                                            ID="uxAddProject"
                                                            Layout="HBoxLayout">
                                                            <Store>
                                                                <ext:Store runat="server"
                                                                    ID="uxSupplementalProjectStore"
                                                                    PageSize="10"
                                                                    RemoteSort="true"
                                                                    OnReadData="deAddProjectGrid">
                                                                     <Parameters>
                                                                         <ext:StoreParameter Name="CrossingId" Value="#{uxSupplementalCrossingGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                                                     </Parameters>
                                                                    <Model>
                                                                        <ext:Model ID="Model6" runat="server">
                                                                            <Fields>
                                                                                <ext:ModelField Name="PROJECT_ID" />
                                                                                <ext:ModelField Name="LONG_NAME" />
                                                                                <ext:ModelField Name="SEGMENT1" />
                                                                                <ext:ModelField Name="ORGANIZATION_NAME" />
                                                                                <ext:ModelField Name="CARRYING_OUT_ORGANIZATION_ID" />
                                                                                <ext:ModelField Name="PROJECT_TYPE" />
                                                                                <ext:ModelField Name="PROJECT_STATUS_CODE" />
                                                                                <ext:ModelField Name="TEMPLATE_FLAG" />
                                                                                <ext:ModelField Name="RAILROAD_ID" />
                                                                                <ext:ModelField Name="CROSSING_ID" />

                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>
                                                                    <Proxy>
                                                                        <ext:PageProxy />
                                                                    </Proxy>
                                                       <Sorters>
                                                             <ext:DataSorter Property="SEGMENT1" Direction="ASC" />
                                                       </Sorters>
                                                                </ext:Store>
                                                            </Store>
                                                            <ColumnModel>
                                                                <Columns>
                                                                    <ext:Column ID="Column11" runat="server" DataIndex="SEGMENT1" Text="Project" Flex="1" />
                                                                    <ext:Column ID="Column4" runat="server" DataIndex="LONG_NAME" Text="Project Name" Flex="2" />
                                                                    <ext:Column ID="Column14" runat="server" DataIndex="ORGANIZATION_NAME" Text="Organization Name" Flex="1" />
                                                                </Columns>
                                                            </ColumnModel>
                                                            <BottomBar>
                                                                <ext:PagingToolbar ID="PagingToolbar3" runat="server" />
                                                            </BottomBar>
                                                            <SelectionModel>
                                                                <ext:RowSelectionModel ID="RowSelectionModel3" runat="server" Mode="Single" />
                                                            </SelectionModel>
                                                            <DirectEvents>
                                                                <SelectionChange OnEvent="deAddProjectValue">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="ProjectId" Value="#{uxAddProject}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
                                                                        <ext:Parameter Name="ProjectName" Value="#{uxAddProject}.getSelectionModel().getSelection()[0].data.LONG_NAME" Mode="Raw" />
                                                                        <ext:Parameter Name="Type" Value="Add" />
                                                                    </ExtraParams>
                                                                </SelectionChange>
                                                            </DirectEvents>
                                                            <Plugins>
                                                                <ext:FilterHeader runat="server" ID="uxAddProjectFilter" Remote="true" />
                                                            </Plugins>
                                                        </ext:GridPanel>
                                                    </Component>
                                                </ext:DropDownField>



                                           

                        <ext:TextArea ID="uxAddRemarks" runat="server" FieldLabel="Remarks" AnchorHorizontal="92%" LabelAlign="Right" TabIndex="7" />
                    </Items>
                    <Buttons>
                        <ext:Button ID="uxAddNewSupplementalButton" runat="server" Text="Add" Icon="Add" TabIndex="8" >
                            <DirectEvents>
                                <Click OnEvent="deAddSupplemental">
                                    <ExtraParams>
                                        <ext:Parameter Name="CrossingId" Value="#{uxSupplementalCrossingGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="uxCancelNewSupplementalButton" runat="server" Text="Cancel" Icon="Delete" TabIndex="9" >
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
        <%------------------------------------------------------------------------------------------------------------------------------------------------------------------%>

                     <ext:Window runat="server"
            ID="uxUpdateSupplementalWindow"
            Layout="FormLayout"
            Hidden="true"
            Title="Add Cut Date"
            Width="800" Modal="true">
            <Items>
                <ext:FormPanel ID="uxUpdateSupplementalForm" runat="server" Layout="FormLayout">
                    <Items>
                        <ext:FieldContainer ID="FieldContainer3" runat="server" Layout="HBoxLayout">
                            <Items>
                              
                                <ext:DateField ID="uxReadOnlyAppDateField" runat="server" FieldLabel="Approved Date" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="1" ReadOnly="true" />                       
                                <ext:DateField ID="uxUpdateCutDate" runat="server" FieldLabel="Cut Date" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="2" />                       
                                  <ext:Label ID="Label3" runat="server" Text="" Width="70" />                                
                                <ext:Checkbox ID="uxReadOnlyRecurring" runat="server" BoxLabel="Recurring" BoxLabelAlign="After" AllowBlank="false" TabIndex="3" ReadOnly="true" />

                            </Items>
                        </ext:FieldContainer>
                                                     
                              <ext:FieldContainer ID="FieldContainer4" runat="server" Layout="HBoxLayout">
                            <Items>

                                <ext:TextField runat="server" ID="uxReadOnlySqFt" FieldLabel="Square Feet" LabelAlign="Right" AnchorHorizontal="100%" TabIndex="4" ReadOnly="true" />
                                 <ext:TextField runat="server" ID="uxReadOnlyServiceCategory" FieldLabel="Service Category" LabelAlign="Right" AnchorHorizontal="100%" TabIndex="4" ReadOnly="true" />
                                <%-- <ext:DropDownField ID="DropDownField1" runat="server" FieldLabel="Service Type" AnchorHorizontal="100%" LabelAlign="Right" Width="480" TabIndex="5" Mode="ValueText" Editable="false" AllowBlank="false"  InvalidCls="allowBlank" IndicatorIcon="BulletRed"  MsgTarget="Side">
                                                    <Component>
                                                        <ext:GridPanel runat="server"
                                                            ID="GridPanel1"
                                                            Layout="HBoxLayout">
                                                            <Store>
                                                                <ext:Store runat="server"
                                                                    ID="Store1"
                                                                    PageSize="10"
                                                                    RemoteSort="true"
                                                                    OnReadData="deAddPricingGrid">
                                                                   
                                                                    <Model>
                                                                        <ext:Model ID="Model4" runat="server">
                                                                            <Fields>
                                                                                <ext:ModelField Name="PRICING_ID" />
                                                                                <ext:ModelField Name="SERVICE_CATEGORY" />
                                                                                <ext:ModelField Name="PRICE" />
                                                                             

                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>
                                                                    <Proxy>
                                                                        <ext:PageProxy />
                                                                    </Proxy>
                                                       <Sorters>
                                                             <ext:DataSorter Property="PRICE" Direction="ASC" />
                                                       </Sorters>
                                                                </ext:Store>
                                                            </Store>
                                                            <ColumnModel>
                                                                <Columns>
                                                                    <ext:Column ID="Column8" runat="server" DataIndex="SERVICE_CATEGORY" Text="Service Category" Flex="1" />
                                                                    <ext:Column ID="Column9" runat="server" DataIndex="PRICE" Text="Price" Flex="1" />
                                                                 
                                                                </Columns>
                                                            </ColumnModel>
                                                            <BottomBar>
                                                                <ext:PagingToolbar ID="PagingToolbar4" runat="server" />
                                                            </BottomBar>
                                                            <SelectionModel>
                                                                <ext:RowSelectionModel ID="RowSelectionModel5" runat="server" Mode="Single" />
                                                            </SelectionModel>
                                                            <DirectEvents>
                                                                <SelectionChange OnEvent="deAddPricingValue">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="PricingId" Value="#{uxSupplementalPricingGrid}.getSelectionModel().getSelection()[0].data.PRICING_ID" Mode="Raw" />
                                                                        <ext:Parameter Name="ServiceCategory" Value="#{uxSupplementalPricingGrid}.getSelectionModel().getSelection()[0].data.SERVICE_CATEGORY" Mode="Raw" />
                                                                        <ext:Parameter Name="Type" Value="Add" />
                                                                    </ExtraParams>
                                                                </SelectionChange>
                                                            </DirectEvents>
                                                            <Plugins>
                                                                <ext:FilterHeader runat="server" ID="FilterHeader2" Remote="true" />
                                                            </Plugins>
                                                        </ext:GridPanel>
                                                    </Component>
                                                </ext:DropDownField>--%>
                                <ext:Label ID="Label4" runat="server" Width="65" />

                            </Items>
                        </ext:FieldContainer> 
                        
                         <ext:TextField runat="server" ID="uxReadOnlyProject" FieldLabel="Project" LabelAlign="Right" AnchorHorizontal="100%" TabIndex="4" ReadOnly="true" />
                          <%-- <ext:DropDownField ID="DropDownField2" runat="server" FieldLabel="Choose Project" AnchorHorizontal="100%" LabelAlign="Right" Width="755" TabIndex="6" Mode="ValueText" Editable="false" AllowBlank="false"  InvalidCls="allowBlank" IndicatorIcon="BulletRed"  MsgTarget="Side">
                                                    <Component>
                                                        <ext:GridPanel runat="server"
                                                            ID="GridPanel2"
                                                            Layout="HBoxLayout">
                                                            <Store>
                                                                <ext:Store runat="server"
                                                                    ID="Store2"
                                                                    PageSize="10"
                                                                    RemoteSort="true"
                                                                    OnReadData="deAddProjectGrid">
                                                                     <Parameters>
                                                                         <ext:StoreParameter Name="CrossingId" Value="#{uxSupplementalCrossingGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                                                     </Parameters>
                                                                    <Model>
                                                                        <ext:Model ID="Model5" runat="server">
                                                                            <Fields>
                                                                                <ext:ModelField Name="PROJECT_ID" />
                                                                                <ext:ModelField Name="LONG_NAME" />
                                                                                <ext:ModelField Name="SEGMENT1" />
                                                                                <ext:ModelField Name="ORGANIZATION_NAME" />
                                                                                <ext:ModelField Name="CARRYING_OUT_ORGANIZATION_ID" />
                                                                                <ext:ModelField Name="PROJECT_TYPE" />
                                                                                <ext:ModelField Name="PROJECT_STATUS_CODE" />
                                                                                <ext:ModelField Name="TEMPLATE_FLAG" />
                                                                                <ext:ModelField Name="RAILROAD_ID" />
                                                                                <ext:ModelField Name="CROSSING_ID" />

                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>
                                                                    <Proxy>
                                                                        <ext:PageProxy />
                                                                    </Proxy>
                                                       <Sorters>
                                                             <ext:DataSorter Property="SEGMENT1" Direction="ASC" />
                                                       </Sorters>
                                                                </ext:Store>
                                                            </Store>
                                                            <ColumnModel>
                                                                <Columns>
                                                                    <ext:Column ID="Column10" runat="server" DataIndex="SEGMENT1" Text="Project" Flex="1" />
                                                                    <ext:Column ID="Column15" runat="server" DataIndex="LONG_NAME" Text="Project Name" Flex="2" />
                                                                    <ext:Column ID="Column16" runat="server" DataIndex="ORGANIZATION_NAME" Text="Organization Name" Flex="1" />
                                                                </Columns>
                                                            </ColumnModel>
                                                            <BottomBar>
                                                                <ext:PagingToolbar ID="PagingToolbar5" runat="server" />
                                                            </BottomBar>
                                                            <SelectionModel>
                                                                <ext:RowSelectionModel ID="RowSelectionModel6" runat="server" Mode="Single" />
                                                            </SelectionModel>
                                                            <DirectEvents>
                                                                <SelectionChange OnEvent="deAddProjectValue">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="ProjectId" Value="#{uxAddProject}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
                                                                        <ext:Parameter Name="ProjectName" Value="#{uxAddProject}.getSelectionModel().getSelection()[0].data.LONG_NAME" Mode="Raw" />
                                                                        <ext:Parameter Name="Type" Value="Add" />
                                                                    </ExtraParams>
                                                                </SelectionChange>
                                                            </DirectEvents>
                                                            <Plugins>
                                                                <ext:FilterHeader runat="server" ID="FilterHeader3" Remote="true" />
                                                            </Plugins>
                                                        </ext:GridPanel>
                                                    </Component>
                                                </ext:DropDownField>--%>



                                           

                        <ext:TextArea ID="uxEditRemarks" runat="server" FieldLabel="Remarks" AnchorHorizontal="92%" LabelAlign="Right" TabIndex="7" />
                    </Items>
                    <Buttons>
                        <ext:Button ID="Button1" runat="server" Text="Update" Icon="Add" TabIndex="8" >
                            <DirectEvents>
                                <Click OnEvent="deUpdateSupplementalForm">
                                    <ExtraParams>
                                        <ext:Parameter Name="SupplementalId" Value="#{uxSupplementalGrid}.getSelectionModel().getSelection()[0].data.SUPPLEMENTAL_ID" Mode="Raw" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="Button2" runat="server" Text="Cancel" Icon="Delete" TabIndex="9" >
                            <Listeners>
                                <Click Handler="#{uxUpdateSupplementalWindow}.hide(); #{uxUpdateSupplementalForm}.reset();" />
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
      </Items>
        </ext:Viewport>

    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umBillingInvoice.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umBillingInvoice" %>

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
      <ext:FormPanel ID="uxFilterForm" runat="server" Margin="5" Region="North" Title="Crossing Invoice Form">
                <Items>
                        <ext:TextField ID="uxRRCI" runat="server" FieldLabel="Railroad" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" Hidden="true" />

                    <ext:FieldSet ID="FieldSet1" runat="server" Title="Invoicing Form">
                        <Items>
                             <ext:ComboBox ID="uxAddAppReqeusted"
                                    runat="server"
                                    FieldLabel="Application #"
                                    LabelAlign="Right"
                                    DisplayField="type"
                                    ValueField="type"
                                    QueryMode="Local"
                                    TypeAhead="true" Width="300" AllowBlank="false" ForceSelection="true" TabIndex="1" InvalidCls="allowBlank" IndicatorIcon="BulletRed"  MsgTarget="Side">
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
                             
                            <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                <Items>
                                  <ext:DateField ID="uxStartDate" runat="server" AnchorHorizontal="100%" FieldLabel="Start Date" LabelAlign="Right" Editable="false" TabIndex="2" EmptyText="ALL" />
                                  <ext:ComboBox ID="uxAddServiceUnit"
                            runat="server" FieldLabel="Service Unit"
                            LabelAlign="Right"
                            AnchorHorizontal="25%"
                            DisplayField="service_unit"
                            ValueField="service_unit"
                            QueryMode="Local" TypeAhead="true" TabIndex="4" ForceSelection="true"  EmptyText="ALL" >
                            <Store>
                                <ext:Store runat="server"
                                    ID="uxAddServiceUnitStore">
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
                      
                                     </Items>
                            </ext:FieldContainer>
                           <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:DateField ID="uxEndDate" runat="server" AnchorHorizontal="100%" FieldLabel="End Date" LabelAlign="Right"  Editable="false" TabIndex="3" EmptyText="ALL" />
                                <ext:ComboBox ID="uxAddSubDiv"
                            runat="server"
                            FieldLabel="Sub-Division"
                            LabelAlign="Right"
                            AnchorHorizontal="25%"
                            DisplayField="sub_division"
                            ValueField="sub_division"
                            TypeAhead="true" TabIndex="5" ForceSelection="true"  EmptyText="ALL" >
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
                                        <ext:Label ID="Label1" runat="server" Text="" Width="25" />

                    <ext:Checkbox runat="server" ID="uxToggleNonSub" BoxLabel="Non-Sub" BoxLabelAlign="After">
                        
                    </ext:Checkbox>
                                 </Items>
                            </ext:FieldContainer>
                                   
                        </Items>
                    </ext:FieldSet>
                      <ext:FieldSet ID="FieldSet2" runat="server" Title="Invoice Information">
                        <Items>
                                  <ext:TextField ID="uxInvoiceNumber" runat="server" FieldLabel="Invoice Number" AnchorHorizontal="20%" LabelAlign="Right" TabIndex="6" AllowBlank="false" InvalidCls="allowBlank" IndicatorIcon="BulletRed"  MsgTarget="Side" />
                                  <ext:DateField ID="uxInvoiceDate" runat="server" AnchorHorizontal="20s%" FieldLabel="Invoice Date" LabelAlign="Right"  Editable="false" TabIndex="7" AllowBlank="false" InvalidCls="allowBlank" IndicatorIcon="BulletRed"  MsgTarget="Side" />                                 
                        </Items>
                    </ext:FieldSet>
                </Items>
                <BottomBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Button runat="server"
                                ID="Button4"
                                Text="View"
                                Icon="ApplicationViewList" Disabled="true">
                                
                         <Listeners>
                                <Click Handler="#{uxInvoiceFormStore}.load()" />
                            </Listeners>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="Button3"
                                Text="Undo Invoice"
                                Icon="StopRed">
                             <DirectEvents>
                            <Click OnEvent="deClearFilters" />
                            </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </BottomBar>
                 <Listeners>
			 <ValidityChange Handler="#{Button4}.setDisabled(!valid);" />
		  </Listeners>
            
            </ext:FormPanel>
             <ext:GridPanel ID="uxApplicationEntryGrid" Title="Completed Crossings" runat="server" Region="Center" Frame="false" Collapsible="true" MultiSelect="true" >
                <SelectionModel>
                    <ext:CheckboxselectionModel ID="CheckboxSelectionModel2" runat="server" AllowDeselect="true" Mode="Multi" />
                </SelectionModel>
                <Store>
                    <ext:Store runat="server"
                        ID="uxInvoiceFormStore" OnReadData="deInvoiceGrid" AutoDataBind="true" AutoLoad="false" >
                      
                        <Model>
                            <ext:Model ID="Model1" runat="server" IDProperty="APPLICATION_ID">
                                <Fields>
                                    <ext:ModelField Name="APPLICATION_ID" />
                                    <ext:ModelField Name="APPLICATION_DATE" />
                                    <ext:ModelField Name="CROSSING_ID" />  
                                    <ext:ModelField Name="CROSSING_NUMBER" /> 
                                    <ext:ModelField Name="APPLICATION_REQUESTED" />                               
                                    <ext:ModelField Name="MILE_POST" />
                                    <ext:ModelField Name="SERVICE_UNIT" />
                                    <ext:ModelField Name="SUB_DIVISION" />
                                    <ext:ModelField Name="SEGMENT1" />
                                    <ext:ModelField Name="PROJECT_ID" />

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
                         <ext:Column ID="Column1" runat="server" DataIndex="CROSSING_NUMBER" Text="DOT Number" Flex="1" />
                        <ext:Column ID="Column2" runat="server" DataIndex="MILE_POST" Text="MP" Flex="1" />
                        <ext:Column ID="Column4" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" />
                        <ext:DateColumn ID="DateColumn1" runat="server" DataIndex="APPLICATION_DATE" Text="Application Date" Flex="1" Format="MM/dd/yyyy" />
                        <ext:Column ID="Column3" runat="server" DataIndex="SUB_DIVISION" Text="Sub Division" Flex="1" />                     
                        <ext:Column ID="Column13" runat="server" DataIndex="SEGMENT1" Text="Project #" Flex="1" />

                    </Columns>
                </ColumnModel> 
                   <DirectEvents>
                       <SelectionChange OnEvent="deValidationInvoiceButton" />
                     </DirectEvents>
               <BottomBar>
                    <ext:Toolbar ID="Toolbar2" runat="server">
                        <Items>
                            <ext:Button runat="server"
                                ID="Button1"
                                Text="Invoice"
                                Icon="PlayGreen" Disabled="true">
                         <DirectEvents>
                             <Click OnEvent="deAddInvoice" >
                                   <ExtraParams>
                                    <ext:Parameter Name="selectedApps" Value="Ext.encode(#{uxApplicationEntryGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                                </ExtraParams>
                                 </Click>
                         </DirectEvents>
                               <%-- <Listeners>
                                    <Click Handler="#{uxInvoiceReportStore}.reload()" />
                                </Listeners>--%>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="Button2"
                                Text="Cancel Selections"
                                Icon="StopRed" Disabled="true">
                             <DirectEvents>
                            <Click OnEvent="deResetInvoice" />
                            </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </BottomBar>
            </ext:GridPanel>          
                    </Items>
            </ext:Viewport>
    </form>
</body>
</html>

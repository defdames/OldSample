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
                    <ext:FieldSet ID="FieldSet1" runat="server" Title="Invoicing Form">
                        <Items>
                             <ext:ComboBox ID="uxAddAppReqeusted"
                                    runat="server"
                                    FieldLabel="Application #"
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
                            <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                <Items>
                                  <ext:DateField ID="uxStartDate" runat="server" AnchorHorizontal="100%" FieldLabel="Start Date" LabelAlign="Right" Editable="false" TabIndex="1" />
                                  <ext:TextField ID="uxInvoiceNumber" runat="server" FieldLabel="Invoice Number" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="3" AllowBlank="false" />
                                </Items>
                            </ext:FieldContainer>
                           <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:DateField ID="uxEndDate" runat="server" AnchorHorizontal="100%" FieldLabel="End Date" LabelAlign="Right"  Editable="false" TabIndex="2" />
                                    <ext:DateField ID="uxInvoiceDate" runat="server" AnchorHorizontal="100s%" FieldLabel="Invoice Date" LabelAlign="Right"  Editable="false" TabIndex="4" AllowBlank="false" />
                                </Items>
                            </ext:FieldContainer>
                           
                                                   
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
                                <Click Handler="#{uxInvoiceFormStore}.load(); #{uxBillingReportWindow}.Show()" />
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
                            <ext:Model ID="Model1" runat="server">
                                <Fields>
                                    <ext:ModelField Name="APPLICATION_ID" />
                                    <ext:ModelField Name="APPLICATION_DATE" />
                                    <ext:ModelField Name="CROSSING_ID" />  
                                    <ext:ModelField Name="CROSSING_NUMBER" /> 
                                    <ext:ModelField Name="APPLICATION_REQUESTED" />                               
                                    <ext:ModelField Name="MILE_POST" />
                                    <ext:ModelField Name="SERVICE_UNIT" />
                                    <ext:ModelField Name="SUB_DIVISION" />
                                  

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
                                <Listeners>
                                    <Click Handler="#{uxInvoiceReportStore}.reload()" />
                                </Listeners>
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
         <ext:Window runat="server"
            ID="uxBillingReportWindow"
            Layout="FormLayout"
            Hidden="true"
            Title="Invoice Report"
            Height="550" Width="650" Modal="true" Closable="false">
            <Items>
                
                 <ext:FormPanel ID="uxViewInvoiceForm" runat="server" Height="30" Layout="FormLayout">
                    <Items>
                 <ext:FieldContainer runat="server" Layout="HBoxLayout" >
                <Items>
                <ext:TextField ID="InvoiceNumTextField" runat="server" FieldLabel="Invoice #" LabelAlign="Right" ReadOnly="true" />
                <ext:TextField ID="InvoiceDateTextField" runat="server" FieldLabel="Date" LabelAlign="Right" ReadOnly="true"  />
                </Items>
                </ext:FieldContainer>
       
                        </Items>
                        </ext:FormPanel>
                 <ext:GridPanel ID="GridPanel1" Title="Invoiced Items" Height="485" runat="server" Frame="false" Collapsible="true" >
               
                <Store>
                    <ext:Store runat="server"
                        ID="uxInvoiceReportStore" OnReadData="deInvoiceReportGrid" AutoDataBind="true" AutoLoad="false" GroupField="SUB_DIVISION">
                        <Parameters>
                              <ext:StoreParameter Name="selectedApps" Value="Ext.encode(#{uxApplicationEntryGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                        </Parameters>
                        <Model>
                            <ext:Model ID="Model2" runat="server">
                                <Fields>
                                    <ext:ModelField Name="APPLICATION_ID" />
                                    <ext:ModelField Name="APPLICATION_DATE" />
                                    <ext:ModelField Name="CROSSING_ID" />  
                                    <ext:ModelField Name="CROSSING_NUMBER" /> 
                                    <ext:ModelField Name="APPLICATION_REQUESTED" />                               
                                    <ext:ModelField Name="MILE_POST" />
                                    <ext:ModelField Name="SERVICE_UNIT" />
                                    <ext:ModelField Name="SUB_DIVISION" />
                                  

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
                         <ext:Column ID="Column5" runat="server" DataIndex="CROSSING_NUMBER" Text="DOT Number" Flex="1" />
                        <ext:Column ID="Column6" runat="server" DataIndex="MILE_POST" Text="MP" Flex="1" />
                        <ext:Column ID="Column7" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" />
                        <ext:DateColumn ID="DateColumn2" runat="server" DataIndex="APPLICATION_DATE" Text="Application Date" Flex="1" Format="MM/dd/yyyy" />
                        <ext:Column ID="Column8" runat="server" DataIndex="SUB_DIVISION" Text="Sub Division" Flex="1" />                     
                       

                    </Columns>
                </ColumnModel> 
                 <Features>
                <ext:Grouping ID="Grouping1"
                    runat="server"
                    HideGroupedHeader="true" Collapsible="false" Cls="x-grid-group-title; x-grid-group-hd" />
            </Features>
               <BottomBar>
                    <ext:Toolbar ID="Toolbar3" runat="server">
                        <Items>
                          
                           <ext:Button runat="server"
                                ID="Button5"
                                Text="Close Invoice Report"
                                Icon="BinClosed">
                             <DirectEvents>
                            <Click OnEvent="deCloseInvoice" />
                            </DirectEvents>
                                 <DirectEvents>
                            <Click OnEvent="deResetInvoice" />
                            </DirectEvents>
                                 <DirectEvents>
                            <Click OnEvent="deClearFilters" />
                            </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </BottomBar>
                  
            </ext:GridPanel>
        </Items>
             </ext:Window>
                    </Items>
            </ext:Viewport>
    </form>
</body>
</html>

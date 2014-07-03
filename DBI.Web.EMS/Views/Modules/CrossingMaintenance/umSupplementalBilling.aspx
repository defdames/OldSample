<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSupplementalBilling.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umSupplementalBilling" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <ext:ResourceManager ID="ResourceManager1" runat="server" />
          <div></div>
      <ext:FormPanel ID="uxFilterForm" runat="server" Margin="5" Title="Supplemental Invoice Form">
                <Items>
                    <ext:FieldSet ID="FieldSet1" runat="server" Title=" Supplement Invoicing Form">
                        <Items>
                          
                            <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                                <Items>
                                  <ext:DateField ID="uxStartDate" runat="server" AnchorHorizontal="100%" FieldLabel="Start Date" LabelAlign="Right" Editable="false" TabIndex="1" />
                                  <ext:TextField ID="uxInvoiceNumber" runat="server" FieldLabel="Invoice Number" AnchorHorizontal="100%" AllowBlank="false" LabelAlign="Right" TabIndex="3" />
                                </Items>
                            </ext:FieldContainer>
                           <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:DateField ID="uxEndDate" runat="server" AnchorHorizontal="100%" FieldLabel="End Date" LabelAlign="Right"  Editable="false" TabIndex="2" />
                                    <ext:DateField ID="uxInvoiceDate" runat="server" AnchorHorizontal="100s%" FieldLabel="Invoice Date" AllowBlank="false" LabelAlign="Right"  Editable="false" TabIndex="4" />
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
                                <Click Handler="#{uxInvoiceSupplementalStore}.load()" />
                            </Listeners>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="Button3"
                                Text="Cancel"
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

        <ext:GridPanel ID="uxInvoiceGrid" Title="Completed Crossings" runat="server" Region="North" Frame="false" Collapsible="true" MultiSelect="true" >
                <SelectionModel>
                    <ext:CheckboxselectionModel ID="CheckboxSelectionModel2" runat="server" AllowDeselect="true" Mode="Multi" />
                </SelectionModel>
                <Store>
                    <ext:Store runat="server"
                        ID="uxInvoiceSupplementalStore" OnReadData="deInvoiceSupplementalGrid" AutoDataBind="true" AutoLoad="false" >
                       <%-- <Parameters>
                              <ext:StoreParameter Name="crossingId" Value="Ext.encode(#{uxApplicationCrossingGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                        </Parameters>--%>
                        <Model>
                            <ext:Model ID="Model1" runat="server">
                                <Fields>
                                    <ext:ModelField Name="CROSSING_ID" />  
                                    <ext:ModelField Name="CROSSING_NUMBER" />                                
                                    <ext:ModelField Name="SUPPLEMENTAL_ID" />
                                    <ext:ModelField Name="APPROVED_DATE" /> 
                                    <ext:ModelField Name="MILE_POST" />
                                    <ext:ModelField Name="SERVICE_TYPE" />
                                    <ext:ModelField Name="TRUCK_NUMBER" />
                                    <ext:ModelField Name="SQUARE_FEET" />
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
                        <ext:Column ID="Column12" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" />
                        <ext:Column ID="Column3" runat="server" DataIndex="SUB_DIVISION" Text="Sub Division" Flex="1" />                     
                        <ext:DateColumn ID="DateColumn1" runat="server" DataIndex="APPROVED_DATE" Text="Approved Date" Flex="1" Format="MM/dd/yyyy" />
                    <ext:Column ID="Column6" runat="server" DataIndex="SERVICE_TYPE" Text="Service Type" Flex="1" />
                    <ext:Column ID="Column4" runat="server" DataIndex="TRUCK_NUMBER" Text="Truck" Flex="1" />
                    <ext:Column ID="Column5" runat="server" DataIndex="SQUARE_FEET" Text="Square Feet" Flex="1" />

                    </Columns>
                </ColumnModel> 
               <BottomBar>
                    <ext:Toolbar ID="Toolbar2" runat="server">
                        <Items>
                            <ext:Button runat="server"
                                ID="Button1"
                                Text="Invoice"
                                Icon="PlayGreen">
                        <DirectEvents>
                            <Click OnEvent="deAddInvoice" >
                                 <ExtraParams>
                                    <ext:Parameter Name="selectedSupp" Value="Ext.encode(#{uxInvoiceGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                                </ExtraParams>
                                </Click>
                        </DirectEvents>
                     
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="Button2"
                                Text="Undo Invoice"
                                Icon="StopRed">
                   
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </BottomBar>
            </ext:GridPanel>
    </div>
    </form>
</body>
</html>

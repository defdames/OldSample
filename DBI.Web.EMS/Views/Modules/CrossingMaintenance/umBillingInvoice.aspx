<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umBillingInvoice.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umBillingInvoice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
  
      <ext:ResourceManager ID="ResourceManager1" runat="server" />
          <div></div>
      <ext:FormPanel ID="uxFilterForm" runat="server" Margin="5" Title="Crossing Invoice Form">
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
                                <Click Handler="#{uxInvoiceFormStore}.load()" />
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
             <ext:GridPanel ID="uxApplicationEntryGrid" Title="Completed Crossings" runat="server" Region="North" Frame="false" Collapsible="true" MultiSelect="true" >
                <SelectionModel>
                    <ext:CheckboxselectionModel ID="CheckboxSelectionModel2" runat="server" AllowDeselect="true" Mode="Multi" />
                </SelectionModel>
                <Store>
                    <ext:Store runat="server"
                        ID="uxInvoiceFormStore" OnReadData="deInvoiceGrid" AutoDataBind="true" AutoLoad="false" >
                       <%-- <Parameters>
                              <ext:StoreParameter Name="crossingId" Value="Ext.encode(#{uxApplicationCrossingGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                        </Parameters>--%>
                        <Model>
                            <ext:Model ID="Model1" runat="server">
                                <Fields>
                                    <ext:ModelField Name="CROSSING_ID" />  
                                    <ext:ModelField Name="CROSSING_NUMBER" />                                
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
                        <ext:DateColumn ID="DateColumn2" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" Format="MM/dd/yyyy" />
                        <ext:Column ID="Column3" runat="server" DataIndex="SUB_DIVISION" Text="Sub Division" Flex="1" />                     
                       

                    </Columns>
                </ColumnModel> 
               <BottomBar>
                    <ext:Toolbar ID="Toolbar2" runat="server">
                        <Items>
                            <ext:Button runat="server"
                                ID="Button1"
                                Text="Invoice"
                                Icon="PlayGreen">
                         <%--<Listeners>
                                <Click Handler="#{uxIncidentStore}.load()" />
                            </Listeners>--%>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="Button2"
                                Text="Undo Invoice"
                                Icon="StopRed">
                           <%--  <DirectEvents>
                            <Click OnEvent="deClearFilters" />
                            </DirectEvents>--%>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </BottomBar>
            </ext:GridPanel>
    </form>
</body>
</html>

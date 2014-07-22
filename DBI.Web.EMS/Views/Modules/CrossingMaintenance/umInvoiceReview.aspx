<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umInvoiceReview.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umInvoiceReview" %>

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
      <ext:FormPanel ID="uxFilterForm" runat="server" Margin="5" Title="Select Invoice Type">
                <Items>
                    <ext:FieldSet ID="FieldSet1" runat="server" Title="Filter">
                        <Items>
                            <ext:ComboBox ID="uxInvoiceChoice"
                            runat="server"
                            FieldLabel="Invoice Type"
                            LabelAlign="Right"
                            AnchorHorizontal="25%"
                            DisplayField="type"
                            ValueField="type"
                            QueryMode="Local"
                            TypeAhead="true" AllowBlank="false" ForceSelection="true" TabIndex="1" >
                            <Store>
                                <ext:Store runat="server"
                                    ID="uxInvoiceChoiceStore" AutoDataBind="true">
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
                                <DirectEvents>
                                    <Select OnEvent="deChooseType" />
                                </DirectEvents>
                        </ext:ComboBox>
                                                   
                        </Items>
                    </ext:FieldSet>
                    
                </Items>
          
            </ext:FormPanel>
        <ext:GridPanel ID="uxInvoiceApplicationGrid" Title="Completed Crossings" runat="server" Region="North" Hidden="true" Frame="false" Collapsible="true" >
              
                <Store>
                    <ext:Store runat="server"
                        ID="uxInvoiceApplicationStore" OnReadData="deApplicationReviewGrid" AutoDataBind="true" AutoLoad="false" >
                      
                        <Model>
                            <ext:Model ID="Model4" runat="server">
                                <Fields>
                                    <ext:ModelField Name="APPLICATION_ID" />
                                    <ext:ModelField Name="INVOICE_ID" />
                                    <ext:ModelField Name="INVOICE_DATE" />
                                    <ext:ModelField Name="INVOICE_NUMBER" />
                                  

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
                         <ext:Column ID="Column2" runat="server" DataIndex="INVOICE_NUMBER" Text="Invoice Number" Flex="1" />
                        <ext:DateColumn ID="DateColumn3" runat="server" DataIndex="INVOICE_DATE" Text="Invoice Date" Flex="1" Format="MM/dd/yyyy" />
                                      
                       

                    </Columns>
                </ColumnModel> 
                
            </ext:GridPanel>
        <ext:GridPanel ID="uxSupplementalInvoiceGrid" Title="Supplemental Invoiced Items" runat="server" Region="North" Frame="false" Hidden="true" Collapsible="true" MultiSelect="true" >
                
                <Store>
                    <ext:Store runat="server"
                        ID="uxInvoiceSupplementalStore" OnReadData="deSupplementalReviewGrid"  AutoDataBind="true" AutoLoad="false" >
                    
                        <Model>
                            <ext:Model ID="Model1" runat="server">
                                <Fields>
                                    <ext:ModelField Name="SUPPLEMENTAL_ID" />
                                    <ext:ModelField Name="INVOICE_SUPP_ID" />  
                                    <ext:ModelField Name="INVOICE_SUPP_NUMBER" />                                
                                    <ext:ModelField Name="INVOICE_SUPP_DATE" /> 
                               

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
                         <ext:Column ID="Column1" runat="server" DataIndex="INVOICE_SUPP_NUMBER" Text="Invoice #" Flex="1" />
                        <ext:DateColumn ID="DateColumn1" runat="server" DataIndex="INVOICE_SUPP_DATE" Text="Invoice Date" Flex="1" Format="MM/dd/yyyy" />
                                         
                      

                    </Columns>
                </ColumnModel> 
              
            </ext:GridPanel>
      <%--  <ext:Window runat="server"
            ID="uxBillingReportWindow"
            Layout="FormLayout"
            Hidden="true"
            Title="Invoice Report"
            Height="550" Width="650" Modal="true" Closable="false">
            <Items>
                
                 <ext:FormPanel ID="uxViewInvoiceForm" runat="server" Height="30" Layout="FormLayout">
                    <Items>
                 <ext:FieldContainer ID="FieldContainer3" runat="server" Layout="HBoxLayout" >
                <Items>
                <ext:TextField ID="InvoiceNumTextField" runat="server" FieldLabel="Invoice #" LabelAlign="Right" ReadOnly="true" />
                <ext:TextField ID="InvoiceDateTextField" runat="server" FieldLabel="Date" LabelAlign="Right" ReadOnly="true" />
                </Items>
                </ext:FieldContainer>
       
                        </Items>
                        </ext:FormPanel>
                 <ext:GridPanel ID="GridPanel1" Title="Invoiced Items" Height="485" runat="server" Frame="false" Collapsible="true" >
               
                <Store>
                    <ext:Store runat="server"
                        ID="uxInvoiceReportStore" AutoDataBind="true" AutoLoad="false" GroupField="SUB_DIVISION">
                     
                        <Model>
                            <ext:Model ID="Model2" runat="server">
                                <Fields>
                                    <ext:ModelField Name="SUPPLEMENTAL_ID" />
                                    <ext:ModelField Name="APPROVED_DATE" />
                                    <ext:ModelField Name="CROSSING_ID" />  
                                    <ext:ModelField Name="CROSSING_NUMBER" /> 
                                    <ext:ModelField Name="TRUCK_NUMBER" />  
                                     <ext:ModelField Name="SERVICE_TYPE" />                              
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
                         <ext:Column ID="Column7" runat="server" DataIndex="CROSSING_NUMBER" Text="DOT Number" Flex="1" />
                        <ext:Column ID="Column8" runat="server" DataIndex="MILE_POST" Text="MP" Flex="1" />
                        <ext:Column ID="Column9" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" />
                        <ext:DateColumn ID="DateColumn2" runat="server" DataIndex="APPROVED_DATE" Text="Application Date" Flex="1" Format="MM/dd/yyyy" />
                        <ext:Column ID="Column10" runat="server" DataIndex="SUB_DIVISION" Text="Sub Division" Flex="1" />     
                        <ext:Column ID="Column11" runat="server" DataIndex="SERVICE_TYPE" Text="Service Type" Flex="1" />                 
                       

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
                       
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </BottomBar>
            </ext:GridPanel>
                </Items>
            </ext:Window>--%>
    </div>
    </form>
</body>
</html>

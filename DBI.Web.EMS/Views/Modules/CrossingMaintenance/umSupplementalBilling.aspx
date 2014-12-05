<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSupplementalBilling.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umSupplementalBilling" %>

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
    <script type="text/javascript">
        var isEditAdjustmentAllowed = function (e) {
            if (e.originalValue == 12345678910) {
                return false;
            }
        }
        var editAdjustment = function (editor, e) {
            if (e.originalValue != e.value) {
                SaveRecord.deSaveAdjustments(e.record.data.ADJ_ID, e.field, e.value);
            }
        }
        Ext.util.Format.CurrencyFactory = function (dp, dSeparator, tSeparator, symbol, red) {
            return function (n) {
                if (n == 12345678910) {
                    return " ";
                }

                var template = '<span style="color:{0};">{1}</span>';

                dp = Math.abs(dp) + 1 ? dp : 2;
                dSeparator = dSeparator || ".";
                tSeparator = tSeparator || ",";

                var m = /(\d+)(?:(\.\d+)|)/.exec(n + ""),
                    x = m[1].length > 3 ? m[1].length % 3 : 0;


                var r = (n < 0 ? '-' : '') // preserve minus sign
                        + (x ? m[1].substr(0, x) + tSeparator : "")
                        + m[1].substr(x).replace(/(\d{3})(?=\d)/g, "$1" + tSeparator)
                        + (dp ? dSeparator + (+m[2] || 0).toFixed(dp).substr(2) : "")
                        + symbol;

                return Ext.String.format(template, (n >= 0 || red == false) ? "black" : "red", r);
            };
        };
        // EXAMPLE
        //var colorErrors = function (value, metadata, record) {
        //    if (record.data.WARNING == "Error") {
        //        metadata.style = "background-color: red;";
        //    }
        //    else if (record.data.WARNING == "Warning") {
        //        metadata.style = "background-color: yellow;";
        //    }
        //    return value;
        //};
        var colorProjectOverride = function (value, metadata, record) {
            if (record.data.TYPE == "OVERRIDE") {
                metadata.style = "background-color: #FFFF99;";
            }
            return value;
        };
        var colorCompareOverride = function (value, metadata, record) {
            if (record.data.COMPARE_PRJ_OVERRIDE == "Y") {
                metadata.style = "background-color: #FFFF99;";
            }

            var template = '<span style="color:{0};">{1}</span>';

            var dp = 2;
            var dSeparator = ".";
            var tSeparator = ",";
            var symbol = '';
            var red = false;

            var m = /(\d+)(?:(\.\d+)|)/.exec(value + ""),
                x = m[1].length > 3 ? m[1].length % 3 : 0;
            var r = (value < 0 ? '-' : '') // preserve minus sign
                    + (x ? m[1].substr(0, x) + tSeparator : "")
                    + m[1].substr(x).replace(/(\d{3})(?=\d)/g, "$1" + tSeparator)
                    + (dp ? dSeparator + (+m[2] || 0).toFixed(dp).substr(2) : "")
                    + symbol;

            return Ext.String.format(template, (value >= 0 || red == false) ? "black" : "red", r);
        };
        var colorWEOverride = function (value, metadata, record) {
            if (record.data.WE_OVERRIDE == "Y") {
                metadata.style = "background-color: #FFFF99;";
            }

            var template = '<span style="color:{0};">{1}</span>';

            var dp = 2;
            var dSeparator = ".";
            var tSeparator = ",";
            var symbol = '';
            var red = false;

            var m = /(\d+)(?:(\.\d+)|)/.exec(value + ""),
                x = m[1].length > 3 ? m[1].length % 3 : 0;
            var r = (value < 0 ? '-' : '') // preserve minus sign
                    + (x ? m[1].substr(0, x) + tSeparator : "")
                    + m[1].substr(x).replace(/(\d{3})(?=\d)/g, "$1" + tSeparator)
                    + (dp ? dSeparator + (+m[2] || 0).toFixed(dp).substr(2) : "")
                    + symbol;

            return Ext.String.format(template, (value >= 0 || red == false) ? "black" : "red", r);
        };
    </script>
   <script>
       var saveData = function () {
           App.Hidden1.setValue(Ext.encode(App.GridPanel1.getRowsValues({ selectedOnly: false })));
       };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <ext:ResourceManager ID="ResourceManager1" runat="server" />
          <div></div>
        <ext:Hidden ID="Hidden1" runat="server" Hidden="true" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
                <Items>

      <ext:FormPanel ID="uxFilterForm" runat="server" Margin="5" Region="North" Title="Supplemental Invoice Form">
                <Items>
                        <ext:TextField ID="uxRRCI" runat="server" FieldLabel="Railroad" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" Hidden="true" />

                    <ext:FieldSet ID="FieldSet1" runat="server" Title=" Supplemental Invoicing Form">
                        <Items>
                          
                            <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                                <Items>
                                  <ext:DateField ID="uxStartDate" runat="server" AnchorHorizontal="100%" FieldLabel="Start Date" LabelAlign="Right" Editable="false" TabIndex="1" EmptyText="ALL" >
                                       <Plugins>
                                <ext:ClearButton ID="ClearButton1" runat="server" />
                            </Plugins>
                                      </ext:DateField>
                                  <ext:ComboBox ID="uxAddServiceUnit"
                            runat="server" FieldLabel="Service Unit"
                            LabelAlign="Right"
                            AnchorHorizontal="25%"
                            DisplayField="service_unit"
                            ValueField="service_unit"
                            QueryMode="Local" TypeAhead="true" TabIndex="3" ForceSelection="true"  EmptyText="ALL" >
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
                              <Plugins>
                                <ext:ClearButton ID="ClearButton2" runat="server" />
                            </Plugins>
                        </ext:ComboBox>
                                </Items>
                            </ext:FieldContainer>
                           <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:DateField ID="uxEndDate" runat="server" AnchorHorizontal="100%" FieldLabel="End Date" LabelAlign="Right"  Editable="false" TabIndex="2" EmptyText="ALL" >
                                         <Plugins>
                                <ext:ClearButton ID="ClearButton3" runat="server" />
                            </Plugins>
                                        </ext:DateField>
                                     <ext:ComboBox ID="uxAddSubDiv"
                            runat="server"
                            FieldLabel="Sub-Division"
                            LabelAlign="Right"
                            AnchorHorizontal="25%"
                            DisplayField="sub_division"
                            ValueField="sub_division"
                            TypeAhead="true" TabIndex="4" ForceSelection="true"  EmptyText="ALL" >
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
                              <Plugins>
                                <ext:ClearButton ID="ClearButton4" runat="server" />
                            </Plugins>
                        </ext:ComboBox>
                                </Items>
                            </ext:FieldContainer>
                           
                                                   
                        </Items>
                    </ext:FieldSet>
                     
                     <ext:FieldSet ID="FieldSet2" runat="server" Title=" Supplemental Invoice Information">
                        <Items>
                                    <ext:TextField ID="uxInvoiceNumber" runat="server" FieldLabel="Invoice Number" AnchorHorizontal="20%" AllowBlank="false" LabelAlign="Right" TabIndex="5" InvalidCls="allowBlank" IndicatorIcon="BulletRed"  MsgTarget="Side" >
                                         <Plugins>
                                <ext:ClearButton ID="ClearButton5" runat="server" />
                            </Plugins>
                                        </ext:TextField>

                                    <ext:DateField ID="uxInvoiceDate" runat="server" AnchorHorizontal="20s%" FieldLabel="Invoice Date" AllowBlank="false" LabelAlign="Right"  Editable="false" TabIndex="6" InvalidCls="allowBlank" IndicatorIcon="BulletRed"  MsgTarget="Side" >
                                         <Plugins>
                                <ext:ClearButton ID="ClearButton6" runat="server" />
                            </Plugins>
                                        </ext:DateField>
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

        <ext:GridPanel ID="uxInvoiceGrid" Title="Completed Crossings" runat="server" Region="Center" Frame="false" Collapsible="true" MultiSelect="true" >
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
                                    <ext:ModelField Name="CROSSING_ID"  />  
                                    <ext:ModelField Name="CROSSING_NUMBER" />                                
                                    <ext:ModelField Name="SUPPLEMENTAL_ID" />
                                    <ext:ModelField Name="APPROVED_DATE" DateFormat="MM/dd/yyyy" /> 
                                    <ext:ModelField Name="MILE_POST" />
                                    <ext:ModelField Name="SERVICE_TYPE" />
                                    <ext:ModelField Name="SQUARE_FEET" />
                                    <ext:ModelField Name="SERVICE_UNIT" />
                                    <ext:ModelField Name="SUB_DIVISION" />
                                     <ext:ModelField Name="SEGMENT1" />
                                    <ext:ModelField Name="PRICE" />
                                    <ext:ModelField Name="TOTALPRICE" />
                                </Fields>
                            </ext:Model>
                        </Model>
                        <Proxy>
                            <ext:PageProxy />
                        </Proxy>
                        <Sorters>
                            <ext:DataSorter Direction="ASC" Property="APPROVED_DATE" />
                        </Sorters>
                    </ext:Store>
                </Store>

                <ColumnModel>
                    <Columns>
                         <ext:Column ID="Column1" runat="server" DataIndex="CROSSING_NUMBER" Text="DOT #" Flex="1" />
                        <ext:Column ID="Column2" runat="server" DataIndex="MILE_POST" Text="MP" Flex="1" />
                        <ext:Column ID="Column12" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" />
                        <ext:Column ID="Column3" runat="server" DataIndex="SUB_DIVISION" Text="Sub Division" Flex="1" />                     
                        <ext:DateColumn ID="DateColumn1" runat="server" DataIndex="APPROVED_DATE" Text="Approved Date" Flex="1" Format="MM/dd/yyyy" />
                    <ext:Column ID="Column6" runat="server" DataIndex="SERVICE_TYPE" Text="Service Type" Flex="1" />
                    <ext:Column ID="Column5" runat="server" DataIndex="SQUARE_FEET" Text="Square Feet" Flex="1" />
                        <ext:Column ID="Column4" runat="server" DataIndex="PRICE" Text="Price Per Ft" Flex="1" >
                        <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                             </ext:Column>
                         <ext:Column ID="Column7" runat="server" DataIndex="TOTALPRICE" Text="Total Price" Flex="1" >
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                             </ext:Column>
                      <%--   <ext:NumberColumn ID="NumberColumn1" runat="server" DataIndex="TOTALPRICE" Text="Total Price" Flex="1" Align="Right">
                                <Renderer Fn="Ext.util.Format.CurrencyFactory(2,'.',',','',false)" />
                            </ext:NumberColumn>--%>
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
                                    <ext:Parameter Name="selectedSupp" Value="Ext.encode(#{uxInvoiceGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                                </ExtraParams>
                                </Click>
                        </DirectEvents>
                              
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

    </div>
    </form>
</body>
</html>

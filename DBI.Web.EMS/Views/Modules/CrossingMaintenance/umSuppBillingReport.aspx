<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSuppBillingReport.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umSuppBillingReport" %>

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
     <script type="text/javascript">

         var GetAdditionalData = function (data, rowIndex, record, orig) {
             var headerCt = this.view.headerCt,
             colspan = headerCt.getColumnCount();
             return {
                 rowBody: data.SPECIAL_INSTRUCTIONS,
                 rowBodyCls: this.rowBodyCls,
                 rowBodyColspan: colspan,


             };
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
         <ext:Hidden ID="Hidden1" runat="server" Hidden="true" />
         <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
                <Items>
         <ext:FormPanel ID="uxFilterForm" runat="server" Margin="5" Region="North" Title="Filter Supplemental Billing Report">
                <Items>
                    <ext:FieldSet ID="FieldSet1" runat="server" Title="Filter">
                        <Items>
                            <ext:TextField ID="uxRRCI" runat="server" FieldLabel="Railroad" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" Hidden="true" />

                           <ext:DateField ID="uxStartDate" runat="server" AnchorHorizontal="25%" FieldLabel="Start Date" LabelAlign="Right" Editable="false" TabIndex="2" EmptyText="ALL" >
                                <Plugins>
                                <ext:ClearButton ID="ClearButton1" runat="server" />
                            </Plugins>
                               </ext:DateField>
                            <ext:DateField ID="uxEndDate" runat="server" AnchorHorizontal="25%" FieldLabel="End Date" LabelAlign="Right"  Editable="false" TabIndex="3" EmptyText="ALL" >
                                 <Plugins>
                                <ext:ClearButton ID="ClearButton2" runat="server" />
                            </Plugins>
                                </ext:DateField>

                            <ext:ComboBox ID="uxAddServiceUnit"
                                runat="server" FieldLabel="Service Unit"
                                LabelAlign="Right"
                                AnchorHorizontal="25%"
                                DisplayField="service_unit"
                                ValueField="service_unit"
                                QueryMode="Local" TypeAhead="true" TabIndex="3" ForceSelection="true" EmptyText="ALL">
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
                                <ext:ClearButton ID="ClearButton3" runat="server" />
                            </Plugins>
                            </ext:ComboBox>
                            <ext:ComboBox ID="uxAddSubDiv"
                                runat="server"
                                FieldLabel="Sub-Division"
                                LabelAlign="Right"
                                AnchorHorizontal="25%"
                                DisplayField="sub_division"
                                ValueField="sub_division"
                                TypeAhead="true" TabIndex="5" ForceSelection="true" EmptyText="ALL">
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
                            <ext:ComboBox runat="server"
                                ID="uxAddStateComboBox"
                                FieldLabel="State"
                                LabelAlign="Right"
                                AnchorHorizontal="25%"
                                DisplayField="name"
                                ValueField="name"
                                QueryMode="Local"
                                TypeAhead="true"
                                ForceSelection="true" TabIndex="4" EmptyText="ALL">
                                <Store>
                                    <ext:Store ID="uxAddStateList" runat="server" AutoDataBind="true" >
                                        <Model>
                                            <ext:Model ID="Model10" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="abbr" />
                                                    <ext:ModelField Name="name" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                        <Reader>
                                            <ext:ArrayReader />
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                 <Plugins>
                                <ext:ClearButton ID="ClearButton5" runat="server" />
                            </Plugins>
                            </ext:ComboBox>
                         
                        </Items>
                    </ext:FieldSet>
                </Items>
                <BottomBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Button runat="server"
                                ID="Button4"
                                Text="Run"
                                Icon="PlayGreen">
                                   <DirectEvents>
                                <Click OnEvent="deSupplementalReportGrid" >
                                    </Click>
                            </DirectEvents>
                        
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
            </ext:FormPanel>
            <ext:Panel runat="server" ID="uxCenterPanel" Region="Center">
              <LayoutConfig>
                <ext:FitLayoutConfig />
              </LayoutConfig>
                    <Items>
                        <ext:Panel ID="Panel" runat="server" ManageHeight="true">
                        </ext:Panel>
                    </Items>
                </ext:Panel>
       
       
                    </Items>
             </ext:Viewport>
    </div>
    </form>
</body>
</html>

﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAppDate.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umAppDate" %>

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
                rowBody: data.REMARKS,
                rowBodyCls: this.rowBodyCls,
                rowBodyColspan: colspan,


            };
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <ext:ResourceManager ID="ResourceManager1" runat="server" />
            <ext:FormPanel ID="uxFilterForm" runat="server" Margin="5" Title="Filter Application Date">
                <Items>
                    <ext:FieldSet ID="FieldSet1" runat="server" Title="Filter">
                        <Items>
                            <ext:TextField ID="uxRRCI" runat="server" FieldLabel="Railroad" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" Hidden="true" />


                            <ext:ComboBox ID="uxAddAppReqeusted"
                                runat="server"
                                FieldLabel="Application #"
                                LabelAlign="Right"
                                AnchorHorizontal="25%"
                                DisplayField="type"
                                ValueField="type"
                                QueryMode="Local"
                                TypeAhead="true" AllowBlank="false" ForceSelection="true" TabIndex="1">
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
                            <ext:DateField ID="uxStartDate" runat="server" AnchorHorizontal="25%" FieldLabel="Start Date" LabelAlign="Right" Editable="false" TabIndex="2" EmptyText="ALL" />
                            <ext:DateField ID="uxEndDate" runat="server" AnchorHorizontal="25%" FieldLabel="End Date" LabelAlign="Right"  Editable="false" TabIndex="2" EmptyText="ALL" />
                            <ext:ComboBox ID="uxAddServiceUnit"
                                runat="server" FieldLabel="Service Unit"
                                LabelAlign="Right"
                                AnchorHorizontal="25%"
                                DisplayField="service_unit"
                                ValueField="service_unit"
                                QueryMode="Local" TypeAhead="true" TabIndex="3" AllowBlank="false" ForceSelection="true" EmptyText="ALL">
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
                            <ext:ComboBox ID="uxAddSubDiv"
                                runat="server"
                                FieldLabel="Sub-Division"
                                LabelAlign="Right"                                
                                AnchorHorizontal="25%"
                                DisplayField="sub_division"
                                ValueField="sub_division"
                                TypeAhead="true" TabIndex="5" AllowBlank="false" ForceSelection="true" EmptyText="ALL">
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
                            <ext:ComboBox runat="server"
                                ID="uxAddStateComboBox"
                                FieldLabel="State"
                                LabelAlign="Right"
                                AnchorHorizontal="25%"
                                DisplayField="name"
                                ValueField="name"
                                QueryMode="Local"
                                TypeAhead="true"
                                AllowBlank="false"
                                ForceSelection="true" TabIndex="4" EmptyText="ALL">
                                <Store>
                                    <ext:Store ID="uxAddStateList" runat="server" AutoDataBind="true">
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
                            <Listeners>
                                <Click Handler="#{uxAppDateStore}.load()" />
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
            </ext:FormPanel>

            <ext:GridPanel
                ID="GridPanel1"
                runat="server"
                Title="Application Date Report"
                Icon="Report"
                Frame="false"
                Resizable="false"
                Collapsible="false">
                <Store>
                    <ext:Store ID="uxAppDateStore"
                        runat="server"
                        GroupField="SUB_DIVISION" AutoDataBind="true" OnReadData="deAppDateGrid" AutoLoad="false">
                        <Model>
                            <ext:Model ID="Model1" runat="server">
                                <Fields>
                                    <ext:ModelField Name="CROSSING_ID" />
                                    <ext:ModelField Name="CROSSING_NUMBER" Type="String" />
                                    <ext:ModelField Name="APPLICATION_DATE" Type="Date" />
                                    <ext:ModelField Name="MILE_POST" />
                                    <ext:ModelField Name="DOT" />
                                    <ext:ModelField Name="STATE" />
                                    <ext:ModelField Name="TRUCK_NUMBER" />
                                    <ext:ModelField Name="CITY" />
                                    <ext:ModelField Name="STREET" />
                                    <ext:ModelField Name="SUB_DIVISION" />
                                    <ext:ModelField Name="REMARKS" />


                                </Fields>
                            </ext:Model>
                        </Model>
                        <Proxy>
                            <ext:PageProxy />
                        </Proxy>

                        <Sorters>
                            <ext:DataSorter Property="SUB_DIVISION" />

                        </Sorters>

                    </ext:Store>
                </Store>

                <ColumnModel ID="ColumnModel1" runat="server">
                    <Columns>
                        <ext:Column ID="uxSubDiv" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />
                        <ext:Column ID="uxMainCrossingNum" runat="server" DataIndex="CROSSING_NUMBER" Text="Crossing #" Flex="1" />
                        <ext:Column ID="Column2" runat="server" Text="State" Flex="1" DataIndex="STATE" />
                        <ext:Column ID="Column1" runat="server" Text="MP" Flex="1" DataIndex="MILE_POST" />
                        <ext:Column ID="Column3" runat="server" Text="DOT" Flex="1" DataIndex="DOT" />
                        <ext:DateColumn runat="server" Text="Date" DataIndex="APPLICATION_DATE" Flex="1" Format="MM/dd/yyyy" />
                        <ext:Column ID="Column4" runat="server" DataIndex="TRUCK_NUMBER" Text="Truck #" Flex="1" />
                        <%--<ext:Column ID="Column8" runat="server" DataIndex="REMARKS" Text="Remarks" Flex="3" />--%>
                    </Columns>
                </ColumnModel>
                <Features>
                    <ext:RowBody ID="RowBody1" runat="server" RowBodyCls="rowBodyCls">

                        <GetAdditionalData Fn="GetAdditionalData" />
                    </ext:RowBody>
                </Features>
                <Features>
                    <ext:Grouping ID="Grouping1"
                        runat="server"
                        HideGroupedHeader="true"
                        Collapsible="false"
                        Cls="x-grid-group-title; x-grid-group-hd" />
                </Features>
                <TopBar>
                    <ext:Toolbar ID="Toolbar2" runat="server">
                        <Items>
                           
                            <ext:Button ID="Button2"
                                runat="server"
                                Text="Print"
                                Icon="Printer"
                                OnClientClick="window.print();" />

                            <ext:Button runat="server"
                                ID="uxExportToPDF"
                                Text="Export to PDF"
                                Icon="PageWhiteAcrobat">
                                <%--<DirectEvents>
                        <Click OnEvent="deExportToPDF" IsUpload="true">
                            <ExtraParams>
                                <ext:Parameter Name="CrossingId" Value="#{GridPanel1}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                            </ExtraParams>
                        </Click>
                    </DirectEvents>--%>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="uxEmailPdf"
                                Text="Email Copy"
                                Icon="EmailAttach"
                                Disabled="false">
                                <%--<DirectEvents>
										<Click OnEvent="deSendPDF" IsUpload="true">
											<ExtraParams>
												<ext:Parameter Name="CrossingId" Value="#{GridPanel1}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
											</ExtraParams>
										</Click>
									</DirectEvents>--%>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
                </BottomBar>
            </ext:GridPanel>
        </div>
    </form>
</body>
</html>

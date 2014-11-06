<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umIncident.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umIncident" %>

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
       
            <ext:GridPanel ID="uxCrossingIncidentGrid" Title="CROSSING INFORMATION" runat="server" Region="North" Layout="HBoxLayout" Collapsible="true">
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="false" Mode="Single" />
                </SelectionModel>
                <Store>
                    <ext:Store runat="server"
                        ID="uxCurrentCrossingStore"
                        OnReadData="deCrossingGridData"
                        PageSize="10"
                        AutoDataBind="true" WarningOnDirty="false">
                        <Model>
                            <ext:Model ID="Model2" runat="server">
                                <Fields>
                                    <ext:ModelField Name="CROSSING_ID" />
                                    <ext:ModelField Name="CONTACT_ID" />
                                    <ext:ModelField Name="CROSSING_NUMBER" Type="String" />
                                    <ext:ModelField Name="PROJECT_ID" />
                                    <ext:ModelField Name="LONG_NAME" />
                                    <ext:ModelField Name="SERVICE_UNIT" />
                                    <ext:ModelField Name="SUB_DIVISION" />
                                    <ext:ModelField Name="CONTACT_NAME" />
                                    <ext:ModelField Name="STATE" />
                                    <ext:ModelField Name="STATUS" />
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

                        <ext:Column ID="uxMainCrossingNum" runat="server" DataIndex="CROSSING_NUMBER" Text="Crossing #" Flex="1" />
                        <ext:Column ID="Column12" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" />
                        <ext:Column ID="uxSubDiv" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />
                        <ext:Column ID="uxMTM" runat="server" DataIndex="STATE" Text="State" Flex="1" />
                        <ext:Column ID="Column3" runat="server" DataIndex="STATUS" Text="Status" Flex="1" />

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
                    <Select Handler=" #{uxAddIncident}.enable(); #{uxCloseIncident}.disable()" />
                </Listeners>

            </ext:GridPanel>
            

            <ext:GridPanel ID="uxIncidentGrid" Title="INCIDENT ENTRIES" runat="server" Region="Center" Layout="FitLayout">

                <Store>
                    <ext:Store runat="server"
                        ID="uxIncidentStore" OnReadData="GetIncidentGridData" AutoLoad="true" AutoDataBind="true" PageSize="10" GroupField="CROSSING_NUMBER">
                      <%--  <Parameters>
                            <ext:StoreParameter Name="CrossingId" Value="#{uxCrossingIncidentGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                        </Parameters>--%>
                        <Model>
                            <ext:Model ID="Model1" runat="server" >
                                <Fields>
                                    <ext:ModelField Name="CROSSING_ID" />
                                     <ext:ModelField Name="CROSSING_NUMBER" />
                                    <ext:ModelField Name="INCIDENT_ID" />
                                    <ext:ModelField Name="DATE_REPORTED" Type="Date" />
                                    <ext:ModelField Name="DATE_CLOSED" Type="Date" />
                                    <ext:ModelField Name="INCIDENT_NUMBER" />
                                    <ext:ModelField Name="SLOW_ORDER" />
                                    <ext:ModelField Name="REMARKS" />
                                </Fields>
                            </ext:Model>
                        </Model>
                        <Proxy>
                            <ext:PageProxy />
                        </Proxy>
                         <Sorters>
                        <ext:DataSorter Property="DATE_REPORTED" Direction="ASC" />
                    </Sorters>
                    </ext:Store>
                </Store>
                <ColumnModel>
                    <Columns>
                        <ext:Column ID="Column2" runat="server" DataIndex="CROSSING_NUMBER" Text="Crossing Number" Flex="1" />
                        <ext:Column ID="Column6" runat="server" DataIndex="INCIDENT_NUMBER" Text="Incident Number" Flex="1" />
                        <ext:DateColumn ID="DateColumn1" runat="server" DataIndex="DATE_REPORTED" Text="Date Reported" Flex="1" Format="MM/dd/yyyy" />
                        <ext:DateColumn ID="DateColumn2" runat="server" DataIndex="DATE_CLOSED" Text="Date Closed" Flex="1" Format="MM/dd/yyyy" />
                        <ext:Column ID="Column1" runat="server" DataIndex="SLOW_ORDER" Text="Slow Order" Flex="1" />
                        <ext:Column ID="Column5" runat="server" DataIndex="REMARKS" Text="Remarks" Flex="3" />

                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" AllowDeselect="true" Mode="Single" />
                </SelectionModel>
               
                <TopBar>
                <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:Button ID="uxAddIncident" runat="server" Text="Add Incident" Icon="ApplicationAdd" Disabled="true">
                        <Listeners>
                            <Click Handler="#{uxIncidentWindow}.show()" />
                        </Listeners>
                        <DirectEvents>
                            <Click OnEvent="deAddSetFocus" />
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="uxCloseIncident" runat="server" Text="Close Incident" Icon="BinClosed" Disabled="true">
                        <Listeners>
                            <Click Handler="#{uxCloseIncidentWindow}.show()" />
                        </Listeners>
                         <DirectEvents>
                    <Click OnEvent="deCloseIncident">
                        <ExtraParams>
                            <ext:Parameter Name="IncidentInfo" Value="Ext.encode(#{uxIncidentGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                        </ExtraParams>
                    </Click>
                </DirectEvents>
                        <DirectEvents>
                            <Click OnEvent="deCloseSetFocus" />
                            
                        </DirectEvents>
                    </ext:Button>
                    <ext:Checkbox runat="server" ID="uxToggleClosed" BoxLabel="Include Closed Incidents" BoxLabelAlign="After">
                        <Listeners>
                            <Change Handler="#{uxIncidentStore}.reload()" />
                        </Listeners>
                    </ext:Checkbox>
               </Items>
            </ext:Toolbar>
                    </TopBar>
                <Listeners>
                    <Select Handler="#{uxCloseIncident}.enable()" />
                    <Deselect Handler="#{uxCloseIncident}.disable()" />
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

            <ext:Window runat="server"
                ID="uxIncidentWindow"
                Layout="FormLayout"
                Hidden="true"
                Title="Add Incident"
                Width="550"
                Closable="false" Modal="true">
                <Items>
                    <ext:FormPanel runat="server" ID="uxIncidentFormPanel" Layout="FormLayout">
                        <Items>
                            <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxIncidentNumber" runat="server" FieldLabel="Incident #" AnchorHorizontal="100%" LabelAlign="Right" AllowBlank="false"  InvalidCls="allowBlank" IndicatorIcon="BulletRed"  MsgTarget="Side" TabIndex="1" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer ID="FieldContainer37" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:DateField ID="uxIncidentDateReported" runat="server" FieldLabel="Date Reported" AnchorHorizontal="100%" LabelAlign="Right" AllowBlank="false"  InvalidCls="allowBlank" IndicatorIcon="BulletRed"  MsgTarget="Side" TabIndex="2" />
                                     <ext:Label ID="Label2" runat="server" Text="" Width="25" />
                                    <ext:Checkbox ID="uxIncidentSlowOrder" runat="server" BoxLabel="Slow Order" BoxLabelAlign="After" AnchorHorizontal="100%" TabIndex="3" />

                                    <%-- <ext:DateField ID="uxIncidentDateClosed" runat="server" FieldLabel="Date Closed" AnchorHorizontal="100%" LabelAlign="Right"  />--%>
                                </Items>
                            </ext:FieldContainer>

                            <ext:TextArea ID="uxIncidentRemarks" runat="server" FieldLabel="Remarks" AnchorHorizontal="100%" LabelAlign="Right" TabIndex="4" />

                        </Items>
                        <Buttons>
                            <ext:Button runat="server" ID="uxAddIncidentButton" Text="Add" Icon="Add" Disabled="true" TabIndex="5">
                                <DirectEvents>
                                    <Click OnEvent="deAddIncident">
                                        <ExtraParams>
                                            <ext:Parameter Name="CrossingId" Value="#{uxCrossingIncidentGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button runat="server" ID="Button3" Text="Cancel" Icon="Delete" TabIndex="6">
                                <Listeners>
                                    <Click Handler="#{uxIncidentFormPanel}.reset();
									#{uxIncidentWindow}.hide()" />
                                </Listeners>
                            </ext:Button>
                        </Buttons>
                        <Listeners>
                            <ValidityChange Handler="#{uxAddIncidentButton}.setDisabled(!valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
            </ext:Window>
            <ext:Window runat="server"
                ID="uxCloseIncidentWindow"
                Layout="FormLayout"
                Hidden="true"
                Title="Close Incident"
                Width="550"
                Closable="false" Modal="true">
                <Items>
                    <ext:FormPanel runat="server" ID="FormPanel1" Layout="FormLayout">
                        <Items>
                            <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:TextField ID="uxCloseIncidentNum" runat="server" FieldLabel="Incident #" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                    <ext:Checkbox ID="uxCloseSlowOrder" runat="server" FieldLabel="Slow Order" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />

                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer ID="FieldContainer3" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:DateField ID="uxCloseDateReported" runat="server" FieldLabel="Date Reported" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                    <ext:DateField ID="uxCloseIncidentDateClosed" runat="server" FieldLabel="Date Closed" AnchorHorizontal="100%" LabelAlign="Right" AllowBlank="false" TabIndex="1" AutoFocus="true" />

                                </Items>
                            </ext:FieldContainer>

                            <ext:TextArea ID="uxCloseRemarks" runat="server" FieldLabel="Remarks" AnchorHorizontal="100%" LabelAlign="Right" />

                        </Items>
                        <Buttons>
                            <ext:Button runat="server" ID="uxCloseIncidentButton" Text="Close" Icon="BinClosed" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="deCloseIncidentForm">
                                        <ExtraParams>
                                            <ext:Parameter Name="IncidentInfo" Value="Ext.encode(#{uxIncidentGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button runat="server" ID="Button2" Text="Cancel" Icon="Delete">
                                <Listeners>
                                    <Click Handler="#{FormPanel1}.reset();
									#{uxCloseIncidentWindow}.hide()" />
                                </Listeners>
                            </ext:Button>
                        </Buttons>
                        <Listeners>
                            <ValidityChange Handler="#{uxCloseIncidentButton}.setDisabled(!valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
            </ext:Window>
      
                    </Items>
              </ext:Viewport>
    
    </form>
</body>
</html>

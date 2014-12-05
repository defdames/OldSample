<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAppDate.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umAppDate" %>

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
                    <ext:FormPanel ID="uxFilterForm" runat="server" Margin="5" Region="North" Title="Filter Application Date" Collapsible="true">
                        <Items>
                            <ext:FieldSet ID="FieldSet1" runat="server" Title="Filter">
                                <Items>
                                    <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                        <Items>
                                            <ext:TextField ID="uxRRCI" runat="server" FieldLabel="Railroad" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" Hidden="true" />
                                            <ext:DropDownField ID="uxDotDropDownField" runat="server" FieldLabel="DOT #" AnchorHorizontal="40%" LabelAlign="Right" Width="505" EmptyText="ALL" Mode="ValueText" Editable="false">
                                                <Component>
                                                    <ext:GridPanel runat="server"
                                                        ID="uxAddProject"
                                                        Layout="HBoxLayout">
                                                        <Store>
                                                            <ext:Store runat="server"
                                                                ID="uxDOTStore"
                                                                PageSize="10"
                                                                RemoteSort="true"
                                                                OnReadData="deLoadDOT">

                                                                <Model>
                                                                    <ext:Model ID="Model6" runat="server">
                                                                        <Fields>
                                                                            <ext:ModelField Name="CROSSING_ID" />
                                                                            <ext:ModelField Name="CROSSING_NUMBER" />
                                                                            <ext:ModelField Name="SUB_DIVISION" />
                                                                            <ext:ModelField Name="STATE" />


                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                                <Proxy>
                                                                    <ext:PageProxy />
                                                                </Proxy>
                                                                <Sorters>
                                                                    <ext:DataSorter Property="CROSSING_NUMBER" Direction="ASC" />
                                                                </Sorters>
                                                            </ext:Store>
                                                        </Store>
                                                        <ColumnModel>
                                                            <Columns>
                                                                <ext:Column ID="Column11" runat="server" DataIndex="CROSSING_NUMBER" Text="DOT" Flex="1" />
                                                                <ext:Column ID="Column6" runat="server" DataIndex="SUB_DIVISION" Text="Sub Division" Flex="2" />
                                                                <ext:Column ID="Column14" runat="server" DataIndex="STATE" Text="State" Flex="1" />
                                                            </Columns>
                                                        </ColumnModel>
                                                        <BottomBar>
                                                            <ext:PagingToolbar ID="PagingToolbar3" runat="server" />
                                                        </BottomBar>
                                                        <SelectionModel>
                                                            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" Mode="Single" />
                                                        </SelectionModel>

                                                        <DirectEvents>
                                                            <SelectionChange OnEvent="deDOTValue">
                                                                <ExtraParams>
                                                                    <ext:Parameter Name="CrossingId" Value="#{uxAddProject}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                                                    <ext:Parameter Name="CrossingNumber" Value="#{uxAddProject}.getSelectionModel().getSelection()[0].data.CROSSING_NUMBER" Mode="Raw" />
                                                                    <ext:Parameter Name="Type" Value="Add" />
                                                                </ExtraParams>
                                                            </SelectionChange>

                                                        </DirectEvents>
                                                        <%--<DirectEvents>
                                                                 <Select OnEvent="deSelectVersion" />
                                                            </DirectEvents>--%>
                                                        <Plugins>
                                                            <ext:FilterHeader runat="server" ID="uxAddProjectFilter" Remote="true" />
                                                        </Plugins>
                                                    </ext:GridPanel>
                                                </Component>
                                            </ext:DropDownField>
                                            <%--   <ext:ComboBox ID="ComboBox1" FieldLabel="DOT #" LabelAlign="Right" 
                runat="server" 
                DisplayField="CROSSING_NUMBER" 
                ValueField="CROSSING_NUMBER"
                TypeAhead="false"
                AnchorHorizontal="25%"
                PageSize="10"
                HideBaseTrigger="true"
                MinChars="0"
                TriggerAction="Query" >
              
                <Store>
                    <ext:Store ID="uxDOTStore" OnReadData="deLoadDOT" runat="server" AutoLoad="false">
                 
                        <Model>
                            <ext:Model ID="Model1" runat="server">
                                <Fields>
                                    <ext:ModelField Name="CROSSING_NUMBER" />
                                 
                                </Fields>
                            </ext:Model>                            
                        </Model>
                         <Proxy>                           
                         <ext:AjaxProxy >
                             <Reader>
                                    <ext:JsonReader Root="data" />
                                </Reader>
                             </ext:AjaxProxy>
                        </Proxy>
                       
                    </ext:Store>
                </Store>
                  
            </ext:ComboBox>    --%>
                                        </Items>
                                    </ext:FieldContainer>
                                    <ext:FieldContainer ID="FieldContainer39" runat="server" Layout="HBoxLayout">
                                        <Items>
                                            <ext:ComboBox ID="uxAddAppReqeusted"
                                                runat="server"
                                                FieldLabel="Application #"
                                                LabelAlign="Right"
                                                AnchorHorizontal="25%"
                                                DisplayField="type"
                                                ValueField="type"
                                                QueryMode="Local"
                                                TypeAhead="true" ForceSelection="true" EmptyText="ALL" TabIndex="1">
                                                <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxAddAppRequestedStore" AutoDataBind="true" AutoLoad="true">
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
                                                <Plugins>
                                                    <ext:ClearButton ID="ClearButton1" runat="server" />
                                                </Plugins>
                                            </ext:ComboBox>
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
                                                    <ext:ClearButton ID="ClearButton4" runat="server" />
                                                </Plugins>
                                            </ext:ComboBox>


                                        </Items>
                                    </ext:FieldContainer>

                                    <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                                        <Items>
                                            <ext:DateField ID="uxStartDate" runat="server" AnchorHorizontal="25%" FieldLabel="Start Date" LabelAlign="Right" Editable="false" TabIndex="2" EmptyText="ALL">
                                                <Plugins>
                                                    <ext:ClearButton ID="ClearButton2" runat="server" />
                                                </Plugins>
                                            </ext:DateField>
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
                                                    <ext:ClearButton ID="ClearButton5" runat="server" />
                                                </Plugins>
                                            </ext:ComboBox>
                                        </Items>
                                    </ext:FieldContainer>
                                    <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                                        <Items>
                                            <ext:DateField ID="uxEndDate" runat="server" AnchorHorizontal="25%" FieldLabel="End Date" LabelAlign="Right" Editable="false" TabIndex="2" EmptyText="ALL">
                                                <Plugins>
                                                    <ext:ClearButton ID="ClearButton3" runat="server" />
                                                </Plugins>
                                            </ext:DateField>
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
                                                <Plugins>
                                                    <ext:ClearButton ID="ClearButton6" runat="server" />
                                                </Plugins>

                                            </ext:ComboBox>
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
                                        Text="Run"
                                        Icon="PlayGreen" Disabled="false">
                                        <DirectEvents>
                                            <Click OnEvent="deAppDateGrid" />
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
                        <Listeners>
                            <ValidityChange Handler="#{Button4}.setDisabled(!valid);" />
                        </Listeners>
                    </ext:FormPanel>
                    <ext:Panel runat="server" ID="uxCenterPanel" Region="Center">
                        <LayoutConfig>
                            <ext:FitLayoutConfig />
                        </LayoutConfig>
                        <Items>
                            <ext:Panel ID="Tab" runat="server" ManageHeight="true">
                            </ext:Panel>
                        </Items>
                    </ext:Panel>

                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>

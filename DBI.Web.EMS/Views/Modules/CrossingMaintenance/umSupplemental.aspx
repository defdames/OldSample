<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSupplemental.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umSupplemental" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <div></div>
        <%-- <Service Units Tab>--%>
        <ext:GridPanel ID="uxSupplementalCrossingGrid" Title="CROSSING LIST FOR SUPPLEMENTAL ENTRY" runat="server" Layout="HBoxLayout" Collapsible="true">
            <SelectionModel>
                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="false" Mode="Single" />
            </SelectionModel>
            <Store>
                <ext:Store runat="server"
                    ID="uxSupplementalCrossingStore"
                    OnReadData="deSupplementalGridData"
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

                    <ext:Column ID="uxMainCrossing" runat="server" DataIndex="CROSSING_NUMBER" Text="Crossing #" Flex="1" />
                    <ext:Column ID="Column8" runat="server" DataIndex="LONG_NAME" Text="Project Name" Flex="2" />
                    <ext:Column ID="Column12" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" />
                    <ext:Column ID="uxSubDiv" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />
                    <ext:Column ID="uxMTM" runat="server" DataIndex="CONTACT_NAME" Text="Manager" Flex="1" />

                </Columns>
            </ColumnModel>
            <Plugins>
                <ext:FilterHeader ID="FilterHeader1" runat="server" />
            </Plugins>
            <DirectEvents>
                <Select OnEvent="GetSupplementalGridData">
                    <ExtraParams>
                        <ext:Parameter Name="CrossingId" Value="#{uxSupplementalCrossingGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                    </ExtraParams>
                </Select>
            </DirectEvents>


            <BottomBar>
                <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True">
                </ext:PagingToolbar>
            </BottomBar>
            <Listeners>
                <Select Handler="#{uxAddSuppButton}.enable();" />
                <Deselect Handler="#{uxAddSuppButton}.disable();" />
            </Listeners>

        </ext:GridPanel>
        <ext:Toolbar ID="Toolbar1" runat="server">
            <Items>
                <ext:Button ID="uxAddSuppButton" runat="server" Text="Add Supplemental" Icon="ApplicationAdd" Disabled="true">
                    <Listeners>
                        <Click Handler="#{uxAddNewSupplementalWindow}.show()" />
                    </Listeners>
                </ext:Button>
               
                <ext:Button ID="uxRemoveSuppButton" runat="server" Text="Delete Supplemental" Icon="ApplicationDelete" Disabled="true">
                    <DirectEvents>
                        <Click OnEvent="deRemoveSupplemental">
                            <Confirmation ConfirmRequest="true" Title="Remove?" Message="Are you sure you want to delete this supplemental entry?" />

                            <ExtraParams>
                                <ext:Parameter Name="SupplementalInfo" Value="Ext.encode(#{uxSupplementalGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                            </ExtraParams>
                        </Click>
                    </DirectEvents>

                </ext:Button>
            </Items>
        </ext:Toolbar>

        <ext:GridPanel ID="uxSupplementalGrid" Title="SUPPLEMENTAL ENTRIES" runat="server" Layout="HBoxLayout">

            <Store>
                <ext:Store runat="server"
                    ID="uxSupplementalStore">
                    <Model>
                        <ext:Model ID="Model1" runat="server">
                            <Fields>
                                <%--        <ext:ModelField Name="COMPLETED_DATE" Type="Date" />--%>

                                <ext:ModelField Name="CROSSING_ID" />
                                <ext:ModelField Name="SUPPLEMENTAL_ID" />
                                <ext:ModelField Name="APPROVED_DATE" Type="Date" />
                                <ext:ModelField Name="SERVICE_TYPE" />
                                <ext:ModelField Name="TRUCK_NUMBER" />
                                <ext:ModelField Name="SQUARE_FEET" />
                                <ext:ModelField Name="RECURRING" />
                                <ext:ModelField Name="REMARKS" />
                            </Fields>
                        </ext:Model>
                    </Model>
                </ext:Store>
            </Store>
            <ColumnModel>
                <Columns>

                    <ext:DateColumn ID="DateColumn1" runat="server" DataIndex="APPROVED_DATE" Text="Approved Date" Flex="1" Format="MM/dd/yyyy" />
                    <ext:Column ID="Column6" runat="server" DataIndex="SERVICE_TYPE" Text="Service Type" Flex="1" />
                    <ext:Column ID="Column3" runat="server" DataIndex="TRUCK_NUMBER" Text="Truck" Flex="1" />
                    <ext:Column ID="Column1" runat="server" DataIndex="SQUARE_FEET" Text="Square Feet" Flex="1" />
                    <ext:Column ID="Column13" runat="server" DataIndex="RECURRING" Text="Recurring" Flex="1" />
                    <ext:Column ID="Column5" runat="server" DataIndex="REMARKS" Text="Remarks" Flex="3" />
                    <%--    <ext:DateColumn ID="DateColumn2" runat="server" DataIndex="COMPLETED_DATE" Text="Completed Date" Flex="1" Format="MM/dd/yyyy" />--%>
                </Columns>
            </ColumnModel>
            <SelectionModel>
                <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" AllowDeselect="true" Mode="Single" />
            </SelectionModel>
            
            <Listeners>
                <Select Handler="#{uxRemoveSuppButton}.enable();" />
                <Deselect Handler="#{uxRemoveSuppButton}.disable();" />
            </Listeners>
        </ext:GridPanel>


        <%-------------------------------------------------Hidden Windows------------------------------------------------------%>

        <ext:Window runat="server"
            ID="uxAddNewSupplementalWindow"
            Layout="FormLayout"
            Hidden="true"
            Title="Add New Supplemental"
            Width="800">
            <Items>
                <ext:FormPanel ID="uxAddSupplementalForm" runat="server" Layout="FormLayout">
                    <Items>
                        <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                            <Items>
                                <ext:DateField ID="uxAddApprovedDateField" runat="server" FieldLabel="Approved Date" AnchorHorizontal="100%" LabelAlign="Right" AllowBlank="false"/>

                                <ext:ComboBox ID="uxAddServiceType"
                                    runat="server"
                                    FieldLabel="Service Type"
                                    LabelAlign="Right"
                                    DisplayField="type"
                                    ValueField="type"
                                    QueryMode="Local"
                                    TypeAhead="true" TabIndex="2" Width="300" AllowBlank="false">
                                    <Store>
                                        <ext:Store runat="server"
                                            ID="uxAddServiceTypeStore" AutoDataBind="true">
                                            <Model>
                                                <ext:Model ID="Model4" runat="server">
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

                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                            <Items>

                                <ext:TextField runat="server" ID="uxAddSquareFeet" FieldLabel="Square Feet" LabelAlign="Right" AnchorHorizontal="100%" AllowBlank="false" />

                                <ext:ComboBox ID="uxAddTruckComboBox"
                                    runat="server"
                                    FieldLabel="Truck #"
                                    LabelAlign="Right"
                                    DisplayField="NAME"
                                    ValueField="PROJECT_ID"
                                    QueryMode="Local"
                                    TypeAhead="true" Width="300" AllowBlank="false">
                                    <Store>
                                        <ext:Store runat="server"
                                            ID="uxAddTruckStore" AutoDataBind="true">
                                            <Model>
                                                <ext:Model ID="Model5" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="NAME" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>

                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>
                                <ext:Checkbox ID="uxAddRecurringBox" runat="server" FieldLabel="Recurring" LabelAlign="Right" AllowBlank="false" />

                            </Items>
                        </ext:FieldContainer>


                        <ext:TextArea ID="uxAddRemarks" runat="server" FieldLabel="Remarks" AnchorHorizontal="92%" LabelAlign="Right" />
                    </Items>
                    <Buttons>
                        <ext:Button ID="uxAddNewSupplementalButton" runat="server" Text="Add" Icon="Add">
                            <DirectEvents>
                                <Click OnEvent="deAddSupplemental">
                                    <ExtraParams>
                                        <ext:Parameter Name="CrossingId" Value="#{uxSupplementalCrossingGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="uxCancelNewSupplementalButton" runat="server" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="#{uxAddSupplementalForm}.reset();
									#{uxAddNewSupplementalWindow}.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <Listeners>
						<ValidityChange Handler="#{uxAddNewSupplementalButton}.setDisabled(!valid);" />
					</Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Window>

        <%---------------------------------------------------------------------------------------------------------%>
      

    </form>
</body>
</html>

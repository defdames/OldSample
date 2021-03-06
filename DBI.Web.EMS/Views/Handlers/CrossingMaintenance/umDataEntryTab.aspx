﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umDataEntryTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umDataEntryTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" />
        <div></div>
        <ext:GridPanel ID="uxApplicationCrossingGrid" Title="CROSSING LIST FOR APPLICATION ENTRY" runat="server" Layout="FitLayout" Collapsible="true" >
           
            <Store>
                <ext:Store runat="server"
                    ID="uxAppEntryCrossingStore"
                    OnReadData="deApplicationGridData"
                    AutoDataBind="true" WarningOnDirty="false" >
                    <Model>
                        <ext:Model ID="Model2" runat="server">
                            <Fields>
                                <ext:ModelField Name="CROSSING_ID" />
                                <ext:ModelField Name="CONTACT_ID" />
                                <ext:ModelField Name="CROSSING_NUMBER" Type="String" />
                                <ext:ModelField Name="PROJECT_ID" />
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
                    <ext:Column ID="Column12" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" />
                    <ext:Column ID="uxSubDiv" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />
                    <ext:Column ID="uxMTM" runat="server" DataIndex="CONTACT_NAME" Text="Manager" Flex="1" />

                </Columns>
            </ColumnModel>
            <Plugins>
                <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
            </Plugins>
           
            <SelectionModel>
                <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" Mode="Multi" />
            </SelectionModel>
            <Listeners>
                <Select Handler="#{uxAddAppButton}.enable();" />
               
            </Listeners>

           <%--  <DirectEvents>
                    <Select OnEvent="GetApplicationGridData">
                        <ExtraParams>
                            <ext:Parameter Name="CrossingId" Value="#{uxApplicationCrossingGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                        </ExtraParams>
                    </Select>
                </DirectEvents>--%>
            <BottomBar>
                <ext:Toolbar ID="Toolbar1" runat="server" HideRefresh="True" >
                     <Items>
                            <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                     </Items>
                </ext:Toolbar>
            </BottomBar>
            <TopBar>
             <ext:Toolbar ID="Toolbar3" runat="server">
            <Items>
                <ext:Button ID="uxAddAppButton" runat="server" Text="Add Entry" Icon="ApplicationAdd" Disabled="true">
                    <Listeners>
                        <Click Handler="#{uxAddNewApplicationEntryWindow}.show()" />
                    </Listeners>
                </ext:Button>
               
                </Items>
                </ext:Toolbar>
           </TopBar>
        </ext:GridPanel>
        
       
      <%--  <ext:GridPanel ID="uxApplicationEntryGrid" Title="APPLICATION ENTRIES" runat="server" Region="North" Layout="HBoxLayout" Collapsible="true" Hidden="true">
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" AllowDeselect="true" Mode="Single" />
                </SelectionModel>
                <Store>
                    <ext:Store runat="server"
                        ID="uxApplicationStore">
                        <Model>
                            <ext:Model ID="Model1" runat="server">
                                <Fields>
                                    <ext:ModelField Name="CROSSING_ID" />
                                  
                                   
                                    <ext:ModelField Name="APPLICATION_ID" />
                                    <ext:ModelField Name="APPLICATION_NUMBER" />
                                    <ext:ModelField Name="APPLICATION_DATE" Type="Date" />
                                    <ext:ModelField Name="APPLICATION_REQUESTED" />
                                    <ext:ModelField Name="TRUCK_NUMBER" />
                                    <ext:ModelField Name="SPRAY" />
                                    <ext:ModelField Name="CUT" />
                                    <ext:ModelField Name="INSPECT" />
                                    <ext:ModelField Name="REMARKS" />

                                </Fields>
                            </ext:Model>
                        </Model>
                       
                    </ext:Store>
                </Store>
                <ColumnModel>
                    <Columns>

                        <ext:Column ID="Column1" runat="server" DataIndex="APPLICATION_NUMBER" Text="Application #" Flex="1" />
                        <ext:DateColumn ID="DateColumn2" runat="server" DataIndex="APPLICATION_DATE" Text="Date" Flex="1" Format="MM/dd/yyyy" />
                        <ext:Column ID="Column3" runat="server" DataIndex="TRUCK_NUMBER" Text="Truck #" Flex="1" />
                         <ext:Column ID="Column2" runat="server" DataIndex="APPLICATION_REQUESTED" Text="App Requested" Flex="1" />
                        <ext:Column ID="Column7" runat="server" DataIndex="SPRAY" Text="Spray" Flex="1" />
                        <ext:Column ID="Column9" runat="server" DataIndex="CUT" Text="Cut" Flex="1" />
                        <ext:Column ID="Column10" runat="server" DataIndex="INSPECT" Text="Inspect" Flex="1" />
                        <ext:Column ID="Column5" runat="server" DataIndex="REMARKS" Text="Remarks" Flex="3" />

                    </Columns>
                </ColumnModel>               

            </ext:GridPanel>--%>


        <%---------------------------------------Hidden Windows-----------------------------------%>

        <ext:Window runat="server"
            ID="uxAddNewApplicationEntryWindow"
            Layout="FormLayout"
            Hidden="true"
            Title="Add Application Entry"
            Width="650" Modal="true">
            <Items>
                <ext:FormPanel ID="uxAddApplicationForm" runat="server" Layout="FormLayout">
                    <Items>


                        <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                            <Items>
                                <ext:ComboBox ID="uxAddAppReqeusted"
                                    runat="server"
                                    FieldLabel="App Requested"
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

                                <ext:Checkbox ID="uxAddEntrySprayBox" runat="server" FieldLabel="Spray" LabelAlign="Right" Width="250" TabIndex="4" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer0" runat="server" Layout="HBoxLayout">
                            <Items>
                                <ext:DateField ID="uxAddEntryDate" runat="server" FieldLabel="Date" LabelAlign="Right" Width="300" AllowBlank="false" Editable="false" TabIndex="2" />

                                <ext:Checkbox ID="uxAddEntryCutBox" runat="server" FieldLabel="Cut" LabelAlign="Right" TabIndex="5" />

                            </Items>
                        </ext:FieldContainer>


                        <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                            <Items>
                                <ext:ComboBox ID="uxAddApplicationTruckComboBox"
                                    runat="server"
                                    FieldLabel="Truck #"
                                    LabelAlign="Right"
                                    DisplayField="NAME"
                                    ValueField="NAME"
                                    QueryMode="Local"
                                    TypeAhead="true" Width="300" AllowBlank="false" ForceSelection="true" TabIndex="3">
                                    <Store>
                                        <ext:Store runat="server"
                                            ID="uxAddApplicationTruckStore" AutoDataBind="true">
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

                                <ext:Checkbox ID="uxAddEntryInspectBox" runat="server" FieldLabel="Inspect" LabelAlign="Right" Width="250" TabIndex="6" />
                            </Items>
                        </ext:FieldContainer>

                       
                        <ext:TextArea ID="uxAddEntryRemarks" FieldLabel="Remarks" runat="server" LabelAlign="Right" TabIndex="7"/>
                    </Items>
                    <Buttons>
                        <ext:Button ID="uxAddApplicationEntryButton" runat="server" Text="Add" Icon="Add">
                            <DirectEvents>
                                <Click OnEvent="deAddApplication">
                                    <ExtraParams>
                                        <ext:Parameter Name="CrossingId" Value="#{uxApplicationCrossingGrid}.getSelectionModel().getSelection()[0].data.CROSSING_ID" Mode="Raw" />
                                        <ext:Parameter Name="selectedCrossings" Value="Ext.encode(#{uxApplicationCrossingGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="uxCancelNewApplicationEntryButton" runat="server" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="#{uxAddApplicationForm}.reset();
									#{uxAddNewApplicationEntryWindow}.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <Listeners>
                        <ValidityChange Handler="#{uxAddApplicationEntryButton}.setDisabled(!valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>


        </ext:Window>
        <%-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------%>

      
    </form>
</body>
</html>

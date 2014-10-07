<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCombinedTab_DBI.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umCombinedTab_DBI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Resources/StyleSheets/main.css" rel="stylesheet" />
    <script type="text/javascript">
        var setIcon = function (value, metadata, record) {
            var tpl = "<img src='{0}' />";
            if (value == "Error") {
                return "<img src='" + App.uxRedWarning.getValue() + "' />";
            }
            else if (value == "Warning") {
                return "<img src='" + App.uxYellowWarning.getValue() + "' />";
            }
            else {
                return "";
            }
        };
        var onShow = function (toolTip, grid) {
            var view = grid.getView(),
				store = grid.getStore(),
				record = view.getRecord(view.findItemByChild(toolTip.triggerElement)),
				column = view.getHeaderByCell(toolTip.triggerElement),
				data = record.get(column.dataIndex);

            toolTip.update(data);
        };

        var SetEmployeeValue = function (value, text) {
            if (!text && !!value) {
                text = EmployeeRenderer(value);
            }

            Ext.net.DropDownField.prototype.setValue.call(this, value, text);
        };

        var SetEmployeeEqValue = function (value, text) {
            if (!text && !!value) {
                text = EmployeeEqRenderer(value);
            }

            Ext.net.DropDownField.prototype.setValue.call(this, value, text);
        };

        var SetEquipmentValue = function (value, text) {
            if (!text && !!value) {
                text = EquipmentRenderer(value);
            }

            Ext.net.DropDownField.prototype.setValue.call(this, value, text);
        };

        var SetTaskValue = function (value, text) {
            if (!text && !!value) {
                text = ProductionTaskRenderer(value);
            }

            Ext.net.DropDownField.prototype.setValue.call(this, value, text);
        };

        var showButtons = function () {
            App.uxSaveFooterButton.show();
            App.uxSaveHeaderButton.show();
            App.uxSaveHeaderButton.enable();
        };

        var disablePostOnError = function () {
            parent.App.uxPostActivityButton.disable();
        };

        var disableOnError = function () {
            parent.App.uxPostActivityButton.disable();
            parent.App.uxApproveActivityButton.disable();
        };

        var EmployeeRenderer = function (value, record) {
            if (!record) {
                return App.uxEmployeeGrid.getSelectionModel().getSelection()[0].data.EMPLOYEE_NAME;
            }
            return record.record.data.EMPLOYEE_NAME;
        };

        var EmployeeEqRenderer = function (value, record) {
            if (!record) {
                return App.uxEmployeeGrid.getSelectionModel().getSelection()[0].data.NAME;
            }
            return record.record.data.NAME;
        };

        var EquipmentRenderer = function (value, record) {
            if (!record) {
                return App.uxEquipmentGrid.getSelectionModel().getSelection()[0].data.NAME;
            }
            return record.record.data.NAME;
        };

        var ProductionTaskRenderer = function (value, record) {
            if (!record) {
                return App.uxProductionGrid.getSelectionModel().getSelection()[0].data.DESCRIPTION;
            }
            return record.record.data.DESCRIPTION;
        };

        var deleteEmployee = function () {
            var EmployeeRecord = App.uxEmployeeGrid.getSelectionModel().getSelection();

            Ext.Msg.confirm('Really Delete?', 'Do you really want to delete this employee?', function (e) {
                if (e == 'yes') {
                    App.uxEmployeeStore.remove(EmployeeRecord);
                    App.direct.dmDeleteEmployee(EmployeeRecord[0].data.EMPLOYEE_ID);
                }
            });
        };

        var deleteEquipment = function () {
            var EquipmentRecord = App.uxEquipmentGrid.getSelectionModel().getSelection();

            Ext.Msg.confirm('Really Delete?', 'Do you really want to delete this equipment entry?', function (e) {
                if (e == 'yes') {
                    App.uxEquipmentStore.remove(EquipmentRecord);
                    App.direct.dmDeleteEquipment(EquipmentRecord[0].data.EQUIPMENT_ID);
                }
            });
        };

        var deleteProduction = function () {
            var ProductionRecord = App.uxProductionGrid.getSelectionModel().getSelection();

            Ext.Msg.confirm('Really Delete?', 'Do you really want to delete this production entry?', function (e) {
                if (e == 'yes') {
                    App.uxProductionStore.remove(ProductionRecord);
                    App.direct.dmDeleteProduction(ProductionRecord[0].data.PRODUCTION_ID);
                }
            });
        };

        var deleteWeather = function () {
            var WeatherRecord = App.uxWeatherGrid.getSelectionModel().getSelection();

            Ext.Msg.confirm('Really Delete?', 'Do you really want to delete this weather entry?', function (e) {
                if (e == 'yes') {
                    App.uxWeatherStore.remove(WeatherRecord);
                    App.direct.dmDeleteWeather(WeatherRecord[0].data.WEATHER_ID);
                }
            });
        };

    </script>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />


    <form id="form1" runat="server">
        <ext:Panel runat="server" ID="uxMainContainer" Layout="AutoLayout">
            <Items>
                <ext:Hidden ID="uxYellowWarning" runat="server" />
                <ext:Hidden ID="uxRedWarning" runat="server" />
                <ext:FormPanel runat="server"
                    ID="uxHeaderPanel" Padding="10" BodyPadding="5" MaxWidth="1200">
                    <Items>
                        <ext:DateField runat="server" ID="uxDateField" FieldLabel="Date" AllowBlank="false" LabelWidth="100" Width="200" />
                        <ext:TextField runat="server" ID="uxHeaderField" FieldLabel="DRS Id" Width="200" LabelWidth="100" ReadOnly="true" />
                        <ext:TextField runat="server" ID="uxOracleField" FieldLabel="Oracle DRS Id" Width="200" ReadOnly="true" />
                        <ext:DropDownField runat="server"
                            ID="uxProjectField"
                            FieldLabel="Project"
                            Mode="ValueText"
                            AllowBlank="false" Editable="false" Width="600" LabelWidth="100">
                            <Component>
                                <ext:GridPanel runat="server"
                                    ID="uxFormProjectGrid"
                                    Layout="HBoxLayout">
                                    <Store>
                                        <ext:Store runat="server"
                                            ID="uxFormProjectStore"
                                            OnReadData="deReadProjectData"
                                            PageSize="10"
                                            RemoteSort="true">
                                            <Model>
                                                <ext:Model runat="server"
                                                    ID="uxFormProjectModel">
                                                    <Fields>
                                                        <ext:ModelField Name="PROJECT_ID" Type="Int" />
                                                        <ext:ModelField Name="ORGANIZATION_NAME" Type="String" />
                                                        <ext:ModelField Name="SEGMENT1" Type="String" />
                                                        <ext:ModelField Name="LONG_NAME" Type="String" />
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
                                            <ext:Column runat="server"
                                                ID="uxFormSegment"
                                                DataIndex="SEGMENT1"
                                                Text="Project #" Flex="15" />
                                            <ext:Column runat="server"
                                                ID="uxFormLong"
                                                DataIndex="LONG_NAME"
                                                Text="Project Name" Flex="50" />
                                            <ext:Column runat="server"
                                                ID="uxFormOrg"
                                                DataIndex="ORGANIZATION_NAME"
                                                Text="Organization Name" Flex="35" />
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="uxFormProjectSelection" runat="server" Mode="Single" />
                                    </SelectionModel>
                                    <DirectEvents>
                                        <SelectionChange OnEvent="deStoreProjectValue">
                                            <ExtraParams>
                                                <ext:Parameter Name="ProjectId" Value="#{uxFormProjectGrid}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
                                                <ext:Parameter Name="LongName" Value="#{uxFormProjectGrid}.getSelectionModel().getSelection()[0].data.LONG_NAME" Mode="Raw" />
                                                <ext:Parameter Name="ProjectNumber" Value="#{uxFormProjectGrid}.getSelectionModel().getSelection()[0].data.SEGMENT1" Mode="Raw" />
                                            </ExtraParams>
                                        </SelectionChange>
                                    </DirectEvents>
                                    <Plugins>
                                        <ext:FilterHeader ID="uxFormProjectFilter" runat="server" Remote="true" />
                                    </Plugins>
                                    <TopBar>
                                        <ext:Toolbar runat="server"
                                            ID="uxFormProjectTop">
                                            <Items>
                                                <ext:Button runat="server"
                                                    ID="uxFormProjectToggleOrg"
                                                    EnableToggle="true"
                                                    Text="All Regions"
                                                    Icon="Group">
                                                    <DirectEvents>
                                                        <Toggle OnEvent="deReloadStore">
                                                            <ExtraParams>
                                                                <ext:Parameter Name="Type" Value="Project" />
                                                            </ExtraParams>
                                                        </Toggle>
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
                                    <BottomBar>
                                        <ext:PagingToolbar ID="uxFormProjectPaging" runat="server" />
                                    </BottomBar>
                                </ext:GridPanel>
                            </Component>
                        </ext:DropDownField>
                        <ext:TextField runat="server" ID="uxSubDivisionField" FieldLabel="Sub-Division" Width="300" LabelWidth="100" />
                        <ext:TextField runat="server" ID="uxContractorField" FieldLabel="Contractor" Width="500" LabelWidth="100" />
                        <ext:DropDownField runat="server"
                            ID="uxSupervisorField"
                            FieldLabel="Supervisor/Area Manager"
                            Mode="ValueText"
                            AllowBlank="false" Editable="false" Width="500" LabelWidth="100">
                            <Component>
                                <ext:GridPanel runat="server"
                                    ID="uxFormEmployeeGrid"
                                    Layout="HBoxLayout">
                                    <Store>
                                        <ext:Store runat="server"
                                            ID="uxFormEmployeeStore"
                                            OnReadData="deLoadEmployees"
                                            PageSize="10"
                                            RemoteSort="true">
                                            <Model>
                                                <ext:Model ID="uxFormEmployeeModel" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="PERSON_ID" Type="Int" />
                                                        <ext:ModelField Name="EMPLOYEE_NAME" Type="String" />
                                                        <ext:ModelField Name="JOB_NAME" Type="String" />
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
                                            <ext:Column runat="server" Text="Person ID" ID="uxFormPersonId" DataIndex="PERSON_ID" Flex="20" />
                                            <ext:Column runat="server" Text="Employee Name" ID="uxFormEmployeeName" DataIndex="EMPLOYEE_NAME" Flex="35" />
                                            <ext:Column runat="server" Text="Job Name" ID="uxFormJobName" DataIndex="JOB_NAME" Flex="35" />
                                        </Columns>
                                    </ColumnModel>
                                    <Plugins>
                                        <ext:FilterHeader runat="server"
                                            ID="uxFormEmployeeFilter"
                                            Remote="true" />
                                    </Plugins>
                                    <DirectEvents>
                                        <SelectionChange OnEvent="deStoreEmployee">
                                            <ExtraParams>
                                                <ext:Parameter Name="EmployeeName" Value="#{uxFormEmployeeGrid}.getSelectionModel().getSelection()[0].data.EMPLOYEE_NAME" Mode="Raw" />
                                                <ext:Parameter Name="PersonID" Value="#{uxFormEmployeeGrid}.getSelectionModel().getSelection()[0].data.PERSON_ID" Mode="Raw" />
                                            </ExtraParams>
                                        </SelectionChange>
                                    </DirectEvents>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="uxFormEmployeeSelection" runat="server" Mode="Single" />
                                    </SelectionModel>
                                    <TopBar>
                                        <ext:Toolbar ID="Toolbar1" runat="server">
                                            <Items>
                                                <ext:Button runat="server"
                                                    ID="uxFormEmployeeToggleOrg"
                                                    EnableToggle="true"
                                                    Text="All Regions"
                                                    Icon="Group">
                                                    <DirectEvents>
                                                        <Toggle OnEvent="deReloadStore">
                                                            <ExtraParams>
                                                                <ext:Parameter Name="Type" Value="Employee" />
                                                            </ExtraParams>
                                                        </Toggle>
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
                                    <BottomBar>
                                        <ext:PagingToolbar ID="uxFormEmployeePaging" runat="server" />
                                    </BottomBar>
                                </ext:GridPanel>
                            </Component>
                        </ext:DropDownField>
                        <ext:TextField runat="server" ID="uxLicenseField" FieldLabel="Business License" Width="250" LabelWidth="100" />
                        <ext:ComboBox runat="server"
                            ID="uxStateField"
                            FieldLabel="Business License State"
                            DisplayField="name"
                            ValueField="name"
                            QueryMode="Local"
                            TypeAhead="true"
                            AllowBlank="true"
                            ForceSelection="true">
                            <Store>
                                <ext:Store ID="uxStateStore" runat="server" AutoDataBind="true">
                                    <Model>
                                        <ext:Model ID="Model8" runat="server">
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
                        <ext:TextField runat="server" ID="uxTypeField" FieldLabel="Type of Work" Width="250" LabelWidth="100" />
                        <ext:ComboBox runat="server"
                            ID="uxDensityField"
                            FieldLabel="Density"
                            QueryMode="Local"
                            TypeAhead="true"
                            AllowBlank="true"
                            ForceSelection="true"
                            Width="200"
                            LabelWidth="100">
                            <Items>
                                <ext:ListItem Text="Low" Value="LOW" />
                                <ext:ListItem Text="Medium" Value="MEDIUM" />
                                <ext:ListItem Text="High" Value="HIGH" />
                            </Items>
                        </ext:ComboBox>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxSaveHeaderButton" Icon="Add" Text="Save" Disabled="true">
                            <DirectEvents>
                                <Click OnEvent="deUpdateHeader">
                                    <EventMask ShowMask="true" />
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                    </Buttons>
                    <Listeners>
                        <ValidityChange Handler="#{uxSaveHeaderButton}.setDisabled(!valid)" />
                    </Listeners>
                </ext:FormPanel>
                <ext:GridPanel runat="server"
                    ID="uxWarningGrid"
                    Title="Warnings/Errors"
                    Padding="10"
                    MaxWidth="1200">
                    <Store>
                        <ext:Store runat="server" ID="uxWarningStore">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="WarningType" />
                                        <ext:ModelField Name="RecordType" />
                                        <ext:ModelField Name="AdditionalInformation" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="WarningType" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel runat="server">
                        <Columns>
                            <ext:Column runat="server" ID="uxWarningColumn" DataIndex="WarningType" Flex="5">
                                <Renderer Fn="setIcon" />
                            </ext:Column>
                            <ext:Column runat="server" DataIndex="WarningType" Text="Warning Type" Flex="10" />
                            <ext:Column runat="server" DataIndex="RecordType" Text="Record" Flex="30" />
                            <ext:Column runat="server" DataIndex="AdditionalInformation" Text="Additional Information" Flex="55" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
                <ext:GridPanel runat="server"
                    ID="uxEmployeeGrid"
                    Title="Employees"
                    PaddingSpec="10 10 30 10"
                    MaxWidth="1200">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxEmployeeStore">
                            <Model>
                                <ext:Model ID="Model2" runat="server" Name="Employee" IDProperty="EMPLOYEE_ID" ClientIdProperty="PhantomId">
                                    <Fields>
                                        <ext:ModelField Name="EMPLOYEE_ID" />
                                        <ext:ModelField Name="PERSON_ID" />
                                        <ext:ModelField Name="EMPLOYEE_NAME" />
                                        <ext:ModelField Name="NAME" />
                                        <ext:ModelField Name="EQUIPMENT_ID" />
                                        <ext:ModelField Name="TIME_IN" Type="Date" />
                                        <ext:ModelField Name="TIME_IN_TIME" Type="Date" />
                                        <ext:ModelField Name="TIME_OUT" Type="Date" />
                                        <ext:ModelField Name="TIME_OUT_TIME" Type="Date" />
                                        <ext:ModelField Name="TRAVEL_TIME_FORMATTED" Type="Date" />
                                        <ext:ModelField Name="DRIVE_TIME_FORMATTED" />
                                        <ext:ModelField Name="TOTAL_HOURS" />
                                        <ext:ModelField Name="PER_DIEM" />
                                        <ext:ModelField Name="FOREMAN_LICENSE" />
                                        <ext:ModelField Name="COMMENTS" />
                                        <ext:ModelField Name="LUNCH_LENGTH" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column10" runat="server" DataIndex="EQUIPMENT_ID" Text="Equipment Name" Flex="13">
                                <Editor>
                                    <ext:DropDownField runat="server" Editable="false"
                                        ID="uxEmployeeEqDropDown"
                                        Mode="ValueText"
                                        AllowBlank="true" Width="500">
                                        <CustomConfig>
                                            <ext:ConfigItem Name="setValue" Value="SetEmployeeEqValue" Mode="Raw" />
                                        </CustomConfig>
                                        <Component>
                                            <ext:GridPanel runat="server"
                                                ID="uxEmployeeEqGrid"
                                                Layout="HBoxLayout">
                                                <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxEmployeeEqStore"
                                                        OnReadData="deReadEquipmentData"
                                                        AutoDataBind="true" AutoLoad="true" ClearOnPageLoad="false">
                                                        <Model>
                                                            <ext:Model ID="Model9" runat="server" IDProperty="EQUIPMENT_ID">
                                                                <Fields>
                                                                    <ext:ModelField Name="EQUIPMENT_ID" Type="Int" />
                                                                    <ext:ModelField Name="NAME" Type="String" />
                                                                    <ext:ModelField Name="SEGMENT1" Type="String" />
                                                                    <ext:ModelField Name="ORGANIZATION_NAME" />
                                                                    <ext:ModelField Name="CLASS_CODE" />
                                                                    <ext:ModelField Name="ODOMETER_START" />
                                                                    <ext:ModelField Name="ODOMETER_END" />
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
                                                        <ext:Column ID="Column12" runat="server" Text="Name" DataIndex="NAME" Flex="15" />
                                                        <ext:Column ID="Column42" runat="server" Text="Class Code" DataIndex="CLASS_CODE" Flex="35" />
                                                        <ext:Column ID="Column43" runat="server" Text="Project Number" DataIndex="SEGMENT1" Flex="20" />
                                                        <ext:Column ID="Column44" runat="server" DataIndex="ODOMETER_START" Text="Starting Units" Flex="15" />
                                                        <ext:Column ID="Column45" runat="server" DataIndex="ODOMETER_END" Text="Ending Units" Flex="15" />
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel Mode="Single" />
                                                </SelectionModel>
                                                <DirectEvents>
                                                    <SelectionChange OnEvent="deStoreGridValue">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="EquipmentId" Value="#{uxEmployeeEqGrid}.getSelectionModel().getSelection()[0].data.EQUIPMENT_ID" Mode="Raw" />
                                                            <ext:Parameter Name="Name" Value="#{uxEmployeeEqGrid}.getSelectionModel().getSelection()[0].data.NAME" Mode="Raw" />
                                                            <ext:Parameter Name="Type" Value="Equipment" />
                                                        </ExtraParams>
                                                    </SelectionChange>
                                                </DirectEvents>
                                            </ext:GridPanel>
                                        </Component>
                                        <Listeners>
                                            <Expand Handler="this.picker.setWidth(500);" />
                                        </Listeners>
                                    </ext:DropDownField>
                                </Editor>
                                <Renderer Fn="EmployeeEqRenderer" />
                            </ext:Column>
                            <ext:Column ID="Column9" runat="server" DataIndex="PERSON_ID" Text="Employee Name" Flex="13">
                                <Renderer Fn="EmployeeRenderer" />
                                <Editor>
                                    <ext:DropDownField runat="server"
                                        ID="uxEmployeeEmpDropDown"
                                        Mode="ValueText"
                                        AllowBlank="false"
                                        Editable="false" Width="500" InvalidCls="allowBlank">
                                        <Component>
                                            <ext:GridPanel runat="server"
                                                ID="uxEmployeeEmpGrid"
                                                Layout="HBoxLayout">
                                                <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxEmployeeEmpStore"
                                                        PageSize="10"
                                                        RemoteSort="true"
                                                        OnReadData="deReadEmployeeData" AutoLoad="true">
                                                        <Model>
                                                            <ext:Model ID="Model1" runat="server" IDProperty="PERSON_ID">
                                                                <Fields>
                                                                    <ext:ModelField Name="PERSON_ID" Type="Int" />
                                                                    <ext:ModelField Name="EMPLOYEE_NAME" Type="String" />
                                                                    <ext:ModelField Name="JOB_NAME" Type="String" />
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
                                                        <ext:Column ID="Column6" runat="server" Text="Person ID" DataIndex="PERSON_ID" Flex="20" />
                                                        <ext:Column ID="Column7" runat="server" Text="Name" DataIndex="EMPLOYEE_NAME" Flex="40" />
                                                        <ext:Column ID="Column8" runat="server" Text="Job Name" DataIndex="JOB_NAME" Flex="40" />
                                                    </Columns>
                                                </ColumnModel>
                                                <TopBar>
                                                    <ext:Toolbar ID="Toolbar2" runat="server">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="uxAddEmployeeRegion"
                                                                Icon="Group"
                                                                Text="All Regions"
                                                                EnableToggle="true">
                                                                <DirectEvents>
                                                                    <Click OnEvent="deRegionToggle">
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Type" Value="Employee" />
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </DirectEvents>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </TopBar>
                                                <BottomBar>
                                                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
                                                </BottomBar>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" Mode="Single" />
                                                </SelectionModel>
                                                <DirectEvents>
                                                    <SelectionChange OnEvent="deStoreGridValue">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="PersonId" Value="#{uxEmployeeEmpGrid}.getSelectionModel().getSelection()[0].data.PERSON_ID" Mode="Raw" />
                                                            <ext:Parameter Name="Name" Value="#{uxEmployeeEmpGrid}.getSelectionModel().getSelection()[0].data.EMPLOYEE_NAME" Mode="Raw" />
                                                            <ext:Parameter Name="Type" Value="Employee" />
                                                        </ExtraParams>
                                                    </SelectionChange>
                                                    <SelectionChange OnEvent="deCheckExistingPerDiem">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="PersonId" Value="#{uxEmployeeEmpGrid}.getSelectionModel().getSelection()[0].data.PERSON_ID" Mode="Raw" />
                                                            <ext:Parameter Name="Form" Value="Add" />
                                                        </ExtraParams>
                                                    </SelectionChange>
                                                </DirectEvents>
                                                <Plugins>
                                                    <ext:FilterHeader runat="server" ID="uxEmployeeEmpFilter" Remote="true" />
                                                </Plugins>
                                                <View>
                                                    <ext:GridView runat="server" ID="uxEmployeeView" />
                                                </View>
                                            </ext:GridPanel>
                                        </Component>
                                        <Listeners>
                                            <Expand Handler="this.picker.setWidth(500);" />
                                        </Listeners>
                                        <CustomConfig>
                                            <ext:ConfigItem Name="setValue" Value="SetEmployeeValue" Mode="Raw" />
                                        </CustomConfig>
                                    </ext:DropDownField>
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column5" runat="server" DataIndex="FOREMAN_LICENSE" Text="License" Flex="7">
                                <Editor>
                                    <ext:TextField runat="server"
                                        ID="uxAddEmployeeLicense" Width="500" />
                                </Editor>
                            </ext:Column>
                            <ext:DateColumn ID="DateColumn2" runat="server" DataIndex="TIME_IN" Text="Time In" Flex="8" Format="M/d/yyyy">
                                <Editor>
                                    <ext:DateField runat="server" ID="uxEmployeeTimeInDate" AllowBlank="false" InvalidCls="allowBlank" />
                                </Editor>
                            </ext:DateColumn>
                            <ext:DateColumn ID="DateColumn1" runat="server" DataIndex="TIME_IN_TIME" Text="Time In" Flex="8" Format="h:mm">
                                <Editor>
                                    <ext:TimeField runat="server" AllowBlank="false" InvalidCls="allowBlank" />
                                </Editor>
                            </ext:DateColumn>
                            <ext:DateColumn ID="DateColumn3" runat="server" DataIndex="TIME_OUT" Text="Time Out" Flex="8" Format="M/d/yyyy">
                                <Editor>
                                    <ext:DateField ID="uxEmployeeTimeOutDate" runat="server" AllowBlank="false" InvalidCls="allowBlank" />
                                </Editor>
                            </ext:DateColumn>
                            <ext:DateColumn ID="DateColumn4" runat="server" DataIndex="TIME_OUT_TIME" Text="Time Out" Flex="8" Format="h:mm">
                                <Editor>
                                    <ext:TimeField runat="server" AllowBlank="false" InvalidCls="allowBlank" />
                                </Editor>
                            </ext:DateColumn>
                            <ext:Column ID="Column4" runat="server" DataIndex="TOTAL_HOURS" Text="Total Hours" Flex="7" />
                            <ext:DateColumn runat="server" DataIndex="TRAVEL_TIME_FORMATTED" Text="Travel Time" Flex="6" Format="H:mm">
                                <Editor>
                                    <ext:TimeField runat="server" MinTime="00:00" MaxTime="23:59" Format="H:mm" />
                                </Editor>
                            </ext:DateColumn>
                            <ext:CheckColumn ID="uxPerDiemColumn" runat="server" DataIndex="PER_DIEM" Text="Per Diem" Flex="6" Editable="true" />
                            <ext:Column ID="Column1" runat="server" DataIndex="LUNCH_LENGTH" Text="Lunch Length" Flex="7" />
                            <ext:Column ID="Column14" runat="server" DataIndex="COMMENTS" Text="Comments" Flex="14">
                                <Editor>
                                    <ext:TextArea runat="server"
                                        ID="uxAddEmployeeComments" Width="500"
                                        Rows="5"
                                        AllowBlank="true" />
                                </Editor>
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <TopBar>
                        <ext:Toolbar ID="uxEmployeeToolbar" runat="server">
                            <Items>
                                <ext:Button ID="uxAddEmployeeButton" runat="server" Text="Add" Icon="ApplicationAdd">
                                    <Listeners>
                                        <Click Handler="#{uxEmployeeStore}.insert(0, new Employee())" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="uxDeleteEmployeeButton" runat="server" Text="Delete" Icon="ApplicationDelete" Disabled="true">
                                    <Listeners>
                                        <Click Fn="deleteEmployee" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSeparator runat="server" />
                                <ext:Button runat="server"
                                    ID="uxChooseLunchHeaderButton"
                                    Text="Choose Lunch Project"
                                    Icon="Link"
                                    Disabled="true">
                                    <Listeners>
                                        <Click Handler="parent.App.direct.dmLoadLunchWindow(App.uxHeaderField.value, App.uxEmployeeGrid.getSelectionModel().getSelection()[0].data.EMPLOYEE_ID)" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" />
                                <ext:Button runat="server"
                                    ID="uxChoosePerDiemButton"
                                    Text="Choose Per Diem"
                                    Icon="LinkAdd"
                                    Disabled="true">
                                    <Listeners>
                                        <Click Handler="parent.App.direct.dmLoadPerDiemWindow(App.uxHeaderField.value, App.uxEmployeeGrid.getSelectionModel().getSelection()[0].data.EMPLOYEE_ID)" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Plugins>
                        <ext:RowEditing runat="server" ClicksToEdit="1" AutoCancel="false" ID="test">
                            <DirectEvents>
                                <Edit OnEvent="deSaveEmployee" Before="return #{uxEmployeeStore}.isDirty();">
                                    <ExtraParams>
                                        <ext:Parameter Name="data" Value="#{uxEmployeeStore}.getChangedData({skipIdForPhantomRecords : false})" Mode="Raw" Encode="true" />
                                        <ext:Parameter Name="TypeName" Value="#{uxEmployeeEmpDropDown}.Value" Mode="Raw" />
                                    </ExtraParams>
                                    <EventMask ShowMask="true" />
                                </Edit>
                            </DirectEvents>
                        </ext:RowEditing>
                    </Plugins>
                    <Listeners>
                        <Select Handler="#{uxDeleteEmployeeButton}.enable(); #{uxChooseLunchHeaderButton}.enable(); #{uxChoosePerDiemButton}.enable()" />
                    </Listeners>
                </ext:GridPanel>
                <ext:GridPanel runat="server" ID="uxEquipmentGrid"
                    Title="Equipment"
                    PaddingSpec="10 10 30 10"
                    MaxWidth="1200">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxEquipmentStore">
                            <Model>
                                <ext:Model runat="server" Name="Equipment" IDProperty="EQUIPMENT_ID" ClientIdProperty="PhantomID">
                                    <Fields>
                                        <ext:ModelField Name="EQUIPMENT_ID" />
                                        <ext:ModelField Name="CLASS_CODE" />
                                        <ext:ModelField Name="ORGANIZATION_NAME" />
                                        <ext:ModelField Name="ODOMETER_START" />
                                        <ext:ModelField Name="ODOMETER_END" />
                                        <ext:ModelField Name="PROJECT_ID" />
                                        <ext:ModelField Name="NAME" />
                                        <ext:ModelField Name="HEADER_ID" />
                                        <ext:ModelField Name="SEGMENT1" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column47" runat="server"
                                DataIndex="SEGMENT1"
                                Text="Project Number" Flex="10">
                            </ext:Column>
                            <ext:Column ID="Column48" runat="server"
                                DataIndex="PROJECT_ID"
                                Text="Name" Flex="10">
                                <Editor>
                                    <ext:DropDownField runat="server" Editable="false"
                                        ID="uxAddEquipmentDropDown"
                                        Mode="ValueText"
                                        AllowBlank="false" InvalidCls="allowBlank">
                                        <Component>
                                            <ext:GridPanel runat="server"
                                                ID="uxAddEquipmentGrid"
                                                Layout="HBoxLayout" Floatable="true" Floating="true">
                                                <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxAddEquipmentDropDownStore"
                                                        OnReadData="deReadEquipmentGrid"
                                                        PageSize="10"
                                                        RemoteSort="true"
                                                        AutoDataBind="true">
                                                        <Model>
                                                            <ext:Model ID="Model11" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="CLASS_CODE" Type="String" />
                                                                    <ext:ModelField Name="NAME" Type="String" />
                                                                    <ext:ModelField Name="ORG_ID" />
                                                                    <ext:ModelField Name="ORGANIZATION_ID" Type="Int" />
                                                                    <ext:ModelField Name="ORGANIZATION_NAME" Type="String" />
                                                                    <ext:ModelField Name="PROJECT_ID" Type="Int" />
                                                                    <ext:ModelField Name="PROJECT_STATUS_CODE" />
                                                                    <ext:ModelField Name="SEGMENT1" Type="Int" />
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
                                                        <ext:Column runat="server"
                                                            ID="uxEquipmentClassCode"
                                                            DataIndex="CLASS_CODE"
                                                            Text="Class Code" />
                                                        <ext:Column runat="server"
                                                            ID="uxEquipmentName"
                                                            DataIndex="NAME"
                                                            Text="Equipment Name" />
                                                        <ext:Column runat="server"
                                                            ID="uxEquipmentOrgName"
                                                            DataIndex="ORGANIZATION_NAME"
                                                            Text="Organization Name" />
                                                        <ext:Column runat="server"
                                                            ID="uxEquipmentSegment"
                                                            DataIndex="SEGMENT1"
                                                            Text="Project Number" />
                                                    </Columns>
                                                </ColumnModel>
                                                <Plugins>
                                                    <ext:FilterHeader ID="uxAddEquipmentFilter" runat="server" Remote="true" />
                                                </Plugins>
                                                <TopBar>
                                                    <ext:Toolbar runat="server"
                                                        ID="uxEquipmentBar">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="uxAddEquipmentToggleOrg"
                                                                EnableToggle="true"
                                                                Text="All Regions"
                                                                Icon="Group">
                                                                <DirectEvents>
                                                                    <Toggle OnEvent="deReloadEquipmentStore">
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Type" Value="Equipment" />
                                                                        </ExtraParams>
                                                                    </Toggle>
                                                                </DirectEvents>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </TopBar>
                                                <BottomBar>
                                                    <ext:PagingToolbar runat="server"
                                                        ID="uxAddEquipmentPaging" />
                                                </BottomBar>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" Mode="Single" />
                                                </SelectionModel>
                                                <DirectEvents>
                                                    <SelectionChange OnEvent="deStoreEquipmentGridValue">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="ProjectId" Value="#{uxAddEquipmentGrid}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
                                                            <ext:Parameter Name="EquipmentName" Value="#{uxAddEquipmentGrid}.getSelectionModel().getSelection()[0].data.NAME" Mode="Raw" />
                                                            <ext:Parameter Name="SEGMENT1" Value="#{uxAddEquipmentGrid}.getSelectionModel().getSelection()[0].data.SEGMENT1" Mode="Raw" />
                                                            <ext:Parameter Name="CLASS_CODE" Value="#{uxAddEquipmentGrid}.getSelectionModel().getSelection()[0].data.CLASS_CODE" Mode="Raw" />
                                                            <ext:Parameter Name="ORGANIZATION_NAME" Value="#{uxAddEquipmentGrid}.getSelectionModel().getSelection()[0].data.ORGANIZATION_NAME" Mode="Raw" />
                                                        </ExtraParams>
                                                    </SelectionChange>
                                                </DirectEvents>
                                            </ext:GridPanel>
                                        </Component>
                                        <Listeners>
                                            <Expand Handler="this.picker.setWidth(500);" />
                                        </Listeners>
                                        <CustomConfig>
                                            <ext:ConfigItem Name="setValue" Value="SetEquipmentValue" Mode="Raw" />
                                        </CustomConfig>
                                    </ext:DropDownField>
                                </Editor>
                                <Renderer Fn="EquipmentRenderer" />
                            </ext:Column>
                            <ext:Column ID="Column49" runat="server"
                                DataIndex="CLASS_CODE"
                                Text="Class Code" Flex="35" />
                            <ext:Column ID="Column50" runat="server"
                                DataIndex="ORGANIZATION_NAME"
                                Text="Organization Name" Flex="25" />
                            <ext:Column ID="Column51" runat="server"
                                DataIndex="ODOMETER_START"
                                Text="Starting Units" Flex="10">
                                <Editor>
                                    <ext:NumberField runat="server" MinValue="0" />
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column52" runat="server"
                                DataIndex="ODOMETER_END"
                                Text="Ending Units" Flex="10">
                                <Editor>
                                    <ext:NumberField runat="server" MinValue="0" />
                                </Editor>
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <TopBar>
                        <ext:Toolbar ID="uxEquipmentToolbar" runat="server">
                            <Items>
                                <ext:Button ID="uxAddEquipmentButton" runat="server" Text="Add" Icon="ApplicationAdd">
                                    <Listeners>
                                        <Click Handler="#{uxEquipmentStore}.insert(0, new Equipment())" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="uxDeleteEquipmentButton" runat="server" Text="Delete" Icon="ApplicationDelete" Disabled="true">
                                    <Listeners>
                                        <Click Fn="deleteEquipment" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Plugins>
                        <ext:RowEditing runat="server" AutoCancel="false" ClicksToEdit="1">
                            <DirectEvents>
                                <Edit OnEvent="deSaveEquipment" Before="return #{uxEquipmentStore}.isDirty();">
                                    <ExtraParams>
                                        <ext:Parameter Name="data" Value="#{uxEquipmentStore}.getChangedData({skipIdForPhantomRecords : false})" Mode="Raw" Encode="true" />
                                    </ExtraParams>
                                </Edit>
                            </DirectEvents>
                        </ext:RowEditing>
                    </Plugins>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="uxEquipmentSM" runat="server" Mode="Single" />
                    </SelectionModel>
                    <View>
                        <ext:GridView runat="server" ID="uxEquipmentView" />
                    </View>
                    <Listeners>
                        <Select Handler="#{uxDeleteEquipmentButton}.enable()" />
                    </Listeners>
                </ext:GridPanel>
                <ext:GridPanel runat="server"
                    ID="uxProductionGrid"
                    Title="Production"
                    PaddingSpec="10 10 30 10"
                    MaxWidth="1200">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxProductionStore">
                            <Model>
                                <ext:Model ID="Model3" runat="server" Name="Production" IDProperty="PRODUCTION_ID" ClientIdProperty="PhantomID">
                                    <Fields>
                                        <ext:ModelField Name="PRODUCTION_ID" />
                                        <ext:ModelField Name="TASK_NUMBER" />
                                        <ext:ModelField Name="DESCRIPTION" />
                                        <ext:ModelField Name="WORK_AREA" />
                                        <ext:ModelField Name="POLE_FROM" />
                                        <ext:ModelField Name="POLE_TO" />
                                        <ext:ModelField Name="ACRES_MILE" />
                                        <ext:ModelField Name="QUANTITY" />
                                        <ext:ModelField Name="TASK_ID" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" DataIndex="TASK_NUMBER" Text="Task Number" Flex="10" />
                            <ext:Column ID="Column15" runat="server" DataIndex="TASK_ID" Text="Task Name" Flex="15">
                                <Editor>
                                    <ext:DropDownField runat="server" Editable="false"
                                        ID="uxAddProductionTask"
                                        Mode="ValueText"
                                        AllowBlank="false" InvalidCls="allowBlank">
                                        <Component>
                                            <ext:GridPanel runat="server"
                                                ID="uxAddProductionTaskGrid"
                                                Layout="HBoxLayout">
                                                <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxAddProductionTaskStore" OnReadData="deReadTaskData" AutoDataBind="true" AutoLoad="true" RemoteSort="true" PageSize="10">
                                                        <Model>
                                                            <ext:Model ID="Model12" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="TASK_ID" />
                                                                    <ext:ModelField Name="TASK_NUMBER" />
                                                                    <ext:ModelField Name="DESCRIPTION" />
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
                                                        <ext:Column ID="Column11" runat="server" DataIndex="TASK_NUMBER" Text="Task Number" Flex="25" />
                                                        <ext:Column ID="Column13" runat="server" DataIndex="DESCRIPTION" Text="Name" Flex="75" />
                                                    </Columns>
                                                </ColumnModel>
                                                <DirectEvents>
                                                    <SelectionChange OnEvent="deStoreTask">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="TaskId" Value="#{uxAddProductionTaskGrid}.getSelectionModel().getSelection()[0].data.TASK_ID" Mode="Raw" />
                                                            <ext:Parameter Name="Description" Value="#{uxAddProductionTaskGrid}.getSelectionModel().getSelection()[0].data.DESCRIPTION" Mode="Raw" />
                                                        </ExtraParams>
                                                    </SelectionChange>
                                                </DirectEvents>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel ID="RowSelectionModel3" runat="server" Mode="Single" />
                                                </SelectionModel>
                                                <Plugins>
                                                    <ext:FilterHeader ID="FilterHeader3" runat="server" Remote="true" />
                                                </Plugins>
                                                <BottomBar>
                                                    <ext:PagingToolbar ID="PagingToolbar3" runat="server" />
                                                </BottomBar>
                                            </ext:GridPanel>
                                        </Component>
                                        <CustomConfig>
                                            <ext:ConfigItem Name="setValue" Value="SetTaskValue" Mode="Raw" />
                                        </CustomConfig>
                                        <Listeners>
                                            <Expand Handler="this.picker.setWidth(500)" />
                                        </Listeners>
                                    </ext:DropDownField>
                                </Editor>
                                <Renderer Fn="ProductionTaskRenderer" />
                            </ext:Column>
                            <ext:Column ID="Column16" runat="server" DataIndex="WORK_AREA" Text="Spray/Work Area" Flex="40">
                                <Editor>
                                    <ext:TextField runat="server" AllowBlank="false" InvalidCls="allowBlank" />
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column17" runat="server" DataIndex="POLE_FROM" Text="Pole/MP From" Flex="9">
                                <Editor>
                                    <ext:TextField runat="server" />
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column18" runat="server" DataIndex="POLE_TO" Text="Pole/MP To" Flex="9">
                                <Editor>
                                    <ext:TextField runat="server" />
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column19" runat="server" DataIndex="ACRES_MILE" Text="Acres/Mile" Flex="9">
                                <Editor>
                                    <ext:NumberField runat="server" MinValue="0" AllowBlank="false" InvalidCls="allowBlank" />
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column20" runat="server" DataIndex="QUANTITY" Text="Gallons" Flex="8">
                                <Editor>
                                    <ext:NumberField runat="server" MinValue="0" AllowBlank="false" InvalidCls="allowBlank" />
                                </Editor>
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <TopBar>
                        <ext:Toolbar ID="uxProductionToolbar" runat="server">
                            <Items>
                                <ext:Button ID="uxAddProductionButton" runat="server" Text="Add" Icon="ApplicationAdd">
                                    <Listeners>
                                        <Click Handler="#{uxProductionStore}.insert(0, new Production())" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="uxDeleteProductionButton" runat="server" Text="Delete" Icon="ApplicationDelete" Disabled="true">
                                    <Listeners>
                                        <Click Fn="deleteProduction" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Listeners>
                        <Select Handler="#{uxDeleteProductionButton}.enable()" />
                    </Listeners>
                    <Plugins>
                        <ext:RowEditing runat="server" ClicksToEdit="1" AutoCancel="false">
                            <DirectEvents>
                                <Edit OnEvent="deSaveProduction" Before="return #{uxProductionStore}.isDirty();">
                                    <ExtraParams>
                                        <ext:Parameter Name="data" Value="#{uxProductionStore}.getChangedData({skipIdForPhantomRecords : false})" Mode="Raw" Encode="true" />
                                    </ExtraParams>
                                </Edit>
                            </DirectEvents>
                        </ext:RowEditing>
                    </Plugins>
                </ext:GridPanel>
                <ext:GridPanel runat="server"
                    ID="uxWeatherGrid"
                    Title="Weather"
                    PaddingSpec="10 10 30 10"
                    MaxWidth="1200">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxWeatherStore">
                            <Model>
                                <ext:Model ID="Model4" runat="server" Name="Weather">
                                    <Fields>
                                        <ext:ModelField Name="WEATHER_ID" />
                                        <ext:ModelField Name="WEATHER_DATE" Type="Date" />
                                        <ext:ModelField Name="WEATHER_TIME" Type="Date" />
                                        <ext:ModelField Name="WIND_DIRECTION" />
                                        <ext:ModelField Name="WIND_VELOCITY" />
                                        <ext:ModelField Name="TEMP" />
                                        <ext:ModelField Name="HUMIDITY" />
                                        <ext:ModelField Name="COMMENTS" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:DateColumn ID="DateColumn6" runat="server" DataIndex="WEATHER_DATE" Text="Date" Format="M/d/yyyy" Flex="8">
                                <Editor>
                                    <ext:DateField runat="server" AllowBlank="false" InvalidCls="allowBlank" />
                                </Editor>
                            </ext:DateColumn>
                            <ext:DateColumn runat="server" DataIndex="WEATHER_TIME" Text="Time" Format="h:mm" Flex="7">
                                <Editor>
                                    <ext:TimeField runat="server" AllowBlank="false" InvalidCls="allowBlank" />
                                </Editor>
                            </ext:DateColumn>
                            <ext:Column ID="Column21" runat="server" DataIndex="WIND_DIRECTION" Text="Wind Direction" Flex="10">
                                <Editor>
                                    <ext:ComboBox runat="server"
                                        ID="uxAddWeatherWindDirection"
                                        DisplayField="name"
                                        ValueField="abbr"
                                        QueryMode="Local"
                                        TypeAhead="true"
                                        ForceSelection="true" Width="500" AllowBlank="false" InvalidCls="allowBlank">
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="uxAddWeatherWindStore">
                                                <Model>
                                                    <ext:Model runat="server">
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
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column22" runat="server" DataIndex="WIND_VELOCITY" Text="Wind Velocity" Flex="10">
                                <Editor>
                                    <ext:NumberField runat="server" MinValue="0" AllowBlank="false" InvalidCls="allowBlank" />
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column23" runat="server" DataIndex="TEMP" Text="Temperature" Flex="10">
                                <Editor>
                                    <ext:NumberField runat="server" MinValue="-50" MaxValue="150" AllowBlank="false" InvalidCls="allowBlank" />
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column24" runat="server" DataIndex="HUMIDITY" Text="Humidity" Flex="10">
                                <Editor>
                                    <ext:NumberField runat="server" MinValue="0" MaxValue="100" AllowBlank="false" InvalidCls="allowBlank" />
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column25" runat="server" DataIndex="COMMENTS" Text="Comments" Flex="45">
                                <Editor>
                                    <ext:TextArea runat="server" />
                                </Editor>
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <TopBar>
                        <ext:Toolbar ID="uxWeatherToolbar" runat="server">
                            <Items>
                                <ext:Button ID="uxAddWeatherButton" runat="server" Text="Add" Icon="ApplicationAdd">
                                    <Listeners>
                                        <Click Handler="#{uxWeatherStore}.insert(0, new Weather())" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="uxDeleteWeatherButton" runat="server" Text="Delete" Icon="ApplicationDelete" Disabled="true">
                                    <Listeners>
                                        <Click Fn="deleteWeather" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Listeners>
                        <Select Handler="#{uxDeleteWeatherButton}.enable()" />
                    </Listeners>
                    <Plugins>
                        <ext:RowEditing ID="RowEditing1" runat="server" ClicksToEdit="1" AutoCancel="false">
                            <DirectEvents>
                                <Edit OnEvent="deSaveWeather" Before="return #{uxWeatherStore}.isDirty();">
                                    <ExtraParams>
                                        <ext:Parameter Name="data" Value="#{uxWeatherStore}.getChangedData({skipIdForPhantomRecords : false})" Mode="Raw" Encode="true" />
                                    </ExtraParams>
                                </Edit>
                            </DirectEvents>
                        </ext:RowEditing>
                    </Plugins>
                </ext:GridPanel>
                <ext:GridPanel runat="server"
                    ID="uxChemicalGrid"
                    Title="Chemical Mix"
                    PaddingSpec="10 10 30 10"
                    MaxWidth="1200">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxChemicalStore">
                            <Model>
                                <ext:Model ID="Model5" runat="server" Name="Chemical">
                                    <Fields>
                                        <ext:ModelField Name="CHEMICAL_MIX_ID" />
                                        <ext:ModelField Name="CHEMICAL_MIX_NUMBER" />
                                        <ext:ModelField Name="TARGET_AREA" />
                                        <ext:ModelField Name="GALLON_ACRE" />
                                        <ext:ModelField Name="GALLON_STARTING" />
                                        <ext:ModelField Name="GALLON_MIXED" />
                                        <ext:ModelField Name="GALLON_REMAINING" />
                                        <ext:ModelField Name="STATE" />
                                        <ext:ModelField Name="COUNTY" />
                                        <ext:ModelField Name="TOTAL" />
                                        <ext:ModelField Name="USED" />
                                        <ext:ModelField Name="ACRES_SPRAYED" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column26" runat="server" DataIndex="CHEMICAL_MIX_NUMBER" Text="Mix #" Flex="4" />
                            <ext:Column ID="Column27" runat="server" DataIndex="TARGET_AREA" Text="Target Area" Flex="10">
                                <Editor>
                                    <ext:TextField runat="server" />
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column28" runat="server" DataIndex="GALLON_ACRE" Text="Gallons/Acre" Flex="8">
                                <Editor>
                                    <ext:NumberField runat="server" MinValue="0" />
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column29" runat="server" DataIndex="GALLON_STARTING" Text="Gallons Starting" Flex="10">
                                <Editor>
                                    <ext:NumberField runat="server" MinValue="0" />
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column30" runat="server" DataIndex="GALLON_MIXED" Text="Gallon Mixed" Flex="8">
                                <Editor>
                                    <ext:NumberField runat="server" MinValue="0" />
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column31" runat="server" DataIndex="TOTAL" Text="Total Gallons" Flex="10" />
                            <ext:Column ID="Column2" runat="server" DataIndex="GALLON_REMAINING" Text="Gallon Remaining" Flex="10">
                                <Editor>
                                    <ext:NumberField runat="server" MinValue="0" />
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column32" runat="server" DataIndex="USED" Text="Gallons Used" Flex="10" />
                            <ext:Column ID="Column33" runat="server" DataIndex="ACRES_SPRAYED" Text="Acres Sprayed" Flex="10" />
                            <ext:Column ID="Column34" runat="server" DataIndex="STATE" Text="State" Flex="10">
                                <Editor>
                                    <ext:ComboBox runat="server"
                                        ID="uxAddChemicalState"
                                        DisplayField="name"
                                        ValueField="name"
                                        QueryMode="Local"
                                        TypeAhead="true"
                                        AllowBlank="false"
                                        ForceSelection="true"
                                        Width="500">
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
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column35" runat="server" DataIndex="COUNTY" Text="County" Flex="10">
                                <Editor>
                                    <ext:TextField runat="server" />
                                </Editor>
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <TopBar>
                        <ext:Toolbar ID="uxChemicalToolbar" runat="server">
                            <Items>
                                <ext:Button ID="uxAddChemicalButton" runat="server" Text="Add" Icon="ApplicationAdd">
                                    <Listeners>
                                        <Click Handler="#{uxChemicalStore}.insert(0, new Chemical())" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="uxDeleteChemicalButton" runat="server" Text="Delete" Icon="ApplicationDelete" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deRemoveChemical">
                                            <Confirmation ConfirmRequest="true" Title="Remove?" Message="Do you really want to remove?" />
                                            <ExtraParams>
                                                <ext:Parameter Name="ChemicalId" Value="#{uxChemicalGrid}.getSelectionModel().getSelection()[0].data.CHEMICAL_MIX_ID" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Listeners>
                        <Select Handler="#{uxDeleteChemicalButton}.enable()" />
                    </Listeners>
                    <Plugins>
                        <ext:RowEditing runat="server" AutoCancel="false" ClicksToEdit="1" />
                    </Plugins>
                </ext:GridPanel>
                <ext:GridPanel runat="server"
                    ID="uxInventoryGrid"
                    Title="Inventory"
                    PaddingSpec="10 10 30 10"
                    MaxWidth="1200">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxInventoryStore">
                            <Model>
                                <ext:Model ID="Model6" runat="server" Name="Inventory">
                                    <Fields>
                                        <ext:ModelField Name="INVENTORY_ID" />
                                        <ext:ModelField Name="CHEMICAL_MIX_NUMBER" />
                                        <ext:ModelField Name="SEGMENT1" />
                                        <ext:ModelField Name="INV_NAME" />
                                        <ext:ModelField Name="SUB_INVENTORY_SECONDARY_NAME" />
                                        <ext:ModelField Name="DESCRIPTION" />
                                        <ext:ModelField Name="RATE" />
                                        <ext:ModelField Name="TOTAL" />
                                        <ext:ModelField Name="UNIT_OF_MEASURE" />
                                        <ext:ModelField Name="EPA_NUMBER" />
                                        <ext:ModelField Name="CONTRACTOR_SUPPLIED" Type="Boolean" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column36" runat="server" DataIndex="CHEMICAL_MIX_NUMBER" Text="Mix #" Flex="4" />
                            <ext:Column ID="Column3" runat="server" DataIndex="SEGMENT1" Text="Item ID" Flex="5" />
                            <ext:Column runat="server" DataIndex="INV_NAME" Text="Inventory Org" Flex="13" />
                            <ext:Column ID="Column37" runat="server" DataIndex="SUB_INVENTORY_SECONDARY_NAME" Text="Sub-Inv Name" Flex="10" />
                            <ext:Column ID="Column38" runat="server" DataIndex="DESCRIPTION" Text="Item" Flex="23" />
                            <ext:Column ID="Column39" runat="server" DataIndex="RATE" Text="Rate" Flex="5">
                                <Editor>
                                    <ext:NumberField runat="server" MinValue="0" />
                                </Editor>
                            </ext:Column>
                            <ext:Column runat="server" DataIndex="TOTAL" Text="Total" Flex="5">
                                <Editor>
                                    <ext:NumberField runat="server" MinValue="0" />
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column40" runat="server" DataIndex="UNIT_OF_MEASURE" Text="Unit" Flex="10" />
                            <ext:Column ID="Column41" runat="server" DataIndex="EPA_NUMBER" Text="EPA Number" Flex="10" />
                            <ext:CheckColumn ID="CheckColumn1" runat="server"
                                DataIndex="CONTRACTOR_SUPPLIED"
                                Text="Customer Material" Flex="15" Editable="true" />
                        </Columns>
                    </ColumnModel>
                    <TopBar>
                        <ext:Toolbar ID="uxInventoryToolbar" runat="server">
                            <Items>
                                <ext:Button ID="uxAddInventoryButton" runat="server" Text="Add" Icon="ApplicationAdd">
                                    <Listeners>
                                        <Click Handler="#{uxInventoryStore}.insert(0, new Inventory())" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="uxEditInventoryButton" runat="server" Text="Edit" Icon="ApplicationEdit" Disabled="true">
                                    <Listeners>
                                        <Click Handler="parent.App.direct.dmLoadInventoryWindow_DBI('Edit', App.uxHeaderField.value, App.uxInventoryGrid.getSelectionModel().getSelection()[0].data.INVENTORY_ID)" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="uxDeleteInventoryButton" runat="server" Text="Delete" Icon="ApplicationDelete" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deRemoveInventory">
                                            <Confirmation ConfirmRequest="true" Title="Really Delete?" Message="Do you really want to delete?" />
                                            <ExtraParams>
                                                <ext:Parameter Name="InventoryId" Value="#{uxInventoryGrid}.getSelectionModel().getSelection()[0].data.INVENTORY_ID" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Listeners>
                        <Select Handler="#{uxEditInventoryButton}.enable(); #{uxDeleteInventoryButton}.enable()" />
                    </Listeners>
                    <Plugins>
                        <ext:RowEditing runat="server" ClicksToEdit="1" AutoCancel="false" />
                    </Plugins>
                </ext:GridPanel>
                <ext:FormPanel runat="server" ID="uxFooterPanel" Padding="10" BodyPadding="5" MaxWidth="1200">
                    <Items>
                        <ext:TextField runat="server" ID="uxReasonForNoWorkField" FieldLabel="Reason for no work" Width="700" LabelWidth="100" />
                        <ext:TextField runat="server" ID="uxHotelField" FieldLabel="Hotel" LabelWidth="100" Width="400" />
                        <ext:TextField runat="server" ID="uxCityField" FieldLabel="City" LabelWidth="100" Width="300" />
                        <ext:ComboBox runat="server"
                            ID="uxFooterStateField"
                            FieldLabel="State"
                            DisplayField="name"
                            ValueField="name"
                            QueryMode="Local"
                            TypeAhead="true"
                            ForceSelection="true">
                            <Store>
                                <ext:Store ID="uxStateList" runat="server" AutoDataBind="true">
                                    <Model>
                                        <ext:Model ID="Model7" runat="server">
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
                        <ext:TextField runat="server" ID="uxPhoneField" FieldLabel="Phone" LabelWidth="100" Width="300" />
                        <ext:TextField runat="server" ID="uxForemanNameField" FieldLabel="Foreman Name" ReadOnly="true" LabelWidth="100" Width="500" />
                        <ext:FileUploadField runat="server"
                            ID="uxForemanImageField"
                            FieldLabel="Foreman Signature" />
                        <ext:FieldContainer ID="FieldContainer1" runat="server" FieldLabel="Foreman Signature" LabelWidth="100">
                            <Items>
                                <ext:Image runat="server" Height="214" ID="uxForemanImage" Width="320" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:TextField runat="server" ID="uxContractNameField" FieldLabel="Contract Rep Name" Width="500" LabelWidth="100" />
                        <ext:FileUploadField runat="server"
                            ID="uxContractImageField"
                            FieldLabel="Contract Representative" />
                        <ext:FieldContainer ID="FieldContainer2" runat="server" FieldLabel="Contract Rep Signature" LabelWidth="100">
                            <Items>
                                <ext:Image runat="server" Height="214" ID="uxContractImage" Width="320" />
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxSaveFooterButton" Text="Save" Icon="Add">
                            <DirectEvents>
                                <Click OnEvent="deUpdateFooter">
                                    <EventMask ShowMask="true" />
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
                <ext:ToolTip ID="ToolTip1"
                    runat="server"
                    Target="={#{uxWarningGrid}.getView().el}"
                    Delegate=".x-grid-cell"
                    TrackMouse="true"
                    UI="Warning"
                    Width="400">
                    <Listeners>
                        <Show Handler="onShow(this, #{uxWarningGrid});" />
                    </Listeners>
                </ext:ToolTip>
                <ext:ToolTip ID="ToolTip2"
                    runat="server"
                    Target="={#{uxEmployeeGrid}.getView().el}"
                    Delegate=".x-grid-cell"
                    TrackMouse="true"
                    UI="Warning"
                    Width="400">
                    <Listeners>
                        <Show Handler="onShow(this, #{uxEmployeeGrid});" />
                    </Listeners>
                </ext:ToolTip>
                <ext:ToolTip ID="ToolTip3"
                    runat="server"
                    Target="={#{uxEquipmentGrid}.getView().el}"
                    Delegate=".x-grid-cell"
                    TrackMouse="true"
                    UI="Warning"
                    Width="400">
                    <Listeners>
                        <Show Handler="onShow(this, #{uxEquipmentGrid});" />
                    </Listeners>
                </ext:ToolTip>
                <ext:ToolTip ID="ToolTip4"
                    runat="server"
                    Target="={#{uxInventoryGrid}.getView().el}"
                    Delegate=".x-grid-cell"
                    TrackMouse="true"
                    UI="Warning"
                    Width="400">
                    <Listeners>
                        <Show Handler="onShow(this, #{uxInventoryGrid});" />
                    </Listeners>
                </ext:ToolTip>
                <ext:ToolTip ID="ToolTip5"
                    runat="server"
                    Target="={#{uxWeatherGrid}.getView().el}"
                    Delegate=".x-grid-cell"
                    TrackMouse="true"
                    UI="Warning"
                    Width="400">
                    <Listeners>
                        <Show Handler="onShow(this, #{uxWeatherGrid});" />
                    </Listeners>
                </ext:ToolTip>
                <ext:ToolTip ID="ToolTip6"
                    runat="server"
                    Target="={#{uxProductionGrid}.getView().el}"
                    Delegate=".x-grid-cell"
                    TrackMouse="true"
                    UI="Warning"
                    Width="400">
                    <Listeners>
                        <Show Handler="onShow(this, #{uxProductionGrid});" />
                    </Listeners>
                </ext:ToolTip>
                <ext:ToolTip ID="ToolTip7"
                    runat="server"
                    Target="={#{uxChemicalGrid}.getView().el}"
                    Delegate=".x-grid-cell"
                    TrackMouse="true"
                    UI="Warning"
                    Width="400">
                    <Listeners>
                        <Show Handler="onShow(this, #{uxChemicalGrid});" />
                    </Listeners>
                </ext:ToolTip>
            </Items>
        </ext:Panel>
    </form>
</body>
</html>

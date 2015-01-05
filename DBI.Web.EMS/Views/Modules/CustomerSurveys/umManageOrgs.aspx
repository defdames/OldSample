<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umManageOrgs.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umManageOrgs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../../Resources/StyleSheets/main.css" />
    <script type="text/javascript">
        Ext.apply(Ext.form.VTypes, {
            numberrange: function (val, field) {
                if (!val) {
                    return;
                }

                if (field.startNumberField && (!field.numberRangeMax || (val != field.numberRangeMax))) {
                    var start = Ext.getCmp(field.startNumberField);

                    if (start) {
                        start.setMaxValue(val);
                        field.numberRangeMax = val;
                        start.validate();
                    }
                } else if (field.endNumberField && (!field.numberRangeMin || (val != field.numberRangeMin))) {
                    var end = Ext.getCmp(field.endNumberField);

                    if (end) {
                        end.setMinValue(val);
                        field.numberRangeMin = val;
                        end.validate();
                    }
                }

                return true;
            }
        });

        var formTypeRenderer = function (value) {
            var r = App.uxFormTypeStore.getById(value);
            if (Ext.isEmpty(r)) {
                return "";
            }
            return r.data.TYPE_NAME;
        };

        var cancelEditRow = function (value) {
            if (value == 'dollar') {
                if (!App.uxDollarStore.getAt(0).data.AMOUNT_ID) {
                    App.uxDollarStore.removeAt(0);
                }
                else {
                    App.uxDeleteDollarButton.enable();
                }
                App.uxAddDollarButton.enable();
                App.uxDollarSelection.setLocked(false);
                App.uxCompanySelectionModel.setLocked(false);
            }
            else {
                if (!App.uxThresholdStore.getAt(0).data.THRESHOLD_ID) {
                    App.uxThresholdStore.removeAt(0);
                }
                else {
                    App.uxDeleteThresholdButton.enable();
                }
                App.uxAddThresholdButton.enable();
                App.uxThresholdSelection.setLocked(false);
                App.uxCompanySelectionModel.setLocked(false);
            }
            checkEditing();
        };

        var AddDollarThreshold = function () {
            App.uxDollarStore.insert(0, new DollarThreshold());
            App.uxDollarSelection.select(0);
            var task = new Ext.util.DelayedTask(function () {
                App.uxDollarRowEdit.startEdit(0, 0);
            });
            task.delay(300);

            // Create DelayedTask and call it after 100 ms
            task = new Ext.util.DelayedTask(function () {
                App.uxDollarGrid.columns[1].getEditor().focusInput();
            });
            task.delay(400);
        };

        var AddThreshold = function () {
            App.uxThresholdStore.insert(0, new Threshold());
            App.uxThresholdSelection.select(0);
            var task = new Ext.util.DelayedTask(function () {
                App.uxThresholdRowEdit.startEdit(0, 0);
            });
            task.delay(300);

            // Create DelayedTask and call it after 100 ms
            task = new Ext.util.DelayedTask(function () {
                App.uxThresholdGrid.columns[2].getEditor().focusInput();
            });
            task.delay(400);
        };

        var onBeforeEdit = function (value) {
            if (value == "dollar") {

                if (App.uxDollarRowEdit.editing)
                    return false;
                else {
                    App.direct.dmSetDirty('true');
                    App.uxDeleteDollarButton.disable();
                    App.uxDollarSelection.setLocked(true);
                    App.uxCompanySelectionModel.setLocked(true);
                    return true;
                }
            }
            else {
                if (App.uxThresholdRowEdit.editing)
                    return false;
                else {
                    App.direct.dmSetDirty('true');
                    App.uxDeleteThresholdButton.disable();
                    App.uxThresholdSelection.setLocked(true);
                    App.uxCompanySelectionModel.setLocked(true);
                    return true;
                }
            }
        };

        var checkEditing = function () {
            if (App.uxThresholdRowEdit.editing || App.uxDollarRowEdit.editing) {
                App.direct.dmSetDirty('true');
            }
            else {
                App.direct.dmSetDirty('false');
            }
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" IsDynamic="false" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:TreePanel
                    ID="uxOrgPanel"
                    runat="server"
                    Title="Organizations"
                    BodyPadding="6"
                    Region="West"
                    Weight="100"
                    Width="300"
                    AutoScroll="true"
                    RootVisible="true"
                    Lines="false"
                    UseArrows="true">
                    <Store>
                        <ext:TreeStore ID="TreeStore1" runat="server" OnReadData="deLoadOrgTree">
                            <Proxy>
                                <ext:PageProxy></ext:PageProxy>
                            </Proxy>
                        </ext:TreeStore>
                    </Store>
                    <Root>
                        <ext:Node NodeID="0" Text="All Companies" Expanded="true" />
                    </Root>
                    <SelectionModel>
                        <ext:TreeSelectionModel ID="uxCompanySelectionModel" runat="server" Mode="Single" />
                    </SelectionModel>
                    <DirectEvents>
                        <ItemClick OnEvent="deLoadDollarStore" Before="return record.isLeaf()" />
                    </DirectEvents>
                    
                </ext:TreePanel>
                <ext:GridPanel runat="server" ID="uxDollarGrid" Title="Dollar Threshold" Region="North" PaddingSpec="10 10 30 10" MinHeight="250" SelectionMemory="false">
                    <Store>
                        <ext:Store runat="server" ID="uxDollarStore" AutoLoad="false" AutoDataBind="true" OnReadData="deReadDollars" PageSize="10">
                            <Model>
                                <ext:Model runat="server" Name="DollarThreshold" IDProperty="AMOUNT_ID" ClientIdProperty="PhantomId">
                                    <Fields>
                                        <ext:ModelField Name="AMOUNT_ID" />
                                        <ext:ModelField Name="TYPE_NAME" />
                                        <ext:ModelField Name="ORG_ID" />
                                        <ext:ModelField Name="ORG_HIER" />
                                        <ext:ModelField Name="LOW_DOLLAR_AMT" />
                                        <ext:ModelField Name="HIGH_DOLLAR_AMT" />
                                        <ext:ModelField Name="TYPE_ID" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="LOW_DOLLAR_AMT" Direction="ASC" />
                            </Sorters>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                        </ext:Store>
                    </Store>
                    <View>
                        <ext:GridView ID="uxDollarView" runat="server" />
                    </View>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="Organization Name" DataIndex="ORG_HIER" Flex="50" />
                            <ext:Column runat="server" Text="Lower Amount" DataIndex="LOW_DOLLAR_AMT" Flex="25" AllowBlank="false" InvalidCls="allowBlank">
                                <Renderer Format="UsMoney" />
                                <Editor>
                                    <ext:NumberField runat="server" ID="uxLowDollarField" AllowBlank="false" InvalidCls="allowBlank" MinValue="1" Vtype="numberrange" EndNumberField="uxHighDollarField" />
                                </Editor>
                            </ext:Column>
                            <ext:Column runat="server" Text="Upper Amount" DataIndex="HIGH_DOLLAR_AMT" Flex="25" AllowBlank="false" InvalidCls="allowBlank">
                                <Renderer Format="UsMoney" />
                                <Editor>
                                    <ext:NumberField runat="server" ID="uxHighDollarField" AllowBlank="false" InvalidCls="allowBlank" MinValue="1" Vtype="numberrange" StartNumberField="uxLowDollarField" />
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column1" runat="server" Text="Form Target" DataIndex="TYPE_ID" AllowBlank="false" InvalidCls="allowBlank">
                                <Editor>
                                    <ext:ComboBox runat="server" ID="uxFormTypeCombo" DisplayField="TYPE_NAME" ValueField="TYPE_ID" AllowBlank="false" InvalidCls="allowBlank">
                                        <Store>
                                            <ext:Store runat="server" ID="uxFormTypeStore" OnReadData="deReadFormTypes" AutoDataBind="true">
                                                <Model>
                                                    <ext:Model ID="Model1" runat="server" IDProperty="TYPE_ID">
                                                        <Fields>
                                                            <ext:ModelField Name="TYPE_ID" />
                                                            <ext:ModelField Name="TYPE_NAME" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>
                                </Editor>
                                <Renderer Fn="formTypeRenderer" />
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                        <ext:RowEditing runat="server" ID="uxDollarRowEdit" ClicksToEdit="2" ClicksToMoveEditor="1" AutoCancel="false" ErrorSummary="false">
                            <Listeners>
                                <CancelEdit Handler="cancelEditRow('dollar')" />
                                <BeforeEdit Handler="if(onBeforeEdit('dollar')){
                                    #{uxAddDollarButton}.disable();
                                    return true;
                                    }
                                    else{
                                    return false;
                                    }" />
                            </Listeners>
                            <DirectEvents>
                                <Edit OnEvent="deSaveDollar" Before="return #{uxDollarStore}.isDirty()">
                                    <ExtraParams>
                                        <ext:Parameter Name="data" Value="#{uxDollarStore}.getChangedData({skipIdForPhantomRecords : false})" Mode="Raw" Encode="true" />
                                    </ExtraParams>
                                </Edit>
                            </DirectEvents>
                        </ext:RowEditing>
                    </Plugins>
                    <Listeners>
                        <Select Handler="#{uxDeleteDollarButton}.enable(); 
                            if(!#{uxThresholdRowEdit}.editing){
                            #{uxAddThresholdButton}.enable(); 
                            #{uxThresholdStore}.reload();
                            #{uxDeleteThresholdButton}.disable();
                            }" />
                    </Listeners>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxAddDollarButton" Icon="ApplicationAdd" Text="Add" Disabled="true">
                                    <Listeners>
                                        <Click Fn="AddDollarThreshold" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxDeleteDollarButton" Icon="ApplicationDelete" Text="Delete" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deDeleteDollar">
                                            <EventMask ShowMask="true" />
                                            <ExtraParams>
                                                <ext:Parameter Name="AmountId" Value="#{uxDollarGrid}.getSelectionModel().getSelection()[0].data.AMOUNT_ID" Mode="Raw" />
                                            </ExtraParams>
                                            <Confirmation ConfirmRequest="true" Title="Really Delete?" Message="Do you really want to delete this entry?" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <BottomBar>
                        <ext:PagingToolbar runat="server" />
                    </BottomBar>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" Mode="Single" ID="uxDollarSelection" />
                    </SelectionModel>
                </ext:GridPanel>
                <ext:GridPanel runat="server" ID="uxThresholdGrid" Region="Center" Title="Threshold Percentages" PaddingSpec="10 10 30 10" MinHeight="250" SelectionMemory="false">
                    <Store>
                        <ext:Store runat="server" ID="uxThresholdStore" AutoDataBind="true" AutoLoad="false" OnReadData="deReadThresholds">
                            <Model>
                                <ext:Model runat="server" Name="Threshold" IDProperty="THRESHOLD_ID" ClientIdProperty="PhantomId">
                                    <Fields>
                                        <ext:ModelField Name="AMOUNT_ID" DefaultValue="0" />
                                        <ext:ModelField Name="LOW_DOLLAR" ServerMapping="CUSTOMER_SURVEY_THRESH_AMT.LOW_DOLLAR_AMT" />
                                        <ext:ModelField Name="HIGH_DOLLAR" ServerMapping="CUSTOMER_SURVEY_THRESH_AMT.HIGH_DOLLAR_AMT" />
                                        <ext:ModelField Name="THRESHOLD" />
                                        <ext:ModelField Name="THRESHOLD_ID" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Parameters>
                                <ext:StoreParameter Name="AmountId" Value="#{uxDollarGrid}.getSelectionModel().getSelection()[0].data.AMOUNT_ID" Mode="Raw" />
                            </Parameters>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                        </ext:Store>
                    </Store>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" Mode="Single" ID="uxThresholdSelection" />
                    </SelectionModel>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="Low Dollar" DataIndex="LOW_DOLLAR" />
                            <ext:Column runat="server" Text="High Dollar" DataIndex="HIGH_DOLLAR" />
                            <ext:Column runat="server" Text="% Threshold" DataIndex="THRESHOLD">
                                <Editor>
                                    <ext:NumberField runat="server" AllowBlank="false" InvalidCls="allowBlank" MinValue="1" />
                                </Editor>
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxAddThresholdButton" Text="Add" Icon="ApplicationAdd" Disabled="true">
                                    <Listeners>
                                        <Click Fn="AddThreshold" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxDeleteThresholdButton" Text="Delete" Icon="ApplicationDelete" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deDeleteThreshold">
                                            <EventMask ShowMask="true" />
                                            <Confirmation Title="Really Delete?" Message="Do you really want to delete?" ConfirmRequest="true" />
                                            <ExtraParams>
                                                <ext:Parameter Name="ThresholdId" Value="#{uxThresholdGrid}.getSelectionModel().getSelection()[0].data.THRESHOLD_ID" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Listeners>
                        <Select Handler="#{uxDeleteThresholdButton}.enable()" />
                    </Listeners>
                    <Plugins>
                        <ext:RowEditing runat="server" ID="uxThresholdRowEdit" ClicksToEdit="2" ClicksToMoveEditor="1" ErrorSummary="false" AutoCancel="false">
                            <Listeners>
                                <BeforeEdit Handler="if(onBeforeEdit('threshold')){
                                    #{uxAddThresholdButton}.disable();
                                    return true;
                                    }
                                    else{
                                    return false;
                                    }" />
                                <CancelEdit Handler="cancelEditRow('threshold')" />
                            </Listeners>
                            <DirectEvents>
                                <Edit OnEvent="deSaveThreshold" Before="return #{uxThresholdStore}.isDirty()">
                                    <ExtraParams>
                                        <ext:Parameter Name="data" Value="#{uxThresholdStore}.getChangedData({skipIdForPhantomRecords : false})" Mode="Raw" Encode="true" />
                                        <ext:Parameter Name="amountId" Value="#{uxDollarGrid}.getSelectionModel().getSelection()[0].data.AMOUNT_ID" Mode="Raw" />
                                    </ExtraParams>
                                </Edit>
                            </DirectEvents>
                        </ext:RowEditing>
                    </Plugins>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
        <ext:Window runat="server" ID="uxAddEditDollarWindow" Hidden="true" Modal="true" Title="Add Dollar Amounts" Width="350">
            <Items>
                <ext:FormPanel runat="server" ID="uxDollarForm" Layout="VBoxLayout">
                    <LayoutConfig>
                        <ext:VBoxLayoutConfig Align="Stretch" />
                    </LayoutConfig>
                    <Items>
                        <ext:Hidden runat="server" ID="uxDollarFormType" />
                        <ext:Hidden runat="server" ID="uxDollarAmountId" />
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxSaveDollarButton" Text="Submit" Icon="Add">
                            <DirectEvents>
                                <Click OnEvent="deSaveDollar">
                                    <EventMask ShowMask="true" />
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="uxCancelDollarButton" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="#{uxAddEditDollarWindow}.hide(); #{uxDollarForm}.reset()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <Listeners>
                        <ValidityChange Handler="#{uxSaveDollarButton}.setDisabled(!valid)" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Window>
        <ext:Window runat="server" ID="uxAddEditThresholdWindow" Hidden="true" Modal="true" Title="Add Percentage" Width="250">
            <Items>
                <ext:FormPanel runat="server" ID="uxThresholdForm" Layout="FormLayout">
                    <Items>
                        <ext:Hidden runat="server" ID="uxThresholdFormType" />
                        <ext:Hidden runat="server" ID="uxThresholdId" />
                        <ext:NumberField runat="server" ID="uxThreshold" FieldLabel="Threshold in %" AllowBlank="false" MinValue="1" MaxValue="100" AllowDecimals="false" AllowExponential="false" InvalidCls="allowBlank" />
                        <ext:Checkbox runat="server" ID="uxRandomCheckbox" LabelAlign="Right" FieldLabel="Random" />
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxSaveThresholdButton" Text="Submit" Icon="Add" Disabled="true">
                            <DirectEvents>
                                <Click OnEvent="deSaveThreshold">
                                    <EventMask ShowMask="true" />
                                    <ExtraParams>
                                        <ext:Parameter Name="AmountId" Value="#{uxDollarGrid}.getSelectionModel().getSelection()[0].data.AMOUNT_ID" Mode="Raw" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="uxCancelThresholdButton" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="#{uxAddEditThresholdWindow}.hide(); #{uxThresholdForm}.reset()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <Listeners>
                        <ValidityChange Handler="#{uxSaveThresholdButton}.setDisabled(!valid)" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>

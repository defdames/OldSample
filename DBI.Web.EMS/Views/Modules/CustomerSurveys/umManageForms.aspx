<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umManageForms.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umCustomerSurveyAdmin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var OrgRenderer = function (value) {
            var r = App.uxAddFormOrgStore.getById(value);
            if (Ext.isEmpty(r)) {
                return "";
            }
            return r.data.ORG_HIER;
        };

        var CatRenderer = function (value) {
            var r = App.uxAddFormCatStore.getById(value);
            if (Ext.isEmpty(r)) {
                return "";
            }
            return r.data.NAME;
        };

        var TypeRenderer = function (value) {
            var r = App.uxQuestionTypeStore.getById(value);
            if (Ext.isEmpty(r)) {
                return "";
            }
            return r.data.QUESTION_TYPE_NAME;
        };

        var FormTypeRenderer = function (value) {
            var r = App.uxFormTypeStore.getById(value);
            if (Ext.isEmpty(r)) {
                return "";
            }
            return r.data.TYPE_NAME;
        };

        var FieldsetRenderer = function (value) {
            var r = App.uxQuestionFieldsetStore.getById(value);
            if (Ext.isEmpty(r)) {
                return "";
            }
            return r.data.TITLE;
        };

        var FieldsetCatRenderer = function (value) {
            var r = App.uxQuestionCategoryStore.getById(value);
            if (Ext.isEmpty(r)) {
                return "";
            }
            return r.data.CATEGORY_NAME;
        };

        var AddOption = function () {
            var Option = new QuestionOption({
                TEXT: App.uxQuestionsGrid.getSelectionModel().getSelection()[0].data.TEXT
            });
            App.uxOptionsStore.insert(0, Option);
        };
    </script>
    <style type="text/css">
        .allowBlank-field {
            background-color: #EFF7FF !important;
            background-image: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" IsDynamic="false" />


        <ext:Viewport ID="uxAdminViewPort" runat="server" Layout="FitLayout">
            <Items>
                <ext:TabPanel runat="server" ID="uxTabPanel">
                    <Items>
                        <ext:Panel runat="server" ID="uxFormsTab" Title="Manage Forms" Layout="AutoLayout" AutoScroll="true">
                            <Items>
                                <ext:GridPanel runat="server" ID="uxFormsGrid" Title="Forms" MaxWidth="1100" Margin="5" MinHeight="250" SelectionMemory="true">
                                    <SelectionModel>
                                        <ext:RowSelectionModel Mode="Single" />
                                    </SelectionModel>
                                    <Store>
                                        <ext:Store runat="server" ID="uxFormsStore" AutoDataBind="true" OnReadData="deReadForms" RemoteSort="true" PageSize="10">
                                            <Model>
                                                <ext:Model ID="Model2" runat="server" Name="Form" IDProperty="FORM_ID" ClientIdProperty="PhantomId">
                                                    <Fields>
                                                        <ext:ModelField Name="FORM_ID" Type="Int" />
                                                        <ext:ModelField Name="FORMS_NAME" Type="String" />
                                                        <ext:ModelField Name="CATEGORY_ID" />
                                                        <ext:ModelField Name="ORG_ID" />
                                                        <ext:ModelField Name="NUM_QUESTIONS" Type="Int" DefaultValue="0" />
                                                        <ext:ModelField Name="TYPE_ID" Type="Int" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                            <Sorters>
                                                <ext:DataSorter Property="FORMS_NAME" Direction="ASC" />
                                            </Sorters>
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel runat="server">
                                        <Columns>
                                            <ext:Column runat="server" DataIndex="FORMS_NAME" Text="Form Name" Flex="1">
                                                <Editor>
                                                    <ext:TextField runat="server" AllowBlank="false" EmptyText="Form Name" InvalidCls="allowBlank" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column runat="server" DataIndex="ORG_ID" Text="Organization Name" Flex="1">
                                                <Renderer Fn="OrgRenderer" />
                                                <Editor>
                                                    <ext:ComboBox runat="server" ForceSelection="true" TypeAhead="true" QueryMode="Local" ValueField="ORG_ID" DisplayField="ORG_HIER" AllowBlank="false" EmptyText="Choose Organization"  InvalidCls="allowBlank">
                                                        <Store>
                                                            <ext:Store runat="server" ID="uxAddFormOrgStore" OnReadData="deReadOrgs" AutoDataBind="true">
                                                                <Model>
                                                                    <ext:Model ID="Model3" runat="server" IDProperty="ORG_ID">
                                                                        <Fields>
                                                                            <ext:ModelField Name="ORG_ID" Type="Int" />
                                                                            <ext:ModelField Name="ORG_HIER" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                                <Proxy>
                                                                    <ext:PageProxy />
                                                                </Proxy>
                                                                <Listeners>
                                                                    <Load Handler="#{uxFormsGrid}.getView().refresh()" />
                                                                </Listeners>
                                                            </ext:Store>
                                                        </Store>
                                                    </ext:ComboBox>
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column runat="server" DataIndex="CATEGORY_ID" Text="Survey Category" Flex="1">
                                                <Renderer Fn="CatRenderer" />
                                                <Editor>
                                                    <ext:ComboBox runat="server" ForceSelection="true" TypeAhead="true" QueryMode="Local" ValueField="CATEGORY_ID" DisplayField="NAME" AllowBlank="false" EmptyText="Choose Category" InvalidCls="allowBlank">
                                                        <Store>
                                                            <ext:Store runat="server" ID="uxAddFormCatStore" OnReadData="deReadCategories" AutoDataBind="true">
                                                                <Model>
                                                                    <ext:Model runat="server" IDProperty="CATEGORY_ID">
                                                                        <Fields>
                                                                            <ext:ModelField Name="CATEGORY_ID" />
                                                                            <ext:ModelField Name="NAME" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                                <Proxy>
                                                                    <ext:PageProxy />
                                                                </Proxy>
                                                                <Listeners>
                                                                    <Load Handler="#{uxFormsGrid}.getView().refresh()" />
                                                                </Listeners>
                                                            </ext:Store>
                                                        </Store>
                                                    </ext:ComboBox>
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column runat="server" DataIndex="TYPE_ID" Text="Form Target Type" Flex="1">
                                                <Renderer Fn="FormTypeRenderer" />
                                                <Editor>
                                                    <ext:ComboBox runat="server" ID="uxFormTypeCombo" Editable="true" ForceSelection="false" TypeAhead="true" QueryMode="Local" ValueField="TYPE_ID" DisplayField="TYPE_NAME" AllowBlank="false" EmptyText="Choose Target Type" InvalidCls="allowBlank">
                                                        <Store>
                                                            <ext:Store runat="server" ID="uxFormTypeStore" OnReadData="deReadFormTypes" AutoDataBind="true">
                                                                <Model>
                                                                    <ext:Model runat="server" IDProperty="TYPE_ID">
                                                                        <Fields>
                                                                            <ext:ModelField Name="TYPE_ID" />
                                                                            <ext:ModelField Name="TYPE_NAME" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                                <Proxy>
                                                                    <ext:PageProxy />
                                                                </Proxy>
                                                                <Listeners>
                                                                    <Load Handler="#{uxFormsGrid}.getView().refresh()" />
                                                                </Listeners>
                                                            </ext:Store>
                                                        </Store>
                                                    </ext:ComboBox>
                                                </Editor>
                                            </ext:Column>
                                        </Columns>
                                    </ColumnModel>
                                    <Plugins>
                                        <ext:FilterHeader runat="server" Remote="true" />
                                        <ext:RowEditing runat="server" ClicksToMoveEditor="1" AutoCancel="false">
                                            <DirectEvents>
                                                <Edit OnEvent="deSaveForm" Before="return #{uxFormsStore}.isDirty();">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="data" Value="#{uxFormsStore}.getChangedData({skipIdForPhantomRecords : false})" Mode="Raw" Encode="true" />
                                                        <ext:Parameter Name="TypeName" Value="#{uxFormTypeCombo}.rawValue" Mode="Raw" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" />
                                                </Edit>
                                            </DirectEvents>
                                        </ext:RowEditing>
                                    </Plugins>
                                    <TopBar>
                                        <ext:Toolbar runat="server">
                                            <Items>
                                                <ext:Button runat="server" Text="Add Form" ID="uxAddFormButton" Icon="ApplicationAdd">
                                                    <Listeners>
                                                        <Click Handler="#{uxFormsStore}.insert(0, new Form());" />
                                                    </Listeners>
                                                </ext:Button>
                                                <ext:Button runat="server" Text="Delete Form" ID="uxDeleteFormButton" Icon="ApplicationDelete" Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="deDeleteForm">
                                                            <Confirmation Title="Really Delete?" Message="Do you really want to delete this form?" ConfirmRequest="true" />
                                                            <EventMask ShowMask="true" />
                                                            <ExtraParams>
                                                                <ext:Parameter Name="FormId" Value="#{uxFormsGrid}.getSelectionModel().getSelection()[0].data.FORM_ID" Mode="Raw" />
                                                            </ExtraParams>
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button runat="server" ID="uxViewFormButton" Text="View Form" Icon="ApplicationViewDetail" Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="deViewForm">
                                                            <ExtraParams>
                                                                <ext:Parameter Name="FormId" Value="#{uxFormsGrid}.getSelectionModel().getSelection()[0].data.FORM_ID" Mode="Raw" />
                                                            </ExtraParams>
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button runat="server" Text="Copy Existing" ID="uxCopyFormButton" Icon="PageCopy" Disabled="true">
                                                    <Listeners>
                                                        <Click Handler="#{uxCopyFormWindow}.show()" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
                                    <BottomBar>
                                        <ext:PagingToolbar runat="server" />
                                    </BottomBar>
                                    <DirectEvents>
                                        <Select OnEvent="deLoadFormDetails" />
                                    </DirectEvents>
                                    <Listeners>
                                        <Select Handler="#{uxDeleteFormButton}.enable(); #{uxViewFormButton}.enable(); #{uxCopyFormButton}.enable()" />
                                    </Listeners>
                                </ext:GridPanel>
                                <ext:GridPanel runat="server" Title="Fieldsets" ID="uxFieldsetsGrid" MaxWidth="1100" Margin="5" MinHeight="250" SelectionMemory="true">
                                    <SelectionModel>
                                        <ext:RowSelectionModel Mode="Single" />
                                    </SelectionModel>
                                    <Store>
                                        <ext:Store runat="server" ID="uxFieldsetsStore" AutoDataBind="true" AutoLoad="false" PageSize="5" RemoteSort="true" OnReadData="deReadFieldsets">
                                            <Model>
                                                <ext:Model ID="Model1" runat="server" Name="Fieldset" IDProperty="FIELDSET_ID" ClientIdProperty="PhantomId">
                                                    <Fields>
                                                        <ext:ModelField Name="FIELDSET_ID" Type="Int" />
                                                        <ext:ModelField Name="TITLE" Type="String" />
                                                        <ext:ModelField Name="CATEGORY_ID" Type="Int" />
                                                        <ext:ModelField Name="SORT_ORDER" Type="Int" />
                                                        <ext:ModelField Name="IS_ACTIVE" Type="Boolean" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Parameters>
                                                <ext:StoreParameter Name="FormId" Value="#{uxFormsGrid}.getSelectionModel().getSelection()[0].data.FORM_ID" Mode="Raw" />
                                            </Parameters>
                                            <Sorters>
                                                <ext:DataSorter Property="SORT_ORDER" Direction="ASC" />
                                            </Sorters>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel ID="ColumnModel1" runat="server">
                                        <Columns>
                                            <ext:Column ID="Column1" runat="server" DataIndex="TITLE" Text="Fieldset Name" Flex="30">
                                                <Editor>
                                                    <ext:TextField runat="server" EmptyText="Fieldset Name" AllowBlank="false" InvalidCls="allowBlank" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column ID="Column2" runat="server" DataIndex="SORT_ORDER" Text="Sort Order" Flex="30">
                                                <Editor>
                                                    <ext:NumberField runat="server" AllowBlank="false" InvalidCls="allowBlank" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column runat="server" DataIndex="CATEGORY_ID" Text="Category" Flex="30">
                                                <Renderer Fn="FieldsetCatRenderer" />
                                                <Editor>
                                                    <ext:ComboBox runat="server" ID="uxFieldsetCatCombo" Editable="true" ForceSelection="false" InvalidCls="allowBlank" TypeAhead="true" QueryMode="Local" ValueField="CATEGORY_ID" DisplayField="CATEGORY_NAME" AllowBlank="false" EmptyText="Select a Question Category">
                                                        <Store>
                                                            <ext:Store runat="server" ID="uxQuestionCategoryStore" OnReadData="deReadQuestionCategories" AutoDataBind="true">
                                                                <Model>
                                                                    <ext:Model runat="server" IDProperty="CATEGORY_ID">
                                                                        <Fields>
                                                                            <ext:ModelField Name="CATEGORY_ID" />
                                                                            <ext:ModelField Name="CATEGORY_NAME" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                                <Proxy>
                                                                    <ext:PageProxy />
                                                                </Proxy>
                                                                <Listeners>
                                                                    <Load Handler="#{uxFieldsetsGrid}.getView().refresh()" />
                                                                </Listeners>
                                                            </ext:Store>
                                                        </Store>
                                                    </ext:ComboBox>
                                                </Editor>
                                            </ext:Column>
                                            <ext:CheckColumn runat="server" DataIndex="IS_ACTIVE" Text="Active" Flex="10" Editable="true" />
                                        </Columns>
                                    </ColumnModel>
                                    <Plugins>
                                        <ext:FilterHeader runat="server" Remote="true" />
                                        <ext:RowEditing runat="server" ClicksToMoveEditor="1" AutoCancel="false">
                                            <DirectEvents>
                                                <Edit OnEvent="deSaveFieldsets" Before="return #{uxFieldsetsStore}.isDirty();">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="data" Value="#{uxFieldsetsStore}.getChangedData()" Mode="Raw" Encode="true" />
                                                        <ext:Parameter Name="FormId" Value="#{uxFormsGrid}.getSelectionModel().getSelection()[0].data.FORM_ID" Mode="Raw" />
                                                        <ext:Parameter Name="CategoryName" Value="#{uxFieldsetCatCombo}.rawValue" Mode="Raw" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" />
                                                </Edit>
                                            </DirectEvents>
                                        </ext:RowEditing>
                                    </Plugins>
                                    <BottomBar>
                                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
                                    </BottomBar>
                                    <TopBar>
                                        <ext:Toolbar runat="server">
                                            <Items>
                                                <ext:Button runat="server" ID="uxAddFieldsetButton" Icon="ApplicationAdd" Text="Add Fieldset" Disabled="true">
                                                    <Listeners>
                                                        <Click Handler="#{uxFieldSetsStore}.insert(0, new Fieldset());" />
                                                    </Listeners>
                                                </ext:Button>
                                                <ext:Button runat="server" ID="uxDeleteFieldsetButton" Icon="ApplicationDelete" Text="Delete Fieldset" Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="deDeleteFieldset">
                                                            <Confirmation ConfirmRequest="true" Title="Really Delete?" Message="Do you really want to delete this fieldset?" />
                                                            <EventMask ShowMask="true" />
                                                            <ExtraParams>
                                                                <ext:Parameter Name="FieldsetId" Value="#{uxFieldsetsGrid}.getSelectionModel().getSelection()[0].data.FIELDSET_ID" Mode="Raw" />
                                                            </ExtraParams>
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
                                    <Listeners>
                                        <Select Handler="#{uxDeleteFieldsetButton}.enable()" />
                                    </Listeners>

                                </ext:GridPanel>
                                <ext:GridPanel runat="server" ID="uxQuestionsGrid" Title="Form Questions" MaxWidth="1100" Margin="5" MinHeight="250">
                                    <Store>
                                        <ext:Store runat="server" ID="uxQuestionsStore" RemoteSort="true" PageSize="5" AutoDataBind="true" AutoLoad="false" OnReadData="deReadQuestions" WarningOnDirty="true">
                                            <Model>
                                                <ext:Model ID="Model6" runat="server" Name="Question" IDProperty="QUESTION_ID" ClientIdProperty="PhantomId">
                                                    <Fields>
                                                        <ext:ModelField Name="QUESTION_ID" Type="Int" />
                                                        <ext:ModelField Name="TEXT" Type="String" />
                                                        <ext:ModelField Name="QUESTION_TYPE_NAME" Type="String" />
                                                        <ext:ModelField Name="TYPE_ID" Type="Int" />
                                                        <ext:ModelField Name="FIELDSET_ID" Type="Int" />
                                                        <ext:ModelField Name="TITLE" Type="String" />
                                                        <ext:ModelField Name="IS_REQUIRED" Type="Boolean" />
                                                        <ext:ModelField Name="IS_ACTIVE" Type="Boolean" />
                                                        <ext:ModelField Name="SORT_ORDER" Type="Int" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Parameters>
                                                <ext:StoreParameter Name="FormId" Value="#{uxFormsGrid}.getSelectionModel().getSelection()[0].data.FORM_ID" Mode="Raw" />
                                            </Parameters>
                                            <Sorters>
                                                <ext:DataSorter Property="SORT_ORDER" Direction="ASC" />
                                            </Sorters>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel runat="server">
                                        <Columns>
                                            <ext:Column runat="server" DataIndex="TEXT" Text="Question Name" Flex="40">
                                                <Editor>
                                                    <ext:TextField runat="server" AllowBlank="false" EmptyText="Question Text" InvalidCls="allowBlank" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column runat="server" DataIndex="TYPE_ID" Text="Question Type" Flex="20">
                                                <Renderer Fn="TypeRenderer" />
                                                <Editor>
                                                    <ext:ComboBox runat="server" ForceSelection="true" TypeAhead="true" InvalidCls="allowBlank" QueryMode="Local" DisplayField="QUESTION_TYPE_NAME" ValueField="TYPE_ID" AllowBlank="false" EmptyText="Select a Question Type">
                                                        <Store>
                                                            <ext:Store runat="server" ID="uxQuestionTypeStore" OnReadData="deReadQuestionTypes" AutoDataBind="true">
                                                                <Model>
                                                                    <ext:Model runat="server" IDProperty="TYPE_ID">
                                                                        <Fields>
                                                                            <ext:ModelField Name="QUESTION_TYPE_NAME" />
                                                                            <ext:ModelField Name="TYPE_ID" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                                <Proxy>
                                                                    <ext:PageProxy />
                                                                </Proxy>
                                                                <Listeners>
                                                                    <Load Handler="#{uxQuestionsGrid}.getView().refresh()" />
                                                                </Listeners>
                                                            </ext:Store>
                                                        </Store>
                                                    </ext:ComboBox>
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column runat="server" DataIndex="FIELDSET_ID" Text="Fieldset" Flex="20">
                                                <Renderer Fn="FieldsetRenderer" />
                                                <Editor>
                                                    <ext:ComboBox runat="server" ForceSelection="true" InvalidCls="allowBlank" TypeAhead="true" QueryMode="Local" DisplayField="TITLE" ValueField="FIELDSET_ID" AllowBlank="false" EmptyText="Select a Fieldset">
                                                        <Store>
                                                            <ext:Store runat="server" ID="uxQuestionFieldsetStore" OnReadData="deReadQuestionFieldsets" AutoDataBind="true" AutoLoad="false">
                                                                <Model>
                                                                    <ext:Model runat="server" IDProperty="FIELDSET_ID">
                                                                        <Fields>
                                                                            <ext:ModelField Name="FIELDSET_ID" />
                                                                            <ext:ModelField Name="TITLE" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                                <Parameters>
                                                                    <ext:StoreParameter Name="FormId" Value="#{uxFormsGrid}.getSelectionModel().getSelection()[0].data.FORM_ID" Mode="Raw" />
                                                                </Parameters>
                                                                <Proxy>
                                                                    <ext:PageProxy />
                                                                </Proxy>
                                                                <Listeners>
                                                                    <Load Handler="#{uxQuestionsGrid}.getView().refresh()" />
                                                                </Listeners>
                                                            </ext:Store>
                                                        </Store>
                                                    </ext:ComboBox>
                                                </Editor>
                                            </ext:Column>
                                            <ext:CheckColumn runat="server" DataIndex="IS_REQUIRED" Text="Required" Editable="true" Flex="8" />
                                            <ext:CheckColumn runat="server" DataIndex="IS_ACTIVE" Text="Active" Editable="true" Flex="8" />
                                            <ext:Column runat="server" DataIndex="SORT_ORDER" Text="Sort Order" Flex="8">
                                                <Editor>
                                                    <ext:NumberField runat="server" AllowBlank="false" InvalidCls="allowBlank" />
                                                </Editor>
                                            </ext:Column>
                                        </Columns>
                                    </ColumnModel>
                                    <Plugins>
                                        <ext:FilterHeader runat="server" Remote="true" />
                                        <ext:RowEditing runat="server" ClicksToMoveEditor="1" AutoCancel="false">
                                            <DirectEvents>
                                                <Edit OnEvent="deSaveQuestions" Before="return #{uxQuestionsStore}.isDirty();">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="data" Value="#{uxQuestionsStore}.getChangedData()" Mode="Raw" Encode="true" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" />
                                                </Edit>
                                            </DirectEvents>
                                        </ext:RowEditing>
                                    </Plugins>
                                    <BottomBar>
                                        <ext:PagingToolbar runat="server" />
                                    </BottomBar>
                                    <DirectEvents>
                                        <Select OnEvent="deLoadOptions">
                                            <ExtraParams>
                                                <ext:Parameter Name="QuestionType" Value="#{uxQuestionsGrid}.getSelectionModel().getSelection()[0].data.TYPE_ID" Mode="Raw" />
                                            </ExtraParams>
                                        </Select>
                                    </DirectEvents>
                                    <TopBar>
                                        <ext:Toolbar runat="server">
                                            <Items>
                                                <ext:Button runat="server" ID="uxAddQuestionButton" Icon="ApplicationAdd" Text="Add Question" Disabled="true">
                                                    <Listeners>
                                                        <Click Handler="#{uxQuestionsStore}.insert(0, new Question());" />
                                                    </Listeners>
                                                </ext:Button>
                                                <ext:Button runat="server" ID="uxDeleteQuestionButton" Icon="ApplicationDelete" Text="Delete Question" Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="deDeleteQuestion">
                                                            <Confirmation ConfirmRequest="true" Title="Really Delete" Message="Do you really want to delete this question?" />
                                                            <EventMask ShowMask="true" />
                                                            <ExtraParams>
                                                                <ext:Parameter Name="QuestionId" Value="#{uxQuestionsGrid}.getSelectionModel().getSelection()[0].data.QUESTION_ID" Mode="Raw" />
                                                            </ExtraParams>
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
                                    <Listeners>
                                        <Select Handler="#{uxDeleteQuestionButton}.enable()" />
                                    </Listeners>
                                </ext:GridPanel>
                                <ext:GridPanel runat="server" Title="Question Options" ID="uxOptionsGrid" MaxWidth="1100" Margin="5" MinHeight="250">
                                    <Store>
                                        <ext:Store runat="server" ID="uxOptionsStore" AutoDataBind="true" AutoLoad="false" PageSize="5" OnReadData="deReadOptions" RemoteSort="true" WarningOnDirty="true">
                                            <Model>
                                                <ext:Model runat="server" Name="QuestionOption" IDProperty="OPTION_ID" ClientIdProperty="PhantomId">
                                                    <Fields>
                                                        <ext:ModelField Name="OPTION_ID" Type="Int" />
                                                        <ext:ModelField Name="QUESTION_ID" Type="Int" />
                                                        <ext:ModelField Name="TEXT" Type="String" />
                                                        <ext:ModelField Name="OPTION_NAME" Type="String" />
                                                        <ext:ModelField Name="SORT_ORDER" Type="Int" />
                                                        <ext:ModelField Name="IS_ACTIVE" Type="Boolean" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Parameters>
                                                <ext:StoreParameter Name="QuestionId" Value="#{uxQuestionsGrid}.getSelectionModel().getSelection()[0].data.QUESTION_ID" Mode="Raw" />
                                            </Parameters>
                                            <Sorters>
                                                <ext:DataSorter Property="SORT_ORDER" Direction="ASC" />
                                            </Sorters>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel runat="server">
                                        <Columns>
                                            <ext:Column runat="server" DataIndex="TEXT" Text="Question" Flex="40" />
                                            <ext:Column runat="server" DataIndex="OPTION_NAME" Text="Option Name" Flex="30">
                                                <Editor>
                                                    <ext:TextField runat="server" AllowBlank="false" EmptyText="Option Name" InvalidCls="allowBlank" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column runat="server" DataIndex="SORT_ORDER" Text="Sort Order" Flex="20">
                                                <Editor>
                                                    <ext:NumberField runat="server" AllowBlank="false" InvalidCls="allowBlank" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:CheckColumn runat="server" DataIndex="IS_ACTIVE" Text="Active" Editable="true" Flex="10" />
                                        </Columns>
                                    </ColumnModel>
                                    <Plugins>
                                        <ext:FilterHeader runat="server" Remote="true" />
                                        <ext:RowEditing runat="server" AutoCancel="false" ClicksToMoveEditor="1">
                                            <DirectEvents>
                                                <Edit OnEvent="deSaveOptions" Before="return #{uxOptionsStore}.isDirty();">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="data" Value="#{uxOptionsStore}.getChangedData()" Mode="Raw" Encode="true" />
                                                        <ext:Parameter Name="QuestionId" Value="#{uxQuestionsGrid}.getSelectionModel().getSelection()[0].data.QUESTION_ID" Mode="Raw" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" />
                                                </Edit>
                                            </DirectEvents>
                                        </ext:RowEditing>
                                    </Plugins>
                                    <BottomBar>
                                        <ext:PagingToolbar runat="server" />
                                    </BottomBar>
                                    <TopBar>
                                        <ext:Toolbar runat="server">
                                            <Items>
                                                <ext:Button ID="uxAddOptionButton" runat="server" Icon="ApplicationAdd" Text="Add Option" Disabled="true">
                                                    <Listeners>
                                                        <Click Fn="AddOption" />
                                                    </Listeners>
                                                </ext:Button>
                                                <ext:Button ID="uxDeleteOptionButton" runat="server" Icon="ApplicationDelete" Text="Delete Option" Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="deDeleteOption">
                                                            <Confirmation ConfirmRequest="true" Title="Really Delete" Message="Do you really want to delete this option" />
                                                            <EventMask ShowMask="true" />
                                                            <ExtraParams>
                                                                <ext:Parameter Name="OptionId" Value="#{uxOptionsGrid}.getSelectionModel().getSelection()[0].data.OPTION_ID" Mode="Raw" />
                                                            </ExtraParams>
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
                                    <Listeners>
                                        <Select Handler="#{uxDeleteOptionButton}.enable()" />
                                    </Listeners>
                                </ext:GridPanel>
                            </Items>
                        </ext:Panel>
                    </Items>
                </ext:TabPanel>
                <ext:Window runat="server" ID="uxCopyFormWindow" Title="Copy Form" Hidden="true" Width="650">
                    <Items>
                        <ext:FormPanel runat="server" ID="uxCopyForm" Layout="FormLayout">
                            <Items>
                                <ext:TextField runat="server" ID="uxCopyFormName" FieldLabel="Form Name" />
                                <ext:ComboBox ID="uxCopyFormOrg" runat="server" ForceSelection="true" TypeAhead="true" QueryMode="Local" ValueField="ORG_ID" DisplayField="ORG_HIER" AllowBlank="false" EmptyText="Choose Organization" FieldLabel="Organization">
                                    <Store>
                                        <ext:Store ID="uxCopyFormOrgStore" runat="server" OnReadData="deReadOrgs" AutoDataBind="true">
                                            <Model>
                                                <ext:Model ID="Model4" runat="server" IDProperty="ORG_ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ORG_ID" Type="Int" />
                                                        <ext:ModelField Name="ORG_HIER" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>
                                <ext:ComboBox ID="uxCopyFormCategory" runat="server" ForceSelection="true" TypeAhead="true" QueryMode="Local" ValueField="CATEGORY_ID" DisplayField="NAME" AllowBlank="false" EmptyText="Choose Category" FieldLabel="Category">
                                    <Store>
                                        <ext:Store runat="server" ID="uxCopyFormCategoryStore" OnReadData="deReadCategories" AutoDataBind="true">
                                            <Model>
                                                <ext:Model ID="Model5" runat="server" IDProperty="CATEGORY_ID">
                                                    <Fields>
                                                        <ext:ModelField Name="CATEGORY_ID" />
                                                        <ext:ModelField Name="NAME" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Listeners>
                                                <Load Handler="#{uxFormsGrid}.getView().refresh()" />
                                            </Listeners>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>
                                <ext:ComboBox runat="server" ID="uxCopyFormType" Editable="true" ForceSelection="false" TypeAhead="true" QueryMode="Local" ValueField="TYPE_ID" DisplayField="TYPE_NAME" AllowBlank="false" EmptyText="Choose Target Type" InvalidCls="allowBlank" FieldLabel="Form Type">
                                    <Store>
                                        <ext:Store runat="server" ID="uxCopyFormTypeStore" OnReadData="deReadFormTypes" AutoDataBind="true">
                                            <Model>
                                                <ext:Model ID="Model7" runat="server" IDProperty="TYPE_ID">
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
                            </Items>
                            <Buttons>
                                <ext:Button runat="server" ID="uxCopyFormSubmit" Text="Submit" Icon="Add">
                                    <DirectEvents>
                                        <Click OnEvent="deCopyForm">
                                            <ExtraParams>
                                                <ext:Parameter Name="FormId" Value="#{uxFormsGrid}.getSelectionModel().getSelection()[0].data.FORM_ID" Mode="Raw" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" Text="Cancel" Icon="Delete">
                                    <Listeners>
                                        <Click Handler="#{uxCopyFormWindow}.hide(); #{uxCopyForm}.reset()" />
                                    </Listeners>
                                </ext:Button>
                            </Buttons>
                        </ext:FormPanel>
                    </Items>
                </ext:Window>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

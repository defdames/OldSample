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

        var FieldsetRenderer = function (value) {
            var r = App.uxQuestionFieldsetStore.getById(value);
            if (Ext.isEmpty(r)) {
                return "";
            }
            return r.data.TITLE;
        };

        var AddOption = function () {
            var Option = new QuestionOption({
                TEXT: App.uxQuestionsGrid.getSelectionModel().getSelection()[0].data.TEXT
            });
            App.uxOptionsStore.insert(0, Option);
        };
    </script>
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
                                                    <ext:TextField runat="server" AllowBlank="false" EmptyText="Form Name" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column runat="server" DataIndex="ORG_ID" Text="Organization Name" Flex="1">
                                                <Renderer Fn="OrgRenderer" />
                                                <Editor>
                                                    <ext:ComboBox runat="server" ForceSelection="true" TypeAhead="true" QueryMode="Local" ValueField="ORG_ID" DisplayField="ORG_HIER" AllowBlank="false" EmptyText="Choose Organization">
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
                                                    <ext:ComboBox runat="server" ForceSelection="true" TypeAhead="true" QueryMode="Local" ValueField="CATEGORY_ID" DisplayField="NAME" AllowBlank="false" EmptyText="Choose Category">
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
                                        </Columns>
                                    </ColumnModel>
                                    <Plugins>
                                        <ext:FilterHeader runat="server" Remote="true" />
                                        <ext:RowEditing runat="server" ClicksToMoveEditor="1" AutoCancel="false">
                                            <DirectEvents>
                                                <Edit OnEvent="deSaveForm" Before="return #{uxFormsStore}.isDirty();">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="data" Value="#{uxFormsStore}.getChangedData({skipIdForPhantomRecords : false})" Mode="Raw" Encode="true" />
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
                                                <ext:Button runat="server" ID="uxViewFormButton" Text="View Form" Icon="ApplicationViewDetail">
                                                    <DirectEvents>
                                                        <Click OnEvent="deViewForm">
                                                            <ExtraParams>
                                                                <ext:Parameter Name="FormId" Value="#{uxFormsGrid}.getSelectionModel().getSelection()[0].data.FORM_ID" Mode="Raw" />
                                                            </ExtraParams>
                                                        </Click>
                                                    </DirectEvents>
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
                                </ext:GridPanel>
                                <ext:GridPanel runat="server" Title="Fieldsets" ID="uxFieldsetsGrid" MaxWidth="1100" Margin="5" MinHeight="250">
                                    <Store>
                                        <ext:Store runat="server" ID="uxFieldsetsStore" AutoDataBind="true" AutoLoad="false" PageSize="5" RemoteSort="true" OnReadData="deReadFieldsets" WarningOnDirty="true">
                                            <Model>
                                                <ext:Model ID="Model1" runat="server" Name="Fieldset" IDProperty="FIELDSET_ID" ClientIdProperty="PhantomId">
                                                    <Fields>
                                                        <ext:ModelField Name="FIELDSET_ID" Type="Int" />
                                                        <ext:ModelField Name="TITLE" Type="String" />
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
                                            <ext:Column ID="Column1" runat="server" DataIndex="TITLE" Text="Fieldset Name" Width="300">
                                                <Editor>
                                                    <ext:TextField runat="server" EmptyText="Fieldset Name" AllowBlank="false" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column ID="Column2" runat="server" DataIndex="SORT_ORDER" Text="Sort Order" Width="150">
                                                <Editor>
                                                    <ext:NumberField runat="server" AllowBlank="false" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:CheckColumn runat="server" DataIndex="IS_ACTIVE" Text="Active" Width="50" Editable="true" />
                                        </Columns>
                                    </ColumnModel>
                                    <Plugins>
                                        <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
                                        <ext:RowEditing runat="server" ClicksToMoveEditor="1" AutoCancel="false">
                                            <DirectEvents>
                                                <Edit OnEvent="deSaveFieldsets" Before="return #{uxFieldsetsStore}.isDirty();">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="data" Value="#{uxFieldsetsStore}.getChangedData()" Mode="Raw" Encode="true" />
                                                        <ext:Parameter Name="FormId" Value="#{uxFormsGrid}.getSelectionModel().getSelection()[0].data.FORM_ID" Mode="Raw" />
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
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
                                </ext:GridPanel>
                                <ext:GridPanel runat="server" ID="uxQuestionsGrid" Title="Form Questions" MaxWidth="1100" Margin="5" MinHeight="250">
                                    <Store>
                                        <ext:Store runat="server" ID="uxQuestionsStore" RemoteSort="true" PageSize="5" AutoDataBind="true" AutoLoad="false" OnReadData="deReadQuestions" WarningOnDirty="true">
                                            <Model>
                                                <ext:Model runat="server" Name="Question" IDProperty="QUESTION_ID" ClientIdProperty="PhantomId">
                                                    <Fields>
                                                        <ext:ModelField Name="QUESTION_ID" Type="Int" />
                                                        <ext:ModelField Name="TEXT" Type="String" />
                                                        <ext:ModelField Name="QUESTION_TYPE_NAME" Type="String" />
                                                        <ext:ModelField Name="TYPE_ID" Type="Int" />
                                                        <ext:ModelField Name="FIELDSET_ID" Type="Int" />
                                                        <ext:ModelField Name="TITLE" Type="String" />
                                                        <ext:ModelField Name="IS_REQUIRED" Type="Boolean" />
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
                                                    <ext:TextField runat="server" AllowBlank="false" EmptyText="Question Text" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column runat="server" DataIndex="TYPE_ID" Text="Question Type" Flex="20">
                                                <Renderer Fn="TypeRenderer" />
                                                <Editor>
                                                    <ext:ComboBox runat="server" ForceSelection="true" TypeAhead="true" QueryMode="Local" DisplayField="QUESTION_TYPE_NAME" ValueField="TYPE_ID" AllowBlank="false" EmptyText="Select a Question Type">
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
                                                    <ext:ComboBox runat="server" ForceSelection="true" TypeAhead="true" QueryMode="Local" DisplayField="TITLE" ValueField="FIELDSET_ID" AllowBlank="false" EmptyText="Select a Fieldset">
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
                                            <ext:CheckColumn runat="server" DataIndex="IS_REQUIRED" Text="Required" Editable="true" Flex="10" />
                                            <ext:Column runat="server" DataIndex="SORT_ORDER" Text="Sort Order" Flex="10">
                                                <Editor>
                                                    <ext:NumberField runat="server" AllowBlank="false" />
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
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
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
                                                    <ext:TextField runat="server" AllowBlank="false" EmptyText="Option Name" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column runat="server" DataIndex="SORT_ORDER" Text="Sort Order" Flex="20">
                                                <Editor>
                                                    <ext:NumberField runat="server" AllowBlank="false" />
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
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
                                </ext:GridPanel>
                            </Items>
                        </ext:Panel>
                    </Items>
                </ext:TabPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

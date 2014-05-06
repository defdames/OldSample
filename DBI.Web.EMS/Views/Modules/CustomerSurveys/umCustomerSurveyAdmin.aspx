<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCustomerSurveyAdmin.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umCustomerSurveyAdmin" %>

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
        <ext:Viewport ID="uxAdminViewPort" runat="server" Layout="BorderLayout">
            <Items>
                <ext:GridPanel runat="server" ID="uxFormsGrid" Title="Forms" Region="West" Width="400">
                    <Store>
                        <ext:Store runat="server" ID="uxFormsStore" AutoDataBind="true" OnReadData="deReadForms" RemoteSort="true" PageSize="25" WarningOnDirty="true">
                            <Model>
                                <ext:Model runat="server" Name="Form" IDProperty="FORM_ID">
                                    <Fields>
                                        <ext:ModelField Name="FORM_ID" Type="Int" />
                                        <ext:ModelField Name="FORMS_NAME" Type="String" />
                                        <ext:ModelField Name="ORG_ID" />
                                        <ext:ModelField Name="NUM_QUESTIONS" Type="Int" DefaultValue="0" />
                                    </Fields>
                                </ext:Model>
                            </Model>
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
                                                    <ext:Model runat="server" IDProperty="ORG_ID">
                                                        <Fields>
                                                            <ext:ModelField Name="ORG_ID" />
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
                                </Editor>
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                        <ext:CellEditing runat="server" />
                    </Plugins>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" Text="Add Form" ID="uxAddFormButton" Icon="ApplicationAdd">
                                    <Listeners>
                                        <Click Handler="#{uxFormsStore}.insert(0, new Form());" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="Button1" runat="server" Text="Save" Icon="Add">
                                    <DirectEvents>
                                        <Click OnEvent="deSaveForms" Before="#{uxFormsStore}.isDirty()" >
                                            <ExtraParams>
                                                <ext:Parameter Name="data" Value="#{uxFormsStore}.getChangedData()" Mode="Raw" Encode="true" />
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
                <ext:Panel runat="server" Title="Form Information" ID="uxBottomPanel" Region="Center" Layout="AutoLayout">
                    <Items>
                        <ext:GridPanel runat="server" Title="Fieldsets" ID="uxFieldsetsGrid" MaxWidth="1000" Margin="5">
                            <Store>
                                <ext:Store runat="server" ID="uxFieldsetsStore" AutoDataBind="true" AutoLoad="false" PageSize="5" RemoteSort="true" OnReadData="deReadFieldsets" WarningOnDirty="true">
                                    <Model>
                                        <ext:Model ID="Model1" runat="server" Name="Fieldset" IDProperty="FIELDSET_ID">
                                            <Fields>
                                                <ext:ModelField Name="FIELDSET_ID" Type="Int" />
                                                <ext:ModelField Name="TITLE" Type="String" />
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
                                </Columns>
                            </ColumnModel>
                            <Plugins>
                                <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
                                <ext:CellEditing runat="server" />
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
                                        <ext:Button runat="server" ID="uxSaveFieldsetButton" Icon="Add" Text="Save" Disabled="true">
                                            <DirectEvents>
                                                <Click OnEvent="deSaveFieldsets" Before="#{uxFieldsetsStore}.isDirty()">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="FormId" Value="#{uxFormsGrid}.getSelectionModel().getSelection()[0].data.FORM_ID" Mode="Raw" />
                                                        <ext:Parameter Name="data" Value="#{uxFieldsetsStore}.getChangedData()" Mode="Raw" Encode="true" />
                                                    </ExtraParams>
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                        </ext:GridPanel>
                        <ext:GridPanel runat="server" ID="uxQuestionsGrid" Title="Form Questions" MaxWidth="1000" Margin="5">
                            <Store>
                                <ext:Store runat="server" ID="uxQuestionsStore" RemoteSort="true" PageSize="10" AutoDataBind="true" AutoLoad="false" OnReadData="deReadQuestions" WarningOnDirty="true">
                                    <Model>
                                        <ext:Model runat="server" Name="Question" IDProperty="QUESTION_ID">
                                            <Fields>
                                                <ext:ModelField Name="QUESTION_ID" UseNull="true" />
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
                                </ext:Store>
                            </Store>
                            <ColumnModel runat="server">
                                <Columns>
                                    <ext:Column runat="server" DataIndex="TEXT" Text="Question Name" Flex="40">
                                        <Editor>
                                            <ext:TextField runat="server" AllowBlank="false" EmptyText="Question Text" />
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column runat="server" DataIndex="TYPE_ID" Text="Question Type" Flex="20" AllowBlank="false">
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
                                <ext:CellEditing runat="server" />
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
                                        <ext:Button runat="server" ID="uxSaveQuestionButton" Icon="Add" Text="Save" Disabled="true">
                                            <DirectEvents>
                                                <Click OnEvent="deSaveQuestions" Before="#{uxQuestionsStore}.isDirty()">
                                                    <EventMask ShowMask="true" />
                                                    <ExtraParams>
                                                        <ext:Parameter Name="data" Value="#{uxQuestionsStore}.getChangedData()" Mode="Raw" Encode="true" />
                                                    </ExtraParams>
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                        </ext:GridPanel>
                        <ext:GridPanel runat="server" Title="Question Options" ID="uxOptionsGrid" MaxWidth="1000" Margin="5">
                            <Store>
                                <ext:Store runat="server" ID="uxOptionsStore" AutoDataBind="true" AutoLoad="false" PageSize="5" OnReadData="deReadOptions" RemoteSort="true" WarningOnDirty="true">
                                    <Model>
                                        <ext:Model runat="server" Name="QuestionOption" IDProperty="OPTION_ID">
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
                                    <Parameters>
                                        <ext:StoreParameter Name="QuestionId" Value="0" ApplyMode="IfNotExists" />
                                    </Parameters>
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
                                <ext:CellEditing runat="server" />
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
                                        <ext:Button runat="server" Icon="Add" Text="Save" ID="uxSaveOptionButton" Disabled="true">
                                            <DirectEvents>
                                                <Click OnEvent="deSaveOptions" Before="#{uxOptionsStore}.isDirty()">
                                                    <EventMask ShowMask="true" />
                                                    <ExtraParams>
                                                        <ext:Parameter Name="data" Value="#{uxOptionsStore}.getChangedData()" Mode="Raw" Encode="true" />
                                                        <ext:Parameter Name="QuestionId" Value="#{uxQuestionsGrid}.getSelectionModel().getSelection()[0].data.QUESTION_ID" Mode="Raw" />
                                                    </ExtraParams>
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                        </ext:GridPanel>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

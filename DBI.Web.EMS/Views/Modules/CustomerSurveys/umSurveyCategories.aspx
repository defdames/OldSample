<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSurveyCategories.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umSurveyCategories" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../../../Resources/StyleSheets/main.css" />
    <script type="text/javascript">
        var cancelEditRow = function (value) {
            if (value == "formtype") {
                if (!App.uxCategoryGrid.getSelectionModel().getSelection()[0].data.CATEGORY_ID) {
                    App.uxCategoriesStore.remove(App.uxCategoryGrid.getSelectionModel().getSelection()[0]);
                }
            }
            else {
                if (!App.uxQuestionCategoryGrid.getSelectionModel().getSelection()[0].data.CATEGORY_ID) {
                    App.uxQuestionCategoryStore.remove(App.uxQuestionCategoryGrid.getSelectionModel().getSelection()[0]);
                }
            }
            App.direct.dmSubtractFromDirty();
        };

        var deleteQuestionCategory = function () {
            var QuestionCategoryRecord = App.uxQuestionCategoryGrid.getSelectionModel().getSelection();

            Ext.Msg.confirm('Really Delete?', 'Do you really want to delete this Question Category?', function (e) {
                if (e == 'yes') {
                    App.uxQuestionCategoryStore.remove(QuestionCategoryRecord);
                    if (QuestionCategoryRecord[0].data.CATEGORY_ID) {
                        App.direct.dmDeleteQuestionCategory(QuestionCategoryRecord[0].data.CATEGORY_ID);
                    }
                }
            });
        };

        var deleteFormType = function () {
            var FormTypeRecord = App.uxCategoryGrid.getSelectionModel().getSelection();

            Ext.Msg.confirm('Really Delete?', 'Do you really want to delete this employee?', function (e) {
                if (e == 'yes') {
                    App.uxCategorieStore.remove(FormTypeRecord);
                    if (FormTypeRecord[0].data.CATEGORY_ID) {
                        App.direct.dmDeleteFormType(FormTypeRecord[0].data.CATEGORY_ID);
                    }
                }
            });
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager runat="server" IsDynamic="false" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:GridPanel runat="server" ID="uxCategoryGrid" Title="Form Types" Region="Center">
                    <Store>
                        <ext:Store runat="server" ID="uxCategoriesStore" OnReadData="deReadCategories" AutoDataBind="true" PageSize="10" RemoteSort="true">
                            <Model>
                                <ext:Model runat="server" Name="FormType" IDProperty="CATEGORY_ID" ClientIdProperty="PhantomId">
                                    <Fields>
                                        <ext:ModelField Name="CATEGORY_ID" Type="Int" />
                                        <ext:ModelField Name="NAME" Type="String" />
                                        <ext:ModelField Name="DESCRIPTION" Type="String" />
                                        <ext:ModelField Name="NUM_FORMS" Type="Int" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Sorters>
                                <ext:DataSorter Property="NAME" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="Category Name" DataIndex="NAME" Flex="25">
                                <Editor>
                                    <ext:TextField runat="server" AllowBlank="false" InvalidCls="allowBlank" />
                                </Editor>
                            </ext:Column>
                            <ext:Column runat="server" Text="Category Description" DataIndex="DESCRIPTION" Flex="50">
                                <Editor>
                                    <ext:TextField runat="server" AllowBlank="false" InvalidCls="allowBlank" />
                                </Editor>
                            </ext:Column>
                            <ext:Column runat="server" Text="Number of Forms" DataIndex="NUM_FORMS" Flex="25" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                        <ext:RowEditing runat="server" ClicksToEdit="1" AutoCancel="false" ClicksToMoveEditor="1" ErrorSummary="false" ID="uxFormTypeRowEdit">
                            <Listeners>
                                <CancelEdit Handler="cancelEditRow('formtype')" />
                                <BeforeEdit Handler="App.direct.dmAddToDirty()" />
                            </Listeners>
                            <DirectEvents>
                                <Edit OnEvent="deSaveCategory" >
                                    <ExtraParams>
                                        <ext:Parameter Name="data" Value="#{uxCategoriesStore}.getChangedData({skipIdForPhantomRecords : false})" Mode="Raw"  Encode="true" />
                                    </ExtraParams>
                                </Edit>
                            </DirectEvents>
                        </ext:RowEditing>
                    </Plugins>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxAddCategoryButton" Text="Add" Icon="ApplicationAdd">
                                    <Listeners>
                                        <Click Handler="#{uxCategoriesStore}.insert(0, new FormType()); #{uxFormTypeRowEdit}.startEdit(0, 0);
                                                            // Create DelayedTask and call it after 100 ms
                                                            var task = new Ext.util.DelayedTask(function(){
                                                            #{uxCategoryGrid}.columns[0].getEditor().focusInput();
                                                            });
                                                            task.delay(100);" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxDeleteCategoryButton" Text="Delete" Icon="ApplicationDelete" Disabled="true">
                                    <Listeners>
                                        <Click Fn="deleteFormType" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <BottomBar>
                        <ext:PagingToolbar runat="server" />
                    </BottomBar>
                    <Listeners>
                        <Select Handler="#{uxDeleteCategoryButton}.enable()" />
                    </Listeners>
                </ext:GridPanel>
                <ext:GridPanel ID="uxQuestionCategoryGrid" runat="server" Title="Question Categories" Region="North">
                    <Store>
                        <ext:Store runat="server" ID="uxQuestionCategoryStore" OnReadData="deReadQuestionCategories" PageSize="10" AutoDataBind="true" RemoteSort="true">
                            <Model>
                                <ext:Model runat="server" Name="QuestionCategory" IDProperty="CATEGORY_ID" ClientIdProperty="PhantomId">
                                    <Fields>
                                        <ext:ModelField Name="CATEGORY_ID" Type="Int" />
                                        <ext:ModelField Name="CATEGORY_NAME" />
                                        <ext:ModelField Name="NUM_QUESTIONS" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Sorters>
                                <ext:DataSorter Property="CATEGORY_NAME" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" DataIndex="CATEGORY_NAME" Text="Name" Flex="1">
                                <Editor>
                                    <ext:TextField runat="server" AllowBlank="false" InvalidCls="allowBlank" />
                                </Editor>
                            </ext:Column>
                            <ext:Column runat="server" DataIndex="NUM_QUESTIONS" Text="Number of Questions" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                        <ext:RowEditing runat="server" ID="uxQuestionCategoryRowEdit" AutoCancel="false" ClicksToEdit="1" ClicksToMoveEditor="1" ErrorSummary="false">
                            <Listeners>
                                <CancelEdit Handler="cancelEditRow('questioncat')" />
                                <BeforeEdit Handler="App.direct.dmAddToDirty()" />
                            </Listeners>
                            <DirectEvents>
                                <Edit OnEvent="deSaveQuestionCategory" Before="return #{uxQuestionCategoryStore}.isDirty();">
                                    <ExtraParams>
                                        <ext:Parameter Name="data" Value="#{uxQuestionCategoryStore}.getChangedData({skipIdForPhantomRecords : false})" Mode="Raw"  Encode="true" />
                                    </ExtraParams>
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
                                <ext:Button runat="server" ID="uxAddQuestionCategoryButton" Text="Add" Icon="ApplicationAdd">
                                    <Listeners>
                                        <Click Handler="#{uxQuestionCategoryStore}.insert(0, new QuestionCategory()); #{uxQuestionCategoryRowEdit}.startEdit(0, 0);
                                                            // Create DelayedTask and call it after 100 ms
                                                            var task = new Ext.util.DelayedTask(function(){
                                                            #{uxQuestionCategoryGrid}.columns[0].getEditor().focusInput();
                                                            });
                                                            task.delay(100);" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxDeleteQuestionCategoryButton" Text="Delete" Icon="ApplicationDelete" Disabled="true">
                                    <Listeners>
                                        <Click Fn="deleteQuestionCategory" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Listeners>
                        <Select Handler="#{uxDeleteQuestionCategoryButton}.enable()" />
                    </Listeners>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

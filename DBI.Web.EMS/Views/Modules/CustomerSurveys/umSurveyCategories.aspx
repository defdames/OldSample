<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSurveyCategories.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umSurveyCategories" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../../../Resources/StyleSheets/main.css" />
    <script type="text/javascript">

        var AddQuestionCategory = function () {
            App.uxQuestionCategoryStore.insert(0, new QuestionCategory());
            App.uxQuestionCategorySelection.select(0);
            var task = new Ext.util.DelayedTask(function () {
                App.uxQuestionCategoryRowEdit.startEdit(0, 0);
            });
            task.delay(100);

            // Create DelayedTask and call it after 100 ms
            task = new Ext.util.DelayedTask(function () {
                App.uxQuestionCategoryGrid.columns[0].getEditor().focusInput();
            });
            task.delay(400);
        };

        var AddFormCategory = function () {
            App.uxCategoriesStore.insert(0, new FormType());
            App.uxCategorySelection.select(0);
            var task = new Ext.util.DelayedTask(function () {
                App.uxFormTypeRowEdit.startEdit(0, 0);
            });
            task.delay(100);

            // Create DelayedTask and call it after 100 ms
            task = new Ext.util.DelayedTask(function () {
                App.uxCategoryGrid.columns[0].getEditor().focusInput();
            });
            task.delay(400);
        };

        var cancelEditRow = function (value) {
            if (value == "formtype") {
                if (!App.uxCategoryStore.getAt(0).data.CATEGORY_ID) {
                    App.uxCategoryStore.removeAt(0);
                }
                else {
                    App.uxDeleteCategoryButton.enable();
                    App.uxCategorySelection.setLocked(false);
                }
            }
            else {
                if (!App.uxQuestionCategoryStore.getAt(0).data.CATEGORY_ID) {
                    App.uxQuestionCategoryStore.removeAt(0);
                }
                else {
                    App.uxDeleteQuestionCategoryButton.enable();
                    App.uxQuestionCategorySelection.setLocked(false);
                }
            }
            checkEditing();
        };

        var deleteQuestionCategory = function () {
            var QuestionCategoryRecord = App.uxQuestionCategoryGrid.getSelectionModel().getSelection();

            Ext.Msg.confirm('Really Delete?', 'Do you really want to delete this Question Category?', function (e) {
                if (e == 'yes') {
                    if (QuestionCategoryRecord[0].data.CATEGORY_ID) {
                        App.direct.dmDeleteQuestionCategory(QuestionCategoryRecord[0].data.CATEGORY_ID);
                    }
                }
            });
        };

        var deleteFormType = function () {
            var FormTypeRecord = App.uxCategoryGrid.getSelectionModel().getSelection();

            Ext.Msg.confirm('Really Delete?', 'Do you really want to delete this Category?', function (e) {
                if (e == 'yes') {
                    if (FormTypeRecord[0].data.CATEGORY_ID) {
                        App.direct.dmDeleteCategory(FormTypeRecord[0].data.CATEGORY_ID);
                    }
                    else {
                        App.uxCategoriesStore.removeAt(0);
                    }
                }
            });
        };

        var onBeforeEdit = function (value) {
            if (value == "formtype") {
                if (App.uxFormTypeRowEdit.editing)
                    return false;
                else {
                    App.direct.dmSetDirty('true');
                    App.uxDeleteCategoryButton.disable();
                    App.uxCategorySelection.setLocked(true);
                    return true;
                }
            }
            else {
                if (App.uxQuestionCategoryRowEdit.editing)
                    return false;
                else {
                    App.direct.dmSetDirty('true');
                    App.uxDeleteQuestionCategoryButton.disable();
                    App.uxQuestionCategorySelection.setLocked(true);
                    return true;
                }
            }
        };

        var checkEditing = function () {
            if (App.uxFormTypeRowEdit.editing || App.uxQuestionCategoryRowEdit.editing) {
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
                <ext:GridPanel runat="server" ID="uxCategoryGrid" Title="Form Types" Region="Center">
                    <Store>
                        <ext:Store runat="server" ID="uxCategoriesStore" OnReadData="deReadCategories" AutoDataBind="true" PageSize="10" RemoteSort="true">
                            <Model>
                                <ext:Model runat="server" Name="FormType" IDProperty="CATEGORY_ID" ClientIdProperty="PhantomId">
                                    <Fields>
                                        <ext:ModelField Name="CATEGORY_ID" Type="Int" />
                                        <ext:ModelField Name="NAME" />
                                        <ext:ModelField Name="DESCRIPTION" />
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
                    <ColumnModel runat="server">
                        <Columns>
                            <ext:Column runat="server" ID="uxCategoryName" Text="Category Name" DataIndex="NAME" Flex="25">
                                <Editor>
                                    <ext:TextField runat="server" ID="uxCategoryNameField" AllowBlank="false" InvalidCls="allowBlank" />
                                </Editor>
                            </ext:Column>
                            <ext:Column runat="server" ID="uxCategoryDescription" Text="Category Description" DataIndex="DESCRIPTION" Flex="50">
                                <Editor>
                                    <ext:TextField runat="server" ID="uxCategoryDescriptionField" AllowBlank="false" InvalidCls="allowBlank" />
                                </Editor>
                            </ext:Column>
                            <ext:Column runat="server" Text="Number of Forms" DataIndex="NUM_FORMS" Flex="25" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                        <ext:RowEditing runat="server" AutoCancel="false" ErrorSummary="false" ID="uxFormTypeRowEdit">
                            <Listeners>
                                <CancelEdit Handler="cancelEditRow('formtype')" />
                                <BeforeEdit Handler="if(onBeforeEdit('formtype')){
                                    #{uxAddCategoryButton}.disable();
                                    return true;
                                    }
                                    else{
                                    return false;
                                    }" />
                            </Listeners>
                            <DirectEvents>
                                <Edit OnEvent="deSaveCategory">
                                    <ExtraParams>
                                        <ext:Parameter Name="data" Value="#{uxCategoriesStore}.getChangedData({skipIdForPhantomRecords : false})" Mode="Raw" Encode="true" />
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
                                        <Click Fn="AddFormCategory" />
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
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" Mode="Single" ID="uxCategorySelection" />
                    </SelectionModel>
                </ext:GridPanel>
                <ext:GridPanel ID="uxQuestionCategoryGrid" runat="server" Title="Question Categories" Region="North" Height="350">
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
                    <ColumnModel runat="server">
                        <Columns>
                            <ext:Column runat="server" ID="uxQuestionCategoryName" DataIndex="CATEGORY_NAME" Text="Name" Flex="1">
                                <Editor>
                                    <ext:TextField runat="server" ID="uxQuestionCategoryNameField" AllowBlank="false" InvalidCls="allowBlank" />
                                </Editor>
                            </ext:Column>
                            <ext:Column runat="server" DataIndex="NUM_QUESTIONS" Text="Number of Questions" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                        <ext:RowEditing runat="server" ID="uxQuestionCategoryRowEdit" AutoCancel="false" ErrorSummary="false">
                            <Listeners>
                                <CancelEdit Handler="cancelEditRow('questioncat')" />
                                <BeforeEdit Handler="if(onBeforeEdit('questioncat')){
                                    #{uxAddQuestionCategoryButton}.disable();
                                    return true;
                                    }
                                    else{
                                    return false;
                                    }" />
                            </Listeners>
                            <DirectEvents>
                                <Edit OnEvent="deSaveQuestionCategory" Before="return #{uxQuestionCategoryStore}.isDirty();">
                                    <ExtraParams>
                                        <ext:Parameter Name="data" Value="#{uxQuestionCategoryStore}.getChangedData({skipIdForPhantomRecords : false})" Mode="Raw" Encode="true" />
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
                                        <Click Fn="AddQuestionCategory" />
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
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" Mode="Single" ID="uxQuestionCategorySelection" />
                    </SelectionModel>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

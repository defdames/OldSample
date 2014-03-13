<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umManageForms.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umManageForms" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="false" />
        <ext:Panel Layout="AutoLayout" runat="server">
            <Items>
                <ext:Panel runat="server" ID="uxTopPanel" Flex="1">
                    <Items>
                <ext:GridPanel runat="server" ID="uxCurrentFormsGrid" 
                    Layout="HBoxLayout" Title="Select a Form">
                    <Store>
                        <ext:Store runat="server" ID="uxCurrentFormsStore" OnReadData="deReadCurrentForms" PageSize="10" RemoteSort="true">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="FORM_ID" />
                                        <ext:ModelField Name="FORMS_NAME" />
                                        <ext:ModelField Name="ORGANIZATION" />
                                        <ext:ModelField Name="NUM_QUESTIONS" />
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
                            <ext:Column runat="server" DataIndex="FORMS_NAME" Text="Form Name" />
                            <ext:Column runat="server" DataIndex="ORGANIZATION" Text="Organization" />
                            <ext:Column runat="server" DataIndex="NUM_QUESTIONS" Text="Number of Questions" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                    </Plugins>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxAddFormButton" Text="Add Form" Icon="ApplicationAdd">
                                    <Listeners>
                                        <Click Handler="#{uxAddFormWindow}.show()" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxEditFormButton" Text="Edit Form" Icon="ApplicationEdit">

                                </ext:Button>
                                <ext:ToolbarSeparator runat="server" />
                                <ext:Button runat="server" ID="uxAddFieldSetButton" Text="Add FieldSet" Icon="ApplicationAdd">
                                    <Listeners>
                                        <Click Handler="#{uxAddFieldSetWindow}.show()" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxEditFieldSetButton" Text="Edit FieldSet" Icon="ApplicationEdit">

                                </ext:Button>
                                <ext:ToolbarSeparator runat="server" />
                                <ext:Button runat="server" ID="uxPreviewFormButton" Text="Preview" Icon="PageWhiteGo">

                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" AllowDeselect="false" Mode="Single" />
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar runat="server" />
                    </BottomBar>
                    <DirectEvents>
                        <SelectionChange OnEvent="deLoadFieldSets">
                            <ExtraParams>
                                <ext:Parameter Name="FormId" Value="#{uxCurrentFormsGrid}.getSelectionModel().getSelection()[0].data.FORM_ID" Mode="Raw" />
                            </ExtraParams>
                        </SelectionChange>
                    </DirectEvents>
                </ext:GridPanel>
                    </Items>
                </ext:Panel>
                <ext:Panel runat="server" ID="uxMiddlePanel" Flex="1" Title="Fieldsets - Drag and Drop to Add/Remove">
                    <Items>
                        <ext:Panel runat="server" Layout="HBoxLayout" ID="uxFieldSetDragDropContainer">
                            <Items>
                                <ext:GridPanel ID="uxCurrentFieldSetsGrid" runat="server" Title="Current Fieldsets" Flex="1" Layout="HBoxLayout">
                                    <Store>
                                        <ext:Store runat="server" ID="uxCurrentFieldSetsStore" AutoDataBind="true">
                                            <Model>
                                                <ext:Model runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="FIELDSET_ID" />
                                                        <ext:ModelField Name="TITLE" />
                                                        <ext:ModelField Name="NUM_QUESTIONS" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column runat="server" Text="Fieldset" DataIndex="FIELDSET_ID" Flex="2"/>
                                            <ext:Column runat="server" Text="Title" DataIndex="TITLE" Flex="2" />
                                            <ext:Column runat="server" Text="Number of Questions" DataIndex="NUM_QUESTIONS" Flex="1" />
                                        </Columns>
                                    </ColumnModel>
                                </ext:GridPanel>
                                <ext:GridPanel ID="uxAvailableFieldSetsGrid" runat="server" Title="Available Field Sets" Flex="1" Layout="HBoxLayout">
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column runat="server" Text="FieldSet" Flex="2"/>
                                            <ext:Column runat="server" Text="Number of Questions" Flex="1" />
                                        </Columns>
                                    </ColumnModel>
                                </ext:GridPanel>
                            </Items>
                            <Buttons>
                                <ext:Button runat="server" ID="uxSaveFieldSetsButton" Text="Save" Icon="Add">

                                </ext:Button>
                                <ext:Button runat="server" ID="uxCancelFieldSetsButton" Text="Cancel" Icon="Delete">

                                </ext:Button>
                            </Buttons>
                        </ext:Panel>
                    </Items>
                </ext:Panel>
                <ext:Panel runat="server" ID="uxBottomPanel" Flex="1" Title="Questions - Drag and Drop to Add/Remove">
                    <Items>
                        <ext:Panel ID="uxQuestionDragDropContainer" runat="server" Layout="HBoxLayout">
                            <Items>
                                <ext:GridPanel ID="uxCurrentQuestionsGrid" runat="server" Title="Current Questions" Flex="1" Layout="HBoxLayout">
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column runat="server" Text="Question Name" Flex="2" />
                                            <ext:Column runat="server" Text="Question Type" Flex="1"/>
                                        </Columns>
                                    </ColumnModel>
                                </ext:GridPanel>
                                <ext:GridPanel ID="uxAvailableQuestionsGrid" runat="server" Title="Available Questions" Flex="1" Layout="HBoxLayout">
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column runat="server" Text="Question Name" Flex="2"/>
                                            <ext:Column runat="server" Text="Question Type" Flex="1"/>
                                        </Columns>
                                    </ColumnModel>
                                </ext:GridPanel>
                            </Items>
                            <Buttons>
                                <ext:Button runat="server" ID="uxSaveQuestionsButton" Text="Save" Icon="Add">

                                </ext:Button>
                                <ext:Button runat="server" ID="uxCancelQuestionsButton" Text="Cancel" Icon="Delete">

                                </ext:Button>
                            </Buttons>
                        </ext:Panel>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Panel>
 <!--Hidden Windows-->
        <ext:Window ID="uxAddFormWindow" runat="server" 
            Title="Add Form" 
            Hidden="true" 
            Width="450" 
            Y="50"
            Modal="true" >
            <Items> 
                <ext:FormPanel runat="server" ID="uxAddFormPanel" Layout="FormLayout">
                    <Items>
                        <ext:TextField runat="server" ID="uxAddFormName" FieldLabel="Name" />                            
                        <ext:ComboBox runat="server" ID="uxAddFormOrg" FieldLabel="Organization" QueryMode="Local" TypeAhead="true">
                            
                        </ext:ComboBox>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxAddFormSubmit" Text="Submit" Icon="Add">
                            <DirectEvents>
                                <Click OnEvent="deAddForm" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="uxAddFormCancel" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="#{uxAddFormPanel}.reset(); #{uxAddFormWindow}.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
        <ext:Window ID="uxAddFieldSetWindow" runat="server" 
            Title="Add FieldSet" 
            Hidden="true" 
            Width="450"
            Y="50"
            Modal="true">
            <Items>
                <ext:FormPanel runat="server" ID="uxAddFieldSetPanel" Layout="HBoxLayout">
                    <Items>
                        <ext:TextField runat="server" ID="uxAddFieldSetTitle" FieldLabel="Title" Flex="1" />
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxAddFieldSetSubmit" Text="Submit" Icon="Add">

                        </ext:Button>
                        <ext:Button runat="server" ID="uxAddFieldSetCancel" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="#{uxAddFieldSetPanel}.reset(); #{uxAddFieldSetWindow}.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>

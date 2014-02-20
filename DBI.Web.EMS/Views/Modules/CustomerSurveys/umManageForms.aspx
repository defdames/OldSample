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

                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                    </Plugins>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxAddFormButton" Text="Add Form" Icon="ApplicationAdd">

                                </ext:Button>
                                <ext:Button runat="server" ID="uxEditFormButton" Text="Edit Form" Icon="ApplicationEdit">

                                </ext:Button>
                                <ext:ToolbarSeparator runat="server" />
                                <ext:Button runat="server" ID="uxAddFieldSetButton" Text="Add FieldSet" Icon="ApplicationAdd">
                                    
                                </ext:Button>
                                <ext:Button runat="server" ID="uxEditFieldSetButton" Text="Edit FieldSet" Icon="ApplicationEdit">

                                </ext:Button>
                                <ext:ToolbarSeparator runat="server" />
                                <ext:Button runat="server" ID="uxPreviewFormButton" Text="Preview" Icon="PageWhiteGo">

                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <BottomBar>
                        <ext:PagingToolbar runat="server" />
                    </BottomBar>
                </ext:GridPanel>
                    </Items>
                </ext:Panel>
                <ext:Panel runat="server" ID="uxMiddlePanel" Flex="1" Title="Fieldsets - Drag and Drop to Add/Remove">
                    <Items>
                        <ext:Panel runat="server" Layout="HBoxLayout" ID="uxFieldSetDragDropContainer">
                            <Items>
                                <ext:GridPanel ID="uxCurrentFieldSetsGrid" runat="server" Title="Current Fieldsets" Flex="1" Layout="HBoxLayout">
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column runat="server" Text="Fieldset" Flex="2"/>
                                            <ext:Column runat="server" Text="Number of Questions" Flex="1" />
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
    </form>
</body>
</html>

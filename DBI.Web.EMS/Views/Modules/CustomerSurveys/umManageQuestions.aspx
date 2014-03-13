<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umManageQuestions.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umManageQuestions" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" IsDynamic="false" />
        <ext:Viewport runat="server" Layout="AutoLayout">
            <Items>
                <ext:GridPanel runat="server" ID="uxCurrentQuestionsGrid" Title="Current Questions">
                    <Store>
                        <ext:Store runat="server" ID="uxCurrentQuestionsStore" OnReadData="deReadQuestions" RemoteSort="true">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="TEXT" />
                                        <ext:ModelField Name="QUESTION_TYPE_NAME" />
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
                            <ext:Column runat="server" Text="Question Name" DataIndex="TEXT" />
                            <ext:Column runat="server" Text="Question Type" DataIndex="QUESTION_TYPE_NAME" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                    </Plugins>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxAddQuestionButton" Text="Add Question" Icon="ApplicationAdd">
                                    <Listeners>
                                        <Click Handler="#{uxQuestionsWindow}.show()" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:GridPanel>
                <ext:GridPanel runat="server" ID="uxCurrentOptionsGrid" Title="Current Options">
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="Question Name" />
                            <ext:Column runat="server" Text="Option Value" />
                        </Columns>
                    </ColumnModel>
                    <TopBar>
                        <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Button runat="server" ID="uxAddQuestionOptionButton" Text="Add Option" Icon="ApplicationAdd">
                                        <Listeners>
                                            <Click Handler="#{uxOptionsWindow}.show()" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="uxDeactivateOptionButton" Text="Deactivate Option" Icon="ApplicationDelete">
                                        <DirectEvents>
                                            <Click OnEvent="deDeactivateOption" />
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
        <ext:Window runat="server" ID="uxQuestionsWindow" Modal="true" Hidden="true" Width="400">
            <Items>
                <ext:FormPanel runat="server" ID="uxQuestionsForm">
                    <Items>
                        <ext:TextField runat="server" ID="uxQuestionName" FieldLabel="Question Name" />
                        <ext:ComboBox runat="server" ID="uxQuestionType" 
                            FieldLabel="Select a Question Type" 
                            QueryMode="Local" 
                            TypeAhead="true" 
                            DisplayField="QUESTION_TYPE_NAME" 
                            ValueField="TYPE_ID">
                            <Store>
                                <ext:Store runat="server" 
                                    AutoDataBind="true" 
                                    ID="uxQuestionTypeStore">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="QUESTION_TYPE_NAME" />
                                                <ext:ModelField Name="TYPE_ID" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                        </ext:ComboBox>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxQuestionSubmit" Text="Submit">
                            <DirectEvents>
                                <Click OnEvent="deAddEditQuestion" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="uxQuestionCancel" Text="Cancel">
                            <Listeners>
                                <Click Handler="#{uxQuestionsForm}.reset(); #{uxQuestionsWindow}.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
        <ext:Window runat="server" ID="uxOptionsWindow" Modal="true" Hidden="true" Width="400">
            <Items>
                <ext:FormPanel runat="server" ID="uxOptionsForm">
                    <Items>
                        <ext:ComboBox runat="server" ID="uxOptionQuestion" 
                            QueryMode="Local" 
                            TypeAhead="true" 
                            FieldLabel="Select a Question"
                            DisplayField="TEXT"
                            ValueField="QUESTION_ID">
                            <Store>
                                <ext:Store runat="server" AutoDataBind="true" ID="uxOptionQuestionStore">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="QUESTION_ID" />
                                                <ext:ModelField Name="TEXT" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                        </ext:ComboBox>
                        <ext:TextField runat="server" ID="uxOptionName" FieldLabel="Option Name" />
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxOptionSubmit" Text="Submit">
                            <DirectEvents>
                                <Click OnEvent="deAddOption" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="uxOptionCancel" Text="Cancel">
                            <Listeners>
                                <Click Handler="#{uxOptionsForm}.reset(); #{uxOptionsWindow}.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>

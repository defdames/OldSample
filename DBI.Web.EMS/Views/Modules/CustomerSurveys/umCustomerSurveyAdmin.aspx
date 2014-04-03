<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCustomerSurveyAdmin.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umCustomerSurveyAdmin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" IsDynamic="false" />
        <ext:Viewport ID="uxAdminViewPort" runat="server" Layout="BorderLayout">
            <Items>
                <ext:GridPanel runat="server" ID="uxFormsGrid" Region="North" Title="Forms">
                    <Store>
                        <ext:Store runat="server" ID="uxFormsStore" AutoDataBind="true" OnReadData="deReadForms" RemoteSort="true" PageSize="10">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="FORM_ID" Type="Int" />
                                        <ext:ModelField Name="FORMS_NAME" Type="String" />
                                        <ext:ModelField Name="ORGANIZATION" Type="String" />
                                        <ext:ModelField Name="NUM_QUESTIONS" Type="Int" />
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
                            <ext:Column runat="server" DataIndex="FORMS_NAME" Text="Form Name" />
                            <ext:Column runat="server" DataIndex="ORGANIZATION" Text="Organization Name" />
                            <ext:Column runat="server" DataIndex="NUM_QUESTIONS" Text="Number of Questions"  />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                    </Plugins>
                    <BottomBar>
                        <ext:PagingToolbar runat="server" />
                    </BottomBar>
                </ext:GridPanel>
                <ext:GridPanel runat="server" ID="uxQuestionsGrid" Region="Center" Title="Form Questions">
                    <Store>
                        <ext:Store runat="server" ID="uxQuestionsStore" RemoteSort="true" PageSize="10" AutoDataBind="true" OnReadData="deReadQuestions">
                            <Model>
                                <ext:Model runat="server" Name="Question" IDProperty="QUESTION_ID">
                                    <Fields>
                                        <ext:ModelField Name="QUESTION_ID" UseNull="true" />
                                        <ext:ModelField Name="TEXT" Type="String" />
                                        <ext:ModelField Name="QUESTION_TYPE_NAME" Type="String" />
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
                            <ext:Column runat="server" DataIndex="TEXT" Text="Question Name" />
                            <ext:Column runat="server" DataIndex="QUESTION_TYPE_NAME" Text="Question Type" />
                            <ext:Column runat="server" DataIndex="TITLE" Text="Fieldset" />
                            <ext:BooleanColumn runat="server" DataIndex="IS_REQUIRED" TrueText="Yes" FalseText="No" Text="Required" />
                            <ext:Column runat="server" DataIndex="SORT_ORDER" Text="Sort Order" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                    </Plugins>
                    <BottomBar>
                        <ext:PagingToolbar runat="server" />
                    </BottomBar>
                </ext:GridPanel>
                <ext:Panel runat="server" Layout="HBoxLayout" Region="South">
                    <Items>
                        <ext:GridPanel runat="server" Flex="1" Title="Question Options" ID="uxOptionsGrid">
                            <Store>
                                <ext:Store runat="server" ID="uxOptionsStore" AutoDataBind="true" PageSize="10" RemoteSort="true" OnReadData="deReadOptions">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="TEXT" Type="String" />
                                                <ext:ModelField Name="OPTION_NAME" Type="String" />
                                                <ext:ModelField Name="SORT_ORDER" Type="Int" />
                                                <ext:ModelField Name="IS_ACTIVE" Type="Boolean" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Parameters>
                                        <ext:StoreParameter Name="QuestionId" Value="#{uxQuestionsGrid}.getSelectionModel().getSelection()[0].data.QUESTION_ID" Mode="Raw" />
                                    </Parameters>
                                </ext:Store>
                            </Store>
                            <ColumnModel runat="server">
                                <Columns>
                                    <ext:Column runat="server" DataIndex="TEXT" Text="Question" />
                                    <ext:Column runat="server" DataIndex="OPTION_NAME" Text="Option Name" />
                                    <ext:Column runat="server" DataIndex="SORT_ORDER" Text="Sort Order" />
                                    <ext:BooleanColumn runat="server" DataIndex="IS_ACTIVE" TrueText="Yes" FalseText="No" Text="Active" />
                                </Columns>
                            </ColumnModel>
                            <Plugins>
                                <ext:FilterHeader runat="server" Remote="true" />
                            </Plugins>
                            <BottomBar>
                                <ext:PagingToolbar runat="server" />
                            </BottomBar>
                        </ext:GridPanel>
                        <ext:GridPanel runat="server" Flex="1" Title="Fieldsets" ID="uxFieldsetsGrid">
                            <Store>
                                <ext:Store runat="server" ID="uxFieldsetsStore" AutoDataBind="true" PageSize="10" RemoteSort="true" OnReadData="deReadFieldsets">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
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
                            <ColumnModel runat="server">
                                <Columns>
                                    <ext:Column runat="server" DataIndex="TITLE" Text="Fieldset Name" />
                                    <ext:Column runat="server" DataInde="SORT_ORDER" Text="Sort Order" />
                                </Columns>
                            </ColumnModel>
                            <Plugins>
                                <ext:FilterHeader runat="server" Remote="true" />
                            </Plugins>
                            <BottomBar>
                                <ext:PagingToolbar runat="server" />
                            </BottomBar>
                        </ext:GridPanel>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

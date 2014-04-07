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
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" IsDynamic="false" />
        <ext:Viewport ID="uxAdminViewPort" runat="server" Layout="AccordionLayout">
            <Items>
                <ext:GridPanel runat="server" ID="uxFormsGrid" Title="Forms" Layout="FitLayout">
                    <Store>
                        <ext:Store runat="server" ID="uxFormsStore" AutoDataBind="true" OnReadData="deReadForms" RemoteSort="true" PageSize="5">
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
                            <ext:Column runat="server" DataIndex="FORMS_NAME" Text="Form Name">
                                <Editor>
                                    <ext:TextField runat="server" />
                                </Editor>
                            </ext:Column>
                            <ext:Column runat="server" DataIndex="ORG_ID" Text="Organization Name">
                                <Renderer Fn="OrgRenderer" />
                                <Editor>
                                    <ext:ComboBox runat="server" ForceSelection="true" TypeAhead="true" QueryMode="Local" ValueField="ORG_ID" DisplayField="ORG_HIER" Editable="false">
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
                            <ext:Column runat="server" DataIndex="NUM_QUESTIONS" Text="Number of Questions" />
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
                                        <Click OnEvent="deSaveForms" Before="#{uxFormsStore}.isDirty()">
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
                    <Listeners>
                        <Select Handler="#{uxQuestionsStore}.reload(); #{uxFieldsetsStore}.reload();" />
                    </Listeners>
                </ext:GridPanel>
                <ext:Panel runat="server" Layout="AutoLayout" Title="Form Information" ID="uxBottomPanel">
                    <Items>
                        <ext:GridPanel runat="server" Title="Fieldsets" ID="uxFieldsetsGrid" MaxWidth="1000" Margin="5">
                            <Store>
                                <ext:Store runat="server" ID="uxFieldsetsStore" AutoDataBind="true" AutoLoad="false" PageSize="5" RemoteSort="true" OnReadData="deReadFieldsets">
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
                                    <ext:Column ID="Column1" runat="server" DataIndex="TITLE" Text="Fieldset Name">
                                        <Editor>
                                            <ext:TextField runat="server" />
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column ID="Column2" runat="server" DataIndex="SORT_ORDER" Text="Sort Order">
                                        <Editor>
                                            <ext:TextField runat="server" />
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
                                        <ext:Button runat="server" Icon="ApplicationAdd" Text="Add Fieldset">
                                            <Listeners>
                                                <Click Handler="#{uxFieldSetsStore}.insert(0, new Fieldset());" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button runat="server" Icon="Add" Text="Save">
                                            <DirectEvents>
                                                <Click OnEvent="deSaveFieldsets">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="FormId" Value="#{uxFormsGrid}.getSelectionModel().getSelection()[0].data.FORM_ID" Mode="Raw" />
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
                                <ext:Store runat="server" ID="uxQuestionsStore" RemoteSort="true" PageSize="10" AutoDataBind="true" AutoLoad="false" OnReadData="deReadQuestions">
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
                            <Listeners>
                                <Select Handler="#{uxOptionsStore}.reload()" />
                            </Listeners>
                            <TopBar>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:Button runat="server" Icon="ApplicationAdd" Text="Add Question">

                                        </ext:Button>
                                        <ext:Button runat="server" Icon="Add" Text="Save">

                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                        </ext:GridPanel>
                        <ext:GridPanel runat="server" Title="Question Options" ID="uxOptionsGrid" MaxWidth="1000" Margin="5">
                            <Store>
                                <ext:Store runat="server" ID="uxOptionsStore" AutoDataBind="true" AutoLoad="false" PageSize="5" RemoteSort="true" OnReadData="deReadOptions">
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
                            <TopBar>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:Button runat="server" Icon="ApplicationAdd" Text="Add Option">

                                        </ext:Button>
                                        <ext:Button runat="server" Icon="Add" Text="Save">

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

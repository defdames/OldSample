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
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="Question Name" />
                            <ext:Column runat="server" Text="Question Type" />
                        </Columns>
                    </ColumnModel>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxAddQuestionButton" Text="Add Question" Icon="ApplicationAdd">
                                    
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

                                    </ext:Button>
                                    <ext:Button runat="server" ID="uxDeactivateOptionButton" Text="Deactivate Option" Icon="ApplicationDelete">

                                    </ext:Button>
                                </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

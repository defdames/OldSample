<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCrossingSecurity.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umCrossingSecurity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <div></div>
                        <ext:Toolbar ID="Toolbar3" runat="server">
                                <Items>
                                    <ext:Button ID="uxAddProjectButton" runat="server" Text="Add Project" Icon="ApplicationAdd" />
                                    <ext:Button ID="uxEditProjectButton" runat="server" Text="Edit Project" Icon="ApplicationEdit" />
                                </Items>
                            </ext:Toolbar>
        <ext:Panel ID="Panel1" runat="server" Width="800" Height="300">
            <LayoutConfig>
                <ext:HBoxLayoutConfig Align="Stretch" Padding="5" />
            </LayoutConfig>
            <Items>
                <ext:GridPanel ID="GridPanel2" runat="server" Flex="1" Title="Select Project" Margins="0 2 0 0" Width="400">
                    <Store>
                        <ext:Store runat="server"
                            ID="Store1"
                            AutoDataBind="true" WarningOnDirty="false">
                            <Model>
                                <ext:Model ID="Model2" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="" />
                               </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column1" runat="server" DataIndex="" Text="Project Name" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="FilterHeader2" runat="server" />
                    </Plugins>
                    <SelectionModel>
                        <ext:CellSelectionModel ID="CellSelectionModel1" runat="server" Mode="Single" />
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar2" runat="server" />
                    </BottomBar>
                        
                    </ext:GridPanel>
                <%--  ---------------------------------------------------------------------------------------------------------------------%>
                <ext:GridPanel ID="GridPanel1" runat="server" Flex="1" Title="Apply Selected Crossing to Project" Margins="0 2 0 0" Width="400">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxCurrentContactStore"
                            AutoDataBind="true" WarningOnDirty="false">
                            <Model>
                                <ext:Model ID="Model1" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="" />

                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="uxNameCON" runat="server" DataIndex="" Text="Crossing #" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="FilterHeader1" runat="server" />
                    </Plugins>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" Mode="Multi" />
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" />

                    </BottomBar>
                </ext:GridPanel>
            </Items>
            <BottomBar>
                <ext:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <ext:ToolbarFill ID="ToolbarFill1" runat="server" />

                        <ext:Button ID="uxApplyButtonCS" runat="server" Text="Apply" Icon="Add" />
                        <ext:Button ID="CancelButtonCS" runat="server" Text="Cancel" Icon="Delete" />
                    </Items>
                </ext:Toolbar>
            </BottomBar>
        </ext:Panel>
    </form>
</body>
</html>

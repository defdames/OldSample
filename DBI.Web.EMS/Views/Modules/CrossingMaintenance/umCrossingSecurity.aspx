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
                       
        <ext:Panel ID="Panel1" runat="server" Width="1500" Height="355">
            <LayoutConfig>
                <ext:HBoxLayoutConfig Align="Stretch" Padding="5" />
            </LayoutConfig>
            <Items>
                <ext:GridPanel ID="GridPanel2" runat="server" Flex="1" Title="Select Project" Margins="0 2 0 0" Width="750">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxCurrentSecurityProjectStore"
                            OnReadData="deSecurityProjectGrid"
                            PageSize="10"
                            AutoDataBind="true" WarningOnDirty="false">
                            <Model>
                                <ext:Model ID="Model2" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="PROJECT_ID" />
                                        <ext:ModelField Name="LONG_NAME" />
                                        <ext:ModelField Name="SEGMENT1" />
                                        <ext:ModelField Name="STATUS_VALUE" />
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
                            <ext:Column ID="Column1" runat="server" DataIndex="SEGMENT1" Text="Project" Flex="1" />
                            <ext:Column ID="Column2" runat="server" DataIndex="LONG_NAME" Text="Project Name" Flex="2" />
                            <ext:Column ID="Column3" runat="server" DataIndex="STATUS_VALUE" Text="Status" Flex="1" />
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
                <ext:GridPanel ID="GridPanel1" runat="server" Flex="1" Title="Apply Selected Crossing to Project" Margins="0 2 0 0" Width="750">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxCurrentSecurityCrossingStore"
                             OnReadData="deSecurityCrossingGridData"
                             PageSize="10"
                            AutoDataBind="true" WarningOnDirty="false">
                            <Model>
                                <ext:Model ID="Model1" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="CROSSING_ID" />
                                        <ext:ModelField Name="CROSSING_NUMBER" />
                                        <ext:ModelField Name="SUB_DIVISION" />
                                        <ext:ModelField Name="MTM" />
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
                            <ext:Column ID="uxNameCON" runat="server" DataIndex="CROSSING_NUMBER" Text="Crossing #" Flex="1" />
                            <ext:Column ID="Column4" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />
                            <ext:Column ID="Column5" runat="server" DataIndex="MTM" Text="Manager" Flex="1" />

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

                        <ext:Button ID="uxApplyButtonCS" runat="server" Text="Associate" Icon="ArrowJoin" />
                        <ext:Button ID="CancelButtonCS" runat="server" Text="Cancel" Icon="Delete" />
                    </Items>
                </ext:Toolbar>
            </BottomBar>
        </ext:Panel>
    </form>
</body>
</html>

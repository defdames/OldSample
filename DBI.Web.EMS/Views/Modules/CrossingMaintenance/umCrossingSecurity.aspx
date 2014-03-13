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
        <ext:GridPanel ID="uxProjectGrid" runat="server" Flex="1" SimpleSelect="true" Title="Select Project" Margins="0 2 0 0">
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
                                <ext:ModelField Name="ORGANIZATION_NAME" />
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
                    <ext:Column ID="Column3" runat="server" DataIndex="ORGANIZATION_NAME" Text="Organization Name" Flex="1" />
                </Columns>
            </ColumnModel>
            <Plugins>
                <ext:FilterHeader ID="FilterHeader2" runat="server" Remote="true" />
            </Plugins>
            <SelectionModel>
                <ext:RowSelectionModel ID="project" runat="server" Mode="Single" AllowDeselect="true" />
            </SelectionModel>
           <%-- <Listeners>
                <Select Handler="#{CheckboxSelectionModel1}.enable()" />
            </Listeners>--%>
            <BottomBar>
                <ext:PagingToolbar ID="PagingToolbar2" runat="server" />
            </BottomBar>
        </ext:GridPanel>
        <%--  ---------------------------------------------------------------------------------------------------------------------%>
        <ext:GridPanel ID="uxCrossingGrid" runat="server" Flex="1" Title="Apply Selected Crossing to Project" Margins="0 2 0 0" >
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
                                <ext:ModelField Name="PROJECT_ID" />
                                <ext:ModelField Name="LONG_NAME" />
                                <ext:ModelField Name="RAILROAD" />
                                <ext:ModelField Name="SERVICE_UNIT" />
                                <ext:ModelField Name="SUB_DIVISION" />

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
                    <ext:Column ID="Column8" runat="server" DataIndex="LONG_NAME" Text="Current Project Name" Flex="2" />
                    <ext:Column ID="Column7" runat="server" DataIndex="RAILROAD" Text="RailRoad" Flex="1" />
                    <ext:Column ID="Column6" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="1" />
                    <ext:Column ID="Column4" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />

                </Columns>
            </ColumnModel>
            <Plugins>
                <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
            </Plugins>
            <SelectionModel>
                <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" Mode="Multi" />
            </SelectionModel>
            <%-- <Listeners>
                <Select Handler="#{uxApplyButtonCS}.enable()" />
            </Listeners>--%>
            <BottomBar>
                <ext:PagingToolbar ID="PagingToolbar1" runat="server" />

            </BottomBar>
        </ext:GridPanel>

        <ext:Toolbar ID="Toolbar1" runat="server">
            <Items>
                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />

                <ext:Button ID="uxApplyButtonCS" runat="server" Text="Associate" Icon="ArrowJoin" >
                    <DirectEvents>
                        <Click OnEvent="deAssociateCrossings">
                            <Confirmation ConfirmRequest="true" Title="Associate?" Message="Are you sure you want to associate the selected crossings with the selected project?" />
                            <ExtraParams>
                                <ext:Parameter Name="projectId" Value="#{uxProjectGrid}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
                                <ext:Parameter Name="selectedCrossings" Value="Ext.encode(#{uxCrossingGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />

                            </ExtraParams>
                        </Click>


                    </DirectEvents>
                </ext:Button>

            </Items>
        </ext:Toolbar>
    </form>
</body>
</html>

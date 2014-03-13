<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEditGroups.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Security.umEditGroups" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../../../Resources/Scripts/functions.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" Namespace="" />
        <ext:Viewport runat="server" ID="uxEditGroupViewPort" Layout="BorderLayout">
            <Items>
                <ext:GridPanel runat="server" ID="uxGroupsGrid" Region="North">
                    <Store>
                        <ext:Store runat="server" ID="uxGroupsStore" OnReadData="deReadGroups" AutoDataBind="true" RemoteSort="true" PageSize="10">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="JOB_ID" />
                                        <ext:ModelField Name="JOB_NAME" />
                                        <ext:ModelField Name="LOCATION" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Sorters>
                                <ext:DataSorter Property="JOB_NAME" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="Job Title" DataIndex="JOB_NAME" />
                            <ext:Column runat="server" Text="Location" DataIndex="LOCATION" />
                        </Columns>
                    </ColumnModel>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxManageGroupsButton" Text="Edit Group Permissions" Icon="ApplicationEdit">
                                    <DirectEvents>
                                        <Click OnEvent="deLoadPermissionsWindow">
                                            <EventMask ShowMask="true" />
                                            <ExtraParams>
                                                <ext:Parameter Name="JobId" Value="#{uxGroupsGrid}.getSelectionModel().getSelection()[0].data.JOB_ID" Mode="Raw" />
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
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                    </Plugins>
                    <DirectEvents>
                        <SelectionChange OnEvent="deLoadPermissions">
                            <ExtraParams>
                                <ext:Parameter Name="JobId" Value="#{uxGroupsGrid}.getSelectionModel().getSelection()[0].data.JOB_ID" Mode="Raw" />
                            </ExtraParams>
                        </SelectionChange>
                    </DirectEvents>
                </ext:GridPanel>
                <ext:GridPanel runat="server" ID="uxPermissionsGrid" Region="Center">
                    <Store>
                        <ext:Store runat="server" ID="uxPermissionsStore" AutoDataBind="true" RemoteSort="true" PageSize="10" OnReadData="deReadPermissions">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="PERMISSION_ID" />
                                        <ext:ModelField Name="PERMISSION_NAME" />
                                        <ext:ModelField Name="DESCRIPTION" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Sorters>
                                <ext:DataSorter Property="PERMISSION_NAME" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="Permission" DataIndex="PERMISSION_NAME" />
                            <ext:Column runat="server" Text="Description" DataIndex="DESCRIPTION" />
                        </Columns>
                    </ColumnModel>
                    <BottomBar>
                        <ext:PagingToolbar runat="server" />
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
        <!--Hidden Windows -->
        <ext:Window runat="server" ID="uxUpdateGroupPermissionWindow" Width="650" Hidden="true" Layout="HBoxLayout">
            <Items>
                <ext:GridPanel runat="server" ID="uxAvaialblePermissions" Title="Available Permissions" Flex="1"> 
                    <Store>
                        <ext:Store runat="server" ID="uxAvailablePermissionsStore">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="PERMISSION_ID" />
                                        <ext:ModelField Name="PERMISSION_NAME" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" DataIndex="PERMISSION_NAME" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
                <ext:Panel runat="server" ID="uxButtonsPanel" Layout ="VBoxLayout">
                    <Items>
                        <ext:Button runat="server" Icon="ResultsetNext" StyleSpec="margin-bottom:2px;">
                            <Listeners>
                                <Click Handler="TwoGridSelector.add(uxAvaialblePermissions, uxSelectedPermissionsGrid);" />
                            </Listeners>
                            <ToolTips>
                                <ext:ToolTip runat="server" Title="Add" Html="Add Selected Rows" />
                            </ToolTips>
                        </ext:Button>
                        <ext:Button runat="server" Icon="ResultsetLast" StyleSpec="margin-bottom:2px;">
                            <Listeners>
                                <Click Handler="TwoGridSelector.addAll(uxAvaialblePermissions, uxSelectedPermissionsGrid);" />
                            </Listeners>
                            <ToolTips>
                                <ext:ToolTip runat="server" Title="Add all" Html="Add All Rows" />
                            </ToolTips>
                        </ext:Button>
                        <ext:Button runat="server" Icon="ResultsetPrevious" StyleSpec="margin-bottom:2px;">
                            <Listeners>
                                <Click Handler="TwoGridSelector.remove(uxAvaialblePermissions, uxSelectedPermissionsGrid);" />
                            </Listeners>
                            <ToolTips>
                                <ext:ToolTip runat="server" Title="Remove" Html="Remove Selected Rows" />
                            </ToolTips>
                        </ext:Button>
                        <ext:Button runat="server" Icon="ResultsetFirst" StyleSpec="margin-bottom:2px;">
                            <Listeners>
                                <Click Handler="TwoGridSelector.removeAll(uxAvaialblePermissions, uxSelectedPermissionsGrid);" />
                            </Listeners>
                            <ToolTips>
                                <ext:ToolTip runat="server" Title="Remove all" Html="Remove All Rows" />
                            </ToolTips>
                        </ext:Button>
                    </Items>
                </ext:Panel>
                <ext:GridPanel runat="server" ID="uxSelectedPermissionsGrid" Title="SelectedPermissions" Flex="1">
                    <Store>
                        <ext:Store runat="server" ID="uxSelectedPermissionsStore">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="PERMISSION_ID" />
                                        <ext:ModelField Name="PERMISSION_NAME" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" DataIndex="PERMISSION_NAME" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
            </Items>
            <Buttons>
                <ext:Button runat="server" ID="uxUpdateGroupPermissionsButton" Text="Submit" Icon="Add">
                    <DirectEvents>
                        <Click OnEvent="deUpdateGroupPermissions">
                            <EventMask ShowMask="true" />
                        </Click>
                    </DirectEvents>
                </ext:Button>
            </Buttons>
        </ext:Window>
    </form>
</body>
</html>

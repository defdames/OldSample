<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEditGroups.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Security.umEditGroups" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" />
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
        <ext:Window runat="server" ID="uxUpdateGroupPermissionWindow" Width="650">
            <Items>
                <ext:FormPanel runat="server" ID="uxUpdateGroupPermissionForm" Layout="FormLayout">
                    <Items>

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
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEditPermissions.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Security.umEditPermissions" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" />
        <ext:Viewport runat="server" ID="uxPermissionsViewPort">
            <Items>
                <ext:GridPanel runat="server" ID="uxCurrentPermissionsGrid" Layout="HBoxLayout">
                    <Store>
                        <ext:Store runat="server" ID="uxCurrentPermissionsStore" OnReadData="deReadPermissions" RemoteSort="true" PageSize="25" AutoDataBind="true">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="PermissionId" />
                                        <ext:ModelField Name="PermissionName" />
                                        <ext:ModelField Name="Description" />
                                        <ext:ModelField Name="ParentPermissionName" />
                                        <ext:ModelField Name="ParentPermId" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="PermissionName" Direction="ASC" />
                            </Sorters>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" DataIndex="PermissionName" Text="Name" Flex="1" />
                            <ext:Column runat="server" DataIndex="Description" Text="Description" Flex="1" />
                            <ext:Column runat="server" DataIndex="ParentPermissionName" Text="Parent Permission" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                    </Plugins>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxAddPermissionButton" Text="Add Permission" Icon="ApplicationAdd">
                                    <DirectEvents>
                                        <Click OnEvent="deLoadAddPermissionWindow">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxEditPermissionButton" Text="EditPermission" Icon="ApplicationEdit">
                                    <DirectEvents>
                                        <Click OnEvent="deLoadEditPermissionWindow">
                                            <EventMask ShowMask="true" />
                                            <ExtraParams>
                                                <ext:Parameter Name="PermissionId" Value="#{uxCurrentPermissionsGrid}.getSelectionModel().getSelection()[0].data.PermissionId" Mode="Raw" />
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
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
        <!--Hidden Windows-->
        <ext:Window runat="server" ID="uxUpdatePermissionsWindow" Width="650" Hidden="true">
            <Items>
                <ext:FormPanel runat="server" ID="uxUpdatePermissionsForm" Layout="FormLayout">
                    <Items>
                        <ext:Hidden  runat="server" ID="uxPermissionFormType" />
                        <ext:Hidden runat="server" ID="uxPermissionId" />
                        <ext:TextField runat="server" ID="uxPermissionName" FieldLabel="Permission Name" />
                        <ext:TextField runat="server" ID="uxPermissionDescription" FieldLabel="Description" />
                        <ext:ComboBox runat="server" ID="uxParentPermissionName" DisplayField="PERMISSION_NAME" 
                            ValueField="PERMISSION_ID" QueryMode="Local" TypeAhead="true" FieldLabel="Parent Permission" ForceSelection="true">
                            <Store>
                                <ext:Store runat="server" ID="uxParentPermissionStore" OnReadData="deReadParents" AutoDataBind="true">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="PERMISSION_NAME" />
                                                <ext:ModelField Name="PERMISSION_ID" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:PageProxy />
                                    </Proxy>
                                </ext:Store>
                            </Store>
                        </ext:ComboBox>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxUpdatePermissionSubmit" Text="Submit" Icon="Add">
                            <DirectEvents>
                                <Click OnEvent="deUpdatePermission">
                                    <EventMask ShowMask="true" />
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="uxUpdatePermissionCancel" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="#{uxUpdatePermissionsForm}.reset(); #{uxUpdatePermissionsWindow}.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>

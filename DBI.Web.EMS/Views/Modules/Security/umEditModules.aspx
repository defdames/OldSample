<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEditModules.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Security.umEditModules" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:GridPanel runat="server" ID="uxModuleGrid" Region="North" Layout="HBoxLayout">
                    <Store>
                        <ext:Store runat="server" ID="uxModuleStore" AutoDataBind="true" OnReadData="deReadModules" PageSize="10" RemoteSort="true">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="MODULE_ID" />
                                        <ext:ModelField Name="MODULE_NAME" />
                                        <ext:ModelField Name="PERMISSION_NAME" ServerMapping="SYS_PERMISSIONS.PERMISSION_NAME" />
                                        <ext:ModelField Name="PERMISSION_DESCRIPTION" ServerMapping="SYS_PERMISSIONS.DESCRIPTION" />
                                    </Fields>                                    
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Sorters>
                                <ext:DataSorter Property="MODULE_NAME" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="Name" DataIndex="MODULE_NAME" Flex="1" />
                            <ext:Column runat="server" Text="Permission Required" DataIndex="PERMISSION_NAME" Flex="1" />
                            <ext:Column runat="server" Text="Permission Description" DataIndex="PERMISSION_DESCRIPTION" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" />
                    </SelectionModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                    </Plugins>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" Text="Add Module" Icon="ApplicationAdd">
                                    <DirectEvents>
                                        <Click OnEvent="deLoadAddModuleWindow">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" Text="Edit Module" Icon="ApplicationEdit">
                                    <DirectEvents>
                                        <Click OnEvent="deLoadEditModuleWindow">
                                            <ExtraParams>
                                                <ext:Parameter Name="ModuleId" Value="#{uxModuleGrid}.getSelectionModel().getSelection()[0].data.MODULE_ID" Mode="Raw" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <BottomBar>
                        <ext:PagingToolbar runat="server" />
                    </BottomBar>
                    <DirectEvents>
                        <SelectionChange OnEvent="deLoadMenuItems">
                            <ExtraParams>
                                <ext:Parameter Name="ModuleId" Value="#{uxModuleGrid}.getSelectionModel().getSelection()[0].data.MODULE_ID" Mode="Raw" />
                            </ExtraParams>
                        </SelectionChange>
                    </DirectEvents>
                </ext:GridPanel>
                <ext:GridPanel runat="server" ID="uxMenuItemsGrid" Region="Center">
                    <Store>
                        <ext:Store runat="server" ID="uxMenuItemsStore" PageSize="10" RemoteSort="true" OnReadData="deReadMenuItems">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="MENU_ID" />
                                        <ext:ModelField Name="ITEM_NAME" />
                                        <ext:ModelField Name="ITEM_URL" />
                                        <ext:ModelField Name="PERMISSION_NAME" ServerMapping="SYS_PERMISSIONS.PERMISSION_NAME" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Sorters>
                                <ext:DataSorter Property="ITEM_NAME" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="Name" DataIndex="ITEM_NAME" />
                            <ext:Column runat="server" Text="URL" DataIndex="ITEM_URL" />
                            <ext:Column runat="server" Text="Permission Required" DataIndex="PERMISSION_NAME" />
                        </Columns>
                    </ColumnModel>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" Text="Add Menu Item" Icon="ApplicationAdd">
                                    <DirectEvents>
                                        <Click OnEvent="deLoadAddItemWindow">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" Text="Edit Menu Item" Icon="ApplicationEdit">
                                    <DirectEvents>
                                        <Click OnEvent="deLoadEditItemWindow">
                                            <ExtraParams>
                                                <ext:Parameter Name="ItemId" Value="#{uxMenuItemsGrid}.getSelectionModel().getSelection()[0].data.MENU_ID" Mode="Raw" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Plugins>
                        <ext:FilterHeader runat="server" />
                    </Plugins>
                    <BottomBar>
                        <ext:PagingToolbar runat="server" />
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
        <!--Hidden Windows-->
        <ext:Window runat="server" ID="uxModulesWindow" Hidden="true" Width="650" Modal="true">
            <Items>
                <ext:Hidden runat="server" ID="uxModuleFormType" />
                <ext:Hidden runat="server" ID="uxModuleId" />
                <ext:FormPanel runat="server" ID="uxModulesForm" Layout="FormLayout">
                    <Items>
                        <ext:TextField runat="server" ID="uxModuleName" FieldLabel="Module Name" />
                        <ext:ComboBox runat="server" ID="uxModulePermission" FieldLabel="Required Permission" DisplayField="PERMISSION_NAME" ValueField="PERMISSION_ID">
                            <Store>
                                <ext:Store runat="server" ID="uxModulePermissionStore" AutoDataBind="true">
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
                        </ext:ComboBox>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxUpdateModuleButton" Text="Submit" Icon="Add">
                            <DirectEvents>
                                <Click OnEvent="deUpdateModule" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="uxCancelModuleButton" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="#{uxModulesForm}.reset(); #{uxModulesWindow}.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
        <ext:Window runat="server" ID="uxMenuItemsWindow" Hidden="true" Width="650" Modal="true">
            <Items>
                <ext:Hidden runat="server" ID="uxMenuItemsFormType" />
                <ext:Hidden runat="server" ID="uxMenuItemId" />
                <ext:FormPanel runat="server" ID="uxMenuItemsForm" Layout="FormLayout">
                    <Items>
                        <ext:TextField runat="server" ID="uxMenuItemName" FieldLabel="Name" />
                        <ext:TextField runat="server" ID="uxMenuItemURL" FieldLabel="Item URL" />
                        <ext:ComboBox runat="server" ID="uxMenuItemModule" FieldLabel="Parent Module" DisplayField="MODULE_NAME" ValueField="MODULE_ID">
                            <Store>
                                <ext:Store runat="server" ID="uxMenuItemModuleStore" AutoDataBind="true">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="MODULE_ID" />
                                                <ext:ModelField Name="MODULE_NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                        </ext:ComboBox>
                        <ext:ComboBox runat="server" ID="uxMenuItemPermission" FieldLabel="Required Permission" DisplayField="PERMISSION_NAME" ValueField="PERMISSION_ID">
                            <Store>
                                <ext:Store runat="server" ID="uxMenuItemPermissionStore" AutoDataBind="true">
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
                        </ext:ComboBox>
                        <ext:ComboBox runat="server" ID="uxMenuItemIcon" FieldLabel="Choose an Icon">
                        </ext:ComboBox>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxUpdateMenuItemButton" Text="Submit" Icon="Add">
                            <DirectEvents>
                                <Click OnEvent="deUpdateMenuItem" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="uxCancelMenuitemButton" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="#{uxMenuItemsForm}.reset(); #{uxMenuItemsWindow}.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>

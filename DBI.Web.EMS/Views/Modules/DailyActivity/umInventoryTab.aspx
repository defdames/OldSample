<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umInventoryTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umInventoryTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
        <ext:GridPanel runat="server"
            ID="uxCurrentInventoryGrid"
            Layout="HBoxLayout">
            <Store>
                <ext:Store runat="server"
                    ID="uxCurrentInventoryStore">
                    <Fields>
                        <ext:ModelField Name="name" />
                    </Fields>
                </ext:Store>
            </Store>
            <ColumnModel>
                <Columns>
                    <ext:Column runat="server"
                        DataIndex="name" />
                </Columns>
            </ColumnModel>
            <TopBar>
                <ext:Toolbar runat="server">
                    <Items>
                        <ext:Button runat="server"
                            ID="uxAddInventoryButton"
                            Text="Add Inventory"
                            Icon="ApplicationAdd">
                            <Listeners>
                                <Click Handler="#{uxAddInventoryWindow}.show()" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button runat="server"
                            ID="uxEditInventoryButton"
                            Text="Edit Inventory"
                            Icon="ApplicationEdit">
                            <DirectEvents>
                                <Click OnEvent="deEditInventoryForm">
                                    <ExtraParams>
                                        <ext:Parameter Name="InventoryInfo" Value="Ext.encode(#{uxCurrentInventoryGrid}.getRowsValues({selectedOnly : true})" Mode="Raw" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                            <Listeners>
                                <Click Handler="#{uxEditInventoryWindow}.show()" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button runat="server"
                            ID="uxRemoveInventoryButton"
                            Text="Remove Inventory"
                            Icon="ApplicationDelete">
                            <DirectEvents>
                                <Click OnEvent="deRemoveInventory">
                                    <ExtraParams>
                                        <ext:Parameter Name="InventoryId" Value="#{uxCurrentInventoryGrid}.getSelectionModel().getSelection()[0].data.INVENTORY_ID" Mode="Raw" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <SelectionModel>
                <ext:RowSelectionModel runat="server" Mode="Single" />
            </SelectionModel>
        </ext:GridPanel>   
        <ext:Window runat="server"
            ID="uxAddInventoryWindow"
            Layout="FormLayout"
            Width="650"
            Hidden="true">
            <Items>
                <ext:FormPanel runat="server"
                    ID="uxAddInventoryForm"
                    Layout="FormLayout">
                    <Items>

                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Window>
        <ext:Window runat="server"
            ID="uxEditInventoryWindow"
            Layout="FormLayout"
            Width="650"
            Hidden="true">
            <Items>
                <ext:FormPanel runat="server"
                    ID="uxEditInventoryForm"
                    Layout="FormLayout">
                    <Items>

                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>

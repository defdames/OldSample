<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umProfileOptions.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Security.Options.umProfileOptions" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>  
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />   
    <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
        <Items>
            <ext:GridPanel ID="uxProfileOptionGridPanel" runat="server" Flex="1" SimpleSelect="true" Title="Profile Options" Margins="5" Region="Center">
                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Button ID="uxAddButton" runat="server" Text="Add" Icon="ApplicationAdd">
                                 <DirectEvents>
                                        <Click OnEvent="deShowAddEditWindow"></Click>
                                   </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="uxEditButton" runat="server" Text="Edit" Icon="ApplicationEdit" Disabled="true"></ext:Button>
                            <ext:Button ID="uxDeleteButton" runat="server" Text="Delete" Icon="ApplicationDelete" Disabled="true"></ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Store>
                    <ext:Store runat="server"
                        ID="uxProfileOptionStore"
                        AutoDataBind="true" RemoteSort="true" PageSize="25" OnReadData="deReadProfileOptions" AutoLoad="true">
                        <Model>
                            <ext:Model ID="uxProfileOptionModel" runat="server" IDProperty="PROFILE_OPTION_ID">
                                <Fields>
                                    <ext:ModelField Name="DESCRIPTION" Type="String" />
                                    <ext:ModelField Name="PROFILE_KEY" Type="String" />
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
                        <ext:Column ID="uxColumn2" runat="server" DataIndex="PROFILE_KEY" Text="Key" Flex="1" />
                        <ext:Column ID="uxColumn1" runat="server" DataIndex="DESCRIPTION" Text="Description" Flex="1" />
                    </Columns>
                </ColumnModel>
                <Plugins>
                    <ext:FilterHeader ID="uxProfileOptionFilterHeader" runat="server" Remote="true" />
                </Plugins>
                <BottomBar>
                    <ext:PagingToolbar ID="uxProfileOptionPagingToolbar" runat="server" />
                </BottomBar>
                <View>
                    <ext:GridView ID="uxProfileOptionGridView" StripeRows="true" runat="server">
                    </ext:GridView>
                </View>
                 <SelectionModel>
                        <ext:RowSelectionModel ID="uxProfileOptionSelectionModel" runat="server" Mode="Simple">
                            <Listeners>
                                <Select Handler="if(#{uxProfileOptionSelectionModel}.getCount() > 0){#{uxEditButton}.enable(); #{uxDeleteButton}.enable();}else {#{uxEditButton}.disable(); #{uxDeleteButton}.disable();}"></Select>
                                <Deselect Handler="if(#{uxProfileOptionSelectionModel}.getCount() > 0){#{uxEditButton}.enable(); #{uxDeleteButton}.enable();}"></Deselect>
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>

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
            <ext:GridPanel ID="uxProfileOptionGridPanel" runat="server" Flex="1" Title="Profile Options" Margins="5" Region="North" SelectionMemory="false">
                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Button ID="uxAddButton" runat="server" Text="Add" Icon="ApplicationAdd">
                                 <DirectEvents>
                                        <Click OnEvent="deShowAddEditWindow"></Click>
                                   </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="uxEditButton" runat="server" Text="Edit" Icon="ApplicationEdit" Disabled="true">
                                 <DirectEvents>
                                        <Click OnEvent="deShowAddEditWindow">
                                        </Click>
                                    </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="uxDeleteButton" runat="server" Text="Delete" Icon="ApplicationDelete" Disabled="true">
                                 <DirectEvents>
                                        <Click OnEvent="deDeleteProfileOption"><Confirmation Message="Are you sure you want to delete the following profile option(s)?" Title="Delete profile option(s)" ConfirmRequest="true"></Confirmation></Click>
                                    </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Store>
                    <ext:Store runat="server"
                        ID="uxProfileOptionStore" RemoteSort="true" PageSize="25" OnReadData="deReadProfileOptions" AutoLoad="true">
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
                            <Listeners><Load Handler="#{uxDeleteButton}.disable();#{uxEditButton}.disable();"></Load></Listeners>
                    </ext:Store>
                </Store>
                <ColumnModel>
                    <Columns>
                        <ext:Column ID="uxColumn2" runat="server" DataIndex="PROFILE_KEY" Text="Name" Flex="1" />
                        <ext:Column ID="uxColumn1" runat="server" DataIndex="DESCRIPTION" Text="Description" Flex="3" />
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
                        <ext:RowSelectionModel ID="uxProfileOptionSelectionModel" runat="server" Mode="Single" AllowDeselect="true">
                            <Listeners>
                                <Select Handler="if(#{uxProfileOptionSelectionModel}.getCount() > 0){#{uxEditButton}.enable(); #{uxDeleteButton}.enable();}else {#{uxEditButton}.disable(); #{uxDeleteButton}.disable();}"></Select>
                                <Deselect Handler="if(#{uxProfileOptionSelectionModel}.getCount() > 0){#{uxEditButton}.enable(); #{uxDeleteButton}.enable();}else {#{uxEditButton}.disable(); #{uxDeleteButton}.disable();}"></Deselect>
                            </Listeners>
                              <DirectEvents>
                                       <Select OnEvent="deProfileOptionSelection">
                                       </Select>
                                   </DirectEvents>
                        </ext:RowSelectionModel>
                    </SelectionModel>
            </ext:GridPanel>




             <ext:GridPanel ID="uxProfileOptionValuesGridPanel" runat="server" Flex="1" Title="Profile Options" Margins="5" Region="Center" SelectionMemory="false">
               <Store>
                    <ext:Store runat="server"
                        ID="uxProfileOptionValuesStore" RemoteSort="true" AutoLoad="false" OnReadData="deReadProfileOptionValues">
                        <Model>
                            <ext:Model ID="Model1" runat="server">
                                <Fields>
                                    <ext:ModelField Name="USER_PROFILE_OPTION_ID" Type="Int" />
                                    <ext:ModelField Name="ORG_PROFILE_OPTION_ID" Type="Int" />
                                    <ext:ModelField Name="PROFILE_OPTION_TYPE" Type="String" />
                                    <ext:ModelField Name="PROFILE_VALUE" Type="String" />
                                    <ext:ModelField Name="PROFILE_OWNER_NAME" Type="String" />
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
                        <ext:Column ID="Column3" runat="server" DataIndex="PROFILE_OPTION_TYPE" Text="Type" Flex="1" />
                        <ext:Column ID="Column2" runat="server" DataIndex="PROFILE_OWNER_NAME" Text="Owner Name" Flex="1" />
                        <ext:Column ID="Column1" runat="server" DataIndex="PROFILE_VALUE" Text="Value" Flex="1" />
                    </Columns>
                </ColumnModel>
                <Plugins>
                    <ext:FilterHeader ID="uxProfileOptionValuesFilter" runat="server" Remote="true" />
                </Plugins>
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
                </BottomBar>
                <View>
                    <ext:GridView ID="GridView1" StripeRows="true" runat="server">
                    </ext:GridView>
                    </View>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>

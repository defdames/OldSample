<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSecurityActivityList.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Security.umSecurityActivityList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager runat="server"  IsDynamic="False" RethrowAjaxExceptions="true">
    </ext:ResourceManager>
    <form id="test" runat="server">
        <ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout" IDMode="Explicit" IsDynamic="False" Namespace="App" RenderXType="True">
            <Items>
                <ext:GridPanel ID="uxSecurityActivityGridPanel"
                    runat="server"
                    Title="System Activitys"
                    Padding="5"
                    Icon="User"
                    Region="Center"
                    Frame="true"
                    Margins="5 5 5 5"
                    SelectionMemory="false">
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" Text="Add Activity" Icon="UserAdd" ID="uxAddActivity">
                                    <Listeners>
                                        <Click Handler="#{uxSecurityAddActivityWindow}.show();#{uxName}.focus();"></Click>
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server"></ext:ToolbarSpacer>
                                <ext:Button runat="server" Text="Edit Activity" Icon="UserEdit" Disabled="true" ID="uxEditActivity">
                                     <DirectEvents>
                                        <Click OnEvent="deEditActivity">
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                               <ext:ToolbarSpacer runat="server"></ext:ToolbarSpacer>
                                <ext:Button runat="server" Text="Delete Activity" Icon="UserDelete" Disabled="true" ID="uxDeleteActivity">
                                    <DirectEvents>
                                        <Click OnEvent="deDeleteActivity">
                                            <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete this Activity?"></Confirmation>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store
                            ID="uxSecurityActivityStore"
                            runat="server" 
                            RemoteSort="true" 
                            PageSize="25"
                            OnReadData="deActivitysDatabind">
                        <Proxy>
                        <ext:PageProxy />
                        </Proxy>
                            <Model>
                                <ext:Model ID="uxSecurityActivityModel" runat="server" IDProperty="ACTIVITY_ID">
                                    <Fields>
                                        <ext:ModelField Name="ACTIVITY_ID" Type="Int" />
                                        <ext:ModelField Name="NAME" Type="String" />
                                        <ext:ModelField Name="DESCRIPTION" Type="String" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                             <Sorters>
                        <ext:DataSorter Property="NAME" Direction="ASC" />
                    </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="uxSecurityActivityColumns" runat="server">
                        <Columns>
                            <ext:Column ID="cName" runat="server" DataIndex="NAME" Text="Name" Width="225" />
                            <ext:Column ID="cDescription" runat="server" DataIndex="DESCRIPTION" Text="Description" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Features>
                        <ext:GridFilters runat="server" ID="uxSecurityActivityGridFilters" Local="true">
                            <Filters>
                                <ext:StringFilter DataIndex="NAME" />
                                <ext:StringFilter DataIndex="DESCRIPTION" />
                            </Filters>
                        </ext:GridFilters>
                    </Features>
                    <BottomBar>
                        <ext:PagingToolbar ID="uxSecurityActivityPaging" runat="server" />
                    </BottomBar>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel  ID="uxSecurityActivitySelectionModel" runat="server" Mode="Single" ShowHeaderCheckbox="false" AllowDeselect="true">
                            <Listeners>
                                <Select Handler="#{uxEditActivity}.enable();#{uxDeleteActivity}.enable();"></Select>
                                <Deselect Handler="#{uxEditActivity}.disable();#{uxDeleteActivity}.disable();"></Deselect>
                            </Listeners>
                        </ext:CheckboxSelectionModel>
                    </SelectionModel>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>

        <!-- Hidden Window for Adding Security Activitys -->
        <ext:Window runat="server" Resizable="false" Icon="UserAdd" DefaultButton="uxSaveActivity" Hidden="true" Width="350" Height="150" Layout="FitLayout" Header="true" Title="Security Activity Maintenance" ID="uxSecurityAddActivityWindow" Closable="true" CloseAction="Hide" Modal="true">
            <Items>
                <ext:FormPanel
                    ID="uxSecurityActivityDetails"
                    runat="server"
                    Margins="5 5 5 5"
                    BodyPadding="2"
                    Frame="true"
                    DefaultAnchor="100%"
                    AutoScroll="True">
                    <Items>
                        <ext:TextField Name="ACTIVITY_ID" id="uxActivityID" Hidden="true" runat="server"></ext:TextField>
                        <ext:TextField Name="NAME" ID="uxName" runat="server" FieldLabel="Name" />
                        <ext:TextField Name="DESCRIPTION" ID="uxDescription" runat="server" FieldLabel="Description" />
                    </Items>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button runat="server" ID="uxSaveActivity" Text="Save" Icon="Disk">
                    <DirectEvents>
                        <Click OnEvent="deSaveActivity">
                            <EventMask ShowMask="true"></EventMask>
                        </Click>
                    </DirectEvents>
                </ext:Button>
                <ext:Button runat="server" ID="uxCancelUserActivity" Text="Cancel">
                    <Listeners>
                        <Click Handler="#{uxSecurityActivityDetails}.getForm().reset();#{uxSecurityAddActivityWindow}.close();"></Click>
                    </Listeners>
                </ext:Button>
            </Buttons>
            <Listeners>
                <Close Handler="#{uxSecurityActivityDetails}.getForm().reset();"></Close>
            </Listeners>
        </ext:Window>

</form>
</body>
</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umManageExisting.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umManageExisting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
        <ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout" IDMode="Explicit" IsDynamic="False" Namespace="App" RenderXType="True">
            <Items>
                <ext:MenuPanel runat="server" ID="uxMenuPanel" Region="West" Weight="60">
                    <Menu runat="server">
                        <Items>
                            <ext:MenuItem Text="Create Activity" runat="server" ID="uxCreate" Href="umDailyActivity.aspx"/>
                            <ext:MenuItem Text="Manage Existing" runat="server" ID="uxManage" />
                        </Items>
                    </Menu>
                </ext:MenuPanel>
                <ext:GridPanel runat="server" ID="uxManageGrid" Region="North">
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" />
                    </SelectionModel>
                    <Store>
                        <ext:Store runat="server" AutoDataBind="true" ID="uxManageGridStore">
                            <Fields>
                                <ext:ModelField Name="HEADER_ID" />
                                <ext:ModelField Name="PROJECT_ID" />
                                <ext:ModelField Name="DA_DATE" />
                                <ext:ModelField Name="SUBDIVISION" />
                                <ext:ModelField Name="CONTRACTOR" />
                                <ext:ModelField Name="PERSON_ID" />
                                <ext:ModelField Name="LICENSE" />
                                <ext:ModelField Name="STATE" />
                                <ext:ModelField Name="APPLICATION_TYPE" />
                                <ext:ModelField Name="DENSITY" />
                                <ext:ModelField Name="CREATE_DATE" />
                                <ext:ModelField Name="MODIFY_DATE" />
                                <ext:ModelField Name="CREATED_BY" />
                                <ext:ModelField Name="MODIFIED_BY" />
                                <ext:ModelField Name="STATUS" />
                            </Fields>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="License" DataIndex="LICENSE" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
                <ext:TabPanel runat="server" ID="uxTabPanel" Region="Center">
                    <Items>
                        <ext:Panel runat="server" 
                            Title="Header"
                            ID="uxHeaderTab">
                            <Loader runat="server" Url="umHeaderTab.aspx">
                                <LoadMask ShowMask="true" />
                            </Loader>                                                        
                        </ext:Panel>
                        <ext:Panel runat="server"
                            Title="Equipment"
                            ID="uxEquipmentTab">
                            <Loader runat="server" Url="umEquipmentTab.aspx">
                                <LoadMask ShowMask="true" />
                            </Loader>
                        </ext:Panel>
                        <ext:Panel runat="server"
                            Title="Production"
                            ID="uxProductionTab">
                            <Loader runat="server" Url="umProductionTab.aspx">
                                <LoadMask ShowMask="true" />
                            </Loader>
                        </ext:Panel>
                        <ext:Panel runat="server"
                            Title="Employees"
                            ID="uxEmployeeTab">
                            <Loader runat="server" Url="umEmployeesTab.aspx">
                                <LoadMask ShowMask="true" />
                            </Loader>
                        </ext:Panel>
                        <ext:Panel runat="server"
                            Title="Chemical Mix"
                            ID="uxChemicalTab">
                            <Loader runat="server" Url="umChemicalTab.aspx">
                                <LoadMask ShowMask="true" />
                            </Loader>
                        </ext:Panel>
                    </Items>
                </ext:TabPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umPrintMultipleWindow.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umPrintMultipleWindow" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:GridPanel runat="server" ID="uxHeaderPostGrid"
                    Layout="HBoxLayout" Region="Center">
                    <Store>
                        <ext:Store runat="server" ID="uxHeaderPostStore"
                            AutoDataBind="true"
                            OnReadData="deReadPostableData"
                            PageSize="8"
                            RemoteSort="false">
                            <Model>
                                <ext:Model ID="Model1" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="HEADER_ID" />
                                        <ext:ModelField Name="DA_DATE" Type="Date" />
                                        <ext:ModelField Name="SEGMENT1" />
                                        <ext:ModelField Name="LONG_NAME" />
                                        <ext:ModelField Name="STATUS" Type="Int" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                              <Sorters>
                                  <ext:DataSorter Property="DA_DATE" Direction="DESC" />
                                  <ext:DataSorter Property="STATUS" Direction="ASC" />
                              </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column1" runat="server" Text="DRS Number" DataIndex="HEADER_ID" Flex="11" />
                            <ext:DateColumn ID="DateColumn1" runat="server" DataIndex="DA_DATE" Format="MM-dd-yyyy" Text="Date" Flex="15" />
                            <ext:Column ID="Column2" runat="server" DataIndex="SEGMENT1" Text="Project" Flex="10" />
                            <ext:Column ID="Column3" runat="server" DataIndex="LONG_NAME" Text="Project Name" Flex="64" />
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" Mode="Simple" />
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
                    </BottomBar>
                    <Plugins>
                        <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" DateFormat="MM-dd-yyyy" />
                    </Plugins>
                    <Buttons>
                        <ext:Button runat="server" ID="uxPostMultipleButton" Text="Save" Icon="Add">
                            <DirectEvents>
                                <Click OnEvent="dePrintMultiple" IsUpload="true" Before="parent.App.uxPlaceholderWindow.hide()">
                                    <ExtraParams>
                                        <ext:Parameter Name="RowsToPrint" Value="Ext.encode(#{uxHeaderPostGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="uxCancelPostButton" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="parentAutoLoadControl.close();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

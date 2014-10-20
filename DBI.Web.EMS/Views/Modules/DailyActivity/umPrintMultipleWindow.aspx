<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umPrintMultipleWindow.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umPrintMultipleWindow" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
        <ext:GridPanel runat="server" ID="uxHeaderPostGrid"
            Layout="HBoxLayout" Height="400" Width="700">
            <Store>
                <ext:Store runat="server" ID="uxHeaderPostStore"
                    AutoDataBind="true"
                    OnReadData="deReadPostableData"
                    PageSize="10"
                    RemoteSort="true">
                    <Model>
                        <ext:Model ID="Model1" runat="server">
                            <Fields>
                                <ext:ModelField Name="HEADER_ID" />
                                <ext:ModelField Name="DA_DATE" Type="Date" />
                                <ext:ModelField Name="SEGMENT1" />
                                <ext:ModelField Name="LONG_NAME" />
                            </Fields>
                        </ext:Model>
                    </Model>
                    <Proxy>
                        <ext:PageProxy />
                    </Proxy>
                    <Sorters>
                        <ext:DataSorter Property="HEADER_ID" Direction="ASC" />
                    </Sorters>
                </ext:Store>
            </Store>
            <ColumnModel>
                <Columns>
                    <ext:Column ID="Column1" runat="server" Text="DRS Id" DataIndex="HEADER_ID" Flex="10" />
                    <ext:DateColumn ID="DateColumn1" runat="server" DataIndex="DA_DATE" Format="MM-dd-yyyy" Text="Activity Date" Flex="15" />
                    <ext:Column ID="Column2" runat="server" DataIndex="SEGMENT1" Text="Project" Flex="10" />
                    <ext:Column ID="Column3" runat="server" DataIndex="LONG_NAME" Text="Project Name" Flex="65" />
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
                        <Click OnEvent="dePrintMultiple" IsUpload="true" Before="parent.App.uxPlaceholderWindow.hide()" >
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
            <Listeners>
                <AfterRender
                    Handler="var win = parentAutoLoadControl.target || parentAutoLoadControl, //you can use just 'parentAutoLoadControl' after update to Ext.NET v2 beta.
									size = this.getSize();
 
								size.height += 70;
								size.width += 12;
								win.setSize(size);"
                    Delay="100" />
            </Listeners>
        </ext:GridPanel>
    </form>
</body>
</html>

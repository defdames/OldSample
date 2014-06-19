<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umPostMultipleWindow.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umPostMultipleWindow" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
        <ext:GridPanel runat="server" ID="uxHeaderPostGrid"
            Layout="HBoxLayout" Height="400">
            <Store>
                <ext:Store runat="server" ID="uxHeaderPostStore"
                    AutoDataBind="true"
                    OnReadData="deReadPostableData"
                    PageSize="10"
                    RemoteSort="true">
                    <Model>
                        <ext:Model runat="server">
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
                </ext:Store>
            </Store>
            <ColumnModel>
                <Columns>
                    <ext:DateColumn runat="server" DataIndex="DA_DATE" Format="MM-dd-yyyy" Text="Activity Date" Flex="15" />
                    <ext:Column runat="server" DataIndex="SEGMENT1" Text="Project" Flex="15" />
                    <ext:Column runat="server" DataIndex="LONG_NAME" Text="Project Name" Flex="70" />
                </Columns>
            </ColumnModel>
            <SelectionModel>
                <ext:CheckboxSelectionModel runat="server" Mode="Multi" />
            </SelectionModel>
            <BottomBar>
                <ext:PagingToolbar runat="server" />
            </BottomBar>
            <Plugins>
                <ext:FilterHeader runat="server" Remote="true" DateFormat="MM-dd-yyyy" />
            </Plugins>
            <Buttons>
                <ext:Button runat="server" ID="uxPostMultipleButton" Text="Save" Icon="Add">
                    <DirectEvents>
                        <Click OnEvent="dePostData">
                            <ExtraParams>
                                <ext:Parameter Name="RowsToPost" Value="Ext.encode(#{uxHeaderPostGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                            </ExtraParams>
                            <EventMask ShowMask="true" />
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

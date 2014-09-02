<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umReorderDetailSheets.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.umReorderDetailSheets" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var closeUpdate = function () {
            parent.App.direct.CloseReorderDetailSheetsWindow();
            parent.Ext.getCmp('uxReorderDetailSheetsForm').close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>

                <ext:FormPanel ID="FormPanel1"
                    runat="server"
                    Region="Center"
                    BodyPadding="20"
                    Disabled="false">
                    <Items>

                        <ext:FieldContainer ID="FieldContainer1"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:GridPanel ID="uxDetailSheetOrderGrid" runat="server" Height="230" Width="340" SortableColumns="false">
                                    <Store>
                                        <ext:Store runat="server"
                                            ID="uxDetailSheetOrderStore"
                                            OnReadData="deLoadDetailSheetNames"
                                            AutoDataBind="true"
                                            WarningOnDirty="false">
                                            <Model>
                                                <ext:Model ID="Model1" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="DETAIL_TASK_ID" />
                                                        <ext:ModelField Name="PROJECT_ID" />
                                                        <ext:ModelField Name="DETAIL_NAME" />
                                                        <ext:ModelField Name="SHEET_ORDER" />
                                                        <ext:ModelField Name="COMMENTS" />
                                                        <ext:ModelField Name="CREATE_DATE" />
                                                        <ext:ModelField Name="CREATED_BY" />
                                                        <ext:ModelField Name="MODIFY_DATE" />
                                                        <ext:ModelField Name="MODIFIED_BY" />
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
                                            <ext:Column ID="Column2" runat="server" DataIndex="DETAIL_NAME" Text="Detail Sheet Name" Flex="6" />
                                        </Columns>
                                    </ColumnModel>
                                    <View>
                                        <ext:GridView ID="GridView1" runat="server">
                                            <Plugins>
                                                <ext:GridDragDrop ID="GridDragDrop1" runat="server" DragText="Drag and drop to reorder" />
                                            </Plugins>
                                        </ext:GridView>
                                    </View>
                                </ext:GridPanel>
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer3"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label1" runat="server" Width="360" Text="* Click on sheet name and drag to reorder." />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer2"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label4" runat="server" Width="360" Height="30" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer17"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label3" runat="server" Width="265" />
                                <ext:Button ID="uxUpdate" runat="server" Text="Close" Width="75" Disabled="false">
                                    <DirectEvents>
                                        <Click OnEvent="deUpdate">
                                            <ExtraParams>
                                                <ext:Parameter Name="Values" Value="Ext.encode(#{uxDetailSheetOrderGrid}.getRowsValues())" Mode="Raw" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="Processing..." />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                </ext:FormPanel>

            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

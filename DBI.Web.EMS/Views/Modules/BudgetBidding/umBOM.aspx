<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umBOM.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.umBOM" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        var closeCancel = function () {
            parent.Ext.getCmp('uxBOMForm').close();
        }
        var closeUpdate = function () {
            parent.App.direct.CloseBOMWindow();
            //parent.App.uxSummaryGridStore.reload();
            parent.Ext.getCmp('uxBOMForm').close();
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

                        <ext:FieldContainer ID="FieldContainer4"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label6" runat="server" Width="100" Text="Please select:" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer1"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:DropDownField ID="uxBOM" runat="server" Width="545" Mode="ValueText" Editable="false">
                                    <Listeners>
                                        <Expand Handler="this.picker.setWidth(550);" />
                                    </Listeners>
                                    <Component>
                                        <ext:GridPanel runat="server"
                                            ID="uxBOMList"
                                            Width="500"
                                            Layout="HBoxLayout"
                                            Frame="true"
                                            ForceFit="true">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxBOMStore"
                                                    PageSize="5"
                                                    RemoteSort="true"
                                                    OnReadData="deLoadBOMDropdown">
                                                    <Model>
                                                        <ext:Model ID="Model7" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="BILL_SEQUENCE_ID" />
                                                                <ext:ModelField Name="ORGANIZATION_ID" />
                                                                <ext:ModelField Name="DESCRIPTION" />
                                                                <ext:ModelField Name="SEGMENT1" />
                                                                <ext:ModelField Name="ATTRIBUTE15" />
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
                                                    <ext:Column ID="Column2" runat="server" Text="Customer Number" DataIndex="ATTRIBUTE15" Flex="1" />
                                                    <ext:Column ID="Column14" runat="server" Text="Inventory Number" DataIndex="SEGMENT1" Flex="1" />
                                                    <ext:Column ID="Column13" runat="server" Text="Description" DataIndex="DESCRIPTION" Flex="3" />
                                                </Columns>
                                            </ColumnModel>
                                            <BottomBar>
                                                <ext:PagingToolbar ID="PagingToolbar3" runat="server" />
                                            </BottomBar>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel3" runat="server" Mode="Single" />
                                            </SelectionModel>
                                            <DirectEvents>
                                                <SelectionChange OnEvent="deSelectBOM">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="BillSeqID" Value="#{uxBOMList}.getSelectionModel().getSelection()[0].data.BILL_SEQUENCE_ID" Mode="Raw" />
                                                        <ext:Parameter Name="BOMDesc" Value="#{uxBOMList}.getSelectionModel().getSelection()[0].data.DESCRIPTION" Mode="Raw" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" />
                                                </SelectionChange>
                                            </DirectEvents>
                                            <Plugins>
                                                <ext:FilterHeader runat="server" ID="uxBOMFilter" Remote="true" />
                                            </Plugins>
                                        </ext:GridPanel>
                                    </Component>
                                </ext:DropDownField>
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer2"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label4" runat="server" Width="560" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:GridPanel ID="uxBOMItemsGrid" runat="server" Height="120">
                            <SelectionModel>
                                <ext:RowSelectionModel ID="uxBOMGridRowModel" runat="server" AllowDeselect="false" Mode="Single" />
                            </SelectionModel>
                            <Store>
                                <ext:Store runat="server"
                                    ID="uxBOMGridStore"
                                    OnReadData="deReadBOMGridData"
                                    AutoDataBind="true"
                                    WarningOnDirty="false">
                                    <Model>
                                        <ext:Model ID="Model1" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="COMPONENT_ITEM_ID" />
                                                <ext:ModelField Name="DESCRIPTION" />
                                                <ext:ModelField Name="PRIMARY_UOM_CODE" />
                                                <ext:ModelField Name="COMPONENT_QUANTITY" />
                                                <ext:ModelField Name="ITEM_COST" />
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
                                    <ext:Column ID="Column5" runat="server" DataIndex="DESCRIPTION" Text="Description" Flex="3" />
                                    <ext:Column ID="Column8" runat="server" DataIndex="ITEM_COST" Text="Unit Cost" Flex="1" />
                                    <ext:Column ID="Column6" runat="server" DataIndex="PRIMARY_UOM_CODE" Text="UOM" Flex="1" />
                                    <ext:Column ID="Column7" runat="server" DataIndex="COMPONENT_QUANTITY" Text="Qty" Flex="1" />
                                </Columns>
                            </ColumnModel>
                            <%--<DirectEvents>
                                <Select OnEvent="deGetFormData">
                                    <ExtraParams>
                                        <ext:Parameter Name="BudBidProjectID" Value="#{uxSummaryGrid}.getSelectionModel().getSelection()[0].data.BUD_BID_PROJECTS_ID" Mode="Raw" />
                                        <ext:Parameter Name="ProjectNumID" Value="#{uxSummaryGrid}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
                                        <ext:Parameter Name="Type" Value="#{uxSummaryGrid}.getSelectionModel().getSelection()[0].data.TYPE" Mode="Raw" />
                                        <ext:Parameter Name="ProjectNum" Value="#{uxSummaryGrid}.getSelectionModel().getSelection()[0].data.PROJECT_NUM" Mode="Raw" />
                                        <ext:Parameter Name="ProjectName" Value="#{uxSummaryGrid}.getSelectionModel().getSelection()[0].data.PROJECT_NAME" Mode="Raw" />
                                    </ExtraParams>
                                    <EventMask ShowMask="true" />
                                </Select>
                            </DirectEvents>--%>
                        </ext:GridPanel>

                        <ext:FieldContainer ID="FieldContainer3"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label5" runat="server" Width="560" Height="40" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer17"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label3" runat="server" Width="365" />
                                <ext:Button ID="uxUpdate" runat="server" Text="Add Items" Icon="Add" Width="85" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deAddBOM">
                                            <EventMask ShowMask="true" Msg="Updating..." />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Label ID="Label16" runat="server" Width="5" />
                                <ext:Button ID="uxCancel" runat="server" Text="Cancel" Icon="Delete" Width="85">
                                    <Listeners>
                                        <Click Fn="closeCancel" />
                                    </Listeners>
                                    <DirectEvents>
                                        <Click>
                                            <EventMask ShowMask="true" Msg="Canceling..." />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                </ext:FormPanel>

                <ext:Hidden ID="uxHidBOMBillSeqID" runat="server" />

            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

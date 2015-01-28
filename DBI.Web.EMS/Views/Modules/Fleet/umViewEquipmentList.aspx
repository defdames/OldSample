<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umViewEquipmentList.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Fleet.umViewEquipmentList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
      <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" Namespace="App" ClientIDMode="Static" />         
       
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
                <ext:GridPanel ID="uxOrganizationEquipmentList" runat="server" Flex="1" Header="false" Margin="5" Region="Center" Scroll="Both" SelectionMemory="true">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server"></ext:ToolbarFill>     
                                <ext:Button ID="uxPrintLabels" runat="server" Text="Print Thermal Labels" Icon="Printer" Disabled="false">
                                    <DirectEvents>
                                        <Click OnEvent="dePrintLabels" Timeout="560000"><EventMask ShowMask="true" Msg="Generating Report"></EventMask></Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Plugins>
                        <ext:FilterHeader ID="uxOrganizationEquipmentListGridFilter" runat="server" Remote="true"  />
                    </Plugins>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxOrganizationEquipmentListStore"
                            AutoDataBind="true" RemoteSort="true" AutoLoad="true" OnReadData="uxOrganizationEquipmentListStore_ReadData" >
                            <Model>
                                <ext:Model ID="Model2" runat="server" IDProperty="PROJECT_ID">
                                    <Fields>
                                        <ext:ModelField Name="SEGMENT1" />
                                        <ext:ModelField Name="NAME" />
                                        <ext:ModelField Name="LONG_NAME" />
                                        <ext:ModelField Name="ORGANIZATION_NAME" />
                                        <ext:ModelField Name="DESCRIPTION" />
                                        <ext:ModelField Name="ATTRIBUTE1" />
                                        <ext:ModelField Name="ATTRIBUTE2" />
                                        <ext:ModelField Name="ATTRIBUTE3" />
                                        <ext:ModelField Name="ATTRIBUTE4" />
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
                            <ext:Column ID="Column14" runat="server" DataIndex="SEGMENT1" Text="Project Number" Locked="false" Sortable="true" Draggable="false" Flex="1">
                            </ext:Column>
                             <ext:Column ID="Column1" runat="server" DataIndex="NAME" Text="Name" Locked="false" Sortable="true" Draggable="false" Flex="1">
                            </ext:Column>
                            <ext:Column ID="Column2" runat="server" DataIndex="LONG_NAME" Text="Description" Locked="false" Sortable="true" Draggable="false" Flex="1">
                            </ext:Column>
                            <ext:Column ID="Column3" runat="server" DataIndex="ORGANIZATION_NAME" Text="Organization" Locked="false" Sortable="true" Draggable="false" Flex="1">
                            </ext:Column>
                            <ext:Column ID="Column4" runat="server" DataIndex="DESCRIPTION" Text="Equipment Number" Locked="false" Sortable="true" Draggable="false" Flex="1">
                            </ext:Column>
                            <ext:Column ID="Column5" runat="server" DataIndex="ATTRIBUTE1" Text="Year" Locked="false" Sortable="true" Draggable="false" Flex="1">
                            </ext:Column>
                              <ext:Column ID="Column6" runat="server" DataIndex="ATTRIBUTE2" Text="Manufacturer" Locked="false" Sortable="true" Draggable="false" Flex="1">
                            </ext:Column>
                              <ext:Column ID="Column7" runat="server" DataIndex="ATTRIBUTE3" Text="Model" Locked="false" Sortable="true" Draggable="false" Flex="1">
                            </ext:Column>
                              <ext:Column ID="Column8" runat="server" DataIndex="ATTRIBUTE4" Text="VIN" Locked="false" Sortable="true" Draggable="false" Flex="1">
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel runat="server" Mode="Simple" ID="uxEquipmentSelection"></ext:CheckboxSelectionModel>
                    </SelectionModel>
                    <View>
                        <ext:GridView ID="GridView2" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View>
                    <BottomBar>
                        <ext:PagingToolbar runat="server"></ext:PagingToolbar>
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>


        
    </form>
</body>
</html>

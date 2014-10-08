<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umBillingMain.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umBillingMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    
      <ext:ResourceManager ID="ResourceManager2" runat="server" />

        <div>
            <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
                <Items>
                         <ext:Toolbar ID="Toolbar1" runat="server" Region="North">
                        <Items>
                             <ext:ComboBox ID="uxRailRoadCI"
                                                runat="server"
                                                FieldLabel="Railroad"
                                                LabelAlign="Right"
                                                DisplayField="RAILROAD"
                                                ValueField="RAILROAD_ID"
                                                QueryMode="Local"
                                                TypeAhead="true" Editable="false">
                                                <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxAddRailRoadStore">
                                                        <Model>
                                                            <ext:Model ID="Model4" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="RAILROAD_ID" />
                                                                    <ext:ModelField Name="RAILROAD" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                    </ext:Store>
                                                </Store>
                                           <DirectEvents>
                                               <Select OnEvent="deLoadUnit" />
                                           </DirectEvents>
                                            </ext:ComboBox>
                            
                        </Items>
                    </ext:Toolbar>
                      <ext:Window
                        runat="server"
                        ID="uxChangeRailroadWindow"
                        Hidden="true"
                        Width="350"
                        Height="350"
                        Modal="true" Closable="false">
                        <Items>

                            <ext:GridPanel ID="uxRailroadGrid" runat="server" Title="Select Railroad" Height="325" Closable="false">
                              
                                <Store>
                                    <ext:Store runat="server"
                                        ID="uxRailRoadStore" OnReadData="deReadRRTypes" AutoDataBind="true"
                                        WarningOnDirty="false">
                                        <Model>
                                            <ext:Model ID="Model1" runat="server" IDProperty="RAILROAD_ID">
                                                <Fields>
                                                    <ext:ModelField Name="RAILROAD_ID" />
                                                    <ext:ModelField Name="RAILROAD" />
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
                                        <ext:Column ID="Column7" runat="server" DataIndex="RAILROAD" Text="Railroad" Flex="1" />
                                    </Columns>
                                </ColumnModel>
                               <DirectEvents>
                                   <Select OnEvent="deLoadRR" />
                               </DirectEvents>
                                    <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" />
                                </SelectionModel>
                            </ext:GridPanel>
                        </Items>
                    </ext:Window>

                    <%--<ext:Panel runat="server" ID="rrToolbar" Region="North" >
                        <Loader runat="server"
                          ID="Loader5" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umRailroadToolbar.aspx">
                        <LoadMask ShowMask="true" />
                         </Loader>
                    </ext:Panel>--%>
                    <ext:TabPanel ID="uxBillingTab" runat="server" Region="Center">
                        <Items>                       
                            <ext:Panel runat="server"
                                Title="Invoicing Form"
                                ID="uxBillingInvoiceTab"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader4" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umBillingInvoice.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>
                                </ext:Panel>
                              
                             <ext:Panel runat="server"
                                Title="Invoice Supplemental"
                                ID="uxSupplementalBilling"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader3" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umSupplementalBilling.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>
                            </ext:Panel>   
                                              
                           <%--  <ext:Panel runat="server"
                                Title="Test"
                                ID="uxInvoiceReview"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader1" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="MaintenanceInvoicePDF.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>
                            </ext:Panel>                     --%>
                        </Items>
                    </ext:TabPanel>

                </Items>
            </ext:Viewport>
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddOverheadDetailLine.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umAddOverheadDetailLine" %>

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
                   <ext:GridPanel ID="GridPanel3" runat="server" Flex="1" Title="Line Detail" Header="false" Padding="5" Region="Center" DefaultButton="uxSaveDetailLine" >
                    <Store>
                        <ext:Store runat="server"
                            ID="uxDetailStore"
                            AutoDataBind="true" RemoteSort="true" OnReadData="deLoadDetailLinesStore" AutoLoad="true">
                            <Model>
                                <ext:Model ID="Model4" runat="server" IDProperty="BUDGET_DETAIL_ID">
                                    <Fields>
                                        <ext:ModelField Name="PERIOD_NAME"></ext:ModelField>
                                        <ext:ModelField Name="AMOUNT"></ext:ModelField>
                                        <ext:ModelField Name="CODE_COMBINATION_ID"></ext:ModelField>
                                        <ext:ModelField Name="PERIOD_NUM"></ext:ModelField>
                                        <ext:ModelField Name="ORG_BUDGET_ID"></ext:ModelField>
                                        <ext:ModelField Name="DETAIL_TYPE"></ext:ModelField>
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
                            <ext:Column ID="Column116" runat="server" DataIndex="PERIOD_NAME" Text="Period" Flex="1" />
                            <ext:Column ID="Column117" runat="server" DataIndex="AMOUNT" Text="Amount" Flex="1" >
                                <Editor>
                                    <ext:NumberField runat="server" AllowBlank="false" ID="uxEditAmount" SelectOnFocus="true">
                                         <Listeners>
                                              <Show Handler="this.el.dom.select();" Delay="150" />
                                          </Listeners>
                                      </ext:NumberField>
                                </Editor>
                                <Renderer Fn="Ext.util.Format.numberRenderer('0,0.00')" />
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                     <View>
                        <ext:GridView ID="GridView4" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View> 
                       <Plugins>
                           <ext:CellEditing runat="server" ClicksToEdit="1"></ext:CellEditing>
                       </Plugins>
                       <BottomBar>
                           <ext:Toolbar runat="server">
                               <Items>
                                   <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                                   <ext:Button runat="server" ID="uxSaveDetailLine" Text="Save" Icon="Disk">
                                       <DirectEvents>
                                           <Click OnEvent="deSaveDetailLine" Success="parent.Ext.getCmp('uxDetailLineMaintenance').close();"><Confirmation ConfirmRequest="true" Message="Are you sure you want to save this detail line?"></Confirmation><EventMask ShowMask="true"></EventMask>
                                               <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel3}.getRowsValues())" Mode="Raw" />
                                             </ExtraParams>
                                           </Click>
                                       </DirectEvents>
                                   </ext:Button>
                                    <ext:Button runat="server" ID="uxCancelDetailLine" Text="Cancel" Icon="Cancel">
                                        <Listeners>
                                            <Click Handler="parent.Ext.getCmp('uxDetailLineMaintenance').close();" />
                                        </Listeners>
                                    </ext:Button>
                               </Items>
                           </ext:Toolbar>
                       </BottomBar>
                </ext:GridPanel>

                </Items>
            </ext:Viewport>
    </form>
</body>
</html>



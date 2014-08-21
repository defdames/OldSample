<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umImportActualsWindow.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umImportActualsWindow" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
     <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" Namespace="App" ClientIDMode="Static" >
        </ext:ResourceManager>         
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
                <ext:GridPanel ID="uxPeriodImportGridPanel" runat="server" Flex="1" Header="false" Padding="5" Region="East" Frame="true"   Margins="5 5 5 5">
                    <KeyMap ID="KeyMap1" runat="server">
                        <Binding>
                            <ext:KeyBinding Handler="#{uxImportSelected}.fireEvent('click');">
                                <Keys>
                                    <ext:Key Code="ENTER" />
                                    <ext:Key Code="RETURN" />
                                </Keys>
                            </ext:KeyBinding>
                        </Binding>
                    </KeyMap>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxDetailStore"
                            AutoDataBind="true" RemoteSort="true" AutoLoad="true" OnReadData="uxDetailStore_ReadData">
                            <Model>
                                <ext:Model ID="Model4" runat="server" IDProperty="PERIOD_NUM">
                                    <Fields>
                                        <ext:ModelField Name="ENTERED_PERIOD_NAME"></ext:ModelField>
                                        <ext:ModelField Name="PERIOD_NUM"></ext:ModelField>
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
                            <ext:Column ID="Column116" runat="server" DataIndex="ENTERED_PERIOD_NAME" Text="Period" Flex="1"  >
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <View>
                        <ext:GridView ID="GridView4" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View> 
                    <SelectionModel>
                        <ext:CheckboxSelectionModel runat="server" Mode="Simple" AllowDeselect="true" ID="uxPeriodSelectionModel"></ext:CheckboxSelectionModel>
                    </SelectionModel>
                       <Buttons>
                           <ext:Button runat="server" ID="uxImportSelected" Text="Import" Icon="DatabaseCopy" AutoFocus="true" TabIndex="2">
                               <DirectEvents>
                                   <Click OnEvent="deImportActuals" Success="parent.Ext.getCmp('uxImportActualsWn').close();"><Confirmation ConfirmRequest="true" Message="Are you sure you want to import actuals for these dates? This will overwrite your current numbers with actual numbers. You can not go back, Are you sure?"></Confirmation>
                                       <EventMask ShowMask="true" Msg="Import actuals, please Wait..."></EventMask>
                                   </Click>
                               </DirectEvents>
                                   </ext:Button>
                                    <ext:Button runat="server" ID="uxCloseButton" Text="Cancel" Icon="Cancel">
                                        <Listeners>
                                            <Click Handler="parent.Ext.getCmp('uxImportActualsWn').close();" />
                                        </Listeners>
                                    </ext:Button>
                       </Buttons> 
                </ext:GridPanel>
                <ext:FormPanel ID="FormPanel2" runat="server" Header="true" BodyPadding="5" Frame="true"
                    Margins="5 5 5 5" Region="Center" Title="Information" Icon="Information" Layout="FitLayout" Flex="1" >
                    <Items>
                      <ext:FieldContainer ID="FieldContainer2" 
                        runat="server"
                        LabelStyle="font-weight:bold;padding:0;"
                        Layout="FitLayout">
                        <Items>
                         <ext:TextArea ID="uxAccountComments" runat="server" Flex="1" Grow="true" ReadOnly="true">
                         </ext:TextArea>
                        </Items>
                    </ext:FieldContainer> 
                    </Items>
                    </ext:FormPanel>
                </Items>
            </ext:Viewport>
    </form>
</body>
</html>



 
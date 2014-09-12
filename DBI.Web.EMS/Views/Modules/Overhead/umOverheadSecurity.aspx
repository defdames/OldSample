<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umOverheadSecurity.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umOverheadSecurity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout" Namespace="App" IDMode="Explicit">
            <Items>
                <ext:Panel runat="server" Layout="AccordionLayout" Width="250" Region="West" Collapsible="true" ID="uxAccordingPanel">
                    <Items>
                        <ext:TreePanel
                    ID="uxOrganizationTreePanel"
                    runat="server"
                    Title="Bussiness Units"
                    AutoScroll="true"
                    RootVisible="false"
                    SingleExpand="true"
                    Lines="false"
                    UseArrows="true"
                    Scroll="Vertical"
                    >
                    <Store>
                        <ext:TreeStore ID="uxOrganizationTreeStore" runat="server" OnReadData="deLoadLegalEntities">
                            <Proxy>
                                <ext:PageProxy></ext:PageProxy>
                            </Proxy>
                            <DirectEvents>
                                <BeforeLoad><EventMask ShowMask="true"></EventMask></BeforeLoad>
                            </DirectEvents>
                        </ext:TreeStore>
                    </Store>
                    <Root>
                        <ext:Node NodeID="0" Expanded="true" />
                    </Root>
                    <SelectionModel>
                        <ext:TreeSelectionModel ID="TreeSelectionModel1" runat="server" Mode="Single" AllowDeselect="true">
                            <DirectEvents>
                                <Select OnEvent="deSelectNode" ><EventMask ShowMask="true"></EventMask>
                                    <ExtraParams>
                                        <ext:Parameter Mode="Raw" Value="record.data.text" Name="ORGANIZATION_NAME"></ext:Parameter>
                                    </ExtraParams>
                                </Select>
                            </DirectEvents>
                        </ext:TreeSelectionModel>
                    </SelectionModel>
                       <View>
                        <ext:TreeView ID="TreeView1" runat="server" LoadMask="true">
                        </ext:TreeView>
                    </View>
                </ext:TreePanel>

                        <ext:MenuPanel ID="uxSystem"
                            runat="server"
                            Title="System Maintenance"
                            Icon="Database" Hidden="true">
                            <Menu ID="Menu2" runat="server">
                                <Items>
                                    <ext:MenuItem Text="Import Actuals" Icon="PageAttach">
                                        <Listeners>
                                            <Click Handler="#{uxImportActuals}.show();"></Click>
                                        </Listeners>
                                    </ext:MenuItem>
                                    <ext:MenuItem Text="Export to Oracle" Disabled="true" Icon="PageCopy" />
                                    <ext:MenuItem Text="Create Fiscal Period" Disabled="true" />
                                </Items>
                            </Menu>
                        </ext:MenuPanel>
                    </Items>
                </ext:Panel>





                <ext:TabPanel runat="server" DeferredRender="true" Region="Center" ID="uxCenterTabPanel" Padding="5" >
                    <Items>
                        <ext:Panel runat="server" Title="Dashboard" Html="This is for a future release that allows a dashboard view for the system admin. Uncompleted budgets, notifications etc.">
                        </ext:Panel>
                    </Items>
                </ext:TabPanel>
                
            </Items>
        </ext:Viewport>



        <ext:Window ID="uxImportActuals" runat="server" Header="true" Title="Import Actuals from Oracle" Width="800" Height="375" Frame="true" Hidden="true" Layout="BorderLayout" Modal="true"  Closable="true" CloseAction="Hide">
            <Items>
                <ext:FormPanel runat="server" Region="West" Flex="1" Header="false" Padding="5" Frame="true" Margins="5 5 5 5">
                    <Items>
                         <ext:FieldContainer ID="FieldContainer2" 
                        runat="server"
                        LabelStyle="font-weight:bold;padding:0;"
                        Layout="HBoxLayout">
                        <Items>
                            <ext:ComboBox runat="server" ID="uxBussinessUnit" Editable="true" TypeAhead="true"
                                FieldLabel="Bussiness Unit" AnchorHorizontal="55%" DisplayField="ID_NAME"
                                ValueField="ID" TriggerAction="All" 
                                MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" >
                                <Store>
                                    <ext:Store runat="server" ID="uxBussinessUnitStore"  AutoLoad="true" OnReadData="uxBussinessUnitStore_ReadData" AutoDataBind="true" >
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                        <Model>
                                            <ext:Model ID="Model3" runat="server" IDProperty="ID">
                                                <Fields>
                                                    <ext:ModelField Name="ID" />
                                                    <ext:ModelField Name="ID_NAME" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>
                        </Items>
                    </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer1" 
                        runat="server"
                        LabelStyle="font-weight:bold;padding:0;"
                        Layout="HBoxLayout">
                        <Items>
                            <ext:ComboBox runat="server" ID="uxFiscalYear" Editable="true" TypeAhead="true"
                                FieldLabel="Fiscal Year" AnchorHorizontal="55%" DisplayField="ID_NAME"
                                ValueField="ID_NAME" TriggerAction="All" 
                                MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" >
                                <Store>
                                    <ext:Store runat="server" ID="uxFiscalYearsStore"  AutoLoad="true" AutoDataBind="true" OnReadData="uxFiscalYearsStore_ReadData" >
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                        <Model>
                                            <ext:Model ID="Model5" runat="server" IDProperty="ID_NAME">
                                                <Fields>
                                                    <ext:ModelField Name="ID_NAME" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                                <Listeners>
                                    <Select Handler="#{uxDetailStore}.removeAll(true);
                                        #{uxDetailStore}.reload();" />
                                </Listeners>
                            </ext:ComboBox>

                        </Items>
                    </ext:FieldContainer>
                       </Items>
                </ext:FormPanel>

                <ext:GridPanel ID="uxPeriodImportGridPanel" runat="server" Flex="1" Header="false" Padding="5" Region="Center" Frame="true"   Margins="5 5 5 5">
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
                            AutoDataBind="true" RemoteSort="true" AutoLoad="false" OnReadData="uxDetailStore_ReadData">
                            <Model>
                                <ext:Model ID="Model4" runat="server" IDProperty="PERIOD_NUM">
                                    <Fields>
                                        <ext:ModelField Name="ENTERED_PERIOD_NAME"></ext:ModelField>
                                        <ext:ModelField Name="PERIOD_NUM"></ext:ModelField>
                                        <ext:ModelField Name="ACTUALS_IMPORTED_FLAG"></ext:ModelField>
                                          <ext:ModelField Name="ADMIN"></ext:ModelField>
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
                        <ext:CheckboxSelectionModel runat="server" Mode="Simple" AllowDeselect="true" ID="uxPeriodSelectionModel" >
                             <Listeners>
                                 <Select Handler="if(#{uxPeriodSelectionModel}.getCount() > 0){#{uxImport}.enable();}else {#{uxImport}.disable();}"></Select>
                                  <Deselect Handler="if(#{uxPeriodSelectionModel}.getCount() > 0){#{uxImport}.enable();}else {#{uxImport}.disable();}"></Deselect>
                                 </Listeners>
                        </ext:CheckboxSelectionModel>
                    </SelectionModel>
                </ext:GridPanel>
                    </Items>
                                <Buttons>
                        <ext:Button runat="server" ID="uxImport" Text="Import Actuals" Disabled="true" icon="DatabaseCopy">
                            <DirectEvents>
                                <Click OnEvent="deOverheadImportActuals"><EventMask ShowMask="true" Msg="Import Actuals, Please Wait..."></EventMask></Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="uxCloseButton" runat="server" Text="Close Form"><Listeners><Click Handler="#{uxImportActuals}.close();"></Click></Listeners></ext:Button>
                    </Buttons>
        </ext:Window>


    </form>
</body>
</html>

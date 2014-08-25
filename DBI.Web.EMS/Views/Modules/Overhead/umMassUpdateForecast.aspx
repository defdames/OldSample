<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umMassUpdateForecast.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umMassUpdateForecast" %>

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
                 <ext:Panel ID="Panel1" 
                    runat="server" 
                    Region="North"
                    Margins="5 5 5 5"
                    Title="Information" 
                    Height="75" 
                    BodyPadding="5"
                    Frame="true" 
                    Icon="Information">
                    <Content>
                        <b>Open Periods</b>
                        <b>Edit Periods</b>
                    </Content>
                </ext:Panel>

                 <ext:GridPanel ID="uxGlAccountSecurityGrid" runat="server" Flex="1" Header="false" Title="Budget Versions By Organization" Region="Center" CollapseDirection="Top"  Margin="5" SelectionMemory="true" >
                     <TopBar>
                         <ext:Toolbar runat="server">
                             <Items>
                                 <ext:Button runat="server" Text="Open" Icon="FolderMagnify" ID="uxMassEdit" Disabled="true">
                                     <Listeners>
                                         <Click Handler="#{FormPanel1}.reset();#{uxOpenCloseBudget}.show();"></Click>
                                     </Listeners>
                                 </ext:Button>
                                 <ext:Button runat="server" Text="Close" Icon="Cancel" ID="uxClose" Disabled="true">
                                     <Listeners>
                                         <Click Handler="#{FormPanel1}.reset();#{uxOpenCloseBudget}.show();"></Click>
                                     </Listeners>
                                 </ext:Button>
                                  <ext:Button runat="server" Text="Lock" Icon="Lock" ID="uxPending" Disabled="true">
                                     <Listeners>
                                         <Click Handler="#{FormPanel1}.reset();#{uxOpenCloseBudget}.show();"></Click>
                                     </Listeners>
                                 </ext:Button>
                             </Items>
                         </ext:Toolbar>
                     </TopBar>
                     <Store>
                        <ext:Store runat="server"
                            ID="uxBudgetVersionByOrganizationStore"
                            AutoDataBind="true" RemoteSort="true" AutoLoad="true" OnReadData="uxBudgetVersionByOrganizationStore_ReadData" >
                            <Model>
                                <ext:Model ID="Model2" runat="server" IDProperty="ORGANIZATION_ID">
                                    <Fields>
                                        <ext:ModelField Name="ACCOUNT_RANGE" />
                                        <ext:ModelField Name="ORGANIZATION_NAME" />
                                        <ext:ModelField Name="ORGANIZATION_ID" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                           <Sorters>
                               <ext:DataSorter Property="ORGANIZATION_NAME" Direction="ASC"></ext:DataSorter>
                           </Sorters>
                        </ext:Store>
                    </Store>                 
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column17" runat="server" DataIndex="ORGANIZATION_NAME" Text="Organization Name" Flex="1" />
                            <ext:Column ID="Column1" runat="server" DataIndex="ACCOUNT_RANGE" Text="Account Range" Flex="1" />
                             <ext:CommandColumn ID="CommandColumn1" runat="server" Width="180">
                                <Commands>
                                    <ext:GridCommand Text="Edit Periods" CommandName="periods" Icon="NoteEdit"></ext:GridCommand>
                                </Commands>
                                  <DirectEvents>
                                    <Command OnEvent="deExecuteCommand">
                                        <ExtraParams>
                                            <ext:Parameter Mode="Raw" Name="Name" Value="record.data.ORGANIZATION_NAME"></ext:Parameter>
                                            <ext:Parameter Mode="Raw" Name="ID" Value="record.data.ORGANIZATION_ID"></ext:Parameter>
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>    
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="uxGlAccountSecurityGridFilter" runat="server" Remote="true" />
                    </Plugins>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel runat="server" AllowDeselect="true" Mode="Simple" ID="uxOrganizationSelectionModel">
                            <Listeners>
                               <Select Handler="if(#{uxOrganizationSelectionModel}.getCount() > 0){#{uxMassEdit}.enable();}else {#{uxMassEdit}.disable();}"></Select>
                                        <Deselect Handler="if(#{uxOrganizationSelectionModel}.getCount() > 0){#{uxMassEdit}.enable();}else {#{uxMassEdit}.disable();}"></Deselect>
                            </Listeners>
                        </ext:CheckboxSelectionModel>
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
                    </BottomBar>
                     <View>
                        <ext:GridView ID="GridView1" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View>                     
                </ext:GridPanel>
                </Items>
            </ext:Viewport>

        <ext:Window runat="server" Resizable="false" Title="Open Forecast Periods" Width="400" Height="200" Layout="FitLayout" Hidden="true" CloseAction="Hide" Closable="true" ID="uxOpenCloseBudget">
            <Items>
                <ext:FormPanel ID="FormPanel1" runat="server" Header="false" BodyPadding="10" DefaultButton="uxAddBudgetType"
                    Margins="5 5 5 5" >
                    <Items>
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
                                    <ext:Store runat="server" ID="uxFiscalYearsStore" OnReadData="deLoadFiscalYears" AutoLoad="false" AutoDataBind="true" >
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
                                    <Select Handler="#{uxBudgetName}.clearValue();
                                        #{uxBudgetNameStore}.removeAll(true);
                                        #{uxBudgetName}.enable();
                                        #{uxBudgetNameStore}.reload();" />
                                </Listeners>
                            </ext:ComboBox>

                        </Items>
                    </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer3" 
                        runat="server"
                        LabelStyle="font-weight:bold;padding:0;"
                        Layout="HBoxLayout">
                        <Items>
                           <ext:ComboBox runat="server" ID="uxBudgetName" Editable="true" TypeAhead="true" Disabled="true"
                                FieldLabel="Budget Name" AnchorHorizontal="55%" DisplayField="BUDGET_DESCRIPTION"
                                ValueField="OVERHEAD_BUDGET_TYPE_ID" TriggerAction="All" 
                                MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" >
                                <Store>
                                    <ext:Store runat="server" ID="uxBudgetNameStore" OnReadData="deLoadBudgetNames" AutoLoad="false" AutoDataBind="true" >
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                        <Model>
                                            <ext:Model ID="Model1" runat="server" IDProperty="OVERHEAD_BUDGET_TYPE_ID">
                                                <Fields>
                                                    <ext:ModelField Name="BUDGET_NAME" />
                                                    <ext:ModelField Name="BUDGET_DESCRIPTION" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>
                        </Items>
                    </ext:FieldContainer>  
                    </Items>
                  
                </ext:FormPanel>
            </Items>
             <Buttons>
                        <ext:Button runat="server" ID="uxOpenPeriod" Text="Open Period" Disabled="true" icon="ApplicationAdd">
                        </ext:Button>
                        <ext:Button ID="uxCloseButton" runat="server" Text="Close Form"><Listeners><Click Handler="#{uxOpenCloseBudget}.close();"></Click></Listeners></ext:Button>
                    </Buttons>
        </ext:Window>
        
        </form>
</body>
</html>

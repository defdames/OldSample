<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umManageOrgs.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umManageOrgs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../../Resources/StyleSheets/main.css" />
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" IsDynamic="false" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:TreePanel
                    ID="uxOrgPanel"
                    runat="server"
                    Title="Organizations"
                    BodyPadding="6"
                    Region="West"
                    Weight="100"
                    Width="300"
                    AutoScroll="true"
                    RootVisible="true"
                    Lines="false"
                    UseArrows="true">
                    <Store>
                        <ext:TreeStore ID="TreeStore1" runat="server" OnReadData="deLoadOrgTree">
                            <Proxy>
                                <ext:PageProxy></ext:PageProxy>
                            </Proxy>
                        </ext:TreeStore>
                    </Store>
                    <Root>
                        <ext:Node NodeID="0" Text="All Companies" Expanded="true" />
                    </Root>
                    <SelectionModel>
                        <ext:TreeSelectionModel ID="uxCompanySelectionModel" runat="server" Mode="Single" />
                    </SelectionModel>
                    <Listeners>
                        <ItemClick Handler="#{uxDollarStore}.reload(); #{uxAddDollarButton}.enable();" />
                    </Listeners>
                </ext:TreePanel>
                <ext:GridPanel runat="server" ID="uxDollarGrid" Title="Dollar Threshold" Region="North">
                    <Store>
                        <ext:Store runat="server" ID="uxDollarStore" AutoLoad="false" AutoDataBind="true" OnReadData="deReadDollars" PageSize="10">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="AMOUNT_ID" />
                                        <ext:ModelField Name="ORG_ID" />
                                        <ext:ModelField Name="ORG_HIER" />
                                        <ext:ModelField Name="LOW_DOLLAR_AMT" />
                                        <ext:ModelField Name="HIGH_DOLLAR_AMT" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="LOW_DOLLAR_AMT" Direction="ASC" />
                            </Sorters>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="Organization Name" DataIndex="ORG_HIER" Flex="50" />
                            <ext:Column runat="server" Text="Lower Amount" DataIndex="LOW_DOLLAR_AMT" Flex="25">
                                <Renderer Format="UsMoney" />
                            </ext:Column>
                            <ext:Column runat="server" Text="Upper Amount" DataIndex="HIGH_DOLLAR_AMT" Flex="25">
                                <Renderer Format="UsMoney" />
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                    </Plugins>
                    <Listeners>
                        <Select Handler="#{uxEditDollarButton}.enable(); #{uxDeleteDollarButton}.enable(); #{uxAddThresholdButton}.enable(); #{uxThresholdStore}.reload();" />
                    </Listeners>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxAddDollarButton" Icon="ApplicationAdd" Text="Add" Disabled="true">
                                    <Listeners>
                                        <Click Handler="#{uxAddEditDollarWindow}.show(); #{uxDollarFormType}.setValue('Add');" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxEditDollarButton" Icon="ApplicationEdit" Text="Edit" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deLoadDollarWindow">
                                            <ExtraParams>
                                                <ext:Parameter Name="AmountId" Value="#{uxDollarGrid}.getSelectionModel().getSelection()[0].data.AMOUNT_ID" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxDeleteDollarButton" Icon="ApplicationDelete" Text="Delete" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deDeleteDollar">
                                            <EventMask ShowMask="true" />
                                            <ExtraParams>
                                                <ext:Parameter Name="AmountId" Value="#{uxDollarGrid}.getSelectionModel().getSelection()[0].data.AMOUNT_ID" Mode="Raw" />
                                            </ExtraParams>
                                            <Confirmation ConfirmRequest="true" Title="Really Delete?" Message="Do you really want to delete this entry?" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>                       
                    </TopBar>
                    <BottomBar>
                        <ext:PagingToolbar runat="server" />
                    </BottomBar>
                </ext:GridPanel>
                <ext:GridPanel runat="server" ID="uxThresholdGrid" Region="Center" Title="Threshold Percentages">
                    <Store>
                        <ext:Store runat="server" ID="uxThresholdStore" AutoDataBind="true" AutoLoad="false" OnReadData="deReadThresholds">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="AMOUNT_ID" />
                                        <ext:ModelField Name="LOW_DOLLAR" ServerMapping="CUSTOMER_SURVEY_THRESH_AMT.LOW_DOLLAR_AMT" />
                                        <ext:ModelField Name="HIGH_DOLLAR" ServerMapping="CUSTOMER_SURVEY_THRESH_AMT.HIGH_DOLLAR_AMT" />
                                        <ext:ModelField Name="THRESHOLD" />
                                        <ext:ModelField Name="THRESHOLD_ID" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Parameters>
                                <ext:StoreParameter Name="AmountId" Value="#{uxDollarGrid}.getSelectionModel().getSelection()[0].data.AMOUNT_ID" Mode="Raw" />
                            </Parameters>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="Low Dollar" DataIndex="LOW_DOLLAR" />
                            <ext:Column runat="server" Text="High Dollar" DataIndex="HIGH_DOLLAR" />
                            <ext:Column runat="server" Text="% Threshold" DataIndex="THRESHOLD" />
                        </Columns>
                    </ColumnModel>
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxAddThresholdButton" Text="Add" Icon="ApplicationAdd" Disabled="true">
                                    <Listeners>
                                        <Click Handler="#{uxAddEditThresholdWindow}.show(); #{uxThresholdFormType}.setValue('Add')" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxEditThresholdButton" Text="Edit" Icon="ApplicationEdit" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deLoadThresholdForm">
                                            <EventMask ShowMask="true" />
                                            <ExtraParams>
                                                <ext:Parameter Name="RowValues" Value="Ext.encode(#{uxThresholdGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw"/>
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxDeleteThresholdButton" Text="Delete" Icon="ApplicationDelete" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deDeleteThreshold">
                                            <EventMask ShowMask="true" />
                                            <Confirmation Title="Really Delete?" Message="Do you really want to delete?" ConfirmRequest="true" />
                                            <ExtraParams>
                                                <ext:Parameter Name="ThresholdId" Value="#{uxThresholdGrid}.getSelectionModel().getSelection()[0].data.THRESHOLD_ID" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Listeners>
                        <Select Handler="#{uxEditThresholdButton}.enable(); #{uxDeleteThresholdButton}.enable()" />
                    </Listeners>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
        <ext:Window runat="server" ID="uxAddEditDollarWindow" Hidden="true" Modal="true" Title="Add Dollar Amounts" Width="350">
            <Items>
                <ext:FormPanel runat="server" ID="uxDollarForm" Layout="VBoxLayout">
                    <LayoutConfig>
                        <ext:VBoxLayoutConfig Align="Stretch" />
                    </LayoutConfig>
                    <Items>
                        <ext:Hidden runat="server" ID="uxDollarFormType" />
                        <ext:Hidden runat="server" ID="uxDollarAmountId" />
                        <ext:ComboBox runat="server" ID="uxFormTypeCombo" FieldLabel="Form Type" DisplayField="TYPE_NAME" ValueField="TYPE_ID">
                            <Store>
                                <ext:Store runat="server" ID="uxFormTypeStore" OnReadData="deReadFormTypes" AutoDataBind="true">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="TYPE_ID" />
                                                <ext:ModelField Name="TYPE_NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:PageProxy />
                                    </Proxy>
                                </ext:Store>
                            </Store>
                        </ext:ComboBox>
                        <ext:NumberField runat="server" ID="uxLowDollar" FieldLabel="Low Dollar Amount" MinValue="1" AllowBlank="false" AllowExponential="false" InvalidCls="allowBlank" MsgTarget="Side" IndicatorIcon="BulletRed" />
                        <ext:NumberField runat="server" ID="uxHighDollar" FieldLabel="High Dollar Amount" MinValue="1" AllowBlank="false" AllowExponential="false" InvalidCls="allowBlank" MsgTarget="Side" IndicatorIcon="BulletRed" />
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxSaveDollarButton" Text="Submit" Icon="Add">
                            <DirectEvents>
                                <Click OnEvent="deSaveDollar">
                                    <EventMask ShowMask="true" />
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="uxCancelDollarButton" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="#{uxAddEditDollarWindow}.hide(); #{uxDollarForm}.reset()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <Listeners>
                        <ValidityChange Handler="#{uxSaveDollarButton}.setDisabled(!valid)" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Window>
        <ext:Window runat="server" ID="uxAddEditThresholdWindow" Hidden="true" Modal="true" Title="Add Percentage" Width="250">
            <Items>
                <ext:FormPanel runat="server" ID="uxThresholdForm" Layout="FormLayout">
                    <Items>
                        <ext:Hidden runat="server" ID="uxThresholdFormType" />
                        <ext:Hidden runat="server" ID="uxThresholdId" />
                        <ext:NumberField runat="server" ID="uxThreshold" FieldLabel="Threshold in %" AllowBlank="false" MinValue="1" MaxValue="100" AllowDecimals="false" AllowExponential="false" InvalidCls="allowBlank" MsgTarget="Side" IndicatorIcon="BulletRed" />
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxSaveThresholdButton" Text="Submit" Icon="Add" Disabled="true">
                            <DirectEvents>
                                <Click OnEvent="deSaveThreshold">
                                    <EventMask ShowMask="true" />
                                    <ExtraParams>
                                        <ext:Parameter Name="AmountId" Value="#{uxDollarGrid}.getSelectionModel().getSelection()[0].data.AMOUNT_ID" Mode="Raw" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="uxCancelThresholdButton" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="#{uxAddEditThresholdWindow}.hide(); #{uxThresholdForm}.reset()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <Listeners>
                        <ValidityChange Handler="#{uxSaveThresholdButton}.setDisabled(!valid)" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>

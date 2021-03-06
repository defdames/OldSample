﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddRemoveBudgetType.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.Views.umAddRemoveBudgetType" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>

                  <ext:Panel ID="Panel1" 
                    runat="server" 
                    Region="North"
                    Margins="5 5 5 5"
                    Title="Information" 
                    Height="100" 
                    BodyPadding="5"
                    Frame="true" 
                    Icon="Information">
                    <Content>
                        <b>Oracle Budget Types</b>
                        <p>These budget types must first be created in Oracle before they can be assigned. If there are no budget names listed you have to create them first.</p>
                    </Content>
                </ext:Panel>

                <ext:FormPanel ID="FormPanel1" runat="server" Header="false" BodyPadding="10" DefaultButton="uxAddBudgetType"
                    Margins="5 5 5 5" Region="Center" >
                    <Items>
                        <ext:FieldContainer ID="FieldContainer1" 
                        runat="server"
                        LabelStyle="font-weight:bold;padding:0;"
                        Layout="HBoxLayout">
                        <Items>
                            
                            <ext:ComboBox runat="server" ID="uxBudgetName" Editable="true" TypeAhead="true"
                                FieldLabel="Budget Name" AnchorHorizontal="55%" DisplayField="BUDGET_NAME"
                                ValueField="BUDGET_NAME" TriggerAction="All" 
                                MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" >
                                <Store>
                                    <ext:Store runat="server" ID="uxBudgetNameStore" OnReadData="deLoadBudgetNames" AutoLoad="false" >
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                        <Model>
                                            <ext:Model ID="Model5" runat="server" IDProperty="BUDGET_NAME">
                                                <Fields>
                                                    <ext:ModelField Name="BUDGET_NAME" />
                                                    <ext:ModelField Name="DESCRIPTION" />
                                                    <ext:ModelField Name="LE_ORG_ID" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                                <Listeners>
                                    <Select Handler="#{uxBudgetDescription}.setValue(#{uxBudgetName}.getStore().getAt(#{uxBudgetName}.getStore().findExact('BUDGET_NAME',#{uxBudgetName}.getValue())).get('DESCRIPTION'));#{uxAddBudgetType}.enable();"></Select>
                                </Listeners>
                            </ext:ComboBox>
                        </Items>
                    </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer2" 
                        runat="server"
                        LabelStyle="font-weight:bold;padding:0;"
                        Layout="HBoxLayout">
                        <Items>
                         <ext:TextField runat="server" ID="uxBudgetDescription" FieldLabel="Description" AnchorHorizontal="55%" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" />  
                        </Items>
                    </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer3" 
                        runat="server"
                        LabelStyle="font-weight:bold;padding:0;"
                        Layout="HBoxLayout">
                        <Items>
                            <ext:ComboBox runat="server" ID="uxLinkedBudgetType" Editable="true" TypeAhead="true"
                                FieldLabel="Linked Type" AnchorHorizontal="55%" DisplayField="BUDGET_NAME"
                                ValueField="OVERHEAD_BUDGET_TYPE_ID" TriggerAction="All" 
                                MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" >
                                <Store>
                                    <ext:Store runat="server" ID="uxLinkedBudgetTypeStore" OnReadData="deLoadLinkedBudgetNames" AutoLoad="false" >
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                        <Model>
                                            <ext:Model ID="Model1" runat="server" IDProperty="OVERHEAD_BUDGET_TYPE_ID">
                                                <Fields>
                                                    <ext:ModelField Name="BUDGET_NAME" />
                                                    <ext:ModelField Name="OVERHEAD_BUDGET_TYPE_ID" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>
                        </Items>
                    </ext:FieldContainer>
                         <ext:FieldContainer ID="FieldContainer4" 
                        runat="server"
                        LabelStyle="font-weight:bold;padding:0;"
                        Layout="HBoxLayout">
                        <Items>
                            <ext:Checkbox runat="server" FieldLabel="Allow Import" ID="uxAllowImportCheckbox"></ext:Checkbox>
                        </Items>
                    </ext:FieldContainer>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxAddBudgetType" Text="Save" Disabled="true" icon="ApplicationAdd">
                            <DirectEvents>
                                <Click OnEvent="deAssignBudgetType" Success="parent.Ext.getCmp('uxAddEditBudgetType').close();"><EventMask ShowMask="true"></EventMask></Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="uxCloseButton" runat="server" Text="Close Form"><Listeners><Click Handler="parent.Ext.getCmp('uxAddEditBudgetType').close();"></Click></Listeners></ext:Button>
                    </Buttons>
                </ext:FormPanel>

                
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

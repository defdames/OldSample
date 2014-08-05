<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umOpenBudgetType.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umOpenBudgetType" %>

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
                  <ext:FormPanel ID="FormPanel1" runat="server" Header="false" BodyPadding="10" DefaultButton="uxAddBudgetType"
                    Margins="5 5 5 5" Region="Center" >
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
                    <Buttons>
                        <ext:Button runat="server" ID="uxOpenPeriod" Text="Create Period" Disabled="true" icon="ApplicationAdd">
                          <DirectEvents>
                              <Click OnEvent="deOpenPeriod" Success="parent.Ext.getCmp('uxOpenBudgetTypeWindow').close();" ><Confirmation ConfirmRequest="true" Message="Are you sure you want to create this budget period?"></Confirmation><EventMask ShowMask="true"></EventMask></Click>
                          </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="uxCloseButton" runat="server" Text="Close Form"><Listeners><Click Handler="parent.Ext.getCmp('uxOpenBudgetTypeWindow').close();"></Click></Listeners></ext:Button>
                    </Buttons>
                </ext:FormPanel>
                </Items>
            </ext:Viewport>
    </form>
</body>
</html>

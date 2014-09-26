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
                                    <Select Handler="#{uxOpenPeriod}.enable();" />
                                </Listeners>
                            </ext:ComboBox>

                        </Items>
                    </ext:FieldContainer>
                          
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxOpenPeriod" Text="Open Next Period" Disabled="true" icon="ApplicationAdd">
                          <DirectEvents>
                              <Click OnEvent="deOpenPeriod" Timeout="5000000" Success="parent.Ext.getCmp('uxOpenBudgetTypeWindow').close();" ><Confirmation ConfirmRequest="true"></Confirmation><EventMask ShowMask="true"></EventMask></Click>
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

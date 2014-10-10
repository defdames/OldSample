<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umImportActualsWindow.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umImportActualsWindow" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>
        var getRowClass = function (record, index) {
            if (record.data.ACTUALS_IMPORTED_FLAG == "Y" && record.data.ADMIN == "N") {
                return "red-row";
            }
        }

        var editAllowed = function (record) {
            if (record.data.ACTUALS_IMPORTED_FLAG == "Y" && record.data.ADMIN == "N") {
                return false;
            }
        }

        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }

    </script>
  
    <style>
        .my-disabled .x-grid-row-checker  {
            filter: alpha(opacity=60);
            opacity: 0.6;
        }

        .red-row .x-grid-cell, .red-row .x-grid-row-checker .red-row .x-grid-rowwrap-div .red-row .myBoldClass.x-grid3-row td  {
            color: red !important;
             filter: alpha(opacity=60);
            opacity: 0.6;
		}

    </style>
</head>
<body>
    <form id="form1" runat="server">
     <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" Namespace="App" ClientIDMode="Static" >
        </ext:ResourceManager>         
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel ID="FormPanel1" runat="server" Header="false" BodyPadding="10" Flex="1" DefaultButton="uxAddBudgetType"
                    Margins="5 5 5 5" Region="Center">
                    <Items>
                              <ext:FieldContainer ID="FieldContainer2" 
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
                                            <ext:Model ID="Model1" runat="server" IDProperty="ID_NAME">
                                                <Fields>
                                                    <ext:ModelField Name="ID_NAME" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                                <Listeners>
                                    <Select Handler="#{uxPeriodName}.enable();" />
                                </Listeners>
                            </ext:ComboBox>

                        </Items>
                    </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer1"
                            runat="server"
                            LabelStyle="font-weight:bold;padding:0;"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:ComboBox runat="server" ID="uxPeriodName" Editable="true" TypeAhead="true"
                                    FieldLabel="Select a Period" AnchorHorizontal="55%" DisplayField="ID_NAME"
                                    ValueField="ID" TriggerAction="All" Disabled="true"
                                    MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1">
                                    <Store>
                                        <ext:Store runat="server" ID="uxPeriodNameStore" OnReadData="deLoadPeriodNames" AutoLoad="false" AutoDataBind="true">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model5" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                        <ext:ModelField Name="ID_NAME" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                     <Listeners>
                                    <Select Handler="#{uxImportButton}.enable();" />
                                </Listeners>
                                </ext:ComboBox>
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxImportButton" Text="Import" Icon="DatabaseCopy" AutoFocus="true" TabIndex="2" Disabled="true">
                            <DirectEvents>
                                <Click OnEvent="deImportActuals" Timeout="300000" Success="if(getParameterByName('AdminImport') == 'Y'){parent.Ext.getCmp('uxImportActualsWn').close();}else{parent.Ext.getCmp('uxOrganizationAccountGridPanel').getStore().load();parent.Ext.getCmp('uxImportActualsWn').close();}"><EventMask ShowMask="true"></EventMask><Confirmation Message="Are you sure you want to import this period?" ConfirmRequest="true"></Confirmation></Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="Button2" Text="Cancel" Icon="Cancel">
                            <Listeners>
                                <Click Handler="parent.Ext.getCmp('uxImportActualsWn').close();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>

                </Items>
            </ext:Viewport>
    </form>
</body>
</html>



 
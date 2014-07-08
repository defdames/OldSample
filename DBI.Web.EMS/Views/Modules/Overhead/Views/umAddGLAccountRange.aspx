<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddGLAccountRange.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.Views.umAddGLAccountRange" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
       <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False"  />         
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>

                <ext:FormPanel ID="FormPanel2" 
            runat="server"
            BodyPadding="10"
                     Region="Center" Margins="5 5 5 5" Flex="1" 
            Layout="ColumnLayout">
            
            <FieldDefaults LabelAlign="Left" MsgTarget="Side" />
                    <Items>
                        <ext:FieldSet ID="FieldSet1"
                    runat="server" 
                    ColumnWidth="0.5"
                    Title="Low" 
                    MarginSpec="0 0 0 10">
                    <Defaults>
                        <ext:Parameter Name="Width" Value="250" />
                        <ext:Parameter Name="LabelWidth" Value="90" />
                    </Defaults>
                    <Items>
                                <ext:ComboBox FieldLabel="Company" runat="server" ID="uxSRSegment1" Editable="true" TypeAhead="true"
                                     AnchorHorizontal="-5" DisplayField="ID" ValueField="ID"
                                    MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                    <Store>
                                        <ext:Store runat="server" ID="uxSRSegment1Store" AutoLoad="false" OnReadData="deLoadSegment" AutoDataBind="true">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model1" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                        <Select Handler="#{uxSRSegment2}.enable();#{uxSRSegment3}.disable();#{uxSRSegment4}.disable();#{uxSRSegment5}.disable();#{uxSRSegment6}.disable();#{uxSRSegment7}.disable();this.getTrigger(0).show();#{uxSRSegment2}.clearValue();#{uxSRSegment2}.getTrigger(0).hide();#{uxSRSegment2Store}.reload();#{uxSRSegment3}.clearValue();#{uxSRSegment3}.getTrigger(0).hide();#{uxSRSegment3Store}.reload();#{uxSRSegment4}.clearValue();#{uxSRSegment4}.getTrigger(0).hide();#{uxSRSegment4Store}.reload();#{uxSRSegment5}.clearValue();#{uxSRSegment5}.getTrigger(0).hide();#{uxSRSegment5Store}.reload();#{uxSRSegment6}.clearValue();#{uxSRSegment6}.getTrigger(0).hide();#{uxSRSegment6Store}.reload();#{uxSRSegment7}.clearValue();#{uxSRSegment7}.getTrigger(0).hide();#{uxSRSegment7Store}.reload();" />
                                        <BeforeQuery Handler="this.getTrigger(0)[this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { 
                                           this.clearValue(); 
                                           this.getTrigger(0).hide();
                                       }" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox FieldLabel="Location" runat="server" ID="uxSRSegment2" Editable="true" TypeAhead="true" Disabled="true"
                                     AnchorHorizontal="-5" DisplayField="ID" ValueField="ID"
                                    MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                   <Store>
                                        <ext:Store runat="server" ID="uxSRSegment2Store" AutoLoad="false" OnReadData="deLoadSegment" AutoDataBind="true">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model3" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                        <Select Handler="#{uxSRSegment3}.enable();#{uxSRSegment4}.disable();#{uxSRSegment5}.disable();#{uxSRSegment6}.disable();#{uxSRSegment7}.disable();this.getTrigger(0).show();#{uxSRSegment3}.clearValue();#{uxSRSegment3}.getTrigger(0).hide();#{uxSRSegment3Store}.reload();#{uxSRSegment4}.clearValue();#{uxSRSegment4}.getTrigger(0).hide();#{uxSRSegment4Store}.reload();#{uxSRSegment5}.clearValue();#{uxSRSegment5}.getTrigger(0).hide();#{uxSRSegment5Store}.reload();#{uxSRSegment6}.clearValue();#{uxSRSegment6}.getTrigger(0).hide();#{uxSRSegment6Store}.reload();#{uxSRSegment7}.clearValue();#{uxSRSegment7}.getTrigger(0).hide();#{uxSRSegment7Store}.reload();" />
                                        <BeforeQuery Handler="this.getTrigger(0)[this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { 
                                           this.clearValue(); 
                                           this.getTrigger(0).hide();
                                       }" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox FieldLabel="Division" runat="server" ID="uxSRSegment3" Editable="true" TypeAhead="true" Disabled="true"
                                     AnchorHorizontal="-5" DisplayField="ID" ValueField="ID"
                                    MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                     <Store>
                                        <ext:Store runat="server" ID="uxSRSegment3Store" AutoLoad="false" OnReadData="deLoadSegment" AutoDataBind="true">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model5" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                        <Select Handler="#{uxSRSegment4}.enable();#{uxSRSegment5}.disable();#{uxSRSegment6}.disable();#{uxSRSegment7}.disable();this.getTrigger(0).show();#{uxSRSegment4}.clearValue();#{uxSRSegment4}.getTrigger(0).hide();#{uxSRSegment4Store}.reload();#{uxSRSegment5}.clearValue();#{uxSRSegment5}.getTrigger(0).hide();#{uxSRSegment5Store}.reload();#{uxSRSegment6}.clearValue();#{uxSRSegment6}.getTrigger(0).hide();#{uxSRSegment6Store}.reload();#{uxSRSegment7}.clearValue();#{uxSRSegment7}.getTrigger(0).hide();#{uxSRSegment7Store}.reload();" />
                                        <BeforeQuery Handler="this.getTrigger(0)[this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { 
                                           this.clearValue(); 
                                           this.getTrigger(0).hide();
                                       }" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox FieldLabel="Branch" runat="server" ID="uxSRSegment4" Editable="true" TypeAhead="true" Disabled="true"
                                     AnchorHorizontal="-5" DisplayField="ID" ValueField="ID"
                                    MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                     <Store>
                                        <ext:Store runat="server" ID="uxSRSegment4Store" AutoLoad="false" OnReadData="deLoadSegment" AutoDataBind="true">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model6" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                        <Select Handler="#{uxSRSegment5}.enable();#{uxSRSegment6}.disable();#{uxSRSegment7}.disable();this.getTrigger(0).show();#{uxSRSegment5}.clearValue();#{uxSRSegment5}.getTrigger(0).hide();#{uxSRSegment5Store}.reload();#{uxSRSegment6}.clearValue();#{uxSRSegment6}.getTrigger(0).hide();#{uxSRSegment6Store}.reload();#{uxSRSegment7}.clearValue();#{uxSRSegment7}.getTrigger(0).hide();#{uxSRSegment7Store}.reload();" />
                                        <BeforeQuery Handler="this.getTrigger(0)[this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { 
                                           this.clearValue(); 
                                           this.getTrigger(0).hide();
                                       }" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox FieldLabel="Account" runat="server" ID="uxSRSegment5" Editable="true" TypeAhead="true" Disabled="true"
                                    AnchorHorizontal="-5" DisplayField="ID" ValueField="ID"
                                    MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                    <Store>
                                        <ext:Store runat="server" ID="uxSRSegment5Store" AutoLoad="false" OnReadData="deLoadSegment" AutoDataBind="true">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model7" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                          <Select Handler="#{uxSRSegment6}.enable();#{uxSRSegment7}.disable();this.getTrigger(0).show();#{uxSRSegment6}.clearValue();#{uxSRSegment6}.getTrigger(0).hide();#{uxSRSegment6Store}.reload();#{uxSRSegment7}.clearValue();#{uxSRSegment7}.getTrigger(0).hide();#{uxSRSegment7Store}.reload();" />
                                        <BeforeQuery Handler="this.getTrigger(0)[this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { 
                                           this.clearValue(); 
                                           this.getTrigger(0).hide();
                                       }" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox FieldLabel="Type" runat="server" ID="uxSRSegment6" Editable="true" TypeAhead="true" Disabled="true"
                                   AnchorHorizontal="-5" DisplayField="ID" ValueField="ID"
                                    MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                    <Store>
                                        <ext:Store runat="server" ID="uxSRSegment6Store" AutoLoad="false" OnReadData="deLoadSegment" AutoDataBind="true">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model8" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                         <Select Handler="#{uxSRSegment7}.enable();this.getTrigger(0).show();#{uxSRSegment7}.clearValue();#{uxSRSegment7}.getTrigger(0).hide();#{uxSRSegment7Store}.reload();" />
                                        <BeforeQuery Handler="this.getTrigger(0)[this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { 
                                           this.clearValue(); 
                                           this.getTrigger(0).hide();
                                       }" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox FieldLabel="Future" runat="server" ID="uxSRSegment7" Editable="true" TypeAhead="true" Disabled="true"
                                     AnchorHorizontal="-5" DisplayField="ID" ValueField="ID"
                                    TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                     <Store>
                                        <ext:Store runat="server" ID="uxSRSegment7Store" AutoLoad="false" OnReadData="deLoadSegment" AutoDataBind="true">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model9" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                        <Select Handler="this.getTrigger(0).show();" />
                                        <BeforeQuery Handler="this.getTrigger(0)[this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { 
                                           this.clearValue(); 
                                           this.getTrigger(0).hide();
                                       }" />
                                    </Listeners>
                                </ext:ComboBox>
                    </Items>                    
                </ext:FieldSet>
                        <ext:FieldSet ID="FieldSet2"
                    runat="server" 
                    ColumnWidth="0.5"
                    Title="High" 
                    MarginSpec="0 0 0 10">
                    <Defaults>
                        <ext:Parameter Name="Width" Value="250" />
                        <ext:Parameter Name="LabelWidth" Value="90" />
                    </Defaults>
                    <Items>
                           <ext:ComboBox FieldLabel="Company" runat="server" ID="uxERSegment1" Editable="false" TypeAhead="false" Disabled="true"
                                     AnchorHorizontal="-5" DisplayField="ID" ValueField="ID"
                                    MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                    <Store>
                                        <ext:Store runat="server" ID="uxERSegment1Store" AutoLoad="false" OnReadData="deLoadSegment" AutoDataBind="true">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model2" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                        <Select Handler="this.getTrigger(0).show();" />
                                        <BeforeQuery Handler="this.getTrigger(0)[this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { 
                                           this.clearValue(); 
                                           this.getTrigger(0).hide();
                                       }" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox FieldLabel="Location" runat="server" ID="ComboBox3" Editable="false" TypeAhead="false" Disabled="true"
                                     AnchorHorizontal="-5" DisplayField="ID_NAME" ValueField="ID"
                                    MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                    <Store>
                                        <ext:Store runat="server" ID="Store3" AutoLoad="false">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model4" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                        <ext:ModelField Name="ID_NAME" />
                                                        <ext:ModelField Name="DESCRIPTION" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                        <Select Handler="this.getTrigger(0).show();#{uxSegment1Description}.setText(#{uxSegment1}.getStore().getAt(#{uxSegment1}.getStore().findExact('ID',#{uxSegment1}.getValue())).get('DESCRIPTION'));#{uxSegment2}.clearValue(); #{uxSegment2Store}.reload();#{uxSegment3}.clearValue(); #{uxSegment3Store}.reload();#{uxSegment4}.clearValue(); #{uxSegment4Store}.reload();if(#{uxGlAccountSecurityStore}.getCount() > 0){#{uxGlAccountSecurityStore}.reload();#{uxGlAccountSecurityGrid}.getView().refresh();#{uxGlAccountSecurityGridFilter}.clearFilter();}#{uxSegment2Description}.setText('');#{uxSegment3Description}.setText('');#{uxSegment4Description}.setText('');"></Select>
                                        <BeforeQuery Handler="this.getTrigger(0)[this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { 
                                           this.clearValue(); 
                                           this.getTrigger(0).hide();
                                        #{uxSegment1Description}.setText('');
                                       }" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox FieldLabel="Division" runat="server" ID="ComboBox9" Editable="false" TypeAhead="false" Disabled="true"
                                     AnchorHorizontal="-5" DisplayField="ID_NAME" ValueField="ID"
                                    MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                    <Store>
                                        <ext:Store runat="server" ID="Store9" AutoLoad="false">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model10" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                        <ext:ModelField Name="ID_NAME" />
                                                        <ext:ModelField Name="DESCRIPTION" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                        <Select Handler="this.getTrigger(0).show();#{uxSegment1Description}.setText(#{uxSegment1}.getStore().getAt(#{uxSegment1}.getStore().findExact('ID',#{uxSegment1}.getValue())).get('DESCRIPTION'));#{uxSegment2}.clearValue(); #{uxSegment2Store}.reload();#{uxSegment3}.clearValue(); #{uxSegment3Store}.reload();#{uxSegment4}.clearValue(); #{uxSegment4Store}.reload();if(#{uxGlAccountSecurityStore}.getCount() > 0){#{uxGlAccountSecurityStore}.reload();#{uxGlAccountSecurityGrid}.getView().refresh();#{uxGlAccountSecurityGridFilter}.clearFilter();}#{uxSegment2Description}.setText('');#{uxSegment3Description}.setText('');#{uxSegment4Description}.setText('');"></Select>
                                        <BeforeQuery Handler="this.getTrigger(0)[this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { 
                                           this.clearValue(); 
                                           this.getTrigger(0).hide();
                                        #{uxSegment1Description}.setText('');
                                       }" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox FieldLabel="Branch" runat="server" ID="ComboBox10" Editable="false" TypeAhead="false" Disabled="true"
                                     AnchorHorizontal="-5" DisplayField="ID_NAME" ValueField="ID"
                                    MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                    <Store>
                                        <ext:Store runat="server" ID="Store10" AutoLoad="false">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model11" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                        <ext:ModelField Name="ID_NAME" />
                                                        <ext:ModelField Name="DESCRIPTION" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                        <Select Handler="this.getTrigger(0).show();#{uxSegment1Description}.setText(#{uxSegment1}.getStore().getAt(#{uxSegment1}.getStore().findExact('ID',#{uxSegment1}.getValue())).get('DESCRIPTION'));#{uxSegment2}.clearValue(); #{uxSegment2Store}.reload();#{uxSegment3}.clearValue(); #{uxSegment3Store}.reload();#{uxSegment4}.clearValue(); #{uxSegment4Store}.reload();if(#{uxGlAccountSecurityStore}.getCount() > 0){#{uxGlAccountSecurityStore}.reload();#{uxGlAccountSecurityGrid}.getView().refresh();#{uxGlAccountSecurityGridFilter}.clearFilter();}#{uxSegment2Description}.setText('');#{uxSegment3Description}.setText('');#{uxSegment4Description}.setText('');"></Select>
                                        <BeforeQuery Handler="this.getTrigger(0)[this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { 
                                           this.clearValue(); 
                                           this.getTrigger(0).hide();
                                        #{uxSegment1Description}.setText('');
                                       }" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox FieldLabel="Account" runat="server" ID="ComboBox11" Editable="false" TypeAhead="false" Disabled="true"
                                    AnchorHorizontal="-5" DisplayField="ID_NAME" ValueField="ID"
                                    MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                    <Store>
                                        <ext:Store runat="server" ID="Store11" AutoLoad="false">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model12" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                        <ext:ModelField Name="ID_NAME" />
                                                        <ext:ModelField Name="DESCRIPTION" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                        <Select Handler="this.getTrigger(0).show();#{uxSegment1Description}.setText(#{uxSegment1}.getStore().getAt(#{uxSegment1}.getStore().findExact('ID',#{uxSegment1}.getValue())).get('DESCRIPTION'));#{uxSegment2}.clearValue(); #{uxSegment2Store}.reload();#{uxSegment3}.clearValue(); #{uxSegment3Store}.reload();#{uxSegment4}.clearValue(); #{uxSegment4Store}.reload();if(#{uxGlAccountSecurityStore}.getCount() > 0){#{uxGlAccountSecurityStore}.reload();#{uxGlAccountSecurityGrid}.getView().refresh();#{uxGlAccountSecurityGridFilter}.clearFilter();}#{uxSegment2Description}.setText('');#{uxSegment3Description}.setText('');#{uxSegment4Description}.setText('');"></Select>
                                        <BeforeQuery Handler="this.getTrigger(0)[this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { 
                                           this.clearValue(); 
                                           this.getTrigger(0).hide();
                                        #{uxSegment1Description}.setText('');
                                       }" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox FieldLabel="Type" runat="server" ID="ComboBox12" Editable="false" TypeAhead="false" 
                                   AnchorHorizontal="-5" DisplayField="ID_NAME" ValueField="ID"
                                    MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                    <Store>
                                        <ext:Store runat="server" ID="Store12" AutoLoad="false">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model13" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                        <ext:ModelField Name="ID_NAME" />
                                                        <ext:ModelField Name="DESCRIPTION" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                        <Select Handler="this.getTrigger(0).show();#{uxSegment1Description}.setText(#{uxSegment1}.getStore().getAt(#{uxSegment1}.getStore().findExact('ID',#{uxSegment1}.getValue())).get('DESCRIPTION'));#{uxSegment2}.clearValue(); #{uxSegment2Store}.reload();#{uxSegment3}.clearValue(); #{uxSegment3Store}.reload();#{uxSegment4}.clearValue(); #{uxSegment4Store}.reload();if(#{uxGlAccountSecurityStore}.getCount() > 0){#{uxGlAccountSecurityStore}.reload();#{uxGlAccountSecurityGrid}.getView().refresh();#{uxGlAccountSecurityGridFilter}.clearFilter();}#{uxSegment2Description}.setText('');#{uxSegment3Description}.setText('');#{uxSegment4Description}.setText('');"></Select>
                                        <BeforeQuery Handler="this.getTrigger(0)[this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { 
                                           this.clearValue(); 
                                           this.getTrigger(0).hide();
                                        #{uxSegment1Description}.setText('');
                                       }" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox FieldLabel="Future" runat="server" ID="ComboBox13" Editable="false" TypeAhead="false"
                                     AnchorHorizontal="-5" DisplayField="ID_NAME" ValueField="ID"
                                    TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                    <Store>
                                        <ext:Store runat="server" ID="Store13" AutoLoad="false">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model14" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                        <ext:ModelField Name="ID_NAME" />
                                                        <ext:ModelField Name="DESCRIPTION" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                        <Select Handler="this.getTrigger(0).show();#{uxSegment1Description}.setText(#{uxSegment1}.getStore().getAt(#{uxSegment1}.getStore().findExact('ID',#{uxSegment1}.getValue())).get('DESCRIPTION'));#{uxSegment2}.clearValue(); #{uxSegment2Store}.reload();#{uxSegment3}.clearValue(); #{uxSegment3Store}.reload();#{uxSegment4}.clearValue(); #{uxSegment4Store}.reload();if(#{uxGlAccountSecurityStore}.getCount() > 0){#{uxGlAccountSecurityStore}.reload();#{uxGlAccountSecurityGrid}.getView().refresh();#{uxGlAccountSecurityGridFilter}.clearFilter();}#{uxSegment2Description}.setText('');#{uxSegment3Description}.setText('');#{uxSegment4Description}.setText('');"></Select>
                                        <BeforeQuery Handler="this.getTrigger(0)[this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { 
                                           this.clearValue(); 
                                           this.getTrigger(0).hide();
                                        #{uxSegment1Description}.setText('');
                                       }" />
                                    </Listeners>
                                </ext:ComboBox>
                    </Items>                    
                </ext:FieldSet>
                    </Items>
            <Buttons>
                <ext:Button ID="uxShowAccounts" runat="server" Text="Accounts View" Icon="Find" />
                <ext:Button ID="uxCloseForm" runat="server" Text="Close Form" />
                 <ext:Button ID="uxClearFilter" runat="server" Text="Clear Filter" />
                <ext:Button ID="uxSaveRange" runat="server" Text="Save" />
            </Buttons>
        </ext:FormPanel>
     
            </Items>
        </ext:Viewport>
   </form>
</body>
</html>
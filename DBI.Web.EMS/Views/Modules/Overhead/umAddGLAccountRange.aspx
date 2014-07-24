<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddGLAccountRange.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umAddGLAccountRange" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
       <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" Namespace="App"  />         
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>

                <ext:FormPanel ID="uxAccountFormPanel" 
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
                                        <Select Handler="#{uxSRSegment2}.enable();
                                            #{uxSRSegment3}.disable();
                                            #{uxSRSegment4}.disable();
                                            #{uxSRSegment5}.disable();
                                            #{uxSRSegment6}.disable();
                                            #{uxSRSegment7}.disable();
                                            #{uxERSegment1}.disable();
                                            #{uxERSegment2}.disable();
                                            #{uxERSegment3}.disable();
                                            #{uxERSegment4}.disable();
                                            #{uxERSegment5}.disable();
                                            #{uxERSegment6}.disable();
                                            #{uxERSegment7}.disable();
                                            #{uxSRSegment2}.clearValue();
                                            #{uxSRSegment2Store}.removeAll(true);
                                            #{uxSRSegment2Store}.reload();
                                            #{uxSRSegment3}.clearValue();
                                            #{uxSRSegment3Store}.removeAll(true);
                                            #{uxSRSegment4}.clearValue();
                                            #{uxSRSegment4Store}.removeAll(true);
                                            #{uxSRSegment5}.clearValue();
                                            #{uxSRSegment5Store}.removeAll(true);
                                            #{uxSRSegment6}.clearValue();
                                            #{uxSRSegment6Store}.removeAll(true);
                                            #{uxSRSegment7}.clearValue();
                                            #{uxSRSegment7Store}.removeAll(true);
                                            #{uxERSegment1}.clearValue();
                                            #{uxERSegment1Store}.removeAll(true);
                                            #{uxERSegment2}.clearValue();
                                            #{uxERSegment2Store}.removeAll(true);
                                            #{uxERSegment3}.clearValue();
                                            #{uxERSegment3Store}.removeAll(true);
                                            #{uxERSegment4}.clearValue();
                                            #{uxERSegment4Store}.removeAll(true);
                                            #{uxERSegment5}.clearValue();
                                            #{uxERSegment5Store}.removeAll(true);
                                            #{uxERSegment6}.clearValue();
                                            #{uxERSegment6Store}.removeAll(true);
                                            #{uxERSegment7}.clearValue();
                                            #{uxERSegment7Store}.removeAll(true);
                                            #{uxERSegment1}.disable();
                                            #{uxERSegment2}.disable();
                                            #{uxERSegment3}.disable();
                                            #{uxERSegment4}.disable();
                                            #{uxERSegment5}.disable();
                                            #{uxERSegment6}.disable();
                                            #{uxERSegment7}.disable();
                                            #{uxShowAccounts}.disable();" />
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
                                        <Select Handler="#{uxSRSegment3}.enable();
                                            #{uxSRSegment4}.disable();
                                            #{uxSRSegment5}.disable();
                                            #{uxSRSegment6}.disable();
                                            #{uxSRSegment7}.disable();
                                            #{uxSRSegment3}.clearValue();
                                            #{uxSRSegment3Store}.removeAll(true);
                                            #{uxSRSegment3Store}.reload();
                                            #{uxSRSegment4}.clearValue();
                                            #{uxSRSegment4Store}.removeAll(true);
                                            #{uxSRSegment5}.clearValue();
                                            #{uxSRSegment5Store}.removeAll(true);
                                            #{uxSRSegment6}.clearValue();
                                            #{uxSRSegment6Store}.removeAll(true);
                                            #{uxSRSegment7}.clearValue();
                                            #{uxSRSegment7Store}.removeAll(true);
                                            #{uxERSegment1}.clearValue();
                                            #{uxERSegment1Store}.removeAll(true);
                                            #{uxERSegment2}.clearValue();
                                            #{uxERSegment2Store}.removeAll(true);
                                            #{uxERSegment3}.clearValue();
                                            #{uxERSegment3Store}.removeAll(true);
                                            #{uxERSegment4}.clearValue();
                                            #{uxERSegment4Store}.removeAll(true);
                                            #{uxERSegment5}.clearValue();
                                            #{uxERSegment5Store}.removeAll(true);
                                            #{uxERSegment6}.clearValue();
                                            #{uxERSegment6Store}.removeAll(true);
                                            #{uxERSegment7}.clearValue();
                                            #{uxERSegment7Store}.removeAll(true);
                                            #{uxERSegment1}.disable();
                                            #{uxERSegment2}.disable();
                                            #{uxERSegment3}.disable();
                                            #{uxERSegment4}.disable();
                                            #{uxERSegment5}.disable();
                                            #{uxERSegment6}.disable();
                                            #{uxERSegment7}.disable();
                                            #{uxShowAccounts}.disable();" />
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
                                        <Select Handler="#{uxSRSegment4}.enable();
                                            #{uxSRSegment5}.disable();
                                            #{uxSRSegment6}.disable();
                                            #{uxSRSegment7}.disable();
                                            #{uxSRSegment4}.clearValue();
                                            #{uxSRSegment4Store}.removeAll(true);
                                            #{uxSRSegment4Store}.reload();
                                            #{uxSRSegment5}.clearValue();
                                            #{uxSRSegment5Store}.removeAll(true);
                                            #{uxSRSegment6}.clearValue();
                                            #{uxSRSegment6Store}.removeAll(true);
                                            #{uxSRSegment7}.clearValue();
                                            #{uxSRSegment7Store}.removeAll(true);
                                            #{uxERSegment1}.clearValue();
                                            #{uxERSegment1Store}.removeAll(true);
                                            #{uxERSegment2}.clearValue();
                                            #{uxERSegment2Store}.removeAll(true);
                                            #{uxERSegment3}.clearValue();
                                            #{uxERSegment3Store}.removeAll(true);
                                            #{uxERSegment4}.clearValue();
                                            #{uxERSegment4Store}.removeAll(true);
                                            #{uxERSegment5}.clearValue();
                                            #{uxERSegment5Store}.removeAll(true);
                                            #{uxERSegment6}.clearValue();
                                            #{uxERSegment6Store}.removeAll(true);
                                            #{uxERSegment7}.clearValue();
                                            #{uxERSegment7Store}.removeAll(true);
                                            #{uxERSegment1}.disable();
                                            #{uxERSegment2}.disable();
                                            #{uxERSegment3}.disable();
                                            #{uxERSegment4}.disable();
                                            #{uxERSegment5}.disable();
                                            #{uxERSegment6}.disable();
                                            #{uxERSegment7}.disable();
                                            #{uxShowAccounts}.disable();" />
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
                                        <Select Handler="#{uxSRSegment5}.enable();
                                            #{uxSRSegment6}.disable();
                                            #{uxSRSegment7}.disable();
                                            #{uxSRSegment5}.clearValue();
                                            #{uxSRSegment5Store}.removeAll(true);
                                            #{uxSRSegment5Store}.reload();
                                            #{uxSRSegment6}.clearValue();
                                            #{uxSRSegment6Store}.removeAll(true);
                                            #{uxSRSegment7}.clearValue();
                                            #{uxSRSegment7Store}.removeAll(true);
                                            #{uxERSegment1}.clearValue();
                                            #{uxERSegment1Store}.removeAll(true);
                                            #{uxERSegment2}.clearValue();
                                            #{uxERSegment2Store}.removeAll(true);
                                            #{uxERSegment3}.clearValue();
                                            #{uxERSegment3Store}.removeAll(true);
                                            #{uxERSegment4}.clearValue();
                                            #{uxERSegment4Store}.removeAll(true);
                                            #{uxERSegment5}.clearValue();
                                            #{uxERSegment5Store}.removeAll(true);
                                            #{uxERSegment6}.clearValue();
                                            #{uxERSegment6Store}.removeAll(true);
                                            #{uxERSegment7}.clearValue();
                                            #{uxERSegment7Store}.removeAll(true);
                                            #{uxERSegment1}.disable();
                                            #{uxERSegment2}.disable();
                                            #{uxERSegment3}.disable();
                                            #{uxERSegment4}.disable();
                                            #{uxERSegment5}.disable();
                                            #{uxERSegment6}.disable();
                                            #{uxERSegment7}.disable();
                                            #{uxShowAccounts}.disable();" />
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
                                        <Select Handler="#{uxSRSegment6}.enable();
                                              #{uxSRSegment7}.disable();
                                              #{uxSRSegment6}.clearValue();
                                              #{uxSRSegment6Store}.removeAll(true);
                                              #{uxSRSegment6Store}.reload();
                                              #{uxSRSegment7}.clearValue();
                                            #{uxSRSegment7Store}.removeAll(true);
                                            #{uxERSegment1}.clearValue();
                                            #{uxERSegment1Store}.removeAll(true);
                                            #{uxERSegment2}.clearValue();
                                            #{uxERSegment2Store}.removeAll(true);
                                            #{uxERSegment3}.clearValue();
                                            #{uxERSegment3Store}.removeAll(true);
                                            #{uxERSegment4}.clearValue();
                                            #{uxERSegment4Store}.removeAll(true);
                                            #{uxERSegment5}.clearValue();
                                            #{uxERSegment5Store}.removeAll(true);
                                            #{uxERSegment6}.clearValue();
                                            #{uxERSegment6Store}.removeAll(true);
                                            #{uxERSegment7}.clearValue();
                                            #{uxERSegment7Store}.removeAll(true);
                                            #{uxERSegment1}.disable();
                                            #{uxERSegment2}.disable();
                                            #{uxERSegment3}.disable();
                                            #{uxERSegment4}.disable();
                                            #{uxERSegment5}.disable();
                                            #{uxERSegment6}.disable();
                                            #{uxERSegment7}.disable();
                                            #{uxShowAccounts}.disable();" />
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
                                         <Select Handler="#{uxSRSegment7}.enable();
                                             #{uxSRSegment7}.clearValue();
                                             #{uxSRSegment7Store}.removeAll(true);
                                             #{uxSRSegment7Store}.reload();
                                             #{uxERSegment1}.clearValue();
                                             #{uxERSegment1Store}.removeAll(true);
                                             #{uxERSegment2}.clearValue();
                                             #{uxERSegment2Store}.removeAll(true);
                                             #{uxERSegment3}.clearValue();
                                             #{uxERSegment3Store}.removeAll(true);
                                             #{uxERSegment4}.clearValue();
                                             #{uxERSegment4Store}.removeAll(true);
                                             #{uxERSegment5}.clearValue();
                                             #{uxERSegment5Store}.removeAll(true);
                                             #{uxERSegment6}.clearValue();
                                             #{uxERSegment6Store}.removeAll(true);
                                             #{uxERSegment7}.clearValue();
                                             #{uxERSegment7Store}.removeAll(true);
                                             #{uxERSegment1}.disable();
                                             #{uxERSegment2}.disable();
                                             #{uxERSegment3}.disable();
                                             #{uxERSegment4}.disable();
                                             #{uxERSegment5}.disable();
                                             #{uxERSegment6}.disable();
                                             #{uxERSegment7}.disable();
                                             #{uxShowAccounts}.disable();" />
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
                                        <Select Handler="#{uxERSegment1}.enable();
                                            #{uxERSegment2}.disable();
                                            #{uxERSegment3}.disable();
                                            #{uxERSegment4}.disable();
                                            #{uxERSegment5}.disable();
                                            #{uxERSegment6}.disable();
                                            #{uxERSegment7}.disable();
                                            #{uxERSegment1}.clearValue();
                                            #{uxERSegment1Store}.removeAll(true);
                                            #{uxERSegment1Store}.reload();
                                            #{uxERSegment2}.clearValue();
                                            #{uxERSegment2Store}.removeAll(true);
                                            #{uxERSegment3}.clearValue();
                                            #{uxERSegment3Store}.removeAll(true);
                                            #{uxERSegment4}.clearValue();
                                            #{uxERSegment4Store}.removeAll(true);
                                            #{uxERSegment5}.clearValue();
                                            #{uxERSegment5Store}.removeAll(true);
                                            #{uxERSegment6}.clearValue();
                                            #{uxERSegment6Store}.removeAll(true);
                                            #{uxERSegment7}.clearValue();
                                            #{uxERSegment7Store}.removeAll(true);
                                            #{uxShowAccounts}.disable();"></Select>
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
                                <ext:ComboBox FieldLabel="Company" runat="server" ID="uxERSegment1" Editable="true" TypeAhead="true" Disabled="true" AlwaysMergeItems="false"
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
                                        <Select Handler="#{uxERSegment2}.enable();
                                            #{uxERSegment3}.disable();
                                            #{uxERSegment4}.disable();
                                            #{uxERSegment5}.disable();
                                            #{uxERSegment6}.disable();
                                            #{uxERSegment7}.disable();
                                            #{uxERSegment2}.clearValue();
                                            #{uxERSegment2Store}.removeAll(true);
                                            #{uxERSegment2Store}.reload();
                                            #{uxERSegment3}.clearValue();
                                            #{uxERSegment3Store}.removeAll(true);
                                            #{uxERSegment4}.clearValue();
                                            #{uxERSegment4Store}.removeAll(true);
                                            #{uxERSegment5}.clearValue();
                                            #{uxERSegment5Store}.removeAll(true);
                                            #{uxERSegment6}.clearValue();
                                            #{uxERSegment6Store}.removeAll(true);
                                            #{uxERSegment7}.clearValue();
                                            #{uxERSegment7Store}.removeAll(true);
                                            #{uxShowAccounts}.disable();" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox FieldLabel="Location" runat="server" ID="uxERSegment2" Editable="true" TypeAhead="true" Disabled="true"
                                     AnchorHorizontal="-5" DisplayField="ID" ValueField="ID"
                                    MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                   <Store>
                                        <ext:Store runat="server" ID="uxERSegment2Store" AutoLoad="false" OnReadData="deLoadSegment" AutoDataBind="true">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model4" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                        <Select Handler="#{uxShowAccounts}.disable();#{uxERSegment3}.enable();#{uxERSegment4}.disable();#{uxERSegment5}.disable();#{uxERSegment6}.disable();#{uxERSegment7}.disable();#{uxERSegment3}.clearValue();#{uxERSegment3Store}.removeAll(true);#{uxERSegment3Store}.reload();#{uxERSegment4}.clearValue();#{uxERSegment4Store}.removeAll(true);#{uxERSegment5}.clearValue();#{uxERSegment5Store}.removeAll(true);#{uxERSegment6}.clearValue();#{uxERSegment6Store}.removeAll(true);#{uxERSegment7}.clearValue();#{uxERSegment7Store}.removeAll(true);" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox FieldLabel="Division" runat="server" ID="uxERSegment3" Editable="true" TypeAhead="true" Disabled="true"
                                     AnchorHorizontal="-5" DisplayField="ID" ValueField="ID"
                                    MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                     <Store>
                                        <ext:Store runat="server" ID="uxERSegment3Store" AutoLoad="false" OnReadData="deLoadSegment" AutoDataBind="true">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model10" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                        <Select Handler="#{uxShowAccounts}.disable();#{uxERSegment4}.enable();#{uxERSegment5}.disable();#{uxERSegment6}.disable();#{uxERSegment7}.disable();#{uxERSegment4}.clearValue();#{uxERSegment4Store}.removeAll(true);#{uxERSegment4Store}.reload();#{uxERSegment5}.clearValue();#{uxERSegment5Store}.removeAll(true);#{uxERSegment6}.clearValue();#{uxERSegment6Store}.removeAll(true);#{uxERSegment7}.clearValue();#{uxERSegment7Store}.removeAll(true);" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox FieldLabel="Branch" runat="server" ID="uxERSegment4" Editable="true" TypeAhead="true" Disabled="true"
                                     AnchorHorizontal="-5" DisplayField="ID" ValueField="ID"
                                    MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                     <Store>
                                        <ext:Store runat="server" ID="uxERSegment4Store" AutoLoad="false" OnReadData="deLoadSegment" AutoDataBind="true">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model11" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                        <Select Handler="#{uxShowAccounts}.disable();#{uxERSegment5}.enable();#{uxERSegment6}.disable();#{uxERSegment7}.disable();#{uxERSegment5}.clearValue();#{uxERSegment5Store}.removeAll(true);#{uxERSegment5Store}.reload();#{uxERSegment6}.clearValue();#{uxERSegment6Store}.removeAll(true);#{uxERSegment7}.clearValue();#{uxERSegment7Store}.removeAll(true);" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox FieldLabel="Account" runat="server" ID="uxERSegment5" Editable="true" TypeAhead="true" Disabled="true"
                                    AnchorHorizontal="-5" DisplayField="ID" ValueField="ID"
                                    MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                    <Store>
                                        <ext:Store runat="server" ID="uxERSegment5Store" AutoLoad="false" OnReadData="deLoadSegment" AutoDataBind="true">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model12" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                          <Select Handler="#{uxShowAccounts}.disable();#{uxERSegment6}.enable();#{uxERSegment7}.disable();#{uxERSegment6}.clearValue();#{uxERSegment6Store}.removeAll(true);#{uxERSegment6Store}.reload();#{uxERSegment7}.clearValue();#{uxERSegment7Store}.removeAll(true);" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox FieldLabel="Type" runat="server" ID="uxERSegment6" Editable="true" TypeAhead="true" Disabled="true"
                                   AnchorHorizontal="-5" DisplayField="ID" ValueField="ID"
                                    MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                    <Store>
                                        <ext:Store runat="server" ID="uxERSegment6Store" AutoLoad="false" OnReadData="deLoadSegment" AutoDataBind="true">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model13" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                         <Select Handler="#{uxERSegment7}.enable();#{uxERSegment7}.clearValue();#{uxERSegment7Store}.removeAll(true);#{uxERSegment7Store}.reload();#{uxShowAccounts}.disable();" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox FieldLabel="Future" runat="server" ID="uxERSegment7" Editable="true" TypeAhead="true" Disabled="true"
                                     AnchorHorizontal="-5" DisplayField="ID" ValueField="ID"
                                    TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                     <Store>
                                        <ext:Store runat="server" ID="uxERSegment7Store" AutoLoad="false" OnReadData="deLoadSegment" AutoDataBind="true">
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model14" runat="server" IDProperty="ID">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                        <Select Handler="#{uxShowAccounts}.enable();#{uxIncludeExcludeFlag}.enable();"></Select>
                                    </Listeners>
                                </ext:ComboBox>


                        
                    </Items>
                        </ext:FieldSet>

                         <ext:FieldSet ID="uxPlaceHolderFieldSet"
                            runat="server"
                            Border="false"
                            ColumnWidth=".5"
                            MarginSpec="0 0 0 10">
                            <Defaults>
                                <ext:Parameter Name="Width" Value="250" />
                                <ext:Parameter Name="LabelWidth" Value="90" />
                            </Defaults>  
                             <Items>
                                 <ext:Image runat="server"></ext:Image>
                             </Items>
                        </ext:FieldSet>


                        <ext:FieldSet ID="FieldSet4"
                            runat="server"
                            Title="Include / Exclude Toggle"
                            ColumnWidth=".5"
                            MarginSpec="0 0 0 10">
                            <Defaults>
                                <ext:Parameter Name="Width" Value="250" />
                                <ext:Parameter Name="LabelWidth" Value="90" />
                            </Defaults>
                            <Items>
                                <ext:ComboBox
                                    ID="uxIncludeExcludeFlag"
                                    runat="server" Editable="true" TypeAhead="true" Disabled="true"
                                    AnchorHorizontal="-5"
                                    TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true" FieldLabel="Include/Exclude">
                                    <Items>
                                        <ext:ListItem Text="Included" Value="I" />
                                        <ext:ListItem Text="Excluded" Value="E" />
                                    </Items>
                                    <Listeners>
                                        <Select Handler="#{uxAddRange}.enable();"></Select>
                                    </Listeners>
                                </ext:ComboBox>
                            </Items>
                        </ext:FieldSet>


                    </Items>
                    <Buttons>
                        <ext:Button ID="uxShowAccounts" runat="server" Text="View Accounts" Icon="Find" Disabled="true">
                            <Listeners>
                                <Click Handler="#{uxGlAccountSecurityStore}.reload();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="uxAddRange" runat="server" Icon="ApplicationAdd" Text="Add Range" Disabled="true">
                    <DirectEvents>
                        <Click OnEvent="deAddAccountRange" Success="parent.Ext.getCmp('uxShowAccountRangeWindow').close();"><EventMask ShowMask="true"></EventMask><Confirmation ConfirmRequest="true" Message="Are you sure you want to add this account range to this organization?"></Confirmation></Click>
                    </DirectEvents>
                </ext:Button>
                <ext:Button ID="uxClearFilter" runat="server" Text="Clear Filter" >
                                   <Listeners><Click Handler="#{uxAccountFormPanel}.reset();
                                             #{uxERSegment1}.disable();
                                             #{uxERSegment2}.disable();
                                             #{uxERSegment3}.disable();
                                             #{uxERSegment4}.disable();
                                             #{uxERSegment5}.disable();
                                             #{uxERSegment6}.disable();
                                             #{uxERSegment7}.disable();
                                             #{uxShowAccounts}.disable();
                                             #{uxSRSegment2}.disable();
                                             #{uxSRSegment3}.disable();
                                             #{uxSRSegment4}.disable();
                                             #{uxSRSegment5}.disable();
                                             #{uxSRSegment6}.disable();
                                             #{uxSRSegment7}.disable();
                                       #{uxAddRange}.disable();"></Click></Listeners>
                                   </ext:Button>
                <ext:Button ID="uxCloseForm" runat="server" Text="Close Form" >
                    <Listeners><Click Handler="parent.Ext.getCmp('uxShowAccountRangeWindow').close();"></Click></Listeners>
                </ext:Button>
              </Buttons>
        </ext:FormPanel>
     <ext:GridPanel ID="uxGlAccountSecurityGrid" runat="server" Flex="1" Title="General Ledger Accounts" Margin="5" Region="South">
                    <Store>
                       <ext:Store runat="server"
                            ID="uxGlAccountSecurityStore"
                            AutoDataBind="true" OnReadData="deReadGLSecurityCodes" RemoteSort="true" PageSize="25" AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model15" runat="server" IDProperty="CODE_COMBINATION_ID">
                                    <Fields>
                                        <ext:ModelField Name="CODE_COMBINATION_ID" />
                                        <ext:ModelField Name="SEGMENT1" />
                                        <ext:ModelField Name="SEGMENT2" />
                                        <ext:ModelField Name="SEGMENT3" />
                                        <ext:ModelField Name="SEGMENT4" />
                                        <ext:ModelField Name="SEGMENT5" />
                                        <ext:ModelField Name="SEGMENT5_DESC" />
                                        <ext:ModelField Name="SEGMENT6" />
                                        <ext:ModelField Name="SEGMENT7" />
                                        <ext:ModelField Name="SEGMENT1_DESC" />
                                        <ext:ModelField Name="SEGMENT2_DESC" />
                                        <ext:ModelField Name="SEGMENT3_DESC" />
                                        <ext:ModelField Name="SEGMENT4_DESC" />
                                        <ext:ModelField Name="SEGMENT5_DESC" />
                                        <ext:ModelField Name="SEGMENT6_DESC" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Sorters>
                                <ext:DataSorter Direction="ASC" Property="SEGMENT1" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                   
                    <ColumnModel>
                        <Columns>
                             <ext:Column ID="Column7" runat="server" DataIndex="SEGMENT5_DESC" Text="Account Name" Flex="2" />
                            <ext:Column ID="Column4" runat="server" DataIndex="SEGMENT1" Text="Company" Flex="1" />
                            <ext:Column ID="Column3" runat="server" DataIndex="SEGMENT2" Text="Location" Flex="1" />
                            <ext:Column ID="Column5" runat="server" DataIndex="SEGMENT3" Text="Division" Flex="1" />
                            <ext:Column ID="Column6" runat="server" DataIndex="SEGMENT4" Text="Branch" Flex="1" />
                            <ext:Column ID="Column10" runat="server" DataIndex="SEGMENT5" Text="Account" Flex="1" />
                            <ext:Column ID="Column11" runat="server" DataIndex="SEGMENT6" Text="Type" Flex="1" />
                            <ext:Column ID="Column12" runat="server" DataIndex="SEGMENT7" Text="Future" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="uxGlAccountSecurityGridFilter" runat="server" Remote="true" />
                    </Plugins>
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
   </form>
</body>
</html>
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
            Title="General Ledger Account Range"
            BodyPadding="10"
                     Region="North" Margins="5 5 5 0" Flex="1"
            Layout="HBoxLayout">
            
            <FieldDefaults LabelAlign="Top" MsgTarget="Side" />

            <Defaults>
                <ext:Parameter Name="Border" Value="false" />
                <ext:Parameter Name="Flex" Value="1" />
                <ext:Parameter Name="Layout" Value="anchor" />
            </Defaults>

            <Items>
                <ext:Panel ID="Panel1" runat="server">
                    <Items>
                        <ext:ComboBox runat="server" ID="uxSegment1" Editable="false" TypeAhead="false"
                                FieldLabel="Company"  AnchorHorizontal="-5" DisplayField="ID_NAME" ValueField="ID" 
                                MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true" >
                                  <Triggers>
                                    <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                <Store>
                                    <ext:Store runat="server" ID="uxCompanyNameStore" AutoLoad="false" >
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                        <Model>
                                            <ext:Model ID="Model6" runat="server" IDProperty="ID">
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
                       <ext:ComboBox runat="server" ID="ComboBox2" Editable="false" TypeAhead="false"
                                FieldLabel="Company"  AnchorHorizontal="-5" DisplayField="ID_NAME" ValueField="ID" 
                                MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true" >
                                  <Triggers>
                                    <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                <Store>
                                    <ext:Store runat="server" ID="Store2" AutoLoad="false" >
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                        <Model>
                                            <ext:Model ID="Model3" runat="server" IDProperty="ID">
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
                </ext:Panel>

                <ext:Panel ID="Panel2" runat="server">
                    <Items>
                        <ext:ComboBox runat="server" ID="ComboBox1" Editable="false" TypeAhead="false"
                                FieldLabel="Company"  AnchorHorizontal="-5" DisplayField="ID_NAME" ValueField="ID" 
                                MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true" >
                                  <Triggers>
                                    <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                <Store>
                                    <ext:Store runat="server" ID="Store1" AutoLoad="false" >
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                        <Model>
                                            <ext:Model ID="Model2" runat="server" IDProperty="ID">
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
                       <ext:ComboBox runat="server" ID="ComboBox3" Editable="false" TypeAhead="false"
                                FieldLabel="Company"  AnchorHorizontal="-5" DisplayField="ID_NAME" ValueField="ID" 
                                MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1" ForceSelection="true" >
                                  <Triggers>
                                    <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                    </Triggers>
                                <Store>
                                    <ext:Store runat="server" ID="Store3" AutoLoad="false" >
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
                    </Items>
                </ext:Panel>
            </Items>

            <Buttons>
                <ext:Button ID="Button1" runat="server" Text="Save" />
                <ext:Button ID="Button2" runat="server" Text="Cancel" />
            </Buttons>
        </ext:FormPanel>

                
                <ext:GridPanel ID="uxGlAccountSecurityGrid" runat="server" Flex="1" SimpleSelect="true" Title="GL Accounts By Filter" Padding="5" Region="Center" Height="400">
                   <Store>
                        <ext:Store runat="server"
                            ID="uxGlAccountSecurityStore"
                            AutoDataBind="true"  RemoteSort="true" PageSize="10" AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model1" runat="server" IDProperty="CODE_COMBINATION_ID">
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
                            <Parameters>
                                <ext:StoreParameter Name="SEGMENT1" Value="#{uxSegment1}.getValue()" Mode="Raw">
                                </ext:StoreParameter>
                                <ext:StoreParameter Name="SEGMENT2" Value="#{uxSegment2}.getValue()" Mode="Raw">
                                </ext:StoreParameter>
                                <ext:StoreParameter Name="SEGMENT3" Value="#{uxSegment3}.getValue()" Mode="Raw">
                                </ext:StoreParameter>
                                <ext:StoreParameter Name="SEGMENT4" Value="#{uxSegment4}.getValue()" Mode="Raw">
                                </ext:StoreParameter>
                            </Parameters>
                             <Listeners><Load Handler="#{uxAddGLCodeButton}.disable();"></Load></Listeners>
                            <Sorters>
                                <ext:DataSorter Direction="ASC" Property="SEGMENT5_DESC" />
                            </Sorters>

                        </ext:Store>
                   </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column1" runat="server" DataIndex="SEGMENT5_DESC" Text="Account Description" Flex="3" />
                            <ext:Column ID="Column2" runat="server" DataIndex="SEGMENT1" Text="Company" Flex="1" />
                            <ext:Column ID="Column3" runat="server" DataIndex="SEGMENT2" Text="Location" Flex="1" />
                            <ext:Column ID="Column4" runat="server" DataIndex="SEGMENT3" Text="Division" Flex="1" />
                            <ext:Column ID="Column5" runat="server" DataIndex="SEGMENT4" Text="Branch" Flex="1" />
                            <ext:Column ID="Column10" runat="server" DataIndex="SEGMENT5" Text="Account" Flex="1" />
                            <ext:Column ID="Column11" runat="server" DataIndex="SEGMENT6" Text="Type" Flex="1" />
                            <ext:Column ID="Column12" runat="server" DataIndex="SEGMENT7" Text="Future" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="uxGlAccountSecurityGridFilter" runat="server" Remote="true" />
                    </Plugins>
                      <SelectionModel>
                               <ext:RowSelectionModel ID="uxGlAccountSecurityGridSelectionModel" runat="server" Mode="Simple">
                                   <Listeners>
                                <Select Handler="if(#{uxGlAccountSecurityGridSelectionModel}.getCount() > 0){#{uxAddGLCodeButton}.enable();}else {#{uxAddGLCodeButton}.disable();}"></Select>
                                       <Deselect Handler="if(#{uxGlAccountSecurityGridSelectionModel}.getCount() > 0){#{uxAddGLCodeButton}.enable();}else {#{uxAddGLCodeButton}.disable();}"></Deselect>
                            </Listeners>
                               </ext:RowSelectionModel>
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar2" runat="server" />
                    </BottomBar>
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button ID="uxAddGLCodeButton" runat="server" Text="Include" Icon="Add" Disabled="true">

                                </ext:Button>
                                 <ext:Button ID="uxExcludeGLCodeButton" runat="server" Text="Exclude" Icon="Delete" Disabled="true">

                                </ext:Button>
                                
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                                         <View>
                        <ext:GridView ID="GridView1" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View> 
                    <ToolTips>
                        <ext:ToolTip ID="uxToolTip"
            runat="server"
            Target="uxGlAccountSecurityGrid"
            Delegate=".x-grid-row"
            TrackMouse="true"
                            UI="Info"
                           Width="300">
            <Listeners>
                <Show Handler="onShow(this, #{uxGlAccountSecurityGrid});" /> 
            </Listeners>
        </ext:ToolTip>  
                    </ToolTips>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
   </form>
</body>
</html>
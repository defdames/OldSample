<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddGLAccount.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umAddGLAccount" %>

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

                <ext:FormPanel runat="server" Title="Account Filters" BodyPadding="10"
                    Margins="5 5 5 0" Region="North" Height="175">
                    <Items>
                        <ext:FieldContainer ID="FieldContainer1" 
                        runat="server"
                        LabelStyle="font-weight:bold;padding:0;"
                        Layout="HBoxLayout">
                        <Items>
                            <ext:ComboBox runat="server" ID="uxSegment1" Editable="false" TypeAhead="true"
                            FieldLabel="Company" AnchorHorizontal="45%" DisplayField="Name" LoadingText="Searching..."
                            ValueField="ID" ForceSelection="true" HideTrigger="false"
                            MinChars="1" TabIndex="0" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1">
                            <Store>
                                <ext:Store runat="server" ID="uxSegment1Store" AutoLoad="false">
                                    <Proxy>
                                        <ext:AjaxProxy Url="GLCompanyCodes.ashx">
                                            <ActionMethods Read="POST" />
                                            <Reader>
                                                <ext:JsonReader Root="glcodes" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Model>
                                        <ext:Model ID="Model5" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="ID" />
                                                <ext:ModelField Name="Name" />
                                                <ext:ModelField Name="Description" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                      <Parameters>                                       
                                        <ext:StoreParameter Name="HIERARCHYID" Value="#{uxSelectedHierarchyID}.getValue()" Mode="Raw">
                                                </ext:StoreParameter>
                                    </Parameters>
                                </ext:Store>
                            </Store>
                            <Listeners>
                                <Select Handler="#{uxSegment1Description}.setText(#{uxSegment1}.getStore().getAt(#{uxSegment1}.getStore().findExact('ID',#{uxSegment1}.getValue())).get('Description'));#{uxSegment2}.clearValue(); #{uxSegment2Store}.reload();#{uxSegment3}.clearValue(); #{uxSegment3Store}.reload();#{uxSegment4}.clearValue(); #{uxSegment4Store}.reload();if(#{uxGlAccountSecurityStore}.getCount() > 0){#{uxGlAccountSecurityStore}.reload();#{uxGlAccountSecurityGrid}.getView().refresh();#{uxGlAccountSecurityGridFilter}.clearFilter();}#{uxSegment2Description}.setText('');#{uxSegment3Description}.setText('');#{uxSegment4Description}.setText('');"></Select>
                            </Listeners>   
                        </ext:ComboBox>
                            <ext:Label runat="server" ID="uxSegment1Description" Margins="0 0 0 5" StyleSpec="color:green;" Flex="1"></ext:Label>
                        </Items>
                    </ext:FieldContainer>

                         <ext:FieldContainer ID="FieldContainer2" 
                        runat="server"
                        LabelStyle="font-weight:bold;padding:0;"
                        Layout="HBoxLayout">
                        <Items>
                         <ext:ComboBox runat="server" ID="uxSegment2" Editable="false" TypeAhead="true"
                            FieldLabel="Location" AnchorHorizontal="45%" DisplayField="Name" LoadingText="Searching..."
                            ValueField="ID" ForceSelection="true" HideTrigger="false"
                            MinChars="1" TabIndex="1" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1">
                            <Store>
                                <ext:Store ID="uxSegment2Store" runat="server" AutoLoad="false">
                                    <Proxy>
                                        <ext:AjaxProxy Url="GLLocationCodes.ashx">
                                            <ActionMethods Read="POST" />
                                            <Reader>
                                                <ext:JsonReader Root="glcodes" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Model>
                                        <ext:Model ID="Model2" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="ID" />
                                                <ext:ModelField Name="Name" />
                                                <ext:ModelField Name="Description" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Parameters>                                       
                                        <ext:StoreParameter Name="SEGMENT1" Value="#{uxSegment1}.getValue()" Mode="Raw">
                                                </ext:StoreParameter>
                                    </Parameters>
                                </ext:Store>
                            </Store>
                              <Listeners>
                                     <Select Handler="#{uxSegment2Description}.setText(#{uxSegment2}.getStore().getAt(#{uxSegment2}.getStore().findExact('ID',#{uxSegment2}.getValue())).get('Description'));#{uxSegment3}.clearValue(); #{uxSegment3Store}.reload();#{uxSegment4}.clearValue(); #{uxSegment4Store}.reload();if(#{uxGlAccountSecurityStore}.getCount() > 0){#{uxGlAccountSecurityStore}.reload();#{uxGlAccountSecurityGrid}.getView().refresh();#{uxGlAccountSecurityGridFilter}.clearFilter();}#{uxSegment3Description}.setText('');#{uxSegment4Description}.setText('');"></Select>
                            </Listeners> 
                        </ext:ComboBox>
                                <ext:Label runat="server" ID="uxSegment2Description" Margins="0 0 0 5" StyleSpec="color:green;" Flex="1"></ext:Label>
                        </Items>
                    </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer3" 
                        runat="server"
                        LabelStyle="font-weight:bold;padding:0;"
                        Layout="HBoxLayout">
                        <Items>
                         <ext:ComboBox runat="server" ID="uxSegment3" Editable="false" TypeAhead="true"
                            FieldLabel="Division" AnchorHorizontal="45%" DisplayField="Name" LoadingText="Searching..."
                            ValueField="ID" ForceSelection="true" HideTrigger="false"
                            MinChars="1" TabIndex="2" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1">
                            <Store>
                                <ext:Store ID="uxSegment3Store" runat="server" AutoLoad="false">
                                    <Proxy>
                                        <ext:AjaxProxy Url="GLDivisionCodes.ashx">
                                            <ActionMethods Read="POST" />
                                            <Reader>
                                                <ext:JsonReader Root="glcodes" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Model>
                                        <ext:Model ID="Model3" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="ID" />
                                                <ext:ModelField Name="Name" />
                                                <ext:ModelField Name="Description" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Parameters>                                       
                                        <ext:StoreParameter Name="SEGMENT1" Value="#{uxSegment1}.getValue()" Mode="Raw">
                                                </ext:StoreParameter>
                                        <ext:StoreParameter Name="SEGMENT2" Value="#{uxSegment2}.getValue()" Mode="Raw">
                                                </ext:StoreParameter>
                                    </Parameters>
                                </ext:Store>
                            </Store>
                              <Listeners>
                                   <Select Handler="#{uxSegment3Description}.setText(#{uxSegment3}.getStore().getAt(#{uxSegment3}.getStore().findExact('ID',#{uxSegment3}.getValue())).get('Description'));#{uxSegment4}.clearValue(); #{uxSegment4Store}.reload();if(#{uxGlAccountSecurityStore}.getCount() > 0){#{uxGlAccountSecurityStore}.reload();#{uxGlAccountSecurityGrid}.getView().refresh();#{uxGlAccountSecurityGridFilter}.clearFilter();}#{uxSegment4Description}.setText('');"></Select>
                            </Listeners> 
                        </ext:ComboBox>
                              <ext:Label runat="server" ID="uxSegment3Description" Margins="0 0 0 5" StyleSpec="color:green;" Flex="1"></ext:Label>
                        </Items>
                    </ext:FieldContainer>

                          <ext:FieldContainer ID="FieldContainer4" 
                        runat="server"
                        LabelStyle="font-weight:bold;padding:0;"
                        Layout="HBoxLayout">
                        <Items>
                         <ext:ComboBox runat="server" ID="uxSegment4" Editable="false" TypeAhead="true"
                            FieldLabel="Branch" AnchorHorizontal="45%" DisplayField="Name" LoadingText="Searching..."
                            ValueField="ID" ForceSelection="true" HideTrigger="false"
                            MinChars="1" TabIndex="3" FieldStyle="background-color: #EFF7FF; background-image: none;" Flex="1">
                            <Store>
                                <ext:Store ID="uxSegment4Store" runat="server" AutoLoad="false">
                                    <Proxy>
                                        <ext:AjaxProxy Url="GLBranchCodes.ashx">
                                            <ActionMethods Read="POST" />
                                            <Reader>
                                                <ext:JsonReader Root="glcodes" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Model>
                                        <ext:Model ID="Model4" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="ID" />
                                                <ext:ModelField Name="Name" />
                                                <ext:ModelField Name="Description" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Parameters>                                       
                                         <ext:StoreParameter Name="SEGMENT1" Value="#{uxSegment1}.getValue()" Mode="Raw">
                                                </ext:StoreParameter>
                                        <ext:StoreParameter Name="SEGMENT2" Value="#{uxSegment2}.getValue()" Mode="Raw">
                                                </ext:StoreParameter>
                                         <ext:StoreParameter Name="SEGMENT3" Value="#{uxSegment3}.getValue()" Mode="Raw">
                                                </ext:StoreParameter>
                                    </Parameters>
                                </ext:Store>
                            </Store>
                             <Listeners>
                                   <Select Handler="#{uxSegment4Description}.setText(#{uxSegment4}.getStore().getAt(#{uxSegment4}.getStore().findExact('ID',#{uxSegment4}.getValue())).get('Description'));#{uxFilterAccounts}.enable();if(#{uxGlAccountSecurityStore}.getCount() > 0){#{uxGlAccountSecurityStore}.reload();#{uxGlAccountSecurityGrid}.getView().refresh();#{uxGlAccountSecurityGridFilter}.clearFilter();}"></Select>
                            </Listeners> 
                        </ext:ComboBox>
                                 <ext:Label runat="server" ID="uxSegment4Description" Margins="0 0 0 5" StyleSpec="color:green;" Flex="1"></ext:Label>
                        </Items>
                    </ext:FieldContainer>

                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxFilterAccounts" Text="Filter" Disabled="true">
                            <DirectEvents>
                                <Click OnEvent="deFilterEvents">
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="uxClearFilterAccounts" Text="Clear Filter">
                            <Listeners>
                                <Click Handler="#{uxFilterAccounts}.disable();#{uxSegment1}.clearValue(); #{uxSegment1Store}.reload();#{uxSegment2}.clearValue(); #{uxSegment2Store}.reload();#{uxSegment3}.clearValue(); #{uxSegment3Store}.reload();#{uxSegment4}.clearValue(); #{uxSegment4Store}.reload();#{uxGlAccountSecurityStore}.reload();#{uxGlAccountSecurityGrid}.getView().refresh();#{uxGlAccountSecurityGridFilter}.clearFilter();#{uxSegment1Description}.setText('');#{uxSegment2Description}.setText('');#{uxSegment3Description}.setText('');#{uxSegment4Description}.setText('');"></Click>
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
                <ext:GridPanel ID="uxGlAccountSecurityGrid" runat="server" Flex="1" SimpleSelect="true" Title="GL Accounts By Filter" Padding="5" Region="Center" Height="400">
                   <Store>
                        <ext:Store runat="server"
                            ID="uxGlAccountSecurityStore"
                            AutoDataBind="true" OnReadData="deReadGLSecurityCodes" RemoteSort="true" PageSize="10" AutoLoad="false">
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
                        </ext:Store>
                   </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column2" runat="server" DataIndex="SEGMENT1" Text="Company" Flex="1" />
                            <ext:Column ID="Column3" runat="server" DataIndex="SEGMENT2" Text="Location" Flex="1" />
                            <ext:Column ID="Column4" runat="server" DataIndex="SEGMENT3" Text="Division" Flex="1" />
                            <ext:Column ID="Column5" runat="server" DataIndex="SEGMENT4" Text="Branch" Flex="1" />
                            <ext:Column ID="Column1" runat="server" DataIndex="SEGMENT5_DESC" Text="Account Description" Flex="2" />
                            <ext:Column ID="Column10" runat="server" DataIndex="SEGMENT5" Text="Account" Flex="1" />
                            <ext:Column ID="Column11" runat="server" DataIndex="SEGMENT6" Text="Type" Flex="1" />
                            <ext:Column ID="Column12" runat="server" DataIndex="SEGMENT7" Text="Future" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="uxGlAccountSecurityGridFilter" runat="server" Remote="true" />
                    </Plugins>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel ID="uxGlAccountSecurityGridSelectionModel" runat="server" Mode="Multi"> 
                        </ext:CheckboxSelectionModel>
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar2" runat="server" />
                    </BottomBar>
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button ID="Button1" runat="server" Text="Add Selected" Icon="Add">
                                    <DirectEvents>
                                        <Click OnEvent="deAddSelectedGlCodes">
                                            <EventMask ShowMask="true"></EventMask>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
   </form>
</body>
</html>

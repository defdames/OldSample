<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddGLAccount.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umAddGLAccount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />         
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel runat="server" Title="Account Filters" BodyPadding="10"
                    Margins="5 5 5 0" Region="North" Height="175">
                    <Items>
                        <ext:ComboBox runat="server" ID="uxSegment1" Editable="true" TypeAhead="false"
                            FieldLabel="Company" AnchorHorizontal="45%" DisplayField="Name" LoadingText="Searching..."
                            ValueField="ID" ForceSelection="true" PageSize="10" HideTrigger="false"
                            MinChars="1" TabIndex="0">
                            <Store>
                                <ext:Store runat="server" AutoLoad="false">
                                    <Proxy>
                                        <ext:AjaxProxy Url="GLCompanyCodes.ashx">
                                            <ActionMethods Read="POST" />
                                            <Reader>
                                                <ext:JsonReader Root="glcodes" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="ID" />
                                                <ext:ModelField Name="Name" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                            <Listeners>
                                <Select Handler="#{uxSegment2}.clearValue(); #{uxSegment2Store}.reload();#{uxSegment3}.clearValue(); #{uxSegment3Store}.reload();#{uxSegment4}.clearValue(); #{uxSegment4Store}.reload();"></Select>
                            </Listeners>   
                        </ext:ComboBox>
                         <ext:ComboBox runat="server" ID="uxSegment2" Editable="true" TypeAhead="false"
                            FieldLabel="Location" AnchorHorizontal="45%" DisplayField="Name" LoadingText="Searching..."
                            ValueField="ID" ForceSelection="true" PageSize="10" HideTrigger="false"
                            MinChars="1" TabIndex="0">
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
                                <Select Handler="#{uxSegment3}.clearValue(); #{uxSegment3Store}.reload();#{uxSegment4}.clearValue(); #{uxSegment4Store}.reload();"></Select>
                            </Listeners> 
                        </ext:ComboBox>
                         <ext:ComboBox runat="server" ID="uxSegment3" Editable="true" TypeAhead="false"
                            FieldLabel="Division" AnchorHorizontal="45%" DisplayField="Name" LoadingText="Searching..."
                            ValueField="ID" ForceSelection="true" PageSize="10" HideTrigger="false"
                            MinChars="1" TabIndex="0">
                            <Store>
                                <ext:Store ID="Store1" runat="server" AutoLoad="false">
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
                                <Select Handler="#{uxSegment4}.clearValue(); #{uxSegment4Store}.reload();"></Select>
                            </Listeners> 
                        </ext:ComboBox>
                         <ext:ComboBox runat="server" ID="uxSegment4" Editable="true" TypeAhead="false"
                            FieldLabel="Branch" AnchorHorizontal="45%" DisplayField="Name" LoadingText="Searching..."
                            ValueField="ID" ForceSelection="true" PageSize="10" HideTrigger="false"
                            MinChars="1" TabIndex="0">
                            <Store>
                                <ext:Store ID="Store2" runat="server" AutoLoad="false">
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
                        </ext:ComboBox>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxFilterAccounts" Text="Filter"></ext:Button>
                        <ext:Button runat="server" ID="uxClearFilterAccounts" Text="Clear Filter"></ext:Button>
                    </Buttons>
                </ext:FormPanel>
                <ext:GridPanel ID="uxOrganizationsGrid" runat="server" Flex="1" SimpleSelect="true" Title="Organization Security By Hierarchy" Padding="5" Region="Center" Height="400">
                   <Store>
                        <ext:Store runat="server"
                            ID="uxGlAccountSecurityStore"
                            AutoDataBind="true" RemoteSort="true" PageSize="10">
                            <Model>
                                <ext:Model ID="Model1" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="CODE_COMBINATION_ID" />
                                        <ext:ModelField Name="SEGMENT5" />
                                        <ext:ModelField Name="SEGMENT5DESC" />
                                        <ext:ModelField Name="SEGMENT6" />
                                        <ext:ModelField Name="SEGMENT6DESC" />
                                        <ext:ModelField Name="SEGMENT7" />
                                        <ext:ModelField Name="SEGMENT7DESC" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column10" runat="server" DataIndex="SEGMENT5" Text="Account" Flex="1" />
                            <ext:Column ID="Column11" runat="server" DataIndex="SEGMENT6" Text="Type" Flex="1" />
                            <ext:Column ID="Column12" runat="server" DataIndex="SEGMENT7" Text="Future" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="FilterHeader2" runat="server" Remote="true" />
                    </Plugins>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" Mode="Multi"></ext:CheckboxSelectionModel>
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar2" runat="server" />
                    </BottomBar>
                <TopBar>
                   <ext:Toolbar ID="Toolbar1" runat="server"><Items> <ext:Button ID="Button1" runat="server" Text="Add Selected" Icon="Add"></ext:Button></Items></ext:Toolbar>
                </TopBar>
                </ext:GridPanel>
                </Items>
        </ext:Viewport>
   </form>
</body>
</html>

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
                <ext:FormPanel runat="server" Title="Account Filters" Layout="FormLayout" Padding="5" Region="North" Height="250">
                    <Items>
                        <ext:ComboBox
                            ID="ComboBox1"
                            runat="server"
                            Editable="false"
                            QueryMode="Local"
                            TriggerAction="All"
                            EmptyText="Select a Company">
                            <Items>
                                <ext:ListItem Text="Belgium" Value="BE" />
                                <ext:ListItem Text="Brazil" Value="BR" />
                                <ext:ListItem Text="Bulgaria" Value="BG" />
                                <ext:ListItem Text="Canada" Value="CA" />
                                <ext:ListItem Text="Chile" Value="CL" />
                                <ext:ListItem Text="Cyprus" Value="CY" />
                                <ext:ListItem Text="Finland" Value="FI" />
                                <ext:ListItem Text="France" Value="FR" />
                                <ext:ListItem Text="Germany" Value="DE" />
                                <ext:ListItem Text="Hungary" Value="HU" />
                                <ext:ListItem Text="Ireland" Value="IE" />
                                <ext:ListItem Text="Israel" Value="IL" />
                                <ext:ListItem Text="Italy" Value="IT" />
                                <ext:ListItem Text="Lithuania" Value="LT" />
                                <ext:ListItem Text="Mexico" Value="MX" />
                                <ext:ListItem Text="Netherlands" Value="NL" />
                                <ext:ListItem Text="New Zealand" Value="NZ" />
                                <ext:ListItem Text="Norway" Value="NO" />
                                <ext:ListItem Text="Pakistan" Value="PK" />
                                <ext:ListItem Text="Poland" Value="PL" />
                                <ext:ListItem Text="Romania" Value="RO" />
                                <ext:ListItem Text="Slovakia" Value="SK" />
                                <ext:ListItem Text="Slovenia" Value="SI" />
                                <ext:ListItem Text="Spain" Value="ES" />
                                <ext:ListItem Text="Sweden" Value="SE" />
                                <ext:ListItem Text="Switzerland" Value="CH" />
                                <ext:ListItem Text="United Kingdom" Value="GB" />
                            </Items>
                        </ext:ComboBox>
                         <ext:ComboBox
                            ID="ComboBox2"
                            runat="server"
                            Editable="false"
                            QueryMode="Local"
                            TriggerAction="All"
                            EmptyText="Select a Location">
                            <Items>
                                <ext:ListItem Text="Belgium" Value="BE" />
                                <ext:ListItem Text="Brazil" Value="BR" />
                                <ext:ListItem Text="Bulgaria" Value="BG" />
                                <ext:ListItem Text="Canada" Value="CA" />
                                <ext:ListItem Text="Chile" Value="CL" />
                                <ext:ListItem Text="Cyprus" Value="CY" />
                                <ext:ListItem Text="Finland" Value="FI" />
                                <ext:ListItem Text="France" Value="FR" />
                                <ext:ListItem Text="Germany" Value="DE" />
                                <ext:ListItem Text="Hungary" Value="HU" />
                                <ext:ListItem Text="Ireland" Value="IE" />
                                <ext:ListItem Text="Israel" Value="IL" />
                                <ext:ListItem Text="Italy" Value="IT" />
                                <ext:ListItem Text="Lithuania" Value="LT" />
                                <ext:ListItem Text="Mexico" Value="MX" />
                                <ext:ListItem Text="Netherlands" Value="NL" />
                                <ext:ListItem Text="New Zealand" Value="NZ" />
                                <ext:ListItem Text="Norway" Value="NO" />
                                <ext:ListItem Text="Pakistan" Value="PK" />
                                <ext:ListItem Text="Poland" Value="PL" />
                                <ext:ListItem Text="Romania" Value="RO" />
                                <ext:ListItem Text="Slovakia" Value="SK" />
                                <ext:ListItem Text="Slovenia" Value="SI" />
                                <ext:ListItem Text="Spain" Value="ES" />
                                <ext:ListItem Text="Sweden" Value="SE" />
                                <ext:ListItem Text="Switzerland" Value="CH" />
                                <ext:ListItem Text="United Kingdom" Value="GB" />
                            </Items>
                        </ext:ComboBox>
                         <ext:ComboBox
                            ID="ComboBox3"
                            runat="server"
                            Editable="false"
                            QueryMode="Local"
                            TriggerAction="All"
                            EmptyText="Select a Division">
                            <Items>
                                <ext:ListItem Text="Belgium" Value="BE" />
                                <ext:ListItem Text="Brazil" Value="BR" />
                                <ext:ListItem Text="Bulgaria" Value="BG" />
                                <ext:ListItem Text="Canada" Value="CA" />
                                <ext:ListItem Text="Chile" Value="CL" />
                                <ext:ListItem Text="Cyprus" Value="CY" />
                                <ext:ListItem Text="Finland" Value="FI" />
                                <ext:ListItem Text="France" Value="FR" />
                                <ext:ListItem Text="Germany" Value="DE" />
                                <ext:ListItem Text="Hungary" Value="HU" />
                                <ext:ListItem Text="Ireland" Value="IE" />
                                <ext:ListItem Text="Israel" Value="IL" />
                                <ext:ListItem Text="Italy" Value="IT" />
                                <ext:ListItem Text="Lithuania" Value="LT" />
                                <ext:ListItem Text="Mexico" Value="MX" />
                                <ext:ListItem Text="Netherlands" Value="NL" />
                                <ext:ListItem Text="New Zealand" Value="NZ" />
                                <ext:ListItem Text="Norway" Value="NO" />
                                <ext:ListItem Text="Pakistan" Value="PK" />
                                <ext:ListItem Text="Poland" Value="PL" />
                                <ext:ListItem Text="Romania" Value="RO" />
                                <ext:ListItem Text="Slovakia" Value="SK" />
                                <ext:ListItem Text="Slovenia" Value="SI" />
                                <ext:ListItem Text="Spain" Value="ES" />
                                <ext:ListItem Text="Sweden" Value="SE" />
                                <ext:ListItem Text="Switzerland" Value="CH" />
                                <ext:ListItem Text="United Kingdom" Value="GB" />
                            </Items>
                        </ext:ComboBox>
                         <ext:ComboBox
                            ID="ComboBox4"
                            runat="server"
                            Editable="false"
                            QueryMode="Local"
                            TriggerAction="All"
                            EmptyText="Select a Branch">
                            <Items>
                                <ext:ListItem Text="Belgium" Value="BE" />
                                <ext:ListItem Text="Brazil" Value="BR" />
                                <ext:ListItem Text="Bulgaria" Value="BG" />
                                <ext:ListItem Text="Canada" Value="CA" />
                                <ext:ListItem Text="Chile" Value="CL" />
                                <ext:ListItem Text="Cyprus" Value="CY" />
                                <ext:ListItem Text="Finland" Value="FI" />
                                <ext:ListItem Text="France" Value="FR" />
                                <ext:ListItem Text="Germany" Value="DE" />
                                <ext:ListItem Text="Hungary" Value="HU" />
                                <ext:ListItem Text="Ireland" Value="IE" />
                                <ext:ListItem Text="Israel" Value="IL" />
                                <ext:ListItem Text="Italy" Value="IT" />
                                <ext:ListItem Text="Lithuania" Value="LT" />
                                <ext:ListItem Text="Mexico" Value="MX" />
                                <ext:ListItem Text="Netherlands" Value="NL" />
                                <ext:ListItem Text="New Zealand" Value="NZ" />
                                <ext:ListItem Text="Norway" Value="NO" />
                                <ext:ListItem Text="Pakistan" Value="PK" />
                                <ext:ListItem Text="Poland" Value="PL" />
                                <ext:ListItem Text="Romania" Value="RO" />
                                <ext:ListItem Text="Slovakia" Value="SK" />
                                <ext:ListItem Text="Slovenia" Value="SI" />
                                <ext:ListItem Text="Spain" Value="ES" />
                                <ext:ListItem Text="Sweden" Value="SE" />
                                <ext:ListItem Text="Switzerland" Value="CH" />
                                <ext:ListItem Text="United Kingdom" Value="GB" />
                            </Items>
                        </ext:ComboBox>
                    </Items>
                    <Buttons>
                        <ext:Button ID="uxFilterAccounts" Text="Filter"></ext:Button>
                        <ext:Button ID="uxClearFilterAccounts" Text="Clear Filter"></ext:Button>
                    </Buttons>
                </ext:FormPanel>
                <ext:GridPanel ID="uxOrganizationsGrid" runat="server" Flex="1" SimpleSelect="true" Title="Organization Security By Hierarchy" Padding="5" Region="Center" Height="400">
                   <Store>
                        <ext:Store runat="server"
                            ID="uxGlAccountSecurityStore"
                            AutoDataBind="true" RemoteSort="true" OnReadData="deReadGLSecurityCodes" PageSize="10">
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

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAccountCategory.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umAccountCategory" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" Namespace="App" ClientIDMode="Static" />         
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>

                  <ext:GridPanel ID="uxAccountCategoryGridPanel" runat="server" Flex="1" SimpleSelect="true" Frame="true" Padding="5" Margins="5 5 5 5" Region="Center" Title="Account Categories">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxAddCategory"  Icon="Add" Text="Add" Disabled="true"> 
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip1" runat="server" Html="This allows you to create a new account category" UI="Info"></ext:ToolTip>
                                    </ToolTips>
                                    <Listeners>
                                        <Click Handler="#{uxAccountCategoryForm}.reset();#{uxCategoryWindow}.show();" />
                                    </Listeners>
                                </ext:Button>
                                 <ext:Button runat="server" ID="uxDeleteCategory"  Icon="Delete" Text="Delete" Disabled="true"> 
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip2" runat="server" Html="This allows you to delete an account category" UI="Info"></ext:ToolTip>
                                    </ToolTips>
                                     <DirectEvents>
                                         <Click OnEvent="deDeleteCategory"><Confirmation Message="Are you sure you want to delete this category? If there are any accounts assigned they will also be removed, are you sure?" ConfirmRequest="true"></Confirmation><EventMask ShowMask="true"></EventMask></Click>
                                     </DirectEvents>
                                         
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxAccountCategoryStore"
                            AutoDataBind="true" RemoteSort="true" AutoLoad="true" OnReadData="uxAccountCategoryStore_ReadData" >
                            <Model>
                                        <ext:Model ID="Model2" runat="server" IDProperty="CATEGORY_ID">
                                            <Fields>
                                                <ext:ModelField Name="NAME"  />
                                                <ext:ModelField Name="DESCRIPTION"  />
                                                <ext:ModelField Name="SORT_ORDER"  />
                                                <ext:ModelField Name="CREATE_DATE"  />
                                                <ext:ModelField Name="CREATED_BY"  />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:PageProxy />
                                    </Proxy>
                            <Sorters>
                                <ext:DataSorter Direction="ASC" Property="NAME"></ext:DataSorter>
                            </Sorters>
                                </ext:Store>
                            </Store>
                            <ColumnModel>
                                <Columns>
                                    <ext:Column ID="Column5" runat="server" DataIndex="SORT_ORDER" Text="Sort Order" Width="125" >
                                       <Editor>
                                    <ext:NumberField runat="server" AllowBlank="false" ID="uxEditCategorySortOrder" SelectOnFocus="true" TabIndex="1" MinValue="1" AllowDecimals="false">
                                         <Listeners>
                                              <Show Handler="this.el.dom.select();" Delay="150" />
                                          </Listeners>
                                      </ext:NumberField>
                                </Editor>
                                    </ext:Column>
                                    <ext:Column ID="Column2" runat="server" DataIndex="NAME" Text="Name" Flex="1" />
                                    <ext:Column ID="Column1" runat="server" DataIndex="DESCRIPTION" Text="Description" Flex="1" />
                                </Columns>
                            </ColumnModel>
                     <SelectionModel>
                         <ext:RowSelectionModel runat="server" Mode="Single" ID="uxAccountCategorySelectionModel" AllowDeselect="true">
                             <DirectEvents>
                                 <Select OnEvent="deSelectCategory">
                                 </Select>
                                 <Deselect OnEvent="deDeSelectCategory"></Deselect>
                             </DirectEvents>
                         </ext:RowSelectionModel>
                            </SelectionModel>
                      <Plugins>
                          <ext:FilterHeader runat="server" Remote="true"></ext:FilterHeader>
                      </Plugins>
                            <BottomBar>
                                <ext:PagingToolbar ID="uxAccountCategoryGridPageBar" runat="server" />
                            </BottomBar>   
                    <View>
                        <ext:GridView ID="uxAccountCategoryGridView" StripeRows="true" runat="server">
                        </ext:GridView>
                    </View>       
                      <Plugins>
                          <ext:CellEditing runat="server" ClicksToEdit="1" ID="uxCellEditor">
                              <DirectEvents>
                                  <Edit OnEvent="deSaveSortOrder">
                                       <ExtraParams>
                                             <ext:Parameter Name="Values" Value="Ext.encode(#{uxAccountCategoryGridPanel}.getRowsValues())" Mode="Raw" />
                                       </ExtraParams>
                                  </Edit>
                              </DirectEvents>
                          </ext:CellEditing>
                      </Plugins>                
                        </ext:GridPanel>

                  <ext:GridPanel ID="uxAccountListGridPanel" runat="server" Flex="1" SimpleSelect="true" Frame="true" Padding="5" Margins="5 5 5 5" Region="South" Title="General Ledger Accounts By Category">
                      <TopBar>
                        <ext:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <ext:Button runat="server" ID="uxAccountMaintenace"  Icon="Add" Text="Add" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deLoadAccountMaintenance"></Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" ID="uxUnAssignAccounts"  Icon="Delete" Text="Delete" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deDeleteAccounts"><Confirmation Message="Are you sure you want to remove the following account(s) from the assigned category?" ConfirmRequest="true"></Confirmation><EventMask ShowMask="true"></EventMask></Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxAccountListStore"
                            AutoDataBind="true" RemoteSort="true" AutoLoad="false" OnReadData="uxAccountListStore_ReadData" >
                            <Model>
                                        <ext:Model ID="Model1" runat="server" IDProperty="ACCOUNT_CATEGORY_ID">
                                            <Fields>
                                                <ext:ModelField Name="CATEGORY_ID"  />
                                                <ext:ModelField Name="ACCOUNT_SEGMENT"  />
                                                <ext:ModelField Name="ACCOUNT_SEGMENT_DESC"  />
                                                <ext:ModelField Name="ACCOUNT_ORDER"  />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:PageProxy />
                                    </Proxy>
                            <Sorters>
                                <ext:DataSorter Direction="ASC" Property="ACCOUNT_SEGMENT"></ext:DataSorter>
                            </Sorters>
                                </ext:Store>
                            </Store>
                            <ColumnModel>
                                <Columns>
                                    <ext:Column ID="Column4" runat="server" DataIndex="ACCOUNT_ORDER" Text="Account Order" Width="150" />
                                    <ext:Column ID="Column3" runat="server" DataIndex="ACCOUNT_SEGMENT_DESC" Text="Account Name" Flex="1" />
                                </Columns>
                            </ColumnModel>
                     <SelectionModel>
                         <ext:RowSelectionModel runat="server" Mode="Simple" ID="uxAccountListSelectionModel" AllowDeselect="true">
                              <Listeners>
                                 <Select Handler="if(#{uxAccountListSelectionModel}.getCount() > 0){#{uxUnAssignAccounts}.enable();}else {#{uxUnAssignAccounts}.disable();}"></Select>
                                        <Deselect Handler="if(#{uxAccountListSelectionModel}.getCount() > 0){#{uxUnAssignAccounts}.enable();}else {#{uxUnAssignAccounts}.disable();}"></Deselect>
                                 </Listeners>
                         </ext:RowSelectionModel>
                            </SelectionModel>
                          <Plugins>
                          <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true"></ext:FilterHeader>
                      </Plugins>
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
                            </BottomBar>   
                    <View>
                        <ext:GridView ID="GridView1" StripeRows="true" runat="server">
                        </ext:GridView>
                    </View>                     
                        </ext:GridPanel>

                </Items>
            </ext:Viewport>

        <ext:Window runat="server" Stateful="false" Width="450" Height="200" Title="Add/Edit Category" Layout="FitLayout" Header="true" Resizable="false" Hidden="true" ID="uxCategoryWindow" CloseAction="Hide" Closable="true" Modal="true" DefaultButton="uxSaveCategory">
            <Items>
              <ext:FormPanel ID="uxAccountCategoryForm" runat="server" Header="false" Frame="true" BodyPadding="10" DefaultButton="uxAddBudgetType"
                    Margins="5 5 5 5" Region="Center" >
                    <Items>
                        <ext:FieldContainer ID="FieldContainer1" 
                        runat="server"
                        LabelStyle="font-weight:bold;padding:0;"
                        Layout="HBoxLayout">
                        <Items>
                           <ext:TextField runat="server" FieldLabel="Name" ID="uxCategoryName" FieldStyle="background-color: #EFF7FF; background-image: none;"  Flex="1"></ext:TextField>
                        </Items>
                    </ext:FieldContainer>
                        <ext:FieldContainer ID="FieldContainer2" 
                        runat="server"
                        LabelStyle="font-weight:bold;padding:0;"
                        Layout="HBoxLayout">
                        <Items>
                         <ext:TextArea runat="server" FieldLabel="Description" ID="uxCategoryDescription"   Flex="1"></ext:TextArea>
                        </Items>
                    </ext:FieldContainer>  
                        </Items>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button runat="server" ID="uxSaveCategory" Icon="Accept" Text="Save">
                   <DirectEvents>
                       <Click OnEvent="deSaveAccountCategory"><Confirmation ConfirmRequest="true" Message="Are you sure you want to save this account category"></Confirmation><EventMask ShowMask="true"></EventMask></Click>
                   </DirectEvents>
                </ext:Button>
                  <ext:Button runat="server" ID="uxCancelSaveCategory" Icon="Cancel" Text="Cancel">
                      <Listeners>
                          <Click Handler="#{uxCategoryWindow}.close();"></Click>
                      </Listeners>
                  </ext:Button>
            </Buttons>
        </ext:Window>


    </form>
</body>
</html>

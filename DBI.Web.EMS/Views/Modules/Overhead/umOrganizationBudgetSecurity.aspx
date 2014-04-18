<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umOrganizationBudgetSecurity.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umOrganizationBudgetSecurity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
      

        <ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout" IDMode="Explicit" Namespace="App" RenderXType="True"> 
            <Items>
                <ext:GridPanel ID="uxGlAccountSecurityGrid" runat="server" Flex="1" Title="Budget Organizations" Padding="5" Region="Center" >
                     <TopBar>
                        <ext:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                 <ext:Button ID="Button2" runat="server" Text="Modify Budget Draft" Icon="DatabaseGear">
                                 </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="uxBudgetVersionByOrganizationStore"
                            AutoDataBind="true" RemoteSort="true"  AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model1" runat="server" IDProperty="OVERHEAD_GL_ID">
                                    <Fields>
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
                            <ext:Column ID="Column17" runat="server" DataIndex="SEGMENT1" Text="Organization Name" Flex="1" />
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
                     <SelectionModel>
                         <ext:CheckboxSelectionModel runat="server"></ext:CheckboxSelectionModel>
                     </SelectionModel>
                </ext:GridPanel>

                <ext:GridPanel ID="GridPanel1" runat="server" Flex="1" Title="Budget Version By Organization" Padding="5" Region="South" >
                    <Store>
                        <ext:Store runat="server"
                            ID="Store1"
                            AutoDataBind="true" RemoteSort="true"  AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model2" runat="server" IDProperty="OVERHEAD_GL_ID">
                                    <Fields>
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
                            <ext:Column ID="Column2" runat="server" DataIndex="SEGMENT1" Text="Fiscal Year" Flex="1" />
                            <ext:Column ID="Column6" runat="server" DataIndex="SEGMENT2" Text="Budget Draft" Flex="1" />
                             <ext:Column ID="Column7" runat="server" DataIndex="SEGMENT2" Text="Status" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
                    </Plugins>
                    
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar2" runat="server" />
                    </BottomBar>
                     <View>
                        <ext:GridView ID="GridView2" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View> 
                </ext:GridPanel>
            </Items>
        </ext:Viewport>


      
    </form>
</body>
</html>

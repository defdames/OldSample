<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddOverheadDetailLine.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umAddOverheadDetailLine" %>

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

         <ext:GridPanel ID="GridPanel2" runat="server" Flex="1" Title="Account Detail Lines" Padding="5" Region="Center" >
                     <TopBar>
                         <ext:Toolbar ID="Toolbar2" runat="server">
                             <Items>
                                 <ext:Button ID="Button2" runat="server" Text="Add" icon="ApplicationAdd"></ext:Button>
                                   <ext:Button ID="Button3" runat="server" Text="Delete" icon="ApplicationDelete"></ext:Button>
                                   <ext:Button ID="Button4" runat="server" Text="Edit" Icon="ApplicationEdit"></ext:Button>
                             </Items>
                         </ext:Toolbar>
                     </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="Store2"
                            AutoDataBind="true" RemoteSort="true"  AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model3" runat="server" IDProperty="OVERHEAD_GL_ID">
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
                            <ext:Column ID="Column110" runat="server" DataIndex="SEGMENT1" Text="Description" Flex="2" />
                             <ext:Column ID="Column113" runat="server" DataIndex="SEGMENT2" Text="Amount" Flex="1" />
                            <ext:Column ID="Column111" runat="server" DataIndex="SEGMENT2" Text="Spread Type" Flex="1" />
                            <ext:Column ID="Column112" runat="server" DataIndex="SEGMENT2" Text="Effective Start Date" Flex="1" />
                            <ext:Column ID="Column115" runat="server" DataIndex="SEGMENT2" Text="Effective End Date" Flex="1" />
                            <ext:ComponentColumn ID="ComponentColumn1" runat="server" DataIndex="SEGMENT2" Text="Comments" Flex="2">
                                <Component>
                                    <ext:TextArea ID="TextArea1" runat="server"></ext:TextArea>
                                </Component>
                            </ext:ComponentColumn>
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
                    </Plugins>
                    
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar2" runat="server" />
                    </BottomBar>
                     <View>
                        <ext:GridView ID="GridView3" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View> 
                </ext:GridPanel>
                   <ext:GridPanel ID="GridPanel3" runat="server" Flex="1" Title="Line Detail" Padding="5" Region="South" >
                     <TopBar>
                         <ext:Toolbar ID="Toolbar3" runat="server">
                             <Items>
                                   <ext:Button ID="Button7" runat="server" Text="Edit" Icon="ApplicationEdit"></ext:Button>
                             </Items>
                         </ext:Toolbar>
                     </TopBar>
                    <Store>
                        <ext:Store runat="server"
                            ID="Store3"
                            AutoDataBind="true" RemoteSort="true"  AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model4" runat="server" IDProperty="OVERHEAD_GL_ID">
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
                            <ext:Column ID="Column116" runat="server" DataIndex="SEGMENT1" Text="Line Date" Flex="1" />
                            <ext:Column ID="Column117" runat="server" DataIndex="SEGMENT2" Text="Amount" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="FilterHeader2" runat="server" Remote="false" />
                    </Plugins>
                     <View>
                        <ext:GridView ID="GridView4" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View> 
                </ext:GridPanel>

                </Items>
            </ext:Viewport>
    </form>
</body>
</html>

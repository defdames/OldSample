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
                   <ext:GridPanel ID="GridPanel3" runat="server" Flex="1" Title="Line Detail" Header="false" Padding="5" Region="Center" >
                    <Store>
                        <ext:Store runat="server"
                            ID="uxDetailStore"
                            AutoDataBind="true" RemoteSort="true" OnReadData="deLoadDetailLinesStore" AutoLoad="true">
                            <Model>
                                <ext:Model ID="Model4" runat="server" IDProperty="BUDGET_DETAIL_ID">
                                    <Fields>
                                        <ext:ModelField Name="PERIOD"></ext:ModelField>
                                        <ext:ModelField Name="AMOUNT"></ext:ModelField>
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
                            <ext:Column ID="Column116" runat="server" DataIndex="PERIOD" Text="Period" Flex="1" />
                            <ext:Column ID="Column117" runat="server" DataIndex="AMOUNT" Text="Amount" Flex="1" />
                        </Columns>
                    </ColumnModel>
                     <View>
                        <ext:GridView ID="GridView4" StripeRows="true" runat="server" TrackOver="true">
                        </ext:GridView>
                    </View> 
                       <BottomBar>
                           <ext:Toolbar runat="server">
                               <Items>
                                   <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                                   <ext:Button runat="server" ID="uxSaveDetailLine" Text="Save" Icon="Disk"></ext:Button>
                                    <ext:Button runat="server" ID="uxCancelDetailLine" Text="Cancel" Icon="Cancel"></ext:Button>
                               </Items>
                           </ext:Toolbar>
                       </BottomBar>
                </ext:GridPanel>

                </Items>
            </ext:Viewport>
    </form>
</body>
</html>

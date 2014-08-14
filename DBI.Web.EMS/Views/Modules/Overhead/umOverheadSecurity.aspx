﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umOverheadSecurity.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umOverheadSecurity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout" Namespace="App" IDMode="Explicit">
            <Items>

                  <ext:TreePanel
                    ID="uxOrganizationTreePanel"
                    runat="server"
                    Title="Business Units"
                    Region="West"
                    Width="250"
                    AutoScroll="true"
                    RootVisible="false"
                    SingleExpand="true"
                    Lines="false"
                    UseArrows="true"
                    Padding="5"
                    Scroll="Vertical"
                    Collapsible="true">
                    <Store>
                        <ext:TreeStore ID="uxOrganizationTreeStore" runat="server" OnReadData="deLoadLegalEntities">
                            <Proxy>
                                <ext:PageProxy></ext:PageProxy>
                            </Proxy>
                            <DirectEvents>
                                <BeforeLoad><EventMask ShowMask="true"></EventMask></BeforeLoad>
                            </DirectEvents>
                        </ext:TreeStore>
                    </Store>
                    <Root>
                        <ext:Node NodeID="0" Expanded="true" />
                    </Root>
                    <SelectionModel>
                        <ext:TreeSelectionModel runat="server" Mode="Single" AllowDeselect="true">
                            <DirectEvents>
                                <Select OnEvent="deSelectNode" ><EventMask ShowMask="true"></EventMask></Select>
                            </DirectEvents>
                        </ext:TreeSelectionModel>
                    </SelectionModel>
                       <View>
                        <ext:TreeView ID="TreeView1" runat="server" LoadMask="true">
                        </ext:TreeView>
                    </View>
                </ext:TreePanel>

                <ext:TabPanel runat="server" DeferredRender="true" Region="Center" ID="uxCenterTabPanel" Padding="5" >
                    <Items>
                        <ext:Panel runat="server" Title="Dashboard" Html="This is for a future release that allows a dashboard view for the system admin. Uncompleted budgets, notifications etc.">
                        </ext:Panel>
                    </Items>
                </ext:TabPanel>
                
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

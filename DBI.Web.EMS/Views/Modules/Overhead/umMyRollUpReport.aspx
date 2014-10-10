<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umMyRollUpReport.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Overhead.umMyRollUpReport" %>

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
                  <ext:TreePanel
                    ID="uxOrgPanel"
                    runat="server"
                    Title="Organizations"
                    BodyPadding="6"
                    Region="West"
                    Weight="100"
                    Width="300"
                    AutoScroll="true"
                    RootVisible="false"
                    SingleExpand="true"
                    Lines="false"
                    UseArrows="true"
                    Collapsible="true" Padding="5">
                    <Store>
                        <ext:TreeStore ID="TreeStore1" runat="server" OnReadData="deLoadOrgTree">
                            <Proxy>
                                <ext:PageProxy></ext:PageProxy>
                            </Proxy>
                        </ext:TreeStore>
                    </Store>
                    <Root>
                        <ext:Node NodeID="0" Text="All Companies" Expanded="true" />
                    </Root>
                    <SelectionModel>
                        <ext:TreeSelectionModel ID="uxCompanySelectionModel" runat="server" Mode="Single">
                             <DirectEvents>
                                <Select OnEvent="deSelectNode" ><EventMask ShowMask="true"></EventMask>
                                    <ExtraParams>
                                        <ext:Parameter Mode="Raw" Value="record.data.text" Name="ORGANIZATION_NAME"></ext:Parameter>
                                    </ExtraParams>
                                </Select>
                            </DirectEvents>
                        </ext:TreeSelectionModel>
                    </SelectionModel>   
                </ext:TreePanel>

                <ext:TabPanel runat="server" DeferredRender="true" Region="Center" ID="uxCenterTabPanel" Padding="5" >
                    <Items>
                    </Items>
                </ext:TabPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

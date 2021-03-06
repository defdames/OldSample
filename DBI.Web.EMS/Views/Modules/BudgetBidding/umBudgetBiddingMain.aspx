﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umBudgetBiddingMain.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.umBudgetBiddingMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .budgetTitle .x-form-display-field {
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>


                <%-------------------------------------------------- Org panel --------------------------------------------------%>
                <ext:TreePanel
                    ID="uxOrgPanel"
                    runat="server"
                    Title="Organizations"
                    BodyPadding="6"
                    Region="West"
                    Weight="100"
                    Width="300"
                    AutoScroll="true"
                    RootVisible="true"
                    SingleExpand="true"
                    Lines="false"
                    UseArrows="true"
                    Collapsible="true">
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
                        <ext:TreeSelectionModel ID="uxCompanySelectionModel" runat="server" Mode="Single"></ext:TreeSelectionModel>
                    </SelectionModel>
                    <DirectEvents>
                        <ItemClick OnEvent="deSelectOrg">
                            <ExtraParams>
                                <ext:Parameter Name="node" Value="record.data.id" Mode="Raw" />
                            </ExtraParams>
                        </ItemClick>
                    </DirectEvents>
                </ext:TreePanel>


                <%-------------------------------------------------- Toolbar --------------------------------------------------%>
                <ext:Toolbar ID="uxMainToolbar" runat="server" Region="North">
                    <Items>
                        <ext:ComboBox ID="uxFiscalYear"
                            runat="server"
                            ValueField="ID_NAME"
                            DisplayField="ID_NAME"
                            Width="120"
                            EmptyText="-- Year --"
                            Editable="false"
                            Disabled="true">
                            <Store>
                                <ext:Store ID="uxFiscalYearStore" runat="server" OnReadData="deLoadFiscalYears" AutoLoad="false">
                                    <Model>
                                        <ext:Model ID="Model1" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="ID_NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:PageProxy />
                                    </Proxy>
                                </ext:Store>
                            </Store>
                            <DirectEvents>
                                <Select OnEvent="deSelectYear">
                                </Select>
                            </DirectEvents>
                        </ext:ComboBox>

                        <ext:Label ID="uxSpace1" runat="server" Width="10" />

                        <ext:ComboBox ID="uxVersion"
                            runat="server"
                            ValueField="ID"
                            DisplayField="ID_NAME"
                            Width="120"
                            EmptyText="-- Versions --"
                            Editable="false"
                            Disabled="true">
                            <Store>
                                <ext:Store ID="uxVersionStore" runat="server" OnReadData="deLoadBudgetVersions" AutoLoad="false">
                                    <Model>
                                        <ext:Model ID="Model2" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="ID" />
                                                <ext:ModelField Name="ID_NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:PageProxy />
                                    </Proxy>
                                </ext:Store>
                            </Store>
                            <DirectEvents>
                                <Select OnEvent="deSelectVersion">
                                </Select>
                            </DirectEvents>
                        </ext:ComboBox>

                        <ext:Label ID="uxSpace2" runat="server" Width="10" />

                        <ext:DisplayField ID="uxYearVersionTitle" runat="server" Text=" " Cls="budgetTitle" />

                        <ext:ToolbarFill />

                        <ext:Button ID="uxOrgSettings" runat="server" Text="Org Settings" Icon="Cog" Disabled="true">
                            <DirectEvents>
                            </DirectEvents>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>


                <%-------------------------------------------------- Budget Panel --------------------------------------------------%>
                <ext:Panel
                    ID="uxBudgetPanel"
                    runat="server"
                    Region="Center">
                    <Loader ID="Loader1"
                        runat="server"
                        Url="umBlankBudget.aspx"
                        Mode="Frame">
                        <LoadMask ShowMask="true" />
                    </Loader>
                </ext:Panel>

                <ext:Hidden ID="uxHidOrgOK" runat="server" />
                <ext:Hidden ID="uxHidYearOK" runat="server" />
                <ext:Hidden ID="uxHidVerOK" runat="server" />

            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

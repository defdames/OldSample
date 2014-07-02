<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umManageOrgs.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umManageOrgs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" IsDynamic="false" />
        <ext:Viewport runat="server" Layout="BorderLayout">
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
                    RootVisible="true"
                    SingleExpand="true"
                    Lines="false"
                    UseArrows="true">
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
                        <ext:TreeSelectionModel ID="uxCompanySelectionModel" runat="server" Mode="Single" />
                    </SelectionModel>
                    <Listeners>
                        <ItemClick Handler="#{uxDollarStore}.reload()" />
                    </Listeners>
                </ext:TreePanel>
                <ext:GridPanel runat="server" ID="uxDollarGrid" Title="Dollar Threshold" Region="North">
                    <Store>
                        <ext:Store runat="server" ID="uxDollarStore" AutoLoad="false" AutoDataBind="true" OnReadData="deReadDollars" PageSize="10">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="AMOUNT_ID" />
                                        <ext:ModelField Name="ORG_ID" />
                                        <ext:ModelField Name="HIERARCHY_NAME" />
                                        <ext:ModelField Name="LOW_THRESHOLD" />
                                        <ext:ModelField Name="HIGH_THRESHOLD" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="LOW_THRESHOLD" Direction="ASC" />
                            </Sorters>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="Organization Name" DataIndex="HIERARCHY_NAME" />
                            <ext:Column runat="server" Text="Lower Threshold" DataIndex="LOW_THRESHOLD">
                                <Renderer Format="UsMoney" />
                            </ext:Column>
                            <ext:Column runat="server" Text="Upper Threshold" DataIndex="HIGH_THRESHOLD">
                                <Renderer Format="UsMoney" />
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <DirectEvents>
                        <Select OnEvent="deLoadFormThreshold">
                            <ExtraParams>
                                <ext:Parameter Name="AmountId" Value="#{uxDollarGrid}.getSelectionModel().getSelection()[0].data.AMOUNT_ID" Mode="Raw" />
                            </ExtraParams>
                            <EventMask ShowMask="true" />
                        </Select>
                    </DirectEvents>
                </ext:GridPanel>
                <ext:FormPanel runat="server" ID="uxOrganizationForm" BodyPadding="10" Region="Center">
                    <Items>
                        <ext:Hidden runat="server" ID="uxFormType" />
                        <ext:NumberField runat="server" ID="uxSmallThreshold" FieldLabel="Threshold for Small Jobs in %" Width="650" LabelWidth="150" InputWidth="50" MinValue="1" MaxValue="100" AllowBlank="false" AllowDecimals="false" AllowExponential="false" />
                        <ext:FieldSet runat="server" Title="Thresholds for Large Jobs" Width="650">
                            <Items>
                                <ext:NumberField runat="server" ID="uxFirstLargeThreshold" FieldLabel="First Threshold in %" LabelWidth="150" InputWidth="50" MinValue="1" MaxValue="100" AllowBlank="false" AllowDecimals="false" AllowExponential="false" />
                                <ext:NumberField runat="server" ID="uxSecondLargeThreshold" FieldLabel="Second Threshold in %" LabelWidth="150" InputWidth="50" MinValue="1" MaxValue="100" AllowBlank="false" AllowDecimals="false" AllowExponential="false" />
                            </Items>
                        </ext:FieldSet>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxOrganizationSubmitButton" Text="Submit" Icon="Add" Disabled="true">
                            <DirectEvents>
                                <Click OnEvent="deSubmitThreshold" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="uxOrganizationCancelButton" Text="Cancel" Icon="Delete">
                        </ext:Button>
                    </Buttons>
                    <Listeners>
                        <ValidityChange Handler="#{uxOrganizationSubmitButton}.setDisabled(!valid)" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

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
                    <DirectEvents>
                        <ItemClick OnEvent="deLoadFormThresholds" />
                    </DirectEvents>
                </ext:TreePanel>
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

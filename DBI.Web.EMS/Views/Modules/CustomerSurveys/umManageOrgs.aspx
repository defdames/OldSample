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
                        <ItemClick Handler="#{uxFormsGridStore}.reload()" />
                    </Listeners>
                </ext:TreePanel>
                <%--<ext:GridPanel runat="server" ID="uxFormsGrid" Region="North">
                    <Store>
                        <ext:Store runat="server" ID="uxFormsGridStore" OnReadData="deReadForms" AutoDataBind="true">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="FORM_ID" />
                                        <ext:ModelField Name="FORMS_NAME" />
                                        <ext:ModelField Name="NUM_QUESTIONS" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" DataIndex="FORMS_NAME" Text="Form Name" Flex="50" />
                            <ext:Column runat="server" DataIndex="NUM_QUESTIONS" Text="Number of Questions" Flex="50" />
                        </Columns>
                    </ColumnModel>
                    <DirectEvents>
                        <Select OnEvent="deLoadFormThresholds">
                            <ExtraParams>
                                <ext:Parameter Name="FormId" Value="#{uxFormsGrid}.getSelectionModel().getSelection()[0].data.FORM_ID" Mode="Raw" />
                                <ext:Parameter Name="OrgId" Value="#{uxOrgPanel}.getSelectionModel().getSelectedNode()" Mode="Raw" />
                            </ExtraParams>
                        </Select>
                    </DirectEvents>
                </ext:GridPanel>--%>
                <ext:FormPanel runat="server" ID="uxOrganizationForm" BodyPadding="10" Region="Center">
                    <Items>
                        <ext:NumberField runat="server" FieldLabel="Threshold for Small Jobs" Width="650" LabelWidth="150" InputWidth="50" MinValue="1" MaxValue="100" AllowBlank="false" AllowDecimals="false" AllowExponential="false" />
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

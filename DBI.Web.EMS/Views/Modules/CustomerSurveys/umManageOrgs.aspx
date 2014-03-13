<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umManageOrgs.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CustomerSurveys.umManageOrgs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" IsDynamic="false" />
        <ext:Viewport runat="server" Layout="AutoLayout">
            <Items>
                <ext:FormPanel runat="server" ID="uxOrganizationForm" BodyPadding="10" Layout="FormLayout" Width="650">
                    <Items>
                        <ext:ComboBox runat="server" ID="uxOrganizationComboBox"
                            QueryMode="Local"
                            TypeAhead="true"
                            FieldLabel="Choose an Organization"
                            DisplayField="LONG_NAME"
                            ValueField="ORG_ID">
                            <Store>
                                <ext:Store runat="server" ID="uxOrganizationStore" AutoDataBind="true">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="ORG_ID" />
                                                <ext:ModelField Name="LONG_NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                        </ext:ComboBox>
                        <ext:DropDownField runat="server" ID="uxFormDropDown"
                            FieldLabel="Choose a form">
                            <Component>
                                <ext:GridPanel runat="server" ID="uxFormDropGrid">
                                    <Store>
                                        <ext:Store runat="server" ID="uxFormDropStore" OnReadData="deReadForms" AutoDataBind="true">
                                            <Model>
                                                <ext:Model runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="FORMS_NAME" />
                                                        <ext:ModelField Name="NUM_QUESTIONS" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column runat="server" Text="Form Name" DataIndex="FORMS_NAME" />
                                            <ext:Column runat="server" Text="Number of Questions" DataIndex="NUM_QUESTIONS" />
                                        </Columns>
                                    </ColumnModel>
                                </ext:GridPanel>
                            </Component>
                        </ext:DropDownField>
                        <ext:TextField runat="server" FieldLabel="Threshold for Small Jobs" />
                        <ext:FieldSet runat="server" Title="Thresholds for Large Jobs">
                            <Items>
                                <ext:TextField runat="server" ID="uxFirstLargeThreshold" FieldLabel="First Threshold in %" />
                                <ext:TextField runat="server" ID="uxSecondLargeThreshold" FieldLabel="Second Threshold in %" />
                            </Items>
                        </ext:FieldSet>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxOrganizationSubmitButton" Text="Submit" Icon="Add">

                        </ext:Button>
                        <ext:Button runat="server" ID="uxOrganizationCancelButton" Text="Cancel" Icon="Delete">

                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umDailyActivity.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umDailyActivity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server"  IsDynamic="False" RethrowAjaxExceptions="true">
    </ext:ResourceManager>
    <form id="form1" runat="server">
        <ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout" IDMode="Explicit" IsDynamic="False" Namespace="App" RenderXType="True">
            <Items>
                <ext:MenuPanel ID="uxMenuPanel" runat="server" Region="West">
                    <Menu runat="server">
                        <Items>
                            <ext:MenuItem ID="uxCreate" Icon="ApplicationAdd" Text="Create Activity" />
                            <ext:MenuItem ID="uxManage" Icon="ApplicationEdit" Text="Manage Existing" />
                        </Items>
                    </Menu>
                </ext:MenuPanel>
                <ext:Panel runat="server" Region="Center" ID="uxCenterPanel" Layout="FitLayout">
                    <Items>
                        <ext:FormPanel ID="uxFormPanel" runat="server" Layout="AnchorLayout" BodyPadding="5" DefaultAnchor="50%" Title="Add Activity" ButtonAlign="Left">
                            <Items>
                                <%--<ext:ComboBox runat="server" ID="uxFormProject" FieldLabel="Select a Project">
                                    <Store>
                                        <ext:Store runat="server" DataSource="uxFormProjectDataSource" AutoDataBind="true">
                                            <Model>
                                                <ext:Model runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="name" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>--%>
                                <ext:DateField runat="server" ID="uxFormDate" FieldLabel="Date" />
                                <ext:TextField runat="server" ID="uxFormSubDivision" FieldLabel="Subdivision"  />
                                <ext:TextField runat="server" ID="uxFormContractor" FieldLabel="Contractor"  />
                                <%--<ext:ComboBox runat="server" ID="uxFormEmployee" FieldLabel="Supervisor/Area Manager">
                                    <Store>
                                        <ext:Store runat="server" DataSource="uxFormEmployeeDataSource" AutoDataBind="true">
                                            <Model>
                                                <ext:Model runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="name" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>--%>
                                <ext:TextField runat="server" ID="uxFormLicense" FieldLabel="License #" />
                                <ext:ComboBox runat="server" ID="uxFormState" FieldLabel="State" DisplayField="name" ValueField="abbr">
                                    <Store>
                                        <ext:Store runat="server" Data="<%# StateList %>" AutoDataBind="true">
                                            <Model>
                                                <ext:Model runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="abbr" />
                                                        <ext:ModelField Name="name" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                            <Reader>
                                                <ext:ArrayReader />
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>
                                <ext:TextField runat="server" ID="uxFormType" FieldLabel="Type" />
                                <ext:ComboBox runat="server" ID="uxFormDensity" FieldLabel="Density">
                                    <Items>
                                        <ext:ListItem Text="Low" Value="low" />
                                        <ext:ListItem Text="Medium" Value="medium" />
                                        <ext:ListItem Text="High" Value="high" />
                                    </Items>
                                </ext:ComboBox>
                            </Items>
                            <Buttons>
                                <ext:Button runat="server" ID="uxFormSubmit" Text="Submit">
                                    <DirectEvents>
                                        <Click OnEvent="deStoreActivity" />
                                    </DirectEvents>    
                                </ext:Button>
                                <ext:Button runat="server" ID="uxFormClear" Text="Clear">
                                    <Listeners>
                                        <Click Handler="#{uxFormPanel}.reset()" />
                                    </Listeners>
                                </ext:Button>
                            </Buttons>
                        </ext:FormPanel>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

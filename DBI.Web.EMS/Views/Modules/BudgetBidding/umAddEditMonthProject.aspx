<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddEditMonthProject.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.umAddEditMonthProject" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        var closeCancel = function () {
            parent.Ext.getCmp('uxAddEditProjectForm').close();
        }
        var closeUpdate = function () {
            parent.App.direct.CloseAddEditProjectWindow();
            //parent.App.uxSummaryGridStore.reload();
            parent.Ext.getCmp('uxAddEditProjectForm').close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>

                <ext:FormPanel ID="FormPanel1"
                    runat="server"
                    Region="Center"
                    BodyPadding="20"
                    Disabled="false">
                    <Items>

                        <ext:FieldContainer ID="FieldContainer1"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label1" runat="server" Width="120" Text="Project Number:" />
                                <ext:DropDownField ID="uxProjectNum" runat="server" Width="110" Mode="ValueText" Editable="false" FieldStyle="background-color: #EFF7FF; background-image: none;">
                                    <Listeners>
                                        <Expand Handler="this.picker.setWidth(500);" />
                                    </Listeners>
                                    <Component>
                                        <ext:GridPanel runat="server"
                                            ID="uxProjectList"
                                            Width="500"
                                            Layout="HBoxLayout"
                                            Frame="true"
                                            ForceFit="true">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxProjectNumStore"
                                                    PageSize="10"
                                                    RemoteSort="true"
                                                    OnReadData="deLoadProjectDropdown">
                                                    <Model>
                                                        <ext:Model ID="Model7" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="PROJECT_ID" />
                                                                <ext:ModelField Name="PROJECT_NUM" Type="String" />
                                                                <ext:ModelField Name="PROJECT_NAME" />
                                                                <ext:ModelField Name="TYPE" />
                                                                <ext:ModelField Name="ORDERKEY" />
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
                                                    <ext:Column ID="Column13" runat="server" Text="Project Number" DataIndex="PROJECT_NUM" Flex="1" />
                                                    <ext:Column ID="Column14" runat="server" Text="Project Long Name" DataIndex="PROJECT_NAME" Flex="3" />
                                                </Columns>
                                            </ColumnModel>
                                            <BottomBar>
                                                <ext:PagingToolbar ID="PagingToolbar3" runat="server" />
                                            </BottomBar>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel3" runat="server" Mode="Single" />
                                            </SelectionModel>
                                            <DirectEvents>
                                                <SelectionChange OnEvent="deSelectProject">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ProjectID" Value="#{uxProjectList}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
                                                        <ext:Parameter Name="ProjectNum" Value="#{uxProjectList}.getSelectionModel().getSelection()[0].data.PROJECT_NUM" Mode="Raw" />
                                                        <ext:Parameter Name="ProjectName" Value="#{uxProjectList}.getSelectionModel().getSelection()[0].data.PROJECT_NAME" Mode="Raw" />
                                                        <ext:Parameter Name="Type" Value="#{uxProjectList}.getSelectionModel().getSelection()[0].data.TYPE" Mode="Raw" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" />
                                                </SelectionChange>
                                            </DirectEvents>
                                            <Plugins>
                                                <ext:FilterHeader runat="server" ID="uxProjectFilter" Remote="true" />
                                            </Plugins>
                                        </ext:GridPanel>
                                    </Component>
                                </ext:DropDownField>
                                <ext:Label ID="Label5" runat="server" Width="415" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer4"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label7" runat="server" Width="120" Text="Project Long Name:" />
                                <ext:TextField ID="uxProjectName" runat="server" Width="380" ReadOnly="true" SelectOnFocus="true" MaxLength="200" EnforceMaxLength="true" FieldStyle="background-color: #EFF7FF; background-image: none;">
                                    <DirectEvents>
                                        <Change OnEvent="deCheckAllowSave" />
                                    </DirectEvents>
                                </ext:TextField>
                                <ext:Label ID="Label2" runat="server" Width="145" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer2"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label4" runat="server" Width="645" Height="260" />
                            </Items>
                        </ext:FieldContainer>

                        <ext:FieldContainer ID="FieldContainer17"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:Label ID="Label3" runat="server" Width="490" />
                                <ext:Button ID="uxSave" runat="server" Text="Save" Icon="Add" Width="75" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="deSave">
                                            <EventMask ShowMask="true" Msg="Saving..." />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Label ID="Label16" runat="server" Width="5" />
                                <ext:Button ID="uxCancel" runat="server" Text="Cancel" Icon="Delete" Width="75">
                                    <Listeners>
                                        <Click Fn="closeCancel" />
                                    </Listeners>
                                    <DirectEvents>
                                        <Click>
                                            <EventMask ShowMask="true" Msg="Canceling..." />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                </ext:FormPanel>

                <%--                <ext:Hidden ID="uxHidBudBidID" runat="server" />
                <ext:Hidden ID="uxHidProjectNumID" runat="server" />
                <ext:Hidden ID="uxHidType" runat="server" />--%>


                <ext:FormPanel ID="FormPanel2"
                    runat="server"
                    Region="South"
                    BodyPadding="20"
                    Height="120"
                    Disabled="false"
                    Visible="true">
                    <Items>
                        <ext:FieldContainer ID="FieldContainer3"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:TextField ID="uxHidBudBidID" runat="server" Width="100" />
                                <ext:TextField ID="uxHidProjectID" runat="server" Width="100" />
                                <ext:TextField ID="uxHidProjectNum" runat="server" Width="100" />
                                <ext:TextField ID="uxHidProjectName" runat="server" Width="100" />
                                <ext:TextField ID="uxHidType" runat="server" Width="100" />
                                <ext:TextField ID="uxHidDetailSheetID" runat="server" Width="100" />
                                <ext:TextField ID="uxHidAddNew" runat="server" Width="100" />
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                </ext:FormPanel>

            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

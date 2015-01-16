<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umAddEditMonthTask.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.umAddEditMonthTask" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        var closeCancel = function () {
            parent.Ext.getCmp('uxAddEditTaskForm').close();
        }
        var closeUpdate = function () {
            parent.App.direct.CloseAddEditTaskWindow();
            //parent.App.uxSummaryGridStore.reload();
            parent.Ext.getCmp('uxAddEditTaskForm').close();
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
                                <ext:Label ID="Label1" runat="server" Width="120" Text="Task Number:" />
                                <ext:DropDownField ID="uxTaskNum" runat="server" Width="110" Mode="ValueText" Editable="false" FieldStyle="background-color: #EFF7FF; background-image: none;">
                                    <Listeners>
                                        <Expand Handler="this.picker.setWidth(500);" />
                                    </Listeners>
                                    <Component>
                                        <ext:GridPanel runat="server"
                                            ID="uxTaskList"
                                            Width="500"
                                            Layout="HBoxLayout"
                                            Frame="true"
                                            ForceFit="true">
                                            <Store>
                                                <ext:Store runat="server"
                                                    ID="uxTaskNumStore"
                                                    PageSize="10"
                                                    RemoteSort="true"
                                                    OnReadData="deLoadTaskDropdown">
                                                    <Model>
                                                        <ext:Model ID="Model7" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="TASK_ID" />
                                                                <ext:ModelField Name="TASK_NUMBER" Type="String" />
                                                                <ext:ModelField Name="DESCRIPTION" />
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
                                                    <ext:Column ID="Column13" runat="server" Text="Task Number" DataIndex="TASK_NUMBER" Flex="1" />
                                                    <ext:Column ID="Column14" runat="server" Text="Task Description" DataIndex="DESCRIPTION" Flex="3" />
                                                </Columns>
                                            </ColumnModel>
                                            <BottomBar>
                                                <ext:PagingToolbar ID="PagingToolbar3" runat="server" />
                                            </BottomBar>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel3" runat="server" Mode="Single" />
                                            </SelectionModel>
                                            <DirectEvents>
                                                <SelectionChange OnEvent="deSelectTask">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="TaskID" Value="#{uxTaskList}.getSelectionModel().getSelection()[0].data.TASK_ID" Mode="Raw" />
                                                        <ext:Parameter Name="TaskNum" Value="#{uxTaskList}.getSelectionModel().getSelection()[0].data.TASK_NUMBER" Mode="Raw" />
                                                        <ext:Parameter Name="TaskName" Value="#{uxTaskList}.getSelectionModel().getSelection()[0].data.DESCRIPTION" Mode="Raw" />
                                                        <ext:Parameter Name="Type" Value="#{uxTaskList}.getSelectionModel().getSelection()[0].data.TYPE" Mode="Raw" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" />
                                                </SelectionChange>
                                            </DirectEvents>
                                            <Plugins>
                                                <ext:FilterHeader runat="server" ID="uxTaskFilter" Remote="true" />
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
                                <ext:Label ID="Label7" runat="server" Width="120" Text="Description:" />
                                <ext:TextField ID="uxTaskName" runat="server" Width="380" ReadOnly="true" SelectOnFocus="true" MaxLength="200" EnforceMaxLength="true" FieldStyle="background-color: #EFF7FF; background-image: none;">
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
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                </ext:FormPanel>

                <ext:FormPanel ID="FormPanel3"
                    runat="server"
                    Region="South"
                    BodyPadding="20"
                    Height="120"
                    Disabled="false"
                    Visible="true">
                    <Items>
                        <ext:FieldContainer ID="FieldContainer5"
                            runat="server"
                            Layout="HBoxLayout">
                            <Items>
                                <ext:TextField ID="uxHidDetailTaskID" runat="server" Width="100" />
                                <ext:TextField ID="uxHidDetailID" runat="server" Width="100" />
                                <ext:TextField ID="uxHidDetailNum" runat="server" Width="100" />
                                <ext:TextField ID="uxHidDetailName" runat="server" Width="100" />
                                <ext:TextField ID="uxHidDetailType" runat="server" Width="100" />
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

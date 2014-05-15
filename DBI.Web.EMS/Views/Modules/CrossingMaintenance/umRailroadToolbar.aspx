<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umRailroadToolbar.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umRailroadToolbar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <ext:ResourceManager ID="ResourceManager2" runat="server" />

        <div>
            <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
                <Items>
                    <ext:Toolbar ID="Toolbar1" runat="server" Region="North">
                        <Items>
                            <ext:Button ID="uxChangeRailroad" runat="server" Text="Change Railroad" Icon="ShapeMoveBack" >

                                <Listeners>
                                    <Click Handler="#{uxChangeRailroadWindow}.show()" />
                                </Listeners>

                                <%-- <DirectEvents>
                        <Click OnEvent="deAssociateCrossings">
                            <Confirmation ConfirmRequest="true" Title="Associate?" Message="Are you sure you want to associate the selected crossings with the selected project?" />
                            <ExtraParams>
                                <ext:Parameter Name="projectId" Value="#{uxProjectGrid}.getSelectionModel().getSelection()[0].data.PROJECT_ID" Mode="Raw" />
                                <ext:Parameter Name="selectedCrossings" Value="Ext.encode(#{uxCrossingGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />

                            </ExtraParams>
                        </Click>--%>


                                <%--</DirectEvents>--%>
                            </ext:Button>
                            <ext:TextField runat="server" ID="uxRailRoadCITextField" FieldLabel="Railroad" LabelAlign="Right" ReadOnly="true" />
                           <%-- <ext:ComboBox ID="uxRailRoadCI"
                                runat="server"
                                FieldLabel="Rail Road"
                                LabelAlign="Right"
                                DisplayField="RAILROAD"
                                ValueField="RAILROAD_ID"
                                QueryMode="Local"
                                TypeAhead="true" Editable="false">
                                <Store>
                                    <ext:Store runat="server"
                                        ID="uxAddRailRoadStore">
                                        <Model>
                                            <ext:Model ID="Model4" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="RAILROAD_ID" />
                                                    <ext:ModelField Name="RAILROAD" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                                <Listeners>
                                               <BeforeSelect Handler="parent.App.#{uxCrossingInfoTab}.hide()" />
                                           </Listeners>

                                <DirectEvents>
                                    <Select OnEvent="deLoadUnit">
                                    </Select>
                                </DirectEvents>
                            </ext:ComboBox>--%>

                        </Items>
                    </ext:Toolbar>
               
                </Items>
            </ext:Viewport>

            
        </div>
    </form>
</body>
</html>

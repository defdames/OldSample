<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCrossingMain.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umCrossingMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Resources/StyleSheets/main.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div></div>
        <ext:ResourceManager ID="ResourceManager2" runat="server" />

        <div>
          
            <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
                <Items>
                       <ext:Toolbar ID="Toolbar1" runat="server" Region="North">
                        <Items>
                             <ext:ComboBox ID="uxRailRoadCI"
                                                runat="server"
                                                FieldLabel="Rail Road"
                                                LabelAlign="Right"
                                                DisplayField="RAILROAD"
                                                ValueField="RAILROAD_ID"
                                                QueryMode="Local"
                                                TypeAhead="true" Editable="false" ForceSelection="true">
                                                <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxSelectRailRoadStore">
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
                                               <%-- <DirectEvents>
                                                    <Select OnEvent="deLoadUnit">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Type" Value="Add" />
                                                        </ExtraParams>
                                                    </Select>
                                                </DirectEvents>
                                                <Listeners>
                                                    <Select Handler="#{uxAddServiceUnitStore}.load()" />
                                                </Listeners>--%>
                                            </ext:ComboBox>
                            
                        </Items>
                    </ext:Toolbar>
                   
                    <ext:TabPanel ID="uxCrossingTab" runat="server" Region="Center">
                        <Items>
                             <ext:Panel runat="server"
                                Title="Railroad & Project"
                                ID="uxRRProject"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader2" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umRailRoadProject.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>

                            </ext:Panel>
                            <ext:Panel runat="server"
                                Title="Crossing Security"
                                ID="uxCrossingSecurity"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader5" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umCrossingSecurity.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>

                            </ext:Panel>

                            <ext:Panel runat="server"
                                Title="Crossing Information"
                                ID="uxCrossingInfoTab"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="uxHeaderLoader" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umCrossingInfoTab.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>

                            </ext:Panel>
                            <ext:Panel runat="server"
                                Title="Contacts"
                                ID="uxContactsTab"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader1" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umContactsTab.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>

                            </ext:Panel>
                          
                           
                      

                        </Items>
                    </ext:TabPanel>

                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>

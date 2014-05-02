<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umDataEntryMain.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umDataEntryMain" %>

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
                                                TypeAhead="true" ForceSelection="true" Editable="false">
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
                                           <DirectEvents>
                                               
                                           </DirectEvents>
                                            </ext:ComboBox>
                            
                        </Items>
                    </ext:Toolbar>
                    <ext:TabPanel ID="uxCrossingTab" runat="server" Region="Center">
                        <Items>                       
                            <ext:Panel runat="server"
                                Title="Application Entry"
                                ID="uxDataEntryTab"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader4" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umDataEntryTab.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>
                                </ext:Panel>
                              
                             <ext:Panel runat="server"
                                Title="Supplemental"
                                ID="uxSupplemental"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader3" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umSupplemental.aspx">
                                    <LoadMask ShowMask="true" />
                                </Loader>
                            </ext:Panel>
                             <ext:Panel runat="server"
                                Title="Incidents"
                                ID="uxIncident"
                                Disabled="false">
                                <Loader runat="server"
                                    ID="Loader1" Mode="Frame" AutoLoad="true" ReloadOnEvent="true" Url="umIncident.aspx">
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

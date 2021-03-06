﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umDataEntryMain.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umDataEntryMain" %>

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

                        </Items>
                    </ext:TabPanel>

                </Items>
            </ext:Viewport>
   </div>
    </form>
</body>
</html>

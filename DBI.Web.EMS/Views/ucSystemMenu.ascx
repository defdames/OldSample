<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSystemMenu.ascx.cs" Inherits="DBI.Web.EMS.Views.ucSystemMenu" %>

<ext:Menu ID="uxMenu"
    runat="server"
    Floating="false"
    Layout="VBoxLayout"
    ShowSeparator="false" Border="false">
    <Defaults>
        <ext:Parameter Name="MenuAlign" Value="tl-bl?" Mode="Value" />
    </Defaults>
    <Items>
        <ext:MenuItem ID="uxAdministration" runat="server" Text="Administration">
            <Listeners>
            <Click Handler="#{DirectMethods}.LoadSecurity();" />
           </Listeners>
        </ext:MenuItem>
    </Items>
</ext:Menu>
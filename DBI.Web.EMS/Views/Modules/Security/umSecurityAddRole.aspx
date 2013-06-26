<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSecurityAddRole.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Security.umSecurityAddRole" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
    <script type="text/javascript">

        var refreshData = function (name) {
            Ext.net.DirectMethod.request('dmRefreshRoles', { url: 'umSecurityRolesList.aspx', cleanrequest: true });
        };
        </script>
<body>
    <ext:ResourceManager runat="server" IsDynamic="False" ShowWarningOnAjaxFailure="false" />

    <ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout" IDMode="Explicit" IsDynamic="False" Namespace="App" RenderXType="True">
        <Items>
          <ext:FormPanel
                ID="uxSecurityRoleDetails"
                runat="server"
                Region="Center"
                Split="true"
                Margins="5 5 5 5"
                BodyPadding="2"
                Frame="true"
                Width="300"
                DefaultAnchor="100%"
                AutoScroll="True">
                <Items>
                    <ext:TextField ID="uxName" runat="server" FieldLabel="Name"  />
                    <ext:TextField ID="uxDescription" runat="server" FieldLabel="Description"  /> 
                </Items>
              <Buttons>
                  <ext:Button runat="server" ID="uxCreateRole" Text="Create User Role" >
                      <DirectEvents>
                          <Click OnEvent ="deAddSecurityRole"><EventMask ShowMask="true"></EventMask></Click>
                      </DirectEvents>
                  </ext:Button>
                  <ext:Button runat="server" ID="uxCancelUserRole" Text="Cancel">
                      <Listeners>
                          <Click Handler ="parent.Ext.WindowMgr.getActive().close();"></Click>
                      </Listeners>
                  </ext:Button>
              </Buttons>
            </ext:FormPanel>
        </Items>
    </ext:Viewport>


</body>
</html>

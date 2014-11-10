<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Portrait.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.ReportTemplates.Portrait" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="100%" AsyncRendering="False" SizeToReportContent="True">
            <LocalReport ReportEmbeddedResource="DBI.Web.EMS.Views.Modules.ReportTemplates.Portrait.rdlc" ReportPath="Views\Modules\ReportTemplates\Portrait.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="Example" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="Get" TypeName="DBI.Data.BBDetail+SubGrid+Data">
            <SelectParameters>
                <asp:QueryStringParameter Name="orgID" QueryStringField="orgID" Type="Int64" />
                <asp:QueryStringParameter Name="projectID" QueryStringField="projectID" Type="Int64" />
                <asp:QueryStringParameter Name="detailSheetID" QueryStringField="detailSheetID" Type="Int64" />
                <asp:Parameter DefaultValue="MATERIAL" Name="recType" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </form>
</body>
</html>

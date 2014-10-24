<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppDateReport.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.Reports.AppDateReport" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="750px" Width="100%">
            <LocalReport ReportEmbeddedResource="DBI.Web.EMS.Views.Modules.CrossingMaintenance.Reports.AppDateReport.rdlc" ReportPath="Views\Modules\CrossingMaintenance\Reports\AppDateReport.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="AppDateReport" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetAppDateList" TypeName="DBI.Data.CROSSING_MAINTENANCE">
            <SelectParameters>
                <asp:QueryStringParameter Name="selectedRailroad" QueryStringField="selectedRailroad" Type="String" />
                <asp:QueryStringParameter Name="selectedServiceUnit" QueryStringField="selectedServiceUnit" Type="String" />
                <asp:QueryStringParameter Name="selectedSubDiv" QueryStringField="selectedSubDiv" Type="String" />
                <asp:QueryStringParameter Name="selectedState" QueryStringField="selectedState" Type="String" />
                <asp:QueryStringParameter Name="selectedApplication" QueryStringField="selectedApplication" Type="Decimal" />
                <asp:QueryStringParameter Name="selectedStart" QueryStringField="selectedStart" Type="DateTime" />
                <asp:QueryStringParameter Name="selectedEnd" QueryStringField="selectedEnd" Type="DateTime" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </form>
</body>
</html>

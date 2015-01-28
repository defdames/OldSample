<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SummaryReport.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.Reports.SummaryReport" %>

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
            <LocalReport ReportEmbeddedResource="DBI.Web.EMS.Views.Modules.CrossingMaintenance.Reports.SummaryReport.rdlc" ReportPath="Views\Modules\CrossingMaintenance\Reports\SummaryReport.rdlc">                 
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="CrossingSummaryReport" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetCrossingSummaryList" TypeName="DBI.Data.CROSSING_MAINTENANCE, DBI.Data, Version=2.2014.1010.283, Culture=neutral, PublicKeyToken=null">
            <SelectParameters>
                <asp:QueryStringParameter Name="selectedRailroad" QueryStringField="selectedRailroad" Type="String" />
                <asp:QueryStringParameter Name="selectedStart" QueryStringField="selectedStart" Type="DateTime" />
                <asp:QueryStringParameter Name="selectedEnd" QueryStringField="selectedEnd" Type="DateTime" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </form>
</body>
</html>

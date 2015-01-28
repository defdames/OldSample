<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InspectionReport.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.Reports.InspectionReport" %>

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
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="750px" Width="100%">
             <LocalReport ReportEmbeddedResource="DBI.Web.EMS.Views.Modules.CrossingMaintenance.Reports.InspectionReport.rdlc" ReportPath="Views\Modules\CrossingMaintenance\Reports\InspectionReport.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="InspectionReport" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetInspectionList" TypeName="DBI.Data.CROSSING_MAINTENANCE">
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
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </form>
</body>
</html>

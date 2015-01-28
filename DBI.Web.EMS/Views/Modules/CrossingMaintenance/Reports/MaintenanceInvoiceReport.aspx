<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaintenanceInvoiceReport.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.Reports.MaintenanceInvoiceReport" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server">
         </asp:ScriptManager>
    <div>
    
    </div>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="540px" Width="100%" WaitControlDisplayAfter="800">
            <LocalReport ReportEmbeddedResource="DBI.Web.EMS.Views.Modules.CrossingMaintenance.Reports.MaintenanceInvoiceReport.rdlc" ReportPath="Views\Modules\CrossingMaintenance\Reports\MaintenanceInvoiceReport.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="MaintenanceInvoiceReport" />
                        </DataSources>
                    </LocalReport>

        </rsweb:ReportViewer>
        
         <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetInvoiceReport" TypeName="DBI.Data.CROSSING_MAINTENANCE">
             <SelectParameters>
                 <asp:QueryStringParameter Name="selectedApp" QueryStringField="selectedApp" Type="String" />
             </SelectParameters>
         </asp:ObjectDataSource>
      
    </form>
</body>
</html>


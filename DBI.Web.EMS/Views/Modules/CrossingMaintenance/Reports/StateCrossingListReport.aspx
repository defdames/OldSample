<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StateCrossingListReport.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.Reports.StateCrossingList" %>

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
        <rsweb:ReportViewer ID="ReportViewer1" runat="server">
             <LocalReport ReportEmbeddedResource="DBI.Web.EMS.Views.Modules.CrossingMaintenance.Reports.StateCrossingListReport.rdlc" ReportPath="Views\Modules\CrossingMaintenance\Reports\StateCrossingList.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="StateCrossingList" />
                        </DataSources>
                    </LocalReport>
        </rsweb:ReportViewer>
         <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetStateCrossingList" TypeName="DBI.Data.CROSSING_MAINTENANCE">
             <SelectParameters>
                 <asp:Parameter Name="selectedRailroad" Type="String" />
                 <asp:Parameter Name="selectedServiceUnit" Type="String" />
                 <asp:Parameter Name="selectedSubDiv" Type="String" />
                 <asp:Parameter Name="selectedState" Type="String" />
             </SelectParameters>
         </asp:ObjectDataSource>
    </form>
</body>
</html>

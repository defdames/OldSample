<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umRepProject.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.Reports.umRepProject" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="473px">
                    <LocalReport ReportEmbeddedResource="DBI.Web.EMS.Views.Modules.BudgetBidding.Reports.umRepProject.rdlc" ReportPath="Views\Modules\BudgetBidding\Reports\umRepProject.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="Project_Details" />
                        </DataSources>
                    </LocalReport>
        </rsweb:ReportViewer>    
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ProjectDetails" TypeName="DBI.Data.BBReports">
            <SelectParameters>
                <asp:Parameter DefaultValue="48149" Name="budBidProjectID" Type="Int64" />
            </SelectParameters>
        </asp:ObjectDataSource>    
    </form>
</body>
</html>

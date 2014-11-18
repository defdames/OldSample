<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umRepRollupOPCompare.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.Reports.umRepRollupOPCompare" %>

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
                    <LocalReport ReportEmbeddedResource="DBI.Web.EMS.Views.Modules.BudgetBidding.Reports.umRepRollupOPCompare.rdlc" ReportPath="Views\Modules\BudgetBidding\Reports\umRepRollupOPCompare.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="Org_Summary" />
                        </DataSources>
                    </LocalReport>
        </rsweb:ReportViewer>    
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="RollupOrgOPCompare2" TypeName="DBI.Data.BBReports+RollupOrgOPCompare">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="" Name="userID" QueryStringField="userID" Type="Int64" />
                <asp:QueryStringParameter Name="hierarchyID" QueryStringField="hierID" Type="Int64" />
                <asp:QueryStringParameter DefaultValue="" Name="orgID" QueryStringField="orgID" Type="Int64" />
                <asp:QueryStringParameter DefaultValue="" Name="yearID" QueryStringField="yearID" Type="Int64" />
                <asp:QueryStringParameter DefaultValue="" Name="verID" QueryStringField="verID" Type="Int64" />
                <asp:QueryStringParameter DefaultValue="" Name="prevYearID" QueryStringField="prevYearID" Type="Int64" />
                <asp:QueryStringParameter DefaultValue="" Name="prevVerID" QueryStringField="prevVerID" Type="Int64" />
            </SelectParameters>
        </asp:ObjectDataSource>    
    </form>
</body>
</html>

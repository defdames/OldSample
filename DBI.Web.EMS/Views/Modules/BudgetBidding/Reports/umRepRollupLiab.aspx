<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umRepRollupLiab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.Reports.umRepOrgLiabilities" %>

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
                    <LocalReport ReportEmbeddedResource="DBI.Web.EMS.Views.Modules.BudgetBidding.Reports.RollupLiabilities.rdlc" ReportPath="Views\Modules\BudgetBidding\Reports\RollupLiabilities.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="All_Projects" />
                        </DataSources>
                    </LocalReport>
        </rsweb:ReportViewer>    
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Liabilities2" TypeName="DBI.Data.BBReports+Liabilities">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="" Name="orgName" QueryStringField="strorgName" Type="String" />
                <asp:QueryStringParameter DefaultValue="" Name="orgID" QueryStringField="orgID" Type="Int64" />
                <asp:QueryStringParameter DefaultValue="" Name="yearID" QueryStringField="yearID" Type="Int64" />
                <asp:QueryStringParameter DefaultValue="" Name="verID" QueryStringField="verID" Type="Int64" />
                <asp:QueryStringParameter DefaultValue="" Name="prevYearID" QueryStringField="prevYearID" Type="Int64" />
                <asp:QueryStringParameter DefaultValue="" Name="prevVerID" QueryStringField="prevVerID" Type="Int64" />
                <asp:QueryStringParameter DefaultValue="" Name="userID" QueryStringField="userID" Type="Int64" />
                <asp:QueryStringParameter DefaultValue="" Name="hierarchyID" QueryStringField="hierID" Type="Int64" />
            </SelectParameters>
        </asp:ObjectDataSource>    
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umReport1.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.Reports.WebForm1" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                    <LocalReport ReportEmbeddedResource="DBI.Web.EMS.Views.Modules.BudgetBidding.Reports.Report1.rdlc" ReportPath="Views\Modules\BudgetBidding\Reports\Report1.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                        </DataSources>
                    </LocalReport>
        </rsweb:ReportViewer>

        

            
    
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Data" TypeName="DBI.Data.BBSummary+Grid, DBI.Data, Version=2.2014.718.199, Culture=neutral, PublicKeyToken=null">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="" Name="orgName" QueryStringField="strorgName" Type="String" />
                <asp:QueryStringParameter DefaultValue="" Name="orgID" QueryStringField="orgID" Type="Int64" />
                <asp:QueryStringParameter DefaultValue="" Name="yearID" QueryStringField="yearID" Type="Int64" />
                <asp:QueryStringParameter DefaultValue="" Name="verID" QueryStringField="verID" Type="Int64" />
                <asp:QueryStringParameter DefaultValue="" Name="prevYearID" QueryStringField="prevYearID" Type="Int64" />
                <asp:QueryStringParameter DefaultValue="" Name="prevVerID" QueryStringField="prevVerID" Type="Int64" />
            </SelectParameters>
        </asp:ObjectDataSource>

        

            
    
      <%--  <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="Data" TypeName="DBI.Data.BBSummary+Grid">
            <SelectParameters>
                <asp:Parameter DefaultValue="" Name="orgName" Type="String"></asp:Parameter>
                <asp:Parameter DefaultValue="1605" Name="orgID" Type="Int64"></asp:Parameter>
                <asp:Parameter DefaultValue="2005" Name="yearID" Type="Int64"></asp:Parameter>
                <asp:Parameter DefaultValue="2" Name="verID" Type="Int64"></asp:Parameter>
                <asp:Parameter DefaultValue="2005" Name="prevYearID" Type="Int64"></asp:Parameter>
                <asp:Parameter DefaultValue="2" Name="prevVerID" Type="Int64"></asp:Parameter>
            </SelectParameters>
        </asp:ObjectDataSource>--%>



            
    
    </form>
</body>
</html>

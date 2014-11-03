<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umRepDetailSheet.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.Reports.umRepDetailSheet" %>

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
            <LocalReport ReportEmbeddedResource="DBI.Web.EMS.Views.Modules.BudgetBidding.Reports.umRepDetailSheet.rdlc" ReportPath="Views\Modules\BudgetBidding\Reports\umRepDetailSheet.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="Material" />
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="Equipment" />
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource3" Name="Personnel" />
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource4" Name="PerDiem" />
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource5" Name="Travel" />
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource6" Name="Motels" />
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource7" Name="Misc" />
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource8" Name="LumpSum" />
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
        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="Get" TypeName="DBI.Data.BBDetail+SubGrid+Data">
            <SelectParameters>
                <asp:QueryStringParameter Name="orgID" QueryStringField="orgID" Type="Int64" />
                <asp:QueryStringParameter Name="projectID" QueryStringField="projectID" Type="Int64" />
                <asp:QueryStringParameter Name="detailSheetID" QueryStringField="detailSheetID" Type="Int64" />
                <asp:Parameter DefaultValue="EQUIPMENT" Name="recType" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" SelectMethod="Get" TypeName="DBI.Data.BBDetail+SubGrid+Data">
            <SelectParameters>
                <asp:QueryStringParameter Name="orgID" QueryStringField="orgID" Type="Int64" />
                <asp:QueryStringParameter Name="projectID" QueryStringField="projectID" Type="Int64" />
                <asp:QueryStringParameter Name="detailSheetID" QueryStringField="detailSheetID" Type="Int64" />
                <asp:Parameter DefaultValue="PERSONNEL" Name="recType" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource4" runat="server" SelectMethod="Get" TypeName="DBI.Data.BBDetail+SubGrid+Data">
            <SelectParameters>
                <asp:QueryStringParameter Name="orgID" QueryStringField="orgID" Type="Int64" />
                <asp:QueryStringParameter Name="projectID" QueryStringField="projectID" Type="Int64" />
                <asp:QueryStringParameter Name="detailSheetID" QueryStringField="detailSheetID" Type="Int64" />
                <asp:Parameter DefaultValue="PERDIEM" Name="recType" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource5" runat="server" SelectMethod="Get" TypeName="DBI.Data.BBDetail+SubGrid+Data">
            <SelectParameters>
                <asp:QueryStringParameter Name="orgID" QueryStringField="orgID" Type="Int64" />
                <asp:QueryStringParameter Name="projectID" QueryStringField="projectID" Type="Int64" />
                <asp:QueryStringParameter Name="detailSheetID" QueryStringField="detailSheetID" Type="Int64" />
                <asp:Parameter DefaultValue="TRAVEL" Name="recType" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource6" runat="server" SelectMethod="Get" TypeName="DBI.Data.BBDetail+SubGrid+Data">
            <SelectParameters>
                <asp:QueryStringParameter Name="orgID" QueryStringField="orgID" Type="Int64" />
                <asp:QueryStringParameter Name="projectID" QueryStringField="projectID" Type="Int64" />
                <asp:QueryStringParameter Name="detailSheetID" QueryStringField="detailSheetID" Type="Int64" />
                <asp:Parameter DefaultValue="MOTELS" Name="recType" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource7" runat="server" SelectMethod="Get" TypeName="DBI.Data.BBDetail+SubGrid+Data">
            <SelectParameters>
                <asp:QueryStringParameter Name="orgID" QueryStringField="orgID" Type="Int64" />
                <asp:QueryStringParameter Name="projectID" QueryStringField="projectID" Type="Int64" />
                <asp:QueryStringParameter Name="detailSheetID" QueryStringField="detailSheetID" Type="Int64" />
                <asp:Parameter DefaultValue="MISC" Name="recType" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource8" runat="server" SelectMethod="Get" TypeName="DBI.Data.BBDetail+SubGrid+Data">
            <SelectParameters>
                <asp:QueryStringParameter Name="orgID" QueryStringField="orgID" Type="Int64" />
                <asp:QueryStringParameter Name="projectID" QueryStringField="projectID" Type="Int64" />
                <asp:QueryStringParameter Name="detailSheetID" QueryStringField="detailSheetID" Type="Int64" />
                <asp:Parameter DefaultValue="LUMPSUM" Name="recType" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </form>
</body>
</html>

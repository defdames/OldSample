﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MissingROW.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.Reports.MissingROW" %>

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
            <LocalReport ReportEmbeddedResource="DBI.Web.EMS.Views.Modules.CrossingMaintenance.Reports.MissingROW.rdlc" ReportPath="Views\Modules\CrossingMaintenance\Reports\MissingROW.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="MissingROWReport" />
                        </DataSources>
                    </LocalReport>
        </rsweb:ReportViewer>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetMissingROWList" TypeName="DBI.Data.CROSSING_MAINTENANCE">
            <SelectParameters>
                <asp:QueryStringParameter Name="selectedRailroad" QueryStringField="selectedRailroad" Type="String" />
                <asp:QueryStringParameter Name="selectedServiceUnit" QueryStringField="selectedServiceUnit" Type="String" />
                <asp:QueryStringParameter Name="selectedSubDiv" QueryStringField="selectedSubDiv" Type="String" />
                <asp:QueryStringParameter Name="selectedState" QueryStringField="selectedState" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </form>
</body>
</html>

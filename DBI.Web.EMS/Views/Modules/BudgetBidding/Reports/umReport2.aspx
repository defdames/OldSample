<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umReport2.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.BudgetBidding.Reports.umReport2" %>

<%@ Register assembly="Telerik.ReportViewer.WebForms, Version=8.1.14.804, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" namespace="Telerik.ReportViewer.WebForms" tagprefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <telerik:ReportViewer ID="ReportViewer1" runat="server">
<typereportsource typename="DBI.Web.EMS.Views.Modules.BudgetBidding.Reports.Report2, DBI.Web.EMS, Version=2.2014.718.199, Culture=neutral, PublicKeyToken=null"></typereportsource>
</telerik:ReportViewer>
    </form>
</body>
</html>

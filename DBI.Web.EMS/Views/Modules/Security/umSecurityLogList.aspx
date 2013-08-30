<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umSecurityLogList.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.Security.umSecurityLogList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager runat="server"  IsDynamic="False" RethrowAjaxExceptions="true">
    </ext:ResourceManager>
    <form id="test" runat="server">
        <ext:Viewport runat="server" ID="uxViewPort" Layout="BorderLayout" IDMode="Explicit" IsDynamic="False" Namespace="App" RenderXType="True">
            <Items>
                <ext:GridPanel ID="uxSecurityLogGridPanel"
                    runat="server"
                    Title="System Activitys"
                    Padding="5"
                    Icon="User"
                    Region="Center"
                    Frame="true"
                    Margins="5 5 5 5"
                    SelectionMemory="false">
                    <Store>
                        <ext:Store
                            ID="uxSecurityLogStore"
                            runat="server" 
                            RemoteSort="true" 
                            PageSize="25"
                            OnReadData="deLogsDatabind">
                        <Proxy>
                        <ext:PageProxy />
                        </Proxy>
                            <Model>
                                <ext:Model ID="uxSecurityLogModel" runat="server" IDProperty="ID">
                                    <Fields>
                                        <ext:ModelField Name="USER_NAME" Type="String" />
                                        <ext:ModelField Name="EMPLOYEE_NAME" Type="String" />
                                        <ext:ModelField Name="USER_CULTURE" Type="String" />
                                        <ext:ModelField Name="GUID" Type="String" />
                                        <ext:ModelField Name="MESSAGE" Type="String" />
                                        <ext:ModelField Name="INNER_EXCEPTION" Type="String" />
                                        <ext:ModelField Name="SOURCE" Type="String" />
                                        <ext:ModelField Name="STACKTRACE" Type="String" />
                                        <ext:ModelField Name="DEBUG" Type="String" />
                                        <ext:ModelField Name="CREATED_DATE" Type="Date" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                             <Sorters>
                        <ext:DataSorter Property="ID" Direction="DESC" />
                    </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="uxSecurityLogColumns" runat="server">
                        <Columns>
                            <ext:Column ID="cGUID" runat="server" DataIndex="GUID" Text="GUID" Flex="1" />
                            <ext:Column ID="cUsername" runat="server" DataIndex="USER_NAME" Text="Username" Flex="1" />
                            <ext:Column ID="cEmployeeName" runat="server" DataIndex="EMPLOYEE_NAME" Text="Full Name" Flex="1" />
                            <ext:Column ID="cUserCulture" runat="server" DataIndex="USER_CULTURE" Text="Culture" Flex="1" Hidden="true" />
                            <ext:Column ID="cMessage" runat="server" DataIndex="MESSAGE" Text="Message" Flex="1" />
                            <ext:Column ID="cInnerException" runat="server" DataIndex="INNER_EXCEPTION" Text="Inner Exception" Flex="1" Hidden="true" />
                            <ext:Column ID="cSource" runat="server" DataIndex="SOURCE" Text="Source" Flex="1" Hidden="true" />
                            <ext:Column ID="cStackTrace" runat="server" DataIndex="STACKTRACE" Text="Stack Trace" Flex="1" Hidden="true" />
                            <ext:Column ID="cDebug" runat="server" DataIndex="DEBUG" Text="Debug" Flex="1" />
                            <ext:DateColumn ID="cCreatedDate" runat="server" DataIndex="CREATED_DATE" Text="Created" Flex="1" Format="dd/MM/yyyy" />
                        </Columns>
                    </ColumnModel>
                    <Features>
                        <ext:GridFilters runat="server" ID="uxSecurityLogGridFilters" Local="true">
                            <Filters>
                                <ext:StringFilter DataIndex="GUID" />
                                 <ext:StringFilter DataIndex="USER_NAME" />
                                 <ext:StringFilter DataIndex="EMPLOYEE_NAME" />
                                <ext:StringFilter DataIndex="USER_CULTURE" />
                                <ext:StringFilter DataIndex="MESSAGE" />
                                <ext:StringFilter DataIndex="INNER_EXCEPTION" />
                                <ext:StringFilter DataIndex="SOURCE" />
                                <ext:StringFilter DataIndex="STACKTRACE" />
                                <ext:StringFilter DataIndex="DEBUG" />
                                <ext:DateFilter DataIndex="CREATED_DATE" />
                            </Filters>
                        </ext:GridFilters>
                    </Features>
                    <BottomBar>
                        <ext:PagingToolbar ID="uxSecurityLogPaging" runat="server" />
                    </BottomBar>
                     <SelectionModel>
                        <ext:CheckboxSelectionModel  ID="uxSecurityActivitySelectionModel" runat="server" Mode="Single" ShowHeaderCheckbox="false" AllowDeselect="true">
                        </ext:CheckboxSelectionModel>
                    </SelectionModel>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
</form>
</body>
</html>
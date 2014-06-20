<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEditTime.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.TimeClock.Edit.umEditTime" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport ID="ViewPort1" runat="server" Layout="BorderLayout">
            <Items>
            <%--    <ext:GridPanel ID="uxEditTimeGrid" runat="server" Frame="true" Region="Center">
                    <Store>
                        <ext:Store ID="uxEditTimeStore" runat="server">
                            <Model>
                                <ext:Model runat="server" IDProperty="ID">
                                    <Fields>
                                        <ext:ModelField Name="DATE"  Type="Date" Mapping="DATETIME"/>   
                                        <ext:ModelField Name="TIME"  Type="Date" Mapping="DATETIME" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel runat="server">
                        <Columns>
                            <ext:DateColumn runat="server" DataIndex="DATE">
                                <Editor>
                                    <ext:DateField runat="server" Format="M/d/yyyy"/>
                                </Editor>
                            </ext:DateColumn>
                            <ext:DateColumn runat="server" DataIndex="TIME" Format="h:mm tt">
                                <Editor>
                                    <ext:TimeField runat="server" Format="h:mm tt"/>
                                </Editor>
                            </ext:DateColumn>
                        </Columns>

                    </ColumnModel>--%>
                    <ext:DateField ID="uxDateTimeInField" runat="server" FieldLabel="TimeIn"></ext:DateField>
                
                    <ext:TimeField ID="uxDateTimeOutField" runat="server" FieldLabel="TimeOut"></ext:TimeField>
               <%-- </ext:GridPanel>--%>
            </Items>
        </ext:Viewport>

    </form>
</body>
</html>

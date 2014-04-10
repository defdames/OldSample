<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umManagerView.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.TimeClock.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    
        <ext:ResourceManager ID="ResourceManager1" runat="server"/>
       
    <ext:viewport ID="Viewport1" runat="server" Layout="Fit">
        
       <Items>
        <ext:GridPanel ID="uxEmployeeHoursGrid" runat="server">
            <SelectionModel>
                <ext:CheckboxSelectionModel ID="CheckBoxSelectionModel1" runat="server" AllowDeselect="true" Mode="Multi"/>               
            </SelectionModel>
            <Store>
                <ext:Store runat="server" ID="uxEmployeeHoursStore" AutoDataBind="true">
                    <Model>
                        <ext:Model ID="Model1" runat="server">
                            <Fields>
                                <ext:ModelField Name="EMPLOYEE_NAME"/>
                                <ext:ModelField Name="TIME_IN" type="Date" />
                                <ext:ModelField Name="TIME_OUT" type="Date"/>
                                <ext:ModelField Name="TOTAL_HOURS" />
                            </Fields>
                        </ext:Model>
                    </Model>
                </ext:Store>
            </Store>
            <ColumnModel>
                <Columns>
                    <ext:Column ID="Column1"
                        runat="server"
                        Text="Name"
                        DataIndex="EMPLOYEE_NAME"
                        Sortable="true" 
                        Locked="True"/>
                    <ext:Column ID="Column21" runat="server" Text="Sunday">
                        <Columns>
                            <ext:DateColumn ID="Column22"
                                runat="server"
                                Text="Time In"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_IN">
                            </ext:DateColumn>
                            <ext:DateColumn ID="Column23"
                                runat="server"
                                Text="Time Out"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_OUT">
                            </ext:DateColumn>
                             <ext:Column ID="Column24"
                                runat="server"
                                Text="Total Hours"
                                DataIndex="TOTAL_HOURS">
                            </ext:Column>
                        </Columns>
                    </ext:Column>
                    <ext:Column ID="Column2" runat="server" Text="Monday">
                        <Columns>
                            <ext:DateColumn ID="uxTimeInMonday"
                                runat="server"
                                Text="Time In"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_IN">
                            </ext:DateColumn>
                            <ext:DateColumn ID="uxTimeOutMonday"
                                runat="server"
                                Text="Time Out"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_OUT">
                            </ext:DateColumn>
                             <ext:Column ID="uxTotalHoursMonday"
                                runat="server"
                                Text="Total Hours"
                                DataIndex="TOTAL_HOURS">
                            </ext:Column>
                        </Columns>
                    </ext:Column>
                    <ext:Column ID="Column3" runat="server" Text="Tuesday">
                        <Columns>
                            <ext:DateColumn ID="Column4"
                                runat="server"
                                Text="Time In"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_IN">
                            </ext:DateColumn>
                            <ext:DateColumn ID="Column5"
                                runat="server"
                                Text="Time Out"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_OUT">
                            </ext:DateColumn>
                             <ext:Column ID="Column6"
                                runat="server"
                                Text="Total Hours"
                                DataIndex="TOTAL_HOURS">
                            </ext:Column>
                        </Columns>
                    </ext:Column>
                    <ext:Column ID="Column7" runat="server" Text="Wednesday">
                        <Columns>
                            <ext:DateColumn ID="Column8"
                                runat="server"
                                Text="Time In"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_IN">
                            </ext:DateColumn>
                            <ext:DateColumn ID="Column9"
                                runat="server"
                                Text="Time Out"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_OUT">
                            </ext:DateColumn>
                             <ext:Column ID="Column10"
                                runat="server"
                                Text="Total Hours"
                                DataIndex="TOTAL_HOURS">
                            </ext:Column>
                        </Columns>
                    </ext:Column>
                    <ext:Column ID="Column11" runat="server" Text="Thursday">
                        <Columns>
                            <ext:DateColumn ID="Column12"
                                runat="server"
                                Text="Time In"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_IN">
                            </ext:DateColumn>
                            <ext:DateColumn ID="Column13"
                                runat="server"
                                Text="Time Out"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_OUT">
                            </ext:DateColumn>
                             <ext:Column ID="Column14"
                                runat="server"
                                Text="Total Hours"
                                DataIndex="TOTAL_HOURS">
                            </ext:Column>
                        </Columns>
                    </ext:Column>
                    <ext:Column ID="Column15" runat="server" Text="Friday">
                        <Columns>
                            <ext:DateColumn ID="Column16"
                                runat="server"
                                Text="Time In"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_IN">
                            </ext:DateColumn>
                            <ext:DateColumn ID="Column17"
                                runat="server"
                                Text="Time Out"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_OUT">
                            </ext:DateColumn>
                             <ext:Column ID="Column18"
                                runat="server"
                                Text="Total Hours"
                                DataIndex="TOTAL_HOURS">
                            </ext:Column>
                        </Columns>
                    </ext:Column>
                    <ext:Column ID="Column19" runat="server" Text="Saturday">
                        <Columns>
                            <ext:DateColumn ID="Column20"
                                runat="server"
                                Text="Time In"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_IN">
                            </ext:DateColumn>
                            <ext:DateColumn ID="Column25"
                                runat="server"
                                Text="Time Out"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_OUT">
                            </ext:DateColumn>
                             <ext:Column ID="Column26"
                                runat="server"
                                Text="Total Hours"
                                DataIndex="TOTAL_HOURS">
                            </ext:Column>
                        </Columns>
                    </ext:Column>
                </Columns>
            </ColumnModel>
        </ext:GridPanel>
           </Items>
        </ext:viewport>
        
    
    
</body>
</html>

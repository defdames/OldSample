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
                                <ext:ModelField Name="TIME_IN" Type="Date"/>
                                <ext:ModelField Name="TIME_OUT" Type="Date"/>
                                <ext:ModelField Name="TOTAL_HOURS" />
                                <ext:ModelField Name="DAY_OF_WEEK" />
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
                    <ext:Column ID="colSunday" runat="server" Dataindex="DAY_OF_WEEK" Text="Sunday">
                        <Columns>
                            <ext:DateColumn ID="TiSunday"
                                runat="server"
                                Text="Time In"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_IN">
                            </ext:DateColumn>
                            <ext:DateColumn ID="ToSunday"
                                runat="server"
                                Text="Time Out"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_OUT">
                            </ext:DateColumn>
                             <ext:Column ID="TotSunday"
                                runat="server"
                                Text="Total Hours"
                                DataIndex="TOTAL_HOURS">
                            </ext:Column>
                        </Columns>
                    </ext:Column>
                    <ext:Column ID="colMonday" runat="server" text="Monday">
                        <Columns>
                            <ext:DateColumn ID="TiMonday"
                                runat="server"
                                Text="Time In"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_IN">
                            </ext:DateColumn>
                            <ext:DateColumn ID="ToMonday"
                                runat="server"
                                Text="Time Out"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_OUT">
                            </ext:DateColumn>
                             <ext:Column ID="TotMonday"
                                runat="server"
                                Text="Total Hours"
                                DataIndex="TOTAL_HOURS">
                            </ext:Column>
                        </Columns>
                    </ext:Column>
                    <ext:Column ID="colTuesday" runat="server" Text="Tuesday">
                        <Columns>
                            <ext:DateColumn ID="TiTuesday"
                                runat="server"
                                Text="Time In"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_IN">
                            </ext:DateColumn>
                            <ext:DateColumn ID="ToTuesday"
                                runat="server"
                                Text="Time Out"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_OUT">
                            </ext:DateColumn>
                             <ext:Column ID="TotTuesday"
                                runat="server"
                                Text="Total Hours"
                                DataIndex="TOTAL_HOURS">
                            </ext:Column>
                        </Columns>
                    </ext:Column>
                    <ext:Column ID="colWednesday" runat="server" Text="Wednesday">
                        <Columns>
                            <ext:DateColumn ID="TiWednesday"
                                runat="server"
                                Text="Time In"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_IN">
                            </ext:DateColumn>
                            <ext:DateColumn ID="ToWednesday"
                                runat="server"
                                Text="Time Out"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_OUT">
                            </ext:DateColumn>
                             <ext:Column ID="TotWednesday"
                                runat="server"
                                Text="Total Hours"
                                DataIndex="TOTAL_HOURS">
                            </ext:Column>
                        </Columns>
                    </ext:Column>
                    <ext:Column ID="colThursday" runat="server" Text="Thursday">
                        <Columns>
                            <ext:DateColumn ID="TiThursday"
                                runat="server"
                                Text="Time In"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_IN">
                            </ext:DateColumn>
                            <ext:DateColumn ID="ToThursday"
                                runat="server"
                                Text="Time Out"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_OUT">
                            </ext:DateColumn>
                             <ext:Column ID="TotThursday"
                                runat="server"
                                Text="Total Hours"
                                DataIndex="TOTAL_HOURS">
                            </ext:Column>
                        </Columns>
                    </ext:Column>
                    <ext:Column ID="colFriday" runat="server" Text="Friday">
                        <Columns>
                            <ext:DateColumn ID="TiFriday"
                                runat="server"
                                Text="Time In"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_IN">
                            </ext:DateColumn>
                            <ext:DateColumn ID="ToFriday"
                                runat="server"
                                Text="Time Out"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_OUT">
                            </ext:DateColumn>
                             <ext:Column ID="TotFriday"
                                runat="server"
                                Text="Total Hours"
                                DataIndex="TOTAL_HOURS">
                            </ext:Column>
                        </Columns>
                    </ext:Column>
                    <ext:Column ID="colSaturday" runat="server" Text="Saturday">
                        <Columns>
                            <ext:DateColumn ID="TiSaturday"
                                runat="server"
                                Text="Time In"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_IN">
                            </ext:DateColumn>
                            <ext:DateColumn ID="ToSaturday"
                                runat="server"
                                Text="Time Out"
                                Format="M/d/yyyy h:mm tt"
                                Dataindex="TIME_OUT">
                            </ext:DateColumn>
                             <ext:Column ID="TotSaturday"
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

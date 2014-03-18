<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCombinedTab_IRM.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umCombinedTab_IRM" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../../Resources/StyleSheets/main.css" rel="stylesheet" />
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
        <ext:Panel runat="server" ID="uxMainContainer" Layout="AutoLayout">
            <Items>
                <ext:GridPanel runat="server"
                    ID="uxHeaderGrid"
                    Title="Header"
                    TitleAlign="Center"
                    Padding="10">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxHeaderStore">
                            <Model>
                                <ext:Model ID="Model1" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="HEADER_ID" />
                                        <ext:ModelField Name="LONG_NAME" />
                                        <ext:ModelField Name="DA_DATE" />
                                        <ext:ModelField Name="SUBDIVISION" />
                                        <ext:ModelField Name="CONTRACTOR" />
                                        <ext:ModelField Name="EMPLOYEE_NAME" />
                                        <ext:ModelField Name="LICENSE" />
                                        <ext:ModelField Name="STATE" />
                                        <ext:ModelField Name="APPLICATION_TYPE" />
                                        <ext:ModelField Name="DENSITY" />
                                        <ext:ModelField Name="DA_HEADER_ID" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" DataIndex="HEADER_ID" Text="DRS Number" Flex="3" />
                            <ext:Column ID="Column1" runat="server" DataIndex="LONG_NAME" Text="Project Name" Flex="8" />
                            <ext:DateColumn ID="DateColumn1" runat="server" DataIndex="DA_DATE" Text="Date" Flex="3" />
                            <ext:Column ID="Column2" runat="server" DataIndex="SUBDIVISION" Text="Sub-Division" Flex="6" />
                            <ext:Column ID="Column3" runat="server" DataIndex="CONTRACTOR" Text="Contractor" Flex="6" />
                            <ext:Column ID="Column4" runat="server" DataIndex="EMPLOYEE_NAME" Text="Supervisor / Area Manager" Flex="6" />
                            <ext:Column ID="Column5" runat="server" DataIndex="LICENSE" Text="License" Flex="4" />
                            <ext:Column ID="Column6" runat="server" DataIndex="STATE" Text="State" Flex="2" />
                            <ext:Column ID="Column7" runat="server" DataIndex="APPLICATION_TYPE" Text="Application/Type of Work" Flex="6" />
                            <ext:Column ID="Column8" runat="server" DataIndex="DENSITY" Text="Density" Flex="3" />
                            <ext:Column runat="server" DataIndex="DA_HEADER_ID" Text="Oracle Header ID" Flex="3" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
                <ext:GridPanel runat="server"
                    ID="uxEquipmentGrid"
                    Title="Equipment and Employees"
                    TitleAlign="Center"
                    Padding="10">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxEquipmentStore">
                            <Model>
                                <ext:Model ID="Model2" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="EMPLOYEE_NAME" />
                                        <ext:ModelField Name="NAME" />
                                        <ext:ModelField Name="TIME_IN" Type="Date" />
                                        <ext:ModelField Name="TIME_OUT" Type="Date" />
                                        <ext:ModelField Name="TRAVEL_TIME" />
                                        <ext:ModelField Name="DRIVE_TIME" />
                                        <ext:ModelField Name="PER_DIEM" />
                                        <ext:ModelField Name="COMMENTS" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column9" runat="server" DataIndex="EMPLOYEE_NAME" Text="Employee Name" Flex="8" />
                            <ext:Column ID="Column10" runat="server" DataIndex="NAME" Text="Equipment Name" Flex="9" />
                            <ext:DateColumn ID="DateColumn2" runat="server" DataIndex="TIME_IN" Text="Time In" Flex="6" Format="M/d/yyyy h:mm tt" />
                            <ext:DateColumn ID="DateColumn3" runat="server" DataIndex="TIME_OUT" Text="Time Out" Flex="6" Format="M/d/yyyy h:mm tt" />
                            <ext:Column ID="Column11" runat="server" DataIndex="TRAVEL_TIME" Text="Travel Time" Flex="6" />
                            <ext:Column ID="Column12" runat="server" DataIndex="DRIVE_TIME" Text="Drive Time" Flex="6" />
                            <ext:Column ID="Column13" runat="server" DataIndex="PER_DIEM" Text="Per Diem" Flex="5" />
                            <ext:Column ID="Column14" runat="server" DataIndex="COMMENTS" Text="Comments" Flex="9" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
                <ext:GridPanel runat="server"
                    ID="uxProductionGrid"
                    Title="Production"
                    TitleAlign="Center"
                    Padding="10">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxProductionStore">
                            <Model>
                                <ext:Model ID="Model3" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="DESCRIPTION" />
                                        <ext:ModelField Name="WORK_AREA" />
                                        <ext:ModelField Name="QUANTITY" />
                                        <ext:ModelField Name="STATION" />
                                        <ext:ModelField Name="EXPENDITURE_TYPE" />
                                        <ext:ModelField Name="BILL_RATE" />
                                        <ext:ModelField Name="UNIT_OF_MEASURE" />
                                        <ext:ModelField Name="SURFACE_TYPE" />
                                        <ext:ModelField Name="COMMENTS" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" DataIndex="DESCRIPTION" Text="Task Name" Flex="6" />
                            <ext:Column runat="server" DataIndex="WORK_AREA" Text="Spray/Work Area" Flex="6" />
                            <ext:Column runat="server" DataIndex="QUANTITY" Text="Quantity" Flex="7" />
                            <ext:Column runat="server" DataIndex="STATION" Text="Station" Flex="7" />
                            <ext:Column runat="server" DataIndex="EXPENDITURE_TYPE" Text="Expenditure Type" Flex="4" />
                            <ext:Column runat="server" DataIndex="BILL_RATE" Text="Bill Rate" />
                            <ext:Column runat="server" DataIndex="UNIT_OF_MEASURE" Text="Unit of Measure" />
                            <ext:Column runat="server" DataIndex="SURFACE_TYPE" Text="Surface Type" />
                            <ext:Column runat="server" DataIndex="COMMENTS" Text="Comments" Flex="4" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
                <ext:GridPanel runat="server"
                    ID="uxWeatherGrid"
                    Title="Weather"
                    TitleAlign="Center"
                    Padding="10">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxWeatherStore">
                            <Model>
                                <ext:Model ID="Model4" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="WEATHER_DATE_TIME" Type="Date" />
                                        <ext:ModelField Name="WIND_DIRECTION" />
                                        <ext:ModelField Name="WIND_VELOCITY" />
                                        <ext:ModelField Name="TEMP" />
                                        <ext:ModelField Name="HUMIDITY" />
                                        <ext:ModelField Name="COMMENTS" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:DateColumn runat="server" DataIndex="WEATHER_DATE_TIME" Text="Date/Time" Format="M/d/yyyy h:mm tt" Flex="6" />
                            <ext:Column runat="server" DataIndex="WIND_DIRECTION" Text="Wind Direction" Flex="1" />
                            <ext:Column runat="server" DataIndex="WIND_VELOCITY" Text="Wind Velocity" Flex="2" />
                            <ext:Column runat="server" DataIndex="TEMP" Text="Temperature" Flex="2" />
                            <ext:Column runat="server" DataIndex="HUMIDITY" Text="Humidity" Flex="3" />
                            <ext:Column runat="server" DataIndex="COMMENTS" Text="Comments" Flex="8" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
                <ext:GridPanel runat="server"
                    ID="uxInventoryGrid"
                    Title="Inventory"
                    TitleAlign="Center"
                    Padding="10">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxInventoryStore">
                            <Model>
                                <ext:Model ID="Model6" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="SUB_INVENTORY_SECONDARY_NAME" />
                                        <ext:ModelField Name="DESCRIPTION" />
                                        <ext:ModelField Name="RATE" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" DataIndex="SUB_INVENTORY_SECONDARY_NAME" Text="Sub-Inv Name" Flex="7" />
                            <ext:Column runat="server" DataIndex="DESCRIPTION" Text="Item" Flex="2" />
                            <ext:Column runat="server" DataIndex="RATE" Text="Quantity" Flex="3" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
                <ext:GridPanel runat="server"
                    ID="uxFooterGrid"
                    Title="Footer"
                    TitleAlign="Center"
                    Padding="10">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxFooterStore">
                            <Model>
                                <ext:Model ID="Model7" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="COMMENTS" />
                                        <ext:ModelField Name="HOTEL_NAME" />
                                        <ext:ModelField Name="HOTEL_CITY" />
                                        <ext:ModelField Name="HOTEL_STATE" />
                                        <ext:ModelField Name="HOTEL_PHONE" />
                                        <ext:ModelField Name="FOREMAN_SIGNATURE" Type="Boolean" />
                                        <ext:ModelField Name="CONTRACT_REP" Type="Boolean" />
                                        <ext:ModelField Name="DOT_REP" Type="Boolean" />
                                        <ext:ModelField Name="DOT_REP_NAME" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" DataIndex="HOTEL_NAME" Text="Hotel Name" Flex="1" />
                            <ext:Column runat="server" DataIndex="HOTEL_CITY" Text="Hotel City" Flex="1" />
                            <ext:Column runat="server" DataIndex="HOTEL_STATE" Text="Hotel State" Flex="1" />
                            <ext:Column runat="server" DataIndex="HOTEL_PHONE" Text="Hotel Phone" Flex="1" />
                            <ext:Column ID="Column15" runat="server" DataIndex="COMMENTS" Text="Comments" Flex="1" />
                            <ext:BooleanColumn runat="server" DataIndex="FOREMAN_SIGNATURE" Text="Foreman Signature Submitted" Flex="1" />
                            <ext:BooleanColumn runat="server" DataIndex="CONTRACT_REP" Text="Contract Rep Signature Submitted" Flex="1" />
                            <ext:BooleanColumn runat="server" DataIndex="DOT_REP" Text="DOT Rep Submitted" Flex="1" />
                            <ext:Column runat="server" DataIndex="DOT_REP_NAME" Text="DOT Rep Name" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
            </Items>
        </ext:Panel>
    </form>
</body>
</html>

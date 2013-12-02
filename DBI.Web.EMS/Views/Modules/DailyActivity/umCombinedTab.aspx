<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCombinedTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umCombinedTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
        <ext:GridPanel runat="server"
            ID="uxHeaderGrid"
            Title="Details"
            Layout="HBoxLayout">
            <Store>
                <ext:Store runat="server"
                    ID="uxHeaderStore"
                    AutoDataBind="true">
                    <Model>
                        <ext:Model runat="server">
                            <Fields>
                                <ext:ModelField Name="PROJECT_ID" />
                                <ext:ModelField Name="LONG_NAME" />
                                <ext:ModelField Name="DA_DATE" />
                                <ext:ModelField Name="SUBDIVISION" />
                                <ext:ModelField Name="CONTRACTOR" />
                                <ext:ModelField Name="PERSON_ID" />
                                <ext:ModelField Name="EMPLOYEE_NAME" />
                                <ext:ModelField Name="LICENSE" />
                                <ext:ModelField Name="STATE" />
                                <ext:ModelField Name="APPLICATION_TYPE" />
                                <ext:ModelField Name="DENSITY" />
                            </Fields>
                        </ext:Model>
                    </Model>
                </ext:Store>
            </Store>
            <ColumnModel>
                <Columns>
                    <ext:Column runat="server" DataIndex="LONG_NAME" Text="Project Name" Flex="10"/>
                    <ext:DateColumn runat="server" DataIndex="DA_DATE" Text="Date" Flex="10" />
                    <ext:Column runat="server" DataIndex="SUBDIVISION" Text="Sub-Division" Flex="10" />
                    <ext:Column runat="server" DataIndex="CONTRACTOR" Text="Contractor" Flex="10" />
                    <ext:Column runat="server" DataIndex="EMPLOYEE_NAME" Text="Supervisor / Area Manager" Flex="10" />
                    <ext:Column runat="server" DataIndex="LICENSE" Text="License" Flex="10" />
                    <ext:Column runat="server" DataIndex="STATE" Text="State" Flex="10" />
                    <ext:Column runat="server" DataIndex="APPLICATION_TYPE" Text="Application/Type of Work" Flex="10" />
                    <ext:Column runat="server" DataIndex="DENSITY" Text="Density" Flex="10" />
                </Columns>
            </ColumnModel>
        </ext:GridPanel>
        <ext:GridPanel runat="server"
            ID="uxEquipmentGrid"
            Title="Equipment and Employees"
            Layout="HBoxLayout">
            <Store>
                <ext:Store runat="server"
                    ID="uxEquipmentStore"
                    AutoDataBind="true">
                    <Model>
                        <ext:Model runat="server">
                            <Fields>
                                <ext:ModelField Name="EMPLOYEE_NAME" />
                                <ext:ModelField Name="NAME" />
                                <ext:ModelField Name="TIME_IN" />
                                <ext:ModelField Name="TIME_OUT" />
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
                    <ext:Column runat="server" DataIndex="EMPLOYEE_NAME" Text="Employee Name" Flex="10" />
                    <ext:Column runat="server" DataIndex="NAME" Text="Equipment Name" Flex="10" />
                    <ext:Column runat="server" DataIndex="TIME_IN" Text="Time In" Flex="10" />
                    <ext:Column runat="server" DataIndex="TIME_OUT" Text="Time Out" Flex="10" />
                    <ext:Column runat="server" DataIndex="TRAVEL_TIME" Text="Travel Time" Flex="10" />
                    <ext:Column runat="server" DataIndex="DRIVE_TIME" Text="Drive Time" Flex="10" />
                    <ext:Column runat="server" DataIndex="PER_DIEM" Text="Per Diem" Flex="10" />
                    <ext:Column runat="server" DataIndex="COMMENTS" Text="Comments" Flex="10" />
                </Columns>
            </ColumnModel>
        </ext:GridPanel>
        <ext:GridPanel runat="server"
            ID="uxProductionGrid"
            Title="Production"
            Layout="HBoxLayout">
            <Store>
                <ext:Store runat="server"
                    ID="uxProductionStore"
                    AutoDataBind="true">
                    <Model>
                        <ext:Model runat="server">
                            <Fields>
                                <ext:ModelField Name="DESCRIPTION" />
                                <ext:ModelField Name="TIME_IN" />
                                <ext:ModelField Name="TIME_OUT" />
                                <ext:ModelField Name="WORK_AREA" />
                                <ext:ModelField Name="POLE_FROM" />
                                <ext:ModelField Name="POLE_TO" />
                                <ext:ModelField Name="ACRES_MILE" />
                                <ext:ModelField Name="GALLONS" />
                            </Fields>
                        </ext:Model>
                    </Model>
                </ext:Store>
            </Store>
            <ColumnModel>
                <Columns>
                    <ext:Column runat="server" DataIndex="DESCRIPTION" Text="Task Name" Flex="10" />
                    <ext:Column runat="server" DataIndex="TIME_IN" Text="Time In" Flex="10" />
                    <ext:Column runat="server" DataIndex="TIME_OUT" Text="Time Out" Flex="10" />
                    <ext:Column runat="server" DataIndex="WORK_AREA" Text="Spray/Work Area" Flex="10" />
                    <ext:Column runat="server" DataIndex="POLE_FROM" Text="Pole From" Flex="10" />
                    <ext:Column runat="server" DataIndex="POLE_TO" Text="Pole To" Flex="10" />
                    <ext:Column runat="server" DataIndex="ACRES_MILE" Text="Acres/Mile" Flex="10" />
                    <ext:Column runat="server" DataIndex="GALLONS" Text="Gallons" Flex="10" />
                </Columns>
            </ColumnModel>
        </ext:GridPanel>
        <ext:GridPanel runat="server"
            ID="uxWeatherGrid"
            Title="Weather"
            Layout="HBoxLayout">
            <Store>
                <ext:Store runat="server"
                    ID="uxWeatherStore"
                    AutoDataBind="true">
                    <Model>
                        <ext:Model runat="server">
                            <Fields>
                                <ext:ModelField Name="WEATHER_DATE_TIME" />
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
                    <ext:Column runat="server" DataIndex="WEATHER_DATE_TIME" Text="Date/Time" Flex="10" />
                    <ext:Column runat="server" DataIndex="WIND_DIRECTION" Text="Wind Direction" Flex="10" />
                    <ext:Column runat="server" DataIndex="WIND_VELOCITY" Text="Wind Velocity" Flex="10" />
                    <ext:Column runat="server" DataIndex="TEMP" Text="Temperature" Flex="10" />
                    <ext:Column runat="server" DataIndex="HUMIDITY" Text="Humidity" Flex="10" />
                    <ext:Column runat="server" DataIndex="COMMENTS" Text="Comments" Flex="10" />
                </Columns>
            </ColumnModel>
        </ext:GridPanel>
        <ext:GridPanel runat="server"
            ID="uxChemicalGrid"
            Title="Chemical Mix"
            Layout="HBoxLayout">
            <Store>
                <ext:Store runat="server"
                    ID="uxChemicalStore"
                    AutoDataBind="true">
                    <Model>
                        <ext:Model runat="server">
                            <Fields>
                                <ext:ModelField Name="CHEMICAL_MIX_NUMBER" />
                                <ext:ModelField Name="TARGET_AREA" />
                                <ext:ModelField Name="GALLON_ACRE" />
                                <ext:ModelField Name="GALLON_STARTING" />
                                <ext:ModelField Name="GALLON_MIXED" />
                                <ext:ModelField Name="GALLON_REMAINING" />
                                <ext:ModelField Name="STATE" />
                                <ext:ModelField Name="COUNTY" />
                                <ext:ModelField Name="TOTAL" />
                                <ext:ModelField Name="USED" />
                                <ext:ModelField Name="SPRAYED" />
                            </Fields>
                        </ext:Model>
                    </Model>
                </ext:Store>
            </Store>
            <ColumnModel>
                <Columns>
                    <ext:Column runat="server" DataIndex="CHEMICAL_MIX_NUMBER" Text="Mix Number" Flex="10" />
                    <ext:Column runat="server" DataIndex="TARGET_AREA" Text="Target Area" Flex="10" />
                    <ext:Column runat="server" DataIndex="GALLON_ACRE" Text="Gallons/Acre" Flex="10" />
                    <ext:Column runat="server" DataIndex="GALLON_STARTING" Text="Gallons Starting" Flex="10" />
                    <ext:Column runat="server" DataIndex="GALLON_MIXED" Text="Gallon Mixed" Flex="10" />
                    <ext:Column runat="server" DataIndex="TOTAL" Text="Total Gallons" Flex="10" />
                    <ext:Column runat="server" DataIndex="USED" Text="Gallons Used" Flex="10" />
                    <ext:Column runat="server" DataIndex="SPRAYED" Text="Acres Sprayed" Flex="10" />
                    <ext:Column runat="server" DataIndex="STATE" Text="State" Flex="10" />
                    <ext:Column runat="server" DataIndex="COUNTY" Text="County" Flex="10" />
                </Columns>
            </ColumnModel>
        </ext:GridPanel>
        <ext:GridPanel runat="server"
            ID="uxInventoryGrid"
            Title="Inventory"
            Layout="HBoxLayout">
            <Store>
                <ext:Store runat="server"
                    ID="uxInventoryStore"
                    AutoDataBind="true">
                    <Model>
                        <ext:Model runat="server">
                            <Fields>
                                <ext:ModelField Name="CHEMICAL_MIX_NUMBER" />
                                <ext:ModelField Name="SUB_INVENTORY_SECONDARY_NAME" />
                                <ext:ModelField Name="DESCRIPTION" />
                                <ext:ModelField Name="RATE" />
                                <ext:ModelField Name="UNIT_OF_MEASURE" />
                                <ext:ModelField Name="EPA_NUMBER" />
                            </Fields>
                        </ext:Model>
                    </Model>
                </ext:Store>
            </Store>
            <ColumnModel>
                <Columns>
                    <ext:Column runat="server" DataIndex="CHEMICAL_MIX_NUMBER" Text="Mix Number" Flex="10" />
                    <ext:Column runat="server" DataIndex="SUB_INVENTORY_SECONDARY_NAME" Text="Sub-Inv Name" Flex="10" />
                    <ext:Column runat="server" DataIndex="DESCRIPTION" Text="Item" Flex="10" />
                    <ext:Column runat="server" DataIndex="RATE" Text="Rate" Flex="10" />
                    <ext:Column runat="server" DataIndex="UNIT_OF_MEASURE" Text="Unit" Flex="10" />
                    <ext:Column runat="server" DataIndex="EPA_NUMBER" Text="EPA Number" Flex="10" /> 
                </Columns>
            </ColumnModel>
        </ext:GridPanel>
        <ext:GridPanel runat="server"
            ID="uxFooterGrid"
            Title="Footer" Layout="HBoxLayout">
            <Store>
                <ext:Store runat="server"
                    ID="uxFooterStore"
                    AutoDataBind="true">
                    <Model>
                        <ext:Model runat="server">
                            <Fields>

                            </Fields>
                        </ext:Model>
                    </Model>
                </ext:Store>
            </Store>
        </ext:GridPanel>
    </form>
</body>
</html>

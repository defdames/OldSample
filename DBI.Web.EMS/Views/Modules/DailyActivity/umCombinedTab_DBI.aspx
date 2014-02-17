<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umCombinedTab_DBI.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umCombinedTab_DBI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                    Title="Details"
                    TitleAlign="Center"
                    Border="false">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxHeaderStore">
                            <Model>
                                <ext:Model ID="Model1" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="LONG_NAME" />
                                        <ext:ModelField Name="DA_DATE" />
                                        <ext:ModelField Name="SUBDIVISION" />
                                        <ext:ModelField Name="CONTRACTOR" />
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
                            <ext:Column ID="Column1" runat="server" DataIndex="LONG_NAME" Text="Project Name" Flex="8" />
                            <ext:DateColumn ID="DateColumn1" runat="server" DataIndex="DA_DATE" Text="Date" Flex="3" />
                            <ext:Column ID="Column2" runat="server" DataIndex="SUBDIVISION" Text="Sub-Division" Flex="6" />
                            <ext:Column ID="Column3" runat="server" DataIndex="CONTRACTOR" Text="Contractor" Flex="6" />
                            <ext:Column ID="Column4" runat="server" DataIndex="EMPLOYEE_NAME" Text="Supervisor / Area Manager" Flex="6" />
                            <ext:Column ID="Column5" runat="server" DataIndex="LICENSE" Text="License" Flex="4" />
                            <ext:Column ID="Column6" runat="server" DataIndex="STATE" Text="State" Flex="2" />
                            <ext:Column ID="Column7" runat="server" DataIndex="APPLICATION_TYPE" Text="Application/Type of Work" Flex="6" />
                            <ext:Column ID="Column8" runat="server" DataIndex="DENSITY" Text="Density" Flex="3" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
                <ext:GridPanel runat="server"
                    ID="uxEquipmentGrid"
                    Title="Equipment and Employees"
                    TitleAlign="Center"
                    Border="false"
                    Collapsed="true"
                    Collapsible="true"
                    TitleCollapse="true">
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
                    Border="false"
                    Collapsed="true"
                    Collapsible="true"
                    TitleCollapse="true">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxProductionStore">
                            <Model>
                                <ext:Model ID="Model3" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="DESCRIPTION" />
                                        <ext:ModelField Name="WORK_AREA" />
                                        <ext:ModelField Name="POLE_FROM" />
                                        <ext:ModelField Name="POLE_TO" />
                                        <ext:ModelField Name="ACRES_MILE" />
                                        <ext:ModelField Name="QUANTITY" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column15" runat="server" DataIndex="DESCRIPTION" Text="Task Name" Flex="6" />
                            <ext:Column ID="Column16" runat="server" DataIndex="WORK_AREA" Text="Spray/Work Area" Flex="6" />
                            <ext:Column ID="Column17" runat="server" DataIndex="POLE_FROM" Text="Pole From" Flex="7" />
                            <ext:Column ID="Column18" runat="server" DataIndex="POLE_TO" Text="Pole To" Flex="7" />
                            <ext:Column ID="Column19" runat="server" DataIndex="ACRES_MILE" Text="Acres/Mile" Flex="4" />
                            <ext:Column ID="Column20" runat="server" DataIndex="QUANTITY" Text="Gallons" Flex="4" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
                <ext:GridPanel runat="server"
                    ID="uxWeatherGrid"
                    Title="Weather"
                    TitleAlign="Center"
                    Border="false"
                    Collapsed="true"
                    Collapsible="true"
                    TitleCollapse="true">
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
                            <ext:DateColumn ID="DateColumn6" runat="server" DataIndex="WEATHER_DATE_TIME" Text="Date/Time" Format="M/d/yyyy h:mm tt" Flex="6" />
                            <ext:Column ID="Column21" runat="server" DataIndex="WIND_DIRECTION" Text="Wind Direction" Flex="1" />
                            <ext:Column ID="Column22" runat="server" DataIndex="WIND_VELOCITY" Text="Wind Velocity" Flex="2" />
                            <ext:Column ID="Column23" runat="server" DataIndex="TEMP" Text="Temperature" Flex="2" />
                            <ext:Column ID="Column24" runat="server" DataIndex="HUMIDITY" Text="Humidity" Flex="3" />
                            <ext:Column ID="Column25" runat="server" DataIndex="COMMENTS" Text="Comments" Flex="8" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
                <ext:GridPanel runat="server"
                    ID="uxChemicalGrid"
                    Title="Chemical Mix"
                    TitleAlign="Center"
                    Border="false"
                    Collapsed="true"
                    Collapsible="true"
                    TitleCollapse="true">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxChemicalStore">
                            <Model>
                                <ext:Model ID="Model5" runat="server">
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
                                        <ext:ModelField Name="ACRES_SPRAYED" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column26" runat="server" DataIndex="CHEMICAL_MIX_NUMBER" Text="Mix Number" Flex="2" />
                            <ext:Column ID="Column27" runat="server" DataIndex="TARGET_AREA" Text="Target Area" Flex="7" />
                            <ext:Column ID="Column28" runat="server" DataIndex="GALLON_ACRE" Text="Gallons/Acre" Flex="5" />
                            <ext:Column ID="Column29" runat="server" DataIndex="GALLON_STARTING" Text="Gallons Starting" Flex="5" />
                            <ext:Column ID="Column30" runat="server" DataIndex="GALLON_MIXED" Text="Gallon Mixed" Flex="5" />
                            <ext:Column ID="Column31" runat="server" DataIndex="TOTAL" Text="Total Gallons" Flex="5" />
                            <ext:Column ID="Column32" runat="server" DataIndex="USED" Text="Gallons Used" Flex="5" />
                            <ext:Column ID="Column33" runat="server" DataIndex="SPRAYED" Text="Acres Sprayed" Flex="5" />
                            <ext:Column ID="Column34" runat="server" DataIndex="STATE" Text="State" Flex="2" />
                            <ext:Column ID="Column35" runat="server" DataIndex="COUNTY" Text="County" Flex="7" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
                <ext:GridPanel runat="server"
                    ID="uxInventoryGrid"
                    Title="Inventory"
                    TitleAlign="Center"
                    Border="false"
                    Collapsed="true"
                    Collapsible="true"
                    TitleCollapse="true">
                    <Store>
                        <ext:Store runat="server"
                            ID="uxInventoryStore">
                            <Model>
                                <ext:Model ID="Model6" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="CHEMICAL_MIX_NUMBER" />
                                        <ext:ModelField Name="SUB_INVENTORY_SECONDARY_NAME" />
                                        <ext:ModelField Name="DESCRIPTION" />
                                        <ext:ModelField Name="RATE" />
                                        <ext:ModelField Name="TOTAL" />
                                        <ext:ModelField Name="UNIT_OF_MEASURE" />
                                        <ext:ModelField Name="EPA_NUMBER" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column36" runat="server" DataIndex="CHEMICAL_MIX_NUMBER" Text="Mix Number" Flex="2" />
                            <ext:Column ID="Column37" runat="server" DataIndex="SUB_INVENTORY_SECONDARY_NAME" Text="Sub-Inv Name" Flex="7" />
                            <ext:Column ID="Column38" runat="server" DataIndex="DESCRIPTION" Text="Item" Flex="2" />
                            <ext:Column ID="Column39" runat="server" DataIndex="RATE" Text="Rate" Flex="3" />
                            <ext:Column runat="server" DataIndex="TOTAL" Text="Total" Flex="3" />
                            <ext:Column ID="Column40" runat="server" DataIndex="UNIT_OF_MEASURE" Text="Unit" Flex="3" />
                            <ext:Column ID="Column41" runat="server" DataIndex="EPA_NUMBER" Text="EPA Number" Flex="6" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
                <ext:GridPanel runat="server"
                    ID="uxFooterGrid"
                    Title="Footer"
                    TitleAlign="Center"
                    Border="false"
                    Collapsed="true"
                    Collapsible="true"
                    TitleCollapse="true">
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
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column ID="Column42" runat="server" DataIndex="COMMENTS" Text="Reason for no Work" Flex="1" />
                            <ext:Column ID="Column43" runat="server" DataIndex="HOTEL_NAME" Text="Hotel Name" Flex="1" />
                            <ext:Column ID="Column44" runat="server" DataIndex="HOTEL_CITY" Text="Hotel City" Flex="1" />
                            <ext:Column ID="Column45" runat="server" DataIndex="HOTEL_STATE" Text="Hotel State" Flex="1" />
                            <ext:Column ID="Column46" runat="server" DataIndex="HOTEL_PHONE" Text="Hotel Phone" Flex="1" />
                            <ext:BooleanColumn ID="BooleanColumn1" runat="server" DataIndex="FOREMAN_SIGNATURE" Text="Foreman Signature Submitted" Flex="1" />
                            <ext:BooleanColumn ID="BooleanColumn2" runat="server" DataIndex="CONTRACT_REP" Text="Contract Rep Signature Submitted" Flex="1" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
            </Items>
        </ext:Panel>
    </form>
</body>
</html>

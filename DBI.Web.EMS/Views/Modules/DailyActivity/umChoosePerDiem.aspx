﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umChoosePerDiem.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umChoosePerDiem" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel runat="server" ID="uxChoosePerDiemFormPanel" Layout="FormLayout" DefaultButton="uxChoosePerDiemSubmitButton" Region="Center">
                    <Items>
                        <ext:DropDownField runat="server" ID="uxChoosePerDiemHeaderId" Mode="ValueText" Editable="false" FieldLabel="Choose a DRS" AllowBlank="false">
                            <Component>
                                <ext:GridPanel runat="server" ID="uxLunchDRSGrid">
                                    <Store>
                                        <ext:Store runat="server" ID="uxChoosePerDiemHeaderIdStore" AutoDataBind="true">
                                            <Model>
                                                <ext:Model ID="Model1" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="ProjectName" Type="String" />
                                                        <ext:ModelField Name="HeaderId" Type="Int" />
                                                        <ext:ModelField Name="TaskNumber" Type="String" />
                                                        <ext:ModelField Name="TaskName" Type="String" />
                                                        <ext:ModelField Name="TaskId" Type="Int" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                            <Proxy>
                                                <ext:PageProxy />
                                            </Proxy>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column ID="Column1" runat="server" DataIndex="HeaderId" Text="DRS Number" Flex="10" />
                                            <ext:Column ID="Column2" runat="server" DataIndex="ProjectName" Text="Project" Flex="50" />
                                            <ext:Column ID="Column3" runat="server" DataIndex="TaskNumber" Text="Task Number" Flex="15" />
                                            <ext:Column ID="Column4" runat="server" DataIndex="TaskName" Text="Task Name" Flex="25" />
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" Mode="Single" />
                                    </SelectionModel>
                                    <DirectEvents>
                                        <SelectionChange OnEvent="deStoreValues">
                                            <ExtraParams>
                                                <ext:Parameter Name="selectedInfo" Value="Ext.encode(#{uxLunchDRSGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                                            </ExtraParams>
                                        </SelectionChange>
                                    </DirectEvents>
                                </ext:GridPanel>
                            </Component>
                        </ext:DropDownField>
                        <ext:Hidden runat="server" ID="uxChoosePerDiemTask" />
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="uxChoosePerDiemSubmitButton" Text="Save" Disabled="true" Icon="Add">
                            <DirectEvents>
                                <Click OnEvent="deUpdatePerDiem">
                                    <EventMask ShowMask="true" />
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="uxChoosePerDiemCancelButton" runat="server" Text="Cancel" Icon="Delete">
                            <Listeners>
                                <Click Handler="#{uxChoosePerDiemFormPanel}.reset();
							parentAutoLoadControl.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <Listeners>
                        <ValidityChange Handler="#{uxChoosePerDiemSubmitButton}.setDisabled(!valid)" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

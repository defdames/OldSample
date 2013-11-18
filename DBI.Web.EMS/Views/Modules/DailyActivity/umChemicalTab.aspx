<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umChemicalTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umChemicalTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
    <form id="form1" runat="server">
        <ext:GridPanel runat="server"
            ID="uxCurrentChemicalGrid"
            Layout="HBoxLayout">
            <Store>
                <ext:Store runat="server"
                    ID="uxCurrentChemicalStore"
                    AutoDataBind="true">
                    <Model>
                        <ext:Model runat="server">
                            <Fields>
                                <ext:ModelField Name="CHEMICAL_MIX_ID" />
                                <ext:ModelField Name="CHEMICAL_MIX_NUMBER" />
                                <ext:ModelField Name="HEADER_ID" />
                                <ext:ModelField Name="TARGET_ARE" />
                                <ext:ModelField Name="GALLON_ACRE" />
                                <ext:ModelField Name="GALLON_STARTING" />
                                <ext:ModelField Name="GALLON_MIXED" />
                                <ext:ModelField Name="GALLON_TOTAL" />
                                <ext:ModelField Name="GALLON_REMAINING" />
                                <ext:ModelField Name="GALLON_USED" />
                                <ext:ModelField Name="ACRES_SPRAYED" />
                                <ext:ModelField Name="STATE" />
                                <ext:ModelField Name="COUNTY" />
                            </Fields>
                        </ext:Model>
                    </Model>
                </ext:Store>
            </Store>
            <ColumnModel>
                <Columns>
                    <ext:Column runat="server" DataIndex="CHEMICAL_MIX_NUMBER" />
                    <ext:Column runat="server" DataIndex="TARGET_ARE" />
                    <ext:Column runat="server" DataIndex="GALLON_ACRE" />
                    <ext:Column runat="server" DataIndex="GALLON_STARTING" />
                    <ext:Column runat="server" DataIndex="GALLON_MIXED" />
                    <ext:Column runat="server" DataIndex="GALLON_TOTAL" />
                    <ext:Column runat="server" DataIndex="GALLON_REMAINING" />
                    <ext:Column runat="server" DataIndex="GALLON_USED" />
                    <ext:Column runat="server" DataIndex="ACRES_SPRAYED" />
                    <ext:Column runat="server" DataIndex="STATE" />
                    <ext:Column runat="server" DataIndex="COUNTY" />
                </Columns>
            </ColumnModel>
            <Buttons>
                <ext:Button runat="server"
                    ID="uxAddChemicalButton"
                    Icon="ApplicationAdd"
                    Text="Add Chemical Mix">
                    <Listeners>
                        <Click Handler="#{uxAddChemicalWindow}.show()" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="uxEditChemicalButton"
                    Icon="ApplicationEdit"
                    Text="Edit Chemical Mix">
                    <DirectEvents>
                        <Click OnEvent="deEditChemicalForm">
                            <ExtraParams>
                                <ext:Parameter Name="ChemicalInfo" Value="Ext.encode(#{uxCurrentChemicalGrid}.getRowsValues({selectedOnly : true}))" Mode="Raw" />
                            </ExtraParams>
                        </Click>
                    </DirectEvents>
                    <Listeners>
                        <Click Handler="#{uxEditChemicalWindow}.show()" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="uxRemoveChemicalButton"
                    Icon="ApplicationDelete"
                    Text="Remove Chemical Mix">
                    <DirectEvents>
                        <Click OnEvent="deRemoveChemical">
                            <Confirmation ConfirmRequest="true" Title="Remove?" Message="Do you really want to remove?" />
                            <ExtraParams>
                                <ext:Parameter Name="ChemicalId" Value="#{uxCurrentChemical}.getSelectionModel().getSelection()[0].data.CHEMICAL_MIX_ID" Mode="Raw" />
                            </ExtraParams>
                        </Click>
                    </DirectEvents>
                </ext:Button>
            </Buttons>
        </ext:GridPanel>
        <ext:Window runat="server"
            ID="uxAddChemicalWindow"
            Layout="FormLayout">
            <Items>
                <ext:FormPanel runat="server"
                    ID="uxAddChemicalForm"
                    Layout="FormLayout">
                    <Items>
                        <ext:TextField runat="server"
                            ID="uxAddChemicalTargetAre" />
                        <ext:NumberField runat="server"
                            ID="uxAddChemicalGallonAcre" />
                        <ext:NumberField runat="server"
                            ID="uxAddChemicalGallonStart" />
                        <ext:NumberField runat="server"
                            ID="uxAddChemicalGallonMixed" />
                        <ext:NumberField runat="server"
                            ID="uxAddChemicalGallonTotal" />
                        <ext:NumberField runat="server"
                            ID="uxAddChemicalGallonRemain" />
                        <ext:NumberField runat="server"
                            ID="uxAddChemicalGallonUsed" />
                        <ext:NumberField runat="server"
                            ID="uxAddChemicalAcresSprayed" />
                        <ext:TextField runat="server"
                            ID="uxAddChemicalState" />
                        <ext:TextField runat="server"
                            ID="uxAddChemicalCounty" />
                    </Items>
                    <Buttons>
                        <ext:Button runat="server"
                            ID="uxAddChemicalSubmit"
                            Icon="ApplicationGo"
                            Text="Submit">
                            <DirectEvents>
                                <Click OnEvent="deAddChemical" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server"
                            ID="uxAddChemicalCancel"
                            Icon="ApplicationStop"
                            Text="Cancel">
                            <Listeners>
                                <Click Handler="#{uxAddChemicalWindow}.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
        <ext:Window runat="server"
            ID="uxEditChemicalWindow"
            Layout="FormLayout">
            <Items>
                <ext:FormPanel runat="server"
                    ID="uxEditChemicalForm"
                    Layout="FormLayout">
                    <Items>
                        <ext:TextField runat="server"
                            ID="uxEditChemicalTargetAre" />
                        <ext:NumberField runat="server"
                            ID="uxEditChemicalGallonAcre" />
                        <ext:NumberField runat="server"
                            ID="uxEditChemicalGallonStart" />
                        <ext:NumberField runat="server"
                            ID="uxEditChemicalGallonMixed" />
                        <ext:NumberField runat="server"
                            ID="uxEditChemicalGallonTotal" />
                        <ext:NumberField runat="server"
                            ID="uxEditChemicalGallonRemain" />
                        <ext:NumberField runat="server"
                            ID="uxEditChemicalGallonUsed" />
                        <ext:NumberField runat="server"
                            ID="uxEditChemicalAcresSprayed" />
                        <ext:TextField runat="server"
                            ID="uxEditChemicalState" />
                        <ext:TextField runat="server"
                            ID="uxEditChemicalCounty" />
                    </Items>
                    <Buttons>
                        <ext:Button runat="server"
                            ID="uxEditChemicalSubmit"
                            Icon="ApplicationGo"
                            Text="Submit">
                            <DirectEvents>
                                <Click OnEvent="uxEditChemical">
                                    <ExtraParams>
                                        <ext:Parameter Name="ChemicalID" Value="#{uxCurrentChemicalGrid}.getSelectionModel().getSelection()[0].data.CHEMICAL_MIX_ID" Mode="Raw" />
                                    </ExtraParams>                                    
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server"
                            ID="uxEditChemicalCancel"
                            Icon="ApplicationStop"
                            Text="Cancel">
                            <Listeners>
                                <Click Handler="#{uxEditChemicalWindow}.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>

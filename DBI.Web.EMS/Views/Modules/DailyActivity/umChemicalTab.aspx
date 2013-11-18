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
                    <ext:Column runat="server" DataIndex="CHEMICAL_MIX_NUMBER" Text="Mix Number" />
                    <ext:Column runat="server" DataIndex="TARGET_ARE" Text="Target" />
                    <ext:Column runat="server" DataIndex="GALLON_ACRE" Text="Gallon Acre" />
                    <ext:Column runat="server" DataIndex="GALLON_STARTING" Text="Gallon Starting"/>
                    <ext:Column runat="server" DataIndex="GALLON_MIXED" Text="Gallon Mixed" />
                    <ext:Column runat="server" DataIndex="GALLON_TOTAL" Text="Gallon Total" />
                    <ext:Column runat="server" DataIndex="GALLON_REMAINING" Text="Gallon Remaining" />
                    <ext:Column runat="server" DataIndex="GALLON_USED" Text="Gallon Used" />
                    <ext:Column runat="server" DataIndex="ACRES_SPRAYED" Text="Acres Sprayed" />
                    <ext:Column runat="server" DataIndex="STATE" Text="State" />
                    <ext:Column runat="server" DataIndex="COUNTY" Text="County" />
                </Columns>
            </ColumnModel>
            <TopBar>
                <ext:Toolbar runat="server">
                    <Items>
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
                                        <ext:Parameter Name="ChemicalId" Value="#{uxCurrentChemicalGrid}.getSelectionModel().getSelection()[0].data.CHEMICAL_MIX_ID" Mode="Raw" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
        </ext:GridPanel>
        <ext:Window runat="server"
            ID="uxAddChemicalWindow"
            Layout="FormLayout"
            Hidden="true"
            Width="650">
            <Items>
                <ext:FormPanel runat="server"
                    ID="uxAddChemicalForm"
                    Layout="FormLayout">
                    <Items>
                        <ext:TextField runat="server"
                            ID="uxAddChemicalTargetAre"
                            FieldLabel="Target" />
                        <ext:TextField runat="server"
                            ID="uxAddChemicalGallonAcre"
                            FieldLabel="Gallon Acre" />
                        <ext:TextField runat="server"
                            ID="uxAddChemicalGallonStart"
                            FieldLabel="Gallon Start" />
                        <ext:TextField runat="server"
                            ID="uxAddChemicalGallonMixed"
                            FieldLabel="Gallon Mixed" />
                        <ext:TextField runat="server"
                            ID="uxAddChemicalGallonTotal" 
                            FieldLabel="Gallon Total" />
                        <ext:TextField runat="server"
                            ID="uxAddChemicalGallonRemain"
                            FieldLabel="Gallon Remaining" />
                        <ext:TextField runat="server"
                            ID="uxAddChemicalGallonUsed"
                            FieldLabel="Gallons Used" />
                        <ext:TextField runat="server"
                            ID="uxAddChemicalAcresSprayed"
                            FieldLabel="Acres Sprayed" />
                        <ext:TextField runat="server"
                            ID="uxAddChemicalState"
                            FieldLabel="State" />
                        <ext:TextField runat="server"
                            ID="uxAddChemicalCounty"
                            FieldLabel="County" />
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
            Layout="FormLayout"
            Hidden="true"
            Width="650">
            <Items>
                <ext:FormPanel runat="server"
                    ID="uxEditChemicalForm"
                    Layout="FormLayout">
                    <Items>
                        <ext:TextField runat="server"
                            ID="uxEditChemicalTargetAre"
                            FieldLabel="Target" />
                        <ext:TextField runat="server"
                            ID="uxEditChemicalGallonAcre"
                            FieldLabel="Gallon Acre" />
                        <ext:TextField runat="server"
                            ID="uxEditChemicalGallonStart"
                            FieldLabel="Gallon Start" />
                        <ext:TextField runat="server"
                            ID="uxEditChemicalGallonMixed"
                            FieldLabel="Gallon Mixed" />
                        <ext:TextField runat="server"
                            ID="uxEditChemicalGallonTotal"
                            FieldLabel="Gallon Total" />
                        <ext:TextField runat="server"
                            ID="uxEditChemicalGallonRemain"
                            FieldLabel="Gallons Remaining" />
                        <ext:TextField runat="server"
                            ID="uxEditChemicalGallonUsed" 
                            FieldLabel="Gallons Used" />
                        <ext:TextField runat="server"
                            ID="uxEditChemicalAcresSprayed"
                            FieldLabel="Acres Sprayed" />
                        <ext:TextField runat="server"
                            ID="uxEditChemicalState"
                            FieldLabel="State" />
                        <ext:TextField runat="server"
                            ID="uxEditChemicalCounty"
                            FieldLabel="County" />
                    </Items>
                    <Buttons>
                        <ext:Button runat="server"
                            ID="uxEditChemicalSubmit"
                            Icon="ApplicationGo"
                            Text="Submit">
                            <DirectEvents>
                                <Click OnEvent="deEditChemical">
                                    <ExtraParams>
                                        <ext:Parameter Name="ChemicalId" Value="#{uxCurrentChemicalGrid}.getSelectionModel().getSelection()[0].data.CHEMICAL_MIX_ID" Mode="Raw" />
                                    </ExtraParams>
                                 </Click>                                    
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

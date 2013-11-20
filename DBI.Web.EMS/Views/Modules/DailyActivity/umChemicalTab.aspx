﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umChemicalTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umChemicalTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var updateAddTotalAndUsed = function () {
            App.uxAddChemicalGallonTotal.setValue(parseInt(App.uxAddChemicalGallonStart.value) + parseInt(App.uxAddChemicalGallonMixed.value));
            App.uxAddChemicalGallonUsed.setValue(parseInt(App.uxAddChemicalGallonTotal.value) - parseInt(App.uxAddChemicalGallonRemain.value));
            App.uxAddChemicalAcresSprayed.setValue(parseInt(App.uxAddChemicalGallonUsed.value) / parseInt(App.uxAddChemicalGallonAcre.value));
        };

        var updateEditTotalAndUsed = function () {
            App.uxEditChemicalGallonTotal.setValue(parseInt(App.uxEditChemicalGallonStart.value) + parseInt(App.uxEditChemicalGallonMixed.value));
            App.uxEditChemicalGallonUsed.setValue(parseInt(App.uxEditChemicalGallonTotal.value) - parseInt(App.uxEditChemicalGallonRemain.value));
            App.uxEditChemicalAcresSprayed.setValue(parseInt(App.uxEditChemicalGallonUsed.value) / parseInt(App.uxEditChemicalGallonAcre.value));
        };

        var doMath = function () {
            var count = App.uxCurrentChemicalGrid.getStore().getCount();
            for (var i = 0; i < count, i++){
                var total = App.uxCurrentChemicalStore.getSelectionModel().getSelection()[i].data.GALLON_STARTING + App.uxCurrentChemicalStore.getSelectionModel().getSelection()[i].data.GALLON_MIXED;
                App.uxCurrentChemicalStore.getAt(i).set("GALLON_TOTAL", total);
                var used = total - App.uxCurrentChemicalStore.getSelectionModel().getSelection()[i].data.GALLON_REMAINING;
                App.uxCurrentChemicalStore.getAt(i).set("GALLON_USED", used);
                var sprayed = used / App.uxCurrentChemicalStore.getSelectionModel().getSelection()[i].data.GALLON_ACRE;
                App.uxCurrentChemicalStore.getAt(i).set("ACRES_SPRAYED", sprayed);

            }
        };
    </script>
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
                    <Listeners>
                        <Load Fn="doMath" />
                    </Listeners>
                </ext:Store>
            </Store>
            <ColumnModel>
                <Columns>
                    <ext:Column runat="server" DataIndex="CHEMICAL_MIX_NUMBER" Text="Mix Number" />
                    <ext:Column runat="server" DataIndex="TARGET_ARE" Text="Target" />
                    <ext:Column runat="server" DataIndex="GALLON_ACRE" Text="Gallon Acre" />
                    <ext:Column runat="server" DataIndex="GALLON_STARTING" Text="Gallon Starting"/>
                    <ext:Column runat="server" DataIndex="GALLON_MIXED" Text="Gallon Mixed" />
                    <ext:Column runat="server" ID="uxGallonTotalGrid" Text="Gallon Total" />
                    <ext:Column runat="server" DataIndex="GALLON_REMAINING" Text="Gallon Remaining" />
                    <ext:Column runat="server" ID="uxGallonUsedGrid" Text="Gallon Used" />
                    <ext:Column runat="server" ID="uxAcresSprayedGrid" Text="Acres Sprayed" />
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
                            FieldLabel="Gallons / Acre" />
                        <ext:TextField runat="server"
                            ID="uxAddChemicalGallonStart"
                            FieldLabel="Gallons Start">
                            <Listeners>
                                <Change Fn="updateAddTotalAndUsed" />
                            </Listeners>
                        </ext:TextField>
                        <ext:TextField runat="server"
                            ID="uxAddChemicalGallonMixed"
                            FieldLabel="Gallons Mixed">
                            <Listeners>
                                <Change Fn="updateAddTotalAndUsed" />
                            </Listeners>
                        </ext:TextField>
                        <ext:TextField runat="server"
                            ID="uxAddChemicalGallonTotal" 
                            FieldLabel="Gallons Total" Disabled="true" />
                        <ext:TextField runat="server"
                            ID="uxAddChemicalGallonRemain"
                            FieldLabel="Gallons Remaining">
                            <Listeners>
                                <Change Fn="updateAddTotalAndUsed" />
                            </Listeners>
                        </ext:TextField>
                        <ext:TextField runat="server"
                            ID="uxAddChemicalGallonUsed"
                            FieldLabel="Gallons Used" Disabled="true"/>
                        <ext:TextField runat="server"
                            ID="uxAddChemicalAcresSprayed"
                            FieldLabel="Acres Sprayed" Disabled="true" />
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
                            FieldLabel="Gallons / Acre" />
                        <ext:TextField runat="server"
                            ID="uxEditChemicalGallonStart"
                            FieldLabel="Gallons Start">
                            <Listeners>
                                <Change Fn="updateEditTotalAndUsed" />
                            </Listeners>
                        </ext:TextField>
                        <ext:TextField runat="server"
                            ID="uxEditChemicalGallonMixed"
                            FieldLabel="Gallons Mixed">
                            <Listeners>
                                <Change Fn="updateEditTotalAndUsed" />
                            </Listeners>
                        </ext:TextField>
                        <ext:TextField runat="server"
                            ID="uxEditChemicalGallonTotal"
                            FieldLabel="Gallons Total" Disabled="true" />
                        <ext:TextField runat="server"
                            ID="uxEditChemicalGallonRemain"
                            FieldLabel="Gallons Remaining">
                            <Listeners>
                                <Change Fn="updateEditTotalAndUsed" />
                            </Listeners>
                        </ext:TextField>
                        <ext:TextField runat="server"
                            ID="uxEditChemicalGallonUsed" 
                            FieldLabel="Gallons Used" Disabled="true" />
                        <ext:TextField runat="server"
                            ID="uxEditChemicalAcresSprayed"
                            FieldLabel="Acres Sprayed" Disabled="true" />
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

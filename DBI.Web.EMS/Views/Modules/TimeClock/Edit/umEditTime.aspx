﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEditTime.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.TimeClock.Edit.umEditTime" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     
    <script type="text/javascript">
        var onKeyUp = function () {
                var me = this,
                    v = me.getValue(),
                    field;

                if (me.startDateField) {
                    field = Ext.getCmp(me.startDateField);
                    field.setMaxValue(v);
                    me.dateRangeMax = v;
                } else if (me.endDateField) {
                    field = Ext.getCmp(me.endDateField);
                    field.setMinValue(v);
                    me.dateRangeMin = v;
                }

                field.validate();
            };    
    </script>


</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Panel runat="server">
            <Items> 
                <ext:FormPanel ID="frmPanelIn" runat="server" ButtonAlign="Left" Title="Edit Time In" BodyPadding="5" Region="North" Layout="FormLayout">
                    <Items>
                        <ext:DateField ID="uxDateInField" runat="server" FieldLabel="Date In" EnableKeyEvents="true" Vtype="daterange" IsRemoteValidation="true">
                            <RemoteValidation OnValidation="ValidateDateTime">
						    </RemoteValidation>
                            <CustomConfig>
                                <ext:ConfigItem Name="endDateField" Value="uxDateOutField" Mode="Value" />
                            </CustomConfig>
                            <Listeners>
                                <KeyUp Fn="onKeyUp" />
                            </Listeners>
                        </ext:DateField>
                
                        <ext:TimeField ID="uxTimeInField" runat="server" FieldLabel="Time In" IsRemoteValidation="true" EnableKeyEvents="true" >
                            <RemoteValidation OnValidation="ValidateDateTime">
						    </RemoteValidation>
                        </ext:TimeField>
                    </Items>
                </ext:FormPanel>
                 <ext:FormPanel ID="frmPanelOut" runat="server" Title="Edit Time Out" BodyPadding="5" Region="North" Layout="FormLayout">
                    <Items>
                        <ext:DateField ID="uxDateOutField" runat="server" Vtype="daterange" FieldLabel="Date Out" EnableKeyEvents="true" IsRemoteValidation="true">
                            <RemoteValidation OnValidation="ValidateDateTime">
						    </RemoteValidation>
                            <CustomConfig>
                                <ext:ConfigItem Name="startDateField" Value="uxDateInField" Mode="Value" />
                            </CustomConfig>
                            <Listeners>
                                <KeyUp Fn="onKeyUp" />
                            </Listeners>
                        </ext:DateField>
                
                        <ext:TimeField ID="uxTimeOutField" runat="server" FieldLabel="Time Out" IsRemoteValidation="true" EnableKeyEvents="true">
                            <RemoteValidation OnValidation="ValidateDateTime">
						    </RemoteValidation>
                        </ext:TimeField>
                    </Items>
                        <Buttons>
						    <ext:Button runat="server" ID="uxEditButton" Text="Save">
								<DirectEvents>
									<Click OnEvent="deEditTime" Success="parent.Ext.getCmp('uxAddEditTime').close();"></Click>
								</DirectEvents>
							</ext:Button>
						</Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Panel>
          


    </form>
</body>
</html>

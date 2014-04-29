﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umChoosePerDiem.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umChoosePerDiem" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
		<ext:Panel runat="server" Layout="FitLayout">
			<Items>
				<ext:FormPanel runat="server" ID="uxChoosePerDiemFormPanel" Layout="FormLayout">
					<Items>
						<ext:ComboBox runat="server" ID="uxChoosePerDiemHeaderId" DisplayField="ProjectTask" ValueField="HeaderId" FieldLabel="Project" EmptyText="Choose Project for Per Diem" ForceSelection="true" LabelWidth="100" Width="500">
							<Store>
								<ext:Store runat="server" ID="uxChoosePerDiemHeaderIdStore">
									<Model>
										<ext:Model runat="server">
											<Fields>
												<ext:ModelField Name="HeaderId" />
												<ext:ModelField Name="ProjectTask" />
											</Fields>
										</ext:Model>
									</Model>
								</ext:Store>
							</Store>
						</ext:ComboBox>
					</Items>
					<Buttons>
						<ext:Button runat="server" Id="uxChoosePerDiemSubmitButton" Text="Submit">
							<DirectEvents>
								<Click OnEvent="deUpdatePerDiem">
									<EventMask ShowMask="true" />
								</Click>
							</DirectEvents>
						</ext:Button>
					</Buttons>
				</ext:FormPanel>
			</Items>
			<Listeners>
				<AfterRender
					Handler="var win = parentAutoLoadControl.target || parentAutoLoadControl, //you can use just 'parentAutoLoadControl' after update to Ext.NET v2 beta.
									size = this.getSize();
 
								size.height += 250;
								size.width += 12;
								win.setSize(size);"
					Delay="100" />
			</Listeners>
		</ext:Panel>
	</form>
</body>
</html>

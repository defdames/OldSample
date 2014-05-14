<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umEquipmentTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umEquipmentTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>

	<link href="../../../Resources/StyleSheets/main.css" rel="stylesheet" />
</head>
<body>
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
	<form id="form1" runat="server">
		<ext:GridPanel runat="server"
			ID="uxCurrentEquipment" Layout="HBoxLayout">
			<TopBar>
				<ext:Toolbar runat="server"
					ID="uxCurrentEquipmentTop">
					<Items>
						<ext:Button runat="server"
							ID="uxAddEquipmentButton"
							Icon="ApplicationAdd"
							Text="Add Equipment">
							<DirectEvents>
								<Click OnEvent="deLoadEquipmentWindow">
									<ExtraParams>
										<ext:Parameter Name="type" Value="Add" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" />
						<ext:Button runat="server"
							ID="uxEditEquipmentButton"
							Icon="ApplicationEdit"
							Text="Edit Equipment"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deLoadEquipmentWindow">
									<ExtraParams>
										<ext:Parameter Name="type" Value="Edit" />
										<ext:Parameter Name="EquipmentId" Value="#{uxCurrentEquipment}.getSelectionModel().getSelection()[0].data.EQUIPMENT_ID" Mode="Raw" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
						<ext:Button runat="server"
							ID="uxRemoveEquipmentButton"
							Icon="ApplicationDelete"
							Text="Remove Equipment"
							Disabled="true">
							<DirectEvents>
								<Click OnEvent="deRemoveEquipment">
									<Confirmation ConfirmRequest="true" Title="Remove?" Message="Do you really want to remove?" />
									<ExtraParams>
										<ext:Parameter Name="EquipmentId" Value="#{uxCurrentEquipment}.getSelectionModel().getSelection()[0].data.EQUIPMENT_ID" Mode="Raw" />
									</ExtraParams>
								</Click>                                
							</DirectEvents>
						</ext:Button>
					</Items>
				</ext:Toolbar>
			</TopBar>
			<Store>
				<ext:Store runat="server"
					ID="uxCurrentEquipmentStore"
					AutoDataBind="true">
					<Model>
						<ext:Model runat="server">
							<Fields>
								<ext:ModelField Name="EQUIPMENT_ID" />
								<ext:ModelField Name="CLASS_CODE" />
								<ext:ModelField Name="ORGANIZATION_NAME" />
								<ext:ModelField Name="ODOMETER_START" />
								<ext:ModelField Name="ODOMETER_END" />
								<ext:ModelField Name="PROJECT_ID" />
								<ext:ModelField Name="NAME" />
								<ext:ModelField Name="HEADER_ID" />
								<ext:ModelField Name="SEGMENT1" />
							</Fields>
						</ext:Model>
					</Model>                    
				</ext:Store>
			</Store>
			<ColumnModel>
				<Columns>
					<ext:Column runat="server"
						DataIndex="SEGMENT1"
						Text="Project Number" />
					<ext:Column runat="server"
						DataIndex="NAME"
						Text="Name" />
					<ext:Column runat="server"
						DataIndex="CLASS_CODE" 
						Text="Class Code"/>
					<ext:Column runat="server"
						DataIndex="ORGANIZATION_NAME" 
						Text="Organization Name"/>
					<ext:Column runat="server"
						DataIndex="ODOMETER_START" 
						Text="Starting Units"/>
					<ext:Column runat="server"
						DataIndex="ODOMETER_END"
						Text="Ending Units" />
				</Columns>
			</ColumnModel>
			<SelectionModel>
				<ext:RowSelectionModel runat="server" AllowDeselect="true" Mode="Single" />
			</SelectionModel>
			<DirectEvents>
				<Select OnEvent="deEnableEdit" />
			</DirectEvents>
			<Listeners>
				<Deselect Handler="#{uxEditEquipmentButton}.disable();
					#{uxRemoveEquipmentButton}.disable()" />
			</Listeners>
		</ext:GridPanel>
		<%-- Hidden Windows --%>
		
	</form>
</body>
</html>

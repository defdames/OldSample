<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umChooseLunchHeader.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.DailyActivity.umChooseLunchHeader" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
	<ext:ResourceManager ID="ResourceManager1" runat="server" IsDynamic="False" />
				<ext:FormPanel runat="server" ID="uxChooseLunchForm" Layout="FormLayout">
					<Items>
						<ext:ComboBox runat="server" ID="uxLunchHeader" DisplayField="ProjectTask" 
							ValueField="HeaderId" QueryMode="Local" ForceSelection="true" TypeAhead="true" EmptyText="Choose a project/task" FieldLabel="Choose Lunch Project">
							<Store>
								<ext:Store runat="server" ID="uxLunchHeaderStore" OnReadData="deReadLunchHeaders" AutoDataBind="true">
									<Model>
										<ext:Model runat="server">
											<Fields>
												<ext:ModelField Name="ProjectTask" Type="String" />
												<ext:ModelField Name="HeaderId" Type="Int" />
											</Fields>
										</ext:Model>
									</Model>
									<Proxy>
										<ext:PageProxy />
									</Proxy>
								</ext:Store>
							</Store>
						</ext:ComboBox>
					</Items>
					<Buttons>
						<ext:Button runat="server" Text="Submit">
							<DirectEvents>
								<Click OnEvent="deStoreLunchChoice">
									<EventMask ShowMask="true" />
								</Click>
							</DirectEvents>
						</ext:Button>
						<ext:Button runat="server" Text="Cancel" Icon="Delete">
							<Listeners>
								<Click Handler="#{uxChooseLunchForm}.reset();
									parentAutoLoadControl.hide()" />
							</Listeners>
						</ext:Button>

							
					</Buttons>
					<Listeners>
										<AfterRender
					Handler="var win = parentAutoLoadControl.target || parentAutoLoadControl, //you can use just 'parentAutoLoadControl' after update to Ext.NET v2 beta.
									size = this.getSize();
 
								size.height += 250;
								size.width += 12;
								win.setSize(size);"
					Delay="100" />
					</Listeners>
				</ext:FormPanel>
	</form>
</body>
</html>

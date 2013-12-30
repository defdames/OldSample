<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umDataEntryTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umDataEntryTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" />
    <div></div>
     <ext:GridPanel ID="GridPanel1" runat="server" Title="Data Entry" Layout="FitLayout">
                   <Store>
				<ext:Store runat="server"
					ID="Store1"
					AutoDataBind="true" WarningOnDirty="false">
					<Model>
						<ext:Model ID="Model2" runat="server">
							<Fields>
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
								<ext:ModelField Name="" />
							</Fields>
						</ext:Model>
					</Model>
					<%--<Listeners>
						<Load Fn="doMath" />
					</Listeners>--%>
				</ext:Store>
			</Store>
			<ColumnModel>
				<Columns>
					<ext:Column ID="uxAppDateDE" runat="server" DataIndex="" Text="Application Date" Flex="1"/>
					<ext:Column ID="uxTruckNumDE" runat="server" DataIndex="" Text="Truck #" Flex="1" />
					<ext:Column ID="uxSprayDE" runat="server" DataIndex="" Text="Spray" Flex="1" />
					<ext:Column ID="uxCutDE" runat="server" DataIndex="" Text="Cut" Flex="1" />
					<ext:Column ID="uxInspectDE" runat="server" DataIndex="" Text="Inspect" Flex="1" />
					<ext:Column runat="server" ID="uxStateDE" Text="State" DataIndex="" Flex="1" />
					<ext:Column ID="uxSubDivisonDE" runat="server" DataIndex="" Text="SubDivision" Flex="1" />
					<ext:Column runat="server" ID="uxCrossingNumDE" Text="Crossing #" DataIndex="" Flex="1" />
					
					
				</Columns>
			</ColumnModel>
                   <TopBar>
                   
                        <ext:Toolbar ID="Toolbar3" runat="server">
                        <Items>
                        <ext:Button ID="uxAddAppButton" runat="server" Text="Add Application Entry" Icon="ApplicationAdd" />
                        <ext:Button ID="uxEditAppButton" runat="server" Text="Edit Application Entry" Icon="ApplicationEdit" />
                        <ext:Button ID="uxDeleteAppButton" runat="server" Text="Delete Application Entry" Icon="ApplicationDelete" />
                       
                        </Items>
                            
                         </ext:Toolbar>
                </TopBar>
                   
               </ext:GridPanel>
                      
                        
               
    
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umContactsTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umContactsTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" ID="ResourceManager2" />
    <div>
      <%--<ContactsTab>--%>
               <ext:GridPanel ID="GridPanel1" runat="server"  Layout="HBoxLayout">
                   <Store>
				<ext:Store runat="server"
					ID="uxCurrentContactStore"
					AutoDataBind="true" WarningOnDirty="false">
					<Model>
						<ext:Model ID="Model1" runat="server">
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
					<ext:Column ID="uxNameCON" runat="server" DataIndex="" Text="Manager Name" Flex="1"/>
					<ext:Column ID="uxAddress1CON" runat="server" DataIndex="" Text="Address 1" Flex="1" />
					<ext:Column ID="uxAddress2CON" runat="server" DataIndex="" Text="Address 2" Flex="1" />
					<ext:Column ID="uxCityCON" runat="server" DataIndex="" Text="City" Flex="1" />
					<ext:Column ID="uxStateCON" runat="server" DataIndex="" Text="State" Flex="1" />
					<ext:Column ID="uxZipCON" runat="server"  Text="Zip" DataIndex="" Flex="1" />
					<ext:Column ID="uxEmailCON" runat="server" DataIndex="" Text="Email" Flex="1" />
					<ext:Column runat="server" ID="uxWorkNumCON" Text="Work #" DataIndex="" Flex="1" />
					<ext:Column runat="server" ID="uxCellNumCON" Text="Cell #" DataIndex="" Flex="1" />
					<ext:Column ID="uxRRCON" runat="server" DataIndex="" Text="RR" Flex="1" />
					
				</Columns>
			</ColumnModel>
                   <TopBar>
                   
                        <ext:Toolbar ID="Toolbar2" runat="server">
                        <Items>
                        <ext:Button ID="uxAddContactButton" runat="server" Text="Add New Contact" Icon="ApplicationAdd" />
                        <ext:Button ID="uxEditContactButton" runat="server" Text="Edit Contact" Icon="ApplicationEdit" />
                        <ext:Button ID="uxAssignContactButton" runat="server" Text="Assign Crossing to Contact" Icon="ApplicationGo" />
                        <ext:Button ID="uxDeleteContact" runat="server" Text="Delete Contact" Icon="ApplicationDelete" />
                        </Items>
                            
                         </ext:Toolbar>
                </TopBar>
                   
               </ext:GridPanel>
    </div>
    </form>
</body>
</html>

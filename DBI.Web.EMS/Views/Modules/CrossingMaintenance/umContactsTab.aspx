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
                        <ext:Button ID="uxAddContactButton" runat="server" Text="Add New Contact" Icon="ApplicationAdd" >
                             <Listeners>
								<Click Handler="#{uxAddContactWindow}.show()" />
							</Listeners>
                        </ext:Button>
                        <ext:Button ID="uxEditContactButton" runat="server" Text="Edit Contact" Icon="ApplicationEdit" >
                             <Listeners>
								<Click Handler="#{uxEditContactWindow}.show()" />
							</Listeners>
                        </ext:Button>
                        <ext:Button ID="uxAssignContactButton" runat="server" Text="Assign Crossing to Contact" Icon="ApplicationGo" >
                            <Listeners>
								<Click Handler="#{uxAssignCrossingWindow}.show()" />
							</Listeners>
                        </ext:Button>
                        <ext:Button ID="uxDeleteContact" runat="server" Text="Delete Contact" Icon="ApplicationDelete" >
                            <DirectEvents>
								<Click OnEvent="deRemoveContact">
									<Confirmation ConfirmRequest="true" Title="Remove?" Message="Are you sure you want to delete this contact?" />
									
								</Click>
							</DirectEvents>
                        </ext:Button>
                            

                        </Items>
                           
                         </ext:Toolbar>
                </TopBar>
                   
               </ext:GridPanel>
        <%-------------------------------------Hidden Windows-------------------------------------------------%>
         <ext:Window runat="server"
			ID="uxAddContactWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Add New Contact"
			Width="250">
        <Items>
        <ext:FormPanel runat="server" Layout="FormLayout">
            <Items>
                
                 <ext:TextField ID="uxAddNewManagerName" runat="server" FieldLabel="Manager Name" AnchorHorizontal="100%" LabelAlign="Right" />
                 <ext:TextField ID="uxAddNewEmail" runat="server" FieldLabel="Email" AnchorHorizontal="100%" LabelAlign="Right" />
                 <ext:DropDownField ID="uxAddNewRR" runat="server" FieldLabel="RR" AnchorHorizontal="100%" LabelAlign="Right"/>
                 <ext:TextField ID="uxAddNewAddress1" runat="server" FieldLabel="Address 1" AnchorHorizontal="100%" LabelAlign="Right" />
                 <ext:TextField ID="uxAddNewAddress2" runat="server" FieldLabel="Address 2" AnchorHorizontal="100%" LabelAlign="Right"/>
                 <ext:DropDownField ID="uxAddNewContactState" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right" />
                 <ext:DropDownField  ID="uxAddNewContactCity" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right" />
                 <ext:TextField ID="uxAddNewContactZip" runat="server" FieldLabel="Zip" AnchorHorizontal="100%" LabelAlign="Right"/>
                 <ext:TextField ID="uxAddNewContactCell" runat="server" FieldLabel="Cell #" AnchorHorizontal="100%" LabelAlign="Right" />
                 <ext:TextField ID="uxAddNewContactOffice" runat="server" FieldLabel="Office #" AnchorHorizontal="100%" LabelAlign="Right"/>
                  
            </Items>
            <Buttons>
                <ext:Button runat="server" ID="uxAddNewContactButton" Text="Add" Icon="Add" />
                <ext:Button runat="server" ID="uxAddNewContactCancelButton" Text="Cancel" Icon="Delete" />

            </Buttons>
        </ext:FormPanel>
             </Items>
        </ext:Window>

          <ext:Window runat="server"
			ID="uxEditContactWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Edit Existing Contact"
			Width="250">
        <Items>
        <ext:FormPanel ID="FormPanel1" runat="server" Layout="FormLayout">
            <Items>
                
                 <ext:TextField ID="TextField1" runat="server" FieldLabel="Manager Name" AnchorHorizontal="100%" LabelAlign="Right" />
                 <ext:TextField ID="TextField2" runat="server" FieldLabel="Email" AnchorHorizontal="100%" LabelAlign="Right" />
                 <ext:DropDownField ID="TextField3" runat="server" FieldLabel="RR" AnchorHorizontal="100%" LabelAlign="Right"/>
                 <ext:TextField ID="TextField4" runat="server" FieldLabel="Address 1" AnchorHorizontal="100%" LabelAlign="Right" />
                 <ext:TextField ID="TextField5" runat="server" FieldLabel="Address 2" AnchorHorizontal="100%" LabelAlign="Right"/>
                 <ext:DropDownField ID="TextField7" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right" />
                 <ext:DropDownField ID="TextField6" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right" />
                 <ext:TextField ID="TextField8" runat="server" FieldLabel="Zip" AnchorHorizontal="100%" LabelAlign="Right"/>
                 <ext:TextField ID="TextField9" runat="server" FieldLabel="Cell #" AnchorHorizontal="100%" LabelAlign="Right" />
                 <ext:TextField ID="TextField10" runat="server" FieldLabel="Office #" AnchorHorizontal="100%" LabelAlign="Right"/>
                  
            </Items>
            <Buttons>
                <ext:Button runat="server" ID="Button1" Text="Update" Icon="Add" />
                <ext:Button runat="server" ID="Button2" Text="Cancel" Icon="Delete" />

            </Buttons>
        </ext:FormPanel>
             </Items>
        </ext:Window>

        <ext:Window runat="server"
			ID="uxAssignCrossingWindow"
			Layout="FormLayout"
			Hidden="true"
            Title="Assign Crossing to Manager"
			Width="250">
        <Items>
        <ext:FormPanel ID="FormPanel2" runat="server" Layout="FormLayout">
            <Items>
                
                 <ext:TextField ID="TextField11" runat="server" FieldLabel="Manager Name" AnchorHorizontal="100%" LabelAlign="Right" />
                 <ext:TextField ID="TextField12" runat="server" FieldLabel="Email" AnchorHorizontal="100%" LabelAlign="Right" />
                 <ext:DropDownField ID="DropDownField1" runat="server" FieldLabel="RR" AnchorHorizontal="100%" LabelAlign="Right"/>
                 <ext:DropDownField ID="DropDownField2" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right" />
                 <ext:DropDownField ID="DropDownField3" runat="server" FieldLabel="Sub-Division" AnchorHorizontal="100%" LabelAlign="Right" />
                    
            </Items>
            <Buttons>
                <ext:Button runat="server" ID="Button3" Text="Assign" Icon="Add" />
                <ext:Button runat="server" ID="Button4" Text="Cancel" Icon="Delete" />

            </Buttons>
        </ext:FormPanel>
             </Items>
        </ext:Window>

    </div>
    </form>
</body>
</html>

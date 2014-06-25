<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umContactsTab.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umContactsTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">

        var getDragDropText = function () {
            var buf = [];

            buf.push("<ul>");

            Ext.each(this.view.panel.getSelectionModel().getSelection(), function (record) {
                buf.push("<li>" + record.data.CROSSING_NUMBER + "</li>");
            });

            buf.push("</ul>");

            return buf.join("");
        };
    </script>


</head>
<body>

    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" ID="ResourceManager2" />
        <div>
            <%--<ContactsTab>--%>
            <ext:GridPanel ID="uxContactMainGrid" runat="server" Layout="HBoxLayout" Collapsible="true" Title="CONTACTS">
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" AllowDeselect="false" Mode="Single" />
                </SelectionModel>
                <Store>
                    <ext:Store runat="server"
                        ID="uxCurrentContactStore"
                        OnReadData="deContactMainGrid"
                        AutoDataBind="true" WarningOnDirty="false" PageSize="15">
                        <Model>
                            <ext:Model ID="Model1" runat="server">
                                <Fields>
                                    <ext:ModelField Name="CONTACT_ID" />
                                    <ext:ModelField Name="CROSSING_ID" />
                                    <ext:ModelField Name="CONTACT_NAME" />
                                    <ext:ModelField Name="WORK_NUMBER" />
                                    <ext:ModelField Name="CELL_NUMBER" />
                                    <ext:ModelField Name="RAILROAD" />
                                </Fields>
                            </ext:Model>
                        </Model>
                        <Proxy>
                            <ext:PageProxy />
                        </Proxy>
                    </ext:Store>
                </Store>
                <ColumnModel>
                    <Columns>
                        <ext:Column ID="uxNameCON" runat="server" DataIndex="CONTACT_NAME" Text="Manager Name" Flex="1" />
                        <ext:Column ID="uxRRCON" runat="server" DataIndex="RAILROAD" Text="RR" Flex="1" />
                        <ext:Column runat="server" ID="uxWorkNumCON" Text="Work #" DataIndex="WORK_NUMBER" Flex="1" />
                        <ext:Column runat="server" ID="uxCellNumCON" Text="Cell #" DataIndex="CELL_NUMBER" Flex="1" />
                        
                    </Columns>
                </ColumnModel>
                <DirectEvents>
                    <Select OnEvent="GetContactGridData">
                        <ExtraParams>
                            <ext:Parameter Name="ContactId" Value="#{uxContactMainGrid}.getSelectionModel().getSelection()[0].data.CONTACT_ID" Mode="Raw" />
                        </ExtraParams>
                    </Select>
                </DirectEvents>
                <DirectEvents>
                    <Select OnEvent="deEditContactsForm">
                        <ExtraParams>
                            <ext:Parameter Name="ContactId" Value="#{uxContactMainGrid}.getSelectionModel().getSelection()[0].data.CONTACT_ID" Mode="Raw" />
                        </ExtraParams>
                    </Select>
                </DirectEvents>
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True">
                    </ext:PagingToolbar>
                </BottomBar>
                 <Listeners>
				<Select Handler="#{uxEditContactButton}.enable(); #{uxDeleteContact}.enable();" /> 
                            
			</Listeners>
            </ext:GridPanel>
            <ext:Toolbar ID="Toolbar2" runat="server">
                <Items>
                    <ext:Button ID="uxAddContactButton" runat="server" Text="Add New Contact" Icon="ApplicationAdd">
                        <Listeners>
                            <Click Handler="#{uxAddContactWindow}.show()" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="uxEditContactButton" runat="server" Text="Edit Contact" Icon="ApplicationEdit" Disabled="true">
                        <Listeners>
                            <Click Handler="#{uxEditContactWindow}.show()" />
                        </Listeners>
                        <DirectEvents>
                            <Click OnEvent="deEditContactsForm">
                                <ExtraParams>
                                    <ext:Parameter Name="ContactId" Value="#{uxContactMainGrid}.getSelectionModel().getSelection()[0].data.CONTACT_ID" Mode="Raw" />
                                </ExtraParams>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                     <ext:Button ID="uxDeleteContact" runat="server" Text="Delete Contact" Icon="ApplicationDelete" Disabled="true">
                        <DirectEvents>
                            <Click OnEvent="deRemoveContact">
                                <Confirmation ConfirmRequest="true" Title="Remove?" Message="Are you sure you want to delete this contact?" />
                                <ExtraParams>
                                    <ext:Parameter Name="ContactId" Value="#{uxContactMainGrid}.getSelectionModel().getSelection()[0].data.CONTACT_ID" Mode="Raw" />
                                </ExtraParams>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="uxAssignContactButton" runat="server" Text="Assign Crossings to Contact" Icon="ApplicationGo">
                        <Listeners>
                            <Click Handler="#{uxAssignCrossingWindow}.show()" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="uxUpdateContactButton" runat="server" Text="Update Manager Crossings" Icon="TransmitGo">
                        <Listeners>
                            <Click Handler="#{uxUpdateContactWindow}.show()" />
                        </Listeners>
                    </ext:Button>
                   
                </Items>
            </ext:Toolbar>
            
            <ext:FormPanel runat="server" ID="uxContactFormPanel" Layout="FormLayout" MaxWidth="750" >
                <Items>
                    <ext:TextField ID="uxContactManagerName" runat="server" FieldLabel="Manager Name" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                    <ext:TextField ID="uxContactRR" runat="server" FieldLabel="RR" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true"  />
                    <ext:FieldContainer runat="server" Layout="HBoxLayout">
                        <Items>
                    
                    <ext:TextField ID="uxContactAddress1" runat="server" FieldLabel="Address 1" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true"  />
                    <ext:TextField ID="uxContactAddress2" runat="server" FieldLabel="Address 2" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                    <ext:TextField ID="uxContactCity" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                            </Items>
                    </ext:FieldContainer>
                     <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                        <Items>
                    <ext:TextField ID="uxContactState" runat="server" FieldLabel="State" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                    <ext:TextField ID="uxContactZip" runat="server" FieldLabel="Zip" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                    <ext:TextField ID="uxContactOffice" runat="server" FieldLabel="Office #" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" />
                                 </Items>
                    </ext:FieldContainer>
                      <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                        <Items>
                    <ext:TextField ID="uxContactCell" runat="server" FieldLabel="Cell #" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" Hidden="true" />
                    
                               </Items>
                    </ext:FieldContainer>
                </Items>
            </ext:FormPanel>
            <%-------------------------------------Hidden Windows-------------------------------------------------%>
            <ext:Window runat="server"
                ID="uxAddContactWindow"
                Layout="FormLayout"
                Hidden="true"
                Title="Add New Contact"
                Width="250" Closable="false" Modal="true">
                <Items>
                    <ext:FormPanel runat="server" ID="uxAddContactForm" Layout="FormLayout">
                        <Items>
                            <ext:TextField ID="uxAddNewManagerName" runat="server" FieldLabel="Manager Name" AnchorHorizontal="100%" LabelAlign="Right" AllowBlank="false" />
                     
                              <ext:ComboBox ID="uxAddRRType" runat="server" FieldLabel="Rail Road" AnchorHorizontal="100%" LabelAlign="Right"
                                                DisplayField="project"
                                                ValueField="project"
                                                QueryMode="Local"
                                                TypeAhead="true" AllowBlank="false" ForceSelection="true">
                                                <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxAddContactRRStore">
                                                        <Model>
                                                            <ext:Model ID="Model9" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="project" />
                                                                    <ext:ModelField Name="project" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                    </ext:Store>
                                                </Store>                                               
                                            </ext:ComboBox>
                            <ext:TextField ID="uxAddNewAddress1" runat="server" FieldLabel="Address 1" AnchorHorizontal="100%" LabelAlign="Right" />
                            <ext:TextField ID="uxAddNewAddress2" runat="server" FieldLabel="Address 2" AnchorHorizontal="100%" LabelAlign="Right" />
                            <ext:TextField ID="uxAddNewContactCityTextField" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right" />
                           
                            <ext:ComboBox runat="server"
                                ID="uxAddContactStateComboBox"
                                FieldLabel="State"
                                LabelAlign="Right"
                                DisplayField="name"
                                ValueField="name"
                                QueryMode="Local"
                                TypeAhead="true"
                                AllowBlank="false" ForceSelection="true">
                                <Store>
                                    <ext:Store ID="uxAddStateList" runat="server" AutoDataBind="true">
                                        <Model>
                                            <ext:Model ID="Model10" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="abbr" />
                                                    <ext:ModelField Name="name" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                        <Reader>
                                            <ext:ArrayReader />
                                        </Reader>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>
                            <ext:TextField ID="uxAddNewContactZip" runat="server" FieldLabel="Zip" AnchorHorizontal="100%" LabelAlign="Right"  />
                            <ext:TextField ID="uxAddNewContactCell" runat="server" FieldLabel="Cell #" AnchorHorizontal="100%" LabelAlign="Right" />
                            <ext:TextField ID="uxAddNewContactOffice" runat="server" FieldLabel="Office #" AnchorHorizontal="100%" LabelAlign="Right" />
                        </Items>
                        <Buttons>
                            <ext:Button runat="server" ID="deAddContacts" Text="Add" Icon="Add" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="deAddContact" />
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button runat="server" ID="uxAddNewContactCancelButton" Text="Cancel" Icon="Delete">
                                <Listeners>
                                    <Click Handler="#{uxAddContactForm}.reset();
									#{uxAddContactWindow}.hide()" />
                                </Listeners>
                            </ext:Button>
                        </Buttons>
                         <Listeners>
						<ValidityChange Handler="#{deAddContacts}.setDisabled(!valid);" />
					</Listeners>
                    </ext:FormPanel>
                </Items>
            </ext:Window>
            <%----------------------------------------------------------------------------------------------------------------------------------------%>
            <ext:Window runat="server"
                ID="uxEditContactWindow"
                Layout="FormLayout"
                Hidden="true"
                Title="Edit Existing Contact"
                Width="250" Closable="false" Modal="true">
                <Items>
                    <ext:FormPanel ID="uxEditContactForm" runat="server" Layout="FormLayout">
                        <Items>
                            <ext:TextField ID="uxEditManagerName" runat="server" FieldLabel="Manager Name" AnchorHorizontal="100%" LabelAlign="Right" AllowBlank="false" />
                    
                              <ext:ComboBox ID="uxEditRRType" runat="server" FieldLabel="Rail Road" AnchorHorizontal="100%" LabelAlign="Right"
                                                DisplayField="project"
                                                ValueField="project"
                                                QueryMode="Local"
                                                TypeAhead="true" AllowBlank="false" ForceSelection="true">
                                                <Store>
                                                    <ext:Store runat="server"
                                                        ID="uxEditContactRRStore">
                                                        <Model>
                                                            <ext:Model ID="Model11" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="project" />
                                                                    <ext:ModelField Name="project" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                    </ext:Store>
                                                </Store>
                                              
                                                
                                            </ext:ComboBox>
                            <ext:TextField ID="uxEditContactAdd1" runat="server" FieldLabel="Address 1" AnchorHorizontal="100%" LabelAlign="Right" />
                            <ext:TextField ID="uxEditContactAdd2" runat="server" FieldLabel="Address 2" AnchorHorizontal="100%" LabelAlign="Right" />
                            <ext:TextField ID="uxEditContactCity" runat="server" FieldLabel="City" AnchorHorizontal="100%" LabelAlign="Right" />
                           
                            <ext:ComboBox runat="server"
                                ID="uxEditContactState"
                                FieldLabel="State"
                                LabelAlign="Right"
                                DisplayField="name"
                                ValueField="name"
                                QueryMode="Local"
                                TypeAhead="true"
                                AllowBlank="false" ForceSelection="true">
                                <Store>
                                    <ext:Store ID="uxEditStateList" runat="server" AutoDataBind="true">
                                        <Model>
                                            <ext:Model ID="Model8" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="abbr" />
                                                    <ext:ModelField Name="name" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                        <Reader>
                                            <ext:ArrayReader />
                                        </Reader>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>
                            <ext:TextField ID="uxEditContactZip" runat="server" FieldLabel="Zip" AnchorHorizontal="100%" LabelAlign="Right" />
                            <ext:TextField ID="uxEditContactCellNum" runat="server" FieldLabel="Cell #" AnchorHorizontal="100%" LabelAlign="Right" />
                            <ext:TextField ID="uxEditContactPhoneNum" runat="server" FieldLabel="Office #" AnchorHorizontal="100%" LabelAlign="Right" />
                        </Items>
                        <Buttons>
                            <ext:Button runat="server" ID="uxEditUpdateButton" Text="Update" Icon="Add">
                                <DirectEvents>
                                    <Click OnEvent="deEditContacts">
                                        <ExtraParams>
                                            <ext:Parameter Name="ContactId" Value="#{uxContactMainGrid}.getSelectionModel().getSelection()[0].data.CONTACT_ID" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button runat="server" ID="uxEditContactCancelButton" Text="Cancel" Icon="Delete">
                                <Listeners>
                                    <Click Handler="#{uxEditContactForm}.reset();
									#{uxEditContactWindow}.hide()" />
                                </Listeners>
                            </ext:Button>
                        </Buttons>
                         <Listeners>
						<ValidityChange Handler="#{uxEditUpdateButton}.setDisabled(!valid);" />
					</Listeners>
                    </ext:FormPanel>
                </Items>
            </ext:Window>
            <%--------------------------------------------------------------------------------------------------------------------------------------%>
            <ext:Window runat="server"
                ID="uxAssignCrossingWindow"
                Layout="FormLayout"
                Hidden="true"
                Title="Assign Crossing to Manager" Width="950" Modal="true">
                <Items>
                    <ext:Panel ID="uxAssignContactPanel" runat="server" Width="940" Height="330">
                        <LayoutConfig>
                            <ext:HBoxLayoutConfig Align="Stretch" Padding="5" />
                        </LayoutConfig>
                        <Items>
                            <ext:GridPanel ID="uxAssignContactGrid" runat="server" Flex="1" Title="Managers" Margins="0 2 0 0">

                                <Store>
                                    <ext:Store runat="server"
                                        ID="uxAssignContactManagerStore"
                                        OnReadData="deAssignContactManagerGrid"
                                        
                                        AutoDataBind="true" WarningOnDirty="false">
                                        <Model>
                                            <ext:Model ID="Model4" runat="server" IDProperty="CONTACT_ID">
                                                <Fields>
                                                    <ext:ModelField Name="CONTACT_ID" />
                                                    <ext:ModelField Name="CONTACT_NAME" />
                                                    <ext:ModelField Name="RAILROAD" />
                                                    <ext:ModelField Name="WORK_NUMBER" />
                                                    <ext:ModelField Name="CELL_NUMBER" />

                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                    </ext:Store>
                                </Store>
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="Column2" runat="server" DataIndex="CONTACT_NAME" Text="Manager" Flex="2" />
                                        <ext:Column ID="Column12" runat="server" DataIndex="RAILROAD" Text="Rail Road" Flex="1" />
                                        <ext:Column ID="Column10" runat="server" DataIndex="WORK_NUMBER" Text="Work Number" Flex="2" />
                                        <ext:Column ID="Column11" runat="server" DataIndex="CELL_NUMBER" Text="Cell Number" Flex="2" />
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="sm" runat="server" Mode="Single" AllowDeselect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <Plugins>
                                    <ext:FilterHeader ID="FilterHeader2" runat="server" Remote="true" />
                                </Plugins>

                               
                                  <Listeners>
				                    <Select Handler="#{uxAssignCrossingGrid}.enable(); " /> 
                                    <Deselect Handler="#{uxAssignCrossingGrid}.disable(); #{uxApplyButtonCON}.disable()"/>
                                     
			                    </Listeners>

                            </ext:GridPanel>
                            <%-----------------------------------------------------------------------------------------------------------------------%>
                            <ext:GridPanel ID="uxAssignCrossingGrid" runat="server" Flex="1" Title="Crossings" Margins="0 2 0 0" Disabled="true">
                                <Store>
                                    <ext:Store runat="server"
                                        ID="uxAssignContactCrossingStore"
                                        OnReadData="deAssignContactCrossingGrid" 
                                        
                                        AutoDataBind="true" WarningOnDirty="false">
                                        <Model>
                                            <ext:Model ID="Model5" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="CROSSING_ID" />
                                                    <ext:ModelField Name="CROSSING_NUMBER" />
                                                    <ext:ModelField Name="RAILROAD" />
                                                    <ext:ModelField Name="SERVICE_UNIT" />
                                                    <ext:ModelField Name="SUB_DIVISION" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                    </ext:Store>
                                </Store>
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="Column6" runat="server" DataIndex="CROSSING_NUMBER" Text="Crossing #" Flex="1" />
                                        <ext:Column ID="Column7" runat="server" DataIndex="RAILROAD" Text="Rail Road" Flex="1" />
                                        <ext:Column ID="Column8" runat="server" DataIndex="SERVICE_UNIT" Text="Service Unit" Flex="2" />
                                        <ext:Column ID="Column9" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="2" />
                                    </Columns>
                                </ColumnModel>
                                <Plugins>
                                    <ext:FilterHeader ID="FilterHeader3" runat="server" Remote="true"/>
                                </Plugins>
                                <SelectionModel>
                                    <ext:CheckboxSelectionModel ID="cm" runat="server" Mode="Multi" />
                                </SelectionModel>
                               <%-- <BottomBar>
                                    <ext:PagingToolbar ID="PagingToolbar4" runat="server" />
                                </BottomBar>--%>
                                  <Listeners>
				                    <Select Handler="#{uxApplyButtonCON}.enable();" /> 
                                    
			                      </Listeners>
                            </ext:GridPanel>

                        </Items>
                    </ext:Panel>
                    <ext:Toolbar ID="Toolbar3" runat="server">
                        <Items>
                            <ext:ToolbarFill ID="ToolbarFill2" runat="server" />

                            <ext:Button ID="uxApplyButtonCON" runat="server" Text="Associate" Icon="ArrowJoin" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="deAssignCrossingtoContact">
                                        <Confirmation ConfirmRequest="true" Title="Associate?" Message="Are you sure you want to associate the selected crossings with the selected contact?" />
                                        <ExtraParams>
                                            <ext:Parameter Name="contactId" Value="#{uxAssignContactGrid}.getSelectionModel().getSelection()[0].data.CONTACT_ID" Mode="Raw" />
                                            <ext:Parameter Name="selectedCrossings" Value="Ext.encode(#{uxAssignCrossingGrid}.getRowsValues({selectedOnly: true}))" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="CancelButtonCON" runat="server" Text="Cancel" Icon="Delete">
                                <Listeners>
                                    <Click Handler="#{uxAssignCrossingWindow}.hide()" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </Items>
            </ext:Window>

            <%----------------------------------------------------------------------------------------------------------------------------------%>

            <ext:Window runat="server"
                ID="uxUpdateContactWindow"
                Layout="FormLayout"
                Hidden="true"
                Title="Update Crossings To New Manager"
                Width="550" Height="120" Closable="false" Modal="true">
                <Items>
                    <ext:FormPanel ID="uxUpdateContactForm" runat="server" Layout="FormLayout">
                        <Items>
                            <ext:DropDownField ID="uxUpdateContactCurrentManager" runat="server" FieldLabel="Current Manager" AnchorHorizontal="100%" LabelAlign="Right" Mode="ValueText" AllowBlank="false" Editable="false">
                                <Component>
                                    <ext:GridPanel runat="server"
                                        ID="uxAddCurrentManager"
                                        Layout="HBoxLayout">
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="uxCurrentManagerStore"
                                                PageSize="10"
                                                RemoteSort="true"
                                                OnReadData="deCurrentManagerGrid">
                                                <Model>
                                                    <ext:Model ID="Model6" runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="CONTACT_ID" Type="Int" />
                                                            <ext:ModelField Name="CONTACT_NAME" Type="String" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column ID="Column3" runat="server" Text="Manager Name" DataIndex="CONTACT_NAME" Width="425" />
                                            </Columns>
                                        </ColumnModel>
                                        <BottomBar>
                                            <ext:PagingToolbar ID="PagingToolbar3" runat="server" />
                                        </BottomBar>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="RowSelectionModel3" runat="server" Mode="Single" />
                                        </SelectionModel>
                                        <DirectEvents>
                                            <SelectionChange OnEvent="deStoreCurrentManagerValue">
                                                <ExtraParams>
                                                    <ext:Parameter Name="ContactId" Value="#{uxAddCurrentManager}.getSelectionModel().getSelection()[0].data.CONTACT_ID" Mode="Raw" />
                                                    <ext:Parameter Name="ContactName" Value="#{uxAddCurrentManager}.getSelectionModel().getSelection()[0].data.CONTACT_NAME" Mode="Raw" />
                                                    <ext:Parameter Name="Type" Value="CurrentManager" />
                                                </ExtraParams>
                                            </SelectionChange>
                                        </DirectEvents>
                                        <Plugins>
                                            <ext:FilterHeader runat="server" ID="uxAddCurrentManagerFilter" Remote="true" />
                                        </Plugins>
                                    </ext:GridPanel>
                                </Component>
                            </ext:DropDownField>
                            <ext:DropDownField ID="uxUpdateContactNewManager" runat="server" FieldLabel="New Manager" AnchorHorizontal="100%" LabelAlign="Right" Mode="ValueText" AllowBlank="false" Editable="false">
                                <Component>
                                    <ext:GridPanel runat="server"
                                        ID="uxNewManagerMainGrid"
                                        Layout="HBoxLayout">
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="uxNewManagerStore"
                                                PageSize="10"
                                                RemoteSort="true"
                                                OnReadData="deNewManagerGrid">
                                                <Model>
                                                    <ext:Model ID="Model7" runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="CONTACT_ID" Type="Int" />
                                                            <ext:ModelField Name="CONTACT_NAME" Type="String" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column ID="Column5" runat="server" Text="Manager Name" DataIndex="CONTACT_NAME" Width="425" />
                                            </Columns>
                                        </ColumnModel>
                                       <%-- <BottomBar>
                                            <ext:PagingToolbar ID="PagingToolbar5" runat="server" />
                                        </BottomBar>--%>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="RowSelectionModel4" runat="server" Mode="Single" />
                                        </SelectionModel>
                                        <DirectEvents>
                                            <SelectionChange OnEvent="deStoreNewManagerValue">
                                                <ExtraParams>
                                                    <ext:Parameter Name="ContactId" Value="#{uxNewManagerMainGrid}.getSelectionModel().getSelection()[0].data.CONTACT_ID" Mode="Raw" />
                                                    <ext:Parameter Name="ContactName" Value="#{uxNewManagerMainGrid}.getSelectionModel().getSelection()[0].data.CONTACT_NAME" Mode="Raw" />
                                                    <ext:Parameter Name="Type" Value="NewManager" />
                                                </ExtraParams>
                                            </SelectionChange>
                                        </DirectEvents>
                                        <Plugins>
                                            <ext:FilterHeader runat="server" ID="uxNewManagerFilter" Remote="true" />
                                        </Plugins>
                                    </ext:GridPanel>
                                </Component>
                            </ext:DropDownField>
                        </Items>
                        <Buttons>
                            <ext:Button runat="server" ID="uxSelectCrossingToUpdate" Text="Select Crossings to Update" Icon="Add" Disabled="true">

                                <DirectEvents>
                                    <Click OnEvent="deShowGrid">
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button runat="server" ID="uxCancelCrossingToUpdate" Text="Cancel Selection" Icon="Delete">
                                <Listeners>
                                    <Click Handler="#{uxUpdateContactForm}.reset();
									#{uxUpdateContactWindow}.hide()" />
                                </Listeners>

                            </ext:Button>
                        </Buttons>
                        <Listeners>
                            <ValidityChange Handler="#{uxSelectCrossingToUpdate}.setDisabled(!valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
            </ext:Window>
            <%------------------------------------------------------------------------------------------------------------------------------------------%>

            <ext:Window runat="server"
                ID="uxTransferCrossingWindow"
                Layout="HBoxLayout"
                Height="593"
                Width="725"
                Hidden="true"
                BodyPadding="5"
                BodyBorder="0"
                Title="Drag and Drop Crossing That Are To Be Updated" Modal="true">
                <Items>
                    <ext:Panel ID="uxTransferCrossingPanel" runat="server" Width="700" Height="553">
                        <LayoutConfig>
                            <ext:HBoxLayoutConfig Align="Stretch" Padding="5" />
                        </LayoutConfig>

                        <Items>

                            <ext:GridPanel
                                ID="uxTransferCrossingsOldManagerGrid"
                                runat="server"
                                MultiSelect="true"
                                Flex="1"
                                Title="Current Managers Crossings"
                                Margins="0 2 0 0" >

                                <Store>
                                    <ext:Store ID="uxCurrentManagerCrossingStore" runat="server">
                                        <Model>
                                            <ext:Model ID="Model2" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="CROSSING_ID" />
                                                    <ext:ModelField Name="CONTACT_ID" />
                                                    <ext:ModelField Name="CROSSING_NUMBER" />
                                                    <ext:ModelField Name="SUB_DIVISION" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>

                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="Column1" runat="server" Text="Crossing Number" Width="280" DataIndex="CROSSING_NUMBER" Flex="1" />
                                        <ext:Column ID="Column13" runat="server" Text="Sub-Division" Width="280" DataIndex="SUB_DIVISION" Flex="1" />
                                   
                                         </Columns>
                                </ColumnModel>

                                <View>
                                    <ext:GridView>

                                        <Plugins>
                                            <ext:GridDragDrop ID="GridDragDrop1" runat="server" DragGroup="firstGridDDGroup" DropGroup="secondGridDDGroup" />
                                        </Plugins>

                                        <Listeners>
                                            <AfterRender Handler="this.plugins[0].dragZone.getDragText = getDragDropText;" Delay="1" />
                                            <Drop Handler="var dropOn = overModel ? ' ' + dropPosition + ' ' + overModel.get('CROSSING_NUMBER') : ' Back To Current Contact'; 
                                             Ext.net.Notification.show({title:'Drag from right to left', html:'Transferred ' + data.records[0].get('CROSSING_NUMBER') + dropOn});" />
                                        </Listeners>

                                    </ext:GridView>
                                </View>
                            </ext:GridPanel>
                            <ext:Panel
                                ID="Panel2"
                                runat="server"
                                Width="35"
                                BodyStyle="background-color: Lightgrey;"
                                Border="false"
                                Layout="Anchor">

                                <Items>
                                    <ext:Panel ID="Panel1" runat="server" Border="false" BodyStyle="background-color: Lightgrey;" AnchorVertical="40%" />
                                    <ext:Panel ID="Panel3" runat="server" Border="false" BodyStyle="background-color: Lightgrey;" BodyPadding="5">

                                        <Items>
                                            <ext:Button ID="Button1" runat="server" Icon="ResultsetNext">

                                                <Listeners>
                                                    <Click Handler="#{uxTransferCrossingsNewManagerStore}.loadData(#{uxCurrentManagerCrossingStore}.proxy.data); #{uxCurrentManagerCrossingStore}.removeAll(); #{uxCurrentManagerCrossingStore}.add();" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Label ID="Label1" runat="server" />
                                            <ext:Button ID="ResetTransfer" runat="server" Icon="ResultsetPrevious">
                                                <Listeners>
                                                    <Click Handler="#{uxCurrentManagerCrossingStore}.loadData(#{uxCurrentManagerCrossingStore}.proxy.data); #{uxTransferCrossingsNewManagerStore}.removeAll();" />
                                                </Listeners>
                                            </ext:Button>

                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            <ext:GridPanel
                                ID="deTransferCrossingsNewManagerGrid"
                                runat="server"
                                MultiSelect="true"
                                Title="Crossings Tranferred to New Manager"
                                Flex="1"
                                Margins="0 0 0 3" AllowDeselect="false">
                                <Store>
                                    <ext:Store ID="uxTransferCrossingsNewManagerStore" runat="server">
                                        <Model>
                                            <ext:Model ID="Model3" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="CONTACT_ID" />
                                                    <ext:ModelField Name="CROSSING_ID" />
                                                    <ext:ModelField Name="CROSSING_NUMBER" />
                                                    <ext:ModelField Name="SUB_DIVISION" />

                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                    </ext:Store>
                                </Store>
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="Column4" runat="server" Text="Crossing Number" Width="280" DataIndex="CROSSING_NUMBER" Flex="1" />
                                        <ext:Column ID="Column14" runat="server" Text="Sub-Division" Width="280" DataIndex="SUB_DIVISION" Flex="1" />
                                    
                                    </Columns>
                                </ColumnModel>
                                <View>
                                    <ext:GridView ID="GridView2" runat="server">
                                        <Plugins>
                                            <ext:GridDragDrop ID="GridDragDrop2" runat="server" DragGroup="secondGridDDGroup" DropGroup="firstGridDDGroup" />
                                        </Plugins>
                                        <Listeners>
                                            <Select Handler="#{TransferCrossingtoContact}.enable()" />
                                        </Listeners>
                                        <Listeners>
                                            <AfterRender Handler="this.plugins[0].dragZone.getDragText = getDragDropText;" Delay="1" />
                                            <Drop Handler="var dropOn = overModel ? ' ' + dropPosition + ' ' + overModel.get('CROSSING_NUMBER') : ' To New Contact'; 
                                               Ext.net.Notification.show({title:'Drag from left to right', html:'Transferred ' + data.records[0].get('CROSSING_NUMBER') + dropOn});" />
                                        </Listeners>
                                    </ext:GridView>
                                </View>
                            </ext:GridPanel>
                        </Items>
                        <BottomBar>
                            <ext:Toolbar ID="Toolbar1" runat="server">
                                <Items>
                                    <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                    <ext:Button ID="TransferCrossingtoContact" runat="server" Text="Update" Icon="TransmitGo">
                                        <DirectEvents>
                                            <Click OnEvent="AssociateTransfer">
                                                <Confirmation ConfirmRequest="true" Title="Associate?" Message="Are you sure you want to transfer the selected crossings to the new contact?" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="ContactId" Value="#{deTransferCrossingsNewManagerGrid}.getSelectionModel().getSelection()[0].data.CONTACT_ID" Mode="Raw" />
                                                    <ext:Parameter Name="selectedCrossings" Value="Ext.encode(#{deTransferCrossingsNewManagerGrid}.getRowsValues())" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="CancelTransfer" runat="server" Text="Cancel" Icon="Delete">
                                        <Listeners>
                                            <Click Handler="#{uxTransferCrossingsNewManagerStore}.reload();
									                    #{uxTransferCrossingWindow}.hide()" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </BottomBar>
                    </ext:Panel>
                </Items>

            </ext:Window>
        </div>
    </form>
</body>
</html>

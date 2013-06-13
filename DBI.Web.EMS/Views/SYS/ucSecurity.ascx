<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSecurity.ascx.cs" Inherits="DBI.Web.EMS.Views.SYS.ucSecurity" %>

<ext:Panel ID="Panel1" runat="server" Header="false" Border="false">
    <LayoutConfig>
        <ext:BorderLayoutConfig></ext:BorderLayoutConfig>
    </LayoutConfig>
  <Items>
                <ext:Panel ID="Panel2" 
                    runat="server" 
                    Region="North"
                    Margins="5 5 5 5"
                    Title="Description" 
                    Height="100" 
                    BodyPadding="5"
                    Frame="true" 
                    Icon="Information">
                    <Content>
                        <b>GridPanel with Form Details</b>
                        <p>Click on any record with the GridPanel and the record details will be loaded into the Details Form.</p>
                    </Content>
                </ext:Panel>
                    <ext:GridPanel ID="GridPanel1" 
                    runat="server" 
                    Title="Employees"
                    Margins="0 0 5 5"
                    Icon="UserSuit"
                    Region="Center" 
                    Frame="true">
                    <Store>
                        <ext:Store ID="Store1" 
                            Data="<%# DBI.Data.DataFactory.SecurityRoles.GetSecurityRoles() %>"
                            runat="server">
                            <Model>
                                <ext:Model ID="Model1" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ID" />
                                        <ext:ModelField Name="NAME" />
                                        <ext:ModelField Name="DESCRIPTION" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                            <ext:Column ID="Column1" runat="server" DataIndex="NAME" Text="Name" Flex="1" />
                            <ext:Column ID="Column2" runat="server" DataIndex="DESCRIPTION" Text="Description" Width="150" />
                        </Columns>
                    </ColumnModel>
                         <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server">
                                <Items>
                                    <ext:Button ID="Button1" runat="server" Text="Reload" Icon="Reload">
                                        <Listeners>
                                               <Click Handler="#{DirectMethods}.Reload();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                </ext:GridPanel>
                
            </Items>
 
</ext:Panel>

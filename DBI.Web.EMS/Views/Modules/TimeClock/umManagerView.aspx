<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umManagerView.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.TimeClock.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    
        <ext:ResourceManager ID="ResourceManager1" runat="server"/>
       
    <ext:viewport ID="Viewport1" runat="server" Layout="Fit">
        
        
       <Items>
        <ext:GridPanel 
            ID="uxEmployeeHoursGrid" 
            runat="server"
            Frame="true"
            Title="Employee Hours"
            Icon ="Table"
            Width ="1200"
            Resizeable="true"
            Collapsable="true">
            <Store>
                <ext:Store 
                    ID="uxEmployeeHoursStore"
                    runat="server"
                    GroupField="EMPLOYEE_NAME"
                    OnReadData="deUpdateEmployeeGrid">
                    <Model>
                        <ext:Model runat="server" IDProperty="EMP_HOURS">
                            <Fields>
                                <ext:ModelField Name="EMPLOYEE_NAME" />
                                <ext:ModelField Name="TIME_IN" Type="Date" />
                                <ext:ModelField Name="TIME_OUT" Type="Date"/>
                                <ext:ModelField Name="TOTAL_HOURS" />
                            </Fields>
                        </ext:Model>
                    </Model>
                    <Sorters>
                        <ext:DataSorter Property="EMPLOYEE_NAME" Direction="ASC"/>
                    </Sorters>
                </ext:Store>
            </Store>
            <ColumnModel runat="server">
                <Columns>
                    <ext:DateColumn runat="server" Text="Time In" DataIndex="TIME_IN" Flex="1" Format="M/d/yyyy h:mm:ss A"/>
                    <ext:DateColumn runat="server" Text="Time Out" DataIndex="TIME_OUT" Flex="1" Format="M/d/yyyy h:mm:ss A"/>
                    <ext:Column runat="server" Text="Total Time" Flex="1" DataIndex="TOTAL_HOURS">
                        <Editor>
                               <ext:TextField runat="server"/>
                        </Editor>
                    </ext:Column>
                    
                </Columns>
            </ColumnModel>
            <Features>
                <ext:Grouping
                    runat="server"
                    HideGroupHeader="true"
                    GroupHeaderTplString="Employee: {name}"
                    StartCollapsed="true"/>                               
            </Features>
            <SelectionModel>
                <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" Mode="Multi" />
            </SelectionModel>
            <Plugins>
                <ext:CellEditing runat="server" ClicksToEdit="2"/>
            </Plugins>  
        </ext:GridPanel>
           </Items>
        </ext:viewport>
        
    
    
</body>
</html>

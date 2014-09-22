<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="umWeeklyWorkActivity.aspx.cs" Inherits="DBI.Web.EMS.Views.Modules.CrossingMaintenance.umWeeklyWorkActivity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <style type="text/css">

        .rowBodyCls .x-grid-cell-rowbody {
            border-style: solid;
            border-width: 0px 0px 1px;
            border-color: black;
        }
       .x-grid-group-title {
        color: #000000;
        font: bold 11px/13px tahoma,arial,verdana,sans-serif;
        }
       .x-grid-group-hd {
        border-width: 0 0 1px 0;
        border-style: solid;
        border-color: #000000;
        padding: 10px 4px 4px 4px;
        background: white;
        cursor: pointer;
        }
          
    </style>
      <script>
          var saveData = function () {
              App.Hidden1.setValue(Ext.encode(App.GridPanel1.getRowsValues({ selectedOnly: false })));
          };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <ext:ResourceManager ID="ResourceManager1" runat="server" />
         <ext:Hidden ID="Hidden1" runat="server" Hidden="true" />
         <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
                <Items>
         <ext:FormPanel ID="uxFilterForm" runat="server" Margin="5" Region="North" Title="Filter Weekly Work Activity">
                <Items>
                    <ext:FieldSet ID="FieldSet1" runat="server" Title="Filter">
                        <Items>
                            <ext:TextField ID="uxRRCI" runat="server" FieldLabel="Railroad" AnchorHorizontal="100%" LabelAlign="Right" ReadOnly="true" Hidden="true" />

                           <ext:DateField ID="uxStartDate" runat="server" AnchorHorizontal="25%" FieldLabel="Start Date" LabelAlign="Right" Editable="false" TabIndex="2" EmptyText="ALL" />
                            <ext:DateField ID="uxEndDate" runat="server" AnchorHorizontal="25%" FieldLabel="End Date" LabelAlign="Right"  Editable="false" TabIndex="3" EmptyText="ALL" />

                            <ext:ComboBox ID="uxAddServiceUnit"
                                runat="server" FieldLabel="Service Unit"
                                LabelAlign="Right"
                                AnchorHorizontal="25%"
                                DisplayField="service_unit"
                                ValueField="service_unit"
                                QueryMode="Local" TypeAhead="true" TabIndex="3" ForceSelection="true" EmptyText="ALL">
                                <Store>
                                    <ext:Store runat="server"
                                        ID="uxAddServiceUnitStore">
                                        <Model>
                                            <ext:Model ID="Model5" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="service_unit" />
                                                    <ext:ModelField Name="service_unit" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                                  <DirectEvents>
                                                    <Select OnEvent="deLoadSubDiv">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Type" Value="Add" />
                                                        </ExtraParams>
                                                    </Select>
                                                </DirectEvents>
                                                <Listeners>
                                                    <Select Handler="#{uxAddSubDivStore}.load()" />
                                                </Listeners>
                            </ext:ComboBox>
                            <ext:ComboBox ID="uxAddSubDiv"
                                runat="server"
                                FieldLabel="Sub-Division"
                                LabelAlign="Right"
                                AnchorHorizontal="25%"
                                DisplayField="sub_division"
                                ValueField="sub_division"
                                TypeAhead="true" TabIndex="5" AllowBlank="false" ForceSelection="true" EmptyText="ALL">
                                <Store>
                                    <ext:Store runat="server"
                                        ID="uxAddSubDivStore">
                                        <Model>
                                            <ext:Model ID="Model7" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="sub_division" />
                                                    <ext:ModelField Name="sub_division" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>

                            </ext:ComboBox>
                            <ext:ComboBox runat="server"
                                ID="uxAddStateComboBox"
                                FieldLabel="State"
                                LabelAlign="Right"
                                AnchorHorizontal="25%"
                                DisplayField="name"
                                ValueField="name"
                                QueryMode="Local"
                                TypeAhead="true"
                                AllowBlank="false"
                                ForceSelection="true" TabIndex="4" EmptyText="ALL">
                                <Store>
                                    <ext:Store ID="uxAddStateList" runat="server" AutoDataBind="true" >
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
                         
                        </Items>
                    </ext:FieldSet>
                </Items>
                <BottomBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Button runat="server"
                                ID="Button4"
                                Text="Run"
                                Icon="PlayGreen">
                                <DirectEvents>
                                    <Click OnEvent="deLaunchGrid" >
                                        <EventMask ShowMask="true" Msg="Loading..." />
                                        </Click>
                                </DirectEvents>
                         <Listeners>
                                <Click Handler="#{uxWeeklyActivityStore}.load()" />
                            </Listeners>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="Button3"
                                Text="Cancel"
                                Icon="StopRed">
                             <DirectEvents>
                            <Click OnEvent="deClearFilters" />
                            </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </BottomBar>
            </ext:FormPanel>
          
       
        <ext:GridPanel
            ID="GridPanel1"
            runat="server"
            Title="Weekly Work Activity"
            Icon="Report"
            Frame="false"
            Resizable="false"
            Collapsible="false"
            Region="Center" 
            Hidden="true">
            <Store>
                <ext:Store ID="uxWeeklyActivityStore"
                    runat="server"
                    GroupField="SUB_DIVISION" OnReadData="deWeeklyActivityGrid" AutoLoad="false" PageSize="15">
                    <Model>
                        <ext:Model ID="Model1" runat="server">
                            <Fields>
                                <ext:ModelField Name="CROSSING_ID" />
                                <ext:ModelField Name="CROSSING_NUMBER" Type="String" />
                                <ext:ModelField Name="APPLICATION_ID" />
                                <ext:ModelField Name="MILE_POST" />
                                <ext:ModelField Name="STATE" />
                                <ext:ModelField Name="CITY" />
                                <ext:ModelField Name="SUB_DIVISION" />
                                <ext:ModelField Name="APPLICATION_DATE" />
                                <ext:ModelField Name="SPRAY" />
                                <ext:ModelField Name="CUT" />
                                <ext:ModelField Name="INSPECT" />                           

                            </Fields>
                        </ext:Model>
                    </Model>
                    <Proxy>
                        <ext:PageProxy />
                    </Proxy>
                 
                    <Sorters>
                        <ext:DataSorter Property="SUB_DIVISION" />

                    </Sorters>

                </ext:Store>
            </Store>

            <ColumnModel ID="ColumnModel1" runat="server">
                <Columns>
                    <ext:Column ID="uxSubDiv" runat="server" DataIndex="SUB_DIVISION" Text="Sub-Division" Flex="1" />                    
                    <ext:Column ID="Column1" runat="server" Text="MP" Flex="1" DataIndex="MILE_POST" />
                    <ext:Column ID="uxMainCrossingNum" runat="server" DataIndex="CROSSING_NUMBER" Text="DOT #" Flex="1" />
                    <ext:Column ID="Column2" runat="server" Text="State" Flex="1" DataIndex="STATE" /> 
                    <ext:DateColumn ID="DateColumn4" runat="server" DataIndex="APPLICATION_DATE" Text="Work Date" Flex="1" Format="MM/dd/yyyy" />                
                    <ext:Column ID="Column11" runat="server" Text="Spray" DataIndex="SPRAY" Flex="1"  />
                    <ext:Column ID="Column3" runat="server" Text="Cut" DataIndex="CUT" Flex="1"  />
                    <ext:Column ID="Column4" runat="server" Text="Inspect" DataIndex="INSPECT" Flex="1"  />                    

                </Columns>
            </ColumnModel>
         
            <Features>
                <ext:Grouping ID="Grouping1"
                    runat="server"
                    HideGroupedHeader="true" Collapsible="false"  Cls="x-grid-group-title; x-grid-group-hd" />
            </Features>
                     
            <TopBar>
                <ext:Toolbar ID="Toolbar2" runat="server">
                    <Items>
                       
                      <ext:Button ID="Button6" runat="server" Text="To XML" AutoPostBack="true" OnClick="ToXml" Icon="PageCode">
                            <Listeners>
                                <Click Fn="saveData" />
                            </Listeners>
                        </ext:Button>
                        
                        <ext:Button ID="Button7" runat="server" Text="To Excel" AutoPostBack="true" OnClick="ToExcel" Icon="PageExcel">
                            <Listeners>
                                <Click Fn="saveData" />
                            </Listeners>
                        </ext:Button>
                        
                        <ext:Button ID="Button8" runat="server" Text="To CSV" AutoPostBack="true" OnClick="ToCsv" Icon="PageAttach">
                            <Listeners>
                                <Click Fn="saveData" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <BottomBar>
                <ext:PagingToolbar ID="PagingToolbar1" runat="server" />
            </BottomBar>
        </ext:GridPanel>
                    </Items>
             </ext:Viewport>
    </div>
    </form>
</body>
</html>

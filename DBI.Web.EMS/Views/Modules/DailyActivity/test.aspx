<%@ Page Language="C#" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<script runat="server">
    public class Department
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string GetIdPlusName()
        {
            return this.ID + ": " + this.Name;
        }

        public static List<Department> GetAll()
        {
            return new List<Department>
            {
                new Department { ID = 1, Name = "Department A" },
                new Department { ID = 2, Name = "Department B" },
                new Department { ID = 3, Name = "Department C" }
            };
        }
    }

    public class Employee
    {
        public int ID { get; set; }
        public string GUID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Department { get; set; }
        public string[] Phone { get; set; }

        public string GetFullName()
        {
            return this.Name + " " + this.Surname;
        }

        public static List<Employee> GetAll()
        {
            return new List<Employee>
            {
                new Employee
                {
                    ID = 1,
                    Name = "Nancy",
                    Surname = "Davolio",
                    Department = 1,
                    Phone = new string[] { "555-555-555", "777-777-777" }
                },
                new Employee
                {
                    ID = 2,
                    Name = "Andrew",
                    Surname = "Fuller",
                    Department = 3,
                    Phone = new string[] { "333-333-333", "111-111-111" }
                }
            };
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Store store = this.GridPanel1.GetStore();

        store.DataSource = Employee.GetAll();
        Store2.DataSource = Department.GetAll();
    }

    protected void StoreValue(object sender, DirectEventArgs e)
    {
        DropDown1.SetValue(e.ExtraParams["ID"], e.ExtraParams["Name"]);
    }     
</script>

<!DOCTYPE html>

<html>
<head id="Head1" runat="server">
    <title>Server Mapping - Ext.NET Examples</title>
    <script type="text/javascript">
        var RenderDepartment = function (value) {
            var r = App.Store2.getById(value);
            if (Ext.isEmpty(r)) {
                return "";
            }

            return r.data.Name;
        };
    </script>
</head>
<body>
    <form id="Form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />

        <ext:Store runat="server" ID="Store2">
            <Model>
                <ext:Model ID="Model2" runat="server" IDProperty="ID">
                    <Fields>
                        <ext:ModelField Name="ID" />
                        <ext:ModelField Name="Name" />
                    </Fields>
                </ext:Model>
            </Model>
            <Listeners>
                <Load Handler="#{GridPanel1}.getView().refresh()" />
            </Listeners>
        </ext:Store>

        <ext:GridPanel
            ID="GridPanel1"
            runat="server"
            Title="List"
            Icon="Application">
            <Store>
                <ext:Store ID="Store1" runat="server">
                    <Model>
                        <ext:Model ID="Model1" runat="server">
                            <Fields>
                                <ext:ModelField Name="ID" Type="Int" />
                                <ext:ModelField Name="Name" />
                                <ext:ModelField Name="Surname" />
                                <ext:ModelField Name="Department" />
                                <ext:ModelField Name="PhoneHome" ServerMapping="Phone[0]" />
                                <ext:ModelField Name="PhoneWork" ServerMapping="Phone[1]" />
                            </Fields>
                        </ext:Model>
                    </Model>
                </ext:Store>
            </Store>
            <ColumnModel ID="ColumnModel1" runat="server">
                <Columns>
                    <ext:Column ID="Column1" runat="server" Text="ID" DataIndex="ID" />
                    <ext:Column ID="Column2" runat="server" Text="NAME" DataIndex="Name" />
                    <ext:Column ID="Column3" runat="server" Text="SURNAME" DataIndex="Surname" />
                    <ext:Column ID="Column5" runat="server" Text="DEPARTMENT" DataIndex="Department">
                        <Editor>
                            <ext:DropDownField runat="server" Mode="ValueText" ID="DropDown1">
                                <Component>
                                    <ext:GridPanel runat="server" ID="GridPanel2" StoreID="Store2">
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column runat="server" DataIndex="ID" Text="ID" />
                                                <ext:Column runat="server" DataIndex="Name" Text="Name" />
                                            </Columns>
                                        </ColumnModel>
                                        <DirectEvents>
                                            <SelectionChange OnEvent="StoreValue">
                                                <ExtraParams>
                                                    <ext:Parameter Name="ID" Value="#{GridPanel2}.getSelectionModel().getSelection()[0].data.ID" Mode="Raw" />
                                                    <ext:Parameter Name="Name" Value="#{GridPanel2}.getSelectionModel().getSelection()[0].data.Name" Mode="Raw" />
                                                </ExtraParams>
                                            </SelectionChange>
                                        </DirectEvents>
                                    </ext:GridPanel>
                                </Component>
                                <Listeners>
                                    <Expand Handler="this.picker.setWidth(250)" />
                                </Listeners>
                            </ext:DropDownField>
                        </Editor>
                        <Renderer Fn="RenderDepartment" />
                    </ext:Column>
                    <ext:Column ID="Column7" runat="server" Text="PHONE HOME" DataIndex="PhoneHome" />
                    <ext:Column ID="Column8" runat="server" Text="PHONE WORK" DataIndex="PhoneWork" />
                </Columns>
            </ColumnModel>
            <Plugins>
                <ext:RowEditing runat="server" ClicksToEdit="1" AutoCancel="false" />
            </Plugins>
        </ext:GridPanel>
    </form>
</body>
</html>

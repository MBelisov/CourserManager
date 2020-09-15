using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace CourserManager
{
    public partial class CourseViewer : Form
    {
        private SchoolEntities schoolContext;
        public CourseViewer()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Get the object for the selected department.
                Department department = new Department();
                department = (Department)this.departmentList.SelectedItem;

                // Bind the grid view to the collection of Course objects 
                // that are related to the selected Department object.
                courseGridView.DataSource = department.Courses;

                // Hide the columns that are bound to the navigation properties on Course.
                courseGridView.Columns["Department"].Visible = false;
                courseGridView.Columns["StudentGrades"].Visible = false;
                courseGridView.Columns["OnlineCourse"].Visible = false;
                courseGridView.Columns["OnsiteCourse"].Visible = false;
                courseGridView.Columns["People"].Visible = false;
                courseGridView.Columns["DepartmentId"].Visible = false;
                courseGridView.AllowUserToDeleteRows = false;
                courseGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
        }

        private void closeForm_Click(object sender, EventArgs e)
        {
            schoolContext.Dispose();
            this.Close();
        }

        private void CourseViewer_Load(object sender, EventArgs e)
        {
            // Initialize the ObjectContext.
            schoolContext = new SchoolEntities();

            // Define a query that returns all Department objects
            // and related Course objects, ordered by name.
            ObjectQuery<Department> departmentQuery = new ObjectQuery<Department>("From d In schoolContext.Departments.Include(Courses) Order By d.Name Select d", schoolContext);

            try
            {
                // Bind the ComboBox control to the query.
                // To prevent the query from being executed multiple times during binding, 
                // it is recommended to bind controls to the result of the Execute method. 
                this.departmentList.DisplayMember = "Name";
                this.departmentList.DataSource = departmentQuery.Execute(MergeOption.AppendOnly);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message)
            }
        }

        private void courseGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void saveChanges_Click(object sender, EventArgs e)
        {
            try
            {
                // Save object changes to the database, 
                // display a message, and refresh the form.
                schoolContext.SaveChanges();
                MessageBox.Show("Changes saved to the database.");
                this.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

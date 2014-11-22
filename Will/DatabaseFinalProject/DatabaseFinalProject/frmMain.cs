using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Data.OleDb;
using System.Collections;
using System.Configuration;


public partial class frmMain : Form
{
    [STAThread]
    public static void Main()
    {
        frmMain main = new frmMain();
        Application.Run(main);
    }

    public frmMain()
    {
        InitializeComponent();
    }

    private string mUserFile = null;
    private OleDbConnection mDB;
    private ArrayList mStudents = new ArrayList();

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
        OpenFileDialog ofd;
        try
        {
            ofd = new OpenFileDialog();
            ofd.Title = "Select the database files to open";
            ofd.Filter = "Database Files (*.accdb)|*.accdb|All Files (*.*)|*.*";
            ofd.InitialDirectory = Path.Combine(Application.StartupPath, @"\Databases");
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                mUserFile = ofd.FileName;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("There was an unexpected error: " + ex.Message);
        }
        LoadDBTable("SELECT * FROM StudentInfo;");
        displayData();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Close();
    }

    // The overloaded validateInput helper method handles the existence check for a given string input form object 
    // and assigns the equivalent value to its corresponding variable. 
    private bool validateInput(TextBox txtInput, out string userInput)
    {
        string fieldName;
        fieldName = txtInput.Name.Substring(3);
        userInput = "";
        if (txtInput.Text == "")
        {
            ShowMessage("Please enter a value for " + fieldName);
            txtInput.Focus();
            return false;
        }
        userInput = txtInput.Text;
        return true;
    }
    // The overloaded validateInput helper methods handle the existence check, type check, and range check for a given 
    // input form object and assigns the equivalent value to its corresponding variable. (This one handles int data.)
    private bool validateInput(TextBox txtInput, int min, int max, out int userInput)
    {
        string fieldName;
        fieldName = txtInput.Name.Substring(3);
        userInput = 0;
        if (txtInput.Text == "")
        {
            ShowMessage("Please enter a value for " + fieldName);
            txtInput.Focus();
            return false;
        }
        if (int.TryParse(txtInput.Text, out userInput) == false)
        {
            ShowMessage("Only numbers are allowed for " + fieldName + ". Please re-enter:");
            txtInput.Focus();
            return false;
        }
        if (userInput < min || userInput > max)
        {
            ShowMessage(fieldName + " must be between " + min.ToString() + " and " + max.ToString());
            txtInput.Focus();
            return false;
        }
        return true;
    }
    // The overloaded validateInput helper methods handle the existence check, type check, and range check for a given 
    // input form object and assigns the equivalent value to its corresponding variable. (This one handles DateTime data.)
    private bool validateInput(TextBox txtInput, DateTime min, DateTime max, out DateTime userInput)
    {
        string fieldName;
        fieldName = txtInput.Name.Substring(3);
        userInput = DateTime.Parse("01/01/1900");
        if (txtInput.Text == "")
        {
            ShowMessage("Please enter a date in the format mm/dd/yyyy for " + fieldName);
            txtInput.Focus();
            return false;
        }
        if (DateTime.TryParse(txtInput.Text, out userInput) == false)
        {
            ShowMessage("Only dates are allowed for " + fieldName + ". Please re-enter:");
            txtInput.Focus();
            return false;
        }
        if (userInput < min || userInput > max)
        {
            ShowMessage(fieldName + " must be between " + min.ToShortDateString() + " and " + max.ToShortDateString());
            txtInput.Focus();
            return false;
        }
        return true;
    }


    private void openDatabaseConnection()
    {
        string connectionString = ConfigurationManager.AppSettings["DBConnectionString"] + mUserFile;
        mDB = new OleDbConnection(connectionString);
    }

    private void closeDatabaseConnection()
    {
        if (mDB != null)
        {
            mDB.Close();
        }
    }

    private void LoadDBTable(string sql)
    {
        if (mUserFile == null)
        {
            ShowMessage("Please select a valid file.");
            return;
        }
        try
        {
            mStudents.Clear();
            openDatabaseConnection();
            mDB.Open();
            OleDbCommand cmd = new OleDbCommand(sql, mDB); ;
            OleDbDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read() == true)
            {
                clsStudent tempRecord = new clsStudent(
                    (int)rdr["StudentID"],
                    (string)rdr["LastName"],
                    (string)rdr["FirstName"],
                    (string)rdr["School"],
                    (string)rdr["City"],
                    (string)rdr["State"],
                    (int)rdr["Zip"]);
                    
                mStudents.Add(tempRecord);
            }
            rdr.Close();

            
        }
        catch (Exception ex)
        {
            ShowMessage("There was an unexpected error: " + ex);
        }
        finally
        {
            closeDatabaseConnection();
        }
    }

    private void displayData()
    {
        string outputLine;

        lstStudent.Items.Clear();

        outputLine = "StudentID".PadRight(10) + "   "
            + "Name".PadRight(18) + "   "
            + "School".PadRight(20) + " "
            + "City".PadRight(17) + " "
            + "State".PadRight(3) + "     "
            + "Zip".PadRight(7);
        lstStudent.Items.Add(outputLine);

        outputLine = "".PadRight(10, '=') + "   "
            + "".PadRight(18, '=') + "   "
            + "".PadRight(20, '=') + " "
            + "".PadRight(17, '=') + " "
            + "".PadRight(5, '=') + "   "
            + "".PadRight(7, '=');
        lstStudent.Items.Add(outputLine);

        //foreach (clsStudent tempContact in mStudents)
        //{
        //    lstStudent.Items.Add(tempContact.ShowStudent());
        //}
    }

    private void btnShow_Click(object sender, EventArgs e)
    {
        LoadDBTable("SELECT * FROM StudentInfo;");
        displayData();
    }

    

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void ShowMessage(string message)
    {
        MessageBox.Show(message, "Error");
    }

    
    private void clearUserInput()
    {
        txtStudentID.Clear();
        txtFirstName.Clear();
        txtLastName.Clear();
        txtSchool.Clear();
        txtCity.Clear();
        txtState.Clear();
        txtZip.Clear();

        
    }

    private void lstStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = lstStudent.SelectedIndex - 2;
        if (lstStudent.SelectedIndex == -1)
        {
            clearUserInput();
            return;
        }
        if (index < 0 || index >= mStudents.Count)
        {
            ShowMessage("Please select a valid student.");
            lstStudent.SelectedIndex = -1;
            return;
        }
            clsStudent temp = (clsStudent)mStudents[index];
            txtStudentID.Text = temp.UnivID.ToString();
            txtFirstName.Text = temp.FirstName;
            txtLastName.Text = temp.LastName;
            txtSchool.Text = temp.Street;
            txtCity.Text = temp.City;
            txtState.Text = temp.State;
            txtZip.Text = temp.Zip.ToString();
           

            try
            {
                openDatabaseConnection();
                mDB.Open();
                string sql = "SELECT University.ID, UniversityName "
                    + "FROM StudentInfo, StudentUniversity, University "
                    + "WHERE StudentInfo.UnivID = StudentUniversity.StudentID "
                    + "AND University.ID = studentUniversity.UniversityID "
                    + "AND StudentInfo.UnivID = " + clsSQL.ToSql(temp.UnivID) + ";";
                OleDbCommand cmd = new OleDbCommand(sql, mDB);
                OleDbDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                
                rdr.Close();
            }
            catch (Exception ex)
            {
                ShowMessage("There was an unexpected error: " + ex.Message);
            }
            finally
            {
                closeDatabaseConnection();
            }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        string sql;
        int univID;
        string firstname;
        string lastname;
        string street;
        string city;
        string state;
        int zip;
        

       
        if (validateInput(txtStudentID, 10000, 99999, out univID) == false)
        {
            return;
        }
        if (validateInput(txtFirstName, out firstname) == false)
        {
            return;
        }
        if (validateInput(txtLastName, out lastname) == false)
        {
            return;
        }
        if (validateInput(txtSchool, out street) == false)
        {
            return;
        }
        if (validateInput(txtCity, out city) == false)
        {
            return;
        }
        if (validateInput(txtState, out state) == false)
        {
            return;
        }
        if (validateInput(txtZip, 10000, 99999, out zip) == false)
        {
            return;
        }
        try
        {
            openDatabaseConnection();
            mDB.Open();
            OleDbCommand cmd;
            sql = "INSERT INTO StudentInfo (UnivID, Fname, Lname, Street, City, State, Zip, Birthdate) VALUES ("
             + clsSQL.ToSql(univID) + ", "
             + clsSQL.ToSql(firstname) + ", "
             + clsSQL.ToSql(lastname) + ", "
             + clsSQL.ToSql(street) + ", "
             + clsSQL.ToSql(city) + ", "
             + clsSQL.ToSql(state) + ", "
             + clsSQL.ToSql(zip) + ")";
            cmd = new OleDbCommand(sql, mDB);
            cmd.ExecuteNonQuery();

           
        }
        catch (Exception ex)
        {
            ShowMessage("There was an unexpected error: " + ex.Message);
        }
        finally
        {
            closeDatabaseConnection();
        }
        LoadDBTable("SELECT * FROM StudentInfo;");
        displayData();
        clearUserInput();
        

    }

    private void btnUpdate_Click(object sender, EventArgs e)
    {
        string sql;
        int univID;
        string firstname;
        string lastname;
        string street;
        string city;
        string state;
        int zip;
        

       
        if (validateInput(txtStudentID, 10000, 99999, out univID) == false)
        {
            return;
        }
        if (validateInput(txtFirstName, out firstname) == false)
        {
            return;
        }
        if (validateInput(txtLastName, out lastname) == false)
        {
            return;
        }
        if (validateInput(txtSchool, out street) == false)
        {
            return;
        }
        if (validateInput(txtCity, out city) == false)
        {
            return;
        }
        if (validateInput(txtState, out state) == false)
        {
            return;
        }
        if (validateInput(txtZip, 10000, 99999, out zip) == false)
        {
            return;
        }
        try
        {
            openDatabaseConnection();
            mDB.Open();
            OleDbCommand cmd;
            sql = "UPDATE StudentInfo SET "
                + "Fname = " + clsSQL.ToSql(firstname) + ", "
                + "Lname = " + clsSQL.ToSql(lastname) + ", "
                + "Street = " + clsSQL.ToSql(street) + ", "
                + "City = " + clsSQL.ToSql(city) + ", "
                + "State = " + clsSQL.ToSql(state) + ", "
                + "Zip = " + clsSQL.ToSql(zip) + ", "
                + "WHERE UnivID = " + clsSQL.ToSql(univID) + ";";
            cmd = new OleDbCommand(sql, mDB);
            cmd.ExecuteNonQuery();

            
        }
        catch (Exception ex)
        {
            ShowMessage("There was an unexpected error: " + ex.Message);
        }
        finally
        {
            closeDatabaseConnection();
        }
        LoadDBTable("SELECT * FROM StudentInfo;");
        displayData();
        clearUserInput();
       
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        string sql;

        if (lstStudent.SelectedIndex < 2)
        {
            ShowMessage("Please select a valid user to delete.");
            return;
        }
        if (MessageBox.Show("Please confirm the deletion", "Confirmation Needed",
            MessageBoxButtons.YesNo) == DialogResult.No)
        {
            ShowMessage("Delete request ignored.");
            lstStudent.SelectedIndex = -1;
            return;
        }
        try
        {
            openDatabaseConnection();
            mDB.Open();
            sql = "DELETE FROM StudentInfo WHERE UnivID = "
                + clsSQL.ToSql(int.Parse(txtStudentID.Text))
                + " AND Fname = " + clsSQL.ToSql(txtFirstName.Text) + ";";
            OleDbCommand cmd = new OleDbCommand(sql, mDB);
            cmd.ExecuteNonQuery();

            sql = "DELETE FROM StudentUniversity WHERE StudentID = "
                + clsSQL.ToSql(int.Parse(txtStudentID.Text)) + ";";
            cmd = new OleDbCommand(sql, mDB);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            ShowMessage("There was an unexpected error" + ex.Message);
        }
        finally
        {
            closeDatabaseConnection();
        }
        LoadDBTable("SELECT * FROM StudentInfo;");
        displayData();
        clearUserInput();


    }

  
}

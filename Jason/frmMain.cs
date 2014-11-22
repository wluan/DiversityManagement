using System;
using System.Windows.Forms;
using System.IO;                     
using System.Collections;            
using System.Data.OleDb;            
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

    // Class scope variables
    private ArrayList mClients = new ArrayList();
    private string mClientFile = "";
    private OleDbConnection mDB;

    // When the Exit button is clicked, terminate the program.
    private void btnExit_Click(object sender, EventArgs e)
    {
        Close();
    }

    // When the clear button is clicked, erase the input and output form objects and position the cursor in the first input field.
    private void btnClear_Click(object sender, EventArgs e)
    {
        eraseInputFields();
        lstSchools.Items.Clear();
        // erase all the data in the arrays
        mClients.Clear();
        txtFirstName.Focus();
    }

    // This helper method is used to open a connection to the database.
    private void openDatabaseConnection()
    {
        string connectionString =
            ConfigurationManager.AppSettings["DBConnectionString"] + mClientFile;
        mDB = new OleDbConnection(connectionString);
    }

    // This helper method releases the DB connection.
    private void closeDatabaseConnection()
    {
        if (mDB != null)
        {
            mDB.Close();
        }
    }

    // The validateInput helper methods handles the existence check, type check, and range check for a given input form
    // object and assigns the equivalent value to its corresponding variable.

    // Validation helper method for integer data types
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

    // Validation helper method for double data types
    private bool validateInput(TextBox txtInput, double min, double max, out double userInput)
    {
        string fieldName;
        fieldName = txtInput.Name.Substring(3);
        userInput = 0D;
        if (txtInput.Text == "")
        {
            ShowMessage("Please enter a value for " + fieldName);
            txtInput.Focus();
            return false;
        }
        if (double.TryParse(txtInput.Text, out userInput) == false)
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

    // Validation helper method for decimal data types
    private bool validateInput(TextBox txtInput, decimal min, decimal max, out decimal userInput)
    {
        string fieldName;
        fieldName = txtInput.Name.Substring(3);
        userInput = 0M;
        if (txtInput.Text == "")
        {
            ShowMessage("Please enter a value for " + fieldName);
            txtInput.Focus();
            return false;
        }
        if (decimal.TryParse(txtInput.Text, out userInput) == false)
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


    // The ShowMessage helper method displays an error message with a standard title and an OK button.
    private void ShowMessage(string msg)
    {
        MessageBox.Show(msg, "Problem found", MessageBoxButtons.OK);
    }

    // This helper method just produces the column headers in the listbox
    private void showHeaders()
    {
        string outputLine;
        // start with a blank listbox
        lstSchools.Items.Clear();

        // construct the column headers
        outputLine = "  CLIENT NAME   " + "  " + "AGE" + "  " + "HGT(in)" + "  " + "START WGT" + "  " + "START BMI" + "  " + "GOAL WGT" + "  " + "GOAL BMI" + "  " + "WKS";
        lstSchools.Items.Add(outputLine);
        outputLine = "================" + "  " + "===" + "  " + "=======" + "  " + "=========" + "  " + "=========" + "  " + "========" + "  " + "========" + "  " + "===";
        lstSchools.Items.Add(outputLine);

    }

    // This helper method displays the data in the arrays in the listbox.
    private void displayData()
    {
        // Define all variables.
        string outputLine;
        double avgStartBMI;
        double avgGoalBMI;
        double highStartBMI;
        double highGoalBMI;
        double lowStartBMI;
        double lowGoalBMI;

        // if there are no clients, display a message
        if (mClients.Count == 0)
        {
            ShowMessage("There are currently no clients. Please add clients.");
            txtFirstName.Focus();
            return;
        }

        showHeaders();

        // show the detailed client information
        foreach (clsClients client in mClients)
        {
            outputLine = client.ShowClient();
            lstSchools.Items.Add(outputLine);
        }

        // compute the BMI high, low, average stats 
        avgStartBMI = 0;
        avgGoalBMI = 0;
        highStartBMI = 0;  // Assign a ridiculously low value so any client value will be higher
        highGoalBMI = 0;   // Assign a ridiculously low value so any client value will be higher
        lowStartBMI = 100; // Assign a ridiculously high value so any client value will be lower
        lowGoalBMI = 100;  // Assign a ridiculously high value so any client value will be lower
        foreach (clsClients client in mClients)
        {
            avgStartBMI += client.StartBMI;
            avgGoalBMI += client.GoalBMI;
            if (highStartBMI < client.StartBMI)
            {
                highStartBMI = client.StartBMI;
            }
            if (highGoalBMI < client.GoalBMI)
            {
                highGoalBMI = client.GoalBMI;
            }
            if (lowStartBMI > client.StartBMI)
            {
                lowStartBMI = client.StartBMI;
            }
            if (lowGoalBMI > client.GoalBMI)
            {
                lowGoalBMI = client.GoalBMI;
            }
        }
        avgStartBMI /= mClients.Count;
        avgGoalBMI /= mClients.Count;

        // display the stats
            outputLine = "                                 " + "AVERAGE" + "    " +
                avgStartBMI.ToString("##.#0").PadLeft(5) + "               " +
                avgGoalBMI.ToString("##.#0").PadLeft(5) ;
            lstSchools.Items.Add(outputLine);
            outputLine = "                                 " + "HIGH   " + "    " +
                highStartBMI.ToString("##.#0").PadLeft(5) + "               " +
                highGoalBMI.ToString("##.#0").PadLeft(5);
            lstSchools.Items.Add(outputLine);
            outputLine = "                                 " + "LOW    " + "    " +
                lowStartBMI.ToString("##.#0").PadLeft(5) + "               " +
                lowGoalBMI.ToString("##.#0").PadLeft(5);
            lstSchools.Items.Add(outputLine);

    }

    // When Add is clicked, the input fileds are validated. If all are acceptable and there is room
    // to add clients, the client is added to the next available spot in the arrays.
    private void btnAdd_Click(object sender, EventArgs e)
    {
        // Define all variables.
        int age;
        double height;
        double startWeight;
        double goalWeight;
        int totalWeeks;
        int trainerID;
        int planID;
        string sql;

        // Validate the user's input.
        if (txtFirstName.Text == "")
        {
            ShowMessage("Please enter the client's first name");
            txtFirstName.Focus();
            return;
        }
        if (txtLastName.Text == "")
        {
            ShowMessage("Please enter the client's last name");
            txtLastName.Focus();
            return;
        }
        if (validateInput(txtAge, 16, 100, out age) == false)
        {
            return;
        }
        if (validateInput(txtHeight, 36.0, 80.5, out height) == false)
        {
            return;
        }
        if (validateInput(txtCurrentWeight, 60.0, 500.0, out startWeight) == false)
        {
            return;
        }
        if (validateInput(txtGoalWeight, 90.0, 300.0, out goalWeight) == false)
        {
            return;
        }
        if (validateInput(txtTotalWeeks, 1, 36, out totalWeeks) == false)
        {
            return;
        }
        if (cboTrainer.SelectedIndex < 0)
        {
            ShowMessage("Please assign a trainer");
            cboTrainer.Focus();
            return;
        }
        clsComboBoxItem trainer = (clsComboBoxItem)cboTrainer.SelectedItem;
        trainerID = (int)trainer.Value;
        if (cboExercisePlan.SelectedIndex < 0)
        {
            ShowMessage("Please assign an Exercise Plan");
            cboExercisePlan.Focus();
            return;
        }
        clsComboBoxItem plan = (clsComboBoxItem)cboExercisePlan.SelectedItem;
        planID = (int)plan.Value;

        // Data is valid and more clients can be added, so update the DB and then load the data rom the DB
        try
        {
            // Add a record into the Clients table.
            openDatabaseConnection();
            mDB.Open();
            OleDbCommand cmd;
            sql = "INSERT INTO Clients (FirstName, Lastname, Age, HeightInches, StartWeight, GoalWeight, Weeks) VALUES (" +
                clsSQL.ToSql(txtFirstName.Text) + ", " +
                clsSQL.ToSql(txtLastName.Text) + ", " +
                clsSQL.ToSql(age) + ", " +
                clsSQL.ToSql(height) + ", " +
                clsSQL.ToSql(startWeight) + ", " +
                clsSQL.ToSql(goalWeight) + ", " +
                clsSQL.ToSql(totalWeeks) + ")";
            cmd = new OleDbCommand(sql, mDB);
            cmd.ExecuteNonQuery();

            // Lookup ID of client record just added to get ID.
            sql = "SELECT * FROM Clients WHERE FirstName = " + clsSQL.ToSql(txtFirstName.Text) +
                " AND LastName = " + clsSQL.ToSql(txtLastName.Text);
            OleDbDataReader rdr;
            cmd = new OleDbCommand(sql, mDB);
            rdr = cmd.ExecuteReader();
            rdr.Read();
            int clientID = (int)rdr["ID"];
            rdr.Close();

            // Insert record into ClientPlan table.
            sql = "INSERT INTO ClientPlan (ClientID, PlanID) VALUES (" +
                clsSQL.ToSql(clientID) + ", " +
                clsSQL.ToSql(planID) + ")";
            cmd = new OleDbCommand(sql, mDB);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            ShowMessage("There was an unexpected problem: " + ex.Message);
        }
        finally
        {
            closeDatabaseConnection();
        }

        // Erase the input values and display the current client roster
        eraseInputFields();
        loadClients("SELECT * FROM Clients");
        displayData();
    }

    // This helper method erases the input textboxes and positions the cursor in the first textbox.
    private void eraseInputFields()
    {
        txtFirstName.Text = "";
        txtLastName.Text = "";
        txtAge.Text = "";
        txtHeight.Text = "";
        txtCurrentWeight.Text = "";
        txtGoalWeight.Text = "";
        txtTotalWeeks.Text = "";
        txtFirstName.Focus();
    }

    // Revise the selected items in the two combo boxes when a name is selected.
    private void lstClients_Click(object sender, EventArgs e)
    {
        ArrayList planID = new ArrayList();
        int selectedClient = lstSchools.SelectedIndex - 2;

        // Make sure a valid client was selected.
        if (selectedClient < 0 || selectedClient >= mClients.Count)
        {
            ShowMessage("Please select a valid client in the listbox.");
            lstSchools.SelectedIndex = -1;
            return;
        }
        // Get the ID of the selected client to fill in the input boxes
        clsClients temp = (clsClients)mClients[selectedClient];
        txtFirstName.Text = temp.FirstName;
        txtLastName.Text = temp.LastName;
        txtAge.Text = temp.Age.ToString();
        txtHeight.Text = temp.Height.ToString();
        txtCurrentWeight.Text = temp.StartWeight.ToString();
        txtGoalWeight.Text = temp.GoalWeight.ToString();
        txtTotalWeeks.Text = temp.Weeks.ToString();

        // Search for the matching plan of the selected client
        string sql = "SELECT * FROM ClientPlan WHERE ClientID=" + clsSQL.ToSql(temp.ID);
        try
        {
            openDatabaseConnection();
            mDB.Open();
            OleDbCommand cmd = new OleDbCommand(sql, mDB);
            OleDbDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read() == true)
            {
                planID.Add(rdr["PlanID"]);
            }
            rdr.Close();
        }
        catch (Exception ex)
        {
            ShowMessage("There was an unexpected problem when adding the client: " + ex.Message + ex.StackTrace);
        }
        finally
        {
            closeDatabaseConnection();
        }

        // Rebuild the trainer and exercise combo boxes, selecting one plan/trainer for the client.
        buildExerciseCombo();
        buildTrainerCombo();
        for (int i = 0; i < cboExercisePlan.Items.Count; i++)
        {
            clsComboBoxItem item = (clsComboBoxItem)cboExercisePlan.Items[i];
            if (planID.Contains(item.Value))
            {
                cboExercisePlan.SelectedIndex = i;
                break;
            }
        }
        // Make sure no trainer is selected.
        cboTrainer.SelectedIndex = -1;

        // Notify the user of the possible actions 
        ShowMessage("To delete " + temp.FirstName + " " + temp.LastName + " as a client, press the delete key. \n" +
            "To modify " + temp.FirstName + "'s data, edit the data in the input fields, select a trainer, select an exercise plan, and click Update.");
    }

    // This method removes the selected client from the roster
    private void btnDelete_Click(object sender, EventArgs e)
    {
        string sql;
        int selectedClient = lstSchools.SelectedIndex - 2;

        // Make sure a valid client was selected.
        if (selectedClient < 0 || selectedClient >= mClients.Count)
        {
            ShowMessage("Please select a valid client in the listbox.");
            lstSchools.SelectedIndex = -1;
            return;
        }

        // Get the ID of the selected client to fill in the input boxes
        clsClients temp = (clsClients)mClients[selectedClient];

        // Delete the selected client and remove the corresponding record in the ClientPlan table
        try
        {
            openDatabaseConnection();
            mDB.Open();
            OleDbCommand cmd;
            sql = "DELETE FROM ClientPlan WHERE ClientID = " + clsSQL.ToSql(temp.ID);
            cmd = new OleDbCommand(sql, mDB);
            cmd.ExecuteNonQuery();

            sql = "DELETE FROM Clients WHERE ID = " + clsSQL.ToSql(temp.ID);
            cmd = new OleDbCommand(sql, mDB);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            ShowMessage("There was an unexpected problem when deleting the client: " + ex.Message);
        }
        finally
        {
            closeDatabaseConnection();
        }

        // Erase the input values, notify the user, and display the current client roster
        eraseInputFields();
        ShowMessage(temp.FirstName + "'s record has been deleted.");
        loadClients("SELECT * FROM Clients");
        displayData();


    }

    // When the Update button is clicked, the user is able to modify the data
    // for the selected client.
    private void btnUpdate_Click(object sender, EventArgs e)
    {
        // Define all variables.
        int age;
        double height;
        double startWeight;
        double goalWeight;
        int totalWeeks;
        int trainerID;
        int planID;
        string sql;

        int selectedClient = lstSchools.SelectedIndex - 2;

        // Make sure a valid client was selected.
        if (selectedClient < 0 || selectedClient >= mClients.Count)
        {
            ShowMessage("Please select a valid client in the listbox.");
            lstSchools.SelectedIndex = -1;
            return;
        }

        // Get the ID of the selected client to fill in the input boxes
        clsClients temp = (clsClients)mClients[selectedClient];

        // Validate the user's input.
        if (txtFirstName.Text == "")
        {
            ShowMessage("Please enter the client's first name");
            txtFirstName.Focus();
            return;
        }
        if (txtLastName.Text == "")
        {
            ShowMessage("Please enter the client's last name");
            txtLastName.Focus();
            return;
        }
        if (validateInput(txtAge, 16, 100, out age) == false)
        {
            return;
        }
        if (validateInput(txtHeight, 36.0, 80.5, out height) == false)
        {
            return;
        }
        if (validateInput(txtCurrentWeight, 60.0, 500.0, out startWeight) == false)
        {
            return;
        }
        if (validateInput(txtGoalWeight, 90.0, 300.0, out goalWeight) == false)
        {
            return;
        }
        if (validateInput(txtTotalWeeks, 1, 36, out totalWeeks) == false)
        {
            return;
        }
        if (cboTrainer.SelectedIndex < 0)
        {
            ShowMessage("Please assign a trainer");
            cboTrainer.Focus();
            return;
        }
        clsComboBoxItem trainer = (clsComboBoxItem)cboTrainer.SelectedItem;
        trainerID = (int)trainer.Value;
        if (cboExercisePlan.SelectedIndex < 0)
        {
            ShowMessage("Please assign an Exercise Plan");
            cboExercisePlan.Focus();
            return;
        }
        clsComboBoxItem plan = (clsComboBoxItem)cboExercisePlan.SelectedItem;
        planID = (int)plan.Value;

        // Data is valid so modify the client and reload the data from the DB
        try
        {
            // Change a record in the Clients table.
            openDatabaseConnection();
            mDB.Open();
            OleDbCommand cmd;
            sql = "UPDATE Clients SET FirstName = " + clsSQL.ToSql(txtFirstName.Text) + 
                ", Lastname = " + clsSQL.ToSql(txtLastName.Text) + 
                ", Age = " + clsSQL.ToSql(age) + 
                ", HeightInches = " + clsSQL.ToSql(height) + 
                ", StartWeight = " + clsSQL.ToSql(startWeight) + 
                ", GoalWeight = " + clsSQL.ToSql(goalWeight) + 
                ", Weeks = " + clsSQL.ToSql(totalWeeks) +
                " WHERE ID = " + clsSQL.ToSql(temp.ID);
            cmd = new OleDbCommand(sql, mDB);
            cmd.ExecuteNonQuery();

            // If the client-plan already exists, ignore. Otherwise, add it.

            sql = "SELECT * FROM  ClientPlan WHERE ClientID = " + clsSQL.ToSql(temp.ID) + " AND PlanID = " + clsSQL.ToSql(planID);
            cmd = new OleDbCommand(sql, mDB);
            OleDbDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() == true) // no need to add a record
            {
                rdr.Close();
            }
            else // need to add the record
            {
                rdr.Close();
                sql = "INSERT INTO ClientPlan (ClientID, PlanID) VALUES (" +
                    clsSQL.ToSql(temp.ID) + ", " +
                    clsSQL.ToSql(planID) + ")";
                cmd = new OleDbCommand(sql, mDB);
                cmd.ExecuteNonQuery();
            }

        }
        catch (Exception ex)
        {
            ShowMessage("There was an unexpected problem: " + ex.Message);
        }
        finally
        {
            closeDatabaseConnection();
        }

        // Erase the input values, notify the user, and display the current client roster
        eraseInputFields();
        ShowMessage(temp.FirstName + "'s record has been updated.");
        loadClients("SELECT * FROM Clients");
        displayData();

    }

    // When File-Open is clicked, the user is able to select the client file to open.
    private void mnuFileOpen_Click(object sender, EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Title = "Select Client DB file to open";
        ofd.Filter = "Client (*.accdb)|*.accdb|All files (*.*)|*.*";
        ofd.InitialDirectory = Path.Combine(Application.StartupPath, @"Databases");

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            // Assign the filename
            mClientFile = ofd.FileName;
        }
        loadClients("SELECT * FROM Clients");

        // Build the combo boxes
        buildTrainerCombo();

        displayData();
    }

    // The loadClients helper method reads the data from the specified file and copies
    // to the client array.
    private void loadClients(string sql)
    {
        clsClients client;
        // Clear out the array before handling the file data.
        mClients.Clear();

        // Read the data from the specified file.

        if (File.Exists(mClientFile) == false)
        {
            ShowMessage(mClientFile + " does not exist. Please open another DB file.");
            return;
        }
        openDatabaseConnection();
        mDB.Open();
        OleDbCommand cmd;
        OleDbDataReader rdr;
        try
        {
            cmd = new OleDbCommand(sql, mDB);
            rdr = cmd.ExecuteReader();
            while (rdr.Read() == true)
            {
                // Add the data from the line just read to the next array element, making sure to get the ID.
                client = new clsClients((int)rdr["ID"], 
                    (string)rdr["FirstName"],
                    (string)rdr["LastName"],
                    (int)rdr["Age"],
                    (double)rdr["HeightInches"],
                    (double)rdr["StartWeight"],
                    (double)rdr["GoalWeight"],
                    (int)rdr["Weeks"]);
                mClients.Add(client);
            }
            rdr.Close();
        }
        catch (Exception ex)
        {
            ShowMessage("There was an unexpected problem: " + ex.Message);
        }
        finally
        {
            closeDatabaseConnection();
        }
    }

    // The buildTrainerCombo helper method reads the data from the Trainers DB table and copies
    // the available trainers to the appropriate combo box.
    private void buildTrainerCombo()
    {
        string sql;
        int selectedPlan = cboExercisePlan.SelectedIndex;
        if (selectedPlan >= 0)
        {
            sql = "SELECT DISTINCT Trainers.ID, FirstName, LastName FROM Trainers, TrainerPlan " +
                "WHERE Trainers.ID=TrainerPlan.TrainerID AND TrainerPlan.PlanID=" + clsSQL.ToSql(selectedPlan) + 
                " ORDER BY LastName, FirstName";
        }
        else
        {
            sql = "SELECT * FROM Trainers ORDER BY LastName, FirstName";
        }
        
        // Clear out the combo box first
        cboTrainer.Items.Clear();

        // Read the data from the specified file.

        if (File.Exists(mClientFile) == false)
        {
            ShowMessage(mClientFile + " does not exist. Please open another DB file.");
            return;
        }
        openDatabaseConnection();
        mDB.Open();
        OleDbCommand cmd;
        OleDbDataReader rdr;
        try
        {
            cmd = new OleDbCommand(sql, mDB);
            rdr = cmd.ExecuteReader();
            while (rdr.Read() == true)
            {
                // Add the data from the line just read to the next array element.
                clsComboBoxItem item = new clsComboBoxItem();
                item.Text = (string)rdr["FirstName"] + " " + (string)rdr["LastName"];
                item.Value = (int)rdr["ID"];
                cboTrainer.Items.Add(item);
            }
            rdr.Close();
        }
        catch (Exception ex)
        {
            ShowMessage("There was an unexpected problem: " + ex.Message);
        }
        finally
        {
            closeDatabaseConnection();
        }
    }

    // The buildExerciseCombo helper method reads the data from the FitnessPlans DB table and copies
    // the available plans to the appropriate combo box.
    private void buildExerciseCombo()
    {
        string sql;

        // If a trainer was selected, show only the plans for that trainer.
        if (cboTrainer.SelectedIndex >= 0)
        {
            clsComboBoxItem trainer = (clsComboBoxItem) cboTrainer.SelectedItem;
            int trainerID = (int)trainer.Value;
            sql = "SELECT DISTINCT FitnessPlans.ID, PlanName, ActivityTime FROM TrainerPlan, FitnessPlans WHERE " +
                "TrainerPlan.TrainerID = " + clsSQL.ToSql(trainerID) + " AND PlanID = FitnessPlans.ID ORDER BY PlanName";
        }
        else
        {
            sql = "SELECT * FROM FitnessPlans ORDER BY PlanName";
        }

        // Clear out the combo box first
        cboExercisePlan.Items.Clear();

        // Read the data from the specified file.

        if (File.Exists(mClientFile) == false)
        {
            ShowMessage(mClientFile + " does not exist. Please open another DB file.");
            return;
        }
        openDatabaseConnection();
        mDB.Open();
        OleDbCommand cmd;
        OleDbDataReader rdr;
        try
        {
            cmd = new OleDbCommand(sql, mDB);
            rdr = cmd.ExecuteReader();
            while (rdr.Read() == true)
            {
                // Add the data from the line just read to the next array element.
                clsComboBoxItem item = new clsComboBoxItem();
                item.Text = (string)rdr["PlanName"];
                item.Value = (int)rdr["ID"];
                cboExercisePlan.Items.Add(item);
            }
            rdr.Close();
        }
        catch (Exception ex)
        {
            ShowMessage("There was an unexpected problem: " + ex.Message);
        }
        finally
        {
            closeDatabaseConnection();
        }
    }

    // Search for the selected client.
    private void btnFind_Click(object sender, EventArgs e)
    {
        // Validate the user's input.
        if ((txtFirstName.Text == "") && (txtLastName.Text == "") && (txtTotalWeeks.Text == ""))
        {
            ShowMessage("Please enter the client's first name, last name, or total weeks to train to perform a search");
            txtFirstName.Focus();
            return;
        }

        // If both first and last names are given, sort the names and then do a binary search.
        if ((txtFirstName.Text != "") && (txtLastName.Text != ""))
        {
            loadClients("SELECT * FROM Clients WHERE FirstName=" + clsSQL.ToSql(txtFirstName.Text) + " AND LastName=" + clsSQL.ToSql(txtLastName.Text));
            displayData();
        }
        else if (txtLastName.Text != "")
        {
            loadClients("SELECT * FROM Clients WHERE LastName=" + clsSQL.ToSql(txtLastName.Text));
            displayData();
        }
        else if (txtFirstName.Text != "")
        {
            loadClients("SELECT * FROM Clients WHERE FirstName=" + clsSQL.ToSql(txtFirstName.Text));
            displayData();
        }
        else // search is for weeks
        {
            loadClients("SELECT * FROM Clients WHERE Weeks=" + clsSQL.ToSql(int.Parse(txtTotalWeeks.Text)));
            displayData();
        }

    }

    // Terminate the program when File-Exit is selected.
    private void mnuFileExit_Click(object sender, EventArgs e)
    {
        Close();
    }

    // Build the appropriate exercises for the selected trainer.
    private void cboTrainer_SelectedIndexChanged(object sender, EventArgs e)
    {
        buildExerciseCombo();
    }

    // When View-Lastname is clicked, the data appears sorted by lastname
    private void mnuViewLastName_Click(object sender, EventArgs e)
    {
        clsSortLName comparer = new clsSortLName();
        mClients.Sort(comparer);
        displayData();
    }

    // When View-Firstname is clicked, the data appears sorted by firstname
    private void mnuViewFirstName_Click(object sender, EventArgs e)
    {
        clsSortFName comparer = new clsSortFName();
        mClients.Sort(comparer);
        displayData();
    }

    // When View-StartWeight is clicked, the data appears sorted by starting weight
    private void mnuViewStartWeight_Click(object sender, EventArgs e)
    {
        clsSortStartWeight comparer = new clsSortStartWeight();
        mClients.Sort(comparer);
        displayData();
    }

    // When View-GoalWeight is clicked, the data appears sorted by goal weight
    private void mnuViewGoalWeight_Click(object sender, EventArgs e)
    {
        clsSortGoalWeight comparer = new clsSortGoalWeight();
        mClients.Sort(comparer);
        displayData();
    }

    // When View-Weeks is clicked, the data appears sorted by weeks to train
    private void mnuViewWeeks_Click(object sender, EventArgs e)
    {
        clsSortWeeks comparer = new clsSortWeeks();
        mClients.Sort(comparer);
        displayData();

    }

    private void frmMain_Load(object sender, EventArgs e)
    {

    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

}
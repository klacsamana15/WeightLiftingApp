using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class WeightLiftingForm : Form
    {
        private ListViewColumnSorter lvwColumnSorter;

        public WeightLiftingForm()
        {
            InitializeComponent();
            // Create an instance of a ListView column sorter and assign it
            // to the ListView control.
            lvwColumnSorter = new ListViewColumnSorter();
            this.lvOrdered.ListViewItemSorter = lvwColumnSorter;
        }

        //search for specific athlete by providing their id and last name
        private void btnSearch_Click(object sender, EventArgs e)
        {
            int searchID = int.Parse(tbSearchID.Text);
            string searchLastName = tbSearchLName.Text;

            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-E2FEQ2B\\SQLEXPRESS;Initial Catalog=Weightlifting;Integrated Security=True");
            conn.Open();

            SqlCommand cmdSearch = new SqlCommand(" select * from Athlete where Id = @Id AND Last_name = @Last_name", conn);
            cmdSearch.Parameters.AddWithValue("@Id", searchID);
            cmdSearch.Parameters.AddWithValue("@Last_name", searchLastName);

            cmdSearch.ExecuteNonQuery();

            SqlDataReader dr = cmdSearch.ExecuteReader();

            dr.Read();

            //make sure that id and last name entered by user matches with data in sql, if it doesn't it's null 
            //transfer data from sql to form
            if(dr.HasRows)
            {
                    tbModifyID.Text = dr[0].ToString();
                    tbModifyID.ForeColor = Color.Black;
                    tbModifyID.Enabled = false;

                    tbModifyFName.Text = dr[1].ToString();
                    tbModifyFName.ForeColor = Color.Black;

                    tbModifyLName.Text = dr[2].ToString();
                    tbModifyLName.ForeColor = Color.Black;

                    cbModifyCountry.Text = dr[3].ToString();
                    cbModifyCountry.ForeColor = Color.Black;

                    tbModifyWeight1.Text = dr[4].ToString();
                    tbModifyWeight1.ForeColor = Color.Black;

                    tbModifyWeight2.Text = dr[5].ToString();
                    tbModifyWeight2.ForeColor = Color.Black;

                    tbModifyWeight3.Text = dr[6].ToString();
                    tbModifyWeight3.ForeColor = Color.Black;
                }
            else
            {
                MessageBox.Show("Athlete ID: " + searchID + "\nAthlete Last Name: " + searchLastName + "\nAthlete not found in system.", "ERROR");
            }

            dr.Close();
            conn.Close();

            tbSearchID.Text = "Enter ID";
            tbSearchID.ForeColor = Color.Silver;
            tbSearchLName.Text = "Enter Last Name";
            tbSearchLName.ForeColor = Color.Silver;

        }

        //functions purely for aesthetics
        private void tbID_Enter(object sender, EventArgs e)
        {
            if (tbID.Text == "ID")
            {
                tbID.Text = "";
                tbID.ForeColor = Color.Black;
            }
        }

        private void tbID_Leave(object sender, EventArgs e)
        {
            if (tbID.Text == "")
            {
                tbID.Text = "ID";
                tbID.ForeColor = Color.Silver;
            }
        }

        private void tbFirstName_Enter(object sender, EventArgs e)
        {
            if (tbFirstName.Text == "First Name")
            {
                tbFirstName.Text = "";
                tbFirstName.ForeColor = Color.Black;
            }
        }

        private void tbFirstName_Leave(object sender, EventArgs e)
        {
            if (tbFirstName.Text == "")
            {
                tbFirstName.Text = "First Name";
                tbFirstName.ForeColor = Color.Silver;
            }
        }

        private void tbLastName_Enter(object sender, EventArgs e)
        {
            if (tbLastName.Text == "Last Name")
            {
                tbLastName.Text = "";
                tbLastName.ForeColor = Color.Black;
            }
        }

        private void tbLastName_Leave(object sender, EventArgs e)
        {
            if (tbLastName.Text == "")
            {
                tbLastName.Text = "Last Name";
                tbLastName.ForeColor = Color.Silver;
            }
        }

        private void cbCountry_Enter(object sender, EventArgs e)
        {
            if (cbCountry.Text != "")
            {
                cbCountry.Text = "";
                cbCountry.ForeColor = Color.Black;
            }
        }

        private void cbCountry_Leave(object sender, EventArgs e)
        {
            if (cbCountry.Text == "")
            {
                cbCountry.Text = "Country";
                cbCountry.ForeColor = Color.Silver;
            }
        }

        private void tbWeight_Enter(object sender, EventArgs e)
        {
            if (tbWeight.Text == "Weight")
            {
                tbWeight.Text = "";
                tbWeight.ForeColor = Color.Black;
            }
        }

        private void tbWeight_Leave(object sender, EventArgs e)
        {
            if (tbWeight.Text == "")
            {
                tbWeight.Text = "Last Name";
                tbWeight.ForeColor = Color.Silver;
            }
        }

        //add athlete by respecting all the necessary requirements
        private void btnAdd_Click(object sender, EventArgs e)
        {

            if ((tbFirstName.Text == "First Name") || (tbLastName.Text == "Last Name") || (tbWeight.Text == "Weight") || (cbCountry.SelectedIndex == -1))
            {
                MessageBox.Show("Please fill in required fields.", "Attention", MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            }
            else
            {
                //name format: no space, and string length > 2
                if ((Regex.IsMatch(tbFirstName.Text, @"^[a-zA-Z]+$") == false) || (tbFirstName.Text.Length > 2 == false))
                {
                    MessageBox.Show("First name must not contain spaces and must be longer than 2 characters.", "First name required.", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                    return;
                }
                //name format: no space, and string length > 2
                if ((Regex.IsMatch(tbLastName.Text, @"^[a-zA-Z]+$") == false) || (tbLastName.Text.Length > 2 == false))
                {
                    MessageBox.Show("Last name must not contain spaces and must be longer than 2 characters.", "Last name required.", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                    return;
                }

                //weight format: must be 2 or 3 digits
                if (Regex.IsMatch(tbWeight.Text, @"^([0-9]{2}|[0-9]{3})$") == false)
                {
                    MessageBox.Show("Please enter a valid weight", "Weight required", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                    return;
                }

                SqlConnection conn = new SqlConnection("Data Source=DESKTOP-E2FEQ2B\\SQLEXPRESS;Initial Catalog=Weightlifting;Integrated Security=True");
                conn.Open();

                SqlCommand cmd = new SqlCommand("insert into Athlete values (@Id, @First_name, @Last_name, @Country, @Weight1, @Weight2, @Weight3)", conn);

                cmd.Parameters.AddWithValue("@Id", int.Parse(tbID.Text));
                cmd.Parameters.AddWithValue("@First_name", tbFirstName.Text);
                cmd.Parameters.AddWithValue("@Last_name", tbLastName.Text);
                cmd.Parameters.AddWithValue("@Country", cbCountry.Text);
                cmd.Parameters.AddWithValue("@Weight1", int.Parse(tbWeight.Text));
                cmd.Parameters.AddWithValue("@Weight2", "0");
                cmd.Parameters.AddWithValue("@Weight3", "0");

                cmd.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show("Athlete added successfully !", "CONFIRMATION");

                //add data entered by user straight to dgvAthlete
                string[] row = { tbID.Text, tbFirstName.Text, tbLastName.Text, cbCountry.Text, tbWeight.Text };
                dgvAthlete.Rows.Add(row);
            }
        }

        //modify athlete's info entered in corresponding text fields
        private void btnModify_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-E2FEQ2B\\SQLEXPRESS;Initial Catalog=Weightlifting;Integrated Security=True");
            conn.Open();

            SqlCommand cmdSearch = new SqlCommand(" update Athlete set First_name = @First_name, Last_name = @Last_name, Country = @Country, Weight1 = @Weight1, Weight2 = @Weight2, Weight3 = @Weight3  where Id=@Id", conn);

            cmdSearch.Parameters.AddWithValue("@Id", int.Parse(tbModifyID.Text));
            cmdSearch.Parameters.AddWithValue("@First_name", tbModifyFName.Text);
            cmdSearch.Parameters.AddWithValue("@Last_name", tbModifyLName.Text);
            cmdSearch.Parameters.AddWithValue("@Country", cbModifyCountry.Text);
            cmdSearch.Parameters.AddWithValue("@Weight1", int.Parse(tbModifyWeight1.Text));
            cmdSearch.Parameters.AddWithValue("@Weight2", int.Parse(tbModifyWeight2.Text));
            cmdSearch.Parameters.AddWithValue("@Weight3", int.Parse(tbModifyWeight3.Text));

            cmdSearch.ExecuteNonQuery();
            
            //aesthetic code
            tbModifyID.Text = "ID";
            tbModifyID.ForeColor = Color.Silver;
            tbModifyFName.Text = "First Name";
            tbModifyFName.ForeColor = Color.Silver;
            tbModifyLName.Text = "Last Name";
            tbModifyLName.ForeColor = Color.Silver;
            cbModifyCountry.Text = "Country";
            cbModifyCountry.ForeColor = Color.Silver;
            tbModifyWeight1.Text = "Weight1";
            tbModifyWeight1.ForeColor = Color.Silver;
            tbModifyWeight2.Text = "Weight2";
            tbModifyWeight2.ForeColor = Color.Silver;
            tbModifyWeight3.Text = "Weight3";
            tbModifyWeight3.ForeColor = Color.Silver;

            conn.Close();

            MessageBox.Show("Athlete updated successfully!", "CONFIRMATION");

            //clear listview once athlete modified to reset interface and perform a new task
            if (lvAthletes.Items != null)
            {
                lvAthletes.Items.Clear();
                lvAthletes.Columns.Clear();
                lvAthletes.Clear();
            }

        }

        //aesthetic
        private void tbSearchID_Enter(object sender, EventArgs e)
        {
            if (tbSearchID.Text == "Enter ID")
            {
                tbSearchID.Text = "";
                tbSearchID.ForeColor = Color.Black;
            }
        }

        private void tbSearchID_Leave(object sender, EventArgs e)
        {
            if (tbSearchID.Text == "")
            {
                tbSearchID.Text = "Enter ID";
                tbSearchID.ForeColor = Color.Silver;
            }
        }

        private void tbSearchLName_Enter(object sender, EventArgs e)
        {
            if (tbSearchLName.Text == "Enter Last Name")
            {
                tbSearchLName.Text = "";
                tbSearchLName.ForeColor = Color.Black;
            }
        }

        private void tbSearchLName_Leave(object sender, EventArgs e)
        {
            if (tbSearchLName.Text == "")
            {
                tbSearchLName.Text = "Enter Last Name";
                tbSearchLName.ForeColor = Color.Silver;
            }
        }

        //delete athlete that was being searched in search group box or clicked from listview
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int deleteID = int.Parse(tbModifyID.Text);

            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-E2FEQ2B\\SQLEXPRESS;Initial Catalog=Weightlifting;Integrated Security=True");
            conn.Open();

            SqlCommand cmdSearch = new SqlCommand(" delete from Athlete where Id = @Id ", conn);

            cmdSearch.Parameters.AddWithValue("@Id", (deleteID));
            cmdSearch.ExecuteNonQuery();

            SqlDataReader dr = cmdSearch.ExecuteReader();

            tbModifyID.Text = "ID";
            tbModifyID.ForeColor = Color.Silver;
            tbModifyFName.Text = "First Name";
            tbModifyFName.ForeColor = Color.Silver;
            tbModifyLName.Text = "Last Name";
            tbModifyLName.ForeColor = Color.Silver;
            cbModifyCountry.Text = "Country";
            cbModifyCountry.ForeColor = Color.Silver;
            tbModifyWeight1.Text = "Weight1";
            tbModifyWeight1.ForeColor = Color.Silver;
            tbModifyWeight2.Text = "Weight2";
            tbModifyWeight2.ForeColor = Color.Silver;
            tbModifyWeight3.Text = "Weight3";
            tbModifyWeight3.ForeColor = Color.Silver;

            conn.Close();

            MessageBox.Show("Athlete deleted successfully!", "CONFIRMATION");

            //clear listview once athlete deleted to reset interface and perform a new task
            if (lvAthletes.Items != null)
            {
                lvAthletes.Items.Clear();
                lvAthletes.Columns.Clear();
                lvAthletes.Clear();
            }

        }

        //code that allows user to click on athlete's id to transfer their info in appropriate text fields
        private void lvAthletes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int chosen;

            if (lvAthletes.SelectedItems.Count > 0)
            {
                chosen = lvAthletes.SelectedIndices[0];
                tbModifyID.Text = lvAthletes.Items[chosen].SubItems[0].Text;
                tbModifyID.Enabled = false;
                tbModifyFName.Text = lvAthletes.Items[chosen].SubItems[1].Text;
                tbModifyLName.Text = lvAthletes.Items[chosen].SubItems[2].Text;
                cbModifyCountry.Text = lvAthletes.Items[chosen].SubItems[3].Text;
                cbModifyCountry.ForeColor = Color.Black;
                tbModifyWeight1.Text = lvAthletes.Items[chosen].SubItems[4].Text;
                tbModifyWeight2.Text = lvAthletes.Items[chosen].SubItems[5].Text;
                tbModifyWeight3.Text = lvAthletes.Items[chosen].SubItems[6].Text;
            }

        }

        //displays list of registered athletes and recent updates
        private void btnViewList_Click(object sender, EventArgs e)
        {
            if (lvAthletes.Items != null)
            {
                lvAthletes.Items.Clear();
                lvAthletes.Columns.Clear();
                lvAthletes.Clear();
            }

            lvAthletes.View = View.Details;
            lvAthletes.Columns.Add("ID");
            lvAthletes.Columns.Add("First Name");
            lvAthletes.Columns.Add("Last Name");
            lvAthletes.Columns.Add("Country");
            lvAthletes.Columns.Add("Weight1");
            lvAthletes.Columns.Add("Weight2");
            lvAthletes.Columns.Add("Weight3");

            for (int i = 0; i < lvAthletes.Columns.Count; i++)
            {
                lvAthletes.Columns[0].Width = 80;
                lvAthletes.Columns[1].Width = 80;
                lvAthletes.Columns[2].Width = 80;
                lvAthletes.Columns[3].Width = 80;
                lvAthletes.Columns[4].Width = 80;
                lvAthletes.Columns[5].Width = 80;
                lvAthletes.Columns[6].Width = 80;

            }

            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-E2FEQ2B\\SQLEXPRESS;Initial Catalog=Weightlifting;Integrated Security=True");
            conn.Open();

            SqlCommand cmd = new SqlCommand("select * from Athlete", conn);
            cmd.ExecuteNonQuery();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string[] athletes = { dr[0].ToString(), (string)dr[1], (string)dr[2], (string)dr[3], dr[4].ToString(), dr[5].ToString(), dr[6].ToString() };
                ListViewItem listAthletes = new ListViewItem(athletes);
                lvAthletes.Items.Add(listAthletes);
            }


            if (lvAthletes.Items.Count == 0)
            {
                MessageBox.Show("No athletes registered!", "ERROR");
            }

            conn.Close();

        }

        //aesthetics
        private void tbModifyID_TextChanged(object sender, EventArgs e)
        {
                tbModifyID.ForeColor = Color.Black;
        }

        private void tbModifyFName_TextChanged(object sender, EventArgs e)
        {
                tbModifyFName.ForeColor = Color.Black;

        }

        private void tbModifyLName_TextChanged(object sender, EventArgs e)
        {
                tbModifyLName.ForeColor = Color.Black;
        }


        private void tbModifyWeight_TextChanged(object sender, EventArgs e)
        {
                tbModifyWeight1.ForeColor = Color.Black;

        }

        //display list of registered athletes
        private void btnRegistered_Click(object sender, EventArgs e)
        {
            if (dgvAthlete.Rows != null)
            {
                dgvAthlete.Rows.Clear();
            }

            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-E2FEQ2B\\SQLEXPRESS;Initial Catalog=Weightlifting;Integrated Security=True");
            conn.Open();

            SqlCommand cmd = new SqlCommand("select * from Athlete", conn);
            cmd.ExecuteNonQuery();

            SqlDataReader dr = cmd.ExecuteReader();

            while(dr.Read())
            {
                string[] athletes = { dr[0].ToString(), (string)dr[1], (string)dr[2], (string)dr[3], dr[4].ToString() };
                dgvAthlete.Rows.Add(athletes);
            }

            dr.Read();
            if(!(dr.HasRows))
            {
                MessageBox.Show("No athletes registered!", "ERROR");
            }

            dr.Close();

            conn.Close();

        }

        //aesthetics
        private void tbModifyWeight2_TextChanged(object sender, EventArgs e)
        {
            tbModifyWeight2.ForeColor = Color.Black;
        }

        private void tbModifyWeight3_TextChanged(object sender, EventArgs e)
        {
            tbModifyWeight3.ForeColor = Color.Black;
        }

        //displays ordered list of athletes
        private void btnOrder_Click(object sender, EventArgs e)
        {
            if (lvOrdered.Items != null)
            {
                lvOrdered.Items.Clear();
                lvOrdered.Columns.Clear();
                lvOrdered.Clear();
            }

            if (tabAthlete.SelectedTab == tabView)
            {
                lvOrdered.View = View.Details;
                lvOrdered.Columns.Add("ID");
                lvOrdered.Columns.Add("First Name");
                lvOrdered.Columns.Add("Last Name");
                lvOrdered.Columns.Add("Country");
                lvOrdered.Columns.Add("Weight1");
                lvOrdered.Columns.Add("Weight2");
                lvOrdered.Columns.Add("Weight3");
                lvOrdered.Columns.Add("Record");

                for (int i = 0; i < lvOrdered.Columns.Count; i++)
                {
                    lvOrdered.Columns[0].Width = 80;
                    lvOrdered.Columns[1].Width = 80;
                    lvOrdered.Columns[2].Width = 80;
                    lvOrdered.Columns[3].Width = 80;
                    lvOrdered.Columns[4].Width = 80;
                    lvOrdered.Columns[5].Width = 80;
                    lvOrdered.Columns[6].Width = 80;
                    lvOrdered.Columns[7].Width = 80;

                }
            }

            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-E2FEQ2B\\SQLEXPRESS;Initial Catalog=Weightlifting;Integrated Security=True");
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT *, (SELECT MAX(RECORD) FROM (VALUES ([Weight1]), ([Weight2]), ([Weight3])) AS WeightTable(RECORD)) AS RECORD FROM Athlete", conn);
            cmd.ExecuteNonQuery();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string[] athletes = { dr[0].ToString(), (string)dr[1], (string)dr[2], (string)dr[3], dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString() };
                ListViewItem listAthletes = new ListViewItem(athletes);
                lvOrdered.Items.Add(listAthletes);
            }

            if (lvOrdered.Items.Count == 0)
            {
                MessageBox.Show("No athletes registered!", "ERROR");
            }

            conn.Close();
        }

        //allows for user to click on listviewOrder and sort columns by ascending or descending order
        private void lvOrdered_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == System.Windows.Forms.SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.lvOrdered.Sort();
        }
    }
    }


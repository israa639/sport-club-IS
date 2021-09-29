using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using CrystalDecisions.Shared;
namespace Sport_club_IS
{
    public partial class Form1 : Form
    {


        int playerid = 0;
        string ordb = "Data Source=ORCL;User Id=hr;Password=hr;";
        OracleConnection conn;
        int userId = -1;
        DataTable playerTrainingDT;
        DataTable playerCompetition;
        CrystalReport1 cr1;
        CrystalReport2 cr2;
        public Form1()
        {
            InitializeComponent();
            addTrainingPanel.Visible = false;
            addcoachPanel.Visible = false;
            addTeamPanel.Visible = false;
            coachProfilePanel.Visible = false;
            choicesPanel.Visible = false;
            coachOptions_Panel.Visible = false;
            CoachUpdatePanel.Visible = false;
            playerloginpanel.Visible = false;
            playerProfilePanel.Visible = false;
            updateplayerpassword_panel.Visible = false;
            this.crystalReportViewer1.Visible = false;
            this.AddCompetitionPanel.Visible = false;
            this.choosereportpanel.Visible = false;
            this.AddPlayerpanel.Visible = false;
            this.report1panel.Visible = false;
            this.report2panel.Visible = false;
            this.crystalReportViewer2.Visible = false;



        }

        private void InsertTraningPanel_Paint(object sender, PaintEventArgs e)
        {

            // DatePicker Validation
            DateAddTraningPicker.MinDate = DateTime.UtcNow.Date;
            DateAddTraningPicker.MaxDate = DateTime.UtcNow.Date.AddYears(1);


        }

        private void AddNewTraning_Click(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
            if (AddSportNameTxt.Text == "")
            {
                MessageBox.Show("Please enter sport name");

            }
            else
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                //enter colunms with the same arrange on database 
                cmd.CommandText = "insert into Training  values (Trainingid_auto_inc.nextval,TO_DATE(:trainingDate, 'dd/MM/yyyy'),:sportName)";
                cmd.Parameters.Add("trainingDate", DateAddTraningPicker.Text);
                cmd.Parameters.Add("sportName", AddSportNameTxt.Text);

                int r = cmd.ExecuteNonQuery();
                if (r != -1)
                {
                    MessageBox.Show("New training is added");
                    AddSportNameTxt.Text = "";
                    DateAddTraningPicker.Text = DateTime.Now.ToString();
                }
            }

        }


        private void AddCoachbtn_Click(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
            if (CFNameTxt.Text == "" || CLNameTxt.Text == "" || CSalaryTxt.Text == "" || CAddressTxt.Text == "" || CSportNameTxt.Text == "" || CBdPicker.Text == "")
            {
                MessageBox.Show("Please complete coach data");
            }
            else
            {
                if (CPhoneNumTxt.Text.Length < 11 || CPhoneNumTxt.Text.ElementAt(0) != '0' || CPhoneNumTxt.Text.ElementAt(1) != '1')
                {
                    MessageBox.Show("Please correct phone number");

                }
                else
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "insert into COACH values (COACHID.nextval,:FirstName,:lastName,:coachPass,:salary,:address,:sportName,TO_DATE(:bd, 'dd/MM/yyyy'),:phoneNum)";
                    cmd.Parameters.Add("FirstName", CFNameTxt.Text);
                    cmd.Parameters.Add("lastName", CLNameTxt.Text);
                    cmd.Parameters.Add("coachPass", "123");
                    cmd.Parameters.Add("salary", float.Parse(CSalaryTxt.Text));
                    cmd.Parameters.Add("address", CAddressTxt.Text);
                    cmd.Parameters.Add("sportName", CSportNameTxt.Text);
                    cmd.Parameters.Add("bd", CBdPicker.Text);
                    cmd.Parameters.Add("phoneNum", CPhoneNumTxt.Text);
                    int r = cmd.ExecuteNonQuery();
                    if (r != -1)
                    {
                        MessageBox.Show("New coach is added");
                        CFNameTxt.Text = "";
                        CLNameTxt.Text = "";
                        CSalaryTxt.Text = "";
                        CAddressTxt.Text = "";
                        CSportNameTxt.Text = "";
                        CBdPicker.Text = "";
                        CPhoneNumTxt.Text = "";
                    }

                }
            }

        }



        private void CPhoneNumTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            //8 is enumeration for backspace key

            if (!Char.IsDigit(ch) && ch != 8)
                e.Handled = true;
        }

        private void CSalaryTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            //8 is enumeration for backspace key
            //46 is enumeration for Del key
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
                e.Handled = true;
        }





        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Dispose();

        }

        private void addTeamPanel_Paint(object sender, PaintEventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
              OracleCommand cmd = new OracleCommand();
              cmd.Connection = conn;
              //cmd.CommandText = "select SPORT_NAME from training";
              //cmd.CommandType = CommandType.Text;
              cmd.CommandText = "GetSportName";
              cmd.CommandType = CommandType.StoredProcedure;
              cmd.Parameters.Add("sportName", OracleDbType.RefCursor, ParameterDirection.Output);
              OracleDataReader dr = cmd.ExecuteReader();
              while (dr.Read())
              {
                  sportName_cb.Items.Add(dr[0]);
              }

              OracleCommand cmd2 = new OracleCommand();
              cmd2.Connection = conn;
              cmd2.CommandText = "GetCoachID";
              cmd2.CommandType = CommandType.StoredProcedure;
              cmd2.Parameters.Add("coach_id", OracleDbType.RefCursor, ParameterDirection.Output);
              dr = cmd2.ExecuteReader();
              while (dr.Read())
              {
                  coachID_cb.Items.Add(dr[0]);
              }
              dr.Close();
        }

        private void addTeam_btn_Click(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
             if (teamName_txt.Text == "" || sportName_cb.Text == "" || rank_txt.Text == "" || coachID_cb.Text == "")
             {
                 MessageBox.Show("Please complete coach data");
             }
             else
             {
                 OracleCommand cmd = new OracleCommand();
                 cmd.Connection = conn;
                 cmd.CommandText = $"insert into team values (TEAMID.nextval, '{teamName_txt.Text}', '{sportName_cb.SelectedItem.ToString()}', '{rank_txt.Text}', '{coachID_cb.SelectedItem.ToString()}')";
                 int r = cmd.ExecuteNonQuery();
                 if (r != -1)
                 {
                     MessageBox.Show("New team is added");
                     teamName_txt.Text = "";
                     sportName_cb.SelectedIndex = -1;
                     rank_txt.Text = "";
                     coachID_cb.SelectedIndex = -1;
                 }
             }
        }

        private void rank_txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            //8 is enumeration for backspace key
            //46 is enumeration for Del key
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
                e.Handled = true;
        }

        private void Update_btn_Click(object sender, EventArgs e)
        {

            // System.Windows.Forms.Application.Exit();
            DMForm form = new DMForm();
            Hide();
            form.ShowDialog();
            Show();
        }

        private void newTeam_btn_Click(object sender, EventArgs e)
        {
            addTeamPanel.Visible = true;
            choicesPanel.Visible = false;

        }

        private void AddTraining_btn_Click(object sender, EventArgs e)
        {
            addTrainingPanel.Visible = true;
            choicesPanel.Visible = false;
        }

        private void AddCoach_btn_Click(object sender, EventArgs e)
        {
            addcoachPanel.Visible = true;
            choicesPanel.Visible = false;
        }

        private void backFromTeamPanel_btn_Click(object sender, EventArgs e)
        {
            addTeamPanel.Visible = false;
            choicesPanel.Visible = true;
        }

        private void backFromTrainingPanel_btn_Click(object sender, EventArgs e)
        {
            addTrainingPanel.Visible = false;
            choicesPanel.Visible = true;
        }

        private void backFormNewCoachPanel_btn_Click(object sender, EventArgs e)
        {
            addcoachPanel.Visible = false;
            choicesPanel.Visible = true;
        }

        private void updatebtn_Click(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
            if (coachPass_txt.Text == "" || confirmPass_txt.Text == "")
            {
                MessageBox.Show("Please complete data");
            }
            else if (coachPass_txt.Text != confirmPass_txt.Text)
            {
                MessageBox.Show("confirm password doesn't match password");
            }
            else
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "update coach set COACHPASSWORD = :pass where CoachID = :id";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("pass", coachPass_txt.Text);
                
                cmd.Parameters.Add("id", ID_txt.Text);
                int r = cmd.ExecuteNonQuery();
                if (r != -1)
                {
                    MessageBox.Show("Updated");
                    coachPass_txt.Text = "";
                    confirmPass_txt.Text = "";
                }

            }

        }

        private void back_btn_Click(object sender, EventArgs e)
        {
            coachOptions_Panel.Visible = true;
            CoachUpdatePanel.Visible = false;
        }

        private void coachProfilePanel_Paint(object sender, PaintEventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand c = new OracleCommand();
            c.Connection = conn;
            //select * from training T inner join works_on as W on T.trainingid = W.trainingid inner join coach as c on c.coachid = W.coachid where w.coachid = :id
            c.CommandText = "select * from Team where  CoachID = :id";
            c.Parameters.Add("id", ID_txt.Text);
            // c.CommandText = "SELECT DATES FROM training, coach, works_on WHERE COACH.coachid = WORKS_ON.coachid AND COACH.CoachID = :id";
            c.CommandType = CommandType.Text;
            //TODO::change 24 from login
            
            OracleDataReader dr = c.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(dr);
            TeamDGV.DataSource = dataTable;
            /********************************************/
            OracleCommand c2 = new OracleCommand();
            c2.Connection = conn;
            c2.CommandText = "SELECT DATES FROM training, coach, works_on WHERE COACH.coachid = WORKS_ON.coachid AND COACH.CoachID = :id";
            c2.CommandType = CommandType.Text;
            //TODO::change 24 from login
            c2.Parameters.Add("id", ID_txt.Text);
            dr = c2.ExecuteReader();
            dataTable = new DataTable();
            dataTable.Load(dr);
            trainingDates_DGV.DataSource = dataTable;
            dr.Close();
        }

        private void backFromCoachprofile_btn_Click(object sender, EventArgs e)
        {
            coachOptions_Panel.Visible = true;
            coachProfilePanel.Visible = false;
        }



        private void coachProfile_btn_Click(object sender, EventArgs e)
        {
            coachProfilePanel.Visible = true;
            coachOptions_Panel.Visible = false;
        }

        private void restPass_btn_Click(object sender, EventArgs e)
        {
            coachOptions_Panel.Visible = false;
            CoachUpdatePanel.Visible = true;
        }

        private void logout_btn_Click(object sender, EventArgs e)
        {
            coachOptions_Panel.Visible = false;
            Login_Panel.Visible = true;
            ID_txt.Text = "";
            pass_txt.Text = "";

            userId = -1;
        }

        private void logIn_btn_Click(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
            if (ID_txt.Text == "" || pass_txt.Text == "")
                MessageBox.Show("please compelete data!");
            else
            {

                if (Admin_rb.Checked)
                {
                    if (ID_txt.Text == "admin" || pass_txt.Text == "admin")
                    {
                        choicesPanel.Visible = true;
                        Login_Panel.Visible = false;
                        ID_txt.Text = "";
                        pass_txt.Text = "";
                    }
                }
                else
                {

                    /*
                    if (coach_rb.Checked)
                    {



                        string procedureName = "";
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = conn;
                        
                            procedureName = "getcoachdata";
                            cmd.CommandText = procedureName;
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("inputId", ID_txt.Text);
                            cmd.Parameters.Add("pass", OracleDbType.NVarchar2, ParameterDirection.Output);
                            cmd.ExecuteNonQuery();
                            

                      if(cmd.Parameters["pass"].Value.ToString()==passTxtBox.Text)
                        {

                            Login_Panel.Visible = false;
                            coachOptions_Panel.Visible = true;





                        }
                        else
                        {
                            MessageBox.Show("please enter correct password");
                        }
                        */



















                    if (coach_rb.Checked)
                    {
                        OracleCommand cmd1 = new OracleCommand();
                           cmd1.Connection = conn;
                          // procedureName = "getcoachdata";
                           cmd1.CommandText = "select * from coach where coachid =:pid";

                           cmd1.Parameters.Add("pid", ID_txt.Text);
                           userId = int.Parse(ID_txt.Text);

                           OracleDataReader dr = cmd1.ExecuteReader();

                           //playerid = int.Parse(dr[0].ToString());
                           if (dr.Read())
                           {
                               string x = dr[2].ToString();
                               if (dr[8].ToString() != pass_txt.Text)
                               {
                                   MessageBox.Show("please enter correct password");

                               }
                               else
                               {
                                   Login_Panel.Visible = false;
                                   coachOptions_Panel.Visible = true;



                               }
                           }

                           dr.Close();
   
                    }
                    else if (player_rb.Checked)
                    {
                        OracleCommand cmd2 = new OracleCommand();
                        cmd2.Connection = conn;
                        cmd2.CommandText = "select * from player where playerid=:pid";
                        cmd2.Parameters.Add("pid", ID_txt.Text);
                        userId = int.Parse(ID_txt.Text);

                        OracleDataReader dr = cmd2.ExecuteReader();

                        //playerid = int.Parse(dr[0].ToString());
                        if (dr.Read())
                        {
                            string x = dr[2].ToString();
                            if (dr[2].ToString() != pass_txt.Text)
                            {
                                MessageBox.Show("please enter correct password");

                            }
                            else
                            {

                                playerloginpanel.Visible = true;
                                Login_Panel.Visible = false;


                            }
                        }
                        
                        dr.Close();






                       
                        

                    }
                }
            }

        }

        private void adminLogout_btn_Click(object sender, EventArgs e)
        {
            choicesPanel.Visible = false;
            Login_Panel.Visible = true;
        }

        private void coachOptions_Panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void playerProfile_btn_Click(object sender, EventArgs e)
        {
            
        }

        private void dgvPlayerComptetion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       /* private void playerUpdate_btn_Click(object sender, EventArgs e)
        {
            playerid = 0;
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = " select * from player where playerid=:pid";
            cmd.Parameters.Add("pid", ID_txt.Text);
            OracleDataReader dr = cmd.ExecuteReader();

            if (playeroldpassword_txt.Text == "" || playernewpass_txt.Text == "" || playerconfirmpass_txt.Text == " ")
            {
                MessageBox.Show("Please complete data");
            }
            else
            {
                if (dr[2].ToString() != playeroldpassword_txt.Text)
                {
                    MessageBox.Show("please re-write the correct password");
                }
                else
                {
                    if (playernewpass_txt.Text == playerconfirmpass_txt.Text)
                    {
                        OracleCommand cmd1 = new OracleCommand();
                        cmd1.CommandText = "update player set playerpassword =:pass where playerid = :pid";
                        cmd1.Parameters.Add("pass", playernewpass_txt.Text);
                        cmd1.Parameters.Add("pid", ID_txt.Text);
                        
                    }
                    else
                    {
                        MessageBox.Show("please re-write the new password");
                    }
                }
            }
        }
        */
        private void updateplayerpass_btn_Click(object sender, EventArgs e)
        {
            playerProfilePanel.Visible = false;
            playerloginpanel.Visible = false;
            updateplayerpassword_panel.Visible = true;

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            
            

            //Login_Panel.Visible = false;
            conn = new OracleConnection(ordb);
            conn.Open();
        }

        private void playerloginpanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AddCompetitionBtn_Click(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            int check = 0;
            if (String.IsNullOrEmpty(compNameTXTBOX.Text) || String.IsNullOrEmpty(startDatePicker.Text) || String.IsNullOrEmpty(endDatePicker.Text) || String.IsNullOrEmpty(SportNameTxtBox.Text))
            {
                MessageBox.Show("please fill the missed boxes");
                check = 1;
            }
            if (check != 1)
            {
                cmd.CommandText = "insert into competition values(competitionid.nextval,:cName,:sdate,:edate,:sname)";
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("cname", compNameTXTBOX.Text);
                cmd.Parameters.Add("sdate", startDatePicker.Text);
                cmd.Parameters.Add("edate", endDatePicker.Text);
                cmd.Parameters.Add("sname", SportNameTxtBox.Text);
                int r = cmd.ExecuteNonQuery();
                if (r != -1)
                {
                    MessageBox.Show("competition is added successfully");
                }
            }
        }

        private void addcompmenue_btn_Click(object sender, EventArgs e)
        {

            choicesPanel.Visible = false;
            this.AddCompetitionPanel.Visible = true;
        }

        private void addPlayer_btn_Click(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            int check = 0;//to check if the 2 passwards are equal and if there is an empty blank

            if (passTxtBox.Text != confPassTxtBox.Text)
            {
                MessageBox.Show("re-type password");
                check = 1;
            }
            if (String.IsNullOrEmpty(phoneTxtBox.Text) || String.IsNullOrEmpty(passTxtBox.Text) || String.IsNullOrEmpty(confPassTxtBox.Text) || String.IsNullOrEmpty(fNameTxtBox.Text) || String.IsNullOrEmpty(lNameTxtBox.Text) || String.IsNullOrEmpty(birthDatePicker.Text) || String.IsNullOrEmpty(genderTxtBox.Text) || String.IsNullOrEmpty(addressTxtBox.Text) || String.IsNullOrEmpty(teamNameTxtBox.Text))
            {
                MessageBox.Show("please fill the missed boxes");
                check = 1;
            }

            if (check == 0)
            {
                cmd.CommandText = "insert into player values(playerID.nextval,:phone,:pass,:fname,:lname,:birth,:gender,:address, (select teamid from team where Name =:tname)) ";
                cmd.Parameters.Add("phone", phoneTxtBox.Text);
                cmd.Parameters.Add("pass", passTxtBox.Text);
                cmd.Parameters.Add("fname", fNameTxtBox.Text);
                cmd.Parameters.Add("lname", lNameTxtBox.Text);
                cmd.Parameters.Add("birth", birthDatePicker.Text);
                cmd.Parameters.Add("gender", genderTxtBox.Text);
                cmd.Parameters.Add("address", addressTxtBox.Text);
                cmd.Parameters.Add("tname", teamNameTxtBox.Text);
                int r = cmd.ExecuteNonQuery();
                if (r != -1)
                {
                    MessageBox.Show("new player is added");
                }

            }
        }

        private void reportmenue_btn_Click(object sender, EventArgs e)
        {
            this.choicesPanel.Visible = false;
            this.choosereportpanel.Visible = true;
        }

        private void firstreport_btn_Click(object sender, EventArgs e)
        {
            
            this.choosereportpanel.Visible = false;
            this.report1panel.Visible = true;
           
        }

        private void report1panel_Paint(object sender, PaintEventArgs e)
        {
            cr1 = new CrystalReport1();
            crystalReportViewer1.ReportSource = cr1;
            this.crystalReportViewer1.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.crystalReportViewer1.Visible = false;
            this.report1panel.Visible = false;
            this.choosereportpanel.Visible = true;
            
        }

        private void report2panel_Paint(object sender, PaintEventArgs e)
        {
            cr2 = new CrystalReport2();
            foreach (ParameterDiscreteValue v in cr2.ParameterFields[0].DefaultValues)
            { sportName_cmb.Items.Add(v.Value); }

            
            this.crystalReportViewer2.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.crystalReportViewer2.Visible = false;
            this.choosereportpanel.Visible = true;
            this.report2panel.Visible = false;
        }

        private void generaterep2_btn_Click(object sender, EventArgs e)
        {
            cr2.SetParameterValue(0, sportName_cmb.Text);



            crystalReportViewer2.ReportSource = cr2;
        }

        private void secondreport_btn_Click(object sender, EventArgs e)
        {
            this.choosereportpanel.Visible = false;
            this.report2panel.Visible = true;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.choosereportpanel.Visible = false;
            this.choicesPanel.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.AddCompetitionPanel.Visible = false;
            this.choicesPanel.Visible = true;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.AddPlayerpanel.Visible = false;
            this.choicesPanel.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.updateplayerpassword_panel.Visible = false;
            this.playerloginpanel.Visible = true;
        }

        

        private void button8_Click(object sender, EventArgs e)
        {
            this.playerProfilePanel.Visible = false;
            this.playerloginpanel.Visible = true;
        }

        private void updateplayerpass_btn_Click_1(object sender, EventArgs e)
        {
            this.playerloginpanel.Visible = false;
            this.updateplayerpassword_panel.Visible = true;
        }

        private void playerProfile_btn_Click_1(object sender, EventArgs e)
        {
            this.playerloginpanel.Visible = false;
            this.playerProfilePanel.Visible = true;
            conn = new OracleConnection(ordb);
            conn.Open();
            playerloginpanel.Visible = false;
            playerProfilePanel.Visible = true;
            playerTrainingDT = new DataTable();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select t1.dates,t1.sport_name , t2.teamname from training t1  join  team t2 on t1.sport_name = t2.sport_name where t1.trainingid = (select trainingid from do where playerid = :pid)and t2.teamid = (select teamid from player where playerid = :pid1 )";
            cmd.Parameters.Add("pid", ID_txt.Text);
            cmd.Parameters.Add("pid1", ID_txt.Text);





            OracleDataReader dr = cmd.ExecuteReader();

            dr = cmd.ExecuteReader();

            playerTrainingDT.Load(dr);
            dgv_trainingPlayer.DataSource = playerTrainingDT;
            dr.Close();
            /////////////////////////////////
            playerCompetition = new DataTable();
            OracleCommand cmd1 = new OracleCommand();
            cmd1.CommandText = "select * from competition where competitionid=(select competitionid from participates_in where teamid=(select teamid from player where playerid=:pid))and startdate >= trunc(sysdate)";
    
            cmd1.Parameters.Add("pid", ID_txt.Text);
            cmd1.Connection = conn;

            OracleDataReader dr1 = cmd1.ExecuteReader();

            dr1 = cmd1.ExecuteReader();

            playerCompetition.Load(dr1);
            dgvPlayerComptetion.DataSource = playerCompetition;
            dr1.Close();


        }

        private void playerlogout_btn_Click(object sender, EventArgs e)
        {
            this.playerloginpanel.Visible = false;
            this.Login_Panel.Visible = true;
            ID_txt.Text = "";
            pass_txt.Text = "";

        }

        private void addplayermenue_btn_Click(object sender, EventArgs e)
        {
            this.choicesPanel.Visible = false;
            this.AddPlayerpanel.Visible = true;
        }

        private void playerUpdate_btn_Click_1(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = " select * from player where playerid=:pid";
            cmd.Parameters.Add("pid", ID_txt.Text);
            playerid = int.Parse(ID_txt.Text);
            OracleDataReader dr = cmd.ExecuteReader();
            if (playeroldpassword_txt.Text == "" || playernewpass_txt.Text == "" || playerconfirmpass_txt.Text == " ")
            {
                MessageBox.Show("Please complete data");
            }
            else
            {
                if (dr.Read())
                {
                    if (dr[2].ToString() != playeroldpassword_txt.Text)
                    {
                        MessageBox.Show("please re-write the correct password");
                    }
                    else
                    {
                        if (playernewpass_txt.Text == playerconfirmpass_txt.Text)
                        {
                            OracleCommand cmd1 = new OracleCommand();
                            cmd1.CommandText = "update player set playerpassword =:pass where playerid = :pid";
                            cmd1.Parameters.Add("pass", playernewpass_txt.Text);
                            cmd1.Parameters.Add("pid", ID_txt.Text);
                            MessageBox.Show("updated successfully");
                        }
                        else
                        {
                            MessageBox.Show("please re-write the new password");
                        }
                    }
                }
                dr.Close();
            }
        }

        private void playerProfilePanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
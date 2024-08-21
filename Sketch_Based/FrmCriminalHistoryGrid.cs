using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;
using FireBaseLib;
using System.Windows.Forms;

namespace Sketch_Based
{
    public partial class FrmCriminalHistoryGrid : Form
    {
        RegisterPojo name = new RegisterPojo();
        RegisterPojo Register = new RegisterPojo();
        Bitmap myface;
              public static DataGridViewRow selectedrow;
        public FrmCriminalHistoryGrid()
        {
            InitializeComponent();
        }

 
        public async void DataFromCloud()
        {
            Cursor.Current = Cursors.WaitCursor;
            var fb = FireBaseDB.init(FBConfig.url);
            RegisterPojo dc = new RegisterPojo();
            var fbdata = await fb.Child("RegisterInfo").OnceAsync<RegisterPojo>();
            int id = 0;
            List<RegisterPojo> plist = new List<RegisterPojo>();
            foreach (var data in fbdata)
            {
                RegisterPojo quest = new RegisterPojo();
                quest.Name = data.Object.Name;
                quest.Date = data.Object.Date;
                quest.Description = data.Object.Description;
                quest.Crime = data.Object.Crime;
                quest.Middlename = data.Object.Middlename;
                quest.Surname = data.Object.Surname;
                quest.Registrationdate = data.Object.Registrationdate;
                quest.Age = data.Object.Age;
                quest.Address = data.Object.Address;
                quest.Mobno = data.Object.Mobno;
                quest.Dob = data.Object.Dob;
                quest.Aadharno = data.Object.Aadharno;
                quest.Gender = data.Object.Gender;
                plist.Add(quest);
                
            }
            dataGridView1.DataSource = plist;
            dataGridView1.Columns[4].Visible = false;
        }
        private void FrmCriminalHistory_Load(object sender, EventArgs e)
        {
            DataFromCloud();       
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedrow = dataGridView1.Rows[e.RowIndex];
                FrmCriminalHistoryRetrive.getform.ShowDialog();

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
    }
    


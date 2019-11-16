using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sinemaa24
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public ArrayList secilmisler = new ArrayList();
        public string seans_id;
        int ucret;
       
        private void Form2_Load(object sender, EventArgs e)
        {
            ucret = secilmisler.Count * 20;
           
            for (int i=0;i<secilmisler.Count;i++)
            {
                textBox1.Text += "," + secilmisler[i] ;
            }
            textBox2.Text = ucret.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            for (int i = 0; i < secilmisler.Count; i++)
            {
               
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = "Server=.\\SQLEXPRESS;Database=sinemaa24;Trusted_Connection=true";
                cn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "insert into biletler (seans_id,ucret,tarih,koltuk) values (@x,@y,getdate(),@t)" ;
                cmd.Parameters.AddWithValue("x", seans_id);
                cmd.Parameters.AddWithValue("y", ucret);
                cmd.Parameters.AddWithValue("t", secilmisler[i]);

                cmd.Connection = cn;

                cmd.ExecuteNonQuery();
              
            }
            
            MessageBox.Show("Biletiniz Alındı.");
            this.Close();
           
        }
    }
}

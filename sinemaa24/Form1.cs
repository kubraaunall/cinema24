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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ArrayList satilmislar = new ArrayList();
        ArrayList secilmisler = new ArrayList();
        DataTable salonlar = new DataTable();
        private void Form1_Load(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = "Server=.\\SQLEXPRESS;Database=sinemaa24;Trusted_Connection=true";
            cn.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from salonlar";
            cmd.Connection = cn;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(salonlar);

            for (int i = 0; i < salonlar.Rows.Count; i++)
            {
                treeView1.Nodes.Add(salonlar.Rows[i][0].ToString(), salonlar.Rows[i]["salon_adi"].ToString());

                SqlConnection cn2 = new SqlConnection();
                cn2.ConnectionString = "Server=.\\SQLEXPRESS;Database=sinemaa24;Trusted_Connection=true";
                cn2.Open();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "select*from seanslar inner join filmler ON seanslar.film_id=filmler.film_id where salon_id=@x";
                cmd2.Parameters.AddWithValue("x", salonlar.Rows[i]["salon_id"]);
                cmd2.Connection = cn2;


                SqlDataReader dr = cmd2.ExecuteReader();
                while(dr.Read())
                {
                 treeView1.Nodes[i].Nodes.Add(dr["seans_id"].ToString(), dr["saat"].ToString()+dr["film_adi"].ToString());
                }
            }


        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (treeView1.SelectedNode.Level != 0)
            {
                
                panel1.Controls.Clear();
                satilmislar.Clear();
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = "Server=.\\SQLEXPRESS;Database=sinemaa24;Trusted_Connection=true";
                cn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from biletler where seans_id=@x";
                cmd.Parameters.AddWithValue("x", treeView1.SelectedNode.Name);
                cmd.Connection = cn;

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    satilmislar.Add(dr["koltuk"].ToString());
                }

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Button btn = new Button();
                        btn.Top = i * 40;
                        btn.Left = j * 40;
                        btn.Height = 35;
                        btn.Width = 40;
                        btn.Text = (((i * 10) + (j + 1)).ToString());

                        if (satilmislar.Contains(btn.Text))
                        {
                            btn.BackColor = Color.Red;
                        }
                        else
                        {
                            btn.BackColor = Color.Green;
                        }
                        btn.Click += Btn_Click2;
                        panel1.Controls.Add(btn);
                    }
                }
            }
        }

        private void Btn_Click2(object sender, EventArgs e)
        {
         

            Button b = (Button)sender;
            if (b.BackColor == Color.Green)
            {
                b.BackColor = Color.Yellow;
                secilmisler.Add(b.Text);
            }
            else if (b.BackColor == Color.Yellow)
            {
                b.BackColor = Color.Green;
                secilmisler.Remove(b.Text);
            }
        }

        private void Btn_Click1(object sender, EventArgs e)
        {

        }

        private void Btn_Click(object sender, EventArgs e)
        {
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.secilmisler = secilmisler;
            f2.seans_id = treeView1.SelectedNode.Name;
            f2.Show();
            
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            treeView1_AfterSelect(null, null);
        }
    }
}

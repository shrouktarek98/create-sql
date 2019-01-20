using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Collections;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {


        string tablename;
        string fieldname;
        XmlNodeList data;
        XmlNodeList columns;
        int count;
        string sign;
        int searchindex;
        string searchvalue;
        Stack<List<int>> stack = new Stack<List<int>>();
        int panelwidth;
        bool hidden;


        public Form1()
        {
            InitializeComponent();
            panel3.Visible = false;
            datagradeview20.Visible = false;
            panelwidth = panelslide.Width;
            hidden = false;
        }

        //...........function 
        //............function to load files name into combobox
        public void list_files(string path)
        {
            string[] files = Directory.GetFiles(path);

            for (int i = 0; i < files.Count(); i++)
            {

                if (Path.GetExtension(files[i]) == ".xml")
                {
                    comboBox1.Items.Add(Path.GetFileName(files[i]));
                }

            }

        }
        //..............end function loading


        //function check data
        public bool check_is_string(string field)
        {
            bool is_string = true;
            int siz = 0;
            string inner = "";
            
         for (int i = 0; i < count; i++)//number of columns
            {
                if (columns[i].Name == field)
                {
                    inner = columns[i].InnerText;

                }
            }

            for (int i = 0; i < inner.Length; i++)
            {
                if (inner[i] >= 48 && inner[i] <= 57 || inner[i] == 46)//number from 0 to 9 and .
                {
                    siz++;
                }
            }
            if (siz == inner.Length)
            {
                is_string = false;
            }
            return is_string;
        }
        //end check_is_string
        //--------------------------------------------------------
        
        public void display()//function display
        {
            if (datagradeview20.ColumnCount == 0)
            {
                datagradeview20.ColumnCount = count;
                for (int i = 0; i < count; i++)
                {
                    datagradeview20.Columns[i].Name = columns[i].Name;
                }

            }

        }
        //end display
        //-------------------------------------------------------
        //function condition_operator

        //---------------------------------------------------------------------------------

        //function to get index of field that i do condition on
        public int find_index(string field)//funtion find index
        {

            for (int i = 0; i < data[0].ChildNodes.Count; i++)
            {
                if (field == data[0].ChildNodes[i].Name)
                {
                    return i;

                }

            }

            return -1;
        }

        //-----------------------------
        //function to get array of index that achieve the condition
        public List<int> condition_result(string sign, string field, string v_search)
        {
            List<int> res = new List<int>();
            searchindex = find_index(field);


            switch (sign)
            {
                case "==":
                    foreach (XmlNode node in data)
                    {
                        if (v_search == node.ChildNodes[searchindex].InnerText)
                        {
                            res.Add(1);
                        }
                        else
                        { res.Add(0); }

                    }
                    break;

                case "!=":
                    foreach (XmlNode node in data)
                    {
                        if (v_search != node.ChildNodes[searchindex].InnerText)
                        {
                            res.Add(1);
                        }
                        else
                        { res.Add(0); }

                    }
                    break;

                case ">":
                    foreach (XmlNode node in data)
                    {
                        if (double.Parse(v_search) < double.Parse(node.ChildNodes[searchindex].InnerText))
                        {
                            res.Add(1);
                        }
                        else
                        { res.Add(0); }

                    }
                    break;

                case "<":
                    foreach (XmlNode node in data)
                    {
                        if (double.Parse(v_search) > double.Parse(node.ChildNodes[searchindex].InnerText))
                        {
                            res.Add(1);
                        }
                        else
                        { res.Add(0); }

                    }
                    break;

                case "IN":
                    string[] fields = v_search.Split(',');
                    int kema = -1;
                    foreach (XmlNode node in data)
                    {
                        for (int i = 0; i < fields.Length; i++)
                        {
                            if (fields[i] == node.ChildNodes[searchindex].InnerText)
                            {
                                kema = 1;
                                break;
                            }
                            else
                            { kema = 0; }
                        }
                        res.Add(kema);
                    }
                    break;


            }//end switch
            return res;

        }//end function condition result
        //-----------------------------------------------------------

        public List<int> final(List<int> condition1, List<int> condition2, string link)
        {
            List<int> fine = new List<int>();
            switch (link)
            {
                case "&&":
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (condition1[i] == 1 && condition2[i] == 1)
                        {
                            fine.Add(1);
                        }
                        else { fine.Add(0); }
                    }
                    break;

                case "||":
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (condition1[i] == 1 || condition2[i] == 1)
                        {
                            fine.Add(1);
                        }
                        else { fine.Add(0); }
                    }
                    break;

            }//end switch
            return fine;
        } //end function final 
        //---------------------------------------------

        public void gridview_show(List<int> final)
        {
            datagradeview20.Columns.Clear();
            display();

            for (int i = 0; i < data.Count; i++)
            {
                if (final[i] == 0)
                {
                    continue;
                }

                ArrayList row = new ArrayList();
                for (int k = 0; k < data[i].ChildNodes.Count; k++)
                {
                    row.Add(data[i].ChildNodes[k].InnerText);
                }



                datagradeview20.Rows.Add(row.ToArray());
            }
        }
        //-----------------------------------------------------------
        public void show_data()//show all data
        {
            display();
            foreach (XmlNode node in data)
            {
               
                ArrayList row = new ArrayList();
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    row.Add(node.ChildNodes[i].InnerText);
                }
                datagradeview20.Rows.Add(row.ToArray());
            }
        }

        private void normal_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
        }

        private void query_Click_1(object sender, EventArgs e)
        {

           Form2 f = new Form2();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)//button sum
        {
            
            double sum = 0;
            searchindex = find_index(fieldname);     // to know index
            datagradeview20.Columns.Clear();
            foreach (XmlNode node in data)
            {
                sum += Convert.ToDouble(node.ChildNodes[searchindex].InnerText);   
            }
            MessageBox.Show("The Sum" + sum );
        }

        private void button1_Click(object sender, EventArgs e)//button avarge
        {
            
            double sum = 0, numofvar = 0;
            searchindex = find_index(fieldname);     // to know index
            datagradeview20.Columns.Clear();
            foreach (XmlNode node in data)
            {    
                sum+= Convert.ToDouble(node.ChildNodes[searchindex].InnerText);           
                numofvar++;
            }
            MessageBox.Show("The average" + sum / numofvar);
        }

        private void change_table_Click(object sender, EventArgs e)
        {
            datagradeview20.Visible = false;
            datagradeview20.Columns.Clear();
            ok_count = 0;
            if (comboBox1.Text == "Table Name") { MessageBox.Show("please choose table name"); return; }

            stack.Clear();
            foreach (XmlNode node in data)
            {
                node.RemoveAll();
            }
            comboBox2.Items.Clear();
            comboBox2.Text = "";

            label4.Visible = false;
            label1.Visible = true;
            comboBox1.Text = "";
            comboBox1.Visible = true;

            button1.Visible = false;
            button2.Visible = false;
            comboBox3.Text = "";
            comboBox4.Text = "";
            comboBox4.Visible = false;
            textBox1.Text = "";
            label5.Text = "";
            comboBox3.Items.Clear();
        }


        private void show_Click(object sender, EventArgs e)
        {
            
            //datagradeview20.Columns.Clear();
            if (comboBox1.Text == "Table Name") { MessageBox.Show("please choose table name"); return; }          
                datagradeview20.Visible = true;
                datagradeview20.Columns.Clear();
                show_data();
            
        }
        //-----------------------------------------------------------
        int ok_count = 0;
        private void button5_Click(object sender, EventArgs e)//button ok
        {

            if (comboBox2.Text == "Field Name" || comboBox3.Text == "" || textBox1.Text == "Table Name")
            {
                MessageBox.Show("please enter the condition ");
                return;
            }
            datagradeview20.Visible = true;
            if (ok_count > 0 && comboBox4.Text == "")
            {
                MessageBox.Show("please enter link(&&  ,  || ) ");
                return;
            }
            fieldname = comboBox2.Text;
            searchvalue = textBox1.Text;
            sign = comboBox3.Text;

            List<int> con = condition_result(sign, fieldname, searchvalue);
            stack.Push(con);
            label5.Text += comboBox4.Text + " " + fieldname + " " + sign + " " + searchvalue + " ";
            if (stack.Count == 2)
            {
                con = final(stack.Pop(), stack.Pop(), comboBox4.Text);
                stack.Push(con);

            }
            gridview_show(con);
            comboBox4.Visible = true;
            ok_count++;
            comboBox3.Text = "";
            comboBox4.Text = "";

            textBox1.Text = "";
            comboBox2.Text = "";
        }
        //-----------------------------------------------------------
        private void button3_Click(object sender, EventArgs e)//button clear
        {
            stack.Clear();
            datagradeview20.Columns.Clear();
            comboBox3.Text = "";
            comboBox4.Text = "";
            comboBox4.Visible = false;
            textBox1.Text = "";
            comboBox2.Text = "";
            label5.Text = "";
            ok_count = 0;
        }
        //-----------------------------------------------------------
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)//combobox1 table
        {
            tablename = comboBox1.SelectedItem.ToString();

            //show file name in label4 and hide the combobox to prevent user from changing file until pushing button 2
            label1.Visible = false;
            comboBox1.Visible = false;
            label4.Text = tablename;
            label4.Visible = true;
            //............................// 

            //if combobox .selected item changed ,change data of field (combobox2)
            this.comboBox2.Items.Clear();
            XmlDocument doc = new XmlDocument();
            doc.Load(tablename);


            data = doc.DocumentElement.ChildNodes;

            columns = data[0].ChildNodes;
            count = columns.Count;


            for (int i = 0; i < count; i++)
            {
                if (columns[i].ChildNodes.Count == 1)
                {
                    comboBox2.Items.Add(columns[i].Name);
                }
            }
        }
        //-----------------------------------------------------------
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)//combobox2 fieled
        {
            
            comboBox3.Items.Clear();
            fieldname = comboBox2.SelectedItem.ToString();
            //loading operation according to type of node

            if (check_is_string(fieldname))
            {
                button1.Visible = false;
                button2.Visible = false;
                comboBox3.Items.AddRange(new object[] { "==", "!=", "IN" });
            }
            else 
            {
                button1.Visible = true;
                button2.Visible = true;

                comboBox3.Items.AddRange(new object[] { ">","<","==", "!=", "IN" });
            }

            //end loading operations


        }
        //-----------------------------------------------------------
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label4.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            comboBox4.Visible = false;

            //.........loading files name into combobox1           
            list_files(@"C:\Users\Mostafa\Documents\Visual Studio 2015\Projects\WindowsFormsApplication2 - Copy\bin\Debug");
            //...........end loading files name


        }

        private void button4_Click(object sender, EventArgs e)
        {
            

             
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_Enter(object sender, EventArgs e)//comboBox1_table
        {
            if (comboBox1.Text == "Table Name")//is empty
            {
                comboBox1.Text = "";
                comboBox1.ForeColor = Color.White;
            }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {

            if (comboBox1.Text == "")
            {
                comboBox1.Text = "Table Name";
                comboBox1.ForeColor = Color.Silver;
            }
        }

        private void comboBox2_Enter(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Field Name")
            {
                comboBox1.Text = "";
                comboBox1.ForeColor = Color.White;
            }
        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                comboBox1.Text = "Field Name";
                comboBox1.ForeColor = Color.Silver;
            }
        }

        private void panel20_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (hidden)
            {

                panelslide.Width = panelslide.Width + 10;
                if (panelslide.Width >= panelwidth)
                {
                    timer1.Stop();
                    hidden = false;
                    this.Refresh();

                }
            }
            else

                panelslide.Width = panelslide.Width - 10;
            if (panelslide.Width <= 0)
            {

                timer1.Stop();
                hidden = true;
                this.Refresh();

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            timer1.Start();

        }

      }

   }
   


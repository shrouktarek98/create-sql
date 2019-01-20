using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Collections;

namespace WindowsFormsApplication2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        string query;
        string tablename;
        XmlNodeList data;
        XmlNodeList columns;
        int count;
        int searchindex;
        string fieldname;
        normalform f = new normalform();


        public void display(XmlNodeList da, DataGridView gridview) //display all headers in first row
        {
          int num=da[0].ChildNodes.Count;
          gridview.ColumnCount=num;
          for(int i=0;i<da[0].ChildNodes.Count;i++)
          {
             gridview.Columns[i].Name = da[0].ChildNodes[i].Name;

          }
        }
        //end function
        //------------------------------------------------------


      public void display(char s, XmlNodeList da, DataGridView gridview) //display all data in *
      {
            display(da, gridview);
           
            foreach(XmlNode node in da)
            {
                ArrayList row = new ArrayList();
                for(int i=0;i<node.ChildNodes.Count;i++)
                {
                    row.Add(node.ChildNodes[i].InnerText);
                }
                gridview.Rows.Add(row.ToArray());
            }

      }
        //end function
        //----------------------------------------------------------------
     public  void display(List<int> list,XmlNodeList da,DataGridView gridview) //show some field
       {
         int num=list.Count;
          gridview.ColumnCount=num;
         for(int i=0;i<list.Count;i++)
         {
             gridview.Columns[i].Name = da[0].ChildNodes[list[i]].Name;
         }
         
         foreach(XmlNode node in da)
         {
             ArrayList row = new ArrayList();
             for(int i=0;i<list.Count;i++)
             {
                 row.Add(node.ChildNodes[list[i]].InnerText);
             }
             gridview.Rows.Add(row.ToArray());
         }
       }
        //end show some ------------------------------------------------------------------------------
         public void gridview_show(List<int> final,XmlNodeList da,DataGridView gridview) 
         {
             display(da, gridview);
             for (int i = 0; i < da.Count;i++ )
             {
                 if (final[i] == 0)
                 {
                     continue;
                 }
                
                 ArrayList row = new ArrayList();
                 for (int k = 0; k < da[i].ChildNodes.Count; k++)
                 {
                     row.Add(da[i].ChildNodes[k].InnerText);
                 }

                 gridview.Rows.Add(row.ToArray());
             }
         }
        //-----------------------------------------------------------------//
        //takes final to know which rows will be shown
        //takes list to know which columns will be shown
        //show data grid view when there is a condition
         public void gridview_show(List<int> final, List<int> list, XmlNodeList da, DataGridView gridview)
         {
             int num = list.Count;
             gridview.ColumnCount = num;
             for (int i = 0; i < list.Count; i++)
             {
                 gridview.Columns[i].Name = da[0].ChildNodes[list[i]].Name;
             }

             for (int i = 0; i < da.Count; i++)
             {
                 if (final[i] == 0)
                 {
                     continue;
                 }


                 ArrayList row = new ArrayList();
                 for (int k = 0; k < list.Count; k++)
                 {
                     row.Add(da[i].ChildNodes[list[k]].InnerText);
                 }
                 gridview.Rows.Add(row.ToArray());

             }
         }
        public int find_index(string fieldname)
        {
            
                for (int i = 0; i < data[0].ChildNodes.Count; i++)
                {
                    if (fieldname == data[0].ChildNodes[i].Name)
                    {
                        return i;
                    }

                }
           
            return -1;
        }
        //--------------------------------------------------------------------------
        public bool check_is_string(string field)
        {
            bool is_string = true;
            int siz = 0;
            string inner = "";
            for (int i = 0; i < count; i++)
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
        public bool checkcondition(string fname, string sign)
        {
            bool b = false;
            int findindex = find_index(fname);
            if (findindex == -1)
            {
                MessageBox.Show("Invalid fieldname");
                return false;
            }
            b = check_is_string(fname);
            if(b==true)// fname is string ==,!=,IN
            {
                if (sign == "=" || sign == "!="||sign=="IN")
                {
                    return true;
                }
                return false;
            }
            return true;
            
        }

        public List<int> condition_result(string sign, string field, string v_search,XmlNodeList da)
        {
            List<int> res = new List<int>();
            searchindex = find_index(field);


            switch (sign)
            {
                case "=":
                    foreach (XmlNode node in da)
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
                    foreach (XmlNode node in da)
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
                    foreach (XmlNode node in da)
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
                    foreach (XmlNode node in da)
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
                    foreach (XmlNode node in da)
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
        public List<int> final(List<int> condition1, List<int> condition2, string link, XmlNodeList da)
        {
            List<int> fine = new List<int>();
            switch (link)
            {
                case "AND":
                    for (int i = 0; i < da.Count; i++)
                    {
                        if (condition1[i] == 1 && condition2[i] == 1)
                        {
                            fine.Add(1);
                        }
                        else { fine.Add(0); }
                    }
                    break;

                case "OR":
                    for (int i = 0; i < da.Count; i++)
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
        private void button1_Click(object sender, EventArgs e)
        {
            
            //datagrade.Columns.Clear();
            if (textBox1.Text == "Please Enter Your Quary....") 
            {
                MessageBox.Show("enter the query");
                return;
            }
            datagrade.Columns.Clear();
            datagrade.Visible = true;

            query = textBox1.Text;
           
            string[] field = query.Split(' ');
            string[] col = field[1].Split(',');
            
            if(field[0]!="select"||field[2]!="from")
            {
                MessageBox.Show("Error!!!!");
                return;
            }
              
            
            tablename = field[3];
            //load
            XmlDocument doc = new XmlDocument();
            doc.Load(tablename + ".xml");
            data = doc.DocumentElement.ChildNodes;

            columns = data[0].ChildNodes;
            count = columns.Count;
            //end load
            if (field.Length == 4)
            {
                if (col.Length == 1)
                {
                    if (col[0] == "*")
                    {
                        display('*', data, datagrade);
                    }
                    else
                    {
                        string s = col[0];
                        fieldname = "";
                        bool isfield = true;
                        for (int i = 0; i < s.Length; i++)
                        {

                            if (s[i] == '(')
                            {
                                isfield = false;
                                break;
                            }

                        }
                        if (isfield == true)
                        {
                            fieldname = s;
                            List<int> findindex = new List<int>();
                            int x = find_index(s);
                            if (x == -1)
                            {
                                MessageBox.Show("Invalid Field!!!");
                                return;
                            }
                            findindex.Add(x);
                            display(findindex, data, datagrade);
                        }
                        else
                        {
                            string order = "";
                            int j = 0;
                            while (s[j] != '(')
                            {
                                order += s[j];
                                j++;
                            }
                            j++;
                            while (s[j] != ')')
                            {
                                fieldname += s[j];
                                j++;
                            }

                            if (order == "avg")
                            {
                                searchindex = find_index(fieldname); // to know index
                                if (searchindex == -1)
                                {
                                    MessageBox.Show("field name is invalid");
                                    return;
                                }
                                bool isstring = check_is_string(fieldname);
                                if (isstring == true)
                                {
                                    MessageBox.Show("invalid operation");
                                    return;
                                }

                                double sum = 0, numofvar = 0;
                                List<int> findindex = new List<int>();
                                findindex.Add(searchindex);
                                display(findindex, data, datagrade);

                                foreach (XmlNode node in data)
                                {

                                    sum += Convert.ToDouble(node.ChildNodes[searchindex].InnerText);
                                    numofvar++;
                                }
                                MessageBox.Show("The average" + sum / numofvar);

                            }
                            else if (order == "sum")
                            {
                                searchindex = find_index(fieldname); // to know index
                                if (searchindex == -1)
                                {
                                    MessageBox.Show("field name is invalid");
                                    return;
                                }
                                bool isstring = check_is_string(fieldname);
                                if (isstring == true)
                                {
                                    MessageBox.Show("invalid operation");
                                    return;
                                }

                                List<int> findindex = new List<int>();
                                findindex.Add(searchindex);
                                display(findindex, data, datagrade);
                                double sum = 0;
                                foreach (XmlNode node in data)
                                {


                                    sum += Convert.ToDouble(node.ChildNodes[searchindex].InnerText);


                                }
                                MessageBox.Show("The Sum" + sum);
                            }
                            else if (order == "count")
                            {
                                searchindex = find_index(fieldname); // to know index
                                if (searchindex == -1)
                                {
                                    MessageBox.Show("field name is invalid");
                                    return;
                                }

                                List<int> findindex = new List<int>();
                                findindex.Add(searchindex);
                                display(findindex, data, datagrade);
                                MessageBox.Show("count : " + data.Count);
                            }
                        }
                    }
                }
                else if (col.Length > 1)
                {
                    List<int> arrfindindex = new List<int>();
                    for (int i = 0; i < col.Length; i++)
                    {
                        searchindex = find_index(col[i]);
                        if (searchindex == -1)
                        {
                            MessageBox.Show("Invalid fieldName");
                            return;
                        }
                        arrfindindex.Add(searchindex);
                    }
                    display(arrfindindex, data, datagrade);
                }
            }
            //-----------------------------------------------------------------------------------------
            else if (field.Length>4)
            {
                if (field[4] != "where") 
                {
                    MessageBox.Show("Error!!!!");
                    return;
                }
              if(field.Length==8)//one condition
              {
                    bool b = checkcondition(field[5], field[6]);
                    if(b==false)
                    {
                        return;
                    }
                  List<int> rowindex = condition_result(field[6], field[5], field[7],data);
                  if (field[1] == "*")
                  {
                      gridview_show(rowindex, data, datagrade);
                  }
                  else
                  {
                      List<int> arrfindindex = new List<int>();
                      int x;
                      for(int i=0;i<col.Length;i++)
                      {
                          x = find_index(col[i]);
                          if(x==-1)
                          {
                              MessageBox.Show("Invalid Field Name!!!");
                              return;
                          }
                          arrfindindex.Add(x);
                      }
                      gridview_show(rowindex, arrfindindex, data, datagrade);
                  }


              }
              else if(field.Length>8)//more than one condition
              {
                  if(field[8]!="AND"&&field[8]!="OR")
                  {
                      MessageBox.Show("Invalid Link!!!");
                      return;
                  }
                  bool b1 = checkcondition(field[5], field[6]);
                  bool  b2= checkcondition(field[9], field[10]);
                  if(b1==false||b2==false)
                  {
                        return;
                  }
                  List<int> rowindex1 = condition_result(field[6], field[5], field[7], data);
                  List<int> rowindex2 = condition_result(field[10], field[9], field[11], data);
                  List<int>finals=final(rowindex1,rowindex2,field[8],data);
                  if (field[1] == "*")
                  {
                      gridview_show(finals, data, datagrade);
                  }
                  else
                  {
                      List<int> arrfindindex = new List<int>();
                      int x;
                      for (int i = 0; i < col.Length; i++)
                      {
                          x = find_index(col[i]);
                          if (x == -1)
                          {
                              MessageBox.Show("Invalid Field Name!!!");
                              return;
                          }
                          arrfindindex.Add(x);
                      }
                      gridview_show(finals, arrfindindex, data, datagrade);
                  }

              }
                  
            }

             
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            datagrade.Visible = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Please Enter Your Quary....")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Please Enter Your Quary....";
                textBox1.ForeColor = Color.Blue;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

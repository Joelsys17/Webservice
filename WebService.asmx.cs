using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;

namespace WebApplication1
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        private SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-5C3JSVR;Initial Catalog=Demo Database NAV (5-0);Integrated Security=True;");

        [WebMethod]
        public String GetWebsiteHtml(String url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string result = reader.ReadToEnd();
            stream.Dispose();
            reader.Dispose();
            return result;
        }

        [WebMethod]
        public ArrayList objects()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM sys.objects WHERE schema_id = SCHEMA_ID('dbo')", con);
                DataTable list = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                list.TableName = "All objects";
                sda.Fill(list);
                ArrayList rows = new ArrayList();
                foreach (DataRow dataRow in list.Rows)
                    rows.Add(string.Join(";", dataRow.ItemArray.Select(item => item.ToString())));
                return rows;

            }
            catch (SqlException)
            {
                throw;
            }
        }

        [WebMethod]
        public ArrayList javaobjects()
        {
            try
            {
                ArrayList array = new ArrayList();
                ArrayList listColumns = new ArrayList(getObjects().Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray());
                var columnNames = getObjects().Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                foreach (DataRow row in getObjects().Rows)
                {
                    ArrayList ab = new ArrayList();
                    foreach (DataColumn column in getObjects().Columns)
                    {
                        if (row[column.ColumnName] != null)
                        {
                            string stringArr = row[column.ColumnName].ToString();

                            ab.Add(stringArr);
                        }
                    }
                    array.Add(ab);
                }
                return array;
            }
            catch (SqlException)
            {
                throw;
            }
        }


        //* [WebMethod]
        // public ArrayList DCJava()
        //  {
        //        foreach (DataColumn column in getObjects().Columns)
        //      {
        //        Console.WriteLine(column.ColumnName);
        //  }
        // return
        // Denna är i LINQ. getObjects() är resultatet av en fylld och fungerande datatable
        // *}
        [WebMethod]
        public ArrayList FillJava()
        {
            ArrayList array = new ArrayList();
            foreach (DataRow r in getObjects().Rows)
            {
                ArrayList row = new ArrayList();
                foreach (object value in r.ItemArray)
                    row.Add(value);

                array.Add(row);
            }
            return array;
        }
        public ArrayList dsToArray()
        {
            ArrayList array = new ArrayList();
            DataTable dt = getObjects();

            ArrayList listColumns = new ArrayList();
            foreach (DataColumn column in dt.Columns)
                listColumns.Add(column.ColumnName);
            array.Add(listColumns);

            foreach (DataRow r in dt.Rows)
            {
                ArrayList row = new ArrayList();
                foreach (object value in r.ItemArray)
                    row.Add(value);

                array.Add(row);
            }
            return array;
        }
        [WebMethod]
        public ArrayList getTable()
        {
            ArrayList list = new ArrayList();
            object[] o = dsToArray().ToArray();
            for (int i = 0; i < o.Length; i++)
            {
                ArrayList row = new ArrayList();
                Object[] content = (Object[])o[i];
                for (int j = 0; j < content.Length; j++)
                {
                    row.Add(content[j].ToString());
                }
                list.Add(row);
            }
            return list;
        }

        [WebMethod]
        public List<Object> ToArray()
        {
            var ret = Array.CreateInstance(typeof(object), getObjects().Rows.Count, getObjects().Columns.Count) as object[,];
            for (var i = 0; i < getObjects().Rows.Count - 1; i++)
                for (var j = 0; j < getObjects().Columns.Count - 1; j++)
                    ret[i, j] = getObjects().Rows[i][j];
            List<object> list = ret.OfType<Object>().ToList();
            return list;
        }
        [WebMethod]
        public System.Data.DataTable getObjects()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT [First Name], [Last Name] FROM [CRONUS Sverige AB$Employee]", con);
                DataTable list = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                list.TableName = "All objects";
                sda.Fill(list);
                return list;
            }
            catch (SqlException)
            {
                throw;
            }
        }

        [WebMethod]
        public DataTable sqlstring()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM sys.objects WHERE schema_id = SCHEMA_ID('dbo')", con);
                DataTable dt2 = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                dt2.TableName = "All objects 1";
                sda.Fill(dt2);
                return dt2;
            }
            catch (SqlException)
            {
                throw;
            }
        }
        //Hej//
        [WebMethod]
        public void Upload(byte[] contents, string filenamesave)
        {
            var appData = Server.MapPath("C:/Web");
            var file = Path.Combine(appData, Path.GetFileName(filenamesave));
            File.WriteAllBytes(file, contents);
        }

        [WebMethod]
        public String txtFile(String filename)
        {
            StreamReader sr = File.OpenText(filename);
            {
                String line = sr.ReadToEnd();
                return line;
            }
        }
        [WebMethod]
        public DataTable Get()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [CRONUS Sverige AB$Employee], [CRONUS Sverige AB$Employee Absence],[CRONUS Sverige AB$Employee Portal Setup],[CRONUS Sverige AB$Employee Qualification],[CRONUS Sverige AB$Employee Relative],[CRONUS Sverige AB$Employee Statistics Group], [CRONUS Sverige AB$Warehouse Employee]", con);
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                dt.TableName = "Uppgift 1";

                sda.Fill(dt);
                return dt;
            }
            catch (SqlException)
            {
                throw;
            }
        }

        [WebMethod]
        public DataTable Get1()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [CRONUS Sverige AB$Employee], [CRONUS Sverige AB$Employee Relative] where [Employee No_] = No_", con);
                DataTable dt1 = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                dt1.TableName = "Uppgift 2";
                sda.Fill(dt1);
                return dt1;
            }
            catch (SqlException)
            {
                throw;
            }
        }
        [WebMethod]
        public DataTable Get2()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM[CRONUS Sverige AB$Employee Absence], [CRONUS Sverige AB$Employee] where[Employee No_] = No_ and[From Date] like '%2004%'", con);
                DataTable dt2 = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                dt2.TableName = "Uppgift 3";
                sda.Fill(dt2);
                return dt2;
            }
            catch (SqlException)
            {
                throw;
            }
        }
        public List<string> get2java()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM[CRONUS Sverige AB$Employee Absence], [CRONUS Sverige AB$Employee] where[Employee No_] = No_ and[From Date] like '%2004%'", con);
                SqlDataReader dr = cmd.ExecuteReader();
                List<string> str = new List<string>();
                while (dr.Read())
                {
                    str.Add(dr.GetValue(0).ToString());
                }
                dr.Close();
                return str;

            }
            catch (SqlException)
            {
                throw;
            }
        }

        [WebMethod]
        public ArrayList java()
        {
            ArrayList rows = new ArrayList();
            foreach (DataRow dataRow in Get1().Rows)
                rows.Add(string.Join(";", dataRow.ItemArray.Select(item => item.ToString())));
            return rows;
        }

        [WebMethod]
        public DataTable Get3()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT [First Name], count(*)  as 'Sickdays' FROM [CRONUS Sverige AB$Employee Absence], [CRONUS Sverige AB$Employee] where [Employee No_] = No_  group by [First Name] Order by COUNT(*) DESC", con);
                DataTable dt3 = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                dt3.TableName = "Uppgift 4";
                sda.Fill(dt3);
                return dt3;
            }
            catch (SqlException)
            {
                throw;
            }
        }
        [WebMethod]
        public DataTable Get4()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT OBJECT_NAME (OBJECT_ID) AS NameofConstraint, SCHEMA_NAME (schema_id) AS SchemaName,OBJECT_NAME (parent_object_id) AS TableName,type_desc AS ConstraintType FROM sys.objects WHERE type_desc IN ('FOREIGN_KEY_CONSTRAINT', 'PRIMARY_KEY_CONSTRAINT')", con);
                DataTable dt4 = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                dt4.TableName = "Uppgift 5";
                sda.Fill(dt4);
                return dt4;
            }
            catch (SqlException)
            {
                throw;
            }
        }

        [WebMethod]
        public DataTable Get5()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT so.name AS TableName, si.name AS IndexName, si.type_desc AS IndexType FROM sys.indexes si JOIN sys.objects so ON si.[object_id] = so.[object_id] WHERE so.type = 'U'--Only get indexes for User Created Tables AND si.name IS NOT NULL ORDER BY so.name, si.type", con);
                DataTable dt5 = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                dt5.TableName = "Uppgift 6";
                sda.Fill(dt5);
                return dt5;
            }
            catch (SqlException)
            {
                throw;
            }
        }
        [WebMethod]
        public DataTable Get6()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT TableName = t.Name, ColumnName = c.Name, dc.Name, dc.definition FROM sys.tables t INNER JOIN sys.default_constraints dc ON t.object_id = dc.parent_object_id INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND c.column_id = dc.parent_column_id ORDER BY t.Name", con);
                DataTable dt6 = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                dt6.TableName = "Uppgift 7";
                sda.Fill(dt6);
                return dt6;
            }
            catch (SqlException)
            {
                throw;
            }
        }
        [WebMethod]
        public DataTable Get7()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'", con);
                DataTable dt7 = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                dt7.TableName = "Uppgift 8";
                sda.Fill(dt7);
                return dt7;
            }
            catch (SqlException)
            {
                throw;
            }
        }
        [WebMethod]
        public DataTable Get8()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [CRONUS Sverige AB$Employee]", con);
                DataTable dt8 = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                dt8.TableName = "Uppgift 9";
                sda.Fill(dt8);
                return dt8;
            }
            catch (SqlException)
            {
                throw;
            }
        }
        [WebMethod]
        public DataTable FindUpdateEmployee(string upnr)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT first_name, last_name, job_title, adress FROM [CRONUS Sverige AB$Employee] WHERE No_ LIKE '%" + upnr + "%'", con);
                DataTable dt9 = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                dt9.TableName = "Employee";
                sda.Fill(dt9);
                return dt9;
            }
            catch (SqlException)
            {
                throw;
            }
        }



        [WebMethod]
        public void DeleteEmploye(string delnr)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("DELETE * FROM[CRONUS Sverige AB$Employee] where No_ LIKE '%" + delnr + "%'", con);
            }
            catch (SqlException)
            {
                throw;
            }
        }

        [WebMethod]
        public void AddEmployee(string sosnr, string name, string adress, string lastname, string worktitle)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO [CRONUS Sverige AB$Employee](No_, First_name, last_name, adress, work_title) VALUES (" + sosnr + "," + name + "," + lastname + "," + adress + "," + worktitle + ")", con);
            }
            catch (SqlException)
            {
                throw;
            }
        }

        [WebMethod]
        public void UpdateEmployee(string sosnr, string name, string adress, string lastname, string worktitle)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE [CRONUS Sverige AB$Employee] SET first_name = " + name + ", last_name =" + lastname + ", adress =" + adress + ", work_title = " + worktitle + "WHERE No_ LIKE '%" + sosnr + "%'", con);
            }
            catch (SqlException)
            {
                throw;
            }
        }
        [WebMethod]
        public ArrayList javaget1()
        {
            ArrayList arrayList = new ArrayList(javaset1());
            return arrayList;
        }
        public List<DataRow> javaset1()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM[CRONUS Sverige AB$Employee Absence], [CRONUS Sverige AB$Employee] where[Employee No_] = No_ and[From Date] like '%2004%'", con);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt2 = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                dt2.TableName = "Uppgift 3";
                sda.Fill(dt2);
                List<DataRow> list1 = new List<DataRow>();
                foreach (DataRow tt in dt2.Rows)
                {
                    list1.Add(tt);
                }
                return list1;

            }
            catch (SqlException)
            {
                throw;
            }
        }


        [WebMethod]
        public DataTable FindEmployee(string sosnr)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [CRONUS Sverige AB$Employee] where No_ LIKE'%" + sosnr + "%'", con);
                DataTable dt12 = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                dt12.TableName = "Delete search";
                sda.Fill(dt12);
                return dt12;

            }
            catch (SqlException)
            {
                throw;
            }
        }
    }
}

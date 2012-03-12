using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;

namespace PictureUploader
{
    public class PixDBInterface
    {
        public PixDBInterface()
        {
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            appDataDir += "\\PersonalPictureManager\\";
            Directory.CreateDirectory(appDataDir);

            m_dbFilename = appDataDir + m_dbFilename;

            m_connectionString = string.Format("DataSource=\"{0}\";Password='{1}'", m_dbFilename, m_dbPassword);
            
            m_insertPixSql = "insert into PixUploaded (Filename, Path, MD5, DateSent) values (@Filename, @Path, @CRC, @DateSent)";
            
            m_insertConfigSql = "insert into ConfigValues(Name, Value) values (@Name, @Value)";
            
            m_insertDirSql = "insert into DirList(Directory, Type) values (@Directory, @Type)";
            m_removeDirSql = "delete from DirList where Directory='@Directory'";
            m_remvoeAllDirsSql = "delete from DirList";

            m_insertPixQueueSql = "insert into PixQueue(Filename) values (@filename)";
            m_removePixQueueSql = "delete from PixQueue where Filename='@filename'";
        }

        #region DatabaseSetup
        
        private string m_dbFilename = "PersonalPictureManagerDB.sdf";
        private string m_dbPassword = "85crxsi";
        private static string m_connectionString;

        public bool CreateDatabase()
        {
            bool created = false;

            if (File.Exists(m_dbFilename))
            { return created; }

            SqlCeEngine en = new SqlCeEngine(m_connectionString);
            en.CreateDatabase();
            created = true;

            return created;
        }

        public void SetupTables()
        {
            // create table for uploaded pix
            string sql = "create table PixUploaded ("
                        + "Filename nvarchar (255) not null "
                        + ",MD5 nvarchar (255) "
                        + ",Path nvarchar (1024)"
                        + ",DateSent nvarchar (25)"
                        + ")";
            ExecNonQuery(sql);


            // create table for directories to be indexed
            sql = "create table DirList ("
                        + "Directory nvarchar (2048) not null "
                        + ",Type int  "
                        + ")";
            ExecNonQuery(sql);

            // create table for configuration
            sql = "create table ConfigValues ("
                        + "Name nvarchar (2048) not null "
                        + ",Value nvarchar (2048) not null "
                        + ")";
            ExecNonQuery(sql);

            sql = "create table Log ("
                        + "Entry nvarchar (2048) not null "
                        + ")";
            ExecNonQuery(sql);

            // create table for configuration
            sql = "create table PixQueue ("
                        + "Filename nvarchar (2048) not null "
                        + ")";
            ExecNonQuery(sql);
        }

        public static void Log(string inData)
        {
            string sqlCmd = "insert into Log(Entry) values ('" + inData + "')";
            SqlCeConnection cn = new SqlCeConnection(m_connectionString);
            if (cn.State == ConnectionState.Closed)
            { cn.Open(); }

            SqlCeCommand cmd;
            try
            {
                cmd = new SqlCeCommand(sqlCmd, cn);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception)
            { }
        }

        public bool CheckIfDbSetup()
        {
            bool done = false;
            string sql = "select * from ConfigValues";
            SqlCeConnection cn = null;
            try
            {
                cn = new SqlCeConnection(m_connectionString);
                if (cn.State == ConnectionState.Closed)
                { cn.Open(); }
                SqlCeCommand cmd = new SqlCeCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                // if you don’t set the result set to 
                // scrollable HasRows does not work
                SqlCeResultSet rs = cmd.ExecuteResultSet(ResultSetOptions.Scrollable);

                // If you need to be able to update the result set, instead use:
                // SqlCeResultSet rs = cmd.ExecuteResultSet(
                // ResultSetOptions.Scrollable | ResultSetOptions.Updatable);
                if (rs.HasRows)
                {
                    done = true;
                }
            }
            catch (Exception)
            { }
            finally
            { 
                if (cn != null)
                    cn.Close(); 
            }

            return done;
        }
        
        private void ExecNonQuery(string sqlCmd)
        {
            SqlCeConnection cn = new SqlCeConnection(m_connectionString);
            if (cn.State == ConnectionState.Closed)
            { cn.Open(); }

            SqlCeCommand cmd;
            try
            {
                cmd = new SqlCeCommand(sqlCmd, cn);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception)
            { }

        }
        #endregion

        #region CopyPixToServer

        private string m_insertPixSql;
        public void AddSentPicture(string filename, string path, string MD5, string dateSent)
        {
            SqlCeConnection cn = new SqlCeConnection(m_connectionString);
            if (cn.State == ConnectionState.Closed)
            { cn.Open(); }

            try
            {
                SqlCeCommand cmd = new SqlCeCommand(m_insertPixSql, cn);
                cmd.Parameters.AddWithValue("@Filename", filename);
                cmd.Parameters.AddWithValue("@Path", path);
                cmd.Parameters.AddWithValue("@CRC", MD5);
                cmd.Parameters.AddWithValue("@DateSent", dateSent);
                cmd.ExecuteNonQuery();
            }
            catch (Exception /* e */)
            {
            }
            finally
            {
                cn.Close();
            }
        }
        public bool CheckIfSent(string MD5)
        {
            bool found = false;

            SqlCeConnection cn = new SqlCeConnection(m_connectionString);
            if (cn.State == ConnectionState.Closed)
            { cn.Open(); }

            // Build the sql query. If this was real life,
            // I’d use a parameter for the where bit
            // to avoid SQL Injection attacks. 
            string sql = "select * from PixUploaded where MD5='" + MD5 + "'";

            try
            {
                SqlCeCommand cmd = new SqlCeCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                // if you don’t set the result set to 
                // scrollable HasRows does not work
                SqlCeResultSet rs = cmd.ExecuteResultSet(ResultSetOptions.Scrollable);

                // If you need to be able to update the result set, instead use:
                // SqlCeResultSet rs = cmd.ExecuteResultSet(
                // ResultSetOptions.Scrollable | ResultSetOptions.Updatable);
                if (rs.HasRows)
                {

                    // Use the get ordinal function so you don’t 
                    // have to worry about remembering what 
                    // order your SQL put the field names in. 
                    //int ordLastName = rs.GetOrdinal("DateSent");
                    //string date = rs.GetString(ordLastName);
                    found = true;
                    //while (rs.Read())
                    //{
                    //output.AppendLine(rs.GetString(ordFirstname)+ ” “ + rs.GetString(ordLastName));
                    //}

                }
            }
            catch (Exception /*ex*/)
            {
            }
            finally
            {
                // Don’t need it anymore so we’ll be good and close it.
                // in a ‘real life’ situation 
                // cn would likely be class level
                cn.Close();
            }
            return found;
        }
        
        #endregion

        #region Configuration
        
        private string m_insertConfigSql;
        public string ReadConfigValue(string inName)
        {
            string result = "";

            SqlCeConnection cn = new SqlCeConnection(m_connectionString);
            if (cn.State == ConnectionState.Closed)
            { cn.Open(); }

            string sql = "select * from ConfigValues where Name='" + inName + "'";
            try
            {
                SqlCeCommand cmd = new SqlCeCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                SqlCeResultSet rs = cmd.ExecuteResultSet(ResultSetOptions.Scrollable);
                if (rs.HasRows)
                {
                    rs.Read();

                    int valueOrdinal = rs.GetOrdinal("Value");
                    result = rs.GetString(valueOrdinal);
                }
            }
            catch (Exception ex)
            {
                int i = 0;
            }
            finally
            {
                cn.Close();
            }
            return result;
        }

        public bool AddConfigValue(string inName, string inValue)
        {
            bool added = false;

            SqlCeConnection cn = new SqlCeConnection(m_connectionString);
            if (cn.State == ConnectionState.Closed)
            { cn.Open(); }

            try
            {
                SqlCeCommand cmd = new SqlCeCommand(m_insertConfigSql, cn);
                cmd.Parameters.AddWithValue("@Name", inName);
                cmd.Parameters.AddWithValue("@Value", inValue);
                cmd.ExecuteNonQuery();
                added = true;
            }
            catch (Exception /* e */)
            {
            }
            finally
            {
                cn.Close();
            }
            return added;
        }

        public void UpdateConfigValue(string inName, string inValue)
        {
            if (string.IsNullOrEmpty(ReadConfigValue(inName)))
            {
                AddConfigValue(inName, inValue);
            }
            else
            {
                string sql = "update ConfigValues set Value='" + inValue + "' where Name='" + inName + "'";
                ExecNonQuery(sql);
            }
        }
        #endregion

        #region DirWatcher
        private string m_insertDirSql;
        private string m_removeDirSql;
        private string m_remvoeAllDirsSql;

        public List<string> ReadDirsToWatch(int inType)
        {
            List<string> retList = new List<string>();

            SqlCeConnection cn = new SqlCeConnection(m_connectionString);
            if (cn.State == ConnectionState.Closed)
            { cn.Open(); }

            string sql = "select * from DirList";

            try
            {
                SqlCeCommand cmd = new SqlCeCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                SqlCeResultSet rs = cmd.ExecuteResultSet(ResultSetOptions.Scrollable);
                if (rs.HasRows)
                {
                    rs.Read();

                    int dirNameId = rs.GetOrdinal("Directory");
                    string dir = rs.GetString(dirNameId);
                    retList.Add(dir);

                    while (rs.Read())
                    {
                        retList.Add(rs.GetString(dirNameId));
                    }
                }
            }
            catch (Exception /*ex*/)
            {
            }
            finally
            {
                cn.Close();
            }
            return retList;
        }
        public bool AddDirToWatch(string inDir, int inType)
        {
            bool added = false;

            SqlCeConnection cn = new SqlCeConnection(m_connectionString);
            if (cn.State == ConnectionState.Closed)
            { cn.Open(); }

            try
            {
                SqlCeCommand cmd = new SqlCeCommand(m_insertDirSql, cn);
                cmd.Parameters.AddWithValue("@Directory", inDir);
                cmd.Parameters.AddWithValue("@Type", inType);
                cmd.ExecuteNonQuery();
                added = true;
            }
            catch (Exception /* e */)
            {
            }
            finally
            {
                cn.Close();
            }
            return added;
        }
        public bool RemoveDirToWatch(string inDir)
        {
            bool added = false;

            SqlCeConnection cn = new SqlCeConnection(m_connectionString);
            if (cn.State == ConnectionState.Closed)
            { cn.Open(); }

            try
            {
                SqlCeCommand cmd = new SqlCeCommand(m_removeDirSql, cn);
                cmd.Parameters.AddWithValue("@Directory", inDir);
                cmd.ExecuteNonQuery();
                added = true;
            }
            catch (Exception /* e */)
            {
            }
            finally
            {
                cn.Close();
            }
            return added;
        }
        public bool RemoveAllDirsToWatch()
        {
            bool added = false;

            SqlCeConnection cn = new SqlCeConnection(m_connectionString);
            if (cn.State == ConnectionState.Closed)
            { cn.Open(); }

            try
            {
                SqlCeCommand cmd = new SqlCeCommand(m_remvoeAllDirsSql, cn);
                cmd.ExecuteNonQuery();
                added = true;
            }
            catch (Exception /* e */)
            {
            }
            finally
            {
                cn.Close();
            }
            return added;
        }

        #endregion

        #region QueuedPixToSend
        private string m_insertPixQueueSql;
        private string m_removePixQueueSql;

        public bool AddFileToPixQueue(string inFile)
        {
            bool added = false;

            SqlCeConnection cn = new SqlCeConnection(m_connectionString);
            if (cn.State == ConnectionState.Closed)
            { cn.Open(); }

            try
            {
                SqlCeCommand cmd = new SqlCeCommand(m_insertPixQueueSql, cn);
                cmd.Parameters.AddWithValue("@filename", inFile);
                cmd.ExecuteNonQuery();
                added = true;
            }
            catch (Exception /* e */)
            {
            }
            finally
            {
                cn.Close();
            }
            return added;
        }

        public bool RemoveEntryFromPixQueue(string inFile)
        {
            bool added = false;

            SqlCeConnection cn = new SqlCeConnection(m_connectionString);
            if (cn.State == ConnectionState.Closed)
            { cn.Open(); }

            try
            {
                SqlCeCommand cmd = new SqlCeCommand(m_removePixQueueSql, cn);
                cmd.Parameters.AddWithValue("@filename", inFile);
                cmd.ExecuteNonQuery();
                added = true;
            }
            catch (Exception /* e */)
            {
            }
            finally
            {
                cn.Close();
            }
            return added;
        }

        public bool CheckIfInPixQueue(string inFile)
        {
            bool found = false;

            SqlCeConnection cn = new SqlCeConnection(m_connectionString);
            if (cn.State == ConnectionState.Closed)
            { cn.Open(); }

            try
            {
                string test = "select * from PixQueue where Filename='" + inFile + "'";
                SqlCeCommand cmd = new SqlCeCommand(test, cn);

                cmd.CommandType = CommandType.Text;
                string foo = cmd.CommandText;
                SqlCeResultSet rs = cmd.ExecuteResultSet(ResultSetOptions.Scrollable);
                if (rs.HasRows)
                {
                    rs.Read();
                    found = true;
                }
                
            }
            catch (Exception /* e */)
            {
            }
            finally
            {
                cn.Close();
            }
            return found;
        }

        public List<string> GetAllQueuedPix()
        {
            List<string> result = new List<string>();

            SqlCeConnection cn = new SqlCeConnection(m_connectionString);
            if (cn.State == ConnectionState.Closed)
            { cn.Open(); }

            string sql = "select * from PixQueue";
            try
            {
                SqlCeCommand cmd = new SqlCeCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                SqlCeResultSet rs = cmd.ExecuteResultSet(ResultSetOptions.Scrollable);
                if (rs.HasRows)
                {
                    rs.Read();

                    int valueOrdinal = rs.GetOrdinal("Filename");
                    result.Add(rs.GetString(valueOrdinal));
                }
            }
            catch (Exception ex)
            {
                int i = 0;
            }
            finally
            {
                cn.Close();
            }
            return result;
        }
        #endregion

    }
}
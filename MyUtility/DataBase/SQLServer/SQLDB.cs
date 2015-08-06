using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;

namespace MyUtility.DataBase
{
  public  class SQLDB
    {

      public bool AttachDB(string Strsqlconnection, string dataBaseFilePath, string dataBaseLogPath,string dataBaseName)
      { 
            StringBuilder attachSQL = new StringBuilder ();
            attachSQL.Append("use master EXEC sp_attach_db @dbname = N'");
            attachSQL.Append(dataBaseName);
            attachSQL.Append("',  @filename1 = N'");
            attachSQL.Append(dataBaseFilePath);
            attachSQL.Append("',  @filename2 = N'");
            attachSQL.Append(dataBaseFilePath);
            attachSQL.Append("'");
            return false;
      
      
      
      }












    }
}

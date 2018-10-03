using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPIServiceII.Models;

/// <summary>
/// Summary description for DBUtils
/// </summary>
public class DBUtils
{
    public DBUtils()
    {
    }

    public Transactions AddTransactions(Transactions trans)
    {

        List<string> error = new List<string>();
       
        string val = "";
        try
        {
            var cb = new SqlConnectionStringBuilder();
     
            cb.ConnectionString = "Data Source=openshift.database.windows.net;Initial Catalog=BankAccountDB;User ID=user;Password=database@12345";

            SqlTransaction transObj = null;
            using (SqlConnection conn = new SqlConnection(cb.ConnectionString))
            {
                conn.Open();
                transObj = conn.BeginTransaction();

                SqlCommand cmd1 = new SqlCommand("INSERT INTO Transactions VALUES (@ACCOUNTNUMBER_DEBITED, @ACCOUNTNUMBER_CREDITED, @AMOUNT, @TRANSACTIONNUMBER, @Date)", conn);
                //cmd.Parameters.AddWithValue("@Id", user.Id);                  
                cmd1.Parameters.AddWithValue("@ACCOUNTNUMBER_DEBITED", trans.AccountNumber_Debited);
                cmd1.Parameters.AddWithValue("@ACCOUNTNUMBER_CREDITED", trans.AccountNumber_Credited);
                cmd1.Parameters.AddWithValue("@AMOUNT", trans.Amount);
                cmd1.Parameters.AddWithValue("@TRANSACTIONNUMBER", trans.TransactionNumber);
                cmd1.Parameters.AddWithValue("@Date", trans.Date);

               // SqlCommand cmd2 = new SqlCommand("update table [dbo].[UserAccount] set ", conn);

                try {
                    cmd1.ExecuteNonQuery();
                    //cmd2.ExecuteNonQuery(); // Throws exception due to foreign key constraint  

                    transObj.Commit();
                }
                catch (Exception ex)
                {
                    transObj.Rollback();
                }
                

            }
        }
        catch (SqlException e)
        {
            val = e.StackTrace;
            error.Add("SqlException");
            error.Add(e.Number.ToString());
            for (int i = 0; i < e.Errors.Count; i++)
            {
                error.Add(e.Errors[i].ToString());
            }

            //  throw new Exception(e.StackTrace);


        }
        catch (Exception e)
        {
            val = e.StackTrace;
            error.Add("exception");
            error.Add(e.StackTrace);
            error.Add(e.Message);
            // throw new Exception(e.StackTrace);

        }

        return trans;

    }





}
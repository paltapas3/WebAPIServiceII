﻿using System;
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

    public Transactions UpdateTransactions(Transactions trans)
    {

        List<string> error = new List<string>();
        Random generator = new Random();
        //string TransNumber = "DU" + generator.Next(0, 99999);

        string val = "";
        try
        {
            var cb = new SqlConnectionStringBuilder();
     
            cb.ConnectionString = "Data Source=openshift.database.windows.net;Initial Catalog=BankAccountDB;User ID=user;Password=database@12345";

            SqlTransaction transObj = null;
            using (SqlConnection conn = new SqlConnection(cb.ConnectionString))
            {
                conn.Open();
                

               
                
                //Debiting Amount
                SqlCommand cmd2 = new SqlCommand("update [dbo].[UserAccount] set U_BALANCE=U_BALANCE-@AMOUNT where U_ACCOUNTNUMBER=@ACCOUNTNUMBER_DEBITED", conn);
                cmd2.Parameters.AddWithValue("@AMOUNT", trans.Amount);
                cmd2.Parameters.AddWithValue("@ACCOUNTNUMBER_DEBITED", trans.AccountNumber_Debited);
                //Crediting Amount
                SqlCommand cmd3 = new SqlCommand("update [dbo].[UserAccount] set U_BALANCE=U_BALANCE+@AMOUNT where U_ACCOUNTNUMBER=@ACCOUNTNUMBER_CREDITED", conn);
                cmd3.Parameters.AddWithValue("@AMOUNT", trans.Amount);
                cmd3.Parameters.AddWithValue("@ACCOUNTNUMBER_CREDITED", trans.AccountNumber_Credited);
                //Updating Transactions table

                SqlCommand cmd1 = new SqlCommand("INSERT INTO Transactions VALUES (@ACCOUNTNUMBER_DEBITED, @ACCOUNTNUMBER_CREDITED, @AMOUNT, @TRANSACTIONNUMBER, @Date)", conn);

                cmd1.Parameters.AddWithValue("@ACCOUNTNUMBER_DEBITED", trans.AccountNumber_Debited);
                cmd1.Parameters.AddWithValue("@ACCOUNTNUMBER_CREDITED", trans.AccountNumber_Credited);
                cmd1.Parameters.AddWithValue("@AMOUNT", trans.Amount);
                cmd1.Parameters.AddWithValue("@TRANSACTIONNUMBER", trans.TransactionNumber);
                cmd1.Parameters.AddWithValue("@Date", trans.Date);

                transObj = conn.BeginTransaction();
                cmd2.Transaction = transObj;
                cmd3.Transaction = transObj;
                cmd1.Transaction = transObj;
                try {
                  //  cmd1.ExecuteNonQuery();
                    int debit=cmd2.ExecuteNonQuery();
                    int credit=cmd3.ExecuteNonQuery();
                    if (debit > 0 && credit > 0)
                    {
                        cmd1.ExecuteNonQuery();
                    }

                    transObj.Commit();
                    return trans;
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


    public List<Transactions> getTransactions()
    {
        List<string> error = new List<string>();
        List<Transactions> transList = new List<Transactions>();
        Transactions transaction;

        try
        {
            var cb = new SqlConnectionStringBuilder();
            cb.ConnectionString = "Data Source=openshift.database.windows.net;Initial Catalog=BankAccountDB;User ID=user;Password=database@12345";

            using (SqlConnection connection = new SqlConnection("Data Source=openshift.database.windows.net;Initial Catalog=BankAccountDB;User ID=user;Password=database@12345"))
            {



                using (SqlCommand command = new SqlCommand("select * from Transactions", connection))
                {
                    connection.Open();
                    SqlDataReader result = command.ExecuteReader();
                    while (result.Read())
                    {
                        transaction = new Transactions();
                        transaction.Tid =Convert.ToInt32(result["T_ID"]);
                        transaction.AccountNumber_Debited = Convert.ToInt32(result["ACCOUNTNUMBER_DEBITED"]);
                        transaction.AccountNumber_Credited = Convert.ToInt32(result["ACCOUNTNUMBER_CREDITED"]);
                        transaction.Amount = Convert.ToInt32(result["AMOUNT"]);
                        transaction.TransactionNumber = result["TRANSACTIONNUMBER"].ToString();
                        transaction.Date = result["Date"].ToString();
                        transList.Add(transaction);

                        //error.Add(result["U_ID"].ToString()+","+ result["U_NAME"].ToString()+","+ result["U_ADDRESS"].ToString()+","+ result["U_PAN"].ToString()+","+ result["U_ACCOUNTTYPE"].ToString()+","+ result["U_BALANCE"].ToString()+","+ result["U_GENDER"].ToString()+","+ result["U_EMAIL"].ToString()+","+ result["U_DOB"].ToString());

                    }


                    connection.Close();
                }
            }
        }
        catch (SqlException e)
        {
            string errormsg = string.Empty;
            for (int i = 0; i < e.Errors.Count; i++)
            {
                errormsg = errormsg + e.Errors[i].ToString();
            }
            transList.Add(new Transactions { TransactionNumber = errormsg });


        }
        catch (Exception e)
        {
            transList.Add(new Transactions { TransactionNumber = e.StackTrace });

        }

        return transList;
    }





}
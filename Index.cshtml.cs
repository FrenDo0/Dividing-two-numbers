using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace testing.Pages
{
    public class IndexModel : PageModel
    {
        public Boolean contains = false;
        public double numberOne = 0;
        public double numberTwo = 0;
        public double result = 0;
        public int rowId = 0;
        public String connectionString = "Data Source=.\\tew_sqlexpress;Initial Catalog=storeNumbers;Integrated Security=True";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            contains = true;
            numberOne = double.Parse(Request.Form["numberOne"]);
            numberTwo = double.Parse(Request.Form["numberTwo"]);

            if(numberOne.ToString().Length == 0 || numberTwo.ToString().Length == 0)
            {
                Console.WriteLine("enter numbers");
            }
            else
            {
                rowId = selectId(numberOne, numberTwo);
                if(rowId != 0)
                {
                    result = numberOne / numberTwo;
                }
                else
                {
                    insertNumbers(numberOne, numberTwo);
                    rowId = selectId(numberOne, numberTwo);
                    result = numberOne / numberTwo;
                }
            }
            
        }
        public void insertNumbers(double numOne,double numTwo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO numbers (number_one,number_two) VALUES (@numOne,@numTwo)";
                    using(SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@numOne", numOne);
                        cmd.Parameters.AddWithValue("@numTwo", numTwo);
                
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong in insert method");
            }
        }
        public int selectId(double numOne,double numTwo)
        {
            int id = 0;
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT id FROM numbers WHERE number_one=@numOne AND number_two=@numTwo";
                    
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@numOne", numOne);
                        cmd.Parameters.AddWithValue("@numTwo", numTwo);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            
                            while (reader.Read())
                            {
                                id = reader.GetInt32(0);
                                cmd.ExecuteReader();
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Something went wrong in insert method");
            }
            return id;
        }
    }
}

using System;
using System.Data.SqlClient;

namespace UserDataAccess
{
    public class SignupDataAccess
    {
        string connstring = "server=localhost;database=student;user=sa;password=wintellect";
        SqlConnection sqlConnection;

        public async Task<bool> InsertEmployeeDetails(Employee enquiryType, string TransType)
        {
            var result = 0;
            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("Usp_InsertEmployeeDetails", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpID", enquiryType.Empid);
                cmd.Parameters.AddWithValue("@Trantype", TransType);
                cmd.Parameters.AddWithValue("@EmpRollnumber", enquiryType.EmpName);
                cmd.Parameters.AddWithValue("@EmpName", enquiryType.EmpName);
                cmd.Parameters.AddWithValue("@EmpAddress", enquiryType.EmpAddress);
                result = cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception ex)
            { }

            return result >= 1;
        }
    }
}
}

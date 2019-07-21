using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserWebservice.Models;

namespace UserWebservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        string connstring = "Server=(localhost)\\MSSQLLocalDB;Database=userdb;Trusted_Connection=True;MultipleActiveResultSets=true";
        SqlConnection sqlConnection;
        [HttpPost]
        public bool Signup(string username, string password)
        {
            UserDetails user = new UserDetails();
            try
            {
                user.UserEmail = username;
                user.Password = password;
                user.AccessCode = GenerateCode();
                SendUsermail(user);
                return InsertUser(user);
            }
            catch (Exception ex)
            { return false; }
        }

        [HttpPost]
        public bool SignupCompletion(string username, string fullname, string address,long contactno,bool preference)
        {
            UserDetails user = new UserDetails();
            try
            {
                user.FullName = fullname;
                user.Address = address;
                user.ContactNumber = contactno;
                user.Preference = preference;
                return UpdateUserDetails(user);
            }
            catch (Exception ex)
            { return false; }
        }

        [HttpPost]
        public bool SignIn(string username, string password,string accesskey)
        {
            try
            {
                return CheckUser(username,password,accesskey);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string GenerateCode()
        {
            StringBuilder randomText = new StringBuilder();
            string alphabets = "012345679ACEFGHKLMNPRSWXZabcdefghijkhlmnopqrstuvwxyz";
            Random r = new Random();
            for (int j = 0; j <= 5; j++)
            {
                randomText.Append(alphabets[r.Next(alphabets.Length)]);
            }

            return randomText.ToString();
        }
        private string SendUsermail(UserDetails user)
        {
            string result;
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("marimuthuinfotech@gmail.com");
                mail.To.Add(user.UserEmail);
                mail.Subject = "Signup Activation Mail";
                mail.IsBodyHtml = true;
                mail.Body = "<p>Hi " + user.UserEmail + "</p>";
                mail.Body += "<p> Thanks for sign up in this website.please use below Activation code for login purpose</p>";
                mail.Body += "<p> Activation Code :" + user.AccessCode + "</p>";
                mail.Body += "<br/><br/> <p>Thanks  <br/> Admin Team </p>";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("marimuthuinfotech@gmail.com", "password");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                result = "success";
            }
            catch (Exception ex)
            {
                result = "failure";
            }
            return result;
        }

        private bool InsertUser(UserDetails user)
        {
            var result = 0;
            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("Usp_InsertUserDetails", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmailId", user.UserEmail);
                cmd.Parameters.AddWithValue("@Trantype", "Insert");
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Accesskey", user.AccessCode);
                result = cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception ex)
            { }

            return result >= 1;

        }
        private bool UpdateUserDetails(UserDetails user)
        {
            var result = 0;
            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("Usp_InsertUserDetails", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmailId", user.UserEmail);
                cmd.Parameters.AddWithValue("@Trantype", "Update");                
                cmd.Parameters.AddWithValue("@FullName", user.FullName);
                cmd.Parameters.AddWithValue("@ContactNumber", user.ContactNumber);
                cmd.Parameters.AddWithValue("@Address", user.Address);
                cmd.Parameters.AddWithValue("@Preference", user.Preference);
                result = cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
            }

            return result >= 1;

        }
        private bool CheckUser(string useremail,string password,string accesskey)
        {
            var result = 0;
            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("Usp_CheckUserDetails", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmailId", useremail);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Accesskey", accesskey);
                result = cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
            }

            return result >= 1;

        }
    }
}

using Azure.Core;
using BookStore.API.Common;
using BookStore.API.Data.Models;
using BookStore.API.Data.Repository.Interface;
using Hl7.Fhir.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.Azure.Documents;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static BookStore.API.Common.Response;
using MimeKit;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using MailKit.Net.Smtp;
using ContentType = MimeKit.ContentType;

namespace BookStore.API.Data.Repository.Implementation
{
    public class AccountRepository: IAccountRepository
    {
        
        private readonly BookStoreContext _db;
        string strqry = string.Empty;
        private string DBconstring = String.Empty;
      
        public AccountRepository(BookStoreContext db)
        {
            _db = db;
            DBconstring = db.Database.GetConnectionString();// db.Database.GetConnectionString("BookStoreDB");
        }   

        
        
        //<summary>
        ///method is used to create user 
        /// </summary>
        public async Task<ServiceResult<string>> create_user(AddUser jsonrequestobj)
        {
            ServiceResult<string> resultobj = new ServiceResult<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(DBconstring))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_CREATE_USER", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region parameters
                        cmd.Parameters.AddWithValue("@FirstName", jsonrequestobj.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", jsonrequestobj.LastName);
                        cmd.Parameters.AddWithValue("@Email", jsonrequestobj.Email);
                        cmd.Parameters.AddWithValue("@Password", jsonrequestobj.Password);
                        cmd.Parameters.AddWithValue("@MobileNumber", jsonrequestobj.MobileNumber);
                        #endregion

                        con.Open();
                        int k = cmd.ExecuteNonQuery();
                        if (k != 0)
                        {
                            resultobj.Code = ServiceResultCode.Ok;
                            //resultobj.Result = Convert.ToString(dt.Rows[0]["CONTRACT_NUM"]);
                            resultobj.ErrorMessage = "User sucessfully created";
                        }                        
                        else
                        {
                            //resultobj.Code = ServiceResultCode.NotFound;
                            resultobj.Result = "";
                            resultobj.ErrorMessage = "No user created";
                        }
                        con.Close();
                    }

                }
                return resultobj;
            }
            catch (Exception ex)
            {
                resultobj.Code = ServiceResultCode.Conflict;
                resultobj.Result = "";
                resultobj.ErrorMessage = ex.Message;
                return resultobj;
            }
        }

        ///<summary>
        ///method of login details
        /// </summary>
        public async Task<ServiceResult<string>> users(LoginModel user)
        {
            ServiceResult<string> resultobj = new ServiceResult<string>();
            ResponseName responseName = new ResponseName();
            try
            {
                string MstrSQL = string.Empty;
                string WhrerClause = user.Email;
                MstrSQL = " select * from AddUser where Email= @Email" + " and Password= @Password";
                using (SqlConnection con = new SqlConnection(DBconstring))
                {
                    using (SqlCommand cmd = new SqlCommand(MstrSQL, con))
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                        cmd.ExecuteScalar();                        
                        SqlDataReader rdr = cmd.ExecuteReader();                      
                        if (rdr.Read())
                        {                           
                                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JWT:UseSecretKey"));
                                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                                var tokeOptions = new JwtSecurityToken(
                                    issuer: "https://localhost:7274",
                                    audience: "https://localhost:5001",
                                    claims: new List<Claim>(),
                                    expires: DateTime.Now.AddMinutes(5),
                                    signingCredentials: signinCredentials
                                );
                                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                                //responseName.accessToken = Convert.ToString(tokenString);                              

                            resultobj.Code = ServiceResultCode.Ok;
                            resultobj.Result = Convert.ToString(tokenString); ;
                            resultobj.ErrorMessage = "successfully loggedin!";
                        }
                        else
                        {
                            resultobj.Code = ServiceResultCode.NotFound;
                            resultobj.Result = "";
                            resultobj.ErrorMessage = "No user registered with this credentials";
                        }
                    }
                    con.Close();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                resultobj.Code = ServiceResultCode.Unauthorized;
                resultobj.Result = "";
                resultobj.ErrorMessage = ex.Message;
                return resultobj;
            }

        }


        ///<summary>
        ///method for user list details 
        ///</summary>
        public async Task<List<AddUser>> user_details(string Email, string Password,int UserId)
        {
            List<AddUser> addUsers = new List<AddUser>();
            AddUser obj;
            try
            {
                string MstrSQL = string.Empty;
                string strwhere = string.Empty;
                string area_id = string.Empty;
                MstrSQL= " select * from AddUser where Email='"+Email+"' or Password = '"+Password+"' or UserId= "+UserId+"" ;
                using (SqlConnection connection = new SqlConnection(DBconstring))
                {
                    SqlCommand cmd = new SqlCommand(MstrSQL, connection);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            obj = new AddUser();
                            
                            //obj.UserId = Convert.ToInt32(row["UserId"]);
                            obj.FirstName = row["FirstName"].ToString();
                            obj.LastName = row["LastName"].ToString(); ;
                            obj.Email = row["Email"].ToString();
                            obj.MobileNumber = row["MobileNumber"].ToString();
                            //similarily for all the property you want to get
                            addUsers.Add(obj);
                        }
                    }
                }
                return addUsers;
            }
            catch (Exception ex)
            {
                addUsers = new List<AddUser>();
                obj = new AddUser
                {
                    errormessage = ex.Message
                };
                addUsers.Add(obj);
                return addUsers;
            }
        }

       

        ///<summary>
        ///method to get category list details 
        ///</summary>
        public async Task<List<Categories>> category_details()
        {
            List<Categories> addCategories= new List<Categories>();
            Categories obj;
            try
            {
                string MstrSQL = string.Empty;
                //MstrSQL = "Exec SP_Validate_USER";
                MstrSQL = " select * from Categories";


                using (SqlConnection connection = new SqlConnection(DBconstring))
                {
                    SqlCommand cmd = new SqlCommand(MstrSQL, connection);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            obj = new Categories();

                            obj.CategoryId = Convert.ToInt32(row["CategoryId"]);
                            obj.Category = Convert.ToString(row["Category"]);

                            addCategories.Add(obj);
                        }
                    }
                }
                return addCategories;
            }
            catch (Exception ex)
            {
                addCategories = new List<Categories>();
                obj = new Categories
                {
                    errormessage = ex.Message
                };
                addCategories.Add(obj);
                return addCategories;
            }
        }

        ///<summary>
        ///method to get subcategory list details 
        ///</summary>
        public async Task<List<SubCategories>> subcategory_details(int CategoryId, string Category, string SubCategory)
        {
            List<SubCategories> addsubCategories = new List<SubCategories>();
            SubCategories obj;
            try
            {
                string MstrSQL = string.Empty;
                //MstrSQL = "Exec SP_Validate_USER";
                MstrSQL = " select * from SubCategories where SubCategory = '"+ SubCategory + "' or Category = '"+ Category + "' Or CategoryId='" + CategoryId + "' ";


                using (SqlConnection connection = new SqlConnection(DBconstring))
                {
                    SqlCommand cmd = new SqlCommand(MstrSQL, connection);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            obj = new SubCategories();

                            obj.CategoryId = Convert.ToInt32(row["CategoryId"]);
                            obj.SubCategory = Convert.ToString(row["SubCategory"]);

                            addsubCategories.Add(obj);
                        }
                    }
                }
                return addsubCategories;
            }
            catch (Exception ex)
            {
                addsubCategories = new List<SubCategories>();
                obj = new SubCategories
                {
                    errormessage = ex.Message
                };
                addsubCategories.Add(obj);
                return addsubCategories;
            }
        }

        //<summary>
        ///method is used to create user 
        /// </summary>
        public async Task<ServiceResult<string>> add_delivery_address(DeliveryAddress jsonrequestobj)
        {
            ServiceResult<string> resultobj = new ServiceResult<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(DBconstring))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ADD_Delivery_Address", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region parameters
                        cmd.Parameters.AddWithValue("@FirstName", jsonrequestobj.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", jsonrequestobj.LastName);
                        cmd.Parameters.AddWithValue("@Address", jsonrequestobj.Address);
                        cmd.Parameters.AddWithValue("@City", jsonrequestobj.City);
                        cmd.Parameters.AddWithValue("@State", jsonrequestobj.State);
                        cmd.Parameters.AddWithValue("@DeliveryMail", jsonrequestobj.DeliveryMail); 
                        cmd.Parameters.AddWithValue("@UserName", jsonrequestobj.UserName);
                        cmd.Parameters.AddWithValue("@Place", jsonrequestobj.Place);
                        cmd.Parameters.AddWithValue("@ZIP", jsonrequestobj.ZIP);

                        #endregion

                        con.Open();
                        int k = cmd.ExecuteNonQuery();
                        if (k != 0)
                        {
                            resultobj.Code = ServiceResultCode.Ok;
                            resultobj.ErrorMessage = "Delivery address sucessfully added";
                        }
                        else
                        {
                            resultobj.Result = "";
                            resultobj.ErrorMessage = "elivery address not saved";
                        }
                        con.Close();
                    }

                }
                return resultobj;
            }
            catch (Exception ex)
            {
                resultobj.Code = ServiceResultCode.Conflict;
                resultobj.Result = "";
                resultobj.ErrorMessage = ex.Message;
                return resultobj;
            }
        }


        ///<summary>
        ///method to get user address
        /// </summary>
        public async Task<List<DeliveryAddress>> get_user_address(string UserName)
        {
            List<DeliveryAddress> getUserAddress = new List<DeliveryAddress>();
            DeliveryAddress obj;
            try
            {
                string MstrSQL = string.Empty;
                MstrSQL = " select * from DeliveryAddress where UserName ='"+ UserName + "' ";
                using (SqlConnection connection = new SqlConnection(DBconstring))
                {
                    SqlCommand cmd = new SqlCommand(MstrSQL, connection);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            obj = new DeliveryAddress();

                            obj.FirstName = Convert.ToString(row["FirstName"]);
                            obj.LastName = Convert.ToString(row["LastName"]);
                            obj.Address = Convert.ToString(row["Address"]);
                            obj.City = Convert.ToString(row["City"]);
                            obj.State = Convert.ToString(row["State"]);
                            obj.DeliveryMail = Convert.ToString(row["DeliveryMail"]);
                            obj.UserName = Convert.ToString(row["UserName"]);
                            obj.Place = Convert.ToString(row["Place"]);
                            obj.ZIP = Convert.ToString(row["ZIP"]);
                            getUserAddress.Add(obj);
                        }
                    }
                }
                return getUserAddress;
            }
            catch (Exception ex)
            {
                getUserAddress = new List<DeliveryAddress>();
                obj = new DeliveryAddress
                {
                    errormessage = ex.Message
                };
                getUserAddress.Add(obj);
                return getUserAddress;
            }
        }

        //<summary>
        ///method is used to create Rating and Reviews 
        /// </summary>
        public async Task<ServiceResult<string>> add_reviews(RatingReview jsonrequestobj)
        {
            ServiceResult<string> resultobj = new ServiceResult<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(DBconstring))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_RatingReviews", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region parameters
                        cmd.Parameters.AddWithValue("@ProductId", jsonrequestobj.ProductId);
                        cmd.Parameters.AddWithValue("@Review", jsonrequestobj.Review);
                        cmd.Parameters.AddWithValue("@Rating", jsonrequestobj.Rating);
                        cmd.Parameters.AddWithValue("@Title", jsonrequestobj.Title);
                        cmd.Parameters.AddWithValue("@CustomerName", jsonrequestobj.CustomerName);
                        cmd.Parameters.AddWithValue("@UserName", jsonrequestobj.UserName);
                        cmd.Parameters.AddWithValue("@Image", jsonrequestobj.Image);                       
                        
                        #endregion
                        con.Open();
                        int k = cmd.ExecuteNonQuery();
                        if (k != 0)
                        {
                            resultobj.Code = ServiceResultCode.Ok;
                            resultobj.ErrorMessage = "Reviews saved sucessfully.";
                        }
                        else
                        {
                            resultobj.Result = "";
                            resultobj.ErrorMessage = "reviews not saved";
                        }
                        con.Close();
                    }
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                resultobj.Code = ServiceResultCode.Conflict;
                resultobj.Result = "";
                resultobj.ErrorMessage = ex.Message;
                return resultobj;
            }
        }



    }
}

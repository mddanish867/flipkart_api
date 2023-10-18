using BookStore.API.Common;
using BookStore.API.Data.Models;
using BookStore.API.Data.Repository.Interface;
using Microsoft.Azure.Documents;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static BookStore.API.Common.Response;

namespace BookStore.API.Data.Repository.Implementation
{
    public class CartRepository:ICartRepository
    {
        private readonly BookStoreContext _db;
        string strqry = string.Empty;
        private string DBconstring = String.Empty;
        public CartRepository(BookStoreContext db)
        {
            _db = db;
            DBconstring = db.Database.GetConnectionString();
        }
        //<summary>
        ///method is used to add product into cart 
        /// </summary>
        public async Task<ServiceResult<string>> addto_cart(Cart jsonrequestobj)
        {
            ServiceResult<string> resultobj = new ServiceResult<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(DBconstring))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ADDTO_CART", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region parameters                        
                        cmd.Parameters.AddWithValue("@ProductId", jsonrequestobj.ProductId);
                        cmd.Parameters.AddWithValue("@UserName", jsonrequestobj.UserName);

                        #endregion

                        con.Open();
                        int k = cmd.ExecuteNonQuery();
                        if (k != 0)
                        {
                            resultobj.Code = ServiceResultCode.Ok;
                            resultobj.ErrorMessage = "Product added sucessfully.";
                        }
                        else
                        {
                            //resultobj.Code = ServiceResultCode.NotFound;
                            resultobj.Result = "";
                            resultobj.ErrorMessage = "No Product Added.";
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
        ///method to get producs details 
        ///</summary>
        public async Task<List<Products>> cart_details(string UserName, int ProductId)
        {
            List<Products> addCartDetails = new List<Products>();
            Products obj;
            try
            {
                string MstrSQL = string.Empty;
                MstrSQL = " select * from Products left join Cart on Products.ProductId = Cart.ProductId where Cart.UserName= '" + UserName + "' Or Cart.ProductId = " + ProductId + "";
                //MstrSQL = " select * from Cart where ProductId = " + ProductId + " Or ProductName = '" + ProductName + "' Or ProductDescription = '" + ProductDescription + "' Or ProductPrice = " + ProductPrice + " Or ProductDiscount = " + ProductDiscount + " Or ProductQuanitity = " + ProductQuanitity + " Or ProductImageurl = '" + ProductImageurl + "' Or Status = " + Status + " Or Category = '" + Category + "' Or SubCategory = '" + SubCategory + "' Or rating = " + rating + " Or Color='" + Color + "' Or Size = '" + Size + "' Or PercDiscount = " + PercDiscount + " Or Type = '" + Type + "' Or Brand = '" + Brand + "' Or Material= '" + Material + "'";


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
                            obj = new Products();
                            obj.ProductId = Convert.ToInt32(row["ProductId"]);
                            if (!string.IsNullOrEmpty(Convert.ToString(row["ProductName"]).Trim()))
                            {
                                obj.ProductName = Convert.ToString(row["ProductName"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["ProductDescription"]).Trim()))
                            {
                                obj.ProductDescription = Convert.ToString(row["ProductDescription"]);
                            }
                            if (Convert.ToDecimal(row["ProductPrice"]) != 0)
                            {
                                obj.ProductPrice = Convert.ToDecimal(row["ProductPrice"]);
                            }
                            if (Convert.ToDecimal(row["ProductDiscount"]) != 0)
                            {
                                obj.ProductDiscount = Convert.ToDecimal(row["ProductDiscount"]);
                            }
                            if (Convert.ToInt32(row["ProductQuanitity"]) != 0)
                            {
                                obj.ProductQuanitity = Convert.ToInt32(row["ProductQuanitity"]);
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(row["AddedOn"]).Trim()))
                            {
                                obj.AddedOn = Convert.ToDateTime(row["AddedOn"]);

                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["ProductImageurl"]).Trim()))
                            {
                                obj.ProductImageurl = Convert.ToString(row["ProductImageurl"]);

                            }
                            if (Convert.ToInt32(row["Status"]) != 0)
                            {
                                obj.Status = Convert.ToInt32(row["Status"]);

                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Category"]).Trim()))
                            {
                                obj.Category = Convert.ToString(row["Category"]);

                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["SubCategory"]).Trim()))
                            {
                                obj.SubCategory = Convert.ToString(row["SubCategory"]);

                            }
                            if (row["rating"] == DBNull.Value)
                            {
                                obj.rating = 0;
                            }
                            else
                            {
                                obj.rating = Convert.ToDecimal(row["rating"]);

                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Color"]).Trim()))
                            {
                                obj.Color = Convert.ToString(row["Color"]);

                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Size"]).Trim()))
                            {
                                obj.Size = Convert.ToString(row["Size"]);

                            }                           
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Type"]).Trim()))
                            {
                                obj.Type = Convert.ToString(row["Type"]);

                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Brands"]).Trim()))
                            {
                                obj.Brands = Convert.ToString(row["Brands"]);

                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Material"]).Trim()))
                            {
                                obj.Material = Convert.ToString(row["Material"]);

                            }                          
                          
                           
                            addCartDetails.Add(obj);
                        }
                    }
                }
                return addCartDetails;
            }
            catch (Exception ex)
            {
                addCartDetails = new List<Products>();
                obj = new Products
                {
                    errormessage = ex.Message
                };
                addCartDetails.Add(obj);
                return addCartDetails;
            }
        }

        ///<summary>
        ///method is used to remove product from cart
        /// </summary>
        public async Task<ServiceResult<string>> remove_product(int ProductId)
        {
            ServiceResult<string> resultobj = new ServiceResult<string>();
            try
            {
                string result = string.Empty;
                string MstrSQL = string.Empty;                
                    MstrSQL = "DELETE FROM Cart where ProductId=" + ProductId + "";  
                
                var rowseffexcted = _db.Database.ExecuteSqlRaw(MstrSQL);
                if (rowseffexcted > 0)
                {
                    resultobj.Code = ServiceResultCode.Ok;
                    //resultobj.Result = Convert.ToString();
                    resultobj.ErrorMessage = "Product removed from cart successfully.";
                }
                else
                {
                    resultobj.Code = ServiceResultCode.NotFound;
                    resultobj.Result = "";
                    resultobj.ErrorMessage = "No product is found on given criteria";
                }
                return resultobj;
            }

            catch (Exception ex)
            {
                resultobj.Code = ServiceResultCode.Conflict;
                resultobj.ErrorMessage = ex.Message;
                return resultobj;
            }

        }

    }
}

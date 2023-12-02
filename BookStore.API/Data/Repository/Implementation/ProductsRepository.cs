using BookStore.API.Common;
using BookStore.API.Data.Models;
using BookStore.API.Data.Repository.Interface;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Azure.Documents;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static BookStore.API.Common.Response;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BookStore.API.Data.Repository.Implementation
{
    public class ProductsRepository:IProductsRepository
    {
        private readonly BookStoreContext _db;
        string strqry = string.Empty;
        private string DBconstring = String.Empty;
        public ProductsRepository(BookStoreContext db)
        {
            _db = db;
            DBconstring = db.Database.GetConnectionString();// db.Database.GetConnectionString("BookStoreDB");
        }

        ///<summary>
        ///method is used to create contract 
        /// </summary>
        public async Task<ServiceResult<string>> add_products(Products jsonrequestobj)
        {
            ServiceResult<string> resultobj = new ServiceResult<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(DBconstring))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_AddProducts", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region parameters
                        cmd.Parameters.AddWithValue("@ProductName", jsonrequestobj.ProductName);
                        cmd.Parameters.AddWithValue("@ProductDescription", jsonrequestobj.ProductDescription);
                        cmd.Parameters.AddWithValue("@ProductPrice", jsonrequestobj.ProductPrice);
                        cmd.Parameters.AddWithValue("@ProductDiscount", jsonrequestobj.ProductDiscount);
                        cmd.Parameters.AddWithValue("@ProductQuanitity", jsonrequestobj.ProductQuanitity);
                        cmd.Parameters.AddWithValue("@AddedOn", jsonrequestobj.AddedOn);
                        cmd.Parameters.AddWithValue("@ProductImageurl", jsonrequestobj.ProductImageurl);
                        cmd.Parameters.AddWithValue("@Status", jsonrequestobj.Status);
                        cmd.Parameters.AddWithValue("@Category", jsonrequestobj.Category);
                        cmd.Parameters.AddWithValue("@SubCategory", jsonrequestobj.SubCategory);

                        //many others parameters are comming through json body we can write similarily
                        #endregion

                        con.Open();
                        int k = cmd.ExecuteNonQuery();
                        if (k != 0)
                        {
                            resultobj.Code = ServiceResultCode.Ok;
                            //resultobj.Result = Convert.ToString(dt.Rows[0]["CONTRACT_NUM"]);
                            resultobj.ErrorMessage = "Product added sucessfully";
                        }
                        else
                        {
                            //resultobj.Code = ServiceResultCode.NotFound;
                            resultobj.Result = "";
                            resultobj.ErrorMessage = "No product added";
                        }
                        con.Close();


                        //SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        //DataTable dt = new();
                        //sda.Fill(dt);
                        //if (dt.Rows.Count > 0)
                        //{
                        //    resultobj.Code = ServiceResultCode.Ok;
                        //    //resultobj.Result = Convert.ToString(dt.Rows[0]["CONTRACT_NUM"]);
                        //    resultobj.ErrorMessage = "Product added sucessfully";
                        //}
                       
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
        ///method to get producs list details 
        ///</summary>
        public async Task<List<Products>> get_products(int ProductId, string ProductName, string ProductDescription, decimal ProductPrice, decimal ProductDiscount, int ProductQuanitity, string ProductImageurl, int Status, string Category, string SubCategory, decimal rating, string Color, string Size, double PercDiscount, string Type, string Brands, string Material, string Sleeve, string Fabrick, string NeckType, string Pattern)
        {
            List<Products> addProducts = new List<Products>();
            Products obj;
            try
            {
                string MstrSQL = string.Empty;
                //MstrSQL = "Exec SP_Validate_USER";
                MstrSQL = " select * from Products where ProductId in ("+ProductId+") Or ProductName in('"+ProductName+"') Or ProductDescription in('"+ProductDescription+"') Or ProductPrice in ("+ ProductPrice + ") Or ProductDiscount in ("+ ProductDiscount + ") Or ProductQuanitity in ("+ ProductQuanitity + ") Or ProductImageurl in ('"+ ProductImageurl + "') Or Status in ("+ Status +") Or Category in ('" + Category + "') Or SubCategory in ('"+ SubCategory + "') Or rating in ("+ rating + ") Or Color in ('"+ Color + "') Or Size in ('"+ Size + "') Or PercDiscount in( "+ PercDiscount + ") Or Type in ('"+ Type + "') Or Brands in ('"+ Brands + "') Or Material in ('"+ Material + "') Or Sleeve in ('"+ Sleeve + "') Or Fabrick in ('" + Fabrick + "') Or NeckType in ('"+ NeckType + "') Or Pattern in( '"+ Pattern + "')";


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
                            if (Convert.ToDecimal(row["ProductPrice"]) !=0)
                            {
                                obj.ProductPrice = Convert.ToDecimal(row["ProductPrice"]);
                            }
                            if (Convert.ToDecimal(row["ProductDiscount"]) !=0)
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
                            if (Convert.ToInt32(row["Status"]) !=0 )
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
                            if (row["rating"]  == DBNull.Value)
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
                            if (row["PercDiscount"] == DBNull.Value)
                            {
                                obj.rating = 0;
                            }
                            else 
                            {
                                obj.PercDiscount = Convert.ToDouble(row["PercDiscount"]);

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
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Sleeve"]).Trim()))
                            {
                                obj.Sleeve = Convert.ToString(row["Sleeve"]);

                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Fabrick"]).Trim()))
                            {
                                obj.Fabrick = Convert.ToString(row["Fabrick"]);

                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["NeckType"]).Trim()))
                            {
                                obj.NeckType = Convert.ToString(row["NeckType"]);

                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Pattern"]).Trim()))
                            {
                                obj.Pattern = Convert.ToString(row["Pattern"]);

                            }
                            addProducts.Add(obj);
                        }
                    }
                }
                return addProducts;
            }
            catch (Exception ex)
            {
                addProducts = new List<Products>();
                obj = new Products
                {
                    errormessage = ex.Message
                };
                addProducts.Add(obj);
                return addProducts;
            }
        }

        ///<summary>
        ///method to get producs details 
        ///</summary>
        public async Task<List<ProductDetails>> products_details(int ProductId)
        {
            List<ProductDetails> addProductsDetails = new List<ProductDetails>();
            ProductDetails obj;
            try
            {
                string MstrSQL = string.Empty;
                //MstrSQL = "Exec SP_Validate_USER";
                MstrSQL = " select Products.ProductId,Image.ImageUrl from Products Inner Join Image ON Products.ProductId=Image.ProductId where Products.ProductId ="+ProductId+"";


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
                            obj = new ProductDetails();
                            obj.ProductId = Convert.ToInt32(row["ProductId"]);
                            
                            if (!string.IsNullOrEmpty(Convert.ToString(row["ImageUrl"]).Trim()))
                            {
                                obj.ImageUrl = Convert.ToString(row["ImageUrl"]);

                            }
                            
                            addProductsDetails.Add(obj);
                        }
                    }
                }
                return addProductsDetails;
            }
            catch (Exception ex)
            {
                addProductsDetails = new List<ProductDetails>();
                obj = new ProductDetails
                {
                    errormessage = ex.Message
                };
                addProductsDetails.Add(obj);
                return addProductsDetails;
            }
        }

        ///<summary>
        ///method is used to create/add products into wishlist
        /// </summary>
        public async Task<ServiceResult<string>> add_favourite(Faverites jsonrequestobj)
        {
            ServiceResult<string> resultobj = new ServiceResult<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(DBconstring))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_AddProductstoWishList", con))
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
                            resultobj.ErrorMessage = "Product added sucessfully";
                        }
                        else
                        {
                            resultobj.Result = "";
                            resultobj.ErrorMessage = "No product added";
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
        ///method to get wish list details 
        ///</summary>
        public async Task<List<Products>> get_favourite_products(string UserName)
        {
            List<Products> addProducts = new List<Products>();
            Products obj;
            try
            {
                string MstrSQL = string.Empty;
                MstrSQL = " select * from Products left join Faverites on Products.ProductId = Faverites.ProductId where Faverites.UserName= '" + UserName + "'";

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
                            if (row["PercDiscount"] == DBNull.Value)
                            {
                                obj.rating = 0;
                            }
                            else
                            {
                                obj.PercDiscount = Convert.ToDouble(row["PercDiscount"]);
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
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Sleeve"]).Trim()))
                            {
                                obj.Sleeve = Convert.ToString(row["Sleeve"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Fabrick"]).Trim()))
                            {
                                obj.Fabrick = Convert.ToString(row["Fabrick"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["NeckType"]).Trim()))
                            {
                                obj.NeckType = Convert.ToString(row["NeckType"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Pattern"]).Trim()))
                            {
                                obj.Pattern = Convert.ToString(row["Pattern"]);
                            }
                            addProducts.Add(obj);
                        }
                    }
                }
                return addProducts;
            }
            catch (Exception ex)
            {
                addProducts = new List<Products>();
                obj = new Products
                {
                    errormessage = ex.Message
                };
                addProducts.Add(obj);
                return addProducts;
            }
        }

        ///<summary>
        ///method to filter the products 
        /// </summary>      
        public async Task<List<Products>> filter_products(int MinPrice, int MaxPrice, string Brands, int Discount, string Size, string Color, string Sleeves)
        {
            List<Products> filterProducts = new List<Products>();
            Products obj;
            try
            {
                using (SqlConnection con = new SqlConnection(DBconstring))
                {
                    SqlCommand cmd = new SqlCommand("SP_Filter_Products", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    #region parameters
                    cmd.Parameters.AddWithValue("@MinPrice", MinPrice);
                    cmd.Parameters.AddWithValue("@MaxPrice", MaxPrice);
                    cmd.Parameters.AddWithValue("@Brands", Brands);
                    cmd.Parameters.AddWithValue("@Discount", Discount);
                    cmd.Parameters.AddWithValue("@Size", Size);
                    cmd.Parameters.AddWithValue("@Color", Color);
                    cmd.Parameters.AddWithValue("@Sleeves", Sleeves);
                    #endregion
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
                            if (row["PercDiscount"] == DBNull.Value)
                            {
                                obj.rating = 0;
                            }
                            else
                            {
                                obj.PercDiscount = Convert.ToDouble(row["PercDiscount"]);
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
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Sleeve"]).Trim()))
                            {
                                obj.Sleeve = Convert.ToString(row["Sleeve"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Fabrick"]).Trim()))
                            {
                                obj.Fabrick = Convert.ToString(row["Fabrick"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["NeckType"]).Trim()))
                            {
                                obj.NeckType = Convert.ToString(row["NeckType"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Pattern"]).Trim()))
                            {
                                obj.Pattern = Convert.ToString(row["Pattern"]);
                            }
                            filterProducts.Add(obj);
                        }
                    }

                }
                return filterProducts;
            }
            catch (Exception ex)
            {
                filterProducts = new List<Products>();
                obj = new Products
                {
                    errormessage = ex.Message
                };
                filterProducts.Add(obj);
                return filterProducts;
            }
        }

        ///<summary>
        ///method to rating of the products 
        /// </summary>  
        public async Task<List<RatingReview>> product_ratings(int ProductId)
        {
            List<RatingReview> productsrating = new List<RatingReview>();
            RatingReview obj;
            try
            {
                string MstrSQL = string.Empty;
                MstrSQL = " Select Review,Rating from RatingReview where ProductId = " + ProductId + " ";

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
                            obj = new RatingReview();
                            //obj.ProductId = Convert.ToInt32(row["ProductId"]);                            
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Review"]).Trim()))
                            {
                                obj.Review = Convert.ToString(row["Review"]);
                            }
                            obj.Rating = Convert.ToInt32(row["Rating"]);                            
                            productsrating.Add(obj);
                        }
                    }
                }
                return productsrating;
            }
            catch (Exception ex)
            {
                productsrating = new List<RatingReview>();
                obj = new RatingReview
                {
                    errormessage = ex.Message
                };
                productsrating.Add(obj);
                return productsrating;
            }
        }

        ///<summary>
        ///method to retrieve rating details
        /// </summary>  
        public async Task<List<RatingReview>> product_ratings_details(int ProductId)
        {
            List<RatingReview> productsrating = new List<RatingReview>();
            RatingReview obj;
            try
            {
                string MstrSQL = string.Empty;
                MstrSQL = "Select Rating,Title,CustomerName,Review,Image,ReviewedAt,City from RatingReview Inner join DeliveryAddress ON RatingReview.UserName = DeliveryAddress.UserName where ProductId = "+ProductId+"";

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
                            obj = new RatingReview();
                            //obj.ProductId = Convert.ToInt32(row["ProductId"]);
                            //obj.TotalRating = Convert.ToInt32(row["TotalRating"]);
                            //obj.TotalReviews = Convert.ToInt32(row["TotalReviews"]);
                            //obj.AverageRating = Convert.ToInt32(row["AverageRating"]);
                            obj.Rating = Convert.ToInt32(row["Rating"]);
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Title"]).Trim()))
                            {
                                obj.Title = Convert.ToString(row["Title"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["CustomerName"]).Trim()))
                            {
                                obj.CustomerName = Convert.ToString(row["CustomerName"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Review"]).Trim()))
                            {
                                obj.Review = Convert.ToString(row["Review"]);
                            }                           
                                obj.ReviewedAt = Convert.ToDateTime(row["ReviewedAt"]);                            

                            if (!string.IsNullOrEmpty(Convert.ToString(row["City"]).Trim()))
                            {
                                obj.City = Convert.ToString(row["City"]);
                            }
                            productsrating.Add(obj);
                        }
                    }
                }
                return productsrating;
            }
            catch (Exception ex)
            {
                productsrating = new List<RatingReview>();
                obj = new RatingReview
                {
                    errormessage = ex.Message
                };
                productsrating.Add(obj);
                return productsrating;
            }
        }


        ///<summary>
        ///method to rating of the products 
        /// </summary>  
        public async Task<List<RatingReview>> purchased_product_ratings(int ProductId)
        {
            List<RatingReview> productsrating = new List<RatingReview>();
            RatingReview obj;
            try
            {
                string MstrSQL = string.Empty;
                MstrSQL = "Select Distinct ProductName,ProductImageUrl,productId from Orders Where ProductId="+ProductId+"";

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
                            obj = new RatingReview();
                            obj.ProductId = Convert.ToInt32(row["ProductId"]);
                            if (!string.IsNullOrEmpty(Convert.ToString(row["ProductName"]).Trim()))
                            {
                                obj.ProductName = Convert.ToString(row["ProductName"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["ProductImageUrl"]).Trim()))
                            {
                                obj.ProductImageUrl = Convert.ToString(row["ProductImageUrl"]).Replace(System.Environment.NewLine, string.Empty);
                            }
                            productsrating.Add(obj);
                        }
                    }
                }
                return productsrating;
            }
            catch (Exception ex)
            {
                productsrating = new List<RatingReview>();
                obj = new RatingReview
                {
                    errormessage = ex.Message
                };
                productsrating.Add(obj);
                return productsrating;
            }
        }

        ///<summary>
        ///method to filter the rating of the products 
        /// </summary>  
        public async Task<List<RatingReview>> product_ratings_filter(int ProductId,string Rating, string Recent)
        {
            List<RatingReview> productsrating = new List<RatingReview>();
            RatingReview obj;
            try
            {
                string MstrSQL = string.Empty;
                if(Rating == "Negative")
                {
                    MstrSQL = "Select ProductId, Rating,Title,CustomerName,Review,Image,ReviewedAt,City from RatingReview Inner join DeliveryAddress ON RatingReview.UserName = DeliveryAddress.UserName where ProductId =" + ProductId + " and (Rating = 1 or Rating = 2) ";
                }
                else if(Rating == "Positive")
                {
                    MstrSQL = "Select ProductId, Rating,Title,CustomerName,Review,Image,ReviewedAt,City from RatingReview Inner join DeliveryAddress ON RatingReview.UserName = DeliveryAddress.UserName where ProductId =" + ProductId + " and (Rating = 3 or Rating = 4 or Rating = 5) ";
                }
                else if(Recent != "" && Recent != null)
                {
                    MstrSQL = "Select ProductId, Rating,Title,CustomerName,Review,Image,ReviewedAt,City from RatingReview Inner join DeliveryAddress ON RatingReview.UserName = DeliveryAddress.UserName where ProductId =" + ProductId + " order by RatingId desc";
                }
                else
                {
                    MstrSQL = "Select ProductId, Rating,Title,CustomerName,Review,Image,ReviewedAt,City from RatingReview Inner join DeliveryAddress ON RatingReview.UserName = DeliveryAddress.UserName where ProductId =" + ProductId + "";
                }

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
                            obj = new RatingReview();
                            obj.ProductId = Convert.ToInt32(row["ProductId"]);                            
                            obj.Rating = Convert.ToInt32(row["Rating"]);
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Title"]).Trim()))
                            {
                                obj.Title = Convert.ToString(row["Title"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["CustomerName"]).Trim()))
                            {
                                obj.CustomerName = Convert.ToString(row["CustomerName"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Review"]).Trim()))
                            {
                                obj.Review = Convert.ToString(row["Review"]);
                            }
                            obj.ReviewedAt = Convert.ToDateTime(row["ReviewedAt"]);

                            if (!string.IsNullOrEmpty(Convert.ToString(row["City"]).Trim()))
                            {
                                obj.City = Convert.ToString(row["City"]);
                            }
                            productsrating.Add(obj);
                        }
                    }
                }
                return productsrating;
            }
            catch (Exception ex)
            {
                productsrating = new List<RatingReview>();
                obj = new RatingReview
                {
                    errormessage = ex.Message
                };
                productsrating.Add(obj);
                return productsrating;
            }
        }


        ///<summary>
        ///method to retrieve Questinair details
        /// </summary>  
        public async Task<List<Questionair>> product_questionair_details(int ProductId, string search)
        {
            List<Questionair> productsQuestionair = new List<Questionair>();
            Questionair obj;
            try
            {
                string MstrSQL = string.Empty;
                if(search != null && search != "")
                {
                    MstrSQL = "Select ProductId,Questions,Answers,QuestioneDate,AnswerDate from QuestionAnswers where Answers Like '%"+ search +"%'";
                }
                else
                {
                    MstrSQL = "Select ProductId,Questions,Answers,QuestioneDate,AnswerDate from QuestionAnswers where ProductId = " + ProductId + "";
                }
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
                            obj = new Questionair();
                            obj.ProductId = Convert.ToInt32(row["ProductId"]);
                          
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Questions"]).Trim()))
                            {
                                obj.Questions = Convert.ToString(row["Questions"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(row["Answers"]).Trim()))
                            {
                                obj.Answers = Convert.ToString(row["Answers"]);
                            }                            
                            obj.QuestioneDate = Convert.ToDateTime(row["QuestioneDate"]);                            
                            obj.AnswerDate = Convert.ToDateTime(row["AnswerDate"]);
                            productsQuestionair.Add(obj);
                        }
                    }
                }
                return productsQuestionair;
            }
            catch (Exception ex)
            {
                productsQuestionair = new List<Questionair>();
                obj = new Questionair
                {
                    errormessage = ex.Message
                };
                productsQuestionair.Add(obj);
                return productsQuestionair;
            }
        }
    }
}

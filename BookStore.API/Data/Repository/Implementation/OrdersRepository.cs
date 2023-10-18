using BookStore.API.Common;
using BookStore.API.Data.Models;
using BookStore.API.Data.Repository.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BookStore.API.Data.Repository.Implementation
{
    public class OrdersRepository:IOrdersRepository
    {
        private readonly BookStoreContext _db;
        string strqry = string.Empty;
        private string DBconstring = String.Empty;
        public OrdersRepository(BookStoreContext db)
        {
            _db = db;
            DBconstring = db.Database.GetConnectionString();
        }

        //<summary>
        ///method is used to create orders 
        /// </summary>
        public async Task<ServiceResult<string>> create_orders(PreOrders jsonrequestobj)
        {
            ServiceResult<string> resultobj = new ServiceResult<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(DBconstring))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_CreatePreOrders", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region parameters
                        cmd.Parameters.AddWithValue("@ProductId", jsonrequestobj.ProductId);
                        cmd.Parameters.AddWithValue("@Quantity", jsonrequestobj.Quantity);
                        cmd.Parameters.AddWithValue("@Color", jsonrequestobj.Color);
                        cmd.Parameters.AddWithValue("@Size", jsonrequestobj.Size);
                        cmd.Parameters.AddWithValue("@UserName", jsonrequestobj.UserName);
                        #endregion
                        con.Open();
                        int k = cmd.ExecuteNonQuery();
                        if (k != 0)
                        {
                            resultobj.Code = ServiceResultCode.Ok;
                            resultobj.ErrorMessage = "order placed sucessfully";
                        }
                        else
                        {
                            resultobj.Result = "";
                            resultobj.ErrorMessage = "No order placed";
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

        //<summary>
        ///method is used to create Order 
        /// </summary>
        public async Task<ServiceResult<string>> add_orders(Orders jsonrequestobj)
        {
            ServiceResult<string> resultobj = new ServiceResult<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(DBconstring))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_Orders", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region parameters
                        cmd.Parameters.AddWithValue("@ProductName", jsonrequestobj.ProductName);
                        cmd.Parameters.AddWithValue("@ProductDescription", jsonrequestobj.ProductDescription);
                        cmd.Parameters.AddWithValue("@ProductPrice", jsonrequestobj.ProductPrice);
                        cmd.Parameters.AddWithValue("@ProductDiscount", jsonrequestobj.ProductDiscount);
                        cmd.Parameters.AddWithValue("@ProductQuanitity", jsonrequestobj.ProductQuanitity);
                        cmd.Parameters.AddWithValue("@PurchaseDate", jsonrequestobj.PurchaseDate);
                        cmd.Parameters.AddWithValue("@ProductImageurl", jsonrequestobj.ProductImageurl);
                        cmd.Parameters.AddWithValue("@Status", jsonrequestobj.Status);
                        cmd.Parameters.AddWithValue("@Category", jsonrequestobj.Category);
                        cmd.Parameters.AddWithValue("@SubCategory", jsonrequestobj.SubCategory);
                        cmd.Parameters.AddWithValue("@Type", jsonrequestobj.Type);
                        cmd.Parameters.AddWithValue("@Color", jsonrequestobj.Color);
                        cmd.Parameters.AddWithValue("@Material", jsonrequestobj.Material);
                        cmd.Parameters.AddWithValue("@Brand", jsonrequestobj.Brand);
                        cmd.Parameters.AddWithValue("@Size", jsonrequestobj.Size);
                        cmd.Parameters.AddWithValue("@rating", jsonrequestobj.rating);
                        cmd.Parameters.AddWithValue("@ProductId", jsonrequestobj.ProductId);
                        cmd.Parameters.AddWithValue("@username", jsonrequestobj.username);
                        cmd.Parameters.AddWithValue("@ShippingCharge", jsonrequestobj.ShippingCharge);
                        cmd.Parameters.AddWithValue("@Count", jsonrequestobj.Count);
                        cmd.Parameters.AddWithValue("@Address", jsonrequestobj.Address);
                        cmd.Parameters.AddWithValue("@Mobile", jsonrequestobj.Mobile);
                        cmd.Parameters.AddWithValue("@TotalAmount", jsonrequestobj.TotalAmount);
                        cmd.Parameters.AddWithValue("@Name", jsonrequestobj.Name);
                        cmd.Parameters.AddWithValue("@PaymentMode", jsonrequestobj.PaymentMode);
                        cmd.Parameters.AddWithValue("@OrderTrackId", jsonrequestobj.OrderTrackId);
                        #endregion
                        con.Open();
                        int k = cmd.ExecuteNonQuery();
                        if (k != 0)
                        {
                            resultobj.Code = ServiceResultCode.Ok;
                            resultobj.ErrorMessage = "Order placed sucessfully.";
                        }
                        else
                        {
                            resultobj.Result = "";
                            resultobj.ErrorMessage = "No order placed there are some error occured try again!";
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
        ///method to get order details 
        /// </summary>
        public async Task<List<Orders>> get_order_details(string UserName, string OrderTrackId)
        {
            List<Orders> getorderdetails = new List<Orders>();
            Orders obj;
            try
            {
                string MstrSQL = string.Empty;
                MstrSQL = " select * from Orders where username ='" + UserName + "' or OrderTrackId = '"+ OrderTrackId + "' ";
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
                            obj = new Orders();

                            obj.ProductName = Convert.ToString(row["ProductName"]);
                            obj.ProductDescription = Convert.ToString(row["ProductDescription"]);
                            obj.ProductPrice = Convert.ToDecimal(row["ProductPrice"]);
                            obj.ProductDiscount = Convert.ToDecimal(row["ProductDiscount"]);
                            obj.ProductQuanitity = Convert.ToInt32(row["ProductQuanitity"]);
                            if (!string.IsNullOrEmpty(Convert.ToString(row["PurchaseDate"]).Trim()))
                            {
                                obj.PurchaseDate = Convert.ToDateTime(row["PurchaseDate"]);
                            }
                            obj.ProductImageurl = Convert.ToString(row["ProductImageurl"]);
                            obj.Status = Convert.ToString(row["Status"]);
                            obj.Category = Convert.ToString(row["Category"]);
                            obj.SubCategory = Convert.ToString(row["SubCategory"]);
                            obj.Type = Convert.ToString(row["Type"]);
                            obj.Color = Convert.ToString(row["Color"]);
                            obj.Material = Convert.ToString(row["Material"]);
                            obj.Brand = Convert.ToString(row["Brand"]);
                            obj.Size = Convert.ToString(row["Size"]);
                            obj.rating = Convert.ToDecimal(row["rating"]);
                            obj.ProductId = Convert.ToInt32(row["ProductId"]);
                            obj.ShippingCharge = Convert.ToString(row["ShippingCharge"]);
                            obj.Count = Convert.ToInt32(row["Count"]);
                            obj.Address = Convert.ToString(row["Address"]);
                            obj.Mobile = Convert.ToString(row["Mobile"]);
                            obj.TotalAmount = Convert.ToDecimal(row["TotalAmount"]);
                            obj.Name = Convert.ToString(row["Name"]);
                            obj.PaymentMode = Convert.ToString(row["PaymentMode"]);
                            obj.OrderTrackId = Convert.ToString(row["OrderTrackId"]);

                            getorderdetails.Add(obj);
                        }
                    }
                }
                return getorderdetails;
            }
            catch (Exception ex)
            {
                getorderdetails = new List<Orders>();
                obj = new Orders
                {
                    errormessage = ex.Message
                };
                getorderdetails.Add(obj);
                return getorderdetails;
            }
        }

        ///<summary>
        /// method to filter the orders
        ///</summary>
           public async Task<List<Orders>> filter_orders(string Status)
        {
            List<Orders> getorderdetails = new List<Orders>();
            Orders obj;
            try
            {
                string MstrSQL = string.Empty;
                MstrSQL = " select * from Orders where Status in('"+Status+"') ";
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
                            obj = new Orders();

                            obj.ProductName = Convert.ToString(row["ProductName"]);
                            obj.ProductDescription = Convert.ToString(row["ProductDescription"]);
                            obj.ProductPrice = Convert.ToDecimal(row["ProductPrice"]);
                            obj.ProductDiscount = Convert.ToDecimal(row["ProductDiscount"]);
                            obj.ProductQuanitity = Convert.ToInt32(row["ProductQuanitity"]);
                            if (!string.IsNullOrEmpty(Convert.ToString(row["PurchaseDate"]).Trim()))
                            {
                                obj.PurchaseDate = Convert.ToDateTime(row["PurchaseDate"]);
                            }
                            obj.ProductImageurl = Convert.ToString(row["ProductImageurl"]);
                            obj.Status = Convert.ToString(row["Status"]);
                            obj.Category = Convert.ToString(row["Category"]);
                            obj.SubCategory = Convert.ToString(row["SubCategory"]);
                            obj.Type = Convert.ToString(row["Type"]);
                            obj.Color = Convert.ToString(row["Color"]);
                            obj.Material = Convert.ToString(row["Material"]);
                            obj.Brand = Convert.ToString(row["Brand"]);
                            obj.Size = Convert.ToString(row["Size"]);
                            obj.rating = Convert.ToDecimal(row["rating"]);
                            obj.ProductId = Convert.ToInt32(row["ProductId"]);
                            obj.ShippingCharge = Convert.ToString(row["ShippingCharge"]);
                            obj.Count = Convert.ToInt32(row["Count"]);
                            obj.Address = Convert.ToString(row["Address"]);
                            obj.Mobile = Convert.ToString(row["Mobile"]);
                            obj.TotalAmount = Convert.ToDecimal(row["TotalAmount"]);
                            obj.Name = Convert.ToString(row["Name"]);
                            obj.PaymentMode = Convert.ToString(row["PaymentMode"]);
                            obj.OrderTrackId = Convert.ToString(row["OrderTrackId"]);

                            getorderdetails.Add(obj);
                        }
                    }
                }
                return getorderdetails;
            }
            catch (Exception ex)
            {
                getorderdetails = new List<Orders>();
                obj = new Orders
                {
                    errormessage = ex.Message
                };
                getorderdetails.Add(obj);
                return getorderdetails;
            }
        }
    }
}

using BookStore.API.Common;
using BookStore.API.Data.Models;

namespace BookStore.API.Services.Interface
{
    public interface IOrdersService
    {
        //<summary>
        ///method is used to create/add orders 
        /// </summary>
        Task<ServiceResult<string>> create_orders(PreOrders jsonrequestobj);
        //<summary>
        ///method is used to create Order 
        /// </summary>
        Task<ServiceResult<string>> add_orders(Orders jsonrequestobj);

        ///<summary>
        ///method to get order details 
        /// </summary>
        Task<List<Orders>> get_order_details(string UserName, string OrderTrackId);

        ///<summary>
        /// method to filter the orders
        /// </summary>
        Task<List<Orders>> filter_orders(string Status);
    }
}

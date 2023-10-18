using BookStore.API.Common;
using BookStore.API.Data.Models;
using BookStore.API.Data.Repository.Implementation;
using BookStore.API.Data.Repository.Interface;
using BookStore.API.Services.Interface;

namespace BookStore.API.Services.Implementation
{
    public class OrdersService :IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrdersService(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        //<summary>
        ///method is used to create/update orders 
        /// </summary>
        public async Task<ServiceResult<string>> create_orders(PreOrders jsonrequestobj)
        {
            ServiceResult<string> resultobj = await _ordersRepository.create_orders(jsonrequestobj);
            return resultobj;
        }


        //<summary>
        ///method is used to create Order 
        /// </summary>
        public async Task<ServiceResult<string>> add_orders(Orders jsonrequestobj)
        {
            ServiceResult<string> resultobj = await _ordersRepository.add_orders(jsonrequestobj);
            return resultobj;
        }

        ///<summary>
        ///method to get order details 
        /// </summary>
        public async Task<List<Orders>> get_order_details(string UserName, string OrderTrackId)
        {
            List<Orders> getOrderDetails = await _ordersRepository.get_order_details(UserName, OrderTrackId);
            return getOrderDetails;
        }

        ///<summary>
        /// method to filter the orders
        /// </summary>
        public async Task<List<Orders>> filter_orders(string Status)
        {
            List<Orders> getOrderDetails = await _ordersRepository.filter_orders(Status);
            return getOrderDetails;
        }

    }
}

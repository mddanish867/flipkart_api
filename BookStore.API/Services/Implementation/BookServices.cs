using BookStore.API.Data.Repository.Interface;
using BookStore.API.Services.Interface;
using BookStore.API.Common.Settlements;

namespace BookStore.API.Services
{

    public class BookServices : IBookServices
    {
        private readonly IBookRepository _bookRepository;

        public BookServices(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        ///<summary>
        ///method of peanut specification list details
        /// </summary>
        public async Task<List<Peanut_Specification>> peanut_spect_exception_list(string comp_id, int crop_year, string buy_pt_id, string pnut_variety_id, string pnut_type_id, string seed_ind, string seg_type, string edible_oil_ind)
        {
            List<Peanut_Specification> peanutspecexceptionlist = await _bookRepository.peanut_spect_exception_list(comp_id, crop_year, buy_pt_id, pnut_variety_id, pnut_type_id, seed_ind, seg_type, edible_oil_ind);
            return peanutspecexceptionlist;
        }
    }
}


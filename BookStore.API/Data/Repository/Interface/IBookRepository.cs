using BookStore.API.Common.Settlements;
using BookStore.API.Data.Repository.Interface;
using System.Text;


namespace BookStore.API.Data.Repository.Interface
{
    public interface IBookRepository
    {
        ///<summary>
        ///method for peanut specification list details
        /// </summary>        
        Task<List<Peanut_Specification>> peanut_spect_exception_list(string comp_id, int crop_year, string buy_pt_id, string pnut_variety_id, string pnut_type_id, string seed_ind, string seg_type, string edible_oil_ind);

    }
}

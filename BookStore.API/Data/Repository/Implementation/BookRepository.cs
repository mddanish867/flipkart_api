using BookStore.API.Data.Models;
using BookStore.API.Data.Repository.Interface;
using BookStore.API.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using BookStore.API.Common.Settlements;
using System.Data;

namespace BookStore.API.Data.Repository.Implementation
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext db;
        string strqry = string.Empty;
        private string DBconstring = String.Empty;
        public BookRepository(BookStoreContext _db)
        {
            db = _db;
            DBconstring = db.Database.GetConnectionString();// db.Database.GetConnectionString("BookStoreDB");
        }
        ///<summary>
        ///method for peanut specification exception list details 
        ///</summary>
        public async Task<List<Peanut_Specification>> peanut_spect_exception_list(string comp_id, int crop_year, string buy_pt_id, string pnut_variety_id, string pnut_type_id, string seed_ind, string seg_type, string edible_oil_ind)
        {
            List<Peanut_Specification> peanutspecexceptionlist = new List<Peanut_Specification>();
            Peanut_Specification obj;
            try
            {
                string MstrSQL = string.Empty;
                string strwhere = string.Empty;
                string area_id = string.Empty;
                MstrSQL = "select * pse.*.PNUT_TYPE_NAME, pvc.PUNT_VARIETY_NAME, pvc.SYMBOL_IND";
                MstrSQL = MstrSQL + "from dbo.PEANUT_SPEC_EXCEPTION as pse with (nolock)";
                MstrSQL = MstrSQL + "inner join dbo.BUYING_POINT-CONTROL as bpc with (nolock)";
                MstrSQL = MstrSQL + "on bpc.COMP_ID=pse.COMP_ID and bpc CROP_YEAR=pse.CROP_YEAR and bpc.BUY_PT_ID=pse.BUY_PT_ID";
                MstrSQL = MstrSQL + "where pse.COMP_ID='" + comp_id + "'";
                MstrSQL = MstrSQL + "and pse.CROP_YEAR" + crop_year;
                if (buy_pt_id != "")
                    MstrSQL = MstrSQL + "and pse.BUY_PT_ID='" + buy_pt_id + "'";
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
                            obj = new Peanut_Specification();
                            obj.ComId = row["COMP_ID"].ToString();
                            obj.Cropyear = Convert.ToDecimal(row["CROP_YEAR"]);
                            obj.BuyingPoint = row["BUY_PT_ID"].ToString();
                            //similarily for all the property you want to get
                            peanutspecexceptionlist.Add(obj);
                        }
                    }
                }
                return peanutspecexceptionlist;
            }
            catch (Exception ex)
            {
                peanutspecexceptionlist = new List<Peanut_Specification>();
                obj = new Peanut_Specification
                {
                    errormessage = ex.Message
                };
                peanutspecexceptionlist.Add(obj);
                return peanutspecexceptionlist;
            }
        }

    public async Task<List<Books>> GetAllBooks()
        {
            var result = await db.Books.Select(x => new Books()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            }).ToListAsync();
            return result;
        }
    }
}

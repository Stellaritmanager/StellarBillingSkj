using StellarBillingSystem.Context;
using System.Data;

namespace StellarBillingSystem_skj.Business
{
    public class BusinessClassCategory
    {
        private readonly BillingContext _billingContext;
        private readonly IConfiguration _configuration;

        public BusinessClassCategory(BillingContext billingContext, IConfiguration configuration)
        {
            _billingContext = billingContext;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public static DataTable convertToDataTableCategoryMaster(IEnumerable<CategoryMasterModel> entities)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("CategoryName", typeof(string));
            dataTable.Columns.Add("MarketRate", typeof(string));

            foreach (var entity in entities)
            {
                dataTable.Rows.Add(entity.CategoryName, entity.MarketRate);
            }

            return dataTable;
        }

    }
}

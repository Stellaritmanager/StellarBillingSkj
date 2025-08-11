using StellarBillingSystem.Context;
using StellarBillingSystem.Models;

namespace StellarBillingSystem_skj.Business
{
    public class SatffAdminBusinessClass
    {
        private readonly BillingContext _billingContext;
        private readonly IConfiguration _configuration;

        public SatffAdminBusinessClass(BillingContext billingContext, IConfiguration configuration)
        {
            _billingContext = billingContext;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public List<RollTypeMaster> RollAccessType(string BranchID)
        {
            var rollid = (
                    from pr in _billingContext.SHrollType
                    where pr.BranchID == BranchID && pr.IsDelete == false
                    select new RollTypeMaster
                    {
                        RollID = pr.RollID,
                        RollName = pr.RollName
                    }
                ).ToList();

            return rollid;
        }

        public List<ScreenNameMasterModel> Screenname()
        {
            var screenname = (
                    from pr in _billingContext.SHScreenName
                    select new ScreenNameMasterModel
                    {
                        ScreenName = pr.ScreenName,
                    }
                ).ToList();

            return screenname;
        }

        public List<BranchMasterModel> Getbranch()

        {
            var branchid = (
                        from pr in _billingContext.SHBranchMaster
                        where pr.IsDelete == false
                        select new BranchMasterModel
                        {
                            BracnchID = pr.BracnchID,

                            BranchName = pr.BranchName

                        }).ToList();
            return branchid;
        }

        public string GetFormattedDateTime()
        {
            DateTime currentDateTime = GetCurrentDateTime();
            return currentDateTime.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public DateTime GetCurrentDateTime()
        {
            string useIST = _configuration.GetValue<string>("DateTimeSettings:UseIST");

            DateTime currentDateTime = DateTime.Now;
            if (useIST.ToLower() == "yes")
            {
                // Return the current time
                return currentDateTime;
            }
            else
            {
                // Add 5 hours and 30 minutes to the current time
                return currentDateTime.AddHours(5).AddMinutes(30);
            }
        }


        public List<String> GetRoll(string userid, string BranchID)
        {

            var query = from sm in _billingContext.SHScreenName
                        join rac in _billingContext.SHRoleaccessModel on sm.ScreenName equals rac.ScreenID
                        join ram in _billingContext.SHrollType on rac.RollID equals ram.RollID
                        join sam in _billingContext.SHStaffAdmin on ram.RollID equals sam.RolltypeID
                        where rac.Authorized == "1"
                            && sam.UserName == userid
                            && sam.BranchID == BranchID
                            && rac.BranchID == BranchID
                            && ram.BranchID == BranchID
                        select sm.ScreenName;

            var result = query.ToList();
            return result;
        }

        public List<String> Getadmin(string userid)
        {

            var query = from sm in _billingContext.SHScreenName
                        join rac in _billingContext.SHRoleaccessModel on sm.ScreenName equals rac.ScreenID
                        join ram in _billingContext.SHrollType on rac.RollID equals ram.RollID
                        join sam in _billingContext.SHStaffAdmin on ram.RollID equals sam.RolltypeID
                        where rac.Authorized == "1" && sam.UserName == userid
                        select sm.ScreenName;

            var result = query.ToList();
            return result;
        }


        public BranchMasterModel Getbranchinitial(string BranchID)

        {
            var branchidini = (
                        from pr in _billingContext.SHBranchMaster
                        where pr.IsDelete == false && pr.BracnchID == BranchID
                        select new BranchMasterModel
                        {
                            BranchInitial = pr.BranchInitial,
                            BranchName = pr.BranchName


                        }).FirstOrDefault();
            return branchidini;
        }

        public StaffAdminModel GetadminRT(string Username)

        {
            var adminRT = (
                        from pr in _billingContext.SHStaffAdmin
                            // join ram in _billingContext.SHrollaccess on pr.StaffID equals ram.StaffID
                        join rol in _billingContext.SHrollType on pr.RolltypeID equals rol.RollID
                        where pr.IsDelete == false && pr.UserName == Username && (rol.RollName == "Admin" || rol.RollName == "Manager") && rol.IsDelete == false
                        select new StaffAdminModel
                        {
                            UserName = pr.UserName

                        }).FirstOrDefault();
            return adminRT;
        }

    }
}

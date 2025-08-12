using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StellarBillingSystem.Context;
using StellarBillingSystem_skj.Business;
using StellarBillingSystem_skj.Models;


[Authorize]
[Route("[controller]/[action]")]
public class RepledgeMasterController : Controller
{
    private BillingContext _billingsoftware;
    private readonly IConfiguration _configuration;


    public RepledgeMasterController(BillingContext billingsoftware, IConfiguration configuration)
    {
        _billingsoftware = billingsoftware;
        _configuration = configuration;
    }

    public IActionResult RepledgeMaster()
    {

        RepledgeViewModel obj = new RepledgeViewModel();

        BusinessBillingSKJ Busbill = new BusinessBillingSKJ(_billingsoftware, _configuration);
        ViewData["repledgerid"] = Busbill.getrepledgerID();

        return View(obj);
    }



    [HttpPost]
    public IActionResult GetArticlesAjax([FromBody] RepledgeViewModel model)
    {
        string branchID = TempData["BranchID"]?.ToString();
        TempData.Keep("BranchID");

        branchID = HttpContext.Session.GetString("BranchID");
        

        BusinessBillingSKJ Busbill = new BusinessBillingSKJ(_billingsoftware, _configuration);
        ViewData["repledgerid"] = Busbill.getrepledgerID();

        List<object> articles = new List<object>();
        List<object> selectedArticleIDs = new List<object>();
        object buyerInfo = null;

        if (!string.IsNullOrEmpty(model.BillID))
        {
            var pledgedArticleIds = (from r in _billingsoftware.Shrepledgeartcile
                                     join b in _billingsoftware.Shbuyerrepledge on r.RepledgeID equals b.RepledgeID
                                     where !r.IsDelete && !b.IsDelete && r.BranchID == branchID && b.BranchID == branchID && b.IsClosed == false
                                     select r.ArticleID).ToHashSet();


            articles = (from d in _billingsoftware.Shbilldetailsskj
                        join a in _billingsoftware.SHArticleMaster on d.ArticleID equals a.ArticleID
                        where d.BillID == model.BillID && d.BranchID == branchID && d.IsDelete == false
                           && !pledgedArticleIds.Contains(a.ArticleID)
                        select new
                        {
                            articleID = a.ArticleID,
                            articleName = a.ArticleName
                        }).ToList<object>();

            if (articles.Count == 0)
            {
                return Json(new { success = false, message = "Bill ID not found or has no articles." });
            }
        }

        if (!string.IsNullOrEmpty(model.RepledgeID))
        {
            selectedArticleIDs = (from ra in _billingsoftware.Shrepledgeartcile
                                  join bu in _billingsoftware.Shbuyerrepledge on ra.RepledgeID equals bu.RepledgeID
                                  join a in _billingsoftware.SHArticleMaster on ra.ArticleID equals a.ArticleID
                                  where ra.RepledgeID == model.RepledgeID && ra.BranchID == branchID && ra.IsDelete == false && bu.IsDelete == false
                                  select new
                                  {
                                      billID = ra.BillID,
                                      articleID = a.ArticleID,
                                      articleName = a.ArticleName,
                                      isClosed = bu.IsClosed
                                  }).ToList<object>();

            buyerInfo = (from bu in _billingsoftware.Shbuyerrepledge
                         where bu.RepledgeID == model.RepledgeID && bu.BranchID == branchID && bu.IsDelete == false
                         select new
                         {
                             buyerName = bu.BuyerName,
                             totalAmount = bu.TotalAmount,
                             interest = bu.Interest,
                             tenure = bu.Tenure,
                             loanHolderName = bu.LoanHolderName,
                             repledgeDate = bu.RepledgeDate,
                             closeDate = bu.CloseDate,
                             isClosed = bu.IsClosed

                         }).FirstOrDefault();

            if (selectedArticleIDs.Count == 0 && buyerInfo == null)
            {
                return Json(new { success = false, message = "Repledge ID not found." });
            }
        }

        return Json(new
        {
            success = true,
            articles,
            selectedArticleIDs,
            buyerInfo
        });
    }


    [HttpPost]
    public IActionResult GetDeletAjax([FromBody] RepledgeViewModel model)
    {
        var branchID = TempData["BranchID"]?.ToString();
        TempData.Keep("BranchID");

        branchID= HttpContext.Session.GetString("BranchID");

        BusinessBillingSKJ Busbill = new BusinessBillingSKJ(_billingsoftware, _configuration);
        ViewData["repledgerid"] = Busbill.getrepledgerID();

        var checkrepledge = _billingsoftware.Shbuyerrepledge.FirstOrDefault(x => x.RepledgeID == model.RepledgeID && x.BranchID == branchID);

        {
            if (checkrepledge != null)
            {
                checkrepledge.IsDelete = true;


                var getallarticle = _billingsoftware.Shrepledgeartcile.Where(x => x.RepledgeID == model.RepledgeID && x.BranchID == branchID).ToList();

                foreach (var result in getallarticle)
                {
                    result.IsDelete = true;

                }

                _billingsoftware.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" });
            }

            return Json(new { success = false, message = "Repledge not found" });
        }
    }



    [HttpPost]
    public IActionResult CloseRepledgeAjax([FromBody] RepledgeViewModel model)
    {
        var branchID = TempData["BranchID"]?.ToString();
        TempData.Keep("BranchID");

        branchID=HttpContext.Session.GetString("BranchID");

        BusinessBillingSKJ Busbill = new BusinessBillingSKJ(_billingsoftware, _configuration);
        ViewData["repledgerid"] = Busbill.getrepledgerID();

        var checkrepledge = _billingsoftware.Shbuyerrepledge.FirstOrDefault(x => x.RepledgeID == model.RepledgeID && x.BranchID == branchID);

        {
            if (checkrepledge != null)
            {
                checkrepledge.IsClosed = true;
                checkrepledge.CloseDate = model.CloseDate;


                _billingsoftware.SaveChanges();
                return Json(new { success = true, message = "Repledge Closed Successfully" });
            }

            return Json(new { success = false, message = "Repledge not found" });
        }
    }



    /* [HttpPost]
     public IActionResult GetArticles(RepledgeViewModel model,string buttonType)
     {
         if (TempData["BranchID"] != null)
         {
             model.BranchID = TempData["BranchID"].ToString();
             TempData.Keep("BranchID");
         }

         if(buttonType=="Delete")
         {
             var checkrepledge = _billingsoftware.Shbuyerrepledge.FirstOrDefault(x=>x.RepledgeID == model.RepledgeID && x.BranchID == model.BranchID);

             {
                 if (checkrepledge != null)
                 {
                     checkrepledge.IsDelete = true;


                     var getallarticle = _billingsoftware.Shrepledgeartcile.Where(x=>x.RepledgeID == model.RepledgeID && x.BranchID == model.BranchID).ToList();

                     foreach(var result in getallarticle)
                     {
                         result.IsDelete = true;

                     }

                     _billingsoftware.SaveChanges();
                     ViewBag.Message = "Deleted Successfully";
                 }
             }
         }

         // Get all bill articles first
         var allBillArticles = (from d in _billingsoftware.Shbilldetailsskj
                                join a in _billingsoftware.SHArticleMaster on d.ArticleID equals a.ArticleID
                                where d.BillID == model.BillID && d.BranchID == model.BranchID
                                select new ArticleSelection
                                {
                                    ArticleID = a.ArticleID,
                                    ArticleName = a.ArticleName
                                }).ToList();

         // Check if repledge already exists
         var existingBuyer = _billingsoftware.Shbuyerrepledge
             .FirstOrDefault(b => b.RepledgeID == model.RepledgeID && b.BranchID == model.BranchID);

         if (existingBuyer != null)
         {
             // Fill buyer data
             model.BuyerName = existingBuyer.BuyerName;
             model.Address = existingBuyer.Address;
             model.PhoneNumber = existingBuyer.PhoneNumber;
             model.TotalAmount = existingBuyer.TotalAmount;
             model.Interest = existingBuyer.Interest;
             model.Tenure = existingBuyer.Tenure;

             // Get already repledged article IDs
             var alreadySelectedIDs = _billingsoftware.Shrepledgeartcile
                 .Where(r => r.RepledgeID == model.RepledgeID && r.BranchID == model.BranchID)
                 .Select(r => r.ArticleID)
                 .ToList();

             model.SelectedArticleIDs = alreadySelectedIDs;
         }

         model.AvailableArticles = allBillArticles;

         return View("RepledgeMaster", model);
     }*/

    [HttpPost]
    public IActionResult SaveRepledgeAjax([FromBody] RepledgeViewModel request)
    {
        var branchID = TempData["BranchID"]?.ToString();
        TempData.Keep("BranchID");

        branchID= HttpContext.Session.GetString("BranchID");

        BusinessBillingSKJ Busbill = new BusinessBillingSKJ(_billingsoftware, _configuration);
        ViewData["repledgerid"] = Busbill.getrepledgerID();

        var buyer = _billingsoftware.Shbuyerrepledge
            .FirstOrDefault(x => x.RepledgeID == request.RepledgeID && x.BranchID == branchID && x.IsDelete == false);

        /*var checkvalidation = buyer.RepledgeID.Equals(request.RepledgeID && buyer.IsDelete == true);
        
            if(checkvalidation!=null)
            {
            return Json(new { message = "Cannot Save Check RepledgeID" });
            }*/


        if (buyer == null)
        {
            buyer = new RepledgeModel
            {
                RepledgeID = request.RepledgeID,
                BranchID = branchID,
                LastUpdatedDate = DateTime.Now.ToString(),
                LastUpdatedUser = User.Claims.First().Value,
                LastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString()
            };
            _billingsoftware.Shbuyerrepledge.Add(buyer);
        }

        buyer.BuyerName = request.BuyerName;
        buyer.Address = request.Address;
        buyer.PhoneNumber = request.PhoneNumber;
        buyer.TotalAmount = request.TotalAmount;
        buyer.Interest = request.Interest;
        buyer.Tenure = request.Tenure;
        buyer.LoanHolderName = request.LoanHolderName;
        buyer.RepledgeDate = request.RepledgeDate;

        foreach (var art in request.Articles)
        {
            bool exists = _billingsoftware.Shrepledgeartcile.Any(r =>
                r.ArticleID == art.ArticleID &&
                r.BillID == art.BillID &&
                r.RepledgeID == request.RepledgeID &&
                r.BranchID == branchID);

            if (!exists)
            {
                var article = new RepledgeArtcileModel
                {
                    ArticleID = art.ArticleID,
                    BillID = art.BillID,
                    RepledgeID = request.RepledgeID,
                    BranchID = branchID,
                    LastUpdatedDate = DateTime.Now.ToString(),
                    LastUpdatedUser = User.Claims.First().Value,
                    LastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                };

                _billingsoftware.Shrepledgeartcile.Add(article);
            }
        }

        _billingsoftware.SaveChanges();
        return Json(new { message = "Saved successfully." });
    }



    [HttpPost]
    public IActionResult SaveRepledge(RepledgeViewModel model)
    {

        if (TempData["BranchID"] != null)
        {
            model.BranchID = TempData["BranchID"].ToString();
            TempData.Keep("BranchID");
        }
        else
        {
            model.BranchID= HttpContext.Session.GetString("BranchID");
        }

        

        var checkdatabuyer = _billingsoftware.Shbuyerrepledge.FirstOrDefault(x => x.RepledgeID == model.RepledgeID);

        if (checkdatabuyer != null)
        {
            checkdatabuyer.RepledgeID = model.RepledgeID;
            checkdatabuyer.BuyerName = model.BuyerName;
            checkdatabuyer.Address = model.Address;
            checkdatabuyer.PhoneNumber = model.PhoneNumber;
            checkdatabuyer.TotalAmount = model.TotalAmount;
            checkdatabuyer.Interest = model.Interest;
            checkdatabuyer.BranchID = model.BranchID;
            checkdatabuyer.Tenure = model.Tenure;
            checkdatabuyer.LastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            checkdatabuyer.LastUpdatedDate = DateTime.Now.ToString();
            checkdatabuyer.LastUpdatedUser = User.Claims.First().Value.ToString();

        }
        else
        {
            // 1. Save BuyerModelSKJ
            var buyer = new RepledgeModel
            {
                RepledgeID = model.RepledgeID,
                BuyerName = model.BuyerName,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                TotalAmount = model.TotalAmount,
                Interest = model.Interest,
                BranchID = model.BranchID,
                Tenure = model.Tenure,
                LastUpdatedDate = DateTime.Now.ToString(),
                LastUpdatedUser = User.Claims.First().Value.ToString(),
                LastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
            };

            _billingsoftware.Shbuyerrepledge.Add(buyer);
        }


        // a. Add new selections
        foreach (var articleId in model.SelectedArticleIDs)
        {
            bool exists = _billingsoftware.Shrepledgeartcile.Any(r =>
                r.ArticleID == articleId &&
                r.RepledgeID == model.RepledgeID &&
                r.BranchID == model.BranchID &&
                r.BillID == model.BillID);

            if (!exists)
            {
                var replArticle = new RepledgeArtcileModel
                {
                    ArticleID = articleId,
                    BillID = model.BillID,
                    BranchID = model.BranchID,
                    RepledgeID = model.RepledgeID,
                    LastUpdatedDate = DateTime.Now.ToString(),
                    LastUpdatedUser = User.Claims.First().Value.ToString(),
                    LastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                };

                _billingsoftware.Shrepledgeartcile.Add(replArticle);
            }
        }


        _billingsoftware.SaveChanges();
        ViewBag.Message = "Payment Saved Successfully";
        return RedirectToAction("RepledgeMaster");
    }
}


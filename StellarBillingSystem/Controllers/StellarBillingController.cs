using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using StellarBillingSystem.Business;
using StellarBillingSystem.Context;
using StellarBillingSystem.Models;
using System.Data;
using System.Globalization;
using System.Web;


namespace StellarBillingSystem.Controllers
{
    [Authorize]
    public class StellarBillingController : Controller
    {
        private BillingContext _billingsoftware;
        private readonly IConfiguration _configuration;


        public StellarBillingController(BillingContext billingsoftware, IConfiguration configuration)
        {
            _billingsoftware = billingsoftware;
            _configuration = configuration;
        }
        public async Task<IActionResult> CategoryMaster()
        {
            CategoryMasterModel model = new CategoryMasterModel();
            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }
            ViewData["Categorydata"] = await AdditionalCategoryMasterFun(model.BranchID);

            return View("CategoryMaster", model);

        }
        public async Task<DataTable> AdditionalCategoryMasterFun(string branchID)
        {

            // Step 1: Perform the query
            var entities = _billingsoftware.SHCategoryMaster
                                  .Where(e => e.BranchID == branchID && e.IsDelete == false).OrderByDescending(e => e.LastUpdatedDate)
                                  .ToList();

            // Step 2: Convert to DataTable
            return BusinessClassBilling.convertToDataTableCategoryMaster(entities);


        }




        [HttpGet]
        public async Task<IActionResult> GetCategory(CategoryMasterModel model, string buttonType)
        {


            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            if (buttonType == "Get")
            {
                var getcategory = await _billingsoftware.SHCategoryMaster.FirstOrDefaultAsync(x => x.CategoryID == model.CategoryID && !x.IsDelete && x.BranchID == model.BranchID);
                if (getcategory != null)
                {
                    var dataTable = await AdditionalCategoryMasterFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["Categorydata"] = dataTable;
                    return View("CategoryMaster", getcategory);
                }
                else
                {
                    CategoryMasterModel par = new CategoryMasterModel();
                    ViewBag.ErrorMessage = "No value for this Category ID";
                    var dataTable = await AdditionalCategoryMasterFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["Categorydata"] = dataTable;
                    return View("CategoryMaster", par);
                }
            }

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryMasterModel model, string buttonType)
        {

            BusinessClassBilling business = new BusinessClassBilling(_billingsoftware, _configuration);

            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }


            if (buttonType == "Get")
            {
                var getcategory = await _billingsoftware.SHCategoryMaster.FirstOrDefaultAsync(x => x.CategoryID == model.CategoryID && !x.IsDelete && x.BranchID == model.BranchID);
                if (getcategory != null)
                {
                    var dataTable1 = await AdditionalCategoryMasterFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["Categorydata"] = dataTable1;
                    return View("CategoryMaster", getcategory);
                }
                else
                {
                    CategoryMasterModel par = new CategoryMasterModel();
                    ViewBag.ErrorMessage = "No value for this Category ID";
                    var dataTable2 = await AdditionalCategoryMasterFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["Categorydata"] = dataTable2;
                    return View("CategoryMaster", par);
                }
            }
            else if (buttonType == "Delete")
            {
                var categorytodelete = await _billingsoftware.SHCategoryMaster.FirstOrDefaultAsync(x => x.CategoryID == model.CategoryID && !x.IsDelete && x.BranchID == model.BranchID);

                if (categorytodelete != null)
                {
                    categorytodelete.IsDelete = true;
                    await _billingsoftware.SaveChangesAsync();

                    ViewBag.Message = "Category deleted successfully";

                    var dataTable3 = await AdditionalCategoryMasterFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["Categorydata"] = dataTable3;

                    model = new CategoryMasterModel();

                    return View("CategoryMaster", model);
                }
                else
                {
                    ViewBag.ErrorMessage = "Category not found";
                    var dataTable4 = await AdditionalCategoryMasterFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["Categorydata"] = dataTable4;
                    model = new CategoryMasterModel();
                    return View("CategoryMaster", model);
                }


            }

            else if (buttonType == "DeleteRetrieve")
            {
                var categorytoretrieve = await _billingsoftware.SHCategoryMaster.FirstOrDefaultAsync(x => x.CategoryID == model.CategoryID && x.BranchID == model.BranchID);

                if (categorytoretrieve != null)
                {
                    if (categorytoretrieve.IsDelete == true)

                    {
                        categorytoretrieve.IsDelete = false;

                        await _billingsoftware.SaveChangesAsync();

                        model.CategoryID = categorytoretrieve.CategoryID;
                        model.CategoryName = categorytoretrieve.CategoryName;

                        ViewBag.Message = "Category retrieved successfully";
                    }
                    else
                    {
                        ViewBag.Message = "CategoryID Already Retrieved ";

                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Category not found";


                }
                var dataTable5 = await AdditionalCategoryMasterFun(model.BranchID);

                // Store the DataTable in ViewData for access in the view
                ViewData["Categorydata"] = dataTable5;


                return View("CategoryMaster", model);
            }
            else if (buttonType == "save")
            {
                HttpContext.Session.SetString("BranchID", model.BranchID);

                var existingCategory = await _billingsoftware.SHCategoryMaster.FirstOrDefaultAsync(x => x.CategoryID == model.CategoryID && x.BranchID == model.BranchID);

                if (existingCategory != null)
                {
                    if (existingCategory.IsDelete)
                    {
                        ViewBag.ErrorMessage = "Cannot Save or Update. Category is marked as deleted.";
                        var dataTable6 = await AdditionalCategoryMasterFun(model.BranchID);

                        // Store the DataTable in ViewData for access in the view
                        ViewData["Categorydata"] = dataTable6;
                        return View("CategoryMaster", model);
                    }
                    existingCategory.CategoryID = model.CategoryID;
                    existingCategory.CategoryName = model.CategoryName;
                    existingCategory.LastUpdatedDate = DateTime.ParseExact(business.GetFormattedDateTime(), "dd/MM/yyyy HH:mm:ss", null);
                    existingCategory.LastUpdatedUser = User.Claims.First().Value.ToString();
                    existingCategory.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                    _billingsoftware.Entry(existingCategory).State = EntityState.Modified;
                }
                else
                {
                    model.LastUpdatedDate = DateTime.ParseExact(business.GetFormattedDateTime(), "dd/MM/yyyy HH:mm:ss", null);
                    model.LastUpdatedUser = User.Claims.First().Value.ToString();
                    model.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();


                    _billingsoftware.SHCategoryMaster.Add(model);
                }


                await _billingsoftware.SaveChangesAsync();

                ViewBag.Message = "Saved Successfully";
            }
            var dataTable = await AdditionalCategoryMasterFun(model.BranchID);

            // Store the DataTable in ViewData for access in the view
            ViewData["Categorydata"] = dataTable;
            model = new CategoryMasterModel();

            return View("CategoryMaster", model);
        }




        public async Task<IActionResult> ProductMaster()
        {

            BusinessClassBilling business = new BusinessClassBilling(_billingsoftware, _configuration);

            var categories = business.GetItemsFromDatabase(TempData["BranchID"].ToString());

            // Convert CategoryMasterModel to SelectListItem
            var selectListItems = categories;

            var model = new ProductDropDownModel
            {
                Items = selectListItems,  // Assign converted SelectListItems
                SelectedItemId = null,
                ObjPro = new ProductMatserModel() // Ensure ObjPro is initialized to avoid null reference
            };


            if (TempData["BranchID"] != null)
            {
                model.ObjPro.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }



            // ViewData["categoryid"] = business.GetCatid(model.BranchID);
            ViewData["discountid"] = business.Getdiscountid(model.ObjPro.BranchID);


            ViewData["categoryid"] = business.GetCatid(model.ObjPro.BranchID);
            ViewData["discountid"] = business.Getdiscountid(model.ObjPro.BranchID);

            ViewData["ProductData"] = await AdditionalProductMasterFun(model.ObjPro.BranchID);

            return View("ProductMaster", model);


        }


        public async Task<DataTable> AdditionalProductMasterFun(string branchID)
        {

            // Step 1: Perform the query
            var entities = _billingsoftware.SHProductMaster
                                  .Where(e => e.BranchID == branchID && e.IsDelete == false).OrderByDescending(e => e.LastUpdatedDate)
                                  .ToList();

            // Step 2: Convert to DataTable
            return BusinessClassBilling.ConvertToDataTableProductMaster(entities);


        }



        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDropDownModel model, string buttonType, string SelectedItem)
        {
            if (TempData["BranchID"] != null)
            {
                model.ObjPro.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            BusinessClassBilling business = new BusinessClassBilling(_billingsoftware, _configuration);

            ViewData["categoryid"] = business.GetCatid(model.ObjPro.BranchID);
            ViewData["discountid"] = business.Getdiscountid(model.ObjPro.BranchID);


            // Fetch categories and convert them to SelectListItem for dropdown
            var categories = business.GetItemsFromDatabase(model.ObjPro.BranchID);
            var selectListItems = categories;


            string? selectedCategoryId = null;

            if (buttonType == "Get")
            {
                // Validation: ensure either ProductID or BarcodeID is entered
                if (string.IsNullOrEmpty(model.ObjPro.ProductID) && string.IsNullOrEmpty(model.ObjPro.BarcodeId))
                {
                    ViewBag.ValidationMessage = "Please enter either ProductID or BarcodeID.";

                    // Fetch and assign product list or additional product data
                    var dataTable = await AdditionalProductMasterFun(model.ObjPro.BranchID);

                    ViewData["ProductData"] = dataTable;


                    var productDropDownModel = business.CreateProductDropDownModel(selectListItems, selectedCategoryId, model.ObjPro);

                    // Return the view with the updated model
                    return View("ProductMaster", productDropDownModel);
                }

                // Fetch product details based on BarcodeID or ProductID
                ProductMatserModel? resultpro = null;
                if (!string.IsNullOrEmpty(model.ObjPro.BarcodeId))
                {
                    resultpro = await _billingsoftware.SHProductMaster
                        .FirstOrDefaultAsync(x => x.BarcodeId == model.ObjPro.BarcodeId && !x.IsDelete && x.BranchID == model.ObjPro.BranchID);
                }
                else
                {
                    resultpro = await _billingsoftware.SHProductMaster
                        .FirstOrDefaultAsync(x => x.ProductID == model.ObjPro.ProductID && !x.IsDelete && x.BranchID == model.ObjPro.BranchID);
                }

                if (resultpro != null)
                {
                    selectedCategoryId = resultpro.CategoryID?.ToString() ?? string.Empty;


                    // Fetch rack partition and godown details for the product
                    var noofitemrack = await _billingsoftware.SHRackPartionProduct
                        .Where(x => x.ProductID == resultpro.ProductID && x.BranchID == resultpro.BranchID)
                        .Select(x => x.Noofitems).FirstOrDefaultAsync();

                    var noofstock = await (
                        from godown in _billingsoftware.SHGodown
                        join product in _billingsoftware.SHProductMaster
                        on godown.ProductID equals product.ProductID
                        where (product.BarcodeId == model.ObjPro.BarcodeId || product.ProductID == model.ObjPro.ProductID) &&
                              godown.BranchID == model.ObjPro.BranchID &&
                              !product.IsDelete && !godown.IsDelete
                        select godown.NumberofStocks
                    ).FirstOrDefaultAsync();

                    // Assign rack and godown stock data to ViewBag
                    ViewBag.GodownNoofitems = noofstock;
                    ViewBag.RackNoofitems = noofitemrack;

                    var productDropDownModel = business.CreateProductDropDownModel(selectListItems, selectedCategoryId, model.ObjPro);



                    // Update the product drop-down model with the fetched data
                    productDropDownModel.ObjPro = resultpro;
                    var dataTable = await AdditionalProductMasterFun(model.ObjPro.BranchID);
                    ViewData["ProductData"] = dataTable;

                    if (resultpro.CategoryID == null || resultpro.CategoryID == string.Empty)
                        productDropDownModel.SelectedItemId = string.Empty;

                    return View("ProductMaster", productDropDownModel);
                }
                else
                {
                    // No product found, return an empty product with a message
                    ViewBag.NoProductMessage = "No value for this product ID";
                    var productDropDownModel = business.CreateProductDropDownModel(selectListItems, selectedCategoryId, model.ObjPro);


                    var dataTable = await AdditionalProductMasterFun(model.ObjPro.BranchID);
                    ViewData["ProductData"] = dataTable;


                    return View("ProductMaster", productDropDownModel);
                }
            }

            if (buttonType == "Clear")
            {

                var clr = new ProductDropDownModel
                {
                    ObjPro = new ProductMatserModel(), // Initialize if not done already
                    Items = business.GetItemsFromDatabase(model.ObjPro.BranchID) // Ensure this returns a list
                };

                ViewBag.ClearMessage = "Fields have been cleared.";

                var dataTable = await AdditionalProductMasterFun(model.ObjPro.BranchID);
                ViewData["ProductData"] = dataTable;

                return View("ProductMaster", clr);
            }



            else if (buttonType == "Delete")
            {
                if (string.IsNullOrEmpty(model.ObjPro.ProductID) && string.IsNullOrEmpty(model.ObjPro.BarcodeId))
                {
                    ViewBag.ValidationMessage = "Please enter either ProductID or BarcodeID.";
                    var dataTable1 = await AdditionalProductMasterFun(model.ObjPro.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["ProductData"] = dataTable1;

                    var model2 = new ProductDropDownModel
                    {
                        ObjPro = new ProductMatserModel(), // Initialize if not done already
                        Items = business.GetItemsFromDatabase(model.ObjPro.BranchID) // Ensure this returns a list
                    };

                    return View("ProductMaster", model2);
                }

                var productToDelete = await _billingsoftware.SHProductMaster.FirstOrDefaultAsync(x => (x.ProductID == model.ObjPro.ProductID || x.BarcodeId == model.ObjPro.BarcodeId) && !x.IsDelete && x.BranchID == model.ObjPro.BranchID);
                if (productToDelete != null)
                {
                    productToDelete.IsDelete = true;
                    await _billingsoftware.SaveChangesAsync();

                    ViewBag.Message = "Product deleted successfully";

                }
                else
                {
                    ViewBag.ErrorMessage = "Product not found";

                }

                var dataTable = await AdditionalProductMasterFun(model.ObjPro.BranchID);

                // Store the DataTable in ViewData for access in the view
                ViewData["ProductData"] = dataTable;

                var model1 = new ProductDropDownModel
                {
                    ObjPro = new ProductMatserModel(), // Initialize if not done already
                    Items = business.GetItemsFromDatabase(model.ObjPro.BranchID) // Ensure this returns a list
                };
                return View("ProductMaster", model1);
            }
            else if (buttonType == "DeleteRetrieve")
            {
                if (string.IsNullOrEmpty(model.ObjPro.ProductID) && string.IsNullOrEmpty(model.ObjPro.BarcodeId))
                {
                    ViewBag.ValidationMessage = "Please enter either ProductID or BarcodeID.";
                    var dataTable1 = await AdditionalProductMasterFun(model.ObjPro.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["ProductData"] = dataTable1;

                    var model2 = new ProductDropDownModel
                    {
                        ObjPro = new ProductMatserModel(), // Initialize if not done already
                        Items = business.GetItemsFromDatabase(model.ObjPro.BranchID) // Ensure this returns a list
                    };

                    return View("ProductMaster", model2);
                }

                var productToRetrieve = await _billingsoftware.SHProductMaster.FirstOrDefaultAsync(x => (x.ProductID == model.ObjPro.ProductID || x.BarcodeId == model.ObjPro.BarcodeId) && x.IsDelete == true && x.BranchID == model.ObjPro.BranchID);
                if (productToRetrieve != null)
                {
                    if (productToRetrieve.IsDelete == true)
                    {

                        productToRetrieve.IsDelete = false;

                        await _billingsoftware.SaveChangesAsync();

                        model.ObjPro.ProductID = productToRetrieve.ProductID;
                        model.ObjPro.CategoryID = productToRetrieve.CategoryID;
                        model.ObjPro.ProductName = productToRetrieve.ProductName;
                        model.ObjPro.Brandname = productToRetrieve.Brandname;
                        model.ObjPro.Price = productToRetrieve.Price;
                        model.ObjPro.DiscountCategory = productToRetrieve.DiscountCategory;
                        model.ObjPro.TotalAmount = productToRetrieve.TotalAmount;
                        model.ObjPro.ImeiNumber = productToRetrieve.ImeiNumber;
                        model.ObjPro.SerialNumber = productToRetrieve.SerialNumber;
                        model.ObjPro.BarcodeId = productToRetrieve.BarcodeId;
                        model.ObjPro.SGST = productToRetrieve.SGST;
                        model.ObjPro.CGST = productToRetrieve.CGST;
                        model.ObjPro.OtherTax = productToRetrieve.OtherTax;

                        ViewBag.Message = "Product retrieved successfully";
                    }
                    else
                    {
                        ViewBag.Message = "ProductID Already Retrieved";
                    }

                }
                else
                {
                    ViewBag.ErrorMessage = "Product not found";
                }
                var dataTable = await AdditionalProductMasterFun(model.ObjPro.BranchID);

                // Store the DataTable in ViewData for access in the view
                ViewData["ProductData"] = dataTable;

                var model1 = new ProductDropDownModel
                {
                    ObjPro = new ProductMatserModel(), // Initialize if not done already
                    Items = business.GetItemsFromDatabase(model.ObjPro.BranchID) // Ensure this returns a list
                };
                return View("ProductMaster", model1);
            }
            else if (buttonType == "Save")
            {
                //if (string.IsNullOrEmpty(model.ProductID))
                //{
                //    ViewBag.ValidationMessage = "Please enter  ProductID";
                //    var dataTable = await AdditionalProductMasterFun(model.BranchID);

                //    // Store the DataTable in ViewData for access in the view
                //    ViewData["ProductData"] = dataTable;
                //    return View("ProductMaster", model);
                //}

                HttpContext.Session.SetString("BranchID", model.ObjPro.BranchID);

                if (string.IsNullOrEmpty(model.ObjPro.BarcodeId))
                {
                    ViewBag.ValidationMessage = "Please enter  BarcodeID.";
                    var dataTable = await AdditionalProductMasterFun(model.ObjPro.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["ProductData"] = dataTable;

                    var model1 = new ProductDropDownModel
                    {
                        ObjPro = new ProductMatserModel(), // Initialize if not done already
                        Items = business.GetItemsFromDatabase(model.ObjPro.BranchID) // Ensure this returns a list
                    };

                    return View("ProductMaster", model1);

                }


                decimal price;
                if (!decimal.TryParse(model.ObjPro.Price, out price))
                {
                    ViewBag.PriceErrorMessage = "Please enter a valid price.";
                    var dataTable = await AdditionalProductMasterFun(model.ObjPro.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["ProductData"] = dataTable;
                    var model1 = new ProductDropDownModel
                    {
                        ObjPro = new ProductMatserModel(), // Initialize if not done already
                        Items = business.GetItemsFromDatabase(model.ObjPro.BranchID) // Ensure this returns a list
                    };

                    return View("ProductMaster", model1);
                }

                decimal discount;

                decimal totalAmount;

                if (!string.IsNullOrEmpty(model.ObjPro.DiscountCategory) && decimal.TryParse(model.ObjPro.DiscountCategory, out discount))
                {
                    // Perform calculation if a valid discount is provided
                    totalAmount = price - (price * discount / 100);
                }
                else
                {
                    // Use the original price if no discount is provided or if the discount is invalid
                    totalAmount = price;
                }

                model.ObjPro.TotalAmount = totalAmount.ToString();



                var existingProduct = await _billingsoftware.SHProductMaster.FirstOrDefaultAsync(x => x.ProductID == model.ObjPro.ProductID && x.BranchID == model.ObjPro.BranchID);
                if (existingProduct != null)
                {
                    if (existingProduct.IsDelete)
                    {
                        ViewBag.ErrorMessage = "Cannot update. Product is marked as deleted.";
                        var dataTable = await AdditionalProductMasterFun(model.ObjPro.BranchID);

                        // Store the DataTable in ViewData for access in the view
                        ViewData["ProductData"] = dataTable;

                        var model1 = new ProductDropDownModel
                        {
                            ObjPro = new ProductMatserModel(), // Initialize if not done already
                            Items = business.GetItemsFromDatabase(model.ObjPro.BranchID) // Ensure this returns a list
                        };

                        return View("ProductMaster", model1);
                    }


                    existingProduct.ProductID = model.ObjPro.ProductID;
                    existingProduct.CategoryID = SelectedItem;
                    existingProduct.ProductName = model.ObjPro.ProductName;
                    existingProduct.BarcodeId = model.ObjPro.BarcodeId;
                    existingProduct.Brandname = model.ObjPro.Brandname;
                    existingProduct.Price = model.ObjPro.Price;
                    existingProduct.ImeiNumber = model.ObjPro.ImeiNumber;
                    existingProduct.SerialNumber = model.ObjPro.SerialNumber;
                    existingProduct.DiscountCategory = model.ObjPro.DiscountCategory;
                    existingProduct.SGST = model.ObjPro.SGST;
                    existingProduct.CGST = model.ObjPro.CGST;
                    existingProduct.OtherTax = model.ObjPro.OtherTax;
                    existingProduct.Price = model.ObjPro.Price;
                    existingProduct.DiscountCategory = model.ObjPro.DiscountCategory;
                    existingProduct.TotalAmount = model.ObjPro.TotalAmount;
                    existingProduct.BranchID = model.ObjPro.BranchID;


                    // existingProduct.TotalAmount = model.TotalAmount - (model.Price * model.Discount / 100 = model.TotalAmount);
                    existingProduct.LastUpdatedDate = DateTime.ParseExact(business.GetFormattedDateTime(), "dd/MM/yyyy HH:mm:ss", null);
                    existingProduct.LastUpdatedUser = User.Claims.First().Value.ToString();
                    existingProduct.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                    _billingsoftware.Entry(existingProduct).State = EntityState.Modified;
                }
                else
                {

                    // Convert strings to decimals, calculate TotalAmount, and convert back to string
                    model.ObjPro.CategoryID = SelectedItem;
                    model.ObjPro.LastUpdatedDate = DateTime.ParseExact(business.GetFormattedDateTime(), "dd/MM/yyyy HH:mm:ss", null);
                    model.ObjPro.LastUpdatedUser = User.Claims.First().Value.ToString();
                    model.ObjPro.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                    _billingsoftware.SHProductMaster.Add(model.ObjPro);
                }

                await _billingsoftware.SaveChangesAsync();

                selectedCategoryId = SelectedItem;

                ViewBag.Message = "Saved Successfully";
            }



            var dataTable2 = await AdditionalProductMasterFun(model.ObjPro.BranchID);

            // Store the DataTable in ViewData for access in the view
            ViewData["ProductData"] = dataTable2;


            var productDropDownModel1 = business.CreateProductDropDownModel(selectListItems, selectedCategoryId, model.ObjPro);
            return View("ProductMaster", productDropDownModel1);
        }





        public async Task<DataTable> AdditionalGodownFun(string branchID)
        {

            var entities = (
                           from g in _billingsoftware.SHGodown
                           join pm in _billingsoftware.SHProductMaster on g.ProductID equals pm.ProductID
                           where g.BranchID == branchID && g.IsDelete == false && pm.BranchID == branchID && pm.IsDelete == false
                           orderby g.LastUpdatedDate descending
                           select new GodownModel
                           {
                               ProductID = pm.ProductName,
                               NumberofStocks = g.NumberofStocks
                           }).ToList();

            // Step 2: Convert to DataTable
            return BusinessClassBilling.ConvertToDataTableGodown(entities);

        }



        public async Task<IActionResult> GodownModel()
        {
            var model = new GodownModel();

            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }


            BusinessClassBilling business = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["godownproductid"] = business.GetProductid(model.BranchID);


            var dataTable = await AdditionalGodownFun(model.BranchID);

            // Store the DataTable in ViewData for access in the view
            ViewData["GodownData"] = dataTable;

            return View("GodownModel", model);

        }


        [HttpPost]

        public async Task<IActionResult> GodDown(GodownModel model, string buttonType)
        {
            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            BusinessClassBilling business = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["godownproductid"] = business.GetProductid(model.BranchID);


            if (buttonType == "DeleteRetrieve")
            {
                var screentoretrieve = await _billingsoftware.SHGodown.FindAsync(model.ProductID, model.BranchID);
                if (screentoretrieve != null)
                {
                    if (screentoretrieve.IsDelete == true)
                    {

                        screentoretrieve.IsDelete = false;

                        await _billingsoftware.SaveChangesAsync();

                        model.ProductID = screentoretrieve.ProductID;
                        model.DatefofPurchase = screentoretrieve.DatefofPurchase;
                        model.NumberofStocks = screentoretrieve.NumberofStocks;
                        model.SupplierInformation = screentoretrieve.SupplierInformation;

                        ViewBag.retMessage = "Deleted Stock retrieved successfully";
                        var dataTable1 = await AdditionalGodownFun(model.BranchID);

                        // Store the DataTable in ViewData for access in the view
                        ViewData["GodownData"] = dataTable1;
                        return View("GodownModel", screentoretrieve);
                    }
                    else
                    {
                        ViewBag.retMessage = "Stock Already Retrieved";
                    }
                }
                else
                {
                    ScreenMasterModel scrn = new ScreenMasterModel();
                    ViewBag.nostockMessage = "Stock not found";
                }
                var dataTable2 = await AdditionalGodownFun(model.BranchID);

                // Store the DataTable in ViewData for access in the view
                ViewData["GodownData"] = dataTable2;
                return View("GodownModel", model);
            }


            if (buttonType == "Save")
            {
                var existinggoddown = await _billingsoftware.SHGodown.FindAsync(model.ProductID, model.BranchID);
                if (existinggoddown != null)
                {
                    if (existinggoddown.IsDelete)
                    {
                        ViewBag.ErrorMessage = "Cannot update. GodowmnID is marked as deleted.";
                        var dataTable3 = await AdditionalGodownFun(model.BranchID);

                        // Store the DataTable in ViewData for access in the view
                        ViewData["GodownData"] = dataTable3;
                        return View("GodownModel", model);
                    }

                    // Convert existing NumberofStocks from string to int
                    int existingNumberOfStocks = 0;
                    int newNumberOfStocks = 0;

                    // Parse the existing number of stocks
                    if (!string.IsNullOrEmpty(existinggoddown.NumberofStocks))
                    {
                        existingNumberOfStocks = int.Parse(existinggoddown.NumberofStocks);
                    }

                    // Parse the new number of stocks
                    if (!string.IsNullOrEmpty(model.NumberofStocks))
                    {
                        newNumberOfStocks = int.Parse(model.NumberofStocks);
                    }

                    // Add the new NumberofStocks to the existing NumberofStocks
                    int updatedNumberOfStocks = existingNumberOfStocks + newNumberOfStocks;

                    // Update the existing record with the new total
                    existinggoddown.NumberofStocks = updatedNumberOfStocks.ToString();



                    existinggoddown.ProductID = model.ProductID;

                    existinggoddown.DatefofPurchase = model.DatefofPurchase;
                    existinggoddown.SupplierInformation = model.SupplierInformation;
                    existinggoddown.IsDelete = model.IsDelete;
                    existinggoddown.BranchID = model.BranchID;
                    existinggoddown.LastUpdatedDate = DateTime.ParseExact(business.GetFormattedDateTime(), "dd/MM/yyyy HH:mm:ss", null);
                    existinggoddown.LastUpdatedUser = User.Claims.First().Value.ToString();
                    existinggoddown.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    _billingsoftware.Entry(existinggoddown).State = EntityState.Modified;

                }

                else
                {

                    model.LastUpdatedDate = DateTime.ParseExact(business.GetFormattedDateTime(), "dd/MM/yyyy HH:mm:ss", null);
                    model.LastUpdatedUser = User.Claims.First().Value.ToString();
                    model.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    _billingsoftware.SHGodown.Add(model);

                }

                await _billingsoftware.SaveChangesAsync();
                ViewBag.Message = "saved Successfully";


            }
            if (buttonType == "Delete")
            {
                var goddown = await _billingsoftware.SHGodown.FindAsync(model.ProductID, model.BranchID);
                if (goddown != null)
                {
                    if (goddown.IsDelete)
                    {
                        ViewBag.ErrorMessage = "Stock Already Deleted";
                        var dataTable4 = await AdditionalGodownFun(model.BranchID);

                        // Store the DataTable in ViewData for access in the view
                        ViewData["GodownData"] = dataTable4;
                        return View("GodownModel", model);
                    }

                    goddown.IsDelete = true;
                    await _billingsoftware.SaveChangesAsync();

                    ViewBag.delMessage = "Stock deleted successfully";
                    var dataTable5 = await AdditionalGodownFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["GodownData"] = dataTable5;
                    model = new GodownModel();
                    return View("GodownModel", model);
                }
                else
                {
                    ViewBag.nostockMessage = "Stock not found";
                    var dataTable6 = await AdditionalGodownFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["GodownData"] = dataTable6;
                    model = new GodownModel();
                    return View("GodownModel", model);
                }
            }



            if (buttonType == "Get")
            {
                var getStock = await _billingsoftware.SHGodown.FirstOrDefaultAsync(x => x.IsDelete == false && x.ProductID == model.ProductID && x.IsDelete == false && x.BranchID == model.BranchID);
                if (getStock != null)
                {
                    var dataTable9 = await AdditionalGodownFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["GodownData"] = dataTable9;

                    return View("GodownModel", getStock);
                }
                else
                {
                    var dataTable7 = await AdditionalGodownFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["GodownData"] = dataTable7;
                    GodownModel role = new GodownModel();

                    ViewBag.getMessage = "No Data found";
                    return View("GodownModel", model);
                }

            }



            ViewBag.Message = "Saved Successfully";

            var dataTable8 = await AdditionalGodownFun(model.BranchID);

            // Store the DataTable in ViewData for access in the view
            ViewData["GodownData"] = dataTable8;



            model = new GodownModel();



            return View("GodownModel", model);

        }



        [HttpPost]
        public async Task<IActionResult> AddCustomer(CustomerMasterModel model)
        {
            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }


            var existingCustomer = await _billingsoftware.SHCustomerMaster.FindAsync(model.MobileNumber, model.BranchID);
            if (existingCustomer != null)
            {
                if (existingCustomer.IsDelete)
                {
                    ViewBag.Message = "Cannot update. Customer Number is marked as deleted.";

                    return View("CustomerMaster", model);
                }
                existingCustomer.CustomerID = model.CustomerID;
                existingCustomer.CustomerName = model.CustomerName;
                existingCustomer.DateofBirth = model.DateofBirth;
                existingCustomer.Gender = model.Gender;
                existingCustomer.Address = model.Address;
                existingCustomer.City = model.City;
                existingCustomer.MobileNumber = model.MobileNumber;
                existingCustomer.IsDelete = model.IsDelete;
                existingCustomer.BranchID = model.BranchID;
                existingCustomer.LastUpdatedDate = DateTime.Now.ToString();
                existingCustomer.LastUpdatedUser = User.Claims.First().Value.ToString();
                existingCustomer.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.Entry(existingCustomer).State = EntityState.Modified;

            }
            else
            {
                model.LastUpdatedDate = DateTime.Now.ToString();
                model.LastUpdatedUser = User.Claims.First().Value.ToString();
                model.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.SHCustomerMaster.Add(model);


            }

            await _billingsoftware.SaveChangesAsync();

            ViewBag.Message = "Saved Successfully";

            CustomerMasterModel mod = new CustomerMasterModel();

            return View("CustomerMaster", mod);
        }
        public async Task<IActionResult> GetCustomer(CustomerMasterModel model)
        {
            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            if (string.IsNullOrEmpty(model.MobileNumber))
            {
                ViewBag.Message = "Mobile number is required";
                return BadRequest("Mobile number is required");
            }

            var customer = await _billingsoftware.SHCustomerMaster.FirstOrDefaultAsync(x => x.IsDelete == false && x.MobileNumber == model.MobileNumber && x.BranchID == model.BranchID);

            if (customer == null)
            {
                model = new CustomerMasterModel();
                ViewBag.ErrorMessage = "Customer Number not found";
                return View("CustomerMaster", model); // Return an empty model if not found or deleted
            }

            model.Gender = string.IsNullOrEmpty(customer.Gender) ? string.Empty : customer.Gender;

            return View("CustomerMaster", customer);
        }

        public async Task<IActionResult> GetDeleteRetrieve(CustomerMasterModel model)
        {
            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }



            if (string.IsNullOrEmpty(model.MobileNumber))
            {
                ViewBag.ErrorMessage = "Mobile number is required";

                return View("CustomerMaster");
            }

            var customer = await _billingsoftware.SHCustomerMaster.FindAsync(model.MobileNumber, model.BranchID);
            if (customer == null)
            {
                model = new CustomerMasterModel();
                ViewBag.ErrorMessage = "Mobile Number not found";
                return View("CustomerMaster", model);
            }

            if (customer.IsDelete == true)
            {
                customer.IsDelete = false;
                customer.LastUpdatedDate = DateTime.Now.ToString();
                customer.LastUpdatedUser = User.Claims.First().Value.ToString();
                customer.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.Entry(customer).State = EntityState.Modified;
                await _billingsoftware.SaveChangesAsync();
                ViewBag.Message = "Customer Number Retrieved Successfully";
            }
            else
            {
                ViewBag.Message = "Customer Number Already Retrieved ";
            }

            return View("CustomerMaster", customer);
        }



        public async Task<IActionResult> DeleteCustomer(string mobileNumber, CustomerMasterModel model)
        {
            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }



            if (string.IsNullOrEmpty(model.MobileNumber))
            {
                ViewBag.ErrorMessage = "Mobile Number not found";
                return View("Error", new CustomerMasterModel());
            }

            var existingCustomer = await _billingsoftware.SHCustomerMaster.FindAsync(model.MobileNumber, model.BranchID);
            if (existingCustomer == null)
            {
                model = new CustomerMasterModel();
                ViewBag.ErrorMessage = "Mobile Number not found";
                return View("CustomerMaster", model);
            }

            if (existingCustomer.IsDelete)
            {
                ViewBag.ErrorMessage = "Customer Number Already Deleted";
                return View("CustomerMaster", model);
            }

            existingCustomer.IsDelete = true;
            existingCustomer.LastUpdatedDate = DateTime.Now.ToString();
            existingCustomer.LastUpdatedUser = User.Claims.First().Value.ToString();
            existingCustomer.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            _billingsoftware.Entry(existingCustomer).State = EntityState.Modified;

            await _billingsoftware.SaveChangesAsync();

            ViewBag.Message = "Deleted Successfully";
            model = new CustomerMasterModel();

            return View("CustomerMaster", model);
        }


        [HttpPost]
        public async Task<IActionResult> AddDiscountCategory(DiscountCategoryMasterModel model, string buttonType)
        {
            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }



            BusinessClassBilling business = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["discountcategoryid"] = business.GetcategoryID(model.BranchID);

            if (buttonType == "Get")
            {
                var getdiscount = await _billingsoftware.SHDiscountCategory.FirstOrDefaultAsync(x => x.CategoryID == model.CategoryID && !x.IsDelete && x.BranchID == model.BranchID);
                if (getdiscount != null)
                {
                    return View("DiscountCategoryMaster", getdiscount);
                }
                else
                {
                    DiscountCategoryMasterModel obj = new DiscountCategoryMasterModel();
                    ViewBag.ErrorMessage = "No value for this category ID";
                    return View("DiscountCategoryMaster", obj);
                }
            }

            else if (buttonType == "Delete")
            {
                var deletetodiscount = await _billingsoftware.SHDiscountCategory.FindAsync(model.CategoryID, model.BranchID);
                if (deletetodiscount != null)
                {
                    if (deletetodiscount.IsDelete)
                    {
                        ViewBag.ErrorMessage = "Discount Category Not Found";
                        return View("DiscountCategoryMaster", model);
                    }

                    deletetodiscount.IsDelete = true;
                    await _billingsoftware.SaveChangesAsync();

                    ViewBag.Message = "Discount deleted successfully";
                    model = new DiscountCategoryMasterModel();

                    return View("DiscountCategoryMaster", model);
                }
                else
                {
                    DiscountCategoryMasterModel obj = new DiscountCategoryMasterModel();
                    ViewBag.ErrorMessage = "Discount Category not found";
                    return View("DiscountCategoryMaster", obj);
                }
            }

            else if (buttonType == "DeleteRetrieve")
            {
                var discountcategorytoretrieve = await _billingsoftware.SHDiscountCategory.FindAsync(model.CategoryID, model.BranchID);
                if (discountcategorytoretrieve != null)
                {
                    if (discountcategorytoretrieve.IsDelete == true)
                    {

                        discountcategorytoretrieve.IsDelete = false;

                        await _billingsoftware.SaveChangesAsync();

                        model.CategoryID = discountcategorytoretrieve.CategoryID;
                        model.DiscountPrice = discountcategorytoretrieve.DiscountPrice;

                        ViewBag.Message = "Discount category retrieved successfully";
                    }
                    else
                    {
                        ViewBag.Message = "Discount category already retrieved";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Discount category not found";
                }
                return View("DiscountCategoryMaster", model);
            }

            else if (buttonType == "save")
            {

                var existingDiscountCategory = await _billingsoftware.SHDiscountCategory.FindAsync(model.CategoryID, model.BranchID);
                if (existingDiscountCategory != null)
                {
                    if (existingDiscountCategory.IsDelete)
                    {
                        ViewBag.ErrorMessage = "Cannot update. Product is marked as deleted.";
                        return View("DiscountCategoryMaster", model);
                    }
                    existingDiscountCategory.CategoryID = model.CategoryID;
                    existingDiscountCategory.DiscountPrice = model.DiscountPrice;
                    existingDiscountCategory.BranchID = model.BranchID;
                    existingDiscountCategory.LastUpdatedDate = DateTime.Now.ToString();
                    existingDiscountCategory.LastUpdatedUser = User.Claims.First().Value.ToString();
                    existingDiscountCategory.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                    _billingsoftware.Entry(existingDiscountCategory).State = EntityState.Modified;

                }
                else
                {
                    model.LastUpdatedDate = DateTime.Now.ToString();
                    model.LastUpdatedUser = User.Claims.First().Value.ToString();
                    model.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                    _billingsoftware.SHDiscountCategory.Add(model);

                }
                await _billingsoftware.SaveChangesAsync();

                ViewBag.Message = "Saved Successfully";
            }
            model = new DiscountCategoryMasterModel();


            return View("DiscountCategoryMaster", model);

        }

        [HttpPost]

        public async Task<IActionResult> AddGST(GSTMasterModel model)
        {
            var existingGst = await _billingsoftware.SHGSTMaster.FindAsync(model.TaxID);
            if (existingGst != null)
            {
                existingGst.TaxID = model.TaxID;
                existingGst.SGST = model.SGST;
                existingGst.CGST = model.CGST;
                existingGst.OtherTax = model.OtherTax;
                existingGst.LastUpdatedDate = DateTime.Now.ToString();
                existingGst.LastUpdatedUser = User.Claims.First().Value.ToString();
                existingGst.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.Entry(existingGst).State = EntityState.Modified;
            }
            else
            {
                model.LastUpdatedDate = DateTime.Now.ToString();
                model.LastUpdatedUser = User.Claims.First().Value.ToString();
                model.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.SHGSTMaster.Add(model);

            }

            await _billingsoftware.SaveChangesAsync();

            ViewBag.Message = "Saved Successfully";

            return View("GSTMasterModel", model);


        }



        [HttpPost]

        public async Task<IActionResult> AddVoucherDetails(VoucherCustomerDetailModel model)
        {
            var existingVoucher = await _billingsoftware.SHVoucherDetails.FindAsync(model.VoucherID);
            if (existingVoucher != null)
            {
                existingVoucher.VoucherID = model.VoucherID;
                existingVoucher.CustomerID = model.CustomerID;
                existingVoucher.ExpiryDate = model.ExpiryDate;
                existingVoucher.LastUpdatedDate = DateTime.Now.ToString();
                existingVoucher.LastUpdatedUser = User.Claims.First().Value.ToString();
                existingVoucher.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.Entry(existingVoucher).State = EntityState.Modified;
            }
            else
            {
                model.LastUpdatedDate = DateTime.Now.ToString();
                model.LastUpdatedUser = User.Claims.First().Value.ToString();
                model.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.SHVoucherDetails.Add(model);

            }


            await _billingsoftware.SaveChangesAsync();

            ViewBag.Message = "Saved Successfully";

            return View("VoucherCustomerDetails", model);

        }

        [HttpPost]

        public async Task<IActionResult> AddNetDiscount(NetDiscountMasterModel model)
        {

            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }


            var NetID = "1";

            var existingnetdiscount = await _billingsoftware.SHNetDiscountMaster.FindAsync(NetID, model.BranchID);
            if (existingnetdiscount != null)
            {

                existingnetdiscount.BranchID = model.BranchID;
                existingnetdiscount.NetDiscount = model.NetDiscount;
                existingnetdiscount.LastUpdatedDate = DateTime.Now.ToString();
                existingnetdiscount.LastUpdatedUser = User.Claims.First().Value.ToString();
                existingnetdiscount.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.Entry(existingnetdiscount).State = EntityState.Modified;
            }
            else
            {
                model.NetID = NetID;
                model.LastUpdatedDate = DateTime.Now.ToString();
                model.LastUpdatedUser = User.Claims.First().Value.ToString();
                model.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                _billingsoftware.SHNetDiscountMaster.Add(model);

            }

            await _billingsoftware.SaveChangesAsync();

            ViewBag.Message = "Saved Successfully";
            model = new NetDiscountMasterModel();


            return View("NetDiscountMaster", model);

        }

        [HttpPost]

        public async Task<IActionResult> AddVoucher(VoucherMasterModel model)
        {
            var existingvoucher = await _billingsoftware.SHVoucherMaster.FindAsync(model.VoucherID);
            if (existingvoucher == null)
            {
                existingvoucher.VoucherID = model.VoucherID;
                existingvoucher.VoucherNumber = model.VoucherNumber;
                existingvoucher.VocherCost = model.VocherCost;
                existingvoucher.ExpiryDate = model.ExpiryDate;
                existingvoucher.VocherDetails = model.VocherDetails;
                existingvoucher.LastUpdatedDate = DateTime.Now.ToString();
                existingvoucher.LastUpdatedUser = User.Claims.First().Value.ToString();
                existingvoucher.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.Entry(existingvoucher).State = EntityState.Modified;

            }
            else
            {
                model.LastUpdatedDate = DateTime.Now.ToString();
                model.LastUpdatedUser = User.Claims.First().Value.ToString();
                model.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.SHVoucherMaster.Add(model);


            }
            await _billingsoftware.SaveChangesAsync();

            ViewBag.Message = "Saved Successfully";

            return View("VoucherMaster", model);

        }




        //points Master

        public async Task<IActionResult> PointsMaster()
        {
            PointsMasterModel par = new PointsMasterModel();
            if (TempData["BranchID"] != null)
            {
                par.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }
            ViewData["Pointsdata"] = await convetToDataTablePointMaster(par.BranchID);

            return View("PointsMaster", par);
        }



        public async Task<DataTable> convetToDataTablePointMaster(string branchID)
        {

            // Step 1: Perform the query
            var en = _billingsoftware.SHPointsMaster
                                  .Where(e => e.BranchID == branchID).OrderByDescending(e => e.LastUpdatedDate)
                                  .ToList();

            // Step 2: Convert to DataTable
            return BusinessClassBilling.convetToDataTablePointMaster(en);


        }




        [HttpPost]

        public async Task<IActionResult> AddPoints(PointsMasterModel model)
        {
            BusinessClassBilling business = new BusinessClassBilling(_billingsoftware, _configuration);

            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }


            var pointsID = "1";

            var existingpoints = await _billingsoftware.SHPointsMaster.FindAsync(pointsID, model.BranchID);
            if (existingpoints != null)
            {

                existingpoints.NetPrice = model.NetPrice;
                existingpoints.NetPoints = model.NetPoints;
                existingpoints.BranchID = model.BranchID;
                existingpoints.LastUpdatedDate = business.GetFormattedDateTime();
                existingpoints.LastUpdatedUser = User.Claims.First().Value.ToString();
                existingpoints.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.Entry(existingpoints).State = EntityState.Modified;

            }
            else
            {
                model.PointsID = pointsID;
                model.LastUpdatedDate = business.GetFormattedDateTime();
                model.LastUpdatedUser = User.Claims.First().Value.ToString();
                model.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();


                _billingsoftware.SHPointsMaster.Add(model);

            }

            await _billingsoftware.SaveChangesAsync();

            ViewBag.Message = "Saved Successfully";

            var dataTable6 = await convetToDataTablePointMaster(model.BranchID);

            // Store the DataTable in ViewData for access in the view
            ViewData["Pointsdata"] = dataTable6;
            model = new PointsMasterModel();

            return View("PointsMaster", model);

        }


        [HttpPost]

        public async Task<IActionResult> Addrack(RackMasterModel model)
        {
            var existingrack = await _billingsoftware.SHRackMaster.FindAsync(model.PartitionID, model.RackID);
            if (existingrack == null)
            {
                existingrack.RackID = model.RackID;
                existingrack.PartitionID = model.PartitionID;
                existingrack.FacilityName = model.FacilityName;
                existingrack.LastUpdatedDate = DateTime.Now.ToString();
                existingrack.LastUpdatedUser = User.Claims.First().Value.ToString();
                existingrack.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.Entry(existingrack).State = EntityState.Modified;

            }
            else
            {
                model.LastUpdatedDate = DateTime.Now.ToString();
                model.LastUpdatedUser = User.Claims.First().Value.ToString();
                model.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.SHRackMaster.Add(model);

            }

            await _billingsoftware.SaveChangesAsync();

            ViewBag.Message = "Saved Successfully";


            return View("RackMaster", model);

        }







        [HttpPost]
        public async Task<IActionResult> AddRackPartition(RackPatrionProductModel model, string buttonType, RackpartitionViewModel viewmodel)
        {

            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }


            BusinessClassBilling business = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["godownproductid"] = business.GetProductid(model.BranchID);

            if (buttonType == "Get")
            {

                var result = business.GetRackview(model.PartitionID, model.ProductID, model.BranchID);
                if (result == null || !result.Any())
                {
                    ViewBag.GetMessage = "No data found.";
                    viewmodel.Viewrackpartition = new List<RackPatrionProductModel>();
                }
                else
                {
                    var viewModelList = result.Select(p => new RackPatrionProductModel
                    {
                        ProductID = p.ProductID,
                        PartitionID = p.PartitionID,
                        Noofitems = p.Noofitems
                    }).ToList();

                    viewmodel.Viewrackpartition = viewModelList;
                }

                return View("RackPatrionProduct", viewmodel);
            }

            else if (buttonType == "DeleteRetrieve")
            {
                var rolltoretrieve = await _billingsoftware.SHRackPartionProduct.FindAsync(model.PartitionID, model.ProductID, model.BranchID);
                if (rolltoretrieve != null)
                {
                    if (rolltoretrieve.Isdelete == true)
                    {

                        rolltoretrieve.Isdelete = false;


                        await _billingsoftware.SaveChangesAsync();

                        model.PartitionID = rolltoretrieve.PartitionID;
                        model.ProductID = rolltoretrieve.ProductID;
                        model.Noofitems = rolltoretrieve.Noofitems;

                        ViewBag.retMessage = "Deleted ProductID retrieved successfully";
                    }
                    else
                    {
                        ViewBag.retMessage = "ProductID  Already  retrieved";
                    }


                }
                else
                {
                    ViewBag.NovalueMessage = "Data Not Found";
                }

                var modelsre = new RackpartitionViewModel
                {
                    Viewrackpartition = new List<RackPatrionProductModel>()
                };
                return View("RackPatrionProduct", modelsre);
            }

            else if (buttonType == "Delete")
            {
                var rolltoretrieve = await _billingsoftware.SHRackPartionProduct.FindAsync(model.PartitionID, model.ProductID, model.BranchID);
                if (rolltoretrieve != null)
                {
                    if (rolltoretrieve.Isdelete)
                    {
                        ViewBag.ErrorMessage = "ProductID Already Deleted";

                        var modelsre = new RackpartitionViewModel
                        {
                            Viewrackpartition = new List<RackPatrionProductModel>()
                        };
                        return View("RackPatrionProduct", modelsre);
                    }

                    rolltoretrieve.Isdelete = true;


                    await _billingsoftware.SaveChangesAsync();



                    ViewBag.retMessage = "Deleted ProductID  successfully";


                }
                else
                {
                    ViewBag.NovalueMessage = "Data Not Found";
                }

                var modelsdel = new RackpartitionViewModel
                {
                    Viewrackpartition = new List<RackPatrionProductModel>()
                };
                return View("RackPatrionProduct", modelsdel);
            }


            var recstockgodwomn = _billingsoftware.SHGodown.FirstOrDefault(x => x.ProductID == model.ProductID && x.BranchID == model.BranchID);
            if (recstockgodwomn == null)
            {
                ViewBag.entergodowmnMessage = "Please enter the Product and Stock in Godown Master";
                var modelgod = new RackpartitionViewModel
                {
                    Viewrackpartition = new List<RackPatrionProductModel>()
                };
                return View("RackPatrionProduct", modelgod);
            }

            var existingrackpartition = await _billingsoftware.SHRackPartionProduct.FindAsync(model.PartitionID, model.ProductID, model.BranchID);
            if (existingrackpartition != null)
            {
                if (existingrackpartition.Isdelete)
                {
                    ViewBag.ErrorMessage = "Cannot update. Product is marked as deleted.";
                    var modelsins = new RackpartitionViewModel
                    {
                        Viewrackpartition = new List<RackPatrionProductModel>()
                    };

                    return View("RackPatrionProduct", modelsins);
                }


                int newStock;
                if (int.TryParse(model.Noofitems, out newStock))
                {
                    int existingstock;
                    if (int.TryParse(existingrackpartition.Noofitems, out existingstock))
                    {
                        int totalstock = int.Parse(recstockgodwomn.NumberofStocks);

                        if (newStock > totalstock)
                        {
                            ViewBag.stockErrorMessage = $"Only {totalstock} Items Available in Godown Stock";
                            var modelsins = new RackpartitionViewModel
                            {
                                Viewrackpartition = new List<RackPatrionProductModel>()
                            };
                            return View("RackPatrionProduct", modelsins);
                        }

                        int currentstock = totalstock - newStock;

                        if (currentstock < 0)
                        {
                            ViewBag.stockErrorMessage = "Insufficient stock in Godown.";
                            var modelsins = new RackpartitionViewModel
                            {
                                Viewrackpartition = new List<RackPatrionProductModel>()
                            };
                            return View("RackPatrionProduct", modelsins);
                        }

                        recstockgodwomn.NumberofStocks = currentstock.ToString();
                        _billingsoftware.Entry(recstockgodwomn).State = EntityState.Modified;

                        existingrackpartition.PartitionID = model.PartitionID;
                        existingrackpartition.ProductID = model.ProductID;
                        existingrackpartition.Noofitems = model.Noofitems;
                        existingrackpartition.BranchID = model.BranchID;
                        existingrackpartition.LastUpdatedDate = DateTime.Now.ToString();
                        existingrackpartition.LastUpdatedUser = User.Claims.First().Value.ToString();
                        existingrackpartition.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                        _billingsoftware.Entry(existingrackpartition).State = EntityState.Modified;



                        await _billingsoftware.SaveChangesAsync();
                    }
                }
            }
            else
            {
                int newStock;
                if (int.TryParse(model.Noofitems, out newStock))
                {
                    var recstock = _billingsoftware.SHGodown.FirstOrDefault(x => x.ProductID == model.ProductID && x.BranchID == model.BranchID);
                    if (recstock != null)
                    {
                        int totalstock = int.Parse(recstock.NumberofStocks);
                        int currentstock = totalstock - newStock;

                        if (currentstock < 0)
                        {
                            ViewBag.stockErrorMessage = "Insufficient stock in Godown.";
                            var modelsins = new RackpartitionViewModel
                            {
                                Viewrackpartition = new List<RackPatrionProductModel>()
                            };

                            return View("RackPatrionProduct", modelsins);
                        }

                        recstock.NumberofStocks = currentstock.ToString();
                        _billingsoftware.Entry(recstock).State = EntityState.Modified;
                    }

                    model.LastUpdatedDate = DateTime.Now.ToString();
                    model.LastUpdatedUser = User.Claims.First().Value.ToString();
                    model.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                    _billingsoftware.SHRackPartionProduct.Add(model);

                    await _billingsoftware.SaveChangesAsync();
                }
            }

            ViewBag.Message = "Saved Successfully";

            //Repopulate the table after save 

            var updatedResult = business.GetRackview(model.PartitionID, model.ProductID, model.BranchID);
            var updatedViewModelList = updatedResult.Select(p => new RackPatrionProductModel
            {
                ProductID = p.ProductID,
                PartitionID = p.PartitionID,
                Noofitems = p.Noofitems
            }).ToList();

            var models = new RackpartitionViewModel
            {
                Viewrackpartition = new List<RackPatrionProductModel>()
            };
            return View("RackPatrionProduct", models);
        }


        // Edit Function for RackPartition Table
        public async Task<IActionResult> Edit(string partitionID, string productID, RackpartitionViewModel model)
        {
            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            BusinessClassBilling business = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["godownproductid"] = business.GetProductid(model.BranchID);

            var RackEdit = await _billingsoftware.SHRackPartionProduct.FindAsync(partitionID, productID, model.BranchID);
            if (RackEdit == null)
            {
                ViewBag.NovalueMessage = "No Data Found";
            }

            var rackviewTable = new RackpartitionViewModel
            {
                PartitionID = RackEdit.PartitionID,
                ProductID = RackEdit.ProductID,
                Noofitems = RackEdit.Noofitems,

                Viewrackpartition = new List<RackPatrionProductModel>()

            };


            var result = business.GetRackview(RackEdit.PartitionID, RackEdit.ProductID, RackEdit.BranchID);
            if (result != null && result.Any())
            {
                var viewModelList = result.Select(p => new RackPatrionProductModel
                {
                    PartitionID = p.PartitionID,
                    ProductID = p.ProductID,
                    Noofitems = p.Noofitems
                }).ToList();

                rackviewTable.Viewrackpartition = viewModelList;
            }


            return View("RackPatrionProduct", rackviewTable);


        }

        // Delete Function for Rack PArtition
        public async Task<IActionResult> Delete(string partitionID, string productID, RackpartitionViewModel viewmodel, RackPatrionProductModel model)
        {
            if (TempData["BranchID"] != null)
            {
                viewmodel.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }


            BusinessClassBilling business = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["godownproductid"] = business.GetProductid(model.BranchID);


            var rackDel = await _billingsoftware.SHRackPartionProduct.FindAsync(partitionID, productID, viewmodel.BranchID);
            if (rackDel != null)
            {
                rackDel.Isdelete = true;
                await _billingsoftware.SaveChangesAsync();
            }
            ViewBag.Delete = "Deleted  Successfully.";

            model = new RackPatrionProductModel();
            viewmodel.Viewrackpartition = new List<RackPatrionProductModel>();

            return View("RackPatrionProduct", viewmodel);
        }



        //imageupload

        private bool IsImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            // Check the file's MIME type to confirm it's an image
            if (file.ContentType.ToLower().StartsWith("image/"))
                return true;

            // You can add more checks here depending on your requirements

            return false;
        }




        public IActionResult GetIdProofImage(string staffId, string branchId)
        {
            var staffAdmin = _billingsoftware.SHStaffAdmin
                .FirstOrDefault(x => x.StaffID == staffId && x.BranchID == branchId && x.IsDelete == false);

            if (staffAdmin != null && staffAdmin.IdProofFile != null)
            {
                // Return the file with appropriate content type
                return File(staffAdmin.IdProofFile, "image/jpeg", "id-proof.jpg"); // Adjust filename and MIME type if necessary
            }

            return NotFound();  // Return 404 if the image is not found
        }



        public async Task<DataTable> AdditionalStaffFun(string branchID)
        {

            // Step 1: Perform the query
            var entities = await (from staff in _billingsoftware.SHStaffAdmin
                                  join rol in _billingsoftware.SHrollaccess
                                  on staff.StaffID equals rol.StaffID into rolacc
                                  from s in rolacc.DefaultIfEmpty()
                                  join rolname in _billingsoftware.SHrollType on s.RollID equals rolname.RollID into roll
                                  from r in roll.DefaultIfEmpty()
                                  where staff.BranchID == branchID && staff.IsDelete == false
                                  orderby staff.LastupdatedDate descending
                                  select new StaffAdminModel
                                  {
                                      StaffID = staff.StaffID,
                                      FullName = staff.FullName,
                                      ResourceTypeID = r.RollName,
                                      PhoneNumber = staff.PhoneNumber,
                                      EmailId = staff.EmailId
                                  }).ToListAsync();

            // Step 2: Convert to DataTable
            return BusinessClassBilling.ConvertToDataTableStaff(entities);


        }



        public async Task<IActionResult> StaffAdmin()
        {
            var model = new StaffAdminModel();

            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            BusinessClassBilling Busbill = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["resoruseid"] = Busbill.GetResourceid(model.BranchID);
            ViewData["branchid"] = Busbill.Getbranch();

            var dataTable = await AdditionalStaffFun(model.BranchID);

            // Store the DataTable in ViewData for access in the view
            ViewData["StaffData"] = dataTable;

            return View(model);
        }

        // staff reg
        [HttpPost]
        public async Task<IActionResult> AddStaff(StaffAdminModel model, string buttontype, IFormFile imageFile)
        {

            BusinessClassBilling Busbill = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["resoruseid"] = Busbill.GetResourceid(model.BranchID);
            ViewData["branchid"] = Busbill.Getbranch();




            if (buttontype == "Get")
            {
                if (TempData["BranchID"] != null)
                {
                    model.BranchID = TempData["BranchID"].ToString();
                    TempData.Keep("BranchID");
                }

                var getstaff = await _billingsoftware.SHStaffAdmin.FirstOrDefaultAsync(x => x.StaffID == model.StaffID && x.IsDelete == false && x.BranchID == model.BranchID);
                if (getstaff != null)
                {
                    model.Gender = string.IsNullOrEmpty(getstaff.Gender) ? string.Empty : getstaff.Gender;


                    var checkid = await _billingsoftware.SHStaffAdmin.FirstOrDefaultAsync(x => x.StaffID == model.StaffID && x.IsDelete == false && x.BranchID == model.BranchID && x.IdProofFile != null);

                    if (checkid != null)
                    {

                        // Prepare the image URL
                        ViewBag.ImageUrl = Url.Action("GetIdProofImage", new { staffId = getstaff.StaffID, branchId = getstaff.BranchID });
                        var dataTable1 = await AdditionalStaffFun(model.BranchID);

                        // Store the DataTable in ViewData for access in the view
                        ViewData["StaffData"] = dataTable1;
                        return View("StaffAdmin", getstaff);
                    }
                    else
                    {
                        var dataTable1 = await AdditionalStaffFun(model.BranchID);

                        // Store the DataTable in ViewData for access in the view
                        ViewData["StaffData"] = dataTable1;
                        return View("StaffAdmin", getstaff);

                    }
                }
                else
                {
                    StaffAdminModel par = new StaffAdminModel();
                    ViewBag.getMessage = "No Data found for this Staff ID";
                    var dataTable2 = await AdditionalStaffFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["StaffData"] = dataTable2;
                    return View("StaffAdmin", par);
                }

            }
            else if (buttontype == "Delete")
            {
                if (TempData["BranchID"] != null)
                {
                    model.BranchID = TempData["BranchID"].ToString();
                    TempData.Keep("BranchID");
                }

                var stafftodelete = await _billingsoftware.SHStaffAdmin.FindAsync(model.StaffID, model.BranchID);
                if (stafftodelete != null)
                {
                    if (stafftodelete.IsDelete)
                    {
                        ViewBag.ErrorMessage = "StaffID Already Deleted";
                        var dataTable3 = await AdditionalStaffFun(model.BranchID);

                        // Store the DataTable in ViewData for access in the view
                        ViewData["StaffData"] = dataTable3;
                        return View("StaffAdmin", model);
                    }

                    stafftodelete.IsDelete = true;
                    await _billingsoftware.SaveChangesAsync();

                    ViewBag.delMessage = "StaffID deleted successfully";
                    var dataTable4 = await AdditionalStaffFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["StaffData"] = dataTable4;

                    model = new StaffAdminModel();
                    return View("StaffAdmin", model);
                }
                else
                {
                    ViewBag.delnoMessage = "StaffID not found";
                    var dataTable5 = await AdditionalStaffFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["StaffData"] = dataTable5;
                    model = new StaffAdminModel();
                    return View("StaffAdmin", model);
                }

            }

            else if (buttontype == "DeleteRetrieve")
            {

                if (TempData["BranchID"] != null)
                {
                    model.BranchID = TempData["BranchID"].ToString();
                    TempData.Keep("BranchID");
                }


                var stafftoretrieve = await _billingsoftware.SHStaffAdmin.FindAsync(model.StaffID, model.BranchID);
                if (stafftoretrieve != null)
                {
                    if (stafftoretrieve.IsDelete == true)
                    {

                        stafftoretrieve.IsDelete = false;

                        await _billingsoftware.SaveChangesAsync();

                        model.StaffID = stafftoretrieve.StaffID;
                        model.FullName = stafftoretrieve.FullName;
                        model.ResourceTypeID = stafftoretrieve.ResourceTypeID;
                        model.FirstName = stafftoretrieve.FirstName;
                        model.LastName = stafftoretrieve.LastName;
                        model.Initial = stafftoretrieve.Initial;
                        model.Prefix = stafftoretrieve.Prefix;
                        model.PhoneNumber = stafftoretrieve.PhoneNumber;
                        model.DateofBirth = stafftoretrieve.DateofBirth;
                        model.Age = stafftoretrieve.Age;
                        model.Gender = stafftoretrieve.Gender;
                        model.Address1 = stafftoretrieve.Address1;
                        model.City = stafftoretrieve.City;
                        model.State = stafftoretrieve.State;
                        model.Pin = stafftoretrieve.Pin;
                        model.EmailId = stafftoretrieve.EmailId;
                        model.Nationality = stafftoretrieve.Nationality;
                        model.UserName = stafftoretrieve.UserName;
                        model.Password = stafftoretrieve.Password;
                        model.IdProofId = stafftoretrieve.IdProofId;
                        model.IdProofName = stafftoretrieve.IdProofName;
                        model.IdProofFile = stafftoretrieve.IdProofFile;

                        ViewBag.retMessage = "Deleted StaffID retrieved successfully";
                    }
                    else
                    {
                        ViewBag.retMessage = "StaffID  Already retrieved";
                    }
                }
                else
                {
                    ViewBag.noretMessage = "StaffID not found";
                }
                var dataTable6 = await AdditionalStaffFun(model.BranchID);

                // Store the DataTable in ViewData for access in the view
                ViewData["StaffData"] = dataTable6;
                return View("StaffAdmin", model);
            }



            var staffcheck = await _billingsoftware.SHStaffAdmin.FirstOrDefaultAsync(x => x.StaffID == model.StaffID && x.BranchID == model.BranchID && (x.UserName != model.UserName));



            if (staffcheck == null)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Validate that the uploaded file is an image (optional)
                    if (!IsImage(imageFile))
                    {
                        ModelState.AddModelError(string.Empty, "Uploaded file is not an image.");
                        var dataTable7 = await AdditionalStaffFun(model.BranchID);

                        // Store the DataTable in ViewData for access in the view
                        ViewData["StaffData"] = dataTable7;
                        return View("StaffAdmin", model);
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(memoryStream);
                        model.IdProofFile = memoryStream.ToArray();
                    }

                }
                else
                {
                    // If no new file is uploaded, retain the existing ID proof file
                    var mod = await _billingsoftware.SHStaffAdmin.FindAsync(model.StaffID, model.BranchID);
                    if (mod != null)
                    {
                        model.IdProofFile = mod.IdProofFile; // Retain existing file
                    }
                }



                var existingStaffAdmin = await _billingsoftware.SHStaffAdmin.FindAsync(model.StaffID, model.BranchID);


                if (string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.Password))
                {
                    ViewBag.validateMessage = "Username and Password are required.";
                    var dataTable8 = await AdditionalStaffFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["StaffData"] = dataTable8;
                    return View("StaffAdmin", model);
                }

                if (existingStaffAdmin != null)
                {
                    if (existingStaffAdmin.IsDelete)
                    {
                        ViewBag.ErrorMessage = "Cannot update. Product is marked as deleted.";
                        var dataTable9 = await AdditionalStaffFun(model.BranchID);

                        // Store the DataTable in ViewData for access in the view
                        ViewData["StaffData"] = dataTable9;
                        return View("StaffAdmin", model);
                    }

                    existingStaffAdmin.StaffID = model.StaffID;
                    existingStaffAdmin.ResourceTypeID = model.ResourceTypeID;
                    existingStaffAdmin.BranchID = model.BranchID;
                    existingStaffAdmin.FirstName = model.FirstName;
                    existingStaffAdmin.LastName = model.LastName;
                    existingStaffAdmin.FullName = model.FullName;
                    existingStaffAdmin.Initial = model.Initial;
                    existingStaffAdmin.Prefix = model.Prefix;
                    existingStaffAdmin.Age = model.Age;
                    existingStaffAdmin.DateofBirth = model.DateofBirth;
                    existingStaffAdmin.EmailId = model.EmailId;
                    existingStaffAdmin.Address1 = model.Address1;
                    existingStaffAdmin.City = model.City;
                    existingStaffAdmin.State = model.State;
                    existingStaffAdmin.Pin = model.Pin;
                    existingStaffAdmin.PhoneNumber = model.PhoneNumber;
                    existingStaffAdmin.EmailId = model.EmailId;
                    existingStaffAdmin.Nationality = model.Nationality;
                    existingStaffAdmin.UserName = model.UserName;
                    existingStaffAdmin.Password = model.Password;
                    existingStaffAdmin.IdProofId = model.IdProofId;
                    existingStaffAdmin.IdProofName = model.IdProofName;
                    existingStaffAdmin.IdProofFile = model.IdProofFile;
                    existingStaffAdmin.LastupdatedDate = DateTime.ParseExact(Busbill.GetFormattedDateTime(), "dd/MM/yyyy HH:mm:ss", null);
                    existingStaffAdmin.LastupdatedUser = User.Claims.First().Value.ToString();
                    existingStaffAdmin.LastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    existingStaffAdmin.Gender = model.Gender;

                    _billingsoftware.Entry(existingStaffAdmin).State = EntityState.Modified;

                }
                else
                {

                    model.LastupdatedDate = DateTime.ParseExact(Busbill.GetFormattedDateTime(), "dd/MM/yyyy HH:mm:ss", null);
                    model.LastupdatedUser = User.Claims.First().Value.ToString();
                    model.LastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    _billingsoftware.SHStaffAdmin.Add(model);
                }
            }
            else
            {

                ViewBag.ExistMessage = "Cannot Update Username";
                var dataTable10 = await AdditionalStaffFun(model.BranchID);

                // Store the DataTable in ViewData for access in the view
                ViewData["StaffData"] = dataTable10;
                return View("StaffAdmin", model);
            }
            await _billingsoftware.SaveChangesAsync();

            ViewBag.Message = "Saved Successfully";

            var dataTable = await AdditionalStaffFun(model.BranchID);

            // Store the DataTable in ViewData for access in the view
            ViewData["StaffData"] = dataTable;
            model = new StaffAdminModel();
            return View("StaffAdmin", model);


        }








        public async Task<IActionResult> AddResourceType(ResourceTypeMasterModel model, string buttontype)
        {
            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }


            if (buttontype == "Get")
            {
                var getres = await _billingsoftware.SHresourceType.FirstOrDefaultAsync(x => x.ResourceTypeID == model.ResourceTypeID && x.IsDelete == false && x.BranchID == model.BranchID);
                if (getres != null)
                {
                    return View("ResourceTypeMaster", getres);
                }
                else
                {
                    ResourceTypeMasterModel res = new ResourceTypeMasterModel();
                    ViewBag.getMessage = "No Data found for this ResourceTypeID";
                    return View("ResourceTypeMaster", res);
                }
            }
            else if (buttontype == "Delete")
            {
                var restodelete = await _billingsoftware.SHresourceType.FindAsync(model.ResourceTypeID, model.BranchID);
                if (restodelete != null)
                {
                    if (restodelete.IsDelete)
                    {
                        ViewBag.ErrorMessage = "ResourceTypeID Already Deleted";
                        return View("ResourceTypeMaster", model);
                    }

                    restodelete.IsDelete = true;
                    await _billingsoftware.SaveChangesAsync();

                    ViewBag.delMessage = "ResourceTypeID deleted successfully";
                    model = new ResourceTypeMasterModel();

                    return View("ResourceTypeMaster", model);
                }
                else
                {
                    ResourceTypeMasterModel res = new ResourceTypeMasterModel();
                    ViewBag.delnoMessage = "ResourceTypeID not found";
                    return View("ResourceTypeMaster", res);
                }

            }

            else if (buttontype == "DeleteRetrieve")
            {
                var restoretrieve = await _billingsoftware.SHresourceType.FindAsync(model.ResourceTypeID, model.BranchID);
                if (restoretrieve != null)
                {
                    if (restoretrieve.IsDelete == true)
                    {

                        restoretrieve.IsDelete = false;

                        await _billingsoftware.SaveChangesAsync();

                        model.ResourceTypeName = restoretrieve.ResourceTypeName;
                        model.ResourceTypeID = restoretrieve.ResourceTypeID;

                        ViewBag.retMessage = "Deleted ResourceTypeID retrieved successfully";
                    }
                    else
                    {
                        ViewBag.retMessage = "ResourceTypeID Already retrieved";
                    }
                }
                else
                {
                    ResourceTypeMasterModel res = new ResourceTypeMasterModel();
                    ViewBag.noretMessage = "ResourceTypeID not found";
                    return View("ResourceTypeMaster", res);
                }
                return View("ResourceTypeMaster", model);
            }


            var existingres = await _billingsoftware.SHresourceType.FindAsync(model.ResourceTypeID, model.BranchID);

            if (existingres != null)
            {
                if (existingres.IsDelete)
                {
                    ViewBag.ErrorMessage = "Cannot update. Product is marked as deleted.";
                    return View("ResourceTypeMaster", model);
                }

                existingres.ResourceTypeName = model.ResourceTypeName;
                existingres.ResourceTypeID = model.ResourceTypeID;
                existingres.BranchID = model.BranchID;
                existingres.lastUpdatedDate = DateTime.Now.ToString();
                existingres.lastUpdatedUser = User.Claims.First().Value.ToString();
                existingres.lastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.Entry(existingres).State = EntityState.Modified;

            }
            else
            {

                model.lastUpdatedDate = DateTime.Now.ToString();
                model.lastUpdatedUser = User.Claims.First().Value.ToString();
                model.lastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                _billingsoftware.SHresourceType.Add(model);
            }
            await _billingsoftware.SaveChangesAsync();

            ViewBag.Message = "Saved Successfully";
            model = new ResourceTypeMasterModel();

            return View("ResourceTypeMaster", model);


        }


        public async Task<IActionResult> AddRoleaccess(RoleAccessModel model, string buttontype)
        {

            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            BusinessClassBilling businessbill = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["screenname"] = businessbill.Screenname();
            ViewData["rollid"] = businessbill.RollAccessType(model.BranchID);
            ViewData["staffid"] = businessbill.GetStaffID(model.BranchID);




            if (buttontype == "Get")
            {
                var getrol = await _billingsoftware.SHRoleaccessModel.FirstOrDefaultAsync(x => x.RollID == model.RollID && x.ScreenID == model.ScreenID && x.Isdelete == false && x.BranchID == model.BranchID);
                if (getrol != null)
                {
                    return View("RoleAccess", getrol);
                }
                else
                {
                    RoleAccessModel role = new RoleAccessModel();
                    ViewBag.getMessage = "No Data found for this RollID";
                    return View("RoleAccess", role);
                }
            }
            else if (buttontype == "Delete")
            {
                var roletodelete = await _billingsoftware.SHRoleaccessModel.FindAsync(model.RollID, model.ScreenID, model.BranchID);
                if (roletodelete != null)
                {
                    if (roletodelete.Isdelete)
                    {
                        ViewBag.ErrorMessage = "RollID Already Deleted";
                        return View("RoleAccess", model);
                    }

                    roletodelete.Isdelete = true;
                    await _billingsoftware.SaveChangesAsync();

                    ViewBag.delMessage = "RollID deleted successfully";
                    model = new RoleAccessModel();

                    return View("RoleAccess", model);
                }
                else
                {
                    ViewBag.delnoMessage = "RollID not found";
                    model = new RoleAccessModel();

                    return View("RoleAccess", model);
                }

            }

            else if (buttontype == "DeleteRetrieve")
            {
                var roltoretrieve = await _billingsoftware.SHRoleaccessModel.FindAsync(model.RollID, model.ScreenID, model.BranchID);
                if (roltoretrieve != null)
                {
                    if (roltoretrieve.Isdelete == true)
                    {

                        roltoretrieve.Isdelete = false;

                        await _billingsoftware.SaveChangesAsync();

                        model.RollID = roltoretrieve.RollID;
                        model.ScreenID = roltoretrieve.ScreenID;
                        model.Access = roltoretrieve.Access;
                        model.Authorized = roltoretrieve.Authorized;

                        ViewBag.retMessage = "Deleted RollID retrieved successfully";
                    }
                    else
                    {
                        ViewBag.retMessage = "RollID Already retrieved";
                    }
                }
                else
                {
                    ViewBag.noretMessage = "RollID not found";
                }
                return View("RoleAccess", model);
            }


            var existingrole = await _billingsoftware.SHRoleaccessModel.FindAsync(model.RollID, model.ScreenID, model.BranchID);

            if (existingrole != null)
            {
                if (existingrole.Isdelete)
                {
                    ViewBag.ErrorMessage = "Cannot update. Product is marked as deleted.";
                    return View("RoleAccess", model);
                }

                existingrole.BranchID = model.BranchID;
                existingrole.RollID = model.RollID;
                existingrole.ScreenID = model.ScreenID;
                existingrole.Access = "true";
                existingrole.Authorized = model.Authorized;
                existingrole.lastUpdatedDate = businessbill.GetFormattedDateTime();
                existingrole.lastUpdatedUser = User.Claims.First().Value.ToString();
                existingrole.lastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.Entry(existingrole).State = EntityState.Modified;

            }
            else
            {
                model.Access = "true";
                model.lastUpdatedDate = businessbill.GetFormattedDateTime();
                model.lastUpdatedUser = User.Claims.First().Value.ToString();
                model.lastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                _billingsoftware.SHRoleaccessModel.Add(model);
            }
            await _billingsoftware.SaveChangesAsync();

            ViewBag.Message = "Saved Successfully";
            model = new RoleAccessModel();

            return View("RoleAccess", model);
        }

        public async Task<IActionResult> AddRollmaster(RollAccessMaster model, string buttontype, List<string> SelectedRollNames)
        {
            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            BusinessClassBilling Busbill = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["rollid"] = Busbill.RollAccessType(model.BranchID);
            ViewData["staffid"] = Busbill.GetStaffID(model.BranchID);


            if (buttontype == "Get")
            {
                var getroll = await _billingsoftware.SHrollaccess.FirstOrDefaultAsync(x => x.StaffID == model.StaffID && x.IsDelete == false && x.BranchID == model.BranchID);
                if (getroll != null)
                {
                    // Fetch the roles for the given staff ID
                    var staffRoles = await _billingsoftware.SHrollaccess
                        .Where(x => x.StaffID == model.StaffID && x.IsDelete == false && x.BranchID == model.BranchID)
                        .Select(x => x.RollID)
                        .ToListAsync();

                    // Store the staff roles in ViewData
                    ViewData["StaffRoles"] = staffRoles;

                    return View("RollAccessMaster", getroll);
                }
                else
                {
                    RollAccessMaster roll = new RollAccessMaster();
                    ViewBag.getMessage = "No Data found for this RollID";
                    return View("RollAccessMaster", roll);
                }
            }
            else if (buttontype == "Delete")
            {
                if (!SelectedRollNames.Any())
                {
                    ViewBag.ErrorMessage = "Please select roll.";
                    return View("RollAccessMaster", model);
                }

                foreach (var rollName in SelectedRollNames)
                {

                    var rolltodelete = await _billingsoftware.SHrollaccess.FindAsync(model.StaffID, rollName, model.BranchID);
                    if (rolltodelete != null)
                    {
                        if (rolltodelete.IsDelete)
                        {
                            ViewBag.ErrorMessage = "RollID Already Deleted";
                            return View("RollAccessMaster", model);
                        }

                        rolltodelete.IsDelete = true;
                        await _billingsoftware.SaveChangesAsync();

                        ViewBag.delMessage = "RollID deleted successfully";
                        model = new RollAccessMaster();


                        return View("RollAccessMaster", model);
                    }

                    else
                    {
                        ViewBag.delnoMessage = "RollID not found";
                        model = new RollAccessMaster();
                        return View("RollAccessMaster", model);
                    }
                }


            }

            else if (buttontype == "DeleteRetrieve")
            {
                foreach (var rollName in SelectedRollNames)
                {
                    var rolltoretrieve = await _billingsoftware.SHrollaccess.FindAsync(model.StaffID, rollName, model.BranchID);
                    if (rolltoretrieve != null)
                    {
                        if (rolltoretrieve.IsDelete == true)
                        {

                            rolltoretrieve.IsDelete = false;

                            await _billingsoftware.SaveChangesAsync();

                            model.RollID = rolltoretrieve.RollID;
                            model.StaffID = rolltoretrieve.StaffID;

                            ViewBag.retMessage = "Deleted RollID retrieved successfully";
                            return View("RollAccessMaster", rolltoretrieve);
                        }
                        else
                        {
                            ViewBag.retMessage = "RollID Already retrieved ";
                        }
                    }
                    else
                    {
                        ViewBag.noretMessage = "RollID not found";
                    }
                }
                return View("RollAccessMaster", model);
            }


            else if (buttontype == "Save")
            {
                if (!SelectedRollNames.Any())
                {
                    ViewBag.ErrorMessage = "Please select roll.";
                    return View("RollAccessMaster", model);
                }

                foreach (var rollName in SelectedRollNames)
                {

                    var existingroll = await _billingsoftware.SHrollaccess.FindAsync(model.StaffID, rollName, model.BranchID);

                    if (existingroll != null)
                    {
                        var duplicateRoll = _billingsoftware.SHrollaccess
                     .FirstOrDefault(x => x.RollID == model.RollID && x.StaffID != model.StaffID && x.BranchID == model.BranchID);

                        if (duplicateRoll == null)
                        {
                            ViewBag.ErrorMessage = "RollID already exists Cannot update same ID";
                            return View("RollAccessMaster", model);
                        }

                        if (existingroll.IsDelete)
                        {
                            ViewBag.ErrorMessage = "Cannot update. Product is marked as deleted.";
                            return View("RollAccessMaster", model);
                        }

                        existingroll.BranchID = model.BranchID;
                        existingroll.RollID = model.RollID;
                        existingroll.StaffID = model.StaffID;
                        existingroll.LastupdatedDate = Busbill.GetFormattedDateTime();
                        existingroll.Lastupdateduser = User.Claims.First().Value.ToString();
                        existingroll.LastupdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                        _billingsoftware.Entry(existingroll).State = EntityState.Modified;

                    }
                    else
                    {
                        var newAccess = new RollAccessMaster
                        {
                            BranchID = model.BranchID,
                            StaffID = model.StaffID,
                            RollID = rollName,
                            LastupdatedDate = Busbill.GetFormattedDateTime(),
                            Lastupdateduser = User.Claims.First().Value.ToString(),
                            LastupdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                        };
                        _billingsoftware.SHrollaccess.Add(newAccess);

                    }
                }
                await _billingsoftware.SaveChangesAsync();

                ViewBag.Message = "Saved Successfully";
            }
            model = new RollAccessMaster();
            return View("RollAccessMaster", model);


        }

        public async Task<IActionResult> AddRolltype(RollTypeMaster model, string buttontype)
        {

            BusinessClassBilling business = new BusinessClassBilling(_billingsoftware, _configuration);

            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            if (buttontype == "Get")
            {
                var getrolltype = await _billingsoftware.SHrollType.FirstOrDefaultAsync(x => x.RollID == model.RollID && x.IsDelete == false && x.BranchID == model.BranchID);
                if (getrolltype != null)
                {
                    return View("RollTypeMaster", getrolltype);
                }
                else
                {
                    RollTypeMaster rolltype = new RollTypeMaster();
                    ViewBag.getMessage = "No Data found for this RollID";
                    return View("RollTypeMaster", rolltype);
                }
            }
            else if (buttontype == "Delete")
            {
                var rolltypetodelete = await _billingsoftware.SHrollType.FindAsync(model.RollID, model.BranchID);
                if (rolltypetodelete != null)
                {
                    if (rolltypetodelete.IsDelete)
                    {
                        ViewBag.ErrorMessage = "RollID Already Deleted ";
                        return View("RollTypeMaster", model);
                    }

                    rolltypetodelete.IsDelete = true;
                    await _billingsoftware.SaveChangesAsync();

                    ViewBag.delMessage = "RollID deleted successfully";
                    model = new RollTypeMaster();
                    return View("RollTypeMaster", model);
                }
                else
                {
                    RollTypeMaster rolltype = new RollTypeMaster();
                    ViewBag.delnoMessage = "RollID not found";

                    return View("RollTypeMaster", rolltype);
                }

            }

            else if (buttontype == "DeleteRetrieve")
            {
                var rolltypetoretrieve = await _billingsoftware.SHrollType.FindAsync(model.RollID, model.BranchID);
                if (rolltypetoretrieve != null)
                {
                    if (rolltypetoretrieve.IsDelete == true)
                    {

                        rolltypetoretrieve.IsDelete = false;

                        await _billingsoftware.SaveChangesAsync();

                        model.RollID = rolltypetoretrieve.RollID;
                        model.RollName = rolltypetoretrieve.RollName;

                        ViewBag.retMessage = "Deleted RollID retrieved successfully";
                    }
                    else
                    {
                        ViewBag.retMessage = " RollID Already retrieved ";
                    }

                }
                else
                {

                    ViewBag.noretMessage = "RollID not found";
                }
                return View("RollTypeMaster", model);
            }


            var existingrolltype = await _billingsoftware.SHrollType.FindAsync(model.RollID, model.BranchID);

            if (existingrolltype != null)
            {
                if (existingrolltype.IsDelete)
                {
                    ViewBag.ErrorMessage = "Cannot update. Product is marked as deleted.";
                    return View("RollTypeMaster", model);
                }

                existingrolltype.BranchID = model.BranchID;
                existingrolltype.RollID = model.RollID;
                existingrolltype.RollName = model.RollName;
                existingrolltype.LastupdatedDate = business.GetFormattedDateTime();
                existingrolltype.LastupdatedUser = User.Claims.First().Value.ToString();
                existingrolltype.LastupdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.Entry(existingrolltype).State = EntityState.Modified;

            }
            else
            {

                model.LastupdatedDate = business.GetFormattedDateTime();
                model.LastupdatedUser = User.Claims.First().Value.ToString();
                model.LastupdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                _billingsoftware.SHrollType.Add(model);
            }
            await _billingsoftware.SaveChangesAsync();

            ViewBag.Message = "Saved Successfully";

            model = new RollTypeMaster();
            return View("RollTypeMaster", model);

        }

        public async Task<IActionResult> Addscreen(ScreenMasterModel model, string buttontype)
        {

            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }


            BusinessClassBilling businessbill = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["screenname"] = businessbill.Screenname();

            if (buttontype == "Get")
            {
                var getscreen = await _billingsoftware.SHScreenMaster.FirstOrDefaultAsync(x => x.ScreenId == model.ScreenId && x.IsDelete == false && x.BranchID == model.BranchID);
                if (getscreen != null)
                {
                    return View("ScreenMaster", getscreen);
                }
                else
                {
                    ScreenMasterModel screen = new ScreenMasterModel();
                    ViewBag.getMessage = "No Data found for this ScreenId";
                    return View("ScreenMaster", screen);
                }

            }

            else if (buttontype == "Delete")
            {
                var screentodelete = await _billingsoftware.SHScreenMaster.FindAsync(model.ScreenId, model.BranchID);

                if (screentodelete != null)
                {
                    if (screentodelete.IsDelete)
                    {
                        ViewBag.ErrorMessage = "ScreenID Already Deleted";
                        return View("ScreenMaster", model);
                    }


                    screentodelete.IsDelete = true;
                    await _billingsoftware.SaveChangesAsync();
                    ViewBag.delMessage = "ScreenId deleted successfully";
                    model = new ScreenMasterModel();

                    return View("ScreenMaster", model);
                }
                else
                {

                    ScreenMasterModel scrn = new ScreenMasterModel();
                    ViewBag.delnoMessage = "ScreenId not found";
                    return View("ScreenMaster", scrn);
                }

            }

            else if (buttontype == "DeleteRetrieve")
            {
                var screentoretrieve = await _billingsoftware.SHScreenMaster.FindAsync(model.ScreenId, model.BranchID);
                if (screentoretrieve != null)
                {
                    if (screentoretrieve.IsDelete == true)
                    {

                        screentoretrieve.IsDelete = false;

                        await _billingsoftware.SaveChangesAsync();

                        model.ScreenId = screentoretrieve.ScreenId;
                        model.ScreenName = screentoretrieve.ScreenName;

                        ViewBag.retMessage = "Deleted ScreenId retrieved successfully";
                    }
                    else
                    {
                        ViewBag.retMessage = "ScreenId Already retrieved";
                    }
                }
                else
                {
                    ScreenMasterModel scrn = new ScreenMasterModel();
                    ViewBag.noretMessage = "ScreenId not found";
                }
                return View("ScreenMaster", model);
            }


            var existingscreen = await _billingsoftware.SHScreenMaster.FindAsync(model.ScreenId, model.BranchID);

            if (existingscreen != null)
            {
                if (existingscreen.IsDelete)
                {
                    ViewBag.ErrorMessage = "Cannot update. Product is marked as deleted.";
                    return View("ScreenMaster", model);
                }

                existingscreen.BranchID = model.BranchID;
                existingscreen.ScreenId = model.ScreenId;
                existingscreen.ScreenName = model.ScreenName;
                existingscreen.lastUpdatedDate = DateTime.Now.ToString();
                existingscreen.lastUpdatedUser = User.Claims.First().Value.ToString();
                existingscreen.lastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.Entry(existingscreen).State = EntityState.Modified;

            }
            else
            {

                model.lastUpdatedDate = DateTime.Now.ToString();
                model.lastUpdatedUser = User.Claims.First().Value.ToString();
                model.lastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                _billingsoftware.SHScreenMaster.Add(model);
            }
            await _billingsoftware.SaveChangesAsync();

            ViewBag.Message = "Saved Successfully";
            model = new ScreenMasterModel();
            return View("ScreenMaster", model);

        }

        //This Method used for Customer Billing
        [HttpPost]
        public async Task<IActionResult> getCustomerBill(BillProductlistModel model, string buttonType, string BillID, string BillDate, string CustomerNumber, string BranchID, string TotalPrice, BillingMasterModel masterModel, BillingDetailsModel detailModel, string Quantity)
        {

            if (model.BillID != null)
            {
                HttpContext.Session.SetString("BillID", model.BillID);
            }
            else
            {
                HttpContext.Session.SetString("BillID", string.Empty);
            }


            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            HttpContext.Session.SetString("BranchID", model.BranchID);



            BusinessClassBilling Busbill = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["productid"] = Busbill.Getproduct(model.BranchID);



            //Code for print the Bill 
            if (buttonType == "Download Bill")
            {
                var checkbillavailable = _billingsoftware.SHbillmaster.FirstOrDefault(x => x.BillID == model.BillID && x.BillDate == model.BillDate && x.CustomerNumber == model.CustomerNumber && x.BranchID == model.BranchID && x.IsDelete == false);

                if (checkbillavailable == null)
                {
                    ViewBag.Getnotfound = "BillID Not Found";

                    return View("CustomerBilling", model);
                }

                String Query = "SELECT \r\n    SD.BillID,\r\n    CONVERT(VARCHAR(10), SD.BillDate, 101) AS BillDate,\r\n    SD.ProductID,\r\n    SP.ProductName,\r\n    FORMAT(TRY_CAST(REPLACE(SD.Price, ',', '') AS DECIMAL(18, 2)), 'N2') AS Price,  -- Remove commas before casting\r\n    SD.Quantity,\r\n    (SELECT Address FROM SHCustomerMaster WHERE MobileNumber = SD.CustomerNumber AND BranchID = SD.BranchID) AS CustomerAddress,\r\n    (SELECT CustomerName FROM SHCustomerMaster WHERE MobileNumber = SD.CustomerNumber AND BranchID = SD.BranchID) AS CustomerName,\r\n    SD.CustomerNumber,\r\n    FORMAT(TRY_CAST(SD.TotalDiscount AS DECIMAL(18, 2)), 'N2') AS DetailDiscount, \r\n    FORMAT(TRY_CAST(SD.Totalprice AS DECIMAL(18, 2)), 'N2') AS DetailTotalprice, \r\n     SB.CGSTPercentage,\r\n    SB.SGSTPercentage,\r\n    SB.TotalDiscount,\r\n    FORMAT(TRY_CAST(SB.NetPrice AS DECIMAL(18, 2)), 'N2') AS MasterTotalprice, \r\n    SP.SerialNumber,\r\n    FORMAT(TRY_CAST(SD.NetPrice AS DECIMAL(18, 2)), 'N2') AS NetPrice, \r\n    (SELECT PaymentID FROM SHPaymentMaster WHERE BillID = SD.BillID AND BillDate = SD.BillDate AND BranchID = SD.BranchID) AS PaymentId\r\nFROM \r\n    SHbilldetails SD\r\nINNER JOIN \r\n    SHbillmaster SB ON SD.BillID = SB.BillID\r\nINNER JOIN \r\n    SHProductMaster SP ON SD.ProductID = SP.ProductID\r\nWHERE \r\n    SD.IsDelete = 0\r\n    AND SD.BillID = '" + BillID + "'   AND SD.BillDate = '" + BillDate + "'     AND SD.CustomerNumber = '" + CustomerNumber + "'      AND SD.BranchID = '" + model.BranchID + "'    AND SP.BranchID = '" + model.BranchID + "'    AND SB.BranchID = '" + model.BranchID + "'";



                var Table = BusinessClassCommon.DataTable(_billingsoftware, Query);



                // Get current date and time
                var currentDateTime = Busbill.GetFormattedDateTime();

                // Create filename with BillID and current datetime
                var fileName = $"{model.BillID}_{currentDateTime}.docx";

                return File(Busbill.PrintBillDetails(Table, model.BranchID), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);




            }

            if (buttonType == "Payment")
            {

                var checkbillexist = await _billingsoftware.SHbillmaster.FirstOrDefaultAsync(x => x.BillID == model.BillID && x.IsDelete == false);

                if (checkbillexist == null)
                {
                    ViewBag.Getnotfound = "Bill Not Saved ";

                    return View("CustomerBilling", model);
                }
                else
                {

                    return RedirectToAction("PaymentBilling", new { BillID = model.BillID, BranchID = model.BranchID });
                }
            }



            if (buttonType == "Add Product")
            {

                if (TempData["BranchID"] != null)
                {
                    detailModel.BranchID = TempData["BranchID"].ToString();
                    TempData.Keep("BranchID");
                }

                //Validation Message for Product or Barcode is Empty while Add Product
                if ((string.IsNullOrWhiteSpace(model.ProductID) || model.ProductID == "ProductID") &&
                      string.IsNullOrWhiteSpace(model.BarCode) || model.ProductID == "Barcode")
                {
                    ViewBag.Getnotfound = "Please enter Product ID or Barcode";
                    return View("CustomerBilling", model);
                }

                // check barcode stock in godown
                var barcodeInGodown = await (from p in _billingsoftware.SHProductMaster
                                             join g in _billingsoftware.SHGodown on p.ProductID equals g.ProductID
                                             where p.BarcodeId == model.BarCode && g.BranchID == model.BranchID && g.IsDelete == false
                                             select g).FirstOrDefaultAsync();

                if (model.BarCode != null && barcodeInGodown == null)
                {
                    ViewBag.Getnotfound = "This Barcode does not exist in Godown.";
                    return View("CustomerBilling", model);
                }






                //Check  ProductID is already is available for Particular BillID

                var existingProductInBillDetails = await _billingsoftware.SHbilldetails
                     .FirstOrDefaultAsync(x => x.ProductID == model.ProductID
                                  && x.BillID == model.BillID
                                  && x.BillDate == model.BillDate
                                  && x.CustomerNumber == model.CustomerNumber
                                  && x.BranchID == model.BranchID
                                  && x.IsDelete == false);

                if (existingProductInBillDetails != null)
                {
                    // ProductID already exists in billdetails
                    ViewBag.Getnotfound = "You cannot Add the same product";
                    return View("CustomerBilling", model);
                }


                // Validation to check whether the given ProductID or Barcode is Available in Product Master
                var productlist = await _billingsoftware.SHProductMaster
                             .Where(p => (p.ProductID == model.ProductID || p.BarcodeId == model.BarCode) && p.IsDelete == false && p.BranchID == model.BranchID)
                             .Select(p => new BillingDetailsModel
                             {
                                 ProductID = p.ProductID,
                                 ProductName = p.ProductName,
                                 Price = p.Price,
                                 Quantity = Quantity,
                                 NetPrice = model.NetPrice

                             }).ToListAsync();


                if (productlist.Count == 0)
                {
                    ViewBag.Getnotfound = "Please enter Valid Product ID or Barcode";
                    return View("CustomerBilling", model);
                }



                //Validation to check the given quantity is greater than stock in godown
                var rackProducts = await (from p in _billingsoftware.SHProductMaster
                                          join g in _billingsoftware.SHGodown on p.ProductID equals g.ProductID
                                          where (p.ProductID == model.ProductID || p.BarcodeId == model.BarCode) && g.BranchID == model.BranchID && g.IsDelete == false
                                          select g).FirstOrDefaultAsync();


                if (rackProducts != null)
                {
                    if (int.TryParse(rackProducts.NumberofStocks, out int currentNoofitems))
                    {
                        int productQuantity = Convert.ToInt32(model.Quantity);


                        if (productQuantity > currentNoofitems)
                        {
                            ViewBag.Getnotfound = $"You have only {currentNoofitems} items in stock";
                            return View("CustomerBilling", model);
                        }

                        rackProducts.NumberofStocks = (currentNoofitems - productQuantity).ToString();

                        _billingsoftware.SaveChanges();
                    }
                }


                var existingbilldetail = await _billingsoftware.SHbilldetails
            .FirstOrDefaultAsync(x => x.BillID == model.BillID && x.BillDate == model.BillDate && x.CustomerNumber == model.CustomerNumber && x.BranchID == model.BranchID && x.ProductID == model.ProductID && x.IsDelete == false);

                if (existingbilldetail != null)
                {
                    existingbilldetail.BillID = detailModel.BillID;
                    existingbilldetail.BillDate = detailModel.BillDate;
                    existingbilldetail.CustomerNumber = detailModel.CustomerNumber;
                    existingbilldetail.ProductID = detailModel.ProductID;
                    existingbilldetail.Discount = detailModel.Discount;
                    existingbilldetail.Price = detailModel.Price;
                    existingbilldetail.Quantity = detailModel.Quantity;
                    existingbilldetail.NetPrice = detailModel.NetPrice;
                    existingbilldetail.Totalprice = detailModel.Totalprice;
                    existingbilldetail.TotalDiscount = detailModel.TotalDiscount;
                    existingbilldetail.ProductName = detailModel.ProductName;
                    existingbilldetail.BranchID = detailModel.BranchID;
                    existingbilldetail.Lastupdateddate = Busbill.GetFormattedDateTime();
                    existingbilldetail.Lastupdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    existingbilldetail.Lastupdateduser = User.Claims.First().Value.ToString();
                    _billingsoftware.Entry(existingbilldetail).State = EntityState.Modified;
                }
                else
                {
                    detailModel.Lastupdateduser = User.Claims.First().Value.ToString();
                    detailModel.Lastupdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    detailModel.Lastupdateddate = Busbill.GetFormattedDateTime();

                    //Validation to check the given quantity and Price is in Correct Format
                    var product = productlist.First();
                    if (product != null)
                    {
                        // Convert strings to numeric types
                        if (decimal.TryParse(product.Price, out decimal price) && int.TryParse(product.Quantity, out int quantity))
                        {
                            // Perform calculation
                            model.NetPrice = (price * quantity).ToString();
                        }
                        else
                        {
                            // Handle conversion failure
                            ViewBag.Getnotfound = "Invalid Quantity format ";
                            return View("CustomerBilling", model);
                        }

                        // Set detailModel properties
                        detailModel.ProductID = product.ProductID;
                        detailModel.ProductName = product.ProductName;
                        detailModel.Price = product.Price;
                        detailModel.Quantity = Quantity;
                        detailModel.NetPrice = model.NetPrice;
                        detailModel.Totalprice = model.NetPrice;
                        detailModel.BillID = BillID;
                        detailModel.BillDate = BillDate;
                        detailModel.CustomerNumber = CustomerNumber;
                    }

                    _billingsoftware.SHbilldetails.Add(detailModel);
                }

                await _billingsoftware.SaveChangesAsync();


                // Calculate total price for SHbillmaster
                var billDetails = await _billingsoftware.SHbilldetails
         .Where(x => x.BillID == detailModel.BillID && x.BillDate == model.BillDate && x.CustomerNumber == model.CustomerNumber && x.BranchID == model.BranchID && x.IsDelete == false)
         .Select(x => x.Totalprice)
         .ToListAsync();

                // Convert Totalprice strings to decimal and sum
                decimal totalPrice = billDetails
                    .Select(price => decimal.TryParse(price, out decimal value) ? value : 0)
                    .Sum();

                var existingbillmaster = await _billingsoftware.SHbillmaster
           .FirstOrDefaultAsync(x => x.BillID == detailModel.BillID && x.BillDate == model.BillDate && x.CustomerNumber == model.CustomerNumber && x.BranchID == model.BranchID && x.IsDelete == false);

                if (existingbillmaster != null)
                {
                    existingbillmaster.Totalprice = totalPrice.ToString();
                    existingbillmaster.NetPrice = totalPrice.ToString();
                    existingbillmaster.Lastupdateddate = Busbill.GetFormattedDateTime();
                    existingbillmaster.Lastupdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    existingbillmaster.Lastupdateduser = User.Claims.First().Value.ToString();
                    _billingsoftware.Entry(existingbillmaster).State = EntityState.Modified;
                }
                else
                {
                    // Add new bill master if it does not exist
                    var newBillMaster = new BillingMasterModel
                    {
                        BillID = model.BillID,
                        BillDate = model.BillDate,
                        CustomerNumber = model.CustomerNumber,
                        Totalprice = totalPrice.ToString(),
                        NetPrice = totalPrice.ToString(),
                        BranchID = model.BranchID,
                        Lastupdateddate = Busbill.GetFormattedDateTime(),
                        Lastupdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        Lastupdateduser = User.Claims.First().Value.ToString()
                    };

                    _billingsoftware.SHbillmaster.Add(newBillMaster);

                }

                await _billingsoftware.SaveChangesAsync();


                productlist = await _billingsoftware.SHbilldetails
          .Where(d => d.BillID == detailModel.BillID && d.BillDate == BillDate && d.CustomerNumber == CustomerNumber && d.BranchID == detailModel.BranchID)
          .Select(d => new BillingDetailsModel
          {
              ProductID = d.ProductID,
              ProductName = d.ProductName,
              Price = d.Price,
              Quantity = d.Quantity,
              NetPrice = d.NetPrice
          }).ToListAsync();


                model.Viewbillproductlist = productlist;

                ViewBag.TotalPrice = totalPrice;
                ViewBag.NetPrice = totalPrice;
                model.BillID = detailModel.BillID;

                return View("CustomerBilling", model);
            }



            if (buttonType == "Get")
            {
                var billID = model.BillID;
                var billDate = model.BillDate;
                var customerNumber = model.CustomerNumber;


                var exbillingDetails = _billingsoftware.SHbilldetails
        .Where(d => d.BillID == billID && d.BranchID == model.BranchID && d.IsDelete == false && d.BillDate == billDate && d.CustomerNumber == customerNumber)
        .ToList();

                // Check if there are any bill details
                if (exbillingDetails != null && exbillingDetails.Count > 0)
                {
                    // Retrieve corresponding master record if available
                    var updatedMasterex = _billingsoftware.SHbillmaster
                        .FirstOrDefault(m => m.BillID == billID && m.BillDate == billDate && m.CustomerNumber == customerNumber && m.IsDelete == false && m.BranchID == model.BranchID);

                    // Prepare the view model
                    var viewModel = new BillProductlistModel
                    {
                        MasterModel = updatedMasterex != null ? new BillingMasterModel
                        {
                            BillID = updatedMasterex.BillID,
                            BillDate = updatedMasterex.BillDate,
                            CustomerNumber = updatedMasterex.CustomerNumber,
                            Totalprice = updatedMasterex.Totalprice,
                            TotalDiscount = updatedMasterex.TotalDiscount,
                            NetPrice = updatedMasterex.NetPrice
                        } : null,
                        Viewbillproductlist = exbillingDetails,
                        BillID = billID,
                        BillDate = billDate,
                        CustomerNumber = customerNumber
                    };



                    ViewBag.TotalPrice = updatedMasterex?.Totalprice;
                    ViewBag.TotalDiscount = updatedMasterex?.TotalDiscount;
                    ViewBag.NetPrice = updatedMasterex?.NetPrice;
                    ViewBag.CGSTPercentage = updatedMasterex?.CGSTPercentage;
                    ViewBag.SGSTPercentage = updatedMasterex?.SGSTPercentage;


                    return View("CustomerBilling", viewModel);
                }

                else
                {
                    BillProductlistModel promodel = new BillProductlistModel();
                    ViewBag.Getnotfound = "No Data Found For This ID";

                    return View("CustomerBilling", promodel);
                }
            }


            if (buttonType == "Delete Bill")
            {

                var checkbillpay = _billingsoftware.SHPaymentMaster.FirstOrDefault(x => x.BillId == model.BillID && x.BillDate == model.BillDate && x.BranchID == model.BranchID);


                if (checkbillpay != null)
                {
                    ViewBag.DelMessage = "There is a payment linked to this bill. Please remove the payment before attempting to delete the bill.";
                    return View("CustomerBilling", model);

                }

                var billMaster = _billingsoftware.SHbillmaster.FirstOrDefault(b => b.BillID == model.BillID && !b.IsDelete && b.BillDate == model.BillDate && model.BranchID == model.BranchID);
                if (billMaster != null)
                {


                    _billingsoftware.SHbillmaster.Remove(billMaster);

                    _billingsoftware.SaveChanges();

                    var billDetails = _billingsoftware.SHbilldetails.Where(b => b.BillID == model.BillID && !b.IsDelete && b.BillDate == model.BillDate && model.BranchID == model.BranchID).ToList();

                    foreach (var detail in billDetails)
                    {


                        _billingsoftware.SHbilldetails.Remove(detail);

                        int productQuantity = Convert.ToInt32(detail.Quantity);
                        detail.Quantity = "0";



                        var rackProduct = _billingsoftware.SHGodown
                            .FirstOrDefault(r => r.ProductID == detail.ProductID && r.BranchID == detail.BranchID);

                        if (rackProduct != null)
                        {
                            int currentNoofitems = Convert.ToInt32(rackProduct.NumberofStocks);
                            rackProduct.NumberofStocks = (currentNoofitems + productQuantity).ToString();
                        }
                    }


                    _billingsoftware.SaveChanges();
                    ViewBag.DelMessage = "Deleted Bill Successfully";
                }
                else
                {
                    ViewBag.DelMessage = "Bill Not Found ";
                }


                return View("CustomerBilling", model);

            }


            if (buttonType == "Save")
            {

                if (TempData["BranchID"] != null)
                {
                    masterModel.BranchID = TempData["BranchID"].ToString();
                    TempData.Keep("BranchID");
                }


                BusinessClassBilling busbill = new BusinessClassBilling(_billingsoftware, _configuration);

                // This query is used to check Bill has Product in BillDetails
                var checkproduct = await _billingsoftware.SHbilldetails.FirstOrDefaultAsync(x => x.BillID == masterModel.BillID && x.BillDate == masterModel.BillDate && x.CustomerNumber == masterModel.CustomerNumber && x.BranchID == masterModel.BranchID);

                if (checkproduct == null)
                {
                    ViewBag.SaveMessage = "Please Add a Product";
                    return View("CustomerBilling", model);
                }


                // Retrieve the existing master record
                var updateMaster = await _billingsoftware.SHbillmaster
                    .FirstOrDefaultAsync(m => m.BillID == model.BillID && m.BranchID == model.BranchID && m.BillDate == model.BillDate && m.CustomerNumber == model.CustomerNumber);



                if (updateMaster != null)
                {
                    if (updateMaster.IsDelete)
                    {
                        ViewBag.Message = "Cannot update. Product is marked as deleted.";
                        return View("CustomerBilling", model);
                    }



                    updateMaster.BillInsertion = false;
                    updateMaster.BillID = masterModel.BillID;
                    updateMaster.BillDate = masterModel.BillDate;
                    updateMaster.CustomerNumber = masterModel.CustomerNumber;
                    updateMaster.Totalprice = masterModel.Totalprice;
                    updateMaster.TotalDiscount = masterModel.TotalDiscount;
                    updateMaster.NetPrice = masterModel.NetPrice ?? masterModel.Totalprice;
                    updateMaster.CGSTPercentage = masterModel.CGSTPercentage;
                    updateMaster.CGSTPercentageAmt = masterModel.CGSTPercentageAmt;
                    updateMaster.SGSTPercentage = masterModel.SGSTPercentage;
                    updateMaster.SGSTPercentageAmt = masterModel.SGSTPercentageAmt;
                    updateMaster.BranchID = masterModel.BranchID;
                    updateMaster.Lastupdateduser = User.Claims.First().Value.ToString();
                    updateMaster.Lastupdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    updateMaster.Lastupdateddate = busbill.GetFormattedDateTime();


                    _billingsoftware.Entry(updateMaster).State = EntityState.Modified;

                }
                else
                {
                    masterModel.NetPrice = masterModel.NetPrice ?? masterModel.Totalprice;
                    masterModel.Billby = User.Claims.First().Value.ToString();
                    masterModel.Lastupdateduser = User.Claims.First().Value.ToString();
                    masterModel.Lastupdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    masterModel.Lastupdateddate = busbill.GetFormattedDateTime();
                    masterModel.BillInsertion = true;

                    _billingsoftware.SHbillmaster.Add(masterModel);

                }


                await _billingsoftware.SaveChangesAsync();




                // Save points calculation
                var checkpoints = await _billingsoftware.SHBillingPoints.FirstOrDefaultAsync(x => x.BillID == model.BillID && x.CustomerNumber == CustomerNumber && x.BranchID == model.BranchID);
                var pointsMaster = await _billingsoftware.SHPointsMaster.FirstOrDefaultAsync(x => x.BranchID == model.BranchID);

                if (pointsMaster != null && pointsMaster.NetPrice != null && pointsMaster.NetPoints != null && pointsMaster.BranchID == model.BranchID)
                {
                    decimal netPointsRatio = Convert.ToDecimal(pointsMaster.NetPoints) / Convert.ToDecimal(pointsMaster.NetPrice);
                    decimal points = Convert.ToDecimal(masterModel.NetPrice ?? masterModel.Totalprice) * netPointsRatio;

                    if (checkpoints != null)
                    {
                        // Update existing points record
                        checkpoints.NetPrice = masterModel.NetPrice;
                        checkpoints.Points = points.ToString("F2");
                        checkpoints.IsUsed = false;
                        checkpoints.DateofReedem = null;
                        checkpoints.BranchID = model.BranchID;

                        _billingsoftware.Entry(checkpoints).State = EntityState.Modified;
                    }
                    else
                    {
                        // Add new points record
                        var billingPoints = new BillingPointsModel
                        {
                            BillID = BillID,
                            CustomerNumber = CustomerNumber,
                            NetPrice = masterModel.NetPrice ?? masterModel.Totalprice,
                            Points = points.ToString("F2"),
                            IsUsed = false,
                            DateofReedem = null,
                            BranchID = model.BranchID
                        };

                        _billingsoftware.SHBillingPoints.Add(billingPoints);
                    }

                    await _billingsoftware.SaveChangesAsync();
                }



                var updatedMaster = await _billingsoftware.SHbillmaster
           .Where(m => m.BillID == masterModel.BillID && m.BranchID == model.BranchID && m.BillDate == masterModel.BillDate && m.CustomerNumber == masterModel.CustomerNumber)
           .FirstOrDefaultAsync();

                if (updatedMaster != null)
                {
                    ViewBag.TotalPrice = updatedMaster.Totalprice;
                    ViewBag.TotalDiscount = updatedMaster.TotalDiscount;
                    ViewBag.NetPrice = updatedMaster.NetPrice;
                    ViewBag.CGSTPercentage = updatedMaster.CGSTPercentage;
                    ViewBag.SGSTPercentage = updatedMaster.SGSTPercentage;

                }


                var billingDetails = await _billingsoftware.SHbilldetails
           .Where(d => d.BillID == masterModel.BillID && d.BranchID == model.BranchID && d.BillDate == masterModel.BillDate && d.CustomerNumber == masterModel.CustomerNumber)
           .ToListAsync();

                /* model.MasterModel = updatedMaster*/
                ;
                model.Viewbillproductlist = billingDetails;


                ViewBag.SaveMessage = "Saved Successfully";


            }

            if (buttonType == "Clear")
            {

                var clr = new BillProductlistModel();
                ViewBag.ClearMessage = "Fields have been cleared.";

                return View("CustomerBilling", clr);
            }



            if (buttonType == "Reedem Points")
            {

                BusinessClassBilling business = new BusinessClassBilling(_billingsoftware, _configuration);

                var billingPoints = await _billingsoftware.SHBillingPoints
           .Where(bp => bp.CustomerNumber == CustomerNumber && !bp.IsUsed && bp.BillID != BillID && bp.BranchID == model.BranchID)
           .ToListAsync();

                var totalPoints = billingPoints.Sum(bp => decimal.TryParse(bp.Points, out decimal pts) ? pts : 0);

                var updatedMaster = await _billingsoftware.SHbillmaster
                    .FirstOrDefaultAsync(m => m.BillID == BillID && m.BranchID == model.BranchID && m.BillDate == BillDate && m.CustomerNumber == CustomerNumber && m.IsDelete == false);

                if (updatedMaster != null)
                {
                    decimal netPrice = decimal.TryParse(updatedMaster.NetPrice, out decimal price) ? price : 0;

                    var Total = netPrice - totalPoints;

                    updatedMaster.NetPrice = Total.ToString("F2");
                    _billingsoftware.Entry(updatedMaster).State = EntityState.Modified;
                    await _billingsoftware.SaveChangesAsync();


                    ViewBag.TotalPrice = updatedMaster.Totalprice;
                    ViewBag.TotalDiscount = updatedMaster.TotalDiscount;
                    ViewBag.NetPrice = updatedMaster.NetPrice;
                    ViewBag.CGSTPercentage = updatedMaster.CGSTPercentage;
                    ViewBag.SGSTPercentage = updatedMaster.SGSTPercentage;


                }
                else
                {

                    ViewBag.SaveMessage = "Cannot Reedem Points Please Save a Product";

                    return View("CustomerBilling", model);

                }

                foreach (var point in billingPoints)
                {
                    point.IsUsed = true;
                    point.DateofReedem = business.GetFormattedDateTime();
                    _billingsoftware.Entry(point).State = EntityState.Modified;

                    ViewBag.SaveMessage = "Points Reedem Successfully";

                }

                await _billingsoftware.SaveChangesAsync();

                var billingDetailsre = await _billingsoftware.SHbilldetails
          .Where(d => d.BillID == masterModel.BillID && d.BranchID == model.BranchID && d.BillDate == masterModel.BillDate && d.CustomerNumber == masterModel.CustomerNumber)
          .ToListAsync();


                model.Viewbillproductlist = billingDetailsre;

                return View("CustomerBilling", model);
            }



            return View("CustomerBilling", model);
        }

        public void PrintDocument(byte[] fileContent)
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), "tempfile.docx");
            System.IO.File.WriteAllBytes(tempFilePath, fileContent);

            // Create a new process to print the file
            var process = new System.Diagnostics.Process();
            process.StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                CreateNoWindow = true,
                Verb = "print",
                FileName = tempFilePath, // Path to the file you want to print
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
            };

            process.Start();
            process.WaitForExit();
        }

        //
        public IActionResult DeleteProduct(string productId, string billID, string billDate, string customerNumber, BillProductlistModel model)
        {

            BusinessClassBilling Busbill = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["productid"] = Busbill.Getproduct(model.BranchID);


            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            var product = _billingsoftware.SHbilldetails
                          .Where(p => p.ProductID == productId && p.BranchID == model.BranchID && p.BillID == billID && p.BillDate == billDate && p.CustomerNumber == customerNumber)
                          .Select(p => new
                          {
                              p.Quantity,
                              p.BranchID,
                              p.IsDelete
                          })
                          .FirstOrDefault();

            if (product != null)
            {
                if (product.IsDelete)
                {
                    ViewBag.DelMessage = "BillID Already Deleted";
                    return View("CustomerBilling", model);
                }

                // Delete the Product from the Product Details
                var productToUpdate = _billingsoftware.SHbilldetails
                    .First(p => p.ProductID == productId && p.BranchID == model.BranchID && p.BillID == billID && p.BillDate == billDate && p.CustomerNumber == customerNumber);

                _billingsoftware.SHbilldetails.Remove(productToUpdate);
                _billingsoftware.SaveChanges();

                // Update Godown to add back the quantity
                var rackProduct = _billingsoftware.SHGodown
                    .FirstOrDefault(r => r.ProductID == productId && r.BranchID == product.BranchID && r.IsDelete == false);

                if (rackProduct != null)
                {
                    int currentNoofitems = Convert.ToInt32(rackProduct.NumberofStocks);
                    int productQuantity = Convert.ToInt32(product.Quantity);
                    rackProduct.NumberofStocks = (currentNoofitems + productQuantity).ToString();
                    _billingsoftware.Entry(rackProduct).State = EntityState.Modified;
                }

                _billingsoftware.SaveChanges();
                ViewBag.DelMessage = "Deleted Product Successfully";
            }

            //var deleteitem = model.Viewbillproductlist.Where(x => x.ProductID == productId && x.BillID == billID && x.BillDate == billDate && x.BranchID == model.BranchID).FirstOrDefault();
            //model.Viewbillproductlist.Remove(deleteitem);


            var billDetail = _billingsoftware.SHbilldetails
                    .Where(b => b.BillID == billID && b.BranchID == model.BranchID && b.BillDate == billDate && b.CustomerNumber == customerNumber)
                    .Select(b => new BillingDetailsModel
                    {
                        ProductID = b.ProductID,
                        ProductName = b.ProductName,
                        Price = b.Price, // Assuming you want to use the price from the database
                        Quantity = b.Quantity,
                        BillDate = b.BillDate,
                        CustomerNumber = b.CustomerNumber,
                        BillID = b.BillID,
                        NetPrice = b.NetPrice
                    })
                    .ToList();

            if (billDetail != null)
            {
                model.Viewbillproductlist = billDetail;
            }
            else
            {
                // Handle case where no detail is found
                model.Viewbillproductlist = new List<BillingDetailsModel>();
            }



            var totalPriceList = _billingsoftware.SHbilldetails
                        .Where(x => x.BillID == billID
                                    && x.BillDate == billDate
                                    && x.CustomerNumber == customerNumber
                                    && x.BranchID == model.BranchID)
                        .Select(x => x.Totalprice)
                        .ToList();

            // Convert the list of strings to decimals and calculate the sum
            decimal totalPriceSum = totalPriceList
                .Where(price => decimal.TryParse(price, out _))  // Filter out invalid strings
                .Sum(price => decimal.Parse(price));             // Sum valid prices


            if (totalPriceSum > 0)
            {

                ViewBag.TotalPrice = totalPriceSum;
            }

            return View("CustomerBilling", model);


        }





        public IActionResult ProductList(string BillID, string BillDate, string CustomerNumber, string PaymentId)
        {
            if (string.IsNullOrEmpty(BillID) && TempData["BillID"] != null)
            {
                BillID = TempData["BillID"].ToString();
                BillDate = TempData["BillDate"].ToString();
                CustomerNumber = TempData["CustomerNumber"].ToString();

            }

            var model = new ProductSelectModel
            {
                Viewproductlist = new List<ProductMatserModel>() // Initialize with an empty list
            };

            ViewBag.BillID = BillID;
            ViewBag.BillDate = BillDate;
            ViewBag.CustomerNumber = CustomerNumber;


            return View(model);


        }






        public IActionResult RollTypeMaster()
        {
            RollTypeMaster rolltype = new RollTypeMaster();

            return View("RollTypeMaster", rolltype);
        }

        public IActionResult BranchMaster()
        {

            BranchMasterModel par = new BranchMasterModel();
            return View("BranchMaster", par);
        }





        public IActionResult ResourceTypeMaster()
        {
            ResourceTypeMasterModel res = new ResourceTypeMasterModel();

            return View("ResourceTypeMaster", res);
        }

        public IActionResult RollAccessMaster()
        {
            var model = new RollAccessMaster();

            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            BusinessClassBilling Busbill = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["rollid"] = Busbill.RollAccessType(model.BranchID);
            ViewData["staffid"] = Busbill.GetStaffID(model.BranchID);

            return View(model);
        }

        public IActionResult RoleAccess()
        {
            var model = new RoleAccessModel();

            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            BusinessClassBilling businessbill = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["screenname"] = businessbill.Screenname();
            ViewData["rollid"] = businessbill.RollAccessType(model.BranchID);
            ViewData["staffid"] = businessbill.GetStaffID(model.BranchID);



            return View(model);
        }

        public IActionResult ScreenMaster()
        {
            BusinessClassBilling businessbill = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["screenname"] = businessbill.Screenname();


            ScreenMasterModel screen = new ScreenMasterModel();
            return View("ScreenMaster", screen);
        }




        public IActionResult CustomerMaster()
        {
            CustomerMasterModel obj = new CustomerMasterModel();

            return View("CustomerMaster", obj);
        }

        public IActionResult DiscountCategoryMaster()
        {
            var model = new DiscountCategoryMasterModel();

            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            BusinessClassBilling business = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["discountcategoryid"] = business.GetcategoryID(model.BranchID);

            return View(model);
        }

        public IActionResult GSTMasterModel()
        {
            return View();
        }

        public IActionResult VoucherCustomerDetails()
        {
            return View();
        }

        public IActionResult NetDiscountMaster()
        {
            NetDiscountMasterModel par = new NetDiscountMasterModel();
            return View("NetDiscountMaster", par);
        }



        public IActionResult PointsReedemDetails()
        {

            return View();
        }

        public IActionResult RackMaster()
        {
            return View();
        }

        public IActionResult RackPatrionProduct()
        {
            var model = new RackpartitionViewModel();

            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            BusinessClassBilling business = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["godownproductid"] = business.GetProductid(model.BranchID);

            model.Viewrackpartition = new List<RackPatrionProductModel>();

            return View(model);
        }


        public IActionResult VoucherMaster()
        {
            return View();
        }

        //This method is used to load cutomer Billing Screen
        public IActionResult CustomerBilling(string productid, string billid, string SelectedProductID)
        {
            var model = new BillProductlistModel();

            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            BusinessClassBilling Busbill = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["productid"] = Busbill.Getproduct(model.BranchID);

            // Retrieve selected product
            var selectedProduct = _billingsoftware.SHProductMaster.FirstOrDefault(p => p.ProductID == productid);

            if (selectedProduct != null)
            {
                // Retrieve bill detail for the specified BillID and ProductID
                var billDetail = _billingsoftware.SHbilldetails
                    .Where(b => b.BillID == billid && b.ProductID == productid && b.BranchID == model.BranchID)
                    .Select(b => new BillingDetailsModel
                    {
                        ProductID = b.ProductID,
                        ProductName = b.ProductName,
                        Price = selectedProduct.TotalAmount,
                        Quantity = b.Quantity,
                        BillDate = b.BillDate,
                        CustomerNumber = b.CustomerNumber,
                        BillID = b.BillID
                    })
                    .FirstOrDefault();

                if (billDetail != null)
                {
                    model.Viewbillproductlist = new List<BillingDetailsModel> { billDetail };
                    model.ProductID = billDetail.ProductID;
                    model.ProductName = billDetail.ProductName;
                    model.Price = billDetail.Price;
                    model.Quantity = billDetail.Quantity;
                    model.BillID = billDetail.BillID;
                    model.BillDate = billDetail.BillDate;
                    model.CustomerNumber = billDetail.CustomerNumber;
                }
                else
                {
                    // Handle case where no detail is found
                    model.Viewbillproductlist = new List<BillingDetailsModel>();
                }
            }
            else
            {
                // Handle case where selected product is not found
                model.Viewbillproductlist = new List<BillingDetailsModel>();
            }

            return View(model);
        }

        public IActionResult PaymentBilling(string BillID, string BranchID)
        {
            PaymentTableViewModel obj = new PaymentTableViewModel();

            // Fetch the bill details from the BillMaster table
            var billDetails = _billingsoftware.SHbillmaster
                                             .Where(b => b.BillID == BillID && b.BranchID == BranchID)
                                             .Select(b => new
                                             {
                                                 b.BillID,
                                                 b.BillDate,
                                                 b.NetPrice
                                             })
                                             .FirstOrDefault();

            // Fetch the payment details from the PaymentMaster and PaymentDetails tables based on the BillID
            var paymentDetails = (from pm in _billingsoftware.SHPaymentMaster
                                  join pd in _billingsoftware.SHPaymentDetails
                                  on pm.PaymentId equals pd.PaymentId
                                  where pm.BillId == BillID && pm.BranchID == BranchID && pd.BranchID == BranchID
                                  select new
                                  {
                                      pd.PaymentId,
                                      pd.PaymentDiscription,
                                      pd.PaymentDate,
                                      pd.PaymentMode,
                                      pd.PaymentTransactionNumber,
                                      pd.PaymentAmount,
                                      pm.Balance // Fetch the balance from the PaymentMaster table
                                  }).ToList();

            // Check if bill details were found
            if (billDetails != null)
            {
                obj.BillId = billDetails.BillID;
                obj.BillDate = billDetails.BillDate;
                obj.StrBillvalue = billDetails.NetPrice;
            }
            else
            {
                ViewBag.Message = "Bill details not found.";
                return View(obj); // Early return if no bill found
            }

            // Check if payment details exist and assign balance accordingly
            if (paymentDetails != null && paymentDetails.Any())
            {
                // If payments exist, use the balance from the PaymentMaster
                ViewBag.Balance = paymentDetails.First().Balance;

                obj.Viewpayment = paymentDetails.Select(pd => new PaymentDetailsModel
                {
                    PaymentDate = pd.PaymentDate,
                    PaymentDiscription = pd.PaymentDiscription,
                    PaymentMode = pd.PaymentMode,
                    PaymentTransactionNumber = pd.PaymentTransactionNumber,
                    PaymentAmount = pd.PaymentAmount
                }).ToList();
            }
            else
            {
                // If no payment exists, set the balance to the bill's NetPrice
                ViewBag.Balance = obj.StrBillvalue;
            }

            // Store values in TempData to pass between requests
            TempData["BillID"] = obj.BillId;
            TempData["BillDate"] = DateTime.Parse(obj.BillDate).ToString("yyyy-MM-dd");
            TempData["BillValue"] = obj.StrBillvalue;
            TempData["Balance"] = ViewBag.Balance;
            TempData["BranchID"] = BranchID;

            return View(obj);


        }

        public IActionResult UpdatePaymentDetails(string billID, string branchID, string billdate, string billValue)
        {
            string formattedBillDate = billdate;

            // Try parsing the billDate
            DateTime parsedBillDate;
            if (DateTime.TryParseExact(billdate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedBillDate))
            {
                // If parsing succeeds, format the date correctly
                formattedBillDate = parsedBillDate.ToString("yyyy-MM-dd");
            }
            else
            {
                // If parsing fails, convert the billDate to DateTime and then format it
                DateTime tempDate;
                if (DateTime.TryParse(billdate, out tempDate))
                {
                    formattedBillDate = tempDate.ToString("yyyy-MM-dd");
                }
            }

            // Fetch the current balance from PaymentMaster
            var paymentDetails = _billingsoftware.SHPaymentMaster
                                                  .Where(p => p.BillId == billID && p.BranchID == branchID && p.BillDate == formattedBillDate)
                                                  .Select(p => new
                                                  {
                                                      p.Balance
                                                  })
                                                  .FirstOrDefault();


            if (paymentDetails != null)
            {
                @ViewBag.Balance = paymentDetails.Balance;
            }
            else
            {
                @ViewBag.Balance = billValue;  // Default to 0 if Balance is null
            }


            return View();
        }









        [HttpPost]
        public async Task<IActionResult> PaymentAction(PaymentTableViewModel model, string buttonType, string selectedSlotId, PaymentDetailsModel detailmodel, string billId, string branchID, string billDate, string billValue)
        {

            BusinessClassBilling business = new BusinessClassBilling(_billingsoftware, _configuration);


            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }


            string formattedBillDate = billDate;

            // Try parsing the billDate
            DateTime parsedBillDate;
            if (DateTime.TryParseExact(billDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedBillDate))
            {
                // If parsing succeeds, format the date correctly
                formattedBillDate = parsedBillDate.ToString("yyyy-MM-dd");
            }
            else
            {
                // If parsing fails, convert the billDate to DateTime and then format it
                DateTime tempDate;
                if (DateTime.TryParse(billDate, out tempDate))
                {
                    formattedBillDate = tempDate.ToString("yyyy-MM-dd");
                }

            }


            //   model.StrBillvalue = BusinessClassCommon.getbalance(_billingsoftware, model.PaymentId, model.BillId,model.BranchID, model.BillDate,detailmodel.PaymentAmount);

            var paymentid = "pay_" + billId;


            if (billId == null && formattedBillDate == null)
            {

                ViewBag.Message = "BillID Not Found";
                var resultdel = UpdatePaymentDetails(billId, branchID, formattedBillDate, billValue);

                return View("PaymentBilling", model);
            }


            if (buttonType == "DeletePayment")
            {
                //Delete Details from DB
                //Delete from database

                var Dbdelete = _billingsoftware.SHPaymentMaster.SingleOrDefault(x => x.BillId == billId && x.BranchID == branchID && x.BillDate == formattedBillDate);

                if (Dbdelete != null)
                {

                    var selectedDBpayment = _billingsoftware.SHPaymentDetails.Where(x => x.PaymentId == paymentid && x.BranchID == branchID).ToList();

                    if (selectedDBpayment.Count == 0)
                    {
                        ViewBag.Message = "Please enter Payment ID";
                        var resultdelm = UpdatePaymentDetails(billId, branchID, formattedBillDate, billValue);
                        return View("PaymentBilling", model);
                    }

                    foreach (var item in selectedDBpayment)
                    {
                        _billingsoftware.SHPaymentDetails.Remove(item);
                        _billingsoftware.SaveChanges();
                    }


                    //Delete Master
                    var SelectedPayMas = _billingsoftware.SHPaymentMaster.SingleOrDefault(x => x.BillId == billId && x.BillDate == formattedBillDate && x.PaymentId == paymentid && x.BranchID == branchID);

                    _billingsoftware.SHPaymentMaster.Remove(SelectedPayMas);
                    _billingsoftware.SaveChanges();
                    ViewBag.Message = "Payment Deleted Successfully";
                }
                else
                {
                    ViewBag.Message = "Payment Not Found";
                }
                var resultdel = UpdatePaymentDetails(billId, branchID, formattedBillDate, billValue);


                //Code here for refresh model
                PaymentTableViewModel objnew = new PaymentTableViewModel();

                model = objnew;


                return View("PaymentBilling", model);
            }


            if (buttonType == "GetPayment")
            {
                var selectDBpayment = _billingsoftware.SHPaymentDetails.Where(x => x.PaymentId == model.PaymentId && x.BranchID == model.BranchID).ToList();


                var SelectPayMas = _billingsoftware.SHPaymentMaster.SingleOrDefault(x => x.BillId == model.BillId && x.PaymentId == model.PaymentId && x.BranchID == model.BranchID && x.BillDate == model.BillDate);

                if (SelectPayMas != null && selectDBpayment != null)
                {

                    if (model.Viewpayment == null)
                        model.Viewpayment = selectDBpayment;

                    model.BillDate = SelectPayMas.BillDate;
                    model.PaymentId = SelectPayMas.PaymentId;
                    model.BranchID = SelectPayMas.BranchID;
                    model.StrBillvalue = SelectPayMas.Balance;
                    model.BillId = SelectPayMas.BillId;

                    var exbilltotal = await _billingsoftware.SHbillmaster.Where(x => x.BillID == model.BillId && x.BillDate == model.BillDate && x.BranchID == model.BranchID).FirstOrDefaultAsync();
                    if (exbilltotal != null)
                        model.Balance = exbilltotal.NetPrice;


                }
                else
                {
                    ViewBag.Message = "Payment ID given is not available, Either it was belongs to different branch,enter correct Payment ID";
                }
            }
            if (buttonType == "DeletePaymentDetail")
            {

                if (string.IsNullOrEmpty(selectedSlotId))
                {
                    ViewBag.Message = "Please select a payment.";
                    var resultdel = UpdatePaymentDetails(billId, branchID, formattedBillDate, billValue);

                    return View("PaymentBilling", model);
                }


                //Delete from database
                var selectedDBpayment = _billingsoftware.SHPaymentDetails.SingleOrDefault(x => x.PaymentDiscription == selectedSlotId && x.PaymentId == paymentid && x.BranchID == branchID);
                if (selectedDBpayment != null)
                {
                    _billingsoftware.SHPaymentDetails.Remove(selectedDBpayment);
                    _billingsoftware.SaveChanges();
                }

                //Delete from grid
                var selectedpayment = model.Viewpayment.SingleOrDefault(x => x.PaymentDiscription == selectedSlotId);
                if (selectedpayment != null)
                {
                    model.Viewpayment.Remove(selectedpayment);
                    ViewBag.Message = "Payment Detail Deleted Successfully";
                }
                else
                {
                    ViewBag.Message = "PaymentID Not Found";
                }

                model.StrBillvalue = BusinessClassCommon.getbalance(_billingsoftware, paymentid, billId, branchID, formattedBillDate, detailmodel.PaymentAmount);

                var exbalance = _billingsoftware.SHPaymentMaster.Where(x => x.BillId == billId && x.BranchID == branchID && x.PaymentId == paymentid && x.BillDate == formattedBillDate).FirstOrDefault();


                if (exbalance != null)
                {
                    exbalance.Balance = model.StrBillvalue;
                    _billingsoftware.Entry(exbalance).State = EntityState.Modified;
                    _billingsoftware.SaveChanges();
                }

                var exbilltotal = await _billingsoftware.SHbillmaster.Where(x => x.BillID == billId && x.BillDate == formattedBillDate && x.BranchID == branchID).FirstOrDefaultAsync();
                if (exbilltotal != null)
                    model.Balance = exbilltotal.NetPrice;

                var resultdelpay = UpdatePaymentDetails(billId, branchID, formattedBillDate, billValue);
                return View("PaymentBilling", model);



            }



            if (buttonType == "Save")
            {

                // Check if no radio button is selected
                if (string.IsNullOrEmpty(selectedSlotId))
                {
                    ViewBag.Message = "Please select a payment.";
                    var resultdel = UpdatePaymentDetails(billId, branchID, formattedBillDate, billValue);

                    return View("PaymentBilling", model);
                }





                double totalpayamount = 0.0;
                foreach (var payment in model.Viewpayment)
                {
                    totalpayamount = totalpayamount + double.Parse(payment.PaymentAmount);
                }

                var billAmount = _billingsoftware.SHbillmaster
                 .Where(x => x.BillID == billId && x.BillDate == formattedBillDate && x.BranchID == branchID)
                 .Select(x => x.NetPrice)
                 .FirstOrDefault();

                // Check if total payment amount exceeds the bill amount
                if (totalpayamount > double.Parse(billAmount))
                {
                    ViewBag.Message = HttpUtility.JavaScriptStringEncode($"Payment amount '{totalpayamount}' exceeds the total bill amount '{billValue}'");
                    var resultdel = UpdatePaymentDetails(billId, branchID, formattedBillDate, billValue);

                    return View("PaymentBilling", model);
                }


                var objbillmaster = new PaymentMasterModel()
                {
                    BillDate = formattedBillDate,
                    PaymentId = paymentid,
                    BranchID = branchID,
                    Balance = billValue,
                    BillId = billId,
                    Lastupdateddate = business.GetFormattedDateTime(),
                    Lastupdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    Lastupdateduser = User.Claims.First().Value.ToString()

                };

                var objpaymas = _billingsoftware.SHPaymentMaster.Where(x => x.BillId == billId && x.BranchID == branchID && x.PaymentId == paymentid && x.BillDate == formattedBillDate).FirstOrDefault();

                if (objpaymas != null)
                {
                    objpaymas.BranchID = branchID;
                    objpaymas.Lastupdateddate = business.GetFormattedDateTime();
                    objpaymas.Lastupdateduser = User.Claims.First().Value.ToString();
                    objpaymas.Lastupdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    objpaymas.Balance = billValue;
                    objpaymas.BillDate = formattedBillDate;
                    objpaymas.BillId = billId;

                    _billingsoftware.Entry(objpaymas).State = EntityState.Modified;
                }
                else
                {
                    _billingsoftware.SHPaymentMaster.Add(objbillmaster);
                }

                _billingsoftware.SaveChanges();

                foreach (var objdetail in model.Viewpayment)
                {
                    var obpaydet = _billingsoftware.SHPaymentDetails.Where(x => x.BranchID == branchID && x.PaymentDiscription == objdetail.PaymentDiscription && x.PaymentId == paymentid).FirstOrDefault();

                    if (obpaydet != null)
                    {
                        obpaydet.BranchID = branchID;
                        obpaydet.Lastupdateduser = User.Claims.First().Value.ToString();
                        obpaydet.Lastupdateddate = business.GetFormattedDateTime();
                        obpaydet.Lastupdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                        obpaydet.PaymentAmount = objdetail.PaymentAmount;
                        obpaydet.PaymentDate = objdetail.PaymentDate;
                        obpaydet.PaymentDiscription = objdetail.PaymentDiscription;
                        obpaydet.PaymentMode = objdetail.PaymentMode;
                        obpaydet.PaymentTransactionNumber = objdetail.PaymentTransactionNumber;

                        _billingsoftware.Entry(obpaydet).State = EntityState.Modified;


                    }
                    else
                    {
                        objdetail.PaymentId = paymentid;
                        objdetail.BranchID = branchID;
                        objdetail.Lastupdateddate = business.GetFormattedDateTime();
                        objdetail.Lastupdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                        objdetail.Lastupdateduser = User.Claims.First().Value.ToString();
                        _billingsoftware.SHPaymentDetails.Add(objdetail);
                    }
                }

                _billingsoftware.SaveChanges();





                model.StrBillvalue = BusinessClassCommon.getbalance(_billingsoftware, paymentid, billId, branchID, formattedBillDate, totalpayamount.ToString());

                var exbalance = _billingsoftware.SHPaymentMaster.FirstOrDefault(x => x.BillId == billId && x.BranchID == branchID && x.PaymentId == paymentid && x.BillDate == formattedBillDate);


                if (exbalance != null)
                {
                    exbalance.Balance = model.StrBillvalue;
                    _billingsoftware.Entry(exbalance).State = EntityState.Modified;
                    _billingsoftware.SaveChanges();
                }

                var exbilltotal = await _billingsoftware.SHbillmaster.FirstOrDefaultAsync(x => x.BillID == billId && x.BillDate == formattedBillDate && x.BranchID == branchID);
                if (exbilltotal != null)
                    model.Balance = exbilltotal.NetPrice;


                ViewBag.Message = "Payment Saved Successfully";
                var resultsav = UpdatePaymentDetails(billId, branchID, formattedBillDate, billValue);


                return View("PaymentBilling", model);

            }
            if (buttonType == "AddPayment")
            {


                BusinessClassBilling obj = new BusinessClassBilling(_billingsoftware, _configuration);
                PaymentDetailsModel objNewPayment = new PaymentDetailsModel
                {
                    PaymentDiscription = obj.GeneratePaymentDescriptionreport(paymentid),
                    PaymentId = paymentid,
                    BranchID = branchID, // Use the branch ID passed in the method

                };

                if (model.Viewpayment == null)
                    model.Viewpayment = new List<PaymentDetailsModel>();

                model.Viewpayment.Add(objNewPayment);


                var exbill = await _billingsoftware.SHbillmaster.FirstOrDefaultAsync(x => x.BillID == billId && x.BillDate == formattedBillDate && x.BranchID == branchID && x.IsDelete == false);
                model.Balance = exbill.NetPrice;

                var exbalance = _billingsoftware.SHPaymentMaster.FirstOrDefault(x => x.BillId == billId && x.BranchID == branchID && x.PaymentId == paymentid && x.BillDate == formattedBillDate);

                if (exbalance != null)
                {
                    model.StrBillvalue = exbalance.Balance;
                }
                _billingsoftware.SaveChanges();

            }
            var result = UpdatePaymentDetails(billId, branchID, formattedBillDate, billValue);

            return View("PaymentBilling", model);
        }



        public async Task<IActionResult> GetBranchMaster(BranchMasterModel model, string buttontype)
        {
            BusinessClassBilling business = new BusinessClassBilling(_billingsoftware, _configuration);

            if (buttontype == "Get")
            {
                var getbranch = await _billingsoftware.SHBranchMaster.FirstOrDefaultAsync(x => x.BracnchID == model.BracnchID && x.IsDelete == false);
                if (getbranch != null)
                {
                    return View("BranchMaster", getbranch);
                }
                else
                {
                    BranchMasterModel par = new BranchMasterModel();
                    ViewBag.getMessage = "No Data found for this Branch ID";
                    return View("BranchMaster", par);
                }
            }
            else if (buttontype == "Delete")
            {
                var branchdel = await _billingsoftware.SHBranchMaster.FirstOrDefaultAsync(x => x.BracnchID == model.BracnchID && x.IsDelete == false);
                if (branchdel != null)
                {
                    if (branchdel.IsDelete)
                    {
                        ViewBag.ErrorMessage = "Cannot update. Product is marked as deleted.";
                        return View("BranchMaster", model);
                    }

                    branchdel.IsDelete = true;
                    await _billingsoftware.SaveChangesAsync();

                    ViewBag.delMessage = "Branch deleted successfully";
                    model = new BranchMasterModel();
                    return View("BranchMaster", model);
                }
                else
                {
                    ViewBag.delnoMessage = "Branch not found";
                    model = new BranchMasterModel();
                    return View("BranchMaster", model);
                }

            }

            else if (buttontype == "DeleteRetrieve")
            {
                var branchdelret = await _billingsoftware.SHBranchMaster.FirstOrDefaultAsync(x => x.BracnchID == model.BracnchID && x.IsDelete == true);
                if (branchdelret != null)
                {
                    branchdelret.IsDelete = false;

                    await _billingsoftware.SaveChangesAsync();

                    model.BracnchID = branchdelret.BracnchID;
                    model.BranchName = branchdelret.BranchName;
                    model.PhoneNumber1 = branchdelret.PhoneNumber1;
                    model.PhoneNumber2 = branchdelret.PhoneNumber2;
                    model.Address1 = branchdelret.Address1;
                    model.Address2 = branchdelret.Address2;
                    model.Country = branchdelret.Country;
                    model.City = branchdelret.City;
                    model.State = branchdelret.State;
                    model.ZipCode = branchdelret.ZipCode;
                    model.IsFranchise = branchdelret.IsFranchise;
                    model.email = branchdelret.email;


                    ViewBag.retMessage = "Deleted Branch retrieved successfully";
                }
                else
                {
                    ViewBag.noretMessage = "Branch not found";
                }
                return View("BranchMaster", model);
            }

            if (string.IsNullOrWhiteSpace(model.BranchName))
            {
                ViewBag.BMessage = "Please enter Branch Name.";
                return View("BranchMaster", model);
            }

            var existingBranch = await _billingsoftware.SHBranchMaster.FindAsync(model.BracnchID, model.BranchName);




            if (existingBranch != null)
            {
                if (existingBranch.IsDelete)
                {
                    ViewBag.ErrorMessage = "Cannot update. Product is marked as deleted.";
                    return View("BranchMaster", model);
                }

                existingBranch.BracnchID = model.BracnchID;
                existingBranch.BranchName = model.BranchName;
                existingBranch.PhoneNumber1 = model.PhoneNumber1;
                existingBranch.PhoneNumber2 = model.PhoneNumber2;
                existingBranch.Address1 = model.Address1;
                existingBranch.Address2 = model.Address2;
                existingBranch.Country = model.Country;
                existingBranch.City = model.City;
                existingBranch.State = model.State;
                existingBranch.ZipCode = model.ZipCode;
                existingBranch.IsFranchise = model.IsFranchise;
                existingBranch.email = model.email;
                existingBranch.LastUpdatedDate = business.GetFormattedDateTime();
                existingBranch.lastUpdatedUser = User.Claims.First().Value.ToString();
                existingBranch.lastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.Entry(existingBranch).State = EntityState.Modified;

            }
            else
            {

                model.LastUpdatedDate = business.GetFormattedDateTime();
                model.lastUpdatedUser = User.Claims.First().Value.ToString();
                model.lastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                _billingsoftware.SHBranchMaster.Add(model);
            }
            await _billingsoftware.SaveChangesAsync();

            ViewBag.Message = "Saved Successfully";

            model = new BranchMasterModel();
            return View("BranchMaster", model);


        }

        public IActionResult Error()
        {
            //Record error from context session
            WebErrorsModel webErrors = new WebErrorsModel();
            webErrors.ErrDateTime = DateTime.Now.ToString();
            webErrors.ErrodDesc = HttpContext.Session.GetString("ErrorMessage").ToString();
            webErrors.Username = User.Claims.First().Value.ToString();
            webErrors.ScreenName = HttpContext.Session.GetString("ScreenName").ToString();
            webErrors.MachineName = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            //Saving error into database
            _billingsoftware.SHWebErrors.Add(webErrors);
            _billingsoftware.SaveChangesAsync();

            return View(webErrors);
        }



        //ADD PRODUCT POPUP
        public async Task<IActionResult> AddProductPop(ProductMatserModel model, string buttonType, string productID, string NumberofStock, GodownModel gmodel)
        {


            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            BusinessClassBilling business = new BusinessClassBilling(_billingsoftware, _configuration);
            ViewData["categoryid"] = business.GetCatid(model.BranchID);
            ViewData["discountid"] = business.Getdiscountid(model.BranchID);
            ViewData["productid"] = business.Getproduct(model.BranchID);

            HttpContext.Session.SetString("BranchID", model.BranchID);

            var existingProduct = await _billingsoftware.SHProductMaster.FirstOrDefaultAsync(x => x.ProductID == model.ProductID && x.BranchID == model.BranchID);
            if (existingProduct != null)
            {
                if (existingProduct.IsDelete)
                {
                    ViewBag.ErrorMessage = "Cannot update. Product is marked as deleted.";
                    return View("PopupViewProduct", model);
                }


                existingProduct.ProductID = model.ProductID;
                existingProduct.CategoryID = model.CategoryID;
                existingProduct.ProductName = model.ProductName;
                existingProduct.BarcodeId = model.BarcodeId;
                existingProduct.Brandname = model.Brandname;
                existingProduct.Price = model.Price;
                existingProduct.ImeiNumber = model.ImeiNumber;
                existingProduct.SerialNumber = model.SerialNumber;
                existingProduct.DiscountCategory = model.DiscountCategory;
                existingProduct.SGST = model.SGST;
                existingProduct.CGST = model.CGST;
                existingProduct.OtherTax = model.OtherTax;
                existingProduct.Price = model.Price;
                existingProduct.DiscountCategory = model.DiscountCategory;
                existingProduct.TotalAmount = model.Price;
                existingProduct.BranchID = model.BranchID;

                // existingProduct.TotalAmount = model.TotalAmount - (model.Price * model.Discount / 100 = model.TotalAmount);
                existingProduct.LastUpdatedDate = DateTime.ParseExact(business.GetFormattedDateTime(), "dd/MM/yyyy HH:mm:ss", null);
                existingProduct.LastUpdatedUser = User.Claims.First().Value.ToString();
                existingProduct.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();


                _billingsoftware.Entry(existingProduct).State = EntityState.Modified;
            }
            else
            {

                // Convert strings to decimals, calculate TotalAmount, and convert back to string

                model.LastUpdatedDate = DateTime.ParseExact(business.GetFormattedDateTime(), "dd/MM/yyyy HH:mm:ss", null);
                model.LastUpdatedUser = User.Claims.First().Value.ToString();
                model.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                model.TotalAmount = model.Price;

                _billingsoftware.SHProductMaster.Add(model);
            }

            await _billingsoftware.SaveChangesAsync();

            decimal price;
            if (!decimal.TryParse(model.Price, out price))
            {
                ViewBag.PriceErrorMessage = "Please enter a valid price.";
                return View("PopupViewProduct", model);
            }


            var existinggodwnstock = _billingsoftware.SHGodown.FirstOrDefault(x => x.ProductID == model.ProductID && x.BranchID == model.BranchID);


            if (existinggodwnstock == null)
            {

                // Create a new instance of SHGodown
                existinggodwnstock = new GodownModel
                {
                    ProductID = model.ProductID,
                    BranchID = model.BranchID,
                    NumberofStocks = NumberofStock,
                    DatefofPurchase = business.GetFormattedDateTime(),
                    LastUpdatedDate = DateTime.ParseExact(business.GetFormattedDateTime(), "dd/MM/yyyy HH:mm:ss", null)
                };


                _billingsoftware.SHGodown.Add(existinggodwnstock);

            }
            else
            {

                if (int.TryParse(existinggodwnstock.NumberofStocks, out int currentStock) && int.TryParse(NumberofStock, out int stockToAdd))
                {
                    // Add the stocks and convert back to string
                    int updatedStock = currentStock + stockToAdd;
                    existinggodwnstock.NumberofStocks = updatedStock.ToString();
                    existinggodwnstock.DatefofPurchase = business.GetFormattedDateTime();
                    existinggodwnstock.LastUpdatedDate = DateTime.ParseExact(business.GetFormattedDateTime(), "dd/MM/yyyy HH:mm:ss", null);

                    _billingsoftware.Entry(existinggodwnstock).State = EntityState.Modified;
                }
            }


            _billingsoftware.SaveChanges();



            BillProductlistModel par = new BillProductlistModel();

            return View("CustomerBilling", par);

        }


        //Add Customer Pop
        public async Task<IActionResult> AddCustomerPop(CustomerMasterModel model)
        {
            BusinessClassBilling business = new BusinessClassBilling(_billingsoftware, _configuration);

            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }



            var existingCustomer = await _billingsoftware.SHCustomerMaster.FindAsync(model.MobileNumber, model.BranchID);
            if (existingCustomer != null)
            {
                if (existingCustomer.IsDelete)
                {
                    ViewBag.Message = "Cannot update. Customer Number is marked as deleted.";

                    return View("CustomerMaster", model);
                }

                existingCustomer.CustomerID = model.CustomerID;
                existingCustomer.CustomerName = model.CustomerName;
                existingCustomer.DateofBirth = model.DateofBirth;
                existingCustomer.Gender = model.Gender;
                existingCustomer.Address = model.Address;
                existingCustomer.City = model.City;
                existingCustomer.MobileNumber = model.MobileNumber;
                existingCustomer.IsDelete = model.IsDelete;
                existingCustomer.BranchID = model.BranchID;
                existingCustomer.LastUpdatedDate = business.GetFormattedDateTime();
                existingCustomer.LastUpdatedUser = User.Claims.First().Value.ToString();
                existingCustomer.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.Entry(existingCustomer).State = EntityState.Modified;

            }
            else
            {
                model.LastUpdatedDate = business.GetFormattedDateTime();
                model.LastUpdatedUser = User.Claims.First().Value.ToString();
                model.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _billingsoftware.SHCustomerMaster.Add(model);


            }

            await _billingsoftware.SaveChangesAsync();

            ViewBag.Message = "Saved Successfully";

            CustomerMasterModel cus = new CustomerMasterModel();

            return View("CustomerBilling", cus);

        }


        //Get Customer Data Pop

        [HttpPost]
        public async Task<IActionResult> getcustomerpop(BillProductlistModel model, string CustomerNumber)
        {
            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            var getdata = from bd in _billingsoftware.SHbilldetails
                          join bm in _billingsoftware.SHbillmaster on bd.BillID equals bm.BillID
                          where bd.CustomerNumber == CustomerNumber && bd.BranchID == model.BranchID && bm.BranchID == model.BranchID && bm.CustomerNumber == CustomerNumber
                          orderby bd.BillID descending
                          select new BillProductlistModel
                          {
                              BillID = bd.BillID,
                              BillDate = bd.BillDate,
                              ProductName = bd.ProductName,
                              ProductID = bd.ProductID
                          };

            var result = await getdata.ToListAsync();


            return Json(result);

        }



        //This Method is used to Load bill from Modal
        public IActionResult loadbill(string productID, string billID, string billDate, string customerNumber, BillProductlistModel model)
        {


            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            BusinessClassBilling Busbill = new BusinessClassBilling(_billingsoftware, _configuration);

            ViewData["productid"] = Busbill.Getproduct(model.BranchID);


            var updatedMasterex = _billingsoftware.SHbillmaster.FirstOrDefault(bm => bm.BillID == billID && bm.BillDate == billDate && bm.CustomerNumber == customerNumber && bm.BranchID == model.BranchID);


            // Query to get details of selected products
            var exbillingDetails = _billingsoftware.SHbilldetails
        .Where(d => d.BranchID == model.BranchID && d.IsDelete == false && d.CustomerNumber == customerNumber && d.BillID == billID && d.BillDate == billDate)
        .ToList();

            // Create ViewModel
            var viewModel = new BillProductlistModel
            {
                MasterModel = updatedMasterex != null ? new BillingMasterModel
                {
                    BillID = updatedMasterex.BillID,
                    BillDate = updatedMasterex.BillDate,
                    CustomerNumber = updatedMasterex.CustomerNumber,
                    Totalprice = updatedMasterex.Totalprice,
                    TotalDiscount = updatedMasterex.TotalDiscount,
                    NetPrice = updatedMasterex.NetPrice,
                    CGSTPercentage = updatedMasterex.CGSTPercentage,
                    SGSTPercentage = updatedMasterex.SGSTPercentage,

                } : null,
                Viewbillproductlist = exbillingDetails,
                BillID = billID,
                BillDate = billDate,
                CustomerNumber = customerNumber
            };

            var billingPoints = _billingsoftware.SHBillingPoints.Where(bp => bp.CustomerNumber == customerNumber
                 && !bp.IsUsed && bp.BillID != billID && bp.BranchID == model.BranchID
                 && _billingsoftware.SHbillmaster
                     .Any(bm => bm.CustomerNumber == bp.CustomerNumber
                                && bm.IsDelete == false && bm.BranchID == model.BranchID)).ToList();


            var totalPoints = billingPoints.Sum(bp => decimal.TryParse(bp.Points, out decimal pts) ? pts : 0);

            ViewBag.Points = totalPoints.ToString("F2");

            ViewBag.TotalPrice = updatedMasterex?.Totalprice;
            ViewBag.TotalDiscount = updatedMasterex?.TotalDiscount;
            ViewBag.NetPrice = updatedMasterex?.NetPrice;
            ViewBag.CGSTPercentage = updatedMasterex?.CGSTPercentage;
            ViewBag.SGSTPercentage = updatedMasterex?.SGSTPercentage;

            // Pass data to the view using ViewBag or ViewModel

            return View("CustomerBilling", viewModel);
        }



    }


}


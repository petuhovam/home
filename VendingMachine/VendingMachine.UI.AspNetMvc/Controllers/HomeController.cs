using System;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;

using VendingMachine.Domain.Erros;
using VendingMachine.Domain.Services.Common;
using VendingMachine.Domain.Services.Domain;
using VendingMachine.Domain.Models;

using VendingMachine.UI.AspNetMvc.Models;
using VendingMachine.UI.AspNetMvc.Services;

namespace VendingMachine.UI.AspNetMvc.Controllers
{
    public class HomeController : Controller
    {
        #region Services

        [Import]
        public ILogsService Logs
        {
            get;
            private set;
        }

        [Import]
        public IDomainModelContext Domain
        {
            get;
            private set;
        }

        #endregion

        #region Methods

        public ViewResult MainViewResult()
        {
            var viewResult = new ViewResult();
            viewResult.ViewName = "Index";

            var userProducts = new ProductModel(Domain.User.Products);
            userProducts.Refresh();

            var userAccount = new AccountModel(Domain.User.Account);
            userAccount.Refresh();

            var vmProducts = new ProductModel(Domain.VM.Products);
            vmProducts.Refresh();

            var vmBankAccount = new AccountModel(Domain.VM.BankAccount);
            vmBankAccount.Refresh();

            viewResult.ViewBag.UserProducts = userProducts;
            viewResult.ViewBag.UserAccount = userAccount;
            viewResult.ViewBag.VMProducts = vmProducts;
            viewResult.ViewBag.VMBankAccount = vmBankAccount;
            viewResult.ViewBag.VMUserAccount = Domain.VM.UserAccount;

            return viewResult;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Logs.Error(filterContext.Exception);

            if (filterContext.Exception is VMException)
            {
                var viewResult = MainViewResult();
                viewResult.ViewBag.ErrorText = filterContext.Exception.Message;

                filterContext.Result = viewResult;
                filterContext.ExceptionHandled = true;
            }
            else
            {
                base.OnException(filterContext);
            }
        }

        #endregion

        #region Actions

        public ActionResult Index(String message)
        {
            var viewResult = MainViewResult();
            viewResult.ViewBag.MessageText = message;
            return viewResult;
        }
        
        public ActionResult OnPutMoney(UInt16 item)
        {
            var res = Domain.User.Account.Get(item);
            if (res.Length > 0)
            {
                Domain.VM.UserAccount.Add(res);
            }
            return RedirectToAction("Index");
        }
        
        public ActionResult BuyProduct(String productName)
        {
            if (productName == null)
                throw new ArgumentNullException("productName");

            var product = Domain.VM.Buy(productName);
            if (product != null)
            {
                Domain.User.Products.Add(product);

                return RedirectToAction("Index", new { message = "Спасибо!" });
            }
            return RedirectToAction("Index");
        }
        
        public ActionResult GetRest()
        {
            var rest = Domain.VM.GetRest();
            if (rest.Length > 0)
            {
                Domain.User.Account.Add(rest);

                var sum = new Account().Add(rest).TotalSum;
                return RedirectToAction("Index", new { message = "Сдача " + sum });
            }
            else
            {
                var viewResult = MainViewResult();
                viewResult.ViewBag.ErrorText = "Внесенная сумма равна нулю";
                return viewResult;
            }
        }

        public ActionResult Error(String message)
        {
            ViewBag.ErrorText = message;

            return View();
        }

        #endregion
    }
}

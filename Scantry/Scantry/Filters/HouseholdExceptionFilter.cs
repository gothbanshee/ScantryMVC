using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using static Scantry.GCommon.Messages.HouseholdMessages;
using static Scantry.GCommon.Messages.GeneralMessages;

namespace Scantry.Filters
{
    public class HouseholdExceptionFilter(ITempDataDictionaryFactory tempDataFactory) : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            ITempDataDictionary tempData = tempDataFactory.GetTempData(context.HttpContext);

            if (context.Exception is UnauthorizedAccessException)
            {
                tempData["ErrorMessage"] = UnauthorizedUser;
                context.Result = new RedirectToActionResult("Index", "Household", null);
                context.ExceptionHandled = true;
            }
            else if (context.Exception is NullReferenceException)
            {
                tempData["ErrorMessage"] = GeneralError;
                context.Result = new RedirectToActionResult("Index", "Household", null);
                context.ExceptionHandled = true;
            }
        }
    }
}
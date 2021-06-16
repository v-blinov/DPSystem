using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace ApiDPSystem.Extension
{
    public static class Extension
    {
        public static List<string> GetErrorList(this ModelStateDictionary modelState)
        {
            var errorList = new List<string>();
            foreach (var error in modelState.Keys.ToList())
                errorList.AddRange(modelState[error].Errors.Select(p => error + " : " + p.ErrorMessage).ToList());

            return errorList;
        }
    }
}

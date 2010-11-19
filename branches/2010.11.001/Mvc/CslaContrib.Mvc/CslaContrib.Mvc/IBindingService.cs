using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Mvc;

namespace CslaContrib.Mvc
{
    public interface IBindingService
    {
        object[] GetArgumentValues(ModelBindingContext bindingContext, string[] arguments);

        void Map(ModelBindingContext bindingContext);

        void UpdateModelState(ModelBindingContext bindingContext);
    }
}
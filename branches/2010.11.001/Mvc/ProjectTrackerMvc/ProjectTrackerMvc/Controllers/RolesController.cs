using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CslaContrib.Mvc;
using ProjectTracker.Library.Admin;

namespace ProjectTrackerMvc.Controllers
{
    [HandleError]
    public class RolesController : Controller
    {
        //
        // GET: /Roles/

        public ActionResult Index([CslaBind(Method = "GetRoles")]Roles roles)
        {
            return View(roles);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string name)
        {
            var roles = Roles.GetRoles();
            var role = roles.AddNew();
            role.Name = name;
            var id = role.Id;
            if (!role.IsValid)
            {
                return JsonWithStatus(role, false, role.BrokenRulesCollection.ToArray());
            }

            roles = roles.Save();

            return JsonWithStatus(roles.GetRoleById(id));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var roles = Roles.GetRoles();
            var role = roles.GetRoleById(id);

            if (!TryUpdateModel(role) || !role.IsValid)
            {
                return JsonWithStatus(role, false, role.BrokenRulesCollection.ToArray());
            }

            roles = roles.Save();

            return JsonWithStatus(roles.GetRoleById(id));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id)
        {
            var roles = Roles.GetRoles();
            roles.Remove(id);
            roles = roles.Save();

            return JsonWithStatus(null);
        }

        private JsonResult JsonWithStatus(object data)
        {
            return JsonWithStatus(data, true, null);
        }
        private JsonResult JsonWithStatus(object data, bool success, string[] messages)
        {
            return Json(new AjaxResult
                            {
                                Data = data,
                                Success = success,
                                Messages = messages
                            });
        }
    }
}

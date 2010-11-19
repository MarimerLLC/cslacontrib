using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CslaContrib.Mvc;
using ProjectTracker.Library;
using ProjectTrackerMvc.ViewModels;

namespace ProjectTrackerMvc.Controllers
{
    public class ResourcesController : Controller
    {
        //
        // GET: /Resources/

        public ActionResult Index(ResourceList resourceList)
        {
            return View(resourceList);
        }

        //
        // GET: /Resources/Details/5

        public ActionResult Details(int id, Resource resource)
        {
            return View(ToViewModel(resource));
        }

        //
        // GET: /Resources/Create
        [CslaAuthorize(AccessType.Create)]
        public ActionResult Create(Resource resource)
        {
            return View(ToViewModel(resource));
        } 

        //
        // POST: /Resources/Create

        [AcceptVerbs(HttpVerbs.Post)]
        [CslaAuthorize(AccessType.Create)]
        public ActionResult Create(Resource resource, FormCollection collection)
        {
            if (!resource.IsValid || !ModelState.IsValid)
            {
                return View(ToViewModel(resource));
            } 

            resource = resource.Save();

            return RedirectToAction("Edit", new { id = resource.Id });
        }

        //
        // GET: /Resources/Edit/5
        [CslaAuthorize(AccessType.Update)]
        public ActionResult Edit(int id, Resource resource)
        {
            return View(ToViewModel(resource));
        }

        //
        // POST: /Resources/Edit/5

        [AcceptVerbs(HttpVerbs.Post)]
        [CslaAuthorize(AccessType.Update)]
        public ActionResult Edit(int id, Resource resource, FormCollection collection)
        {
            if (!resource.IsValid || !ModelState.IsValid)
            {
                return View(ToViewModel(resource));
            }

            resource.Save();

            return RedirectToAction("Index");
        }

        //
        // POST: /Resources/Delete/5

        [AcceptVerbs(HttpVerbs.Post)]
        [CslaAuthorize(AccessType.Delete, ModelType=typeof(Resource))]
        public ActionResult Delete(int id)
        {
            Resource.DeleteResource(id);

            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [CslaAuthorize(AccessType.Update)]
        public ActionResult AssignProject(int id, Guid projectId, int role, [CslaBind(Method = "GetResource", Arguments = "id")]Resource resource)
        {
            resource.Assignments.AssignTo(projectId);
            resource.Assignments[projectId].Role = role;

            resource = resource.Save();

            return RedirectToAction("Edit", new { id= resource.Id });
        }

        [CslaAuthorize(AccessType.Update)]
        public ActionResult RemoveAssignment(int id, Guid projectId, [CslaBind(Method = "GetResource", Arguments = "id")]Resource resource)
        {
            resource.Assignments.Remove(projectId);
            resource = resource.Save();

            return RedirectToAction("Edit", new { id= resource.Id });
        }

        private ResourceViewModel ToViewModel(Resource resource)
        {
            return new ResourceViewModel(resource, RoleList.GetList(), ProjectList.GetProjectList());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using ProjectTracker.Library;
using ProjectTrackerMvc.ViewModels;
using CslaContrib.Mvc;

namespace ProjectTrackerMvc.Controllers
{
    [HandleError]
    public class ProjectsController : Controller
    {
        //
        // GET: /Projects/

        public ActionResult Index()
        {
            return View(ProjectList.GetProjectList());
        }

        //
        // GET: /Projects/Details/5

        public ActionResult Details(Guid id)
        {
            var project = Project.GetProject(id);

            return View(ToViewModel(project));
        }

        //
        // GET: /Projects/Create
        [CslaAuthorize(AccessType.Create, ModelType = typeof(Project))]
        public ActionResult Create()
        {
            var project = Project.NewProject();
            
            return View(ToViewModel(project));
        } 

        //
        // POST: /Projects/Create

        [AcceptVerbs(HttpVerbs.Post)]
        [CslaAuthorize(AccessType.Create, ModelType = typeof(Project))]
        public ActionResult Create(FormCollection collection)
        {
            var project = Project.NewProject();

            if (!TryUpdateModel(project) || !project.IsValid)
            {
                return View(ToViewModel(project));
            }

            project = project.Save();

            return RedirectToAction("Edit", new { id = project.Id });
        }

        //
        // GET: /Projects/Edit/5
        [CslaAuthorize(AccessType.Update, ModelType = typeof(Project))]
        public ActionResult Edit(Guid id)
        {
            var project = Project.GetProject(id);

            return View(ToViewModel(project));
        }

        //
        // POST: /Projects/Edit/5

        [AcceptVerbs(HttpVerbs.Post)]
        [CslaAuthorize(AccessType.Update, ModelType = typeof(Project))]
        public ActionResult Edit(Guid id, FormCollection collection)
        {
            var project = Project.GetProject(id);
            
            if(!TryUpdateModel(project) || !project.IsValid)
            {
                return View(ToViewModel(project));
            }

            project.Save();

            return RedirectToAction("Index");
        }

        //
        // POST: /Projects/Delete/5

        [AcceptVerbs(HttpVerbs.Post)]
        [CslaAuthorize(AccessType.Delete, ModelType = typeof(Project))]
        public ActionResult Delete(Guid id)
        {
            Project.DeleteProject(id);

            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [CslaAuthorize(AccessType.Update, ModelType = typeof(Project))]
        public ActionResult AssignResource(Guid id, int resourceId, int role)
        {
            var project = Project.GetProject(id);
            project.Resources.Assign(resourceId);
            project.Resources.GetItem(resourceId).Role = role;

            project = project.Save();

            return RedirectToAction("Edit", new { id = project.Id });
        }

        [CslaAuthorize(AccessType.Update, ModelType = typeof(Project))]
        public ActionResult RemoveResource(Guid id, int resourceId)
        {
            var project = Project.GetProject(id);
            project.Resources.Remove(resourceId);
            project = project.Save();

            return RedirectToAction("Edit", new { id = project.Id });
        }

        private ProjectViewModel ToViewModel(Project project)
        {
            return new ProjectViewModel(project, RoleList.GetList(), ResourceList.GetResourceList());
        }

    }

}

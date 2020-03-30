﻿using BExIS.Dcm.UploadWizard;
using BExIS.Dim.Entities.Mapping;
using BExIS.Dlm.Entities.Data;
using BExIS.Dlm.Entities.DataStructure;
using BExIS.Dlm.Services.Data;
using BExIS.Dlm.Services.DataStructure;
using BExIS.IO;
using BExIS.IO.Transform.Input;
using BExIS.IO.Transform.Output;
using BExIS.IO.Transform.Validation.Exceptions;
using BExIS.Modules.Dcm.UI.Helpers;
using BExIS.Modules.Dcm.UI.Models;
using BExIS.Security.Entities.Subjects;
using BExIS.Security.Services.Subjects;
using BExIS.Security.Services.Utilities;
using BExIS.Utils.Data.Upload;
using BExIS.Utils.Upload;
using BExIS.Xml.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Vaiona.Entities.Common;
using Vaiona.Logging.Aspects;
using Vaiona.Persistence.Api;
using Vaiona.Web.Mvc;

namespace BExIS.Modules.Dcm.UI.Controllers
{
    public class SubmitSummaryController : AsyncController
    {
        private TaskManager TaskManager;
        private FileStream Stream;

        private static IDictionary<Guid, int> tasks = new Dictionary<Guid, int>();

        private UploadHelper uploadWizardHelper = new UploadHelper();

        private Dictionary<string, object> _bus = new Dictionary<string, object>(); 


        // GET: /DCM/Summary/

        private User _user;

        [HttpGet]
        public ActionResult Summary(int index)
        {
            TaskManager = (TaskManager)Session["TaskManager"];
            //set current stepinfo based on index
            if (TaskManager != null)
            {
                TaskManager.SetCurrent(index);
                // remove if existing
                TaskManager.RemoveExecutedStep(TaskManager.Current());

                _bus = TaskManager.Bus;
            }

            SummaryModel model = new SummaryModel();
            model.StepInfo = TaskManager.Current();

            model = updateModel(model);

            if (TaskManager.Bus.ContainsKey(TaskManager.DATASET_TITLE) && TaskManager.Bus[TaskManager.DATASET_TITLE]  != null)
            {
                model.DatasetTitle = TaskManager.Bus[TaskManager.DATASET_TITLE].ToString();
            }


            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Summary(object[] data)
        {
            TaskManager = (TaskManager)Session["TaskManager"];
            _bus = TaskManager.Bus;

            SummaryModel model = new SummaryModel();

            model.StepInfo = TaskManager.Current();

            
            DataASyncUploadHelper asyncUploadHelper = new DataASyncUploadHelper();
            asyncUploadHelper.Bus = _bus;
            asyncUploadHelper.User = GetUser();

                if (TaskManager.Bus.ContainsKey(TaskManager.DATASET_TITLE) && TaskManager.Bus[TaskManager.DATASET_TITLE] != null)
                {
                    model.DatasetTitle = TaskManager.Bus[TaskManager.DATASET_TITLE].ToString();
                }

            Task.Run(() => asyncUploadHelper.FinishUpload());

            // send email after starting the upload
            var es = new EmailService();
            var user = GetUser();

            es.Send("upload is starting",
                "...",
                new List<string>() { user.Email },
                new List<string>() { ConfigurationManager.AppSettings["SystemEmail"] }
                );

            // set model for the page
            #region set summary

            model = updateModel(model);

            #endregion set summary

            //ToDo: remove all changed from dataset and version
            return PartialView(model);
        }



        private User GetUser()
        {
            if (_user == null)
            {

                string username = string.Empty;

                try
                {
                    username = HttpContext.User.Identity.Name;
                }
                catch { return null; }

                UserManager userManager = new UserManager();

                _user = userManager.FindByNameAsync(username).Result;
            }

            return _user;
        }

        private SummaryModel updateModel(SummaryModel model)
        {

            TaskManager = (TaskManager)Session["TaskManager"];

            if (_bus.ContainsKey(TaskManager.DATASET_ID))
            {
                model.DatasetId = Convert.ToInt32(_bus[TaskManager.DATASET_ID]);
            }

            if (_bus.ContainsKey(TaskManager.DATASET_TITLE))
            {
                model.DatasetTitle = _bus[TaskManager.DATASET_TITLE].ToString();
            }

            if (_bus.ContainsKey(TaskManager.DATASTRUCTURE_ID))
            {
                model.DataStructureId = Convert.ToInt32(_bus[TaskManager.DATASTRUCTURE_ID]);
            }

            if (_bus.ContainsKey(TaskManager.DATASTRUCTURE_TITLE))
            {
                model.DataStructureTitle = _bus[TaskManager.DATASTRUCTURE_TITLE].ToString();
            }

            if (_bus.ContainsKey(TaskManager.RESEARCHPLAN_ID))
            {
                model.ResearchPlanId = Convert.ToInt32(_bus[TaskManager.RESEARCHPLAN_ID].ToString());
            }

            if (_bus.ContainsKey(TaskManager.RESEARCHPLAN_TITLE))
            {
                model.ResearchPlanTitle = _bus[TaskManager.RESEARCHPLAN_TITLE].ToString();
            }

            return model;
        }
    }

}
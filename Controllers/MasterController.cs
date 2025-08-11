/***************************************************************************************
 * File Name    : MasterController.cs
 * Description  : Controller for managing Unit Master views.
 *                Demonstrates usage of helper classes without database integration.
 * 
 * Developer    : Aman Kala
 * Created Date : August 7, 2025
 * Last Modified: August 7, 2025
 * Version      : 1.0.0
 * 
 * Revision History:
 * -------------------------------------------------------------------------------------
 * Date        | Developer     | Description
 * -------------------------------------------------------------------------------------
 * 2025-08-07  | Aman Kala     | Initial creation with helper class usage.
 ***************************************************************************************/

using EMP.Controllers;
using EMP.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;

namespace EMP.Controllers
{
    public class MasterController : BaseController
    {
        private readonly IToastNotification _toastNotification;
        public MasterController(ILogger<BaseController> logger, IToastNotification toastNotification) : base(logger)
        {
            _toastNotification = toastNotification;
        }

        #region Unit
        /// <summary>
        /// Displays the Unit Master index view.
        /// This view lists all existing units.
        /// </summary>
        /// <returns>The Index view.</returns>
        public IActionResult UnitIndex()
        {
            return View();
        }
        /// <summary>
        /// Displays the Create Unit view.
        /// This view allows users to input unit details and upload a logo.
        /// </summary>
        /// <returns>The Create view.</returns>
        [HttpGet]
        public IActionResult AddUnit()
        {
            return View();
        }

        /// <summary>
        /// Handles the creation of a new unit with an optional uploaded file.
        /// Validates file type and size before saving it to disk.
        /// </summary>
        /// <param name="unitName">The name of the unit being created.</param>
        /// <param name="file">The uploaded file (e.g., logo or document).</param>
        /// <returns>Redirects to Index on success or returns to Create view on failure.</returns>
        [HttpPost]
        public async Task<IActionResult> AddUnit(string unitName, IFormFile file)
        {
            
            try
            {
                if (string.IsNullOrWhiteSpace(unitName))
                {
                    ExceptionHelper.ThrowValidation("Unit name is required.");
                }
                if (file != null)
                {
                    // Validate file for both type and size
                    if (!FileHelper.ValidateFile(file, out string errorMessage))
                    {
                        ModelState.AddModelError("file", errorMessage);
                        _toastNotification.AddErrorToastMessage(errorMessage);
                        return View("AddUnit");
                    }

                    string uniqueFileName = FileHelper.GenerateUniqueFileName(file.FileName);
                    string uploadPath = Path.Combine("wwwroot", "uploads");

                    await FileHelper.SaveFileToDisk(file, uploadPath, uniqueFileName);
                }

                TempData["Success"] = "Unit created successfully.";
                _toastNotification.AddSuccessToastMessage("Unit Created Successfully");
                return RedirectToAction("UnitIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred.");
                _toastNotification.AddErrorToastMessage("An Unexpected Error Occured");
                return View("AddUnit");
            }
        }
        #endregion
    }
}

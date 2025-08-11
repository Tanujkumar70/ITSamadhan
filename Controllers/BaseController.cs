/***************************************************************************************
 * File Name    : BaseController.cs
 * Description  : Base controller providing shared functionality such as logging.
 *                All controllers should inherit from this class.
 * 
 * Developer    : Aman Kala
 * Created Date : August 6, 2025
 * Last Modified: August 6, 2025
 * Version      : 1.0.0
 * 
 * Revision History:
 * -------------------------------------------------------------------------------------
 * Date        | Developer     | Description
 * -------------------------------------------------------------------------------------
 * 2025-08-06  | Aman Kala     | Initial creation of BaseController with logging support.
 *
 ****************************************************************************************/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace EMP.Controllers
{
    /// <summary>
    /// Base controller that provides common functionality such as request logging.
    /// All controllers should inherit from this class.
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Logger instance used to log request details.
        /// </summary>
        protected readonly ILogger<BaseController> _logger;

        /// <summary>
        /// Constructor to initialize the logger.
        /// </summary>
        /// <param name="logger">Injected logger instance.</param>
        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Called before every action executes. Logs the request path automatically.
        /// </summary>
        /// <param name="context">The context for the action.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var path = context.HttpContext.Request.Path;

            // Log the incoming HTTP request path.
            _logger.LogInformation($"Request received: {path}");

            base.OnActionExecuting(context);
        }
    }
}



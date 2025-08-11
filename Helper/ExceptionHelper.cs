/***************************************************************************************
 * File Name    : ExceptionHelper.cs
 * Description  : Defines custom exception types and provides helper methods for
 *                consistent exception creation and usage across the application.
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
 * 2025-08-06  | Aman Kala   | Initial creation of the ExceptionHelper class.
 * 
 ****************************************************************************************/

using System;

namespace EMP.Helper
{
    /// <summary>
    /// Thrown when a business rule is violated.
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) { }

        public BusinessException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Thrown when a required resource or record is not found.
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Thrown when input validation fails.
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }

        public ValidationException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Optional utility class for throwing structured exceptions.
    /// </summary>
    public static class ExceptionHelper
    {
        public static void ThrowBusiness(string message)
        {
            throw new BusinessException(message);
        }

        public static void ThrowValidation(string message)
        {
            throw new ValidationException(message);
        }

        public static void ThrowNotFound(string entityName, object key)
        {
            throw new NotFoundException($"{entityName} with identifier '{key}' was not found.");
        }
    }
}


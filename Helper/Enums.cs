/***************************************************************************************
 * File Name    : Enums.cs
 * Description  : Contains enums for user roles and user account statuses, with descriptions
 *                used across the application.
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
 * 2025-08-06  | Aman Kala     | Initial creation of EnumUserRole and EnumUserAccountStatus.
 *
 ****************************************************************************************/

using System.ComponentModel;

namespace EMP.Helper
{
    public enum EnumUserRole
    {
        [Description("Admin")]
        Admin = 1,

        [Description("Unit Admin")]
        UnitAdmin = 2,

        [Description("User")]
        User = 3
    }

    public enum EnumUserAccountStatus
    {
        [Description("Inactive")]
        Inactive = 0,

        [Description("Active")]
        Active = 1
    }
}


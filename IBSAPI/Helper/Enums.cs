using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Reflection;

namespace IBSAPI.Helper
{
    public class Enums
    {
        public enum ResultFlag
        {
            [Description("Success message")]
            SucessMessage = 1,
            [Description("error")]
            ErrorMessage = 0,
            [Description("Invalid token")]
            TokenMessage = 2,
            [Description("Internal Exception")]
            InternamException = 4
        }
    }
}

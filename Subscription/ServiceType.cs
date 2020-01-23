using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Subscription
{
    public enum ServiceType
    {
        [Description("SERVICE1")]
        Service1,
        [Description("SERVICE2")]
        Service2
               // need to add here mode service types according to the services need to work with wcf.
        // DESCRIPTOIN MUST BE UPPER CASE
    }
    public static class EnumExtensionMethods
    {
        public static string GetDescription(this Enum GenericEnum)
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return GenericEnum.ToString();
        }

    }
}

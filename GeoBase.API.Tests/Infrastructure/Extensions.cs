using System.Reflection;

namespace GeoBase.API.Tests.Infrastructure;


public static class RenderExtensions
{
    public static string RenderMembers(this object o)
    {
        var properties = o.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        return string.Join(",", properties.Select(p => $"{p.Name} = {p.GetValue(o)}"));
    }
}
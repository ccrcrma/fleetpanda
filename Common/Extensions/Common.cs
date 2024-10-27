using Newtonsoft.Json;

namespace fleetpanda.common.Extensions;
public static class Common
{
    public static TResult CloneAs<TResult>(this object source) where TResult : class, new()
    {
        var json = JsonConvert.SerializeObject(source);
        return JsonConvert.DeserializeObject<TResult>(json)!;
    }
}

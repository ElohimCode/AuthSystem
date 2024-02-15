namespace Common.Authorization
{
    public record AppPermission(string Feature, string Action, string Group, string Description, bool IsBasic = false)
    {
        public string Name => NameFor(Feature, Action);
        public static string NameFor(string feature, string action)
        {
            return $"Permissions.{feature}.{action}";
        }
    }
}

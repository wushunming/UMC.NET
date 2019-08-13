
using UMC.Web;

namespace UMC.Demo.Activities
{

    [Mapping("Settings", Auth = WebAuthType.Admin)]
    public class SettingsFlow : WebFlow
    {

        public override WebActivity GetFirstActivity()
        {
            switch (this.Context.Request.Command)
            {
                case "Wildcard":
                    return new SettingsWildcardActivity();
                case "Role":
                    return new SettingsRoleActivity();
                case "User":
                    return new SettingsUserActivity();

            }

            return WebActivity.Empty;
        }
    }
}

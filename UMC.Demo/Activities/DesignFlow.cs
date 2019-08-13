
using UMC.Web;

namespace UMC.Demo.Activities
{

    [Mapping("Design", Auth = WebAuthType.User)]
    class DesignFlow : WebFlow
    {
        public override WebActivity GetFirstActivity()
        {
            switch (this.Context.Request.Command)
            {
                case "Item":
                    return new DesignItemActivity();
                case "Click":
                    return new DesignClickActivity();
                case "Custom":
                    return new DesignCustomActivity();
                case "Items":
                    return new DesignItemsActivity();
                case "Image":
                    return new DesignImageActivity();
                case "UI":
                    return WebActivity.Empty; 
                case "Home":
                    return new DesignUIActivity(false);
                default:
                    if (this.Context.Request.Command.StartsWith("UI"))
                    {
                        return new DesignConfigActivity();
                    }
                    return WebActivity.Empty;

            }

        }
    }
}

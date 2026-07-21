using Microsoft.AspNetCore.Mvc;

namespace AtlantisMarketplace.Api.Controllers;

/// <summary>
/// This class does not need to be changed for the purposes of the task - though you may use it.
/// </summary>
public class AtlantisControllerBase : ControllerBase
{
    // Pretend we are getting this from auth header
    protected string GetLoggedUserId()
    {
        return "FakeUserId";
    }
}
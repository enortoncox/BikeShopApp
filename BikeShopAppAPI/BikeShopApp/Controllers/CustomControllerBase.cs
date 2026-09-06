using Microsoft.AspNetCore.Mvc;

namespace BikeShopApp.WebAPI.Controllers
{
    /// <summary>
    /// Provides common functionality for API controllers.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomControllerBase : ControllerBase
    {
    }
}

using Microsoft.AspNetCore.Mvc;
using TheGymApplication.ViewModels.Common;
using TheGymDomain.Models.Common;

namespace TheGymApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GenericController<TEntity, TEntityViewModel> : Controller where TEntity: Entity where TEntityViewModel: EntityViewModel
    {
    }
}

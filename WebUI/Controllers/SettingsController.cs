using fleetpanda.common.Extensions;
using fleetpanda.dataaccess.Entities;
using fleetpanda.dataaccess.Repositories.Abstractions;
using fleetpanda.webui.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

namespace fleetpanda.webui.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ISettingsRepository _repo;
        private readonly IServiceProvider _serviceProvider;
        public SettingsController(ISettingsRepository settingsRepository, IServiceProvider serviceProvider)
        {
            _repo = settingsRepository;
            _serviceProvider = serviceProvider;
        }
        public async Task<IActionResult> Configure()
        {
            var resp = await _repo.GetActiveJobSetting();
            if (!resp.Success)
                return View();

            var model = resp.Data as JobSettings;
            if (model is null) return View();
            var dto = model.CloneAs<JobSettingsDto>();
            return View(dto);

        }

        [HttpPost]
        public async  Task<IActionResult> Configure(JobSettingsDto args)
        {
            var model = args.CloneAs<JobSettings>();
            await _repo.AddJobSettingsAsync(model);
            return RedirectToAction("Configure");
        }
    }
}

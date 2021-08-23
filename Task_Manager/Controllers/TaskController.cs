using ApiServices.ApiService.Interfaces;
using ApiServices.EmailService;
using AutoMapper;
using BL.Managers.Interfaces;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Task_Manager.DTO;

namespace Task_Manager.Controllers
{
    [Route("api/task")]
    [System.Web.Http.Authorize]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskManager _taskManager;
        private readonly IWeatherProcessor _weatherProcessor;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<User> _userManager;

        public TaskController(
            IMapper mapper,
            ITaskManager taskManager,
            UserManager<User> userManager,
            IEmailSender emailSender,
            IWeatherProcessor weatherProcessor)
        {
            _weatherProcessor = weatherProcessor;
            _mapper = mapper;
            _taskManager = taskManager;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] int? id, CancellationToken token)
        {
            if (id == null || !ModelState.IsValid)
                return BadRequest();

            var result = await _taskManager.DeleteTaskAsync(id.Value, token);

            if (result == 0)
            {
                return Ok(false);
            }

            return Ok(true);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] TaskModelDto taskModel, CancellationToken token)
        {
            if (taskModel == null || !ModelState.IsValid)
                return BadRequest();
            try
            {
               var e = _userManager.FindByNameAsync(User.Identity.Name);
                var dataApi = await _weatherProcessor.LoadWeatherInformation(taskModel.Url, taskModel.Header);
            }
            catch(Exception ex)
            {
                throw new Exception();
            }

            taskModel.Email = _userManager.FindByNameAsync(User.Identity.Name).Result.Email.ToString();
            //var dataApi = await _weatherProcessor.LoadWeatherInformation(taskModel.Url, taskModel.Header);
            var task = _mapper.Map<TaskModel>(taskModel);

            var result = await _taskManager.AddTaskAsync(task, token);

            //var message = new Message(new string[] { null /*user.Email */}, task.Name, task.Description, null);
            //await _emailSender.SendEmailAsync(message);

            if (result == 0)
            {
                return Ok(false);
            }

            return Ok(true);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] TaskModelDto taskModel, CancellationToken token)
        {
            if (taskModel == null || !ModelState.IsValid)
                return BadRequest();

            var task = _mapper.Map<TaskModel>(taskModel);

            var result = await _taskManager.UpdateAsync(task, token);

            if (result == 0)
            {
                return Ok(false);
            }

            return Ok(true);
        }

        [HttpPost("getTaskById")]
        public async Task<IActionResult> GetTaskById([FromBody] int? id, CancellationToken token)
        {
            if (id == 0 || !ModelState.IsValid)
                return BadRequest();

            var taskById = await _taskManager.GetTaskByIdAsync(id.Value, token);

            if (taskById == null)
            {
                return Ok(false);
            }

            return Ok(taskById);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            try
            {
                var e = _userManager.FindByNameAsync(User.Identity.Name);
                
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            var tasks = await _taskManager.GetAllTaskAsync(token);

            return Ok(tasks);
        }

    }
}

using AutoMapper;
using BL.Managers.Interfaces;
using Entities.Models;
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
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public TaskController(
            IMapper mapper,
            ITaskManager taskManager,
            UserManager<User> userManager
            )
        {
            _mapper = mapper;
            _taskManager = taskManager;
            _userManager = userManager;
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] int? id, CancellationToken token)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] TaskModelDto taskModel, CancellationToken token)
        {
            try
            {
                if (taskModel == null || !ModelState.IsValid)
                    return BadRequest();

                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                taskModel.Email = user.Email;
                var task = _mapper.Map<TaskModel>(taskModel);

                var result = await _taskManager.AddTaskAsync(task, token);

                if (result == 0)
                {
                    return Ok(false);
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] TaskModelDto taskModel, CancellationToken token)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("getTaskById")]
        public async Task<IActionResult> GetTaskById([FromBody] int? id, CancellationToken token)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            try
            {
                var tasks = await _taskManager.GetAllTaskAsync(token);

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}

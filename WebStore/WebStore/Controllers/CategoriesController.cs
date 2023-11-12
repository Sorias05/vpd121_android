using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.Data.Entities;
using WebStore.Data.Entities.Identity;
using WebStore.Models.Category;

namespace WebStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly AppEFContext _appEFContext;
        private readonly IMapper _mapper;
        private readonly UserManager<UserEntity> _userManager;
        public CategoriesController(AppEFContext appEFContext, IMapper mapper,
            UserManager<UserEntity> userManager)
        {
            _appEFContext = appEFContext;
            _mapper = mapper;
            _userManager = userManager;
        }
        [HttpGet("list")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Claims.First().Value;
            var user = await _userManager.FindByEmailAsync(userName);
            var model = await _appEFContext.Categories
                .Where(x => x.IsDeleted == false)
                .Where(x => x.UserId == user.Id)
                .Select(x => _mapper.Map<CategoryItemViewModel>(x))
                .ToListAsync();

            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            string userName = User.Claims.First().Value;
            var user = await _userManager.FindByEmailAsync(userName);
            var cat = await _appEFContext.Categories
                .Where(x => x.IsDeleted == false)
                .Where(x => x.UserId == user.Id)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (cat == null)
                return NotFound();

            var model = _mapper.Map<CategoryItemViewModel>(cat);
            return Ok(model);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CategoryCreateViewModel model)
        {
            try
            {
                var cat = _mapper.Map<CategoryEntity>(model);
                string userName = User.Claims.First().Value;
                var user = await _userManager.FindByEmailAsync(userName);
                cat.UserId = user.Id;
                string imageName = String.Empty;
                if (model.Image != null)
                {
                    string exp = Path.GetExtension(model.Image.FileName);
                    imageName = Path.GetRandomFileName() + exp;
                    string dirSaveImage = Path.Combine(Directory.GetCurrentDirectory(), "images", imageName);
                    using (var stream = System.IO.File.Create(dirSaveImage))
                    {
                        await model.Image.CopyToAsync(stream);
                    }
                }
                cat.Image = imageName;
                await _appEFContext.Categories.AddAsync(cat);
                await _appEFContext.SaveChangesAsync();
                return Ok(_mapper.Map<CategoryItemViewModel>(cat));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPut("update")]
        public async Task<IActionResult> Put([FromBody] CategoryUpdateViewModel model)
        {
            try
            {
                string userName = User.Claims.First().Value;
                var user = await _userManager.FindByEmailAsync(userName);
                var cat = await _appEFContext.Categories
                .Where(x => x.IsDeleted == false)
                .Where(x => x.UserId == user.Id)
                .SingleOrDefaultAsync(x => x.Id == model.Id);

                if (cat == null)
                    return NotFound();

                cat.Name = model.Name;
                cat.Description = model.Description;
                cat.Image = model.Image;
                await _appEFContext.SaveChangesAsync();

                return Ok(_mapper.Map<CategoryItemViewModel>(cat));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                string userName = User.Claims.First().Value;
                var user = await _userManager.FindByEmailAsync(userName);
                var cat = await _appEFContext.Categories
                .Where(x => x.IsDeleted == false)
                .Where(x => x.UserId == user.Id)
                .SingleOrDefaultAsync(x => x.Id == id);

                if (cat == null)
                    return NotFound();

                cat.IsDeleted = true;
                await _appEFContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}

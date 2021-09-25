using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheGymApplication.ViewModels.Common;
using TheGymDomain.Interfaces;
using TheGymDomain.Models.Common;

namespace TheGymApplication.Controllers
{
    //TODO: model validation
    //TODO: AddRange and EditRange
    [Route("[controller]")]
    [ApiController]
    public class GenericController<TEntity, TEntityViewModel> : Controller where TEntity: Entity where TEntityViewModel: EntityViewModel
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly IRepository<TEntity> _repository;

        public GenericController(IUnitOfWork unitOfWork, IMapper mapper, IRepository<TEntity> repository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var model = await _repository.GetByIdAsync(Id);
            if(model != null)
            {
                return Ok(_mapper.Map<TEntityViewModel>(model));
            }

            return BadRequest();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<List<TEntityViewModel>>(await _repository.GetAsync()));
        }

        //TODO: model validation
        [HttpPost("Add")]
        public async Task<IActionResult> Add(TEntityViewModel vm)
        {
            var model = _mapper.Map<TEntity>(vm);
            model = await _repository.AddAsync(model);
            await _unitOfWork.SaveAsync(User);
            return Ok();
        }

        [HttpPut("Edit")]
        public virtual async Task<IActionResult> Edit(TEntityViewModel vm)
        {
            var model = await _repository.GetByIdAsync(vm.Id);
            if(model == null)
            {
                return BadRequest();
            }
            model = _mapper.Map(vm, model);

            _repository.Update(model);
            await _unitOfWork.SaveAsync(User);
            return Ok();
        }

        //TODO: model validation
        [HttpDelete("DeleteById/{id}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            if((await _repository.GetByIdAsync(id)) == null)
            {
                return BadRequest();
            }
            await _repository.RemoveAsync(id);
            await _unitOfWork.SaveAsync(User);

            return Ok();
        }

        //TODO: model validation
        [HttpDelete("DeleteRangeById/{id}")]
        public async Task<IActionResult> DeleteRange(IEnumerable<Guid> idList)
        {
            foreach(var id in idList)
            {
                if (await _repository.GetByIdAsync(id) == null)
                {
                    return BadRequest();
                }
            }

            await _repository.RemoveRangeAsync(idList);
            await _unitOfWork.SaveAsync(User);

            return Ok();
        }
    }
}

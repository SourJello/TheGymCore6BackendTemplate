using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheGymApplication.ViewModels.Common;
using TheGymDomain.Interfaces;
using TheGymDomain.Models.Common;

namespace TheGymApplication.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class GenericController<TEntity, TEntityViewModel> : Controller where TEntity: Entity where TEntityViewModel: EntityViewModel
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly IRepository<TEntity> _repository;
        protected readonly IValidator<TEntity> _validator;

        public GenericController(IUnitOfWork unitOfWork, IMapper mapper, IRepository<TEntity> repository, IValidator<TEntity> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = repository;
            _validator = validator;
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

        [HttpPost("Add")]
        public async Task<IActionResult> Add(TEntityViewModel vm)
        {
            var model = _mapper.Map<TEntity>(vm);
            if (_validator.Validate(model).IsValid)
            {
                model = await _repository.AddAsync(model);
                await _unitOfWork.SaveAsync(User);
                return Ok();
            }
            Log.Error(_validator.Validate(model).Errors.ToString());
            return BadRequest();
        }

        [HttpPost("AddRange")]
        public async Task<IActionResult> AddRange(IEnumerable<TEntityViewModel> vm)
        {
            var models = _mapper.Map<List<TEntity>>(vm);
            foreach(var model in models)
            {
                if (!_validator.Validate(model).IsValid)
                {

                    Log.Error(_validator.Validate(model).Errors.ToString());
                    return BadRequest();
                }
            }
            await _repository.AddRangeAsync(models);
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

            if (_validator.Validate(model).IsValid)
            {
                _repository.Update(model);
                await _unitOfWork.SaveAsync(User);
                return Ok();
            }
            Log.Error(_validator.Validate(model).Errors.ToString());
            return BadRequest();
        }

        [HttpPut("Edit")]
        public virtual async Task<IActionResult> EditRange(IEnumerable<TEntityViewModel> vms)
        {
            foreach(var vm in vms)
            {
                if (await _repository.GetByIdAsync(vm.Id) == null)
                {
                    return BadRequest();
                }
            }

            var models = _mapper.Map<List<TEntity>>(vms);
            foreach(var model in models)
            {
                if (!_validator.Validate(model).IsValid)
                {
                    Log.Error(_validator.Validate(model).Errors.ToString());
                    return BadRequest();

                }
            }

            _repository.UpdateRange(models);
            await _unitOfWork.SaveAsync(User);
            return Ok();

        }

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

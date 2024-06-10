using DominandoEFCore18.Data;
using DominandoEFCore18.Data.Repositories;
using DominandoEFCore18.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore18.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartamentoController : ControllerBase
    {
        
        private readonly ILogger<DepartamentoController> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IDepartamentoRepository _departamentoRepository;
        private readonly IDepartamentoGenericRepository _departamentoGenericRepository;

        public DepartamentoController(ILogger<DepartamentoController> logger, IDepartamentoRepository departamentoRepository, IUnitOfWork uow, IDepartamentoGenericRepository departamentoGenericRepository)
        {
            _logger = logger;
            _departamentoRepository = departamentoRepository;
            _uow = uow;
            _departamentoGenericRepository = departamentoGenericRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var departamento = await _departamentoRepository.GetByIdAsync(id);

            return Ok(departamento);
        }

        [HttpPost]
        public IActionResult CreateDepartamento(Departamento departamento)
        {
            //_departamentoRepository.Add(departamento);
            _uow.DepartamentoRepository.Add(departamento);

            _uow.Commit();

            return Ok(departamento);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveDepartamentoAsync(int id)
        {
            var departamento = await _uow.DepartamentoGenericRepository.GetByIdAsync(id);

            _uow.DepartamentoGenericRepository.Remove(departamento);

            _uow.Commit();

            return Ok(departamento);
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarDepartamentoAsync([FromQuery] string descricao)
        {
            var departamento = await _uow.DepartamentoGenericRepository.GetDataAsync(
                p => p.Descricao.Contains(descricao),
                p => p.Include(c => c.Colaboradores),
                take: 2
            );

            return Ok(departamento);
        }
    }
}
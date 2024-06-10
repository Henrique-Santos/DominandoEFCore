using DominandoEFCore17.Data;
using DominandoEFCore17.Domain;
using Microsoft.AspNetCore.Mvc;

namespace DominandoEFCore17.Controllers
{
    [ApiController]
    [Route("{tenant}/[controller]")]
    public class PersonController : ControllerBase
    {

        private readonly ILogger<PersonController> _logger;

        public PersonController(ILogger<PersonController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Person> Get([FromServices]ApplicationDbContext db)
        {
            var persons = db.Persons.ToArray();

            return persons;
        }
    }
}

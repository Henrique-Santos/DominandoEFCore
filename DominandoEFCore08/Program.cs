using DominandoEFCore08.Data;
using DominandoEFCore08.Domain;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DominandoEFCore08
{
    class Program
    {
        static void Main(string[] args)
        {
            /* ---------------- Modelo de dados ------------------------ */
            //Collations();
            //PropagarDados();
            //Esquema();
            //ConversorCustomizado();
            //TrabalhandoComPropriedadeDeSombra();
            //TiposDePropriedades();
            //Relacionamento1Para1();
            //Relacionamento1ParaMuitos();
            //RelacionamentoMuitosParaMuitos();
            //CampoDeApoio();
            //ExemploTPH();
            PacotesDePropriedades();
        }

        static void PacotesDePropriedades()
        {
            using (var db = new ApplicationDbContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var configuracao = new Dictionary<string, object>
                {
                    ["Chave"] = "SenhaBancoDeDados",
                    ["Valor"] = Guid.NewGuid().ToString()
                };

                db.Configuracoes.Add(configuracao);
                db.SaveChanges();

                var configuracoes = db.Configuracoes
                    .AsNoTracking()
                    .Where(p => p["Chave"] == "SenhaBancoDeDados")
                    .ToArray();

                foreach (var dic in configuracoes)
                {
                    Console.WriteLine($"Chave: {dic["Chave"]} - Valor: {dic["Valor"]}");
                }
            }
        }

        // (TPA) Tabela Por Hierarquia
        static void ExemploTPH()
        {
            using (var db = new ApplicationDbContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var pessoa = new Pessoa { Nome = "Fulano de Tal" };

                var instrutor = new Instrutor { Nome = "Rafael Almeida", Tecnologia = ".NET", Desde = DateTime.Now };

                var aluno = new Aluno { Nome = "Maria Thysbe", Idade = 31, DataContrato = DateTime.Now.AddDays(-1) };

                db.AddRange(pessoa, instrutor, aluno);
                db.SaveChanges();

                var pessoas = db.Pessoas.AsNoTracking().ToArray();
                var instrutores = db.Instrutores.AsNoTracking().ToArray();
                //var alunos = db.Alunos.AsNoTracking().ToArray();

                // Obtendo Alunos a partir de Pessoas por meio do Tipo
                var alunos = db.Pessoas.OfType<Aluno>().AsNoTracking().ToArray();

                Console.WriteLine("Pessoas **************");
                foreach (var p in pessoas)
                {
                    Console.WriteLine($"Id: {p.Id} -> {p.Nome}");
                }

                Console.WriteLine("Instrutores **************");
                foreach (var p in instrutores)
                {
                    Console.WriteLine($"Id: {p.Id} -> {p.Nome}, Tecnologia: {p.Tecnologia}, Desde: {p.Desde}");
                }

                Console.WriteLine("Alunos **************");
                foreach (var p in alunos)
                {
                    Console.WriteLine($"Id: {p.Id} -> {p.Nome}, Idade: {p.Idade}, Data do Contrato: {p.DataContrato}");
                }
            }
        }

        static void CampoDeApoio()
        {
            using var db = new ApplicationDbContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var documento = new Documento();
            documento.SetCpf("12345678933");

            db.Documentos.Add(documento);
            db.SaveChanges();

            foreach (var doc in db.Documentos.AsNoTracking())
            {
                Console.WriteLine($"CPF -> {doc.GetCpf()}");
            }
        }

        static void RelacionamentoMuitosParaMuitos()
        {
            using (var db = new ApplicationDbContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var ator1 = new Ator { Nome = "Rafael" };
                var ator2 = new Ator { Nome = "Pires" };
                var ator3 = new Ator { Nome = "Bruno" };

                var filme1 = new Filme { Descricao = "A volta dos que não foram" };
                var filme2 = new Filme { Descricao = "Poeira em alto mar" };
                var filme3 = new Filme { Descricao = "As mil tranças do rei careca" };

                ator1.Filmes.Add(filme1);
                ator1.Filmes.Add(filme2);

                ator2.Filmes.Add(filme1);

                filme3.Atores.Add(ator1);
                filme3.Atores.Add(ator2);
                filme3.Atores.Add(ator3);

                db.AddRange(ator1, ator2, filme3);

                db.SaveChanges();

                foreach (var ator in db.Atores.Include(a => a.Filmes))
                {
                    Console.WriteLine($"Ator: {ator.Nome}");

                    foreach (var filme in ator.Filmes)
                    {
                        Console.WriteLine($"\t Filme: {filme.Descricao}");
                    }
                }
            }
        }

        static void Relacionamento1ParaMuitos()
        {
            using (var db = new ApplicationDbContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var estado = new Estado
                {
                    Nome = "Sergipe",
                    Governador = new() { Nome = "Rafael" },
                    Cidades = [new Cidade { Nome = "Itabaiana" }]
                };

                db.Estados.Add(estado);

                db.SaveChanges();
            }

            using (var db = new ApplicationDbContext())
            {
                var estados = db.Estados.Include(e => e.Cidades).AsNoTracking().ToList();

                estados[0].Cidades.Add(new Cidade { Nome = "Aracaju" });

                db.SaveChanges();

                foreach (var estado in estados)
                {
                    Console.WriteLine($"Estado: {estado.Nome}, Governado: {estado.Governador.Nome}");

                    foreach (var cidade in estado.Cidades)
                    {
                        Console.WriteLine($"\t Cidade: {cidade.Nome}");
                    }
                }
            }
        }

        static void Relacionamento1Para1()
        {
            using var db = new ApplicationDbContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var estado = new Estado
            {
                Nome = "Sergipe",
                Governador = new() { Nome = "Rafael" }
            };

            db.Estados.Add(estado);

            db.SaveChanges();

            var estados = db.Estados.AsNoTracking().ToList();

            estados.ForEach(estado => Console.WriteLine($"Estado: {estado.Nome}, Governado: {estado.Governador.Nome}"));
        }

        static void TiposDePropriedades()
        {
            using var db = new ApplicationDbContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var cliente = new Cliente { 
                Nome = "Fulano",
                Telefone = "(79) 98888-9999",
                Endereco = new() { Bairro = "Centro", Cidade = "Sao Paulo" }
            };

            db.Clientes.Add(cliente);

            db.SaveChanges();

            var clientes = db.Clientes.AsNoTracking().ToList();

            var options = new JsonSerializerOptions { WriteIndented = true };

            clientes.ForEach(c =>
            {
                var json = JsonSerializer.Serialize(c, options);

                Console.WriteLine(json);
            });
        }

        static void TrabalhandoComPropriedadeDeSombra()
        {
            using var db = new ApplicationDbContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var departamento = new Departamento { Descricao = "Departamento propriedade de sombra" };

            db.Departamentos.Add(departamento);

            // Acessando shadow property
            db.Entry(departamento).Property("UltimaAtualizacao").CurrentValue = DateTime.Now;

            db.SaveChanges();

            // Fazendo uma consulta usando a shadow property
            var departamentos = db.Departamentos.Where(d => EF.Property<DateTime>(d, "UltimaAtualizacao") < DateTime.Now).ToArray();
        }

        static void ConversorCustomizado()
        {
            using var db = new ApplicationDbContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Conversores.Add(new Domain.Conversor { Status = Domain.Status.Devolvido });

            db.SaveChanges();

            var conversorEmAnalise = db.Conversores.AsNoTracking().FirstOrDefault(c => c.Status == Domain.Status.Analise);
            var conversorDevolvido = db.Conversores.AsNoTracking().FirstOrDefault(c => c.Status == Domain.Status.Devolvido);
        }

        static void Esquema()
        {
            using var db = new ApplicationDbContext();

            var script = db.Database.GenerateCreateScript();

            Console.WriteLine(script);
        }

        static void PropagarDados()
        {
            using var db = new ApplicationDbContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var script = db.Database.GenerateCreateScript();

            Console.WriteLine(script);
        }

        static void Collations()
        {
            using var db = new ApplicationDbContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}
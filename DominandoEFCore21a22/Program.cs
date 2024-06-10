using DominandoEFCore21a22;
using DominandoEFCore21a22.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

/* ---------------- Sobrescrevendo comportamentos do ef-core ------------------------ */

DiagnosticListener.AllListeners.Subscribe(new MyInterceptorListener());

using var db = new ApplicationDbContext();

db.Database.EnsureCreated();

var sql = db.Departamentos.Where(p => p.Id > 0).ToQueryString();

_ = db.Departamentos.Where(p => p.Id > 0).ToArray();

Console.WriteLine(sql);
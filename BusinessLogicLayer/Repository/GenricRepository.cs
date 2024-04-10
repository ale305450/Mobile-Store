using DataAccessLayer.Data;
using DataAccessLayer.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository
{
    public class GenricRepository<T> : IGenricRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> DbTable;
        public GenricRepository(ApplicationDbContext context)
        {
            _context = context;
            DbTable = _context.Set<T>();
        }
        public void Delete(Guid id)
        {
            T db = DbTable.Find(id);
            if (db != null)
                _context.Remove(db);
        }
        public IEnumerable<T> GetAll()
        {
            return DbTable.ToList();
        }
        public T GetById(Guid id)
        {
            return DbTable.Find(id);
        }
        public void Create(T entity)
        {
            DbTable.Add(entity);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

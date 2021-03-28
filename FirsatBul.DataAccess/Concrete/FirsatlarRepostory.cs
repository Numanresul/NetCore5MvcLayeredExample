using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirsatBul.DataAccess.Abstract;
using FirsatBul.Entities;

namespace FirsatBul.DataAccess.Concrete
{
    public class FirsatlarRepostory : IFirsatlarRepository
    {
        private FirsatBulDbContext _firsatBulDbContext;
        public FirsatlarRepostory(FirsatBulDbContext firsatBulDbContext)
        {
            _firsatBulDbContext = firsatBulDbContext;
        }

        //--------------------------

        public Firsatlar CreateFirsat(Firsatlar firsat)
        {
            _firsatBulDbContext.Add(firsat);
            _firsatBulDbContext.SaveChanges();
            return firsat;
        }

        public void DeleteFirsat(int id)
        {
            throw new NotImplementedException();
        }

        public List<Firsatlar> GetAllFirsatlar()
        {
            return _firsatBulDbContext.Firsatlar.ToList();
            
        }

        public Firsatlar GetFirsatById(int id)
        {
            throw new NotImplementedException();
        }

        public Firsatlar UpdateFirsat(Firsatlar firsat)
        {
            throw new NotImplementedException();
        }
    }
}

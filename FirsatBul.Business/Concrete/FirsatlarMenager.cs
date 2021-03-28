using FirsatBul.Business.Abstract;
using FirsatBul.DataAccess.Abstract;
using FirsatBul.DataAccess.Concrete;
using FirsatBul.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirsatBul.Business.Concrete
{
    public class FirsatlarMenager : IFirsatlarService  // ilgili interface'i kalıtım alarak içeriğindeki metotları dolduruyoruz.
    {
        private IFirsatlarRepository _firsatlarService;
        public FirsatlarMenager(IFirsatlarRepository firsatlarService) 
        {
            //_hotelRepository = new HotelRepository();
            _firsatlarService = firsatlarService; // dependency injection prensibine aykırı olan new ile bir nesne oluşturmamak için için dependency injection gereği startup da tanımlama yapıyoruz
        }

        public Firsatlar CreateFirsat(Firsatlar firsat)
        {
          return _firsatlarService.CreateFirsat(firsat);
            
        }

        public void DeleteFirsat(int id)
        {
            throw new NotImplementedException();
        }

        public List<Firsatlar> GetAllFirsatlar()
        {
            return _firsatlarService.GetAllFirsatlar();
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

using FirsatBul.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirsatBul.DataAccess.Abstract
{
    public interface IFirsatlarRepository // bir interface oluşturarak veritabanı işlemleri yapacak class'ımızın içereceği metotları ve property leri belirliyoruz.
    {
        List<Firsatlar> GetAllFirsatlar();
        Firsatlar GetFirsatById(int id);
        Firsatlar CreateFirsat(Firsatlar firsat);
        Firsatlar UpdateFirsat(Firsatlar firsat);
        void DeleteFirsat(int id);
    }
}

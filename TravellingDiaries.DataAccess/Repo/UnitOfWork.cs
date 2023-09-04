using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravellingDiaries.DataAccess.Data;
using TravellingDiaries.DataAccess.Repo.Auth;
using TravellingDiaries.DataAccess.Repo.IRepo;
using TravellingDiaries.DataAccess.Repo.IRepo.Auth;

namespace TravellingDiaries.DataAccess.Repo;

public class UnitOfWork : IUnitOfWork
{
    private AppDbContext _db;
    public UnitOfWork(AppDbContext db)
    {
        _db = db;

        //Auth = new AuthRepo(_db);
    }

    public void Save()
    {
        _db.SaveChangesAsync();
    }
    //public IAuthRepo Auth { get; private set; }
}

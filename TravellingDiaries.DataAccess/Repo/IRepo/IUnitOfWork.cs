using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravellingDiaries.DataAccess.Repo.IRepo.Auth;

namespace TravellingDiaries.DataAccess.Repo.IRepo;

public interface IUnitOfWork
{
    void Save();
    //IAuthRepo Auth { get; }
}

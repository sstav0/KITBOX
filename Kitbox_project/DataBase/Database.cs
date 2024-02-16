using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Kitbox_project;

public abstract class Database
{
    public abstract string user();
    public abstract string password();
    public abstract string tablename();

    public abstract void GetList();
    public abstract void GetById();
    public abstract void Save();
    public abstract void Update();
   
}
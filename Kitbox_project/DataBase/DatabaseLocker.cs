namespace Kitbox_project;

using System;
using Kitbox_project.DataBase;
using Kitbox_project.Models;
using MySql.Data.MySqlClient;
public class DatabaseLocker : Database
    {
    
    public DatabaseLocker(string id, string password) : base(id, password)
    {
        tablename = "Locker";
    }
    
    //retrieve every idLocker from a cabinet
    //Not tested !! !! !! !! !! !! !! !! !! !! !! !! !! /!\
    public async Task<List<string>> GetLockersIdList(string cabinetId)
    {
        Dictionary<string, string> conditionDict = new Dictionary<string, string> { { "idCabinet", cabinetId } };
        List<string> columList = new List<string>() { "idCabinet" };
        List<Dictionary<string,string>> idCaninetLiDict = await GetData(conditionDict, columList);
        List<string> idListToReturn = new List<string>();
        foreach (Dictionary<string,string> dic in idCaninetLiDict)
        {
            idListToReturn.Add(dic["idLocker"]);
            Console.WriteLine(dic["idLocker"]);
        }
        return idListToReturn;
    }
    /*
    public async Task<Dictionary<string,string>> GetLockerById(string lockerId)
    {
        
    }
    */
}

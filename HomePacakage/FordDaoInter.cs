using System.Collections.Generic;
using System.Data;

namespace Client_Management_2._1
{
    interface FordDaoInter
    {
        void Add(Ford ford, Description description);
        //void Update(Ford ford, Description description, int id, bool control);
        void Update(Ford ford, Description description, int id, int control);
        void Delete(int id);
        List<Ford> GetById(int id);
        List<Ford> GetByName(string name);      
        List<Ford> GetByPhone(string phone);
        List<Ford> GetByVin(string vin);
        List<Ford> GetByCarPlateNumber(string carPlateNumber);
        DataTable GetByCPNumber(string carPlateNumber);
    }
}

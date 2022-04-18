using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Management_2._1.BlackClientPackage
{
    interface BlackClientDaoInter
    {
        void Add(BlackClient blackClient);
        void Update(BlackClient blackClient);
        void Delete(int id);

        List<BlackClient> GetByName(string name);
        List<BlackClient> GetByPhone(string phone);
        List<BlackClient> GetByCPNumber(string cpnumber);
        List<BlackClient> GetByVin(string vin);
    }
}

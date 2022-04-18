using Client_Management_2._1.BlackClientPackage;
using Client_Management_2._1.OrdersPackage;

namespace Client_Management_2._1
{
    class Context
    {
        public static FordDaoInter InstanceOfFordDao()
        {
            return new FordDaoImpl();
        }

        public static OrderDaoInter InstanceOfOrderDao()
        {
            return new OrderDaoImpl();
        }

        public static BlackClientDaoInter InstanceOfBlackClientDao()
        {
            return new BlackClientDaoImpl();
        }

    }
}

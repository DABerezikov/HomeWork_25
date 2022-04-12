using System;

namespace HomeWork_25
{
    public interface IClient
    {
        Guid ID { get; set; }

        string Name { get; set; }

        double DepositRate { get; set; }

        Account<IndividualClient> Deposit { get; set; }
        void AddDepositAcount(Account<IndividualClient> account);


    }
  


}

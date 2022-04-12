using System;

namespace HomeWork_25
{
    internal class EntityClient : IndividualClient
    {
        public EntityClient(string Name) : base(Name)
        {

            this.depositRate = 24;
        }
        private Account<EntityClient> deposit;

        public Account<EntityClient> Deposit { get { return this.deposit; } set { this.deposit = value; } }

        public void AddDepositAcount(Account<EntityClient> account)
        {
            this.deposit = account;
        }
    }

    public class IndividualClient : IClient
    {
        public IndividualClient(string Name)
        
        {
            this.id = Guid.NewGuid();
            this.name = Name;
            this.depositRate = 12;


        }
        private protected Guid id;
        private protected string name;
        private protected double depositRate;
        private Account<IndividualClient> deposit;

        public Guid ID { get { return this.id; } set { this.id = value; } }

        public string Name { get { return this.name; } set { this.name = value; } }

        public double DepositRate { get { return this.depositRate; } set { this.depositRate = value; } }



        

        public Account<IndividualClient> Deposit { get { return this.deposit; } set { this.deposit = value; } }

        public void AddDepositAcount(Account<IndividualClient> account)
        {
            this.deposit = account;
        }
    }

    internal class VIPClient : IndividualClient
    {
        public VIPClient(string Name) : base(Name)
        {
            this.depositRate = 36;
        }
        private Account<VIPClient> deposit;

        public Account<VIPClient> Deposit { get { return this.deposit; } set { this.deposit = value; } }

        public void AddDepositAcount(Account<VIPClient> account)
        {
            this.deposit = account;
        }


    }
}

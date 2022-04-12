using System;

namespace HomeWork_25
{

    public interface IAccount
    {
        /// <summary>
        /// Номер клиента банка
        /// </summary>
        Guid ClientID { get; set; } 
        /// <summary>
        /// Тип счета
        /// </summary>
        double DepositRate { get ; set ;}

        /// <summary>
        /// Дата открытия счета
        /// </summary>
        string OpeningDate { get; set ; }

        /// <summary>
        /// Дата открытия счета
        /// </summary>
        string ClosingDate { get; set; }

        /// <summary>
        /// Сумма на счету
        /// </summary>
        double Amount { get; set; }

        /// <summary>
        /// Процент по счету
        /// </summary>
        double Interest { get; set; }

        /// <summary>
        /// Дата пополнения счета
        /// </summary>
        string RefillDate { get; set; }

        /// <summary>
        /// Накопленные проценты
        /// </summary>
        double TempInterest { get; set; }


        void OpenAccount(double Amount);


        /// <summary>
        /// Метод пополнения счета
        /// </summary>
        /// <param name="Refill">Сумма пополнения</param>
        void Refill(double Refill);


        /// <summary>
        /// Метод для перевода со счета
        /// </summary>
        /// <param name="Transfer">Сумма перевода</param>
        void Transfer(double Transfer);


        /// <summary>
        /// Метод для закрытия счета
        /// </summary>
        /// <returns></returns>
        double CloseAccount();


        void TransferAccount(IAccount Sender, IAccount Recipient, double Amount);
        

    }

    public interface IRefill<out T>
        where T : IAccount
    {
        T Refill(double Refill);
    }

        


    public class Account<T> : IAccount
         where T : IClient

    {
        public Account()
        {
            this.clientID = default;
            this.openingDate = default;
            this.closingDate = null;
            this.amount = default;
            this.depositRate = default;
            this.interest = default;
            this.refillDate = null;

        }
        public Account(double Amount, T Client) 
        {
            this.clientID = Client.ID;
            this.openingDate = DateTime.Now.ToShortDateString();
            this.closingDate = null;
            this.amount = Amount;
            this.depositRate = Client.DepositRate;
            this.interest = Interest;
            this.refillDate = null;
           

        }

        public Account(double Amount, IClient Client)
        {
            this.clientID = Client.ID;
            this.openingDate = DateTime.Now.ToShortDateString();
            this.closingDate = null;
            this.amount = Amount;
            this.depositRate = Client.DepositRate;
            this.interest = Interest;
            this.refillDate = null;
        }


        /// <summary>
        /// Поля счета в банке
        /// </summary>        
        private Guid clientID;                   // Номер клиента       
        private string openingDate;             // Дата открытия счета
        private string closingDate;             // Дата закрытия счета
        private double amount;                  // Сумма на счете
        private protected double depositRate;   // Процентная ставка по счету
        private double interest;                // Сумма процентов на счете        
        private string refillDate;              // Дата пополнения счета



        /// <summary>
        /// Номер клиента банка
        /// </summary>
        public Guid ClientID { get { return this.clientID; } set { this.clientID = value; } }

        /// <summary>
        /// Тип счета
        /// </summary>
        public double DepositRate { get { return this.depositRate; } set { this.depositRate = value; } }

        /// <summary>
        /// Дата открытия счета
        /// </summary>
        public string OpeningDate { get { return this.openingDate; } set { this.openingDate = value; } }

        /// <summary>
        /// Дата открытия счета
        /// </summary>
        public string ClosingDate { get { return this.closingDate; } set { this.closingDate = value; } }

        /// <summary>
        /// Сумма на счету
        /// </summary>
        public double Amount { get { return Math.Round(this.amount); } set { this.amount = value; } }



        /// <summary>
        /// Процент по счету
        /// </summary>
        public double Interest
        {
            get
            {
                double year;
                if (RefillDate == null)
                {
                    year = (DateTime.Now - DateTime.Parse(this.openingDate)).Days / 365.25;                    

                }
                else
                {
                    year = (DateTime.Now - DateTime.Parse(this.refillDate)).Days / 365.25;                   

                }
                this.interest = this.amount * year * this.depositRate / 100;

                return Math.Round(this.interest, 2);

            }
            set { this.interest = value; }

        }


        /// <summary>
        /// Дата пополнения счета
        /// </summary>
        public string RefillDate { get { return this.refillDate; } set { this.refillDate = value; } }

        public double TempInterest { get; set; }
        

        /// <summary>
        /// Метод открытия счета
        /// </summary>
        /// <param name="Amount">Сумма для внесения на счет</param>
        public void OpenAccount(double Amount)
        {
            this.amount = Amount;
            this.openingDate = DateTime.Now.ToShortDateString();
        }

        /// <summary>
        /// Метод пополнения счета
        /// </summary>
        /// <param name="Refill">Сумма пополнения</param>
        public void Refill(double Refill)
        {
            TempInterest += Interest;                               // Сохранение текущих процентов
            this.amount += Refill;                                  // Увеличение суммы на счете на величину пополнения и количества процентов на данный момент            
            this.refillDate = DateTime.Now.ToShortDateString();     // Установление даты пополнения счета для дальнейшего расчета процентов 

        }

        /// <summary>
        /// Метод для перевода со счета
        /// </summary>
        /// <param name="Transfer">Сумма перевода</param>
        public void Transfer(double Transfer)
        {
            TempInterest += Interest;                               // Сохранение текущих процентов
            this.amount -= Transfer;                                // Уменьшение суммы на счете на величину перевода            
            this.refillDate = DateTime.Now.ToShortDateString();     // Установление даты пополнения счета для дальнейшего расчета процентов 

        }

        /// <summary>
        /// Метод для закрытия счета
        /// </summary>
        /// <returns></returns>
        public double CloseAccount()
        {
            this.amount += Interest + TempInterest;
            double pay = Amount;
            Amount = 0;
            this.closingDate = DateTime.Now.ToShortDateString();
            return pay;
        }

        public void TransferAccount(IAccount Sender, IAccount Recipient, double Amount)
        {
            Sender.Transfer(Amount);
            Recipient.Refill(Amount);
        }


    }
    

}

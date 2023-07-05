using System;
//Martin Karlsson NET22
//Implementerat design mönster Singleton, Strategy, Factory
namespace ImplementeraLabb
{
    public class cardHolder
    {
        public class CardHolder
        {
            private string cardNum;
            private int pin;
            private string firstName;
            private string lastName;
            private double balance;

            public CardHolder(string cardNum, int pin, string firstName, string lastName, double balance)
            {
                this.cardNum = cardNum;
                this.pin = pin;
                this.firstName = firstName;
                this.lastName = lastName;
                this.balance = balance;
            }

            public string GetCardNum()
            {
                return cardNum;
            }

            public int GetPin()
            {
                return pin;
            }

            public string GetFirstName()
            {
                return firstName;
            }

            public string GetLastName()
            {
                return lastName;
            }

            public double GetBalance()
            {
                return balance;
            }

            public void SetCardNum(string newCardNum)
            {
                cardNum = newCardNum;
            }

            public void SetPin(int newPin)
            {
                pin = newPin;
            }

            public void SetFirstName(string newFirstName)
            {
                firstName = newFirstName;
            }

            public void SetLastName(string newLastName)
            {
                lastName = newLastName;
            }

            public void SetBalance(double newBalance)
            {
                balance = newBalance;
            }
        }
        // Strategy design mönstret tillåter oss att ändra på våra metoder utan att överkomplicera det genom att använda oss utav vår strategy context
        
        public interface ITransactionStrategy  
        {
            void PerformTransaction(CardHolder currentUser);
        }

        public class DepositStrategy : ITransactionStrategy
        {
            public void PerformTransaction(CardHolder currentUser)
            {
                Console.WriteLine("How much money would you like to deposit: ");
                double deposit = double.Parse(Console.ReadLine());
                currentUser.SetBalance(currentUser.GetBalance() + deposit);
                Console.WriteLine("Thank you for your transaction. Your new balance is: " + currentUser.GetBalance());
            }
        }

        public class WithdrawStrategy : ITransactionStrategy
        {
            public void PerformTransaction(CardHolder currentUser)
            {
                Console.WriteLine("How much money would you like to withdraw: ");
                double withdrawal = double.Parse(Console.ReadLine());
                if (currentUser.GetBalance() < withdrawal)
                {
                    Console.WriteLine("Insufficient balance :(");
                }
                else
                {
                    currentUser.SetBalance(currentUser.GetBalance() - withdrawal);
                    Console.WriteLine("Thank you for your precious time. ");
                }
            }
        }

        public class BalanceStrategy : ITransactionStrategy
        {
            public void PerformTransaction(CardHolder currentUser)
            {
                Console.WriteLine("Your current balance: " + currentUser.GetBalance());
            }
        }

        public interface ITransactionStrategyFactory
        {
            ITransactionStrategy CreateStrategy(int option);
        }

        public class TransactionStrategyFactory : ITransactionStrategyFactory
        {
            public ITransactionStrategy CreateStrategy(int option)
            {
                switch (option)
                {
                    case 1:
                        return new DepositStrategy();
                    case 2:
                        return new WithdrawStrategy();
                    case 3:
                        return new BalanceStrategy();
                    default:
                        throw new ArgumentException("Invalid option.");
                }
            }
        }

        public class TransactionContext
        {
            private ITransactionStrategy strategy;

            public TransactionContext(ITransactionStrategy strategy)
            {
                this.strategy = strategy;
            }

            public void ExecuteStrategy(CardHolder currentUser)
            {
                strategy.PerformTransaction(currentUser);
            }
        }
        // Genom att använda oss utav Singleton design mönstret så ser vi till att bara en instans av klassen kan bli skapad och att programmet får tillgång till koden inuti singleton
        
        public class CardHolderSingleton
        {
            private static CardHolderSingleton instance;
            private List<CardHolder> cardHolders;
            private CardHolder currentUser;

            private CardHolderSingleton()
            {
                cardHolders = new List<CardHolder>
        {
            new CardHolder("283756123", 1234, "Jill", "Carlsson", 800.22),
            new CardHolder("056837455", 3355, "Reidun", "Nilsson", 250.25),
            new CardHolder("144872345", 8675, "Andreas", "Blom", 880.33),
            new CardHolder("465778633", 3333, "Martin", "Karlsson", 550.11),
            new CardHolder("456734552", 2456, "Christoph", "Waltz", 999.23)
        };
            }

            public static CardHolderSingleton GetInstance()
            {
                if (instance == null)
                {
                    instance = new CardHolderSingleton();
                }
                return instance;
            }

            public void RunBankSystem()
            {
                Console.WriteLine("Welcome to the bank of Sundsvall");
                Console.WriteLine("Please insert your debit card number: ");
                string debitCardNum = "";

                while (true)
                {
                    try
                    {
                        debitCardNum = Console.ReadLine();
                        currentUser = cardHolders.FirstOrDefault(a => a.GetCardNum() == debitCardNum);
                        if (currentUser != null)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid card, please try again");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Invalid card, please try again");
                    }
                }

                Console.WriteLine("Please enter your pin: ");
                int userPin = 0;
                while (true)
                {
                    try
                    {
                        userPin = int.Parse(Console.ReadLine());
                        if (currentUser != null && currentUser.GetPin() == userPin)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Incorrect pin, please try again");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Incorrect pin, please try again");
                    }
                }

                Console.WriteLine("Welcome " + currentUser.GetFirstName());
                // Implementerar Factory method som med hjälp av Transaktions gränsnittet och kan därefter skapa en specifik transaktion baserat på vad som är angivet
                ITransactionStrategyFactory strategyFactory = new TransactionStrategyFactory(); 
                

                int option = 0;
                do
                {
                    PrintOptions();
                    try
                    {
                        option = int.Parse(Console.ReadLine());
                        if (option >= 1 && option <= 3)
                        {
                            ITransactionStrategy strategy = strategyFactory.CreateStrategy(option);
                            TransactionContext context = new TransactionContext(strategy);
                            context.ExecuteStrategy(currentUser);
                        }
                        else if (option == 4)
                        {
                            break;
                        }
                        else
                        {
                            option = 2;
                        }
                    }
                    catch
                    {
                        option = 0;
                    }
                }
                while (option != 4);

                Console.WriteLine("Thank you! Grand bank of Sundsvall thanks you. ");
            }

            private void PrintOptions()
            {
                Console.WriteLine("Choose one of the following options");
                Console.WriteLine("1. Make a deposit");
                Console.WriteLine("2. Make a withdrawal");
                Console.WriteLine("3. Show your bank balance");
                Console.WriteLine("4. Exit");
            }
        }

        public class Program
        {
            public static void Main(string[] args)
            {
                CardHolderSingleton cardHolderSingleton = CardHolderSingleton.GetInstance();
                cardHolderSingleton.RunBankSystem();
            }
        }
        }
    }
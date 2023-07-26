using System;
using System.Xml;

namespace Shop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Shop Shop = new Shop();

            Shop.Work();
        }
    }

    class Shop
    {
        private Vendor _vendor = new Vendor();
        private Player _player = new Player(10);

        public Shop()
        {
            CreateShop();
        }

        public bool IsWork { get; private set; }
        public int QuantityItems { get; private set; }

        public void Work()
        {
            while (IsWork)
            {
                ShowMenuShop();
                CloseShop();
            }
        }

        private void CreateShop()
        {
            IsWork = true;
            QuantityItems = _vendor.CalculateQuantityItems();
        }

        private void CloseShop()
        {
            QuantityItems = _vendor.CalculateQuantityItems();

            if (QuantityItems == 0)
            {
                IsWork = false;
                Console.WriteLine($"магазин закрыт. Товар закончился");
                Console.ReadKey();
            }
        }


        private void ShowMenuShop()
        {
            const string ShowAllItemsInShopMenu = "1";
            const string SellItemsMenu = "2";
            const string ShowInventorMenu = "3";

            Console.Clear();
            Console.WriteLine("Добро пожаловать в магазин");
            Console.WriteLine($"Выберите пункт в меню:");
            Console.WriteLine($"{ShowAllItemsInShopMenu} - Показать товары в магазине");
            Console.WriteLine($"{SellItemsMenu} - Купить товар");
            Console.WriteLine($"{ShowInventorMenu} - Показать инвентарь");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case ShowAllItemsInShopMenu:
                    ShowItemsWarehouse();
                    break;

                case SellItemsMenu:
                    SellItem();
                    break;

                case ShowInventorMenu:

                    break;

                default:
                    Console.WriteLine("Ошибка ввода команды.");
                    Console.ReadKey();
                    break;
            }
        }

        private void ShowItemsWarehouse()
        {
            _vendor.ShowAllItems();
        }

        private void SellItem()
        {
            if (TryGetItem() == false)
                Console.WriteLine("Покупка не состоялась");
            else
                Console.WriteLine("Вы приобрели товар");

            Console.ReadKey();
        }

        private bool TryGetItem()
        {

            if (_vendor.TryGetItem(out Potion potion, out int amount) == false)
                return false;

            Console.WriteLine("Введите количество товара для покупки.");

            if (GetQuantityItems(out int quantutyItems, amount) == false)
                return false;

            int bill = potion.Price * quantutyItems;

            if(_player.CanPay(bill)==false)
            {
                Console.WriteLine("Не достаточно денег.");
                return false;
            }

            _player.Buy(potion, quantutyItems, bill);
            _vendor.Sell(potion, quantutyItems, bill);
            return true;
        }

        private bool GetQuantityItems(out int quantutyItems, int amount)
        {
            if (Utilite.TryGetPositiveNumber(out quantutyItems)==false)
                return false;

            if (quantutyItems > amount)
            {
                Console.WriteLine("Нет такого количества товара");
                return false;
            }

            return true;
        }
    }

    class Potion
    {
        public Potion(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; private set; }
        public int Price { get; private set; }

        public void ShowInfo()
        {
            Console.Write($"Зелье {Name}, цена {Price} золотых за штуку.");
        }
    }

    class Stack
    {
        public Stack(Potion potion, int quantity)
        {
            Potion = potion;
            Quantity = quantity;
        }

        public Potion Potion { get; private set; }
        public int Quantity { get; private set; }

        public void ShowInfo()
        {
            Potion.ShowInfo();
            Console.Write($" Количество - {Quantity}.\n");
        }

        public void IncreaseQuantity(int quantity)
        {
            Quantity = Quantity + quantity;
        }

        public void DecreaseQuantity(int quantity)
        {
            Quantity = Quantity - quantity;
        }

    }

    class Character
    {
        protected List<Stack> Stacks = new List<Stack>();

        public Character(int money)
        {
            Money = money;
        }

        public int Money { get; protected set; }

        public void Info()
        {
            Console.WriteLine($"Количество денег - {Money} золотых.\n");
        }

        public void GetItem(Stack stack)
        {
            Stacks.Add(stack);
        }

        public void ShowAllItems()
        {
            for (int i = 0; i < Stacks.Count; i++)
            {
                int numberInlist = i + 1;
                Console.Write($"{numberInlist}) ");
                Stacks[i].ShowInfo();
            }

            Console.ReadKey();
        }

        public bool GetNumberStack(int numberItem)
        {
            numberItem--;

            if (numberItem > Stacks.Count)
            {
                Console.WriteLine("Нет такого товара.");
                return false;
            }

            return true;
        }
    }

    class Player : Character
    {
        public Player(int money) : base(money) { }

        public bool CanPay(int bill)
        {
            return Money >= bill;
        }

        public void Buy(Potion potion, int amount, int bill)
        {
            bool isExistItem=false;

            for (int i = 0;i<Stacks.Count;i++)
            {
                if (Stacks[i].Potion.Name == potion.Name)
                {
                    isExistItem = true;
                    Stacks[i].IncreaseQuantity(amount);
                    Money -= bill;
                    return;
                }
            }

            if (isExistItem==false)
                Stacks.Add(new Stack(potion, amount));
        }
    }

    class Vendor : Character
    {
        public Vendor() : base(100)
        {
            Fill();
        }

        public int CalculateQuantityItems()
        {
            int quantity = 0;

            for (int i = 0; i < Stacks.Count; i++)
                quantity += Stacks[i].Quantity;

            return quantity;
        }

        private void Fill()
        {
            Stacks.Add(new Stack(new Potion("Исцеления", 4), 10));
            Stacks.Add(new Stack(new Potion("Востановления", 5), 15));
            Stacks.Add(new Stack(new Potion("Среднее исцеления", 10), 6));
            Stacks.Add(new Stack(new Potion("Скорость", 50), 3));
        }

        public bool TryGetItem(out Potion potion, out int amount)
        {
            potion = null;
            amount = 0;

            Console.WriteLine("Введите номер товара для покупки.");

            if (Utilite.TryGetPositiveNumber(out int potionIndex) == false)
                return false;

            if (potionIndex > Stacks.Count)
                return false;

            potion = Stacks[potionIndex - 1].Potion;
            amount = Stacks[potionIndex - 1].Quantity;
            return true;
        }

        public void Sell(Potion potion, int amount, int bill)
        {
            for (int i = 0; i < Stacks.Count; i++)
            {
                if (Stacks[i].Potion.Name == potion.Name)
                {
                    Stacks[i].DecreaseQuantity(amount);
                    Money += bill;
                    return;
                }
            }
        }
    }

    class Utilite
    {
        public static bool TryGetPositiveNumber(out int numder)
        {
            string userInput;

            do
            {
                userInput = Console.ReadLine();
            }
            while (GetInputValue(userInput, out numder));

            if (GetNumberRange(numder))
            {
                Console.WriteLine("Хорошая попытка.");
                return false;
            }

            return true;
        }

        private static bool GetInputValue(string input, out int number)
        {
            if (int.TryParse(input, out number) == false)
            {
                Console.WriteLine("Не корректный ввод.");
                return true;
            }

            return false;
        }

        private static bool GetNumberRange(int number)
        {
            int positiveValue = 0;

            if (number < positiveValue)
                return true;

            return false;
        }
    }
}
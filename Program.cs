namespace Shop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Vendor vendor = new Vendor();
            Player player = new Player(100);
            Shop Shop = new Shop(vendor, player);

            Shop.Work();
        }
    }

    class Shop
    {
        private Vendor _vendor;
        private Player _player;

        private bool _isWork;
        private int _quantityItems;

        public Shop(Vendor vendor, Player player)
        {
            Create();
            _vendor = vendor;
            _player = player;
        }

        public void Work()
        {
            while (_isWork)
            {
                ShowMenu();
                Close();
            }
        }

        private void Create()
        {
            _isWork = true;
            _quantityItems = _vendor.CalculateQuantityItems();
        }

        private void Close()
        {
            _quantityItems = _vendor.CalculateQuantityItems();

            if (_quantityItems == 0)
            {
                _isWork = false;
                Console.WriteLine($"магазин закрыт. Товар закончился");
                Console.ReadKey();
            }
        }

        private void ShowMenu()
        {
            const string CommandShowAllItemsInShop = "1";
            const string CommandSellItems = "2";
            const string CommandShowInventor = "3";

            Console.Clear();
            Console.WriteLine("Добро пожаловать в магазин");
            Console.WriteLine($"Выберите пункт в меню:");
            Console.WriteLine($"{CommandShowAllItemsInShop} - Показать товары в магазине");
            Console.WriteLine($"{CommandSellItems} - Купить товар");
            Console.WriteLine($"{CommandShowInventor} - Показать инвентарь");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case CommandShowAllItemsInShop:
                    ShowItemsWarehouse();
                    break;

                case CommandSellItems:
                    SellItem();
                    break;

                case CommandShowInventor:
                    ShowInventorPlayer();
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
            _vendor.ShowAllItems();

            if (_vendor.TryGetItem(out Potion potion, out int amount) == false)
                return false;

            Console.WriteLine("Введите количество товара для покупки.");

            if (GetQuantityItems(out int quantutyItems, amount) == false)
                return false;

            int bill = potion.Price * quantutyItems;

            if (_player.CanPay(bill) == false)
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
            int lowerLimit = 0;
            quantutyItems=Utilite.GetNumberInRange(lowerLimit);

            if (quantutyItems > amount)
            {
                Console.WriteLine("Нет такого количества товара");
                return false;
            }

            return true;
        }

        private void ShowInventorPlayer()
        {
            _player.ShowMoney();
            _player.ShowAllItems();
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
            Quantity += quantity;
        }

        public void DecreaseQuantity(int quantity)
        {
            Quantity -= quantity;
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

        public void ShowMoney()
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
            bool isExistItem = false;

            for (int i = 0; i < Stacks.Count; i++)
            {
                if (Stacks[i].Potion.Name == potion.Name)
                {
                    isExistItem = true;
                    Stacks[i].IncreaseQuantity(amount);
                    Console.ReadKey();
                    return;
                }
            }

            if (isExistItem == false)
                Stacks.Add(new Stack(potion, amount));

            Money -= bill;
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

        public bool TryGetItem(out Potion potion, out int amount)
        {
            potion = null;
            amount = 0;

            Console.WriteLine("Введите номер товара для покупки.");

            int lowerLimit = 0;
            int potionIndex = Utilite.GetNumberInRange(lowerLimit);

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

        private void Fill()
        {
            Stacks.Add(new Stack(new Potion("Исцеления", 4), 10));
            Stacks.Add(new Stack(new Potion("Востановления", 5), 15));
            Stacks.Add(new Stack(new Potion("Среднее исцеления", 10), 6));
            Stacks.Add(new Stack(new Potion("Скорость", 50), 3));
        }
    }

    class Utilite
    {
        public static int GetNumberInRange(int lowerLimitRangeNumbers = Int32.MinValue, int upperLimitRangeNumbers = Int32.MaxValue)
        {
            bool isEnterNumber = true;
            int enterNumber = 0;
            string userInput;

            while (isEnterNumber)
            {
                Console.WriteLine($"Введите число.");

                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out enterNumber) == false)
                    Console.WriteLine("Не корректный ввод.");
                else if (VerifyForAcceptableNumber(enterNumber, lowerLimitRangeNumbers, upperLimitRangeNumbers))
                    isEnterNumber = false;
            }

            return enterNumber;
        }

        private static bool VerifyForAcceptableNumber(int number, int lowerLimitRangeNumbers, int upperLimitRangeNumbers)
        {
            if (number < lowerLimitRangeNumbers)
            {
                Console.WriteLine($"Число вышло за нижний предел допустимого значения.");
                return false;
            }
            else if (number > upperLimitRangeNumbers)
            {
                Console.WriteLine($"Число вышло за верхний предел допустимого значения.");
                return false;
            }

            return true;
        }
    }
}
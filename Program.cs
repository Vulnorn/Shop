namespace Shop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player(1000);
            Vendor vendor = new Vendor();

            vendor.Trade(player);
            
        }
    }

    class Player
    {
        private List<Potion> _inventory = new List<Potion>();

        public Player(int money)
        {
            Money = money;
        }

        public int Money { get; private set; }

        public void Info()
        {
            Console.WriteLine($"Количество денег - {Money} золотых.\n");
        }

        public void GetItem(Potion potion)
        {
            _inventory.Add(potion);
            Pay(potion);
        }

        public void ShowInvetory()
        {
            for (int i = 0; i < _inventory.Count; i++)
                _inventory[i].ShowInfo();     
            
            Console.ReadKey();
        }

        private void Pay(Potion potion)
        {
            Money -= potion.Quantity * potion.Price;
        }
    }

    class Vendor
    {
        private List<Potion> _items = new List<Potion>();

        public Vendor()
        {
            CreateShop();
            CalculateQuantityItems();
            IsWork = true;
        }

        public int Money { get; private set; }
        public int QuantityItems { get; private set; }
        public bool IsWork { get; private set; }

        public void Trade(Player player)
        {

            while (WokShop())
            {
                Console.Clear();
                Console.WriteLine("Добро пожаловать в магазин, что жедаете купить?");
                int inputUser;

                player.Info();
                ShowItems();

                Console.WriteLine("Выберите зелье которое хотите купить.");

                if (GetInputUser(out inputUser) == false)
                    continue;

                inputUser -= 1;

                SellPotion(player, inputUser);
                player.ShowInvetory();

                CloseShop();
            }
        }

        private bool SellPotion(Player player, int inputUser)
        {
            if (CheckQuantityItems(inputUser) == false)
                return false;

            Console.WriteLine("Сколько штук хотите купить?");

            if (GetInputUser(out int quantityItemsBuy) == false)
                return false;

            if (CheckSolvencyPlayer(player, quantityItemsBuy, inputUser) == false)
                return false;

            player.GetItem(new Potion(_items[inputUser].Name, quantityItemsBuy, _items[inputUser].Price));



            return true;
        }

        private bool CheckSolvencyPlayer(Player player, int quantityItemsBuy, int inputUser)
        {
            int solvencyPlayer = player.Money - (quantityItemsBuy * _items[inputUser].Price);

            if (solvencyPlayer < 0)
            {
                Console.WriteLine("У Вас не достаточно денег для покупки.");
                return false;
            }

            return true;
        }

        private bool CheckQuantityItems(int inputUser)
        {
            if (GetNumberRange(_items[inputUser].Quantity))
            {
                Console.WriteLine("Товар закончился, его не возможно купить.");
                Console.ReadKey();
                return false;
            }

            if (inputUser > _items.Count)
            {
                Console.WriteLine("Нет такого количества товара");
                Console.ReadKey();
                return false;
            }

            return true;
        }


        private void ShowItems()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                int index = i + 1;
                Console.Write($"{index}) ");
                _items[i].ShowInfo();
            }
        }

        private void CreateShop()
        {

            _items.Add(new Potion("Исцеления", 10, 4));
            _items.Add(new Potion("Восполнения", 14, 5));
            _items.Add(new Potion("Среднего Исцеления", 24, 12));
            _items.Add(new Potion("Скорости", 4, 50));
        }

        private bool WokShop()
        {
            return IsWork;
        }

        private void CalculateQuantityItems()
        {
            for (int i = 0; i < _items.Count; i++)
                QuantityItems += _items[i].Quantity;
        }

        private void CloseShop()
        {
            if (QuantityItems == 0)
            {
                IsWork = false;
                Console.WriteLine($"магазин закрыт. Товар закончился");
                Console.ReadKey();
            }
        }

        private bool GetInputUser(out int numder)
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
                Console.ReadKey();
                return false;
            }

            return true;
        }

        private bool GetInputValue(string input, out int number)
        {
            if (int.TryParse(input, out number) == false)
            {
                Console.WriteLine("Не корректный ввод.");
                Console.ReadKey();
                return true;
            }

            return false;
        }

        private bool GetNumberRange(int number)
        {
            int positiveValue = 0;

            if (number < positiveValue)
                return true;

            return false;
        }


    }

    class Potion
    {
        public Potion(string name, int quantity, int price)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
        }

        public string Name { get; private set; }
        public int Quantity { get; private set; }
        public int Price { get; private set; }

        public void ShowInfo()
        {
            Console.WriteLine($"Зелье {Name} - {Quantity} штук, стоит {Price} золотых за штуку.");
        }


    }
}

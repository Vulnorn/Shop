namespace Shop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player(10);
            Vendor vendor= new Vendor();
            Shop Shop = new Shop();

            Shop.Work();
        }
    }

    class Shop
    {

        public Shop()
        {
            CreateShop();
            CalculateQuantityItems();
            IsWork = true;
        }

        public bool IsWork { get; private set; }

        public void Work(Character player)
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

                inputUser--;

                SellPotion(player, inputUser);
                player.ShowInvetory();

                CloseShop();
            }
        }

        private bool SellPotion(Character player, int inputUser)
        {

        }

        private bool CheckSolvencyPlayer(Character player, int quantityItemsBuy, int inputUser)
        {

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

        }

        private void CreateShop()
        {

            _items.Add(new Potion("Исцеления", 10, 4));
            _items.Add(new Potion("Восполнения", 14, 5));
            _items.Add(new Potion("Среднего Исцеления", 24, 12));
            _items.Add(new Potion("Скорости", 4, 50));

            _boxes.Add(new Stack(new Potion("Исцеления", 10, 4), 4));
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
    }

    class Repository
    {
        public List<Stack> Stack = new List<Stack>();

        public void GetItem(Stack stack)
        {
           Stack.Add(stack);
        }


    }

    class Inventor : Repository
    {
        public Inventor()
        {
            Stack = new List<Stack>();
        }
    }

    class Warehouse : Repository
    {
        public Warehouse()
        {
            Stack = new List<Stack>();
            Fill();
        }

        private void Fill()
        {
            Stack.Add(new Stack( new Potion("Исцеления", 4),10));
            Stack.Add(new Stack(new Potion("Востановления", 5), 15));
            Stack.Add(new Stack(new Potion("Среднее исцеления", 10), 6));
            Stack.Add(new Stack(new Potion("Скорость", 50), 3));
        }
    }

    class Character
    {
        protected Repository Repository = new Repository();

        public Character(int money)
        {
            Money = money;
        }

        public int Money { get; private set; }

        public void Info()
        {
            Console.WriteLine($"Количество денег - {Money} золотых.\n");
        }

        public void GetItem(Stack stack)
        {
            Repository.Stack.Add(stack);
        }

        public void ShowAllItems()
        {
            for (int i = 0; i < Repository.Stack.Count; i++)
                Repository.Stack[i].ShowInfo();

            Console.ReadKey();
        }

        private void Pay()
        {
        }
    }

    class Player : Character
    {
        public Player(int money) : base(money) { }
    }

    class Vendor : Character
    {
        public Vendor() : base(100) { }
    }
}

using System;
using System.Collections.Generic;

namespace Supermarket
{
    class Program
    {
        static void Main(string[] args)
        {
            Supermarket supermarket = new Supermarket();

            supermarket.CreateBuyers();
            supermarket.AddCells();
            supermarket.ShowInfo();
            supermarket.ServeClients();
            supermarket.ShowMoney();
        }
    }

    class Supermarket
    {
        private int _money;
        private int _numberbuyers = 3;
        private Random _random = new Random();
        private List<Cell> _cells = new List<Cell>();
        private Queue<Buyer> _buyers = new Queue<Buyer>();

        public Supermarket()
        {
            _cells.Add(new Cell(new Product("Лук", 20), 1000));
            _cells.Add(new Cell(new Product("Редис", 20), 1000));
            _cells.Add(new Cell(new Product("Арбуз", 10), 1000));
            _cells.Add(new Cell(new Product("Огурцы", 30), 1000));
            _cells.Add(new Cell(new Product("Свекла", 15), 1000));
            _cells.Add(new Cell(new Product("Капуста", 30), 1000));
            _cells.Add(new Cell(new Product("Морковь", 25), 1000));
            _cells.Add(new Cell(new Product("Картофель", 25), 1000));
        }

        public void CreateBuyers()
        {
            for (int i = 0; i < _numberbuyers; i++)
            {
                _buyers.Enqueue(new Buyer());
            }
        }

        public void AddCells()
        {
            int numberCells = 8;
            int quantityWeight = 9;
            int minQuantityWeight = 4;

            foreach (Buyer buyer in _buyers)
            {
                for (int i = 0; i < numberCells; i++)
                {
                    int randomNumber = _random.Next(_cells.Count);
                    int randomWeightQuantity = _random.Next(quantityWeight) + minQuantityWeight;
                    buyer.AddCell(_cells[randomNumber].Product.Name, _cells[randomNumber].Product.Cost, randomWeightQuantity);
                    _cells[randomNumber].ReduceWeight(randomWeightQuantity);
                }
            }
        }

        public void ServeClients()
        {
            int numberBuyer = 1;

            do
            {
                Buyer buyer = _buyers.Dequeue();
                Console.WriteLine("Покупатель " + numberBuyer + " убрал из корзины:");

                while (buyer.Money <= buyer.GetCost())
                {
                    int index = _random.Next(buyer.GetCountCells());
                    IncreaceWeight(buyer.GetName(index), buyer.GetWeight(index));
                    Console.WriteLine(buyer.GetName(index) + "   " + buyer.GetWeight(index) + " кг   " + buyer.GetCost(index) + " руб.");
                    buyer.DeleteCell(index);
                }

                numberBuyer++;
                buyer.ReduceMoney(buyer.GetCost());
                _money += buyer.GetCost();
                Console.Write("\n");
                buyer.ShowInfo();
            }
            while (_buyers.Count > 0);
        }

        public void IncreaceWeight(string name, int weight)
        {
            foreach (Cell cell in _cells)
            {
                if (cell.Product.Name == name)
                {
                    cell.IncreaseWeight(weight);
                }
            }
        }

        public void ShowInfo()
        {
            int numberBuyer = 1;
            Console.WriteLine("Количество покупателей в очереди " + _buyers.Count);
            ShowMoney();
            Console.Write("\n");

            foreach (Buyer buyer in _buyers)
            {
                Console.WriteLine("Покупатель " + numberBuyer++);
                buyer.ShowInfo();
            }
        }

        public void ShowMoney()
        {
            Console.WriteLine("Количество денег у магазина " + _money + " руб.");
        }
    }

    class Product
    {
        public Product(string name, int cost)
        {
            Name = name;
            Cost = cost;
        }

        public string Name { get; private set; }
        public int Cost { get; private set; }

    }

    class Cell
    {
        public Cell(Product product, int weight)
        {
            Product = product;
            Weight = weight;
        }

        public Product Product { get; private set; }
        public int Weight { get; private set; }

        public void ReduceWeight(int weight)
        {
            Weight -= weight;
        }

        public void IncreaseWeight(int weight)
        {
            Weight += weight;
        }

        public int CalculateCost()
        {
            return Product.Cost * Weight;
        }
    }

    class Buyer
    {
        private Basket _basket = new Basket();

        public Buyer()
        {
            Money = 1000;
        }

        public int Money { get; private set; }

        public void AddCell(string name, int coast, int weight)
        {
            _basket.AddCell(name, coast, weight);
        }

        public int GetCost()
        {
            return _basket.SumUpCost();
        }

        public void ReduceMoney(int money)
        {
            Money -= money;
        }

        public int DeleteCell(int index)
        {
            return _basket.DeleteCell(index);
        }

        public int GetCountCells()
        {
            return _basket.GetCount();
        }

        public string GetName(int index)
        {
            return _basket.GetName(index);
        }

        public int GetWeight(int index)
        {
            return _basket.GetWeight(index);
        }

        public int GetCost(int index)
        {
            return _basket.GetCost(index);
        }

        public void ShowInfo()
        {
            Console.WriteLine("У покупателя осталось " + Money + " руб.");
            _basket.ShowInfo();
        }
    }

    class Basket
    {
        private List<Cell> _cells = new List<Cell>();

        public void AddCell(string name, int coast, int weight)
        {
            _cells.Add(new Cell(new Product(name, coast), weight));
        }

        public int SumUpCost()
        {
            int costProducts = 0;

            foreach (Cell cell in _cells)
            {
                costProducts += cell.CalculateCost();
            }

            return costProducts;
        }

        public int DeleteCell(int index)
        {
            int weight = _cells[index].Weight;
            _cells.RemoveAt(index);
            return weight;
        }

        public int GetCount()
        {
            return _cells.Count;
        }

        public string GetName(int index)
        {
            return _cells[index].Product.Name;
        }

        public int GetWeight(int index)
        {
            return _cells[index].Weight;
        }

        public int GetCost(int index)
        {
            return _cells[index].Weight * _cells[index].Product.Cost;
        }

        public void ShowInfo()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                Console.WriteLine(_cells[i].Product.Name + "     " + _cells[i].Product.Cost + " руб/кг   " + _cells[i].Weight + " кг   " + _cells[i].Product.Cost * _cells[i].Weight + " руб.");
            }

            Console.WriteLine("Итого " + SumUpCost() + " руб.");
            Console.Write("\n");
        }
    }
}
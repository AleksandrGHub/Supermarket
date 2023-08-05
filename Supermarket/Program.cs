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
            supermarket.PayProducts();
        }
    }

    class Supermarket
    {
        private int _money;
        private int _numberbuyers = 1;
        private Random _random = new Random();
        private List<Cell> _cells = new List<Cell>();
        private List<Buyer> _buyers = new List<Buyer>();

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
                _buyers.Add(new Buyer());
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

        public void PayProducts()
        {
            do
            {
                while (_buyers[0].Money <= _buyers[0].GetCost())
                {
                    int index = _random.Next(_buyers[0].GetCountCells());
                    IncreaceWeight(_buyers[0].GetName(index), _buyers[0].GetWeight(index));
                    _buyers[0].DeleteCell(index);
                }
                _buyers[0].ReduceMoney(_buyers[0].GetCost());
                _money += _buyers[0].GetCost();
                _buyers.RemoveAt(0);
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
    }

    class Product
    {
        public string Name { get; private set; }
        public int Cost { get; private set; }

        public Product(string name, int cost)
        {
            Name = name;
            Cost = cost;
        }
    }

    class Cell
    {
        public Product Product { get; private set; }
        public int Weight { get; private set; }

        public Cell(Product product, int weight)
        {
            Product = product;
            Weight = weight;
        }

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
        public int Money { get; private set; }
        private Basket _basket = new Basket();

        public Buyer()
        {
            Money = 1000;
        }

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
            _cells.RemoveAt(index);
            return _cells[index].Weight;
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
    }
}
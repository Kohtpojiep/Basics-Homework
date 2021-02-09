using System;
using System.Collections.Generic;

namespace OOP
{
    class Program
    {
        /*
         * 1) Инкапсуляция - это вроде некой компановки элементов по своим ящикам.
         * Когда, например, в коробке из под игрушек не должно оказаться носка или тетради,
         * а они должны были находиться на своих местах (носки в коробке с вещами, а тетрадь
         * в коробке с макулатурой)
         * 2) Инкапсуляция - это когда мы предоставляем некий набор возможностей пользователю,
         * не раскрывая ему деталей о том, как это работает
         * (Например, зачем вам нужно знать о том, как проходят механизмы пробегания тока у пульта и
         * как телевизор будет обрабатывать испускаемый инфокрасный луч, когда вы нажали на одну из кнопок
         * Вам только нужно знать, какой результат вернет эта кнопка, если вы на нее просто нажмете)
        */
        // Что не является икапсуляцией
        private class Pen
        {
            private string _socksName;
            public void RepairMachine() { }
            public Pen(string socksName)
            {
                _socksName = socksName;
            }
        }

        // Пример инкапсуляции
        private class RemoteControllerButton
        {
            private string _name;
            private int Number { get; set; }
            public RemoteControllerButton(string name, int number)
            {
                _name = name;
                Number = number;
            }
            public void Press() { }
            public int GetNumber()
            {
                return Number;
            }
        }
        private class RemoteController
        {
            private RemoteControllerButton[] RemoteControllerButtons { get; set; }
            private string Model
            {
                set
                {
                    if (value != null)
                        Model = value;
                }
                get { return Model; }
            }
            public RemoteController(RemoteControllerButton[] buttons, string model)
            {
                RemoteControllerButtons = buttons;
                Model = model;
            }
            public int GetButtonsCount()
            {
                return RemoteControllerButtons.Length;
            }
            public void PressOnButton(int buttonNumber)
            {
                foreach (var remoteControllerButton in RemoteControllerButtons)
                {
                    if (remoteControllerButton.GetNumber() == buttonNumber)
                        remoteControllerButton.Press();
                }
            }
        }


        /*
         * Наследование - это когда дочерний класс имеет доступ к свойствам и функционалу своих потомков.
         * Обычно наследование используется для дополнения своих потомков, путем добавления в дочерний
         * класс некого функционала.
        */

        // Что не является наследованием
        private class Animal
        {
            private int _age;
            public string Name { get; set; }
            public Animal(int age)
            {
                _age = age;
            }
            public void Voice() { }
        }
        private class DocherniiAnimal
        {
            private int _age;
            private int _weight;
            public string Name { get; set; }
            public DocherniiAnimal(int age, int weight)
            {
                _age = age;
                _weight = weight;
            }
            public void Voice() { }
            public void Walk() { }
        }

        // Пример наследования
        private class Subject
        {
            protected int Weight { get; private set; }
            public void SetWeight(int weight)
            {
                Weight = weight;
            }
            public void Destroy() { }
        }

        private class Paper : Subject
        {
            public Paper(int weight)
            {
                SetWeight(weight);
            }

            public int GetWeight()
            {
                return Weight;
            }
        }

        /*
         * Полиморфизм - это когда одно и то же действие базового класса (может) выполняться по разному у
         * наследуемых ему классах.
         * Для работы полиморфизма требуется наследование.
        */

        // Пример полиморфизма
        private class BaseDoings
        {
            public virtual void Open() { }
            public virtual void Close() { }
        }

        private class MSSQL : BaseDoings
        {
            public override void Open()
            {
                // Provide connection to MSSQL Server
                Console.WriteLine("Connected to MSSQL Server");
            }

            public override void Close()
            {
                // Closing connection to MSSQL Server
                Console.WriteLine("Disconnected from MSSQL Server");
            }
        }

        private class MySQL : BaseDoings
        {
            public override void Open()
            {
                // Provide connection to MSSQL Server
                Console.WriteLine("Connected to MySQL Server");
            }

            public override void Close()
            {
                // Closing connection to MSSQL Server
                Console.WriteLine("Disconnected from MySQL Server");
            }
        }

        // Классы, которые докажут, что использовать полиморфизм круто
        private class NotCoolBaseDoings
        {
            public void Open()
            {
                // Just trolling if not overrided...
                Console.WriteLine("ConnectionDone");
            }
        }
        private class NotCoolMSSQL : NotCoolBaseDoings
        {
            public void Open()
            {
                // Provide connection to MSSQL Server
                Console.WriteLine("Connected to MSSQL Server");
            }
        }

        private class NotCoolMySQL : NotCoolBaseDoings
        {
            public void Open()
            {
                // Provide connection to MSSQL Server
                Console.WriteLine("Connected to MySQL Server");
            }
        }

        static void Main()
        {
            // Пример наследования
            Subject subject = new Subject();
            subject.SetWeight(10);
            subject.Destroy();
            Paper paper = new Paper(15);
            paper.Destroy();
            paper.GetWeight();

            Subject ivisedPaper = paper;
            //ivisedPaper.GetWeight();   // Не сработает, хотя это бумага
            //Paper newPaper = new Subject(); // В обратную сторону наследование не работает

            // Пример полиморфизма
            Console.WriteLine("Полиморфизм:");
            BaseDoings[] connections = new BaseDoings[] { new MSSQL(), new MySQL() };
            for (int i = 0; i < connections.Length; i++)
                connections[i].Open();

            Console.WriteLine("Наследование без использования полиморфизма:");
            NotCoolBaseDoings[] notCoolConnections = new NotCoolBaseDoings[] { new NotCoolMSSQL(), new NotCoolMySQL() };
            for (int i = 0; i < notCoolConnections.Length; i++)
                notCoolConnections[i].Open();

            Console.WriteLine("Hello World!");
        }
    }
}

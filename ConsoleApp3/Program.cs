﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    /*

    ДЗ: Реализовать консольную игру на основе логической задачки.

    ДЗ: Реализовать наследование, инкапсуляцию и полиморфизм.

    Крестьянину нужно перевезти через реку волка, козу и капусту. 
    Но лодка такова, что в ней может поместиться только крестьянин, а с ним или один волк, или одна коза, или одна капуста. 
    Но если оставить волка с козой, то волк съест козу, а если оставить козу с капустой, то коза съест капусту. 
    Как перевез свой груз крестьянин? 

    1) Перевозим козу
    2) Перевозим капусту
    3) Возвращаем козу
    4) Перевозим волка
    5) возвращаемся за козой
    - всех перевезли.

    ----

    Pac
    + PK(ID)
    + Title
    + FK(AuthorID) ref Authors.Id

    Write
    + ID
    + Pac
    + Doc

    Doc
    + PK(ID)
    + Text
    + FK(AuthorID) ref Authors.Id

    ----
    Blogs
    + PK(ID)
    + Title
    + FK(AuthorID) ref Authors.Id

    Posts
    + PK(ID)
    + Text
    + FK(AuthorID) ref Authors.Id

    Authors
    + PK(ID)
    + Name

    */

    // Предел - консоль -.-
    public class Game
    {
        private enum Places
        {
            LeftSide,
            RightSide
        }

        private enum GameState
        {
            NotStarted,
            Started,
            Finished,
            Losted
        }

        private Places _humanStandsOn = Places.LeftSide;
        private GameState _gameState = GameState.NotStarted;

        private GameObject[] _gameObjects;
        private List<GameObject> _gameObjectsOnLeftCoast;
        private List<GameObject> _gameObjectsOnRightCoast;

        /// <summary>
        /// Старт игры, проверка компонентов игры
        /// </summary>
        public void StartTheGame()
        {
            if (_gameState == GameState.Started)
                throw new Exception("Игра уже была начата");

            if (_gameObjects == null)
                throw new Exception("Инициализируйте объекты");

            _gameObjectsOnLeftCoast = _gameObjects.ToList();
            _gameObjectsOnRightCoast = new List<GameObject>();
            _gameState = GameState.Started;

            string resultOfOperation = "Select the move object";
            while (_gameState != GameState.Finished && _gameState != GameState.Losted)
            {
                Console.Clear();
                Console.WriteLine(resultOfOperation);
                Console.WriteLine(DisplayInfo());
                Console.WriteLine("Input here name of move object or just press enter to change coast");
                resultOfOperation = new string('-', 20) + "\n" +
                                    ChoiceGameObjectForMoving(Console.ReadLine());
            }
        }

        /// <summary>
        /// Завершение игры
        /// </summary>
        public void BreakTheGame()
        {
            Console.Clear();
            if (_gameState == GameState.Losted)
                Console.WriteLine("You have losted");
            else
                Console.WriteLine("You have winned!");

            Console.WriteLine("Enter any key to leave");
            Console.ReadKey();

            //_gameObjectsOnLeftCoast = GameObjects.ToList();
            //_gameObjectsOnRightCoast = new List<GameObject>();
            //_gameState = GameState.NotStarted;
            //_humanStandsOn = Places.LeftSide;
        }


        /// <summary>
        /// Загрузка объектов этой игры по умолчанию.
        /// </summary>
        public void LoadDefaultGameObjects()
        {
            Cabbage cabbage = new Cabbage();
            Goat goat = new Goat();
            goat.WriteGameObjectThatCanEat(cabbage);
            Wolf wolf = new Wolf();
            wolf.WriteGameObjectThatCanEat(goat);

            _gameObjects = new GameObject[] { cabbage, goat, wolf };
        }

        /// <summary>
        /// Загрузка игровых объектов, если игра не была начата
        /// Не понятно, что делать, если загружаются несколько объектов одного и того же класса...
        /// </summary>
        /// <param name="gameObjects"></param>
        public void LoadGameObjects(GameObject[] gameObjects)
        {
            if (_gameState != GameState.Started)
            {
                if (gameObjects == null || gameObjects.Length == 0)
                    throw new Exception("Пустой массив");

                if (gameObjects.Count(x => IfCorrectGameObjectGetHisTypeName(x) == null) > 0)
                    throw new Exception("Имеются непредусмотренные классы");

                var max = gameObjects.Max(x =>
                    gameObjects.Count(y =>
                        IfCorrectGameObjectGetHisTypeName(y) == IfCorrectGameObjectGetHisTypeName(x))
                    );

                if (max > 1)
                    throw new Exception("Имеются повторяющиеся типы");

                _gameObjects = gameObjects;
            }
            else
            {
                throw new Exception("Игра уже идет...");
            }
        }

        private string IfCorrectGameObjectGetHisTypeName(GameObject gameObject)
        {
            if (gameObject is Wolf)
                return typeof(Wolf).Name;
            if (gameObject is Cabbage)
                return typeof(Cabbage).Name;
            if (gameObject is Goat)
                return typeof(Goat).Name;
            return null;
        }

        /// <summary>
        /// Отображение информации об игре
        /// </summary>
        /// <returns></returns>
        private string DisplayInfo()
        {
            if (_gameObjectsOnLeftCoast == null && _gameObjectsOnRightCoast == null)
            {
                return "Nothing to show there";
            }

            string mapInformation = String.Join('\n', new[]
            {
                new string('-', 20),
                "Left coast:",
                GetTypeNamesOfGameObjectsArray(_gameObjectsOnLeftCoast.ToArray()),
                "\n",
                "Right coast:",
                GetTypeNamesOfGameObjectsArray(_gameObjectsOnRightCoast.ToArray()),
                "\n",
                $"The human stands on: {GetCurrentPlace()}",
                new string('-', 20)
            });
            return mapInformation;
        }

        private string GetCurrentPlace()
        {
            return _humanStandsOn switch
            {
                Places.LeftSide => "Left side",
                Places.RightSide => "Right side",
                _ => "Error"
            };
        }

        /// <summary>
        /// Вывод названия всех игровых объектов, находящихся в массиве
        /// </summary>
        /// <param name="gameObjects"></param>
        /// <returns></returns>
        private string GetTypeNamesOfGameObjectsArray(GameObject[] gameObjects)
        {
            if (gameObjects == null)
                return null;

            if (gameObjects.Length > 0)
            {
                List<string> result = new List<string>();
                foreach (var gameObject in gameObjects)
                {
                    result.Add(gameObject.GetType().Name);
                }
                return string.Join(',', result);
            }
            return null;
        }


        /// <summary>
        /// Проверка поведения объектов на противоположном берегу.
        /// </summary>
        private void CheckOppositeCoast()
        {
            switch (_humanStandsOn)
            {
                case Places.LeftSide:
                    foreach (var gameObject in _gameObjectsOnRightCoast)
                    {
                        if (gameObject.CanItEatOneOfThem(_gameObjectsOnRightCoast.ToArray()) == true)
                        {
                            _gameState = GameState.Losted;
                            BreakTheGame();
                        }
                    }
                    break;
                case Places.RightSide:
                    foreach (var gameObject in _gameObjectsOnLeftCoast)
                    {
                        if (gameObject.CanItEatOneOfThem(_gameObjectsOnLeftCoast.ToArray()) == true)
                        {
                            _gameState = GameState.Losted;
                            BreakTheGame();
                        }
                    }
                    break;
            }

            if (_gameObjectsOnLeftCoast.Count == 0)
            {
                _gameState = GameState.Finished;
                BreakTheGame();
            }
        }

        /// <summary>
        /// Выбор игрового объекта для перемещения по его наименованию.
        /// </summary>
        /// <param name="gameObjectTypeName"></param>
        /// <returns></returns>
        private string ChoiceGameObjectForMoving(string gameObjectTypeName)
        {
            if (String.IsNullOrEmpty(gameObjectTypeName))
            {
                MoveToOtherPlace();
                CheckOppositeCoast();
                return "Changed place";
            }
            foreach (var gameObject in _gameObjects)
            {
                if (gameObject.GetType().Name.ToLower() == gameObjectTypeName.ToLower())
                {
                    string moveResult = MoveToOtherPlace(gameObject);
                    if (moveResult != null)
                        return moveResult;

                    CheckOppositeCoast();
                    return $"Changed place and moved {gameObjectTypeName}";
                }
            }
            return "That name of object was not found";
        }

        /// <summary>
        /// Перемещение на другое место. Перемещение объекта опционально. 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        private string MoveToOtherPlace(GameObject gameObject = null)
        {
            switch (_humanStandsOn)
            {
                case Places.LeftSide:
                    if (gameObject != null)
                    {
                        if (_gameObjectsOnLeftCoast
                            .Exists(x => x.GetType().Name == gameObject.GetType().Name) == false)
                            return "This object was not found on left coast";

                        _gameObjectsOnLeftCoast.Remove(gameObject);
                        _gameObjectsOnRightCoast.Add(gameObject);
                    }
                    _humanStandsOn = Places.RightSide;
                    break;

                case Places.RightSide:
                    if (gameObject != null)
                    {
                        if (_gameObjectsOnRightCoast
                            .Exists(x => x.GetType().Name == gameObject.GetType().Name) == false)
                            return "This object was not found on right coast";

                        _gameObjectsOnRightCoast.Remove(gameObject);
                        _gameObjectsOnLeftCoast.Add(gameObject);
                    }
                    _humanStandsOn = Places.LeftSide;
                    break;

                default:
                    throw new Exception("Человек находится на непредусмотренном месте");
            }
            return null;
        }
    }

    public abstract class GameObject
    {
        private GameObject[] _whatCanEat { get; set; }

        private bool CanIBeEatenByMyself(GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.GetType() == this.GetType())
                    return true;
            }
            return false;
        }
        public void WriteGameObjectThatCanEat(GameObject gameObject)
        {
            GameObject[] gameObjects = new GameObject[] { gameObject };

            if (CanIBeEatenByMyself(gameObjects) == true)
                throw new Exception("Объект не может съесть самого себя");

            _whatCanEat = gameObjects;
        }
        public void WriteGameObjectsThatCanEat(GameObject[] gameObjects)
        {
            if (CanIBeEatenByMyself(gameObjects) == true)
                throw new Exception("Объект не может съесть самого себя");

            _whatCanEat = gameObjects;
        }
        public bool CanItEatOneOfThem(GameObject[] gameObjects)
        {
            if (_whatCanEat == null || _whatCanEat.Length == 0)
                return false;

            foreach (var canEatGameObject in _whatCanEat)
            {
                foreach (var gameObject in gameObjects)
                {
                    if (canEatGameObject.GetType().Name == gameObject.GetType().Name)
                        return true;
                }
            }

            return false;
        }
    }

    public class Wolf : GameObject
    {
    }

    public class Goat : GameObject
    {
    }

    public class Cabbage : GameObject
    {
    }

    class Program
    {
        static void Main()
        {
            // Список левого берега
            // Список правого берега
            // Добавляем в левый берег объекты
            // Старт игры
            // Выбираем объект для перевозки
            // Помещаем его на лодку
            // Сравниваем все объекты на берегах
            // Если нет проблем, то перемещаем
            // Иначе игра завершается

            Cabbage cabbage = new Cabbage();

            Goat goat = new Goat();
            goat.WriteGameObjectThatCanEat(cabbage);

            Wolf wolf = new Wolf();
            wolf.WriteGameObjectThatCanEat(goat);

            GameObject[] gameObjects = new GameObject[] { cabbage, goat, wolf };
            Game game = new Game();
            game.LoadGameObjects(gameObjects);
            game.StartTheGame();
        }
    }
}

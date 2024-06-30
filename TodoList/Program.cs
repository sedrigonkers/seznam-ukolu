using System;
using System.Collections.Generic;

namespace TodoList
{
    class Program
    {
        public class TextHighlight
        {
            public void Color(ConsoleColor color, string text)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(text);
                Console.ResetColor();
            }

            public void Highlight(string text)
            {
                Console.WriteLine("* " + text + " *");
            }
        }

        public class Todo
        {

            public bool fullfield;
            public string text;


            public Todo(string text)
            {
                this.text = text;
                this.fullfield = false;
            }

            public void SwitchFullfield()
            {
                fullfield = !fullfield;
            }

            public string GetText()
            {
                if (fullfield)
                {
                    return "\x2705 " + text;

                }
                else return "\x2610 " + text;
            }

            public bool IsFullfield()
            {
                return fullfield;
            }

        }



        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            TextHighlight textHighlight = new TextHighlight();


            string[] options = {"exit", "view my todos", "add todo", "remove from todo list"}; /// список опций
            List<Todo> todoListItems = new List<Todo>(); /// список задач

            
            todoListItems.Add(new Todo("покормить кота"));


            while (true)
            {

                textHighlight.Color(ConsoleColor.Yellow, 
                    "Welcome to the TODO list!\n" +
                    "Here are the oprions you can select:\n"
                    );


                /// Перечисление доступных опций
                for (int i = 0; i < options.Length; i++) {
                    Console.WriteLine("{0} - {1}", i, options[i]);
                };


                /// Спрашиваем какую опцию выберет пользователь
                string userInput = Console.ReadLine();
                int optionId;

                bool optionParseResult = int.TryParse(userInput, out optionId);


                /// Если введена несуществующая опция даём знать пользователю и возвращаемся в цикл
                if (!optionParseResult)
                {
                    Console.Clear();
                    textHighlight.Color(ConsoleColor.Red, "Wrong option!");
                    continue;
                }

                int currentTodoListItem = 0;

                /// Главный СВИТЧ опций, проверяет по переменной optionId (int)
                switch (optionId)
                {

                    /// просмотр существующих опций
                    case 1:
                        bool exit = false;
                        while (!exit)
                        {
                            Console.Clear();


                            if (currentTodoListItem >= todoListItems.Count) { currentTodoListItem = 0; }; /// Если индекс выбранного элемента не соотносится с количеством элементов то он сбрасывается
                            if (currentTodoListItem < 0) { currentTodoListItem = todoListItems.Count - 1; }; /// Если индекс уходит в минус мы назначаем его на последний элемент

                            for (int i = 0; i < todoListItems.Count; i++)
                            {
                                if (i == currentTodoListItem)
                                {
                                    Console.BackgroundColor = ConsoleColor.Yellow;
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.WriteLine(todoListItems[i].GetText());
                                    Console.ResetColor();
                                } else {
                                    Console.WriteLine(todoListItems[i].GetText());
                                }
                            };

                            textHighlight.Color(ConsoleColor.Yellow, "\nЧтобы выйти в главное меню нажмите 0");

                            switch (Console.ReadKey().Key)
                            {
                                case ConsoleKey.G:
                                    exit = true;
                                    break;

                                case ConsoleKey.DownArrow:
                                    currentTodoListItem++;
                                    continue;

                                case ConsoleKey.UpArrow:
                                    currentTodoListItem--;
                                    continue;

                                case ConsoleKey.Enter:
                                    todoListItems[currentTodoListItem].SwitchFullfield();
                                    continue;

                                case ConsoleKey.Delete:
                                    if (todoListItems.Count > 0)
                                    {
                                        todoListItems.RemoveAt(currentTodoListItem);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    continue;

                                default: 
                                    break;
                            }

                        
                        }
                        break;



                    case 2:
                        Console.Clear();

                        textHighlight.Color(ConsoleColor.Yellow, "Write your TODO\n");

                        while (true)
                        {
                            

                            string newTodoListText = Console.ReadLine();
                            if (newTodoListText.Length == 0)
                            {
                                textHighlight.Color(ConsoleColor.Yellow, "Your TODO Can't be void");

                                continue;
                            }

                            todoListItems.Add(new Todo(newTodoListText));
                            break;

                        }
                        break;


                    /// выход
                    case 0:
                        textHighlight.Color(ConsoleColor.Red, "Thanks for using the TODO list. Goodbye!");
                        return;


                    default:
                        break;

                        
                }

                Console.Clear();
                continue;
            }
            

        }
    }
}

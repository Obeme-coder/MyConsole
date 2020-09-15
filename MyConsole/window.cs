using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace MyConsole
{
    public interface IWindow 
    {
        string title { get; set; }
        void Start();
        void Exit();
        void Print();
        void Interaction(ConsoleKeyInfo key);
    }
    /////////////Menu////////////////
    public class Menu : IWindow
    {
        public int cursor { get; set; } = 0;
        public string title { get; set; }
        public ILine[] lines { get; set; }
        bool withExit = false;
        public Func<Menu, bool> onExit = (x) => { return true; };
        public Menu(string _title, params ILine[] _lines)
        {
            title = _title;
            lines = _lines.Concat(new ILine[] { new Exit() }).ToArray();
        }
        public Menu(string _title, bool withExit, params ILine[] _lines)
        {
            title = _title;
            this.withExit = withExit;
            if (withExit)
            {
                lines = _lines.Concat(new ILine[] { new Exit() }).ToArray();
            }
            else
            {
                lines = _lines;
            }
        }
        public void Start()
        {
            Print();
            while (true)
            {
                Interaction(Console.ReadKey(true));
                Print();
            }
        }
        public void Print()
        {
            Console.Clear();
            Console.WriteLine(title.ToUpper() + "\n");
            for (int i = 0; i < lines.Length; i++)
            {
                if (false)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                lines[i].Title(cursor == i);
                Console.ResetColor();
            }
        }
        public void Interaction(ConsoleKeyInfo key)
        {
            if (lines[cursor].Interaction(key))
            {
                return;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                cursor = Math.Min(lines.Length - 1, cursor + 1);
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                cursor = Math.Max(0, cursor - 1);
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                if (withExit)
                {
                    App.Back();
                }
            }
        }
        public void Exit()
        {
            onExit(this);
        }
    }
    ////////////Stream//////////////
    //class Stream : IWindow
    //{
    //    public int cursor { get; set; }
    //    public string title { get; set; }
    //    public string path;
    //    Mutex mtx;
    //    bool play;
    //    public Stream(string _title, string _path)
    //    {
    //        title = _title;
    //        path = _path;
    //        mtx = new Mutex(false, @"Global\" + path.GetHashCode());
    //    }
    //    public void Print()
    //    {
    //        Thread th = new Thread(Escape_check);
    //        th.Start();
    //        play = true;
    //        while (play)
    //        {
    //            mtx.WaitOne();
    //            string x = File.ReadAllText(path);
    //            mtx.ReleaseMutex();
    //            Console.Clear();
    //            Console.ResetColor();
    //            Console.WriteLine(title.ToUpper() + "\n");
    //            Console.WriteLine();
    //            Console.WriteLine(x);
    //            Thread.Sleep((int)App.settings["update frequency"]);
    //        }
    //    }
    //    public void Interaction(ConsoleKeyInfo key)
    //    {
    //    }
    //    public void Escape_check()
    //    {
    //        while (true)
    //        {
    //            if (Console.ReadKey(false).Key == ConsoleKey.Escape)
    //            {
    //                Console.Beep();
    //                play = false;
    //                App.path.Pop();
    //                return;
    //            }
    //        }
    //    }
    //}
    ////////////Settings//////////////
}

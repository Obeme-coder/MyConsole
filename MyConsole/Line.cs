using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsole
{
    public interface ILine
    {
        string title { get; set; }
        void Title(bool underCursor);
        bool Interaction(ConsoleKeyInfo key);
    }
    ////////////Button////////////
    public class Button : ILine
    {
        public string title { get; set; }
        public Action action;
        public Button(string _title, Action action)
        {
            title = _title;
            this.action = action;
        }
        public void Title(bool underCursor)
        {
            if (underCursor)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.WriteLine(title);
        }
        public bool Interaction(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Enter)
            {
                action.Invoke();
            }
            else
            {
                return false;
            }
            return true;
        }
    }
    ////////////Link//////////////
    public class Link : ILine
    {
        public string title { get; set; }
        public IWindow link;
        public Link(string _title, IWindow _link)
        {
            title = _title;
            link = _link;
        }
        public void Title(bool underCursor)
        {
            if (underCursor)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.WriteLine(title);
        }
        public bool Interaction(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Enter)
            {
                App.Forward(link);
            }
            else
            {
                return false;
            }
            return true;
        }
    }
    ////////////Sign//////////////
    public class Sign : ILine
    {
        public string title { get; set; }
        public Sign(string _title)
        {
            title = _title;
        }
        public void Title(bool underCursor)
        {
            if (underCursor)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.WriteLine(title);
        }
        public bool Interaction(ConsoleKeyInfo key)
        {
            return false;
        }
    }
    ////////////Exit//////////////
    public class Exit : ILine
    {
        public string title { get; set; } = "back";
        public Exit()
        {

        }
        public void Title(bool underCursor)
        {
            if (underCursor)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            if (App.path.Count == 1)
            {
                Console.WriteLine("exit");
            }
            else
            {
                Console.WriteLine(title);
            }
        }
        public bool Interaction(ConsoleKeyInfo key)
        {
            if(key.Key == ConsoleKey.Enter)
            {
                App.Back();
            }
            else
            {
                return false;
            }
            return true;
        }
    }
    ////////////RangeFloat//////////////
    public class RangeFloat : ILine
    {
        public string title { get; set; }
        public Dictionary<string, float> sourse;
        public string key;
        public float step;
        public float min;
        public float max;
        public RangeFloat(string _title, Dictionary<string, float> sourse, string key, float value, float _step, float _min, float _max)
        {
            title = _title;
            this.sourse = sourse;
            this.key = key;
            sourse[key] = value;
            step = _step;
            min = _min;
            max = _max;
        }
        public void Title(bool underCursor)
        {
            if (underCursor)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.WriteLine(title + (sourse[key] - step < min? "  ":" <") + sourse[key].ToString() + (sourse[key] + step > max ? " " : ">"));
        }
        public bool Interaction(ConsoleKeyInfo key)
        {
            if(key.Key == ConsoleKey.LeftArrow)
            {
                sourse[this.key] = Math.Max(min, sourse[this.key] - step);
            }
            else if (key.Key == ConsoleKey.RightArrow)
            {
                sourse[this.key] = Math.Min(max, sourse[this.key] + step);
            }
            else
            {
                return false;
            }
            return true;
        }
    }
    ////////////RangeBool//////////////
    public class RangeBool : ILine
    {
        public string title { get; set; }
        public Dictionary<string, bool> sourse;
        public string key;
        public RangeBool(string _title, Dictionary<string, bool> sourse, string key, bool value)
        {
            title = _title;
            this.key = key;
            this.sourse = sourse;
            sourse[key] = value;
        }
        public void Title(bool underCursor)
        {
            if (underCursor)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.WriteLine(title + " <" + sourse[key].ToString() + ">");
        }
        public bool Interaction(ConsoleKeyInfo key)
        {
            if(key.Key == ConsoleKey.LeftArrow)
            {
                sourse[this.key] = !sourse[this.key];
            }
            else if (key.Key == ConsoleKey.RightArrow)
            {
                sourse[this.key] = !sourse[this.key];
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}

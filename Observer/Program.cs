using System;
using System.Threading.Tasks;

namespace Observer
{
    class Program
    {
        static void Main(string[] args)
        {
            Init init = new Init("http://localhost:58957");
            init.Start();
        }
    }
}

using Microsoft.AspNetCore.Components.Forms;
using System.Runtime.CompilerServices;

namespace MyBGList.Logging;

public class LogEvents
{
    protected class Category
    {
        public const int Controllers = 1_0000_0;
    }

    protected class Method
    {
        public const int Get = 1;
        public const int Post = 2;
        public const int Put = 3;
        public const int Delete = 4;
    }

    public class Controller
    {
        public class BoardGames
        {
            private const int ThisController = Category.Controllers + 001_0;

            public const int Get = ThisController + Method.Get;
            public const int Post = ThisController + Method.Post;
            public const int Put = ThisController + Method.Put;
            public const int Delete = ThisController + Method.Delete;
        }

        public class Domains
        {
            private const int ThisController = Category.Controllers + 002_0;

            public const int Get = ThisController + Method.Get;
            public const int Post = ThisController + Method.Post;
            public const int Put = ThisController + Method.Put;
            public const int Delete = ThisController + Method.Delete;
        }

        public class Mechanics
        {
            private const int ThisController = Category.Controllers + 003_0;

            public const int Get = ThisController + Method.Get;
            public const int Post = ThisController + Method.Post;
            public const int Put = ThisController + Method.Put;
            public const int Delete = ThisController + Method.Delete;
        }

        public class Seed
        {
            private const int ThisController = Category.Controllers + 004_0;

            public const int Get = ThisController + Method.Get;
            public const int Post = ThisController + Method.Post;
            public const int Put = ThisController + Method.Put;
            public const int Delete = ThisController + Method.Delete;
        }
    }
}
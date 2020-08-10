
    //public delegate bool PredicateCommand(string item);
   // public delegate TRetunType PredicateCommand<TSource,TRetunType>(TSource item);
    class Program
    {
      public   static bool CheckStringLengthgreaterThanFour(string item)
        {
            return item.Length > 4;
        }
        public static bool CheckNumberType(int item)
        {
            return item % 2 == 0;
        }
        static void Main(string[] args)
        {
            int[] numbers = { 45, 6, 7, 8, 9, 900, 788 };

            string[] names = { "Philips", "PIC", "Siemens", "Bosch", "Biny" };
            //Func<string,bool> _command = 
            //    new Func<string,bool>(Program.CheckStringLengthgreaterThanFour);
            //Function Closure
            Func<string, bool> _command = (string item) => { return item.Length > 4; };

            Fun_____(names,_command);

            Func<int,bool> _intPredicateCommand = new Func<int,bool>(Program.CheckNumberType);
            Fun_____(numbers, _intPredicateCommand);
        }

        //LINQ - Query Function Reuse (independnect of data types and Collections)

            //Parametric Polymorphism - Generics
            //Iterator Pattern 
            //Command Pattern to parameterize predicate/Logic 
        
           //Where Function Imitation
        static IEnumerable<T> Fun_____<T>(IEnumerable<T> source,Func<T,bool> predicate)
        {
            List<T> resultList = new List<T>();
            //select all the strings from names where length > 4
            foreach(T item in source)
            { 
                if (predicate.Invoke(item))
                {
                    resultList.Add(item);
                }
            }
            return resultList;

        }

    }

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace SimpleCalculator3
{

    public interface ICalculator
    {
        String Calculate(String input);
    }

    public interface IOperation
    {
        int Operate(int left, int right);
    }

    public interface IOperationData
    {
        Char Symbol { get; }
        //bool IsExploding { get; }
    }

    [Export(typeof(IOperation)),
        ExportMetadata("Symbol", '+')]//can specify multiple attributes within a single sq bracket pair
    class Add : IOperation
    {
        public int Operate(int left, int right)
        {
            return left + right;
        }
    }

    [Export(typeof(IOperation))]
    [ExportMetadata("Symbol", '-')]
    class Subtract : IOperation
    {

        public int Operate(int left, int right)
        {
            return left - right;
        }

    }



    [Export(typeof(ICalculator))]
    class MySimpleCalculator : ICalculator
    {
        [ImportMany(AllowRecomposition = true)]
        IEnumerable<Lazy<IOperation, IOperationData>> operations;

        public String Calculate(String input)
        {
            int left;
            int right;
            Char operation;
            int fn = FindFirstNonDigit(input); //finds the operator
            if (fn < 0) return "Could not parse command.";

            try
            {
                //separate out the operands
                left = int.Parse(input.Substring(0, fn));
                right = int.Parse(input.Substring(fn + 1));
            }
            catch
            {
                return "Could not parse command.";
            }

            operation = input[fn];

            foreach (Lazy<IOperation, IOperationData> i in operations)
            {
                if (i.Metadata.Symbol.Equals(operation)) 
                    return i.Value.Operate(left, right).ToString();
            }
            throw new InvalidOperationException();
            //return "Operation Not Found!";
        }

        private int FindFirstNonDigit(String s)
        {

            for (int i = 0; i < s.Length; i++)
            {
                if (!(Char.IsDigit(s[i]))) return i;
            }
            return -1;
        }


    }


    class Program
    {
        private CompositionContainer _container;

        [Import(typeof(ICalculator))]
        public ICalculator calculator;

        private DirectoryCatalog dCat;

        private Program()
        {
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Program class
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
            //catalog.Catalogs.Add(new DirectoryCatalog("C:\\Users\\SomeUser\\Documents\\Visual Studio 2010\\Projects\\SimpleCalculator3\\SimpleCalculator3\\Extensions"));
            this.dCat = new DirectoryCatalog(".\\Extensions");
            this.dCat.Changed += dCat_Changed;
            catalog.Catalogs.Add(dCat);

            //Create the CompositionContainer with the parts in the catalog
            _container = new CompositionContainer(catalog);

            //Fill the imports of this object
            try
            {
                this._container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
        }

        void dCat_Changed(object sender, ComposablePartCatalogChangeEventArgs e)
        {
            Console.WriteLine("Directory catalog changed");
        }

        public void Recompose()
        {
            this.dCat.Refresh();
            this._container.ComposeParts(this);
        }



        static void Main(string[] args)
        {
            Program p = new Program(); //Composition is performed in the constructor
            String s;
            Console.WriteLine("Enter Command:");
            while (true)
            {
                s = Console.ReadLine();
                try
                {
                    Console.WriteLine(p.calculator.Calculate(s));
                }
                catch (InvalidOperationException exOp)
                {
                    p.Recompose();
                    Console.WriteLine("Op not found. Container has been recomposed. Try again.");
                }
            }
            

        }
    }
}

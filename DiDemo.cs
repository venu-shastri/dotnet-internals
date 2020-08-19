using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiDemo
{
    public class Car {
        private CRDIEngine _engine;
        Wheel frontWeel, rearWheel;
        MonoBody body;
        MannualGearBox gearBox;

        public Car(CRDIEngine engine,Wheel frontWeel,Wheel rearWheel, MonoBody body, MannualGearBox gearBox) {

            this._engine = engine;
            this.frontWeel = frontWeel;
            this.rearWheel = rearWheel;
            this.body = body;
            this.gearBox = gearBox;

        }
    }
    public class CRDIEngine { 
    
    public CRDIEngine(Starter starter,FuelPump fuelPump, Ignition ignition)
        {

        }
    }
    public class Wheel {
    
        public Wheel(Rim rim,MrfTyre type) { }
    }
    public class Rim { }
    public class MrfTyre { }
    public class MannualGearBox { }
    public class MonoBody { }
    public class Starter { }
    public class FuelPump { }
    public class Ignition { }

    
    

    
    class Program
    {
        static void Main(string[] args)
        {
            //Driver 
            //Injection By Hand - limits software extensibility
            //Car car = new Car(
            //    new CRDIEngine(
            //        new Starter(), 
            //        new FuelPump(),
            //        new Ignition()), 
            //    new Wheel(
            //        new Rim(),
            //        new MrfTyre ()), 
            //    new Wheel(
            //        new Rim(),
            //        new MrfTyre()),
            //    new MonoBody(),
            //    new MannualGearBox());
           Car obj =depencyResolver.GetService(typeof(Car));//AutoWiring


        }
    }
}

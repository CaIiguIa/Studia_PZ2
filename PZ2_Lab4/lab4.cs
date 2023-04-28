namespace PZ2_Lab4;

class Lab4
{
    public static void Main(string[] args)
    {
        PrepareEmployees();
        PrepareTerritories();
        PrepareEmployeeTerritories();
        PrepareRegions();
        PrepareOrders();
        PrepareOrderDetails();

        Console.WriteLine("\n\n### 2: Nazwiska pracowników ###\n");
        {
            var r2 = from e in Employees.instances select new { nazwisko = e.lastname };
            foreach (var text in (r2.ToList()))
            {
                Console.WriteLine(text.nazwisko);
            }
        }


        Console.WriteLine("\n\n### 3: Regiony i terytoria pracowników ###\n");
        {
            var r3 = from e in Employees.instances
                join et in EmployeeTerritories.instances on e.employeeid equals et.employeeid
                join t in Territories.instances on et.territoryid equals t.territoryid
                join r in Regions.instances on t.regionid equals r.regionid
                select new { nazwisko = e.lastname, region = r.regiondescription, terytorium = t.territorydescription };
            foreach (var text in (r3.ToList()))
            {
                Console.WriteLine(text.nazwisko + " " + text.region + " " + text.terytorium);
            }
        }

        Console.WriteLine("\n\n### 4: Nazwiska pracowników zgrupowane po regionie ###\n");
        {
            var r4 = (from g in (from e in Employees.instances
                    join et in EmployeeTerritories.instances on e.employeeid equals et.employeeid
                    join t in Territories.instances on et.territoryid equals t.territoryid
                    join r in Regions.instances on t.regionid equals r.regionid
                    select new { nazwisko = e.lastname, region = r.regiondescription }).Distinct()
                group g by g.region
                into grupa
                select (grupa));
            foreach (var text in (r4.ToList()))
            {
                Console.WriteLine("{0}:", text.Key);
                foreach (var pr in text)
                {
                    Console.WriteLine("     {0}", pr.nazwisko);
                }
            }
        }


        Console.WriteLine("\n\n### 5: Liczba pracowników w regionach ###\n");
        {
            var r5 = (from g in (from e in Employees.instances
                    join et in EmployeeTerritories.instances on e.employeeid equals et.employeeid
                    join t in Territories.instances on et.territoryid equals t.territoryid
                    join r in Regions.instances on t.regionid equals r.regionid
                    select new { nazwisko = e.lastname, region = r.regiondescription }).Distinct()
                group g by g.region
                into grupa
                select new
                {
                    nazwa = grupa.Key,
                    ile = grupa.Count()
                });
            foreach (var text in (r5.ToList()))
            {
                Console.WriteLine(text.nazwa + ":  " + text.ile);
            }
        }

        Console.WriteLine("\n\n### 6: Statystyki pracowników ###\n");
        var r6 = (from g in (from e in Employees.instances
                join o in Order.instances on e.employeeid equals o.employeeid
                join od in OrderDetails.instances on o.orderid equals od.orderid
                select new
                {
                    nazwisko = e.lastname, o.orderid,
                    cena = Double.Parse(od.unitprice.Replace(".", ",")) * Double.Parse(od.quantity.Replace(".", ",")) *
                           (1 - Double.Parse(od.discount.Replace(".", ",")))
                })
            group g by g.nazwisko
            into grupa
            select new
            {
                nazwa = grupa.Key,
                ile = grupa.Count(),
                avg = grupa.Average(x => x.cena),
                max = grupa.Max(x => x.cena)
            });
        foreach (var text in (r6.ToList()))
        {
            Console.WriteLine(text.nazwa + ":  ");
            Console.WriteLine("    Ilość zamówień: " + text.ile);
            Console.WriteLine("    Średnia wartość zamówień: " + text.avg);
            Console.WriteLine("    Maksymalna wartość zamówienia: " + text.max + "\n");
        }
    }

    public static List<List<String>> regions;
    public static List<List<String>> territories;
    public static List<List<String>> employeeTerritories;
    public static List<List<String>> employees;
    public static List<List<String>> orders;
    public static List<List<String>> orderDetails;

    class OrderDetails
    {
        public String orderid { get; set; }
        public String productid { get; set; }
        public String unitprice { get; set; }
        public String quantity { get; set; }
        public String discount { get; set; }

        public static List<OrderDetails> instances;

        public OrderDetails(string orderid, string productid, string unitprice, string quantity, string discount)
        {
            this.orderid = orderid;
            this.productid = productid;
            this.unitprice = unitprice;
            this.quantity = quantity;
            this.discount = discount;
        }

        public override string ToString()
        {
            return
                $"{nameof(orderid)}: {orderid}, {nameof(productid)}: {productid}, {nameof(unitprice)}: {unitprice}, {nameof(quantity)}: {quantity}, {nameof(discount)}: {discount}";
        }
    }

    class Order
    {
        public String orderid { get; set; }
        public String customerid { get; set; }
        public String employeeid { get; set; }
        public String orderdate { get; set; }
        public String requireddate { get; set; }
        public String shippeddate { get; set; }
        public String shipvia { get; set; }
        public String freight { get; set; }
        public String shipname { get; set; }
        public String shipaddress { get; set; }
        public String shipcity { get; set; }
        public String shipregion { get; set; }
        public String shippostalcode { get; set; }
        public String shipcountry { get; set; }
        public static List<Order> instances;

        public Order(string orderid, string customerid, string employeeid, string orderdate, string requireddate,
            string shippeddate, string shipvia, string freight, string shipname, string shipaddress, string shipcity,
            string shipregion, string shippostalcode, string shipcountry)
        {
            this.orderid = orderid;
            this.customerid = customerid;
            this.employeeid = employeeid;
            this.orderdate = orderdate;
            this.requireddate = requireddate;
            this.shippeddate = shippeddate;
            this.shipvia = shipvia;
            this.freight = freight;
            this.shipname = shipname;
            this.shipaddress = shipaddress;
            this.shipcity = shipcity;
            this.shipregion = shipregion;
            this.shippostalcode = shippostalcode;
            this.shipcountry = shipcountry;
        }

        public override string ToString()
        {
            return
                $"{nameof(orderid)}: {orderid}, {nameof(customerid)}: {customerid}, {nameof(employeeid)}: {employeeid}, {nameof(orderdate)}: {orderdate}, {nameof(requireddate)}: {requireddate}, {nameof(shippeddate)}: {shippeddate}, {nameof(shipvia)}: {shipvia}, {nameof(freight)}: {freight}, {nameof(shipname)}: {shipname}, {nameof(shipaddress)}: {shipaddress}, {nameof(shipcity)}: {shipcity}, {nameof(shipregion)}: {shipregion}, {nameof(shippostalcode)}: {shippostalcode}, {nameof(shipcountry)}: {shipcountry}";
        }
    }

    class Regions
    {
        public String regionid { get; set; }
        public String regiondescription { get; set; }
        public static List<Regions> instances;

        public Regions(string regionid, string regiondescription)
        {
            this.regionid = regionid;
            this.regiondescription = regiondescription;
        }

        public override string ToString()
        {
            return $"{nameof(regionid)}: {regionid}, {nameof(regiondescription)}: {regiondescription}";
        }
    }

    class Territories
    {
        public String territoryid { get; set; }
        public String territorydescription { get; set; }
        public String regionid { get; set; }
        public static List<Territories> instances;

        public Territories(string territoryid, string territorydescription, string regionid)
        {
            this.territoryid = territoryid;
            this.territorydescription = territorydescription;
            this.regionid = regionid;
        }

        public override string ToString()
        {
            return
                $"{nameof(territoryid)}: {territoryid}, {nameof(territorydescription)}: {territorydescription}, {nameof(regionid)}: {regionid}";
        }
    }

    class EmployeeTerritories
    {
        public String employeeid { get; set; }
        public String territoryid { get; set; }
        public static List<EmployeeTerritories> instances;

        public EmployeeTerritories(string employeeid, string territoryid)
        {
            this.employeeid = employeeid;
            this.territoryid = territoryid;
        }

        public override string ToString()
        {
            return $"{nameof(employeeid)}: {employeeid}, {nameof(territoryid)}: {territoryid}";
        }
    }

    class Employees
    {
        public String employeeid { get; set; }
        public String lastname { get; set; }
        public String firstname { get; set; }
        public String title { get; set; }
        public String titleofcourtesy { get; set; }
        public String birthdate { get; set; }
        public String hiredate { get; set; }
        public String address { get; set; }
        public String city { get; set; }
        public String region { get; set; }
        public String postalcode { get; set; }
        public String country { get; set; }
        public String homephone { get; set; }
        public String extension { get; set; }
        public String photo { get; set; }
        public String notes { get; set; }
        public String reportsto { get; set; }
        public String photopath { get; set; }
        public static List<Employees> instances;

        public Employees(string employeeid, string lastname, string firstname, string title, string titleofcourtesy,
            string birthdate, string hiredate, string address, string city, string region, string postalcode,
            string country, string homephone, string extension, string photo, string notes, string reportsto,
            string photopath)
        {
            this.employeeid = employeeid;
            this.lastname = lastname;
            this.firstname = firstname;
            this.title = title;
            this.titleofcourtesy = titleofcourtesy;
            this.birthdate = birthdate;
            this.hiredate = hiredate;
            this.address = address;
            this.city = city;
            this.region = region;
            this.postalcode = postalcode;
            this.country = country;
            this.homephone = homephone;
            this.extension = extension;
            this.photo = photo;
            this.notes = notes;
            this.reportsto = reportsto;
            this.photopath = photopath;
        }

        public override string ToString()
        {
            return
                $"{nameof(employeeid)}: {employeeid}, {nameof(lastname)}: {lastname}, {nameof(firstname)}: {firstname}, {nameof(title)}: {title}, {nameof(titleofcourtesy)}: {titleofcourtesy}, {nameof(birthdate)}: {birthdate}, {nameof(hiredate)}: {hiredate}, {nameof(address)}: {address}, {nameof(city)}: {city}, {nameof(region)}: {region}, {nameof(postalcode)}: {postalcode}, {nameof(country)}: {country}, {nameof(homephone)}: {homephone}, {nameof(extension)}: {extension}, {nameof(photo)}: {photo}, {nameof(notes)}: {notes}, {nameof(reportsto)}: {reportsto}, {nameof(photopath)}: {photopath}";
        }
    }

    public static void print2DList(List<List<String>> lista)
    {
        foreach (var ll in lista)
        {
            foreach (var text in ll)
            {
                Console.Write(text + ", ");
            }

            Console.WriteLine();
        }
    }

    public static void PrepareEmployees()
    {
        using (var reader =
               new StreamReader(
                   @"C:\Users\sdjur\Documents\Programowanie\Rider\C#\Projekt1\PierwszyProjekt\PZ2_Lab4\employees.csv"))
        {
            employees = new List<List<String>>();
            while (!reader.EndOfStream)
            {
                List<string> list = new List<string>();

                var line = reader.ReadLine();
                var values = line.Split(',');

                foreach (var text in values)
                {
                    list.Add(text);
                }

                employees.Add(list);
            }
        }

        Employees.instances = new List<Employees>();
        for (int i = 1; i < employees.Count; i++)
        {
            Employees.instances.Add(new Employees(employees[i][0], employees[i][1], employees[i][2], employees[i][3],
                employees[i][4], employees[i][5], employees[i][6], employees[i][7], employees[i][8], employees[i][9],
                employees[i][10], employees[i][11], employees[i][12], employees[i][13], employees[i][14],
                employees[i][15], employees[i][16], employees[i][17]));
        }
    }

    public static void PrepareEmployeeTerritories()
    {
        using (var reader =
               new StreamReader(
                   @"C:\Users\sdjur\Documents\Programowanie\Rider\C#\Projekt1\PierwszyProjekt\PZ2_Lab4\employee_territories.csv"))
        {
            employeeTerritories = new List<List<String>>();
            while (!reader.EndOfStream)
            {
                List<string> list = new List<string>();

                var line = reader.ReadLine();
                var values = line.Split(',');

                foreach (var text in values)
                {
                    list.Add(text);
                }

                employeeTerritories.Add(list);
            }
        }

        EmployeeTerritories.instances = new List<EmployeeTerritories>();
        for (int i = 1; i < employeeTerritories.Count; i++)
        {
            EmployeeTerritories.instances.Add(new EmployeeTerritories(employeeTerritories[i][0],
                employeeTerritories[i][1]));
        }
    }

    public static void PrepareTerritories()
    {
        using (var reader =
               new StreamReader(
                   @"C:\Users\sdjur\Documents\Programowanie\Rider\C#\Projekt1\PierwszyProjekt\PZ2_Lab4\territories.csv"))
        {
            territories = new List<List<String>>();
            while (!reader.EndOfStream)
            {
                List<string> list = new List<string>();

                var line = reader.ReadLine();
                var values = line.Split(',');

                foreach (var text in values)
                {
                    list.Add(text);
                }

                territories.Add(list);
            }
        }

        Territories.instances = new List<Territories>();
        for (int i = 1; i < territories.Count; i++)
        {
            Territories.instances.Add(new Territories(territories[i][0], territories[i][1], territories[i][2]));
        }
    }

    public static void PrepareRegions()
    {
        using (var reader =
               new StreamReader(
                   @"C:\Users\sdjur\Documents\Programowanie\Rider\C#\Projekt1\PierwszyProjekt\PZ2_Lab4\regions.csv"))
        {
            regions = new List<List<String>>();

            while (!reader.EndOfStream)
            {
                List<string> list = new List<string>();
                var line = reader.ReadLine();
                var values = line.Split(',');

                foreach (var text in values)
                {
                    list.Add(text);
                }

                regions.Add(list);
            }
        }

        Regions.instances = new List<Regions>();
        for (int i = 1; i < regions.Count; i++)
        {
            Regions.instances.Add(new Regions(regions[i][0], regions[i][1]));
        }
    }

    public static void PrepareOrders()
    {
        using (var reader =
               new StreamReader(
                   @"C:\Users\sdjur\Documents\Programowanie\Rider\C#\Projekt1\PierwszyProjekt\PZ2_Lab4\orders.csv"))
        {
            orders = new List<List<String>>();

            while (!reader.EndOfStream)
            {
                List<string> list = new List<string>();
                var line = reader.ReadLine();
                var values = line.Split(',');

                foreach (var text in values)
                {
                    list.Add(text);
                }

                orders.Add(list);
            }
        }

        Order.instances = new List<Order>();
        for (int i = 1; i < orders.Count; i++)
        {
            Order.instances.Add(new Order(orders[i][0], orders[i][1], orders[i][2], orders[i][3], orders[i][4],
                orders[i][5], orders[i][6], orders[i][7], orders[i][8], orders[i][9], orders[i][10], orders[i][11],
                orders[i][12], orders[i][13]));
        }
    }

    public static void PrepareOrderDetails()
    {
        using (var reader =
               new StreamReader(
                   @"C:\Users\sdjur\Documents\Programowanie\Rider\C#\Projekt1\PierwszyProjekt\PZ2_Lab4\orders_details.csv"))
        {
            orderDetails = new List<List<String>>();

            while (!reader.EndOfStream)
            {
                List<string> list = new List<string>();
                var line = reader.ReadLine();
                var values = line.Split(',');

                foreach (var text in values)
                {
                    list.Add(text);
                }

                orderDetails.Add(list);
            }
        }

        OrderDetails.instances = new List<OrderDetails>();
        for (int i = 1; i < orderDetails.Count; i++)
        {
            OrderDetails.instances.Add(new OrderDetails(orderDetails[i][0], orderDetails[i][1], orderDetails[i][2],
                orderDetails[i][3], orderDetails[i][4]));
        }
    }
}
using System;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

public class RSA
{
    private int p;
    public int P 
    {
        get { return p;}
        set { p = value; }
    }

    private int q { get; set; }
    public int Q
    {
        get { return q; }
        set { q = value; }
    }

    private int e;
    public int E
    {
        get { return e; }
        set { e = value; }
    }

    public int n; // modul p*q
    public int f; // eiler function

    public int d;

    public void Calculate () // get n, f
    {
        n = p * q;
        f = (p - 1) * (q - 1);
    }

    public bool IsPrime(int number)
    {
        if (number <= 1)
            return false;
        else if (number % 2 == 0)
            return number == 2;

        long N = (int)(Math.Sqrt(number) + 0.5);

        for (int i = 3; i <= N; i += 2)
            if (number % i == 0)
                return false;

        return true;
    }

    public bool IsSimple(int number)
    {
        if ( ( number % f == 0 ) || ( f % number == 0 ) ) { return false; }
        return true;
    }

    public long Encrypt(long EMessage)
    {
        EMessage = Convert.ToInt64(Math.Pow(EMessage, e)) % n;
        return EMessage;
    }
    
    public long Decrypt (long DMessage)
    {
        DMessage = Convert.ToInt64(Math.Pow(DMessage, d)) % n;
        return DMessage;
    }
}

public class Algorithm
{
    public void ToEncrypt()
    {
        RSA RSA = new RSA();
        Console.WriteLine("\nВведите P и Q через пробел: ");

        string UserInput = Console.ReadLine();
        string[] Splitted = UserInput.Split(' ', 2);

        RSA.P = Convert.ToInt32(Splitted[0]);
        RSA.Q = Convert.ToInt32(Splitted[1]);
        RSA.Calculate();

        /*
        ПРИМЕР e для { p=3; q=7; n=21; f=12 }
        ВЫБИРАЕМ e: 
        (i) оно должно быть простое, 
        (ii) оно должно быть меньше φ — остаются варианты: 3, 5, 7, 11, 
        (iii) оно должно быть взаимно простое с φ; остаются варианты 5, 7, 11. Выберем e=5. Это, так называемая, открытая экспонента.
         */

        //                                                 get e option
        List<int> eOption = new List<int>();

        for (int i = 0; i < RSA.f; i++)
        {
            if (i % 2 != 0 && RSA.IsPrime(i) && RSA.IsSimple(i)) { eOption.Add(i); }
        }

        //                                                  // get e
        bool isE = true;
        while (isE)
        {

            Console.Write("Выберите значение из вариантов числа e: ");
            foreach (var nn in eOption)
            {
                Console.Write(nn + " ");
            }

            Console.WriteLine(); // Next line for type e value

            RSA.E = Convert.ToInt32(Console.ReadLine());
            if (eOption.Contains(RSA.E)) { isE = false; }
        }

        //                                                    get OPENKey
        string OpenKey = $"[n, e] [{RSA.n}, {RSA.E}]";
        //Console.WriteLine($"Открытый ключ: {OpenKey}");

        //                                                    get CLOSEKey
        for (int d = 0; d < Math.Pow(10, 6); d++)
        {
            if (((d * RSA.E) % RSA.f == 1) && (d > RSA.E))
            {
                RSA.d = d;
                break;
            }
        }
        string CloseKey = $"[d, n] [{RSA.d}, {RSA.n}]";
        //Console.WriteLine($"Закрытый ключ: {CloseKey}");

        Console.WriteLine($"\nВ итоге:\np = {RSA.P} q = {RSA.Q}\nn = {RSA.n}\nf = {RSA.f}\nОткрытый ключ: {OpenKey}\nЗакрытый ключ: {CloseKey}\n");

        //                                                    Encrypt Message
        Console.WriteLine("Enter your message ( <= n): ");

        long EMessage = RSA.n + 1;
        bool isEMessage = true;
        while (isEMessage)
        {
            EMessage = Convert.ToInt64(Console.ReadLine());
            if (EMessage <= RSA.n) { isEMessage = false; }
        }
        EMessage = RSA.Encrypt(EMessage);

        Console.WriteLine($"Your Encrypted message: {EMessage}\n\n");
    }

    //                                                 Decrypt Message input d,n - closekey
    public void ToDecrypt(int d, int n)
    {
        RSA RSA = new RSA();
        RSA.d = d;
        RSA.n = n;

        Console.WriteLine("\nEnter your message to DECRYPT: ");
        long DMessage = Convert.ToInt64(Console.ReadLine());
        DMessage = RSA.Decrypt(DMessage);
        Console.WriteLine($"Your Decrypted message: {DMessage}\n\n");

    }
}

public class Program()
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Created by Petrov Nikita. ASOiUB-23-1 Tyumen Industrial University TIU\n");
        Algorithm algoritm = new Algorithm();
        while (true)
        {
            Console.WriteLine("RSA ALGORITHM\n1.Encrypt message\n2.Decrypt message\n3.Exit\nEnter your choice(1,2,3): ");
            int UserChoice = Convert.ToInt32(Console.ReadLine());
            if (UserChoice == 1) { algoritm.ToEncrypt(); }
            else if (UserChoice == 2)
            {
                Console.WriteLine("Введите закрытый ключ ( d и n через пробел): ");
                string UserInput = Console.ReadLine();
                string[] Splitted = UserInput.Split(' ', 2);

                int d = Convert.ToInt32(Splitted[0]);
                int n = Convert.ToInt32(Splitted[1]);

                algoritm.ToDecrypt(d, n);
            }
            else if (UserChoice == 3)
            {
                Environment.Exit(0);
            }
        }
    }
}

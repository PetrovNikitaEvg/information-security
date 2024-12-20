using System;
using System.Linq;

public class DoubleTranspositionCipher
{
    public static string Encrypt(string text, int cols, int rows, string colKey, string rowKey)
    {
        if (text.Length != cols * rows)
        {
            return "Error: Text length does not match matrix size.";
        }
        if (colKey.Length != cols || rowKey.Length != rows)
        {
            return "Error: Key lengths do not match matrix size.";
        }
        if (!colKey.All(char.IsDigit) || !rowKey.All(char.IsDigit))
        {
            return "Error: Keys must contain only digits.";
        }

        int[] colKeyInt = colKey.Select(c => int.Parse(c.ToString()) - 1).ToArray();
        int[] rowKeyInt = rowKey.Select(c => int.Parse(c.ToString()) - 1).ToArray();

        char[,] matrix = new char[rows, cols];
        int k = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = text[k++];
            }
        }

        char[,] transposedMatrix = new char[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                transposedMatrix[i, colKeyInt[j]] = matrix[i, j];
            }
        }

        char[,] finalMatrix = new char[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                finalMatrix[rowKeyInt[i], j] = transposedMatrix[i, j];
            }
        }

        string encryptedText = "";
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                encryptedText += finalMatrix[i, j];
            }
        }

        return encryptedText;
    }

    public static string Decrypt(string encryptedText, int cols, int rows, string colKey, string rowKey)
    {
        if (encryptedText.Length != cols * rows)
        {
            return "Error: Text length does not match matrix size.";
        }
        if (colKey.Length != cols || rowKey.Length != rows)
        {
            return "Error: Key lengths do not match matrix size.";
        }
        if (!colKey.All(char.IsDigit) || !rowKey.All(char.IsDigit))
        {
            return "Error: Keys must contain only digits.";
        }

        int[] colKeyInt = colKey.Select(c => int.Parse(c.ToString()) - 1).ToArray();
        int[] rowKeyInt = rowKey.Select(c => int.Parse(c.ToString()) - 1).ToArray();

        // Создаем массивы для обратного порядка ключей
        int[] colKeyOrder = Enumerable.Range(0, cols).OrderBy(i => colKeyInt[i]).ToArray();
        int[] rowKeyOrder = Enumerable.Range(0, rows).OrderBy(i => rowKeyInt[i]).ToArray();

        char[,] matrix = new char[rows, cols];
        int k = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = encryptedText[k++];
            }
        }

        // Обратная перестановка строк
        char[,] intermediateMatrix = new char[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                intermediateMatrix[rowKeyOrder[i], j] = matrix[i, j];
            }
        }


        // Обратная перестановка столбцов
        char[,] decryptedMatrix = new char[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                decryptedMatrix[i, colKeyOrder[j]] = intermediateMatrix[i, j];
            }
        }


        string decryptedText = "";
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                decryptedText += decryptedMatrix[i, j];
            }
        }

        return decryptedText;
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("Двойная перестановка:");
        while (true) { 
            Console.WriteLine("Выберите действие:");
        Console.WriteLine("1. Зашифровать текст");
        Console.WriteLine("2. Расшифровать текст");
        Console.Write("Введите номер действия (1 или 2): ");

        int choice;
        if (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2))
        {
            Console.WriteLine("Неверный ввод.");
            return;
        }

        string text;
        string result;
        int cols, rows;
        string colKey, rowKey;

        switch (choice)
        {
            case 1: // Шифрование
                Console.WriteLine("Введите текст для шифрования:");
                text = Console.ReadLine();
                Console.WriteLine("Введите количество столбцов:");
                if (!int.TryParse(Console.ReadLine(), out cols)) { Console.WriteLine("Неверный ввод."); return; }
                Console.WriteLine("Введите количество строк:");
                if (!int.TryParse(Console.ReadLine(), out rows)) { Console.WriteLine("Неверный ввод."); return; }
                Console.WriteLine("Введите ключ для перестановки столбцов (например, 312):");
                colKey = Console.ReadLine();
                Console.WriteLine("Введите ключ для перестановки строк (например, 231):");
                rowKey = Console.ReadLine();

                if (text.Length != cols * rows)
                {
                    Console.WriteLine("Ошибка: Размер текста не совпадает с размером матрицы.");
                    return;
                }
                result = Encrypt(text, cols, rows, colKey, rowKey);
                Console.WriteLine($"Зашифрованный текст: {result}\n");
                break;

            case 2: // Расшифровка
                Console.WriteLine("Введите текст для расшифровки:");
                text = Console.ReadLine();
                Console.WriteLine("Введите количество столбцов:");
                if (!int.TryParse(Console.ReadLine(), out cols)) { Console.WriteLine("Неверный ввод."); return; }
                Console.WriteLine("Введите количество строк:");
                if (!int.TryParse(Console.ReadLine(), out rows)) { Console.WriteLine("Неверный ввод."); return; }
                Console.WriteLine("Введите ключ для перестановки столбцов (например, 312):");
                colKey = Console.ReadLine();
                Console.WriteLine("Введите ключ для перестановки строк (например, 231):");
                rowKey = Console.ReadLine();

                if (text.Length != cols * rows)
                {
                    Console.WriteLine("Ошибка: Размер текста не совпадает с размером матрицы.");
                    return;
                }

                result = Decrypt(text, cols, rows, colKey, rowKey);
                Console.WriteLine($"Расшифрованный текст: {result} \n");
                break;
            default:
                Console.WriteLine("Неверный выбор действия.");
                break;
            }
        }
    }
}
